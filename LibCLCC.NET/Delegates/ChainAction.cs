using LibCLCC.NET.Collections;
using System;

namespace LibCLCC.NET.Delegates
{
    public class ChainAction : ConnectableList<Action>
    {
        public void Invoke()
        {
            foreach (var action in this)
            {
                action();
            }
        }
        public static ChainAction operator +(ChainAction e, Action a)
        {
            e.Add(a);
            return e;
        }
    }
    public class ChainAction<T> : ConnectableList<Action<T>>
    {
        public void Invoke(T t)
        {
            foreach (var action in this)
            {
                action(t);
            }
        }
        public static ChainAction<T> operator +(ChainAction<T> e, Action<T> a)
        {
            e.Add(a);
            return e;
        }
    }
    public class ChainAction<T, U> : ConnectableList<Action<T, U>>
    {
        public void Invoke(T t, U u)
        {
            foreach (var action in this)
            {
                action(t, u);
            }
        }
        public static ChainAction<T, U> operator +(ChainAction<T, U> e, Action<T, U> a)
        {
            e.Add(a);
            return e;
        }
    }
    public class ChainAction<T, U, V> : ConnectableList<Action<T, U, V>>
    {
        public void Invoke(T t, U u, V v)
        {
            foreach (var action in this)
            {
                action(t, u, v);
            }
        }
        public static ChainAction<T, U, V> operator +(ChainAction<T, U, V> e, Action<T, U, V> a)
        {
            e.Add(a);
            return e;
        }
    }
}
