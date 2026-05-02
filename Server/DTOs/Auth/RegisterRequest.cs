namespace Server.DTOs.Auth
{
    public class RegisterRequest
    {
        public string user_name { get; set; } = string.Empty;
        public string? email { get; set; }
        public string phone { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
