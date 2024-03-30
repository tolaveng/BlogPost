namespace Core.Application.Services
{
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
