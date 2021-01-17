using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Modifier.Base
{
    public abstract partial class IsValidPropertyBase : PropertyModifierBase
    {
        #region Static Fields
        protected static string _MainKey = Guid.NewGuid().ToString();
        #endregion

        #region Constructor
        public IsValidPropertyBase(IModelAdaptor adaptor, PropertyInfo propertyInfo)
        {
            Adaptor = adaptor ?? throw new ArgumentNullException(nameof(adaptor));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
        #endregion

        #region Get
        public abstract bool Get { get; }
        #endregion

        #region Validations
        protected Dictionary<string, Delegate_Ma_Val_Pn<object, bool>> Validations
        {
            get
            {
                Dictionary<string, Delegate_Ma_Val_Pn<object, bool>> validationDictionary;
                Adaptor.IsValid_ValidationDictionary.TryGetValue(PropertyInfo.Name, out validationDictionary);
                if (validationDictionary == null)
                {
                    Adaptor.IsValid_ValidationDictionary[PropertyInfo.Name] = new Dictionary<string, Delegate_Ma_Val_Pn<object, bool>>();
                    return Adaptor.IsValid_ValidationDictionary[PropertyInfo.Name];
                }
                return validationDictionary;
            }
        }
        #endregion

        #region SkipValidation
        protected bool SkipValidation
        {
            get
            {
                bool skip;
                if (!Adaptor.IsValid_SkipValidationDictionary.TryGetValue(PropertyInfo.Name, out skip))
                    return true;
                return skip;
            }
            set
            {
                Adaptor.IsValid_SkipValidationDictionary[PropertyInfo.Name] = value;
                Adaptor.Property(PropertyInfo.Name).ForceNotify();
            }
        }
        #endregion

        #region Key
        internal string Key { get; set; }
        #endregion

        #region Cast
        public static implicit operator bool(IsValidPropertyBase valBase)
        {
            if (valBase == null)
                return true;
            return valBase.Get;
        }
        #endregion
    }
}
