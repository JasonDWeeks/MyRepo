using Common.ModelTools.ModelAdaptor.Hierarchy;

namespace Common.ModelTools.ModelAdaptor.Library
{
    public static partial class ModelAdaptorLibrary_Utilities
    {
        #region GetMessage
        public static string GetMessage(this IModelAdaptor modelAdaptor, string propertyName, string message, params object[] templateArgs)
        {
            var isValid = modelAdaptor.Property(propertyName).IsValid;
            if (!isValid[message])
                return string.Format(message, templateArgs);
            return null;
        }
        #endregion

        #region SetAsOriginal
        public static void SetAsOriginal(this IModelAdaptorHierarchy obj)
        {
            if (obj.Adaptor.IsNew)
                obj.Adaptor.IsNew = false;

            if (!obj.Adaptor.IsDeleted)
                obj.Adaptor.SetAsOriginal();
        }
        #endregion
    }
}
