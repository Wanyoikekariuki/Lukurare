using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AppliedCountryNetworkSender
    {
        public long Id { get; set; }
        public long AccountEntityAppliedSenderId { get; set; }
        public int CountryNetworkId { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }
        public int? SenderTypeId { get; set; }

        public virtual AccountEntityAppliedSender AccountEntityAppliedSender { get; set; }
        public virtual CountryNetwork CountryNetwork { get; set; }
        public virtual SenderType SenderType { get; set; }
    }
}
