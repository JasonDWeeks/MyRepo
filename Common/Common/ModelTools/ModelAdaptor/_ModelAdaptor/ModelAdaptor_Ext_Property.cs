using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Modifier;
using System;
using System.Linq.Expressions;

namespace Common.ModelTools.ModelAdaptor
{
    public static partial class ModelAdaptor_Ext_Property
    {
        #region Property
        public static PropertyModifier<object, object> Property(this IModelAdaptor source, string propertyName)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = modelAdaptor.ModelProperties.GetByName(propertyName) ??
                throw new InvalidOperationException(string.Format("Could not find Property '{0}' in type '{1}'", propertyName, modelAdaptor.Model.GetType().Name));

            return new PropertyModifier<object, object>(modelAdaptor, propertyInfo);
        }
        #endregion

        #region Property<T, TProperty>
        public static PropertyModifier<TProperty, T> Property<T, TProperty>(this ModelAdaptor<T> source, Expression<Func<T, TProperty>> propertyExpression)
            where T : class, new()
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var propertyName = propertyExpression.GetPropertyNameFromExpression() ??
                throw new ArgumentException("Could not obtain property name from Lambda expression", nameof(propertyExpression));

            var propertyInfo = modelAdaptor.ModelProperties.GetByName(propertyName) ??
                throw new InvalidOperationException(string.Format("Could not find Property '{0}' in type '{1}'", propertyName, modelAdaptor.Model.GetType().Name));

            return new PropertyModifier<TProperty, T>(modelAdaptor, propertyInfo);
        }
        #endregion
    }
}
