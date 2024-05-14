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

namespace LukurareBackend.Repositories.Configuration
{
    public class ClassesRepository : EFDatabaseModelBaseRepository<SchoolMsClass>
    {
        public ClassesRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<SchoolMsClass>> Add(
            SchoolMsClass model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClass>(true);
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

                context.SchoolMsClasses.Add(modelInsert);

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

        public override SchoolMsClass CloneModel(SchoolMsClass model)
        {
            return new SchoolMsClass
            {
                Id = model.Id,
                ClassName = model.ClassName,
                CountryId = model.Country.Id,
                IsActive = model.IsActive
            };
        }

        public override async Task<ExecutionResult<SchoolMsClass>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClass>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsClasses.FirstOrDefaultAsync(r => r.Id == id);
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

        public override async Task<ExecutionResult<SchoolMsClass>> Update(
            SchoolMsClass model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClass>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.ClassName = model.ClassName;
                DbInstance.CountryId = model.Country.Id;
                DbInstance.IsActive = model.IsActive;

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

        public override async Task<ExecutionResult<SchoolMsClass>> Delete(
            SchoolMsClass model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClass>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var classToDelete = context.SchoolMsClasses.FirstOrDefault(r => r.Id == model.Id);

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The class you are trying to delete does not exist";
                    return executionResult;
                }

                /* executionResult = await GetItem((int)model.Id, context);
                 if (executionResult.IsOkay == false)
                     return executionResult;
 */
                context.SchoolMsClasses.Remove(classToDelete);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    var country = context.Countries.FirstOrDefault(
                        r => r.Id == classToDelete.CountryId
                    );
                    classToDelete.Country = country;

                    executionResult.Result = CloneModel(classToDelete);
                    //executionResult.Result = await GetModelForInsertUpdateFromId(classToDelete.Id, classToDelete, context);
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

                var classes = (
                    await context.SchoolMsClasses
                        .Join(
                            context.Countries,
                            cl => cl.CountryId,
                            c => c.Id,
                            (cl, c) => new { cl, c }
                        )
                        .Where(r => true)
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.cl.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsClass
                        {
                            Id = r.cl.Id,
                            ClassName = r.cl.ClassName,
                            IsActive = r.cl.IsActive,
                            CountryId = r.cl.CountryId,
                            Country = new Country { Id = r.c.Id, CountryName = r.c.CountryName, }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsClass>(classes, args);
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

        public override async Task<ExecutionResult<SchoolMsClass>> IsValidGeneral(
            SchoolMsClass model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsClass>>(() =>
            {
                var result = new ExecutionResult<SchoolMsClass>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.ClassName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the grade name";
                    return result;
                }

                //var classNameInDb = context.SchoolMsClasses.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
                var classNameInDb = context.SchoolMsClasses.FirstOrDefault(
                    r =>
                        r.ClassName.Equals(model.ClassName)
                        && r.Id != model.Id
                        && r.CountryId == model.Country.Id
                );
                if (classNameInDb != null)
                {
                    result.IsOkay = false;
                    result.Message = "There is a grade with that name already";
                    return result;
                }
                var countryInDb = context.Countries.FirstOrDefault(r => r.Id == model.Country.Id);
                if (countryInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "The supplied country does not exit in the system";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsClass>> IsValidInsert(
            SchoolMsClass model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<SchoolMsClass>> IsValidUpdate(
            SchoolMsClass model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            /* result = await GetItem((int)model.Id, context);
             if (result.IsOkay == false)
                 return result;*/
            DbInstance = await context.SchoolMsClasses.FirstOrDefaultAsync(r => r.Id == model.Id);
            if (DbInstance == null)
            {
                result.IsOkay = false;
                result.Message = $"The entry with id supplied does not exist";
                return result;
            }

            return result;
        }
    }
}
