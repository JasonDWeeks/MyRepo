using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.ModelTools.Common.Internal
{
    static class ExpressionExtensions
    {
        public static object GetValue(
            this Expression 
                expression, 
            params object[] 
                lamdaParameterObjects)
        {
            var lExp = expression as LambdaExpression;
            if (lExp != null)
                return lExp.Compile().DynamicInvoke(lamdaParameterObjects);
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        public static string GetPropertyNameFromExpression<T1, T2>(
            this Expression<Func<T1, T2>> propertyExpression)
        {
            string propertyName = null;
            if (propertyExpression == null)
                return propertyName;
            if (propertyExpression.Body is MemberExpression)
                propertyName = (propertyExpression.Body as MemberExpression)?.Member?.Name;
            else
                propertyName = ((propertyExpression.Body as UnaryExpression)?.Operand as MemberExpression)?.Member?.Name;

            return propertyName;
        }

        public static List<string> GetPropertyNamesFromExpressions<T1, T2>(this IEnumerable<Expression<Func<T1, T2>>> expressions)
        {
            return 
                expressions
                    .Select(x => x.GetPropertyNameFromExpression())
                    .Where(x => x != null)
                    .ToList();
        }

        public static List<Expression<Func<T1, T2>>> GetExcludeProperties<T1, T2>(this IEnumerable<string> properties, Type type)
        {
            var ret = new List<Expression<Func<T1, T2>>>();

            var list = properties
                .Select(x => type.GetProperty(x))
                .Where(x => x != null)
                .ToList();

            list.ForEach(property =>
            {
                var parameter = Expression.Parameter(type, "x");
                var member = Expression.MakeMemberAccess(parameter, property);

                var method = 
                    typeof(QuickMapper.QuickMapper)
                        .GetMethod(
                            nameof(QuickMapper.QuickMapper.MapToExclude), 
                            BindingFlags.Public | BindingFlags.Static
                        );

                method = method.MakeGenericMethod(property.PropertyType);

                var methodCall = Expression.Call(null, method, member);

                ret.Add(Expression.Lambda<Func<T1, T2>>(methodCall, parameter));
            });

            return ret;
        }
    }
}
