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

namespace LukurareBackend.Repositories.Student
{
    public class StudentSubjectLevelRepository
        : EFDatabaseModelBaseRepository<SchoolMsStudentSubjectLevel>
    {
        public StudentSubjectLevelRepository(string currentModule)
            : base(currentModule) { }

        public async override Task<ExecutionResult<SchoolMsStudentSubjectLevel>> Add(
            SchoolMsStudentSubjectLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentSubjectLevel>(true);
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

                if (modelInsert.IsCurrentLevel)
                {
                    var currentSubjectLevels = context.SchoolMsStudentSubjectLevels
                        .Where(
                            r =>
                                r.SchoolMsStudentId == modelInsert.SchoolMsStudentId
                                && r.SchoolMsSubjectId == modelInsert.SchoolMsSubjectId
                                && r.IsCurrentLevel == true
                        )
                        .ToList();
                    foreach (var item in currentSubjectLevels)
                    {
                        var subjectLevelToUpdate =
                            context.SchoolMsStudentSubjectLevels.FirstOrDefault(
                                r => r.Id == item.Id
                            );
                        subjectLevelToUpdate.IsCurrentLevel = false;
                    }
                }

                context.SchoolMsStudentSubjectLevels.Add(modelInsert);

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

        public override SchoolMsStudentSubjectLevel CloneModel(SchoolMsStudentSubjectLevel model)
        {
            return new SchoolMsStudentSubjectLevel
            {
                Id = model.Id,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                IsCurrentLevel = model.IsCurrentLevel,
                SchoolMsSubjectId = model.SchoolMsSubject.Id,
                SchoolMsStudentId = model.SchoolMsStudent.Id,
                SchoolMsLevelId = model.SchoolMsLevel.Id
            };
        }

        public override async Task<ExecutionResult<SchoolMsStudentSubjectLevel>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentSubjectLevel>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsStudentSubjectLevels.FirstOrDefaultAsync(
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

        public async Task<ExecutionResult<IEnumerable<SubAccount>>> GetSchools(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SubAccount>>(
                true,
                "",
                new HashSet<SubAccount>()
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

                    var schoolDetails = await context.SubAccounts
                        .Join(
                            context.SchoolMsSchools,
                            sa => sa.Id,
                            sc => sc.SubAccountId,
                            (sa, sc) => new { sa, sc }
                        )
                        .Where(r => r.sa.AccountName.Contains(filterValue))
                        .OrderBy(r => r.sa.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = schoolDetails.Select(
                        r =>
                            new SubAccount
                            {
                                Id = r.sa.Id,
                                AccountName = r.sa.AccountName,
                                SchoolMsSchool = new SchoolMsSchool { Id = r.sc.Id }
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

        public override async Task<ExecutionResult<SchoolMsStudentSubjectLevel>> Update(
            SchoolMsStudentSubjectLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentSubjectLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate




                if (model.IsCurrentLevel)
                {
                    var currentSubjectLevels = context.SchoolMsStudentSubjectLevels
                        .Where(
                            r =>
                                r.SchoolMsStudentId == model.SchoolMsStudentId
                                && r.SchoolMsSubjectId == model.SchoolMsSubjectId
                                && r.IsCurrentLevel == true
                        )
                        .ToList();
                    if (currentSubjectLevels.Count > 0)
                    {
                        foreach (var item in currentSubjectLevels)
                        {
                            var subjectLevel = context.SchoolMsStudentSubjectLevels.FirstOrDefault(
                                r => r.Id == item.Id
                            );
                            subjectLevel.IsCurrentLevel = false;
                        }
                    }
                }

                var subjectLevelToUpdate = context.SchoolMsStudentSubjectLevels.FirstOrDefault(
                    r => r.Id == model.Id
                );

                subjectLevelToUpdate.IsCurrentLevel = model.IsCurrentLevel;
                subjectLevelToUpdate.SchoolMsSubjectId = model.SchoolMsSubject.Id;
                subjectLevelToUpdate.SchoolMsLevelId = model.SchoolMsLevel.Id;

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

        public override async Task<ExecutionResult<SchoolMsStudentSubjectLevel>> Delete(
            SchoolMsStudentSubjectLevel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentSubjectLevel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var classToDelete = context.SchoolMsStudentSubjectLevels.FirstOrDefault(
                    r => r.Id == model.Id
                );

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The level you are trying to delete does not exist";
                    return executionResult;
                }

                executionResult = await GetItem((int)model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.SchoolMsStudentSubjectLevels.Remove(DbInstance);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    var level = context.SchoolMsLevels.FirstOrDefault(
                        r => r.Id == DbInstance.SchoolMsLevelId
                    );
                    DbInstance.SchoolMsLevel = level;
                    var student = context.SchoolMsStudents.FirstOrDefault(
                        r => r.Id == DbInstance.SchoolMsStudentId
                    );
                    DbInstance.SchoolMsStudent = student;
                    var subject = context.SchoolMsSubjects.FirstOrDefault(
                        r => r.Id == DbInstance.SchoolMsSubjectId
                    );
                    DbInstance.SchoolMsSubject = subject;

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
                if (args.HasFilterFields("SchoolMsStudentId"))
                    filterValueId = args.GetFilterValuePredicate(
                        r => r.field == "SchoolMsStudentId",
                        0
                    );

                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
                    filterValueIntId = -1;

                var studentDetails = (
                    await context.SchoolMsStudentSubjectLevels
                        .Join(
                            context.SchoolMsStudents,
                            sl => sl.SchoolMsStudentId,
                            st => st.Id,
                            (sl, st) => new { sl, st }
                        )
                        .Join(
                            context.SchoolMsSubjects,
                            slst => slst.sl.SchoolMsSubjectId,
                            sb => sb.Id,
                            (slst, sb) => new { slst, sb }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            slstsb => slstsb.slst.sl.SchoolMsLevelId,
                            l => l.Id,
                            (slstsb, l) => new { slstsb, l }
                        )
                        .Where(
                            r =>
                                filterValueIntId == -1
                                    ? true
                                    : r.slstsb.slst.sl.SchoolMsStudentId == filterValueIntId
                        )
                        .Skip(0)
                        .Take(args.take * args.page)
                        //.OrderBy(r => r.x.aeas.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsStudentSubjectLevel
                        {
                            Id = r.slstsb.slst.sl.Id,
                            SchoolMsStudentId = r.slstsb.slst.sl.SchoolMsStudentId,
                            SchoolMsLevelId = r.slstsb.slst.sl.SchoolMsLevelId,
                            SchoolMsSubjectId = r.slstsb.slst.sl.SchoolMsSubjectId,
                            IsCurrentLevel = r.slstsb.slst.sl.IsCurrentLevel,
                            EndDate = r.slstsb.slst.sl.EndDate,
                            StartDate = r.slstsb.slst.sl.StartDate,
                            SchoolMsStudent = new SchoolMsStudent { Id = r.slstsb.slst.st.Id, },
                            SchoolMsLevel = new SchoolMsLevel
                            {
                                Id = r.l.Id,
                                LevelName = r.l.LevelName
                            },
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.slstsb.sb.Id,
                                SubjectName = r.slstsb.sb.SubjectName,
                                SubjectCode = r.slstsb.sb.SubjectCode,
                                Abbreviation = r.slstsb.sb.Abbreviation,
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsStudentSubjectLevel>(
                    studentDetails,
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

        public override async Task<ExecutionResult<SchoolMsStudentSubjectLevel>> IsValidGeneral(
            SchoolMsStudentSubjectLevel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsStudentSubjectLevel>>(() =>
            {
                var result = new ExecutionResult<SchoolMsStudentSubjectLevel>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (model.EndDate == null)
                {
                    result.IsOkay = false;
                    result.Message = "please supply the end date";
                    return result;
                }
                var studentInDb = context.SchoolMsStudents.FirstOrDefault(
                    r => r.Id == model.SchoolMsStudent.Id
                );

                if (studentInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the student";
                    return result;
                }

                var levelInDb = context.SchoolMsLevels.FirstOrDefault(
                    r => r.Id == model.SchoolMsLevel.Id
                );

                if (levelInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the level";
                    return result;
                }

                var subjectInDb = context.SchoolMsSubjects.FirstOrDefault(
                    r => r.Id == model.SchoolMsSubject.Id
                );

                if (subjectInDb == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the subject";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsStudentSubjectLevel>> IsValidInsert(
            SchoolMsStudentSubjectLevel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            var currentSubjectLevels = context.SchoolMsStudentSubjectLevels.FirstOrDefault(
                r =>
                    r.SchoolMsStudentId == model.SchoolMsStudent.Id
                    && r.SchoolMsSubjectId == model.SchoolMsSubject.Id
                    && r.SchoolMsLevelId == model.SchoolMsLevel.Id
            );

            if (currentSubjectLevels != null)
            {
                result.IsOkay = false;
                result.Message = "Can not add the same subject and level twice";
                return result;
            }

            return result;
        }

        public override async Task<ExecutionResult<SchoolMsStudentSubjectLevel>> IsValidUpdate(
            SchoolMsStudentSubjectLevel model,
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
