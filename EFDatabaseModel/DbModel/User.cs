using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class User
    {
        public User()
        {
            TicketingTechnicianTicketTimes = new HashSet<TicketingTechnicianTicketTime>();
            TicketingTickets = new HashSet<TicketingTicket>();
            UserBranches = new HashSet<UserBranch>();
            UserLoginAttempts = new HashSet<UserLoginAttempt>();
            UserPeriodHistories = new HashSet<UserPeriodHistory>();
            UserRoles = new HashSet<UserRole>();
            UserUpdateLogs = new HashSet<UserUpdateLog>();
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long AccountEntityId { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool OtpUseSms { get; set; }
        public bool OtpUseMail { get; set; }
        public bool NoOtp { get; set; }
        public int PasswordDuration { get; set; }
        public string UserImagePath { get; set; }
        public int SupportedUserTypeId { get; set; }
        public string EncryptionKey { get; set; }

        public virtual AccountEntity AccountEntity { get; set; }
        public virtual SupportedUserType SupportedUserType { get; set; }
        public virtual UserSelfRegistrationDetail UserSelfRegistrationDetail { get; set; }
        public virtual ICollection<TicketingTechnicianTicketTime> TicketingTechnicianTicketTimes { get; set; }
        public virtual ICollection<TicketingTicket> TicketingTickets { get; set; }
        public virtual ICollection<UserBranch> UserBranches { get; set; }
        public virtual ICollection<UserLoginAttempt> UserLoginAttempts { get; set; }
        public virtual ICollection<UserPeriodHistory> UserPeriodHistories { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserUpdateLog> UserUpdateLogs { get; set; }
    }
}
