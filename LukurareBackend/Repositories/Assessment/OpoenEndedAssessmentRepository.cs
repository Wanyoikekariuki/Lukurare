using EFDatabaseModel.Repository;
using LukurareBackend.ViewModel;
using System;
using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using Microsoft.EntityFrameworkCore;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Transaction;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Assessment
{
    public class OpoenEndedAssessmentRepository : EFDatabaseModelBaseRepository<AssessmentViewModel>
    {
        public OpoenEndedAssessmentRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<AssessmentViewModel>> Add(
            AssessmentViewModel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AssessmentViewModel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result
                var modelInsert = CloneModel(model);

                var optionLetters = context.SchoolMsAnswerOptionLetters.ToList();

                if (optionLetters.Count == 0)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "Please supply the answer option letters before trying to add a lesson guide";
                    return executionResult;
                }

                /* if (model.QuestionNumber == 0)
                 {
                     executionResult.IsOkay = false;
                     executionResult.Message = "Please enter question number";
                     return executionResult;
                 }*/

                model.SystemTransactionId = 1;

                int questionNumber = model.QuestionNumber;

                var questionNumberExist = context.SchoolMsAssessmentQuestions.FirstOrDefault(
                    r =>
                        r.SchoolMsAssessmentGroupId == model.SchoolMsAssessmentGroupId
                        && r.SchoolMsAssessmentGroupId == modelInsert.SchoolMsAssessmentGroupId
                        && r.QuestionNumber == questionNumber
                );

                var sortedNumbers = context.SchoolMsAssessmentQuestions
                    .Join(
                        context.SchoolMsAssessmentGroups,
                        ass => ass.SchoolMsAssessmentGroupId,
                        ag => ag.Id,
                        (ass, ag) => new { ass, ag }
                    )
                    .Where(
                        r =>
                            r.ass.SchoolMsAssessmentGroupId == model.SchoolMsAssessmentGroupId
                            && r.ag.IsOpenEnded == true
                    )
                    .OrderBy(r => r.ass.QuestionNumber)
                    .Select(r => r.ass.QuestionNumber)
                    .ToList();
                questionNumber = sortedNumbers.LastOrDefault() + 1;

                var defaultCorrectOptionLetter = context.SchoolMsAnswerOptionLetters
                    .FirstOrDefault()
                    .Id;

                var assessmentToAdd = new SchoolMsAssessmentQuestion
                {
                    Id = modelInsert.Id,
                    SchoolMsAssessmentGroupId = model.SchoolMsAssessmentGroupId,
                    QuestionNumber = questionNumber,
                    Question = modelInsert.Question,
                    CorrectAnswerResponseText = modelInsert.CorrectAnswerResponseText,
                    CorrectOptionLetterId = defaultCorrectOptionLetter,
                    WrongAnswerResponseText = modelInsert.WrongAnswerResponseText,
                    DateToSend = modelInsert.DateToSend,
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
                //context.SaveChanges();



                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    modelInsert.Id = assessmentToAdd.Id;
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
                    executionResult.Result = model;
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
                            context.SchoolMsClasses,
                            agsb => agsb.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (agsb, cl) => new { agsb, cl }
                        )
                        .Join(
                            context.SchoolMsLevels,
                            agsbcl => agsbcl.agsb.ag.SchoolMsLevelId,
                            l => l.Id,
                            (agsbcl, l) => new { agsbcl, l }
                        )
                        .Where(r => r.agsbcl.agsb.ag.IsOpenEnded == true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.l.LevelName)
                        .ToListAsync();
                    result.Result = levels.Select(
                        r => new SchoolMsLevel { Id = r.l.Id, LevelName = r.l.LevelName }
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
            NeedDataSourceEventArgs args,
            int SchoolMsClassId,
            int GroupId
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
                            context.SchoolMsClasses,
                            agsb => agsb.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (agsb, cl) => new { agsb, cl }
                        )
                        .Where(
                            r =>
                                r.cl.Id == SchoolMsClassId
                                && r.agsb.ag.Id == GroupId
                                && r.agsb.ag.IsOpenEnded == true
                        )
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.agsb.sb.SubjectCode)
                        .ToListAsync();

                    result.Result = subjects.Select(
                        r =>
                            new SchoolMsSubject
                            {
                                Id = r.agsb.sb.Id,
                                SubjectName = r.agsb.sb.SubjectName
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
        > GetClasSubjectTopics(
            NeedDataSourceEventArgs args,
            int SchoolMsClassId,
            int GroupId,
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
                    /* if (string.IsNullOrEmpty(filterValue))
                         return result;*/

                    /* var themes = await context.SchoolMsAssessmentGroups
                                       .Join(context.SchoolMsSubjects, agst => agst.st.SchoolMsSubjectId, sb => sb.Id, (agst, sb) => new { agst, sb })
                                       .Join(context.SchoolMsClasses, agstsb => agstsb.agst.st.SchoolMsClassId, cl => cl.Id, (agstsb, cl) => new { agstsb, cl })
                                       .Where(r => r.cl.Id == SchoolMsClassId && r.agstsb.agst.ag.Id == GroupId && r.agstsb.sb.Id == SchoolMsSubjectId && r.agstsb.agst.ag.IsOpenEnded == true)
                                       .Skip(args.skip)
                                       .Take(args.take)
                                        .OrderBy(r => r.agstsb.agst.ag.Id)
                                         .ToListAsync();

                     result.Result = themes
                     .Select(r => new SchoolMsClassSubjectTopic
                     {
                         Id = r.agstsb.agst.st.Id,
                         TopicName = r.agstsb.agst.st.TopicName

                     });
                */
                }
            }
            catch (Exception ex)
            {
                result = result.CopyFromException(ex);
            }
            return result;
        }

        public async Task<
            ExecutionResult<IEnumerable<SchoolMsAssessmentGroup>>
        > GetAssessmentGroups(NeedDataSourceEventArgs args, int LevelId)
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsAssessmentGroup>>(
                true,
                "",
                new HashSet<SchoolMsAssessmentGroup>()
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

                    var groups = await context.SchoolMsAssessmentGroups
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
                        .Where(r => r.agsbl.l.Id == LevelId && r.agsbl.agsb.ag.IsOpenEnded == true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.agsbl.agsb.ag.Id)
                        .ToListAsync();

                    result.Result = groups.Select(
                        r =>
                            new SchoolMsAssessmentGroup
                            {
                                Id = r.agsbl.agsb.ag.Id,
                                GroupName = r.agsbl.agsb.ag.GroupName,
                                SchoolMsLevelId = r.agsbl.l.Id
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
                    var filterValue = args.GetFilterValue(0).ToLower();
                    ;
                    if (string.IsNullOrEmpty(filterValue))
                        return result;

                    var returnData = await context.SchoolMsAnswerOptionLetters
                        .Where(r => r.OptionLetter.Contains(filterValue))
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

        public override AssessmentViewModel CloneModel(AssessmentViewModel model)
        {
            return new AssessmentViewModel
            {
                Question = model.Question,
                CorrectAnswerResponseText = model.CorrectAnswerResponseText,
                CorrectOptionLetterId = model.CorrectOptionLetter.Id,
                WrongAnswerResponseText = model.WrongAnswerResponseText,
                DateToSend = model.DateToSend,
                SystemTransactionId = 1
            };
        }

        public override async Task<ExecutionResult<AssessmentViewModel>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AssessmentViewModel>(true, "Entry found");
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

        public override async Task<ExecutionResult<AssessmentViewModel>> Update(
            AssessmentViewModel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AssessmentViewModel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                /*  executionResult = await this.IsValidUpdate(model, context);
                  if (executionResult.IsOkay == false)
                      return executionResult;*/
                //if fail return the failed executionn result

                if (model == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The model is null";
                    return executionResult;
                }

                var schoolMsAssessment =
                    await context.SchoolMsAssessmentQuestions.FirstOrDefaultAsync(
                        r => r.Id == model.Id
                    );

                if (schoolMsAssessment == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "The assessment record you are trying to update does not exist";
                    return executionResult;
                }
                //dbInstance is updated in GETITEM in IsValidUpdate

                var assessmentToUpadate =
                    await context.SchoolMsAssessmentQuestions.FirstOrDefaultAsync(
                        r => r.Id == model.Id
                    );

                // assessmentToUpadate.SchoolMsLevelId = model.SchoolMsLevelId;
                //assessmentToUpadate.SchoolMsSubjectId = model.SchoolMsSubjectId;

                assessmentToUpadate.SchoolMsAssessmentGroupId = model.SchoolMsAssessmentGroup.Id;
                assessmentToUpadate.Question = model.Question;
                assessmentToUpadate.CorrectOptionLetterId = model.CorrectOptionLetter.Id;
                assessmentToUpadate.CorrectAnswerResponseText = model.CorrectAnswerResponseText;
                assessmentToUpadate.WrongAnswerResponseText = model.WrongAnswerResponseText;
                assessmentToUpadate.DateToSend = model.DateToSend.ToLocalTime();
                assessmentToUpadate.SystemTransactionId = model.SystemTransactionId;

                if (string.IsNullOrEmpty(assessmentToUpadate.CorrectAnswerResponseText))
                {
                    assessmentToUpadate.CorrectAnswerResponseText = null;
                }
                if (string.IsNullOrEmpty(assessmentToUpadate.WrongAnswerResponseText))
                {
                    assessmentToUpadate.WrongAnswerResponseText = null;
                }
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    assessmentToUpadate.Id = assessmentToUpadate.Id;

                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        assessmentToUpadate.Id,
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

        public override async Task<ExecutionResult<AssessmentViewModel>> Delete(
            AssessmentViewModel model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AssessmentViewModel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var assessmenToDelete = context.SchoolMsAssessmentQuestions
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
                        assag => assag.assag.ass.CorrectOptionLetterId,
                        ol => ol.Id,
                        (assag, ol) => new { assag, ol }
                    )
                    .Join(
                        context.SchoolMsLevels,
                        assagol => assagol.assag.assag.ag.SchoolMsLevelId,
                        l => l.Id,
                        (assagol, l) => new { assagol, l }
                    )
                    .Join(
                        context.SchoolMsClasses,
                        assagoll => assagoll.assagol.assag.assag.ag.SchoolMsClassId,
                        cl => cl.Id,
                        (assagoll, cl) => new { assagoll, cl }
                    )
                    .FirstOrDefault(r => r.assagoll.assagol.assag.assag.ass.Id == model.Id);

                if (assessmenToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        "The assessment you are trying to delete does not exist";
                    return executionResult;
                }

                /* executionResult = await GetItem((int)model.Id, context);
                 if (executionResult.IsOkay == false)
                     return executionResult;*/

                context.SchoolMsAssessmentQuestions.Remove(
                    assessmenToDelete.assagoll.assagol.assag.assag.ass
                );
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";

                    var assessmentViewModel = new AssessmentViewModel
                    {
                        Question = assessmenToDelete.assagoll.assagol.assag.assag.ass.Question,
                        CorrectAnswerResponseText = assessmenToDelete
                            .assagoll
                            .assagol
                            .assag
                            .assag
                            .ass
                            .CorrectAnswerResponseText,
                        CorrectOptionLetterId = assessmenToDelete.assagoll.assagol.ol.Id,
                        CorrectOptionLetter = assessmenToDelete.assagoll.assagol.ol,
                        WrongAnswerResponseText = assessmenToDelete
                            .assagoll
                            .assagol
                            .assag
                            .assag
                            .ass
                            .WrongAnswerResponseText,
                        DateToSend = assessmenToDelete.assagoll.assagol.assag.assag.ass.DateToSend,
                        SystemTransactionId = assessmenToDelete
                            .assagoll
                            .assagol
                            .assag
                            .assag
                            .ass
                            .SystemTransactionId
                    };

                    executionResult.Result = CloneModel(assessmentViewModel);
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
                        .Where(
                            r =>
                                (
                                    filterValueIntId == -1
                                        ? true
                                        : r.assagsbol.assagsb.assag.ass.Id == filterValueIntId
                                )
                                && r.assagsbol.assagsb.assag.ag.IsOpenEnded == true
                        )
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.l.LevelName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AssessmentViewModel
                        {
                            Id = r.assagsbol.assagsb.assag.ass.Id,
                            QuestionNumber = r.assagsbol.assagsb.assag.ass.QuestionNumber,
                            Question = r.assagsbol.assagsb.assag.ass.Question,
                            DateToSend = r.assagsbol.assagsb.assag.ass.DateToSend,
                            CorrectAnswerResponseText = r.assagsbol
                                .assagsb
                                .assag
                                .ass
                                .CorrectAnswerResponseText,
                            WrongAnswerResponseText = r.assagsbol
                                .assagsb
                                .assag
                                .ass
                                .WrongAnswerResponseText,
                            SchoolMsAssessmentGroup = new SchoolMsAssessmentGroup
                            {
                                Id = r.assagsbol.assagsb.assag.ag.Id,
                                SchoolMsSubject = new SchoolMsSubject
                                {
                                    Id = r.assagsbol.assagsb.sb.Id,
                                    SubjectName = r.assagsbol.assagsb.sb.SubjectName,
                                },
                                SchoolMsLevel = new SchoolMsLevel
                                {
                                    Id = r.l.Id,
                                    LevelName = r.l.LevelName,
                                },
                            },
                            CorrectOptionLetter = new SchoolMsAnswerOptionLetter
                            {
                                Id = r.assagsbol.ol.Id,
                                OptionLetter = r.assagsbol.ol.OptionLetter
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

                // var SchoolMsSubjectIdValue = args.GetFilterValuePredicate(r => r.field == "SchoolMsSubjectId", 0);
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
                    executionResult.Message = "Please select Grade";
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
                            assagol => assagol.assagsb.assag.ag.SchoolMsLevelId,
                            l => l.Id,
                            (assagol, l) => new { assagol, l }
                        )
                        .Join(
                            context.SchoolMsClasses,
                            assagoll => assagoll.assagol.assagsb.assag.ag.SchoolMsClassId,
                            cl => cl.Id,
                            (assagoll, cl) => new { assagoll, cl }
                        )
                        .Where(
                            r =>
                                r.assagoll.l.Id == SchoolMsLevelId
                                && r.assagoll.assagol.assagsb.assag.ag.Id
                                    == SchoolMsAssessmentGroupId
                                && r.assagoll.assagol.assagsb.assag.ag.IsOpenEnded == true
                                && r.assagoll.assagol.assagsb.assag.ass.Cancelled == false
                        )
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.assagoll.l.LevelName)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AssessmentViewModel
                        {
                            Id = r.assagoll.assagol.assagsb.assag.ass.Id,
                            QuestionNumber = r.assagoll.assagol.assagsb.assag.ass.QuestionNumber,
                            Question = r.assagoll.assagol.assagsb.assag.ass.Question,
                            DateToSend = r.assagoll.assagol.assagsb.assag.ass.DateToSend,
                            CorrectAnswerResponseText = r.assagoll
                                .assagol
                                .assagsb
                                .assag
                                .ass
                                .CorrectAnswerResponseText,
                            WrongAnswerResponseText = r.assagoll
                                .assagol
                                .assagsb
                                .assag
                                .ass
                                .WrongAnswerResponseText,
                            SchoolMsSubjectId = r.assagoll.assagol.assagsb.sb.Id,
                            SchoolMsLevelId = r.assagoll.l.Id,
                            SchoolMsClassId = r.cl.Id,
                            SchoolMsAssessmentGroupId = r.assagoll.assagol.assagsb.assag.ag.Id,
                            CorrectOptionLetter = new SchoolMsAnswerOptionLetter
                            {
                                Id = r.assagoll.assagol.ol.Id,
                                OptionLetter = r.assagoll.assagol.ol.OptionLetter
                            },
                            SchoolMsLevel = new SchoolMsLevel
                            {
                                Id = r.assagoll.l.Id,
                                LevelName = r.assagoll.l.LevelName,
                            },
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.assagoll.assagol.assagsb.sb.Id,
                                SubjectName = r.assagoll.assagol.assagsb.sb.SubjectName,
                            },
                            SchoolMsAssessmentGroup = new SchoolMsAssessmentGroup
                            {
                                Id = r.assagoll.assagol.assagsb.assag.ag.Id,
                                SchoolMsClass = new SchoolMsClass
                                {
                                    Id = r.cl.Id,
                                    ClassName = r.cl.ClassName
                                },
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

        public override async Task<ExecutionResult<AssessmentViewModel>> IsValidGeneral(
            AssessmentViewModel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<AssessmentViewModel>>(() =>
            {
                var result = new ExecutionResult<AssessmentViewModel>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.Question))
                {
                    result.IsOkay = false;
                    result.Message = "Please supply the question";
                    return result;
                }
                //var classNameInDb = context.SchoolMsAssessmentes.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
                /*  var subjectLevel = context.SchoolMsLevels.FirstOrDefault(r => r.Id == model.SchoolMsAssessmentGroup.SchoolMsLevel.Id);
                  if (subjectLevel == null)
                  {
                      result.IsOkay = false;
                      result.Message = "Please select the subject level";
                      return result;
                  }*/
                /*   var subjects = context.SchoolMsSubjects.FirstOrDefault(r => r.Id == model.SchoolMsAssessmentGroup.SchoolMsClassSubjectTopic.SchoolMsSubject.Id);
                   if (subjects == null)
                   {
                       result.IsOkay = false;
                       result.Message = "Please select subject";
                       return result;
                   }*/
                var assessmentGroup = context.SchoolMsAssessmentGroups.FirstOrDefault(
                    r => r.Id == model.SchoolMsAssessmentGroupId
                );

                if (assessmentGroup == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please select the assessment group";
                    return result;
                }
                var level = context.SchoolMsLevels.FirstOrDefault(
                    r => r.Id == model.SchoolMsLevel.Id
                );

                if (level == null)
                {
                    result.IsOkay = false;
                    result.Message = "Please select the level";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<AssessmentViewModel>> IsValidInsert(
            AssessmentViewModel model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            return result;
        }

        public override async Task<ExecutionResult<AssessmentViewModel>> IsValidUpdate(
            AssessmentViewModel model,
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
