using LibCLCC.NET.AbstractFileSystem;

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
                WriteExampleFile(file);
            }
            else Console.WriteLine("Failed Successfully.");

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
            if (root.GetFile("/home/creeperlv/.bashrc", out var file1))
            {
                WriteExampleFile(file1);
            }
            else Console.WriteLine("Fail.");
        }

        private static void WriteExampleFile(FileDescriptor file)
        {
            if (file?.TryOpen(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, out var stream) ?? false)
            {
                if (stream is not null)
                {
                    using StreamWriter SW = new StreamWriter(stream);
                    SW.Write("""PS1="\\w\\h:"\n""");
                    SW.Flush();
                }
            }
        }
    }
}