using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class orgosContext : DbContext
    {
        public orgosContext()
        {
        }

        public orgosContext(DbContextOptions<orgosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountEntity> AccountEntities { get; set; }
        public virtual DbSet<AccountEntityActiveState> AccountEntityActiveStates { get; set; }
        public virtual DbSet<AccountEntityAppliedSender> AccountEntityAppliedSenders { get; set; }
        public virtual DbSet<AccountEntityContact> AccountEntityContacts { get; set; }
        public virtual DbSet<AccountEntityGroup> AccountEntityGroups { get; set; }
        public virtual DbSet<AccountEntityGroupContact> AccountEntityGroupContacts { get; set; }
        public virtual DbSet<AccountEntitySecondaryIdentificationDocument> AccountEntitySecondaryIdentificationDocuments { get; set; }
        public virtual DbSet<AccountEntityType> AccountEntityTypes { get; set; }
        public virtual DbSet<AccountEntityTypeAdditionalDetail> AccountEntityTypeAdditionalDetails { get; set; }
        public virtual DbSet<AccountEntityTypeAdditionalDetailsValue> AccountEntityTypeAdditionalDetailsValues { get; set; }
        public virtual DbSet<AccountEntityTypeRequiredDocument> AccountEntityTypeRequiredDocuments { get; set; }
        public virtual DbSet<AppliedCountryNetworkSender> AppliedCountryNetworkSenders { get; set; }
        public virtual DbSet<BillReferenceServiceRequest> BillReferenceServiceRequests { get; set; }
        public virtual DbSet<BillReferenceServiceType> BillReferenceServiceTypes { get; set; }
        public virtual DbSet<CompanyModule> CompanyModules { get; set; }
        public virtual DbSet<CompanyProject> CompanyProjects { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryNetwork> CountryNetworks { get; set; }
        public virtual DbSet<CountryNetworkPrefix> CountryNetworkPrefixes { get; set; }
        public virtual DbSet<CountryStructure> CountryStructures { get; set; }
        public virtual DbSet<CountryStructureLevel> CountryStructureLevels { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<IdentificationDocumentType> IdentificationDocumentTypes { get; set; }
        public virtual DbSet<MeterState> MeterStates { get; set; }
        public virtual DbSet<MfsAccountType> MfsAccountTypes { get; set; }
        public virtual DbSet<MfsAccountTypeTransactionAdditionalDetail> MfsAccountTypeTransactionAdditionalDetails { get; set; }
        public virtual DbSet<MfsCurrencyConversion> MfsCurrencyConversions { get; set; }
        public virtual DbSet<MfsCurrencyType> MfsCurrencyTypes { get; set; }
        public virtual DbSet<MfsEntityAccount> MfsEntityAccounts { get; set; }
        public virtual DbSet<MfsEntityAccountTransaction> MfsEntityAccountTransactions { get; set; }
        public virtual DbSet<MfsEntityAccountTransactionAdditionalDetail> MfsEntityAccountTransactionAdditionalDetails { get; set; }
        public virtual DbSet<MfsServiceCommission> MfsServiceCommissions { get; set; }
        public virtual DbSet<MfsServiceCommissionDefault> MfsServiceCommissionDefaults { get; set; }
        public virtual DbSet<MsfAccountPaymode> MsfAccountPaymodes { get; set; }
        public virtual DbSet<MsfSystemTransactionReceipt> MsfSystemTransactionReceipts { get; set; }
        public virtual DbSet<NavigationUrl> NavigationUrls { get; set; }
        public virtual DbSet<NavigationUrlGroup> NavigationUrlGroups { get; set; }
        public virtual DbSet<ProjectApplication> ProjectApplications { get; set; }
        public virtual DbSet<ProjectApplicationStatus> ProjectApplicationStatuses { get; set; }
        public virtual DbSet<RateType> RateTypes { get; set; }
        public virtual DbSet<SchoolMsAnswerOptionLetter> SchoolMsAnswerOptionLetters { get; set; }
        public virtual DbSet<SchoolMsAssessmentAnswerOption> SchoolMsAssessmentAnswerOptions { get; set; }
        public virtual DbSet<SchoolMsAssessmentGroup> SchoolMsAssessmentGroups { get; set; }
        public virtual DbSet<SchoolMsAssessmentQuestion> SchoolMsAssessmentQuestions { get; set; }
        public virtual DbSet<SchoolMsClass> SchoolMsClasses { get; set; }
        public virtual DbSet<SchoolMsClassSubjectTopic> SchoolMsClassSubjectTopics { get; set; }
        public virtual DbSet<SchoolMsInboxMaxSyncedId> SchoolMsInboxMaxSyncedIds { get; set; }
        public virtual DbSet<SchoolMsLesson> SchoolMsLessons { get; set; }
        public virtual DbSet<SchoolMsLessonGuide> SchoolMsLessonGuides { get; set; }
        public virtual DbSet<SchoolMsLessonGuideSmsSchedule> SchoolMsLessonGuideSmsSchedules { get; set; }
        public virtual DbSet<SchoolMsLessonSmsSchedule> SchoolMsLessonSmsSchedules { get; set; }
        public virtual DbSet<SchoolMsLevel> SchoolMsLevels { get; set; }
        public virtual DbSet<SchoolMsSchool> SchoolMsSchools { get; set; }
        public virtual DbSet<SchoolMsSmsInbox> SchoolMsSmsInboxes { get; set; }
        public virtual DbSet<SchoolMsSmsToSend> SchoolMsSmsToSends { get; set; }
        public virtual DbSet<SchoolMsStudent> SchoolMsStudents { get; set; }
        public virtual DbSet<SchoolMsStudentAssessmentQuestionSmsSchedule> SchoolMsStudentAssessmentQuestionSmsSchedules { get; set; }
        public virtual DbSet<SchoolMsStudentClassHistory> SchoolMsStudentClassHistories { get; set; }
        public virtual DbSet<SchoolMsStudentGuardian> SchoolMsStudentGuardians { get; set; }
        public virtual DbSet<SchoolMsStudentParent> SchoolMsStudentParents { get; set; }
        public virtual DbSet<SchoolMsStudentSubjectLevel> SchoolMsStudentSubjectLevels { get; set; }
        public virtual DbSet<SchoolMsSubject> SchoolMsSubjects { get; set; }
        public virtual DbSet<SenderType> SenderTypes { get; set; }
        public virtual DbSet<SmsGateway> SmsGateways { get; set; }
        public virtual DbSet<SmsGatewayCostPrice> SmsGatewayCostPrices { get; set; }
        public virtual DbSet<SmsRatePlanTemplate> SmsRatePlanTemplates { get; set; }
        public virtual DbSet<SubAccount> SubAccounts { get; set; }
        public virtual DbSet<SubAccountBranch> SubAccountBranches { get; set; }
        public virtual DbSet<SupportedUserType> SupportedUserTypes { get; set; }
        public virtual DbSet<SystemModule> SystemModules { get; set; }
        public virtual DbSet<SystemRole> SystemRoles { get; set; }
        public virtual DbSet<SystemTransaction> SystemTransactions { get; set; }
        public virtual DbSet<TicketingDealer> TicketingDealers { get; set; }
        public virtual DbSet<TicketingMerchant> TicketingMerchants { get; set; }
        public virtual DbSet<TicketingPaybill> TicketingPaybills { get; set; }
        public virtual DbSet<TicketingTechnicianTicketTime> TicketingTechnicianTicketTimes { get; set; }
        public virtual DbSet<TicketingTicket> TicketingTickets { get; set; }
        public virtual DbSet<TicketingTicketStatus> TicketingTicketStatuses { get; set; }
        public virtual DbSet<TimeSchedule> TimeSchedules { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBranch> UserBranches { get; set; }
        public virtual DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }
        public virtual DbSet<UserPeriodHistory> UserPeriodHistories { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserSelfRegistrationDetail> UserSelfRegistrationDetails { get; set; }
        public virtual DbSet<UserUpdateLog> UserUpdateLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("name=bulkSMSGatwayConnectionString");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.ToTable("account_entity");

                entity.HasIndex(e => e.AccountEntityActiveStateId, "account_entity_active_state_id");

                entity.HasIndex(e => e.AccountEntityTypeId, "account_entity_type_Id");

                entity.HasIndex(e => e.EntityNo, "entity_no");

                entity.HasIndex(e => e.GenderId, "gender_id");

                entity.HasIndex(e => e.IdentificationDocumentNumber, "identification_document_number");

                entity.HasIndex(e => e.IdentificationDocumentTypeId, "identification_document_type_id");

                entity.HasIndex(e => e.Id, "idx_account_entity_id");

                entity.HasIndex(e => e.SubAccountBranchId, "sub_account_branch_id");

                entity.HasIndex(e => e.WhiteListedDomain, "white_listed_domain")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityActiveStateId)
                    .HasColumnType("int(11)")
                    .HasColumnName("account_entity_active_state_id");

                entity.Property(e => e.AccountEntityTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("account_entity_type_Id");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.EntityDetailsValidated).HasColumnName("entity_details_validated");

                entity.Property(e => e.EntityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("entity_name");

                entity.Property(e => e.EntityNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("entity_no");

                entity.Property(e => e.EntityUserName)
                    .HasMaxLength(45)
                    .HasColumnName("entity_user_name");

                entity.Property(e => e.GenderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("gender_id");

                entity.Property(e => e.IdentificationDocumentNumber)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("identification_document_number");

                entity.Property(e => e.IdentificationDocumentTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("identification_document_type_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsBackUpRecord).HasColumnName("is_back_up_record");

                entity.Property(e => e.Phone1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("phone1");

                entity.Property(e => e.Phone2)
                    .HasMaxLength(30)
                    .HasColumnName("phone2");

                entity.Property(e => e.PhysicalAddress)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("physical_address");

                entity.Property(e => e.PostalAddress)
                    .HasMaxLength(100)
                    .HasColumnName("postal_address");

                entity.Property(e => e.ProfileImageUrl)
                    .HasMaxLength(512)
                    .HasColumnName("profile_image_url");

                entity.Property(e => e.RegDate).HasColumnName("reg_date");

                entity.Property(e => e.SubAccountBranchId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sub_account_branch_id");

                entity.Property(e => e.WhiteListedDomain)
                    .HasMaxLength(100)
                    .HasColumnName("white_listed_domain");

                entity.HasOne(d => d.AccountEntityActiveState)
                    .WithMany(p => p.AccountEntities)
                    .HasForeignKey(d => d.AccountEntityActiveStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_ibfk_1");

                entity.HasOne(d => d.AccountEntityType)
                    .WithMany(p => p.AccountEntities)
                    .HasForeignKey(d => d.AccountEntityTypeId)
                    .HasConstraintName("account_entity_ibfk_5");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AccountEntities)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_ibfk_4");

                entity.HasOne(d => d.IdentificationDocumentType)
                    .WithMany(p => p.AccountEntities)
                    .HasForeignKey(d => d.IdentificationDocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_ibfk_2");

                entity.HasOne(d => d.SubAccountBranch)
                    .WithMany(p => p.AccountEntities)
                    .HasForeignKey(d => d.SubAccountBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_ibfk_3");
            });

            modelBuilder.Entity<AccountEntityActiveState>(entity =>
            {
                entity.ToTable("account_entity_active_state");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("state_name");
            });

            modelBuilder.Entity<AccountEntityAppliedSender>(entity =>
            {
                entity.ToTable("account_entity_applied_sender");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => new { e.SenderName, e.AccountEntityId }, "sender_name")
                    .IsUnique();

                entity.HasIndex(e => e.SenderTypeId, "sender_type_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.Approved).HasColumnName("approved");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.DefaultSender).HasColumnName("default_sender");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.Property(e => e.SenderName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("sender_name");

                entity.Property(e => e.SenderTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sender_type_id");

                entity.Property(e => e.UploadedDocumentLink)
                    .HasMaxLength(500)
                    .HasColumnName("uploaded_document_link");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.AccountEntityAppliedSenders)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_applied_sender_ibfk_1");

                entity.HasOne(d => d.SenderType)
                    .WithMany(p => p.AccountEntityAppliedSenders)
                    .HasForeignKey(d => d.SenderTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_applied_sender_ibfk_2");
            });

            modelBuilder.Entity<AccountEntityContact>(entity =>
            {
                entity.ToTable("account_entity_contact");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => e.BillReferenceServiceTypeId, "bill_reference_service_type_id");

                entity.HasIndex(e => new { e.AccountEntityId, e.ContactPhone }, "contact_phone")
                    .IsUnique();

                entity.HasIndex(e => e.CountryId, "country_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.BillReferenceServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("bill_reference_service_type_id");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(45)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("contact_name");

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("contact_phone");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_id");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.AccountEntityContacts)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_contact_ibfk_3");

                entity.HasOne(d => d.BillReferenceServiceType)
                    .WithMany(p => p.AccountEntityContacts)
                    .HasForeignKey(d => d.BillReferenceServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_contact_ibfk_5");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.AccountEntityContacts)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_contact_ibfk_4");
            });

            modelBuilder.Entity<AccountEntityGroup>(entity =>
            {
                entity.ToTable("account_entity_group");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => new { e.GroupName, e.AccountEntityId }, "group_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("group_name");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.AccountEntityGroups)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_group_ibfk_1");
            });

            modelBuilder.Entity<AccountEntityGroupContact>(entity =>
            {
                entity.ToTable("account_entity_group_contact");

                entity.HasIndex(e => e.AccountEntityContactId, "account_entity_contact_id");

                entity.HasIndex(e => new { e.AccountEntityGroupId, e.AccountEntityContactId }, "account_entity_group_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityContactId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_contact_id");

                entity.Property(e => e.AccountEntityGroupId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_group_id");

                entity.HasOne(d => d.AccountEntityContact)
                    .WithMany(p => p.AccountEntityGroupContacts)
                    .HasForeignKey(d => d.AccountEntityContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_group_contact_ibfk_4");

                entity.HasOne(d => d.AccountEntityGroup)
                    .WithMany(p => p.AccountEntityGroupContacts)
                    .HasForeignKey(d => d.AccountEntityGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_group_contact_ibfk_3");
            });

            modelBuilder.Entity<AccountEntitySecondaryIdentificationDocument>(entity =>
            {
                entity.ToTable("account_entity_secondary_identification_document");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => e.RequiredDocumentsId, "required_documents_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Path)
                    .HasMaxLength(500)
                    .HasColumnName("path");

                entity.Property(e => e.RequiredDocumentsId)
                    .HasColumnType("int(11)")
                    .HasColumnName("required_documents_id");

                entity.Property(e => e.Validated).HasColumnName("validated");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.AccountEntitySecondaryIdentificationDocuments)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_secondary_identification_document_ibfk_1");

                entity.HasOne(d => d.RequiredDocuments)
                    .WithMany(p => p.AccountEntitySecondaryIdentificationDocuments)
                    .HasForeignKey(d => d.RequiredDocumentsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_secondary_identification_document_ibfk_2");
            });

            modelBuilder.Entity<AccountEntityType>(entity =>
            {
                entity.ToTable("account_entity_type");

                entity.HasIndex(e => e.ParentId, "parent_id");

                entity.HasIndex(e => e.Prefix, "prefix");

                entity.HasIndex(e => e.TypeName, "type_name");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("prefix");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<AccountEntityTypeAdditionalDetail>(entity =>
            {
                entity.ToTable("account_entity_type_additional_details");

                entity.HasIndex(e => e.AccountEntityTypeId, "account_entity_type_id");

                entity.HasIndex(e => e.KeyName, "key_name");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("account_entity_type_id");

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("data_type");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("key_name");

                entity.Property(e => e.Required).HasColumnName("required");

                entity.HasOne(d => d.AccountEntityType)
                    .WithMany(p => p.AccountEntityTypeAdditionalDetails)
                    .HasForeignKey(d => d.AccountEntityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_type_additional_details_ibfk_1");
            });

            modelBuilder.Entity<AccountEntityTypeAdditionalDetailsValue>(entity =>
            {
                entity.ToTable("account_entity_type_additional_details_value");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => e.TypeAdditionalDetailsId, "account_entity_type_additional_details_value_ibfk_1");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.TypeAdditionalDetailsId)
                    .HasColumnType("int(11)")
                    .HasColumnName("type_additional_details_id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("value");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.AccountEntityTypeAdditionalDetailsValues)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_type_additional_details_value_ibfk_2");

                entity.HasOne(d => d.TypeAdditionalDetails)
                    .WithMany(p => p.AccountEntityTypeAdditionalDetailsValues)
                    .HasForeignKey(d => d.TypeAdditionalDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_type_additional_details_value_ibfk_1");
            });

            modelBuilder.Entity<AccountEntityTypeRequiredDocument>(entity =>
            {
                entity.ToTable("account_entity_type_required_documents");

                entity.HasIndex(e => e.AccountEntityTypeId, "account_entity_type_id");

                entity.HasIndex(e => e.IdentificationDocumentTypeId, "identification_document_type_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("account_entity_type_id");

                entity.Property(e => e.IdentificationDocumentTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("identification_document_type_id");

                entity.Property(e => e.Required).HasColumnName("required");

                entity.HasOne(d => d.AccountEntityType)
                    .WithMany(p => p.AccountEntityTypeRequiredDocuments)
                    .HasForeignKey(d => d.AccountEntityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_type_required_documents_ibfk_1");

                entity.HasOne(d => d.IdentificationDocumentType)
                    .WithMany(p => p.AccountEntityTypeRequiredDocuments)
                    .HasForeignKey(d => d.IdentificationDocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_entity_type_required_documents_ibfk_2");
            });

            modelBuilder.Entity<AppliedCountryNetworkSender>(entity =>
            {
                entity.ToTable("applied_country_network_sender");

                entity.HasIndex(e => e.AccountEntityAppliedSenderId, "account_entity_applied_sender_id");

                entity.HasIndex(e => e.CountryNetworkId, "country_network_id");

                entity.HasIndex(e => e.SenderTypeId, "sender_type_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityAppliedSenderId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_applied_sender_id");

                entity.Property(e => e.Approved).HasColumnName("approved");

                entity.Property(e => e.CountryNetworkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_network_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.SenderTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sender_type_id");

                entity.HasOne(d => d.AccountEntityAppliedSender)
                    .WithMany(p => p.AppliedCountryNetworkSenders)
                    .HasForeignKey(d => d.AccountEntityAppliedSenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("applied_country_network_sender_ibfk_1");

                entity.HasOne(d => d.CountryNetwork)
                    .WithMany(p => p.AppliedCountryNetworkSenders)
                    .HasForeignKey(d => d.CountryNetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("applied_country_network_sender_ibfk_2");

                entity.HasOne(d => d.SenderType)
                    .WithMany(p => p.AppliedCountryNetworkSenders)
                    .HasForeignKey(d => d.SenderTypeId)
                    .HasConstraintName("applied_country_network_sender_ibfk_3");
            });

            modelBuilder.Entity<BillReferenceServiceRequest>(entity =>
            {
                entity.ToTable("bill_reference_service_request");

                entity.HasIndex(e => e.AccountEntityContactId, "account_entity_contact_id");

                entity.HasIndex(e => e.SystemTransactionId, "system_transaction_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityContactId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_contact_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Attempts)
                    .HasColumnType("int(11)")
                    .HasColumnName("attempts");

                entity.Property(e => e.Narration)
                    .IsRequired()
                    .HasMaxLength(270)
                    .HasColumnName("narration");

                entity.Property(e => e.Processed).HasColumnName("processed");

                entity.Property(e => e.ProcessedDate).HasColumnName("processed_date");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.SystemTransactionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_transaction_id");

                entity.HasOne(d => d.AccountEntityContact)
                    .WithMany(p => p.BillReferenceServiceRequests)
                    .HasForeignKey(d => d.AccountEntityContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bill_reference_service_request_ibfk_1");

                entity.HasOne(d => d.SystemTransaction)
                    .WithMany(p => p.BillReferenceServiceRequests)
                    .HasForeignKey(d => d.SystemTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bill_reference_service_request_ibfk_2");
            });

            modelBuilder.Entity<BillReferenceServiceType>(entity =>
            {
                entity.ToTable("bill_reference_service_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ServiceId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("service_id");

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("service_name");
            });

            modelBuilder.Entity<CompanyModule>(entity =>
            {
                entity.ToTable("company_module");

                entity.HasIndex(e => e.SubAccountId, "sub_account_id");

                entity.HasIndex(e => e.SystemModuleId, "system_module_id");

                entity.HasIndex(e => new { e.SystemModuleId, e.SubAccountId }, "system_module_id_account_id_unq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.OneTransactionStagePerUser).HasColumnName("one_transaction_stage_per_user");

                entity.Property(e => e.SubAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sub_account_id");

                entity.Property(e => e.SystemModuleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("system_module_id");

                entity.HasOne(d => d.SubAccount)
                    .WithMany(p => p.CompanyModules)
                    .HasForeignKey(d => d.SubAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("company_module_ibfk_2");

                entity.HasOne(d => d.SystemModule)
                    .WithMany(p => p.CompanyModules)
                    .HasForeignKey(d => d.SystemModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("company_module_ibfk_1");
            });

            modelBuilder.Entity<CompanyProject>(entity =>
            {
                entity.ToTable("company_project");

                entity.HasIndex(e => e.AccountEntityId, "AccountEntityId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AccountEntityId).HasColumnType("bigint(11)");

                entity.Property(e => e.DatePosted).HasColumnType("date");

                entity.Property(e => e.OpenPosition).HasColumnType("int(11)");

                entity.Property(e => e.PostAs).HasMaxLength(255);

                entity.Property(e => e.ProjectCategory).HasMaxLength(255);

                entity.Property(e => e.ProjectDetails).HasColumnType("mediumtext");

                entity.Property(e => e.ProjectEnd).HasColumnType("date");

                entity.Property(e => e.ProjectLocation).HasMaxLength(255);

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.ProjectRecommendation).HasColumnType("mediumtext");

                entity.Property(e => e.ProjectStart).HasColumnType("date");

                entity.Property(e => e.WorkScope).HasColumnType("mediumtext");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.CompanyProjects)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("company_project_ibfk_1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.HasIndex(e => e.CountryCode, "country_code_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.CountryName, "country_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("country_code");

                entity.Property(e => e.CountryIcon)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("country_icon");

                entity.Property(e => e.CountryIso)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("country_iso");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country_name");

                entity.Property(e => e.LowestCountryStructureLevel)
                    .HasColumnType("int(11)")
                    .HasColumnName("lowest_country_structure_level");
            });

            modelBuilder.Entity<CountryNetwork>(entity =>
            {
                entity.ToTable("country_network");

                entity.HasIndex(e => e.CountryId, "country_id");

                entity.HasIndex(e => e.MccMnc, "mcc-mnc")
                    .IsUnique();

                entity.HasIndex(e => e.Mcc, "mccmnc_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_id");

                entity.Property(e => e.Mcc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("mcc");

                entity.Property(e => e.MccMnc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("mcc-mnc");

                entity.Property(e => e.Mnc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("mnc");

                entity.Property(e => e.NetworkName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("network_name");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryNetworks)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("country_network_ibfk_1");
            });

            modelBuilder.Entity<CountryNetworkPrefix>(entity =>
            {
                entity.ToTable("country_network_prefix");

                entity.HasIndex(e => e.CountryNetworkId, "country_network_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryNetworkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_network_id");

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("prefix");

                entity.HasOne(d => d.CountryNetwork)
                    .WithMany(p => p.CountryNetworkPrefixes)
                    .HasForeignKey(d => d.CountryNetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("country_network_prefix_ibfk_1");
            });

            modelBuilder.Entity<CountryStructure>(entity =>
            {
                entity.ToTable("country_structure");

                entity.HasIndex(e => e.CountryStructureLevelId, "country_structure_level_id");

                entity.HasIndex(e => e.ParentCountryStructureId, "parent_country_structure_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryStructureLevelId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("country_structure_level_id");

                entity.Property(e => e.ParentCountryStructureId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent_country_structure_id");

                entity.Property(e => e.StructureName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("structure_name");

                entity.HasOne(d => d.CountryStructureLevel)
                    .WithMany(p => p.CountryStructures)
                    .HasForeignKey(d => d.CountryStructureLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("country_structure_ibfk_3");

                entity.HasOne(d => d.ParentCountryStructure)
                    .WithMany(p => p.InverseParentCountryStructure)
                    .HasForeignKey(d => d.ParentCountryStructureId)
                    .HasConstraintName("country_structure_ibfk_2");
            });

            modelBuilder.Entity<CountryStructureLevel>(entity =>
            {
                entity.ToTable("country_structure_level");

                entity.HasIndex(e => e.CountryId, "country_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_id");

                entity.Property(e => e.Level)
                    .HasColumnType("int(11)")
                    .HasColumnName("level");

                entity.Property(e => e.LevelName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("level_name");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryStructureLevels)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("country_structure_level_ibfk_1");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.EmployeeNumber)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("employee_number");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_ibfk_1");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("abbreviation");

                entity.Property(e => e.GenderName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("gender_name");
            });

            modelBuilder.Entity<IdentificationDocumentType>(entity =>
            {
                entity.ToTable("identification_document_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("document_name");
            });

            modelBuilder.Entity<MeterState>(entity =>
            {
                entity.ToTable("meter_state");

                entity.HasIndex(e => e.MfsEntityAccountId, "mfs_entity_account_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.MeterStatus).HasColumnName("meter_status");

                entity.Property(e => e.MfsEntityAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_entity_account_id");

                entity.HasOne(d => d.MfsEntityAccount)
                    .WithMany(p => p.MeterStates)
                    .HasForeignKey(d => d.MfsEntityAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("meter_state_ibfk_1");
            });

            modelBuilder.Entity<MfsAccountType>(entity =>
            {
                entity.ToTable("mfs_account_type");

                entity.HasIndex(e => e.SubAccountId, "sub_account_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("account_code");

                entity.Property(e => e.AccountNameAlias)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("account_name_alias");

                entity.Property(e => e.AccountTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("account_type_name");

                entity.Property(e => e.SubAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sub_account_id");

                entity.HasOne(d => d.SubAccount)
                    .WithMany(p => p.MfsAccountTypes)
                    .HasForeignKey(d => d.SubAccountId)
                    .HasConstraintName("mfs_account_type_ibfk_1");
            });

            modelBuilder.Entity<MfsAccountTypeTransactionAdditionalDetail>(entity =>
            {
                entity.ToTable("mfs_account_type_transaction_additional_details");

                entity.HasIndex(e => new { e.MfsAccountTypeId, e.KeyName }, "mfs_account_type_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.KeyDataType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("key_data_type");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("key_name");

                entity.Property(e => e.MfsAccountTypeId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_account_type_id");

                entity.HasOne(d => d.MfsAccountType)
                    .WithMany(p => p.MfsAccountTypeTransactionAdditionalDetails)
                    .HasForeignKey(d => d.MfsAccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_account_type_transaction_additional_details_ibfk_1");
            });

            modelBuilder.Entity<MfsCurrencyConversion>(entity =>
            {
                entity.ToTable("mfs_currency_conversion");

                entity.HasIndex(e => new { e.BaseCurrencyId, e.QuoteCurrencyId }, "base_and_quote")
                    .IsUnique();

                entity.HasIndex(e => e.BaseCurrencyId, "base_index");

                entity.HasIndex(e => e.QuoteCurrencyId, "quote_index");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AskSellingPrice).HasColumnName("ask_selling_price");

                entity.Property(e => e.BaseCurrencyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("base_currency_id");

                entity.Property(e => e.BidBuyingPrice).HasColumnName("bid_buying_price");

                entity.Property(e => e.CurrentExchangeRate).HasColumnName("current_exchange_rate");

                entity.Property(e => e.QuoteCurrencyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("quote_currency_id");

                entity.HasOne(d => d.BaseCurrency)
                    .WithMany(p => p.MfsCurrencyConversionBaseCurrencies)
                    .HasForeignKey(d => d.BaseCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_currency_conversion_ibfk_1");

                entity.HasOne(d => d.QuoteCurrency)
                    .WithMany(p => p.MfsCurrencyConversionQuoteCurrencies)
                    .HasForeignKey(d => d.QuoteCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_currency_conversion_ibfk_2");
            });

            modelBuilder.Entity<MfsCurrencyType>(entity =>
            {
                entity.ToTable("mfs_currency_type");

                entity.HasIndex(e => e.CurrencyCode, "currency_code")
                    .IsUnique();

                entity.HasIndex(e => e.CurrencyName, "currency_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("currency_code");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("currency_name");

                entity.Property(e => e.CurrencySymbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("currency_symbol");
            });

            modelBuilder.Entity<MfsEntityAccount>(entity =>
            {
                entity.ToTable("mfs_entity_account");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => e.AccountNumber, "account_number")
                    .IsUnique();

                entity.HasIndex(e => e.MfsAccountTypeId, "mfs_account_type_id");

                entity.HasIndex(e => e.MfsCurrencyTypeId, "mfs_currency_type_id");

                entity.HasIndex(e => e.SystemTransactionId, "system_transaction_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("account_name");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("account_number");

                entity.Property(e => e.DateCreated).HasColumnName("date_created");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.MfsAccountTypeId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_account_type_id");

                entity.Property(e => e.MfsCurrencyTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("mfs_currency_type_id");

                entity.Property(e => e.SystemTransactionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_transaction_id");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.MfsEntityAccounts)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_ibfk_1");

                entity.HasOne(d => d.MfsAccountType)
                    .WithMany(p => p.MfsEntityAccounts)
                    .HasForeignKey(d => d.MfsAccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_ibfk_2");

                entity.HasOne(d => d.MfsCurrencyType)
                    .WithMany(p => p.MfsEntityAccounts)
                    .HasForeignKey(d => d.MfsCurrencyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_ibfk_3");

                entity.HasOne(d => d.SystemTransaction)
                    .WithMany(p => p.MfsEntityAccounts)
                    .HasForeignKey(d => d.SystemTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_ibfk_4");
            });

            modelBuilder.Entity<MfsEntityAccountTransaction>(entity =>
            {
                entity.ToTable("mfs_entity_account_transaction");

                entity.HasIndex(e => e.MfsEntityAccountId, "mfs_entity_account_id");

                entity.HasIndex(e => e.MfsSystemTransactionReceiptId, "mfs_system_transaction_receipt_id");

                entity.HasIndex(e => e.MsfAccountPaymodeId, "msf_account_paymode_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AmountInCredit).HasColumnName("amount_in_credit");

                entity.Property(e => e.AmountOutDebit).HasColumnName("amount_out_debit");

                entity.Property(e => e.MfsEntityAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_entity_account_id");

                entity.Property(e => e.MfsSystemTransactionReceiptId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_system_transaction_receipt_id");

                entity.Property(e => e.MsfAccountPaymodeId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("msf_account_paymode_id");

                entity.Property(e => e.Narration)
                    .HasMaxLength(250)
                    .HasColumnName("narration");

                entity.Property(e => e.Posted).HasColumnName("posted");

                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

                entity.Property(e => e.ValueDate).HasColumnName("value_date");

                entity.HasOne(d => d.MfsEntityAccount)
                    .WithMany(p => p.MfsEntityAccountTransactions)
                    .HasForeignKey(d => d.MfsEntityAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_transaction_ibfk_2");

                entity.HasOne(d => d.MfsSystemTransactionReceipt)
                    .WithMany(p => p.MfsEntityAccountTransactions)
                    .HasForeignKey(d => d.MfsSystemTransactionReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_transaction_ibfk_1");

                entity.HasOne(d => d.MsfAccountPaymode)
                    .WithMany(p => p.MfsEntityAccountTransactions)
                    .HasForeignKey(d => d.MsfAccountPaymodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_transaction_ibfk_3");
            });

            modelBuilder.Entity<MfsEntityAccountTransactionAdditionalDetail>(entity =>
            {
                entity.ToTable("mfs_entity_account_transaction_additional_details");

                entity.HasIndex(e => new { e.MfsAdditionalDetailsId, e.MfsEntityAccountTransactionsId, e.KeyValue }, "mfs_additional_details_id");

                entity.HasIndex(e => e.MfsEntityAccountTransactionsId, "mfs_entity_account_transactions_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.KeyValue).HasColumnName("key_value");

                entity.Property(e => e.MfsAdditionalDetailsId)
                    .HasColumnType("int(11)")
                    .HasColumnName("mfs_additional_details_id");

                entity.Property(e => e.MfsEntityAccountTransactionsId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_entity_account_transactions_id");

                entity.HasOne(d => d.MfsAdditionalDetails)
                    .WithMany(p => p.MfsEntityAccountTransactionAdditionalDetails)
                    .HasForeignKey(d => d.MfsAdditionalDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_transaction_additional_details_ibfk_1");

                entity.HasOne(d => d.MfsEntityAccountTransactions)
                    .WithMany(p => p.MfsEntityAccountTransactionAdditionalDetails)
                    .HasForeignKey(d => d.MfsEntityAccountTransactionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_entity_account_transaction_additional_details_ibfk_2");
            });

            modelBuilder.Entity<MfsServiceCommission>(entity =>
            {
                entity.ToTable("mfs_service_commission");

                entity.HasIndex(e => e.BillReferenceServiceTypeId, "bill_reference_service_type_id");

                entity.HasIndex(e => new { e.MfsEntityAccountId, e.BillReferenceServiceTypeId }, "mfs_entity_account_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.BillReferenceServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("bill_reference_service_type_id");

                entity.Property(e => e.MfsEntityAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_entity_account_id");

                entity.Property(e => e.Narration)
                    .HasMaxLength(200)
                    .HasColumnName("narration");

                entity.Property(e => e.PercentageCommission).HasColumnName("percentage_commission");

                entity.HasOne(d => d.BillReferenceServiceType)
                    .WithMany(p => p.MfsServiceCommissions)
                    .HasForeignKey(d => d.BillReferenceServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_service_commission_ibfk_2");

                entity.HasOne(d => d.MfsEntityAccount)
                    .WithMany(p => p.MfsServiceCommissions)
                    .HasForeignKey(d => d.MfsEntityAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_service_commission_ibfk_1");
            });

            modelBuilder.Entity<MfsServiceCommissionDefault>(entity =>
            {
                entity.ToTable("mfs_service_commission_default");

                entity.HasIndex(e => new { e.BillReferenceServiceTypeId, e.MfsEntityAccountId }, "bill_reference_service_type_id")
                    .IsUnique();

                entity.HasIndex(e => e.MfsEntityAccountId, "mfs_entity_account_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.BillReferenceServiceTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("bill_reference_service_type_id");

                entity.Property(e => e.MfsEntityAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("mfs_entity_account_id");

                entity.Property(e => e.Narration)
                    .HasMaxLength(200)
                    .HasColumnName("narration");

                entity.Property(e => e.PercentageCommission).HasColumnName("percentage_commission");

                entity.HasOne(d => d.BillReferenceServiceType)
                    .WithMany(p => p.MfsServiceCommissionDefaults)
                    .HasForeignKey(d => d.BillReferenceServiceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_service_commission_default_ibfk_2");

                entity.HasOne(d => d.MfsEntityAccount)
                    .WithMany(p => p.MfsServiceCommissionDefaults)
                    .HasForeignKey(d => d.MfsEntityAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mfs_service_commission_default_ibfk_1");
            });

            modelBuilder.Entity<MsfAccountPaymode>(entity =>
            {
                entity.ToTable("msf_account_paymode");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.IsDeferredMode).HasColumnName("is_deferred_mode");

                entity.Property(e => e.PaymodeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("paymode_name");

                entity.Property(e => e.RequiresReferenceNo).HasColumnName("requires_reference_no");
            });

            modelBuilder.Entity<MsfSystemTransactionReceipt>(entity =>
            {
                entity.ToTable("msf_system_transaction_receipt");

                entity.HasIndex(e => e.ReceiptNo, "receipt_no");

                entity.HasIndex(e => e.SystemTransactionId, "system_transaction_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.ExternalSystemReceiptNo)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("external_system_receipt_no");

                entity.Property(e => e.InternalManualReceiptNo)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("internal_manual_receipt_no");

                entity.Property(e => e.PaymodeReferenceNo)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("paymode_reference_no");

                entity.Property(e => e.ReceiptNo)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("receipt_no");

                entity.Property(e => e.SystemTransactionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_transaction_id");

                entity.HasOne(d => d.SystemTransaction)
                    .WithMany(p => p.MsfSystemTransactionReceipts)
                    .HasForeignKey(d => d.SystemTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("msf_system_transaction_receipt_ibfk_1");
            });

            modelBuilder.Entity<NavigationUrl>(entity =>
            {
                entity.ToTable("navigation_url");

                entity.HasIndex(e => e.NavigationUrlChildId, "navigation_url_child_id");

                entity.HasIndex(e => e.NavigationUrlGroupId, "navigation_url_group_id");

                entity.HasIndex(e => e.NavigationUrlParentId, "navigation_url_parent_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.NavigationUrlChildId)
                    .HasColumnType("int(11)")
                    .HasColumnName("navigation_url_child_id");

                entity.Property(e => e.NavigationUrlGroupId)
                    .HasColumnType("int(11)")
                    .HasColumnName("navigation_url_group_id");

                entity.Property(e => e.NavigationUrlParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("navigation_url_parent_id");

                entity.Property(e => e.RelativeUrl)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("relative_url");

                entity.Property(e => e.RootParentNode).HasColumnName("root_parent_node");

                entity.Property(e => e.StyleClass)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("style_class");

                entity.Property(e => e.UrlName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("url_name");

                entity.HasOne(d => d.NavigationUrlChild)
                    .WithMany(p => p.InverseNavigationUrlChild)
                    .HasForeignKey(d => d.NavigationUrlChildId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("navigation_url_ibfk_1");

                entity.HasOne(d => d.NavigationUrlGroup)
                    .WithMany(p => p.NavigationUrls)
                    .HasForeignKey(d => d.NavigationUrlGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("navigation_url_ibfk_3");
            });

            modelBuilder.Entity<NavigationUrlGroup>(entity =>
            {
                entity.ToTable("navigation_url_group");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(45)
                    .HasColumnName("group_name");
            });

            modelBuilder.Entity<ProjectApplication>(entity =>
            {
                entity.ToTable("project_application");

                entity.HasIndex(e => e.AccountEntityId, "AccountEntityId");

                entity.HasIndex(e => e.ProjectApplicationStatusId, "ProjectApplication_StatusId");

                entity.HasIndex(e => e.ProjectId, "ProjectId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AccountEntityId).HasColumnType("bigint(11)");

                entity.Property(e => e.ProjectApplicationStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ProjectApplication_StatusId");

                entity.Property(e => e.ProjectId).HasColumnType("int(11)");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.ProjectApplications)
                    .HasForeignKey(d => d.AccountEntityId)
                    .HasConstraintName("project_application_ibfk_1");

                entity.HasOne(d => d.ProjectApplicationStatus)
                    .WithMany(p => p.ProjectApplications)
                    .HasForeignKey(d => d.ProjectApplicationStatusId)
                    .HasConstraintName("project_application_ibfk_2");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectApplications)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("project_application_ibfk_3");
            });

            modelBuilder.Entity<ProjectApplicationStatus>(entity =>
            {
                entity.ToTable("project_application_status");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ApplicationStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Application_Status");
            });

            modelBuilder.Entity<RateType>(entity =>
            {
                entity.ToTable("rate_type");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SchoolMsAnswerOptionLetter>(entity =>
            {
                entity.ToTable("school_ms_answer_option_letter");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.OptionLetter)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("option_letter");
            });

            modelBuilder.Entity<SchoolMsAssessmentAnswerOption>(entity =>
            {
                entity.ToTable("school_ms_assessment_answer_option");

                entity.HasIndex(e => e.SchoolMsAssessmentId, "schoo_ms_assessment_id");

                entity.HasIndex(e => e.SchoolMsAnswerOptionLetterId, "school_ms_answer_option_letter_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.SchoolMsAnswerOptionLetterId)
                    .HasColumnType("int(11)")
                    .HasColumnName("school_ms_answer_option_letter_id");

                entity.Property(e => e.SchoolMsAssessmentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_assessment_id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("value");

                entity.HasOne(d => d.SchoolMsAnswerOptionLetter)
                    .WithMany(p => p.SchoolMsAssessmentAnswerOptions)
                    .HasForeignKey(d => d.SchoolMsAnswerOptionLetterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_answer_option_ibfk_1");

                entity.HasOne(d => d.SchoolMsAssessment)
                    .WithMany(p => p.SchoolMsAssessmentAnswerOptions)
                    .HasForeignKey(d => d.SchoolMsAssessmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_answer_option_ibfk_2");
            });

            modelBuilder.Entity<SchoolMsAssessmentGroup>(entity =>
            {
                entity.ToTable("school_ms_assessment_group");

                entity.HasIndex(e => e.SchoolMsClassId, "school_ms_class_id");

                entity.HasIndex(e => e.SchoolMsSubjectId, "school_ms_class_subject_id");

                entity.HasIndex(e => e.SchoolMsLevelId, "school_ms_leve_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("group_name");

                entity.Property(e => e.IsOpenEnded).HasColumnName("is_open_ended");

                entity.Property(e => e.SchoolMsClassId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_class_id");

                entity.Property(e => e.SchoolMsLevelId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_level_id");

                entity.Property(e => e.SchoolMsSubjectId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_subject_id");

                entity.HasOne(d => d.SchoolMsClass)
                    .WithMany(p => p.SchoolMsAssessmentGroups)
                    .HasForeignKey(d => d.SchoolMsClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_group_ibfk_3");

                entity.HasOne(d => d.SchoolMsLevel)
                    .WithMany(p => p.SchoolMsAssessmentGroups)
                    .HasForeignKey(d => d.SchoolMsLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_group_ibfk_2");

                entity.HasOne(d => d.SchoolMsSubject)
                    .WithMany(p => p.SchoolMsAssessmentGroups)
                    .HasForeignKey(d => d.SchoolMsSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_group_ibfk_4");
            });

            modelBuilder.Entity<SchoolMsAssessmentQuestion>(entity =>
            {
                entity.ToTable("school_ms_assessment_question");

                entity.HasIndex(e => e.CorrectOptionLetterId, "school_ms_answer_option_letter_id");

                entity.HasIndex(e => e.SchoolMsAssessmentGroupId, "school_ms_assessment_group_id");

                entity.HasIndex(e => e.SystemTransactionId, "system_transaction_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Cancelled).HasColumnName("cancelled");

                entity.Property(e => e.CancelledDate).HasColumnName("cancelled_date");

                entity.Property(e => e.CorrectAnswerResponseText)
                    .HasMaxLength(255)
                    .HasColumnName("correct_answer_response_text");

                entity.Property(e => e.CorrectOptionLetterId)
                    .HasColumnType("int(11)")
                    .HasColumnName("correct_option_letter_id");

                entity.Property(e => e.DateToSend).HasColumnName("date_to_send");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("question");

                entity.Property(e => e.QuestionNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("question_number");

                entity.Property(e => e.SchoolMsAssessmentGroupId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_assessment_group_id");

                entity.Property(e => e.SystemTransactionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_transaction_id");

                entity.Property(e => e.WrongAnswerResponseText)
                    .HasMaxLength(255)
                    .HasColumnName("wrong_answer_response_text");

                entity.HasOne(d => d.CorrectOptionLetter)
                    .WithMany(p => p.SchoolMsAssessmentQuestions)
                    .HasForeignKey(d => d.CorrectOptionLetterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_question_ibfk_1");

                entity.HasOne(d => d.SchoolMsAssessmentGroup)
                    .WithMany(p => p.SchoolMsAssessmentQuestions)
                    .HasForeignKey(d => d.SchoolMsAssessmentGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_assessment_question_ibfk_2");
            });

            modelBuilder.Entity<SchoolMsClass>(entity =>
            {
                entity.ToTable("school_ms_class");

                entity.HasIndex(e => e.CountryId, "country_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("class_name");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.SchoolMsClasses)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_class_ibfk_1");
            });

            modelBuilder.Entity<SchoolMsClassSubjectTopic>(entity =>
            {
                entity.ToTable("school_ms_class_subject_topic");

                entity.HasIndex(e => e.SchoolMsClassId, "school_ms_class_id");

                entity.HasIndex(e => e.SchoolMsSubjectId, "school_ms_subject_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.DateCreated).HasColumnName("date_created");

                entity.Property(e => e.SchoolMsClassId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_class_id");

                entity.Property(e => e.SchoolMsSubjectId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_subject_id");

                entity.Property(e => e.TopicName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("topic_name");

                entity.HasOne(d => d.SchoolMsClass)
                    .WithMany(p => p.SchoolMsClassSubjectTopics)
                    .HasForeignKey(d => d.SchoolMsClassId)
                    .HasConstraintName("school_ms_class_subject_topic_ibfk_3");

                entity.HasOne(d => d.SchoolMsSubject)
                    .WithMany(p => p.SchoolMsClassSubjectTopics)
                    .HasForeignKey(d => d.SchoolMsSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_class_subject_topic_ibfk_4");
            });

            modelBuilder.Entity<SchoolMsInboxMaxSyncedId>(entity =>
            {
                entity.ToTable("school_ms_inbox_max_synced_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.LastSyncedTime).HasColumnName("last_synced_time");

                entity.Property(e => e.MaxSyncedId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("max_synced_id");

                entity.Property(e => e.ProviderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("provider_id");
            });

            modelBuilder.Entity<SchoolMsLesson>(entity =>
            {
                entity.ToTable("school_ms_lesson");

                entity.HasIndex(e => e.SchoolMsLessonGuideId, "school_ms_guide_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Cancelled).HasColumnName("cancelled");

                entity.Property(e => e.CancelledDate).HasColumnName("cancelled_date");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.DateToSend).HasColumnName("date_to_send");

                entity.Property(e => e.LessonContent)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("lesson_content");

                entity.Property(e => e.SchoolMsLessonGuideId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_lesson_guide_id");

                entity.HasOne(d => d.SchoolMsLessonGuide)
                    .WithMany(p => p.SchoolMsLessons)
                    .HasForeignKey(d => d.SchoolMsLessonGuideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_ibfk_1");
            });

            modelBuilder.Entity<SchoolMsLessonGuide>(entity =>
            {
                entity.ToTable("school_ms_lesson_guide");

                entity.HasIndex(e => e.SchoolMsClassSubjectTopicId, "school_ms_class_subject_topic_id");

                entity.HasIndex(e => e.SchoolMsLevelId, "school_ms_level_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Cancelled).HasColumnName("cancelled");

                entity.Property(e => e.CancelledDate).HasColumnName("cancelled_date");

                entity.Property(e => e.DateCreated).HasColumnName("date_created");

                entity.Property(e => e.DateToSend).HasColumnName("date_to_send");

                entity.Property(e => e.GuideContent)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("guide_content");

                entity.Property(e => e.SchoolMsClassSubjectTopicId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_class_subject_topic_id");

                entity.Property(e => e.SchoolMsLevelId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_level_id");

                entity.HasOne(d => d.SchoolMsClassSubjectTopic)
                    .WithMany(p => p.SchoolMsLessonGuides)
                    .HasForeignKey(d => d.SchoolMsClassSubjectTopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_guide_ibfk_5");

                entity.HasOne(d => d.SchoolMsLevel)
                    .WithMany(p => p.SchoolMsLessonGuides)
                    .HasForeignKey(d => d.SchoolMsLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_guide_ibfk_8");
            });

            modelBuilder.Entity<SchoolMsLessonGuideSmsSchedule>(entity =>
            {
                entity.ToTable("school_ms_lesson_guide_sms_schedule");

                entity.HasIndex(e => e.SchoolMsLessonGuideId, "school_ms_lesson_guide_id");

                entity.HasIndex(e => e.SchoolMsSmsToSendId, "school_ms_sms_to_send_id");

                entity.HasIndex(e => e.SchoolMsStudentId, "school_ms_student_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.SchoolMsLessonGuideId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_lesson_guide_id");

                entity.Property(e => e.SchoolMsSmsToSendId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_sms_to_send_id");

                entity.Property(e => e.SchoolMsStudentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_id");

                entity.HasOne(d => d.SchoolMsLessonGuide)
                    .WithMany(p => p.SchoolMsLessonGuideSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsLessonGuideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_guide_sms_schedule_ibfk1");

                entity.HasOne(d => d.SchoolMsSmsToSend)
                    .WithMany(p => p.SchoolMsLessonGuideSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsSmsToSendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_guide_sms_schedule_ibfk3");

                entity.HasOne(d => d.SchoolMsStudent)
                    .WithMany(p => p.SchoolMsLessonGuideSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_guide_sms_schedule_ibfk2");
            });

            modelBuilder.Entity<SchoolMsLessonSmsSchedule>(entity =>
            {
                entity.ToTable("school_ms_lesson_sms_schedule");

                entity.HasIndex(e => e.SchoolMsLessonId, "school_ms_lesson_guide_id");

                entity.HasIndex(e => e.SchoolMsLessonId, "school_ms_lesson_id");

                entity.HasIndex(e => e.SchoolMsSmsToSendId, "school_ms_sms_to_send_id");

                entity.HasIndex(e => e.SchoolMsStudentId, "school_ms_student_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.SchoolMsLessonId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_lesson_id");

                entity.Property(e => e.SchoolMsSmsToSendId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_sms_to_send_id");

                entity.Property(e => e.SchoolMsStudentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_id");

                entity.HasOne(d => d.SchoolMsLesson)
                    .WithMany(p => p.SchoolMsLessonSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsLessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_sms_schedule_ibfk4");

                entity.HasOne(d => d.SchoolMsSmsToSend)
                    .WithMany(p => p.SchoolMsLessonSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsSmsToSendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_sms_schedule_ibfk3");

                entity.HasOne(d => d.SchoolMsStudent)
                    .WithMany(p => p.SchoolMsLessonSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_lesson_sms_schedule_ibfk2");
            });

            modelBuilder.Entity<SchoolMsLevel>(entity =>
            {
                entity.ToTable("school_ms_level");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.LevelName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("level_name");
            });

            modelBuilder.Entity<SchoolMsSchool>(entity =>
            {
                entity.ToTable("school_ms_school");

                entity.HasIndex(e => e.CountryStructureId, "country_structure_id");

                entity.HasIndex(e => e.SubAccountId, "sub_account_id")
                    .IsUnique();

                entity.HasIndex(e => e.SubAccountId, "sub_account_id_2");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryStructureId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("country_structure_id");

                entity.Property(e => e.SubAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sub_account_id");

                entity.HasOne(d => d.CountryStructure)
                    .WithMany(p => p.SchoolMsSchools)
                    .HasForeignKey(d => d.CountryStructureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_school_ibfk_2");

                entity.HasOne(d => d.SubAccount)
                    .WithOne(p => p.SchoolMsSchool)
                    .HasForeignKey<SchoolMsSchool>(d => d.SubAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_school_ibfk_5");
            });

            modelBuilder.Entity<SchoolMsSmsInbox>(entity =>
            {
                entity.ToTable("school_ms_sms_inbox");

                entity.HasIndex(e => e.RemoteId, "RemoteId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Datesent, "datesent");

                entity.HasIndex(e => e.FromPhone, "from_phone");

                entity.HasIndex(e => e.ProcessedByAssessmentService, "processed_by_assessment_service");

                entity.HasIndex(e => e.SchoolMsSmsToSendId, "school_ms_sms_to_send_id");

                entity.HasIndex(e => e.ToSenderId, "to_sender_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Datesent)
                    .HasColumnName("datesent")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FromPhone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("from_phone");

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("keyword");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("message");

                entity.Property(e => e.ProcessedByAssessmentService).HasColumnName("processed_by_assessment_service");

                entity.Property(e => e.RemoteId).HasColumnType("bigint(20)");

                entity.Property(e => e.SchoolMsSmsToSendId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_sms_to_send_id");

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasMaxLength(251)
                    .HasColumnName("tag")
                    .HasDefaultValueSql("'Unknown'");

                entity.Property(e => e.ToSenderId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("to_sender_id");
            });

            modelBuilder.Entity<SchoolMsSmsToSend>(entity =>
            {
                entity.ToTable("school_ms_sms_to_send");

                entity.HasIndex(e => e.BatchJobId, "batch_job_id");

                entity.HasIndex(e => e.EntityId, "entity_id");

                entity.HasIndex(e => e.PhoneNumber, "phone_number");

                entity.HasIndex(e => e.SchoolMsLessonGuideId, "school_ms_lesson_guide_id");

                entity.HasIndex(e => e.SchoolMsLessonId, "school_ms_lesson_id");

                entity.HasIndex(e => e.SchoolMsSmsInboxId, "school_ms_sms_inbox_id");

                entity.HasIndex(e => e.SchoolMsStudentAssessmentQuestionSmsScheduleId, "school_ms_student_assessment_question_sms_schedule_id");

                entity.HasIndex(e => e.SentTime, "sent_time");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.BatchJobId)
                    .HasMaxLength(100)
                    .HasColumnName("batch_job_id");

                entity.Property(e => e.DeliveredDate).HasColumnName("delivered_date");

                entity.Property(e => e.EntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("entity_id");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.ParentEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent_entity_id");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("phone_number");

                entity.Property(e => e.ScheduledDateTime).HasColumnName("scheduled_date_time");

                entity.Property(e => e.SchoolMsLessonGuideId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_lesson_guide_id");

                entity.Property(e => e.SchoolMsLessonId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_lesson_id");

                entity.Property(e => e.SchoolMsSmsInboxId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_sms_inbox_id");

                entity.Property(e => e.SchoolMsStudentAssessmentQuestionSmsScheduleId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_assessment_question_sms_schedule_id");

                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("sender_id");

                entity.Property(e => e.SentTime).HasColumnName("sent_time");

                entity.Property(e => e.Tag)
                    .HasMaxLength(250)
                    .HasColumnName("tag");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.SchoolMsSmsToSends)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("school_ms_sms_to_send_ibfk4");

                entity.HasOne(d => d.SchoolMsLessonGuide)
                    .WithMany(p => p.SchoolMsSmsToSends)
                    .HasForeignKey(d => d.SchoolMsLessonGuideId)
                    .HasConstraintName("school_ms_sms_to_send_ibfk2");

                entity.HasOne(d => d.SchoolMsLesson)
                    .WithMany(p => p.SchoolMsSmsToSends)
                    .HasForeignKey(d => d.SchoolMsLessonId)
                    .HasConstraintName("school_ms_sms_to_send_ibfk1");

                entity.HasOne(d => d.SchoolMsSmsInbox)
                    .WithMany(p => p.SchoolMsSmsToSends)
                    .HasForeignKey(d => d.SchoolMsSmsInboxId)
                    .HasConstraintName("school_ms_sms_to_send_ibfk5");

                entity.HasOne(d => d.SchoolMsStudentAssessmentQuestionSmsSchedule)
                    .WithMany(p => p.SchoolMsSmsToSends)
                    .HasForeignKey(d => d.SchoolMsStudentAssessmentQuestionSmsScheduleId)
                    .HasConstraintName("school_ms_sms_to_send_ibfk3");
            });

            modelBuilder.Entity<SchoolMsStudent>(entity =>
            {
                entity.ToTable("school_ms_student");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id")
                    .IsUnique();

                entity.HasIndex(e => e.RegionStudentIdentifier, "region_student_identifier");

                entity.HasIndex(e => e.SchoolMsSchoolId, "school_ms_school_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.RegionStudentIdentifier)
                    .HasMaxLength(191)
                    .HasColumnName("region_student_identifier");

                entity.Property(e => e.SchoolMsSchoolId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_school_id");

                entity.HasOne(d => d.AccountEntity)
                    .WithOne(p => p.SchoolMsStudent)
                    .HasForeignKey<SchoolMsStudent>(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_ibfk_1");

                entity.HasOne(d => d.SchoolMsSchool)
                    .WithMany(p => p.SchoolMsStudents)
                    .HasForeignKey(d => d.SchoolMsSchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_ibfk_2");
            });

            modelBuilder.Entity<SchoolMsStudentAssessmentQuestionSmsSchedule>(entity =>
            {
                entity.ToTable("school_ms_student_assessment_question_sms_schedule");

                entity.HasIndex(e => e.Cancelled, "cancelled");

                entity.HasIndex(e => e.Expired, "expired");

                entity.HasIndex(e => e.QueuedTime, "queued_time");

                entity.HasIndex(e => e.Responded, "responded");

                entity.HasIndex(e => e.ResponseBatchJobId, "response_batch_job_id");

                entity.HasIndex(e => e.ResponseSendTime, "response_send_time");

                entity.HasIndex(e => e.SchoolMsAssessmentQuestionId, "school_ms_assessment_question_id");

                entity.HasIndex(e => e.SchoolMsSmsInboxId, "school_ms_sms_inbox_id");

                entity.HasIndex(e => e.SchoolMsSmsToSendId, "school_ms_sms_to_send_id");

                entity.HasIndex(e => e.SchoolMsStudentId, "school_ms_student_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Cancelled).HasColumnName("cancelled");

                entity.Property(e => e.CancelledDate).HasColumnName("cancelled_date");

                entity.Property(e => e.CapturedResponse)
                    .HasMaxLength(100)
                    .HasColumnName("captured_response");

                entity.Property(e => e.Expired).HasColumnName("expired");

                entity.Property(e => e.ExpiryTime).HasColumnName("expiry_time");

                entity.Property(e => e.QueuedTime).HasColumnName("queued_time");

                entity.Property(e => e.Responded).HasColumnName("responded");

                entity.Property(e => e.ResponseBatchJobId)
                    .HasMaxLength(45)
                    .HasColumnName("response_batch_job_id");

                entity.Property(e => e.ResponseIsCorrectAnswer).HasColumnName("response_is_correct_answer");

                entity.Property(e => e.ResponseSendTime).HasColumnName("response_send_time");

                entity.Property(e => e.ScheduledTime).HasColumnName("scheduled_time");

                entity.Property(e => e.SchoolMsAssessmentQuestionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_assessment_question_id");

                entity.Property(e => e.SchoolMsSmsInboxId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_sms_inbox_id");

                entity.Property(e => e.SchoolMsSmsToSendId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_sms_to_send_id");

                entity.Property(e => e.SchoolMsStudentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_id");

                entity.HasOne(d => d.SchoolMsAssessmentQuestion)
                    .WithMany(p => p.SchoolMsStudentAssessmentQuestionSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsAssessmentQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_assessment_question_sms_schedule_ibfk1");

                entity.HasOne(d => d.SchoolMsSmsInbox)
                    .WithMany(p => p.SchoolMsStudentAssessmentQuestionSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsSmsInboxId)
                    .HasConstraintName("student_assessment_question_sms_schedule_ibfk3");

                entity.HasOne(d => d.SchoolMsSmsToSend)
                    .WithMany(p => p.SchoolMsStudentAssessmentQuestionSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsSmsToSendId)
                    .HasConstraintName("student_assessment_question_sms_schedule_ibfk4");

                entity.HasOne(d => d.SchoolMsStudent)
                    .WithMany(p => p.SchoolMsStudentAssessmentQuestionSmsSchedules)
                    .HasForeignKey(d => d.SchoolMsStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_assessment_question_sms_schedule_ibfk2");
            });

            modelBuilder.Entity<SchoolMsStudentClassHistory>(entity =>
            {
                entity.ToTable("school_ms_student_class_history");

                entity.HasIndex(e => e.SchoolMsClassId, "school_ms_class_id");

                entity.HasIndex(e => e.SchoolMsStudentId, "school_ms_student_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AcademicYear)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("academic_year");

                entity.Property(e => e.EntryGrade)
                    .HasMaxLength(200)
                    .HasColumnName("entry_grade");

                entity.Property(e => e.IsCurrentClass).HasColumnName("is_current_class");

                entity.Property(e => e.SchoolMsClassId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_class_id");

                entity.Property(e => e.SchoolMsStudentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_id");

                entity.HasOne(d => d.SchoolMsClass)
                    .WithMany(p => p.SchoolMsStudentClassHistories)
                    .HasForeignKey(d => d.SchoolMsClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_class_history_ibfk_6");

                entity.HasOne(d => d.SchoolMsStudent)
                    .WithMany(p => p.SchoolMsStudentClassHistories)
                    .HasForeignKey(d => d.SchoolMsStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_class_history_ibfk_7");
            });

            modelBuilder.Entity<SchoolMsStudentGuardian>(entity =>
            {
                entity.ToTable("school_ms_student_guardian");

                entity.HasIndex(e => e.SchoolMsStudentParentId, "guardian_account_entity_id");

                entity.HasIndex(e => new { e.SchoolMsStudentId, e.SchoolMsStudentParentId }, "phone_and_student_guardian_number")
                    .IsUnique();

                entity.HasIndex(e => new { e.SchoolMsStudentId, e.SchoolMsStudentParentId, e.StudentGuardianNumber }, "phone_and_student_guardian_number2")
                    .IsUnique();

                entity.HasIndex(e => e.SchoolMsStudentId, "school_ms_student_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.IsPrimaryContact).HasColumnName("is_primary_contact");

                entity.Property(e => e.SchoolMsStudentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_id");

                entity.Property(e => e.SchoolMsStudentParentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_parent_id");

                entity.Property(e => e.StudentGuardianNumber)
                    .HasMaxLength(40)
                    .HasColumnName("student_guardian_number");

                entity.HasOne(d => d.SchoolMsStudent)
                    .WithMany(p => p.SchoolMsStudentGuardians)
                    .HasForeignKey(d => d.SchoolMsStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_guardian_ibfk_3");

                entity.HasOne(d => d.SchoolMsStudentParent)
                    .WithMany(p => p.SchoolMsStudentGuardians)
                    .HasForeignKey(d => d.SchoolMsStudentParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_guardian_ibfk_4");
            });

            modelBuilder.Entity<SchoolMsStudentParent>(entity =>
            {
                entity.ToTable("school_ms_student_parent");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.SchoolMsStudentParents)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_parent_ibfk_1");
            });

            modelBuilder.Entity<SchoolMsStudentSubjectLevel>(entity =>
            {
                entity.ToTable("school_ms_student_subject_level");

                entity.HasIndex(e => e.SchoolMsStudentId, "school_ms_student_id");

                entity.HasIndex(e => e.SchoolMsSubjectId, "school_ms_subject_id");

                entity.HasIndex(e => e.SchoolMsLevelId, "school_ms_subject_level_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.IsCurrentLevel).HasColumnName("is_current_level");

                entity.Property(e => e.SchoolMsLevelId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_level_id");

                entity.Property(e => e.SchoolMsStudentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_student_id");

                entity.Property(e => e.SchoolMsSubjectId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("school_ms_subject_id");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.SchoolMsLevel)
                    .WithMany(p => p.SchoolMsStudentSubjectLevels)
                    .HasForeignKey(d => d.SchoolMsLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_subject_level_ibfk_2");

                entity.HasOne(d => d.SchoolMsStudent)
                    .WithMany(p => p.SchoolMsStudentSubjectLevels)
                    .HasForeignKey(d => d.SchoolMsStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_subject_level_ibfk_1");

                entity.HasOne(d => d.SchoolMsSubject)
                    .WithMany(p => p.SchoolMsStudentSubjectLevels)
                    .HasForeignKey(d => d.SchoolMsSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_student_subject_level_ibfk_4");
            });

            modelBuilder.Entity<SchoolMsSubject>(entity =>
            {
                entity.ToTable("school_ms_subject");

                entity.HasIndex(e => e.CountryId, "country_id");

                entity.HasIndex(e => new { e.SubjectCode, e.CountryId }, "subject_code")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("abbreviation");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.SubjectCode)
                    .IsRequired()
                    .HasMaxLength(191)
                    .HasColumnName("subject_code");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("subject_name");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.SchoolMsSubjects)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("school_ms_subject_ibfk_1");
            });

            modelBuilder.Entity<SenderType>(entity =>
            {
                entity.ToTable("sender_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.RequiredDocumentTemplateLink)
                    .HasMaxLength(500)
                    .HasColumnName("required_document_template_link");

                entity.Property(e => e.SenderTypeName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("sender_type_name");
            });

            modelBuilder.Entity<SmsGateway>(entity =>
            {
                entity.ToTable("sms_gateway");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.GatewayName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("gateway_name");

                entity.Property(e => e.IsOurGateway).HasColumnName("is_our_gateway");
            });

            modelBuilder.Entity<SmsGatewayCostPrice>(entity =>
            {
                entity.ToTable("sms_gateway_cost_price");

                entity.HasIndex(e => e.CountryNetworkId, "country_network_id");

                entity.HasIndex(e => e.SmsGatewayId, "sms_gateway_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.CostPrice).HasColumnName("cost_price");

                entity.Property(e => e.CountryNetworkId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_network_id");

                entity.Property(e => e.DeactivatedDate).HasColumnName("deactivated_date");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.OverwriteExistingPrice).HasColumnName("overwrite_existing_price");

                entity.Property(e => e.SmsGatewayId)
                    .HasColumnType("int(11)")
                    .HasColumnName("sms_gateway_id");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.CountryNetwork)
                    .WithMany(p => p.SmsGatewayCostPrices)
                    .HasForeignKey(d => d.CountryNetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sms_gateway_cost_price_ibfk_7");

                entity.HasOne(d => d.SmsGateway)
                    .WithMany(p => p.SmsGatewayCostPrices)
                    .HasForeignKey(d => d.SmsGatewayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sms_gateway_cost_price_ibfk_8");
            });

            modelBuilder.Entity<SmsRatePlanTemplate>(entity =>
            {
                entity.ToTable("sms_rate_plan_template");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => e.SmsGatewayCostPriceId, "sms_gateway_cost_price_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.DeactivatedDate).HasColumnName("deactivated_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.SellingPrice).HasColumnName("selling_price");

                entity.Property(e => e.SmsGatewayCostPriceId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sms_gateway_cost_price_id");

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("template_name");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.SmsRatePlanTemplates)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sms_rate_plan_template_ibfk_5");

                entity.HasOne(d => d.SmsGatewayCostPrice)
                    .WithMany(p => p.SmsRatePlanTemplates)
                    .HasForeignKey(d => d.SmsGatewayCostPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sms_rate_plan_template_ibfk_6");
            });

            modelBuilder.Entity<SubAccount>(entity =>
            {
                entity.ToTable("sub_account");

                entity.HasIndex(e => e.Id, "UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("account_name");

                entity.Property(e => e.AccountNo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("account_no");

                entity.Property(e => e.ContactPerson)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("contact_person");

                entity.Property(e => e.DateCreated).HasColumnName("date_created");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ParentId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("phone");
            });

            modelBuilder.Entity<SubAccountBranch>(entity =>
            {
                entity.ToTable("sub_account_branch");

                entity.HasIndex(e => e.SubAccountId, "sub_account_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.BranchCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("branch_code");

                entity.Property(e => e.BranchName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("branch_name");

                entity.Property(e => e.BuildingAddress)
                    .IsRequired()
                    .HasColumnName("building_address");

                entity.Property(e => e.Phone1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("phone1");

                entity.Property(e => e.Phone2)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("phone2");

                entity.Property(e => e.PostalAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("postal_address");

                entity.Property(e => e.SubAccountId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sub_account_id");

                entity.HasOne(d => d.SubAccount)
                    .WithMany(p => p.SubAccountBranches)
                    .HasForeignKey(d => d.SubAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sub_account_branch_ibfk_1");
            });

            modelBuilder.Entity<SupportedUserType>(entity =>
            {
                entity.ToTable("supported_user_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("user_type");
            });

            modelBuilder.Entity<SystemModule>(entity =>
            {
                entity.ToTable("system_module");

                entity.HasIndex(e => e.ModuleName, "module_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("module_name");
            });

            modelBuilder.Entity<SystemRole>(entity =>
            {
                entity.ToTable("system_role");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<SystemTransaction>(entity =>
            {
                entity.ToTable("system_transaction");

                entity.HasIndex(e => e.InitiatingCompanySystemModuleId, "initiating_company_system_module_id");

                entity.HasIndex(e => e.InitiatingTransactingBranchId, "initiating_transacting_branch_id");

                entity.HasIndex(e => e.TargetedTransactingBranchId, "targeted_transacting_branch_id");

                entity.HasIndex(e => e.TransactionDate, "transaction_date");

                entity.HasIndex(e => e.TransactionNo, "transaction_no")
                    .IsUnique();

                entity.HasIndex(e => e.TransactionTypesId, "transaction_types_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.HasIndex(e => e.UserLoginAttemptId, "user_login_attempt_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.ExternalSystemRefNo)
                    .HasMaxLength(50)
                    .HasColumnName("external_system_ref_no");

                entity.Property(e => e.InitiatingCompanySystemModuleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("initiating_company_system_module_id");

                entity.Property(e => e.InitiatingTransactingBranchId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("initiating_transacting_branch_id");

                entity.Property(e => e.TargetedTransactingBranchId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("targeted_transacting_branch_id");

                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

                entity.Property(e => e.TransactionNarration)
                    .HasMaxLength(255)
                    .HasColumnName("transaction_narration");

                entity.Property(e => e.TransactionNo)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("transaction_no");

                entity.Property(e => e.TransactionTypesId)
                    .HasColumnType("int(11)")
                    .HasColumnName("transaction_types_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.Property(e => e.UserLoginAttemptId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_login_attempt_id");
            });

            modelBuilder.Entity<TicketingDealer>(entity =>
            {
                entity.ToTable("ticketing_dealer");

                entity.HasIndex(e => e.DealerId, "dealer_id");

                entity.HasIndex(e => e.MerchantId, "merchant_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.DealerId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("dealer_id");

                entity.Property(e => e.MerchantId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("merchant_id");

                entity.HasOne(d => d.Dealer)
                    .WithMany(p => p.TicketingDealers)
                    .HasForeignKey(d => d.DealerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dealer_id");

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.TicketingDealers)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("merchant_id");
            });

            modelBuilder.Entity<TicketingMerchant>(entity =>
            {
                entity.ToTable("ticketing_merchant");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.MerchantName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("merchant_name");

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("prefix");
            });

            modelBuilder.Entity<TicketingPaybill>(entity =>
            {
                entity.ToTable("ticketing_paybill");

                entity.HasIndex(e => e.PaybillNumber, "paybill_number_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MerchantId, "ticketing_merchant_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.MerchantId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("merchant_id");

                entity.Property(e => e.PaybillNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("paybill_number");

                entity.HasOne(d => d.Merchant)
                    .WithMany(p => p.TicketingPaybills)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticketing_merchant");
            });

            modelBuilder.Entity<TicketingTechnicianTicketTime>(entity =>
            {
                entity.ToTable("ticketing_technician_ticket_times");

                entity.HasIndex(e => e.TechnicianUserId, "technicianUserId");

                entity.HasIndex(e => e.TicketId, "ticketId");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.TechnicianUserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("technician_user_id");

                entity.Property(e => e.TicketId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("ticket_id");

                entity.HasOne(d => d.TechnicianUser)
                    .WithMany(p => p.TicketingTechnicianTicketTimes)
                    .HasForeignKey(d => d.TechnicianUserId)
                    .HasConstraintName("technicianUserId");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketingTechnicianTicketTimes)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticketId");
            });

            modelBuilder.Entity<TicketingTicket>(entity =>
            {
                entity.ToTable("ticketing_ticket");

                entity.HasIndex(e => e.DateAdded, "date_added");

                entity.HasIndex(e => e.DateClosed, "date_closed");

                entity.HasIndex(e => e.InitiatingUserId, "initiating_user_id");

                entity.HasIndex(e => e.PhoneNo, "phone_no");

                entity.HasIndex(e => e.ReferenceNumber, "reference_number");

                entity.HasIndex(e => e.TargetingAccountEntityId, "targeting_account_entity_id");

                entity.HasIndex(e => e.TicketNumber, "ticket_number")
                    .IsUnique();

                entity.HasIndex(e => e.TicketStatusId, "ticket_status_id_idx");

                entity.HasIndex(e => e.TicketingPaybillId, "ticketing_paybill_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Complexity)
                    .HasColumnType("int(11)")
                    .HasColumnName("complexity");

                entity.Property(e => e.DateAdded).HasColumnName("date_added");

                entity.Property(e => e.DateClosed).HasColumnName("date_closed");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.InitiatingUserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("initiating_user_id");

                entity.Property(e => e.IsUrgent)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_urgent");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("notes");

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("phone_no");

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("reference_number");

                entity.Property(e => e.TargetingAccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("targeting_account_entity_id");

                entity.Property(e => e.TicketNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("ticket_number");

                entity.Property(e => e.TicketStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ticket_status_id");

                entity.Property(e => e.TicketingPaybillId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("ticketing_paybill_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("title");

                entity.HasOne(d => d.InitiatingUser)
                    .WithMany(p => p.TicketingTickets)
                    .HasForeignKey(d => d.InitiatingUserId)
                    .HasConstraintName("initiating_user_id");

                entity.HasOne(d => d.TargetingAccountEntity)
                    .WithMany(p => p.TicketingTickets)
                    .HasForeignKey(d => d.TargetingAccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("targeting_account_entity_id");

                entity.HasOne(d => d.TicketStatus)
                    .WithMany(p => p.TicketingTickets)
                    .HasForeignKey(d => d.TicketStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticket_status_id");

                entity.HasOne(d => d.TicketingPaybill)
                    .WithMany(p => p.TicketingTickets)
                    .HasForeignKey(d => d.TicketingPaybillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticketing_paybill_id");
            });

            modelBuilder.Entity<TicketingTicketStatus>(entity =>
            {
                entity.ToTable("ticketing_ticket_status");

                entity.HasIndex(e => e.StatusCode, "status_code_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.StatusName, "status_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("status_code");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("status_name");
            });

            modelBuilder.Entity<TimeSchedule>(entity =>
            {
                entity.ToTable("time_schedule");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DbFriendlyName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("db_friendly_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("transaction_type");

                entity.HasIndex(e => e.Id, "id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.AccountEntityId, "account_entity_id");

                entity.HasIndex(e => e.SupportedUserTypeId, "supported_user_type_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountEntityId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("account_entity_id");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("email_address");

                entity.Property(e => e.EncryptionKey)
                    .HasMaxLength(255)
                    .HasColumnName("encryption_key");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.NoOtp).HasColumnName("no_otp");

                entity.Property(e => e.OtpUseMail).HasColumnName("otp_use_mail");

                entity.Property(e => e.OtpUseSms).HasColumnName("otp_use_sms");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.PasswordDuration)
                    .HasColumnType("int(11)")
                    .HasColumnName("password_duration");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("phone_number");

                entity.Property(e => e.SupportedUserTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("supported_user_type_id");

                entity.Property(e => e.UserImagePath)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("user_image_path");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("username");

                entity.HasOne(d => d.AccountEntity)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AccountEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_ibfk_3");

                entity.HasOne(d => d.SupportedUserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SupportedUserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_ibfk_4");
            });

            modelBuilder.Entity<UserBranch>(entity =>
            {
                entity.ToTable("user_branch");

                entity.HasIndex(e => e.SubAccountBranchId, "sub_account_branch_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.SubAccountBranchId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("sub_account_branch_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.SubAccountBranch)
                    .WithMany(p => p.UserBranches)
                    .HasForeignKey(d => d.SubAccountBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_branch_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBranches)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_branch_ibfk_2");
            });

            modelBuilder.Entity<UserLoginAttempt>(entity =>
            {
                entity.ToTable("user_login_attempt");

                entity.HasIndex(e => e.AuthToken, "auth_token_unique")
                    .IsUnique();

                entity.HasIndex(e => new { e.LoginSuccessful, e.AttemptDate, e.AccountDeactivated }, "login_successful");

                entity.HasIndex(e => e.ResetPassword, "screen_locked");

                entity.HasIndex(e => e.ScreenLocked, "screen_lockedIdx");

                entity.HasIndex(e => e.SessionExpired, "session_expired");

                entity.HasIndex(e => e.TransactingBranchId, "transacting_branch_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AccountDeactivated).HasColumnName("account_deactivated");

                entity.Property(e => e.AttemptDate).HasColumnName("attempt_date");

                entity.Property(e => e.AttemptTime).HasColumnName("attempt_time");

                entity.Property(e => e.AuthToken)
                    .HasMaxLength(500)
                    .HasColumnName("auth_token");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(125)
                    .HasColumnName("ip_address");

                entity.Property(e => e.LastAccessTime).HasColumnName("last_access_time");

                entity.Property(e => e.LoginSuccessful).HasColumnName("login_successful");

                entity.Property(e => e.OptCode)
                    .HasMaxLength(45)
                    .HasColumnName("opt_code");

                entity.Property(e => e.OtpCodeLastSentTime).HasColumnName("otp_code_last_sent_time");

                entity.Property(e => e.OtpCodeValidated).HasColumnName("otp_code_validated");

                entity.Property(e => e.RequestDomain)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("request_domain");

                entity.Property(e => e.ResetPassword).HasColumnName("reset_password");

                entity.Property(e => e.ScreenLocked).HasColumnName("screen_locked");

                entity.Property(e => e.SessionExpired).HasColumnName("session_expired");

                entity.Property(e => e.TransactingBranchId)
                    .HasColumnType("int(11)")
                    .HasColumnName("transacting_branch_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.Property(e => e.UserLoginCodeId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_login_code_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLoginAttempts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_login_attempt_ibfk_1");
            });

            modelBuilder.Entity<UserPeriodHistory>(entity =>
            {
                entity.ToTable("user_period_history");

                entity.HasIndex(e => e.IsRenewed, "is_renewed");

                entity.HasIndex(e => e.RenewalDate, "renewal_date");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");

                entity.Property(e => e.IsRenewed).HasColumnName("is_renewed");

                entity.Property(e => e.OldPassword)
                    .HasMaxLength(250)
                    .HasColumnName("old_password");

                entity.Property(e => e.RenewalDate).HasColumnName("renewal_date");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPeriodHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_period_history_ibfk_1");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");

                entity.HasIndex(e => e.SystemRoleId, "system_role_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.SystemRoleId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_role_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.SystemRole)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.SystemRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_role_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_role_ibfk_3");
            });

            modelBuilder.Entity<UserSelfRegistrationDetail>(entity =>
            {
                entity.ToTable("user_self_registration_details");

                entity.HasIndex(e => e.SystemTransactionId, "system_transaction_id")
                    .IsUnique();

                entity.HasIndex(e => e.UserId, "user_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AllClientDetailsUpdated).HasColumnName("all_client_details_updated");

                entity.Property(e => e.ConfirmedEmail).HasColumnName("confirmed_email");

                entity.Property(e => e.ConfirmedPhone).HasColumnName("confirmed_phone");

                entity.Property(e => e.SystemTransactionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_transaction_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserSelfRegistrationDetail)
                    .HasForeignKey<UserSelfRegistrationDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_self_registration_details_ibfk_1");
            });

            modelBuilder.Entity<UserUpdateLog>(entity =>
            {
                entity.ToTable("user_update_logs");

                entity.HasIndex(e => e.BackUserIdLog, "back_user_id_log");

                entity.HasIndex(e => e.SystemTransactionId, "system_transaction_id");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.ApprovalDate).HasColumnName("approval_date");

                entity.Property(e => e.Approved).HasColumnName("approved");

                entity.Property(e => e.BackUserIdLog)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("back_user_id_log");

                entity.Property(e => e.Cancelled).HasColumnName("cancelled");

                entity.Property(e => e.SystemTransactionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("system_transaction_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserUpdateLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_update_logs_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
