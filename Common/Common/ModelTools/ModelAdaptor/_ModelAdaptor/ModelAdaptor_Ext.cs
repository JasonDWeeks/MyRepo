using System;
using System.Linq.Expressions;

namespace Common.ModelTools.ModelAdaptor
{
    public static partial class ModelAdaptor_Ext
    {
        #region ToAdaptor
        public static ModelAdaptor<T> ToAdaptor<T>(this T source)
            where T : class, new()
        {
            var obj = source ?? throw new ArgumentNullException(nameof(source));

            return new ModelAdaptor<T>(obj);
        }

        public static ModelAdaptor<T> ToAdaptor<T>(this T source, params Expression<Func<T, object>>[] exclude)
            where T : class, new()
        {
            var obj = source ?? throw new ArgumentNullException(nameof(source));

            return new ModelAdaptor<T>(obj, exclude);
        }

        public static ModelAdaptor<T> ToAdaptor<T>(this T source, params string[] exclude)
            where T : class, new()
        {
            var obj = source ?? throw new ArgumentNullException(nameof(source));

            return new ModelAdaptor<T>(obj, exclude);
        }
        #endregion
    }
}
