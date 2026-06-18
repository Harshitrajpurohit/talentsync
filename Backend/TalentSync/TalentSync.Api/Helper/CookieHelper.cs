namespace TalentSync.Api.Helper
{
    public class CookieHelper
    {
        private const string RefreshTokenName = "refreshToken";

        public void SetRefreshTokenCookie(HttpResponse httpResponse, string refreshToken)
        {
            httpResponse.Cookies.Append(RefreshTokenName, refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // true in production
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddMinutes(5)
                });
        }
        public void DeleteRefreshTokenCookie(HttpResponse response)
        {
            response.Cookies.Delete(
                RefreshTokenName);
        }
    }
}
