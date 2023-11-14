namespace JWT.Demo.Helpers.Methods
{
    public class CookieService : ICookieService
    {
        public void SetRefreshToken(HttpResponse response, string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
