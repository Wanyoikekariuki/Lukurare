using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class ProjectApplication
    {
        public int Id { get; set; }
        public long? AccountEntityId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectApplicationStatusId { get; set; }
        public DateTime? DateApplied { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual CompanyProject Project { get; set; }
        public virtual ProjectApplicationStatus ProjectApplicationStatus { get; set; }
    }
}
