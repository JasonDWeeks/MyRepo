using System;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    static partial class ModelAdaptor_Ext_Cache
    {
        #region GetCacheValue
        public static bool GetCacheValue<T>(this IModelAdaptor modelAdaptor, string propertyName, string propertyModifierKey, out T value)
        {
            value = default(T);

            if (modelAdaptor.TrySetMember_COUNT == 0)
                return false;

            var key = modelAdaptor.Cache.ToList()
                .Where(x => x.Key?.Item1 == propertyName && x.Key?.Item2 == propertyModifierKey)
                .Select(x => x.Key).FirstOrDefault();

            if (key != null)
            {
                object val;
                modelAdaptor.Cache.TryGetValue(key, out val);
                if (val != null)
                    value = (T)val;
                return true;
            }

            return false;
        }
        #endregion

        #region SetCacheValue
        public static void SetCacheValue(this IModelAdaptor modelAdaptor, string propertyName, string propertyModifierKey, object value)
        {
            if (modelAdaptor.TrySetMember_COUNT == 0)
                return;

            modelAdaptor.Cache[new Tuple<string, string>(propertyName, propertyModifierKey)] = value;
        }
        #endregion
    }
}
