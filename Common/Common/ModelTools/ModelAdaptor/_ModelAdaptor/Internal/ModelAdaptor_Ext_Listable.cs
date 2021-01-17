using Common.ModelTools.Common.Internal;
using System;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Listable
    {
        #region OnPropertyChanged_ListableProperty
        public static void OnPropertyChanged_ListableProperty(this IModelAdaptor modelAdaptor, string propertyName, EventHandler eventHandler)
        {
            modelAdaptor.OnPropertyChanged(propertyName);
            modelAdaptor.OnPropertyChanged_Properties_Active();
            modelAdaptor.OnPropertyChanged_IsEdited_IsValid();
            modelAdaptor.OnPropertyChanged_Extensions_Active();
            modelAdaptor.StateModified_Raise(null);
            eventHandler?.Invoke(modelAdaptor, null);
        }
        #endregion
    }
}
