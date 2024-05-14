using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace ProjectBase.KendoUiSupport
{
    public class FilterUtility<T>
        where T : class, new()
    {
        public FilterUtility() { }

        public FilterUtility(IEnumerable<T> entity, NeedDataSourceEventArgs needDataSourceEventArgs)
        {
            Entity = entity;
            NeedDataSourceEventArgs = needDataSourceEventArgs;
            finalResult = new { entity }.entity;
        }

        private IEnumerable<T> Entity { get; set; }

        public NeedDataSourceEventArgs NeedDataSourceEventArgs { get; private set; }

        public IEnumerable<T> Filter(out int total)
        {
            if (Entity == null)
            {
                total = 0;
                return null;
            }
            if (NeedDataSourceEventArgs == null)
            {
                total = finalResult.Count();
                return finalResult;
            }
            var filterArgs = NeedDataSourceEventArgs.filter;
            var filters =
                filterArgs != null && filterArgs.filters != null
                    ? filterArgs.filters
                    : new List<descriptor>();

            foreach (var desc in filters)
            {
                #region get dynamic
                Func<T, bool> filter = e =>
                {
                    var type = typeof(T);
                    var property = type.GetProperty(desc.field);
                    var propType = property.PropertyType;

                    if (
                        propType.IsGenericType
                        && propType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    )
                        //Get the underlying type property instead of the nullable generic
                        propType = new NullableConverter(propType).UnderlyingType;

                    var val1 = Convert.ChangeType(property.GetValue(e), propType);
                    var val2 = Convert.ChangeType(desc.value, propType);

                    Double x1,
                        x2;
                    switch (desc.@operator)
                    {
                        case Operator.neq:
                            return !val2.Equals(val1);
                        case Operator.gt:
                            x1 = Convert.ToDouble(val1);
                            x2 = Convert.ToDouble(val2);
                            return x1 > x2;
                        case Operator.gte:
                            x1 = Convert.ToDouble(val1);
                            x2 = Convert.ToDouble(val2);
                            return x1 >= x2;
                        case Operator.lt:
                            x1 = Convert.ToDouble(val1);
                            x2 = Convert.ToDouble(val2);
                            return x1 < x2;
                        case Operator.lte:
                            x1 = Convert.ToDouble(val1);
                            x2 = Convert.ToDouble(val2);
                            return x1 <= x2;
                        case Operator.startswith:
                        {
                            var value1ToUse = val1 == null ? string.Empty : val1.ToString();
                            return value1ToUse.StartsWith(
                                val2.ToString(),
                                StringComparison.OrdinalIgnoreCase
                            );
                        }
                        case Operator.contains:
                        {
                            var value1ToUse = val1 == null ? string.Empty : val1.ToString();
                            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                                value1ToUse,
                                val2.ToString(),
                                CompareOptions.IgnoreCase
                            );
                            return index >= 0;
                        }
                        case Operator.endswith:
                        {
                            var value1ToUse = val1 == null ? string.Empty : val1.ToString();
                            return value1ToUse.EndsWith(
                                val2.ToString(),
                                StringComparison.OrdinalIgnoreCase
                            );
                        }

                        default:
                            return val2.Equals(val1);
                    }
                };
                #endregion
                var recordSet = finalResult.Where(filter);
                finalResult = recordSet as IList<T> ?? recordSet.ToList();
            }
            total = finalResult.Count();

            try
            {
                finalResult = Sort(finalResult);
                finalResult = finalResult
                    .Skip(NeedDataSourceEventArgs.skip)
                    .Take(NeedDataSourceEventArgs.take);
            }
            catch (Exception) { }
            return finalResult.AsEnumerable();
        }

        private object SortRecucursive(string descValue, object objectEntity)
        {
            #region login flow
            //recurse to the end and load the data
            //current call 1
            //sub_account.account1.account_name
            //postProperty==account1.account_name
            //currentProperty=sub_account
            //p_type=type of(user)
            //entity2=typeof(sub_account)
            //current call 2
            //account1.account_name
            //postProperty==account1
            //currentProperty=account_name
            //p_type=type of(sub_accounts)
            //entity2=typeof(account_name)
            //set the value for account_name on p=object account1
            //current call 2
            //account_name
            //postProperty==account_name
            //currentProperty=account_name
            //p_type=type of(account_name)
            //not avvailable(entity2=typeof(account_name))
            //return value for account_name

            #endregion
            var parentType = objectEntity.GetType();
            string postProperty = descValue.Substring(descValue.IndexOf('.') + 1);
            string currentProperty = descValue.Substring(
                0,
                descValue.IndexOf('.') == -1 ? descValue.Length : descValue.IndexOf('.')
            );
            string propertToAssign = postProperty.Equals(currentProperty)
                ? postProperty
                : postProperty.Substring(
                    0,
                    postProperty.IndexOf('.') == -1
                        ? postProperty.Length
                        : postProperty.IndexOf('.')
                );
            if (descValue.Contains('.'))
            {
                object entity2 = parentType
                    .GetProperty(currentProperty)
                    .GetValue(objectEntity, null);
                var currentType = entity2.GetType();
                object returnVal = SortRecucursive(postProperty, entity2);
                //currentType.GetProperty(propertToAssign).SetValue(entity2, returnVal);
                return returnVal;
            }
            else
            {
                return parentType.GetProperty(postProperty).GetValue(objectEntity, null);
            }
        }

        public IEnumerable<T> Sort(IEnumerable<T> enumerable)
        {
            var type = typeof(T);
            var sortArgs = NeedDataSourceEventArgs.sort;
            if (sortArgs == null || sortArgs.Count == 0)
                return enumerable;

            var sorters = sortArgs != null ? sortArgs : new List<sort>();
            IEnumerable<T> sortedEnumerable = null;
            foreach (var sortiem in sorters)
            {
                List<T> results = enumerable.ToList();
                T @object = results.FirstOrDefault();

                var propType = type.GetProperty("sub_accounts");
                var propvalues = propType.GetValue(@object, null);
                if (sortiem.dir == SortOrder.desc)
                    sortedEnumerable =
                        from entity in enumerable
                        orderby SortRecucursive(sortiem.field, entity) descending
                        select entity;
                else
                    sortedEnumerable =
                        from entity in enumerable
                        orderby SortRecucursive(sortiem.field, entity) ascending
                        select entity;
            }
            return sortedEnumerable == null && sortedEnumerable.Count() <= 0
                ? enumerable
                : sortedEnumerable;
        }

        public IEnumerable<T> Sort()
        {
            if (Entity == null)
                return null;

            var type = typeof(T);
            var sortArgs = NeedDataSourceEventArgs.sort;
            if (sortArgs == null || sortArgs.Count == 0)
                return Entity;

            var entities = Entity;
            IEnumerable<T> sortedEnumerable;

            if (sortArgs[0].dir == SortOrder.desc)
                sortedEnumerable =
                    from entity in entities
                    orderby type.GetProperty(sortArgs[0].field).GetValue(entity, null) descending
                    select entity;
            else
                sortedEnumerable =
                    from entity in entities
                    orderby type.GetProperty(sortArgs[0].field).GetValue(entity, null) ascending
                    select entity;
            return sortedEnumerable
                .Skip(NeedDataSourceEventArgs.skip)
                .Take(NeedDataSourceEventArgs.take);
        }

        private IEnumerable<T> finalResult { get; set; }
    }
}
