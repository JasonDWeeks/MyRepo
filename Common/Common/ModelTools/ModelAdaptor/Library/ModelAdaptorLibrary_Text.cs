using Common.ModelTools.ModelAdaptor.Modifier;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Library
{
    public static partial class ModelAdaptorLibrary_Text
    {
        #region UpdateSource_TextToSource
        public static void Set_UpdateSource_TextToSource<T>(this TextProperty<T> property, Delegate_Ma_Val_Pn<T, T> additionalModification = null)
        {
            property.Set_UpdateSource(
                (ma, ptv, pn) =>
                {
                    if (string.IsNullOrWhiteSpace(ptv))
                        return default(T);

                    var type = ma.ModelProperties.GetByName(pn).PropertyType;

                    MethodInfo tryParseMethod;
                    type._GetParseMethod(pn, true, out tryParseMethod);

                    var args = new object[] { ptv, null };
                    tryParseMethod.Invoke(null, args);

                    var ret = (T)args[1];
                    if (additionalModification != null)
                        return additionalModification(ma, ret, pn);
                    return ret;
                }
            );
        }
        #endregion

        #region IfNull_TextInitialize
        public static void Set_IfNull_TextInitialize<T>(this TextProperty<T> property, Delegate_Ma_Val_Pn<string, string> additionalModification = null)
        {
            property.Set_IfNull(
                (ma, pv, pn) =>
                {
                    var ret = pv?.ToString();
                    if (additionalModification != null)
                        return additionalModification(ma, ret, pn);
                    return ret;
                }
            );
        }
        #endregion
    }
}
