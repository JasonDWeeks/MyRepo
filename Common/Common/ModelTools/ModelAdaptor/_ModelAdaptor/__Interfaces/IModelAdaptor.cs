using Common.ModelTools.ModelAdaptor.Modifier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace Common.ModelTools.ModelAdaptor
{
    public interface IModelAdaptor : INotifyPropertyChanged
    {
        #region Model
        object Model { get; }
        ItemCollection<PropertyInfo> ModelProperties { get; }
        ItemCollection<string> ActiveModelProperties { get; }
        ItemCollection<string> ExcludeModelProperties { get; }
        ItemCollection<PropertyInfo> ModelAdaptorProperties { get; }
        #endregion

        #region Attachments
        ItemDictionary<string, List<AttachmentInfo>> AttachmentDictionary { get; }

        ItemField<int> Attachment_SetAsOriginal_COUNT { get; }
        ItemField<int> Attachment_Revert_COUNT { get; }

        ItemCollection<string> Attachment_SetAsOriginal_SkipList { get; }
        ItemCollection<string> Attachment_Revert_SkipList { get; }
        #endregion

        #region Cache
        ItemDictionary<Tuple<string, string>, object> Cache { get; }
        #endregion

        #region Dictionaries
        ItemDictionary<string, Delegate_Ma_Val1_Val2_Pn<object, bool>> IsEdited_CompareDictionary { get; }
        ItemDictionary<string, Dictionary<string, Delegate_Ma_Val_Pn<object, bool>>> IsValid_ValidationDictionary { get; }
        ItemDictionary<string, bool> IsValid_SkipValidationDictionary { get; }
        ItemDictionary<string, EventHandler<PropertyChangeEventArgs>> Value_ChangingDictionary { get; }
        ItemDictionary<string, EventHandler<PropertyChangeEventArgs>> Value_ChangedDictionary { get; }
        ItemDictionary<string, bool> Value_DuringSetAsOriginalDictionary { get; }
        ItemDictionary<string, bool> Value_DuringRevertDictionary { get; }
        ItemDictionary<string, string> TextDictionary { get; }
        ItemDictionary<string, Delegate_Ma_Val_Pn<string, object>> Text_UpdateSourceDictionary { get; }
        ItemDictionary<string, Delegate_Ma_Val_Pn<object, string>> Text_IfNullDictionary { get; }
        ItemDictionary<string, DelegateConverter<Delegate_Ma_Pn<string>, string>> LabelDictionary { get; }
        ItemDictionary<string, DelegateConverter<Delegate_Ma_Pn<string>, string>> MessageDictionary { get; }
        ItemDictionary<string, DelegateConverter<Delegate_Ma_Pn<bool>, bool>> ReadOnlyDictionary { get; }
        #endregion

        #region Dynamic
        ItemField<int> TrySetMember_COUNT { get; }
        #endregion

        #region Extension
        Dictionary<string, Delegate_Ma<object>> Extension { get; }
        ItemCollection<string> ActiveExtensionProperties { get; }
        #endregion

        #region Listable
        bool IsNew { get; set; }
        bool IsDeleted { get; set; }
        DelegateConverter<Func<bool>, bool> CanDelete { get; set; }
        bool IsSelected { get; set; }
        ItemField<int> Sort { get; }

        event EventHandler IsNew_Changed;
        event EventHandler IsSelected_Changed;
        event EventHandler IsDeleted_Changed;
        event EventHandler Sort_Changed;
        #endregion

        #region Revertable
        ItemField<object> Original { get; }
        ItemField<bool> DuringSetAsOriginal { get; }
        ItemField<bool> DuringRevert { get; }

        ICommand SetAsOriginal_Command { get; }
        ICommand Revert_Command { get; }
        #endregion

        #region State
        bool IsEdited { get; }
        bool IsValid { get; }

        event EventHandler<PropertyChangeEventArgs> StateModified;
        #endregion
    }
}
