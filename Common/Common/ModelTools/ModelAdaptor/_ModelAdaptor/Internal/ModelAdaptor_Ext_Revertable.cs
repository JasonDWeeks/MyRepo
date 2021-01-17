using Common.ModelTools.Common.Internal;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Revertable
    {
        #region ClearText
        public static void ClearText(this IModelAdaptor modelAdaptor, string propertyName)
        {
            modelAdaptor.Property(propertyName).Text.Clear();
        }
        #endregion

        #region ClearText_Properties_Active
        public static void ClearText_Properties_Active(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.ActiveModelProperties.ToList().ForEach(propertyName =>
            {
                modelAdaptor.ClearText(propertyName);
            });
        }
        #endregion
    }
}
