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
    public class SchoolRepository : EFDatabaseModelBaseRepository<SchoolMsSchool>
    {
        public SchoolRepository(string currentModule)
            : base(currentModule) { }

        public override async Task<ExecutionResult<SchoolMsSchool>> Add(
            SchoolMsSchool model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsSchool>(true);
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

                var subAccounts = new SubAccount
                {
                    AccountNo = model.SubAccount.AccountNo,
                    AccountName = model.SubAccount.AccountName,
                    DateCreated = DateTime.UtcNow,
                    IsActive = model.SubAccount.IsActive,
                    ContactPerson = model.SubAccount.ContactPerson,
                    Email = model.SubAccount.Email,
                    Phone = model.SubAccount.Phone,
                };
                context.SubAccounts.Add(subAccounts);
                await context.SaveChangesAsync();

                modelInsert.SubAccountId = subAccounts.Id;

                //var defaultCountryStructrure = context.CountryStructures.FirstOrDefault();
                modelInsert.SubAccountId = subAccounts.Id;
                modelInsert.CountryStructureId = model.CountryStructure.Id;
                context.SchoolMsSchools.Add(modelInsert);

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

        public async Task<ExecutionResult<IEnumerable<SchoolMsSchool>>> GetSubAccounts(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<SchoolMsSchool>>(
                true,
                "",
                new HashSet<SchoolMsSchool>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    /*var filterValue = args.GetFilterValue(0).ToLower(); ;
                    if (string.IsNullOrEmpty(filterValue))
                        return result;*/



                    var schoolDetails = await context.SubAccounts
                        .Join(
                            context.SchoolMsSchools,
                            sa => sa.Id,
                            sc => sc.SubAccountId,
                            (sa, sc) => new { sa, sc }
                        )
                        .Where(r => true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.sa.Id)
                        .ToListAsync();

                    result.Result = schoolDetails.Select(
                        r =>
                            new SchoolMsSchool
                            {
                                Id = r.sc.Id,
                                SubAccountId = r.sc.SubAccountId,
                                CountryStructureId = r.sc.CountryStructureId,
                                SubAccount = new SubAccount
                                {
                                    Id = r.sa.Id,
                                    AccountName = r.sa.AccountName,
                                    Phone = r.sa.Phone,
                                    AccountNo = r.sa.AccountNo,
                                    Email = r.sa.Email,
                                    DateCreated = r.sa.DateCreated,
                                    ContactPerson = r.sa.ContactPerson,
                                }
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

        public async Task<ExecutionResult<IEnumerable<CountryStructure>>> GetCountryStructure(
            NeedDataSourceEventArgs args
        )
        {
            var result = new ExecutionResult<IEnumerable<CountryStructure>>(
                true,
                "",
                new HashSet<CountryStructure>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
                    /* var filterValue = args.GetFilterValue(0).ToLower(); ;
                     if (string.IsNullOrEmpty(filterValue))
                         return result;*/



                    var schoolDetails = await context.CountryStructureLevels
                        .Join(
                            context.CountryStructures,
                            sl => sl.Id,
                            cs => cs.CountryStructureLevelId,
                            (sl, cs) => new { sl, cs }
                        )
                        .Join(
                            context.Countries,
                            slcs => slcs.sl.CountryId,
                            c => c.Id,
                            (slcs, c) => new { slcs, c }
                        )
                        .Join(
                            context.CountryStructures,
                            slcsc => slcsc.slcs.cs.ParentCountryStructureId,
                            lcs => lcs.Id,
                            (slcsc, lcs) => new { slcsc, lcs }
                        )
                        .Join(
                            context.CountryStructures,
                            slcsclcs => slcsclcs.lcs.ParentCountryStructureId,
                            llcs => llcs.Id,
                            (slcsclcs, llcs) => new { slcsclcs, llcs }
                        )
                        .Where(r => true)
                        .Skip(args.skip)
                        .Take(args.take)
                        .OrderBy(r => r.slcsclcs.slcsc.slcs.sl.Level)
                        .ToListAsync();

                    result.Result = schoolDetails.Select(
                        r =>
                            new CountryStructure
                            {
                                ParentCountryStructure = new CountryStructure
                                {
                                    Id = r.slcsclcs.slcsc.slcs.cs.Id,
                                    StructureName = r.slcsclcs.slcsc.slcs.cs.StructureName,
                                },
                                Id = r.slcsclcs.slcsc.slcs.cs.Id,
                                StructureName = r.slcsclcs.slcsc.slcs.cs.StructureName,
                                CountryStructureLevel = new CountryStructureLevel
                                {
                                    Id = r.slcsclcs.slcsc.slcs.sl.Id,
                                    LevelName = r.slcsclcs.slcsc.slcs.sl.LevelName,
                                    Level = r.slcsclcs.slcsc.slcs.sl.Level,
                                    Country = new Country
                                    {
                                        Id = r.slcsclcs.slcsc.c.Id,
                                        CountryName = r.slcsclcs.slcsc.c.CountryName,
                                    },
                                },
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

        public override SchoolMsSchool CloneModel(SchoolMsSchool model)
        {
            return new SchoolMsSchool { Id = model.Id, };
        }

        public override async Task<ExecutionResult<SchoolMsSchool>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsSchool>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                DbInstance = await context.SchoolMsSchools.FirstOrDefaultAsync(r => r.Id == id);
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

        public override async Task<ExecutionResult<SchoolMsSchool>> Update(
            SchoolMsSchool model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsSchool>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid
                executionResult = await this.IsValidUpdate(model, context);
                if (executionResult.IsOkay == false)
                    return executionResult; //if fail return the failed executionn result

                //dbInstance is updated in GETITEM in IsValidUpdate
                DbInstance.CountryStructureId = model.CountryStructure.Id;
                DbInstance.SubAccountId = model.SubAccount.Id;
                var subAccounts = context.SubAccounts.FirstOrDefault(
                    r => r.Id == model.SubAccount.Id
                );

                subAccounts.AccountNo = model.SubAccount.AccountNo;
                subAccounts.AccountName = model.SubAccount.AccountName;
                subAccounts.DateCreated = model.SubAccount.DateCreated;
                subAccounts.ContactPerson = model.SubAccount.ContactPerson;
                subAccounts.Email = model.SubAccount.Email;
                subAccounts.Phone = model.SubAccount.Phone;
                subAccounts.IsActive = true;

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

        public override async Task<ExecutionResult<SchoolMsSchool>> Delete(
            SchoolMsSchool model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsSchool>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                var classToDelete = context.SchoolMsSchools.FirstOrDefault(r => r.Id == model.Id);

                if (classToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The class you are trying to delete does not exist";
                    return executionResult;
                }

                executionResult = await GetItem((int)model.Id, context);
                if (executionResult.IsOkay == false)
                    return executionResult;

                context.SchoolMsSchools.Remove(DbInstance);
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

                /*var schoolDetails = await context.CountryStructureLevels
                       .Join(context.CountryStructures, sl => sl.Id, cs => cs.CountryStructureLevelId, (sl, cs) => new { sl, cs })
                       .Join(context.Countries, slcs => slcs.cs.CountryId, c => c.Id, (slcs, c) => new { slcs, c })
                       .Join(context.CountryStructures, slcsc => slcsc.slcs.cs.ParentCountryStructureId, lcs => lcs.Id, (slcsc, lcs) => new { slcsc, lcs })
                       .Join(context.CountryStructures, slcsclcs => slcsclcs.lcs.ParentCountryStructureId, llcs => llcs.Id, (slcsclcs, llcs) => new { slcsclcs, llcs })

                       .Where(r => true);*/


                var schoolDetails = (
                    await context.CountryStructureLevels
                        .Join(
                            context.CountryStructures,
                            sl => sl.Id,
                            cs => cs.CountryStructureLevelId,
                            (sl, cs) => new { sl, cs }
                        )
                        .Join(
                            context.Countries,
                            slcs => slcs.sl.CountryId,
                            c => c.Id,
                            (slcs, c) => new { slcs, c }
                        )
                        .Join(
                            context.CountryStructures,
                            slcsc => slcsc.slcs.cs.ParentCountryStructureId,
                            lcs => lcs.Id,
                            (slcsc, lcs) => new { slcsc, lcs }
                        )
                        .Join(
                            context.CountryStructures,
                            slcsclcs => slcsclcs.lcs.ParentCountryStructureId,
                            llcs => llcs.Id,
                            (slcsclcs, llcs) => new { slcsclcs, llcs }
                        )
                        .Join(
                            context.SchoolMsSchools,
                            slcsclcsllcs => slcsclcsllcs.slcsclcs.slcsc.slcs.cs.Id,
                            sch => sch.CountryStructureId,
                            (slcsclcsllcs, sch) => new { slcsclcsllcs, sch }
                        )
                        .Join(
                            context.SubAccounts,
                            slcsclcsllcssch => slcsclcsllcssch.sch.SubAccountId,
                            sa => sa.Id,
                            (slcsclcsllcssch, sa) => new { slcsclcsllcssch, sa }
                        )
                        .Where(r => true)
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.slcsclcsllcssch.slcsclcsllcs.slcsclcs.lcs.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsSchool
                        {
                            Id = r.slcsclcsllcssch.sch.Id,
                            CountryStructure = new CountryStructure
                            {
                                //wasrd leve3
                                Id = r.slcsclcsllcssch.slcsclcsllcs.slcsclcs.slcsc.slcs.cs.Id,
                                StructureName = r.slcsclcsllcssch
                                    .slcsclcsllcs
                                    .slcsclcs
                                    .slcsc
                                    .slcs
                                    .cs
                                    .StructureName,
                                ParentCountryStructure = new CountryStructure
                                {
                                    //sub county leve2
                                    Id = r.slcsclcsllcssch.slcsclcsllcs.slcsclcs.lcs.Id,
                                    StructureName = r.slcsclcsllcssch
                                        .slcsclcsllcs
                                        .slcsclcs
                                        .lcs
                                        .StructureName,
                                    ParentCountryStructure = new CountryStructure
                                    {
                                        //county level 1
                                        Id = r.slcsclcsllcssch.slcsclcsllcs.llcs.Id,
                                        StructureName = r.slcsclcsllcssch
                                            .slcsclcsllcs
                                            .llcs
                                            .StructureName
                                    }
                                }
                            },
                            SubAccount = new SubAccount
                            {
                                Id = r.sa.Id,
                                AccountName = r.sa.AccountName,
                                Phone = r.sa.Phone,
                                AccountNo = r.sa.AccountNo,
                                DateCreated = r.sa.DateCreated,
                                Email = r.sa.Email,
                                ContactPerson = r.sa.ContactPerson,
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsSchool>(schoolDetails, args);
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

        public override async Task<ExecutionResult<SchoolMsSchool>> IsValidGeneral(
            SchoolMsSchool model,
            EFDatabaseModelDatabaseContext context
        )
        {
            return await Task.Run<ExecutionResult<SchoolMsSchool>>(() =>
            {
                var result = new ExecutionResult<SchoolMsSchool>(true);
                if (model == null)
                {
                    result.IsOkay = false;
                    result.Message = "model is null";
                    return result;
                }
                if (string.IsNullOrEmpty(model.SubAccount.AccountName))
                {
                    result.IsOkay = false;
                    result.Message = "Please provide the school name";
                    return result;
                }
                if (string.IsNullOrEmpty(model.SubAccount.Email))
                {
                    result.IsOkay = false;
                    result.Message = "Please provide email";
                    return result;
                }

                if (string.IsNullOrEmpty(model.SubAccount.ContactPerson))
                {
                    result.IsOkay = false;
                    result.Message = "Please provide contact person";
                    return result;
                }
                if (string.IsNullOrEmpty(model.SubAccount.Phone))
                {
                    result.IsOkay = false;
                    result.Message = "Please provide phone number";
                    return result;
                }

                /*var countryStructureInDb = context.CountryStructures.FirstOrDefault(r => r.Id == model.CountryStructureId);
                if (model.CountryStructureId == 0 || countryStructureInDb == null)
               {
                   result.IsOkay = false;
                  result.Message = "Please provide country structure details";
                   return result;
               }*/

                //var classNameInDb = context.SchoolMsSchooles.FirstOrDefault(r =>  string.Equals(model.ClassName, r.ClassName, StringComparison.OrdinalIgnoreCase));                var schoolNameInDb = context.SubAccounts.FirstOrDefault(r => r.AccountName.Equals(model.SubAccount.AccountName) && r.Id != model.SubAccount.Id);
                var schoolNameInDb = context.SubAccounts.FirstOrDefault(
                    r =>
                        r.AccountName.Equals(model.SubAccount.AccountName)
                        && r.Id != model.SubAccount.Id
                );
                if (schoolNameInDb != null)
                {
                    result.IsOkay = false;
                    result.Message = "There is a school with that name already";
                    return result;
                }

                return result;
            });
        }

        public override async Task<ExecutionResult<SchoolMsSchool>> IsValidInsert(
            SchoolMsSchool model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;
            return result;
        }

        public override async Task<ExecutionResult<SchoolMsSchool>> IsValidUpdate(
            SchoolMsSchool model,
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
