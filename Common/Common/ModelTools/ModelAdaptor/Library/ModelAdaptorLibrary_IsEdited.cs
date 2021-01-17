using Common.ModelTools.ModelAdaptor.Hierarchy;
using Common.ModelTools.ModelAdaptor.Modifier;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Library
{
    public static partial class ModelAdaptorLibrary_IsEdited
    {
        #region IgnoreWhiteSpace
        public static void Set_Compare_IgnoreWhiteSpace(this IsEditedProperty<string> property, Delegate_Ma_Val1_Val2_Pn<string, bool> additionalCriteria = null, bool andOperator = true)
        {
            property.Set_Compare(
                (ma, curr, orig, pn) =>
                {
                    if (string.IsNullOrWhiteSpace(curr))
                        curr = null;
                    if (string.IsNullOrWhiteSpace(orig))
                        orig = null;
                    var isEdited = orig != curr;

                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, curr, orig, pn) && isEdited;
                        return additionalCriteria.Invoke(ma, curr, orig, pn) || isEdited;
                    }

                    return isEdited;
                }
            );
        }
        #endregion

        #region NonNullable_CheckText
        public static void Set_Compare_NonNullable_CheckText<T>(this IsEditedProperty<T> property, Delegate_Ma_Val1_Val2_Pn<T, bool> additionalCriteria = null, bool andOperator = true)
            where T : struct
        {
            property.Set_Compare(
                (ma, curr, orig, pn) =>
                {
                    var isValid = ma._TextConversion(curr, pn);
                    if (!isValid)
                        return true;

                    var isEdited = !Equals(curr, orig);

                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, curr, orig, pn) && isEdited;
                        return additionalCriteria.Invoke(ma, curr, orig, pn) || isEdited;
                    }

                    return isEdited;
                }
            );
        }
        #endregion

        #region Children
        public static void Set_Compare_Children<T>(this IsEditedProperty<ModelAdaptorChildren<T>> property, Delegate_Ma_Pn<bool> additionalCriteria = null, bool andOperator = true)
            where T : IModelAdaptorHierarchy
        {
            property.Set_Compare(
                (ma, curr, orig, pn) =>
                {
                    var children = (ModelAdaptorChildren<T>)ma.Property(pn).Value.Get;
                    bool isEdited = false;
                    if (children != null)
                        isEdited = children.Loaded.Item && (children.List.Any(x => x.IsNew) || children.List.Any(x => x.IsDeleted) || children.Display.Any(x => (bool)x.IsEdited));
                    if (additionalCriteria != null)
                    {
                        if (andOperator)
                            return additionalCriteria.Invoke(ma, pn) && isEdited;
                        return additionalCriteria.Invoke(ma, pn) || isEdited;
                    }
                    return isEdited;
                }
            );
        }
        #endregion
    }
}
