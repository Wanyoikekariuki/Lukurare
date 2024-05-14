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

namespace ProjectBaseWeb.Repositories.Configuration
{
    public class CompanyModuleRepository : EFDatabaseModelBaseRepository<CompanyModule>
    {
        private SystemModule SystemModuleSelected;

        public CompanyModuleRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override CompanyModule CloneModel(CompanyModule model)
        {
            return new CompanyModule
            {
                Id = model.Id,
                SystemModuleId = model.SystemModuleId,
                SubAccountId = model.SubAccountId,
                OneTransactionStagePerUser = model.OneTransactionStagePerUser
            };
        }

        public async override Task<ExecutionResult<CompanyModule>> Add(
            CompanyModule model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CompanyModule>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //insert the okay object to the DB
                var modelInsert = CloneModel(model);

                context.CompanyModules.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //return a new instance of the inserted object
                    //executionResult.Result = CloneModel(modelInsert);
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        modelInsert.Id,
                        modelInsert,
                        context
                    );
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

        public async override Task<ExecutionResult<CompanyModule>> Delete(
            CompanyModule model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CompanyModule>(true);
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

                context.CompanyModules.Remove(DbInstance);
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

        public async override Task<ExecutionResult<CompanyModule>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CompanyModule>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.CompanyModules.FirstOrDefaultAsync(r => r.Id == id);
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
            >(true, "CompanyModule instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SystemModule then ensure the Filter SystemModule Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("SystemModuleId"))
                    filterValue = args.GetFilterValuePredicate(r => r.field == "SystemModuleId", 0);

                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) == false && parsedInt == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = string.Format(
                        "Sorry SystemModuleId filter value has to be provided that is a valid integer",
                        new object[0]
                    );
                    return executionResult;
                }
                var companyModules = (
                    await context.CompanyModules
                        .Join(
                            context.SubAccounts,
                            cm => cm.SubAccountId,
                            sa => sa.Id,
                            (cm, sa) => new { cm, sa }
                        )
                        .Join(
                            context.SystemModules,
                            cmsm => cmsm.cm.SystemModuleId,
                            sm => sm.Id,
                            (cmsm, sm) => new { cmsm, sm }
                        )
                        .Where(r => parsedInt ? r.cmsm.cm.SystemModuleId == filterValueInt : true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.cmsm.cm.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new CompanyModule
                        {
                            Id = r.cmsm.cm.Id,
                            SystemModuleId = r.cmsm.cm.SystemModuleId,
                            SubAccountId = r.cmsm.cm.SubAccountId,
                            OneTransactionStagePerUser = r.cmsm.cm.OneTransactionStagePerUser,
                            SystemModule = new SystemModule { Id = r.sm.Id },
                            SubAccount = new SubAccount
                            {
                                Id = r.cmsm.sa.Id,
                                AccountName = r.cmsm.sa.AccountName,
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<CompanyModule>(companyModules, args);
                var filteredData = filterSupporter.FilterData(r => r.Id);
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

        public async override Task<ExecutionResult<CompanyModule>> IsValidGeneral(
            CompanyModule model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<CompanyModule>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return result;
            }

            //enure the system module id supplied is valid
            SystemModuleSelected = await context.SystemModules.FirstOrDefaultAsync(
                r => r.Id == model.SystemModuleId
            );
            if (SystemModuleSelected == null)
            {
                result.IsOkay = false;
                result.Message = "Please  supply the System Module ";
                return result;
            }

            return result;
        }

        public async override Task<ExecutionResult<CompanyModule>> IsValidInsert(
            CompanyModule model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation


            return result;
        }

        public async override Task<ExecutionResult<CompanyModule>> IsValidUpdate(
            CompanyModule model,
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

        public async override Task<ExecutionResult<CompanyModule>> Update(
            CompanyModule model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CompanyModule>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.SystemModuleId = model.SystemModuleId;
                DbInstance.SubAccountId = model.SubAccountId;
                DbInstance.OneTransactionStagePerUser = model.OneTransactionStagePerUser;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    //executionResult.Result = CloneModel(DbInstance);
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
        public async Task<ExecutionResult<IEnumerable<SubAccount>>> GetSubAccount(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SubAccount>>(
                true,
                "",
                new HashSet<SubAccount>()
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

                    var returnData = await context.SubAccounts
                        .Where(r => r.AccountName.ToLower().Contains(filterValue))
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SubAccount { Id = r.Id, AccountName = r.AccountName }
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
