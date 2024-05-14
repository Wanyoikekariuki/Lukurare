using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsAnswerOptionLetter
    {
        public SchoolMsAnswerOptionLetter()
        {
            SchoolMsAssessmentAnswerOptions = new HashSet<SchoolMsAssessmentAnswerOption>();
            SchoolMsAssessmentQuestions = new HashSet<SchoolMsAssessmentQuestion>();
        }

        public int Id { get; set; }
        public string OptionLetter { get; set; }

        public virtual ICollection<SchoolMsAssessmentAnswerOption> SchoolMsAssessmentAnswerOptions { get; set; }
        public virtual ICollection<SchoolMsAssessmentQuestion> SchoolMsAssessmentQuestions { get; set; }
    }
}
