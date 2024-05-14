using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class CompanyProject
    {
        public CompanyProject()
        {
            ProjectApplications = new HashSet<ProjectApplication>();
        }

        public int Id { get; set; }
        public long AccountEntityId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCategory { get; set; }
        public string ProjectDetails { get; set; }
        public DateTime? ProjectStart { get; set; }
        public DateTime? ProjectEnd { get; set; }
        public int? OpenPosition { get; set; }
        public string ProjectRecommendation { get; set; }
        public string WorkScope { get; set; }
        public string PostAs { get; set; }
        public string ProjectLocation { get; set; }
        public DateTime? DatePosted { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual ICollection<ProjectApplication> ProjectApplications { get; set; }
    }
}
