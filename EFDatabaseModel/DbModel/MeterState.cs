using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class MeterState
    {
        public int Id { get; set; }
        public long MfsEntityAccountId { get; set; }
        public bool MeterStatus { get; set; }

        public virtual MfsEntityAccount MfsEntityAccount { get; set; }
    }
}
