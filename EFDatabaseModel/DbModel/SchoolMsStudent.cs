using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsStudent
    {
        public SchoolMsStudent()
        {
            SchoolMsLessonGuideSmsSchedules = new HashSet<SchoolMsLessonGuideSmsSchedule>();
            SchoolMsLessonSmsSchedules = new HashSet<SchoolMsLessonSmsSchedule>();
            SchoolMsStudentAssessmentQuestionSmsSchedules = new HashSet<SchoolMsStudentAssessmentQuestionSmsSchedule>();
            SchoolMsStudentClassHistories = new HashSet<SchoolMsStudentClassHistory>();
            SchoolMsStudentGuardians = new HashSet<SchoolMsStudentGuardian>();
            SchoolMsStudentSubjectLevels = new HashSet<SchoolMsStudentSubjectLevel>();
        }

        public long Id { get; set; }
        public long AccountEntityId { get; set; }
        public string RegionStudentIdentifier { get; set; }
        public long SchoolMsSchoolId { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual SchoolMsSchool SchoolMsSchool { get; set; }
        public virtual ICollection<SchoolMsLessonGuideSmsSchedule> SchoolMsLessonGuideSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsLessonSmsSchedule> SchoolMsLessonSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsStudentAssessmentQuestionSmsSchedule> SchoolMsStudentAssessmentQuestionSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsStudentClassHistory> SchoolMsStudentClassHistories { get; set; }
        public virtual ICollection<SchoolMsStudentGuardian> SchoolMsStudentGuardians { get; set; }
        public virtual ICollection<SchoolMsStudentSubjectLevel> SchoolMsStudentSubjectLevels { get; set; }
    }
}
