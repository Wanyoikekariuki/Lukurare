using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsAssessmentQuestion
    {
        public SchoolMsAssessmentQuestion()
        {
            SchoolMsAssessmentAnswerOptions = new HashSet<SchoolMsAssessmentAnswerOption>();
            SchoolMsStudentAssessmentQuestionSmsSchedules = new HashSet<SchoolMsStudentAssessmentQuestionSmsSchedule>();
        }

        public long Id { get; set; }
        public string Question { get; set; }
        public int QuestionNumber { get; set; }
        public int CorrectOptionLetterId { get; set; }
        public string CorrectAnswerResponseText { get; set; }
        public string WrongAnswerResponseText { get; set; }
        public DateTime DateToSend { get; set; }
        public long SystemTransactionId { get; set; }
        public long SchoolMsAssessmentGroupId { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelledDate { get; set; }

        public virtual SchoolMsAnswerOptionLetter CorrectOptionLetter { get; set; }
        public virtual SchoolMsAssessmentGroup SchoolMsAssessmentGroup { get; set; }
        public virtual ICollection<SchoolMsAssessmentAnswerOption> SchoolMsAssessmentAnswerOptions { get; set; }
        public virtual ICollection<SchoolMsStudentAssessmentQuestionSmsSchedule> SchoolMsStudentAssessmentQuestionSmsSchedules { get; set; }
    }
}
