using System;
using System.IO;
using System.Linq;

namespace ptree
{
    internal class IgnorePolicy
    {
        private readonly string[] nameIgnores;
        private readonly string scanRoot;
        private readonly Ignore.Ignore? gitignore;

        public IgnorePolicy(string[] nameIgnores, string scanRoot, bool loadGitignore)
        {
            this.nameIgnores = nameIgnores ?? new string[] { };
            this.scanRoot = scanRoot;
            this.gitignore = loadGitignore ? LoadGitignore(scanRoot) : null;
        }

        private static Ignore.Ignore? LoadGitignore(string scanRoot)
        {
            string path = Path.Combine(scanRoot, ".gitignore");

            if (!File.Exists(path))
                return null;

            try
            {
                string[] rules = File.ReadAllLines(path)
                    .Select(line => line.Trim())
                    .Where(line => line.Length > 0 && !line.StartsWith("#"))
                    .ToArray();

                var ignore = new Ignore.Ignore();
                ignore.Add(rules);
                return ignore;
            }
            catch
            {
                return null;
            }
        }

        public bool IsIgnored(FileSystemInfo entry)
        {
            if (nameIgnores.Contains(entry.Name, StringComparer.OrdinalIgnoreCase))
                return true;

            if (gitignore == null)
                return false;

            string relativePath =
                Path.GetRelativePath(scanRoot, entry.FullName).Replace('\\', '/');

            return gitignore.IsIgnored(relativePath);
        }
    }
}
