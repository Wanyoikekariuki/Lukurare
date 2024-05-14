using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFDatabaseModel.Contexts;
using System.Collections.Generic;

namespace ProjectBaseWeb.Repositories.Configuration.Navigation
{
    public class NavigationTreeRepository : EFDatabaseModelBaseRepository<NavigationUrl>
    {
        public NavigationTreeRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override NavigationUrl CloneModel(NavigationUrl model)
        {
            var result = new NavigationUrl
            {
                Id = model.Id,
                RelativeUrl = model.RelativeUrl,
                StyleClass = model.StyleClass,
                UrlName = model.UrlName,
                NavigationUrlGroupId = model.NavigationUrlGroupId,
                NavigationUrlParentId = model.NavigationUrlParentId,
            };
            if (model.NavigationUrlGroup != null)
                result.NavigationUrlGroup = new NavigationURLGroupRepository(
                    CurrentModule
                ).CloneModel(model.NavigationUrlGroup);
            return result;
        }

        public async override Task<ExecutionResult<NavigationUrl>> Add(
            NavigationUrl model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrl>(true);
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
                modelInsert.NavigationUrlGroup = null;

                context.NavigationUrls.Add(modelInsert);

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

        public async override Task<ExecutionResult<NavigationUrl>> Delete(
            NavigationUrl model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrl>(true);
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

                context.NavigationUrls.Remove(DbInstance);
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

        public async override Task<ExecutionResult<NavigationUrl>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrl>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.NavigationUrls.FirstOrDefaultAsync(r => r.Id == id);
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
            >(true, "NavigationUrl instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                var countriesQr = (
                    context.NavigationUrls
                        .Join(
                            context.NavigationUrlGroups,
                            nuc => nuc.NavigationUrlGroupId,
                            nug => nug.Id,
                            (nuc, nug) => new { nuc, nug }
                        )
                        .Where(r => true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.nuc.Id)
                );

                var countries = (await countriesQr.ToListAsync()).Select(r =>
                {
                    var navigationUrl = new NavigationUrl
                    {
                        Id = r.nuc.Id,
                        NavigationUrlGroupId = r.nuc.NavigationUrlGroupId,
                        NavigationUrlParentId = r.nuc.NavigationUrlParentId,
                        RelativeUrl = r.nuc.RelativeUrl,
                        RootParentNode = r.nuc.NavigationUrlParentId.HasValue,
                        StyleClass = r.nuc.StyleClass,
                        UrlName = r.nuc.UrlName,
                        NavigationUrlGroup = new NavigationUrlGroup
                        {
                            Id = r.nug.Id,
                            GroupName = r.nug.GroupName,
                            Description = r.nug.Description,
                        },
                    };

                    return navigationUrl;
                });

                var filterSupporter = new FilterSupporter<NavigationUrl>(countries, args);
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

        public async override Task<ExecutionResult<NavigationUrl>> IsValidGeneral(
            NavigationUrl model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run
            var result = new ExecutionResult<NavigationUrl>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return result;
            }

            if (string.IsNullOrEmpty(model.RelativeUrl))
            {
                result.IsOkay = false;
                result.Message = "Please supply the relative url";
                return result;
            }

            if (string.IsNullOrEmpty(model.UrlName))
            {
                result.IsOkay = false;
                result.Message = "Please supply the url name";
                return result;
            }
            //ensure that the group is set
            var navGroup = await context.NavigationUrlGroups.FirstOrDefaultAsync(
                r => r.Id == model.NavigationUrlGroupId
            );
            if (navGroup == null)
            {
                result.IsOkay = false;
                result.Message = "Please supply/select the Navigation URL group";
                return result;
            }
            return result;
        }

        public async override Task<ExecutionResult<NavigationUrl>> IsValidInsert(
            NavigationUrl model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public async override Task<ExecutionResult<NavigationUrl>> IsValidUpdate(
            NavigationUrl model,
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

        public async override Task<ExecutionResult<NavigationUrl>> Update(
            NavigationUrl model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NavigationUrl>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.RelativeUrl = model.RelativeUrl;
                DbInstance.StyleClass = model.StyleClass;
                DbInstance.NavigationUrlGroupId = model.NavigationUrlGroupId;
                DbInstance.UrlName = model.UrlName;

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

        #region fill combo boxes
        public async Task<
            ExecutionResult<IEnumerable<NavigationUrlGroup>>
        > GetNavigationURLGroupData(NeedDataSourceEventArgs args)
        {
            var result = new ExecutionResult<IEnumerable<NavigationUrlGroup>>(true);
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    var filterValue = args.GetFilterValue(0);
                    var returnData = await context.NavigationUrlGroups
                        .Where(
                            r =>
                                string.IsNullOrEmpty(filterValue)
                                    ? true
                                    : r.GroupName.Contains(filterValue)
                        )
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new NavigationUrlGroup
                            {
                                Id = r.Id,
                                GroupName = r.GroupName,
                                Description = r.Description,
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
