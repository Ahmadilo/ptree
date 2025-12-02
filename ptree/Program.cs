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
                Console.WriteLine("Usage: ptree show [--deep N]");
                return;
            }

            // القيمة الافتراضية للعمق (لا نهائي)
            int depth = 5;

            // فحص وجود --deep
            int deepIndex = Array.IndexOf(args, "--deep");
            if (deepIndex >= 0 && deepIndex + 1 < args.Length)
            {
                if (int.TryParse(args[deepIndex + 1], out int d))
                    depth = d;
            }

            string root = Directory.GetCurrentDirectory();
            TreePrinter.PrintDirectory(root, "   ", IgnoreDirs, 1, depth);
        }
    }
}
