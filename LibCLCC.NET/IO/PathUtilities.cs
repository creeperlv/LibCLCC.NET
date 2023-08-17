using System.Runtime.CompilerServices;

namespace LibCLCC.NET.IO
{
    /// <summary>
    /// Some functions about file path.
    /// </summary>
    public static class PathUtilities
    {
        /// <summary>
        /// Segment a path string with ``/'' and ``\''.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] SegmentPath(string path)
        {
            return path.Split('/', '\\');
        }
    }
}
