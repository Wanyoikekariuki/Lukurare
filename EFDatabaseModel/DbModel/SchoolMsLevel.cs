using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsLevel
    {
        public SchoolMsLevel()
        {
            SchoolMsAssessmentGroups = new HashSet<SchoolMsAssessmentGroup>();
            SchoolMsLessonGuides = new HashSet<SchoolMsLessonGuide>();
            SchoolMsStudentSubjectLevels = new HashSet<SchoolMsStudentSubjectLevel>();
        }

        public long Id { get; set; }
        public string LevelName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SchoolMsAssessmentGroup> SchoolMsAssessmentGroups { get; set; }
        public virtual ICollection<SchoolMsLessonGuide> SchoolMsLessonGuides { get; set; }
        public virtual ICollection<SchoolMsStudentSubjectLevel> SchoolMsStudentSubjectLevels { get; set; }
    }
}
