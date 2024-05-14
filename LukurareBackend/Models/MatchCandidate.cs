using EFDatabaseModel.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models
{
   public class MatchCandidate
    {
        public AccountEntity AccountEntity { get; set; }
        public long CandidateId { get; set; }

    }
}
