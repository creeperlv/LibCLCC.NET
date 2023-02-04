using System;
using System.Collections.Generic;

namespace LibCLCC.NET.Collections
{
    /// <summary>
    /// Helpful when working with Unity3D.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class KVPair<K, V> : IEqualityComparer<KVPair<K, V>>, IEquatable<KVPair<K, V>>
    {
        public K Key;
        public V Value;
        public KVPair() { }

        public KVPair(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public bool Equals(KVPair<K, V> x, KVPair<K, V> y)
        {
            int a;
            return x.Key.Equals(y.Key);
        }

        public bool Equals(KVPair<K, V> other)
        {
            return other.Key.Equals(Key);
        }

        public int GetHashCode(KVPair<K, V> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
