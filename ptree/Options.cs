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

        public static int deep = 5;
        public static string[] ignore = new string[] { };
        public static string[] focus = new string[] { };
        public static string[] collapse = new string[] { };
        public static string log = string.Empty;
        public static bool isCount = false;
        public static bool isNoIgnore = false;  

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

            var countOption = new Option<bool>(name: "--count")
            {
                Description = "Display count of files and directories instead of tree.",
                Arity = ArgumentArity.Zero
            };

            var noignoreOpt = new Option<bool>(name: "--no-ignore")
            {
                Description = "Do not ignore any directories.",
                Arity = ArgumentArity.Zero
            };

            showCommand.Add(deepOption);
            showCommand.Add(ignoreOpt);
            showCommand.Add(focusOpt);
            showCommand.Add(collapseOpt);
            showCommand.Add(logOption);
            showCommand.Add(countOption);
            showCommand.Add(noignoreOpt);

            Parser = (context) =>
            {
                deep = context.GetValue(deepOption);
                isNoIgnore = context.GetValue(noignoreOpt);

                if(isNoIgnore)
                {
                    ignore = context.GetValue(ignoreOpt) ?? new string[] {};
                }
                else
                {
                    ignore = Program.IgnoreDirs.Concat(context.GetValue(ignoreOpt) ?? new string[] { }).ToArray();
                }

                focus = context.GetValue(focusOpt) ?? new[] { "focus" };
                collapse = context.GetValue(collapseOpt) ?? new[] { "collapse" };
                log = context.GetValue(logOption) ?? "log";
                isCount = context.GetValue(countOption);

                Action.Invoke();
            };

            showCommand.SetAction(Parser);

            rootCommand.Subcommands.Add(showCommand);

            return rootCommand;
        }
    }
}
