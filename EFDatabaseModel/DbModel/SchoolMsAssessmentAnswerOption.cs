using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsAssessmentAnswerOption
    {
        public int Id { get; set; }
        public long SchoolMsAssessmentId { get; set; }
        public int SchoolMsAnswerOptionLetterId { get; set; }
        public string Value { get; set; }

        public virtual SchoolMsAnswerOptionLetter SchoolMsAnswerOptionLetter { get; set; }
        public virtual SchoolMsAssessmentQuestion SchoolMsAssessment { get; set; }
    }
}
