using Authentication.Jwt.Models;
using CommunicationLibrary.Gateways.Config;
using EFDatabaseModel.DbModel;
using ProjectBase.Repository.Transaction;
using System.Threading.Tasks;

namespace Authentication.Jwt.Service
{
    public interface IUserJwtService
    {
        Task<ExecutionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model);

        Task<ExecutionResult<User>> GetById(int id);

        Task<ExecutionResult<User>> ValidateToken(string token, string domain);

        Task<ExecutionResult<string>> EndSession(string token);
        Task<ExecutionResult<AuthenticateResponse>> RenewPasswordInitiate(
            AuthenticateRequest model
        );
        Task<ExecutionResult<AuthenticateResponse>> SendRenewPasswordOTP(
            AuthenticateResponse model,
            string domain
        );
        Task<ExecutionResult<AuthenticateResponse>> RenewPassword(RenewPasswordModel model);
        //Task<ExecutionResult<RegisterResponse>> Register(RegisterRequest model);
        Task<ExecutionResult<UserLoginAttempt>> LastLoginData(User user, string currentLoginToken);
    }
}
