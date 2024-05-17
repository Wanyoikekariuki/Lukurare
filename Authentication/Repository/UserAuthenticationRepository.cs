using Authentication.Jwt.Config;
using Authentication.Jwt.Models;
using CommunicationLibrary.Gateways.Config;
using CommunicationLibrary.Gateways.Telephony.SMS;
using CommunicationLibrary.Telephony.Messaging.SMS;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectBase.Database.Connection;
using ProjectBase.Repository;
using ProjectBase.Repository.Transaction;
using ProjectBase.Security;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Repository
{
    public class UserAuthenticationRepository
        : DbContextRepository<User, EFDatabaseModelDatabaseContext>
    {
        private bool IsResetPasswordRequest;
        private SMSGatewayDefination smsGatewayDefination;

        public UserAuthenticationRepository(string currentModule)
            : base(currentModule) { }

        public UserAuthenticationRepository(
            string currentModule,
            SMSGatewayDefination smsGatewayDefination
        )
            : this(currentModule)
        {
            this.smsGatewayDefination = smsGatewayDefination;
        }

        public override User CloneModel(User model)
        {
            return new User
            {
                Username = model.Username,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                Password = model.Password,
                AccountEntityId = model.AccountEntityId,
                Id = model.Id,
                IsActive = model.IsActive,
                EncryptionKey = model.EncryptionKey,
                NoOtp = model.NoOtp,
                OtpUseMail = model.OtpUseMail,
                OtpUseSms = model.OtpUseSms,
                SupportedUserTypeId = model.SupportedUserTypeId,
            };
        }

        public async Task<ExecutionResult<AuthenticateResponse>> Authenticate(
            AuthenticateRequest model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AuthenticateResponse>(
                true,
                ConfigConstants.AuthenticationSuccessful
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                #region validate

                if (model == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "model is null";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Username))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the user name";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the password";
                    return executionResult;
                }

                #endregion

                var passwordHashed = EncryptionHelper.SHA256HexHash(model.Password);

                //var connn = context.Database.GetDbConnection();
                //simple login logic for now
                var loggedInUser = await context.Users.FirstOrDefaultAsync(
                    r => r.Username == model.Username
                );

                bool? requiresOtp = null;
                bool requiresResetPassword = false;

                string invalidUsernameAndPasswordString = "Invalid username or password";
                if (
                    loggedInUser == null /*|| (loggedInUser != null /*&& loggedInUser.IsActive == false*/
                )
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = invalidUsernameAndPasswordString;
                    return executionResult;
                }
                else if (loggedInUser.IsActive == false && loggedInUser != null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Your account is inactive.To activate your account contact the Administrator.";
                    return executionResult;
                }
                else if (
                    !string.IsNullOrEmpty(loggedInUser.Password)
                    && loggedInUser.Password != passwordHashed
                    && IsResetPasswordRequest == false
                )
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = invalidUsernameAndPasswordString;
                    return executionResult;
                }
                else if (
                    string.IsNullOrEmpty(loggedInUser.Password) || IsResetPasswordRequest == true
                )
                {
                    //executionResult.IsOkay = false;
                    executionResult.Message = ConfigConstants.RenewPasswordRequiredText;
                    requiresResetPassword = true;
                }

                var rawAuthToken = Guid.NewGuid().ToString();
                var authTokenHash = AuthenticateTokenHash(rawAuthToken);
                //incase the user is valid Just log them in and generate a token and saved it as hashed on the DB to prevent snooping

                var logLoginAttempt = new UserLoginAttempt
                {
                    AccountDeactivated = false,
                    AttemptDate = DateTime.UtcNow,
                    AttemptTime = DateTime.UtcNow.TimeOfDay,
                    LastAccessTime = DateTime.UtcNow,
                    AuthToken = authTokenHash,
                    IpAddress = model.RemoteIpAddress,
                    OptCode = "NA",
                    LoginSuccessful = true,
                    UserId = loggedInUser.Id,
                    RequestDomain = model.Domain,
                    ResetPassword = requiresResetPassword,
                    OtpCodeValidated = requiresOtp,
                    SessionExpired = false,
                    ScreenLocked = false,
                    TransactingBranchId = 0
                };

                context.UserLoginAttempts.Add(logLoginAttempt);

                if (createContextHere)
                {
                    //executionResult.Result = new AuthenticateResponse(loggedInUser, JsonConvert.SerializeObject(logLoginAttempt));
                    //return executionResult;
                    await context.SaveChangesAsync();

                    //executionResult.Message = "After save";
                }
                executionResult.Result = new AuthenticateResponse(loggedInUser, rawAuthToken);
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        internal async Task<ExecutionResult<UserLoginAttempt>> LastLoginData(
            User user,
            string currentLoginToken,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<UserLoginAttempt>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var authTokenHash = AuthenticateTokenHash(currentLoginToken);

                var currentToken = await context.UserLoginAttempts.FirstOrDefaultAsync(
                    r => r.AuthToken == authTokenHash && r.UserId == user.Id
                );
                if (currentToken == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"Sorry the token supplied does not exist";
                    return executionResult;
                }

                var previousToken = await context.UserLoginAttempts
                    .OrderByDescending(r => r.Id)
                    .LastOrDefaultAsync(
                        r =>
                            r.Id < currentToken.Id
                            && r.UserId == user.Id
                            && r.LoginSuccessful == true
                    );
                if (previousToken == null)
                {
                    //incase this is the first login then use this one the previos one
                    previousToken = currentToken;
                }

                executionResult.Result = new UserLoginAttempt
                {
                    AttemptDate = previousToken.AttemptDate,
                    AttemptTime = previousToken.AttemptTime,
                    IpAddress = previousToken.IpAddress,
                };
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }

            return executionResult;
        }

        internal async Task<ExecutionResult<AuthenticateResponse>> RenewPassword(
            RenewPasswordModel model,
            AppJwtSettings appJwtSettings,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AuthenticateResponse>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                #region validate


                if (model == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "model is null";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Token))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token";
                    return executionResult;
                }
                if (string.IsNullOrEmpty(model.Domain))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid domain";
                    return executionResult;
                }

                if (model.Password != model.Password1)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Entered passwords do not match";
                    return executionResult;
                }

                var passwordStr = passwordStrengthChecker(model.Password);
                if (!string.IsNullOrEmpty(passwordStr))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Passwords not strong enough"; //failed message
                    return executionResult;
                }

                #endregion

                //the way to hack is to steal the token from the user not the db
                var authTokenHash = AuthenticateTokenHash(model.Token);
                //simple login logic for now
                var currentToken = await (
                    context.UserLoginAttempts.Join(
                        context.Users,
                        ula => ula.UserId,
                        usr => usr.Id,
                        (ula, usr) => new { usr, ula }
                    )
                ).FirstOrDefaultAsync(
                    r => r.ula.AuthToken == authTokenHash && r.ula.RequestDomain == model.Domain
                );

                if (currentToken == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token";
                    return executionResult;
                }

                var timeElapsed = DateTime.UtcNow - currentToken.ula.LastAccessTime;
                if (timeElapsed.TotalSeconds > appJwtSettings.SessionLengthSeconds)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. timed out";
                    return executionResult;
                }
                else if (currentToken.ula.SessionExpired)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. expired";
                    return executionResult;
                }
                else if (currentToken.ula.ResetPassword == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. Token not for reset password";
                    return executionResult;
                }
                //previously commented
                if (currentToken.ula.OtpCodeLastSentTime.HasValue)
                {
                    var timeElapsedOtp =
                        DateTime.UtcNow - currentToken.ula.OtpCodeLastSentTime.Value;
                    if (timeElapsedOtp.TotalMinutes > appJwtSettings.OTPExpiryMinutes)
                    {
                        executionResult.IsOkay = false;
                        executionResult.Message = "OTP has expired";
                        return executionResult;
                    }
                }
                else
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "Generate an OTP by clicking Send OTP and use to complete renew";
                    return executionResult;
                }
                //up to here
                var hashedOTP = AuthenticateTokenHash(model.OtpCode); //  "NA"

                if (hashedOTP != currentToken.ula.OptCode)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid OTP. OTP does not match the last one sent";
                    return executionResult;
                }

                currentToken.ula.LastAccessTime = DateTime.UtcNow;
                currentToken.ula.OptCode = hashedOTP;
                currentToken.usr.Password = EncryptionHelper.SHA256HexHash(model.Password);

                if (createContextHere)
                {
                    context.SaveChanges();
                    executionResult.Message = "Password renewed. Use it to login";
                    //executionResult.Result = CloneModel(currentToken.usr);
                }
                //else
                //    executionResult.Result = currentToken.usr;
                executionResult.Result = new AuthenticateResponse(currentToken.usr, model.Token);
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        private string passwordStrengthChecker(string password)
        {
            return "";
        }

        internal async Task<ExecutionResult<AuthenticateResponse>> SendRenewPasswordOTP(
            AuthenticateResponse model,
            string domain,
            AppJwtSettings appJwtSettings,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AuthenticateResponse>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                #region validate

                if (string.IsNullOrEmpty(domain))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "domain is empty";
                    return executionResult;
                }

                if (model == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "model is null";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Token))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token";
                    return executionResult;
                }

                #endregion

                //the way to hack is to steal the token from the user not the db
                var authTokenHash = AuthenticateTokenHash(model.Token);
                //simple login logic for now
                var currentToken = await (
                    context.UserLoginAttempts.Join(
                        context.Users,
                        ula => ula.UserId,
                        usr => usr.Id,
                        (ula, usr) => new { usr, ula }
                    )
                ).FirstOrDefaultAsync(
                    r => r.ula.AuthToken == authTokenHash && r.ula.RequestDomain == domain
                );

                if (currentToken == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token";
                    return executionResult;
                }

                var timeElapsed = DateTime.UtcNow - currentToken.ula.LastAccessTime;
                if (timeElapsed.TotalSeconds > appJwtSettings.SessionLengthSeconds)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. timed out";
                    return executionResult;
                }
                else if (currentToken.ula.SessionExpired)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. expired";
                    return executionResult;
                }
                else if (currentToken.ula.ResetPassword == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. Token not for reset password";
                    return executionResult;
                }

                currentToken.ula.LastAccessTime = DateTime.UtcNow;
                #region send the OTP CODE HERE
                var otpCodeGenerated = EncryptionHelper.GetRandomID(DateTime.Now.Ticks).ToString();
                //if (currentToken.usr.OtpUseSms || currentToken.usr.OtpUseMail == false)
                //{
                //var communicationRepo = new SMSTextMessageDefaultGateway(CurrentModule, smsGatewayDefination);
                var otpMessage =
                    $"Your one time password(OTP) is {otpCodeGenerated}. It expires in {appJwtSettings.OTPExpiryMinutes} minutes ";
                //var smsMessage = new SMSTextMessage(otpMessage, currentToken.usr.PhoneNumber, smsGatewayDefination.DefaultMaskName);
                //var resultSend = communicationRepo.SendSMS(new HashSet<SMSTextMessage> { smsMessage });
                //if (resultSend.IsOkay == false)
                //{
                //    executionResult.IsOkay = false;
                //    executionResult.Message = $"Sending OTP SMS failed. {resultSend.Message}";
                //    return executionResult;
                //}

                //send by email start
                //var builder = new ConfigurationBuilder().AddJsonFile(
                //    $"appsettings.json",
                //    true,
                //    true
                //);
                //var config = builder.Build();
                var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                var request = new MailRequest
                {
                    ToEmail = model.EmailAddress,
                    Subject = "One Time Password(OTP) code",
                    Body = otpMessage,
                    From = FromEmail
                };

                var uri = new Uri(Domain);
                var client = new RestClient(uri);
                client.AddDefaultHeader("Content-Type", "application/json");
                client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                reques.AddJsonBody(request);

                var restClientResult = client.Execute<MailRequest>(reques);

                //}
                //else
                //{
                //    executionResult.IsOkay = false;
                //    executionResult.Message = "Invalid set up. Use mail for OTP not supported";
                //    return executionResult;
                //}
                #endregion
                var hashedOTP = AuthenticateTokenHash(otpCodeGenerated);
                currentToken.ula.OptCode = hashedOTP;
                currentToken.ula.OtpCodeLastSentTime = DateTime.UtcNow;
                if (createContextHere)
                {
                    context.SaveChanges();
                    executionResult.Message =
                        "Your OTP code has been sent. Enter it and supply to create your password";
                    //executionResult.Result = CloneModel(currentToken.usr);
                }
                //else
                //    executionResult.Result = currentToken.usr;
                executionResult.Result = new AuthenticateResponse(currentToken.usr, model.Token);
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        internal async Task<ExecutionResult<AuthenticateResponse>> RenewPasswordInitiate(
            AuthenticateRequest model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            IsResetPasswordRequest = true;
            return await Authenticate(model, context);
        }

        internal async Task<ExecutionResult<string>> EndSession(
            string token,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<string>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                #region validate
                if (string.IsNullOrEmpty(token))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "token is empty";
                    return executionResult;
                }
                #endregion


                var authTokenHash = AuthenticateTokenHash(token);
                //simple login logic for now
                var currentToken = await (
                    context.UserLoginAttempts.Join(
                        context.Users,
                        ula => ula.UserId,
                        usr => usr.Id,
                        (ula, usr) => new { usr, ula }
                    )
                ).FirstOrDefaultAsync(r => r.ula.AuthToken == authTokenHash);

                if (currentToken == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token";
                    return executionResult;
                }
                currentToken.ula.LastAccessTime = DateTime.UtcNow;
                currentToken.ula.SessionExpired = true;

                if (createContextHere)
                {
                    context.SaveChanges();
                    executionResult.Message = "Login successful";
                }
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        /// <summary>
        /// Generates the token Hash for authentication sakes.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string AuthenticateTokenHash(string token)
        {
            return EncryptionHelper.SHA256Base64Hash(token);
        }

        internal async Task<ExecutionResult<User>> ValidateToken(
            string token,
            string domain,
            AppJwtSettings appJwtSettings,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<User>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                #region validate

                if (string.IsNullOrEmpty(token))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "token is empty";
                    return executionResult;
                }

                #endregion

                //the way to hack is to steal the token from the user not the db
                var authTokenHash = AuthenticateTokenHash(token);
                //simple login logic for now
                var currentToken = await (
                    context.UserLoginAttempts.Join(
                        context.Users,
                        ula => ula.UserId,
                        usr => usr.Id,
                        (ula, usr) => new { usr, ula }
                    )
                ).FirstOrDefaultAsync(
                    r => r.ula.AuthToken == authTokenHash && r.ula.RequestDomain == domain
                );

                if (currentToken == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token";
                    return executionResult;
                }

                var timeElapsed = DateTime.UtcNow - currentToken.ula.LastAccessTime;
                if (timeElapsed.TotalSeconds > appJwtSettings.SessionLengthSeconds)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. Session timed out";
                    return executionResult;
                }
                else if (currentToken.ula.SessionExpired)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. Session expired";
                    return executionResult;
                }
                else if (currentToken.ula.ResetPassword)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Invalid token. Token for reset password";
                    executionResult.TechnicalMessage = ConfigConstants.RenewPasswordRequiredText;
                    return executionResult;
                }
                else if (
                    currentToken.ula.OtpCodeValidated.HasValue
                    && currentToken.ula.OtpCodeValidated.Value == false
                )
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "Invalid token. Token for otp code, Otp not validated";
                    executionResult.TechnicalMessage = ConfigConstants.OTPCodeRequired;
                    return executionResult;
                }

                currentToken.ula.LastAccessTime = DateTime.UtcNow;
                if (createContextHere)
                {
                    context.SaveChanges();
                    executionResult.Message = "Valid token";
                    executionResult.Result = CloneModel(currentToken.usr);
                }
                else
                    executionResult.Result = currentToken.usr;
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public async Task<ExecutionResult<User>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<User>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.Users.FirstOrDefaultAsync(r => r.Id == id);
                if (DbInstance == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"The entry with id {id} supplied does not exist";
                    return executionResult;
                }

                //set the Result of executionResult object to the item found in the database
                //to avoid issues with serialization always use new
                executionResult.Result = CloneModel(DbInstance);
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }
    }
}