using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.ModelTools.ModelAdaptor
{
    public class ItemDictionary<TKey, TValue>
    {
        public ItemDictionary(Dictionary<TKey, TValue> items)
        {
            _Items = items;
        }

        internal Dictionary<TKey, TValue> _Items;

        public ReadOnlyDictionary<TKey, TValue> Items
        {
            get
            {
                return new ReadOnlyDictionary<TKey, TValue>(_Items);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _Items[key];
            }
            internal set
            {
                _Items[key] = value;
            }
        }

        public static implicit operator ItemDictionary<TKey, TValue>(Dictionary<TKey, TValue> value)
        {
            return new ItemDictionary<TKey, TValue>(value);
        }
    }

    public static class ItemDictionary
    {
        #region public
        public static bool TryGetValue<TKey, TValue>(this ItemDictionary<TKey, TValue> itemDictionary, TKey key, out TValue value)
        {
            return itemDictionary._Items.TryGetValue(key, out value);
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> Where<TKey, TValue>(this ItemDictionary<TKey, TValue> itemDictionary, Func<KeyValuePair<TKey, TValue>, bool> whereClause)
        {
            return itemDictionary._Items.Where(whereClause);
        }

        public static List<KeyValuePair<TKey, TValue>> ToList<TKey, TValue>(this ItemDictionary<TKey, TValue> itemDictionary)
        {
            return itemDictionary._Items.ToList();
        }
        #endregion

        #region internal
        internal static void Remove<TKey, TValue>(this ItemDictionary<TKey, TValue> itemDictionary, TKey key)
        {
            itemDictionary._Items.Remove(key);
        }

        internal static void Clear<TKey, TValue>(this ItemDictionary<TKey, TValue> itemDictionary)
        {
            itemDictionary._Items.Clear();
        }

        internal static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this ItemDictionary<TKey, TValue> itemDictionary)
        {
            return itemDictionary?._Items;
        }
        #endregion
    }
}
