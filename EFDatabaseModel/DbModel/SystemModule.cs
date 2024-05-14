using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SystemModule
    {
        public SystemModule()
        {
            CompanyModules = new HashSet<CompanyModule>();
        }

        public int Id { get; set; }
        public string ModuleName { get; set; }

        public virtual ICollection<CompanyModule> CompanyModules { get; set; }
    }
}
