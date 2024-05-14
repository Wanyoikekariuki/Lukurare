using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class CountryStructureLevel
    {
        public CountryStructureLevel()
        {
            CountryStructures = new HashSet<CountryStructure>();
        }

        public long Id { get; set; }
        public int Level { get; set; }
        public string LevelName { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<CountryStructure> CountryStructures { get; set; }
    }
}
