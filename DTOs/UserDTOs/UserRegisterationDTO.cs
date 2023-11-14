using System.ComponentModel.DataAnnotations;

namespace JWT.Demo.DTOs.UserDTOs
{
    public class UserRegisterationDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
