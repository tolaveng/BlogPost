namespace Core.Application.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public bool IsDisabled { get; set; }
    }
}
