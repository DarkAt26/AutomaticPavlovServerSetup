using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticPavlovServerSetup
{
    internal class Core
    {
        public static void Main()
        {
            Console.WriteLine("Hello PavServer");

            RunCommandWithBash("sudo apt update && sudo apt install -y gdb curl lib32gcc1 libc++-dev unzip");

            Console.WriteLine("Bye PavServer");
        }


        public static string RunCommandWithBash(string command)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = "/bin/bash";
            psi.Arguments = command;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using var process = Process.Start(psi);

            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();

            return output;
        }
    }
}
