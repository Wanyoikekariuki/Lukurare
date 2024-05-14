using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsStudentGuardian
    {
        public long Id { get; set; }
        public long SchoolMsStudentId { get; set; }
        public long SchoolMsStudentParentId { get; set; }
        public string StudentGuardianNumber { get; set; }
        public bool IsPrimaryContact { get; set; }

        public virtual SchoolMsStudent SchoolMsStudent { get; set; }
        public virtual SchoolMsStudentParent SchoolMsStudentParent { get; set; }
    }
}
