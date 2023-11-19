using System;
using System.Collections.Generic;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    /// <summary>
    /// FileDescriptor using System.IO.
    /// </summary>
    public sealed class FileSystemFileDescriptor : FileDescriptor
    {
        /// <summary>
        /// Opend Stream of the file.
        /// </summary>
        public Stream OpendStream;
        /// <summary>
        /// Exposed for convenience. Re-assign with care!
        /// </summary>
        public FileSystemInfo UnderlyingFileSystemInfo;
        /// <summary>
        /// Initialize the descriptor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public FileSystemFileDescriptor(IDirectoryProvider provider, string path, FileSystemInfo info) : base(provider, path)
        {
            this.UnderlyingFileSystemInfo = info;
        }
        /// <summary>
        /// Dispose opend stream.
        /// </summary>
        public override void Dispose()
        {
            OpendStream?.Dispose();
        }
        /// <summary>
        /// Get the existence of the file descriptor.
        /// </summary>
        /// <returns></returns>
        public override bool IsExists()
        {
            return UnderlyingFileSystemInfo.Exists;
        }
        /// <summary>
        /// Try delete the file descriptor.
        /// </summary>
        /// <returns></returns>
        public override bool TryDelete()
        {
            try
            {
                if (UnderlyingFileSystemInfo is FileInfo fi)
                {
                    fi.Delete();
                    return true;
                }
                else if (UnderlyingFileSystemInfo is DirectoryInfo di)
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
            if (UnderlyingFileSystemInfo is FileInfo fi)
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
            else if (UnderlyingFileSystemInfo is DirectoryInfo di)
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
        public override bool TryOpen(FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream stream)
        {
            if (UnderlyingFileSystemInfo is FileInfo fi)
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
            if (UnderlyingFileSystemInfo is DirectoryInfo di)
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
            if (UnderlyingFileSystemInfo is DirectoryInfo di)
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
            if (UnderlyingFileSystemInfo is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateFileSystemInfos())
                {
                    yield return new FileSystemFileDescriptor(this.provider, di.Name, item) { IsDirectory = item is DirectoryInfo };
                }
            }
            base.EnumerateDirectories();
        }
    }
}
