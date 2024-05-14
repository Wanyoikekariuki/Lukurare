using System;
using System.Collections.Generic;
using System.Text;

namespace LukurareBackend.Models.DashBoard
{
    public class DashBoardModel
    {
        public int totalRetainerFeeCollection { get; set; }
        public int totalNumberofAgents { get; set; }
        public int totalNumberofCustomers { get; set; }
        public int totalApplicationFeeCollection { get; set; }
        public int totalOtherPaymentCollection { get; set; }

        public DateTime? x { get; set; }
        public float y { get; set; }
        public virtual StatisticsModel Statistics { get; set; }
    }
}
