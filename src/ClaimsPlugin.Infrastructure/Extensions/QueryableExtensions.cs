using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Infrastructure.Extensions
{
    internal static class QueryableExtensions
    {
          public static IQueryable<TDestination> AppendQuery<TSource, TDestination>(
            this IQueryable<TSource> source,
            Func<IQueryable<TSource>, IQueryable<TDestination>> builder)
        {
            return builder(source);
        }
        
    }
}