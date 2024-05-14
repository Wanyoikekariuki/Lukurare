using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsLesson
    {
        public SchoolMsLesson()
        {
            SchoolMsLessonSmsSchedules = new HashSet<SchoolMsLessonSmsSchedule>();
            SchoolMsSmsToSends = new HashSet<SchoolMsSmsToSend>();
        }

        public long Id { get; set; }
        public long SchoolMsLessonGuideId { get; set; }
        public string LessonContent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DateToSend { get; set; }
        public bool Cancelled { get; set; }
        public DateTime CancelledDate { get; set; }

        public virtual SchoolMsLessonGuide SchoolMsLessonGuide { get; set; }
        public virtual ICollection<SchoolMsLessonSmsSchedule> SchoolMsLessonSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsSmsToSend> SchoolMsSmsToSends { get; set; }
    }
}
