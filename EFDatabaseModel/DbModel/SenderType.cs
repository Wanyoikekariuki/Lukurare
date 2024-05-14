using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SenderType
    {
        public SenderType()
        {
            AccountEntityAppliedSenders = new HashSet<AccountEntityAppliedSender>();
            AppliedCountryNetworkSenders = new HashSet<AppliedCountryNetworkSender>();
        }

        public int Id { get; set; }
        public string SenderTypeName { get; set; }
        public string RequiredDocumentTemplateLink { get; set; }

        public virtual ICollection<AccountEntityAppliedSender> AccountEntityAppliedSenders { get; set; }
        public virtual ICollection<AppliedCountryNetworkSender> AppliedCountryNetworkSenders { get; set; }
    }
}
