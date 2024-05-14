using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MsfSystemTransactionReceipt
    {
        public MsfSystemTransactionReceipt()
        {
            MfsEntityAccountTransactions = new HashSet<MfsEntityAccountTransaction>();
        }

        public long Id { get; set; }
        public long SystemTransactionId { get; set; }
        public string ReceiptNo { get; set; }
        public string PaymodeReferenceNo { get; set; }
        public string InternalManualReceiptNo { get; set; }
        public string ExternalSystemReceiptNo { get; set; }

        public virtual SystemTransaction SystemTransaction { get; set; }
        public virtual ICollection<MfsEntityAccountTransaction> MfsEntityAccountTransactions { get; set; }
    }
}
