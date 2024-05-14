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
    public class ClassSubjectTopicRepository
        : EFDatabaseModelBaseRepository<SchoolMsClassSubjectTopic>
    {
        public ClassSubjectTopicRepository(string currentModule)
            : base(currentModule) { }

        public async override Task<ExecutionResult<SchoolMsClassSubjectTopic>> Add(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClassSubjectTopic>(true);
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

                context.SchoolMsClassSubjectTopics.Add(modelInsert);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";

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

        public override SchoolMsClassSubjectTopic CloneModel(SchoolMsClassSubjectTopic model)
        {
            return new SchoolMsClassSubjectTopic
            {
                Id = model.Id,
                SchoolMsClassId = model.SchoolMsClassId,
                SchoolMsSubjectId = model.SchoolMsSubjectId,
                TopicName = model.TopicName,
                DateCreated = DateTime.UtcNow,
                Active = model.Active
            };
        }

        public override async Task<ExecutionResult<SchoolMsClassSubjectTopic>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClassSubjectTopic>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsClassSubjectTopics.FirstOrDefaultAsync(
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

        public override async Task<ExecutionResult<SchoolMsClassSubjectTopic>> Update(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClassSubjectTopic>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate

                //DbInstance.SchoolMsClassId = model.SchoolMsClassId;
                //DbInstance.SchoolMsSubjectId = model.SchoolMsSubjectId;
                DbInstance.TopicName = model.TopicName;
                //DbInstance.DateCreated = DbInstance.DateCreated;

                DbInstance.Active = model.Active;

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        DbInstance.Id,
                        DbInstance,
                        context
                    );
                    //executionResult.Result = CloneModel(DbInstance);
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

        public override async Task<ExecutionResult<SchoolMsClassSubjectTopic>> Delete(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsClassSubjectTopic>(true);
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

                context.SchoolMsClassSubjectTopics.Remove(DbInstance);
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

                var topics = (
                    await context.SchoolMsClassSubjectTopics
                        .Join(
                            context.SchoolMsSubjects,
                            t => t.SchoolMsSubjectId,
                            s => s.Id,
                            (t, s) => new { t, s }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            ts => ts.t.SchoolMsClassId,
                            c => c.Id,
                            (ts, c) => new { ts, c }
                        )
                        .Where(r => true)
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.ts.t.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsClassSubjectTopic
                        {
                            Id = r.ts.t.Id,
                            TopicName = r.ts.t.TopicName,
                            DateCreated = r.ts.t.DateCreated,
                            Active = r.ts.t.Active,
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.ts.s.Id,
                                SubjectName = r.ts.s.SubjectName,
                                SubjectCode = r.ts.s.SubjectCode,
                                Abbreviation = r.ts.s.Abbreviation
                            },
                            SchoolMsClass = new SchoolMsClass
                            {
                                Id = r.c.Id,
                                ClassName = r.c.ClassName
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsClassSubjectTopic>(topics, args);
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

        public async Task<ExecutionResult<IEnumerable<SchoolMsSubject>>> GetSubjects(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsSubject>>(
                true,
                "",
                new HashSet<SchoolMsSubject>()
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

                    var returnData = await context.SchoolMsSubjects
                        .Where(r => r.SubjectName.ToLower().Contains(filterValue))
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SchoolMsSubject { Id = r.Id, SubjectName = r.SubjectName, }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async Task<ExecutionResult<IEnumerable<SchoolMsClass>>> GetClasses(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsClass>>(
                true,
                "",
                new HashSet<SchoolMsClass>()
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

                    var returnData = await context.SchoolMsClasses
                        .Where(r => r.ClassName.ToLower().Contains(filterValue))
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SchoolMsClass { Id = r.Id, ClassName = r.ClassName }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public override async Task<ExecutionResult<SchoolMsClassSubjectTopic>> IsValidGeneral(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsClassSubjectTopic>>(() =>
            {
                var result = new ExecutionResult<SchoolMsClassSubjectTopic>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.TopicName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the topic";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsClassSubjectTopic>> IsValidInsert(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<SchoolMsClassSubjectTopic>> IsValidUpdate(
            SchoolMsClassSubjectTopic model,
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
