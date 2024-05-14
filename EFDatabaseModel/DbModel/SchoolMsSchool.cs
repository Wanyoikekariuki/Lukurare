using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsSchool
    {
        public SchoolMsSchool()
        {
            SchoolMsStudents = new HashSet<SchoolMsStudent>();
        }

        public long Id { get; set; }
        public long SubAccountId { get; set; }
        public long CountryStructureId { get; set; }

        public virtual CountryStructure CountryStructure { get; set; }
        public virtual SubAccount SubAccount { get; set; }
        public virtual ICollection<SchoolMsStudent> SchoolMsStudents { get; set; }
    }
}
