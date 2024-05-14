using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class CompanyModule
    {
        public int Id { get; set; }
        public int SystemModuleId { get; set; }
        public long SubAccountId { get; set; }
        public bool OneTransactionStagePerUser { get; set; }

        public virtual SubAccount SubAccount { get; set; }
        public virtual SystemModule SystemModule { get; set; }
    }
}
