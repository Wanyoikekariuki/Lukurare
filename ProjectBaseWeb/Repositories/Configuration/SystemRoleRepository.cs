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
    public class SystemRoleRepository : EFDatabaseModelBaseRepository<SystemRole>
    {
        public SystemRoleRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override SystemRole CloneModel(SystemRole model)
        {
            return new SystemRole { Id = model.Id, RoleName = model.RoleName };
        }

        public async override Task<ExecutionResult<SystemRole>> Add(
            SystemRole model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SystemRole>(true);
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

                context.SystemRoles.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //return a new instance of the inserted object
                    executionResult.Result = CloneModel(modelInsert);
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

        public async override Task<ExecutionResult<SystemRole>> Delete(
            SystemRole model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SystemRole>(true);
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

                context.SystemRoles.Remove(DbInstance);
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

        public async override Task<ExecutionResult<SystemRole>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SystemRole>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SystemRoles.FirstOrDefaultAsync(r => r.Id == id);
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
            >(true, "SystemRole instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of SystemModule then ensure the Filter SystemModule Id Is Passed

                var roles = await context.SystemRoles
                    .Where(r => true)
                    .Skip(args.skip)
                    .Take(args.take)
                    .OrderBy(r => r.Id)
                    .ToListAsync();

                var filterSupporter = new FilterSupporter<SystemRole>(roles, args);
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

        public async override Task<ExecutionResult<SystemRole>> IsValidGeneral(
            SystemRole model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<SystemRole>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return result;
            }
            if (model.RoleName == null)
            {
                result.IsOkay = false;
                result.Message = "Supply the role name";
                return result;
            }

            return result;
        }

        public async override Task<ExecutionResult<SystemRole>> IsValidInsert(
            SystemRole model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation


            return result;
        }

        public async override Task<ExecutionResult<SystemRole>> IsValidUpdate(
            SystemRole model,
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

        public async override Task<ExecutionResult<SystemRole>> Update(
            SystemRole model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SystemRole>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate

                DbInstance.RoleName = model.RoleName;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
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
    }
}
