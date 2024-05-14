using EFDatabaseModel.Contexts;
using EFDatabaseModel.Repository;
using LukurareBackend.Models.Hubs;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using System.Threading.Tasks;
using EFDatabaseModel.DbModel;
using System;
using RestSharp;
using ProjectBase.Database.Connection;
using System.Linq;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


using System.Security.Cryptography;
using EFDatabaseModel.SmartMeterModels;
using Microsoft.AspNetCore.Mvc;
using LukurareBackend.Models;
using System.Diagnostics;

namespace LukurareBackend.Repositories.Hub
{
    public class ChatRepository : EFDatabaseModelBaseRepository<NotificationModel>
    {
        private string currentModule;

        public ChatRepository(string currentModule)
           : base(currentModule)
        {

            this.currentModule = currentModule;


        }
        public async Task<ExecutionResult<NotificationModel>> SendNotification(NotificationModel model, EFDatabaseModelDatabaseContext context = null)
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<NotificationModel>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var accountEntityId = UserPrinciple.User.AccountEntityId;
                //var accountEntityType = context.AccountEntityTypes
                //    .Join(context.AccountEntities)
               
                
                if (model.ReceiverEmail == null/* && model.ReceiverNo == null*/)
                {
                    executionResult.IsOkay = false;
                    executionResult.Message = "Email is required";
                    return executionResult;
                }

                if (model.ReceiverEmail != null)
                {
                    var getSender = await context.AccountEntities.FirstOrDefaultAsync(
                        a => a.Email == model.ReceiverEmail);
                    if (getSender != null)
                    {
                        model.ReceiverNo = getSender.Phone1;
                    }
                    else
                    {
                        executionResult.IsOkay = false;
                        executionResult.Message = "Email does not exist";
                        return executionResult;
                    }
                }


                var smsRecord = new SchoolMsSmsToSend
                {
                    SenderId = model.Title, // Set Title into sender_id column
                    PhoneNumber = model.ReceiverNo, // Set ReceiverNo into phone_number column
                    Message = model.Message, // Set Message into message column
                    EntityId = accountEntityId, // Set accountEntityId into entity_id column
                    SentTime = DateTime.UtcNow,
                    BatchJobId = "0",
                };
                // Save the new SMS record to the database
                context.SchoolMsSmsToSends.Add(smsRecord);
                if (createContextHere)
                {
                    await context.SaveChangesAsync();

                    var Domain = ContextConnectionService._configuration["AppConstants:Domain"];
                    var FromEmail = ContextConnectionService._configuration["AppConstants:FromEmail"];
                    var request = new MailRequest
                    {
                        ToEmail = model.ReceiverEmail,
                        Subject = model.Title,
                        Body = model.Message,

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

                        executionResult.IsOkay = true;
                        executionResult.Message = "Message Sent successfully";

                }

                return executionResult;


            }
            catch (Exception ex)
            {
                executionResult.Message = $"Error: {ex.Message}";
                return executionResult;
            }


        }


        public async Task<ExecutionResult<List<SchoolMsSmsToSend>>> GetNotifications(EFDatabaseModelDatabaseContext context = null)
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<List<SchoolMsSmsToSend>>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();
                var accountEntityId = UserPrinciple.User.AccountEntityId;
                var getReceiverNo = await context.AccountEntities.FindAsync(accountEntityId);
                if (getReceiverNo != null)
                {
                    var receiverNo = getReceiverNo.Phone1;
                    // Retrieve records from the school_ms_sms_to_send table based on the provided receiverNo
                    var getNotifications = await context.SchoolMsSmsToSends
                    .Join(context.AccountEntities,
                       a => a.EntityId, b => b.Id,
                       (a, b) => new { a, b })
                    .Where(sms => sms.a.PhoneNumber == receiverNo && sms.a.BatchJobId == "0")
                    .Select(m => new SchoolMsSmsToSend
                    {
                        Id = m.a.Id,
                        SenderId = m.a.SenderId,
                        PhoneNumber = m.a.PhoneNumber,
                        Message = m.a.Message,
                        SentTime = m.a.SentTime,
                        BatchJobId = m.a.BatchJobId,
                        Entity = new AccountEntity
                        {
                            EntityName = m.b.EntityName,
                            Phone1 = m.b.Phone1,
                            Email = m.b.Email
                        }
                    })
                    .ToListAsync();
                    executionResult.Result = getNotifications;

                }

                return executionResult;
            }
            catch (Exception ex)
            {

                executionResult.Message = $"Error: {ex.Message}";
                return executionResult;
            }
        }
        public async Task<ExecutionResult<SchoolMsSmsToSend>> UpdateNotification(SchoolMsSmsToSend model)
        {
            {
                try
                {
                    using (EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext())
                    {

                        var executionResult = new ExecutionResult<SchoolMsSmsToSend>(true);

                        var notification = await context.SchoolMsSmsToSends.FindAsync(model.Id);

                        if (notification != null && model.BatchJobId == "0")
                        {
                            notification.BatchJobId = "1";
                            await context.SaveChangesAsync();
                        }



                        return executionResult;

                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"An exception occurred: {ex}");

                    // Example: Throw a custom exception
                    throw new Exception("An error occurred updating notification", ex);
                }
            }

        }

        public override Task<ExecutionResult<NotificationModel>> Add(NotificationModel model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new System.NotImplementedException();
        }

        public override NotificationModel CloneModel(NotificationModel model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ExecutionResult<NotificationModel>> GetItem(int id, EFDatabaseModelDatabaseContext context = null)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ExecutionResult<NotificationModel>> Update(NotificationModel model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new System.NotImplementedException();
        }

        //retrieving the users contact list 

        public async Task<ExecutionResult<List<User>>> GetContactList(EFDatabaseModelDatabaseContext context = null)
        {
            var createContextHere = context == null;

            var executionResult = new ExecutionResult<List<User>>(true);
            var list = new List<User>();

            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();

                var accountEntityId = UserPrinciple.User.AccountEntityId;

                var loggedInSubAccounts = await context.Users
                    .Join(context.AccountEntities, u => u.AccountEntityId, ae => ae.Id, (u, ae) => new { u, ae })
                    .Join(context.SubAccountBranches, a => a.ae.SubAccountBranchId, sab => sab.Id, (a, sab) => new { a, sab })
                    .Join(context.SubAccounts, x => x.sab.SubAccountId, sa => sa.Id, (x, sa) => new { x, sa })
                    .Where(m => m.x.a.ae.Id == accountEntityId)
                    .FirstOrDefaultAsync();


                var entityType = await context.AccountEntities
                    .Join(context.AccountEntityTypes, ae => ae.AccountEntityTypeId, aet => aet.Id, (ae, aet) => new { ae, aet })
                    .Where(b => b.ae.Id == accountEntityId && b.aet.TypeName == DefaultConfiguration.AccountEntityType.customerTypeName)
                    .FirstOrDefaultAsync();

                var isCustomer = true;

                //Retrieve agents records
                

                if (entityType == null)
                {
                    isCustomer = false;
                    entityType = await context.AccountEntities
                    .Join(context.AccountEntityTypes, ae => ae.AccountEntityTypeId, aet => aet.Id, (ae, aet) => new { ae, aet })
                    .Where(b => b.ae.Id == accountEntityId && b.aet.TypeName == DefaultConfiguration.AccountEntityType.agentTypeName)
                    .FirstOrDefaultAsync();

                      
                }

                if (entityType == null)
                {
                    isCustomer = false;
                    entityType = await context.AccountEntities
                    .Join(context.AccountEntityTypes, ae => ae.AccountEntityTypeId, aet => aet.Id, (ae, aet) => new { ae, aet })
                    .Where(b => b.ae.Id == accountEntityId && b.aet.TypeName == DefaultConfiguration.AccountEntityType.adminTypeName)
                    .FirstOrDefaultAsync();

                }




                if (entityType == null)
                {
                    executionResult.Message = $"Entity type not defined. ";
                    executionResult.IsOkay = false;

                    return executionResult;
                }
                if (isCustomer)
                {
                    var customerParent = await context.Users
                    .Join(context.AccountEntities, u => u.AccountEntityId, ae => ae.Id, (u, ae) => new { u, ae })
                    .Join(context.SubAccountBranches, a => a.ae.SubAccountBranchId, sab => sab.Id, (a, sab) => new { a, sab })
                    .Join(context.SubAccounts, x => x.sab.SubAccountId, sa => sa.Id, (x, sa) => new { x, sa })
                    .Where(m => m.sa.Id == loggedInSubAccounts.sa.ParentId)
                    .Select(b => new User
                    { 
                        Id = b.x.a.u.Id,
                        Username = b.x.a.u.Username,
                        AccountEntity = new AccountEntity
                        {
                            Id = b.x.a.ae.Id,
                            EntityUserName = b.x.a.ae.EntityUserName,
                            Phone1 = b.x.a.ae.Phone1,
                            ProfileImageUrl = b.x.a.ae.ProfileImageUrl
                        }
                    })
                    .FirstOrDefaultAsync();

                    //suppose there are no contacts found 
                    if (customerParent == null )
                    {
                        executionResult.Message = $"Contacts not found";
                        return executionResult;
                    }
                    list.Add(customerParent);
                    executionResult.Result = list;
                    executionResult.IsOkay = true;
                    executionResult.Message = $"Contact List retrieval was successful";
                }
                else {

                    var agentList = await context.Users
                        .Join(context.AccountEntities, u => u.AccountEntityId, ae => ae.Id, (u, ae) => new { u, ae })
                        .Join(context.SubAccountBranches, a => a.ae.SubAccountBranchId, sab => sab.Id, (a, sab) => new { a, sab })
                        .Join(context.SubAccounts, x => x.sab.SubAccountId, sa => sa.Id, (x, sa) => new { x, sa })
                        .Where(m => m.sa.ParentId == loggedInSubAccounts.sa.Id)
                        .Select(b => new User
                        {
                            Id = b.x.a.u.Id,
                            Username = b.x.a.u.Username,
                            AccountEntity = new AccountEntity
                            {
                                 Id = b.x.a.ae.Id,
                                EntityUserName = b.x.a.ae.EntityUserName,
                                Phone1 = b.x.a.ae.Phone1,
                                ProfileImageUrl = b.x.a.ae.ProfileImageUrl
                            }
                        })
                        .ToListAsync();

                    //suppose there are no contacts found 
                    if (agentList == null || agentList[0] == null)
                    {
                        //executionResult.IsOkay = false;
                        executionResult.Message = $"Contacts not found";
                        return executionResult;
                    }
                    executionResult.Result = agentList;
                    executionResult.IsOkay = true;
                    executionResult.Message = $"Contact List retrieval was successful";

                }

            }
            catch (Exception ex)
            {
                executionResult.Message = $"Error: {ex.Message}";
                //return executionResult;
            }
            return executionResult;
        }

     
    }
}
