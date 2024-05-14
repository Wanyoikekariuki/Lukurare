using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class UserSelfRegistrationDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long SystemTransactionId { get; set; }
        public bool ConfirmedEmail { get; set; }
        public bool ConfirmedPhone { get; set; }
        public bool AllClientDetailsUpdated { get; set; }

        public virtual User User { get; set; }
    }
}
