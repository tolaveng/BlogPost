using Core.Domain.Attributes;

namespace Core.Domain.Documents
{
    [MongoDbCollection("Users")]
    [MongoDbIndexes("Username"), MongoDbIndexes("Email")]
    public class User : Document
    {
        public string Username { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalf { get; set; } = string.Empty;
        public bool IsDisabled { get; set; }
    }
}
