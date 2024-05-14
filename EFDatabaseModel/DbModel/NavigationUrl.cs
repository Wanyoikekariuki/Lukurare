using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class NavigationUrl
    {
        public NavigationUrl()
        {
            InverseNavigationUrlChild = new HashSet<NavigationUrl>();
        }

        public int Id { get; set; }
        public string RelativeUrl { get; set; }
        public string StyleClass { get; set; }
        public int NavigationUrlGroupId { get; set; }
        public string UrlName { get; set; }
        public bool? RootParentNode { get; set; }
        public int? NavigationUrlParentId { get; set; }
        public int NavigationUrlChildId { get; set; }

        public virtual NavigationUrl NavigationUrlChild { get; set; }
        public virtual NavigationUrlGroup NavigationUrlGroup { get; set; }
        public virtual ICollection<NavigationUrl> InverseNavigationUrlChild { get; set; }
    }
}
