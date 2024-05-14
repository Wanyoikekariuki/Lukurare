using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class SchoolMsInboxMaxSyncedId
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public long MaxSyncedId { get; set; }
        public DateTime LastSyncedTime { get; set; }
    }
}
