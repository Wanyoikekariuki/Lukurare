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
    public class SubjectLevelRepository : EFDatabaseModelBaseRepository<SchoolMsLevel>
    {
        public SubjectLevelRepository(string currentModule)
            : base(currentModule) { }

        public async override Task<ExecutionResult<SchoolMsLevel>> Add(
            SchoolMsLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed execution result

                //insert the okay object to the DB
                var modelInsert = CloneModel(model);

                context.SchoolMsLevels.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //executionResult.Result = CloneModel(modelInsert);
                    //var Document = new IdentificationDocumentType
                    //{
                    //    Id=modelInsert.IdentificationDocumentTypeId,
                    //    DocumentName=modelInsert.IdentificationDocumentType.DocumentName

                    //};
                    //modelInsert.IdentificationDocumentType=Document;
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

        public override SchoolMsLevel CloneModel(SchoolMsLevel model)
        {
            return new SchoolMsLevel
            {
                Id = model.Id,
                Description = model.Description,
                LevelName = model.LevelName,
            };
        }

        public override async Task<ExecutionResult<SchoolMsLevel>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsLevel>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsLevels.FirstOrDefaultAsync(r => r.Id == id);
                if (DbInstance == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"The entry with id {id} supplied does not exist";
                    return executionResult;
                }

                //set the Result of executionResult object to the item found in the database
                //to avoid issues with serialization always use new

                // executionResult.Result = CloneModel(DbInstance);
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

        public override async Task<ExecutionResult<SchoolMsLevel>> Update(
            SchoolMsLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate

                DbInstance.Description = model.Description;
                DbInstance.LevelName = model.LevelName;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        model.Id,
                        model,
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

        public override async Task<ExecutionResult<SchoolMsLevel>> Delete(
            SchoolMsLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var classToDelete = context.SchoolMsLevels.FirstOrDefault(r => r.Id == model.Id);

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "The subject level you are trying to delete does not exist";
                    return executionResult;
                }
                /*
                                executionResult = await GetItem((int)model.Id, context);
                                if (executionResult.IsOkay == false)
                                    return executionResult;
                */
                context.SchoolMsLevels.Remove(classToDelete);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = CloneModel(classToDelete);
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

                var studentDetails = (
                    await context.SchoolMsLevels
                        .Where(r => true)
                        .Skip(0)
                        .Take(args.take * args.page)
                        //.OrderBy(r => r.x.aeas.Id)
                        .ToListAsync()
                );

                var filterSupporter = new FilterSupporter<SchoolMsLevel>(studentDetails, args);
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

        public override async Task<ExecutionResult<SchoolMsLevel>> IsValidGeneral(
            SchoolMsLevel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsLevel>>(() =>
            {
                var result = new ExecutionResult<SchoolMsLevel>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.LevelName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the subject level name";
                    return result;
                }

                if (string.IsNullOrEmpty(model.Description))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the level description";
                    return result;
                }
                var levelInDb = context.SchoolMsLevels.FirstOrDefault(
                    r => r.LevelName.Equals(model.LevelName) && r.Id != model.Id
                );
                if (levelInDb != null)
                {
                    result.IsOkay = false;
                    result.Message = "There is a level with that name already";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsLevel>> IsValidInsert(
            SchoolMsLevel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<SchoolMsLevel>> IsValidUpdate(
            SchoolMsLevel model,
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
