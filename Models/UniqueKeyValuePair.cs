
using System;

namespace ModelClient
{
    [Serializable]
    public class UniqueKeyValuePair<T1, T2> : IComparable where T1 : class where T2 : class
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }
        public CompareMode Compare { get; set; } = CompareMode.OnlyKeys;

        public UniqueKeyValuePair()
        {
        }

        public UniqueKeyValuePair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public int CompareTo(object obj) =>
            obj is UniqueKeyValuePair<T1, T2> pair
                ? (pair.Key?.Equals(Key) ?? Key == null) && (Compare == CompareMode.OnlyKeys || (pair.Value?.Equals(Value) ?? Value == null))
                    ? 0
                    : 1
                : 1;
    }
}
