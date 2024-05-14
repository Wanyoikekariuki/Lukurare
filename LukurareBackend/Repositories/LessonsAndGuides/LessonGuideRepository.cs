//using EFDatabaseModel.Contexts;
//using EFDatabaseModel.DbModel;
//using EFDatabaseModel.Repository;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml;
//using ProjectBase.KendoUiSupport;
//using ProjectBase.Repository.Transaction;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LukurareBackend.Repositories.LessonsAndGuides
//{
//    public class LessonGuideRepository : EFDatabaseModelBaseRepository<SchoolMsLessonGuide>
//    {
//        public LessonGuideRepository(string currentModule)
//            : base(currentModule) { }

//        public async override Task<ExecutionResult<SchoolMsLessonGuide>> Add(
//            SchoolMsLessonGuide model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLessonGuide>(true);
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

//                var defaultTheme = context.SchoolMsClassSubjectTopics.FirstOrDefault(
//                    r =>
//                        r.SchoolMsSubjectId == model.SchoolMsClassSubjectTopic.SchoolMsSubject.Id
//                        && r.TopicName.Equals(model.SchoolMsClassSubjectTopic.TopicName)
//                );
//                if (defaultTheme == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = "The selected theme and subject have not been set";
//                    return executionResult;
//                }
//                modelInsert.SchoolMsClassSubjectTopicId = defaultTheme.Id;
//                context.SchoolMsLessonGuides.Add(modelInsert);

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

//        public async Task<ExecutionResult<SchoolMsStudentClassHistory>> AddFromFile(
//            IFormFileCollection files,
//            String pathForUploadedFiled
//        )
//        {
//            var executionResult = new ExecutionResult<SchoolMsStudentClassHistory>(true);

//            try
//            {
//                long uploaded_size = 0;

//                var uploaded_files = files;

//                int iCounter = 0;

//                string sFiles_uploaded = "";
//                string uploaded_Filename = "";
//                foreach (var uploaded_file in uploaded_files)
//                {
//                    //----< Uploaded File >----

//                    iCounter++;

//                    uploaded_size += uploaded_file.Length;

//                    sFiles_uploaded += "\n" + uploaded_file.FileName;

//                    //< Filename >

//                    uploaded_Filename = $" {Guid.NewGuid().ToString()}-{uploaded_file.FileName}";

//                    string new_Filename_on_Server = pathForUploadedFiled + "\\" + uploaded_Filename;

//                    //</ Filename >



//                    //< Copy File to Target >

//                    using (
//                        FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create)
//                    )
//                    {
//                        await uploaded_file.CopyToAsync(stream);
//                    }

//                    //< Copy File to Target >

//                    //----</ Uploaded File >----
//                }

//                string message =
//                    $"Upload successful!\n files uploaded: {iCounter} \nsize: {uploaded_size} \n  {sFiles_uploaded}";

//                var contactsList = new List<SchoolMsStudentClassHistory>();
//                FileInfo existingFile = new FileInfo(pathForUploadedFiled + uploaded_Filename);

//                using (var package = new ExcelPackage(existingFile))
//                {
//                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//                    var currentSheet = package.Workbook.Worksheets;
//                    var workSheet = currentSheet.FirstOrDefault();

//                    if (workSheet == null)
//                    {
//                        //No work sheet
//                    }
//                    var noOfCol = workSheet.Dimension.End.Column;
//                    var noOfRow = workSheet.Dimension.End.Row;
//                    List<SchoolMsLessonGuide> lessonGuides = new List<SchoolMsLessonGuide>();

//                    using (var context = new EFDatabaseModelDatabaseContext())
//                    {
//                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
//                        {
//                            String level = workSheet.Cells[rowIterator, 1].Value.ToString();
//                            string subject = workSheet.Cells[rowIterator, 2].Value.ToString();
//                            string topic = workSheet.Cells[rowIterator, 3].Value.ToString();
//                            string content = workSheet.Cells[rowIterator, 4].Value.ToString();
//                            string date = workSheet.Cells[rowIterator, 5].Value.ToString();

//                            if (string.IsNullOrEmpty(level))
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message = "There is a guide with no level";
//                                return await Task.FromResult(executionResult);
//                            }

//                            var levelInDb = context.SchoolMsLevels.FirstOrDefault(
//                                r => r.LevelName.Equals(level.Trim())
//                            );
//                            if (levelInDb == null)
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message = "There is no level named '" + level + "'";
//                                return await Task.FromResult(executionResult);
//                            }

//                            if (string.IsNullOrEmpty(subject))
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message = "There is a guide with no Subject";
//                                return await Task.FromResult(executionResult);
//                            }
//                            var subjectInDb = context.SchoolMsSubjects.FirstOrDefault(
//                                r => r.SubjectName.Equals(subject.Trim())
//                            );
//                            if (subjectInDb == null)
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message =
//                                    "There is no subject named '" + subject + "'";
//                                return await Task.FromResult(executionResult);
//                            }

//                            if (string.IsNullOrEmpty(topic))
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message =
//                                    "Attempting to add a guide without a theme";
//                                return await Task.FromResult(executionResult);
//                            }
//                            var themeInDb = context.SchoolMsSubjects
//                                .Join(
//                                    context.SchoolMsClassSubjectTopics,
//                                    sub => sub.Id,
//                                    t => t.SchoolMsSubjectId,
//                                    (sub, t) => new { sub, t }
//                                )
//                                .FirstOrDefault(
//                                    r =>
//                                        r.sub.Id == subjectInDb.Id
//                                        && r.t.TopicName.Equals(topic.Trim())
//                                );

//                            if (themeInDb == null)
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message =
//                                    "The theme '" + topic + "' has not been configured ";
//                                return await Task.FromResult(executionResult);
//                            }
//                            if (string.IsNullOrEmpty(content))
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message = "Can not add a lesson without content";
//                                return await Task.FromResult(executionResult);
//                            }

//                            DateTime validDate;
//                            if (!DateTime.TryParse(date, out DateTime result))
//                            {
//                                executionResult.IsOkay = false;
//                                executionResult.Message = "Invalid date : " + date;
//                                return await Task.FromResult(executionResult);
//                            }
//                            validDate = result.ToUniversalTime();

//                            var theme = context.SchoolMsClassSubjectTopics
//                                .FirstOrDefault(
//                                    r =>
//                                        r.SchoolMsSubjectId == subjectInDb.Id
//                                        && r.TopicName.Equals(topic.Trim())
//                                )
//                                .Id;
//                            var student = new SchoolMsLessonGuide
//                            {
//                                SchoolMsClassSubjectTopicId = theme,
//                                GuideContent = content,
//                                SchoolMsLevelId = levelInDb.Id,
//                                DateCreated = DateTime.UtcNow,
//                                DateToSend = validDate,
//                                Cancelled = false,
//                                CancelledDate = DateTime.UtcNow,
//                            };

//                            lessonGuides.Add(student);
//                        }
//                        ;

//                        foreach (var guide in lessonGuides)
//                        {
//                            context.SchoolMsLessonGuides.Add(guide);
//                        }

//                        await context.SaveChangesAsync();
//                        executionResult.Message = "Guide(s) added succefully";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                executionResult.IsOkay = false;
//                executionResult.Message = ex.Message;
//            }

//            return executionResult;
//        }

//        public override SchoolMsLessonGuide CloneModel(SchoolMsLessonGuide model)
//        {
//            return new SchoolMsLessonGuide
//            {
//                Id = model.Id,
//                SchoolMsLevelId = model.SchoolMsLevel.Id,
//                DateCreated = model.DateCreated,
//                DateToSend = model.DateToSend,
//                GuideContent = model.GuideContent,
//                Cancelled = false,
//                CancelledDate = DateTime.UtcNow
//            };
//        }

//        public override async Task<ExecutionResult<SchoolMsLessonGuide>> GetItem(
//            int id,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLessonGuide>(true, "Entry found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                DbInstance = await context.SchoolMsLessonGuides.FirstOrDefaultAsync(
//                    r => r.Id == id
//                );
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

//        public override async Task<ExecutionResult<SchoolMsLessonGuide>> Update(
//            SchoolMsLessonGuide model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLessonGuide>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidUpdate(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed executionn result

//                //dbInstance is updated in GETITEM in IsValidUpdate

//                DbInstance.SchoolMsClassSubjectTopicId = model.SchoolMsClassSubjectTopic.Id;
//                DbInstance.GuideContent = model.GuideContent;
//                DbInstance.SchoolMsLevelId = model.SchoolMsLevel.Id;
//                DbInstance.DateCreated = model.DateCreated;
//                DbInstance.DateToSend = model.DateToSend;
//                DbInstance.CancelledDate = model.CancelledDate;
//                DbInstance.Cancelled = model.Cancelled;

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

//        public override async Task<ExecutionResult<SchoolMsLessonGuide>> Delete(
//            SchoolMsLessonGuide model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsLessonGuide>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //use get item to check if this ID is valid and incase it is
//                //Get item will set the DbInstance using the current context if Succefully looaded object
//                var lessonToDelete = context.SchoolMsLessonGuides.FirstOrDefault(
//                    r => r.Id == model.Id
//                );

//                if (lessonToDelete == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message =
//                        "The lesson guide you are trying to cancell does not exist";
//                    return executionResult;
//                }

//                /*executionResult = await GetItem((int)model.Id, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;*/
//                lessonToDelete.Cancelled = true;
//                lessonToDelete.CancelledDate = DateTime.UtcNow;

//                //context.SchoolMsLessonGuides.Remove(lessonToDelete);
//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Lesson guide has been cancelled";

//                    var subjectTopic = context.SchoolMsClassSubjectTopics.FirstOrDefault(
//                        r => r.Id == lessonToDelete.SchoolMsClassSubjectTopicId
//                    );
//                    lessonToDelete.SchoolMsClassSubjectTopic = subjectTopic;
//                    var level = context.SchoolMsLevels.FirstOrDefault(
//                        r => r.Id == lessonToDelete.SchoolMsLevelId
//                    );
//                    lessonToDelete.SchoolMsLevel = level;

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

//                var lessonGuides = (
//                    await context.SchoolMsLessonGuides
//                        .Join(
//                            context.SchoolMsLevels,
//                            lg => lg.SchoolMsLevelId,
//                            l => l.Id,
//                            (lg, l) => new { lg, l }
//                        )
//                        .Join(
//                            context.SchoolMsClassSubjectTopics,
//                            lgl => lgl.lg.SchoolMsClassSubjectTopicId,
//                            st => st.Id,
//                            (lgl, st) => new { lgl, st }
//                        )
//                        .Join(
//                            context.SchoolMsSubjects,
//                            lglst => lglst.st.SchoolMsSubjectId,
//                            sb => sb.Id,
//                            (lglst, sb) => new { lglst, sb }
//                        )
//                        .Join(
//                            context.SchoolMsClasses,
//                            lglstsb => lglstsb.lglst.st.SchoolMsClassId,
//                            cl => cl.Id,
//                            (lglstsb, cl) => new { lglstsb, cl }
//                        )
//                        .Where(r => r.lglstsb.lglst.lgl.lg.Cancelled == false)
//                        .Skip(0)
//                        .Take(args.take * args.page)
//                        //.OrderBy(r => r.x.aeas.Id)
//                        .ToListAsync()
//                ).Select(
//                    r =>
//                        new SchoolMsLessonGuide
//                        {
//                            Id = r.lglstsb.lglst.lgl.lg.Id,
//                            SchoolMsClassSubjectTopicId = r.lglstsb
//                                .lglst
//                                .lgl
//                                .lg
//                                .SchoolMsClassSubjectTopicId,
//                            GuideContent = r.lglstsb.lglst.lgl.lg.GuideContent,
//                            DateCreated = r.lglstsb.lglst.lgl.lg.DateCreated,
//                            DateToSend = r.lglstsb.lglst.lgl.lg.DateToSend,
//                            CancelledDate = r.lglstsb.lglst.lgl.lg.CancelledDate,
//                            Cancelled = r.lglstsb.lglst.lgl.lg.Cancelled,
//                            SchoolMsLevel = new SchoolMsLevel
//                            {
//                                Id = r.lglstsb.lglst.lgl.l.Id,
//                                LevelName = r.lglstsb.lglst.lgl.l.LevelName,
//                            },
//                            SchoolMsClassSubjectTopic = new SchoolMsClassSubjectTopic
//                            {
//                                Id = r.lglstsb.lglst.st.Id,
//                                TopicName = r.lglstsb.lglst.st.TopicName,
//                                SchoolMsSubject = new SchoolMsSubject
//                                {
//                                    Id = r.lglstsb.sb.Id,
//                                    SubjectName = r.lglstsb.sb.SubjectName,
//                                },
//                                SchoolMsClass = new SchoolMsClass
//                                {
//                                    Id = r.cl.Id,
//                                    ClassName = r.cl.ClassName
//                                }
//                            }
//                        }
//                );

//                var filterSupporter = new FilterSupporter<SchoolMsLessonGuide>(lessonGuides, args);
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

//        public async Task<ExecutionResult<IEnumerable<SchoolMsSubject>>> GetSubjects(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<SchoolMsSubject>>(
//                true,
//                "",
//                new HashSet<SchoolMsSubject>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
//                    if (string.IsNullOrEmpty(filterValue))
//                        return result;*/

//                    var returnData = await context.SchoolMsSubjects
//                        // .Join(context.SchoolMsSubjects, t => t.SchoolMsSubjectId, sb => sb.Id, (t, sb) => new { t, sb })
//                        //.Join(context.SchoolMsClasses, tsb => tsb.t.SchoolMsClassId, cl => cl.Id, (tsb, cl) => new { tsb, cl })

//                        .Where(r => true)
//                        .OrderBy(r => r.SubjectCode)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r => new SchoolMsSubject { Id = r.Id, SubjectName = r.SubjectName, }
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
//                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
//                    if (string.IsNullOrEmpty(filterValue))
//                        return result;*/

//                    var returnData = await context.SchoolMsLevels
//                        .Where(r => true)
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

//        public async Task<
//            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
//        > GetLessonGuidesSent(NeedDataSourceEventArgs args)
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
//                    await context.SchoolMsLessonGuideSmsSchedules
//                        .Join(
//                            context.SchoolMsStudents,
//                            lgs => lgs.SchoolMsStudentId,
//                            st => st.Id,
//                            (lgs, st) => new { lgs, st }
//                        )
//                        .Join(
//                            context.SchoolMsLessonGuides,
//                            lgsst => lgsst.lgs.SchoolMsLessonGuideId,
//                            lg => lg.Id,
//                            (lgsst, lg) => new { lgsst, lg }
//                        )
//                        .Join(
//                            context.SchoolMsSmsToSends,
//                            lgsstlg => lgsstlg.lgsst.lgs.SchoolMsSmsToSendId,
//                            sm => sm.Id,
//                            (lgsstlg, sm) => new { lgsstlg, sm }
//                        )
//                        .Where(
//                            r =>
//                                filterValueIntId == -1
//                                    ? true
//                                    : r.lgsstlg.lgsst.st.Id == filterValueIntId
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

//        public async Task<ExecutionResult<IEnumerable<SchoolMsClassSubjectTopic>>> GetSubjectTopic(
//            NeedDataSourceEventArgs args,
//            int SchoolMsSubjectId
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<SchoolMsClassSubjectTopic>>(
//                true,
//                "",
//                new HashSet<SchoolMsClassSubjectTopic>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
//                    if (string.IsNullOrEmpty(filterValue))
//                        return result;*/


//                    var returnData = await context.SchoolMsClassSubjectTopics
//                        .Join(
//                            context.SchoolMsSubjects,
//                            t => t.SchoolMsSubjectId,
//                            sb => sb.Id,
//                            (t, sb) => new { t, sb }
//                        )
//                        .Where(r => r.sb.Id == SchoolMsSubjectId)
//                        .OrderBy(r => r.sb.Id)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new SchoolMsClassSubjectTopic
//                            {
//                                Id = r.t.Id,
//                                TopicName = r.t.TopicName,
//                                SchoolMsSubjectId = r.sb.Id,
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

//        public override async Task<ExecutionResult<SchoolMsLessonGuide>> IsValidGeneral(
//            SchoolMsLessonGuide model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            return await Task.Run<ExecutionResult<SchoolMsLessonGuide>>(() =>
//            {
//                var result = new ExecutionResult<SchoolMsLessonGuide>(true);
//                if (model == null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "model is null";
//                    return result;
//                }
//                /*if (string.IsNullOrEmpty(model.Topic))
//                {
//                    result.IsOkay = false;
//                    result.Message = "Please supply the topic";
//                    return result;
//                }*/

//                if (string.IsNullOrEmpty(model.GuideContent))
//                {
//                    result.IsOkay = false;
//                    result.Message = "Please supply the guide content";
//                    return result;
//                }

//                return result;
//            });
//        }

//        public override async Task<ExecutionResult<SchoolMsLessonGuide>> IsValidInsert(
//            SchoolMsLessonGuide model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            return result;
//        }

//        public override async Task<ExecutionResult<SchoolMsLessonGuide>> IsValidUpdate(
//            SchoolMsLessonGuide model,
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
