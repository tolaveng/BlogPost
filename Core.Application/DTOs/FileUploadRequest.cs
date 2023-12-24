namespace Core.Application.DTOs
{
    public class FileUploadRequest
    {
        public Stream Stream { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;

        public string[] ImageExtensions => new[] { ".png", ".jpg", ".jpeg", ".gif" };
        public string Extension
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FileName)) return string.Empty;
                return Path.GetExtension(FileName).ToLowerInvariant();
            }
        }

        public bool IsImage
        {
            get
            {
                return ImageExtensions.Contains(Extension);
            }
        }

        public long FileSize => Stream.Length;
        

        public FileUploadRequest(Stream stream, string name, string fileName, string contentType)
        {
            Name = name;
            Stream = stream;
            FileName = fileName;
            ContentType = contentType;
        }

        public bool IsValidFile()
        {
            if (Stream == null || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(FileName)) return false;
            return true;
        }
    }
}
