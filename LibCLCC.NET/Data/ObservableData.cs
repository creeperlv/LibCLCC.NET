using LibCLCC.NET.Delegates;
using System.Collections.Generic;
using System.Text;

namespace LibCLCC.NET.Data {
    /// <summary>
    /// Observable Data. Changing the referencing object will notify the chain. Changing internal data to the holding data (like A.b where A is a class and b is a field) will not cause chain action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableData<T> {
        T _value;
        /// <summary>
        /// Value
        /// </summary>
        public T Value {
            get => _value; set {
                _value = value;
                OnChange.Invoke(value);
            }
        }
        /// <summary>
        /// Preserved for serialization purpose.
        /// </summary>
        public ObservableData() {}
        /// <summary>
        /// Initialize data with pre-defined data.
        /// </summary>
        /// <param name="v"></param>
        public ObservableData(T v) { _value = v; }

        /// <summary>
        /// Invoke when the Value is changed;
        /// </summary>
        public ChainAction<T> OnChange { get; } = new ChainAction<T>();
        /// <summary>
        /// Invoke when the data will be released.
        /// </summary>
        public ChainAction OnRelease { get; } = new ChainAction();
        /// <summary>
        /// Notify the data will release.
        /// </summary>
        public void Release() {
            OnRelease.Invoke();
            OnRelease.Clear();
            OnChange.Clear();
        }
    }
}
