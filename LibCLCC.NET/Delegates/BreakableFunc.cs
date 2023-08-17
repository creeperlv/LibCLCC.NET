using LibCLCC.NET.Collections;
using System;

namespace LibCLCC.NET.Delegates
{
    /// <summary>
    /// If a func returns true will break the func chain.
    /// </summary>
    public class BreakableFunc : ConnectableList<Func<bool>>
    {
        /// <summary>
        /// Invoke all functions on the chain.
        /// </summary>
        /// <returns>If the chain was broken.</returns>
        public bool Invoke()
        {
            foreach (var item in this)
            {
                if (item.Invoke())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add a function to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BreakableFunc operator +(BreakableFunc e, Func<bool> a)
        {
            e.Add(a);
            return e;
        }
    }
    /// <summary>
    /// if a func returns true will break the func chain.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BreakableFunc<T> : ConnectableList<Func<T, bool>>
    {
        /// <summary>
        /// Invoke all functions on the chain.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>If the chain was broken.</returns>
        public bool Invoke(T obj)
        {
            foreach (var item in this)
            {
                if (item.Invoke(obj))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add a function to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BreakableFunc<T> operator +(BreakableFunc<T> e, Func<T, bool> a)
        {
            e.Add(a);
            return e;
        }
    }
    /// <summary>
    /// if a func returns true will break the func chain.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BreakableFunc<T, U> : ConnectableList<Func<T, U, bool>>
    {
        /// <summary>
        /// Invoke all functions on the chain.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <returns>If the chain was broken.</returns>
        public bool Invoke(T t, U u)
        {
            foreach (var item in this)
            {
                if (item.Invoke(t, u))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add a function to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BreakableFunc<T, U> operator +(BreakableFunc<T, U> e, Func<T, U, bool> a)
        {
            e.Add(a);
            return e;
        }
    }
    /// <summary>
    /// if a func returns true will break the func chain.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class BreakableFunc<T, U, V> : ConnectableList<Func<T, U, V, bool>>
    {
        /// <summary>
        /// Invoke all functions on the chain.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns>If the chain was broken.</returns>
        public bool Invoke(T t, U u, V v)
        {
            foreach (var item in this)
            {
                if (item.Invoke(t, u, v))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add a function to the chain.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BreakableFunc<T, U, V> operator +(BreakableFunc<T, U, V> e, Func<T, U, V, bool> a)
        {
            e.Add(a);
            return e;
        }
    }
}
