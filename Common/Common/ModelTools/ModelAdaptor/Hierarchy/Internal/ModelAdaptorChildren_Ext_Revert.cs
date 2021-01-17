using Common.ModelTools.Common.Internal;

namespace Common.ModelTools.ModelAdaptor.Hierarchy.Internal
{
    static partial class ModelAdaptorChildren_Ext_Revert
    {
        #region OnRevert_Raise
        public static void OnRevert_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnRevert));
        }
        #endregion
    }
}
