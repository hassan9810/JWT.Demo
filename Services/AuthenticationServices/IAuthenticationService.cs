using JWT.Demo.Models.Authentication;

namespace JWT.Demo.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> RegisterAsync(RegisterModel model);
        Task<AuthenticationModel> LoginAsync(LoginModel model);
        Task<string> AddRoleAsync(RoleModel model);
        Task<AuthenticationModel> GetRefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

    }
}
