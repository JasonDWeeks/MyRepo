using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Hierarchy.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Common.ModelTools.ModelAdaptor.Hierarchy
{
    public partial class ModelAdaptorChildren<TChild> : IModelAdaptorChildren
        where TChild : IModelAdaptorHierarchy
    {
        #region PropertyName
        private string _PropertyName;
        public string PropertyName 
        { 
            get
            {
                return _PropertyName;
            }
            set
            {
                _PropertyName = value;
                this.OnPropertyChanged(nameof(PropertyName));
            }
        }
        #endregion

        #region Parent
        public IModelAdaptorHierarchy Parent { get; set; }
        #endregion

        #region Check
        private ItemField<bool> _Loaded;
        public ItemField<bool> Loaded
        {
            get
            {
                if (_Loaded == null)
                {
                    _Loaded = false;
                    _Loaded.OnSet += (s, e) => this.OnPropertyChanged(nameof(Loaded));
                }
                return _Loaded;
            }
        }
        public ItemField<bool> Skip { get; } = false;
        public ItemField<bool> SkipLoad { get; } = false;

        public event EventHandler PreLoad;
        public event EventHandler PostLoad;
        #endregion

        #region Crud

        #region New_Command
        private Command _New_Command;
        public ICommand New_Command
        {
            get
            {
                if (_New_Command == null)
                    _New_Command = new Command(this.New);
                return _New_Command;
            }
        }
        #endregion

        #region Save_Command
        private Command _Save_Command;
        public ICommand Save_Command
        {
            get
            {
                if (_Save_Command == null)
                    _Save_Command = new Command(param =>
                    {
                        if (param == null)
                            this.Save();
                        else if (param is bool)
                            this.Save((bool)param);
                    });
                return _Save_Command;
            }
        }
        #endregion

        #region Delete_Command
        private Command _Delete_Command;
        public ICommand Delete_Command
        {
            get
            {
                if (_Delete_Command == null)
                    _Delete_Command = new Command(this.Delete);
                return _Delete_Command;
            }
        }
        #endregion

        #region DeleteAll_Command
        private Command _DeleteAll_Command;
        public ICommand DeleteAll_Command
        {
            get
            {
                if (_DeleteAll_Command == null)
                    _DeleteAll_Command = new Command(this.DeleteAll);
                return _DeleteAll_Command;
            }
        }
        #endregion

        #region Events
        public event EventHandler OnNew;
        public event EventHandler OnAdd;
        public event EventHandler OnSave;
        public event EventHandler OnDelete;
        public event EventHandler OnDeleteAll;
        #endregion

        #endregion

        #region Initialization

        #region GetChildren
        public Func<IEnumerable<IModelAdaptorHierarchy>> GetChildren { get; set; }
        #endregion

        #region InitializeNewChild
        public Action<TChild> InitializeNewChild { get; set; }
        Action<IModelAdaptorHierarchy> IModelAdaptorChildren.InitializeNewChild
        {
            get
            {
                return ch => InitializeNewChild?.Invoke((TChild)ch);
            }
            set
            {
                InitializeNewChild = ch => value?.Invoke(ch);
            }
        }
        #endregion

        #region InitializeChild
        public Action<TChild> InitializeChild { get; set; }
        Action<IModelAdaptorHierarchy> IModelAdaptorChildren.InitializeChild
        {
            get
            {
                return ch => InitializeChild?.Invoke((TChild)ch);
            }
            set
            {
                InitializeChild = ch => value?.Invoke(ch);
            }
        }
        #endregion

        #region ForEachChildBeforeSave
        public Action<TChild> ForEachChildBeforeSave { get; set; }
        Action<IModelAdaptorHierarchy> IModelAdaptorChildren.ForEachChildBeforeSave
        {
            get
            {
                return ch => ForEachChildBeforeSave?.Invoke((TChild)ch);
            }
            set
            {
                ForEachChildBeforeSave = ch => value?.Invoke(ch);
            }
        }
        #endregion

        #region CreateNewChld
        public IModelAdaptorHierarchy CreateNewChld()
        {
            var type = typeof(TChild);
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                if (type.IsValueType)
                {
                    return new object() as IModelAdaptorHierarchy;
                }
                return null;
            }

            return Activator.CreateInstance(type) as IModelAdaptorHierarchy;
        }
        #endregion

        #endregion

        #region List

        #region List
        private List<IModelAdaptor> _List;
        public List<IModelAdaptor> List
        {
            get
            {
                if (_List == null)
                {
                    _List = this.PopulateList();

                    // force display to load
                    var d = Display;
                }
                return _List;
            }
        }
        #endregion

        #region Display
        private ObservableCollection<dynamic> _Display;
        public ObservableCollection<dynamic> Display
        {
            get
            {
                if (_Display == null || !_Display.Any())
                    _Display = new ObservableCollection<dynamic>(List.Where(x => !x.IsDeleted).OrderBy(x => x.Sort).Select(x => x as dynamic));
                return _Display;
            }
        }
        #endregion

        #region Display_Casted
        public List<IModelAdaptor> Display_Casted
        {
            get
            {
                return Display.Select(x => (IModelAdaptor)x).ToList();
            }
        }

        public List<TChild> Display_Casted_Models
        {
            get
            {
                return Display.Select(x => (TChild)((IModelAdaptor)x).Model).ToList();
            }
        }
        #endregion

        #region Display_Any
        public bool Display_Any => Display.Any();
        #endregion

        #region Selected
        private IModelAdaptor _Selected;
        public dynamic Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = (IModelAdaptor)value;
                this.OnPropertyChanged(nameof(Selected));
                this.OnPropertyChanged(nameof(ItemSelected));
                OnSelect?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Selected_Casted
        public IModelAdaptor Selected_Casted
        {
            get
            {
                return _Selected;
            }
        }

        public TChild Selected_Casted_Model
        {
            get
            {
                if (Selected == null)
                    return default(TChild);
                return (TChild)_Selected?.Model;
            }
        }
        #endregion

        #region ItemSelected
        public bool ItemSelected => Selected != null;
        #endregion

        #region Events
        public event EventHandler OnSelect;
        #endregion

        #endregion

        #region Revert
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
        public event EventHandler OnRevert;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
