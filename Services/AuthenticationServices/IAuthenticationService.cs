using JWT.Demo.DTOs.AuthenticationDTOs;

namespace JWT.Demo.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<AuthenticationDTO> RegisterAsync(RegisterDTO model);
        Task<AuthenticationDTO> LoginAsync(LoginDTO model);
        Task<string> AddRoleAsync(RoleDTO model);
        Task<AuthenticationDTO> GetRefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

    }
}
