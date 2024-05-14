using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsSmsToSend
    {
        public SchoolMsSmsToSend()
        {
            SchoolMsLessonGuideSmsSchedules = new HashSet<SchoolMsLessonGuideSmsSchedule>();
            SchoolMsLessonSmsSchedules = new HashSet<SchoolMsLessonSmsSchedule>();
            SchoolMsStudentAssessmentQuestionSmsSchedules = new HashSet<SchoolMsStudentAssessmentQuestionSmsSchedule>();
        }

        public long Id { get; set; }
        public string SenderId { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public DateTime? SentTime { get; set; }
        public long? SchoolMsLessonId { get; set; }
        public long? SchoolMsLessonGuideId { get; set; }
        public long? SchoolMsStudentAssessmentQuestionSmsScheduleId { get; set; }
        public long? EntityId { get; set; }
        public long? ParentEntityId { get; set; }
        public long? SchoolMsSmsInboxId { get; set; }
        public string BatchJobId { get; set; }
        public string Tag { get; set; }
        public DateTime? DeliveredDate { get; set; }

        public virtual AccountEntity Entity { get; set; }
        public virtual SchoolMsLesson SchoolMsLesson { get; set; }
        public virtual SchoolMsLessonGuide SchoolMsLessonGuide { get; set; }
        public virtual SchoolMsSmsInbox SchoolMsSmsInbox { get; set; }
        public virtual SchoolMsStudentAssessmentQuestionSmsSchedule SchoolMsStudentAssessmentQuestionSmsSchedule { get; set; }
        public virtual ICollection<SchoolMsLessonGuideSmsSchedule> SchoolMsLessonGuideSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsLessonSmsSchedule> SchoolMsLessonSmsSchedules { get; set; }
        public virtual ICollection<SchoolMsStudentAssessmentQuestionSmsSchedule> SchoolMsStudentAssessmentQuestionSmsSchedules { get; set; }
    }
}
