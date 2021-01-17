using Common.ModelTools.Common.Internal;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Extendable
    {
        #region GetExtensionProperty
        public static bool GetExtensionProperty(this IModelAdaptor modelAdaptor, string extensionPropertyName, out object value)
        {
            value = null;
            if (!modelAdaptor.IsExtensionProperty(extensionPropertyName))
                return false;
            var propertyName = extensionPropertyName.Replace(modelAdaptor.GetExtensionPropertyPrefix(), string.Empty);
            if (modelAdaptor.Extension[propertyName] == null)
                return false;
            value = modelAdaptor.Extension[propertyName](modelAdaptor as IModelAdaptor);

            if (!modelAdaptor.ActiveExtensionProperties.Any(x => x == propertyName))
                modelAdaptor.ActiveExtensionProperties.Add(propertyName);

            return true;
        }
        #endregion

        #region IsExtensionProperty
        public static bool IsExtensionProperty(this IModelAdaptor modelAdaptor, string extensionPropertyName)
        {
            return modelAdaptor.Extension.Select(x => x.Key).Any(x => (modelAdaptor.GetExtensionPropertyPrefix() + x) == extensionPropertyName);
        }
        #endregion

        #region ExtensionPropertyPrefix
        public static string GetExtensionPropertyPrefix(this IModelAdaptor modelAdaptor)
        { 
            return "_x_";
        }
        #endregion

        #region OnPropertyChanged_Extensions_Active
        public static void OnPropertyChanged_Extensions_Active(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.ActiveExtensionProperties.ToList().ForEach(propertyName =>
            {
                modelAdaptor.OnPropertyChanged(modelAdaptor.GetExtensionPropertyPrefix() + propertyName);
            });
        }
        #endregion
    }
}
