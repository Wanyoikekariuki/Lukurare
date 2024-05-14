//using LukurareBackend.Repositories.Accounts;
//using EFDatabaseModel.Contexts;
//using EFDatabaseModel.DbModel;
//using EFDatabaseModel.Models.Authentication;
//using EFDatabaseModel.Repository;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using ProjectBase.KendoUiSupport;
//using ProjectBase.Repository.Configuration;
//using ProjectBase.Repository.Transaction;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LukurareBackend.Repositories.LoadAirtime
//{
//    public class BillReferenceServiceRequestRepository
//        : EFDatabaseModelBaseRepository<BillReferenceServiceRequest>
//    {
//        public BillReferenceServiceRequestRepository(string currentModule)
//            : base(currentModule) { }

//        public virtual UserPrinciple GetCurrentPrincipalGenericPrinciple()
//        {
//            var princip = CurrentPrincipal as UserPrinciple;

//            return princip;
//        }

//        public override BillReferenceServiceRequest CloneModel(BillReferenceServiceRequest model)
//        {
//            long contactid = 0;
//            if (model.AccountEntityContact == null)
//            {
//                contactid = model.AccountEntityContactId;
//            }
//            else
//            {
//                contactid = model.AccountEntityContact.Id;
//            }

//            var Value = new BillReferenceServiceRequest
//            {
//                Id = model.Id,
//                SystemTransactionId = model.SystemTransactionId,
//                AccountEntityContactId = contactid,
//                Amount = model.Amount,
//                Processed = model.Processed,
//                Success = model.Success,
//                Narration = model.Narration
//            };
//            return Value;
//        }

//        public async override Task<ExecutionResult<BillReferenceServiceRequest>> Add(
//            BillReferenceServiceRequest model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<BillReferenceServiceRequest>(true);
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //validate wether the supplied model object is valid
//                executionResult = await IsValidInsert(model, context);
//                if (executionResult.IsOkay == false)
//                    return executionResult; //if fail return the failed execution result
//                var modelInsert = CloneModel(model);

//                var setter = new SystemTransactions.SystemTransactionSetter(
//                    CurrentModule,
//                    SystemTransactions.SystemTransactionSetter.TransactionTypes.Insert,
//                    UserPrinciple
//                );
//                var result = await setter.BeginTransaction(context);
//                if (result.IsOkay == false)
//                    return executionResult;
//                //return result.CloneDiffrentType<AccountEntity>();

//                SystemTransaction transactionObject = result.Result;

//                modelInsert.SystemTransaction = transactionObject;
//                int id = 0;
//                if (modelInsert.AccountEntityContactId == 0)
//                {
//                    if (model.AccountEntityContact.ContactPhone.StartsWith("+254"))
//                    {
//                        model.AccountEntityContact.ContactPhone =
//                            model.AccountEntityContact.ContactPhone.Replace("+254", "0");
//                    }
//                    if (model.AccountEntityContact.ContactPhone.StartsWith("254"))
//                    {
//                        string x = model.AccountEntityContact.ContactPhone.Substring(3);
//                        model.AccountEntityContact.ContactPhone = "0" + x;
//                    }
//                    var contactExist = context.AccountEntityContacts.FirstOrDefault(
//                        r =>
//                            r.ContactPhone == model.AccountEntityContact.ContactPhone
//                            && r.AccountEntityId == UserPrinciple.User.AccountEntityId
//                    );
//                    if (contactExist == null)
//                    {
//                        string[] safaricom =
//                        {
//                            "0110",
//                            "0111",
//                            "0112",
//                            "0113",
//                            "0114",
//                            "0115",
//                            "0700",
//                            "0701",
//                            "0702",
//                            "0703",
//                            "0704",
//                            "0705",
//                            "0706",
//                            "0707",
//                            "0708",
//                            "0710",
//                            "0711",
//                            "0712",
//                            "0713",
//                            "0714",
//                            "0715",
//                            "0716",
//                            "0717",
//                            "0718",
//                            "0719",
//                            "0720",
//                            "0720",
//                            "0721",
//                            "0722",
//                            "0723",
//                            "0724",
//                            "0725",
//                            "0726",
//                            "0727",
//                            "0728",
//                            "0729",
//                            "0740",
//                            "0741",
//                            "0742",
//                            "0743",
//                            "0744",
//                            "0745",
//                            "0757",
//                            "0758",
//                            "0759",
//                            "0769",
//                            "0746",
//                            "0748",
//                            "0790",
//                            "0791",
//                            "0792",
//                            "0793",
//                            "0794",
//                            "0795",
//                            "0796",
//                            "0797",
//                            "0798",
//                            "0799",
//                            "0768"
//                        };
//                        string[] airtel =
//                        {
//                            "0100",
//                            "0101",
//                            "0102",
//                            "0103",
//                            "0104",
//                            "0105",
//                            "0106",
//                            "0730",
//                            "0731",
//                            "0732",
//                            "0733",
//                            "0734",
//                            "0735",
//                            "0736",
//                            "0737",
//                            "0738",
//                            "0739",
//                            "0750",
//                            "0751",
//                            "0752",
//                            "0753",
//                            "0754",
//                            "0755",
//                            "0756",
//                            "0762",
//                            "0780",
//                            "0781",
//                            "0782",
//                            "0783",
//                            "0784",
//                            "0785",
//                            "0786",
//                            "0787",
//                            "0788",
//                            "0789"
//                        };
//                        string[] telkom =
//                        {
//                            "0202",
//                            "0770",
//                            "0771",
//                            "0772",
//                            "0773",
//                            "0774",
//                            "0775",
//                            "0776",
//                            "0777",
//                            "0778",
//                            "0779"
//                        };
//                        var network = context.BillReferenceServiceTypes.FirstOrDefault();

//                        var prefix = model.AccountEntityContact.ContactPhone.Substring(0, 4);
//                        if (telkom.Contains(prefix))
//                        {
//                            network = context.BillReferenceServiceTypes.FirstOrDefault(
//                                r => r.ServiceId == "2"
//                            );
//                        }
//                        else if (
//                            airtel.Contains(model.AccountEntityContact.ContactPhone.Substring(0, 4))
//                        )
//                        {
//                            network = context.BillReferenceServiceTypes.FirstOrDefault(
//                                r => r.ServiceId == "1"
//                            );
//                        }
//                        else if (
//                            safaricom.Contains(
//                                model.AccountEntityContact.ContactPhone.Substring(0, 4)
//                            )
//                        )
//                        {
//                            network = context.BillReferenceServiceTypes.FirstOrDefault(
//                                r => r.ServiceId == "3"
//                            );
//                        }
//                        var country = await context.Countries.FirstOrDefaultAsync(
//                            r => r.CountryIso == "KE"
//                        );
//                        id = network.Id;
//                        var contact = new AccountEntityContact
//                        {
//                            ContactEmail = "Not supplied",
//                            ContactPhone = model.AccountEntityContact.ContactPhone,
//                            ContactName = "New Contact",
//                            BillReferenceServiceTypeId = network.Id,
//                            CountryId = country.Id,
//                            AccountEntityId = UserPrinciple.User.AccountEntityId
//                        };
//                        context.AccountEntityContacts.Add(contact);
//                        modelInsert.AccountEntityContact = contact;
//                    }
//                    else
//                    {
//                        modelInsert.AccountEntityContactId = contactExist.Id;
//                        id = contactExist.BillReferenceServiceTypeId;
//                    }
//                }
//                else
//                {
//                    id = model.AccountEntityContact.BillReferenceServiceTypeId;
//                }

//                //insert the okay object to the DB
//                context.BillReferenceServiceRequests.Add(modelInsert);

//                if (createContextHere)
//                {
//                    //loop from here
//                    //
//                    List<AccountEntity> entities = new List<AccountEntity>();
//                    var child = context.SubAccounts
//                        .Join(
//                            context.SubAccountBranches,
//                            sa => sa.Id,
//                            sb => sb.SubAccountId,
//                            (sa, sb) => new { sa, sb }
//                        )
//                        .Join(
//                            context.AccountEntities,
//                            ent => ent.sb.Id,
//                            accent => accent.SubAccountBranchId,
//                            (ent, accent) => new { ent, accent }
//                        )
//                        .FirstOrDefault(s => s.accent.Id == UserPrinciple.User.AccountEntityId);

//                    if (child == null)
//                        return executionResult;

//                    entities.Add(child.accent);

//                    //var parent = context.SubAccounts
//                    //   .Join(context.SubAccountBranches, sa => sa.Id, sb => sb.SubAccountId, (sa, sb) => new { sa, sb })
//                    //   .Join(context.AccountEntities, ent => ent.sa.Id, accent => accent.SubAccountBranchId, (ent, accent) => new { ent, accent })
//                    //   .FirstOrDefault(s => s.ent.sa.Id == child.ent.sa.ParentId);
//                    while (child.ent.sa.ParentId != null)
//                    {
//                        var parent = context.SubAccounts
//                            .Join(
//                                context.SubAccountBranches,
//                                sa => sa.Id,
//                                sb => sb.SubAccountId,
//                                (sa, sb) => new { sa, sb }
//                            )
//                            .Join(
//                                context.AccountEntities,
//                                ent => ent.sb.Id,
//                                accent => accent.SubAccountBranchId,
//                                (ent, accent) => new { ent, accent }
//                            )
//                            .FirstOrDefault(s => s.ent.sa.Id == child.ent.sa.ParentId);

//                        if (parent == null)
//                            break;

//                        entities.Add(parent.accent);

//                        child = parent;
//                    }
//                    ;

//                    //
//                    foreach (var entity in entities)
//                    {
//                        var account = await context.MfsEntityAccounts.FirstOrDefaultAsync(
//                            r => r.AccountEntityId == entity.Id
//                        );
//                        if (account == null)
//                        {
//                            executionResult.Message =
//                                $"Entity account for entity {entity.Id} does not exist in the database";
//                            break;
//                        }
//                        var paymode = await context.MsfAccountPaymodes.FirstOrDefaultAsync(
//                            r => r.PaymodeName == "Cash"
//                        );
//                        if (paymode == null)
//                        {
//                            executionResult.Message = "Cash paymode does not exist in the database";
//                            break;
//                        }

//                        var percentage = await context.MfsServiceCommissions.FirstOrDefaultAsync(
//                            r =>
//                                r.MfsEntityAccountId == account.Id
//                                && r.BillReferenceServiceTypeId == id
//                        );
//                        if (percentage == null)
//                        {
//                            executionResult.Message =
//                                "Commmission not defined for this entity account";
//                            break;
//                        }
//                        var comm = modelInsert.Amount * percentage.PercentageCommission / 100;

//                        var modelInser = new MfsEntityAccountTransaction
//                        {
//                            MsfAccountPaymodeId = paymode.Id,
//                            MfsEntityAccountId = account.Id,
//                            AmountOutDebit = modelInsert.Amount - comm,
//                            Narration = "Float reduction from credit sale"
//                        };

//                        var floataccount = new ResellerProfileRepository(
//                            "BillReferenceServiceRequest"
//                        );
//                        floataccount.SetCurrentPrincipal(UserPrinciple);
//                        var final = await floataccount.Add(modelInser, context);
//                    }
//                    //final loop position

//                    /*var comm = new ResellerProfileRepository("BillReferenceServiceRequest");
//                    comm.SetCurrentPrincipal(UserPrinciple);
//                    var commissionresult = await comm.Add(commission, context);**/
//                    try
//                    {
//                        await context.SaveChangesAsync();
//                        executionResult.Message = "top up sent successfully";
//                        // executionResult.Result = modelInsert;
//                        executionResult.Result = await GetModelForInsertUpdateFromId(
//                            modelInsert.Id,
//                            modelInsert,
//                            context
//                        );
//                    }
//                    catch (Exception ex)
//                    {
//                        //if an error occured copy its data from the exception object into the execution result by
//                        executionResult.CopyFromException(ex);
//                    }
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

//        public async override Task<ExecutionResult<BillReferenceServiceRequest>> GetItem(
//            int id,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<BillReferenceServiceRequest>(
//                true,
//                "Entry found"
//            );
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                //var dbJonInstance = await context.BillReferenceServiceRequests
//                //    .Join(context.SubAccountBranches, ae => ae.SubAccountBranchId, sab => sab.Id, (ae, sab) => new { ae, sab })
//                //    .Join(context.SubAccounts, aesab => aesab.sab.SubAccountId, sa => sa.Id, (aesab, sa) => new { aesab, sa })
//                //   .FirstOrDefaultAsync(r => r.aesab.ae.Id == id);
//                DbInstance = await context.BillReferenceServiceRequests.FirstOrDefaultAsync(
//                    r => r.Id == id
//                );
//                if (DbInstance == null)
//                {
//                    executionResult.IsOkay = false;
//                    executionResult.Message = $"The entry with id {id} supplied does not exist";
//                    return executionResult;
//                }

//                //DbInstance = dbJonInstance.aesab.ae;
//                //DbInstance.SubAccountBranch = dbJonInstance.aesab.sab;
//                //DbInstance.SubAccountBranch.SubAccount = dbJonInstance.sa;

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

//        public override Task<ExecutionResult<BillReferenceServiceRequest>> IsValidGeneral(
//            BillReferenceServiceRequest model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            //since this IsValid general does have a Asyn Call force it to be asyn by using task.Run

//            var result = new ExecutionResult<BillReferenceServiceRequest>(true);
//            if (model == null)
//            {
//                result.IsOkay = false;
//                result.Message = "model is null";
//                return Task.FromResult(result);
//            }
//            if (model.Amount == default(float))
//            {
//                result.IsOkay = false;
//                result.Message = "Please supply  amount for this request";
//                return Task.FromResult(result);
//            }
//            if (model.AccountEntityContact == null)
//            {
//                result.IsOkay = false;
//                result.Message = "Please supply the Contact";
//                return Task.FromResult(result);
//            }

//            //SubAccountSelected = await context.SubAccounts.FirstOrDefaultAsync(r => r.Id == model.SubAccountId);
//            //if (SubAccountSelected == null)
//            //{
//            //    result.IsOkay = false;
//            //    result.Message = "Please  supply the Sub Account ";
//            //    return result;
//            //}

//            return Task.FromResult(result);
//        }

//        public async override Task<ExecutionResult<BillReferenceServiceRequest>> IsValidInsert(
//            BillReferenceServiceRequest model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            var result = await IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            //perform insert specific validation


//            return result;
//        }

//        public async override Task<ExecutionResult<BillReferenceServiceRequest>> IsValidUpdate(
//            BillReferenceServiceRequest model,
//            EFDatabaseModelDatabaseContext context
//        )
//        {
//            //if it passes general validation then use get item to complete update validation
//            var result = await IsValidGeneral(model, context);
//            if (result.IsOkay == false)
//                return result;

//            result = await GetItem((int)model.Id, context);
//            if (result.IsOkay == false)
//                return result;

//            //ensure that there is not other object with a diffrent ID in db with the same country code as this one
//            return result;
//        }

//        #region fill combo boxes
//        public async Task<ExecutionResult<IEnumerable<IdentificationDocumentType>>> GetDocumentType(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<IdentificationDocumentType>>(
//                true,
//                "",
//                new HashSet<IdentificationDocumentType>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    var filterValue = args.GetFilterValue(0).ToLower();
//                    ;
//                    if (string.IsNullOrEmpty(filterValue))
//                        return result;

//                    var returnData = await context.IdentificationDocumentTypes
//                        .Where(r => true)
//                        .OrderBy(r => r.Id)
//                        .Skip(args.skip)
//                        .Take(args.take)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new IdentificationDocumentType
//                            {
//                                Id = r.Id,
//                                DocumentName = r.DocumentName
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

//        public override Task<ExecutionResult<BillReferenceServiceRequest>> Update(
//            BillReferenceServiceRequest model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            throw new NotImplementedException();
//        }

//        #endregion


//        public async Task<ExecutionResult<IEnumerable<BillReferenceServiceType>>> GetServices(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<BillReferenceServiceType>>(
//                true,
//                "",
//                new HashSet<BillReferenceServiceType>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    // args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    //var filterValue = args.GetFilterValue(0).ToLower(); ;
//                    //if (string.IsNullOrEmpty(filterValue))
//                    //    return result;

//                    var returnData = await context.BillReferenceServiceTypes
//                        .Where(r => true)
//                        .OrderBy(r => r.Id)
//                        .Skip(0)
//                        .Take(10)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new BillReferenceServiceType
//                            {
//                                Id = r.Id,
//                                ServiceName = r.ServiceName,
//                                ServiceId = r.ServiceId
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

//        public async Task<ExecutionResult<IEnumerable<AccountEntityContact>>> GetContacts(
//            NeedDataSourceEventArgs args
//        )
//        {
//            var result = new ExecutionResult<IEnumerable<AccountEntityContact>>(
//                true,
//                "",
//                new HashSet<AccountEntityContact>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    // args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                    //var filterValue = args.GetFilterValue(0).ToLower(); ;
//                    //if (string.IsNullOrEmpty(filterValue))
//                    //    return result;r.AccountEntityId.Equals(UserPrinciple.User.AccountEntityId

//                    var returnData = await context.AccountEntityContacts
//                        .Join(
//                            context.BillReferenceServiceTypes,
//                            x => x.BillReferenceServiceTypeId,
//                            y => y.Id,
//                            (x, y) => new { x, y }
//                        )
//                        .Where(
//                            r =>
//                                r.x.AccountEntityId
//                                == GetCurrentPrincipalGenericPrinciple().User.AccountEntityId
//                        )
//                        .OrderBy(r => r.x.Id)
//                        .Skip(0)
//                        .Take(100)
//                        .ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new AccountEntityContact
//                            {
//                                Id = r.x.Id,
//                                ContactName = r.x.ContactName,
//                                ContactPhone = r.x.ContactPhone,
//                                ContactEmail = r.x.ContactEmail,
//                                BillReferenceServiceType = new BillReferenceServiceType
//                                {
//                                    Id = r.y.Id
//                                }
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

//        public async Task<ExecutionResult<BillReferenceServiceRequest>> BulkTopup(
//            List<BillReferenceServiceRequest> list
//        )
//        {
//            var executionResult = new ExecutionResult<BillReferenceServiceRequest>(true);

//            try
//            {
//                using EFDatabaseModelDatabaseContext context = new EFDatabaseModelDatabaseContext();

//                float sum = 0,
//                    totalcommission = 0;

//                foreach (var index in list)
//                {
//                    executionResult = await Add(index, context);
//                    sum = sum + index.Amount;
//                    // var percentages = await context.MfsServiceCommissions.FirstOrDefaultAsync(r => r.MfsEntityAccountId == account.Id && r.BillReferenceServiceTypeId == index.AccountEntityContact.BillReferenceServiceTypeId);
//                    // totalcommission = totalcommission + index.Amount * percentages.PercentageCommission / 100;
//                }

//                //if (executionResult.IsOkay == false)
//                //{
//                //    return executionResult;
//                //}

//                List<AccountEntity> entities = new List<AccountEntity>();
//                var child = context.SubAccounts
//                    .Join(
//                        context.SubAccountBranches,
//                        sa => sa.Id,
//                        sb => sb.SubAccountId,
//                        (sa, sb) => new { sa, sb }
//                    )
//                    .Join(
//                        context.AccountEntities,
//                        ent => ent.sb.Id,
//                        accent => accent.SubAccountBranchId,
//                        (ent, accent) => new { ent, accent }
//                    )
//                    .FirstOrDefault(s => s.accent.Id == UserPrinciple.User.AccountEntityId);

//                if (child == null)
//                    return executionResult;

//                entities.Add(child.accent);

//                while (child.ent.sa.ParentId != null)
//                {
//                    var parent = context.SubAccounts
//                        .Join(
//                            context.SubAccountBranches,
//                            sa => sa.Id,
//                            sb => sb.SubAccountId,
//                            (sa, sb) => new { sa, sb }
//                        )
//                        .Join(
//                            context.AccountEntities,
//                            ent => ent.sb.Id,
//                            accent => accent.SubAccountBranchId,
//                            (ent, accent) => new { ent, accent }
//                        )
//                        .FirstOrDefault(s => s.ent.sa.Id == child.ent.sa.ParentId);

//                    if (parent == null)
//                        break;

//                    entities.Add(parent.accent);

//                    child = parent;
//                }
//                ;

//                //
//                foreach (var entity in entities)
//                {
//                    var account = await context.MfsEntityAccounts.FirstOrDefaultAsync(
//                        r => r.AccountEntityId == entity.Id
//                    );
//                    if (account == null)
//                    {
//                        executionResult.Message =
//                            $"Entity account for entity {entity.Id} does not exist in the database";
//                        break;
//                    }
//                    var paymode = await context.MsfAccountPaymodes.FirstOrDefaultAsync(
//                        r => r.PaymodeName == "Cash"
//                    );
//                    if (paymode == null)
//                    {
//                        executionResult.Message = "Cash paymode does not exist in the database";
//                        break;
//                    }

//                    totalcommission = 0;
//                    foreach (var index in list)
//                    {
//                        var percentages = await context.MfsServiceCommissions.FirstOrDefaultAsync(
//                            r =>
//                                r.MfsEntityAccountId == account.Id
//                                && r.BillReferenceServiceTypeId
//                                    == index.AccountEntityContact.BillReferenceServiceTypeId
//                        );
//                        totalcommission =
//                            totalcommission + index.Amount * percentages.PercentageCommission / 100;
//                    }

//                    var modelInsert = new MfsEntityAccountTransaction
//                    {
//                        MsfAccountPaymodeId = paymode.Id,
//                        MfsEntityAccountId = account.Id,
//                        AmountOutDebit = sum - totalcommission,
//                        Narration = "Float reduction from credit sale"
//                    };

//                    var floataccount = new ResellerProfileRepository("BillReferenceServiceRequest");
//                    floataccount.SetCurrentPrincipal(UserPrinciple);
//                    var final = await floataccount.Add(modelInsert, context);
//                    //var percentage = await context.MfsServiceCommissions.Where(r => r.MfsEntityAccountId == account.Id).ToListAsync();
//                    //if (percentage == null)
//                    //{
//                    //    executionResult.Message = "Commmission not defined for this entity account";
//                    //    return executionResult;
//                    //}

//                    //var commission = new MfsEntityAccountTransaction
//                    //{


//                    //    MsfAccountPaymodeId = paymode.Id,
//                    //    MfsEntityAccountId = account.Id,
//                    //    AmountInCredit=totalcommission,
//                    //    Narration = "Commission earned"
//                    //};

//                    //var comm = new ResellerProfileRepository("BillReferenceServiceRequest");
//                    //comm.SetCurrentPrincipal(UserPrinciple);
//                    //var commissionresult = await comm.Add(commission, context);
//                }
//                await context.SaveChangesAsync();
//                executionResult.Message = "Requests sent successfully";
//                executionResult.Result = new BillReferenceServiceRequest();
//            }
//            catch (Exception ex)
//            {
//                executionResult.CopyFromException(ex);
//            }
//            return executionResult;
//        }

//        public async override Task<
//            ExecutionResult<ItemsRequestedResult<NeedDataSourceEventArgs>>
//        > GetKendoGridFiltered(
//            NeedDataSourceEventArgs args,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            //incase the context was null then mark this and use this into create a new context instance and later dispose it in finally
//            var createContextHere = context == null;
//            var executionResult = new ExecutionResult<
//                ItemsRequestedResult<NeedDataSourceEventArgs>
//            >(true, "Bill reference instance found");
//            try
//            {
//                if (createContextHere)
//                    context = new EFDatabaseModelDatabaseContext();

//                args = NeedDataSourceEventArgs.EnsureArgsValid(args);
//                ////since this will be loaded from a detail table of SmsGateway then ensure the Filter SmsGateway Id Is Passed
//                int filterValueInt = -1;
//                string filterValue = null;
//                if (args.HasFilterFields("Id"))
//                    filterValue = args.GetFilterValuePredicate(r => r.field == "Id", 0);
//                //int filterValueInt = -1;
//                //var  filterValue = GroupID;
//                bool parsedInt = int.TryParse(filterValue, out filterValueInt);

//                var accentity = await context.BillReferenceServiceRequests.ToListAsync();

//                args.filter = null;

//                var filterSupporter = new FilterSupporter<BillReferenceServiceRequest>(
//                    accentity,
//                    args
//                ); //accentity
//                var filteredData = filterSupporter.FilterData<long>(r => r.Id);
//                executionResult.Result = filterSupporter.itemsResult;
//                //executionResult.Result.Result =accentity;
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
//    }
//}
