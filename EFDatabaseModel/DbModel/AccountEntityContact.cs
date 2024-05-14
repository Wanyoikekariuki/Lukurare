using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntityContact
    {
        public AccountEntityContact()
        {
            AccountEntityGroupContacts = new HashSet<AccountEntityGroupContact>();
            BillReferenceServiceRequests = new HashSet<BillReferenceServiceRequest>();
        }

        public long Id { get; set; }
        public long AccountEntityId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int CountryId { get; set; }
        public int BillReferenceServiceTypeId { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual BillReferenceServiceType BillReferenceServiceType { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<AccountEntityGroupContact> AccountEntityGroupContacts { get; set; }
        public virtual ICollection<BillReferenceServiceRequest> BillReferenceServiceRequests { get; set; }
    }
}
