using LibCLCC.NET.AbstractFileSystem;
using LibCLCC.NET.Data;

namespace AbstractFileSystemSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SystemRoot root = new SystemRoot();
            root.MapDirectory("/bin/", new SystemFileSystemDirectoryProvider(new DirectoryInfo("./fs/bin/")));
            root.MapDirectory("/etc/", new SystemFileSystemDirectoryProvider(new DirectoryInfo("./fs/etc_data/")));
            MappedDirectoryProvider homeProvider = new MappedDirectoryProvider();
            homeProvider.MapDirectory("creeperlv", new SystemFileSystemDirectoryProvider(new DirectoryInfo("./fs/homes/usr000")));
            homeProvider.MapDirectory("CRLV", new SystemFileSystemDirectoryProvider(new DirectoryInfo("./fs/homes/usr001")));
            //root.MapDirectory("/home/", new SystemFileSystemDirectoryProvider(new DirectoryInfo("./fs/data/home")));
            root.MapDirectory("/home/", homeProvider);
            if (root.GetFile("/home/.bashrc", out var file))
            {
                WriteExampleFile(file, "PS1=\"\\w\\h:\"\n");
            }
            else Console.WriteLine("Failed Successfully.");

            {

                if (root.GetDirectory("/home/", out var dir))
                {
                    var result = dir.EnumerateDirectories();
                    while (result.MoveNext())
                    {
                        Console.WriteLine(result.Current.FilePath);
                    }
                }
            }
            {

                if (root.GetDirectory("/home/creeperlv/", out var dir))
                {
                    if (dir.TryCreate())
                    {
                        Console.WriteLine("Created home directory.");
                    }
                    else
                    {
                        Console.WriteLine("Fail to create directory.");
                    }
                }
            }
            if (root.GetFile("/home/creeperlv/.bashrc", out var file1))
            {
                WriteExampleFile(file1, "PS1=\"\\w\\h:\"\nSecond Line.\nThird Line");
                file1.TryDelete();
                WriteExampleFile(file1, "Second Write.\n");

            }
            else Console.WriteLine("Fail.");
        }

        private static void WriteExampleFile(FileDescriptor file, string value)
        {
            if (file?.TryOpen(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, out var stream) ?? false)
            {
                if (stream is not null)
                {
                    using StreamWriter SW = new(stream);
                    SW.Write(value);
                    SW.Flush();
                }
            }
        }
    }
}