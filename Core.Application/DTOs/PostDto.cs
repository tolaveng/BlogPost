using Core.Application.Utils;
using Core.Domain.Enums;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    public class PostDto
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "UserId is empty")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; } = ObjectId.Empty;

        [Required(ErrorMessage = "PostType is empty")]
        public PostType PostType { get; set; } = PostType.Post;

        [Required(ErrorMessage = "Title is empty")]
        [MinLength(4, ErrorMessage = "Title is at least 5 letters")]
        public string Title { get; set; } = string.Empty;


        [Required(ErrorMessage = "Path is empty")]
        [MinLength(4, ErrorMessage = "Path is at least 5 letters")]
        public string Path { get; set; } = string.Empty;


        public string Content { get; set; } = string.Empty;


        public string Summary { get; set; } = string.Empty;
        public string[] Tags { get; set; } = new string[0];

        public DateTime PublishedDateTime { get; set; } = DateTime.Now;
        public DateTime ModifiedDateTime { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; }
        public bool IsArhived { get; set; }
        public string FeatureImageUrl { get; set; } = string.Empty;
    }
}
