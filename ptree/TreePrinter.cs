using System;
using System.IO;
using System.Linq;

namespace ptree
{
    public static class TreePrinter
    {
        public static void PrintDirectory(
        string path,
        string indent,
        string[] ignore,
        int level,
        int maxDepth)
        {
            var dirInfo = new DirectoryInfo(path);

            // طباعة اسم الجذر فقط
            if (level == 1)
                Console.WriteLine(dirInfo.Name);

            // إذا وصلنا للحد الأقصى → لا ننزل أكثر
            if (level > maxDepth)
                return;

            var items = dirInfo.GetFileSystemInfos()
                               .Where(f => !ignore.Contains(f.Name))
                               .OrderBy(f => f is DirectoryInfo ? 0 : 1)
                               .ThenBy(f => f.Name)
                               .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                bool last = i == items.Count - 1;

                Console.Write(indent);
                Console.Write(last ? "└── " : "├── ");
                Console.WriteLine(item.Name);

                if (item is DirectoryInfo)
                {
                    PrintDirectory(
                        item.FullName,
                        indent + (last ? "    " : "│   "),
                        ignore,
                        level + 1,
                        maxDepth
                    );
                }
            }
        }
    }

}

