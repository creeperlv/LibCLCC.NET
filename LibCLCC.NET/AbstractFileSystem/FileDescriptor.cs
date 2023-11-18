using System;
using System.Collections.Generic;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    /// <summary>
    /// A base FileDescriptor. Must use inherited class.
    /// </summary>
    public abstract class FileDescriptor : IDisposable
    {
        /// <summary>
        /// Query Path
        /// </summary>
        public string FilePath;
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
            FilePath = path;
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
        public virtual bool TryOpen(FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream stream)
        {
            return provider.TryOpen(this, fileMode, fileAccess, share, out stream);
        }
        /// <summary>
        /// Dispose the descriptor.
        /// </summary>
        public virtual void Dispose()
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
}
