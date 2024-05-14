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
    public class CountryStructureRepository : EFDatabaseModelBaseRepository<CountryStructure>
    {
        public CountryStructureRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<CountryStructure>> Add(
            CountryStructure model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructure>(true);
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

                context.CountryStructures.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //return a new instance of the inserted object
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

        public async Task<
            ExecutionResult<IEnumerable<CountryStructureLevel>>
        > GetCountryStructureLevel(NeedDataSourceEventArgs args, int levelAdding)
        {
            var result = new ExecutionResult<IEnumerable<CountryStructureLevel>>(
                true,
                "",
                new HashSet<CountryStructureLevel>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    var filterValue = args.GetFilterValue(0).ToLower();
                    ;

                    var countryStractureLevel = (
                        await context.CountryStructureLevels
                            .Join(
                                context.Countries,
                                cs => cs.CountryId,
                                c => c.Id,
                                (cs, c) => new { cs, c }
                            )
                            .Where(
                                r =>
                                    (
                                        string.IsNullOrEmpty(filterValue)
                                            ? true
                                            : r.c.CountryName.Contains(filterValue)
                                    )
                                    && r.cs.Level == levelAdding
                            )
                            .OrderBy(r => r.cs.Id)
                            .Skip(args.skip)
                            .Take(args.take)
                            .ToListAsync()
                    ).Select(
                        r =>
                            new CountryStructureLevel
                            {
                                Id = r.cs.Id,
                                Level = r.cs.Level,
                                LevelName = r.cs.LevelName,
                                CountryId = r.cs.CountryId,
                                Country = new Country
                                {
                                    Id = r.c.Id,
                                    CountryIso = r.c.CountryIso,
                                    CountryName = r.c.CountryName
                                }
                            }
                    );

                    result.Result = countryStractureLevel;
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public override CountryStructure CloneModel(CountryStructure model)
        {
            return new CountryStructure
            {
                Id = model.Id,
                ParentCountryStructureId = model.ParentCountryStructureId,
                CountryStructureLevelId = model.CountryStructureLevelId,
                StructureName = model.StructureName
            };
        }

        public override async Task<ExecutionResult<CountryStructure>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructure>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.CountryStructures.FirstOrDefaultAsync(r => r.Id == id);
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

        public override async Task<ExecutionResult<CountryStructure>> Update(
            CountryStructure model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructure>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.StructureName = model.StructureName;
                DbInstance.ParentCountryStructureId = model.ParentCountryStructureId;
                DbInstance.CountryStructureLevelId = model.CountryStructureLevelId;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
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

        public override async Task<ExecutionResult<CountryStructure>> Delete(
            CountryStructure model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<CountryStructure>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var classToDelete = context.CountryStructures.FirstOrDefault(r => r.Id == model.Id);

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The class you are trying to delete does not exist";
                    return executionResult;
                }

                executionResult = await GetItem((int)model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.CountryStructures.Remove(DbInstance);
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

                int filterValueIntId = -1;
                string filterValueId = string.Empty;
                if (args.HasFilterFields("Id"))
                    filterValueId = args.GetFilterValuePredicate(r => r.field == "Id", 0);

                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
                    filterValueIntId = -1;

                var countryStructures = (
                    await context.CountryStructures
                        .Join(
                            context.CountryStructureLevels,
                            cs => cs.CountryStructureLevelId,
                            csl => csl.Id,
                            (cs, csl) => new { cs, csl }
                        )
                        .Join(
                            context.Countries,
                            cscsl => cscsl.csl.CountryId,
                            cn => cn.Id,
                            (cscsl, cn) => new { cscsl, cn }
                        )
                        .Where(
                            r => filterValueIntId == -1 ? true : r.cscsl.cs.Id == filterValueIntId
                        )
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.cn.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new CountryStructure
                        {
                            Id = r.cscsl.cs.Id,
                            CountryStructureLevelId = r.cscsl.cs.CountryStructureLevelId,
                            CountryStructureLevel = new CountryStructureLevel
                            {
                                Id = r.cscsl.csl.Id,
                                CountryId = r.cscsl.csl.CountryId,
                                Level = r.cscsl.csl.Level,
                                LevelName = r.cscsl.csl.LevelName,
                                Country = new Country
                                {
                                    Id = r.cn.Id,
                                    CountryName = r.cn.CountryName,
                                    CountryIso = r.cn.CountryIso
                                }
                            },
                            ParentCountryStructureId = r.cscsl.cs.ParentCountryStructureId,
                            StructureName = r.cscsl.cs.StructureName,
                        }
                );

                var filterSupporter = new FilterSupporter<CountryStructure>(
                    countryStructures,
                    args
                );
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

        public override async Task<ExecutionResult<CountryStructure>> IsValidGeneral(
            CountryStructure model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<CountryStructure>>(() =>
            {
                var result = new ExecutionResult<CountryStructure>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.StructureName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the structure name";
                    return result;
                }

                var countryStracturelevel = context.CountryStructureLevels.FirstOrDefault(
                    r => r.Id == model.CountryStructureLevelId
                );
                if (countryStracturelevel == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please select country stracture level";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<CountryStructure>> IsValidInsert(
            CountryStructure model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<CountryStructure>> IsValidUpdate(
            CountryStructure model,
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
