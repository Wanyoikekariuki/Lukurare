using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsAccountType
    {
        public MfsAccountType()
        {
            MfsAccountTypeTransactionAdditionalDetails = new HashSet<MfsAccountTypeTransactionAdditionalDetail>();
            MfsEntityAccounts = new HashSet<MfsEntityAccount>();
        }

        public long Id { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountCode { get; set; }
        public string AccountNameAlias { get; set; }
        public long? SubAccountId { get; set; }

        public virtual SubAccount SubAccount { get; set; }
        public virtual ICollection<MfsAccountTypeTransactionAdditionalDetail> MfsAccountTypeTransactionAdditionalDetails { get; set; }
        public virtual ICollection<MfsEntityAccount> MfsEntityAccounts { get; set; }
    }
}
