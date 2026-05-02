namespace Server.DTOs.Auth
{
    public class LoginRequest
    {
        public string phone { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
