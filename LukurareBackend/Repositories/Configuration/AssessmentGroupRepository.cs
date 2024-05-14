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
    public class AssessmentGroupRepository : EFDatabaseModelBaseRepository<SchoolMsAssessmentGroup>
    {
        public AssessmentGroupRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> Add(
            SchoolMsAssessmentGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentGroup>(true);
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

                modelInsert.SchoolMsClassId = context.SchoolMsClasses.FirstOrDefault().Id;
                modelInsert.GroupName =
                    $"{model.SchoolMsSubject.SubjectName.Substring(0, 3)} - {modelInsert.GroupName}";
                context.SchoolMsAssessmentGroups.Add(modelInsert);

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

        public override SchoolMsAssessmentGroup CloneModel(SchoolMsAssessmentGroup model)
        {
            return new SchoolMsAssessmentGroup
            {
                Id = model.Id,
                GroupName = model.GroupName,
                SchoolMsLevelId = model.SchoolMsLevel.Id,
                SchoolMsSubjectId = model.SchoolMsSubject.Id,
                IsOpenEnded = model.IsOpenEnded,
            };
        }

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentGroup>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsAssessmentGroups.FirstOrDefaultAsync(
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

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> Update(
            SchoolMsAssessmentGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentGroup>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate


                DbInstance.SchoolMsClassId = model.SchoolMsClass.Id;
                DbInstance.SchoolMsSubjectId = model.SchoolMsSubject.Id;
                DbInstance.SchoolMsLevelId = model.SchoolMsLevel.Id;
                DbInstance.GroupName = model.GroupName;
                DbInstance.IsOpenEnded = model.IsOpenEnded;

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

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> Delete(
            SchoolMsAssessmentGroup model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentGroup>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object

                var classToDelete = context.SchoolMsAssessmentGroups
                    .Join(
                        context.SchoolMsLevels,
                        ag => ag.SchoolMsLevelId,
                        l => l.Id,
                        (ag, l) => new { ag, l }
                    )
                    .Join(
                        context.SchoolMsClasses,
                        agl => agl.ag.SchoolMsClassId,
                        cl => cl.Id,
                        (agl, cl) => new { agl, cl }
                    )
                    .Join(
                        context.SchoolMsSubjects,
                        aglcl => aglcl.agl.ag.SchoolMsSubjectId,
                        sb => sb.Id,
                        (aglcl, sb) => new { aglcl, sb }
                    )
                    .FirstOrDefault(r => r.aglcl.agl.ag.Id == model.Id);

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The class you are trying to delete does not exist";
                    return executionResult;
                }

                /* executionResult = await GetItem((int)model.Id, context);
                 if (executionResult.IsOkay == false)
                     return executionResult;*/

                context.SchoolMsAssessmentGroups.Remove(classToDelete.aglcl.agl.ag);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = CloneModel(classToDelete.aglcl.agl.ag);
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
                    /* if (string.IsNullOrEmpty(filterValue))
                         return result;*/
                    //r => filterValue == null ? true : r.RoleName.ToLower().Contains(filterValue)

                    var returnData = await context.SchoolMsSubjects
                        .Where(r => true)
                        .OrderBy(r => r.SubjectCode)
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

        public async Task<ExecutionResult<IEnumerable<SchoolMsLevel>>> GetLevels(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsLevel>>(
                true,
                "",
                new HashSet<SchoolMsLevel>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    /* var filterValue = args.GetFilterValue(0).ToLower(); ;
                     if (string.IsNullOrEmpty(filterValue))
                         return result;*/

                    var returnData = await context.SchoolMsLevels
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SchoolMsLevel { Id = r.Id, LevelName = r.LevelName, }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async Task<ExecutionResult<IEnumerable<SchoolMsClass>>> GetGrades(
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
                    /* var filterValue = args.GetFilterValue(0).ToLower(); ;
                     if (string.IsNullOrEmpty(filterValue))
                         return result;*/

                    var returnData = await context.SchoolMsClasses
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r => new SchoolMsClass { Id = r.Id, ClassName = r.ClassName, }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async Task<ExecutionResult<IEnumerable<SchoolMsClassSubjectTopic>>> GetSubjectTopic(
            NeedDataSourceEventArgs args,
            int SchoolMsClassId,
            int SchoolMsSubjectId
        )
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsClassSubjectTopic>>(
                true,
                "",
                new HashSet<SchoolMsClassSubjectTopic>()
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

                    var returnData = await context.SchoolMsClassSubjectTopics
                        .Join(
                            context.SchoolMsSubjects,
                            t => t.SchoolMsSubjectId,
                            sb => sb.Id,
                            (t, sb) => new { t, sb }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            tsb => tsb.t.SchoolMsClassId,
                            cl => cl.Id,
                            (tsb, cl) => new { tsb, cl }
                        )
                        .Where(r => r.cl.Id == SchoolMsClassId && r.tsb.sb.Id == SchoolMsSubjectId)
                        .OrderBy(r => r.cl.ClassName)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new SchoolMsClassSubjectTopic
                            {
                                Id = r.tsb.t.Id,
                                TopicName = r.tsb.t.TopicName,
                                SchoolMsClassId = r.cl.Id,
                                SchoolMsSubjectId = r.tsb.sb.Id,
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
            >(true, "Assessment groups");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                var classes = (
                    await context.SchoolMsAssessmentGroups
                        .Join(
                            context.SchoolMsLevels,
                            ag => ag.SchoolMsLevelId,
                            l => l.Id,
                            (ag, l) => new { ag, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            agl => agl.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (agl, cl) => new { agl, cl }
                        )
                        .Join(
                            context.SchoolMsSubjects,
                            aglcl => aglcl.agl.ag.SchoolMsSubjectId,
                            sb => sb.Id,
                            (aglcl, sb) => new { aglcl, sb }
                        )
                        .Where(r => true)
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.aglcl.agl.l.LevelName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsAssessmentGroup
                        {
                            Id = r.aglcl.agl.ag.Id,
                            GroupName = r.aglcl.agl.ag.GroupName,
                            IsOpenEnded = r.aglcl.agl.ag.IsOpenEnded,
                            SchoolMsLevel = new SchoolMsLevel
                            {
                                Id = r.aglcl.agl.l.Id,
                                LevelName = r.aglcl.agl.l.LevelName,
                            },
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.sb.Id,
                                SubjectName = r.sb.SubjectName,
                                Abbreviation = r.sb.Abbreviation
                            },
                            SchoolMsClass = new SchoolMsClass
                            {
                                Id = r.aglcl.cl.Id,
                                ClassName = r.aglcl.cl.ClassName,
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsAssessmentGroup>(classes, args);
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

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> IsValidGeneral(
            SchoolMsAssessmentGroup model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsAssessmentGroup>>(() =>
            {
                var result = new ExecutionResult<SchoolMsAssessmentGroup>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.GroupName))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the group name";
                    return result;
                }

                //var classNameInDb = context.SchoolMsClasses.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
                string GroupName =
                    $"{model.SchoolMsSubject.SubjectName.Substring(0, 3)} - {model.GroupName}";

                var classNameInDb = context.SchoolMsAssessmentGroups.FirstOrDefault(
                    r =>
                        r.GroupName.Equals(GroupName)
                        && r.Id != model.Id
                        && r.SchoolMsLevelId == model.SchoolMsLevel.Id
                        && r.SchoolMsSubjectId == model.SchoolMsSubject.Id
                );
                if (classNameInDb != null)
                {
                    result.IsOkay = false;
                    result.Message = "There is an exam with that name already";
                    return result;
                }
                /* var countryInDb = context.Countries.FirstOrDefault(r => r.Id == model.Country.Id);
                 if (countryInDb == null)
                 {
                     result.IsOkay = false;
                     result.Message = "The supplied country does not exit in the system";
                     return result;
                 }*/


                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> IsValidInsert(
            SchoolMsAssessmentGroup model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<SchoolMsAssessmentGroup>> IsValidUpdate(
            SchoolMsAssessmentGroup model,
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
            DbInstance = await context.SchoolMsAssessmentGroups.FirstOrDefaultAsync(
                r => r.Id == model.Id
            );
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
