
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ModelClient
{
    [Serializable]
    public class UniqueItemList<T1, T2> : IList<UniqueKeyValuePair<T1, T2>> where T1 : class where T2 : class
    {
        #region IList<T>
        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public UniqueKeyValuePair<T1, T2> this[int index] { get => Items[index]; set => Items[index] = value; }

        public int IndexOf(UniqueKeyValuePair<T1, T2> item) => Items.IndexOf(item);

        public void RemoveAt(int index) => Items.RemoveAt(index);

        public void Clear() => Items.Clear();

        public void CopyTo(UniqueKeyValuePair<T1, T2>[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

        public bool Remove(UniqueKeyValuePair<T1, T2> item)
        {
            if (!Items.Contains(item)) return false;
            Items.Remove(item);
            return true;
        }

        public IEnumerator<UniqueKeyValuePair<T1, T2>> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
        #endregion

        public List<UniqueKeyValuePair<T1, T2>> Items { get; set; } = new List<UniqueKeyValuePair<T1, T2>>();
        public CompareMode Mode { get; set; } = CompareMode.OnlyKeys;

        public void Insert(int index, UniqueKeyValuePair<T1, T2> item)
        {
            if (!Items.Contains(item))
            {
                Items.Insert(index, item);
                item.Compare = Mode;
            }
            else
                throw new ItemAlreadyExistsException();
        }

        public void Add(UniqueKeyValuePair<T1, T2> item)
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
                item.Compare = Mode;
            }
            else
                throw new ItemAlreadyExistsException();
        }

        public bool Contains(UniqueKeyValuePair<T1, T2> item) => Items.Any(p => p.CompareTo(item) == 0);

        public class ItemAlreadyExistsException : Exception
        {
            private const string _message = "List already contains an element with the same key.";

            public ItemAlreadyExistsException(Exception? innerException = null) : base(_message, innerException)
            {
            }
        }
    }
}
