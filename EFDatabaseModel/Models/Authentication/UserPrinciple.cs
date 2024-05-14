using EFDatabaseModel.DbModel;
using System;
using System.Security.Principal;

namespace EFDatabaseModel.Models.Authentication
{
    public class UserPrinciple : GenericPrincipal
    {
        public UserPrinciple(IIdentity identity)
            : base(identity, new string[] { }) { }

        public UserPrinciple(IIdentity identity, User user)
            : this(identity)
        {
            User = user;
        }

        public UserPrinciple(IIdentity identity, User user, DateTime _lastLoginDate)
            : this(identity, user)
        {
            LastLoginDate = _lastLoginDate;
        }

        public UserPrinciple(IIdentity identity, User user, DateTime _lastLoginDate, string lastIP)
            : this(identity, user, _lastLoginDate)
        {
            LastLoginIP = lastIP;
        }

        public User User { get; set; }
        public string Token { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public long? UserLoginAttemptId { get; set; }
    }
}
