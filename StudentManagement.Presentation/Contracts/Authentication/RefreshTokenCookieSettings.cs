namespace StudentSkillManagement.Presentation.Contracts.Authentication
{
    public static class RefreshTokenCookieSettings
    {
        public const string CookieName = "refreshToken";

        public static CookieOptions Create(DateTime expiresAtUtc)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Path = "/api/Auth",
                Expires = expiresAtUtc
            };
        }
    }
}
