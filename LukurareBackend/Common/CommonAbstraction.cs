using EFDatabaseModel.DbModel;
using EFDatabaseModel.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Common
{
    public static class CommonAbstraction
    {
        //public static async Task<MfsEntityAccountTransaction> InsertAccountTransaction(MfsEntityAccountTransaction model, EFDatabaseModelDatabaseContext context, string transactionReference)
        //{
        //    var populateTable = new MfsEntityAccountTransaction();
        //    try
        //    {

        //        string referenceNo = transactionReference;

        //        var paymode = await context.MsfAccountPaymodes.FirstOrDefaultAsync(
        //               r => r.PaymodeName == DefaultConfiguration.MfsAccountPaymode.cashPaymodeName
        //           );
        //        var setter = new LukurareBackend.SystemTransactions.SystemTransactionSetter(
        //            currentModule,
        //           LukurareBackend
        //                .SystemTransactions
        //                .SystemTransactionSetter
        //                .TransactionTypes
        //                .Insert,
        //            UserPrinciple
        //        );
        //        var receiptResult = await setter.BeginTransaction(context);
        //        SystemTransaction transactionObject = receiptResult.Result;

        //        var receiptsetter =
        //            new LukurareBackend.SystemTransactionsReceipt.SystemTransactionReceiptSetter();
        //        var receipt = await receiptsetter.BeginTransaction(
        //            context,
        //            transactionObject,
        //            referenceNo
        //        );
        //        MsfSystemTransactionReceipt receiptObject = receipt.Result;

        //        populateTable = new MfsEntityAccountTransaction
        //        {
        //            MfsEntityAccountId = model.MfsEntityAccountId,
        //            AmountInCredit = model.AmountInCredit,
        //            AmountOutDebit = model.AmountOutDebit,
        //            TransactionDate = DateTime.UtcNow,
        //            ValueDate = DateTime.UtcNow,
        //            Posted = true,
        //            MsfAccountPaymodeId = paymode.Id,
        //            Narration = model.Narration,
        //            MfsSystemTransactionReceipt = receiptObject
        //        };

        //        context.MfsEntityAccountTransactions.Add(populateTable);


        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    return populateTable;

        //}
    }
}
