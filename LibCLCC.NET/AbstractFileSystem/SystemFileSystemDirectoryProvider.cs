using System;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    /// <summary>
    /// Provider using System.IO;
    /// </summary>
    public class SystemFileSystemDirectoryProvider : IDirectoryProvider
    {
        DirectoryInfo root;
        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="AutoCreateRoot">Whether automatically </param>
        public SystemFileSystemDirectoryProvider(DirectoryInfo root, bool AutoCreateRoot = true)
        {
            this.root = root;
            if (!root.Exists)
            {
                if (AutoCreateRoot)
                    try
                    {
                        root.Create();
                    }
                    catch (Exception)
                    {
                    }
            }
        }
        /// <summary>
        /// Close the opend stream.
        /// </summary>
        /// <param name="FD"></param>
        public void Close(FileDescriptor FD)
        {
            if (FD is FileSystemFileDescriptor fsfd)
            {
                fsfd.OpendStream?.Close();
            }
        }
        /// <summary>
        /// Dispose the opened stream.
        /// </summary>
        /// <param name="FD"></param>
        public void DisposeFile(FileDescriptor FD)
        {
            if (FD is FileSystemFileDescriptor fsfd)
            {
                fsfd.Dispose();
            }
        }
        /// <summary>
        /// Get a file by given path no matter if it exists.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryGetFile(PathQuery path, out FileDescriptor FD)
        {
            var _path = path.ConstructRemainingPath().Replace('\\', Path.PathSeparator).Replace('/', Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
                _path = _path[1..];
            }
            string fileName = Path.Combine(root.FullName, _path);
            FileInfo info = new FileInfo(fileName);
            FD = new FileSystemFileDescriptor(this, path.ConstructRemainingPath(), info) { IsDirectory = false };
            return true;
        }

        /// <summary>
        /// Get a directory by given path no matter if it exists.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryGetDirectory(PathQuery path, out FileDescriptor FD)
        {
            string remain_path = path.ConstructRemainingPath();
            var _path = remain_path.Replace('\\', Path.PathSeparator).Replace('/', Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
                _path = _path[1..];
            }
            string fileName = Path.Combine(root.FullName, _path);
            DirectoryInfo info = new DirectoryInfo(fileName);
            FD = new FileSystemFileDescriptor(this, remain_path, info) { IsDirectory = true };
            return true;
        }
        /// <summary>
        /// Open a file descriptor.
        /// </summary>
        /// <param name="FD"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="share"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool TryOpen(FileDescriptor FD, FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream stream)
        {
            if (FD is FileSystemFileDescriptor fsfd)
            {
                if (fsfd.info is FileInfo fi)
                {
                    stream = fi.Open(fileMode, fileAccess, share);
                    return true;
                }
            }
            stream = null;
            return false;
        }
        /// <summary>
        /// Query an existing file or folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryQuery(PathQuery path, out FileDescriptor FD)
        {
            string path_original = path.ConstructRemainingPath();
            var _path = path_original.Replace('\\', Path.PathSeparator).Replace('/', Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
                _path = _path[1..];
            }
            var rp = Path.Combine(root.FullName, _path);
            if (File.Exists(rp))
            {
                FD = new FileSystemFileDescriptor(this, path_original, new FileInfo(rp)) { IsDirectory = false };
                return true;
            }
            else if (Directory.Exists(rp))
            {
                FD = new FileSystemFileDescriptor(this, path_original, new DirectoryInfo(rp)) { IsDirectory = true };
                return true;
            }
            FD = null;
            return false;
        }
        /// <summary>
        /// Get the file descriptor representation of the provider.
        /// </summary>
        /// <param name="map_path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryGetSelf(string map_path, out FileDescriptor FD)
        {
            FD = new FileSystemFileDescriptor(this, map_path, root) { IsDirectory = true };
            return true;
        }
    }
}
