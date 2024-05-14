using Authentication.Jwt.Config;
using Authentication.Jwt.Models;
using Authentication.Repository;
using CommunicationLibrary.Gateways.Config;
using EFDatabaseModel.DbModel;
using Microsoft.Extensions.Options;
using ProjectBase.Repository.Transaction;
using System.Threading.Tasks;

namespace Authentication.Jwt.Service
{
    /// <summary>
    /// This class generates the JWT token for the supplied user ID
    /// </summary>
    public class UserJwtService : IUserJwtService
    {
        private readonly AppJwtSettings _appSettings;
        private readonly UserAuthenticationRepository _userRepo;
        private readonly SMSGatewayDefination _smsGatewayDefination;
        //private readonly UserSelfRegistrationRepository _userSelfRepo;

        public UserJwtService(
            IOptions<AppJwtSettings> appSettings,
            IOptions<SMSGatewayDefination> smsGatewayDefination
        )
        {
            _appSettings = appSettings.Value;
            _smsGatewayDefination = smsGatewayDefination.Value;
            _userRepo = new UserAuthenticationRepository(
                "JWT Authentication",
                _smsGatewayDefination
            );
            //_userSelfRepo = new UserSelfRegistrationRepository(
            //    "JWT Authentication",
            //    _smsGatewayDefination
            //);
        }

        //public async Task<ExecutionResult<RegisterResponse>> Register(RegisterRequest model)
        //{
        //    if (model == null)
        //        return new ExecutionResult<RegisterResponse>(false, "model is actually null");

        //    var registerResult = await _userSelfRepo.Register(model);

        //    // return null if user not found
        //    if (registerResult.IsOkay == false)
        //        return registerResult;

        //    //authentication successful so generate jwt token
        //    //var token = generateJwtToken(loginResult.Result.User);

        //    return new ExecutionResult<RegisterResponse>(
        //        true,
        //        registerResult.Message,
        //        registerResult.Result
        //    );
        //}

        public async Task<ExecutionResult<AuthenticateResponse>> Authenticate(
            AuthenticateRequest model
        )
        {
            if (model == null)
                return new ExecutionResult<AuthenticateResponse>(false, "model is null");

            var loginResult = await _userRepo.Authenticate(model);

            // return null if user not found
            if (loginResult.IsOkay == false)
                return loginResult;

            //authentication successful so generate jwt token
            //var token = generateJwtToken(loginResult.Result.User);

            return new ExecutionResult<AuthenticateResponse>(
                true,
                loginResult.Message,
                loginResult.Result
            );
        }

        public async Task<ExecutionResult<string>> EndSession(string token)
        {
            return await _userRepo.EndSession(token);
        }

        public async Task<ExecutionResult<User>> GetById(int id)
        {
            return await _userRepo.GetItem(id);
        }

        public async Task<ExecutionResult<AuthenticateResponse>> RenewPassword(
            RenewPasswordModel model
        )
        {
            return await _userRepo.RenewPassword(model, _appSettings);
        }

        public async Task<ExecutionResult<AuthenticateResponse>> RenewPasswordInitiate(
            AuthenticateRequest model
        )
        {
            return await _userRepo.RenewPasswordInitiate(model);
        }

        public async Task<ExecutionResult<AuthenticateResponse>> SendRenewPasswordOTP(
            AuthenticateResponse model,
            string domain
        )
        {
            return await _userRepo.SendRenewPasswordOTP(model, domain, _appSettings);
        }

        public async Task<ExecutionResult<User>> ValidateToken(string token, string domain)
        {
            return await _userRepo.ValidateToken(token, domain, _appSettings);
        }

        public async Task<ExecutionResult<UserLoginAttempt>> LastLoginData(
            User user,
            string currentLoginToken
        )
        {
            return await _userRepo.LastLoginData(user, currentLoginToken);
        }

        //private string generateJwtToken(User user)
        //{
        //    //generate token that is valid for as defined by the ticks in AppSettings
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);//the secret can be the users pwd
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
        //        Expires = DateTime.UtcNow.AddTicks(_appSettings.SessionLengthSeconds),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }
}
