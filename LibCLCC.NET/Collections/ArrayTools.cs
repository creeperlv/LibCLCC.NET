namespace LibCLCC.NET.Collections {
    /// <summary>
    /// Tools for arrays.
    /// </summary>
    public static class ArrayTools {
        /// <summary>
        /// Is an array contains an element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Contains<T>(T[] arr,T t) {
            foreach (var item in arr) {
                if(item.Equals(t)) return true;
            }
            return false;
        }
    }
}
