namespace ProjectBase.KendoUiSupport
{
    public class ItemsRequestedResult<T>
    {
        public int DataSetCount { get; set; }

        public bool isSuccessull { get; set; }

        public T request { get; set; }

        public object Result { get; set; }

        public ItemsRequestedResult() { }
    }
}
