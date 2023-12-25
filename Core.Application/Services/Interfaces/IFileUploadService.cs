using Core.Application.DTOs;
using MongoDB.Bson;
using static Core.Application.Services.FileUploadService;

namespace Core.Application.Services.Interfaces
{
    public interface IFileUploadService
    {
        string[] AllowExtensions { get; set; }
        long MaxFileSize { get; set; }

        event EventHandler<UploadProgressEventArgs> UploadProgress;

        Task<FileUploadResponse> UploadFileAsync(FileUploadRequest fileUploadRequest, CancellationToken ct);
        Task<bool> DeleteFileAsync(string fileName);
        Task<Pagination<FileUploadResponse>> GetUploadFilesAsync(string paginationToken, int pageSize, CancellationToken ct);
    }
}
