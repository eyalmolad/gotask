using System;

namespace DirectoryWritable
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var result = DirectoryUtils.IsWritable(dir);
            Console.Write($"{dir} - result={result}");

            dir = Environment.SystemDirectory;
            result = DirectoryUtils.IsWritable(dir);
            Console.Write($"{dir} - result={result}");
        }
    }
}
