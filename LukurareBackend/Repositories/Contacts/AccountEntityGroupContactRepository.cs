using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using Microsoft.EntityFrameworkCore;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Contacts
{
    public class AccountEntityGroupContactRepository
        : EFDatabaseModelBaseRepository<AccountEntityGroupContact>
    {
        public AccountEntityGroupContactRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override AccountEntityGroupContact CloneModel(AccountEntityGroupContact model)
        {
            var returnValue = new AccountEntityGroupContact
            {
                Id = model.Id,
                AccountEntityGroupId = model.AccountEntityGroupId,
                AccountEntityContactId = model.AccountEntityContactId
            };

            return returnValue;
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> Add(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //insert the okay object to the DB
                var modelInsert = CloneModel(model);

                context.AccountEntityGroupContacts.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //return a new instance of the inserted object
                    // executionResult.Result = CloneModel(modelInsert);
                    // executionResult.Result = await GetModelForInsertUpdateFromId(modelInsert.Id, modelInsert, context);
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

        public async override Task<ExecutionResult<AccountEntityGroupContact>> Delete(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);
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

                context.AccountEntityGroupContacts.Remove(DbInstance);
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

        public async override Task<ExecutionResult<AccountEntityGroupContact>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.AccountEntityGroupContacts.FirstOrDefaultAsync(
                    r => r.Id == id
                );
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
            >(true, "AccountEntityGroupContact instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("AccountEntityGroupId"))
                    filterValue = args.GetFilterValuePredicate(
                        r => r.field == "AccountEntityGroupId",
                        0
                    );

                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) == false && parsedInt == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = string.Format(
                        "Sorry Account Entity Group Id filter value has to be provided that is a valid integer",
                        new object[0]
                    );
                    return executionResult;
                }
                var senders = (
                    await context.AccountEntityGroupContacts
                        .Join(
                            context.AccountEntityGroups,
                            gc => gc.AccountEntityGroupId,
                            g => g.Id,
                            (gc, g) => new { gc, g }
                        )
                        .Join(
                            context.AccountEntityContacts,
                            x => x.gc.AccountEntityContactId,
                            c => c.Id,
                            (x, c) => new { x, c }
                        )
                        .Where(r => r.x.g.AccountEntityId == UserPrinciple.User.AccountEntityId)
                        //.Where(r => parsedInt ? r.x.gc.AccountEntityGroupId == filterValueInt : true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.x.gc.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntityGroupContact
                        {
                            Id = r.x.gc.Id,
                            AccountEntityGroupId = r.x.gc.AccountEntityGroupId,
                            AccountEntityGroup = new AccountEntityGroup { Id = r.x.g.Id },
                            AccountEntityContact = new AccountEntityContact
                            {
                                Id = r.c.Id,
                                ContactName = r.c.ContactName,
                                ContactPhone = r.c.ContactPhone,
                                ContactEmail = r.c.ContactEmail
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<AccountEntityGroupContact>(senders, args);
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

        public override Task<ExecutionResult<AccountEntityGroupContact>> IsValidGeneral(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<AccountEntityGroupContact>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> IsValidInsert(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation
            if (this.UserPrinciple == null)
            {
                result.IsOkay = false;
                result.Message = "UserPrinciple is null";
                return result;
            }
            if (this.UserPrinciple.User == null)
            {
                result.IsOkay = false;
                result.Message = "UserPrinciple.User is null";
                return result;
            }

            return result;
        }

        public async override Task<ExecutionResult<AccountEntityGroupContact>> IsValidUpdate(
            AccountEntityGroupContact model,
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

        public async override Task<ExecutionResult<AccountEntityGroupContact>> Update(
            AccountEntityGroupContact model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroupContact>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate


                DbInstance.AccountEntityGroupId = model.AccountEntityGroupId;
                DbInstance.AccountEntityContactId = model.AccountEntityContactId;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        DbInstance.Id,
                        DbInstance,
                        context
                    );
                    //executionResult.Result = CloneModel(DbInstance);
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
        public async Task<ExecutionResult<IEnumerable<AccountEntityContact>>> GetContact(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<AccountEntityContact>>(
                true,
                "",
                new HashSet<AccountEntityContact>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    var filterValue = args.GetFilterValue(0).ToLower();
                    ;
                    //if (string.IsNullOrEmpty(filterValue))
                    //    return result;

                    var returnData = await context.AccountEntityContacts
                        .Where(
                            r =>
                                filterValue == null
                                    ? true
                                    : r.ContactName.ToLower().Contains(filterValue)
                        )
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new AccountEntityContact
                            {
                                Id = r.Id,
                                ContactName = r.ContactName,
                                ContactPhone = r.ContactPhone,
                                ContactEmail = r.ContactEmail
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
    }
}
