using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class CountryNetworkPrefix
    {
        public int Id { get; set; }
        public int CountryNetworkId { get; set; }
        public string Prefix { get; set; }

        public virtual CountryNetwork CountryNetwork { get; set; }
    }
}
