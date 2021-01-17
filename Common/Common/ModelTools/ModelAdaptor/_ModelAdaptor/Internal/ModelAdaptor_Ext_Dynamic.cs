using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Dynamic
    {
        #region GetMember
        public static bool GetMember(this IModelAdaptor modelAdaptor, string propertyName, out object value)
        {
            value = null;

            if (string.IsNullOrWhiteSpace(propertyName))
                return false;

            if (modelAdaptor.GetBaseProperty(propertyName, out value))
                return true;

            if (modelAdaptor.GetExtensionProperty(propertyName, out value))
                return true;

            var propertyInfo = modelAdaptor.GetPropertyInfo(propertyName);
            if (propertyInfo == null)
                return false;

            if (propertyName.EndsWith(nameof(ModifierProperty._IsEdited)))
                value = (bool)modelAdaptor.Property(propertyInfo.Name).IsEdited;
            else if (propertyName.EndsWith(nameof(ModifierProperty._IsValid)))
                value = (bool)modelAdaptor.Property(propertyInfo.Name).IsValid;
            else if (propertyName.EndsWith(nameof(ModifierProperty._Value)))
                value = modelAdaptor.Property(propertyInfo.Name).Value;
            else if (propertyName.EndsWith(nameof(ModifierProperty._Text)))
                value = (string)modelAdaptor.Property(propertyInfo.Name).Text;
            else if (propertyName.EndsWith(nameof(ModifierProperty._Label)))
                value = (string)modelAdaptor.Property(propertyInfo.Name).Label;
            else if (propertyName.EndsWith(nameof(ModifierProperty._Message)))
                value = (string)modelAdaptor.Property(propertyInfo.Name).Message;
            else if (propertyName.EndsWith(nameof(ModifierProperty._ReadOnly)))
                value = (bool)modelAdaptor.Property(propertyInfo.Name).ReadOnly;
            else
                value = propertyInfo.GetValue(modelAdaptor.Model);

            if (!modelAdaptor.ActiveModelProperties.Any(x => x == propertyInfo.Name))
                modelAdaptor.ActiveModelProperties.Add(propertyInfo.Name);

            return true;
        }
        #endregion

        #region SetMember
        public static bool SetMember(this IModelAdaptor modelAdaptor, string propertyName, object value)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return false;

            modelAdaptor.TrySetMemberStart();

            var IsBaseProperty = modelAdaptor.SetBaseProperty(propertyName, value);
            if (IsBaseProperty)
            {
                modelAdaptor.TrySetMemberExist();
                return true;
            }

            var propertyInfo = modelAdaptor.GetPropertyInfo(propertyName);
            if (propertyInfo == null)
            {
                modelAdaptor.TrySetMemberExist();
                return false;
            }

            if (!modelAdaptor.ActiveModelProperties.Any(x => x == propertyInfo.Name))
                modelAdaptor.ActiveModelProperties.Add(propertyInfo.Name);

            if (propertyName.EndsWith(nameof(ModifierProperty._Text)))
                modelAdaptor.Property(propertyInfo.Name).Text = (string)value;
            else if (propertyName.EndsWith(nameof(ModifierProperty._Label)))
                modelAdaptor.Property(propertyInfo.Name).Label = (string)value;
            else if (propertyName.EndsWith(nameof(ModifierProperty._Message)))
                modelAdaptor.Property(propertyInfo.Name).Message = (string)value;
            else if (propertyName.EndsWith(nameof(ModifierProperty._ReadOnly)))
                modelAdaptor.Property(propertyInfo.Name).ReadOnly = (bool)value;
            else
            {
                modelAdaptor.Property(propertyName).Value.Changing_Raise();
                if (propertyInfo.SetMethod != null)
                    propertyInfo.SetValue(modelAdaptor.Model, value);
                modelAdaptor.Property(propertyName).Value.Changed_Raise();
                modelAdaptor.OnPropertyChanged_DynamicProperty(propertyName);
            }

            modelAdaptor.TrySetMemberExist();
            return true;
        }
        #endregion

        #region Private
        private static void TrySetMemberStart(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.TrySetMember_COUNT.Increment();
        }

        private static void TrySetMemberExist(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.TrySetMember_COUNT.Decrement();
            if (modelAdaptor.TrySetMember_COUNT == 0)
                modelAdaptor.Cache.Clear();
        }

        private static PropertyInfo GetPropertyInfo(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.GetModifierProperties().ToList().ForEach(pn => propertyName = propertyName.Replace(pn, string.Empty));
            propertyName = propertyName.Replace(modelAdaptor.GetModelPropertyPrefix(), string.Empty);

            return modelAdaptor.ModelProperties.GetByName(propertyName);
        }

        private static bool GetBaseProperty(this IModelAdaptor modelAdaptor, string propertyName, out object value)
        {
            value = null;
            if (!modelAdaptor.IsBaseProperty(propertyName))
                return false;

            value = modelAdaptor.ModelAdaptorProperties.GetByName(propertyName).GetValue(modelAdaptor);
            return true;
        }

        private static bool SetBaseProperty(this IModelAdaptor modelAdaptor, string propertyName, object value)
        {
            if (!modelAdaptor.IsBaseProperty(propertyName))
                return false;

            modelAdaptor.ModelAdaptorProperties.GetByName(propertyName).SetValue(modelAdaptor, value);
            return true;
        }
        private static bool IsBaseProperty(this IModelAdaptor modelAdaptor, string propertyName)
        {
            return modelAdaptor.GetBaseProperties().Contains(propertyName);
        }

        private static IEnumerable<string> GetBaseProperties(this IModelAdaptor modelAdaptor)
        {
            return Enum.GetValues(typeof(ModelAdaptorBaseProperty)).Cast<ModelAdaptorBaseProperty>().Select(x => x.ToString());
        }

        public static void OnPropertyChanged_DynamicProperty(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.OnPropertyChanged_Property(propertyName);
            modelAdaptor.OnPropertyChanged_Property_Attachments(propertyName);
            modelAdaptor.OnPropertyChanged_IsEdited_IsValid();
            modelAdaptor.OnPropertyChanged_Extensions_Active();
            modelAdaptor.StateModified_Raise(propertyName);
        }
        #endregion
    }
}
