using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
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

        public FileUploadService(IOptions<AzureStorageSetting> options)
        {
            setting = options.Value;
        }


        private void OnUploadProgress(string fileName, int progress)
        {
            UploadProgress?.Invoke(this, new UploadProgressEventArgs(fileName, progress));
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request, CancellationToken ct)
        {
            OnUploadProgress(request.FileName, 30);
            await Task.Delay(1000);
            if (ct.IsCancellationRequested) return null;
            OnUploadProgress(request.FileName, 40);
            await Task.Delay(1000);
            if (ct.IsCancellationRequested) return null;
            OnUploadProgress(request.FileName, 50);
            await Task.Delay(1000);
            OnUploadProgress(request.FileName, 70);
            await Task.Delay(1000);
            if (ct.IsCancellationRequested) return null;
            OnUploadProgress(request.FileName, 90);
            await Task.Delay(1000);
            if (ct.IsCancellationRequested) return null;
            OnUploadProgress(request.FileName, 100);
            return FileUploadResponse.Succeed("tessxzt");
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
