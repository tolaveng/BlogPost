namespace Core.Application.DTOs
{
    public class FileUploadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FileUri { get; set; } = string.Empty;

        public string FileContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public bool IsImage => FileContentType != null && FileContentType.StartsWith("image");

        public FileUploadResponse()
        {

        }

        public FileUploadResponse(string name, string fileName, string fileUri)
        {
            Name = name;
            FileName = fileName;
            FileUri = fileUri;
        }

        public static FileUploadResponse Succeed(string name, string fileName, string fileUri, string contentType)
        {
            return new FileUploadResponse() {
                Success = true,
                Name = name,
                FileName = fileName,
                FileUri = fileUri,
                FileContentType = contentType
            };
        }

        public static FileUploadResponse Succeed(string fileName, long fileSize)
        {
            return new FileUploadResponse() { Success = true, FileName = fileName, FileSize = fileSize };
        }

        public static FileUploadResponse Fail(string message)
        {
            return new FileUploadResponse() { Success = false, Message = message };
        }
    }
}
