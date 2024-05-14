using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsSmsInbox
    {
        public SchoolMsSmsInbox()
        {
            SchoolMsSmsToSends = new HashSet<SchoolMsSmsToSend>();
            SchoolMsStudentAssessmentQuestionSmsSchedules = new HashSet<SchoolMsStudentAssessmentQuestionSmsSchedule>();
        }

        public long Id { get; set; }
        public string FromPhone { get; set; }
        public string Keyword { get; set; }
        public string ToSenderId { get; set; }
        public string Message { get; set; }
        public DateTime Datesent { get; set; }
        public string Tag { get; set; }
        public long RemoteId { get; set; }
        public bool ProcessedByAssessmentService { get; set; }
        public long? SchoolMsSmsToSendId { get; set; }

        public virtual ICollection<SchoolMsSmsToSend> SchoolMsSmsToSends { get; set; }
        public virtual ICollection<SchoolMsStudentAssessmentQuestionSmsSchedule> SchoolMsStudentAssessmentQuestionSmsSchedules { get; set; }
    }
}
