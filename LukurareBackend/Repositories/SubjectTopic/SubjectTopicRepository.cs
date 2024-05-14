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

namespace LukurareBackend.Repositories.SubjectTopic
{
    public class SubjectTopicRepository : EFDatabaseModelBaseRepository<SchoolMsClassSubjectTopic>
    {
        public SubjectTopicRepository(string currentModule)
            : base(currentModule) { }

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
                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
                    if (string.IsNullOrEmpty(filterValue))
                        return result;
*/
                    var returnData = await context.SchoolMsSubjects
                        .Join(
                            context.Countries,
                            sb => sb.CountryId,
                            c => c.Id,
                            (sb, c) => new { sb, c }
                        )
                        .Where(r => true)
                        .OrderBy(r => r.sb.SubjectCode)
                        .Skip(args.skip)
                        .Take(args.take)
                        .ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new SchoolMsSubject
                            {
                                Id = r.sb.Id,
                                SubjectName = r.sb.SubjectName,
                                SubjectCode = r.sb.SubjectCode,
                                Abbreviation = r.sb.Abbreviation,
                                //Country = new Country { Id = r.c.Id, CountryName = r.c.CountryName }
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

        public override SchoolMsClassSubjectTopic CloneModel(SchoolMsClassSubjectTopic model)
        {
            var Value = new SchoolMsClassSubjectTopic
            {
                Id = model.Id,
                Active = model.Active,
                DateCreated = model.DateCreated,
                //SchoolMsClassId = model.SchoolMsClass.Id,
                SchoolMsSubjectId = model.SchoolMsSubject.Id,
                TopicName = model.TopicName.Trim()
            };

            return Value;
        }

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
                var defaultGrade = context.SchoolMsClasses.FirstOrDefault().Id;
                modelInsert.SchoolMsClassId = defaultGrade;
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

        public async override Task<ExecutionResult<SchoolMsClassSubjectTopic>> Delete(
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
                /*
                                executionResult = await GetItem((int)model.Id, context);
                                if (executionResult.IsOkay == false)
                                    return executionResult;*/


                var themeTodelete = context.SchoolMsClassSubjectTopics
                    .Join(
                        context.SchoolMsClasses,
                        st => st.SchoolMsClassId,
                        cl => cl.Id,
                        (st, cl) => new { st, cl }
                    )
                    .Join(
                        context.SchoolMsSubjects,
                        stcl => stcl.st.SchoolMsSubjectId,
                        sb => sb.Id,
                        (stcl, sb) => new { stcl, sb }
                    )
                    .FirstOrDefault(r => r.stcl.st.Id == model.Id);
                if (themeTodelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The theme you are trying to delete does not exist";
                    return executionResult;
                }

                context.SchoolMsClassSubjectTopics.Remove(themeTodelete.stcl.st);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = CloneModel(themeTodelete.stcl.st);
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

        public async override Task<ExecutionResult<SchoolMsClassSubjectTopic>> GetItem(
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

                //var dbJonInstance = await context.AccountEntities
                //    .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
                //    .Join(context.SubAccounts, aesab => aesab.sab.SubAccountId, sa => sa.Id, (aesab, sa) => new { aesab, sa })
                //   .FirstOrDefaultAsync(r => r.aesab.ae.Id == id);
                DbInstance = await context.SchoolMsClassSubjectTopics.FirstOrDefaultAsync(
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

        public async override Task<
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
            >(true, "SchoolMsClassSubjectTopic instance found");
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

                var accentity = (
                    await context.SchoolMsClassSubjectTopics
                        .Join(
                            context.SchoolMsClasses,
                            st => st.SchoolMsClassId,
                            cl => cl.Id,
                            (st, cl) => new { st, cl }
                        )
                        .Join(
                            context.SchoolMsSubjects,
                            stcl => stcl.st.SchoolMsSubjectId,
                            sb => sb.Id,
                            (stcl, sb) => new { stcl, sb }
                        )
                        //   .Where(r => filterValueIntId == -1 ? true : r.ch.Id == filterValueIntId)
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.stcl.st.DateCreated)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsClassSubjectTopic
                        {
                            Id = r.stcl.st.Id,
                            Active = r.stcl.st.Active,
                            DateCreated = r.stcl.st.DateCreated,
                            TopicName = r.stcl.st.TopicName,
                            SchoolMsClass = new SchoolMsClass
                            {
                                Id = r.stcl.cl.Id,
                                ClassName = r.stcl.cl.ClassName
                            },
                            SchoolMsSubject = new SchoolMsSubject
                            {
                                Id = r.sb.Id,
                                SubjectCode = r.sb.SubjectCode,
                                SubjectName = r.sb.SubjectName
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsClassSubjectTopic>(
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

        public override Task<ExecutionResult<SchoolMsClassSubjectTopic>> IsValidGeneral(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<SchoolMsClassSubjectTopic>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }
            var themeInDb = context.SchoolMsClassSubjectTopics.FirstOrDefault(
                r =>
                    r.Id != model.Id
                    && r.SchoolMsSubjectId == model.SchoolMsSubject.Id
                    && r.TopicName.Equals(model.TopicName.Trim())
            );
            if (themeInDb != null)
            {
                result.IsOkay = false;
                result.Message = "There is a theme with the name you are trying to add";
                return Task.FromResult(result);
            }

            if (string.IsNullOrEmpty(model.TopicName))
            {
                result.IsOkay = false;
                result.Message = "Please supply the topic Name";
                return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<SchoolMsClassSubjectTopic>> IsValidInsert(
            SchoolMsClassSubjectTopic model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation


            return result;
        }

        public async override Task<ExecutionResult<SchoolMsClassSubjectTopic>> IsValidUpdate(
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

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<SchoolMsClassSubjectTopic>> Update(
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
                //executionResult = await this.IsValidUpdate(model, context);
                DbInstance = await context.SchoolMsClassSubjectTopics.FirstOrDefaultAsync(
                    r => r.Id == model.Id
                );
                if (DbInstance == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = $"The entry with id supplied does not exist";
                    return executionResult;
                }
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is set in GETITEM in IsValidUpdate

                #region update the entity



                DbInstance.Active = model.Active;
                DbInstance.DateCreated = model.DateCreated;
                DbInstance.TopicName = model.TopicName.Trim();
                DbInstance.SchoolMsClassId = model.SchoolMsClass.Id;
                DbInstance.SchoolMsSubjectId = model.SchoolMsSubject.Id;

                #endregion

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
