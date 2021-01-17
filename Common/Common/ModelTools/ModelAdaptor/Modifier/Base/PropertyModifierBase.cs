using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Modifier.Base
{
    public abstract class PropertyModifierBase
    {
        #region Adaptor
        protected IModelAdaptor Adaptor { get; set; }
        #endregion

        #region PropertyInfo
        protected PropertyInfo PropertyInfo { get; set; }
        #endregion
    }
}
