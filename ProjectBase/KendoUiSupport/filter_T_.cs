using System.Collections.Generic;

namespace ProjectBase.KendoUiSupport
{
    public class filter<T>
    {
        public List<descriptor<T>> filters { get; set; }

        public string logic { get; set; }

        public filter() { }
    }
}
