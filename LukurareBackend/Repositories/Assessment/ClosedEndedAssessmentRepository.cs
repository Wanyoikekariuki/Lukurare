using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using Microsoft.EntityFrameworkCore;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using LukurareBackend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Assessment
{
    public class ClosedEndedAssessmentRepository
        : EFDatabaseModelBaseRepository<List<AssessmentViewModel>>
    {
        public ClosedEndedAssessmentRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> Add(
            List<AssessmentViewModel> model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<List<AssessmentViewModel>>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                var optionLetters = context.SchoolMsAnswerOptionLetters.ToList();

                if (optionLetters.Count == 0)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "Please supply the answer option letters before trying to add a lesson guide";
                    return executionResult;
                }

                var firstItem = model.FirstOrDefault();
                var Subject = context.SchoolMsSubjects.FirstOrDefault(
                    r => r.Id == firstItem.SchoolMsSubjectId
                );
                var examToAdd = new SchoolMsAssessmentGroup
                {
                    SchoolMsClassId = context.SchoolMsClasses.FirstOrDefault().Id,
                    SchoolMsSubjectId = Subject.Id,
                    SchoolMsLevelId = firstItem.SchoolMsLevelId,
                    IsOpenEnded = false,
                    GroupName = $"{Subject.SubjectName.Substring(0, 3)} - {firstItem.ExamName}"
                };
                context.SchoolMsAssessmentGroups.Add(examToAdd);

                foreach (var item in model)
                {
                    var assessmentToAdd = new SchoolMsAssessmentQuestion
                    {
                        Id = item.Id,
                        SchoolMsAssessmentGroup = examToAdd,
                        QuestionNumber = item.QuestionNumber,
                        Question = item.Question,
                        CorrectAnswerResponseText = item.CorrectAnswerResponseText,
                        CorrectOptionLetterId = item.CorrectOptionLetter.Id,
                        WrongAnswerResponseText = item.WrongAnswerResponseText,
                        DateToSend = item.DateToSend,
                        SystemTransactionId = 1
                    };
                    if (string.IsNullOrEmpty(assessmentToAdd.CorrectAnswerResponseText))
                    {
                        assessmentToAdd.CorrectAnswerResponseText = null;
                    }
                    if (string.IsNullOrEmpty(assessmentToAdd.WrongAnswerResponseText))
                    {
                        assessmentToAdd.WrongAnswerResponseText = null;
                    }

                    context.SchoolMsAssessmentQuestions.Add(assessmentToAdd);
                    int defaultValue = 1;

                    foreach (var optionLetter in item.SchoolMsAssessmentAnswerOptions)
                    {
                        var assessmentAnswerOptionToadd = new SchoolMsAssessmentAnswerOption
                        {
                            SchoolMsAssessment = assessmentToAdd,
                            SchoolMsAnswerOptionLetterId = optionLetter.Id,
                            Value = optionLetter.Value
                        };
                        context.SchoolMsAssessmentAnswerOptions.Add(assessmentAnswerOptionToadd);
                        defaultValue++;
                    }
                }

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Saved successfully";
                    // modelInsert.Id = assessmentToAdd.Id;
                    //executionResult.Result = await GetModelForInsertUpdateFromId(modelInsert.Id, modelInsert, context);
                }
                else
                {
                    //incase the context was not created here return the object
                    //as inserted to the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    //  executionResult.Result = model;
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
            ExecutionResult<IEnumerable<SchoolMsClass>>
        > GetClassesWithAssessmentGroupsSet(NeedDataSourceEventArgs args)
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
                    /* if (string.IsNullOrEmpty(filterValue))
                         return result;*/
                    var themes = await context.SchoolMsClasses
                        .Where(r => true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.Id)
                        .ToListAsync();

                    result.Result = themes.Select(
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

                var classes = (
                    await context.SchoolMsAssessmentQuestions
                        .Join(
                            context.SchoolMsAssessmentGroups,
                            ass => ass.SchoolMsAssessmentGroupId,
                            ag => ag.Id,
                            (ass, ag) => new { ass, ag }
                        )
                        .Join(
                            context.SchoolMsSubjects,
                            assag => assag.ag.SchoolMsSubjectId,
                            sb => sb.Id,
                            (assag, sb) => new { assag, sb }
                        )
                        .Join(
                            context.SchoolMsAnswerOptionLetters,
                            assagsb => assagsb.assag.ass.CorrectOptionLetterId,
                            ol => ol.Id,
                            (assagsb, ol) => new { assagsb, ol }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            assagsbol => assagsbol.assagsb.assag.ag.SchoolMsLevelId,
                            l => l.Id,
                            (assagsbol, l) => new { assagsbol, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            assagsboll => assagsboll.assagsbol.assagsb.assag.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (assagsboll, cl) => new { assagsboll, cl }
                        )
                        .Where(r => true)
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.assagsboll.l.LevelName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AssessmentViewModel
                        {
                            SchoolMsClassId = r.cl.Id,
                            SchoolMsLevelId = r.assagsboll.l.Id,
                            Id = r.assagsboll.assagsbol.assagsb.assag.ass.Id,
                            QuestionNumber = r.assagsboll
                                .assagsbol
                                .assagsb
                                .assag
                                .ass
                                .QuestionNumber,
                            Question = r.assagsboll.assagsbol.assagsb.assag.ass.Question,
                            DateToSend = r.assagsboll.assagsbol.assagsb.assag.ass.DateToSend,
                            CorrectAnswerResponseText = r.assagsboll
                                .assagsbol
                                .assagsb
                                .assag
                                .ass
                                .CorrectAnswerResponseText,
                            WrongAnswerResponseText = r.assagsboll
                                .assagsbol
                                .assagsb
                                .assag
                                .ass
                                .WrongAnswerResponseText,
                            SchoolMsSubjectId = r.assagsboll.assagsbol.assagsb.sb.Id,
                            SchoolMsAssessmentGroupId = r.assagsboll.assagsbol.assagsb.assag.ag.Id,
                            CorrectOptionLetter = new SchoolMsAnswerOptionLetter
                            {
                                Id = r.assagsboll.assagsbol.ol.Id,
                                OptionLetter = r.assagsboll.assagsbol.ol.OptionLetter
                            },
                            SchoolMsLevel = new SchoolMsLevel
                            {
                                Id = r.assagsboll.l.Id,
                                LevelName = r.assagsboll.l.LevelName,
                            },
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.assagsboll.assagsbol.assagsb.sb.Id,
                                SubjectName = r.assagsboll.assagsbol.assagsb.sb.SubjectName,
                            },
                            SchoolMsClass = new SchoolMsClass
                            {
                                Id = r.cl.Id,
                                ClassName = r.cl.ClassName
                            },
                            SchoolMsAssessmentGroup = new SchoolMsAssessmentGroup
                            {
                                Id = r.assagsboll.assagsbol.assagsb.assag.ag.Id
                            }
                        }
                );
                var filterSupporter = new FilterSupporter<AssessmentViewModel>(classes, args);
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

        public async Task<ExecutionResult<IEnumerable<SchoolMsLevel>>> GetSubjectLevels(
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
                    var filterValue = args.GetFilterValue(0).ToLower();
                    ;
                    /* if (string.IsNullOrEmpty(filterValue))
                         return result;*/

                    var levels = await context.SchoolMsAssessmentGroups
                        .Join(
                            context.SchoolMsSubjects,
                            ag => ag.SchoolMsSubjectId,
                            sb => sb.Id,
                            (ag, sb) => new { ag, sb }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            agsb => agsb.ag.SchoolMsLevelId,
                            l => l.Id,
                            (agsb, l) => new { agsb, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            agsbl => agsbl.agsb.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (agsbl, cl) => new { agsbl, cl }
                        )
                        .Where(r => r.agsbl.agsb.ag.IsOpenEnded == false)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.agsbl.l.LevelName)
                        .ToListAsync();

                    result.Result = levels.Select(
                        r =>
                            new SchoolMsLevel { Id = r.agsbl.l.Id, LevelName = r.agsbl.l.LevelName }
                    );
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
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
                    var subjects = await context.SchoolMsAssessmentGroups
                        .Join(
                            context.SchoolMsSubjects,
                            ag => ag.SchoolMsSubjectId,
                            sb => sb.Id,
                            (ag, sb) => new { ag, sb }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            agsb => agsb.ag.SchoolMsLevelId,
                            l => l.Id,
                            (agsb, l) => new { agsb, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            agsbl => agsbl.agsb.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (agsbl, cl) => new { agsbl, cl }
                        )
                        .Where(r => r.agsbl.agsb.ag.IsOpenEnded == false)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.agsbl.agsb.sb.SubjectCode)
                        .ToListAsync();

                    result.Result = subjects.Select(
                        r =>
                            new SchoolMsSubject
                            {
                                Id = r.agsbl.agsb.sb.Id,
                                SubjectName = r.agsbl.agsb.sb.SubjectName
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

        public async Task<
            ExecutionResult<IEnumerable<SchoolMsClassSubjectTopic>>
        > GetClasSubjectTopics(NeedDataSourceEventArgs args, int SchoolMsSubjectId)
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
                    /* if (string.IsNullOrEmpty(filterValue))
                         return result;*/



                    var themes = await context.SchoolMsAssessmentGroups
                        .Join(
                            context.SchoolMsSubjects,
                            ag => ag.SchoolMsSubjectId,
                            sb => sb.Id,
                            (ag, sb) => new { ag, sb }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            agsb => agsb.ag.SchoolMsLevelId,
                            l => l.Id,
                            (agsb, l) => new { agsb, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            agsbl => agsbl.agsb.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (agsbl, cl) => new { agsbl, cl }
                        )
                        .Where(r => r.agsbl.agsb.ag.IsOpenEnded == false)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.agsbl.l.LevelName)
                        .ToListAsync();

                    result.Result = themes.Select(
                        r =>
                            new SchoolMsClassSubjectTopic
                            {
                                /*Id = r.st.Id,
                                TopicName = r.st.TopicName*/
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

        public async Task<
            ExecutionResult<IEnumerable<SchoolMsAnswerOptionLetter>>
        > GetAnswerOptionLetters(NeedDataSourceEventArgs args)
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsAnswerOptionLetter>>(
                true,
                "",
                new HashSet<SchoolMsAnswerOptionLetter>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    /*  var filterValue = args.GetFilterValue(0).ToLower(); ;
                      if (string.IsNullOrEmpty(filterValue))
                          return result;
        */
                    var returnData = await context.SchoolMsAnswerOptionLetters
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new SchoolMsAnswerOptionLetter
                            {
                                Id = r.Id,
                                OptionLetter = r.OptionLetter
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

        public override List<AssessmentViewModel> CloneModel(List<AssessmentViewModel> model)
        {
            List<AssessmentViewModel> list = new List<AssessmentViewModel>();
            foreach (var item in model)
            {
                var viewModel = new AssessmentViewModel
                {
                    Id = item.Id,
                    Question = item.Question,
                    CorrectAnswerResponseText = item.CorrectAnswerResponseText,
                    CorrectOptionLetterId = item.CorrectOptionLetter.Id,
                    WrongAnswerResponseText = item.WrongAnswerResponseText,
                    DateToSend = item.DateToSend,
                    SystemTransactionId = item.SystemTransactionId
                };
                list.Add(viewModel);
            }

            return list;
        }

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<List<AssessmentViewModel>>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var schoolMsAssessment =
                    await context.SchoolMsAssessmentQuestions.FirstOrDefaultAsync(r => r.Id == id);
                if (schoolMsAssessment == null)
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

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> Update(
            List<AssessmentViewModel> model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<List<AssessmentViewModel>>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                /*  executionResult = await this.IsValidUpdate(model, context);
                  if (executionResult.IsOkay == false)
                      return executionResult;*/
                //if fail return the failed executionn result


                /* var assessmentToUpadate = await context.SchoolMsAssessmentQuestions.FirstOrDefaultAsync(r => r.Id == model.Id);
        
                 if (assessmentToUpadate == null)
                 {
                     executionResult.IsOkay = false;
                     executionResult.Message = "The assessment record you are trying to update does not exist";
                     return executionResult;
                 }*/
                //dbInstance is updated in GETITEM in IsValidUpdate


                // assessmentToUpadate.SchoolMsLevelId = model.SchoolMsLevelId;
                //assessmentToUpadate.SchoolMsSubjectId = model.SchoolMsSubjectId;

                /*   assessmentToUpadate.SchoolMsAssessmentGroupId = model.SchoolMsAssessmentGroupId;
                   assessmentToUpadate.Question = model.Question;
                   assessmentToUpadate.CorrectOptionLetterId = model.CorrectOptionLetter.Id;
                   assessmentToUpadate.CorrectAnswerResponseText = model.CorrectAnswerResponseText;
                   assessmentToUpadate.WrongAnswerResponseText = model.WrongAnswerResponseText;
                   assessmentToUpadate.DateToSend = model.DateToSend.ToLocalTime();
                   assessmentToUpadate.SystemTransactionId = model.SystemTransactionId;*/

                /* if (string.IsNullOrEmpty(assessmentToUpadate.CorrectAnswerResponseText))
                 {
                     assessmentToUpadate.CorrectAnswerResponseText = null;
                 }
                 if (string.IsNullOrEmpty(assessmentToUpadate.WrongAnswerResponseText))
                 {
                     assessmentToUpadate.WrongAnswerResponseText = null;
                 }*/

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    // assessmentToUpadate.Id = assessmentToUpadate.Id;

                    //executionResult.Result = await GetModelForInsertUpdateFromId(assessmentToUpadate.Id, DbInstance, context);
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

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> Delete(
            List<AssessmentViewModel> model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<List<AssessmentViewModel>>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                //var assessmenToDelete = context.SchoolMsAssessmentQuestions.FirstOrDefault(r => r.Id == model.Id);

                /*if (assessmenToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The assessment you are trying to cancel does not exist";
                    return executionResult;
                }
        
                */
                /* executionResult = await GetItem((int)model.Id, context);
                         if (executionResult.IsOkay == false)
                             return executionResult;*/
                /*
                        assessmenToDelete.Cancelled = true;*/
                //context.SchoolMsAssessmentQuestions.Remove(assessmenToDelete);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Assessment ";
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

        public class AssessmentFilter
        {
            public int SchoolMsSubjectId { get; set; }
            public int SchoolMsLevelId { get; set; }
            public int SchoolMsClassSubjectTopicId { get; set; }
        }

        public async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetAssessments(NeedDataSourceEventArgs args)
        {
            EFDatabaseModelDatabaseContext context = null;
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "Class instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                //  var SchoolMsSubjectIdValue = args.GetFilterValuePredicate(r => r.field == "SchoolMsSubjectId", 0);
                // var SchoolMsLevelIdValue = args.GetFilterValuePredicate(r => r.field == "SchoolMsLevelId", 0);
                // var TopicIdValue = args.GetFilterValuePredicate(r => r.field == "SchoolMsClassSubjectTopicId", 0);
                var LevelValue = args.GetFilterValuePredicate(r => r.field == "SchoolMsLevelId", 0);
                var GroupValue = args.GetFilterValuePredicate(
                    r => r.field == "SchoolMsAssessmentGroupId",
                    0
                );

                long SchoolMsLevelId = -1,
                    SchoolMsAssessmentGroupId = -1;

                if (long.TryParse(LevelValue, out SchoolMsLevelId) == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Please select level";
                    return executionResult;
                }

                if (long.TryParse(GroupValue, out SchoolMsAssessmentGroupId) == false)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Please select assessment group";
                    return executionResult;
                }
                var classes = (
                    await context.SchoolMsAssessmentQuestions
                        .Join(
                            context.SchoolMsAssessmentGroups,
                            ass => ass.SchoolMsAssessmentGroupId,
                            ag => ag.Id,
                            (ass, ag) => new { ass, ag }
                        )
                        .Join(
                            context.SchoolMsSubjects,
                            assag => assag.ag.SchoolMsSubjectId,
                            sb => sb.Id,
                            (assag, sb) => new { assag, sb }
                        )
                        .Join(
                            context.SchoolMsAnswerOptionLetters,
                            assagsb => assagsb.assag.ass.CorrectOptionLetterId,
                            ol => ol.Id,
                            (assagsb, ol) => new { assagsb, ol }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            assagsbol => assagsbol.assagsb.assag.ag.SchoolMsLevelId,
                            l => l.Id,
                            (assagsbol, l) => new { assagsbol, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            assagsboll => assagsboll.assagsbol.assagsb.assag.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (assagsboll, cl) => new { assagsboll, cl }
                        )
                        .Where(
                            r =>
                                r.assagsboll.l.Id == SchoolMsLevelId
                                && r.assagsboll.assagsbol.assagsb.assag.ag.Id
                                    == SchoolMsAssessmentGroupId
                                && r.assagsboll.assagsbol.assagsb.assag.ag.IsOpenEnded == false
                                && r.assagsboll.assagsbol.assagsb.assag.ass.Cancelled == false
                        )
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.assagsboll.l.LevelName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AssessmentViewModel
                        {
                            SchoolMsClassId = r.cl.Id,
                            SchoolMsLevelId = r.assagsboll.l.Id,
                            Id = r.assagsboll.assagsbol.assagsb.assag.ass.Id,
                            QuestionNumber = r.assagsboll
                                .assagsbol
                                .assagsb
                                .assag
                                .ass
                                .QuestionNumber,
                            Question = r.assagsboll.assagsbol.assagsb.assag.ass.Question,
                            DateToSend = r.assagsboll.assagsbol.assagsb.assag.ass.DateToSend,
                            CorrectAnswerResponseText = r.assagsboll
                                .assagsbol
                                .assagsb
                                .assag
                                .ass
                                .CorrectAnswerResponseText,
                            WrongAnswerResponseText = r.assagsboll
                                .assagsbol
                                .assagsb
                                .assag
                                .ass
                                .WrongAnswerResponseText,
                            SchoolMsSubjectId = r.assagsboll.assagsbol.assagsb.sb.Id,
                            SchoolMsAssessmentGroupId = r.assagsboll.assagsbol.assagsb.assag.ag.Id,
                            CorrectOptionLetter = new SchoolMsAnswerOptionLetter
                            {
                                Id = r.assagsboll.assagsbol.ol.Id,
                                OptionLetter = r.assagsboll.assagsbol.ol.OptionLetter
                            },
                            SchoolMsLevel = new SchoolMsLevel
                            {
                                Id = r.assagsboll.l.Id,
                                LevelName = r.assagsboll.l.LevelName,
                            },
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.assagsboll.assagsbol.assagsb.sb.Id,
                                SubjectName = r.assagsboll.assagsbol.assagsb.sb.SubjectName,
                            },
                            SchoolMsClass = new SchoolMsClass
                            {
                                Id = r.cl.Id,
                                ClassName = r.cl.ClassName
                            },
                            SchoolMsAssessmentGroup = new SchoolMsAssessmentGroup
                            {
                                Id = r.assagsboll.assagsbol.assagsb.assag.ag.Id
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<AssessmentViewModel>(classes, args);
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

        public async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetAnswerOptionLetters(
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
                    await context.SchoolMsAssessmentAnswerOptions
                        .Join(
                            context.SchoolMsAnswerOptionLetters,
                            ao => ao.SchoolMsAnswerOptionLetterId,
                            al => al.Id,
                            (ao, al) => new { ao, al }
                        )
                        .Where(r => true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.ao.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsAssessmentAnswerOption
                        {
                            Id = r.ao.Id,
                            SchoolMsAnswerOptionLetter = new SchoolMsAnswerOptionLetter
                            {
                                Id = r.al.Id,
                                OptionLetter = r.al.OptionLetter
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsAssessmentAnswerOption>(
                    classes,
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

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> IsValidGeneral(
            List<AssessmentViewModel> model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<List<AssessmentViewModel>>>(() =>
            {
                var result = new ExecutionResult<List<AssessmentViewModel>>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                var Exam = model.FirstOrDefault();
                if (string.IsNullOrEmpty(Exam.ExamName))
                {
                    result.IsOkay = false;
                    result.Message = "Please enter the exam name";
                    return result;
                }

                foreach (var item in model)
                {
                    if (string.IsNullOrEmpty(item.Question))
                    {
                        result.IsOkay = false;
                        result.Message = "Please supply the question";
                        return result;
                    }
                    // var classNameInDb = context.SchoolMsAssessmentes.FirstOrDefault(r => string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
                    var level = context.SchoolMsLevels.FirstOrDefault(
                        r => r.Id == item.SchoolMsLevelId
                    );

                    if (level == null)
                    {
                        result.IsOkay = false;
                        result.Message = "Please select the level";
                        return result;
                    }
                    var subjects = context.SchoolMsSubjects.FirstOrDefault(
                        r => r.Id == item.SchoolMsSubjectId
                    );
                    if (subjects == null)
                    {
                        result.IsOkay = false;
                        result.Message = "Please select subject";
                        return result;
                    }

                    var correctOptionLetter = context.SchoolMsAnswerOptionLetters.FirstOrDefault(
                        r => r.Id == item.CorrectOptionLetter.Id
                    );
                    if (correctOptionLetter == null)
                    {
                        result.IsOkay = false;
                        result.Message = "Please select the correct option letter";
                        return result;
                    }

                    var Subject = context.SchoolMsSubjects.FirstOrDefault(
                        r => r.Id == item.SchoolMsSubjectId
                    );
                    var examName = $"{Subject.SubjectName.Substring(0, 3)} - {item.ExamName}";
                    var examNameInDb = context.SchoolMsAssessmentGroups.FirstOrDefault(
                        r =>
                            r.GroupName.Equals(examName)
                            && r.SchoolMsLevelId == item.SchoolMsLevelId
                            && r.SchoolMsSubjectId == item.SchoolMsSubjectId
                    );
                    if (examNameInDb != null)
                    {
                        result.IsOkay = false;
                        result.Message = "There is an exam with that name already";
                        return result;
                    }
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> IsValidInsert(
            List<AssessmentViewModel> model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<List<AssessmentViewModel>>> IsValidUpdate(
            List<AssessmentViewModel> model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation


            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //result = await GetItem((int)model.Id, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }
    }
}
