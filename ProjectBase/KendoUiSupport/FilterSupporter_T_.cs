using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBase.KendoUiSupport
{
    public class FilterSupporter<T>
        where T : class, new()
    {
        private FilterUtility<T> util;

        public ItemsRequestedResult<NeedDataSourceEventArgs> itemsResult { get; private set; }

        public FilterSupporter() { }

        public FilterSupporter(IEnumerable<T> data, NeedDataSourceEventArgs args)
        {
            util = new FilterUtility<T>(data, args);
        }

        public IEnumerable<T> FilterData<TOrderBy>(Func<T, TOrderBy> orderbyDefault)
        {
            int num = 0;
            IEnumerable<T> ts = util.Filter(out num).OrderBy<T, TOrderBy>(orderbyDefault);
            ItemsRequestedResult<NeedDataSourceEventArgs> itemsRequestedResult =
                new ItemsRequestedResult<NeedDataSourceEventArgs>()
                {
                    request = util.NeedDataSourceEventArgs
                };
            itemsResult = itemsRequestedResult;
            if (ts == null || ts.Count<T>() <= 0)
            {
                itemsResult.isSuccessull = false;
                itemsResult.Result = new List<T>();
                itemsResult.DataSetCount = 0;
            }
            else
            {
                itemsResult.isSuccessull = true;
                itemsResult.Result = ts.ToList<T>();
                itemsResult.DataSetCount = num;
            }
            return ts;
        }

        public IEnumerable<T> FilterDataOrdeyByDesceding<TOrderBy>(Func<T, TOrderBy> orderbyDefault)
        {
            int num = 0;
            IEnumerable<T> ts = util.Filter(out num).OrderByDescending<T, TOrderBy>(orderbyDefault);
            ItemsRequestedResult<NeedDataSourceEventArgs> itemsRequestedResult =
                new ItemsRequestedResult<NeedDataSourceEventArgs>()
                {
                    request = util.NeedDataSourceEventArgs
                };
            itemsResult = itemsRequestedResult;
            if (ts == null || ts.Count<T>() <= 0)
            {
                itemsResult.isSuccessull = false;
                itemsResult.Result = null;
                itemsResult.DataSetCount = 0;
            }
            else
            {
                itemsResult.isSuccessull = true;
                itemsResult.Result = ts.ToList<T>();
                itemsResult.DataSetCount = num;
            }
            return ts;
        }
    }
}
