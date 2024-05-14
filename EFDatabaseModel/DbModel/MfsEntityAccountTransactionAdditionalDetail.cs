using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsEntityAccountTransactionAdditionalDetail
    {
        public int Id { get; set; }
        public int MfsAdditionalDetailsId { get; set; }
        public long MfsEntityAccountTransactionsId { get; set; }
        public double KeyValue { get; set; }

        public virtual MfsAccountTypeTransactionAdditionalDetail MfsAdditionalDetails { get; set; }
        public virtual MfsEntityAccountTransaction MfsEntityAccountTransactions { get; set; }
    }
}
