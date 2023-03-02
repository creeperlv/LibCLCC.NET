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
        /// <summary>
        /// The Key
        /// </summary>
        public K Key;
        /// <summary>
        /// The Value
        /// </summary>
        public V Value;
        /// <summary>
        /// Empty CTOR that do nothing for serialization/deserialization purpose.
        /// </summary>
        public KVPair() { }
        /// <summary>
        /// Initialize a KVPair.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public KVPair(K key, V value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// Judge by comparing Key.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(KVPair<K, V> x, KVPair<K, V> y)
        {
            return x.Key.Equals(y.Key);
        }
        /// <summary>
        /// Judge by comparing Key.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(KVPair<K, V> other)
        {
            return other.Key.Equals(Key);
        }
        /// <summary>
        /// Return the hash code of the key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(KVPair<K, V> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
