
using Authentication.Jwt.Models;
using CommunicationLibrary.Gateways.Config;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectBase.Database.Connection;
using ProjectBase.Repository;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Repository
{
    public class UserSelfRegistrationRepository
        : DbContextRepository<AccountEntity, EFDatabaseModelDatabaseContext>
    {
        //private SMSGatewayDefination smsGatewayDefination;
        String module = null;

        public UserSelfRegistrationRepository(string currentModule)
            : base(currentModule)
        {
            module = currentModule;
        }

       

        public override AccountEntity CloneModel(AccountEntity model)
        {
            var Value = new AccountEntity
            {
                Id = model.Id,
                SubAccountBranchId = model.SubAccountBranchId,
                EntityNo = model.EntityNo,
                EntityName = model.EntityName,
                RegDate = model.RegDate,
                DateOfBirth = model.DateOfBirth,
                Phone1 = model.Phone1,
                Phone2 = model.Phone2,
                Email = model.Email,
                IsActive = model.IsActive,
                PhysicalAddress = model.PhysicalAddress,
                PostalAddress = model.PostalAddress,
                WhiteListedDomain = model.WhiteListedDomain,
                EntityUserName = model.EntityUserName,
                IsBackUpRecord = model.IsBackUpRecord,
                EntityDetailsValidated = model.EntityDetailsValidated,
                IdentificationDocumentNumber = model.IdentificationDocumentNumber,
                IdentificationDocumentTypeId = model.IdentificationDocumentTypeId,
                GenderId = model.GenderId,
                AccountEntityActiveStateId = model.AccountEntityActiveStateId
            };

            return Value;
        }
       

            public async Task<ExecutionResult<RegisterResponse>> Register(
            RegisterRequest model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<RegisterResponse>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                #region validate

                if (model == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "model is  surely null";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Username))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the user name";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the Email Address";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.Phone))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the Phone";
                    return executionResult;
                }

                if (string.IsNullOrEmpty(model.PhysicalAddress))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the Physical Address";
                    return executionResult;
                }
                if (string.IsNullOrEmpty(model.FullName))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the Name";
                    return executionResult;
                }
                if (string.IsNullOrEmpty(model.NationalID))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the National ID or Company Reg";
                    return executionResult;
                }
                if (string.IsNullOrEmpty(model.EntityTypeName))
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Supply the User category applicable to you";
                    return executionResult;
                }

                #endregion

                //simple register logic for now

                //var parentEntity = await context.AccountEntities
                //    .Join(
                //        context.SubAccountBranches,
                //        ae => ae.SubAccountBranchId,
                //        sab => sab.Id,
                //        (ae, sab) => new { ae, sab }
                //    )
                //    .Join(
                //        context.SubAccounts,
                //        x => x.sab.SubAccountId,
                //        sa => sa.Id,
                //        (x, sa) => new { x, sa }
                //    )
                //    .Where(r => r.x.ae.WhiteListedDomain == model.Domain)
                //    .FirstOrDefaultAsync();

                //if (parentEntity == null)
                //{
                //    executionResult.IsOkay = false;
                //    executionResult.Message = "Parent account with this domain does not exist";
                //    return executionResult;
                //}
                var entityexist = await context.AccountEntities.FirstOrDefaultAsync(
                    r => r.Email == model.Email || r.Phone1 == model.Phone || r.IdentificationDocumentNumber == model.NationalID || r.EntityUserName == model.Username
                );
                if (entityexist != null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "An account with this email/phone/Username/ID No. already exists.Please check your email for login credentials or reset password if you remember the username";
                    return executionResult;
                }

                var otherGender = await context.Genders.FirstOrDefaultAsync(
                    r => r.GenderName == DefaultConfiguration.Gender.OtherGenderName
                );
                if (otherGender == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Gender with name {DefaultConfiguration.Gender.OtherGenderName} has not been defined";
                    return executionResult;
                }

                var natDocType = await context.IdentificationDocumentTypes.FirstOrDefaultAsync(
                    r =>
                        r.DocumentName
                        == DefaultConfiguration.IdentificationDocumentType.NationalIDDocumentName
                );
                if (natDocType == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Identification document type with name {DefaultConfiguration.IdentificationDocumentType.NationalIDDocumentName} has not been defined";
                    return executionResult;
                }

                var accountEntityActiveState =
                    await context.AccountEntityActiveStates.FirstOrDefaultAsync(
                        r =>
                            r.StateName
                            == DefaultConfiguration.AccountActiveState.ActiveAccountStateName
                    );
                if (accountEntityActiveState == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Account active state with name {DefaultConfiguration.AccountActiveState.ActiveAccountStateName} has not been defined";
                    return executionResult;
                }

                var supportedUserType = await context.SupportedUserTypes.FirstOrDefaultAsync(
                    r => r.UserType == DefaultConfiguration.SupportedUserType.WebUserType
                );
                if (supportedUserType == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"The supported user type with name {DefaultConfiguration.SupportedUserType.WebUserType} has not been defined";
                    return executionResult;
                }

                var defaultAccount1 = await context.MfsAccountTypes.FirstOrDefaultAsync(
                    r => r.AccountTypeName == DefaultConfiguration.Accounts.AccountName  
                  
                );
                var defaultAccount2 = await context.MfsAccountTypes.FirstOrDefaultAsync(
                    r => r.AccountTypeName == DefaultConfiguration.Accounts.RetainerAccount

                );
                var defaultAccount3 = await context.MfsAccountTypes.FirstOrDefaultAsync(
                    r => r.AccountTypeName == DefaultConfiguration.Accounts.ApplicationAccount

                );
                if ( defaultAccount1 == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Account with name {DefaultConfiguration.Accounts.AccountName} has not been defined";
                    return executionResult;
                }
                if (defaultAccount2 == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Account with name {DefaultConfiguration.Accounts.RetainerAccount} has not been defined";
                    return executionResult;
                }

                if (defaultAccount3 == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Account with name {DefaultConfiguration.Accounts.ApplicationAccount} has not been defined";
                    return executionResult;
                }

                var defaultCurrency = await context.MfsCurrencyTypes.FirstOrDefaultAsync(
                    r => r.CurrencyCode == DefaultConfiguration.Currency.defaultCurrency
                );
                if (defaultCurrency == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"The currency  with code {DefaultConfiguration.SupportedUserType.WebUserType} has not been defined";
                    return executionResult;
                }

                var systemRole = context.SystemRoles.FirstOrDefault(y => y.RoleName == DefaultConfiguration.Systemroles.roleName);


                var code = ProjectBase.Security.EncryptionHelper
                    .GetRandomID(DateTime.UtcNow.Ticks, 0, 10000)
                    .ToString();
                var entityType = await context.AccountEntityTypes.FirstOrDefaultAsync(
                    r => r.TypeName == model.EntityTypeName
                );
                var activate = true;

                if (model.EntityTypeName == DefaultConfiguration.AccountEntityType.agentTypeName)
                {
                    activate = false;
                }

                var subAccountBranch = new SubAccountBranch
                {
                    BranchCode = code,
                    BranchName = model.FullName,
                    Phone1 = model.Phone,
                    Phone2 = "",
                    PostalAddress = model.PostalAddress,
                    BuildingAddress = model.PhysicalAddress,
                    SubAccount = new SubAccount
                    {
                        AccountName = model.FullName,
                        AccountNo = code,
                        ContactPerson = model.Username,
                        Email = model.Email,
                        DateCreated = DateTime.UtcNow,
                        Phone = model.Phone,
                        IsActive = true
                        //ParentId = parentEntity.sa.Id
                    }
                };

                var modelInsert = new AccountEntity
                {
                    EntityNo = code,
                    EntityName = model.FullName,
                    RegDate = DateTime.UtcNow,
                    DateOfBirth = DateTime.UtcNow.AddYears(-18),
                    Phone1 = model.Phone,
                    Email = model.Email,
                    IsActive = true,
                    PhysicalAddress = model.PhysicalAddress,
                    PostalAddress = model.PostalAddress,
                    EntityUserName = model.Username,
                    EntityDetailsValidated = false,
                    IdentificationDocumentNumber = model.NationalID,
                    IdentificationDocumentTypeId = natDocType.Id,
                    GenderId = otherGender.Id,
                    AccountEntityActiveStateId = accountEntityActiveState.Id,
                    AccountEntityTypeId=entityType.Id
                };

                var user = new User
                {
                    Password = null,
                    Username = model.Username,
                    IsActive = activate,
                    PhoneNumber = model.Phone,
                    EmailAddress = model.Email,
                    OtpUseSms = true,
                    OtpUseMail = false,
                    NoOtp = false,
                    PasswordDuration = 15,
                    UserImagePath = "",
                    SupportedUserTypeId = supportedUserType.Id,
                };

                var userRole = new UserRole
                {
                    User = user,
                    SystemRoleId = systemRole.Id,
                    Active = true
                };

                context.UserRoles.Add(userRole);

                var listBranch = new List<UserBranch>();
                listBranch.Add(
                    new UserBranch { IsActive = true, SubAccountBranch = subAccountBranch }
                );
                user.UserBranches = listBranch;

                modelInsert.Users.Add(user);

                modelInsert.SubAccountBranch = subAccountBranch;

                
                context.AccountEntities.Add(modelInsert);


                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                  
                    var Domain =ContextConnectionService._configuration["AppConstants:Domain"];
                    var AdminEmail = ContextConnectionService._configuration["AppConstants:AdminEmail"];
                    var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                    var request = new MailRequest
                    {
                        ToEmail = modelInsert.Email,
                        Subject = "New Account Credentials",
                        Body =
                            $"Your account username is:{modelInsert.EntityUserName} and password is:{code}.Use them to login at this url: {Domain}",
                        From = FromEmail
                    };

                    var Adminrequest = new MailRequest
                    {
                        ToEmail = AdminEmail,
                        Subject = $"New {modelInsert.AccountEntityType.TypeName} Registered",
                        Body =
                            $"A new {modelInsert.AccountEntityType.TypeName} has registered. Username is:{modelInsert.EntityUserName} and email is {modelInsert.Email}",
                        From = FromEmail
                    };


                    var uri = new Uri(Domain);
                    var client = new RestClient(uri);
                    client.AddDefaultHeader("Content-Type", "application/json");
                    client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                    //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                    var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                    reques.AddJsonBody(request);

                    var restClientResult = await client.ExecuteAsync<MailRequest>(reques);


                    var AdminClient = new RestClient(uri);
                    AdminClient.AddDefaultHeader("Content-Type", "application/json");
                    AdminClient.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                    //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");

                    var Adminreques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                    Adminreques.AddJsonBody(Adminrequest);

                    var restAdminResult = await AdminClient.ExecuteAsync<MailRequest>(Adminreques);


                    executionResult.Message =
                        $"Account Created successfully.Check your email for login credentials."; //Username:{modelInsert.EntityUserName}  Password:{code}

                }
                else
                {
                    return executionResult;
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

        public async Task<ExecutionResult<IEnumerable<AccountEntityType>>> GetEntityType()
        {
            var result = new ExecutionResult<IEnumerable<AccountEntityType>>(
                true,
                "",
                new HashSet<AccountEntityType>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                  
                    var returnData =await context.AccountEntityTypes.ToListAsync();

                    result.Result = returnData.Where(r => r.TypeName == "Intern" || r.TypeName == "Company" || r.TypeName == "Admin" )
                        .Select(
                        r =>
                            new AccountEntityType
                            {
                                Id = r.Id,
                               TypeName = r.TypeName,
                                Prefix = r.Prefix
                               
                            }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }
    }
}
