using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticPavlovServerSetup
{
    internal class Core
    {
        static void Main()
        {
            Console.WriteLine("Hey");
            foreach (string command in commands)
            {
                Command(command);
            }

            Console.WriteLine("Bye");
        }

        public static void Command(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        static string[] commands = {
            "sudo apt update && sudo apt install -y gdb curl lib32gcc1 libc++-dev unzip",
            "sudo useradd -m steam",
            "adduser steam sudo",
            "sudo chsh -s /bin/bash steam",
            "sudo passwd steam",
            "pwdSt3am",
            "pwdSt3am",
            "su - steam -c \"mkdir /home/steam/Steam && cd ~/Steam && curl -sqL \"https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz\" | tar zxvf -\"",
            "echo \"pwdSt3am\" | su - steam -c \"sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so\"",
            "echo \"pwdSt3am\" | su - steam -c \"sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so\"",
            "su - steam -c \"/home/steam/Steam/steamcmd.sh +force_install_dir /home/steam/pavlovserver +login anonymous +app_update 622970 -beta shack +Exit\"",
            "su - steam -c \"/home/steam/Steam/steamcmd.sh +login anonymous +app_update 1007 +quit\"",
            "su - steam -c \"mkdir -p /home/steam/.steam/sdk64\"",
            "su - steam -c \"cp /home/steam/Steam/steamapps/common/Steamworks\\ SDK\\ Redist/linux64/steamclient.so ~/.steam/sdk64/steamclient.so\"",
            "su - steam -c \"cp /home/steam/Steam/steamapps/common/Steamworks\\ SDK\\ Redist/linux64/steamclient.so ~/pavlovserver/Pavlov/Binaries/Linux/steamclient.so\"",
            "echo \"pwdSt3am\" | su - steam -c \"sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so\"",
            "echo \"pwdSt3am\" | su - steam -c \"sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so\"",
            "su - steam -c \"chmod +x ~/pavlovserver/PavlovServer.sh\"",
            "su - steam -c \"mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Logs\"",
            "su - steam -c \"mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Config/LinuxServer\"",
            "su - steam -c \"mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Mods\"",
            "sudo ufw status",
            "sudo ufw allow 7777",
            "sudo ufw allow 8177",
            "sudo ufw allow 9100",
            "sudo ufw status",
            "sysctl -w net.ipv6.conf.all.disable_ipv6=1",
            "sysctl -w net.ipv6.conf.default.disable_ipv6=1",
            "sysctl -w net.ipv6.conf.lo.disable_ipv6=1"
            
                
            //    ,
            //""
        };
    }
}
