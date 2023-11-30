using Core.Application.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    public class UserDto
    {

        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Username cannot be empty")]
        [StringLength(32, ErrorMessage = "Username too long (32 character limit)")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public bool IsDisabled { get; set; }

    }
}
