using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using LukurareBackend.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LukurareBackend.Models.DashBoard;

namespace LukurareBackend.Repositories.Accounts
{
    public class AccountProfileRepository : PatnerAccountEntityRepository
    {
        public AccountProfileRepository(string currentModule)
            : base(currentModule) { }

        /// <summary>
        /// Helps to create a copy of the current Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async override Task<ExecutionResult<AccountEntity>> Add(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var code = ProjectBase.Security.EncryptionHelper
                    .GetRandomID(DateTime.UtcNow.Ticks, 0, 1000)
                    .ToString();

                //var doctypes = await context.IdentificationDocumentTypes.FirstOrDefaultAsync(r => r.DocumentName == DefaultConfiguration.IdentificationDocumentType.MobilePhoneDocumentName);
                //if (doctypes == null)
                //{
                //    executionResult.IsOkay = false;
                //    executionResult.Message = $"The Identification document type with name { DefaultConfiguration.IdentificationDocumentType.MobilePhoneDocumentName} has not been defined";
                //    return executionResult;
                //}
                var activestates = await context.AccountEntityActiveStates.FirstOrDefaultAsync(
                    r =>
                        r.StateName
                        == DefaultConfiguration.AccountActiveState.ActiveAccountStateName
                );
                if (activestates == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"The Account entity state type with name {DefaultConfiguration.IdentificationDocumentType.MobilePhoneDocumentName} has not been defined";
                    return executionResult;
                }
                var modelInsert = new AccountEntity
                {
                    EntityName = model.EntityName,
                    RegDate = DateTime.UtcNow,
                    DateOfBirth = DateTime.UtcNow,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    EntityNo = code,
                    Email = model.Email,
                    IsActive = true,
                    PhysicalAddress = model.PhysicalAddress,
                    PostalAddress = model.PostalAddress,
                    EntityUserName = model.EntityUserName,
                    IsBackUpRecord = false,
                    EntityDetailsValidated = false,
                    IdentificationDocumentNumber = model.IdentificationDocumentNumber,
                    IdentificationDocumentTypeId = model.IdentificationDocumentType.Id,
                    AccountEntityActiveStateId = activestates.Id
                };

                executionResult = await base.Add(modelInsert, context);
                var employee = new Employee
                {
                    EmployeeNumber = code,
                    AccountEntity = executionResult.Result
                };

                context.Employees.Add(employee);

                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Create operation was successful";
                    executionResult.Result = await GetModelForInsertUpdateFromId(
                        executionResult.Result.Id,
                        executionResult.Result,
                        context
                    );
                }
                else
                {
                    executionResult.Result = modelInsert;
                }
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
                var excetion = ex.ToString();
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                if (createContextHere)
                    await context.DisposeAsync();
            }
            return executionResult;
        }

        public async override Task<ExecutionResult<AccountEntity>> Delete(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var result = base.Delete(model);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Delete operation was successful";
                    executionResult.Result = CloneModel(DbInstance);
                }
                else
                {
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
            >(true, "AccountEntity instance found");
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
                    await context.AccountEntities
                        // .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, idt => idt.Id, (ae, idt) => new { ae, idt })
                        //.Join(context.SubAccounts, x => x.idt.SubAccountId, s => s.Id, (x, s) => new { x, s })
                        //.Join(context.Users, ae => ae.Id, u => u.AccountEntityId, (ae, u) => new { ae, u })
                        .Where(r => r.Id == UserPrinciple.User.AccountEntityId) //r=>r==true
                        .OrderByDescending(r => r.IsActive)
                        .Skip(0)
                        .Take(args.take) // * args.page
                        .OrderBy(r => r.Id)
                        .ToListAsync()
                );
                //.Select(r => new AccountEntity
                //{
                //    Id = r.x.ae.Id,
                //    Phone1 = r.x.ae.Phone1,
                //    Email = r.x.ae.Email,
                //    IsActive = r.x.ae.IsActive,
                //    EntityUserName = r.x.ae.EntityUserName,
                //    EntityName = r.x.ae.EntityName

                //});

                var filterSupporter = new FilterSupporter<AccountEntity>(accentity, args); //accentity
                var filteredData = filterSupporter.FilterData<int>(r => (int)r.Id);
                executionResult.Result = filterSupporter.itemsResult;
                //executionResult.Result = entity;
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

        public async override Task<ExecutionResult<AccountEntity>> Update(
            AccountEntity model,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                //validate wether the supplied model object is valid

                //if fail return the failed executionn result

                //dbInstance is set in GETITEM in IsValidUpdate

                #region update the entity
                var updateModel = new AccountEntity
                {
                    EntityName = model.EntityUserName,
                    EntityNo = "123",
                    EntityUserName = model.EntityUserName,
                    Phone1 = model.Phone1,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    Id = model.Id
                };
                executionResult = await base.Update(model);

                if (executionResult.IsOkay == false)
                    return executionResult;
                if (createContextHere)
                {
                    await context.SaveChangesAsync();
                    executionResult.Message = "Update operation was successful";
                    //executionResult.Result = CloneModel(DbInstance);
                    //executionResult.Result = await GetModelForInsertUpdateFromId(executionResult.Result.Id, executionResult.Result, context);
                }
                else
                {
                    //incase the context was not created here return the object
                    //thats removed from the context
                    //helps the caller method to have log of the object in ChangeTraking inside DbContext
                    executionResult.Result = updateModel;
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

        public async Task<ExecutionResult<IEnumerable<IdentificationDocumentType>>> GetDocuments(
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
                    var returnData = await context.IdentificationDocumentTypes
                        .Where(r => true)
                        .OrderBy(r => r.Id)
                        .Skip(0)
                        .Take(10)
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

        public async Task<AccountEntity> GetParentDetails()
        {
            //var result = new ExecutionResult<AccountEntity>(true);
            var result = new AccountEntity();
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    var entity = await context.AccountEntities.FirstOrDefaultAsync(
                        r => r.Id == UserPrinciple.User.AccountEntityId
                    );
                    var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                        r => r.Id == entity.SubAccountBranchId
                    );
                    var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                        r => r.Id == branchi.SubAccountId
                    );

                    var accentity = (
                        await context.AccountEntities
                            .Join(
                                context.SubAccountBranches,
                                ae => ae.SubAccountBranchId,
                                idt => idt.Id,
                                (ae, idt) => new { ae, idt }
                            )
                            .Join(
                                context.SubAccounts,
                                x => x.idt.SubAccountId,
                                s => s.Id,
                                (x, s) => new { x, s }
                            )
                            //.Join(context.Users, ae => ae.Id, u => u.AccountEntityId, (ae, u) => new { ae, u })
                            .Where(r => r.s.Id == subacc.ParentId) //r=>r==true
                            .OrderByDescending(r => r.x.ae.IsActive)
                            .Skip(0)
                            .Take(10) // * args.page
                            .OrderBy(r => r.x.ae.Id)
                            .ToListAsync()
                    ).Select(
                        r =>
                            new AccountEntity
                            {
                                Id = r.x.ae.Id,
                                Phone1 = r.x.ae.Phone1,
                                Email = r.x.ae.Email,
                                IsActive = r.x.ae.IsActive,
                                EntityUserName = r.x.ae.EntityUserName,
                                EntityName = r.x.ae.EntityName,
                                ProfileImageUrl = r.x.ae.ProfileImageUrl
                            }
                    );

                    //var filterSupporter = new FilterSupporter<AccountEntity>(accentity, args);
                    //var filteredData = filterSupporter.FilterData<int>(r => (int)r.Id);
                    result = accentity.FirstOrDefault(); //filterSupporter.itemsResult;
                }
            }
            catch (Exception ex)
            {
                //result = result.CopyFromException(ex);
            }
            return result;
        }

        private bool CheckFileExistsInDatabase(string fileName)
        {
            // Query the database to check if the file with the given name already exists
            EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext();
            using (context)
            {
                return context.AccountEntitySecondaryIdentificationDocuments.Any(a => a.Path == $"/UploadedFiles/{fileName}");
            }
        }

        public async Task<ExecutionResult<AccountEntity>> UploadAccounts(
            IFormFileCollection files,
            string path
        )
        {
            var executionResult = new ExecutionResult<AccountEntity>(true);

            try
            {
                long uploaded_size = 0;

                string path_for_Uploaded_Files = $"{path}/UploadedFiles/";

                //</ init >

                // Initialize a list to keep track of uploaded file names
                //List<string> uploadedFileNames = new List<string>();

                //< get form_files >

                //IFormFile uploaded_File

                var uploaded_files = files;

                //</ get form_files >



                //------< @Loop: Uploaded Files >------

                int iCounter = 0;

                string sFiles_uploaded = "";

                string uploaded_Filename = "";
                string new_Filename_on_Server = "";

                foreach (var uploaded_file in uploaded_files)
                {
                    //----< Uploaded File >----

                    iCounter++;

                    uploaded_size += uploaded_file.Length;

                    sFiles_uploaded += "\n" + uploaded_file.FileName;

                    //< Filename >

                    uploaded_Filename = uploaded_file.FileName;

                    //string uploadedFileName = uploaded_file.FileName;

                    // Check if the file with the same name is already uploaded
                    if (CheckFileExistsInDatabase(uploaded_Filename))
                    {
                        executionResult.Message = $"File '{uploaded_Filename}' already uploaded.";
                        return executionResult;
                    }


                    new_Filename_on_Server = $"{path_for_Uploaded_Files}{uploaded_Filename}";

                    //</ Filename >



                    //< Copy File to Target >

                    using (
                        FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create)
                    )
                    {
                        await uploaded_file.CopyToAsync(stream);
                        EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext();
                        using (context)
                        {
                            var accountEntity = context.AccountEntities.FirstOrDefault(r => r.Id == UserPrinciple.User.AccountEntityId);
                            accountEntity.ProfileImageUrl = $"/UploadedFiles/{uploaded_Filename}";
                            context.AccountEntities.Update(accountEntity);
                            context.SaveChanges();
                        }
                        executionResult.Message = "Image uploaded successfully"; // to    /UploadedFiles/{uploaded_Filename}
                    }
                }

                //
            }
            catch (Exception ex) {
                executionResult.Message = ex.Message;

            }

            return executionResult;
        }

        public async Task<BalanceModel> GetBalance()
        {
            var result = new BalanceModel();

            //var result = new ExecutionResult<IEnumerable<IdentificationDocumentType>>(true, "", new HashSet<IdentificationDocumentType>());

            using (var context = new EFDatabaseModelDatabaseContext())
            {
                #region remove
                try
                {
                    var entity = await context.MfsEntityAccounts.FirstOrDefaultAsync(
                        r => r.AccountEntityId == UserPrinciple.User.AccountEntityId
                    );
                    var resultBalance = await context.MfsEntityAccountTransactions
                        .Where(r => r.Posted == true && r.MfsEntityAccountId == entity.Id)
                        .GroupBy(rg => rg.MfsEntityAccountId)
                        .Select(
                            r =>
                                new BalanceModel
                                {
                                    balance =
                                        r.Sum(r2 => r2.AmountInCredit)
                                        - r.Sum(r2 => r2.AmountOutDebit)
                                }
                        )
                        .FirstOrDefaultAsync();
                    // r => (r.Sum(r2 => r2.AmountInCredit) - r.Sum(r2 => r2.AmountOutDebit)))

                    if (resultBalance == null)
                    {
                        return result;
                    }
                    result = new BalanceModel { balance = resultBalance.balance };
                    //new BalanceModel
                    //{
                    //    balance = r.Sum(r2 => r2.AmountInCredit) - r.Sum(r2 => r2.AmountOutDebit)
                    //})
                }
                catch (Exception ex)
                {
                    //result = result.CopyFromException(ex);
                }

                #endregion
            }
            return result;
        }

        public async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetFloatList(NeedDataSourceEventArgs args)
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally

            var executionResult = new ExecutionResult<
                ItemsRequestedResult<NeedDataSourceEventArgs>
            >(true, "MfsEntityAccountTransaction instance found");
            try
            {
                var context = new EFDatabaseModelDatabaseContext();

                args = NeedDataSourceEventArgs.EnsureArgsValid(args);

                var entity = await context.MfsEntityAccounts.FirstOrDefaultAsync(
                    r => r.AccountEntityId == UserPrinciple.User.AccountEntityId
                );
                //var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(r => r.Id == entity.SubAccountBranchId);
                //var subacc = await context.SubAccounts.FirstOrDefaultAsync(r => r.Id == branchi.SubAccountId);


                var accentity = (
                    await context.MfsEntityAccountTransactions
                        .Join(
                            context.MsfSystemTransactionReceipts,
                            ae => ae.MfsSystemTransactionReceiptId,
                            idt => idt.Id,
                            (ae, idt) => new { ae, idt }
                        )
                        .Join(
                            context.MfsEntityAccounts,
                            x => x.ae.MfsEntityAccountId,
                            s => s.Id,
                            (x, s) => new { x, s }
                        )
                        //.Join(context.AccountEntities, y => y.s.AccountEntityId, idt => idt.Id, (y, idt) => new { y, idt })
                        // .Join(context.SubAccountBranches, z => z.idt.SubAccountBranchId, sab => sab.Id, (z, sab) => new { z, sab })
                        //.Join(context.SubAccounts, w => w.sab.SubAccountId, sa => sa.Id, (w, sa) => new { w, sa })


                        .Where(r => r.s.Id == entity.Id && r.x.ae.AmountInCredit > 0) // && r.x.ae.Narration== "Float topup by parent account"
                        //.OrderBy(r => r.x.ae.Id)
                        .OrderBy(r => r.x.ae.Id)
                        .Skip(0)
                        .Take(args.take) // * args.page
                        //.OrderByDescending(r => r.w.z.y.x.ae.TransactionDate)
                        //.OrderBy(r => r.x.ae.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new MfsEntityAccountTransaction
                        {
                            Id = r.x.ae.Id,
                            TransactionDate = r.x.ae.TransactionDate,
                            AmountInCredit = r.x.ae.AmountInCredit,
                            Narration = r.x.ae.Narration,
                            MfsSystemTransactionReceipt = new MsfSystemTransactionReceipt
                            {
                                Id = r.x.idt.Id,
                                ReceiptNo = r.x.idt.ReceiptNo
                            }
                        }
                );
                //args.filter = null;

                var filterSupporter = new FilterSupporter<MfsEntityAccountTransaction>(
                    accentity,
                    args
                ); //accentity
                var filteredData = filterSupporter.FilterData<long>(r => r.Id);
                executionResult.Result = filterSupporter.itemsResult;
                //executionResult.Result.Result=accentity;
            }
            catch (Exception ex)
            {
                //if an error occured copy its data from the exception object into the execution result by
                executionResult.CopyFromException(ex);
            }
            finally
            {
                //incase the context was created in this method then dispose it here
                //if (createContextHere)
                //    await context.DisposeAsync();
            }
            return executionResult;
        }
    }
}
                #endregion
