using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsAccountTypeTransactionAdditionalDetail
    {
        public MfsAccountTypeTransactionAdditionalDetail()
        {
            MfsEntityAccountTransactionAdditionalDetails = new HashSet<MfsEntityAccountTransactionAdditionalDetail>();
        }

        public int Id { get; set; }
        public long MfsAccountTypeId { get; set; }
        public string KeyName { get; set; }
        public string KeyDataType { get; set; }

        public virtual MfsAccountType MfsAccountType { get; set; }
        public virtual ICollection<MfsEntityAccountTransactionAdditionalDetail> MfsEntityAccountTransactionAdditionalDetails { get; set; }
    }
}
