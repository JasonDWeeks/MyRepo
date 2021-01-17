using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor
{
    public static partial class ModelAdaptor_Ext_Children
    {
        #region ChildrenProperties
        public static IEnumerable<PropertyInfo> GetChildrenProperties(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            return modelAdaptor.ModelProperties.Where(x => x.PropertyType.GetInterface(nameof(IModelAdaptorChildren)) != null);
        }
        #endregion

        #region Children
        public static IModelAdaptorChildren Children(this IModelAdaptor source, string propertyName)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = modelAdaptor.GetChildrenProperties().GetByName(propertyName) ??
                throw new InvalidOperationException(string.Format("Could not find Property '{0}' in type '{1}'", propertyName, nameof(IModelAdaptorChildren)));

            return (IModelAdaptorChildren)propertyInfo.GetValue(modelAdaptor.Model);
        }
        #endregion

        #region Children<T, TProperty>
        public static IModelAdaptorChildren Children<T, TProperty>(this ModelAdaptor<T> source, Expression<Func<T, TProperty>> propertyExpression)
            where T : class, new()
            where TProperty : IModelAdaptorChildren
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var propertyName = propertyExpression.GetPropertyNameFromExpression() ??
                throw new ArgumentException("Could not obtain property name from Lambda expression", nameof(propertyExpression));

            var propertyInfo = modelAdaptor.GetChildrenProperties().GetByName(propertyName) ??
                throw new InvalidOperationException(string.Format("Could not find Property '{0}' in type '{1}'", propertyName, nameof(IModelAdaptorChildren)));

            return (IModelAdaptorChildren)propertyInfo.GetValue(modelAdaptor.Model);
        }
        #endregion
    }
}
