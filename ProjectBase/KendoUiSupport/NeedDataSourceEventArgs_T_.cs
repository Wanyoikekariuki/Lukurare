using System.Collections.Generic;

namespace ProjectBase.KendoUiSupport
{
    public class NeedDataSourceEventArgs<T>
    {
        public filter<T> filter { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public int skip { get; set; }

        public List<ProjectBase.KendoUiSupport.sort> sort { get; set; }

        public int take { get; set; }

        public NeedDataSourceEventArgs() { }
    }
}
