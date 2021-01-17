using Common.ModelTools.Common.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Common.ModelTools.ModelAdaptor
{
    public sealed class ItemCollection<T>
    {
        public ItemCollection(IEnumerable<T> items)
        {
            _Items = items?.ToList();
        }

        internal List<T> _Items;
        
        public ReadOnlyCollection<T> Items
        {
            get
            {
                return new ReadOnlyCollection<T>(_Items);
            }
        }

        public static implicit operator ItemCollection<T>(List<T> value)
        {
            return new ItemCollection<T>(value);
        }
    }

    public static class ItemCollection
    {
        #region public
        public static bool Contains<T>(this ItemCollection<T> collection, T value)
        {
            return collection._Items.Contains(value);
        }

        public static bool Any<T>(this ItemCollection<T> collection)
        {
            return collection._Items.Any();
        }

        public static bool Any<T>(this ItemCollection<T> collection, Func<T, bool> whereClause)
        {
            return collection._Items.Any(whereClause);
        }

        public static IEnumerable<T> Where<T>(this ItemCollection<T> collection, Func<T, bool> whereClause)
        {
            return collection._Items.Where(whereClause);
        }

        public static IEnumerable<TResult> Select<T, TResult>(this ItemCollection<T> collection, Func<T, TResult> selector)
        {
            return collection._Items.Select(selector);
        }

        public static T FirstOrDefault<T>(this ItemCollection<T> collection)
        {
            return collection._Items.FirstOrDefault();
        }

        public static T FirstOrDefault<T>(this ItemCollection<T> collection, Func<T, bool> whereClause)
        {
            return collection._Items.FirstOrDefault(whereClause);
        }

        public static T[] ToArray<T>(this ItemCollection<T> collection)
        {
            return collection._Items.ToArray();
        }

        public static List<T> ToList<T>(this ItemCollection<T> collection)
        {
            return collection._Items.ToList();
        }

        public static void ForEach<T>(this ItemCollection<T> collection, Action<T> action)
        {
            collection._Items.ToList().ForEach(action);
        }
        #endregion

        #region Internal
        internal static PropertyInfo GetByName(this ItemCollection<PropertyInfo> collection, string propertyName)
        {
            return collection._Items.GetByName(propertyName);
        }

        internal static void Add<T>(this ItemCollection<T> collection, T item)
        {
            collection._Items.Add(item);
        }

        internal static void Remove<T>(this ItemCollection<T> collection, T item)
        {
            collection._Items.Remove(item);
        }

        internal static void RemoveAll<T>(this ItemCollection<T> collection, Predicate<T> match)
        {
            collection._Items.RemoveAll(match);
        }

        internal static void Clear<T>(this ItemCollection<T> collection)
        {
            collection._Items.Clear();
        }
        #endregion
    }
}
