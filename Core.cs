using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using AutomaticPavlovServerSetup;
using Newtonsoft.Json;


namespace AutomaticPavlovServerSetup
{
    internal class Core
    {
        public static RootObject serverConfig = new RootObject();
        static void Main()
        {
            Console.WriteLine("-AutomaticPavlovServerSetup-");

            string configPath = Path.Combine(Directory.GetCurrentDirectory(), "APSS_SetupConfig.json");

            if (File.Exists(configPath))
            {
                serverConfig = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(configPath));
            }
            else
            {
                CreateFile(configPath, JsonConvert.SerializeObject(serverConfig, Formatting.Indented));
                Console.WriteLine(JsonConvert.SerializeObject(serverConfig, Formatting.Indented));
            }

            if (JsonConvert.SerializeObject(new RootObject(), Formatting.Indented) == JsonConvert.SerializeObject(serverConfig, Formatting.Indented))
            {
                Console.WriteLine("Failed. Setup APSS_SetupConfig.json");
                return;
            }

            Console.WriteLine("InputCommand (Setup/Update): ");

            string input = Console.ReadLine();

            DateTime startTime = DateTime.UtcNow;

            Console.WriteLine(startTime + " Starting...");


            switch (input.ToLower())
            {
                case "setup":
                    SetupServer();
                    break;

                case "update":
                    UpdateServer();
                    break;
            }
            
            DateTime endTime = DateTime.UtcNow;

            TimeSpan time = endTime - startTime;

            Console.WriteLine(endTime + " Done. Finished in " + time.Minutes + "min " + time.Seconds + "s.");
            Console.WriteLine("-AutomaticPavlovServerSetup-");
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

        public static void OpenPorts(List<int> ports)
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

        public static void SteamCMDCommand(string arguments)
        {
            Console.WriteLine(DateTime.UtcNow);

            string steamCmdPath = "/home/steam/Steam/steamcmd.sh";

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
        }

        public static void SetupServer()
        {
            Command("sudo apt update && sudo apt install -y gdb curl lib32gcc1 libc++-dev unzip");
            AddSteamUser(serverConfig.SteamPassword);
            Command("su - steam -c 'mkdir -p /home/steam/Steam && cd /home/steam/Steam && curl -sqL \"https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz\" | tar zxvf -'");

            Command("echo \"" + serverConfig.SteamPassword + "\" | sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so");
            Command("echo \"" + serverConfig.SteamPassword + "\" | sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so");


            SteamCMDCommand("+force_install_dir /home/steam/pavlovserver +login anonymous +app_update 622970 " + serverConfig.Platform + " +quit");
            SteamCMDCommand("+login anonymous +app_update 1007 +quit");


            Command("sudo -u steam mkdir -p /home/steam/.steam/sdk64");

            Command("echo \"" + serverConfig.SteamPassword + "\" | sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so");
            Command("echo \"" + serverConfig.SteamPassword + "\" | sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so");
            
            Command("sudo -u steam chmod +x /home/steam/pavlovserver/PavlovServer.sh");

            Command("sudo -u steam mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Logs");
            Command("sudo -u steam mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Config/LinuxServer");
            Command("sudo -u steam mkdir -p /home/steam/pavlovserver/Pavlov/Saved/Mods");


            List<int> ports = new List<int>();
            ports.AddRange(serverConfig.StandardPorts);
            ports.Add(serverConfig.RconSettings.Port);
            

            OpenPorts(ports);
            DisableIPv6();

            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/mods.txt", string.Join("\r\n", serverConfig.Mods));
            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/whitelist.txt", string.Join("\r\n", serverConfig.Whitelist));
            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/blacklist.txt", string.Join("\r\n", serverConfig.Blacklist));

            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/RconSettings.txt", "Password=" + serverConfig.RconSettings.Password + "\r\n" + "Port=" + serverConfig.RconSettings.Port);

            string MapRotationString = "";
            foreach (string s in serverConfig.ConfigSetup.MapRotation)
            {
                MapRotationString += "MapRotation=(MapId=\"" + s.Split(' ')[0] + "\", GameMode=\"" + s.Split(' ')[1] + "\")\r\n";
            }

            CreateFile("/home/steam/pavlovserver/Pavlov/Saved/Config/LinuxServer/Game.ini", serverConfig.ConfigSetup.Config + "\r\n\r\n" + MapRotationString + string.Join("\r\nAdditionalMods=", serverConfig.ConfigSetup.AdditionalMods));

            CreateFile("/etc/systemd/system/pavlovserver.service", serverConfig.PavlovService);

            if (serverConfig.StartServerAfterCompletion)
            {
                Command("sudo systemctl start pavlovserver");
            }
        }
    
        public static void UpdateServer()
        {
            Command("sudo systemctl stop pavlovserver");

            SteamCMDCommand("+force_install_dir /home/steam/pavlovserver +login anonymous +app_update 622970 " + serverConfig.Platform + " +quit");
            SteamCMDCommand("+login anonymous +app_update 1007 +quit");

            Command("sudo -u steam mkdir -p /home/steam/.steam/sdk64");

            Command("echo \"" + serverConfig.SteamPassword + "\" | sudo -S rm /usr/lib/x86_64-linux-gnu/libc++.so");
            Command("echo \"" + serverConfig.SteamPassword + "\" | sudo -S ln -s /usr/lib/x86_64-linux-gnu/libc++.so.1 /usr/lib/x86_64-linux-gnu/libc++.so");

            Command("sudo systemctl start pavlovserver");
        }
    }



    public class RconSettings
    {
        public int Port { get; set; } = 9100;
        public string Password { get; set; } = "";
    }

    public class ConfigSetup
    {
        public string Config { get; set; } = "[/Script/Pavlov.DedicatedServer]\nbEnabled=true\nServerName=\"AutomaticPavlovServerSetup\" \nMaxPlayers=10\nApiKey=\"ABC123FALSEKEYDONTUSEME\"\nbSecured=true\nbCustomServer=true \nbVerboseLogging=false \nbCompetitive=false\nbWhitelist=false \nRefreshListTime=120 \nLimitedAmmoType=0 \nTickRate=90\nTimeLimit=60\nAFKTimeLimit=300\n#Password=0000 \n#BalanceTableURL=\"vankruptgames/BalancingTable/main\"";
        public List<string> MapRotation { get; set; } = new List<string> { "UGC1758245796 GUN", "datacenter SND", "sand DM" };
        public List<string> AdditionalMods { get; set; } = new List<string> { "UGC3462586" };
    }

    public class RootObject
    {
        public List<int> StandardPorts { get; set; } = new List<int> { 7777, 8177 };
        public RconSettings RconSettings { get; set; } = new RconSettings();
        public List<string> Mods { get; set; } = new List<string>();
        public List<string> Whitelist { get; set; } = new List<string>();
        public List<string> Blacklist { get; set; } = new List<string>();
        public string PavlovService { get; set; } = "[Unit]\nDescription=Pavlov VR dedicated server\n\n[Service]\nType=simple\nWorkingDirectory=/home/steam/pavlovserver\nExecStart=/home/steam/pavlovserver/PavlovServer.sh\n\nRestartSec=1\nRestart=always\nUser=steam\nGroup=steam\n\n[Install]\nWantedBy = multi-user.target";
        public ConfigSetup ConfigSetup { get; set; } = new ConfigSetup();
        public bool StartServerAfterCompletion { get; set; } = true;
        public string SteamPassword { get; set; } = "pwdSt3am";
        public string Platform { get; set; } = "-beta shack";
    }

}
