using JWT.Demo.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWT.Demo.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
