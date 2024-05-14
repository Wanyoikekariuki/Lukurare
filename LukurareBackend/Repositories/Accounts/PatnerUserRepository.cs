using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFDatabaseModel.Contexts;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using ProjectBase.Repository.Configuration;
using RestSharp;
using ProjectBase.Database.Connection;

namespace LukurareBackend.Repositories.Accounts
{
    public class PatnerUserRepository : EFDatabaseModelBaseRepository<User>
    {
        public PatnerUserRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override User CloneModel(User model)
        {
            return new User
            {
                Id = model.Id,
                Username = model.Username,
                Password = model.Password,
                AccountEntityId = model.AccountEntityId,
                IsActive = model.IsActive,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                OtpUseSms = model.OtpUseSms,
                OtpUseMail = model.OtpUseMail,
                NoOtp = model.NoOtp,
                PasswordDuration = model.PasswordDuration,
                UserImagePath = model.UserImagePath,
                SupportedUserTypeId = model.SupportedUserTypeId,
                EncryptionKey = model.EncryptionKey
            };
        }

        public async override Task<ExecutionResult<User>> Add(
            User model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<User>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();
             
                var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];


                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result
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

                var modelInsert = new User
                {
                    AccountEntityId = UserPrinciple.User.AccountEntityId,
                    Password = null,
                    Username = model.Username,
                    IsActive = true,
                    PhoneNumber = model.PhoneNumber,
                    EmailAddress = model.EmailAddress,
                    OtpUseSms =true,
                    OtpUseMail = false,
                    NoOtp = false,
                    PasswordDuration = 15,
                    UserImagePath = "",
                    SupportedUserTypeId = supportedUserType.Id,
                };

                context.Users.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    var request = new MailRequest
                    {
                        ToEmail = modelInsert.EmailAddress,
                        Subject = "New Account Credentials",
                        Body =
                            $"Your account username is:{modelInsert.Username} and password is:{modelInsert.PhoneNumber}.Use them to login at this url: {Domain}",
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
                    executionResult.Message = "Create operation was successful";

                    //return a new instance of the inserted object

                    //executionResult.Result = await GetModelForInsertUpdateFromId(modelInsert.Id, modelInsert, context);
                }
                else
                {
                    //incase the context was not created here return the object
                    //as inserted to the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    executionResult.Result = modelInsert;
                }
            }
            catch (Exception ex)
            {
                executionResult.Message = "Error occurred";
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

        public async override Task<ExecutionResult<User>> Delete(
            User model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<User>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                executionResult = await IsValidGeneral(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                executionResult = await GetItem((int)model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.Users.Remove(DbInstance);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = CloneModel(DbInstance);
                }
                else
                {
                    //incase the context was not created here return the object
                    //thats removed from the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    executionResult.Result = DbInstance;
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

        public async override Task<ExecutionResult<User>> GetItem(
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

        public async override Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "User instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of Account Entity then ensure the Filter  Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("AccountEntityId"))
                    filterValue = args.GetFilterValuePredicate(
                        r => r.field == "AccountEntityId",
                        0
                    );

                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) || parsedInt == false)
                    filterValueInt = -1;

                int filterValueIntId = -1;
                string filterValueId = null;
                if (args.HasFilterFields("Id"))
                    filterValueId = args.GetFilterValuePredicate(r => r.field == "Id", 0);

                parsedInt = int.TryParse(filterValueId, out filterValueIntId);
                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
                    filterValueIntId = -1;

                var theusers = (
                    await context.Users
                        .Join(
                            context.SupportedUserTypes,
                            u => u.SupportedUserTypeId,
                            sut => sut.Id,
                            (u, sut) => new { u, sut }
                        )
                        .Where(r => r.u.AccountEntityId == UserPrinciple.User.AccountEntityId) //filterValueInt == -1 ? (filterValueIntId == -1 ? true : r.u.Id == filterValueIntId) : r.u.AccountEntityId == filterValueInt)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.u.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new User
                        {
                            Id = r.u.Id,
                            Username = r.u.Username,
                            IsActive = r.u.IsActive,
                            PhoneNumber = r.u.PhoneNumber,
                            EmailAddress = r.u.EmailAddress

                            //SupportedUserType = new SupportedUserType
                            //{
                            //    Id = r.sut.Id,
                            //    UserType = r.sut.UserType
                            //}
                        }
                );

                var filterSupporter = new FilterSupporter<User>(theusers, args);
                var filteredData = filterSupporter.FilterData<int>(r => (int)r.Id);
                executionResult.Result = filterSupporter.itemsResult;
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

        public async override Task<ExecutionResult<User>> IsValidGeneral(
            User model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run
            return await Task.Run<ExecutionResult<User>>(() =>
            {
                var result = new ExecutionResult<User>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.Username))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the user name";
                    return result;
                }

                if (string.IsNullOrEmpty(model.PhoneNumber))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the phone number";
                    return result;
                }
                if (string.IsNullOrEmpty(model.EmailAddress))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the email address";
                    return result;
                }

                return result;
            });
        }

        public async override Task<ExecutionResult<User>> IsValidInsert(
            User model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation
            var countryAlreadyRegistered = await context.Users
                .Where(r => r.Username == model.Username)//&& model.AccountEntityId==UserPrinciple.User.AccountEntityId
                .FirstOrDefaultAsync();
            if (countryAlreadyRegistered != null)
            {
                result.IsOkay = false;
                
                    result.Message =
                        $"The user with username: {countryAlreadyRegistered.Username}  is already registered in  this the system ";

                return result;
            }

            return result;
        }

        public async override Task<ExecutionResult<User>> IsValidUpdate(
            User model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            result = await GetItem((int)model.Id, context);
            if (result.IsOkay == false)
                return result;

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<User>> Update(
            User model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<User>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate

                DbInstance.Username = model.Username;

                DbInstance.IsActive = model.IsActive;
                DbInstance.PhoneNumber = model.PhoneNumber;
                DbInstance.EmailAddress = model.EmailAddress;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        DbInstance.Id,
                        DbInstance,
                        context
                    );
                }
                else
                {
                    //incase the context was not created here return the object
                    //thats removed from the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    executionResult.Result = DbInstance;
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

        #region fill combo boxes
        public async Task<ExecutionResult<IEnumerable<SupportedUserType>>> GetUserType(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SupportedUserType>>(
                true,
                "",
                new HashSet<SupportedUserType>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    var filterValue = args.GetFilterValue(0).ToLower();
                    ;
                    if (string.IsNullOrEmpty(filterValue))
                        return result;

                    var returnData = await context.SupportedUserTypes
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SupportedUserType { Id = r.Id, UserType = r.UserType }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        #endregion

        public async Task<ExecutionResult<User>> UploadAccounts(
            IFormFileCollection files,
            string path
        )
        {
            var executionResult = new ExecutionResult<User>(true);

            if (files.Count() < 1)
            {
                executionResult.IsOkay = false;
                executionResult.Message = "No file uploaded";
                return executionResult;
            }
            try
            {
                long uploaded_size = 0;

                string path_for_Uploaded_Files = $"{path}\\UploadedFiles\\";

                //</ init >



                //< get form_files >

                //IFormFile uploaded_File

                var uploaded_files = files;

                //</ get form_files >



                //------< @Loop: Uploaded Files >------

                int iCounter = 0;

                string sFiles_uploaded = "";

                string uploaded_Filename = "";
                string new_Filename_on_Server = "";

                foreach (var uploaded_file in uploaded_files)
                {
                    //----< Uploaded File >----

                    iCounter++;

                    uploaded_size += uploaded_file.Length;

                    sFiles_uploaded += "\n" + uploaded_file.FileName;

                    //< Filename >

                    uploaded_Filename = uploaded_file.FileName;

                    new_Filename_on_Server =
                        $"{path_for_Uploaded_Files}\\{Guid.NewGuid().ToString()}{uploaded_Filename}";

                    //</ Filename >



                    //< Copy File to Target >

                    using (
                        FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create)
                    )
                    {
                        await uploaded_file.CopyToAsync(stream);
                    }

                    //< Copy File to Target >

                    //----</ Uploaded File >----
                }

                //
                var accountsList = new List<User>();
                FileInfo existingFile = new FileInfo(new_Filename_on_Server);
                using (var package = new ExcelPackage(existingFile))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.FirstOrDefault();

                    if (workSheet == null)
                    {
                        executionResult.IsOkay = false;
                        executionResult.Message = "No worksheet found";
                        return executionResult;
                    }
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    List<User> acounts = new List<User>();
                    using (
                        EFDatabaseModelDatabaseContext context =
                            new EFDatabaseModelDatabaseContext()
                    )
                    {
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            string Username = null,
                                Phone = null,
                                Email = null;
                            if (workSheet.Cells[rowIterator, 1].Value != null)
                            {
                                Username = workSheet.Cells[rowIterator, 1].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 2].Value != null)
                            {
                                Phone = workSheet.Cells[rowIterator, 2].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                Email = workSheet.Cells[rowIterator, 3].Value.ToString();
                            }

                            if (!(Username == null && Phone == null && Email == null))
                            {
                                if (string.IsNullOrEmpty(Username))
                                {
                                    executionResult.IsOkay = false;
                                    executionResult.Message = "Please supply all Usernames";
                                    return executionResult;
                                }

                                var pIdInDb = context.Users.FirstOrDefault(
                                    r =>
                                        r.Username.Equals(Username)
                                        && r.AccountEntityId == UserPrinciple.User.AccountEntityId
                                );
                                if (pIdInDb == null)
                                {
                                    var row = new User
                                    {
                                        Username = Username,
                                        PhoneNumber = Phone,
                                        EmailAddress = Email,
                                        IsActive = true
                                    };
                                    accountsList.Add(row);
                                }
                            }
                        }
                    }

                    foreach (var row in accountsList)
                    {
                        executionResult = await Add(row);
                    }

                    //if (executionResult.IsOkay == false)
                    //{
                    //    return executionResult;
                    //}
                    //else
                    //{
                    //    await context.SaveChangesAsync();
                    //    executionResult.Message = "Users saved successfully";
                    //}
                }
            }
            catch (Exception ex) { }

            return executionResult;
        }
    }
}
