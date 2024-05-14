using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityAppliedSender
    {
        public AccountEntityAppliedSender()
        {
            AppliedCountryNetworkSenders = new HashSet<AppliedCountryNetworkSender>();
        }

        public long Id { get; set; }
        public string SenderName { get; set; }
        public long AccountEntityId { get; set; }
        public bool Approved { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? Deleted { get; set; }
        public int SenderTypeId { get; set; }
        public string Description { get; set; }
        public string UploadedDocumentLink { get; set; }
        public bool DefaultSender { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual SenderType SenderType { get; set; }
        public virtual ICollection<AppliedCountryNetworkSender> AppliedCountryNetworkSenders { get; set; }
    }
}
