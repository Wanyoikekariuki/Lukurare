using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using Microsoft.EntityFrameworkCore;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Entities
{
    public class PatnerAccountEntityRepository : EFDatabaseModelBaseRepository<AccountEntity>
    {
        public PatnerAccountEntityRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override AccountEntity CloneModel(AccountEntity model)
        {
            var Value = new AccountEntity
            {
                Id = model.Id,
                SubAccountBranchId = model.SubAccountBranchId,
                EntityNo = ProjectBase.Security.EncryptionHelper.GetRandomID(DateTime.UtcNow.Ticks, 0, 1000).ToString(),
                EntityName = model.EntityName,
                RegDate =DateTime.UtcNow,
                DateOfBirth = DateTime.UtcNow.AddYears(-18),
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
                //IdentificationDocumentTypeId = model.IdentificationDocumentTypeId,
                //AccountEntityActiveStateId = model.AccountEntityActiveStateId
            };

            return Value;
        }

        public async override Task<ExecutionResult<AccountEntity>> Add(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);
            try
            {
                if (createContextHere)

                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed execution result

                //insert the okay object to the DB
                var modelInsert = CloneModel(model);
                var entityexist = await context.AccountEntities.FirstOrDefaultAsync(
                    r =>  r.Phone1 == modelInsert.Phone1
                );
                if (entityexist != null)
                {
                    executionResult.Result = entityexist;
                    //executionResult.IsOkay = false;
                    executionResult.Message = "An account with this email or phone already exists";
                    return executionResult;
                }
                var entity = await context.AccountEntities.FirstOrDefaultAsync(
                    r => r.Id == UserPrinciple.User.AccountEntityId
                );
                var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                    r => r.Id == entity.SubAccountBranchId
                );
                var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                    r => r.Id == branchi.SubAccountId
                );
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
                modelInsert.GenderId = otherGender.Id;
                var defaultAccount = await context.AccountEntityTypes.FirstOrDefaultAsync(
                  r => r.TypeName == DefaultConfiguration.AccountEntityType.customerTypeName
              );
                if (defaultAccount == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Account with name {DefaultConfiguration.AccountEntityType.customerTypeName} has not been defined";
                    return executionResult;
                }
                modelInsert.AccountEntityTypeId = defaultAccount.Id;
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
                var activestates = await context.AccountEntityActiveStates.FirstOrDefaultAsync(
                    r =>
                        r.StateName
                        == DefaultConfiguration.AccountActiveState.ActiveAccountStateName
                );
				var documentType = await context.IdentificationDocumentTypes.FirstOrDefaultAsync(
					r =>
						r.DocumentName
						== DefaultConfiguration.IdentificationDocumentType.MobilePhoneDocumentName
				);
                modelInsert.IdentificationDocumentTypeId= documentType.Id;
				if (activestates == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"The Account entity state type with name {DefaultConfiguration.AccountActiveState.ActiveAccountStateName} has not been defined";
                    return executionResult;
                }
                modelInsert.AccountEntityActiveStateId = activestates.Id;
                // modelInsert.AccountEntityActiveState.AccountEntities = null;
                //    new AccountEntityActiveState {

                //StateName=activestates.StateName,
                //AccountEntities=null
                //};
                var code = ProjectBase.Security.EncryptionHelper.GetRandomID(DateTime.UtcNow.Ticks, 0, 1000).ToString();
                //modelInsert.EntityNo = code;
                var subAccountBranch = new SubAccountBranch
                {
                    BranchCode = code,
                    BranchName = model.EntityName,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    PostalAddress = model.PostalAddress,
                    BuildingAddress = model.PhysicalAddress,
                    SubAccount = new SubAccount
                    {
                        AccountName = model.EntityName,
                        AccountNo = code,
                        ContactPerson = model.EntityName,
                        Email = model.Email,
                        DateCreated = DateTime.UtcNow,
                        Phone = model.Phone1,
                        IsActive = model.IsActive,
                        ParentId = subacc.Id //UserPrinciple.User.AccountEntityId //
                    }
                };
                var userCreate = new User
                {
                   
                    Password = null,
                    Username = model.EntityUserName,
                    IsActive = model.IsActive,
                    PhoneNumber = model.Phone1,
                    EmailAddress = model.Email,
                    OtpUseSms = true,
                    OtpUseMail = false,
                    NoOtp = false,
                    PasswordDuration = 15,
                    SupportedUserTypeId = supportedUserType.Id,
                    UserImagePath = ""
                };

                var listBranch = new List<UserBranch>();
                listBranch.Add(
                    new UserBranch { IsActive = true, SubAccountBranch = subAccountBranch }
                );

                userCreate.UserBranches = listBranch;
                modelInsert.Users.Add(userCreate);

                modelInsert.SubAccountBranch = subAccountBranch;
                context.AccountEntities.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    executionResult.Result = modelInsert;
                    /*executionResult.Result = await GetModelForInsertUpdateFromId(
                        modelInsert.Id,
                        modelInsert,
                        context
                    );**/
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

        public async override Task<ExecutionResult<AccountEntity>> Delete(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);
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

                context.AccountEntities.Remove(DbInstance);
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

        public async override Task<ExecutionResult<AccountEntity>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //var dbJonInstance = await context.AccountEntities
                //    .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
                //    .Join(context.SubAccounts, aesab => aesab.sab.SubAccountId, sa => sa.Id, (aesab, sa) => new { aesab, sa })
                //   .FirstOrDefaultAsync(r => r.aesab.ae.Id == id);
                DbInstance = await context.AccountEntities.FirstOrDefaultAsync(r => r.Id == id);
                if (DbInstance == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"The entry with id {id} supplied does not exist";
                    return executionResult;
                }

                //DbInstance = dbJonInstance.aesab.ae;
                //DbInstance.SubAccountBranch = dbJonInstance.aesab.sab;
                //DbInstance.SubAccountBranch.SubAccount = dbJonInstance.sa;

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
            >(true, "AccountEntity instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                int filterValueIntId = -1;
                string filterValueId = string.Empty;
                if (args.HasFilterFields("Id"))
                    filterValueId = args.GetFilterValuePredicate(r => r.field == "Id", 0);

                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
                    filterValueIntId = -1;

                var accentity = (
                    await context.AccountEntities
                        .Join(
                            context.IdentificationDocumentTypes,
                            ae => ae.IdentificationDocumentTypeId,
                            idt => idt.Id,
                            (ae, idt) => new { ae, idt }
                        )
                        .Join(
                            context.AccountEntityActiveStates,
                            x => x.ae.AccountEntityActiveStateId,
                            s => s.Id,
                            (x, s) => new { x, s }
                        )
                        //.Where(r => filterValueIntId == -1 ? true : r.x.ae.Id == filterValueIntId)
                        .Where(
                            r =>
                                r.x.ae.SubAccountBranch.SubAccount.ParentId
                                == UserPrinciple.User.AccountEntityId
                        )
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.x.ae.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntity
                        {
                            Id = r.x.ae.Id,
                            SubAccountBranchId = r.x.ae.SubAccountBranchId,
                            EntityNo = r.x.ae.EntityNo,
                            EntityName = r.x.ae.EntityName,
                            RegDate = r.x.ae.RegDate,
                            DateOfBirth = r.x.ae.DateOfBirth,


                            Phone1 = r.x.ae.Phone1,
                            Phone2 = r.x.ae.Phone2,
                            Email = r.x.ae.Email,
                            IsActive = r.x.ae.IsActive,
                            PhysicalAddress = r.x.ae.PhysicalAddress,
                            PostalAddress = r.x.ae.PostalAddress,
                            WhiteListedDomain = r.x.ae.WhiteListedDomain,
                            EntityUserName = r.x.ae.EntityUserName,
                            IsBackUpRecord = r.x.ae.IsBackUpRecord,
                            EntityDetailsValidated = r.x.ae.EntityDetailsValidated,
                            IdentificationDocumentNumber = r.x.ae.IdentificationDocumentNumber,
                            IdentificationDocumentTypeId = r.x.ae.IdentificationDocumentTypeId,
                            IdentificationDocumentType = new IdentificationDocumentType
                            {
                                Id = r.x.idt.Id,
                                DocumentName = r.x.idt.DocumentName
                            },
                            AccountEntityActiveState = new AccountEntityActiveState
                            {
                                Id = r.s.Id,
                                StateName = r.s.StateName
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<AccountEntity>(accentity, args);
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

        public override Task<ExecutionResult<AccountEntity>> IsValidGeneral(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<AccountEntity>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }
            if (string.IsNullOrEmpty(model.EntityName))
            {
                result.IsOkay = false;
                result.Message = "Please supply the Entity Name";
                return Task.FromResult(result);
            }
            //if (string.IsNullOrEmpty(model.EntityNo))
            //{
            //    result.IsOkay = false;
            //    result.Message = "Please supply the Entity Number";
            //    return Task.FromResult(result);
            //}

            if (string.IsNullOrEmpty(model.Phone1))
            {
                result.IsOkay = false;
                result.Message = "Please supply the Phone Number";
                return Task.FromResult(result);
            }

            //SubAccountSelected = await context.SubAccounts.FirstOrDefaultAsync(r => r.Id == model.SubAccountId);
            //if (SubAccountSelected == null)
            //{
            //    result.IsOkay = false;
            //    result.Message = "Please  supply the Sub Account ";
            //    return result;
            //}

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<AccountEntity>> IsValidInsert(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation


            return result;
        }

        public async override Task<ExecutionResult<AccountEntity>> IsValidUpdate(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            result = await GetItem((int)model.Id, context);
            if (result.IsOkay == false)
                return result;

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<AccountEntity>> Update(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is set in GETITEM in IsValidUpdate

                #region update the entity
                if (createContextHere)
                {
                    //DbInstance.EntityNo = model.EntityNo;
                    DbInstance.EntityName = model.EntityName;
                    DbInstance.EntityUserName = model.EntityName;
                    //DbInstance.RegDate = model.RegDate;
                    //DbInstance.DateOfBirth = model.DateOfBirth;
                    DbInstance.Phone1 = model.Phone1;
                    DbInstance.Phone2 = model.Phone2;
                    DbInstance.Email = model.Email;
                    //DbInstance.IsActive = model.IsActive;
                    DbInstance.PhysicalAddress = model.PhysicalAddress;
                    DbInstance.PostalAddress = model.PostalAddress;
                    DbInstance.WhiteListedDomain = model.WhiteListedDomain;
                    DbInstance.ProfileImageUrl = model.ProfileImageUrl;
                    //DbInstance.IsBackUpRecord = model.IsBackUpRecord;
                    //DbInstance.EntityDetailsValidated = model.EntityDetailsValidated;
                    DbInstance.IdentificationDocumentNumber = model.IdentificationDocumentNumber;
                    //DbInstance.IdentificationDocumentTypeId = model.IdentificationDocumentTypeId;
                    //DbInstance.AccountEntityActiveStateId = model.AccountEntityActiveStateId;
                }
                else
                {
                    var pIdInDb = context.Users.FirstOrDefault(
                        r => r.Username.Equals(DbInstance.Id)
                    );
                    DbInstance.EntityUserName = model.EntityUserName;
                    DbInstance.EntityName = model.EntityName;
                    DbInstance.Phone1 = model.Phone1;
                    DbInstance.IdentificationDocumentNumber = model.IdentificationDocumentNumber;
                    DbInstance.PhysicalAddress = model.PhysicalAddress;
                    DbInstance.Email = model.Email;
                    DbInstance.IsActive = model.IsActive;

                    //update user
                    
                    if (pIdInDb != null)
                    {
                        pIdInDb.Username = model.EntityUserName;
                        pIdInDb.PhoneNumber = model.Phone1;
                        pIdInDb.EmailAddress = model.Email;
                        pIdInDb.IsActive = model.IsActive;
                        context.Users.Update(pIdInDb);
                    }
                }

                #endregion
                #region update the subaccount

                //DbInstance.SubAccountBranch.Phone1 = model.SubAccountBranch.Phone1;
                //DbInstance.SubAccountBranch.Phone2 = model.Phone2;
                //DbInstance.SubAccountBranch.BuildingAddress = model.PhysicalAddress;
                //DbInstance.SubAccountBranch.PostalAddress = model.PostalAddress;
                //DbInstance.SubAccountBranch.BranchName = model.EntityName;
                //DbInstance.SubAccountBranch.BranchCode = model.EntityNo;

                //#endregion
                //#region update the account

                //DbInstance.SubAccountBranch.SubAccount.Phone = model.Phone1;
                //DbInstance.SubAccountBranch.SubAccount.AccountName = model.EntityName;
                //DbInstance.SubAccountBranch.SubAccount.AccountNo = model.EntityNo;
                //DbInstance.SubAccountBranch.SubAccount.DateCreated = model.RegDate;
                //DbInstance.SubAccountBranch.SubAccount.Email = model.Email;
                //DbInstance.SubAccountBranch.SubAccount.ContactPerson = model.EntityUserName;
                //DbInstance.SubAccountBranch.SubAccount.IsActive = model.IsActive;
                #endregion
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    //executionResult.Result = CloneModel(DbInstance);
                    //executionResult.Result = await GetModelForInsertUpdateFromId(DbInstance.Id, DbInstance, context);
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
        public async Task<ExecutionResult<IEnumerable<IdentificationDocumentType>>> GetDocumentType(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<IdentificationDocumentType>>(
                true,
                "",
                new HashSet<IdentificationDocumentType>()
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

                    var returnData = await context.IdentificationDocumentTypes
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new IdentificationDocumentType
                            {
                                Id = r.Id,
                                DocumentName = r.DocumentName
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

        #endregion
        #region fill combo boxes
        public async Task<ExecutionResult<IEnumerable<AccountEntityActiveState>>> GetEntityState(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<AccountEntityActiveState>>(
                true,
                "",
                new HashSet<AccountEntityActiveState>()
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

                    var returnData = await context.AccountEntityActiveStates
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new AccountEntityActiveState { Id = r.Id, StateName = r.StateName }
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
    }
}
