using Common.ModelTools.Common.Internal;
using Common.ModelTools.ModelAdaptor.Hierarchy.Internal;
using Common.ModelTools.ModelAdaptor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Hierarchy
{
    public static partial class ModelAdaptorChildren_Ext_Crud
    {
        #region New
        public static void New(this IModelAdaptorChildren source)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            var newItem = adaptorChildren.AddItem(null, true, null);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Selected = newItem;
            newItem.OnPropertyChanged_IsEdited_IsValid();

            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnNew_Raise();
        }
        #endregion

        #region Add
        public static void Add(this IModelAdaptorChildren source, IEnumerable<IModelAdaptorHierarchy> items, int? index = null)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            var newItems = adaptorChildren.AddItems(items, index);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Selected = newItems.FirstOrDefault();
            newItems.ToList().ForEach(x => x.OnPropertyChanged_IsEdited_IsValid());

            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnAdd_Raise();
        }

        public static void Add(this IModelAdaptorChildren source, IModelAdaptorHierarchy item, int? index = null)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            var newItem = adaptorChildren.AddItem(item, false, index);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Selected = newItem;
            newItem.OnPropertyChanged_IsEdited_IsValid();

            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnAdd_Raise();
        }
        #endregion

        #region Save
        public static void Save(this IModelAdaptorChildren source, bool hardDelete = false)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            adaptorChildren.CheckLoaded()?.SaveItems(hardDelete);

            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnSave_Raise();
        }
        #endregion

        #region Delete
        public static void Delete(this IModelAdaptorChildren source)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            adaptorChildren.CheckLoaded()?.DeleteItem(null, true);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnDelete_Raise();
        }

        public static void Delete(this IModelAdaptorChildren source, IModelAdaptor item)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            adaptorChildren.CheckLoaded()?.DeleteItem(item);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnDelete_Raise();
        }

        public static void Delete(this IModelAdaptorChildren source, IEnumerable<IModelAdaptor> items)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            adaptorChildren.CheckLoaded()?.DeleteItems(items);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnDelete_Raise();
        }
        #endregion

        #region DeleteAll
        public static void DeleteAll(this IModelAdaptorChildren source)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            adaptorChildren.CheckLoaded()?.DeleteItems(adaptorChildren.List);

            adaptorChildren.ResetDisplay();
            adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
            adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));

            adaptorChildren.OnDeleteAll_Raise();
        }
        #endregion

        #region RemoveDeletedItems
        public static void RemoveDeletedItems(this IModelAdaptorChildren source)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));

            if (adaptorChildren.CheckLoaded()?.List.Any(x => x.IsDeleted) ?? false)
            {
                adaptorChildren.CheckLoaded()?.ListRemoveAll_ReSort(x => x.IsDeleted);

                adaptorChildren.ResetDisplay();
                adaptorChildren.Parent?.Adaptor?.Property(adaptorChildren.PropertyName).ForceNotify();
                adaptorChildren.OnPropertyChanged(nameof(adaptorChildren.Display_Any));
            }
        }
        #endregion

        #region Move
        public static void Move(this IModelAdaptorChildren source, IModelAdaptorChildren destination, IModelAdaptor item, int? index = null)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));
            var adaptorChildren_dest = destination ?? throw new ArgumentNullException(nameof(destination));

            if (source == destination)
                return;
            
            if (adaptorChildren.List.Contains(item))
            {
                adaptorChildren.Delete(item);
                adaptorChildren_dest.Add(item.Model as IModelAdaptorHierarchy, index);
            }
        }

        public static void Move(this IModelAdaptorChildren source, IModelAdaptorChildren destination, IEnumerable<IModelAdaptor> items, int? index = null)
        {
            var adaptorChildren = source ?? throw new ArgumentNullException(nameof(source));
            var adaptorChildren_dest = destination ?? throw new ArgumentNullException(nameof(destination));

            if (source == destination)
                return;

            var items_toMove = items.Where(x => adaptorChildren.List.Contains(x)).ToList();

            adaptorChildren.Delete(items_toMove);
            adaptorChildren_dest.Add(items_toMove.Select(x => x.Model as IModelAdaptorHierarchy), index);
        }
        #endregion
    }
}
