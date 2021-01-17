using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.ModelTools.ModelAdaptor
{
    public class ItemField<T> : IComparable<ItemField<T>>
    {
        public ItemField(T item)
        {
            _Item = item;
        }

        internal T _Item = default(T);
        public T Item
        {
            get
            {
                return _Item;
            }
            internal set
            {
                _Item = value;
                OnSet?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetItemNoNotify(T value)
        {
            _Item = value;
        }


        public static implicit operator T(ItemField<T> itemField)
        {
            return itemField.Item;
        }

        public static implicit operator ItemField<T>(T value)
        {
            return new ItemField<T>(value);
        }

        internal event EventHandler OnSet;

        public int CompareTo(ItemField<T> other)
        {
            if (other == null)
                return 1;

            return (Item as IComparable<T>)?.CompareTo(other.Item) ?? 0;
        }
    }

    static class ItemField
    {
        public static int Increment(this ItemField<int> itemField)
        {
            return Interlocked.Increment(ref itemField._Item);
        }

        public static int Decrement(this ItemField<int> itemField)
        {
            return Interlocked.Decrement(ref itemField._Item);
        }
    }
}
