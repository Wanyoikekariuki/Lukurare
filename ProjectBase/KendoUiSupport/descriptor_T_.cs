namespace ProjectBase.KendoUiSupport
{
    public class descriptor<T>
    {
        public string field { get; set; }

        public Operator @operator { get; set; }

        public T @value { get; set; }

        public descriptor() { }
    }
}
