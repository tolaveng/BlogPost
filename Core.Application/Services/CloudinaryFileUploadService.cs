using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Core.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class CloudinaryFileUploadService : IFileUploadService
    {
        private readonly Cloudinary cloudinary;

        public string[] AllowExtensions { get; set; } = { ".png", ".jpg", ".jpeg" };
        public long MaxFileSize { get; set; } = 1024 * 1024 * 5; // 5M

        public event EventHandler<UploadProgressEventArgs> UploadProgress;

        public CloudinaryFileUploadService(IOptions<CloudinarySetting> option)
        {
            var setting = option.Value;
            var account = new Account(setting.Name, setting.Key, setting.Secret);
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
        }


        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var deleteParams = new DeletionParams(fileName);
                var deleted = await cloudinary.DestroyAsync(deleteParams).ConfigureAwait(false);
                return deleted.StatusCode == HttpStatusCode.OK;

            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Pagination<FileUploadResponse>> GetUploadFilesAsync(string paginationToken, int pageSize, CancellationToken ct)
        {
            var response = new Pagination<FileUploadResponse>() {
                HasNext = false,
                Count = 0,
                Items = Array.Empty<FileUploadResponse>(),
                TotalPages = 0,
            };

            if (cloudinary == null) return response;

            var listParams = new ListResourcesParams()
            {
                NextCursor = paginationToken,
                MaxResults = pageSize,
                ResourceType = ResourceType.Image,
                Type = "upload",
            };
            
            var resources = await cloudinary.ListResourcesAsync(listParams, ct).ConfigureAwait(false);
            if (resources.StatusCode != HttpStatusCode.OK || resources.Error != null) return response;

            var files = resources.Resources.Select(x => 
                new FileUploadResponse()
                {
                    Name = x.DisplayName,
                    FileName = x.PublicId,
                    FileUri = x.Uri.ToString(),
                    FileContentType = x.ResourceType,
                    ThumbnailUrl = CreateThumbnailUrl(x),
                });
            
            response.PaginationToken = resources.NextCursor;
            response.HasNext = !string.IsNullOrEmpty(resources.NextCursor);
            response.Count = resources.Resources.Length;
            response.Items = files;
            

            return response;
        }

        private string CreateThumbnailUrl(Resource resource)
        {
            if (resource.ResourceType != "image") return "";

            try
            {
                // resource.PublicId NOT working, it required double extension ".jpg.jpg"
                var imageName = resource.Uri.ToString().Split("/").Last();
                var uri = cloudinary.Api.UrlImgUp.Transform(new Transformation()
                    .Width(200).Crop("thumb"))
                    .BuildUrl(imageName);

                return uri.ToString();
            } catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request, CancellationToken ct)
        {
            if (cloudinary == null)
            {
                return FileUploadResponse.Fail("Something went wrong. Cannot access to cloud.");
            }

            try
            {
                if (!request.IsValidFile() || !request.IsImage)
                {
                    return FileUploadResponse.Fail("Something went wrong. File is not valid");
                }

                if (string.IsNullOrEmpty(request.Extension) || !AllowExtensions.Contains(request.Extension))
                {
                    return FileUploadResponse.Fail($"File type must be ({string.Join(", ", AllowExtensions)})");
                }

                if (request.FileSize > MaxFileSize)
                {
                    return FileUploadResponse.Fail("File is too big");
                }

                

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(request.FileName, request.Stream),
                    DisplayName = request.Name,
                    PublicId = request.FileName.Replace(" ", "-"),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = true,
                };

                var result = await cloudinary.UploadAsync(uploadParams, ct);

                if (result == null || result.Uri == null || result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return FileUploadResponse.Fail("File upload to cloud failed");
                }

                return FileUploadResponse.Succeed(request.Name, request.FileName, result.Uri.ToString(), request.ContentType);

            }
            catch (Exception ex)
            {
                // TODO: add logger
                return FileUploadResponse.Fail("Something went wrong. Access to Azure cloud. Exception occured");
            }
        }

        private void OnUploadProgress(string fileName, int progress)
        {
            UploadProgress?.Invoke(this, new UploadProgressEventArgs(fileName, progress));
        }
    }
}
