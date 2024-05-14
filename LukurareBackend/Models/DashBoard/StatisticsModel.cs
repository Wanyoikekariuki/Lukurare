using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models.DashBoard
{
    public class StatisticsModel
    {
        public int total { get; set; }
        public int active { get; set; }
        public int inactive { get; set; }
        public int lessons { get; set; }
        public int retention { get; set; }
        public int subscription { get; set; }
        public string username { get; set; }
        public DateTime lastlogindate { get; set; }

        public string lastloginIP { get; set; }
    }
}
