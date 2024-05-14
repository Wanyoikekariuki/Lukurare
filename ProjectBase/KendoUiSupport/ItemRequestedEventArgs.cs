namespace ProjectBase.KendoUiSupport
{
    public class ItemRequestedEventArgs
    {
        public string CurrentPage { get; set; }

        public string PageSize { get; set; }

        public object SearchText { get; set; }

        public ItemRequestedEventArgs() { }
    }
}
