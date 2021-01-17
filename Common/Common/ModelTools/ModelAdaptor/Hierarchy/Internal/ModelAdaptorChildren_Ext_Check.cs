using Common.ModelTools.Common.Internal;
using System;
using System.Collections.Generic;

namespace Common.ModelTools.ModelAdaptor.Hierarchy.Internal
{
    static partial class ModelAdaptorChildren_Ext_Check
    {
        #region CheckSkip
        public static IModelAdaptorChildren CheckSkip(this IModelAdaptorChildren modelAdaptorChildren)
        {
            if (modelAdaptorChildren.Skip)
                return null;
            return modelAdaptorChildren;
        }
        #endregion

        #region CheckLoaded
        public static IModelAdaptorChildren CheckLoaded(this IModelAdaptorChildren modelAdaptorChildren)
        {
            if (!modelAdaptorChildren.Loaded)
                return null;
            return modelAdaptorChildren;
        }
        #endregion

        #region CheckSkipLoad
        public static IModelAdaptorChildren CheckSkipLoad(this IModelAdaptorChildren modelAdaptorChildren)
        {
            if (modelAdaptorChildren.SkipLoad)
                return null;
            return modelAdaptorChildren;
        }
        #endregion

        #region SkipBlock
        public static void SkipBlock(this IModelAdaptorChildren modelAdaptorChildren, Action process)
        {
            modelAdaptorChildren.Skip.Item = true;
            process();
            modelAdaptorChildren.Skip.Item = false;
        }
        #endregion

        #region SkipLoadBlock
        public static IEnumerable<IModelAdaptor> SkipLoadBlock(this IModelAdaptorChildren modelAdaptorChildren, Func<IEnumerable<IModelAdaptor>> list)
        {
            modelAdaptorChildren.SkipLoad.Item = true;
            var ret = list();
            modelAdaptorChildren.SkipLoad.Item = false;
            return ret;
        }
        #endregion

        #region PreLoad_Raise
        public static void PreLoad_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.PreLoad));
        }
        #endregion

        #region PostLoad_Raise
        public static void PostLoad_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.PostLoad));
        }
        #endregion
    }
}
