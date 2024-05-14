using System.Collections.Generic;

namespace EFDatabaseModel.SmartMeterModels
{
    public class ReadMeterBatchResponse
    {
        public int code { get; set; }
        public string info { get; set; }
        public QueryData data { get; set; }
    }

    public class QueryData
    {
        public List<QueryResponseData> data { get; set; }
    }

    public class QueryResponseData
    {
        public string collectoraddr { get; set; }
        public string meteraddr { get; set; }
        public double meternumber { get; set; }
        public string freezedate { get; set; }
        public string readdate { get; set; }

        public string valvestatus { get; set; }

        public double voltage { get; set; }

        public string csq { get; set; }
        public string imei { get; set; }
        public string batteryvoltage { get; set; }
        public string attackstate { get; set; }
    }
}
