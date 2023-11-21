using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    public class UserDto
    {
        [Required(ErrorMessage = "Username cannot be empty")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public bool IsDisabled { get; set; }
    }
}
