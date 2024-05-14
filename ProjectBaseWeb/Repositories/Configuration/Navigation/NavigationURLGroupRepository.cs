using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFDatabaseModel.Contexts;

namespace ProjectBaseWeb.Repositories.Configuration.Navigation
{
    public class NavigationURLGroupRepository : EFDatabaseModelBaseRepository<NavigationUrlGroup>
    {
        public NavigationURLGroupRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override NavigationUrlGroup CloneModel(NavigationUrlGroup model)
        {
            return new NavigationUrlGroup
            {
                Id = model.Id,
                GroupName = model.GroupName,
                Description = model.Description
            };
        }

        public async override Task<ExecutionResult<NavigationUrlGroup>> Add(
            NavigationUrlGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrlGroup>(true);
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

                context.NavigationUrlGroups.Add(modelInsert);

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

        public async override Task<ExecutionResult<NavigationUrlGroup>> Delete(
            NavigationUrlGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrlGroup>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                executionResult = await IsValidGeneral(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                executionResult = await GetItem(model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.NavigationUrlGroups.Remove(DbInstance);
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

        public async override Task<ExecutionResult<NavigationUrlGroup>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrlGroup>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.NavigationUrlGroups.FirstOrDefaultAsync(r => r.Id == id);
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
            >(true, "NavigationUrlGroup instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                var countries = await context.NavigationUrlGroups
                    .Where(r => true)
                    .Skip(args.skip)
                    .Take(args.take)
                    .OrderBy(r => r.Id)
                    .ToListAsync();

                var filterSupporter = new FilterSupporter<NavigationUrlGroup>(countries, args);
                var filteredData = filterSupporter.FilterData<int>(r => r.Id);
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

        public async override Task<ExecutionResult<NavigationUrlGroup>> IsValidGeneral(
            NavigationUrlGroup model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run
            return await Task.Run<ExecutionResult<NavigationUrlGroup>>(() =>
            {
                var result = new ExecutionResult<NavigationUrlGroup>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }

                if (string.IsNullOrEmpty(model.GroupName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the country code";
                    return result;
                }
                if (string.IsNullOrEmpty(model.Description))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the country icon";
                    return result;
                }

                return result;
            });
        }

        public async override Task<ExecutionResult<NavigationUrlGroup>> IsValidInsert(
            NavigationUrlGroup model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation
            var countryAlreadyRegistered = await context.NavigationUrlGroups
                .Where(r => r.GroupName == model.GroupName)
                .FirstOrDefaultAsync();
            if (countryAlreadyRegistered != null)
            {
                result.IsOkay = false;
                result.Message = $"The group name {model.GroupName} is already used";
                return result;
            }

            return result;
        }

        public async override Task<ExecutionResult<NavigationUrlGroup>> IsValidUpdate(
            NavigationUrlGroup model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            result = await GetItem(model.Id, context);
            if (result.IsOkay == false)
                return result;

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<NavigationUrlGroup>> Update(
            NavigationUrlGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrlGroup>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.GroupName = model.GroupName;
                DbInstance.Description = model.Description;

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
