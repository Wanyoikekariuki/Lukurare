using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsLessonGuide
    {
        public SchoolMsLessonGuide()
        {
            SchoolMsLessonGuideSmsSchedules = new HashSet<SchoolMsLessonGuideSmsSchedule>();
            SchoolMsLessons = new HashSet<SchoolMsLesson>();
            SchoolMsSmsToSends = new HashSet<SchoolMsSmsToSend>();
        }

        public long Id { get; set; }
        public long SchoolMsClassSubjectTopicId { get; set; }
        public string GuideContent { get; set; }
        public long SchoolMsLevelId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateToSend { get; set; }
        public bool Cancelled { get; set; }
        public DateTime CancelledDate { get; set; }

        public virtual SchoolMsClassSubjectTopic SchoolMsClassSubjectTopic { get; set; }
        public virtual SchoolMsLevel SchoolMsLevel { get; set; }
        public virtual ICollection<SchoolMsLessonGuideSmsSchedule> SchoolMsLessonGuideSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsLesson> SchoolMsLessons { get; set; }
        public virtual ICollection<SchoolMsSmsToSend> SchoolMsSmsToSends { get; set; }
    }
}
