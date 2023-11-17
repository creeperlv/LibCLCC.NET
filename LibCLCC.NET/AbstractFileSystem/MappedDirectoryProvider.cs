﻿using System.Collections.Generic;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    /// <summary>
    /// A mapped directory. Cannot directly create a file in the pseudo directory this provides.
    /// </summary>
    public class MappedDirectoryProvider : IDirectoryProvider
    {
        Dictionary<string, IDirectoryProvider> PathMap = new Dictionary<string, IDirectoryProvider>();
        /// <summary>
        /// Close using real provider.
        /// </summary>
        /// <param name="FD"></param>
        public void Close(FileDescriptor FD)
        {
            FD.provider.Close(FD);
        }
        /// <summary>
        /// No sub directories are allowed.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="provider"></param>
        public void MapDirectory(string Path, IDirectoryProvider provider)
        {
            PathMap.Add(Path, provider);
        }
        /// <summary>
        /// Dispose a file through its real provider.
        /// </summary>
        /// <param name="FD"></param>
        public void DisposeFile(FileDescriptor FD)
        {
            FD.provider.DisposeFile(FD);
        }
        /// <summary>
        /// Try get a directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryGetDirectory(PathQuery path, out FileDescriptor FD)
        {
            var p = path.Query.query.Current.FinalizeString();
            if (PathMap.ContainsKey(p))
            {
                path.Query.query.MoveNext();
                return PathMap[p].TryGetDirectory(path, out FD);
            }
            FD = null;
            return false;
        }
        /// <summary>
        /// Try get a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryGetFile(PathQuery path, out FileDescriptor FD)
        {
            var p = path.Query.query.Current.FinalizeString();
            if (PathMap.ContainsKey(p))
            {
                path.Query.query.MoveNext();
                return PathMap[p].TryGetFile(path, out FD);
            }
            FD = null;
            return false;
        }
        /// <summary>
        /// Try Open using FD's real descriptor.
        /// </summary>
        /// <param name="FD"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="share"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool TryOpen(FileDescriptor FD, FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream stream)
        {
            return FD.provider.TryOpen(FD,fileMode, fileAccess, share,out stream);
        }
        /// <summary>
        /// Query an existing file descriptor.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryQuery(PathQuery path, out FileDescriptor FD)
        {
            var p = path.Query.query.Current.FinalizeString();
            if (PathMap.ContainsKey(p))
            {
                path.Query.query.MoveNext();
                return PathMap[p].TryQuery(path, out FD);
            }
            FD = null;
            return false;
        }
    }
}
