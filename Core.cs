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

            Command("sudo apt update && sudo apt install -y gdb curl lib32gcc1 libc++-dev unzip");
            AddSteamUser("pwdSt3am");
            Command("su - steam -c 'mkdir -p /home/steam/Steam && cd /home/steam/Steam && curl -sqL \"https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz\" | tar zxvf -'");
            Command("echo \"pwdSt3am\" | sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so");
            Command("echo \"pwdSt3am\" | sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so");
            //CommandSteam("sudo su -l steam -c \"/home/steam/Steam/steamcmd.sh +force_install_dir /home/steam/pavlovserver +login anonymous +app_update 622970 -beta shack +exit\"");

            Console.WriteLine(DateTime.UtcNow);

            string steamCmdPath = "/home/steam/Steam/steamcmd.sh";
            string arguments = "+force_install_dir /home/steam/pavlovserver +login anonymous +app_update 622970 -beta shack +quit";

            // Run the command as the 'steam' user
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"runuser -l steam -c '{steamCmdPath} {arguments}'\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };


            // Start the process
            using (Process process = new Process { StartInfo = psi })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine("ERROR: " + e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();  // Wait until the process completes
            }

            Console.WriteLine("SteamCMD process completed.");
            Console.WriteLine(DateTime.UtcNow);

            arguments = "+login anonymous +app_update 1007 +quit";

            // Run the command as the 'steam' user
            ProcessStartInfo psi2 = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"runuser -l steam -c '{steamCmdPath} {arguments}'\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };


            // Start the process
            using (Process process = new Process { StartInfo = psi2 })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine("ERROR: " + e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();  // Wait until the process completes
            }

            Console.WriteLine("SteamCMD1007 process completed.");
            Console.WriteLine(DateTime.UtcNow);




            //Command("su - steam -c \"/home/steam/Steam/steamcmd.sh +login anonymous +app_update 1007 +quit\"");
            Command("sudo -u steam mkdir -p /home/steam/.steam/sdk64");
            Command("echo \"pwdSt3am\" | sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so");
            Command("echo \"pwdSt3am\" | sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so");
            Command("sudo -u steam chmod +x /home/steam/pavlovserver/PavlovServer.sh");

            Command("sudo -u steam mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Logs");
            Command("sudo -u steam mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Config/LinuxServer");
            Command("sudo -u steam mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Mods");


            int[] ports = { 7777, 8177, 9100 };

            OpenPorts(ports);
            DisableIPv6();

            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/mods.txt", "");
            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/whitelist.txt", "");
            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/RconSettings.txt", "");

            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/LinuxServer/Game.ini", "[/Script/Pavlov.DedicatedServer]\r\nbEnabled=true\r\nServerName=\"My_private_idaho\" \r\nMaxPlayers=10     #its recommended for the best experience to keep shack servers at or below 10 players pcvr and psvr are both capped at 24 too. \r\nApiKey=\"ABC123FALSEKEYDONTUSEME\"\r\nbSecured=true\r\nbCustomServer=true \r\nbVerboseLogging=false \r\nbCompetitive=false #This only works for SND\r\nbWhitelist=false \r\nRefreshListTime=120 \r\nLimitedAmmoType=0 \r\nTickRate=90\r\nTimeLimit=60\r\nAFKTimeLimit=300\r\n#Password=0000 \r\n#BalanceTableURL=\"vankruptgames/BalancingTable/main\"\r\nMapRotation=(MapId=\"UGC1758245796\", GameMode=\"GUN\")\r\nMapRotation=(MapId=\"datacenter\", GameMode=\"SND\")\r\nMapRotation=(MapId=\"sand\", GameMode=\"DM\")\r\nAdditionalMods=UGC3462586");

            CreateFile("/etc/systemd/system/pavlovserver.service", "[Unit]\r\nDescription=Pavlov VR dedicated server\r\n\r\n[Service]\r\nType=simple\r\nWorkingDirectory=/home/steam/pavlovserver\r\nExecStart=/home/steam/pavlovserver/PavlovServer.sh\r\n\r\nRestartSec=1\r\nRestart=always\r\nUser=steam\r\nGroup=steam\r\n\r\n[Install]\r\nWantedBy = multi-user.target");

            Console.WriteLine("Bye");
        }

        public static void CreateDirectory(string directoryPath)
        {
            string user = "steam";
            // The command to create the directory using 'sudo' and 'mkdir'
            string command = $"sudo -u {user} mkdir -p {directoryPath}";

            // Set up the process start info to run the shell command
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash", // Use bash shell
                Arguments = "-c \"" + command + "\"", // Execute the command in bash
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Execute the process and wait for it to exit
            using (Process process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                }
            }
        }
        
        public static void Command(string command)
        {
            Console.WriteLine("Executing command: " + command);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",  // Use bash shell to execute the command
                Arguments = $"-c \"{command}\"",  // -c allows the command to be passed
                UseShellExecute = false,       // Must be false for redirection to work
                CreateNoWindow = true          // Avoid opening a new console window
            };

            using (Process process = new Process { StartInfo = psi })
            {
                    process.Start(); // Start the process
                    process.WaitForExit(); // Wait for the process to exit
            }
        }

        public static void CommandSteam(string command)
        {
            Console.WriteLine("Executing command: " + command);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",  // Use bash shell to execute the command
                Arguments = $"-c \"{command}\"",  // -c allows the command to be passed
                RedirectStandardOutput = false,  // Capture output for live stream
                RedirectStandardError = false,   // Capture error output for live stream
                UseShellExecute = false,       // Must be false for redirection to work
                CreateNoWindow = true,          // Avoid opening a new console window
                WorkingDirectory = "/home/steam"
            };

            using (Process process = new Process { StartInfo = psi })
            {
                // Start the process
                process.Start();

                // Wait for the process to exit
                process.WaitForExit();
            }
        }

        public static void AddSteamUser(string password)
        {
            Command("sudo useradd -m steam");
            Command("adduser steam sudo");
            Command("sudo chsh -s /bin/bash steam");
            Command("echo \"steam:" + password + "\" | sudo chpasswd");
        }

        public static void OpenPorts(int[] ports)
        {
            Command("sudo ufw status");
            foreach (int port in ports)
            {
                Command("sudo ufw allow " + port);
            }
            Command("sudo ufw status");
        }

        public static void DisableIPv6()
        {
            Command("sysctl -w net.ipv6.conf.all.disable_ipv6=1");
            Command("sysctl -w net.ipv6.conf.default.disable_ipv6=1");
            Command("sysctl -w net.ipv6.conf.lo.disable_ipv6=1");
        }

        public static void CreateFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);

            Console.WriteLine("File created and written successfully.");
        }
    }
}
