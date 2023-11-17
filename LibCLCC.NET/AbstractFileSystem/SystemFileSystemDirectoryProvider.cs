using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibCLCC.NET.AbstractFileSystem
{
    public class SystemFileSystemDirectoryProvider : IDirectoryProvider
    {
        DirectoryInfo root;
        public SystemFileSystemDirectoryProvider(DirectoryInfo root)
        {
            this.root = root;
            if (!root.Exists)
            {
                try
                {
                    root.Create();
                }
                catch (Exception)
                {
                }
            }
        }

        public void Close(FileDescriptor FD)
        {
            if(FD is FileSystemFileDescriptor fsfd)
            {
                fsfd.OpendStream?.Close();
            }
        }

        public void DisposeFile(FileDescriptor FD)
        {
            if (FD is FileSystemFileDescriptor fsfd)
            {
                fsfd.OpendStream?.Dispose();
            }
        }

        public bool TryObtainFile(PathQuery path , out FileDescriptor? FD)
        {
            var _path=path.OriginalQeuryPath.FinalizeString().Replace('\\', Path.PathSeparator).Replace('/',Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
              _path=  _path[ 1.. ];
            }
            string fileName = Path.Combine(root.FullName , _path);
            FileInfo info = new FileInfo(fileName);
            FD = new FileSystemFileDescriptor(this , path.OriginalQeuryPath.FinalizeString() , info) { IsDirectory=false};
            return true;
        }

        public bool TryObtainDirectory(PathQuery path , out FileDescriptor? FD)
        {
            var _path=path.OriginalQeuryPath.FinalizeString().Replace('\\', Path.PathSeparator).Replace('/',Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
                _path = _path[ 1.. ];
            }
            string fileName = Path.Combine(root.FullName , _path);
            DirectoryInfo info = new DirectoryInfo(fileName);
            FD=new FileSystemFileDescriptor(this,path.OriginalQeuryPath.FinalizeString() , info) { IsDirectory=true};
            return true;
        }

        public bool TryOpen(FileDescriptor FD , FileMode fileMode , FileAccess fileAccess , FileShare share , out Stream? stream)
        {
            if (FD is FileSystemFileDescriptor fsfd)
            {
                if (fsfd.info is FileInfo fi)
                {
                    stream = fi.Open(fileMode , fileAccess , share);
                    return true;
                }
            }
            stream = null;
            return false;
        }

        public bool TryQuery(PathQuery path , out FileDescriptor? FD)
        {
            string path_original = path.OriginalQeuryPath.FinalizeString();
            var _path= path_original.Replace('\\', Path.PathSeparator).Replace('/', Path.DirectorySeparatorChar);
            if (_path.StartsWith(Path.DirectorySeparatorChar))
            {
              _path=  _path[ 1.. ];
            }
            var rp = Path.Combine(root.FullName , _path);
            if (System.IO.File.Exists(rp))
            {
                FD = new FileSystemFileDescriptor(this , path_original , new FileInfo(rp)) { IsDirectory = false };
                return true;
            }
            else if (Directory.Exists(rp))
            {
                FD = new FileSystemFileDescriptor(this , path_original , new DirectoryInfo(rp)) { IsDirectory = true };
                return true;
            }
            FD = null;
            return false;
        }
    }
    public sealed class FileSystemFileDescriptor : FileDescriptor
    {
        internal Stream? OpendStream;
        internal FileSystemInfo info;

        public FileSystemFileDescriptor(IDirectoryProvider provider , string path , FileSystemInfo info) : base(provider , path)
        {
            this.info = info;
        }
        public override bool TryOpen(FileMode fileMode , FileAccess fileAccess , FileShare share , out Stream? stream)
        {
            if (info is FileInfo fi)
            {
                stream = fi.Open(fileMode , fileAccess , share);
                return true;
            }
            stream = null;
            return false;
        }
        public override IEnumerator<FileDescriptor> EnumerateDirectories()
        {
            if(!IsDirectory)throw new NotSupportedException();
            if (info is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateDirectories())
                {
                    yield return new FileSystemFileDescriptor(this.provider , di.Name , item) { IsDirectory = true };
                }
            }
            base.EnumerateDirectories();
        }
        public override IEnumerator<FileDescriptor> EnumerateFiles()
        {
            if(!IsDirectory)throw new NotSupportedException();
            if (info is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateFiles())
                {
                    yield return new FileSystemFileDescriptor(this.provider , di.Name , item) { IsDirectory = false };
                }
            }
            base.EnumerateFiles();
        }
        public override IEnumerator<FileDescriptor> EnumerateAnyDescriptors()
        {
            if(!IsDirectory)throw new NotSupportedException();
            if (info is DirectoryInfo di)
            {
                foreach (var item in di.EnumerateFileSystemInfos())
                {
                    yield return new FileSystemFileDescriptor(this.provider , di.Name , item) { IsDirectory = item is DirectoryInfo };
                }
            }
            base.EnumerateDirectories();
        }
    }
    public class FileDescriptor : IDisposable
    {
        public string Path;
        public IDirectoryProvider provider;
        public bool IsDirectory;
        public FileDescriptor(IDirectoryProvider provider , string path)
        {
            this.provider = provider;
            Path = path;
        }

        public virtual bool TryOpen(FileMode fileMode , FileAccess fileAccess , FileShare share , out Stream? stream)
        {
            return provider.TryOpen(this , fileMode , fileAccess , share , out stream);
        }
        public void Dispose()
        {
            provider.DisposeFile(this);
        }
        public virtual IEnumerator<FileDescriptor> EnumerateDirectories()
        {
            if(!IsDirectory)throw new NotSupportedException();
            yield break;
        }
        public virtual IEnumerator<FileDescriptor> EnumerateFiles()
        {
            if(!IsDirectory)throw new NotSupportedException();
            yield break;
        }
        public virtual IEnumerator<FileDescriptor> EnumerateAnyDescriptors()
        {
            if(!IsDirectory)throw new NotSupportedException();
            yield break;
        }
    }
    public interface IDirectoryProvider
    {
        bool TryObtainFile(PathQuery path , out FileDescriptor? FD);
        bool TryObtainDirectory(PathQuery path , out FileDescriptor? FD);
        bool TryQuery(PathQuery path , out FileDescriptor? FD);
        bool TryOpen(FileDescriptor FD , FileMode fileMode , FileAccess fileAccess , FileShare share , out Stream? stream);
        void Close(FileDescriptor FD);
        void DisposeFile(FileDescriptor FD);
    }
}
