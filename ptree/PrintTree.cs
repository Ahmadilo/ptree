using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ptree
{
    internal class PrintTree
    {
        private static void Print(Tree root)
        {
            Console.WriteLine(root.Name); // اطبع root بدون خطوط

            for (int i = 0; i < root.Children.Count; i++)
            {
                var child = root.Children[i];
                bool lastChild = (i == root.Children.Count - 1);

                Print(child, "", lastChild);
            }
        }

        private static void Print(Tree node, string indent, bool isLast)
        {
            // اطبع اسم العقدة الحالية
            Console.Write(indent);

            if (isLast)
                Console.Write("└── ");
            else
                Console.Write("├── ");

            // طباعة الاسم مع تمييز بسيط للحالات
            if (node.isFile)
            {
                Console.WriteLine(node.Name);
                return;
            }

            if (node.isCollapse && Options.isCount)
            {
                Console.WriteLine(node.Name + $"/ (collapsed, {node.Files()} Files)");
                return;
            }
            else if(node.isCollapse)
            {
                Console.WriteLine(node.Name + "/ (collapsed)");
                return;
            }
            else if(Options.isCount)
            {
                Console.WriteLine(node.Name + $"/ ({node.Files()} Files)");
            }
            else
            {
                Console.WriteLine(node.Name + "/");
            }


            // إذا لم يكن collapse → اطبع الأطفال
            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                bool lastChild = (i == node.Children.Count - 1);

                Print(child, indent + (isLast ? "    " : "│   "), lastChild);
            }
        }

        private static void LogJson(Tree root)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(root, options);

            File.WriteAllText("ptree-debug.json", json);
        }


        public static void Run()
        {
            
            Tree root = Tree.Scan(Directory.GetCurrentDirectory(), Options.deep, Options.ignore);
            //if(Options.isCount)
            //{
            //    root.ComputeFileCount();
            //}
            root.Focus(Options.focus);
            root.Collapse(Options.collapse);
            //LogJson(root);
            Print(root);
        }
    }
}
