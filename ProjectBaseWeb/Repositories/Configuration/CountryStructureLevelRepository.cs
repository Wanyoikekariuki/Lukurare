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
    public class CountryStructureLevelRepository
        : EFDatabaseModelBaseRepository<CountryStructureLevel>
    {
        public CountryStructureLevelRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<CountryStructureLevel>> Add(
            CountryStructureLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructureLevel>(true);
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

                context.CountryStructureLevels.Add(modelInsert);

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

        public override CountryStructureLevel CloneModel(CountryStructureLevel model)
        {
            return new CountryStructureLevel
            {
                Id = model.Id,
                Level = model.Level,
                LevelName = model.LevelName,
                CountryId = model.CountryId
            };
        }

        public override async Task<ExecutionResult<CountryStructureLevel>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructureLevel>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.CountryStructureLevels.FirstOrDefaultAsync(
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

        public override async Task<ExecutionResult<CountryStructureLevel>> Update(
            CountryStructureLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructureLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.CountryId = model.CountryId;
                DbInstance.Level = model.Level;
                DbInstance.LevelName = model.LevelName;

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

        public override async Task<ExecutionResult<CountryStructureLevel>> Delete(
            CountryStructureLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructureLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var classToDelete = context.CountryStructureLevels.FirstOrDefault(
                    r => r.Id == model.Id
                );

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The class you are trying to delete does not exist";
                    return executionResult;
                }

                executionResult = await GetItem((int)model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.CountryStructureLevels.Remove(DbInstance);
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

        public override async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "Class instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                //since this will be loaded from a detail table of Country then ensure the Filter Country Id Is Passed
                int filterValueInt = -1;
                string filterValue = null;
                if (args.HasFilterFields("CountryId"))
                    filterValue = args.GetFilterValuePredicate(r => r.field == "CountryId", 0);

                bool parsedInt = int.TryParse(filterValue, out filterValueInt);
                if (string.IsNullOrEmpty(filterValue) || parsedInt == false)
                {
                    filterValueInt = -1;
                    //executionResult.IsOkay = false;
                    //executionResult.Message = string.Format("Sorry CountryId filter value has to be provided that is a valid integer", new object[0]);
                    //return executionResult;
                }

                var Levels = (
                    await context.CountryStructureLevels
                        .Join(
                            context.Countries,
                            csl => csl.CountryId,
                            c => c.Id,
                            (csl, c) => new { csl, c }
                        )
                        .Where(r => filterValueInt == -1 ? true : r.csl.CountryId == filterValueInt)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.csl.Level)
                        .ToListAsync()
                ).Select(
                    r =>
                        new CountryStructureLevel
                        {
                            Id = r.csl.Id,
                            Level = r.csl.Level,
                            LevelName = r.csl.LevelName,
                            CountryId = r.csl.CountryId,
                            Country = new Country { Id = r.c.Id, CountryName = r.c.CountryName }
                        }
                );

                var filterSupporter = new FilterSupporter<CountryStructureLevel>(Levels, args);
                var filteredData = filterSupporter.FilterData<long>(r => r.Id);
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

        public override async Task<ExecutionResult<CountryStructureLevel>> IsValidGeneral(
            CountryStructureLevel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<CountryStructureLevel>>(() =>
            {
                var result = new ExecutionResult<CountryStructureLevel>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.LevelName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the Level name";
                    return result;
                }
                if (model.Level == 0)
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the Level";
                    return result;
                }

                ////var classNameInDb = context.SchoolMsClasses.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
                //var classNameInDb = context.CountryStructureLevels.FirstOrDefault(r => r.LevelName.Equals(model.LevelName));
                //if (classNameInDb != null)
                //{
                //    result.IsOkay = false;
                //    result.Message = "There is a level with that name already";
                //    return result;
                //}
                var countryInDb = context.Countries.FirstOrDefault(r => r.Id == model.CountryId);
                if (model.CountryId == 0 || countryInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "The supplied country does not exit in the system";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<CountryStructureLevel>> IsValidInsert(
            CountryStructureLevel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<CountryStructureLevel>> IsValidUpdate(
            CountryStructureLevel model,
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

            return result;
        }
    }
}
