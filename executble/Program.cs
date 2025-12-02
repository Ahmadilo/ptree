using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        string exePath = @"C:\Users\User\source\repos\ptree\ptree\bin\Debug\net8.0\ptree.exe";
        string workingDir = @"C:\Projects\librarys\java-script-librarys\react-hooks-library";

        var process = new Process();
        process.StartInfo.FileName = exePath;
        process.StartInfo.Arguments = "show --deep 2";
        process.StartInfo.WorkingDirectory = workingDir;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        // قراءة نتائج التشغيل
        string output = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        // طباعة النتائج
        Console.WriteLine("=== OUTPUT ===");
        Console.WriteLine(output);

        if (!string.IsNullOrWhiteSpace(errors))
        {
            Console.WriteLine("=== ERRORS ===");
            Console.WriteLine(errors);
        }
    }
}
