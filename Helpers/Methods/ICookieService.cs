namespace JWT.Demo.Helpers.Methods
{
    public interface ICookieService
    {
        void SetRefreshToken(HttpResponse response, string refreshToken, DateTime expires);
    }
}
