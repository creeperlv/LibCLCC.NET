using LibCLCC.NET.Collections;
using System;

namespace LibCLCC.NET.Delegates {
    /// <summary>
    /// As name says, chain action.
    /// </summary>
    public class ChainAction : ConnectableList<Action> {
        /// <summary>
        /// Invoke all actions on the chain.
        /// </summary>
        public void Invoke() {
            foreach (var action in this) {
                action();
            }
        }
        /// <summary>
        /// Add an action to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ChainAction operator +(ChainAction e, Action a) {
            e.Add(a);
            return e;
        }
    }
    /// <summary>
    /// As name says, chain action.
    /// </summary>
    public class ChainAction<T> : ConnectableList<Action<T>> {
        /// <summary>
        /// Invoke all actions on the chain.
        /// </summary>
        /// <param name="t"></param>
        public void Invoke(T t) {
            foreach (var action in this) {
                action(t);
            }
        }
        /// <summary>
        /// Add an action to the chain.
        /// /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ChainAction<T> operator +(ChainAction<T> e, Action<T> a) {
            e.Add(a);
            return e;
        }
    }
    /// <summary>
    /// As name says, chain action.
    /// </summary>
    public class ChainAction<T, U> : ConnectableList<Action<T, U>> {
        /// <summary>
        /// Invoke all actions on the chain.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="u"></param>
        public void Invoke(T t, U u) {
            foreach (var action in this) {
                action(t, u);
            }
        }
        /// <summary>
        /// Add an action to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ChainAction<T, U> operator +(ChainAction<T, U> e, Action<T, U> a) {
            e.Add(a);
            return e;
        }
    }
    /// <summary>
    /// As name says, chain action.
    /// </summary>
    public class ChainAction<T, U, V> : ConnectableList<Action<T, U, V>> {
        /// <summary>
        /// Invoke all actions on the chain.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        public void Invoke(T t, U u, V v) {
            foreach (var action in this) {
                action(t, u, v);
            }
        }
        /// <summary>
        /// Add an action to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ChainAction<T, U, V> operator +(ChainAction<T, U, V> e, Action<T, U, V> a) {
            e.Add(a);
            return e;
        }
    }
}
