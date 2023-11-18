using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
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
        /// <summary>
        /// Try to query an existing file descriptor.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        bool TryQuery(PathQuery path, out FileDescriptor FD);
        /// <summary>
        /// Try to open a descriptor.
        /// </summary>
        /// <param name="FD"></param>
        /// <param name="fileMode"></param>
        /// <param name="fileAccess"></param>
        /// <param name="share"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        bool TryOpen(FileDescriptor FD, FileMode fileMode, FileAccess fileAccess, FileShare share, out Stream stream);
        /// <summary>
        /// Close a descriptor.
        /// </summary>
        /// <param name="FD"></param>
        void Close(FileDescriptor FD);
        /// <summary>
        /// Call FileDescriptor.Dispose().
        /// </summary>
        /// <param name="FD"></param>
        void DisposeFile(FileDescriptor FD);
        /// <summary>
        /// Get the file descriptor representation of the provider it self.
        /// </summary>
        /// <param name="map_path"></param>
        /// <param name="FD"></param>
        /// <returns></returns>
        bool TryGetSelf(string map_path, out FileDescriptor FD);
    }
}
