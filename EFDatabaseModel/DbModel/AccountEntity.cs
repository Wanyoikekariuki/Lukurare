using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class AccountEntity
    {
        public AccountEntity()
        {
            AccountEntityAppliedSenders = new HashSet<AccountEntityAppliedSender>();
            AccountEntityContacts = new HashSet<AccountEntityContact>();
            AccountEntityGroups = new HashSet<AccountEntityGroup>();
            AccountEntitySecondaryIdentificationDocuments = new HashSet<AccountEntitySecondaryIdentificationDocument>();
            AccountEntityTypeAdditionalDetailsValues = new HashSet<AccountEntityTypeAdditionalDetailsValue>();
            CompanyProjects = new HashSet<CompanyProject>();
            Employees = new HashSet<Employee>();
            MfsEntityAccounts = new HashSet<MfsEntityAccount>();
            ProjectApplications = new HashSet<ProjectApplication>();
            SchoolMsSmsToSends = new HashSet<SchoolMsSmsToSend>();
            SchoolMsStudentParents = new HashSet<SchoolMsStudentParent>();
            SmsRatePlanTemplates = new HashSet<SmsRatePlanTemplate>();
            TicketingDealers = new HashSet<TicketingDealer>();
            TicketingTickets = new HashSet<TicketingTicket>();
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string EntityNo { get; set; }
        public string EntityName { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalAddress { get; set; }
        public long SubAccountBranchId { get; set; }
        public string WhiteListedDomain { get; set; }
        public string EntityUserName { get; set; }
        public bool IsBackUpRecord { get; set; }
        public bool EntityDetailsValidated { get; set; }
        public string IdentificationDocumentNumber { get; set; }
        public int IdentificationDocumentTypeId { get; set; }
        public int GenderId { get; set; }
        public int AccountEntityActiveStateId { get; set; }
        public string ProfileImageUrl { get; set; }
        public int? AccountEntityTypeId { get; set; }

        public virtual AccountEntityActiveState AccountEntityActiveState { get; set; }
        public virtual AccountEntityType AccountEntityType { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual IdentificationDocumentType IdentificationDocumentType { get; set; }
        public virtual SubAccountBranch SubAccountBranch { get; set; }
        public virtual SchoolMsStudent SchoolMsStudent { get; set; }
        public virtual ICollection<AccountEntityAppliedSender> AccountEntityAppliedSenders { get; set; }
        public virtual ICollection<AccountEntityContact> AccountEntityContacts { get; set; }
        public virtual ICollection<AccountEntityGroup> AccountEntityGroups { get; set; }
        public virtual ICollection<AccountEntitySecondaryIdentificationDocument> AccountEntitySecondaryIdentificationDocuments { get; set; }
        public virtual ICollection<AccountEntityTypeAdditionalDetailsValue> AccountEntityTypeAdditionalDetailsValues { get; set; }
        public virtual ICollection<CompanyProject> CompanyProjects { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<MfsEntityAccount> MfsEntityAccounts { get; set; }
        public virtual ICollection<ProjectApplication> ProjectApplications { get; set; }
        public virtual ICollection<SchoolMsSmsToSend> SchoolMsSmsToSends { get; set; }
        public virtual ICollection<SchoolMsStudentParent> SchoolMsStudentParents { get; set; }
        public virtual ICollection<SmsRatePlanTemplate> SmsRatePlanTemplates { get; set; }
        public virtual ICollection<TicketingDealer> TicketingDealers { get; set; }
        public virtual ICollection<TicketingTicket> TicketingTickets { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
