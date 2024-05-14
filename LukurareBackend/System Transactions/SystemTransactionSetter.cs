using System;
using System.Linq;
using System.Threading.Tasks;
using ProjectBase.Repository.Transaction;
using EFDatabaseModel.Models.Authentication;
using EFDatabaseModel.DbModel;
using EFDatabaseModel.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LukurareBackend.SystemTransactions
{
    public class SystemTransactionSetter
    {
        public SystemTransactionSetter(
            string currentModule,
            TransactionTypes transactionType,
            UserPrinciple currentUser
        )
        {
            CurrentModule = currentModule;
            TransactionType = transactionType;
            UserPrinciple = currentUser;
        }

        public string CurrentModule { get; }
        public TransactionTypes TransactionType { get; }
        public UserPrinciple UserPrinciple { get; }

        public async Task<ExecutionResult<SystemTransaction>> BeginTransaction(
            EFDatabaseModelDatabaseContext context,
            DateTime? Tdate = null,
            string narration = null,
            string transactionNo = null,
            string extenalRef = null,
            long? targetedTransactingBranchId = null
        )
        {
            var result = new ExecutionResult<SystemTransaction>(true, "");
            var dateUse = Tdate ?? DateTime.UtcNow;

            var saID = UserPrinciple.User.AccountEntityId;
            var checkModulem = await context.AccountEntities
                .Join(
                    context.SubAccountBranches,
                    sm => sm.SubAccountBranchId,
                    cm => cm.Id,
                    (sm, cm) => new { sm, cm }
                )
                .Join(
                    context.SubAccounts,
                    smcm => smcm.cm.SubAccountId,
                    sa => sa.Id,
                    (smcm, sa) => new { smcm, sa }
                )
                .FirstOrDefaultAsync(r => r.smcm.sm.Id == saID);

            var checkModule = await context.SystemModules
                .Join(
                    context.CompanyModules,
                    sm => sm.Id,
                    cm => cm.SystemModuleId,
                    (sm, cm) => new { sm, cm }
                )
                .FirstOrDefaultAsync(
                    r => r.sm.ModuleName == CurrentModule && r.cm.SubAccountId == checkModulem.sa.Id
                );
            var systemModuleexist = await context.SystemModules.FirstOrDefaultAsync(
                r => r.ModuleName == CurrentModule
            );
            var systemid = 0;
            if (checkModule == null)
            {
                var executionResult = new ExecutionResult<CompanyModule>(true);
                using var mycontext = new EFDatabaseModelDatabaseContext();
                var systemModules = new SystemModule();
                var companyModule = new CompanyModule();

                systemModules = new SystemModule { ModuleName = CurrentModule };

                if (systemModuleexist == null)
                {
                    companyModule = new CompanyModule
                    {
                        OneTransactionStagePerUser = false,
                        SystemModule = systemModules,
                        SubAccountId = checkModulem.sa.Id
                    };
                }
                else
                {
                    companyModule = new CompanyModule
                    {
                        OneTransactionStagePerUser = false,
                        SystemModuleId = systemModuleexist.Id,
                        SubAccountId = checkModulem.sa.Id
                    };
                }
                mycontext.CompanyModules.Add(companyModule);
                mycontext.SaveChanges();
                executionResult.Result = companyModule;
                systemid = executionResult.Result.Id;
            }
            else
            {
                systemid = checkModule.cm.Id;
            }

            var transactionNoUse = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(transactionNo))
                transactionNoUse = transactionNo;
            int type = (int)TransactionType;
            var SystemTransactions = new SystemTransaction
            {
                TransactionDate = dateUse,
                ExternalSystemRefNo = extenalRef,
                InitiatingTransactingBranchId = checkModulem.smcm.cm.Id,
                TargetedTransactingBranchId = targetedTransactingBranchId,
                TransactionNarration = narration,
                TransactionNo = transactionNoUse,
                TransactionTypesId = type,
                UserId = UserPrinciple.User.Id,
                UserLoginAttemptId = UserPrinciple.UserLoginAttemptId,
                InitiatingCompanySystemModuleId = systemid
            };
            //context.SystemTransactions.Add(SystemTransactions);

            result.Result = SystemTransactions;

            return result;
        }

        public enum TransactionTypes
        {
            Insert = 0,
            Update = 1,
            Delete = 2,
            Select = 3,
            Reverse = 4,
            DiffrentDayReverse = 5,
            PaymentsReverse = 5,
            ReverseToTransactionDate = 6
        }
    }
}
//var setter = new SystemTransactions.SystemTransactionSetter(CurrentModule, SystemTransactions.SystemTransactionSetter.TransactionTypes.Insert, UserPrinciple);
//var result = await setter.BeginTransaction(context);
//if (result.IsOkay == false)
//    return result.CloneDiffrentType<AccountEntity>();
//SystemTransaction transactionObject = result.Result;
