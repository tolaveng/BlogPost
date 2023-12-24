﻿namespace Core.Application.DTOs
{
    public class FileUploadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FileUri { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public static FileUploadResponse Succeed(string fileName, string fileUri)
        {
            return new FileUploadResponse() { Success = true, FileName = fileName, FileUri = fileUri };
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
