using System;
using System.IO;

namespace ptree
{
    class Program
    {
        public static string[] IgnoreDirs = new[]
        {
            "node_modules", "vendor", ".git", "bin", "obj"
        };
        public static string Argemnts = string.Empty;
        static int Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Directory.SetCurrentDirectory("C:\\Users\\User");
            var root = Options.GetRootCommand();
            args = new string[] { "show", "--deep", "2", "--no-files" };
            Argemnts = "ptree " + string.Join(" ", args);
            return root.Parse(args).Invoke();
        }
    }
}
