namespace Frontend.Services
{
    public class UserSessionService
    {
        public string? AccessToken { get; private set; }

        public void SetToken(string token)
        {
            AccessToken = token;
        }

        public void ClearToken()
        {
            AccessToken = null;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);
    }
}
