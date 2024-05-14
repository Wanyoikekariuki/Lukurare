namespace EFDatabaseModel.SmartMeterModels
{
    public class LoginResponse
    {
        public int code { get; set; }
        public string info { get; set; }

        public Data data { get; set; }
    }

    public class Data
    {
        public string token { get; set; }
    }
}
