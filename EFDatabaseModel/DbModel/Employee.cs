using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class Employee
    {
        public long Id { get; set; }
        public long AccountEntityId { get; set; }
        public string EmployeeNumber { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
    }
}
