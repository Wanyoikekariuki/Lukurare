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
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Assessment
{
    public class AssessmentAnswerOptionsRepository
        : EFDatabaseModelBaseRepository<SchoolMsAssessmentAnswerOption>
    {
        public AssessmentAnswerOptionsRepository(string currentModule)
            : base(currentModule) { }

        public override SchoolMsAssessmentAnswerOption CloneModel(
            SchoolMsAssessmentAnswerOption model
        )
        {
            var Value = new SchoolMsAssessmentAnswerOption { Id = model.Id, };

            return Value;
        }

        public async override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> Add(
            SchoolMsAssessmentAnswerOption model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentAnswerOption>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidInsert(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed execution result

                executionResult.IsOkay = false;
                executionResult.Message = "Just edit the values of the option availa";
                return executionResult;
                //insert the okay object to the DB
                var modelInsert = CloneModel(model);

                // context.AccountEntities.Add(modelInsert.GuardianAccountEntity);

                context.SaveChanges();

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

        public async override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> Delete(
            SchoolMsAssessmentAnswerOption model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentAnswerOption>(true);
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

                context.SchoolMsAssessmentAnswerOptions.Remove(DbInstance);
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

        public async override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentAnswerOption>(
                true,
                "Entry found"
            );
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //var dbJonInstance = await context.AccountEntities
                //    .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
                //    .Join(context.SubAccounts, aesab => aesab.sab.SubAccountId, sa => sa.Id, (aesab, sa) => new { aesab, sa })
                //   .FirstOrDefaultAsync(r => r.aesab.ae.Id == id);
                DbInstance = await context.SchoolMsAssessmentAnswerOptions.FirstOrDefaultAsync(
                    r => r.Id == id
                );
                if (DbInstance == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"The entry with id {id} supplied does not exist";
                    return executionResult;
                }

                //DbInstance = dbJonInstance.aesab.ae;
                //DbInstance.SubAccountBranch = dbJonInstance.aesab.sab;
                //DbInstance.SubAccountBranch.SubAccount = dbJonInstance.sa;

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

        public override async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "SchoolMsAssessmentAnswerOption instance found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                int filterValueIntId = -1;
                string filterValueId = string.Empty;
                if (args.HasFilterFields("SchoolMsAssessmentId"))
                    filterValueId = args.GetFilterValuePredicate(
                        r => r.field == "SchoolMsAssessmentId",
                        0
                    );

                var parsedInt = int.TryParse(filterValueId, out filterValueIntId);
                if (string.IsNullOrEmpty(filterValueId) || parsedInt == false)
                    filterValueIntId = -1;

                var accentity = (
                    await context.SchoolMsAssessmentAnswerOptions
                        .Join(
                            context.SchoolMsAssessmentQuestions,
                            ao => ao.SchoolMsAssessmentId,
                            ass => ass.Id,
                            (ao, ass) => new { ao, ass }
                        )
                        .Join(
                            context.SchoolMsAnswerOptionLetters,
                            aoass => aoass.ao.SchoolMsAnswerOptionLetterId,
                            ol => ol.Id,
                            (aoass, ol) => new { aoass, ol }
                        )
                        .Where(
                            r => filterValueIntId == -1 ? true : r.aoass.ass.Id == filterValueIntId
                        )
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.ol.OptionLetter)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsAssessmentAnswerOption
                        {
                            Id = r.aoass.ao.Id,
                            SchoolMsAssessmentId = r.aoass.ao.SchoolMsAssessmentId,
                            Value = r.aoass.ao.Value,
                            SchoolMsAnswerOptionLetter = new SchoolMsAnswerOptionLetter
                            {
                                Id = r.ol.Id,
                                OptionLetter = r.ol.OptionLetter,
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsAssessmentAnswerOption>(
                    accentity,
                    args
                );
                var filteredData = filterSupporter.FilterData<int>(r => (int)r.Id);
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

        public override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> IsValidGeneral(
            SchoolMsAssessmentAnswerOption model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<SchoolMsAssessmentAnswerOption>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }
            if (string.IsNullOrEmpty(model.Value))
            {
                result.IsOkay = false;
                result.Message = "Please supply the value";
                return Task.FromResult(result);
            }

            /*if (string.IsNullOrEmpty(model.GuardianAccountEntity.Phone1))
            {
                result.IsOkay = false;
                result.Message = "Please supply the Phone Number";
                return Task.FromResult(result);
            }*/



            //SubAccountSelected = await context.SubAccounts.FirstOrDefaultAsync(r => r.Id == model.SubAccountId);
            //if (SubAccountSelected == null)
            //{
            //    result.IsOkay = false;
            //    result.Message = "Please  supply the Sub Account ";
            //    return result;
            //}

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> IsValidInsert(
            SchoolMsAssessmentAnswerOption model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation


            return result;
        }

        public async override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> IsValidUpdate(
            SchoolMsAssessmentAnswerOption model,
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

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<SchoolMsAssessmentAnswerOption>> Update(
            SchoolMsAssessmentAnswerOption model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsAssessmentAnswerOption>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                /* executionResult = await this.IsValidUpdate(model, context);
                 if (executionResult.IsOkay == false)
                     return executionResult;//if fail return the failed executionn result
 */
                var schoolMsAssessment =
                    await context.SchoolMsAssessmentAnswerOptions.FirstOrDefaultAsync(
                        r => r.Id == model.Id
                    );
                if (schoolMsAssessment == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The record you are trying to update does not exist";
                    return executionResult;
                }
                var optionValues = context.SchoolMsAssessmentAnswerOptions
                    .Where(
                        r =>
                            r.Id != model.Id && r.SchoolMsAssessmentId == model.SchoolMsAssessmentId
                    )
                    .ToList();
                foreach (var item in optionValues)
                {
                    if (item.Value.Trim().Equals(model.Value.Trim()))
                    {
                        executionResult.IsOkay = false;
                        executionResult.Message = "The value suppied already exist";
                        return executionResult;
                    }
                }

                #region update the entity
                // DbInstance.GuardianAccountEntity.SubAccountBranchId = model.GuardianAccountEntity.SubAccountBranchId;

                #endregion
                #region update asssesment answer option
                DbInstance = await context.SchoolMsAssessmentAnswerOptions.FirstOrDefaultAsync(
                    r => r.Id == model.Id
                );

                DbInstance.Value = model.Value.Trim();

                //update the transaction id if there is no default value
                var defaultValues = context.SchoolMsAssessmentAnswerOptions.FirstOrDefault(
                    r =>
                        r.Id != model.Id
                        && r.Value.Contains("Default value")
                        && r.SchoolMsAssessmentId == model.SchoolMsAssessmentId
                );

                if (!DbInstance.Value.Contains("Default value") && defaultValues == null)
                {
                    var assessmentQuestionToUpdate =
                        await context.SchoolMsAssessmentQuestions.FirstOrDefaultAsync(
                            r => r.Id == model.SchoolMsAssessmentId
                        );
                    assessmentQuestionToUpdate.SystemTransactionId = 1;
                }

                #endregion

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

        #region fill combo boxes
        public async Task<ExecutionResult<IEnumerable<IdentificationDocumentType>>> GetDocumentType(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<IdentificationDocumentType>>(
                true,
                "",
                new HashSet<IdentificationDocumentType>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    var filterValue = args.GetFilterValue(0).ToLower();
                    ;
                    //if (string.IsNullOrEmpty(filterValue))
                    //    return result;

                    var returnData = await context.IdentificationDocumentTypes
                        .Where(
                            r =>
                                filterValue == null
                                    ? true
                                    : r.DocumentName.ToLower().Contains(filterValue)
                        )
                        .OrderBy(r => r.Id)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new IdentificationDocumentType
                            {
                                Id = r.Id,
                                DocumentName = r.DocumentName
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

        #endregion
    }
}
