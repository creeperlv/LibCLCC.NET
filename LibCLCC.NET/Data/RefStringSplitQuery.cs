using System.Collections;
using System.Collections.Generic;

namespace LibCLCC.NET.Data
{
    /// <summary>
    /// Query of splitting ref string.
    /// </summary>
    public struct RefStringSplitQuery : IEnumerable<RefString>
    {
        internal IEnumerator<RefString> query;
        /// <summary>
        /// If query reaches end.
        /// </summary>
        public bool ReachEnd;
        /// <summary>
        /// Move next.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            ReachEnd=!query.MoveNext();
            return ReachEnd;
        }
        /// <summary>
        /// Get the current ref string in the query.
        /// </summary>
        public RefString Current=>query.Current;
        /// <summary>
        /// Get the enumerator of the query.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<RefString> GetEnumerator()
        {
            return query;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
