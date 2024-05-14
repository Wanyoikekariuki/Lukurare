using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsStudentParent
    {
        public SchoolMsStudentParent()
        {
            SchoolMsStudentGuardians = new HashSet<SchoolMsStudentGuardian>();
        }

        public long Id { get; set; }
        public long AccountEntityId { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual ICollection<SchoolMsStudentGuardian> SchoolMsStudentGuardians { get; set; }
    }
}
