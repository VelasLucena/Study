using Microsoft.EntityFrameworkCore;
using SudyApi.Properties.Enuns;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SudyApi.Data.Helper
{
    public static class QueryHelper
    {
        internal static readonly MethodInfo AsNoTrackingMethodInfo = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethod("AsNoTracking")!;

        public static IQueryable<TEntity> ApplyOrderBy<TEntity>(this IQueryable<TEntity> query, string? keySelector, Ordering ordering = Ordering.Desc)
        {
            if (keySelector == null)
                keySelector = AttributeIsPrimaryKey(typeof(TEntity));

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

        private static string AttributeIsPrimaryKey(Type obj)
        {
            PropertyInfo? item = obj.GetProperties().FirstOrDefault();

            if (item == null)
                return string.Empty;

            KeyAttribute? attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

            if (attribute == null)
            {
                foreach (PropertyInfo property in obj.GetProperties())
                {
                    attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                    if (attribute != null)
                        return property.Name;
                }
            }

            return item.Name;
        }
    }
}
