using EFDatabaseModel.Contexts;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Repository;
using LukurareBackend.Models;
using Microsoft.EntityFrameworkCore;
using ProjectBase.Repository.Configuration;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukurareBackend.Repositories.Accounts
{
    public class PaymentRepository : EFDatabaseModelBaseRepository<Payment>
    {
        private string currentModule;
       // private long transactionObject;


        //private long transactionObject;

        public PaymentRepository(string currentModule)
            : base(currentModule)
        {

            this.currentModule = currentModule;


        }


        public override async Task<ExecutionResult<Payment>> Add(Payment model, EFDatabaseModelDatabaseContext context = null)
        {
            var createContextHere = context == null;
            var executionResult = new ExecutionResult<Payment>(true);
            try
            {
                if (createContextHere)
                    context = new EFDatabaseModelDatabaseContext();
                var accountEntityId = model.Id;

                //since in the database the account is Float and in the UI it is Others
                if(model.AccountName == "Others")
                {
                    model.AccountName = DefaultConfiguration.MfsAccountTypeName.defaultAccountTypeName;
                }
                if (model.AccountName == "Retainer Fee")
                {
                    model.PaymentPurpose = DefaultConfiguration.MfsAccountTypeName.retainerAccountTypeName;
                }
                if (model.AccountName == "Application Fee")
                {
                    model.PaymentPurpose = DefaultConfiguration.MfsAccountTypeName.applicationAccountTypeName;
                }
                if (model.AccountName == "IELTS")
                {
                    model.PaymentPurpose = DefaultConfiguration.MfsAccountTypeName.IELTSAccountTypeName;
                }
                if (model.AccountName == "ECA")
                {
                    model.PaymentPurpose = DefaultConfiguration.MfsAccountTypeName.ECAAccountTypeName;
                }
                if (model.AccountName == "Final Deduction")
                {
                    model.PaymentPurpose = DefaultConfiguration.MfsAccountTypeName.FinalDeductionAccountTypeName;
                }
                if (accountEntityId == 0)
                {
                     accountEntityId = UserPrinciple.User.AccountEntityId;
                }
                var accountCreated = new MfsEntityAccount();
                var account  = context.MfsEntityAccounts
                    .Join(context.MfsAccountTypes, entityAccount => entityAccount.MfsAccountTypeId,
                    accountTypes => accountTypes.Id, (entityAccount, accountTypes) => new { entityAccount, accountTypes })
                    .FirstOrDefault(a => a.entityAccount.AccountEntityId == accountEntityId && a.accountTypes.AccountTypeName == model.AccountName);

                //var accountExists = context.MfsEntityAccounts
                   // .FirstOrDefault(a => a.AccountEntityId == accountEntityId);

                if ( account == null)
                {

                    // Create the user account if it doesn't exist
                    var currencytype = context.MfsCurrencyTypes.FirstOrDefault(r => r.CurrencyCode == DefaultConfiguration.Currency.defaultCurrency);
                    var accounttype = context.MfsAccountTypes.FirstOrDefault(s => s.AccountTypeName == model.AccountName);

                    // Check if the accounttype is null
                    if (accounttype == null)
                    {
                        executionResult.IsOkay = false;
                        executionResult.Message = "Account type not found.";
                        return executionResult;
                    }


                    var setter = new LukurareBackend.SystemTransactions.SystemTransactionSetter(
                 currentModule,
                 LukurareBackend
                     .SystemTransactions
                     .SystemTransactionSetter
                     .TransactionTypes
                     .Insert,
                 UserPrinciple
                 );
                    var receiptResult = await setter.BeginTransaction(context);
                    SystemTransaction transactionObject = receiptResult.Result;

                    accountCreated = new MfsEntityAccount
                    {
                        //checks for nullity in account and if null, we use the accountCreated variable instead
                        AccountEntityId = accountEntityId,
                        MfsAccountTypeId = accounttype.Id,
                        DateCreated = DateTime.UtcNow,
                        AccountNumber = Guid.NewGuid().ToString(),
                        AccountName = accounttype.AccountTypeName,
                        MfsCurrencyTypeId = currencytype.Id,
                        IsActive = true,
                        SystemTransaction = transactionObject,

                    };

                    context.MfsEntityAccounts.Add(accountCreated);
                    context.SaveChanges();
                    
                }

                var AccountTransaction = new MfsEntityAccountTransaction
                {
                    MfsEntityAccountId = account != null ? account.entityAccount.Id : accountCreated.Id,
                    AmountInCredit = (float)model.Amount,
                    MsfAccountPaymodeId = model.PaymodeId,
                    Narration = model.PaymentPurpose,
                    Posted = false,
                    //MfsEntityAccount = accountExists
                };

                var insertedModel = await InsertAccountTransaction(AccountTransaction, context, model.ReferenceNumber);
                context.SaveChanges();
                executionResult.Message = "PAYMENT DETAILS UPLOADED SUCCESSFULLY";
            }
            catch (Exception ex)
            {
                executionResult.CopyFromException(ex);
                var exception = ex.ToString();
            }
            finally
            {
                if (createContextHere)
                    await context.DisposeAsync();
            }
            executionResult.Result = null;
            return executionResult;
        }

        public override Payment CloneModel(Payment model)
        {
            throw new NotImplementedException();
        }

        public override Task<ExecutionResult<Payment>> GetItem(int id, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }
       
        public override Task<ExecutionResult<Payment>> Update(Payment model, EFDatabaseModelDatabaseContext context = null)
        {
            throw new NotImplementedException();
        }

        private bool IsModelValid(Payment model)
        {
            if (model == null)
            {
                return false;
            }

            // Check if the Name property is provided and not empty
            if (string.IsNullOrEmpty(model.ReferenceNumber))
            {
                return false;
            }

            // Check if the Amount is greater than zero
            if (model.Amount <= 0)
            {
                return false;
            }

            // Add additional validation rules as per your requirements

            return true;
        }

        public async Task<MfsEntityAccountTransaction> InsertAccountTransaction(MfsEntityAccountTransaction model, EFDatabaseModelDatabaseContext context, string transactionReference)
        {
            var populateTable = new MfsEntityAccountTransaction();
            try
            {

                string referenceNo = transactionReference;

                //var paymode = await context.MfsEntityAccountTransactions
                //    .Join(context.MsfAccountPaymodes, c => c.MsfAccountPaymodeId, d => d.Id, (c, d) => new { c, d })
                //    .Where(x => x.c.MsfAccountPaymodeId == model.MsfAccountPaymodeId)
                //    .Select(v => new MsfAccountPaymode
                //    { 
                //        Id = v.d.Id,
                //        PaymodeName = v.d.PaymodeName
                    
                //    })
                //    .FirstOrDefaultAsync();

                var setter = new LukurareBackend.SystemTransactions.SystemTransactionSetter(
                    currentModule,
                    LukurareBackend
                        .SystemTransactions
                        .SystemTransactionSetter
                        .TransactionTypes
                        .Insert,
                    UserPrinciple
                );
                var receiptResult = await setter.BeginTransaction(context);
                SystemTransaction transactionObject = receiptResult.Result;

                var receiptsetter =
                    new LukurareBackend.SystemTransactionsReceipt.SystemTransactionReceiptSetter();
                var receipt = await receiptsetter.BeginTransaction(
                    context,
                    transactionObject,
                    referenceNo
                );
                MsfSystemTransactionReceipt receiptObject = receipt.Result;

                populateTable = new MfsEntityAccountTransaction
                {
                    MfsEntityAccountId = model.MfsEntityAccountId,
                    AmountInCredit = model.AmountInCredit,
                    AmountOutDebit = model.AmountOutDebit,
                    TransactionDate = DateTime.UtcNow,
                    ValueDate = model.ValueDate,
                    Posted = model.Posted,
                    MsfAccountPaymodeId = model.MsfAccountPaymodeId,
                    Narration = model.Narration,
                    MfsSystemTransactionReceipt = receiptObject
                };
           
                context.MfsEntityAccountTransactions.Add(populateTable);


            }
            catch (Exception e)
            {

            }

            return populateTable;

        }


    
    }
}
