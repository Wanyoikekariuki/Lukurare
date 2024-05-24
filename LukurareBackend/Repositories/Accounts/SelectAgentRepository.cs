using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using LukurareBackend.Models;
using LukurareBackend.Models.Hubs;
using LukurareBackend.Repositories.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509.SigI;
using ProjectBase.Database.Connection;
using ProjectBase.KendoUiSupport;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using RestSharp;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories
{
    public class SelectAgentRepository : PatnerAccountEntityRepository
    {


        public SelectAgentRepository(string currentModule)
           : base(currentModule)
        { }
        public async Task<ExecutionResult<List<AccountEntity>>> SelectAgent()
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<List<AccountEntity>>(true);

                        var chooseAgent = await context.AccountEntities
                            .Join(context.AccountEntityTypes, a => a.AccountEntityTypeId, s => s.Id, (a, s) => new { a, s })
                            .Where(x => x.s.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName)
                            .Select(u => new AccountEntity
                            {
                                Id = u.a.Id,
                                SubAccountBranchId = u.a.SubAccountBranchId,
                                EntityName = u.a.EntityName,
                                Phone1 = u.a.Phone1,
                                Email = u.a.Email

                            })
                            .ToListAsync();

                        executionResult.Result = chooseAgent;
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
                    throw new Exception("An error occurred while selecting an Agent", ex);
                }
            }

        }

        public async Task<ExecutionResult<AccountEntity>> SubmitAgent(AccountEntity model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AccountEntity>(true);

                        var submitAgent = await context.AccountEntities
                           .Join(context.SubAccountBranches, x => x.SubAccountBranchId, y => y.Id, (x, y) => new { x, y })
                           .Join(context.SubAccounts, e => e.y.SubAccountId, z => z.Id, (e, z) => new { e, z })
                           .Where(t => t.e.x.Id == model.Id)
                           .FirstOrDefaultAsync();

                        var getCustomer = await context.AccountEntities
                         .Join(context.SubAccountBranches, x => x.SubAccountBranchId, y => y.Id, (x, y) => new { x, y })
                         .Join(context.SubAccounts, e => e.y.SubAccountId, z => z.Id, (e, z) => new { e, z })
                         .Where(t => t.e.x.Id == accountEntityId)
                         .FirstOrDefaultAsync();

                        if (getCustomer.z.ParentId == null)
                        {
                            getCustomer.z.ParentId = submitAgent.z.Id;

                            var tableMessage = new StringBuilder();
                            tableMessage.AppendLine($"Dear {model.EntityName},");
                            tableMessage.AppendLine();
                            tableMessage.AppendLine("We hope this message finds you well. We are thrilled to inform you that you have a new customer who has chosen you as their agent.");
                            tableMessage.AppendLine("These are the details of the new customer who has registered under you:");
                            tableMessage.AppendLine();
                            tableMessage.AppendLine($"Customer Name: {getCustomer.z.AccountName}");
                            tableMessage.AppendLine($"Email Address: {getCustomer.z.Email}");
                            tableMessage.AppendLine($"Phone Number: {getCustomer.z.Phone}");
                            tableMessage.AppendLine();
                            tableMessage.AppendLine("If you require any additional information about the customer, please do not hesitate to reach out to the customer.");

                            var plainTextMessage = tableMessage.ToString();
                            var htmlMessage = plainTextMessage.Replace("\n", "<br>");

                            var smsRecord = new SchoolMsSmsToSend
                            {
                                SenderId = "You have a new Customer!", // Set Title into sender_id column
                                PhoneNumber = model.Phone1, // Set ReceiverNo into phone_number column
                                Message = plainTextMessage,
                                EntityId = accountEntityId, // Set accountEntityId into entity_id column
                                SentTime = DateTime.UtcNow,
                                BatchJobId = "0",
                            };
                            context.SchoolMsSmsToSends.Add(smsRecord);
                            //if (createContextHere)
                            //{
                            await context.SaveChangesAsync();

                            var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                            var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                            var request = new MailRequest
                            {
                                ToEmail = model.Email,
                                Subject = smsRecord.SenderId,
                                Body = htmlMessage,

                                From = FromEmail
                            };

                            var uri = new Uri(Domain);
                            var client = new RestClient(uri);
                            client.AddDefaultHeader("Content-Type", "application/json");
                            client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                                                                                   //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                            var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                            reques.AddJsonBody(request);

                            var restClientResult = await client.ExecuteAsync<MailRequest>(reques);
                            await context.SaveChangesAsync();

                            executionResult.Message = "Selected agent Successfully.";
                        }
                        else
                        {
                            executionResult.Message = "You have already chosen an agent.";
                        }

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
                    throw new Exception("An error occurred while submiting an Agent", ex);
                }
            }

        }
        public async Task<ExecutionResult<SubAccountBranch>> AgentDetails()
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<SubAccountBranch>(true);

                        var getDetails = await context.AccountEntities
                           .Join(context.SubAccountBranches, x => x.SubAccountBranchId, y => y.Id, (x, y) => new { x, y })
                           .Join(context.SubAccounts, e => e.y.SubAccountId, z => z.Id, (e, z) => new { e, z })
                           .Where(t => t.e.x.Id == accountEntityId)
                           .Select(q => new AccountEntity
                           {
                               SubAccountBranch = new SubAccountBranch
                               {
                                   SubAccount = new SubAccount
                                   {
                                       ParentId = q.z.ParentId
                                   }
                               }
                           })
                           .FirstOrDefaultAsync();

                        if (getDetails.SubAccountBranch.SubAccount.ParentId != null)
                        {
                            var agent = await context.SubAccountBranches
                           .Join(context.SubAccounts, x => x.SubAccountId, y => y.Id, (x, y) => new { x, y })
                           .Where(t => t.y.Id == getDetails.SubAccountBranch.SubAccount.ParentId)
                           .Select(s => new SubAccountBranch
                           {
                               BuildingAddress = s.x.BuildingAddress,
                               SubAccount = new SubAccount
                               {
                                   AccountName = s.y.AccountName,
                                   Email = s.y.Email,
                                   Phone = s.y.Phone
                               }
                           })
                           .FirstOrDefaultAsync();

                            executionResult.Result = agent;

                        }

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
                    throw new Exception("An error occurred while getting agent details", ex);
                }
            }

        }

        public async Task<ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>> CustomerInfo()
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = UserPrinciple.User.AccountEntityId;

                    var executionResult = new ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>(true);

                    var additionalDetails = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, ae => ae.TypeAdditionalDetailsId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.AccountEntities, u => u.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                        .Select(k => new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetails = new AccountEntityTypeAdditionalDetail
                            {

                                KeyName = k.u.t.KeyName

                            },
                            Value = k.u.ae.Value
                        })
                        .ToListAsync();

                    executionResult.Result = additionalDetails;

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
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }

        public async Task<ExecutionResult<List<Country>>> SelectCountry()
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<List<Country>>(true);

                        var chooseCountry = await context.Countries
                            .Select(u => new Country
                            {
                                Id = u.Id,
                                CountryCode = u.CountryCode,
                                CountryName = u.CountryName,
                                CountryIcon = u.CountryIcon,
                                CountryIso = u.CountryIso

                            })
                            .ToListAsync();

                        executionResult.Result = chooseCountry;
                        return executionResult;
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while selecting a countries", ex);
                }
            }

        }

        public async Task<ExecutionResult<List<AccountEntitySecondaryIdentificationDocument>>> UploadedDocuments(long id)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {

                    var accountEntity = id;

                    if (accountEntity == 0)
                    {
                        accountEntity = UserPrinciple.User.AccountEntityId;
                    }



                    var accountType = context.AccountEntities
                       .Join(context.AccountEntityTypes, d => d.AccountEntityTypeId, e => e.Id, (d, e) => new { d, e })
                       .Where(s => s.d.Id == accountEntity)

                       .FirstOrDefault();

                    var executionResult = new ExecutionResult<List<AccountEntitySecondaryIdentificationDocument>>(true);

                    if (accountType.e.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName)
                    {
                        var accountEType = context.AccountEntities
                       .Join(context.AccountEntityTypes, d => d.AccountEntityTypeId, e => e.Id, (d, e) => new { d, e })
                       .Where(n => n.d.Id == accountEntity)
                       .FirstOrDefault();

                        var uploadedDocuments = await context.AccountEntitySecondaryIdentificationDocuments
                            .Join(context.AccountEntityTypeRequiredDocuments, ae => ae.RequiredDocumentsId, t => t.Id, (ae, t) => new { ae, t })
                            .Join(context.IdentificationDocumentTypes, s => s.t.IdentificationDocumentTypeId, r => r.Id, (s, r) => new { s, r })
                            .Join(context.AccountEntities, u => u.s.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                            .Where(t => t.v.Id == accountEntity)
                            .Select(k => new AccountEntitySecondaryIdentificationDocument
                            {
                                RequiredDocuments = new AccountEntityTypeRequiredDocument
                                {
                                    //IdentificationDocumentTypeId = k.u.r.Id,
                                    IdentificationDocumentType = new IdentificationDocumentType
                                    {
                                        DocumentName = k.u.r.DocumentName,
                                    }
                                },
                                Id = k.u.s.ae.Id,
                                Validated = k.u.s.ae.Validated,
                                Path = k.u.s.ae.Path,
                            })
                            .ToListAsync();

                        executionResult.Result = uploadedDocuments;
                    }

                    //if (accountType.e.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName)
                    //{
                    //    var accountEType = context.AccountEntities
                    // .Join(context.AccountEntityTypes, d => d.AccountEntityTypeId, e => e.Id, (d, e) => new { d, e })
                    // .Where(n => n.d.Id == accountEntity)
                    // .FirstOrDefault();

                    //    var uploadedDocuments = await context.AccountEntitySecondaryIdentificationDocuments
                    //        .Join(context.AccountEntityTypeRequiredDocuments, ae => ae.RequiredDocumentsId, t => t.Id, (ae, t) => new { ae, t })
                    //        .Join(context.IdentificationDocumentTypes, s => s.t.IdentificationDocumentTypeId, r => r.Id, (s, r) => new { s, r })
                    //        .Join(context.AccountEntities, u => u.s.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                    //        .Where(t => t.v.Id == accountEntity)
                    //        .Select(k => new AccountEntitySecondaryIdentificationDocument
                    //        {
                    //            RequiredDocuments = new AccountEntityTypeRequiredDocument
                    //            {
                    //                //IdentificationDocumentTypeId = k.u.r.Id,                                    
                    //                IdentificationDocumentType = new IdentificationDocumentType
                    //                {
                    //                    DocumentName = k.u.r.DocumentName,
                    //                }
                    //            },
                    //            Id = k.u.s.ae.Id,
                    //            Validated = k.u.s.ae.Validated,
                    //            Path = k.u.s.ae.Path,
                    //        })
                    //        .ToListAsync();

                    //    executionResult.Result = uploadedDocuments;
                    //}

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
                throw new Exception("An error occurred while checking uploaded documents.", ex);
            }
        }

        public async Task<ExecutionResult<AccountEntitySecondaryIdentificationDocument>> ConfirmDocument(AccountEntitySecondaryIdentificationDocument model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {

                        var executionResult = new ExecutionResult<AccountEntitySecondaryIdentificationDocument>(true);

                        var validateDocument = await context.AccountEntitySecondaryIdentificationDocuments.FindAsync(model.Id);

                        if (validateDocument == null)
                        {
                            // Transaction not found, you can handle this case as required (throw exception, return null, etc.).
                            return null;
                        }

                        if (validateDocument.Validated == false && validateDocument.Path != null)
                        {
                            validateDocument.Validated = true;

                            await context.SaveChangesAsync();


                            executionResult.Message = "The document has been Approved Successfully.";
                        }

                        else
                        {
                            executionResult.Message = "This document has already been Approved";

                        }


                        return executionResult;

                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while confirming Payments", ex);
                }
            }

        }
        public async Task<ExecutionResult<AccountEntitySecondaryIdentificationDocument>> DeclineDocument(AccountEntitySecondaryIdentificationDocument model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {
                        var accountEntityId = UserPrinciple.User.AccountEntityId;

                        var executionResult = new ExecutionResult<AccountEntitySecondaryIdentificationDocument>(true);

                        var deleteDocument = await context.AccountEntitySecondaryIdentificationDocuments.FirstOrDefaultAsync(n => n.Id == model.Id);
                        //var deleteDocument = await context.AccountEntitySecondaryIdentificationDocuments
                        //    .Join(context.AccountEntities, a => a.AccountEntityId, b => b.Id, (a, b) => new { a, b })
                        //    .Join(context.AccountEntityTypeRequiredDocuments, e => e.a.RequiredDocumentsId, ae => ae.Id, (e, ae) => new { e, ae })
                        //    .Join(context.IdentificationDocumentTypes, bc => bc.ae.IdentificationDocumentTypeId, cd => cd.Id, (bc,cd) => new { bc, cd })
                        //    .Where(c => c.bc.e.a.Id == model.Id)                            
                        //    .FirstOrDefaultAsync();


                        var getCustomer = await context.AccountEntitySecondaryIdentificationDocuments
                            .Join(context.AccountEntities, a => a.AccountEntityId, b => b.Id, (a, b) => new { a, b })
                            //.Join(context.AccountEntityTypeRequiredDocuments, e => e.a.RequiredDocumentsId, ae => ae.Id, (e, ae) => new { e, ae })
                            //.Join(context.IdentificationDocumentTypes, bc => bc.ae.IdentificationDocumentTypeId, cd => cd.Id, (bc, cd) => new { bc, cd })
                            .Where(c => c.b.Id == model.AccountEntityId)
                            .Select(d => new AccountEntity
                            {
                                EntityName = d.b.EntityName,
                                Phone1 = d.b.Phone1,        
                                Email = d.b.Email,
                                Id = d.b.Id
                            })
                            .FirstOrDefaultAsync();
                        var getDocument = await context.AccountEntitySecondaryIdentificationDocuments
                            .Join(context.AccountEntityTypeRequiredDocuments, ex => ex.RequiredDocumentsId, xy => xy.Id, (ex, xy) => new { ex, xy })
                            .Join(context.IdentificationDocumentTypes, vx => vx.xy.IdentificationDocumentTypeId, rs => rs.Id, (vx, rs) => new { vx, rs })
                            .Where(k => k.rs.Id == model.RequiredDocumentsId)
                            .Select(cn => new IdentificationDocumentType
                            {
                                DocumentName = cn.rs.DocumentName
                            }
                            )
                            .FirstOrDefaultAsync();

                        if (getCustomer == null)
                        {
                            // Transaction not found, you can handle this case as required (throw exception, return null, etc.).
                            return null;
                        }

                        if (deleteDocument.Validated == false && deleteDocument.Path != null)
                        {
                            context.AccountEntitySecondaryIdentificationDocuments.Remove(deleteDocument);
                            //context.AccountEntitySecondaryIdentificationDocuments.Remove();

                            //var documentToDelete = new AccountEntitySecondaryIdentificationDocument { Id = getCustomer.Id };
                            //context.Entry(documentToDelete).State = EntityState.Deleted;

                            var tableMessage = new StringBuilder();
                            tableMessage.AppendLine($"Dear {getCustomer.EntityName},");
                            tableMessage.AppendLine();
                            tableMessage.AppendLine($"Your agent has rejected the {getDocument.DocumentName} " +
                                $"document that you had uploaded.");
                            tableMessage.AppendLine("Kindly reach out to your agent for more clarification.");

                            var plainTextMessage = tableMessage.ToString();
                            var htmlMessage = plainTextMessage.Replace("\n", "<br>");

                            var smsRecord = new SchoolMsSmsToSend
                            {
                                SenderId = "Your document has been rejected", // Set Title into sender_id column
                                PhoneNumber = getCustomer.Phone1, // Set ReceiverNo into phone_number column
                                Message = plainTextMessage,
                                EntityId = accountEntityId, // Set accountEntityId into entity_id column
                                SentTime = DateTime.UtcNow,
                                BatchJobId = "0",
                            };
                            context.SchoolMsSmsToSends.Add(smsRecord);
                            //if (createContextHere)
                            //{
                            await context.SaveChangesAsync();

                            var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                            var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                            var request = new MailRequest
                            {
                                ToEmail = getCustomer.Email,
                                Subject = smsRecord.SenderId,
                                Body = htmlMessage,

                                From = FromEmail
                            };

                            var uri = new Uri(Domain);
                            var client = new RestClient(uri);
                            client.AddDefaultHeader("Content-Type", "application/json");
                            client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                                                                                   //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                            var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                            reques.AddJsonBody(request);

                            var restClientResult = await client.ExecuteAsync<MailRequest>(reques);


                            await context.SaveChangesAsync();


                            executionResult.Message = "The document has been Declined Successfully.";
                        }

                        else
                        {
                            executionResult.Message = "You cannot decline a validated document ";

                        }


                        return executionResult;

                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while declining the document", ex);
                }
            }

        }




        public async Task<ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>> AdditionalDetails(int id)
        {
            try
            {
                using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                {
                    var accountEntityId = id;

                    var executionResult = new ExecutionResult<List<AccountEntityTypeAdditionalDetailsValue>>(true);

                    var additionalDetails = await context.AccountEntityTypeAdditionalDetailsValues
                        .Join(context.AccountEntityTypeAdditionalDetails, ae => ae.TypeAdditionalDetailsId, t => t.Id, (ae, t) => new { ae, t })
                        .Join(context.AccountEntities, u => u.ae.AccountEntityId, v => v.Id, (u, v) => new { u, v })
                        .Where(t => t.v.Id == accountEntityId)
                        .Select(k => new AccountEntityTypeAdditionalDetailsValue
                        {
                            TypeAdditionalDetails = new AccountEntityTypeAdditionalDetail
                            {

                                KeyName = k.u.t.KeyName

                            },
                            Value = k.u.ae.Value
                        })
                        .ToListAsync();

                    executionResult.Result = additionalDetails;

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
                throw new Exception("An error occurred while checking additional details", ex);
            }
        }
        public async Task<ExecutionResult<IEnumerable<AccountEntityType>>> GetAccountType()
        {
            var result = new ExecutionResult<IEnumerable<AccountEntityType>>(
                true,
                "",
                new HashSet<AccountEntityType>()
            );
            try
            {
                using (var context = new EFDatabaseModelDatabaseContext())
                {
                    var returnData = await context.AccountEntityTypes.ToListAsync();

                    result.Result = returnData.Select(
                        r =>
                            new AccountEntityType
                            {
                                Id = r.Id,
                                TypeName = r.TypeName,
                                Prefix = r.Prefix,
                                //AccountNameAlias = r.AccountNameAlias
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


                var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                var code = ProjectBase.Security.EncryptionHelper
                    .GetRandomID(DateTime.UtcNow.Ticks, 0, 1000)
                    .ToString();

                var defaultAccount = await context.AccountEntityTypes.FirstOrDefaultAsync(
                    r => r.TypeName == DefaultConfiguration.AccountEntityType.customerTypeName
                );
                if (defaultAccount == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Account with name {DefaultConfiguration.AccountEntityType.customerTypeName} has not been defined";
                    return executionResult;
                }
                var natDocType = await context.IdentificationDocumentTypes.FirstOrDefaultAsync(
                     r =>
                         r.DocumentName
                         == DefaultConfiguration.IdentificationDocumentType.NationalIDDocumentName
                 );
                if (natDocType == null)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message =
                        $"Identification document type with name {DefaultConfiguration.IdentificationDocumentType.NationalIDDocumentName} has not been defined";
                    return executionResult;
                }
                //var defaultCurrency = await context.MfsCurrencyTypes.FirstOrDefaultAsync(
                //    r => r.CurrencyCode == DefaultConfiguration.Currency.defaultCurrency
                //);
                //if (defaultCurrency == null)
                //{
                //    executionResult.IsOkay = false;
                //    executionResult.Message =
                //        $"The currency  with code {DefaultConfiguration.SupportedUserType.WebUserType} has not been defined";
                //    return executionResult;
                //}

                var modelInsert = new AccountEntity
                {

                    EntityName = model.EntityName,
                    RegDate = DateTime.UtcNow,
                    DateOfBirth = DateTime.UtcNow,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone1,
                    EntityNo = code,
                    AccountEntityTypeId = defaultAccount.Id,
                    Email = model.Email,
                    IsActive = true,
                    PhysicalAddress = model.PhysicalAddress,
                    PostalAddress = model.PostalAddress,
                    EntityUserName = model.EntityUserName,
                    IsBackUpRecord = false,
                    EntityDetailsValidated = false,
                    IdentificationDocumentNumber = model.IdentificationDocumentNumber,
                    IdentificationDocumentTypeId = natDocType.Id,
                };

                executionResult = await base.Add(modelInsert, context);
                if (!executionResult.IsOkay)
                {
                    return executionResult;
                }
                //var user = new User
                //{                    
                //    AccountEntity = executionResult.Result
                //};
                //context.Users.Add(user);

                //var mode = new MfsEntityAccount
                //{
                //    MfsAccountTypeId = defaultAccount.Id,
                //    MfsCurrencyTypeId = defaultCurrency.Id,
                //    AccountName = executionResult.Result.EntityName,
                //    AccountNumber = executionResult.Result.EntityNo,
                //    AccountEntity = executionResult.Result,
                //};
                //var floataccount = new ResellerFloatAccountRepository(module);
                //floataccount.SetCurrentPrincipal(UserPrinciple);
                //var final = await floataccount.Add(mode, context);
                //var parentEntityAccount = await context.MfsEntityAccounts.FirstOrDefaultAsync(
                //    r => r.AccountEntityId == UserPrinciple.User.AccountEntityId
                //);
                //var parentCommissions = await context.MfsServiceCommissions
                //    .Where(r => r.MfsEntityAccountId == parentEntityAccount.Id)
                //    .ToListAsync();
                //foreach (var item in parentCommissions)
                //{
                //    var childcommission = new MfsServiceCommission
                //    {
                //        PercentageCommission = item.PercentageCommission,
                //        BillReferenceServiceTypeId = item.BillReferenceServiceTypeId,
                //        MfsEntityAccount = final.Result,
                //        Narration = "Rate Inherited from parent"
                //    };
                //    context.MfsServiceCommissions.Add(childcommission);
                //}

                if (createContextHere)
                {
                    await context.SaveChangesAsync();


                    var request = new MailRequest
                    {
                        ToEmail = modelInsert.Email,
                        Subject = "New Account Credentials",
                        Body =
                            $"Your account username is:{modelInsert.EntityUserName} and password is:{code}.Use them to login at this url: {Domain}",
                        From = FromEmail
                    };

                    var uri = new Uri(Domain);
                    var client = new RestClient(uri);
                    client.AddDefaultHeader("Content-Type", "application/json");
                    client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                    //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                    var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                    reques.AddJsonBody(request);

                    var restClientResult = await client.ExecuteAsync<MailRequest>(reques);

                    executionResult.Message =
                        $"Account Created successfully!"; //Username:{modelInsert.EntityUserName}  Password:{code}

                    //await context.SaveChangesAsync();
                    //var request = new MailRequest
                    //{
                    //    ToEmail = executionResult.Result.Email,
                    //    Subject = "New Account Credentials",
                    //    Body =
                    //        $"Your account username is:{executionResult.Result.EntityUserName} and password is:{code}.Use them to login at this url: {Domain}",
                    //    From = FromEmail
                    //};

                    //var uri = new Uri(Domain);
                    //var client = new RestClient(uri);
                    //client.AddDefaultHeader("Content-Type", "application/json");
                    //client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
                    ////client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
                    //var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

                    //reques.AddJsonBody(request);

                    //var restClientResult = client.Execute<MailRequest>(reques);
                    ////await mailService.SendEmailAsync(request);
                    //executionResult.Message = "Create operation was successful";
                    ////executionResult.Result = await GetModelForInsertUpdateFromId(
                    ////    executionResult.Result.Id,
                    ////    executionResult.Result,
                    ////    context
                    ////);
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

        public async override Task<ExecutionResult<AccountEntity>> Update(AccountEntity model, EFDatabaseModelDatabaseContext context = null)
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


                var updateModel = new AccountEntity
                {
                    Id = model.Id,
                    EntityName = model.EntityName,
                    Phone1 = model.Phone1,
                    PhysicalAddress = model.PhysicalAddress,
                    IdentificationDocumentNumber = model.IdentificationDocumentNumber,
                    Email = model.Email,
                    IsActive = model.IsActive
                };
                executionResult = await base.Update(updateModel, context);

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
         
        public async override Task<ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>> GetKendoGridFiltered(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null
        )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var list = new List<AccountEntity>();
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
                var entity = await context.AccountEntities.FirstOrDefaultAsync(
                    r => r.Id == UserPrinciple.User.AccountEntityId
                );
                var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                    r => r.Id == entity.SubAccountBranchId
                );
                var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                    r => r.Id == branchi.SubAccountId
                );
                var accountType = context.AccountEntities
                    .Join(context.AccountEntityTypes, d => d.AccountEntityTypeId, e => e.Id, (d, e) => new { d, e })
                    .Where(s => s.d.Id == entity.Id)

                    .FirstOrDefault();
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
                        .Where(r => r.s.ParentId == subacc.Id && r.x.ae.IsActive) //r=>r==true
                                                                                  //OrderByDescending(r => r.x.ae.IsActive)
                        .Skip(0)
                        .Take(args.take) // * args.page
                        .OrderBy(r => r.x.ae.Id)
                        .ToListAsync()
                        ).Select(
                        r => new AccountEntity
                        {
                            Id = r.x.ae.Id,
                            Phone1 = r.x.ae.Phone1,
                            Email = r.x.ae.Email,
                            IsActive = r.x.ae.IsActive,
                            EntityUserName = r.x.ae.EntityUserName,
                            EntityName = r.x.ae.EntityName,
                            PhysicalAddress = r.x.ae.PhysicalAddress,
                            EntityNo = r.x.ae.EntityNo,
                            Phone2 = r.x.ae.Phone2,
                            PostalAddress = r.x.ae.PostalAddress,
                            //SubAccountBranch = new SubAccountBranch
                            //{
                            //    SubAccount = new SubAccount
                            //    {
                            //        AccountName = r.s.AccountName
                            //    }
                            //}
                        }
                );
                

                //foreach (var item in accentity)
                //{
                //    var balModel = new MfsEntityAccount
                //    {
                //        AccountEntityId = item.Id,
                //        MfsAccountType = new MfsAccountType
                //        {
                //            AccountTypeName = DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName

                //        }

                //    };
                //    var bal = await GetBalance(balModel);
                //    var balance = bal.Result;

                //    if (balance == -1.0)
                //        balance = 0.0;

                //    //reused EntityNo property so as to carry the balance to the frontend
                //    item.EntityNo = balance.ToString();

                //    //var retainerFeeViaAgent = ContextConnectionService._configuration["AccountTypeFees:RetainerFeeViaAgent"];
                //    //var feeViaAgent = double.Parse(retainerFeeViaAgent);

                //    //var calculatedBal = balance - feeViaAgent;
                //    var calculatedBal = balance;
                //    var status = "N/A"; //default Fee status is unpaid

                //    //if (calculatedBal > 0)
                //    //{
                //    //    status = "OverPaid";
                //    //}
                //    //if (calculatedBal == 0.0 && balance != 0) // for the case there's no account, balance = 0 hence status is unpaid still
                //    //{
                //    //    status = "Paid";
                //    //}

                //    //if (calculatedBal < 0 && Math.Abs(calculatedBal) != feeViaAgent)
                //    //{
                //    //    status = "Partially Paid";
                //    //}

                //    //reused Phone2 property so as to carry the Fee status to the frontend
                //    item.Phone2 = status;

                //    //reused PostalAddress property so as to carry the accountname to the frontend
                //    item.PostalAddress = item.SubAccountBranch.SubAccount.AccountName;

                //    list.Add(item);
                //}



                var filterSupporter = new FilterSupporter<AccountEntity>(list, args);
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

        //public async Task<ExecutionResult<Balances>> SendClaim([FromBody]
        //  AccountEntity model,
        //  EFDatabaseModelDatabaseContext context = null
        //)
        //{
        //    var createContextHere = context == null;
        //    var executionResult = new ExecutionResult<Balances>(true);
        //    try
        //    {
        //        if (createContextHere)
        //            context = new EFDatabaseModelDatabaseContext();

        //        var accountEntityId = UserPrinciple.User.AccountEntityId;
        //        //var accountEntityType = context.AccountEntityTypes
        //        //    .Join(context.AccountEntities)

        //        var balModel = new MfsEntityAccount
        //        {
        //            AccountEntityId = model.Id,
        //            MfsAccountType = new MfsAccountType
        //            {
        //                AccountTypeName = DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName
        //            }
        //        };
        //        var result = await GetBalance(balModel);

        //        var retainerFeeViaAgent = ContextConnectionService._configuration["AccountTypeFees:RetainerFeeViaAgent"]; //extract the various fees from appsettings

        //        var feeViaAdmin = double.Parse(retainerFeeViaAgent);

        //        if (result.Result != feeViaAdmin)
        //        {
        //            executionResult.IsOkay = false;
        //            executionResult.Message = "Your customer must have finished paying up";
        //            return executionResult;
        //        }

        //        var agent = await context.AccountEntities.FirstOrDefaultAsync(
        //             a => a.Id == accountEntityId);

        //        //if (balance. == null)
        //        //{
        //        //    {
        //        //        executionResult.IsOkay = false;
        //        //        executionResult.Message = "Your clients Email is not defined";
        //        //        return executionResult;
        //        //    }
        //        //}


        //        var smsRecord = new SchoolMsSmsToSend
        //        {
        //            SenderId = "Claim", // Set Title into sender_id column
        //            PhoneNumber = model.Phone1, // Set ReceiverNo into phone_number column
        //            Message = " ", // Set Message into message column
        //            EntityId = accountEntityId, // Set accountEntityId into entity_id column
        //            SentTime = DateTime.UtcNow,
        //            BatchJobId = "0",
        //        };
        //        //Save the new SMS record to the database
        //        context.SchoolMsSmsToSends.Add(smsRecord);
        //        if (createContextHere)
        //        {
        //            await context.SaveChangesAsync();
        //            var adminEmail = ContextConnectionService._configuration["AppConstants:AdminEmail"];
        //            var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
        //            var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
        //            var request = new MailRequest
        //            {
        //                ToEmail = adminEmail,
        //                Subject = $" Commission Claim Request for Agent {agent.EntityName}",
        //                Body = $"I hope this email finds you well. I am writing to formally request the payment of the commission owed to me for the successful completion of Payment of my Client. As per our agreed-upon terms " +
        //                $", it is my understanding that I am entitled to a commission of Retainer Fee for this transaction. <br>" +
        //                $"<br>" +
        //                $"To provide you with the necessary details for My Client Info :" +
        //                $"Name : {model.EntityName} <br>" +
        //                $"Phone Number : {model.Phone1}" +
        //                $"Email : {model.Email}" +
        //                $"<br>" +
        //                $"<br>" +
        //                $"I appreciate your prompt attention to this matter, and I trust that the commission payment will be processed in a timely manner, as agreed upon." +
        //                $" Please let me know the expected timeline for the payment, and if there are any specific forms or procedures I need to follow to facilitate the transfer. <br>" +
        //                $"<br>" +
        //                $"Thank you for your cooperation, and I look forward to your positive response. I value our business relationship and wish to maintain it on a mutually beneficial basis. <br>" +
        //                $"<br>" +
        //                $"Sincerely," +
        //                $"<br>" +
        //                $"{agent.EntityName} <br>" +
        //                $"{agent.Phone1}",
        //                From = FromEmail
        //            };

        //            var uri = new Uri(Domain);
        //            var client = new RestClient(uri);
        //            client.AddDefaultHeader("Content-Type", "application/json");
        //            client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
        //                                                                   //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
        //            var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

        //            reques.AddJsonBody(request);

        //            var restClientResult = await client.ExecuteAsync<MailRequest>(reques);

        //            executionResult.IsOkay = true;
        //            executionResult.Message = "Claim Sent successfully";

        //        }

        //        return executionResult;



        //    }
        //    catch (Exception ex)
        //    {
        //        executionResult.Message = $"Error: {ex.Message}";
        //        return executionResult;
        //    }
        //}

        //public async Task<ExecutionResult<Balances>> SendReversal([FromBody]
        //    AccountEntity model, 
        //    EFDatabaseModelDatabaseContext context = null
        //    )
        //{
        //    var createContextHere = context == null;
        //    var executionResult = new ExecutionResult<Balances>(true);
        //    try
        //    {
        //        if (createContextHere)
        //            context = new EFDatabaseModelDatabaseContext();

        //        var accountEntityId = UserPrinciple.User.AccountEntityId;
        //        //var accountEntityType = context.AccountEntityTypes
        //        //    .Join(context.AccountEntities)

        //        var balModel = new MfsEntityAccount
        //        {
        //            AccountEntityId = model.Id,
        //            MfsAccountType = new MfsAccountType
        //            {
        //                AccountTypeName = DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName
        //            }
        //        };
        //        var result = await GetBalance(balModel);

        //        if (result.Result == 0.0)
        //        {
        //            executionResult.IsOkay = false;
        //            executionResult.Message = "Your customer has not overpaid";
        //            return executionResult;
        //        }

        //        var agent = await context.AccountEntities.FirstOrDefaultAsync(
        //             a => a.Id == accountEntityId);

        //        //if (balance. == null)
        //        //{
        //        //    {
        //        //        executionResult.IsOkay = false;
        //        //        executionResult.Message = "Your clients Email is not defined";
        //        //        return executionResult;
        //        //    }
        //        //}


        //        var smsRecord = new SchoolMsSmsToSend
        //        {
        //            SenderId = "Reverse", // Set Title into sender_id column
        //            PhoneNumber = model.Phone1, // Set ReceiverNo into phone_number column
        //            Message = $"Reversal for {model.Id}", // Set Message into message column
        //            EntityId = accountEntityId, // Set accountEntityId into entity_id column
        //            SentTime = DateTime.UtcNow,
        //            BatchJobId = "0",
        //        };
        //        //Save the new SMS record to the database
        //        context.SchoolMsSmsToSends.Add(smsRecord);
        //        if (createContextHere)
        //        {
        //            await context.SaveChangesAsync();
        //            //var adminEmail = ContextConnectionService._configuration["AppConstants:AdminEmail"];
        //            var adminEmail = ContextConnectionService._configuration["AppConstants:AdminEmail"];
        //            var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
        //            var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
        //            var request = new MailRequest
        //            {
        //                ToEmail = adminEmail,
        //                Subject = $"Request for Reversal of Overpayment From  Agent {agent.EntityName} ",
        //                Body = $"I hope this email finds you well. I am writing to bring to your attention an issue that requires immediate resolution." +
        //                $" It has come to our attention that there has been an overpayment on our account, and we kindly request your assistance in facilitating the reversal of this overpayment. <br>" +
        //                $"<br>" +
        //                $"To provide you with the necessary details for reference, please find below the relevant information: <br>" +
        //                $"Client Name : {model.EntityName} <br>" +
        //                $"Client Phone Number : {model.Phone1} <br>" + 
        //                $"Client Email : {model.Email} <br>" +
        //                $"<br>" +
        //                $"We understand that errors can occur, and we greatly appreciate your prompt attention to this matter. Our records indicate that the overpayment was unintentional and is the result of an accounting error." +
        //                $" We kindly request that you initiate the process to refund the excess amount at your earliest convenience. <br> " +
        //                $"<br>" +
        //                $"If there are any specific procedures or forms that we need to follow to facilitate this reversal, please do not hesitate to share them with us. We are committed to working closely with your team to ensure that this issue is resolved efficiently. <br>" +
        //                $" <br>" +
        //                $"We value our business relationship with your company and appreciate your prompt attention to this matter. Your cooperation in resolving this issue is of utmost importance to us.<br>" +
        //                $"Thank you for your understanding and prompt action. We look forward to a swift resolution and confirmation of the overpayment reversal.<br>" +
        //                $"Sincerely,<br>" +
        //                $"<br>" +
        //                $"{agent.EntityName} <br>" +
        //                $"{agent.Phone1}",

        //                From = FromEmail
        //            };

        //            var uri = new Uri(Domain);
        //            var client = new RestClient(uri);
        //            client.AddDefaultHeader("Content-Type", "application/json");
        //            client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
        //                                                                   //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
        //            var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

        //            reques.AddJsonBody(request);

        //            var restClientResult = await client.ExecuteAsync<MailRequest>(reques);

        //            executionResult.IsOkay = true;
        //            executionResult.Message = "Reversal Sent successfully";

        //        }

        //        return executionResult;



        //    }
        //    catch (Exception ex)
        //    {
        //        executionResult.Message = $"Error: {ex.Message}";
        //        return executionResult;
        //    }
        //}


        public async Task<
            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
        > GetAgents(
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
                        .Join(context.AccountEntityTypes,
                        az => az.x.ae.AccountEntityTypeId,
                        u => u.Id,
                        (az, u) => new { az, u }
                        )
                        .Where(r => r.u.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName) //r=>r==true
                                                                                                          //OrderByDescending(r => r.x.ae.IsActive)
                        .Skip(0)
                        .Take(args.take) // * args.page
                        .OrderBy(r => r.az.x.ae.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntity
                        {
                            Id = r.az.x.ae.Id,
                            Phone1 = r.az.x.ae.Phone1,
                            Email = r.az.x.ae.Email,
                            IsActive = r.az.x.ae.IsActive,
                            EntityUserName = r.az.x.ae.EntityUserName,
                            EntityName = r.az.x.ae.EntityName,
                            PhysicalAddress = r.az.x.ae.PhysicalAddress

                        }
                );

                var filterSupporter = new FilterSupporter<AccountEntity>(accentity, args);
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

        public async Task<
           ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
       > GetAllCustomers(
           NeedDataSourceEventArgs args,
           EFDatabaseModelDatabaseContext context = null
       )
        {
            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
            var createContextHere = context == null;
            var list = new List<AccountEntity>();
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
                        .Join(context.AccountEntityTypes, az => az.x.ae.AccountEntityTypeId, u => u.Id, (az, u) => new { az, u })
                        .Where(r => r.u.TypeName == DefaultConfiguration.AccountEntityType.InternTypeName) //r=>r==true
                                                                                                             //OrderByDescending(r => r.x.ae.IsActive)
                        .Skip(0)
                        .Take(args.take) // * args.page
                        .OrderBy(r => r.az.x.ae.Id)
                        .ToListAsync()
                ).Select(
                    r =>
                        new AccountEntity
                        {
                            Id = r.az.x.ae.Id,
                            Phone1 = r.az.x.ae.Phone1,
                            Email = r.az.x.ae.Email,
                            IsActive = r.az.x.ae.IsActive,
                            EntityUserName = r.az.x.ae.EntityUserName,
                            EntityName = r.az.x.ae.EntityName,
                            PhysicalAddress = r.az.x.ae.PhysicalAddress,
                            //SubAccountBranch = new SubAccountBranch
                            //{
                            //    SubAccount = new SubAccount
                            //    {
                            //        ParentId = r.az.s.ParentId,
                            //        // Fetch the account name based on ParentId
                            //        AccountName = context.SubAccounts
                            //    .Where(sa => sa.Id == r.az.s.ParentId)
                            //    .Select(sa => sa.AccountName)
                            //    .FirstOrDefault()
                            //    }
                            //}
                        }
                );

                //foreach (var item in accentity)
                //{
                //    var balModel = new MfsEntityAccount
                //    {
                //        AccountEntityId = item.Id,
                //        MfsAccountType = new MfsAccountType
                //        {
                //            AccountTypeName = DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName

                //        }

                //    };
                //    var bal = await GetBalance(balModel);
                //    var balance = bal.Result;

                //    if (balance == -1.0)
                //        balance = 0.0;

                //    //reused EntityNo property so as to carry the balance to the frontend
                //    item.EntityNo = balance.ToString();

                //    //var retainerFeeViaAgent = ContextConnectionService._configuration["AccountTypeFees:RetainerFeeViaAgent"];
                //    //var feeViaAgent = double.Parse(retainerFeeViaAgent);

                //    //var calculatedBal = balance - feeViaAgent;

                //    var calculatedBal = balance;

                //    var status = "N/A"; //default Fee status is unpaid
                //    //
                //    //if (calculatedBal > 0)
                //    //{
                //    //    status = "OverPaid";
                //    //}
                //    //if (calculatedBal == 0.0 && balance != 0) // for the case there's no account, balance = 0 hence status is unpaid still
                //    //{
                //    //    status = "Paid";
                //    //}

                //    //if (calculatedBal < 0 && Math.Abs(calculatedBal) != feeViaAgent)
                //    //{
                //    //    status = "Partially Paid";
                //    //}

                //    //reused Phone2 property so as to carry the Fee status to the frontend
                //    item.Phone2 = status;

                //    ////reused PostalAddress property so as to carry the accountname to the frontend
                //    //item.PostalAddress = item.SubAccountBranch.SubAccount.AccountName;

                //    list.Add(item);
                //}




                var filterSupporter = new FilterSupporter<AccountEntity>(accentity, args);
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

        //public async Task<ExecutionResult<double>> GetBalance(MfsEntityAccount model, EFDatabaseModelDatabaseContext context = null)
        //{
        //    //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
        //    var createContextHere = context == null;

        //    var executionResult = new ExecutionResult<double>(true);



        //    try
        //    {
        //        if (createContextHere)
        //            context = new EFDatabaseModelDatabaseContext();

        //        var accountExist = await context.MfsEntityAccounts
        //                .Join(context.MfsAccountTypes, ea => ea.MfsAccountTypeId,
        //                    mfsAccountType => mfsAccountType.Id,
        //                    (ea, mfsAccountType) => new { ea, mfsAccountType }
        //                    )
        //                .Where(
        //                   jk =>
        //                    jk.mfsAccountType.AccountTypeName == model.MfsAccountType.AccountTypeName
        //                    && jk.ea.AccountEntityId == model.AccountEntityId
        //                )
        //                .FirstOrDefaultAsync();

        //        if (accountExist == null)
        //        {
        //            //because it is the account that doesn't exist but the balance calculation was a success
                   
        //            executionResult.Result = -1.0;
        //            executionResult.IsOkay = true;
        //            return executionResult;
        //        }
        //        var accountBal = await context.MfsEntityAccountTransactions
        //                    .Join(context.MfsEntityAccounts,
        //                        transactions => transactions.MfsEntityAccountId,
        //                        mfsEntityAccount => mfsEntityAccount.Id,
        //                        (transactions, mfsEntityAccount) => new { transactions, mfsEntityAccount }
        //                     )
        //                    .Where(
        //                          ts => ts.mfsEntityAccount.Id == accountExist.ea.Id && ts.transactions.Posted == true
        //                     )
        //                    .SumAsync(w => w.transactions.AmountInCredit - w.transactions.AmountOutDebit);

        //        executionResult.Message = "Balance calculated succesfully";
        //        executionResult.Result = accountBal;
        //        executionResult.IsOkay = true;
        //        return executionResult;

        //    }
        //    catch (Exception ex)
        //    {
        //        //if an error occured copy its data from the exception object into the execution result by
        //        executionResult.CopyFromException(ex);
        //    }
        //    finally
        //    {
        //        //incase the context was created in this method then dispose it here
        //        if (createContextHere)
        //            await context.DisposeAsync();
        //    }
        //    return executionResult;
        //}

        public async Task<ExecutionResult<AccountEntity>> BulkAdd(
           AccountEntity model,
           EFDatabaseModelDatabaseContext context = null

            )
        {

            //var createContextHere = context == null;
            var executionResult = new ExecutionResult<AccountEntity>(true);

            try
            {
                //if (createContextHere)
                //    context = new EFDatabaseModelDatabaseContext();

                //foreach (var entity in model)
                //{
                //    var addaresult = await Add(entity);
                //    if (addaresult.IsOkay == false)
                //    {
                //        executionResult.Message = $"{executionResult.Message} {entity.EntityName} failed with error message : {addaresult.Message}";
                //        executionResult.IsOkay = false;
                //    }
                //    //executionResult.Result.Add(addaresult.Result);


                //}
                var addaresult = await Add(model);
                if (addaresult.IsOkay == false)
                {
                    executionResult.Message = $"{executionResult.Message} {model.EntityName} failed with error message : {addaresult.Message}";
                    executionResult.IsOkay = false;
                }
                if (executionResult.IsOkay == true)
                {
                    executionResult.Message = "Accounts added successfully!";

                }
                else
                    executionResult.IsOkay = true;

            }
            catch (Exception ex)
            {
                executionResult.CopyFromException(ex);
                _ = ex.ToString();


            }

            return executionResult;

        }
        public async Task<ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>> GetPayments(
            NeedDataSourceEventArgs args,
            EFDatabaseModelDatabaseContext context = null)
        {
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
                var entity = await context.AccountEntities.FirstOrDefaultAsync(
                    r => r.Id == UserPrinciple.User.AccountEntityId
                );
                var branchi = await context.SubAccountBranches.FirstOrDefaultAsync(
                    r => r.Id == entity.SubAccountBranchId
                );
                var subacc = await context.SubAccounts.FirstOrDefaultAsync(
                    r => r.Id == branchi.SubAccountId
                );

                var checkPayments = await context.MfsEntityAccountTransactions
                    .Join(context.MsfSystemTransactionReceipts,
                       a => a.MfsSystemTransactionReceiptId,
                       ab => ab.Id,
                       (a, ab) => new { a, ab })
                    .Join(context.MsfAccountPaymodes,
                       e => e.a.MsfAccountPaymodeId,
                       ae => ae.Id,
                       (e, ae) => new { e, ae })
                    .Join(context.MfsEntityAccounts,
                       b => b.e.a.MfsEntityAccountId,
                       ab => ab.Id,
                       (b, ab) => new { b, ab })
                    .Join(context.AccountEntities,
                       c => c.ab.AccountEntityId,
                       bc => bc.Id,
                       (c, bc) => new { c, bc })
                    .Join(context.SubAccountBranches,
                       d => d.bc.SubAccountBranchId,
                       da => da.Id,
                       (d, da) => new { d, da })
                    .Join(context.SubAccounts,
                       de => de.da.SubAccountId,
                       s => s.Id,
                       (de, s) => new { de, s })
                    .Where(r => r.s.ParentId == subacc.Id && r.de.d.c.b.e.a.Posted == true)

                    .Skip(0)
                    .Take(args.take) // * args.page
                    .OrderBy(r => r.de.d.c.b.e.a.Id)
                   .Select(n => new MfsEntityAccountTransaction
                   {
                       Id = n.de.d.c.b.e.a.Id,
                       MfsEntityAccount = new MfsEntityAccount
                       {
                           AccountEntity = new AccountEntity
                           {
                               EntityName = n.de.d.bc.EntityName,
                               EntityUserName = n.de.d.bc.EntityUserName,
                           }
                       },
                       MsfAccountPaymode = new MsfAccountPaymode
                       {
                           PaymodeName = n.de.d.c.b.ae.PaymodeName,
                       },
                       AmountInCredit = n.de.d.c.b.e.a.AmountInCredit,
                       Narration = n.de.d.c.b.e.a.Narration,
                       MfsSystemTransactionReceipt = new MsfSystemTransactionReceipt
                       {
                           ReceiptNo = n.de.d.c.b.e.ab.ReceiptNo
                       },
                       TransactionDate = n.de.d.c.b.e.a.TransactionDate
                   })
                    .ToListAsync();
                var filterSupporter = new FilterSupporter<MfsEntityAccountTransaction>(checkPayments, args);
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
        public async Task<ExecutionResult<MfsEntityAccountTransaction>> ConfirmPayments(MfsEntityAccountTransaction model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {

                        var executionResult = new ExecutionResult<MfsEntityAccountTransaction>(true);

                        var existingTransaction = await context.MfsEntityAccountTransactions.FindAsync(model.Id);

                        if (existingTransaction == null)
                        {
                            // Transaction not found, you can handle this case as required (throw exception, return null, etc.).
                            return null;
                        }
                        if (existingTransaction.Posted == true && existingTransaction.ValueDate == null)
                        {
                            existingTransaction.Posted = true;
                            existingTransaction.ValueDate = DateTime.UtcNow;
                            await context.SaveChangesAsync();
                            executionResult.Message = "Payments Confirmed Successfully.";
                        }


                        return executionResult;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while confirming Payments", ex);
                }
            }

        }

        public async Task<ExecutionResult<MfsEntityAccountTransaction>> RejectPayments(MfsEntityAccountTransaction model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {

                        var executionResult = new ExecutionResult<MfsEntityAccountTransaction>(true);

                        var existingTransaction = await context.MfsEntityAccountTransactions.FindAsync(model.Id);

                        if (existingTransaction == null)
                        {
                            // Transaction not found, you can handle this case as required (throw exception, return null, etc.).
                            return null;
                        }
                        if (existingTransaction.Posted == true)
                        {
                            existingTransaction.Posted = false;
                            await context.SaveChangesAsync();
                            executionResult.Message = "Payments Rejected Successfully.";
                        }


                        return executionResult;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred while confirming Payments", ex);
                }
            }

        }

        //public async Task<ExecutionResult<Balances>> SendClaimCommission(Balances model, EFDatabaseModelDatabaseContext context = null)
        //{
        //    var createContextHere = context == null;
        //    var executionResult = new ExecutionResult<Balances>(true);
        //    try
        //    {
        //        if (createContextHere)
        //            context = new EFDatabaseModelDatabaseContext();

        //        var accountEntityId = UserPrinciple.User.AccountEntityId;
        //        //var accountEntityType = context.AccountEntityTypes
        //        //    .Join(context.AccountEntities)
        //        //var Title = "Claim";
        //        var content = new NotificationModel();

        //        content.Title = "Claim";
        //        content.Message = $"The following agent: {accountEntityId}, is claiming their commission for {model.UserName} who has completed their retainer fee.";

        //        var adminEmail = "ivyjane953@gmail.com";
        //        if (content.ReceiverEmail == null && content.Title == "Claim"/* && model.ReceiverNo == null*/)
        //        {
        //            //model.ReceiverEmail = "ivyjane953@gmail.com"/*ContextConnectionService._configuration["AppConstants:AdminEmail"]*/;

        //            var getSender = await context.AccountEntities.FirstOrDefaultAsync(
        //                 a => a.Email == adminEmail);
        //            if (getSender != null)
        //            {
        //                content.ReceiverNo = getSender.Phone1;
        //            }
        //            else
        //            {
        //                executionResult.IsOkay = false;
        //                executionResult.Message = "Email does not exist";
        //                return executionResult;
        //            }
        //        }

        //        if (content.ReceiverEmail != null)
        //        {
        //            executionResult.IsOkay = false;
        //            executionResult.Message = "Wrong Email.";
        //            return executionResult;

        //        }
        //        content.ReceiverEmail = adminEmail;
        //        var smsRecord = new SchoolMsSmsToSend
        //        {
        //            SenderId = content.Title, // Set Title into sender_id column
        //            PhoneNumber = content.ReceiverNo, // Set ReceiverNo into phone_number column
        //            Message = content.Message, // Set Message into message column
        //            EntityId = accountEntityId, // Set accountEntityId into entity_id column
        //            SentTime = DateTime.UtcNow,
        //            BatchJobId = "0",
        //        };
        //        // Save the new SMS record to the database
        //        context.SchoolMsSmsToSends.Add(smsRecord);
        //        if (createContextHere)
        //        {
        //            await context.SaveChangesAsync();

        //            var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
        //            var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
        //            var request = new MailRequest
        //            {
        //                ToEmail = content.ReceiverEmail,
        //                Subject = content.Title,
        //                Body = content.Message,

        //                From = FromEmail
        //            };

        //            var uri = new Uri(Domain);
        //            var client = new RestClient(uri);
        //            client.AddDefaultHeader("Content-Type", "application/json");
        //            client.AddDefaultHeader("Accept", "application/json"); //ensure the application goes with the application/json header for accept
        //                                                                   //client.AddDefaultHeader("Authorization", $"Basic {bearerToken}");
        //            var reques = new RestRequest("/Email/Mail/SendMail", Method.POST); //create an object to send this request just compliments the RestClient

        //            reques.AddJsonBody(request);

        //            var restClientResult = await client.ExecuteAsync<MailRequest>(reques);

        //            executionResult.IsOkay = true;
        //            executionResult.Message = "Message Sent successfully";

        //        }

        //        return executionResult;


        //    }
        //    catch (Exception ex)
        //    {
        //        executionResult.Message = $"Error: {ex.Message}";
        //        return executionResult;
        //    }


        //}

        //public async Task<ExecutionResult<Balances>> GetCustomerBalance(MfsEntityAccount model, EFDatabaseModelDatabaseContext context = null)
        //{
        //    var createContextHere = context == null;
        //    var executionResult = new ExecutionResult<Balances>(true);

        //    try
        //    {
        //        if (createContextHere)
        //            context = new EFDatabaseModelDatabaseContext();

        //        var accountEntityId = model.AccountEntityId;

        //        if (accountEntityId == 0)
        //        {
        //             accountEntityId =  UserPrinciple.User.AccountEntityId;
        //             model.AccountEntityId = accountEntityId;
        //        }
               

        //        var loggedInSubAccount = await context.SubAccounts
        //            .Join(context.SubAccountBranches, subAccounts => subAccounts.Id, subAccountBranches => subAccountBranches.SubAccountId, (subAccounts, subAccountBranches) => new { subAccounts, subAccountBranches })
        //            .Join(context.AccountEntities, x => x.subAccountBranches.Id, entities => entities.SubAccountBranchId, (x, entities) => new { x, entities })
        //            .Where(r => r.entities.Id == accountEntityId)
        //            .FirstOrDefaultAsync();

        //        //check the parent type
        //        var parentType = await context.SubAccounts
        //            .Join(context.SubAccountBranches, sa => sa.Id, sab => sab.SubAccountId, (sa, sab) => new { sa, sab })
        //            .Join(context.AccountEntities, s => s.sab.Id, ae => ae.SubAccountBranchId, (s, ae) => new { s, ae })
        //            .Join(context.AccountEntityTypes, z => z.ae.AccountEntityTypeId, aet => aet.Id, (z, aet) => new { z, aet })
        //            .Where(r => r.z.s.sa.Id == loggedInSubAccount.x.subAccounts.ParentId)
        //            .FirstOrDefaultAsync();

        //        if (parentType == null)
        //        {
        //            executionResult.Message = "Kindly got to the Home Page to choose an agent or admin before performing any kind of payments";
        //            executionResult.IsOkay = false;
        //            return executionResult;
        //        }
        //        var balModel = model;

        //        var status = "N/A"; //default fee status is unpaid

        //        //call the GetBalance method
        //        var bal = await GetBalance(balModel);
        //        var balance = bal.Result;

        //        var calculatedBal = 0.0;

        //        if (balance == -1.0)
        //            balance = 0.0;
        //        calculatedBal = balance;

        //        if (model.MfsAccountType.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName)
        //        {
        //            //check account entity type , either agent or admin, to determine the fee they will pay
        //            //check for the case a customer is under agent
        //            //if (parentType.aet.TypeName == DefaultConfiguration.AccountEntityType.adminTypeName)
        //            //{
        //            //    var retainerFeeViaAdmin = ContextConnectionService._configuration["AccountTypeFees:RetainerFeeViaAdmin"]; //extract the various fees from appsettings
        //            //
        //            //    var feeViaAdmin = double.Parse(retainerFeeViaAdmin);
        //            //
        //            //    calculatedBal = balance - feeViaAdmin;
        //            //    calculatedBal = balance;
        //            //
        //            //    if (Math.Abs(calculatedBal) < feeViaAdmin && calculatedBal < 0)
        //            //        status = "Paid Partially";
        //            //}
        //            ////check for the case a customer is under agent
        //            //if (parentType.aet.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName)
        //            //{
        //            //    var retainerFeeViaAgent = ContextConnectionService._configuration["AccountTypeFees:RetainerFeeViaAgent"];
        //            //    var feeViaAgent = double.Parse(retainerFeeViaAgent);
        //            //    calculatedBal = balance - feeViaAgent;
        //            //
        //            //    if (Math.Abs(calculatedBal) < feeViaAgent && calculatedBal < 0)
        //            //        status = "Paid Partially";
        //            //    calculatedBal = balance;
        //            //}
        //        }

        //        if (model.MfsAccountType.AccountTypeName == DefaultConfiguration.MfsAccountTypeName.applicationAccountTypeName)
        //        {
        //            //var ApplicationFee = ContextConnectionService._configuration["AccountTypeFees:ApplicationFee"];
        //            //var applicationFee = double.Parse(ApplicationFee);
        //            //calculatedBal = balance - applicationFee;
        //            //
        //            //if (Math.Abs(calculatedBal) < applicationFee && calculatedBal < 0)
        //            //    status = "Paid Partially";
        //        }

        //        //if (calculatedBal > 0)
        //        //{
        //        //    status = "OverPaid";
        //        //}
        //        //if (calculatedBal == 0.0 && balance != 0) // for the case there's no account, balance = 0 hence status is unpaid still
        //        //{
        //        //    status = "Paid";
        //        //}

        //        executionResult.Result = new Balances
        //        {
        //            Balance = calculatedBal,
        //            Status = status
        //        };
        //        executionResult.Message = "Customer balances retrieved successfully.";
        //        executionResult.IsOkay = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        executionResult.Message = $"Error: {ex.Message}";

        //    }
        //    finally
        //    {
        //        // incase the context was created in this method then dispose it here
        //        if (createContextHere)
        //            await context.DisposeAsync();
        //    }
        //    return executionResult;
        //}

        //    public async Task<ExecutionResult<MfsEntityAccountTransaction>> GetAllCustomersBalance(MfsAccountType model, EFDatabaseModelDatabaseContext context = null)
        //    {
        //        var createContextHere = context == null;
        //        var executionResult = new ExecutionResult<MfsEntityAccountTransaction>(true);

        //        try
        //        {
        //            if (createContextHere)
        //                context = new EFDatabaseModelDatabaseContext();

        //            var accountEntity = await context.AccountEntities
        //                .Join(context.AccountEntityTypes, entity => entity.AccountEntityTypeId, accountType => accountType.Id, (entity, accountType) => new { entity, accountType })
        //           .FirstOrDefaultAsync(r => r.entity.Id == UserPrinciple.User.AccountEntityId);

        //            if (accountEntity.accountType.TypeName == DefaultConfiguration.AccountEntityType.adminTypeName)
        //            {

        //            }

        //            var balModel = new MfsEntityAccount
        //            {
        //                AccountEntityId = accountEntity.entity.Id,
        //                MfsAccountType = new MfsAccountType
        //                {
        //                    AccountTypeName = model.AccountTypeName
        //                }
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            executionResult.Message = $"Error: {ex.Message}";

        //        }
        //        finally
        //        {
        //            // incase the context was created in this method then dispose it here
        //            if (createContextHere)
        //                await context.DisposeAsync();
        //        }
        //        return executionResult;
        //    }
        //}
    
    }
}



