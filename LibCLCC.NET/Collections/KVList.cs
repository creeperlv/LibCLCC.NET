using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LibCLCC.NET.Collections
{
    /// <summary>
    /// Improved version of KVList from Site13Kernel.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class KVList<K, V> : List<KVPair<K, V>>
    {
        /// <summary>
        /// Convert the KVList to a dictionary.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<K, V> ToDictionary()
        {
            Dictionary<K, V> __RESULT = new Dictionary<K, V>();
            foreach (var item in this)
            {
                __RESULT.Add(item.Key, item.Value);
            }
            return __RESULT;
        }
        /// <summary>
        /// Convert the KVList to a dictionary with a custom processor that processes Keys.
        /// </summary>
        /// <param name="K_Process"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<K, V> ToDictionary(Func<K, K> K_Process)
        {
            Dictionary<K, V> __RESULT = new Dictionary<K, V>();
            foreach (var item in this)
            {
                __RESULT.Add(K_Process(item.Key), item.Value);
            }
            return __RESULT;
        }
        /// <summary>
        /// Convert the KVList to a dictionary with a custom processor that processes Values.
        /// </summary>
        /// <param name="V_Process"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<K, V> ToDictionary(Func<V, V> V_Process)
        {
            Dictionary<K, V> __RESULT = new Dictionary<K, V>();
            foreach (var item in this)
            {
                __RESULT.Add((item.Key), V_Process(item.Value));
            }
            return __RESULT;
        }
        /// <summary>
        /// Convert the KVList to a dictionary with a custom processor that processes a k-v pair.
        /// </summary>
        /// <param name="K_Process"></param>
        /// <param name="V_Process"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<K, V> ToDictionary(Func<K,K> K_Process, Func<V, V> V_Process)
        {
            Dictionary<K, V> __RESULT = new Dictionary<K, V>();
            foreach (var item in this)
            {
                __RESULT.Add(K_Process(item.Key), V_Process(item.Value));
            }
            return __RESULT;
        }
        /// <summary>
        /// Add a key and value pair.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key, V value)
        {
            Add(new KVPair<K, V>(key, value));
        }
        /// <summary>
        /// Add the given KVPair.
        /// </summary>
        /// <param name="pair"></param>
        public new void Add(KVPair<K,V> pair) {
            base.Add(pair);
        }
    }
}
