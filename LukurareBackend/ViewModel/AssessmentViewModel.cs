using EFDatabaseModel.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.ViewModel
{
    public class AssessmentViewModel
    {
        public long Id { get; set; }
        public long SchoolMsLevelId { get; set; }
        public long SchoolMsClassId { get; set; }
        public long SchoolMsSubjectId { get; set; }
        public long SchoolMsClassSubjectTopicId { get; set; }
        public long SchoolMsAssessmentGroupId { get; set; }
        public virtual SchoolMsAssessmentGroup SchoolMsAssessmentGroup { get; set; }
        public virtual SchoolMsClass SchoolMsClass { get; set; }

        public string Question { get; set; }
        public int QuestionNumber { get; set; }
        public int CorrectOptionLetterId { get; set; }
        public int AssessmentAnswerOptionLetter { get; set; }
        public string CorrectAnswerResponseText { get; set; }
        public string WrongAnswerResponseText { get; set; }
        public string ExamName { get; set; }
        public DateTime DateToSend { get; set; }
        public long SystemTransactionId { get; set; }

        public virtual SchoolMsAnswerOptionLetter CorrectOptionLetter { get; set; }
        public virtual SchoolMsClassSubjectTopic SchoolMsClassSubjectTopic { get; set; }
        public virtual SchoolMsLevel SchoolMsLevel { get; set; }
        public virtual SchoolMsSubject SchoolMsSubject { get; set; }
        public virtual ICollection<SchoolMsAssessmentAnswerOption> SchoolMsAssessmentAnswerOptions { get; set; }
    }
}
