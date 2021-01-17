using Common.ModelTools.Common.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor.Hierarchy.Internal
{
    static partial class ModelAdaptorChildren_Ext_Crud
    {
        #region AddItems
        public static IEnumerable<IModelAdaptor> AddItems(this IModelAdaptorChildren modelAdaptorChildren, IEnumerable<IModelAdaptorHierarchy> items, int? index)
        {
            var ret = new List<IModelAdaptor>();
            items?.ToList().ForEach(item =>
            {
                var newItem = modelAdaptorChildren.AddItem(item, false, index);
                if (newItem != null)
                {
                    index = newItem.Sort + 1;
                    ret.Add(newItem);
                }
            });
            return ret;
        }
        #endregion

        #region AddItem
        public static IModelAdaptor AddItem(this IModelAdaptorChildren modelAdaptorChildren, IModelAdaptorHierarchy item, bool createIfNull, int? index)
        {
            IModelAdaptorHierarchy newItem = null;
            if (item == null && !createIfNull)
                return null;

            if (item == null)
            {
                newItem = modelAdaptorChildren.CreateNewChld();
                if (newItem == null)
                    return null;

                newItem.Parent = modelAdaptorChildren.Parent;
                modelAdaptorChildren.InitializeNewChild?.Invoke(newItem);
            }
            else
            {
                newItem = modelAdaptorChildren.CopyChild(item);
                if (newItem == null)
                    return null;

                newItem.Parent = modelAdaptorChildren.Parent;
                modelAdaptorChildren.InitializeNewChild?.Invoke(newItem);

                item.Adaptor.GetChildrenProperties().ToList().ForEach(childrenProp =>
                {
                    var oldChildren = item.Adaptor.Children(childrenProp.Name);
                    var newChildren = newItem?.Adaptor.Children(childrenProp.Name);

                    newChildren?.AddItems(oldChildren.Display.Select(x => (x as IModelAdaptor).Model as IModelAdaptorHierarchy), null);
                });
            }

            if (newItem != null)
            {
                newItem.Adaptor.IsNew = true;
                newItem.Adaptor.StateModified += (s, e) => modelAdaptorChildren.CheckSkip()?.Parent?.Adaptor?.Property(modelAdaptorChildren.PropertyName).ForceNotify();

                int sort = index ?? 0;
                int sortMax = modelAdaptorChildren.List.Select(x => x.Sort.Item).DefaultIfEmpty(0).Max();
                int sortMin = modelAdaptorChildren.List.Select(x => x.Sort.Item).DefaultIfEmpty(0).Min();

                if (!modelAdaptorChildren.List.Any())
                {
                    newItem.Adaptor.Sort.Item = 0;
                }
                else if (!index.HasValue)
                {
                    newItem.Adaptor.Sort.Item = sortMax + 1;
                }
                else
                {
                    if (sort > sortMax)
                        sort = sortMax + 1;

                    if (sort < sortMin)
                        sort = 0;

                    modelAdaptorChildren.List.Where(x => x.Sort >= sort).ToList().ForEach(x => x.Sort.Item = x.Sort + 1);
                }

                modelAdaptorChildren.ListAdd_ReSort(newItem.Adaptor);
            }

            return newItem?.Adaptor;
        }
        #endregion

        #region SaveItems
        public static void SaveItems(this IModelAdaptorChildren modelAdaptorChildren, bool hardDelete)
        {
            if (modelAdaptorChildren.ForEachChildBeforeSave != null)
                modelAdaptorChildren.List.Select(x => x.Model as IModelAdaptorHierarchy).ToList().ForEach(modelAdaptorChildren.ForEachChildBeforeSave);

            modelAdaptorChildren.SkipBlock(() =>
                modelAdaptorChildren.List.ForEach(x => (x.Model as IModelAdaptorHierarchy).Save())
            );

            if (hardDelete)
                modelAdaptorChildren.ListRemoveAll_ReSort(x => x.IsDeleted);
        }
        #endregion

        #region DeleteItems
        public static void DeleteItems(this IModelAdaptorChildren modelAdaptorChildren, IEnumerable<IModelAdaptor> items)
        {
            items?.Where(x => x.CanDelete).ToList().ForEach(item => modelAdaptorChildren.DeleteItem(item, false, true));
        }
        #endregion

        #region DeleteItem
        public static void DeleteItem(this IModelAdaptorChildren modelAdaptorChildren, IModelAdaptor item = null, bool deleteSelectedOnNull = false,  bool skipNotify = false)
        {
            if (item == null && !deleteSelectedOnNull)
                return;

            if (item == null)
                item = modelAdaptorChildren.Selected as IModelAdaptor;

            if (skipNotify)
                modelAdaptorChildren.SkipBlock(() => item.IsDeleted = true);
            else
                item.IsDeleted = true;

            if (item.IsNew)
                modelAdaptorChildren.ListRemove_ReSort(item);
        }
        #endregion

        #region RevertItems
        public static void RevertItems(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.ListRemoveAll_ReSort(x => x.IsNew);

            modelAdaptorChildren.SkipBlock(() =>
                modelAdaptorChildren.List.OrderBy(x => x.Sort).ToList().ForEach(x =>
                {
                    if (x.IsDeleted)
                        x.IsDeleted = false;

                    (x.Model as IModelAdaptorHierarchy).Revert();
                })
            );
        }
        #endregion

        #region ListAdd_ReSort
        public static void ListAdd_ReSort(this IModelAdaptorChildren modelAdaptorChildren, IModelAdaptor item)
        {
            modelAdaptorChildren.List.Add(item);
            int sort = 0;
            modelAdaptorChildren.List.OrderBy(x => x.Sort).ToList().ForEach(x => x.Sort.Item = sort++);
        }
        #endregion

        #region ListRemove_ReSort
        public static void ListRemove_ReSort(this IModelAdaptorChildren modelAdaptorChildren, IModelAdaptor item)
        {
            modelAdaptorChildren.List.Remove(item);
            int sort = 0;
            modelAdaptorChildren.List.OrderBy(x => x.Sort).ToList().ForEach(x => x.Sort.Item = sort++);
        }
        #endregion

        #region ListRemoveAll_ReSort
        public static void ListRemoveAll_ReSort(this IModelAdaptorChildren modelAdaptorChildren, Predicate<IModelAdaptor> predicate)
        {
            modelAdaptorChildren.List.RemoveAll(predicate);
            int sort = 0;
            modelAdaptorChildren.List.OrderBy(x => x.Sort).ToList().ForEach(x => x.Sort.Item = sort++);
        }
        #endregion

        #region OnNew_Raise
        public static void OnNew_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnNew));
        }
        #endregion

        #region OnAdd_Raise
        public static void OnAdd_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnAdd));
        }
        #endregion

        #region OnSave_Raise
        public static void OnSave_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnSave));
        }
        #endregion

        #region OnDelete_Raise
        public static void OnDelete_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnDelete));
        }
        #endregion

        #region OnDeleteAll_Raise
        public static void OnDeleteAll_Raise(this IModelAdaptorChildren modelAdaptorChildren)
        {
            modelAdaptorChildren.RaiseEvent(nameof(modelAdaptorChildren.OnDeleteAll));
        }
        #endregion
    }
}
