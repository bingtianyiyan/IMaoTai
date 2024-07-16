using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.Helpers
{
   public static class ExtensionHelper
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IEnumerable<T> PageSkipAndTake<T>(this IEnumerable<T> query, int skip,int takeSize)
        {
            return query.Skip((skip - 1) * takeSize).Take(takeSize);
        }

    }
}
