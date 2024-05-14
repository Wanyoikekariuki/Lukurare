using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsClass
    {
        public SchoolMsClass()
        {
            SchoolMsAssessmentGroups = new HashSet<SchoolMsAssessmentGroup>();
            SchoolMsClassSubjectTopics = new HashSet<SchoolMsClassSubjectTopic>();
            SchoolMsStudentClassHistories = new HashSet<SchoolMsStudentClassHistory>();
        }

        public long Id { get; set; }
        public int CountryId { get; set; }
        public string ClassName { get; set; }
        public bool IsActive { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<SchoolMsAssessmentGroup> SchoolMsAssessmentGroups { get; set; }
        public virtual ICollection<SchoolMsClassSubjectTopic> SchoolMsClassSubjectTopics { get; set; }
        public virtual ICollection<SchoolMsStudentClassHistory> SchoolMsStudentClassHistories { get; set; }
    }
}
