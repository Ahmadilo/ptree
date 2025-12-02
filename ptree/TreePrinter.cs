using System;
using System.IO;
using System.Linq;

namespace ptree
{
    public static class TreePrinter
    {
        public static void PrintDirectory(string path, string indent, string[] ignore)
        {
            var dirInfo = new DirectoryInfo(path);

            // طباعة اسم المجلد الجذري فقط
            if (indent == "")
                Console.WriteLine(dirInfo.Name);

            var items = dirInfo.GetFileSystemInfos()
                               .Where(f => !ignore.Contains(f.Name))
                               .OrderBy(f => f is DirectoryInfo ? 0 : 1)
                               .ThenBy(f => f.Name);

            foreach (var item in items)
            {
                bool last = item == items.Last();

                Console.Write(indent);
                Console.Write(last ? "└── " : "├── ");
                Console.WriteLine(item.Name);

                if (item is DirectoryInfo)
                {
                    PrintDirectory(
                        item.FullName,
                        indent + (last ? "    " : "│   "),
                        ignore
                    );
                }
            }
        }
    }

}

