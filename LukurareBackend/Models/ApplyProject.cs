using EFDatabaseModel.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models
{
   public class ApplyProject
    {
        public long AccountEntityId { get; set; }

        public int CompanyProjectId { get; set; }

        public string ApplicationStatus { get; set; }
    }
}
