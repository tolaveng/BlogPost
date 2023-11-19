using Core.Domain.Attributes;

namespace Core.Domain.Documents
{
    [MongoDbCollection("FileUploads")]
    [MongoDbIndexes("Name")]
    public class FileUpload : Document
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public bool IsArchived { get; set; }
    }
}
