using LibCLCC.NET.Data;
using System.Collections.Generic;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    /// <summary>
    /// Root of a file system.
    /// </summary>
    public class SystemRoot
    {
        Dictionary<string , IDirectoryProvider> PathMap = new Dictionary<string , IDirectoryProvider>();
        /// <summary>
        /// Map a path to an IDirectoryProvider.
        /// </summary>
        /// <param name="ProjectedPath"></param>
        /// <param name="provider"></param>
        public void MapDirectory(string ProjectedPath , IDirectoryProvider provider)
        {
            ProjectedPath = ProjectedPath.Replace('\\' , '/');
            PathMap.Add(ProjectedPath , provider);
        }
        /// <summary>
        /// Get a file no matter its existence.
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool GetFile(string TargetPath , out FileDescriptor FD)
        {
            var query=(RefString)TargetPath;
            foreach (var item in PathMap)
            {
                if (TargetPath.StartsWith(item.Key))
                {
                    if (item.Value.TryGetFile(query.Substring(item.Key.Length) , out FD))
                    {
                        return true;
                    }
                }
            }
            FD = null;
            return false;
        }
        /// <summary>
        /// Get a directory no matter its existence.
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool GetDirectory(string TargetPath , out FileDescriptor FD)
        {
            var query=(RefString)TargetPath;
            foreach (var item in PathMap)
            {
                if (TargetPath.StartsWith(item.Key))
                {
                    if (item.Value.TryGetDirectory(query.Substring(item.Key.Length) , out FD))
                    {
                        return true;
                    }
                }
            }
            FD = null;
            return false;
        }
        /// <summary>
        /// Get an existing file descriptor.
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryQuery(string TargetPath , out FileDescriptor FD)
        {
            var query=(RefString)TargetPath;
            foreach (var item in PathMap)
            {
                if (TargetPath.StartsWith(item.Key))
                {
                    if (item.Value.TryQuery(query.Substring(item.Key.Length) , out FD))
                    {
                        return true;
                    }
                }
            }
            FD = null;
            return false;
        }
    }
    /// <summary>
    /// Query of a path.
    /// </summary>
    public struct PathQuery
    {
        /// <summary>
        /// Original query string.
        /// </summary>
        public RefString OriginalQueryPath;
        /// <summary>
        /// Query of the path.
        /// </summary>
        public RefStringSplitQuery Query;
        /// <summary>
        /// Construct a path start from remaining query.
        /// </summary>
        /// <returns></returns>
        public string ConstructRemainingPath()
        {
            return OriginalQueryPath.Ref[Query.query.Current.Offset..(OriginalQueryPath.Length+OriginalQueryPath.Offset)];
        }
        /// <summary>
        /// Construct a query from a string.
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator PathQuery(string path)
        {
            var pq=new PathQuery { OriginalQueryPath = path };
            pq.Query=pq.OriginalQueryPath.SplitQuery('/','\\',Path.DirectorySeparatorChar);
            pq.Query.MoveNext();
            return pq;
        }
        /// <summary>
        /// Construct a query from a ref string/
        /// </summary>
        /// <param name="path"></param>
        public static implicit operator PathQuery(RefString path)
        {
            var pq=new PathQuery { OriginalQueryPath = path };
            pq.Query=pq.OriginalQueryPath.SplitQuery('/','\\',Path.DirectorySeparatorChar);
            pq.Query.MoveNext();
            return pq;
        }
    }
}
