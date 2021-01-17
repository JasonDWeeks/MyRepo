using Common.ModelTools.ModelAdaptor.Hierarchy;
using Common.ModelTools.ModelAdaptor.Modifier;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Library
{
    public static partial class ModelAdaptorLibrary_IsValid
    {
        #region Required
        public static void Set_Validation_Required<T>(this IsValidProperty<T> property, Delegate_Ma_Val_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var isValid = ma._Required(pv, pn, false);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }

        public static void Set_Validation_Required<T>(this IsValidPropertyIndexValue<T> property, Delegate_Ma_Val_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var isValid = ma._Required(pv, pn, false);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }
        #endregion

        #region Required_Text
        public static void Set_Validation_Required_Text<T>(this IsValidProperty<T> property, Delegate_Ma_Val_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var isValid = ma._Required(pv, pn, true);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }

        public static void Set_Validation_Required_Text<T>(this IsValidPropertyIndexValue<T> property, Delegate_Ma_Val_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var isValid = ma._Required(pv, pn, true);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }
        #endregion

        #region MaxLength
        public static void Set_Validation_MaxLength(this IsValidProperty<string> property, int maxLength)
        {
            property.Set_Validation(
                (ma, pv, pn) => (pv ?? "").Length <= maxLength
            );
        }

        public static void Set_Validation_MaxLength(this IsValidPropertyIndexValue<string> property, int maxLength)
        {
            property.Set_Validation(
                (ma, pv, pn) => (pv ?? "").Length <= maxLength
            );
        }
        #endregion

        #region TextConversion
        public static void Set_Validation_TextConversion<T>(this IsValidProperty<T> property, Delegate_Ma_Val_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var isValid = ma._TextConversion(pv, pn);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }

        public static void Set_Validation_TextConversion<T>(this IsValidPropertyIndexValue<T> property, Delegate_Ma_Val_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var isValid = ma._TextConversion(pv, pn);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }
        #endregion

        #region Children
        public static void Set_Validation_Children<T>(this IsValidProperty<ModelAdaptorChildren<T>> property, Delegate_Ma_Val_Pn<ModelAdaptorChildren<T>, bool> additionalCriteria = null, bool andOperator = true)
            where T : IModelAdaptorHierarchy
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var children = (ModelAdaptorChildren<T>)ma.Property(pn).Value.Get;
                    bool isValid = true;
                    if (children != null)
                        isValid = !children.Loaded.Item || children.Display.All(x => (bool)x.IsValid);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }

        public static void Set_Validation_Children<T>(this IsValidPropertyIndexValue<ModelAdaptorChildren<T>> property, Delegate_Ma_Val_Pn<ModelAdaptorChildren<T>, bool> additionalCriteria = null, bool andOperator = true)
            where T : IModelAdaptorHierarchy
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var children = (ModelAdaptorChildren<T>)ma.Property(pn).Value.Get;
                    bool isValid = true;
                    if (children != null)
                        isValid = !children.Loaded.Item || children.Display.All(x => (bool)x.IsValid);
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && isValid;
                        return additionalCriteria.Invoke(ma, pv, pn) || isValid;
                    }
                    return isValid;
                }
            );
        }
        #endregion

        #region Children_Required
        public static void Set_Validation_Children_Required<T>(this IsValidProperty<ModelAdaptorChildren<T>> property, Delegate_Ma_Val_Pn<ModelAdaptorChildren<T>, bool> additionalCriteria = null, bool andOperator = true)
            where T : IModelAdaptorHierarchy
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var children = (ModelAdaptorChildren<T>)ma.Property(pn).Value.Get;
                    var any = !children.Loaded.Item || children.Display.Any();
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && any;
                        return additionalCriteria.Invoke(ma, pv, pn) || any;
                    }
                    return any;
                }
            );
        }

        public static void Set_Validation_Children_Required<T>(this IsValidPropertyIndexValue<ModelAdaptorChildren<T>> property, Delegate_Ma_Val_Pn<ModelAdaptorChildren<T>, bool> additionalCriteria = null, bool andOperator = true)
            where T : IModelAdaptorHierarchy
        {
            property.Set_Validation(
                (ma, pv, pn) =>
                {
                    var children = (ModelAdaptorChildren<T>)ma.Property(pn).Value.Get;
                    var any = !children.Loaded.Item || children.Display.Any();
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pv, pn) && any;
                        return additionalCriteria.Invoke(ma, pv, pn) || any;
                    }
                    return any;
                }
            );
        }
        #endregion
    }
}
