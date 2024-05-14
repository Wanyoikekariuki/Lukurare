using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsStudentSubjectLevel
    {
        public long Id { get; set; }
        public long SchoolMsStudentId { get; set; }
        public long SchoolMsSubjectId { get; set; }
        public long SchoolMsLevelId { get; set; }
        public bool IsCurrentLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual SchoolMsLevel SchoolMsLevel { get; set; }
        public virtual SchoolMsStudent SchoolMsStudent { get; set; }
        public virtual SchoolMsSubject SchoolMsSubject { get; set; }
    }
}
