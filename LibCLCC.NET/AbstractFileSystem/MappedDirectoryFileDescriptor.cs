using System.Collections.Generic;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    /// <summary>
    /// Represent a mapped directory provider.
    /// </summary>
    public class MappedDirectoryFileDescriptor : FileDescriptor
    {
        /// <summary>
        /// Mapped file desciptor.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="path"></param>
        public MappedDirectoryFileDescriptor(IDirectoryProvider provider, string path) : base(provider, path)
        {
        }
        /// <summary>
        /// Enumerate mapped directories.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<FileDescriptor> EnumerateDirectories()
        {
            if (provider is MappedDirectoryProvider mdp)
            {
                foreach (var item in mdp.PathMap)
                {
                    if (item.Value.TryGetSelf(Path.Combine(FilePath, item.Key), out var fd))
                    {
                        yield return fd;
                    }
                }
                foreach (var item in mdp.FileMap)
                {
                    yield return item.Value;
                }
            }
            yield break;
        }
        /// <summary>
        /// Enumerate all mapped directories and file.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<FileDescriptor> EnumerateAnyDescriptors()
        {
            if (provider is MappedDirectoryProvider mdp)
            {
                foreach (var item in mdp.PathMap)
                {
                    if (item.Value.TryGetSelf(Path.Combine(FilePath, item.Key), out var fd))
                    {
                        yield return fd;
                    }
                }
                foreach (var item in mdp.FileMap)
                {
                    yield return item.Value;
                }
            }
            yield break;
        }
        /// <summary>
        /// Enumerate mapped files
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<FileDescriptor> EnumerateFiles()
        {
            if (provider is MappedDirectoryProvider mdp)
                foreach (var item in mdp.FileMap)
                {
                    yield return item.Value;
                }
            yield break;
        }
    }
}
