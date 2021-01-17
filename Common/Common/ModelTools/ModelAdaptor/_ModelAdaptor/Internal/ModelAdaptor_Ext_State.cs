using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Modifier;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_State
    {
        #region GetChangeEventArg
        public static PropertyChangeEventArgs GetChangeEventArg(this IModelAdaptor modelAdaptor, string propertyName)
        {
            if (propertyName != null)
            {
                var property = modelAdaptor.Property(propertyName).Value;
                return new PropertyChangeEventArgs(property.Get_Original, property.Get, propertyName);
            }
            else
                return new PropertyChangeEventArgs(null, null);
        }
        #endregion

        #region StateModified_Raise
        public static void StateModified_Raise(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.RaiseEvent(nameof(modelAdaptor.StateModified), modelAdaptor.GetChangeEventArg(propertyName));
        }
        #endregion

        #region OnPropertyChanged_IsEdited_IsValid
        public static void OnPropertyChanged_IsEdited_IsValid(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.IsEdited));
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.IsValid));
            modelAdaptor.OnPropertyChanged(nameof(modelAdaptor.CanDelete));
        }
        #endregion
    }
}
