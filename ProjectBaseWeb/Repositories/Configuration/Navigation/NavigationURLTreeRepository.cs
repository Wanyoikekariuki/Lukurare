//using EFDatabaseModel.DbModel;
//using EFDatabaseModel.Repository;
//using ProjectBase.KendoUiSupport;
//using ProjectBase.Repository.Transaction;
//using System;
//using System.Threading.Tasks;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using EFDatabaseModel.Contexts;

//namespace ProjectBaseWeb.Repositories.Configuration.Navigation
//{
//    public class NavigationURLTreeRepository : EFDatabaseModelBaseRepository<NavigationUrlTree>
//    {
//        public NavigationURLTreeRepository(string currentModule)
//            : base(currentModule)
//        {

//        }

//        /// <summary>
//        /// Helps to create a copy of the current Model
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        public override NavigationUrlTree CloneModel(NavigationUrlTree model)
//        {
//            var result = new NavigationUrlTree
//            {
//                Id = model.Id,
//                NavigationUrlChildId = model.NavigationUrlChildId,
//                NavigationUrlParentId = model.NavigationUrlParentId,
//            };
//            if (model.NavigationUrlChild != null)
//                result.NavigationUrlChild = new NavigationURLRepository(CurrentModule).CloneModel(model.NavigationUrlChild);

//            if (model.NavigationUrlParent != null)
//                result.NavigationUrlParent = new NavigationURLRepository(CurrentModule).CloneModel(model.NavigationUrlParent);

//            return result;
//        }
//        public async override Task<ExecutionResult<NavigationUrlTree>> Add(NavigationUrlTree model, EFDatabaseModelDatabaseContext context = null)
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<NavigationUrlTree>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidInsert(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;//if fail return the failed executionn result

//                //insert the okay object to the DB
//                var modelInsert = CloneModel(model);
//                modelInsert.NavigationUrlChild.NavigationUrlGroup = null;
//                if (model.NavigationUrlChildId != model.NavigationUrlParentId)
//                {
//                    modelInsert.NavigationUrlParent = null;
//                    modelInsert.NavigationUrlParentId = model.NavigationUrlParentId;
//                }
//                else
//                {
//                    //creating root project that references it self
//                    modelInsert.NavigationUrlChild.NavigationUrlGroupId = model.NavigationUrlChild.NavigationUrlGroupId;
//                    modelInsert.NavigationUrlParent = modelInsert.NavigationUrlChild;
//                }

//                //model.NavigationUrlChild.RootParentNode = false;
//                context.NavigationUrlTrees.Add(modelInsert);

//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Create operation was successful";
//                    //return a new instance of the inserted object
//                    executionResult.Result = CloneModel(modelInsert);
//                }
//                else
//                {
//                    //incase the context was not created here return the object
//                    //as inserted to the context
//                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
//                    executionResult.Result = modelInsert;
//                }
//            }
//            catch (Exception ex)
//            {
//                //if an error occured copy its data from the exception object into the execution result by
//                executionResult.CopyFromException(ex);
//            }
//            finally
//            {
//                //incase the context was created in this method then dispose it here
//                if (createContextHere)
//                    await context.DisposeAsync();
//            }
//            return executionResult;
//        }

//        public async override Task<ExecutionResult<NavigationUrlTree>> Delete(NavigationUrlTree model, EFDatabaseModelDatabaseContext context = null)
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<NavigationUrlTree>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //use get item to check if this ID is valid and incase it is
//                //Get item will set the DbInstance using the current context if Succefully looaded object
//                executionResult = await IsValidGeneral(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;

//                executionResult = await GetItem(model.Id, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;

//                context.NavigationUrlTrees.Remove(DbInstance);
//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Delete operation was successful";
//                    executionResult.Result = CloneModel(DbInstance);
//                }
//                else
//                {
//                    //incase the context was not created here return the object
//                    //thats removed from the context
//                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
//                    executionResult.Result = DbInstance;
//                }
//            }
//            catch (Exception ex)
//            {
//                //if an error occured copy its data from the exception object into the execution result by
//                executionResult.CopyFromException(ex);
//            }
//            finally
//            {
//                //incase the context was created in this method then dispose it here
//                if (createContextHere)
//                    await context.DisposeAsync();
//            }
//            return executionResult;
//        }


//        public async override Task<ExecutionResult<NavigationUrlTree>> GetItem(int id, EFDatabaseModelDatabaseContext context = null)
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<NavigationUrlTree>(true, "Entry found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                var genericInstance = await (context.NavigationUrlTrees
//                    .Join(context.NavigationUrls, nut => nut.NavigationUrlChildId, nuc => nuc.Id, (nut, nuc) => new { nuc, nut })
//                    .FirstOrDefaultAsync(r => r.nut.Id == id));
//                if (genericInstance == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = $"The entry with id {id} supplied does not exist";
//                    return executionResult;
//                }
//                DbInstance = genericInstance.nut;
//                DbInstance.NavigationUrlChild = genericInstance.nuc;
//                //set the Result of executionResult object to the item found in the database
//                //to avoid issues with serialization always use new
//                executionResult.Result = CloneModel(DbInstance);

//            }
//            catch (Exception ex)
//            {
//                //if an error occured copy its data from the exception object into the execution result by
//                executionResult.CopyFromException(ex);
//            }
//            finally
//            {
//                //incase the context was created in this method then dispose it here
//                if (createContextHere)
//                    await context.DisposeAsync();
//            }
//            return executionResult;
//        }

//        /// <summary>
//        /// Return the tree view for the root parents
//        /// </summary>
//        /// <param name="args"></param>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        public async override Task<ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>> GetKendoGridFiltered(NeedDataSourceEventArgs args, EFDatabaseModelDatabaseContext context = null)
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>(true, "NavigationUrlTree instance found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

//                var dataListRaw = await (context.NavigationUrlTrees
//                    .Join(context.NavigationUrls, nut => nut.NavigationUrlChildId, nuc => nuc.Id, (nut, nuc) => new { nut, nuc })
//                    .Join(context.NavigationUrls, nutnuc => nutnuc.nut.NavigationUrlParentId, nup => nup.Id, (nutnuc, nup) => new { nutnuc, nup })
//                    .Join(context.NavigationUrlGroups, nutnucnup => nutnucnup.nutnuc.nuc.NavigationUrlGroupId, nugc => nugc.Id, (nutnucnup, nugc) => new { nutnucnup, nugc })
//                    .Join(context.NavigationUrlGroups, nutnucnupnugc => nutnucnupnugc.nutnucnup.nup.NavigationUrlGroupId, nugp => nugp.Id, (nutnucnupnugc, nugp) => new { nutnucnupnugc, nugp })
//                    .Where(r => true)
//                    .Skip(args.skip)
//                    .Take(args.take)
//                    .OrderBy(r => r.nutnucnupnugc.nutnucnup.nutnuc.nut.Id)
//                    .ToListAsync());

//                var dataListReturn = dataListRaw.Select(r => new NavigationUrlTree
//                {
//                    Id = r.nutnucnupnugc.nutnucnup.nutnuc.nut.Id,
//                    NavigationUrlChildId = r.nutnucnupnugc.nutnucnup.nutnuc.nut.NavigationUrlChildId,
//                    NavigationUrlChild = new NavigationUrl
//                    {
//                        Id = r.nutnucnupnugc.nutnucnup.nutnuc.nuc.Id,
//                        NavigationUrlGroup = new NavigationUrlGroup
//                        {
//                            Id = r.nutnucnupnugc.nugc.Id,
//                            GroupName = r.nutnucnupnugc.nugc.GroupName,
//                        },
//                        NavigationUrlGroupId = r.nutnucnupnugc.nutnucnup.nutnuc.nuc.NavigationUrlGroupId,
//                        RelativeUrl = r.nutnucnupnugc.nutnucnup.nutnuc.nuc.RelativeUrl,
//                        StyleClass = r.nutnucnupnugc.nutnucnup.nutnuc.nuc.StyleClass,
//                        UrlName = r.nutnucnupnugc.nutnucnup.nutnuc.nuc.UrlName,
//                    },
//                    NavigationUrlParentId = r.nutnucnupnugc.nutnucnup.nutnuc.nut.NavigationUrlParentId,
//                    NavigationUrlParent = new NavigationUrl
//                    {
//                        Id = r.nutnucnupnugc.nutnucnup.nup.Id,
//                        NavigationUrlGroup = new NavigationUrlGroup
//                        {
//                            Id = r.nugp.Id,
//                            GroupName = r.nugp.GroupName,
//                        },
//                        NavigationUrlGroupId = r.nutnucnupnugc.nutnucnup.nup.NavigationUrlGroupId,
//                        RelativeUrl = r.nutnucnupnugc.nutnucnup.nup.RelativeUrl,
//                        StyleClass = r.nutnucnupnugc.nutnucnup.nup.StyleClass,
//                        UrlName = r.nutnucnupnugc.nutnucnup.nup.UrlName,
//                    },
//                });


//                var filterSupporter = new FilterSupporter<NavigationUrlTree>(dataListReturn, args);
//                var filteredData = filterSupporter.FilterData<int>(r => r.Id);
//                executionResult.Result = filterSupporter.itemsResult;

//            }
//            catch (Exception ex)
//            {
//                //if an error occured copy its data from the exception object into the execution result by
//                executionResult.CopyFromException(ex);
//            }
//            finally
//            {
//                //incase the context was created in this method then dispose it here
//                if (createContextHere)
//                    await context.DisposeAsync();
//            }
//            return executionResult;
//        }

//        public async override Task<ExecutionResult<NavigationUrlTree>> IsValidGeneral(NavigationUrlTree model, EFDatabaseModelDatabaseContext context)
//        {
//            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run


//            var result = new ExecutionResult<NavigationUrlTree>(true);
//            if (model == null)
//            {
//                result.IsOkay = false;
//                result.Message = "model is null";
//                return result;
//            }

//            //ensure that the group is set


//            if (model.NavigationUrlChild == null)
//            {
//                result.IsOkay = false;
//                result.Message = "Child not data has not been supplied";
//                return result;
//            }

//            var validNavigationUrlGeneral = await new NavigationURLRepository(CurrentModule)
//                .IsValidGeneral(model.NavigationUrlChild, context);
//            if (validNavigationUrlGeneral.IsOkay == false)
//            {
//                result.IsOkay = false;
//                result.Message = $"Child Navigation URL Validation Failed. Message {validNavigationUrlGeneral.Message} ";
//                return result;
//            }

//            return result;

//        }

//        public async override Task<ExecutionResult<NavigationUrlTree>> IsValidInsert(NavigationUrlTree model, EFDatabaseModelDatabaseContext context)
//        {
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            //incase parent is not equal to child then this a child and parent must exist
//            if (model.NavigationUrlChildId != model.NavigationUrlParentId)
//            {
//                var parentNavGroup = await context.NavigationUrls.FirstOrDefaultAsync(r => r.Id == model.NavigationUrlParentId);
//                if (parentNavGroup == null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "Please parent node has not been supplied";
//                    return result;
//                }
//            }
//            //perform insert specific validation
//            //var countryAlreadyRegistered = await context.NavigationUrlTrees
//            //    .Where(r => r.NavigationUrlChildId == model.NavigationUrlChildId)
//            //    .FirstOrDefaultAsync();
//            //if (countryAlreadyRegistered != null)
//            //{
//            //    result.IsOkay = false;
//            //    result.Message = $"The group name {model.NavigationUrlChildId} is already used";
//            //    return result;
//            //}



//            return result;
//        }

//        public async override Task<ExecutionResult<NavigationUrlTree>> IsValidUpdate(NavigationUrlTree model, EFDatabaseModelDatabaseContext context)
//        {
//            //if it passes general validation then use get item to complete update validation
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            result = await GetItem(model.Id, context);
//            if (result.IsOkay == false)
//                return result;

//            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
//            return result;
//        }

//        public async override Task<ExecutionResult<NavigationUrlTree>> Update(NavigationUrlTree model, EFDatabaseModelDatabaseContext context = null)
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<NavigationUrlTree>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidUpdate(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;//if fail return the failed executionn result

//                //dbInstance is updated in GETITEM in IsValidUpdate
//                //DbInstance.NavigationUrlChildId = model.NavigationUrlChildId;
//                //DbInstance.NavigationUrlParentId = model.NavigationUrlParentId;
//                DbInstance.NavigationUrlChild.RelativeUrl = model.NavigationUrlChild.RelativeUrl;
//                DbInstance.NavigationUrlChild.StyleClass = model.NavigationUrlChild.StyleClass;
//                DbInstance.NavigationUrlChild.NavigationUrlGroupId = model.NavigationUrlChild.NavigationUrlGroupId;
//                DbInstance.NavigationUrlChild.UrlName = model.NavigationUrlChild.UrlName;


//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Update operation was successful";
//                    executionResult.Result = CloneModel(DbInstance);
//                }
//                else
//                {
//                    //incase the context was not created here return the object
//                    //thats removed from the context
//                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
//                    executionResult.Result = DbInstance;
//                }
//            }
//            catch (Exception ex)
//            {
//                //if an error occured copy its data from the exception object into the execution result by
//                executionResult.CopyFromException(ex);
//            }
//            finally
//            {
//                //incase the context was created in this method then dispose it here
//                if (createContextHere)
//                    await context.DisposeAsync();
//            }
//            return executionResult;
//        }
//    }
//}
