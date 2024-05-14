using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class UserLoginAttempt
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool LoginSuccessful { get; set; }
        public DateTime AttemptDate { get; set; }
        public TimeSpan AttemptTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public bool AccountDeactivated { get; set; }
        public string IpAddress { get; set; }
        public long? UserLoginCodeId { get; set; }
        public int TransactingBranchId { get; set; }
        public bool ResetPassword { get; set; }
        public bool ScreenLocked { get; set; }
        public bool SessionExpired { get; set; }
        public string AuthToken { get; set; }
        public string OptCode { get; set; }
        public string RequestDomain { get; set; }
        public bool? OtpCodeValidated { get; set; }
        public DateTime? OtpCodeLastSentTime { get; set; }

        public virtual User User { get; set; }
    }
}
