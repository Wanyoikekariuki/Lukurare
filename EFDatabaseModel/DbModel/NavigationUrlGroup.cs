using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class NavigationUrlGroup
    {
        public NavigationUrlGroup()
        {
            NavigationUrls = new HashSet<NavigationUrl>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<NavigationUrl> NavigationUrls { get; set; }
    }
}
