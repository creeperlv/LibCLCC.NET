using System;
using System.Collections.Generic;
using System.Text;

namespace LibCLCC.NET.Collections {
    /// <summary>
    /// Connectable List, an extension to list that can add an entire list in front of or after the current list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ConnectableList<T> : List<T> {
        /// <summary>
        /// Add the elements in __l after the end of current list.
        /// </summary>
        /// <param name="__l"></param>
        public void ConnectAfterEnd(List<T> __l) {
            foreach (var item in __l) {
                this.Add(item);
            }
        }
        /// <summary>
        /// Add the elements in __l after the end of current list.
        /// </summary>
        /// <param name="__l"></param>
        public void ConnectAfterEnd(ConnectableList<T> __l) {
            foreach (var item in __l) {
                this.Add(item);
            }
        }
        /// <summary>
        /// Add the elements in __l in front of the beginning of current list.
        /// </summary>
        /// <param name="__l"></param>
        public void ConnectBeforeStart(List<T> __l) {
            this.InsertRange(0, __l);
        }
        /// <summary>
        /// Add the elements in __l in front of the beginning of current list.
        /// </summary>
        /// <param name="__l"></param>
        public void ConnectBeforeStart(ConnectableList<T> __l) {
            this.InsertRange(0, __l);
        }
        /// <summary>
        /// Connect two Connectable Lists.
        /// </summary>
        /// <param name="__L"></param>
        /// <param name="__R"></param>
        /// <returns></returns>
        public static ConnectableList<T> operator +(ConnectableList<T> __L, ConnectableList<T> __R) {
            var result = new ConnectableList<T>();
            result.ConnectAfterEnd(__L);
            result.ConnectAfterEnd(__R);
            return result;
        }
        /// <summary>
        /// Connect two lists.
        /// </summary>
        /// <param name="__L"></param>
        /// <param name="__R"></param>
        /// <returns></returns>
        public static ConnectableList<T> operator +(ConnectableList<T> __L, List<T> __R) {
            var result = new ConnectableList<T>();
            result.ConnectAfterEnd(__L);
            result.ConnectAfterEnd(__R);
            return result;
        }
        /// <summary>
        /// Connect two lists.
        /// </summary>
        /// <param name="__L"></param>
        /// <param name="__R"></param>
        /// <returns></returns>
        public static ConnectableList<T> operator +(List<T> __L, ConnectableList<T> __R) {
            var result = new ConnectableList<T>();
            result.ConnectAfterEnd(__L);
            result.ConnectAfterEnd(__R);
            return result;
        }
    }
}
