using LibCLCC.NET.Data;
using System.Collections.Generic;
using System.IO;

namespace LibCLCC.NET.AbstractFileSystem
{
    public class SystemRoot
    {
        Dictionary<string , IDirectoryProvider> PathMap = new Dictionary<string , IDirectoryProvider>();
        public void MapProvider(string ProjectedPath , IDirectoryProvider provider)
        {
            ProjectedPath = ProjectedPath.Replace('\\' , '/');
            PathMap.Add(ProjectedPath , provider);
        }
        public bool TryObtainFile(string TargetPath , out FileDescriptor? FD)
        {
            var query=(RefString)TargetPath;
            foreach (var item in PathMap)
            {
                if (TargetPath.StartsWith(item.Key))
                {
                    if (item.Value.TryObtainFile(query.Substring(item.Key.Length) , out FD))
                    {
                        return true;
                    }
                }
            }
            FD = null;
            return false;
        }
        public bool TryObtainDirectory(string TargetPath , out FileDescriptor? FD)
        {
            var query=(RefString)TargetPath;
            foreach (var item in PathMap)
            {
                if (TargetPath.StartsWith(item.Key))
                {
                    if (item.Value.TryObtainDirectory(query.Substring(item.Key.Length) , out FD))
                    {
                        return true;
                    }
                }
            }
            FD = null;
            return false;
        }
        public bool TryQuery(string TargetPath , out FileDescriptor? FD)
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
    public struct PathQuery
    {
        public RefString OriginalQeuryPath;
        public IEnumerator<RefString> Splitted;
        public static implicit operator PathQuery(string path)
        {
            var pq=new PathQuery { OriginalQeuryPath = path };
            pq.Splitted=pq.OriginalQeuryPath.Split('/','\\',Path.DirectorySeparatorChar);
            return pq;
        }
        public static implicit operator PathQuery(RefString path)
        {
            var pq=new PathQuery { OriginalQeuryPath = path };
            pq.Splitted=pq.OriginalQeuryPath.Split('/','\\',Path.DirectorySeparatorChar);
            return pq;
        }
    }
    public class MappedDirectoryProvider : IDirectoryProvider
    {
        Dictionary<string , IDirectoryProvider> PathMap = new Dictionary<string , IDirectoryProvider>();
        public void Close(FileDescriptor FD)
        {
        }

        public void DisposeFile(FileDescriptor FD)
        {
        }

        public bool TryObtainDirectory(PathQuery path , out FileDescriptor? FD)
        {
            throw new System.NotImplementedException();
        }

        public bool TryObtainFile(PathQuery path , out FileDescriptor? FD)
        {
            throw new System.NotImplementedException();
        }

        public bool TryOpen(FileDescriptor FD , FileMode fileMode , FileAccess fileAccess , FileShare share , out Stream? stream)
        {
            throw new System.NotImplementedException();
        }

        public bool TryQuery(PathQuery path , out FileDescriptor? FD)
        {
            throw new System.NotImplementedException();
        }
    }
}
