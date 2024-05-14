namespace Authentication.Jwt.Models
{
    public class RenewPasswordModel : AuthenticateRequest
    {
        public string Token { get; set; }
        public string Password1 { get; set; }
        public string OtpCode { get; set; }
    }
}
