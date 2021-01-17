using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ModelTools.ModelAdaptor.Hierarchy
{
    public static partial class ModelAdaptorChildren_Ext_ViewModel
    {
        public static T MainViewModel<T>(this IModelAdaptorHierarchy obj)
            where T : class
        {
            if (obj == null)
                return null;

            if (obj.Parent is T)
                return (T)obj.Parent;

            return (obj.Parent as IModelAdaptorHierarchy)?.MainViewModel<T>();
        }
    }
}
