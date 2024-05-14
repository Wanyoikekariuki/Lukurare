using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsSubject
    {
        public SchoolMsSubject()
        {
            SchoolMsAssessmentGroups = new HashSet<SchoolMsAssessmentGroup>();
            SchoolMsClassSubjectTopics = new HashSet<SchoolMsClassSubjectTopic>();
            SchoolMsStudentSubjectLevels = new HashSet<SchoolMsStudentSubjectLevel>();
        }

        public long Id { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string Abbreviation { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<SchoolMsAssessmentGroup> SchoolMsAssessmentGroups { get; set; }
        public virtual ICollection<SchoolMsClassSubjectTopic> SchoolMsClassSubjectTopics { get; set; }
        public virtual ICollection<SchoolMsStudentSubjectLevel> SchoolMsStudentSubjectLevels { get; set; }
    }
}
