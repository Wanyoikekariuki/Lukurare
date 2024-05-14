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

//namespace LukurareBackend.Repositories.Configuration
//{
//    public class SubjectsRepository : EFDatabaseModelBaseRepository<SchoolMsSubject>
//    {
//        public SubjectsRepository(string currentModule)
//            : base(currentModule) { }

//        public override async Task<ExecutionResult<SchoolMsSubject>> Add(
//            SchoolMsSubject model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsSubject>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidInsert(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed executionn result

//                //insert the okay object to the DB
//                var modelInsert = CloneModel(model);

//                context.SchoolMsSubjects.Add(modelInsert);

//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Create operation was successful";
//                    //return a new instance of the inserted object
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

//        public override SchoolMsSubject CloneModel(SchoolMsSubject model)
//        {
//            return new SchoolMsSubject
//            {
//                Id = model.Id,
//                Abbreviation = model.Abbreviation,
//                SubjectCode = model.SubjectCode,
//                SubjectName = model.SubjectName,
//                //CountryId = model.Country.Id,
//                IsActive = model.IsActive
//            };
//        }

//        public override async Task<ExecutionResult<SchoolMsSubject>> GetItem(
//            int id,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsSubject>(true, "Entry found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                DbInstance = await context.SchoolMsSubjects.FirstOrDefaultAsync(r => r.Id == id);
//                if (DbInstance == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = $"The entry with id {id} supplied does not exist";
//                    return executionResult;
//                }

//                //set the Result of executionResult object to the item found in the database
//                //to avoid issues with serialization always use new
//                executionResult.Result = CloneModel(DbInstance);
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

//        public override async Task<ExecutionResult<SchoolMsSubject>> Update(
//            SchoolMsSubject model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsSubject>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await this.IsValidUpdate(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed executionn result

//                //dbInstance is updated in GETITEM in IsValidUpdate
//                DbInstance.SubjectName = model.SubjectName;
//                DbInstance.SubjectCode = model.SubjectCode;
//                DbInstance.Abbreviation = model.Abbreviation;
//                //DbInstance.CountryId = model.Country.Id;
//                DbInstance.IsActive = model.IsActive;

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

//        public override async Task<ExecutionResult<SchoolMsSubject>> Delete(
//            SchoolMsSubject model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<SchoolMsSubject>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //use get item to check if this ID is valid and incase it is
//                //Get item will set the DbInstance using the current context if Succefully looaded object
//                var subjectToDelete = context.SchoolMsSubjects.FirstOrDefault(
//                    r => r.Id == model.Id
//                );

//                if (subjectToDelete == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = "The subject you are trying to delete does not exist";
//                    return executionResult;
//                }

//                /*executionResult = await GetItem((int)model.Id, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult;*/

//                context.SchoolMsSubjects.Remove(subjectToDelete);
//                if (createContextHere)
//                {
//                    await context.SaveChangesAsync();
//                    executionResult.Message = "Delete operation was successful";
//                    var country = context.Countries.FirstOrDefault(
//                        r => r.Id == subjectToDelete.CountryId
//                    );
//                    subjectToDelete.Country = country;

//                    executionResult.Result = CloneModel(subjectToDelete);

//                    //executionResult.Result = await GetModelForInsertUpdateFromId(subjectToDelete.Id, subjectToDelete, context);
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

//                var classes = (
//                    await context.SchoolMsSubjects
//                        .Join(
//                            context.Countries,
//                            s => s.CountryId,
//                            c => c.Id,
//                            (s, c) => new { s, c }
//                        )
//                        .Where(r => true)
//                        .Skip(0)
//                        .Take(args.take * args.page)
//                        //.OrderBy(r => r.x.aeas.Id)
//                        .ToListAsync()
//                ).Select(
//                    r =>
//                        new SchoolMsSubject
//                        {
//                            Id = r.s.Id,
//                            SubjectCode = r.s.SubjectCode,
//                            SubjectName = r.s.SubjectName,
//                            Abbreviation = r.s.Abbreviation,
//                            IsActive = r.s.IsActive,
//                            Country = new Country { Id = r.c.Id, CountryName = r.c.CountryName }
//                        }
//                );

//                var filterSupporter = new FilterSupporter<SchoolMsSubject>(classes, args);
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

//        public override async Task<ExecutionResult<SchoolMsSubject>> IsValidGeneral(
//            SchoolMsSubject model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            return await Task.Run<ExecutionResult<SchoolMsSubject>>(() =>
//            {
//                var result = new ExecutionResult<SchoolMsSubject>(true);
//                if (model == null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "model is null";
//                    return result;
//                }
//                if (string.IsNullOrEmpty(model.SubjectName))
//                {
//                    result.IsOkay = false;
//                    result.Message = "Please supply the subject name";
//                    return result;
//                }
//                if (string.IsNullOrEmpty(model.SubjectCode))
//                {
//                    result.IsOkay = false;
//                    result.Message = "Please supply the subject code";
//                    return result;
//                }

//                //var classNameInDb = context.SchoolMsSubjectes.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));
//                var subjectInDb = context.SchoolMsSubjects.FirstOrDefault(
//                    r =>
//                        r.Id != model.Id
//                        && r.SubjectName.Equals(model.SubjectName)
//                        && r.CountryId == model.Country.Id
//                );
//                if (subjectInDb != null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "There is a subject with that entered name already";
//                    return result;
//                }
//                var subjectCodeInDb = context.SchoolMsSubjects.FirstOrDefault(
//                    r =>
//                        r.Id != model.Id
//                        && r.SubjectCode.Equals(model.SubjectCode)
//                        && r.CountryId == model.Country.Id
//                );
//                if (subjectCodeInDb != null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "There is a subject with the the entered code already";
//                    return result;
//                }
//                var subjectAbbreviationInDb = context.SchoolMsSubjects.FirstOrDefault(
//                    r =>
//                        r.Id != model.Id
//                        && r.Abbreviation.Equals(model.Abbreviation)
//                        && r.CountryId == model.Country.Id
//                );
//                if (subjectAbbreviationInDb != null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "There is a subject with the the entered abbreation already";
//                    return result;
//                }

//                var countryInDb = context.Countries.FirstOrDefault(r => r.Id == model.Country.Id);
//                if (countryInDb == null)
//                {
//                    result.IsOkay = false;
//                    result.Message = "The supplied country does not exit in the system";
//                    return result;
//                }

//                return result;
//            });
//        }

//        public override async Task<ExecutionResult<SchoolMsSubject>> IsValidInsert(
//            SchoolMsSubject model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            return result;
//        }

//        public override async Task<ExecutionResult<SchoolMsSubject>> IsValidUpdate(
//            SchoolMsSubject model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            //if it passes general validation then use get item to complete update validation
//            var result = await this.IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;
//            /*
//                        result = await GetItem((int)model.Id, context);
//                        if (result.IsOkay == false)
//                            return result;*/
//            DbInstance = await context.SchoolMsSubjects.FirstOrDefaultAsync(r => r.Id == model.Id);
//            if (DbInstance == null)
//            {
//                result.IsOkay = false;
//                result.Message = "The entry with id supplied does not exist";
//                return result;
//            }
//            return result;
//        }

//        #region fill combo boxes
//        public async Task<ExecutionResult<IEnumerable<Country>>> GetCountryCombo(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<Country>>(
//                true,
//                "",
//                new HashSet<Country>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
//                    if (string.IsNullOrEmpty(filterValue))
//                        return result;*/

//                    var returnData = await context.Countries
//                        //.Where(r => r.CountryName.Contains(filterValue))
//                        .Where(r => true)
//                        .OrderBy(r => r.Id)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new Country
//                            {
//                                Id = r.Id,
//                                CountryCode = r.CountryCode,
//                                CountryName = r.CountryName,
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

//        #endregion
//    }
//}
