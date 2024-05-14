using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBase.KendoUiSupport
{
    public static class KendoSupportExtentions
    {
        public static string GetFilterValue(this NeedDataSourceEventArgs args, int pos)
        {
            object obj;
            if (
                args == null
                || args.filter == null
                || args.filter.filters == null
                || !args.filter.filters.Any<descriptor>()
            )
            {
                obj = "";
            }
            else
            {
                obj = args.filter.filters.ElementAt<descriptor>(pos).@value;
            }
            object obj1 = obj;
            if (obj1 == null)
            {
                return string.Empty;
            }
            return obj1.ToString();
        }

        public static string GetFilterValue<T>(this NeedDataSourceEventArgs<T> args, int pos)
        {
            string str;
            if (
                args == null
                || args.filter == null
                || args.filter.filters == null
                || !args.filter.filters.Any<descriptor<T>>()
            )
            {
                str = "";
            }
            else
            {
                T t = args.filter.filters.ElementAt<descriptor<T>>(pos).@value;
                str = t.ToString();
            }
            return str;
        }

        public static string GetFilterValuePredicate(
            this NeedDataSourceEventArgs args,
            Func<descriptor, bool> predicate,
            int pos
        )
        {
            if (
                args != null
                && args.filter != null
                && args.filter.filters != null
                && args.filter.filters.Any<descriptor>()
            )
            {
                IEnumerable<descriptor> descriptors = args.filter.filters.Where<descriptor>(
                    predicate
                );
                if (descriptors.Any<descriptor>())
                {
                    return descriptors.ElementAt<descriptor>(pos).@value.ToString();
                }
            }
            return null;
        }

        public static bool HasFilterFields(this NeedDataSourceEventArgs args)
        {
            return !string.IsNullOrEmpty(args.GetFilterValue(0));
        }

        public static bool HasFilterFields(this NeedDataSourceEventArgs args, string fieldName)
        {
            if (string.IsNullOrEmpty(args.GetFilterValue(0)))
            {
                return false;
            }
            return args.filter.filters.FirstOrDefault<descriptor>(
                    (descriptor r) => r.field == fieldName
                ) != null;
        }
    }
}
