using System.Linq.Expressions;
using TorinoRestaurant.Application.Common.Models;

namespace TorinoRestaurant.Application.Common.Extensions;

public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ParamsSearchWithMultiSort sortModels)
        {
            try
            {
                var expression = source.Expression;
                int count = 0;
                foreach (var item in sortModels.OrderBy)
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    Expression selector = parameter;
                    foreach (var member in item.Split('.'))
                    {
                        selector = Expression.PropertyOrField(selector, member);
                    }
                    var method = string.Equals(sortModels.Order[count], "DESC", StringComparison.OrdinalIgnoreCase) ?
                        (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                        (count == 0 ? "OrderBy" : "ThenBy");
                    expression = Expression.Call(typeof(Queryable), method,
                        new Type[] { source.ElementType, selector.Type },
                        expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                    count++;
                }
                return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
            }
            catch
            {
                return source;
            }

        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ParamsSearch sortModels)
        {
            try
            {
                var expression = source.Expression;
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression selector = parameter;
                foreach (var member in sortModels.OrderBy.Split('.'))
                {
                    selector = Expression.PropertyOrField(selector, member);
                }
                var method = string.Equals(sortModels.Order, "DESC", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                return source.Provider.CreateQuery<T>(expression);
            }
            catch
            {
                return source;
            }

        }
    }