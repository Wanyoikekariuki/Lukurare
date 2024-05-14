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

namespace LukurareBackend.Repositories.Sms
{
    public class AccountEntityGroupRepository : EFDatabaseModelBaseRepository<AccountEntityGroup>
    {
        public AccountEntityGroupRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override AccountEntityGroup CloneModel(AccountEntityGroup model)
        {
            var returnValue = new AccountEntityGroup
            {
                Id = model.Id,
                GroupName = model.GroupName,
                AccountEntityId = UserPrinciple.User.AccountEntityId
            };

            return returnValue;
        }

        public async override Task<ExecutionResult<AccountEntityGroup>> Add(
            AccountEntityGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroup>(true);
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

                context.AccountEntityGroups.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //return a new instance of the inserted object
                    // executionResult.Result = CloneModel(modelInsert);
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

        public async override Task<ExecutionResult<AccountEntityGroup>> Delete(
            AccountEntityGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroup>(true);
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

                var groupcontacts = context.AccountEntityGroupContacts
                    .Where(r => r.AccountEntityGroupId == model.Id)
                    .ToListAsync();
                if (groupcontacts != null)
                {
                    foreach (var groupcontact in await groupcontacts) // query executed and data obtained from database
                    {
                        context.AccountEntityGroupContacts.Remove(groupcontact);
                    }
                }
                context.AccountEntityGroups.Remove(DbInstance);
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

        public async override Task<ExecutionResult<AccountEntityGroup>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroup>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.AccountEntityGroups.FirstOrDefaultAsync(r => r.Id == id);
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
            >(true, "AccountEntityGroup instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("AccountEntityId"))
                    filterValue = args.GetFilterValuePredicate(
                        r => r.field == "AccountEntityId",
                        0
                    );

                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) == false && parsedInt == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = string.Format(
                        "Sorry Account Entity Id filter value has to be provided that is a valid integer",
                        new object[0]
                    );
                    return executionResult;
                }
                var senders = (
                    await context.AccountEntityGroups
                        .Join(
                            context.AccountEntities,
                            aeg => aeg.AccountEntityId,
                            ae => ae.Id,
                            (aeg, ae) => new { aeg, ae }
                        )
                        .Where(r => r.aeg.AccountEntityId == UserPrinciple.User.AccountEntityId)
                        //.Where(r => parsedInt ? r.aeg.AccountEntityId == filterValueInt : true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.aeg.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntityGroup
                        {
                            Id = r.aeg.Id,
                            GroupName = r.aeg.GroupName,
                            AccountEntity = new AccountEntity { Id = r.ae.Id }
                        }
                );

                var filterSupporter = new FilterSupporter<AccountEntityGroup>(senders, args);
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

        public override Task<ExecutionResult<AccountEntityGroup>> IsValidGeneral(
            AccountEntityGroup model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<AccountEntityGroup>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<AccountEntityGroup>> IsValidInsert(
            AccountEntityGroup model,
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
            model.AccountEntityId = UserPrinciple.User.AccountEntityId;

            return result;
        }

        public async override Task<ExecutionResult<AccountEntityGroup>> IsValidUpdate(
            AccountEntityGroup model,
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

        public async override Task<ExecutionResult<AccountEntityGroup>> Update(
            AccountEntityGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntityGroup>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate



                DbInstance.GroupName = model.GroupName;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    //executionResult.Result = await GetModelForInsertUpdateFromId(DbInstance.Id, DbInstance, context);
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
    }
}
