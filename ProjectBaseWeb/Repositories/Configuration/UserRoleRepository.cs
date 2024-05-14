//using EFDatabaseModel.Contexts;
//using EFDatabaseModel.DbModel;
//using EFDatabaseModel.Repository;
//using Microsoft.EntityFrameworkCore;
//using ProjectBase.KendoUiSupport;
//using ProjectBase.Repository.Transaction;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProjectBaseWeb.Repositories.Configuration
//{
//    public class UserRoleRepository : EFDatabaseModelBaseRepository<UserRole>
//    {
//        public UserRoleRepository(string currentModule)
//            : base(currentModule) { }

//        /// <summary>
//        /// Helps to create a copy of the current Model
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        public override UserRole CloneModel(UserRole model)
//        {
//            return new UserRole
//            {
//                Id = model.Id,
//                UserId = model.UserId,
//                SystemRoleId = model.SystemRoleId,
//                Active = model.Active
//            };
//        }

//        public async override Task<ExecutionResult<UserRole>> Add(
//            UserRole model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<UserRole>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await IsValidInsert(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed executionn result

//                //insert the okay object to the DB
//                var modelInsert = CloneModel(model);

//                context.UserRoles.Add(modelInsert);

//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Create operation was successful";
//                    //return a new instance of the inserted object
//                    //executionResult.Result = CloneModel(modelInsert);
//                    executionResult.Result = await GetModelForInsertUpdateFromId(
//                        modelInsert.Id,
//                        modelInsert,
//                        context
//                    );
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

//        public async override Task<ExecutionResult<UserRole>> Delete(
//            UserRole model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<UserRole>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //use get item to check if this ID is valid and incase it is
//                //Get item will set the DbInstance using the current context if Succefully looaded object
//                executionResult = await IsValidGeneral(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;

//                executionResult = await GetItem((int)model.Id, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;

//                context.UserRoles.Remove(DbInstance);
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

//        public async override Task<ExecutionResult<UserRole>> GetItem(
//            int id,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<UserRole>(true, "Entry found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                DbInstance = await context.UserRoles.FirstOrDefaultAsync(r => r.Id == id);
//                if (DbInstance == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = $"The entry with id {id} supplied does not exist";
//                    return executionResult;
//                }

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

//        public async override Task<
//            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
//        > GetKendoGridFiltered(
//            NeedDataSourceEventArgs args,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<
//                ItemsRequestedResult<NeedDataSourceEventArgs>
//            >(true, "UserRole instance found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                //since this will be loaded from a detail table of SystemModule then ensure the Filter SystemModule Id Is Passed

//                string filterValue = null;
//                if (args.HasFilterFields("Username"))
//                    filterValue = args.GetFilterValuePredicate(r => r.field == "Username", 0);

//                string filterValueId = null;
//                if (args.HasFilterFields("Id"))
//                    filterValueId = args.GetFilterValuePredicate(r => r.field == "Id", 0);
//                int filterValueIntId = -1;
//                bool parsedInt = int.TryParse(filterValueId, out filterValueIntId);

//                var companyModules = (
//                    await context.UserRoles
//                        .Join(context.Users, ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u })
//                        .Join(
//                            context.SystemRoles,
//                            uru => uru.ur.SystemRoleId,
//                            sr => sr.Id,
//                            (uru, sr) => new { uru, sr }
//                        )
//                        .Where(
//                            r =>
//                                parsedInt
//                                    ? r.uru.ur.Id == filterValueIntId
//                                    : r.uru.u.Username == filterValue
//                        )
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .OrderBy(r => r.uru.ur.Id)
//                        .ToListAsync()
//                ).Select(
//                    r =>
//                        new UserRole
//                        {
//                            Id = r.uru.ur.Id,
//                            UserId = r.uru.ur.UserId,
//                            SystemRoleId = r.uru.ur.SystemRoleId,
//                            Active = r.uru.ur.Active,
//                            User = new User { Id = r.uru.u.Id },
//                            SystemRole = new SystemRole { Id = r.sr.Id, RoleName = r.sr.RoleName }
//                        }
//                );

//                var filterSupporter = new FilterSupporter<UserRole>(companyModules, args);
//                if (args.HasFilterFields("Username"))
//                    args.filter.filters.Remove(
//                        args.filter.filters.FirstOrDefault(r => r.field == "Username")
//                    );
//                var filteredData = filterSupporter.FilterData(r => r.Id);
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

//        public override Task<ExecutionResult<UserRole>> IsValidGeneral(
//            UserRole model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

//            var result = new ExecutionResult<UserRole>(true);
//            if (model == null)
//            {
//                result.IsOkay = false;
//                result.Message = "model is null";
//                return Task.FromResult(result);
//            }

//            return Task.FromResult(result);
//        }

//        public async override Task<ExecutionResult<UserRole>> IsValidInsert(
//            UserRole model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            var result = await IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            if (model.User == null)
//            {
//                result.IsOkay = false;
//                result.Message = "User is null";
//                return result;
//            }

//            if (string.IsNullOrEmpty(model.User.Username))
//            {
//                result.IsOkay = false;
//                result.Message = "User.Username is null";
//                return result;
//            }
//            var userInDb = await context.Users.FirstOrDefaultAsync(
//                r => r.Username == model.User.Username
//            );
//            if (userInDb == null)
//            {
//                result.IsOkay = false;
//                result.Message = $"Supplied user name {model.User.Username} does not exist";
//                return result;
//            }
//            model.UserId = userInDb.Id;
//            //perform insert specific validation


//            return result;
//        }

//        public async override Task<ExecutionResult<UserRole>> IsValidUpdate(
//            UserRole model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            //if it passes general validation then use get item to complete update validation
//            var result = await IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            result = await GetItem((int)model.Id, context);
//            if (result.IsOkay == false)
//                return result;

//            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
//            return result;
//        }

//        public async override Task<ExecutionResult<UserRole>> Update(
//            UserRole model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<UserRole>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await IsValidUpdate(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed executionn result

//                //dbInstance is updated in GETITEM in IsValidUpdate
//                DbInstance.SystemRoleId = model.SystemRoleId;
//                DbInstance.UserId = model.UserId;
//                DbInstance.Active = model.Active;

//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Update operation was successful";
//                    //executionResult.Result = CloneModel(DbInstance);
//                    executionResult.Result = await GetModelForInsertUpdateFromId(
//                        DbInstance.Id,
//                        DbInstance,
//                        context
//                    );
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

//        #region fill combo boxes
//        public async Task<ExecutionResult<IEnumerable<SystemRole>>> GetSystemRole(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<SystemRole>>(
//                true,
//                "",
//                new HashSet<SystemRole>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    var filterValue = args.GetFilterValue(0).ToLower();
//                    ;
//                    //if (string.IsNullOrEmpty(filterValue))
//                    //    return result;

//                    var returnData = await context.SystemRoles
//                        .Where(
//                            r =>
//                                filterValue == null
//                                    ? true
//                                    : r.RoleName.ToLower().Contains(filterValue)
//                        )
//                        .OrderBy(r => r.Id)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r => new SystemRole { Id = r.Id, RoleName = r.RoleName }
//                    );
//                }
//            }
//            catch (Exception ex)
//            {
//                result = result.CopyFromException(ex);
//            }
//            return result;
//        }

//        #endregion
//    }
//}
