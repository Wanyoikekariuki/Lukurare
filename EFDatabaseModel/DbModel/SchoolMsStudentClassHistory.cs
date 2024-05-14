using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsStudentClassHistory
    {
        public long Id { get; set; }
        public long SchoolMsClassId { get; set; }
        public long SchoolMsStudentId { get; set; }
        public string AcademicYear { get; set; }
        public bool IsCurrentClass { get; set; }
        public string EntryGrade { get; set; }

        public virtual SchoolMsClass SchoolMsClass { get; set; }
        public virtual SchoolMsStudent SchoolMsStudent { get; set; }
    }
}
