using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Hierarchy.Internal;
using System;

namespace Common.ModelTools.ModelAdaptor.Hierarchy
{
    public static partial class ModelAdaptorChildren_Ext_Revert
    {
        #region Revert
        public static void Revert(this IModelAdaptorChildren source)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            adaptorChildren.CheckLoaded()?.RevertItems();

            adaptorChildren.ResetDisplay();
            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnRevert_Raise();
        }
        #endregion
    }
}
