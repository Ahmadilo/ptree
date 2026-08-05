using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TextCopy;

namespace ptree
{
    internal class PrintTree
    {
        static StringBuilder TreeRender = new StringBuilder();

        private static void PrintLine(Tree node, string indent, bool isLast)
        {
            Console.Write(indent);
            TreeRender.Append(indent);

            string branch = isLast ? "└── " : "├── ";
            Console.Write(branch);
            TreeRender.Append(branch);

            string line;

            if (node.isFile)
            {
                line = node.Name;
            }
            else if (node.isCollapse)
            {
                if (Options.isCount || Options.counts.Contains(node.Name))
                    line = $"{node.Name}/ (collapsed, {node.Files()} Files)";
                else
                    line = $"{node.Name}/ (collapsed)";
            }
            else if (Options.isCount || Options.counts.Contains(node.Name))
            {
                line = $"{node.Name}/ ({node.Files()} Files)";
            }
            else
            {
                line = $"{node.Name}/";
            }

            Console.WriteLine(line);
            TreeRender.AppendLine(line);
        }


        private static bool Stop(Tree node)
        {
            if (node == null)
                return true;

            if (node.isFile)
                return true;

            if (node.isCollapse)
                return true;

            return false;
        }

        private static void Print(Tree root)
        {
            TreeRender.AppendLine(Program.Argemnts);
            TreeRender.AppendLine();

            Console.WriteLine(root.Name); // اطبع root بدون خطوط
            TreeRender.AppendLine(root.Name);

            for (int i = 0; i < root.Children.Count; i++)
            {
                var child = root.Children[i];
                bool lastChild = (i == root.Children.Count - 1);

                Print(child, "", lastChild);
            }
        }

        private static void Print(Tree node, string indent, bool isLast)
        {
            if (node.isFile && Options.isNotFiles)
                return;

            PrintLine(node, indent, isLast);

            if (Stop(node))
                return;

            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                bool lastChild = (i == node.Children.Count - 1);

                Print(
                    child,
                    indent + (isLast ? "    " : "│   "),
                    lastChild
                );
            }
        }

        private static void LogTree()
        {
            if (Options.log == string.Empty)
                return;
            try
            {
                File.WriteAllText(Options.log, TreeRender.ToString());
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }

                Console.WriteLine(e.Message);
            }
        }

        public static void Run()
        {
            
            Tree root = Tree.Scan(Options.path, Options.deep, Options.ignore);
            //if(Options.isCount)
            //{
            //    root.ComputeFileCount();
            //}
            root.Focus(Options.focus);
            root.Collapse(Options.collapse);
            Print(root);
            if(Options.isCopy == true)
            {
                ClipboardService.SetText(TreeRender.ToString());
            }
            LogTree();
        }
    }
}
