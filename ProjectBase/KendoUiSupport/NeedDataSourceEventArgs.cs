using System.Collections.Generic;

namespace ProjectBase.KendoUiSupport
{
    public class NeedDataSourceEventArgs
    {
        public static NeedDataSourceEventArgs EnsureArgsValid(NeedDataSourceEventArgs args)
        {
            if (args == null)
                args = new NeedDataSourceEventArgs();

            var skip = 0;
            var take = 100;

            skip = args.skip;
            take = args.take == 0 ? take : args.take;
            args.take = take;

            return args;
        }

        public ProjectBase.KendoUiSupport.filter filter { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public int skip { get; set; }

        public List<ProjectBase.KendoUiSupport.sort> sort { get; set; }

        public int take { get; set; }

        public NeedDataSourceEventArgs() { }

        public NeedDataSourceEventArgs(List<descriptor> descriptors)
            : this()
        {
            filter = new filter { filters = descriptors };
            if (take <= 0)
                take = 100;
            if (page == 0)
                page = 1; //incase take is not defined then set it to default of 100 for API that use this to filter
        }

        public NeedDataSourceEventArgs(descriptor descriptor)
            : this(new List<descriptor> { descriptor }) { }

        public NeedDataSourceEventArgs(string value, string fieldValue, Operator operatorvalue)
            : this(
                new descriptor
                {
                    field = fieldValue,
                    @operator = operatorvalue,
                    value = value
                }
            ) { }

        public NeedDataSourceEventArgs(string value)
            : this(new descriptor { value = value }) { }
    }
}
