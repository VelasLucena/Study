using Elastic.CommonSchema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Nest;
using StackExchange.Redis;
using SudyApi.Properties.Enuns;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SudyApi.Utility
{
    public static class QueryHelper
    {
        internal static readonly MethodInfo AsNoTrackingMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethod("AsNoTracking")!;

        public static IQueryable<TEntity> ApplyOrderBy<TEntity>(this IQueryable<TEntity> query, string keySelector, Ordering ordering = Ordering.Desc)
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    query = query.OrderBy(p => EF.Property<TEntity>(p, keySelector));
                    break;
                case Ordering.Desc:
                    query = query.OrderByDescending(p => EF.Property<TEntity>(p, keySelector));
                    break;
            }

            return query;
        }

        public static IQueryable<TEntity> ApplyTracking<TEntity>(this IQueryable<TEntity> query, bool isTracking = true) where TEntity : class
        {
            if (!isTracking)
                query = query.AsNoTracking();
            
            return query;
        }
    }
}
