using LibCLCC.NET.Delegates;
using System;
using System.Collections.Generic;

namespace LibCLCC.NET.Collections
{
    [Serializable]
    public class ReactableList<T> : List<T>
    {
        public ReactableList() { }
        public BreakableFunc<ReactableList<T>, T> ReactChain = new BreakableFunc<ReactableList<T>, T>();
        public new void Add(T t)
        {
            base.Add(t);
            ReactChain.Invoke(this, t);
        }
    }
}
