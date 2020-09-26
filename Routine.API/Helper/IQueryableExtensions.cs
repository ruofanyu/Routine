using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Routine.API.Services;

namespace Routine.API.Helper
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentException(nameof(mappingDictionary));
            }
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(",");

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith("desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"没有找到key为{propertyName}的映射");
                }

                var propertyMappingVlaue = mappingDictionary[propertyName];
                if (propertyMappingVlaue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingVlaue));
                }
                foreach (var destiantionProperty in propertyMappingVlaue.DestinationProperties.Reverse())
                {
                    if (propertyMappingVlaue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    source = source.OrderBy(destiantionProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}
