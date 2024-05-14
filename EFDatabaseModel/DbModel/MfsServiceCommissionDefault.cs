using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MfsServiceCommissionDefault
    {
        public long Id { get; set; }
        public int BillReferenceServiceTypeId { get; set; }
        public float PercentageCommission { get; set; }
        public string Narration { get; set; }
        public long MfsEntityAccountId { get; set; }

        public virtual BillReferenceServiceType BillReferenceServiceType { get; set; }
        public virtual MfsEntityAccount MfsEntityAccount { get; set; }
    }
}
