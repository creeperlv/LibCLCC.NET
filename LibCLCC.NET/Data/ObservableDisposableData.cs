using LibCLCC.NET.Delegates;
using System;

namespace LibCLCC.NET.Data {
    /// <summary>
    /// Observable Disposable Data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableDisposableData<T>:IDisposable where T : IDisposable {
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
        public ObservableDisposableData() { }
        /// <summary>
        /// Initialize data with pre-defined data.
        /// </summary>
        /// <param name="v"></param>
        public ObservableDisposableData(T v) { _value = v; }

        /// <summary>
        /// Invoke when the Value is changed;
        /// </summary>
        public ChainAction<T> OnChange { get; } = new ChainAction<T>();
        /// <summary>
        /// Invoke when the data will be released.
        /// </summary>
        public ChainAction<T> OnRelease { get; } = new ChainAction<T>();
        /// <summary>
        /// Invoke when the data is being disposed.
        /// </summary>
        public ChainAction<T> OnDispose { get; } = new ChainAction<T>();
        /// <summary>
        /// Invoke when the data is disposed.
        /// </summary>
        public ChainAction<T> AfterDispose { get; } = new ChainAction<T>();
        /// <summary>
        /// Notify the data will release.
        /// </summary>
        public void Release() {
            OnRelease.Invoke(Value);
            OnRelease.Clear();
            OnChange.Clear();
        }
        /// <summary>
        /// Dispose holding object.
        /// </summary>
        public void Dispose() {
            OnDispose.Invoke(Value);
            Value.Dispose();
            AfterDispose.Invoke(Value);
            AfterDispose.Clear();
            OnDispose.Clear();
        }
    }
}
