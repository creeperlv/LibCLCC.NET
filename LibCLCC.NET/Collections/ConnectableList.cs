using System;
using System.Collections.Generic;
using System.Text;

namespace LibCLCC.NET.Collections
{
    [Serializable]
    public class ConnectableList<T> : List<T>
    {
        public void ConnectAfterEnd(List<T> __l)
        {
            foreach (var item in __l)
            {
                this.Add(item);
            }
        }
        public void ConnectAfterEnd(ConnectableList<T> __l)
        {
            foreach (var item in __l)
            {
                this.Add(item);
            }
        }
        public void ConnectBeforeStart(List<T> __l)
        {
            this.InsertRange(0, __l);
        }
        public void ConnectBeforeStart(ConnectableList<T> __l)
        {
            this.InsertRange(0, __l);
        }
    }
}
