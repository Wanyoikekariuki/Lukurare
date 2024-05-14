using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MsfAccountPaymode
    {
        public MsfAccountPaymode()
        {
            MfsEntityAccountTransactions = new HashSet<MfsEntityAccountTransaction>();
        }

        public long Id { get; set; }
        public string PaymodeName { get; set; }
        public bool IsDeferredMode { get; set; }
        public bool RequiresReferenceNo { get; set; }

        public virtual ICollection<MfsEntityAccountTransaction> MfsEntityAccountTransactions { get; set; }
    }
}
