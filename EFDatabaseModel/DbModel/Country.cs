using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class Country
    {
        public Country()
        {
            AccountEntityContacts = new HashSet<AccountEntityContact>();
            CountryNetworks = new HashSet<CountryNetwork>();
            CountryStructureLevels = new HashSet<CountryStructureLevel>();
            SchoolMsClasses = new HashSet<SchoolMsClass>();
            SchoolMsSubjects = new HashSet<SchoolMsSubject>();
        }

        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryIso { get; set; }
        public string CountryIcon { get; set; }
        public int? LowestCountryStructureLevel { get; set; }

        public virtual ICollection<AccountEntityContact> AccountEntityContacts { get; set; }
        public virtual ICollection<CountryNetwork> CountryNetworks { get; set; }
        public virtual ICollection<CountryStructureLevel> CountryStructureLevels { get; set; }
        public virtual ICollection<SchoolMsClass> SchoolMsClasses { get; set; }
        public virtual ICollection<SchoolMsSubject> SchoolMsSubjects { get; set; }
    }
}
