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
    public class StudentParentRepository : EFDatabaseModelBaseRepository<SchoolMsStudentGuardian>
    {
        public StudentParentRepository(string currentModule)
            : base(currentModule) { }

        public override SchoolMsStudentGuardian CloneModel(SchoolMsStudentGuardian model)
        {
            var Value = new SchoolMsStudentGuardian
            {
                Id = model.Id,
                SchoolMsStudent = new SchoolMsStudent { Id = model.SchoolMsStudent.Id, },
                SchoolMsStudentParent = new SchoolMsStudentParent
                {
                    Id = model.SchoolMsStudentParent.Id,
                    AccountEntity = new AccountEntity
                    {
                        Id = model.Id,
                        SubAccountBranchId = model
                            .SchoolMsStudentParent
                            .AccountEntity
                            .SubAccountBranchId,
                        EntityName = model.SchoolMsStudentParent.AccountEntity.EntityName,
                        EntityNo = model.SchoolMsStudentParent.AccountEntity.EntityNo,
                        RegDate = model.SchoolMsStudentParent.AccountEntity.RegDate,
                        DateOfBirth = model.SchoolMsStudentParent.AccountEntity.DateOfBirth,
                        Phone1 = model.SchoolMsStudentParent.AccountEntity.Phone1,
                        Email = model.SchoolMsStudentParent.AccountEntity.Email,
                        IsActive = model.SchoolMsStudentParent.AccountEntity.IsActive,
                        PhysicalAddress = model.SchoolMsStudentParent.AccountEntity.PhysicalAddress,
                        PostalAddress = model.SchoolMsStudentParent.AccountEntity.PostalAddress,
                        IsBackUpRecord = model.SchoolMsStudentParent.AccountEntity.IsBackUpRecord,
                        EntityDetailsValidated = model
                            .SchoolMsStudentParent
                            .AccountEntity
                            .EntityDetailsValidated,
                        IdentificationDocumentTypeId = model
                            .SchoolMsStudentParent
                            .AccountEntity
                            .IdentificationDocumentTypeId,
                        AccountEntityActiveState = model
                            .SchoolMsStudentParent
                            .AccountEntity
                            .AccountEntityActiveState
                    }
                }

                // SchoolMsStudent.AccountEntityId = model.SchoolMsStudent.AccountEntity.Id,
            };

            return Value;
        }

        public async override Task<ExecutionResult<SchoolMsStudentGuardian>> Add(
            SchoolMsStudentGuardian model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentGuardian>(true);
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
                Random random = new Random();

                if (string.IsNullOrEmpty(model.SchoolMsStudentParent.AccountEntity.EntityNo))
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.EntityNo =
                        "" + random.Next(1000);
                }
                if (string.IsNullOrEmpty(model.SchoolMsStudentParent.AccountEntity.PhysicalAddress))
                {
                    random = new Random();
                    modelInsert.SchoolMsStudentParent.AccountEntity.PhysicalAddress =
                        "" + random.Next(1000);
                }
                if (model.SchoolMsStudentParent.AccountEntity.RegDate == null)
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.RegDate = DateTime.Now;
                }
                if (model.SchoolMsStudentParent.AccountEntity.DateOfBirth == null)
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.DateOfBirth = DateTime.Now;
                }

                var defaultGender = context.Genders.FirstOrDefault(
                    r => r.Id == modelInsert.SchoolMsStudentParent.AccountEntity.GenderId
                );

                if (defaultGender == null)
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.GenderId = context.Genders
                        .FirstOrDefault()
                        .Id;
                }
                var defaultSubAccountBranch = context.SubAccountBranches.FirstOrDefault(
                    r => r.Id == modelInsert.SchoolMsStudentParent.AccountEntity.SubAccountBranchId
                );

                if (defaultSubAccountBranch == null)
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.SubAccountBranchId =
                        context.SubAccountBranches.FirstOrDefault().Id;
                }

                var defaultIdentificationDoc = context.IdentificationDocumentTypes.FirstOrDefault(
                    r =>
                        r.Id
                        == modelInsert
                            .SchoolMsStudentParent
                            .AccountEntity
                            .IdentificationDocumentTypeId
                );

                if (defaultIdentificationDoc == null)
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.IdentificationDocumentTypeId =
                        context.IdentificationDocumentTypes.FirstOrDefault().Id;
                }
                var defaultAcivityState = context.AccountEntityActiveStates.FirstOrDefault(
                    r =>
                        r.Id
                        == modelInsert
                            .SchoolMsStudentParent
                            .AccountEntity
                            .AccountEntityActiveStateId
                );

                if (defaultAcivityState == null)
                {
                    modelInsert.SchoolMsStudentParent.AccountEntity.AccountEntityActiveStateId =
                        context.AccountEntityActiveStates.FirstOrDefault().Id;
                }

                if (
                    string.IsNullOrEmpty(
                        model.SchoolMsStudentParent.AccountEntity.IdentificationDocumentNumber
                    )
                )
                {
                    random = new Random();
                    modelInsert.SchoolMsStudentParent.AccountEntity.IdentificationDocumentNumber =
                        "23d" + random.Next(1000);
                }

                if (
                    string.IsNullOrEmpty(
                        model.SchoolMsStudentParent.AccountEntity.IdentificationDocumentNumber
                    )
                )
                {
                    random = new Random();
                    modelInsert.SchoolMsStudentParent.AccountEntity.IdentificationDocumentNumber =
                        "23d" + random.Next(1000);
                }

                modelInsert.SchoolMsStudentParent.AccountEntity.IsBackUpRecord = true;
                modelInsert.SchoolMsStudentParent.AccountEntity.EntityDetailsValidated = true;

                modelInsert.SchoolMsStudentParent.AccountEntity.IdentificationDocumentType = null;

                context.AccountEntities.Add(modelInsert.SchoolMsStudentParent.AccountEntity);
                //context.SaveChanges();

                var studentParent = new SchoolMsStudentParent
                {
                    AccountEntity = modelInsert.SchoolMsStudentParent.AccountEntity,
                };

                context.SchoolMsStudentParents.Add(studentParent);

                //context.SaveChanges();
                var studentGuardian = new SchoolMsStudentGuardian
                {
                    SchoolMsStudentId = modelInsert.SchoolMsStudent.Id,
                    SchoolMsStudentParent = studentParent,
                    IsPrimaryContact = true,
                };

                //prevent duplicating student guardian number for students sharing a parent
                var studentInGuardianTable = context.SchoolMsStudentGuardians.FirstOrDefault(
                    r => r.SchoolMsStudentId == modelInsert.SchoolMsStudent.Id
                );
                random = new Random();

                var studentGuardianNumber = studentGuardian.StudentGuardianNumber =
                    "" + random.Next(1000);
                studentGuardian.StudentGuardianNumber = studentGuardianNumber;

                if (studentInGuardianTable != null)
                {
                    if (!string.IsNullOrEmpty(studentInGuardianTable.StudentGuardianNumber))
                    {
                        studentGuardian.StudentGuardianNumber =
                            studentInGuardianTable.StudentGuardianNumber;

                        var phoneNumberInDB = context.AccountEntities
                            .Join(
                                context.SchoolMsStudentGuardians,
                                ae => ae.Id,
                                sg => sg.Id,
                                (ae, sg) => new { ae, sg }
                            )
                            .FirstOrDefault(
                                r =>
                                    r.ae.Phone1.Equals(
                                        modelInsert.SchoolMsStudentParent.AccountEntity.Phone1
                                    )
                            );

                        var studentParentNumber = context.SchoolMsStudentGuardians.FirstOrDefault(
                            r =>
                                r.StudentGuardianNumber.Equals(
                                    studentGuardian.StudentGuardianNumber
                                )
                        );
                        if (phoneNumberInDB != null && studentParentNumber != null)
                        {
                            var studentGuardianDetails = context.SchoolMsStudentGuardians
                                .Where(r => r.SchoolMsStudentId == modelInsert.SchoolMsStudent.Id)
                                .ToList();

                            foreach (var item in studentGuardianDetails)
                            {
                                item.StudentGuardianNumber = studentGuardianNumber;
                            }
                        }
                    }

                    if (studentGuardian.IsPrimaryContact)
                    {
                        var studentGuardianDetails = context.SchoolMsStudentGuardians
                            .Where(r => r.SchoolMsStudentId == modelInsert.SchoolMsStudent.Id)
                            .ToList();

                        foreach (var item in studentGuardianDetails)
                        {
                            item.IsPrimaryContact = false;
                        }
                    }
                }

                context.SchoolMsStudentGuardians.Add(studentGuardian);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    //modelInsert. SchoolMsStudentId = modelInsert.SchoolMsStudent.Id;

                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        studentGuardian.Id,
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

        public async override Task<ExecutionResult<SchoolMsStudentGuardian>> Delete(
            SchoolMsStudentGuardian model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentGuardian>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //use get item to check if this ID is valid and incase it is
                //Get item will set the DbInstance using the current context if Succefully looaded object
                /*  executionResult = await IsValidGeneral(model, context);
                  if (executionResult.IsOkay == false)
                      return executionResult;

                  executionResult = await GetItem((int)model.Id, context);
                  if (executionResult.IsOkay == false)
                      return executionResult;*/


                var guradianToDelete = context.SchoolMsStudentGuardians
                    .Join(
                        context.SchoolMsStudentParents,
                        sg => sg.SchoolMsStudentParentId,
                        sp => sp.Id,
                        (sg, sp) => new { sg, sp }
                    )
                    .Join(
                        context.SchoolMsStudents,
                        sgsp => sgsp.sg.SchoolMsStudentId,
                        st => st.Id,
                        (sgsp, st) => new { sgsp, st }
                    )
                    .Join(
                        context.SchoolMsStudentClassHistories,
                        sgspst => sgspst.st.Id,
                        ch => ch.SchoolMsStudentId,
                        (sgspst, ch) => new { sgspst, ch }
                    )
                    .Join(
                        context.AccountEntities,
                        sgspstch => sgspstch.sgspst.sgsp.sp.AccountEntityId,
                        ae => ae.Id,
                        (aesgch, ae) => new { aesgch, ae }
                    )
                    .FirstOrDefault(r => r.aesgch.sgspst.sgsp.sg.Id == model.Id);

                if (guradianToDelete == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The record you are tring to delete does not exist";
                    return executionResult;
                }

                context.SchoolMsStudentGuardians.Remove(guradianToDelete.aesgch.sgspst.sgsp.sg);
                context.SchoolMsStudentParents.Remove(guradianToDelete.aesgch.sgspst.sgsp.sp);
                context.AccountEntities.Remove(guradianToDelete.ae);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = CloneModel(guradianToDelete.aesgch.sgspst.sgsp.sg);
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

        public async override Task<ExecutionResult<SchoolMsStudentGuardian>> GetItem(
            int id,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentGuardian>(true, "Entry found");
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //var dbJonInstance = await context.AccountEntities
                //    .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
                //    .Join(context.SubAccounts, aesab => aesab.sab.SubAccountId, sa => sa.Id, (aesab, sa) => new { aesab, sa })
                //   .FirstOrDefaultAsync(r => r.aesab.ae.Id == id);
                DbInstance = await context.SchoolMsStudentGuardians.FirstOrDefaultAsync(
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
            >(true, "SchoolMsStudentGuardian instance found");
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

                var accentity = (
                    await context.SchoolMsStudentGuardians
                        .Join(
                            context.SchoolMsStudentParents,
                            sg => sg.SchoolMsStudentParentId,
                            sp => sp.Id,
                            (sg, sp) => new { sg, sp }
                        )
                        .Join(
                            context.SchoolMsStudents,
                            aesg => aesg.sg.SchoolMsStudentId,
                            st => st.Id,
                            (aesg, st) => new { aesg, st }
                        )
                        .Join(
                            context.AccountEntities,
                            aesgst => aesgst.aesg.sp.AccountEntityId,
                            ae => ae.Id,
                            (aesgst, ae) => new { aesgst, ae }
                        )
                        .Where(
                            r =>
                                filterValueIntId == -1
                                    ? true
                                    : r.aesgst.aesg.sg.SchoolMsStudentId == filterValueIntId
                        )
                        .Skip(0)
                        .Take(args.take * args.page)
                        .OrderBy(r => r.aesgst.aesg.sg.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new SchoolMsStudentGuardian
                        {
                            Id = r.aesgst.aesg.sg.Id,
                            SchoolMsStudentId = r.aesgst.aesg.sg.SchoolMsStudentId,
                            SchoolMsStudent = new SchoolMsStudent { Id = r.aesgst.st.Id },
                            SchoolMsStudentParent = new SchoolMsStudentParent
                            {
                                AccountEntity = new AccountEntity
                                {
                                    Id = r.ae.Id,
                                    SubAccountBranchId = r.ae.SubAccountBranchId,
                                    EntityNo = r.ae.EntityNo,
                                    EntityName = r.ae.EntityName,
                                    RegDate = r.ae.RegDate,
                                    DateOfBirth = r.ae.DateOfBirth,
                                    Phone1 = r.ae.Phone1,
                                    Phone2 = r.ae.Phone2,
                                    Email = r.ae.Email,
                                    IsActive = r.ae.IsActive,
                                    PhysicalAddress = r.ae.PhysicalAddress,
                                    PostalAddress = r.ae.PostalAddress,
                                    WhiteListedDomain = r.ae.WhiteListedDomain,
                                    EntityUserName = r.ae.EntityUserName,
                                    IsBackUpRecord = r.ae.IsBackUpRecord,
                                    EntityDetailsValidated = r.ae.EntityDetailsValidated,
                                    IdentificationDocumentNumber =
                                        r.ae.IdentificationDocumentNumber,
                                    IdentificationDocumentTypeId =
                                        r.ae.IdentificationDocumentTypeId,
                                }
                            }
                        }
                );

                var filterSupporter = new FilterSupporter<SchoolMsStudentGuardian>(accentity, args);
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

        public override Task<ExecutionResult<SchoolMsStudentGuardian>> IsValidGeneral(
            SchoolMsStudentGuardian model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

            var result = new ExecutionResult<SchoolMsStudentGuardian>(true);
            if (model == null)
            {
                result.IsOkay = false;
                result.Message = "model is null";
                return Task.FromResult(result);
            }
            if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.EntityName))
            {
                result.IsOkay = false;
                result.Message = "Please supply the Entity Name";
                return Task.FromResult(result);
            }
            /*if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.PhysicalAddress))
            {
                result.IsOkay = false;
                result.Message = "Please supply the Physical address";
                return Task.FromResult(result);
            }*/

            if (string.IsNullOrEmpty(model.SchoolMsStudent.AccountEntity.Phone1))
            {
                result.IsOkay = false;
                result.Message = "Please supply the Phone Number";
                return Task.FromResult(result);
            }

            //SubAccountSelected = await context.SubAccounts.FirstOrDefaultAsync(r => r.Id == model.SubAccountId);
            //if (SubAccountSelected == null)
            //{
            //    result.IsOkay = false;
            //    result.Message = "Please  supply the Sub Account ";
            //    return result;
            //}

            return Task.FromResult(result);
        }

        public async override Task<ExecutionResult<SchoolMsStudentGuardian>> IsValidInsert(
            SchoolMsStudentGuardian model,
            EFDatabaseModelDatabaseContext context
        )
        {
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            //perform insert specific validation


            return result;
        }

        public async override Task<ExecutionResult<SchoolMsStudentGuardian>> IsValidUpdate(
            SchoolMsStudentGuardian model,
            EFDatabaseModelDatabaseContext context
        )
        {
            //if it passes general validation then use get item to complete update validation
            var result = await this.IsValidGeneral(model, context);
            if (result.IsOkay == false)
                return result;

            /*result = await GetItem((int)model.Id, context);
            if (result.IsOkay == false)
                return result;*/
            DbInstance = await context.SchoolMsStudentGuardians.FirstOrDefaultAsync(
                r => r.Id == model.Id
            );
            if (DbInstance == null)
            {
                result.IsOkay = false;
                result.Message = "The entry supplied does not exist";
                return result;
            }

            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
            return result;
        }

        public async override Task<ExecutionResult<SchoolMsStudentGuardian>> Update(
            SchoolMsStudentGuardian model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<SchoolMsStudentGuardian>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();
                //validate wether the supplied model object is valid
                /*   executionResult = await this.IsValidUpdate(model, context);
                   if (executionResult.IsOkay == false)
                       return executionResult*/
                ;

                //dbInstance is set in GETITEM in IsValidUpdate

                #region update the entity
                var parentDetailsToUpdate = context.SchoolMsStudentGuardians
                    .Join(
                        context.SchoolMsStudentParents,
                        sg => sg.SchoolMsStudentParentId,
                        sp => sp.Id,
                        (sg, sp) => new { sg, sp }
                    )
                    .Join(
                        context.SchoolMsStudents,
                        sgsp => sgsp.sg.SchoolMsStudentId,
                        st => st.Id,
                        (sgsp, st) => new { sgsp, st }
                    )
                    .Join(
                        context.SchoolMsStudentClassHistories,
                        sgspst => sgspst.st.Id,
                        ch => ch.SchoolMsStudentId,
                        (sgspst, ch) => new { sgspst, ch }
                    )
                    .Join(
                        context.AccountEntities,
                        sgspstch => sgspstch.sgspst.sgsp.sp.AccountEntityId,
                        ae => ae.Id,
                        (aesgch, ae) => new { aesgch, ae }
                    )
                    .FirstOrDefault(r => r.aesgch.sgspst.sgsp.sg.Id == model.Id);

                if (parentDetailsToUpdate == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "The record you are trying to update does not exist";
                    return executionResult;
                }

                parentDetailsToUpdate.ae.SubAccountBranchId = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .SubAccountBranchId;
                parentDetailsToUpdate.ae.EntityNo = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .EntityNo;
                parentDetailsToUpdate.ae.EntityName = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .EntityName;
                parentDetailsToUpdate.ae.EntityUserName = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .EntityName;
                parentDetailsToUpdate.ae.RegDate = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .RegDate;
                parentDetailsToUpdate.ae.DateOfBirth = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .DateOfBirth;
                parentDetailsToUpdate.ae.Phone1 = model.SchoolMsStudentParent.AccountEntity.Phone1;
                parentDetailsToUpdate.ae.Phone2 = model.SchoolMsStudentParent.AccountEntity.Phone2;
                parentDetailsToUpdate.ae.Email = model.SchoolMsStudentParent.AccountEntity.Email;
                parentDetailsToUpdate.ae.IsActive = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .IsActive;
                parentDetailsToUpdate.ae.PhysicalAddress = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .PhysicalAddress;
                parentDetailsToUpdate.ae.PostalAddress = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .PostalAddress;
                parentDetailsToUpdate.ae.WhiteListedDomain = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .WhiteListedDomain;
                parentDetailsToUpdate.ae.IsBackUpRecord = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .IsBackUpRecord;
                parentDetailsToUpdate.ae.EntityDetailsValidated = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .EntityDetailsValidated;
                parentDetailsToUpdate.ae.IdentificationDocumentNumber = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .IdentificationDocumentNumber;
                parentDetailsToUpdate.ae.IdentificationDocumentTypeId = model
                    .SchoolMsStudentParent
                    .AccountEntity
                    .IdentificationDocumentTypeId;

                #endregion
                #region update the branch

                //DbInstance.SubAccountBranch.Phone1 = model.SubAccountBranch.Phone1;
                //DbInstance.SubAccountBranch.Phone2 = model.Phone2;
                //DbInstance.SubAccountBranch.BuildingAddress = model.PhysicalAddress;
                //DbInstance.SubAccountBranch.PostalAddress = model.PostalAddress;
                //DbInstance.SubAccountBranch.BranchName = model.EntityName;
                //DbInstance.SubAccountBranch.BranchCode = model.EntityNo;

                //#endregion
                //#region update the account

                //DbInstance.SubAccountBranch.SubAccount.Phone = model.Phone1;
                //DbInstance.SubAccountBranch.SubAccount.AccountName = model.EntityName;
                //DbInstance.SubAccountBranch.SubAccount.AccountNo = model.EntityNo;
                //DbInstance.SubAccountBranch.SubAccount.DateCreated = model.RegDate;
                //DbInstance.SubAccountBranch.SubAccount.Email = model.Email;
                //DbInstance.SubAccountBranch.SubAccount.ContactPerson = model.EntityUserName;
                //DbInstance.SubAccountBranch.SubAccount.IsActive = model.IsActive;
                #endregion
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    //executionResult.Result = CloneModel(DbInstance);
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        model.Id,
                        model,
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
