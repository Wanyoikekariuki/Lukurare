using System;
using System.Collections.Generic;

#nullable disable

namespace EFDatabaseModel.DbModel
{
    public partial class TimeSchedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DbFriendlyName { get; set; }
    }
}
