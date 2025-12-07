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

        static int Main(string[] args)
        {
            var root = Options.GetRootCommand();
            return root.Parse(args).Invoke();
        }
    }
}
