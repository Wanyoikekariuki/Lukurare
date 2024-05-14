using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using LukurareBackend.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using ProjectBase.Repository.Transaction;
using ProjectBase.KendoUiSupport;
using EFDatabaseModel.Repository;


using LukurareBackend.Models;
using EFDatabaseModel.Models;

using ProjectBase.Repository.Configuration;
using InterEaseBackend.Models;

namespace LukurareBackend.Repositories.DashBoard
{
    public class DashBoardModelRepository : EFDatabaseModelBaseRepository<DashBoardModel>
    //: DbContextRepository<DashBoardModel, EFDatabaseModelDatabaseContext>
    {
        public DashBoardModelRepository(string currentModule)
            : base(currentModule) { }

        public override Task<ExecutionResult<DashBoardModel>> Add(DashBoardModel model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }

        public override DashBoardModel CloneModel(DashBoardModel model)
        {
            throw new NotImplementedException();
        }


        public virtual UserPrinciple GetCurrentPrincipalGenericPrinciple()
        {
            var princip = CurrentPrincipal as UserPrinciple;

            return princip;
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

        public override Task<ExecutionResult<DashBoardModel>> GetItem(int id, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }

        public override Task<ExecutionResult<DashBoardModel>> Update(DashBoardModel model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }
              

        public async Task<ExecutionResult<AccountEntity>> UploadAccounts(
           IFormFileCollection files,
           string path
       )
        {
            var executionResult = new ExecutionResult<AccountEntity>(true);


            try
            {
                EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext();
                using (context)
                {
                    var accountEntity = context.AccountEntities.FirstOrDefault(r => r.Id == UserPrinciple.User.AccountEntityId);
                    long uploaded_size = 0;

                    string path_for_Uploaded_Files = $"{path}/UploadedFiles/";

                    //</ init >



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

                        DateTime now = DateTime.Now;
                        string formattedDate = now.ToString("ddMMyyyymmHHss");

                        //< Filename >
                        uploaded_Filename = uploaded_file.FileName;
                        new_Filename_on_Server = $"{path_for_Uploaded_Files}{accountEntity.EntityUserName}{uploaded_Filename}{formattedDate}.pdf";                                

                        //</ Filename >

                        //< Copy File to Target >

                        using (
                            FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create)
                        )
                        {
                            await uploaded_file.CopyToAsync(stream);

                            var documentName = $"{uploaded_Filename}";
                            var required = await context.IdentificationDocumentTypes
                                .Join(context.AccountEntityTypeRequiredDocuments, r => r.Id, x => x.IdentificationDocumentTypeId, (r, x) => new { r, x })
                                .Where(ae => ae.r.DocumentName == documentName && ae.x.AccountEntityTypeId == accountEntity.AccountEntityTypeId)
                                .FirstOrDefaultAsync();

                            if (required == null)
                            {
                                executionResult.Message = "Documents for this user type not defined";
                                executionResult.IsOkay = true;
                                return executionResult;
                            }
                            var accountEntitySecondaryIdentificationDocuments = new AccountEntitySecondaryIdentificationDocument
                            {
                                AccountEntityId = accountEntity.Id,
                                RequiredDocumentsId = required.x.Id,
                                Path = $"/UploadedFiles/{accountEntity.EntityUserName}{uploaded_Filename}{formattedDate}.pdf",
                                Validated = false,
                                Active = true
                            };
                            context.AccountEntitySecondaryIdentificationDocuments.Add(accountEntitySecondaryIdentificationDocuments);
                            context.SaveChanges();

                            executionResult.Message = "The file has uploaded  successfully and The Detail have been saved succefully"; // to    /UploadedFiles/{uploaded_Filename}


                        }
                    }
                }
            }
            catch (Exception ex) {
                executionResult.Message = ex.Message;

                executionResult.IsOkay = true;
            }

            return executionResult;
        }

        //public async Task<ExecutionResult<DashBoardModel>> GetDashboardSummary()
        //{

        //    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
        //    {
        //        var executionResult = new ExecutionResult<DashBoardModel>(true);
        //        var finalModel = new DashBoardModel();

        //        try
        //        {


        //            var accounts



        //            executionResult.Result = finalModel;
        //        }
        //        catch (Exception ex)
        //        {
        //            executionResult.Exception = ex;
        //            executionResult.Message = ex.Message;
        //            executionResult.IsOkay = false;
        //        }

        //        return executionResult;
        //        //b.f.e.d.c.b.meat.TransactionDate
        //    }


        //}


        public async Task<ExecutionResult<List<MissingDocumentInfo>>> CheckRequiredDocuments()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var accountEntity = context.AccountEntities.FirstOrDefault(u => u.Id == accountEntityId);

                    var accountType = context.AccountEntities
                        .Join(context.AccountEntityTypes, d => d.AccountEntityTypeId, e => e.Id, (d, e) => new { d, e })
                        .Where(n => n.d.Id == accountEntityId)
                        .FirstOrDefault();

                    var executionResult = new ExecutionResult<List<MissingDocumentInfo>>(true);

                var requiredDocumentIds = await context.AccountEntityTypeRequiredDocuments                        
                       .Where(aetr => aetr.AccountEntityTypeId == accountEntity.AccountEntityTypeId) 
                       .Select(aetr => aetr.IdentificationDocumentTypeId)
                       .ToListAsync();

                    var existingDocuments = await context.AccountEntitySecondaryIdentificationDocuments
                        .Join(context.AccountEntities, a => a.AccountEntityId, b => b.Id, (a, b) => new { a, b })
                        .Join(context.AccountEntityTypeRequiredDocuments, c => c.a.RequiredDocumentsId, e => e.Id, (c, e) => new { c, e })
                        .Join(context.AccountEntityTypes, ab => ab.c.b.AccountEntityTypeId, ac => ac.Id, (ab, ac) => new { ab, ac })
                        .Where(d => d.ab.c.b.Id == accountEntityId && d.ac.Id == d.ab.c.b.AccountEntityTypeId)                        
                        .ToListAsync();

                    // Extract the existing document IDs
                    var existingDocumentIds = existingDocuments.Select(d => d.ab.e.IdentificationDocumentTypeId).ToList();

                    // Find missing document IDs
                    var missingDocumentIds = requiredDocumentIds.Except(existingDocumentIds).ToList();
                 
                    var missingDocuments = await context.IdentificationDocumentTypes
                   .Where(d => missingDocumentIds.Contains(d.Id))
                   .Select(d => new MissingDocumentInfo { DocumentName = d.DocumentName, Required = context.AccountEntityTypeRequiredDocuments.Any(aetr => aetr.IdentificationDocumentTypeId == d.Id && aetr.Required) })
                   .ToListAsync();

                    executionResult.Result = missingDocuments;

                    return executionResult;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, such as logging the exception details or throwing a custom exception.
                // You can also consider returning a specific error result instead.

                // Example: Log the exception
                Console.WriteLine($"An exception occurred: {ex}");

                // Example: Throw a custom exception
                throw new Exception("An error occurred while checking required documents.", ex);
            }
        }


        public List<DashBoardModel> GetMessageReplies()
        {
            using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
            {
                var myList = new List<DashBoardModel>();

                var content = context.SchoolMsSmsInboxes
                    .Where(r => r.Datesent != null)
                    .GroupBy(r => (r.Datesent).Date)
                    .Select(g => new DashBoardModel { x = g.Key, y = g.Count() })
                    .ToList();

                return content;
            }
        }
        public async Task<ExecutionResult<AccountEntity>> SubmitUserUploadDetails(
            KeyNames model
            )
        {
            var executionResult = new ExecutionResult<AccountEntity>(true);

            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntity = await context.AccountEntities.FirstOrDefaultAsync(r => r.Id == UserPrinciple.User.AccountEntityId);
                    var KeyId = 0;
                    string value = null;

                    if (model.IDNumber != null)
                    {
                        var IDNumber = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.NationaIDNumber);
                        KeyId = IDNumber.Id;
                        value = model.IDNumber;
                    }
                    if (model.ParentIDNumber != null)
                    {
                        var parentIDNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.ParentIDNUmber);
                        KeyId = parentIDNo.Id;
                        value = model.ParentIDNumber;
                    }
                    if (model.SpouseIDNumber != null)
                    {
                        var spouseIdNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.SpouceIDNumber);
                        KeyId = spouseIdNo.Id;
                        value = model.SpouseIDNumber;
                    }
                    if (model.KRAPinNumber != null)
                    {
                        var KraPinNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.KraPinNumber);
                        KeyId = KraPinNo.Id;
                        value = model.KRAPinNumber;
                    }
                    if (model.GoodConductNumber != null)
                    {
                        var GoodConductNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.GoodConductNumber);
                        KeyId = GoodConductNo.Id;
                        value = model.GoodConductNumber;
                    }
                    if (model.BirthCertificateNumber != null)
                    {
                        var BirthCertNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.BirthCertificateNumber);
                        KeyId = BirthCertNo.Id;
                        value = model.BirthCertificateNumber;
                    }
                    if (model.ChildsBirthCertificateNumber != null)
                    {
                        var ChildsBirthCertNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.ChildBirthCertificateNumber);
                        KeyId = ChildsBirthCertNo.Id;
                        value = model.ChildsBirthCertificateNumber;
                    }
                    if (model.PassPortNumber != null)
                    {
                        var PassPortNo = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.PassportNumber);
                        KeyId = PassPortNo.Id;
                        value = model.PassPortNumber;
                    }
                    if (model.CountryOrigin != null && model.CountryCitizenship != null && model.CountryTravelling != null && model.CurrentCountry != null)
                    {
                        var CountryOfOrigin = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.CountryOfOrigin);
                        var addCountryOfOrigin = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = CountryOfOrigin.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.CountryOrigin
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(addCountryOfOrigin);
                        var CountryCitizenship = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.Citizenship);
                        var addCountryCitizenship = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = CountryCitizenship.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.CountryCitizenship
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(addCountryCitizenship);
                        var CountryTravelling = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.CountryToTravel);
                        var AddCountryTravelling = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = CountryTravelling.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.CountryTravelling
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(AddCountryTravelling);
                        var CurrentCountry = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.CurrentCountry);
                        var AddCurrentCountry = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = CurrentCountry.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.CurrentCountry
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(AddCurrentCountry);

                        context.SaveChanges();
                        executionResult.Message = "The Detail have been saved succefully";
                        return executionResult;
                    }
                    if (model.Age != null && model.HighestLevelOfEducation != null && model.HowDidYouKnowOfUs != null)
                    {
                        var Age = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.Age);
                        var addAge = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = Age.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.Age
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(addAge);

                        var HighestlevelOfEducation = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.HighestLevelOfEducation);
                        //var HighestLevelOfEducation = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.HighestLevelOfEducation);
                        var addHighestLevelOfEducation = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = HighestlevelOfEducation.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.HighestLevelOfEducation
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(addHighestLevelOfEducation);

                        var HowDidYouKnowOfUs = context.AccountEntityTypeAdditionalDetails.FirstOrDefault(y => y.KeyName == DefaultConfiguration.AccountEntityTypeAditionalDetails.HowDidYouKnowOfUs);
                        var addHowDidYouKnowOfUs = new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetailsId = HowDidYouKnowOfUs.Id,
                            AccountEntityId = accountEntity.Id,
                            Value = model.HowDidYouKnowOfUs
                        };
                        context.AccountEntityTypeAdditionalDetailsValues.Add(addHowDidYouKnowOfUs);

                        context.SaveChanges();
                        executionResult.Message = "Details have been saved succefully";
                        return executionResult;

                    }
                     
                    var additionalDetails = new AccountEntityTypeAdditionalDetailsValue
                    {
                        TypeAdditionalDetailsId = KeyId,
                        AccountEntityId = accountEntity.Id,
                        Value = value
                    };
                    context.AccountEntityTypeAdditionalDetailsValues.Add(additionalDetails);
                    context.SaveChanges();
                    executionResult.Message = "The Detail have been saved succefully";
                }
            }
            catch (Exception ex)
            {

                executionResult.Message = ex.Message;
            }

            return executionResult;
        }

        public async Task<ExecutionResult<AccountEntityType>> GetUserType()
        {
            var executionResult = new ExecutionResult<AccountEntityType>(true);

            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {

                    var accountEntity = context.AccountEntities.FirstOrDefault(r => r.Id == UserPrinciple.User.AccountEntityId);
                    var accountEntityId = accountEntity.Id;
                    var requiredEntityTypeId = accountEntity.AccountEntityTypeId;

                    var accountEntityTypeName =await context.AccountEntityTypes
                        .Join(context.AccountEntities, y => y.Id, u => u.AccountEntityTypeId, (y, u) => new { y, u })
                        .Where(ae => ae.u.Id == accountEntityId)
                        .Select(s => new AccountEntityType
                        {
                            Id = s.y.Id,
                            TypeName = s.y.TypeName,
                        })
                        .FirstOrDefaultAsync();

                    executionResult.Result = accountEntityTypeName;
                }
            }
            catch (Exception ex)
            {

                executionResult.Message = ex.Message;
                executionResult.IsOkay = false;
            }

            return executionResult;
        }

        public async Task<ExecutionResult<List<SchoolMsSmsToSend>>> GetAllMessages()
        {
            var executionResult = new ExecutionResult<List<SchoolMsSmsToSend>>(true);

            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;


                    var messages = await context.SchoolMsSmsToSends
                        .Where(m => m.EntityId == accountEntityId)
                        .Select(m => new SchoolMsSmsToSend
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            Message = m.Message,
                            PhoneNumber = m.PhoneNumber,
                            SentTime = m.SentTime
                        })
                        .ToListAsync();

                    executionResult.Result = messages;

                }
            }
            catch (Exception ex)
            {
                executionResult.Message = ex.Message;
                executionResult.IsOkay = false;
            }
            return executionResult;
        }



        public async Task<ExecutionResult<DashBoardModel>> GetDashboardSummary()
        {

            using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
            {
                var executionResult = new ExecutionResult<DashBoardModel>(true);
                var finalModel = new DashBoardModel();

                try
                {

                    var userPrincipal = (
                          await context.AccountEntities
                        .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
                        .Join(context.SubAccounts, q => q.sab.SubAccountId, sa => sa.Id, (q, sa) => new { q, sa })
                        .FirstOrDefaultAsync(b => b.q.ae.Id == (GetCurrentPrincipalGenericPrinciple().User.AccountEntityId))
                        );

                    // Meter Accounts Queries

                    var allCustomerTransactions= await context.MfsEntityAccountTransactions
                        .Join(context.MfsEntityAccounts, b => b.MfsEntityAccountId, mea => mea.Id, (b, mea) => new { b, mea })
                        .Join(context.AccountEntities, c => c.mea.AccountEntityId, ae => ae.Id, (c, ae) => new { c, ae })
                        .Join(context.AccountEntityTypes, d => d.ae.AccountEntityTypeId, sab => sab.Id, (d, sab) => new { d, sab })
                        .Join(context.MfsAccountTypes, f => f.d.c.mea.MfsAccountTypeId, t => t.Id, (f,t) => new { f ,t})
                        .Where(s => s.f.sab.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName)
                        .OrderByDescending(y => y.f.d.c.b.Id)
                        .ToListAsync();
                    var totalMoneyCollected =  allCustomerTransactions.Sum(q => q.f.d.c.b.AmountInCredit - q.f.d.c.b.AmountOutDebit);
                    var totalRetainerFees = allCustomerTransactions.Where(x => x.t.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName).Sum(q => q.f.d.c.b.AmountInCredit - q.f.d.c.b.AmountOutDebit);
                    int totalRetainerFeeInt = Convert.ToInt32(totalRetainerFees);
                    finalModel.totalRetainerFeeCollection = totalRetainerFeeInt;
                    var totalApplicationFees = allCustomerTransactions.Where(x => x.t.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.applicationAccountTypeName).Sum(q => q.f.d.c.b.AmountInCredit - q.f.d.c.b.AmountOutDebit);
                    int totalApplicationFeeInt = Convert.ToInt32(totalApplicationFees);
                    finalModel.totalApplicationFeeCollection = totalApplicationFeeInt;
                    var totalOtherPaymentFees = allCustomerTransactions.Where(x => x.t.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.defaultAccountTypeName).Sum(q => q.f.d.c.b.AmountInCredit - q.f.d.c.b.AmountOutDebit);
                    int totalOtherPaymentFeeInt = Convert.ToInt32(totalOtherPaymentFees);
                    finalModel.totalOtherPaymentCollection = totalOtherPaymentFeeInt;

                    var AllUsersAvailable = await context.AccountEntities
                            .Join(context.AccountEntityTypes, ae => ae.AccountEntityTypeId, idt => idt.Id, (ae, idt) => new { ae, idt })                            
                            .Where(r => r.idt.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName/* || r.idt.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName*/)
                            .Skip(0)
                            .OrderBy(r => r.ae.Id)
                            .ToListAsync();


                    //var totalAgents = AllUsersAvailable.Where(w => w.idt.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName).Count();
                    //int totalAgentsInt = Convert.ToInt32(totalAgents);
                    //finalModel.totalNumberofAgents = totalAgentsInt;

                    var totalClients = AllUsersAvailable.Where(w => w.idt.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName).Count();
                    int totalCustomerInt = Convert.ToInt32(totalClients);
                    finalModel.totalNumberofCustomers = totalCustomerInt;

                    executionResult.Result = finalModel;
                }
                catch (Exception ex)
                {
                    executionResult.Exception = ex;
                    executionResult.Message = ex.Message;
                    executionResult.IsOkay = false;
                }

                return executionResult;
                //b.f.e.d.c.b.meat.TransactionDate
            }

        }


        public async Task<ExecutionResult<List<List<Charts>>>> GetCharts()
        {
            var executionResult = new ExecutionResult<List<List<Charts>>>(true);

            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var myList = new List<List<Charts>>();

                    //.Date for the beginning of the day
                    var currentDate = DateTime.UtcNow;
                    var lastOneWeek = currentDate.Date.AddDays(-7);

                    var userPrincipal = (
                          await context.AccountEntities
                        .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
                        .Join(context.SubAccounts, q => q.sab.SubAccountId, sa => sa.Id, (q, sa) => new { q, sa })
                        .FirstOrDefaultAsync(b => b.q.ae.Id == (GetCurrentPrincipalGenericPrinciple().User.AccountEntityId))
                        );

                    var allCustomerTransactions = await context.MfsEntityAccountTransactions
                        .Join(context.MfsEntityAccounts, b => b.MfsEntityAccountId, mea => mea.Id, (b, mea) => new { b, mea })
                        .Join(context.AccountEntities, c => c.mea.AccountEntityId, ae => ae.Id, (c, ae) => new { c, ae })
                        .Join(context.AccountEntityTypes, d => d.ae.AccountEntityTypeId, sab => sab.Id, (d, sab) => new { d, sab })
                        .Join(context.MfsAccountTypes, f => f.d.c.mea.MfsAccountTypeId, t => t.Id, (f, t) => new { f, t })
                        .Where(s => s.f.sab.TypeName == DefaultConfiguration.AccountEntityType.customerTypeName)
                        .OrderByDescending(y => y.f.d.c.b.Id)
                        .ToListAsync();

                    var totalMeterCollections = allCustomerTransactions.Where(k => k.t.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.defaultAccountTypeName
                                        && k.f.d.c.b.TransactionDate >= lastOneWeek && k.f.d.c.b.TransactionDate <= currentDate)
                        .GroupBy(m => m.f.d.c.b.TransactionDate.Date)
                            .Select(g => new Charts
                            {
                                x = g.Key,
                                y = g.Sum(k => k.f.d.c.b.AmountInCredit)
                            }).ToList();

                    myList.Add(totalMeterCollections);

                    var totalRentCollections = allCustomerTransactions.Where(k => k.t.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.applicationAccountTypeName
                                        && k.f.d.c.b.TransactionDate >= lastOneWeek && k.f.d.c.b.TransactionDate <= currentDate)
                        .GroupBy(m => m.f.d.c.b.TransactionDate.Date)
                            .Select(g => new Charts
                            {
                                x = g.Key,
                                y = g.Sum(k => k.f.d.c.b.AmountInCredit)
                            }).ToList();

                    myList.Add(totalRentCollections);

                    var meterList = new List<DateTime?>();
                    var rentList = new List<DateTime?>();

                    for (int i = 0; i < myList[0].Count; i++)
                    {
                        meterList.Add(myList[0][i].x);
                    }

                    for (int i = 0; i < myList[1].Count; i++)
                    {
                        rentList.Add(myList[1][i].x);
                        if (!meterList.Contains(myList[1][i].x))
                        {
                            var model = new Charts { x = myList[1][i].x, y = 0 };

                            myList[0].Add(model);
                        }
                    }

                    for (int i = 0; i < myList[0].Count; i++)
                    {
                        if (!rentList.Contains(myList[0][i].x))
                        {
                            var model = new Charts { x = myList[0][i].x, y = 0 };

                            myList[1].Add(model);
                        }

                    }

                    //myList[0].Sort();
                    for (int i = 0; i < myList[0].Count - 1; i++)
                    {
                        if (myList[0][i].x > myList[0][i + 1].x)
                        {
                            var temp = myList[0][i];
                            myList[0][i] = myList[0][i + 1];
                            myList[0][i + 1] = temp;

                            i = -1;
                        }
                    }
                    for (int i = 0; i < myList[1].Count - 1; i++)
                    {
                        if (myList[1][i].x > myList[1][i + 1].x)
                        {
                            var temp2 = myList[1][i];
                            myList[1][i] = myList[1][i + 1];
                            myList[1][i + 1] = temp2;

                            i = -1;
                        }
                    }


                    executionResult.Result = myList;
                }
            }
            catch (Exception ex)
            {
                executionResult.Exception = ex.InnerException;
                executionResult.IsOkay = false;
                executionResult.Message = "Failed to retrieve data";
            }

            return executionResult;
        }


    }
}

   