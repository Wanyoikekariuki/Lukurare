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

//namespace LukurareBackend.Repositories.LessonsAndGuides
//{
//    public class LessonsRepository : EFDatabaseModelBaseRepository<SchoolMsLesson>
//    {
//        public LessonsRepository(string currentModule)
//            : base(currentModule) { }

//        public async override Task<ExecutionResult<SchoolMsLesson>> Add(
//            SchoolMsLesson model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLesson>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidInsert(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed execution result

//                //insert the okay object to the DB
//                var modelInsert = CloneModel(model);

//                context.SchoolMsLessons.Add(modelInsert);

//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Create operation was successful";
//                    //executionResult.Result = CloneModel(modelInsert);
//                    //var Document = new IdentificationDocumentType
//                    //{
//                    //    Id=modelInsert.IdentificationDocumentTypeId,
//                    //    DocumentName=modelInsert.IdentificationDocumentType.DocumentName

//                    //};
//                    //modelInsert.IdentificationDocumentType=Document;
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

//        public override SchoolMsLesson CloneModel(SchoolMsLesson model)
//        {
//            return new SchoolMsLesson
//            {
//                Id = model.Id,
//                LessonContent = model.LessonContent,
//                CreateDate = model.CreateDate,
//                DateToSend = model.DateToSend,
//                SchoolMsLessonGuideId = model.SchoolMsLessonGuide.Id
//            };
//        }

//        public override async Task<ExecutionResult<SchoolMsLesson>> GetItem(
//            int id,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLesson>(true, "Entry found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                DbInstance = await context.SchoolMsLessons.FirstOrDefaultAsync(r => r.Id == id);
//                if (DbInstance == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = $"The entry with id {id} supplied does not exist";
//                    return executionResult;
//                }

//                //set the Result of executionResult object to the item found in the database
//                //to avoid issues with serialization always use new

//                // executionResult.Result = CloneModel(DbInstance);
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

//        public override async Task<ExecutionResult<SchoolMsLesson>> Update(
//            SchoolMsLesson model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLesson>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidUpdate(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed executionn result

//                //dbInstance is updated in GETITEM in IsValidUpdate

//                DbInstance.LessonContent = model.LessonContent;
//                DbInstance.CreateDate = model.CreateDate;
//                DbInstance.DateToSend = model.DateToSend;
//                DbInstance.SchoolMsLessonGuideId = model.SchoolMsLessonGuide.Id;

//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Update operation was successful";
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

//        public override async Task<ExecutionResult<SchoolMsLesson>> Delete(
//            SchoolMsLesson model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLesson>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //use get item to check if this ID is valid and incase it is
//                //Get item will set the DbInstance using the current context if Succefully looaded object
//                var lessonToDelete = context.SchoolMsLessons.FirstOrDefault(r => r.Id == model.Id);

//                if (lessonToDelete == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = "The lesson you are trying to cancell does not exist";
//                    return executionResult;
//                }

//                /* executionResult = await GetItem((int)model.Id, context);
//                 if (executionResult.IsOkay == false)
//                     return executionResult;
// */
//                lessonToDelete.Cancelled = true;
//                lessonToDelete.CancelledDate = DateTime.UtcNow;
//                // context.SchoolMsLessons.Remove(lessonToDelete);
//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Cancell operation was successful";

//                    var lessonGuide = context.SchoolMsLessonGuides.FirstOrDefault(
//                        r => r.Id == lessonToDelete.SchoolMsLessonGuideId
//                    );
//                    // lessonToDelete.SchoolMsClassSubjectTopic = subjectTopic;
//                    // var level = context.SchoolMsLevels.FirstOrDefault(r => r.Id == lessonToDelete.SchoolMsLevelId);
//                    lessonToDelete.SchoolMsLessonGuide = lessonGuide;

//                    executionResult.Result = CloneModel(lessonToDelete);
//                    //executionResult.Result = await GetModelForInsertUpdateFromId(lessonToDelete.Id, lessonToDelete, context);
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

//        public override async Task<
//            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
//        > GetKendoGridFiltered(
//            NeedDataSourceEventArgs args,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<
//                ItemsRequestedResult<NeedDataSourceEventArgs>
//            >(true, "Class instance found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

//                int filterValueIntId = -1;
//                string filterValueId = string.Empty;
//                if (args.HasFilterFields("SchoolMsLessonGuideId"))
//                    filterValueId = args.GetFilterValuePredicate(
//                        r => r.field == "SchoolMsLessonGuideId",
//                        0
//                    );

//                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
//                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
//                    filterValueIntId = -1;

//                var lessonGuides = (
//                    await context.SchoolMsLessons
//                        .Join(
//                            context.SchoolMsLessonGuides,
//                            ls => ls.SchoolMsLessonGuideId,
//                            lg => lg.Id,
//                            (ls, lg) => new { ls, lg }
//                        )
//                        .Join(
//                            context.SchoolMsClassSubjectTopics,
//                            lslg => lslg.lg.SchoolMsClassSubjectTopicId,
//                            cls => cls.Id,
//                            (lslg, cls) => new { lslg, cls }
//                        )
//                        .Where(
//                            r =>
//                                filterValueIntId == -1
//                                    ? true
//                                    : r.lslg.lg.Id == filterValueIntId
//                                        && r.lslg.ls.Cancelled == false
//                        )
//                        .Skip(0)
//                        .Take(args.take * args.page)
//                        //.OrderBy(r => r.x.aeas.Id)
//                        .ToListAsync()
//                ).Select(
//                    r =>
//                        new SchoolMsLesson
//                        {
//                            Id = r.lslg.ls.Id,
//                            LessonContent = r.lslg.ls.LessonContent,
//                            CreateDate = r.lslg.ls.CreateDate,
//                            DateToSend = r.lslg.ls.DateToSend,
//                            SchoolMsLessonGuideId = r.lslg.lg.Id,
//                            SchoolMsLessonGuide = new SchoolMsLessonGuide
//                            {
//                                Id = r.lslg.lg.Id,
//                                SchoolMsClassSubjectTopic = new SchoolMsClassSubjectTopic
//                                {
//                                    Id = r.cls.Id,
//                                    TopicName = r.cls.TopicName
//                                }
//                            }
//                        }
//                );

//                var filterSupporter = new FilterSupporter<SchoolMsLesson>(lessonGuides, args);
//                var filteredData = filterSupporter.FilterData<long>(r => r.Id);
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

//        public async Task<
//            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
//        > GetLessonsSent(NeedDataSourceEventArgs args)
//        {
//            EFDatabaseModelDatabaseContext context = null;
//            var executionResult = new ExecutionResult<
//                ItemsRequestedResult<NeedDataSourceEventArgs>
//            >(true, "Class instance found");
//            try
//            {
//                context = new EFDatabaseModelDatabaseContext();

//                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

//                int filterValueIntId = -1;
//                string filterValueId = string.Empty;
//                if (args.HasFilterFields("SchoolMsStudentId"))
//                    filterValueId = args.GetFilterValuePredicate(
//                        r => r.field == "SchoolMsStudentId",
//                        0
//                    );

//                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
//                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
//                    filterValueIntId = -1;

//                var sms = (
//                    await context.SchoolMsLessonSmsSchedules
//                        .Join(
//                            context.SchoolMsStudents,
//                            ls => ls.SchoolMsStudentId,
//                            st => st.Id,
//                            (ls, st) => new { ls, st }
//                        )
//                        .Join(
//                            context.SchoolMsLessons,
//                            lsst => lsst.ls.SchoolMsLessonId,
//                            l => l.Id,
//                            (lsst, l) => new { lsst, l }
//                        )
//                        .Join(
//                            context.SchoolMsSmsToSends,
//                            lsstl => lsstl.lsst.ls.SchoolMsSmsToSendId,
//                            sm => sm.Id,
//                            (lsstl, sm) => new { lsstl, sm }
//                        )
//                        .Where(
//                            r =>
//                                filterValueIntId == -1
//                                    ? true
//                                    : r.lsstl.lsst.st.Id == filterValueIntId
//                        )
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        //.OrderBy(r => r.x.aeas.Id)
//                        .ToListAsync()
//                ).Select(
//                    r =>
//                        new SchoolMsLessonGuideSmsSchedule
//                        {
//                            Id = r.sm.Id,
//                            SchoolMsSmsToSend = new SchoolMsSmsToSend
//                            {
//                                PhoneNumber = r.sm.PhoneNumber,
//                                SentTime = r.sm.SentTime
//                            }
//                        }
//                );

//                var filterSupporter = new FilterSupporter<SchoolMsLessonGuideSmsSchedule>(
//                    sms,
//                    args
//                );
//                var filteredData = filterSupporter.FilterData<long>(r => r.Id);
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
//                await context.DisposeAsync();
//            }
//            return executionResult;
//        }

//        public async Task<ExecutionResult<IEnumerable<SchoolMsLessonGuide>>> GetLessonsGuides(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<SchoolMsLessonGuide>>(
//                true,
//                "",
//                new HashSet<SchoolMsLessonGuide>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    var filterValue = args.GetFilterValue(0).ToLower();
//                    ;
//                    if (string.IsNullOrEmpty(filterValue))
//                        return result;

//                    var returnData = await context.SchoolMsLessonGuides
//                        .Join(
//                            context.SchoolMsClassSubjectTopics,
//                            lg => lg.SchoolMsClassSubjectTopicId,
//                            clt => clt.Id,
//                            (lg, clt) => new { lg, clt }
//                        )
//                        .Where(r => r.clt.TopicName.Contains(filterValue))
//                        .OrderBy(r => r.clt.Id)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new SchoolMsLessonGuide
//                            {
//                                Id = r.lg.Id,
//                                SchoolMsClassSubjectTopic = new SchoolMsClassSubjectTopic
//                                {
//                                    Id = r.clt.Id,
//                                    TopicName = r.clt.TopicName
//                                }
//                            }
//                    );
//                }
//            }
//            catch (Exception ex)
//            {
//                result = result.CopyFromException(ex);
//            }
//            return result;
//        }

//        public async Task<ExecutionResult<IEnumerable<SchoolMsLevel>>> GetLevels(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<SchoolMsLevel>>(
//                true,
//                "",
//                new HashSet<SchoolMsLevel>()
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

//                    var returnData = await context.SchoolMsLevels
//                        .Where(
//                            r =>
//                                filterValue == null
//                                    ? true
//                                    : r.LevelName.ToLower().Contains(filterValue)
//                        )
//                        .OrderBy(r => r.Id)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r => new SchoolMsLevel { Id = r.Id, LevelName = r.LevelName, }
//                    );
//                }
//            }
//            catch (Exception ex)
//            {
//                result = result.CopyFromException(ex);
//            }
//            return result;
//        }

//        public override async Task<ExecutionResult<SchoolMsLesson>> IsValidGeneral(
//            SchoolMsLesson model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            return await Task.Run<ExecutionResult<SchoolMsLesson>>(() =>
//            {
//                var result = new ExecutionResult<SchoolMsLesson>(true);
//                if (model == null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "model is null";
//                    return result;
//                }
//                if (string.IsNullOrEmpty(model.LessonContent))
//                {
//                    result.IsOkay = false;
//                    result.Message = "Please supply the lesson content";
//                    return result;
//                }

//                return result;
//            });
//        }

//        public override async Task<ExecutionResult<SchoolMsLesson>> IsValidInsert(
//            SchoolMsLesson model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            return result;
//        }

//        public override async Task<ExecutionResult<SchoolMsLesson>> IsValidUpdate(
//            SchoolMsLesson model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            //if it passes general validation then use get item to complete update validation
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            result = await GetItem((int)model.Id, context);
//            if (result.IsOkay == false)
//                return result;

//            return result;
//        }
//    }
//}
