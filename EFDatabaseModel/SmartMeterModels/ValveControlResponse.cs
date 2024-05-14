namespace EFDatabaseModel.SmartMeterModels
{
    public class ValveControlResponse
    {
        public int code { get; set; }
        public string info { get; set; }

        public ValveData data { get; set; }
    }

    public class ValveData
    {
        public string result { get; set; }
        public string message { get; set; }
    }
}
