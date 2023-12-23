using Core.Application.Utils;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApp.Components.FileUploader
{
    public class FileUploaderFile {
        public string Name { get; set; }
        public string FileName { get; set; }
        public IBrowserFile BrowserFile { get; set; }
        public int ProgressPercent { get; set; }

        public FileUploaderFile(IBrowserFile browserFile)
        {
            Name = browserFile.Name;
            FileName = browserFile.Name.SanitiseFileName();
            BrowserFile = browserFile;
            ProgressPercent = 0;
        }
    }
}
