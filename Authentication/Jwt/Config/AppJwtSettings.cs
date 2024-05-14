namespace Authentication.Jwt.Config
{
    public class AppJwtSettings
    {
        public string Secret { get; set; }
        public long SessionLengthSeconds { get; set; }
        public int OTPExpiryMinutes { get; set; }
    }
}
