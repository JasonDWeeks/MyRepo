using Common.ModelTools.ModelAdaptor.Modifier.Base;
using System;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Modifier
{
    public partial class IsValidProperty<T> : IsValidPropertyBase
    {
        #region Constructor
        public IsValidProperty(IModelAdaptor adaptor, System.Reflection.PropertyInfo propertyInfo) : base(adaptor, propertyInfo)
        {
            Key = _MainKey;
        }
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

                return Validations.Select(x => x.Key).All(x => this[x]);
            }
        }
        #endregion

        #region IsValidPropertyIndexValue
        private IsValidPropertyIndexValue<T> _IsValidPropertyIndexValue;
        public IsValidPropertyIndexValue<T> this[string key]
        {
            get
            {
                if (_IsValidPropertyIndexValue == null)
                    _IsValidPropertyIndexValue = new IsValidPropertyIndexValue<T>(Adaptor, PropertyInfo);
                _IsValidPropertyIndexValue.Key = key;
                return _IsValidPropertyIndexValue;
            }
        }
        #endregion

        #region SkipValidationIfNotNewAndNotEdited
        public bool SkipValidationIfNotNewAndNotEdited
        {
            get { return SkipValidation; }
            set { SkipValidation = value; }
        }
        #endregion

        #region Has_Validations
        public bool Has_Validations
        {
            get
            {
                return Validations.Any();
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
            Validations.Remove(_MainKey);
        }
        #endregion

        #region Remove_AllValidations
        public void Remove_AllValidations()
        {
            Validations.Clear();
        }
        #endregion
    }
}
