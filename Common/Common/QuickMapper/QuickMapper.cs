using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common.Extensions;

namespace Common.QuickMapper
{
    public static class QuickMapper
    {
        #region Public

        #region MapTo()
        public static TDestination MapTo<TDestination>(this object source)
            where TDestination : class, new()
        {
            return Map(source, new TDestination());
        }

        public static TDestination MapTo<TDestination>(this object source, params string[] includeProperties)
            where TDestination : class, new()
        {
            return Map(source, new TDestination(), ConvertStrings(includeProperties)) as TDestination;
        }

        public static TDestination MapTo<TDestination>(this object source, params Expression<Func<TDestination, object>>[] includeProperties)
            where TDestination : class, new()
        {
            return Map(source, new TDestination(), GetIncludeNames(includeProperties)) as TDestination;
        }
        #endregion

        #region MapTo(destination)
        public static TDestination MapTo<TDestination>(this object source, TDestination destination)
            where TDestination : class
        {
            return Map(source, destination);
        }

        public static TDestination MapTo<TDestination>(this object source, TDestination destination, params string[] includeProperties)
            where TDestination : class
        {
            return Map(source, destination, ConvertStrings(includeProperties)) as TDestination;
        }

        public static TDestination MapTo<TDestination>(this object source, TDestination destination, params Expression<Func<TDestination, object>>[] includeProperties)
            where TDestination : class
        {
            return Map(source, destination, GetIncludeNames(includeProperties)) as TDestination;
        }
        #endregion

        #region MapTo(source, destination)
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            return Map(source, destination, new List<IncludePropertiesInfo>().ToArray()) as TDestination;
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, params string[] includeProperties)
            where TSource : class
            where TDestination : class
        {
            return Map(source, destination, ConvertStrings(includeProperties)) as TDestination;
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, params Expression<Func<TDestination, object>>[] includeProperties)
            where TSource : class
            where TDestination : class
        {
            return Map(source, destination, GetIncludeNames(includeProperties)) as TDestination;
        }
        #endregion

        #region MapTo Modifiers
        public static object MapToInclude<TSource>(this TSource Source, params Expression<Func<TSource, object>>[] includeProperties)
        {
            return null;
        }

        public static object MapToExclude<TSource>(this TSource Source)
        {
            return null;
        }

        public static object MapToValue<TSource>(this TSource Source, TSource Value)
        {
            return null;
        }

        public static object MapToValue<TSource>(this TSource Source, Func<TSource, TSource> Value)
        {
            return null;
        }
        #endregion

        #region MapFrom DataTable
        public static TDestination MapFromDataTableTo<TDestination>(this DataTable Source)
            where TDestination : class, new()
        {
            return MapFromDataTable(Source, new TDestination()) as TDestination;
        }

        public static TDestination MapFromDataTableTo<TDestination>(this DataTable Source, TDestination destination)
            where TDestination : class
        {
            return MapFromDataTable(Source, destination) as TDestination;
        }

        public static TDestination MapFromDataRowTo<TDestination>(this DataRow Source)
            where TDestination : class, new()
        {
            return MapFromDataRow(Source, new TDestination()) as TDestination;
        }

        public static TDestination MapFromDataRowTo<TDestination>(this DataRow Source, TDestination destination)
            where TDestination : class
        {
            return MapFromDataRow(Source, destination) as TDestination;
        }
        #endregion

        #endregion

        #region Private

        #region Fields
        static private object[] _emptyParams = new object[] { };
        static private List<string> _validModifierMethods = new List<string>
        {
            nameof(MapToInclude),
            nameof(MapToExclude),
            nameof(MapToValue)
        };
        #endregion

        #region IncludePropertiesInfo
        private class IncludePropertiesInfo
        {
            public string PropertyName { get; set; }
            public bool Exclude { get; set; }
            public IncludePropertiesInfo[] ChildIncludeProperties { get; set; }
            public Expression Value { get; set; }
            public bool LambdaValue
            {
                get
                {
                    return Value != null && Value is LambdaExpression;
                }
            }
        }

        private static IncludePropertiesInfo[] ConvertStrings(string[] includeProperties)
        {
            var includes = new List<IncludePropertiesInfo>();
            foreach (var propertyName in includeProperties)
            {
                includes.Add(new IncludePropertiesInfo { PropertyName = propertyName });
            }
            return includes.ToArray();
        }

        private static IncludePropertiesInfo[] GetIncludeNames(object includeProperties)
        {
            var includes = new List<IncludePropertiesInfo>();

            var newArrayExprerssion = includeProperties as NewArrayExpression;
            if (newArrayExprerssion != null)
            {
                foreach (UnaryExpression expression in newArrayExprerssion.Expressions)
                {
                    includes.Add(CreateIncludePropertyInfo(expression.Operand as LambdaExpression));
                }
            }
            else if (includeProperties is IEnumerable)
            {
                foreach (LambdaExpression expression in (includeProperties as IEnumerable))
                {
                    includes.Add(CreateIncludePropertyInfo(expression));
                }
            }

            return includes.ToArray();
        }

        private static IncludePropertiesInfo CreateIncludePropertyInfo(LambdaExpression expression)
        {
            var includePropertiesInfo = new IncludePropertiesInfo { PropertyName = GetPropertyName(expression) };

            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodbody = expression.Body as MethodCallExpression;
                if (methodbody.Method.DeclaringType == typeof(QuickMapper) && _validModifierMethods.Contains(methodbody.Method.Name))
                {
                    includePropertiesInfo.PropertyName = GetPropertyName(methodbody.Arguments.FirstOrDefault());

                    if (methodbody.Method.Name.Equals(nameof(MapToInclude)))
                    {
                        includePropertiesInfo.ChildIncludeProperties = GetIncludeNames(methodbody.Arguments.LastOrDefault());
                        return includePropertiesInfo;
                    }

                    if (methodbody.Method.Name.Equals(nameof(MapToExclude)))
                    {
                        includePropertiesInfo.Exclude = true;
                        return includePropertiesInfo;
                    }

                    if (methodbody.Method.Name.Equals(nameof(MapToValue)))
                    {
                        includePropertiesInfo.Value = methodbody.Arguments.LastOrDefault();
                        return includePropertiesInfo;
                    }
                }
            }

            return includePropertiesInfo;
        }

        private static string GetPropertyName(Expression expression)
        {
            if (expression is LambdaExpression)
                return GetPropertyName((expression as LambdaExpression).Body);

            if (expression is MemberExpression)
                return (expression as MemberExpression).Member.Name;
            else
                return ((expression as UnaryExpression)?.Operand as MemberExpression)?.Member.Name;
        }
        #endregion

        #region Map
        private static object Map(object source, object destination, params IncludePropertiesInfo[] includeProperties)
        {
            if (source != null && destination != null)
            {
                if (source != null && destination != null && (destination.GetType().Name == "ICollection`1" || destination.GetType().GetInterface("ICollection`1") != null))
                {
                        var destinationGenericType = destination.GetType();
                    var destinationGenericArg = destinationGenericType.GetGenericArguments().FirstOrDefault();
                    return DoMapCollection(source, destination, destinationGenericArg, includeProperties);
                }
                return DoMap(source, destination, includeProperties);
            }
            else
            {
                return source != null ? destination : null;
            }
        }

        private static object DoMapCollection(object source, object destination, Type destinationGenericArg, params IncludePropertiesInfo[] includeProperties)
        {
            if (destinationGenericArg.GetConstructor(Type.EmptyTypes) != null)
            {
                var clearMethod = destination.GetType().GetMethods().FirstOrDefault(x => x.Name == "Clear" && !x.GetParameters().Any());
                var addMethod = destination.GetType().GetMethods().FirstOrDefault(x => x.Name == "Add" && x.GetParameters().Count() == 1);

                clearMethod?.Invoke(destination, _emptyParams);
                foreach (var src in source as IEnumerable)
                {
                    if (addMethod != null)
                    {
                        var dest = System.Activator.CreateInstance(destinationGenericArg, _emptyParams);
                        addMethod.Invoke(destination, new object[] { DoMap(src, dest, includeProperties) });
                    }
                }
            }
            return destination;
        }

        private static object DoMap(object source, object destination, params IncludePropertiesInfo[] includeProperties)
        {
            foreach (var destinationProperty in destination.GetType().GetProperties())
            {
                if (destinationProperty.SetMethod == null)
                    continue;

                var sourceProperty = source.GetType().GetProperty(destinationProperty.Name);
                var includeProp = includeProperties.Where(x => x.PropertyName == destinationProperty.Name).FirstOrDefault();

                if (ContinueAfterDoMapForPropertyWithinIncludeList(source, destination, sourceProperty, destinationProperty, includeProp))
                    continue;

                if (sourceProperty == null)
                    continue;

                if (sourceProperty.PropertyType == destinationProperty.PropertyType ||
                    Nullable.GetUnderlyingType(sourceProperty.PropertyType) == destinationProperty.PropertyType ||
                    sourceProperty.PropertyType == Nullable.GetUnderlyingType(destinationProperty.PropertyType))
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, sourceValue);
                    continue;
                }
            }
            return destination;
        }

        private static bool ContinueAfterDoMapForPropertyWithinIncludeList(object source, object destination, PropertyInfo sourceProperty, PropertyInfo destinationProperty, IncludePropertiesInfo includeProp)
        {
            if (includeProp != null)
            {
                if (includeProp.Exclude)
                    return true;

                if (includeProp.Value != null)
                {
                    if (includeProp.LambdaValue)
                    {
                        if (sourceProperty != null &&
                            (sourceProperty.PropertyType == destinationProperty.PropertyType ||
                             Nullable.GetUnderlyingType(sourceProperty.PropertyType) == destinationProperty.PropertyType ||
                             sourceProperty.PropertyType == Nullable.GetUnderlyingType(destinationProperty.PropertyType)))
                        {
                            var sourceValue = sourceProperty.GetValue(source);
                            destinationProperty.SetValue(destination, includeProp.Value.GetValue(sourceValue));
                        }
                        else
                            destinationProperty.SetValue(destination, includeProp.Value.GetValue(null));
                        return true;
                    }
                    else
                    {
                        destinationProperty.SetValue(destination, includeProp.Value.GetValue());
                        return true;
                    }
                }

                if (destinationProperty.PropertyType.GetConstructor(Type.EmptyTypes) != null)
                {
                    if (sourceProperty == null)
                        return true;

                    var sourceValue = sourceProperty.GetValue(source);
                    var destinationMapValue = System.Activator.CreateInstance(destinationProperty.PropertyType, _emptyParams);
                    var childIncludeProperties = includeProp.ChildIncludeProperties;
                    if (childIncludeProperties != null)
                        destinationProperty.SetValue(destination, Map(sourceValue, destinationMapValue, childIncludeProperties));
                    else
                        destinationProperty.SetValue(destination, Map(sourceValue, destinationMapValue));
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Map DataTable
        private static object MapFromDataTable(DataTable source, object destination)
        {
            if (source != null && destination != null && destination as ICollection != null && destination.GetType().IsGenericType)
            {
                var destinationGenericType = destination.GetType();
                var destinationGenericArg = destinationGenericType.GetGenericArguments()[0];
                return DoMapDataTable(source, destination, destinationGenericArg);
            }
            else
            {
                return source != null ? destination : null;
            }
        }

        private static object MapFromDataRow(DataRow source, object destination)
        {
            if (source != null && destination != null)
                return DoMapDataRow(source, destination);
            else
                return source != null ? destination : null;
        }

        private static object DoMapDataTable(DataTable source, object destination, Type destinationGenericArg)
        {
            if (destinationGenericArg.GetConstructor(Type.EmptyTypes) != null)
            {
                var clearMethod = destination.GetType().GetMethods().FirstOrDefault(x => x.Name == "Clear" && !x.GetParameters().Any());
                var addMethod = destination.GetType().GetMethods().FirstOrDefault(x => x.Name == "Add" && x.GetParameters().Count() == 1);

                clearMethod?.Invoke(destination, _emptyParams);

                for (int i = 0; i < source.Rows.Count; i++)
                {
                    if (addMethod != null)
                    {
                        var src = source.Rows[i];
                        var dest = System.Activator.CreateInstance(destinationGenericArg, _emptyParams);
                        addMethod.Invoke(destination, new object[] { DoMapDataRow(src, dest) });
                    }
                }
            }
            return destination;
        }

        private static object DoMapDataRow(DataRow source, object destination)
        {
            foreach (var destinationProperty in destination.GetType().GetProperties())
            {
                if (destinationProperty.SetMethod == null)
                    continue;

                var sourceValue = source[destinationProperty.Name];

                if (sourceValue == null || sourceValue is System.DBNull)
                    continue;

                var sourcePropertyType = sourceValue.GetType();

                if (sourceValue.GetType() == destinationProperty.PropertyType ||
                    sourceValue.GetType() == Nullable.GetUnderlyingType(destinationProperty.PropertyType))
                    destinationProperty.SetValue(destination, sourceValue);
            }
            return destination;
        }
        #endregion

        #endregion
    }
}
