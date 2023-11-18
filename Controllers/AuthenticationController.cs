namespace JWT.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICookieService _cookieService;
        public AuthenticationController(IAuthenticationService authenticationService, ICookieService cookieService)
        {
            _authenticationService = authenticationService;
            _cookieService = cookieService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            _cookieService.SetRefreshToken(Response, result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginDTO model)
        {
            var result = await _authenticationService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                _cookieService.SetRefreshToken(Response, result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("AssignRoleToUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> GetRefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authenticationService.GetRefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            _cookieService.SetRefreshToken(Response, result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDTO revokeTokenDTO)
        {
            var token = revokeTokenDTO.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authenticationService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }
    }
}
namespace JWT.Demo.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class AuthenticationV2 : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationV2(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                var authenticationModel = new AuthenticationDTO
                {
                    IsAuthenticated = result.IsAuthenticated,
                    Username = result.Username,
                    Email = result.Email,
                    Roles = result.Roles,
                    Token = result.Token,
                    ExpiresOn = result.ExpiresOn,
                    RefreshToken = result.RefreshToken,
                    RefreshTokenExpiration = result.RefreshTokenExpiration
                };

                return Ok(authenticationModel);
            }

            var authenticationModelWithoutRefreshToken = new AuthenticationDTO
            {
                IsAuthenticated = result.IsAuthenticated,
                Username = result.Username,
                Email = result.Email,
                Roles = result.Roles,
                Token = result.Token,
                ExpiresOn = result.ExpiresOn
            };

            return Ok(authenticationModelWithoutRefreshToken);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginDTO model)
        {
            var result = await _authenticationService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                var authenticationModel = new AuthenticationDTO
                {
                    IsAuthenticated = result.IsAuthenticated,
                    Username = result.Username,
                    Email = result.Email,
                    Roles = result.Roles,
                    Token = result.Token,
                    ExpiresOn = result.ExpiresOn,
                    RefreshToken = result.RefreshToken,
                    RefreshTokenExpiration = result.RefreshTokenExpiration
                };

                return Ok(authenticationModel);
            }

            var authenticationModelWithoutRefreshToken = new AuthenticationDTO
            {
                IsAuthenticated = result.IsAuthenticated,
                Username = result.Username,
                Email = result.Email,
                Roles = result.Roles,
                Token = result.Token,
                ExpiresOn = result.ExpiresOn
            };

            return Ok(authenticationModelWithoutRefreshToken);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = refreshTokenRequest?.RefreshToken;

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token is required.");

            var result = await _authenticationService.GetRefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                return Ok(new { RefreshToken = result.RefreshToken, RefreshTokenExpiration = result.RefreshTokenExpiration });
            }

            return BadRequest("Refresh token not found.");
        }

        [HttpPost("AssignRoleToUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDTO revokeTokenDTO)
        {
            var token = revokeTokenDTO?.Token;

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authenticationService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }
    }
}
