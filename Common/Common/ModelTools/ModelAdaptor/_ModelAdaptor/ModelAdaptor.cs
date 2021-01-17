using Common.ModelTools.Common.Internal;
using Common.QuickMapper;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using Common.ModelTools.ModelAdaptor.Modifier;
using System.Dynamic;
using Common.ModelTools.ModelAdaptor.Internal;
using System.Reflection;
using System.Windows.Input;

namespace Common.ModelTools.ModelAdaptor
{
    public partial class ModelAdaptor<T> : DynamicObject, IModelAdaptor
        where T : class, new()
    {
        #region Constructor
        public ModelAdaptor(T current)
        {
            Model = current ?? throw new ArgumentNullException(nameof(current));
            Original = Model.MapTo<T>();
        }

        public ModelAdaptor(T current, params Expression<Func<T, object>>[] exclude)
        {
            Model = current ?? throw new ArgumentNullException(nameof(current));
            ExcludeModelProperties = exclude.GetPropertyNamesFromExpressions().ToList() ?? new List<string>();
            Original = Model.MapTo<T>(ExcludeModelProperties.ToArray());
        }

        public ModelAdaptor(T current, params string[] exclude)
        {
            Model = current ?? throw new ArgumentNullException(nameof(current));
            ExcludeModelProperties = exclude?.ToList() ?? new List<string>();
            Original = Model.MapTo<T>(ExcludeModelProperties.ToArray());
        }
        #endregion

        #region Attachments
        public ItemDictionary<string, List<AttachmentInfo>> AttachmentDictionary { get; } = new Dictionary<string, List<AttachmentInfo>>();
        public ItemField<int> Attachment_SetAsOriginal_COUNT { get; } = 0;
        public ItemField<int> Attachment_Revert_COUNT { get; } = 0;
        public ItemCollection<string> Attachment_SetAsOriginal_SkipList { get; } = new List<string>();
        public ItemCollection<string> Attachment_Revert_SkipList { get; } = new List<string>();
        #endregion

        #region Cache
        public ItemDictionary<Tuple<string, string>, object> Cache { get; } = new Dictionary<Tuple<string, string>, object>();
        #endregion

        #region Dictionaries
        public ItemDictionary<string, Delegate_Ma_Val1_Val2_Pn<object, bool>> IsEdited_CompareDictionary { get; } = new Dictionary<string, Delegate_Ma_Val1_Val2_Pn<object, bool>>();
        public ItemDictionary<string, Dictionary<string, Delegate_Ma_Val_Pn<object, bool>>> IsValid_ValidationDictionary { get; } = new Dictionary<string, Dictionary<string, Delegate_Ma_Val_Pn<object, bool>>>();
        public ItemDictionary<string, bool> IsValid_SkipValidationDictionary { get; } = new Dictionary<string, bool>();
        public ItemDictionary<string, EventHandler<PropertyChangeEventArgs>> Value_ChangingDictionary { get; } = new Dictionary<string, EventHandler<PropertyChangeEventArgs>>();
        public ItemDictionary<string, EventHandler<PropertyChangeEventArgs>> Value_ChangedDictionary { get; } = new Dictionary<string, EventHandler<PropertyChangeEventArgs>>();
        public ItemDictionary<string, bool> Value_DuringSetAsOriginalDictionary { get; } = new Dictionary<string, bool>();
        public ItemDictionary<string, bool> Value_DuringRevertDictionary { get; } = new Dictionary<string, bool>();
        public ItemDictionary<string, string> TextDictionary { get; } = new Dictionary<string, string>();
        public ItemDictionary<string, Delegate_Ma_Val_Pn<string, object>> Text_UpdateSourceDictionary { get; } = new Dictionary<string, Delegate_Ma_Val_Pn<string, object>>();
        public ItemDictionary<string, Delegate_Ma_Val_Pn<object, string>> Text_IfNullDictionary { get; } = new Dictionary<string, Delegate_Ma_Val_Pn<object, string>>();
        public ItemDictionary<string, DelegateConverter<Delegate_Ma_Pn<string>, string>> LabelDictionary { get; } = new Dictionary<string, DelegateConverter<Delegate_Ma_Pn<string>, string>>();
        public ItemDictionary<string, DelegateConverter<Delegate_Ma_Pn<string>, string>> MessageDictionary { get; } = new Dictionary<string, DelegateConverter<Delegate_Ma_Pn<string>, string>>();
        public ItemDictionary<string, DelegateConverter<Delegate_Ma_Pn<bool>, bool>> ReadOnlyDictionary { get; } = new Dictionary<string, DelegateConverter<Delegate_Ma_Pn<bool>, bool>>();
        #endregion

        #region Dynamic
        public ItemField<int> TrySetMember_COUNT { get; } = 0;

        public override bool TryGetMember(GetMemberBinder binder, out object value)
        {
            return this.GetMember(binder.Name, out value);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return this.SetMember(binder.Name, value);
        }
        #endregion

        #region Extendable
        private Dictionary<string, Delegate_Ma<object>> _Extension;
        public Dictionary<string, Delegate_Ma<object>> Extension
        {
            get
            {
                if (_Extension == null)
                    _Extension = new Dictionary<string, Delegate_Ma<object>>();
                return _Extension;
            }
        }

        public ItemCollection<string> ActiveExtensionProperties { get; } = new List<string>();
        #endregion

        #region Listable

        #region IsNew
        private bool _IsNew;
        public bool IsNew
        {
            get { return _IsNew; }
            set
            {
                _IsNew = value;
                this.OnPropertyChanged_ListableProperty(nameof(IsNew), IsNew_Changed);
            }
        }
        #endregion

        #region IsSelcted
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                this.OnPropertyChanged_ListableProperty(nameof(IsSelected), IsSelected_Changed);
            }
        }
        #endregion

        #region IsDeleted
        private bool _IsDeleted;
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                _IsDeleted = value;
                this.OnPropertyChanged_ListableProperty(nameof(IsDeleted), IsDeleted_Changed);
            }
        }
        #endregion

        #region CanDelete
        private DelegateConverter<Func<bool>, bool> _CanDelete = true;
        public DelegateConverter<Func<bool>, bool> CanDelete
        {
            get { return _CanDelete; }
            set
            {
                _CanDelete = value;
                this.OnPropertyChanged(nameof(CanDelete));
            }
        }
        #endregion

        #region Sort
        private ItemField<int> _Sort;
        public ItemField<int> Sort
        {
            get
            {
                if (_Sort == null)
                {
                    _Sort = 0;
                    _Sort.OnSet += (s, e) => this.OnPropertyChanged_ListableProperty(nameof(Sort), Sort_Changed);
                }
                return _Sort;
            }
        }
        #endregion

        #region Events
        public event EventHandler IsNew_Changed;
        public event EventHandler IsSelected_Changed;
        public event EventHandler IsDeleted_Changed;
        public event EventHandler Sort_Changed;
        #endregion

        #endregion

        #region Model
        public T Model { get; private set; }
        object IModelAdaptor.Model
        {
            get { return Model; }
        }

        private ItemCollection<PropertyInfo> _ModelProperties;
        public ItemCollection<PropertyInfo> ModelProperties
        {
            get
            {
                if (_ModelProperties == null)
                    _ModelProperties = Model.GetType().GetProperties().ToList();
                return _ModelProperties;
            }
        }

        public ItemCollection<string> ActiveModelProperties { get; } = new List<string>();
        public ItemCollection<string> ExcludeModelProperties { get; } = new List<string>();


        private ItemCollection<PropertyInfo> _ModelAdaptorProperties;
        public ItemCollection<PropertyInfo> ModelAdaptorProperties
        {
            get
            {
                if (_ModelAdaptorProperties == null)
                    _ModelAdaptorProperties = new ItemCollection<PropertyInfo>(this.GetType().GetProperties());
                return _ModelAdaptorProperties;
            }
        }
        #endregion

        #region Revertable

        #region Original
        public ItemField<T> Original { get; } = new T();

        ItemField<object> IModelAdaptor.Original
        {
            get { return Original.Item; }
        }
        #endregion

        #region DuringSetAsOriginal
        private ItemField<bool> _DuringSetAsOriginal;
        public ItemField<bool> DuringSetAsOriginal
        {
            get
            {
                if (_DuringSetAsOriginal == null)
                {
                    _DuringSetAsOriginal = false;
                    _DuringSetAsOriginal.OnSet += (s, e) => this.OnPropertyChanged(nameof(DuringSetAsOriginal));
                }
                return _DuringSetAsOriginal;
            }
        }
        #endregion

        #region DuringRevert
        private ItemField<bool> _DuringRevert;
        public ItemField<bool> DuringRevert
        {
            get
            {
                if (_DuringRevert == null)
                {
                    _DuringRevert = false;
                    _DuringRevert.OnSet += (s, e) => this.OnPropertyChanged(nameof(DuringRevert));
                }
                return _DuringRevert;
            }
        }
        #endregion

        #region SetAsOriginal_Command
        private Command _SetAsOriginal_Command;
        public ICommand SetAsOriginal_Command
        {
            get
            {
                if (_SetAsOriginal_Command == null)
                    _SetAsOriginal_Command = new Command(this.SetAsOriginal);
                return _SetAsOriginal_Command;
            }
        }
        #endregion

        #region Revert_Command
        private Command _Revert_Command;
        public ICommand Revert_Command
        {
            get
            {
                if (_Revert_Command == null)
                    _Revert_Command = new Command(this.Revert);
                return _Revert_Command;
            }
        }
        #endregion

        #endregion

        #region State
        public bool IsEdited
        {
            get
            {
                return this.GetEditableProperties().Where(x => ActiveModelProperties.Contains(x.Name)).Any(x => this.Property(x.Name).IsEdited);
            }
        }

        public bool IsValid
        {
            get
            {
                return this.GetEditableProperties().Where(x => ActiveModelProperties.Contains(x.Name)).All(x => this.Property(x.Name).IsValid);
            }
        }

        public event EventHandler<PropertyChangeEventArgs> StateModified;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

