using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SystemTransaction
    {
        public SystemTransaction()
        {
            BillReferenceServiceRequests = new HashSet<BillReferenceServiceRequest>();
            MfsEntityAccounts = new HashSet<MfsEntityAccount>();
            MsfSystemTransactionReceipts = new HashSet<MsfSystemTransactionReceipt>();
        }

        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public int TransactionTypesId { get; set; }
        public long UserId { get; set; }
        public int InitiatingCompanySystemModuleId { get; set; }
        public long? InitiatingTransactingBranchId { get; set; }
        public long? TargetedTransactingBranchId { get; set; }
        public long? UserLoginAttemptId { get; set; }
        public string TransactionNarration { get; set; }
        public string ExternalSystemRefNo { get; set; }

        public virtual ICollection<BillReferenceServiceRequest> BillReferenceServiceRequests { get; set; }
        public virtual ICollection<MfsEntityAccount> MfsEntityAccounts { get; set; }
        public virtual ICollection<MsfSystemTransactionReceipt> MsfSystemTransactionReceipts { get; set; }
    }
}
