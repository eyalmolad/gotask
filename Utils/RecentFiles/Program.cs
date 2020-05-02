using System.Runtime.InteropServices;

namespace RecentFiles
{
   
    class Program
    {        
        static void Main(string[] args)
        {
            RecentDocsHelpers.ClearAll();

            // add c:\temp\sample.json to recent files.
            RecentDocsHelpers.AddFile(@"c:\temp\sample.json");
        }
    }

    public static class RecentDocsHelpers
    {
        public enum ShellAddToRecentDocsFlags
        {
            Pidl = 0x001,
            Path = 0x002,
            PathW = 0x003
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern void SHAddToRecentDocs(ShellAddToRecentDocsFlags flag, string path);

        public static void AddFile(string path)
        {
            SHAddToRecentDocs(ShellAddToRecentDocsFlags.PathW, path);
        }

        public static void ClearAll()
        {
            SHAddToRecentDocs(ShellAddToRecentDocsFlags.Pidl, null);
        }
    }
}
