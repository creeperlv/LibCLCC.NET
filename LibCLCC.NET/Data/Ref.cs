namespace LibCLCC.NET.Data
{
    /// <summary>
    /// Reerence to a struct.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Ref<T> where T : struct
    {
        /// <summary>
        /// Referred data.
        /// </summary>
        public T Data;
        /// <summary>
        /// Initialize with default data.
        /// </summary>
        public Ref()
        {
            Data = default(T);
        }
        /// <summary>
        /// Initialize with given data.
        /// </summary>
        /// <param name="data"></param>
        public Ref(T data)
        {
            Data = data;
        }
    }
}
