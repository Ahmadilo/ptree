using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptree
{
    internal class Tree
    {
        public string FullPathName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; 
        public bool isFile { get; set; }
        public bool isFocus { get; set; }
        public bool isCollapse { get; set; }
        public List<Tree> Children { get; set; } = new List<Tree>();
        public int FileCount { get; set; } = 0;

        private static void ScanDirectory(Tree parent, DirectoryInfo dir, int level, int maxDepth, string[] ignore)
        {
            // لا ننزل أكثر من العمق المسموح
            if (level > maxDepth)
                return;
            try
            {
                // قراءة الملفات والمجلدات
                FileSystemInfo[] entries = dir.GetFileSystemInfos();

                foreach (var entry in entries)
                {
                    // تجاهل المجلدات المحددة
                    if (ignore.Contains(entry.Name, StringComparer.OrdinalIgnoreCase))
                        continue;

                    // إنشاء node جديد
                    var node = new Tree
                    {
                        Name = entry.Name,
                        FullPathName = entry.FullName,
                        isFile = entry is FileInfo,
                    };

                    parent.Children.Add(node);

                    // لو كان مجلد → اكمل المسح
                    if (entry is DirectoryInfo subDir)
                    {
                        ScanDirectory(node, subDir, level + 1, maxDepth, ignore);
                    }
                }
            }
            catch(UnauthorizedAccessException)
            {
                parent.Name += " (Access Denied)";
            }
            catch (Exception ex)
            {
                // لأي أخطاء أخرى غير متوقعة
            }

        }

        public static Tree Scan(string rootPath, int deep, string[] ignore)
        {
            DirectoryInfo rootDir = new DirectoryInfo(rootPath);

            var root = new Tree
            {
                FullPathName = rootDir.FullName,
                Name = rootDir.Name,
                isFile = false
            };

            ScanDirectory(root, rootDir, 1, deep, ignore);

            return root;
        }

        public void Collapse(string[] collapse)
        {
            // لو ما في شيء نطويه → لا تفعل شيء
            if (collapse == null || collapse.Length == 0)
                return;

            Queue<Tree> queue = new Queue<Tree>();
            queue.Enqueue(this); // ابدأ من الجذر

            while (queue.Count > 0)
            {
                Tree node = queue.Dequeue();

                // إذا اسم المجلد يطابق collapse list → اعمل collapse
                // ولا تنزل داخل الأطفال
                if (collapse.Contains(node.Name, StringComparer.OrdinalIgnoreCase))
                {
                    node.isCollapse = true;
                    continue; // VERY important: skip children
                }

                // نضيف الأطفال للبحث BFS
                foreach (var child in node.Children)
                {
                    // Collapse منطقي فقط للمجلدات، ليس للملفات
                    if (!child.isFile)
                        queue.Enqueue(child);
                }
            }
        }

        public void Focus(string[] focusList)
        {
            if (focusList == null || focusList.Length == 0)
                return;

            // VERY IMPORTANT: collapse everything first
            CollapseAll(this);

            // Then open only the focused path
            ApplyFocus(this, focusList);
        }

        private void CollapseAll(Tree node)
        {
            if (!node.isFile)
                node.isCollapse = true;

            foreach (var child in node.Children)
                CollapseAll(child);
        }

        private void Apply_BFS(Tree Node, Action<Tree> apply)
        {
            if (Node == null)
                return;
            Queue<Tree> queue = new Queue<Tree>();

            queue.Enqueue(Node);

            while (queue.Count > 0)
            {
                Tree current = queue.Dequeue();
                apply(current);
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        private bool ApplyFocus(Tree node, string[] focusList)
        {
            bool isFocusedNode =
                focusList.Contains(node.Name, StringComparer.OrdinalIgnoreCase);

            bool shouldExpand = isFocusedNode;

            foreach (var child in node.Children)
            {
                if (ApplyFocus(child, focusList))
                    shouldExpand = true;
            }

            // إذا node هو هدف focus
            if (isFocusedNode)
            {
                node.isCollapse = false;
                node.isFocus = true;

                // 👇 افتح جميع أطفاله مباشرة
                Apply_BFS(node, n =>
                {
                    if (!n.isFile)
                    {
                        n.isCollapse = false;
                    }
                });
            }
            else if (shouldExpand)
            {
                // عقدة في المسار إلى target
                node.isCollapse = false;
                node.isFocus = true;
            }

            return shouldExpand;
        }

        public int ComputeFileCount()
        {
            // إذا node ملف → 1
            if (isFile)
            {
                FileCount = 1;
                return 1;
            }

            int sum = 0;

            foreach (var child in Children)
            {
                sum += child.ComputeFileCount();
            }

            FileCount = sum;
            return sum;
        }

        public static int CountFiles(string path, string[] ignore)
        {
            int count = 0;

            try
            {
                foreach (var entry in Directory.EnumerateFileSystemEntries(path))
                {
                    string name = Path.GetFileName(entry);

                    if (ignore.Contains(name, StringComparer.OrdinalIgnoreCase))
                        continue;

                    if (File.Exists(entry))
                    {
                        count++;
                    }
                    else if (Directory.Exists(entry))
                    {
                        count += CountFiles(entry, ignore);
                    }
                }
            }
            catch
            {
                // access denied, symlink loops, etc.
            }

            return count;
        }

        public int Files()
        {
            return CountFiles(this.FullPathName, Options.ignore);
        }
    }
}
