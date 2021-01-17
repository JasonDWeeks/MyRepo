using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.ModelTools.Common.Internal
{
    static class CollectionExtensions
    {
        public static void RemoveAll<T>(this ObservableCollection<T> list, Predicate<T> predicate)
        {
            var temp = list.ToList();
            foreach (var item in temp)
            {
                if (predicate(item))
                    list.Remove(item);
            }
        }
    }
}
