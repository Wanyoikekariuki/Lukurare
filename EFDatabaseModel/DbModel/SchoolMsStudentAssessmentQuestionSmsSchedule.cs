using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsStudentAssessmentQuestionSmsSchedule
    {
        public SchoolMsStudentAssessmentQuestionSmsSchedule()
        {
            SchoolMsSmsToSends = new HashSet<SchoolMsSmsToSend>();
        }

        public long Id { get; set; }
        public long SchoolMsAssessmentQuestionId { get; set; }
        public long SchoolMsStudentId { get; set; }
        public string CapturedResponse { get; set; }
        public long? SchoolMsSmsInboxId { get; set; }
        public long? SchoolMsSmsToSendId { get; set; }
        public bool? ResponseIsCorrectAnswer { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public DateTime? QueuedTime { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public bool Expired { get; set; }
        public bool Responded { get; set; }
        public DateTime? ResponseSendTime { get; set; }
        public string ResponseBatchJobId { get; set; }

        public virtual SchoolMsAssessmentQuestion SchoolMsAssessmentQuestion { get; set; }
        public virtual SchoolMsSmsInbox SchoolMsSmsInbox { get; set; }
        public virtual SchoolMsSmsToSend SchoolMsSmsToSend { get; set; }
        public virtual SchoolMsStudent SchoolMsStudent { get; set; }
        public virtual ICollection<SchoolMsSmsToSend> SchoolMsSmsToSends { get; set; }
    }
}
