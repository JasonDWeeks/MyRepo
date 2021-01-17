using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor.Library
{
    public static partial class ModelAdaptorLibrary
    {
        #region _Required
        internal static bool _Required<T>(this IModelAdaptor modelAdaptor, T propertyValue, string propertyName, bool text)
        {
            bool textValid = true;
            var propertyText = modelAdaptor.Property(propertyName).Text;
            if (propertyText.Has_UpdateSource)
                textValid = !string.IsNullOrWhiteSpace(propertyText);

            bool valueValid = true;
            Type type = propertyValue?.GetType();
            if (type == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(propertyValue.ToString()))
                    valueValid = false;
            }
            else
            {
                if (EqualityComparer<T>.Default.Equals(propertyValue, default(T)) && !typeof(T).IsValueType)
                    valueValid = false;
            }

            if (text)
                return textValid;
            return valueValid;
        }
        #endregion

        #region _TextConversion
        internal static bool _TextConversion<T>(this IModelAdaptor modelAdaptor, T propertyValue, string propertyName)
        {
            var type = modelAdaptor.Model.GetType().GetProperty(propertyName).PropertyType;

            MethodInfo tryParseMethod;
            type._GetParseMethod(propertyName, true, out tryParseMethod);
            var args = new object[] { (string)modelAdaptor.Property(propertyName).Text, null };

            var result = (bool)tryParseMethod.Invoke(null, args);
            return result;
        }
        #endregion

        #region _GetParseMethod
        internal static bool _GetParseMethod(this Type type, string propertyName, bool throwException, out MethodInfo tryParseMethod)
        {
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            var argTypes = new Type[] { typeof(string), type.MakeByRefType() };
            tryParseMethod = type.GetMethod("TryParse", argTypes);
            if (tryParseMethod == null)
            {
                if (throwException)
                    throw new InvalidOperationException(string.Format("Could not find TryParse method for Property '{0}' of type '{1}'.", propertyName, type.Name));
                else
                    return false;
            }

            if (!tryParseMethod.IsStatic)
            {
                if (throwException)
                    throw new InvalidOperationException(string.Format("TryParse method for Property '{0}' of type '{1}' must be static.", propertyName, type.Name));
                else
                    return false;
            }
            return true;
        }
        #endregion
    }
}
