using Common.ModelTools.ModelAdaptor.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Base;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public partial class IsEditedProperty<T> : PropertyModifierBase
    {
        #region Constructor
        public IsEditedProperty(IModelAdaptor adaptor, PropertyInfo propertyInfo)
        {
            Adaptor = adaptor ?? throw new ArgumentNullException(nameof(adaptor));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
        #endregion

        #region Get
        public bool Get
        {
            get
            {
                if (Adaptor.IsDeleted)
                    return false;

                bool value;
                if (Adaptor.GetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._IsEdited), out value))
                    return value;

                var current = PropertyInfo.GetValue(Adaptor.Model);
                var original = PropertyInfo.GetValue(Adaptor.Original.Item);

                value = Compare?.Invoke(Adaptor, current, original, PropertyInfo.Name) ?? !Equals(current, original);
                Adaptor.SetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._IsEdited), value);

                return value;
            }
        }
        #endregion

        #region Has_Compare
        public bool Has_Compare
        {
            get
            {
                return Compare != null;
            }
        }
        #endregion

        #region Set_Compare
        public void Set_Compare(Delegate_Ma_Val1_Val2_Pn<T, bool> compareDelegate)
        {
            if (compareDelegate == null)
                throw new ArgumentNullException(nameof(compareDelegate));
            Compare = (a, p1, p2, pn) => compareDelegate(a, (T)p1, (T)p2, pn);
        }
        #endregion

        #region Remove_Compare
        public void Remove_Compare()
        {
            Compare = null;
        }
        #endregion

        #region Compare
        private Delegate_Ma_Val1_Val2_Pn<object, bool> Compare
        {
            get
            {
                Delegate_Ma_Val1_Val2_Pn<object, bool> compareDelegate;
                Adaptor.IsEdited_CompareDictionary.TryGetValue(PropertyInfo.Name, out compareDelegate);
                return compareDelegate;
            }
            set
            {
                if (value == null)
                    Adaptor.IsEdited_CompareDictionary.Remove(PropertyInfo.Name);
                else
                    Adaptor.IsEdited_CompareDictionary[PropertyInfo.Name] = value;
            }
        }
        #endregion

        #region Cast
        public static implicit operator bool(IsEditedProperty<T> IsEditedProperty)
        {
            if (IsEditedProperty == null)
                return true;
            return IsEditedProperty.Get;
        }
        #endregion
    }
}
