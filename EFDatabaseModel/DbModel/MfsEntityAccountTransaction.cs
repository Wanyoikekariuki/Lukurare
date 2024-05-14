using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsEntityAccountTransaction
    {
        public MfsEntityAccountTransaction()
        {
            MfsEntityAccountTransactionAdditionalDetails = new HashSet<MfsEntityAccountTransactionAdditionalDetail>();
        }

        public long Id { get; set; }
        public long MfsEntityAccountId { get; set; }
        public float AmountInCredit { get; set; }
        public float AmountOutDebit { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool Posted { get; set; }
        public long MsfAccountPaymodeId { get; set; }
        public string Narration { get; set; }
        public long MfsSystemTransactionReceiptId { get; set; }

        public virtual MfsEntityAccount MfsEntityAccount { get; set; }
        public virtual MsfSystemTransactionReceipt MfsSystemTransactionReceipt { get; set; }
        public virtual MsfAccountPaymode MsfAccountPaymode { get; set; }
        public virtual ICollection<MfsEntityAccountTransactionAdditionalDetail> MfsEntityAccountTransactionAdditionalDetails { get; set; }
    }
}
