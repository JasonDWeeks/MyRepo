using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Common.ModelTools.ModelAdaptor.Hierarchy
{
    public interface IModelAdaptorChildren : INotifyPropertyChanged
    {
        string PropertyName { get; set; }
        IModelAdaptorHierarchy Parent { get; set; }

        #region Check
        ItemField<bool> Loaded { get; }
        ItemField<bool> Skip { get; }
        ItemField<bool> SkipLoad { get; }

        event EventHandler PreLoad;
        event EventHandler PostLoad;
        #endregion

        #region CRUD
        ICommand New_Command { get; }
        ICommand Save_Command { get; }
        ICommand Delete_Command { get; }
        ICommand DeleteAll_Command { get; }

        event EventHandler OnNew;
        event EventHandler OnAdd;
        event EventHandler OnSave;
        event EventHandler OnDelete;
        event EventHandler OnDeleteAll;
        #endregion

        #region Initialization
        Func<IEnumerable<IModelAdaptorHierarchy>> GetChildren { get; set; }
        Action<IModelAdaptorHierarchy> InitializeNewChild { get; set; }
        Action<IModelAdaptorHierarchy> InitializeChild { get; set; }
        Action<IModelAdaptorHierarchy> ForEachChildBeforeSave { get; set; }

        IModelAdaptorHierarchy CreateNewChld();
        #endregion

        #region List
        List<IModelAdaptor> List { get; }
        ObservableCollection<dynamic> Display { get; }
        List<IModelAdaptor> Display_Casted { get; }
        bool Display_Any { get; }
        dynamic Selected { get; set; }
        IModelAdaptor Selected_Casted { get; }
        bool ItemSelected { get; }

        event EventHandler OnSelect;
        #endregion

        #region Revert
        ICommand Revert_Command { get; }

        event EventHandler OnRevert;
        #endregion
    }
}
