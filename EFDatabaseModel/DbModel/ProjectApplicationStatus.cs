using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class ProjectApplicationStatus
    {
        public ProjectApplicationStatus()
        {
            ProjectApplications = new HashSet<ProjectApplication>();
        }

        public int Id { get; set; }
        public string ApplicationStatus { get; set; }

        public virtual ICollection<ProjectApplication> ProjectApplications { get; set; }
    }
}
