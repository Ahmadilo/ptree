using System;
using System.IO;

namespace ptree
{
    class Program
    {
        static readonly string[] IgnoreDirs = new[]
        {
            "node_modules", "vendor", ".git", "bin", "obj"
        };

        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] != "show")
            {
                Console.WriteLine("Usage: ptree show");
                return;
            }

            string root = Directory.GetCurrentDirectory();
            TreePrinter.PrintDirectory(root, "", IgnoreDirs);
        }
    }
}
