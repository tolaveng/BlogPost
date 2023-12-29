using Core.Domain.Attributes;
using Core.Domain.Enums;
using MongoDB.Bson;


namespace Core.Domain.Documents
{
    [MongoDbCollection("Posts")]
    [MongoDbIndexes("Title"), MongoDbIndexes("Path")]
    public class Post : Document
    {
        public ObjectId UserId { get; set; } = ObjectId.Empty;
        public PostType PostType { get; set; } = PostType.Post;
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string[] Tags { get; set; } = new string[0];
        public DateTime PublishedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool IsPublished { get; set; }
        public bool IsArhived { get; set; }
        public string FeatureImageUrl { get; set; } = string.Empty;

        public long ViewCount { get; set; }
    }
}
