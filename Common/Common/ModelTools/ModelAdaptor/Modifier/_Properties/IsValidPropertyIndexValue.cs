using Common.ModelTools.ModelAdaptor.Internal;
using Common.ModelTools.ModelAdaptor.Modifier.Base;
using Common.ModelTools.ModelAdaptor.Modifier.Internal;
using System;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public partial class IsValidPropertyIndexValue<T> : IsValidPropertyBase
    {
        #region Constructors
        public IsValidPropertyIndexValue(IModelAdaptor adaptor, PropertyInfo propertyInfo, string key) : base(adaptor, propertyInfo)
        {
            Key = key;
        }

        internal IsValidPropertyIndexValue(IModelAdaptor adaptor, PropertyInfo propertyInfo) : base(adaptor, propertyInfo)
        { }
        #endregion

        #region Get
        public override bool Get
        {
            get
            {
                if (Adaptor.IsDeleted)
                    return true;

                if (SkipValidation && !Adaptor.IsNew && !Adaptor.Property(PropertyInfo.Name).IsEdited)
                    return true;

                bool value;
                if (Adaptor.GetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._IsValid) + Key, out value))
                    return value;

                value = Validation?.Invoke(Adaptor, PropertyInfo.GetValue(Adaptor.Model), PropertyInfo.Name) ?? true;
                Adaptor.SetCacheValue(PropertyInfo.Name, nameof(ModifierProperty._IsValid) + Key, value);

                return value;
            }
        }
        #endregion

        #region Has_Validation
        public bool Has_Validation
        {
            get
            {
                return Validation != null;
            }
        }
        #endregion

        #region Set_Validation
        public void Set_Validation(Delegate_Ma_Val_Pn<T, bool> validationDelegate)
        {
            if (validationDelegate == null)
                throw new ArgumentNullException(nameof(validationDelegate));
            Validations[Key] = (a, p, pn) => validationDelegate(a, (T)p, pn);
        }
        #endregion

        #region Remove_Validation
        public void Remove_Validation()
        {
            Validations.Remove(Key);
        }
        #endregion

        #region Validation
        private Delegate_Ma_Val_Pn<object, bool> Validation
        {
            get
            {
                Delegate_Ma_Val_Pn<object, bool> validation;
                Validations.TryGetValue(Key, out validation);
                return validation;
            }
        }
        #endregion
    }
}
