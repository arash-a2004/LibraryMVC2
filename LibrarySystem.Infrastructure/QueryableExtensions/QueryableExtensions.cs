using System;
using System.Linq;
using System.Linq.Expressions;

namespace LibrarySystem.Infrastructure.QueryableExtensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
