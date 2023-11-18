using System.ComponentModel.DataAnnotations;

namespace JWT.Demo.DTOs.AuthenticationDTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}