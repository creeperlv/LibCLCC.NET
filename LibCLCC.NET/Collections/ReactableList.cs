using LibCLCC.NET.Delegates;
using System;
using System.Collections.Generic;

namespace LibCLCC.NET.Collections {
    /// <summary>
    /// Cause reaction when modifying the list. (Currently only when adding/removing)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ReactableList<T> : List<T> {
        /// <summary>
        /// Empty initializer for serialization/deserialization purpose.
        /// </summary>
        public ReactableList() { }
        /// <summary>
        /// ReactChain for add operations.
        /// </summary>
        public BreakableFunc<ReactableList<T>, T> ReactChain = new BreakableFunc<ReactableList<T>, T>();
        /// <summary>
        /// ReactChain for removal operations.
        /// </summary>
        public BreakableFunc<ReactableList<T>, T> RemoveReactChain = new BreakableFunc<ReactableList<T>, T>();
        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="t"></param>
        public new void Add(T t) {
            base.Add(t);
            ReactChain.Invoke(this, t);
        }
        /// <summary>
        /// Remove an item.
        /// </summary>
        /// <param name="t"></param>
        public new void Remove(T t) {
            base.Remove(t);
            RemoveReactChain.Invoke(this, t);
        }
        /// <summary>
        /// Remove an item with given index.
        /// </summary>
        /// <param name="index"></param>
        public new void RemoveAt(int index) {
            T t = base[index];
            base.RemoveAt(index);
            RemoveReactChain.Invoke(this, t);
        }
    }
}
