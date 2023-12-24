using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Core.Application.Utils;
using Core.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Core.Application.Services
{
    public class FileUploadService : IFileUploadService
    {

        public string[] AllowExtensions { get; set; } = { ".png", ".jpg", ".jpeg" };
        public long MaxFileSize { get; set; } = 1024 * 1024 * 5; // 5M

        public event EventHandler<UploadProgressEventArgs> UploadProgress;

        private AzureStorageSetting setting;

        private BlobContainerClient containerClient;

        public FileUploadService(IOptions<AzureStorageSetting> options)
        {
            setting = options.Value;
        }


        private async Task<BlobContainerClient> CreateContainer()
        {
            //BlobServiceClient blobServiceClient = new BlobServiceClient(setting.ConnectionString);
            //BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(setting.ContainerName);
            //await containerClient.CreateIfNotExistsAsync();
            //return containerClient;

            if (containerClient != null) return containerClient;

            containerClient = new BlobContainerClient(setting.ConnectionString, setting.ContainerName);
            try
            {
                await containerClient.CreateIfNotExistsAsync();
                return containerClient;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private void OnUploadProgress(string fileName, int progress)
        {
            UploadProgress?.Invoke(this, new UploadProgressEventArgs(fileName, progress));
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request, CancellationToken ct)
        {
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

                var containerClient = await CreateContainer();
                if (containerClient == null)
                {
                    return FileUploadResponse.Fail("Something went wrong. Cannot access to Azure cloud.");
                }

                return await UploadToAzureStorage(request, ct);

            } catch (Exception ex)
            {
                // TODO: add logger
                return FileUploadResponse.Fail("Something went wrong. Access to Azure cloud. Exception occured");
            }
        }

        private async Task<FileUploadResponse> UploadToAzureStorage(FileUploadRequest request, CancellationToken ct)
        {
            var blobClient = containerClient.GetBlobClient(request.FileName);
            if (blobClient == null)
            {
                throw new InvalidOperationException("Cannot access to Blog client");
            }
            try
            {
                // override it
                await blobClient.DeleteIfExistsAsync();

                // set options progress
                var uploadOption = new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders() { ContentType = request.ContentType },
                    TransferOptions = new Azure.Storage.StorageTransferOptions() { 
                        InitialTransferSize = 1024 * 1024,
                        MaximumConcurrency = 2
                    },
                    ProgressHandler = new Progress<long>(progress =>
                    {
                        var progressing = Math.Ceiling(Decimal.Divide(progress, request.FileSize) * 100);
                        OnUploadProgress(request.FileName, (int)progressing);
                    })
                };

                // upload
                await blobClient.UploadAsync(request.Stream, uploadOption, ct);

                if (ct.IsCancellationRequested)
                {
                    await blobClient.DeleteIfExistsAsync();
                }

                OnUploadProgress(request.FileName, 100);
                var fileUri = AzureUtil.GetServiceSasUriForBlob(blobClient, null, true);
                return FileUploadResponse.Succeed(blobClient.Name, fileUri.ToString());

            } catch (Exception ex)
            {
                OnUploadProgress(request.FileName, 0);
                await blobClient.DeleteIfExistsAsync();
                return FileUploadResponse.Fail("Something went wrong. Upload failed");
            }
        }

        public class UploadProgressEventArgs
        {
            public string FileName { get; set; }
            public int Progress { get; set; }

            public UploadProgressEventArgs(string fileName, int progress)
            {
                FileName = fileName;
                Progress = progress;
            }
        }
    }
}
