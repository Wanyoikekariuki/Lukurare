using EFDatabaseModel.DbModel;
using Newtonsoft.Json;

namespace Authentication.Jwt.Models
{
    public class AuthenticateResponse
    {
        public long Id
        {
            get { return User == null ? -1 : User.Id; }
        }
        public string Token { get; set; }

        public string Phone
        {
            get
            {
                if (User != null)
                    return User.PhoneNumber;
                else
                    return "";
            }
        }

        public string EmailAddress
        {
            get
            {
                if (User != null)
                    return User.EmailAddress;
                else
                    return "";
            }
        }

        [JsonIgnore]
        public User User { get; }

        public AuthenticateResponse(User user, string token)
        {
            User = user;
            Token = token;
        }
    }
}
