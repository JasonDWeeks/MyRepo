using Common.ModelTools.Common.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Hierarchy.Internal
{
    static partial class ModelAdaptorChildren_Ext_List
    {
        #region PopulateList
        public static List<IModelAdaptor> PopulateList(this IModelAdaptorChildren modelAdaptorChildren)
        {
            var ret = new List<IModelAdaptor>();

            modelAdaptorChildren.PreLoad_Raise();
            var children = modelAdaptorChildren.CheckSkipLoad()?.GetChildren?.Invoke()?.ToList();
            int sort = 0;
            children?.ForEach(child =>
            {
                modelAdaptorChildren.InitializeChild?.Invoke(child);
                child.Parent = modelAdaptorChildren.Parent;
                child.Adaptor.StateModified += (s, e) => modelAdaptorChildren.CheckSkip()?.Parent?.Adaptor?.Property(modelAdaptorChildren.PropertyName).ForceNotify();
                child.Adaptor.Sort.SetItemNoNotify(sort++);
                ret.Add(child.Adaptor);
            });
            modelAdaptorChildren.Loaded.Item = true;
            modelAdaptorChildren.PostLoad_Raise();

            return ret;
        }
        #endregion

        #region ResetDisplay
        public static void ResetDisplay(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.Display.Clear();
            modelAdaptorChildren.OnPropertyChanged(nameof(modelAdaptorChildren.Display));
        }
        #endregion

        #region OnSelect_Raise
        public static void OnSelect_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnSelect));
        }
        #endregion
    }
}
