using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class CountryStructure
    {
        public CountryStructure()
        {
            InverseParentCountryStructure = new HashSet<CountryStructure>();
            SchoolMsSchools = new HashSet<SchoolMsSchool>();
        }

        public long Id { get; set; }
        public string StructureName { get; set; }
        public long CountryStructureLevelId { get; set; }
        public long? ParentCountryStructureId { get; set; }

        public virtual CountryStructureLevel CountryStructureLevel { get; set; }
        public virtual CountryStructure ParentCountryStructure { get; set; }
        public virtual ICollection<CountryStructure> InverseParentCountryStructure { get; set; }
        public virtual ICollection<SchoolMsSchool> SchoolMsSchools { get; set; }
    }
}
