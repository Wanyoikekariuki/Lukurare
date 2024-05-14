using System;
using System.Linq;
using System.Threading.Tasks;
using ProjectBase.Repository.Transaction;
using EFDatabaseModel.Models.Authentication;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LukurareBackend.SystemTransactionsReceipt
{
    public class SystemTransactionReceiptSetter
    {
        public SystemTransactionReceiptSetter() { }

        public Task<ExecutionResult<MsfSystemTransactionReceipt>> BeginTransaction(
            EFDatabaseModelDatabaseContext context,
            SystemTransaction stobject,
            string receiptNo = null,
            string paymodeNo = null,
            string internalNo = null,
            string externalNo = null
        )
        {
            var result = new ExecutionResult<MsfSystemTransactionReceipt>(true, "");

            var receiptNoUse = Guid.NewGuid().ToString();
            var paymodeNoUse = Guid.NewGuid().ToString();
            var internalNoUse = Guid.NewGuid().ToString();
            var externalNoUse = Guid.NewGuid().ToString();

            //var transactionNoUse = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(receiptNo))
                receiptNoUse = receiptNo;
            if (!string.IsNullOrEmpty(paymodeNo))
                paymodeNoUse = paymodeNo;
            if (!string.IsNullOrEmpty(internalNo))
                internalNoUse = internalNo;
            if (!string.IsNullOrEmpty(externalNo))
                externalNoUse = externalNo;

            var SystemTransactions = new MsfSystemTransactionReceipt
            {
                SystemTransaction = stobject,
                ReceiptNo = receiptNoUse,
                PaymodeReferenceNo = paymodeNoUse,
                InternalManualReceiptNo = internalNoUse,
                ExternalSystemReceiptNo = externalNoUse
            };
            context.MsfSystemTransactionReceipts.Add(SystemTransactions);

            result.Result = SystemTransactions;

            return Task.FromResult(result);
        }
    }
}
