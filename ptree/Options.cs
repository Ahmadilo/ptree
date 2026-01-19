using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ptree
{
    internal class Options
    {
        // ptree commands
        // show:
        //  --deep <int>
        //  --ignore <dir1,dir2,...>
        //  --focus <dir1, dir2,...>
        //  --collapse <dir1, dir2,...>
        //  --log <fileName>
        //  --count <dir1, dir2, ...>
        //  --count-all
        //  --from <path>

        public static string path = Directory.GetCurrentDirectory();
        public static string from = string.Empty;
        public static int deep = 5;
        public static string[] ignore = new string[] { };
        public static string[] focus = new string[] { };
        public static string[] collapse = new string[] { };
        public static string[] counts = new string[] { };
        public static string log = string.Empty;
        public static bool isCount = false;
        public static bool isNoIgnore = false;
        public static bool isNotFiles = false;

        static string[] ReadGitIgnore(string rootPath)
        {
            string gitignorePath = Path.Combine(rootPath, ".gitignore");

            if (!File.Exists(gitignorePath))
                return Array.Empty<string>();

            return File.ReadAllLines(gitignorePath)
                .Select(line => line.Trim())
                .Where(line =>
                    !string.IsNullOrWhiteSpace(line) &&
                    !line.StartsWith("#") &&
                    line.EndsWith("/"))
                .Select(line => line.TrimEnd('/'))
                .ToArray();
        }


        static Action<ParseResult> Parser = (context) => { };

        static Action Action = () =>
        {
            PrintTree.Run();
        };

        public static RootCommand GetRootCommand()
        {
            var rootCommand = new RootCommand("ptree - A directory tree viewer with advanced filtering options.");

            var showCommand = new Command("show", "Display the directory tree with specified options.");

            var deepOption = new Option<int>("--deep")
            {
                Description = "Depth of directory tree to display.",
                Arity = ArgumentArity.ZeroOrOne,
                DefaultValueFactory = (a) => 5
            };

            var ignoreOpt = new Option<string[]>(name: "--ignore")
            {
                Description = "Directories to ignore (comma-separated).",
                Arity = ArgumentArity.OneOrMore,
                AllowMultipleArgumentsPerToken = true
            };

            var focusOpt = new Option<string[]>(name: "--focus")
            {
                Description = "Directories to focus on (comma-separated).",
                Arity = ArgumentArity.OneOrMore,
                AllowMultipleArgumentsPerToken = true
            };

            var collapseOpt = new Option<string[]>(name: "--collapse")
            {
                Description = "Directories to collapse (comma-separated).",
                Arity = ArgumentArity.OneOrMore,
                AllowMultipleArgumentsPerToken = true
            };

            var logOption = new Option<string>(name: "--log")
            {
                Description = "Log output to specified file.",
                Arity = ArgumentArity.ExactlyOne
            };

            var countOption = new Option<string[]>(name: "--count")
            {
                Description = "Display count of Files in Directres that you chose.",
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true
            };

            var countAllOption = new Option<bool>(name: "--count-all")
            {
                Description = "Display count of Files in All Directres in Project.",
                Arity = ArgumentArity.ZeroOrOne
            };

            var noignoreOpt = new Option<bool>(name: "--no-ignore")
            {
                Description = "Do not ignore any directories.",
                Arity = ArgumentArity.Zero
            };

            var nofilesOpt = new Option<bool>(name: "--no-files")
            {
                Description = "hide the all files",
                Arity = ArgumentArity.Zero
            };

            var fromOpt = new Option<string>(name: "--from")
            { 
                Description = "Change the Scan from root to any directore under the root",
                Arity = ArgumentArity.ExactlyOne,
            };


            showCommand.Add(deepOption);
            showCommand.Add(ignoreOpt);
            showCommand.Add(focusOpt);
            showCommand.Add(collapseOpt);
            showCommand.Add(logOption);
            showCommand.Add(countOption);
            showCommand.Add(noignoreOpt);
            showCommand.Add(nofilesOpt);
            showCommand.Add(fromOpt);
            showCommand.Add(countAllOption);

            Parser = (context) =>
            {
                deep = context.GetValue(deepOption);
                isNoIgnore = context.GetValue(noignoreOpt);

                from = context.GetValue(fromOpt) ?? string.Empty;

                if(from != string.Empty)
                {
                    from = from.Trim();
                    from = from.TrimStart('/');
                    string frompath = Path.Combine(path, from);

                    if(!Directory.Exists(frompath))
                    {
                        Console.WriteLine(frompath + " is not Exist!!!");
                        return;
                    }

                    path = frompath;
                }

                if(isNoIgnore)
                {
                    ignore = context.GetValue(ignoreOpt) ?? new string[] {};
                }
                else
                {
                    ignore = Program.IgnoreDirs.Concat(context.GetValue(ignoreOpt) ?? new string[] { }).ToArray();
                    //string[] names = ReadGitIgnore(Directory.GetCurrentDirectory());

                    //if(names.Length > 0)
                    //{
                    //    ignore = ignore.ToHashSet().Concat(names.ToHashSet(StringComparer.OrdinalIgnoreCase)).ToArray();
                    //}
                }

                focus = context.GetValue(focusOpt) ?? new[] { "" };
                collapse = context.GetValue(collapseOpt) ?? new[] { "" };
                log = context.GetValue(logOption) ?? string.Empty;
                isCount = context.GetValue(countAllOption);
                isNotFiles = context.GetValue(nofilesOpt);
                counts = context.GetValue(countOption) ?? new[] { "" };

                Action.Invoke();
            };

            showCommand.SetAction(Parser);

            rootCommand.Subcommands.Add(showCommand);

            return rootCommand;
        }
    }
}
