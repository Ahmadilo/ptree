using System;
using System.IO;

namespace ptree
{
    class Program
    {
        static string[] IgnoreDirs = new[]
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

            int ingoredIndex = Array.IndexOf(args, "--ignore");
            if(ingoredIndex >= 0 && ingoredIndex + 1 == args.Length)
            {
                for(int i = 0; i < IgnoreDirs.Length; i++)
                {
                    Console.WriteLine(IgnoreDirs[i]);
                }
                return;
            }
            else if(ingoredIndex >= 0 && ingoredIndex + 1 < args.Length)
            {
                for (int i = ingoredIndex + 1; i < args.Length; i++)
                {
                    IgnoreDirs = IgnoreDirs.Concat(new string[] { args[i] } ).ToArray();
                }
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

            int focusIndex = Array.IndexOf(args, "--focus");

            string root = Directory.GetCurrentDirectory();
            if(focusIndex >= 0 && focusIndex + 1 < args.Length)
            {
                string focusDir = args[focusIndex + 1];
                if(Directory.Exists(focusDir))
                {
                    TreePrinter.PrintDirectory(root, "   ", IgnoreDirs, 1, depth, focusDir);
                    return;
                }
                else
                {
                    Console.WriteLine($"The directory '{focusDir}' does not exist.");
                    return;
                }
            }

            TreePrinter.PrintDirectory(root, "   ", IgnoreDirs, 1, depth);
        }
    }
}
