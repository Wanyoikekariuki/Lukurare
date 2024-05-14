using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsAssessmentGroup
    {
        public SchoolMsAssessmentGroup()
        {
            SchoolMsAssessmentQuestions = new HashSet<SchoolMsAssessmentQuestion>();
        }

        public long Id { get; set; }
        public long SchoolMsLevelId { get; set; }
        public long SchoolMsSubjectId { get; set; }
        public long SchoolMsClassId { get; set; }
        public string GroupName { get; set; }
        public bool IsOpenEnded { get; set; }

        public virtual SchoolMsClass SchoolMsClass { get; set; }
        public virtual SchoolMsLevel SchoolMsLevel { get; set; }
        public virtual SchoolMsSubject SchoolMsSubject { get; set; }
        public virtual ICollection<SchoolMsAssessmentQuestion> SchoolMsAssessmentQuestions { get; set; }
    }
}
