namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Model
    {
        #region ExtensionPropertyPrefix
        public static string GetModelPropertyPrefix(this IModelAdaptor modelAdaptor)
        {
            return "_m_";
        }
        #endregion

        #region OnPropertyChanged_Properties_Active
        public static void OnPropertyChanged_Properties_Active(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.ActiveModelProperties.ForEach(propertyName =>
            {
                modelAdaptor.OnPropertyChanged_Property(propertyName);
            });
        }
        #endregion
    }
}
