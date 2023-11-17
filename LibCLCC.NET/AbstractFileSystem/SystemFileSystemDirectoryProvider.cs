using System;
using System.Collections.Generic;
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
                fsfd.OpendStream?.Dispose();
            }
        }
        /// <summary>
        /// Get a file by given path no matter if it exists.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        public bool TryGetFile(PathQuery path, out FileDescriptor? FD)
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
        public bool TryGetDirectory(PathQuery path, out FileDescriptor? FD)
        {
            var _path = path.ConstructRemainingPath().Replace('\\', Path.PathSeparator).Replace('/', Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
                _path = _path[1..];
            }
            string fileName = Path.Combine(root.FullName, _path);
            DirectoryInfo info = new DirectoryInfo(fileName);
            FD = new FileSystemFileDescriptor(this, path.ConstructRemainingPath(), info) { IsDirectory = true };
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
    }
    /// <summary>
    /// FileDescriptor using System.IO.
    /// </summary>
    public sealed class FileSystemFileDescriptor : FileDescriptor
    {
        internal Stream OpendStream;
        internal FileSystemInfo info;
        /// <summary>
        /// Initialize the descriptor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public FileSystemFileDescriptor(IDirectoryProvider provider, string path, FileSystemInfo info) : base(provider, path)
        {
            this.info = info;
        }
        /// <summary>
        /// Get the existence of the file descriptor.
        /// </summary>
        /// <returns></returns>
        public override bool IsExists()
        {
            return info.Exists;
        }
        /// <summary>
        /// Try delete the file descriptor.
        /// </summary>
        /// <returns></returns>
        public override bool TryDelete()
        {
            try
            {
                if (info is FileInfo fi)
                {
                    fi.Delete();
                    return true;
                }
                else if (info is DirectoryInfo di)
                {
                    di.Delete(true);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// Create the file descriptor on real file system.
        /// </summary>
        /// <returns></returns>
        public override bool TryCreate()
        {
            if (info is FileInfo fi)
            {
                try
                {
                    fi.Create().Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (info is DirectoryInfo di)
            {
                try
                {
                    di.Create();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// Try open the file. Faile if is a directory.
        /// </summary>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="share"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override bool TryOpen(FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream? stream)
        {
            if (info is FileInfo fi)
            {
                try
                {
                    stream = fi.Open(fileMode, fileAccess, share);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            stream = null;
            return false;
        }
        /// <summary>
        /// Enumerate Directories.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public override IEnumerator<FileDescriptor> EnumerateDirectories()
        {
            if (!IsDirectory) throw new NotSupportedException();
            if (info is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateDirectories())
                {
                    yield return new FileSystemFileDescriptor(this.provider, di.Name, item) { IsDirectory = true };
                }
            }
            base.EnumerateDirectories();
        }
        /// <summary>
        /// List all files.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public override IEnumerator<FileDescriptor> EnumerateFiles()
        {
            if (!IsDirectory) throw new NotSupportedException();
            if (info is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateFiles())
                {
                    yield return new FileSystemFileDescriptor(this.provider, di.Name, item) { IsDirectory = false };
                }
            }
            base.EnumerateFiles();
        }
        /// <summary>
        /// Enumerate all kind of descriptor.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public override IEnumerator<FileDescriptor> EnumerateAnyDescriptors()
        {
            if (!IsDirectory) throw new NotSupportedException();
            if (info is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateFileSystemInfos())
                {
                    yield return new FileSystemFileDescriptor(this.provider, di.Name, item) { IsDirectory = item is DirectoryInfo };
                }
            }
            base.EnumerateDirectories();
        }
    }
    /// <summary>
    /// A base FileDescriptor. Must use inherited class.
    /// </summary>
    public abstract class FileDescriptor : IDisposable
    {
        /// <summary>
        /// Query Path
        /// </summary>
        public string Path;
        /// <summary>
        /// Provider of the file.
        /// </summary>
        public IDirectoryProvider provider;
        /// <summary>
        /// If the file desc is a directory.
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// Initialize the descriptor.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="path"></param>
        public FileDescriptor(IDirectoryProvider provider, string path)
        {
            this.provider = provider;
            Path = path;
        }
        /// <summary>
        /// Try create the descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual bool TryCreate()
        {
            return false;
        }
        /// <summary>
        /// Try delete the descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual bool TryDelete()
        {
            return false;
        }
        /// <summary>
        /// Test existence of the descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsExists()
        {
            return false;
        }
        /// <summary>
        /// Try open the descriptor.
        /// </summary>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="share"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual bool TryOpen(FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream? stream)
        {
            return provider.TryOpen(this, fileMode, fileAccess, share, out stream);
        }
        /// <summary>
        /// Dispose the descriptor.
        /// </summary>
        public void Dispose()
        {
            provider.DisposeFile(this);
        }
        /// <summary>
        /// List all directories.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual IEnumerator<FileDescriptor> EnumerateDirectories()
        {
            if (!IsDirectory) throw new NotSupportedException();
            yield break;
        }
        /// <summary>
        /// List all files.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual IEnumerator<FileDescriptor> EnumerateFiles()
        {
            if (!IsDirectory) throw new NotSupportedException();
            yield break;
        }
        /// <summary>
        /// List all kind of file descriptors.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual IEnumerator<FileDescriptor> EnumerateAnyDescriptors()
        {
            if (!IsDirectory) throw new NotSupportedException();
            yield break;
        }
    }
    /// <summary>
    /// Provider interface.
    /// </summary>
    public interface IDirectoryProvider
    {
        /// <summary>
        /// Get a file no matter its existence.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        bool TryGetFile(PathQuery path, out FileDescriptor FD);
        /// <summary>
        /// Get a directory no matter its existence.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        bool TryGetDirectory(PathQuery path, out FileDescriptor FD);
        bool TryQuery(PathQuery path, out FileDescriptor FD);
        bool TryOpen(FileDescriptor FD, FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream stream);
        void Close(FileDescriptor FD);
        void DisposeFile(FileDescriptor FD);
    }
}
