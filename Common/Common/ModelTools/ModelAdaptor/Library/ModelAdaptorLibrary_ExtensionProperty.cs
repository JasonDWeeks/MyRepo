using Common.ModelTools.ModelAdaptor.Modifier;
using System;
using System.Text;

namespace Common.ModelTools.ModelAdaptor.Library
{
    public static partial class ModelAdaptorLibrary_ExtensionProperty
    {
        #region Title
        public static void Set_Extension_Title(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.Extension["Title"] = (ma) =>
            {
                if (ma.IsNew)
                    return "New";
                return "Edit";
            };
        }

        public static void Set_Extension_Title(this IModelAdaptor modelAdaptor, string title, bool titleOnly = false)
        {
            if (titleOnly)
            {
                modelAdaptor.Extension["Title"] = (ma) => title;
            }
            else
            {
                var t = title != null ? " " + title : string.Empty;
                modelAdaptor.Extension["Title"] = (ma) =>
                {
                    if (ma.IsNew)
                        return "New" + t;
                    return "Edit" + t;
                };
            }
        }

        public static void Set_Extension_Title(this IModelAdaptor modelAdaptor, Func<string> title, bool titleOnly = false)
        {
            if (titleOnly)
            {
                modelAdaptor.Extension["Title"] = (ma) => title();
            }
            else
            {
                modelAdaptor.Extension["Title"] = (ma) =>
                {
                    var t = title != null ? " " + title() : string.Empty;
                    if (ma.IsNew)
                        return "New" + t;
                    return "Edit" + t;
                };
            }
        }
        #endregion

        #region CanSave
        public static void Set_Extension_CanSave(this IModelAdaptor modelAdaptor, Delegate_Ma<bool> additionalCriteria = null, bool andOperator = true)
        {
            if (additionalCriteria != null)
            {
                if (andOperator)
                    modelAdaptor.Extension["CanSave"] = (ma) => additionalCriteria(ma) && ma.IsEdited && ma.IsValid;
                else
                    modelAdaptor.Extension["CanSave"] = (ma) => additionalCriteria(ma) || (ma.IsEdited && ma.IsValid);
            }
            else
                modelAdaptor.Extension["CanSave"] = (ma) => ma.IsEdited && ma.IsValid;
        }
        #endregion

        #region Status
        public static void Set_Extension_Status(this IModelAdaptor modelAdaptor)
        {
            modelAdaptor.Extension["Status"] = (ma) =>
            {
                var msg = new StringBuilder();
                if (ma.IsNew)
                    msg.Append("* New *");
                else if (ma.IsEdited)
                    msg.Append("* Edited *");

                if (!ma.IsValid)
                {
                    if (msg.Length > 0)
                        msg.Append(" ");
                    msg.Append("Invalid!");
                }
                return msg.ToString();
            };
        }
        #endregion
    }
}
