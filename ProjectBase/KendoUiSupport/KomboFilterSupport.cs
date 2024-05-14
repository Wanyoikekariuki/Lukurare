using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBase.KendoUiSupport
{
    public static class KomboFilterSupport
    {
        public static ItemsRequestedResult<NeedDataSourceEventArgs> ProcessAlreadyFiltered<
            T,
            TOrderBy
        >(NeedDataSourceEventArgs args, IEnumerable<T> results, Func<T, TOrderBy> orderbyDefault)
        {
            int num = results.Count<T>();
            ItemsRequestedResult<NeedDataSourceEventArgs> itemsRequestedResult =
                new ItemsRequestedResult<NeedDataSourceEventArgs>() { request = args };
            if (results == null || results.Count<T>() <= 0)
            {
                itemsRequestedResult.isSuccessull = false;
                itemsRequestedResult.Result = new List<T>();
                itemsRequestedResult.DataSetCount = 0;
            }
            else
            {
                itemsRequestedResult.isSuccessull = true;
                itemsRequestedResult.DataSetCount = num;
                results = results.Skip<T>(args.skip).Take<T>(args.take);
                itemsRequestedResult.Result = results.ToList<T>();
            }
            return itemsRequestedResult;
        }
    }
}
