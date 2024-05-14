using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models
{
    public class AcceptApplication
    {
        public int ProjectId { get; set; }
        public int ApplicationId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string ProjectName { get; set; }

        
    }
}
