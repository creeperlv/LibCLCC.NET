namespace LibCLCC.NET.IO
{
    public static class PathUtilities
    {
        /// <summary>
        /// Segment a path string with ``/'' and ``\''.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] SegmentPath(string path)
        {
            return path.Split('/', '\\');
        }
    }
}
