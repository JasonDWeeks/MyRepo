using System;

namespace Common.ModelTools.ModelAdaptor.Hierarchy.Internal
{
    static partial class ModelAdaptorChildren_Ext_Initialization
    {
        #region CopyChild
        public static IModelAdaptorHierarchy CopyChild(this IModelAdaptorChildren modelAdaptorChildren, object child)
        {
            var type = child.GetType();
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                if (type.IsValueType)
                {
                    return new object() as IModelAdaptorHierarchy;
                }
                return null;
            }
            var ret = Activator.CreateInstance(type);
            QuickMapper.QuickMapper.Map(child, ret);
            return ret as IModelAdaptorHierarchy;
        }
        #endregion
    }
}
