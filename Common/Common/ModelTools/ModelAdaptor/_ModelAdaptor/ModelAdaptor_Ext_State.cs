using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Hierarchy;
using Common.ModelTools.ModelAdaptor.Modifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor
{
    public static partial class ModelAdaptor_Ext_State
    {
        #region EditableProperties
        public static IEnumerable<PropertyInfo> GetEditableProperties(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            return modelAdaptor.ModelProperties
                .Where(
                    x =>
                        x.SetMethod != null ||
                        x.PropertyType.GetInterface(nameof(IModelAdaptorChildren)) != null
                );
        }
        #endregion

        #region IsEdited_Exclude
        public static bool IsEdited_Exclude(this IModelAdaptor source, params string[] excludeProperties)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            var list = excludeProperties.ToList();
            return modelAdaptor.GetEditableProperties()
                .Where(
                    x => 
                        !list.Contains(x.Name) && 
                        modelAdaptor.ActiveModelProperties.Contains(x.Name)
                )
                .Any(x => modelAdaptor.Property(x.Name).IsEdited);
        }
        #endregion

        #region IsEdited_Exclude<T>
        public static bool IsEdited_Exclude<T>(this ModelAdaptor<T> source, params Expression<Func<T, object>>[] excludeProperties)
            where T : class, new()
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            var list = excludeProperties.Select(x => x.GetPropertyNameFromExpression()).Where(x => x != null).ToList();
            return modelAdaptor.GetEditableProperties()
                .Where(
                    x => 
                        !list.Contains(x.Name) &&
                        modelAdaptor.ActiveModelProperties.Contains(x.Name)
                )
                .Any(x => modelAdaptor.Property(x.Name).IsEdited);
        }
        #endregion

        #region IsEdited_Exclude_Children
        public static bool IsEdited_Exclude_Children(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            var children = modelAdaptor.GetChildrenProperties().Select(x => x.Name).ToList();
            return modelAdaptor.GetEditableProperties()
                .Where(
                    x =>
                        !children.Contains(x.Name) &&
                        modelAdaptor.ActiveModelProperties.Contains(x.Name)
                )
                .Any(x => modelAdaptor.Property(x.Name).IsEdited);
        }
        #endregion

        #region PropertyValues
        public static IEnumerable<IPropertyChangeInfo> GetPropertyValues(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            var activeProperties = modelAdaptor.GetEditableProperties()
                .Where(
                    x => 
                        modelAdaptor.ActiveModelProperties.Contains(x.Name) && 
                        x.PropertyType.GetInterface(nameof(IModelAdaptorChildren)) == null
                );
            return activeProperties
                .Select(
                    x => 
                        new PropertyChangeEventArgs(
                            modelAdaptor.ModelProperties.GetByName(x.Name).GetValue(modelAdaptor.Original.Item),
                            x.GetValue(modelAdaptor.Model), x.Name
                        )
                )
                .ToArray();
        }
        #endregion

        #region PropertyChanges
        public static IEnumerable<IPropertyChangeInfo> GetPropertyChanges(this IModelAdaptor source)
        {
            var modelAdaptor = source ?? throw new ArgumentNullException(nameof(source));

            return modelAdaptor.GetPropertyValues().Where(x => modelAdaptor.Property(x.PropertyName).IsEdited);
        }
        #endregion

        #region ForceNotify_IsEdited_IsValid
        public static void ForceNotify_IsEdited_IsValid(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.IsEdited));
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.IsValid));
        }
        #endregion

        #region ForceNotify_IsEdited
        public static void ForceNotify_IsEdited(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.IsEdited));
        }
        #endregion

        #region ForceNotify_IsValid
        public static void ForceNotify_IsValid(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.IsValid));
        }
        #endregion
    }
}
