namespace Authentication.Jwt.Models
{
    public class AuthenticateRequest
    {
        //[Required]
        public string Username { get; set; }

        //[Required]
        public string Password { get; set; }

        public string CurrentPage { get; set; }

        public string Domain { get; set; }
        public string RemoteIpAddress { get; set; }
    }
}
