//Use this command to download the required file from github.
```
wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS
```

//Use this command to make the downloaded file an executeable.
```
chmod +x APSS
```

//Use this command to run the executeable. It will create a setup config if none is provided or existing already.
```
./APSS
```
//Use this command to run the executeable with the "update" argument. This will make the server update instead of setting up a new install.
```
./APSS update
```

//Use this command to edit the SetupConfig.
```
nano APSS_SetupConfig.json
```

//Use this command to setup the server with a setup config directly. after the ./APSS you need to add a ' , then your minified json string of the setup config and then another '. You can minify your json setupconfig here https://www.browserling.com/tools/json-minify. A json setup config example can be found in the repo https://github.com/DarkAt26/AutomaticPavlovServerSetup/blob/master/APSS_SetupConfig.json.
```
./APSS '{"Platform":"-beta shack","RconPort":9100,"RconPassword":"","Config":"[/Script/Pavlov.DedicatedServer]\r\nbEnabled=true\r\nServerName=\"AutomaticPavlovServerSetup\" \r\nMaxPlayers=10\r\nServerKey=\"\"\r\nbSecured=true\r\nbCustomServer=true \r\nbVerboseLogging=false \r\nbCompetitive=false\r\nbWhitelist=false \r\nRefreshListTime=120 \r\nLimitedAmmoType=0 \r\nTickRate=90\r\nTimeLimit=60\r\nAFKTimeLimit=300\r\n#Password=0000 \r\n#BalanceTableURL=\"vankruptgames/BalancingTable/main\"","MapRotation":["UGC1758245796 GUN","datacenter SND","sand DM"],"AdditionalMods":["UGC3462586"],"Mods":[],"Whitelist":[],"Blacklist":[],"StandardPorts":[7777,8177],"SteamPassword":"pwdSt3am","StartServerAfterCompletion":true}'
```
//The "wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS", "chmod +x APSS" and "./APSS" + optionally "update" or a config can be combined like this, to install the code, make it an executeable and run it with just one line of commands.
```
wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS && chmod +x APSS && ./APSS
```

//This can then also look like this using the default setup config. A single command line to setup a full pavlov server.
```
wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS && chmod +x APSS && ./APSS '{"Platform":"-beta shack","RconPort":9100,"RconPassword":"","Config":"[/Script/Pavlov.DedicatedServer]\r\nbEnabled=true\r\nServerName=\"AutomaticPavlovServerSetup\" \r\nMaxPlayers=10\r\nServerKey=\"\"\r\nbSecured=true\r\nbCustomServer=true \r\nbVerboseLogging=false \r\nbCompetitive=false\r\nbWhitelist=false \r\nRefreshListTime=120 \r\nLimitedAmmoType=0 \r\nTickRate=90\r\nTimeLimit=60\r\nAFKTimeLimit=300\r\n#Password=0000 \r\n#BalanceTableURL=\"vankruptgames/BalancingTable/main\"","MapRotation":["UGC1758245796 GUN","datacenter SND","sand DM"],"AdditionalMods":["UGC3462586"],"Mods":[],"Whitelist":[],"Blacklist":[],"StandardPorts":[7777,8177],"SteamPassword":"pwdSt3am","StartServerAfterCompletion":true}'
```

//SetupConfig explaination:<br>
"Platform": Here you set what type of server it should install. "" <- PC; "-beta shack" <- Shack<br>
"RconPort": Set the number of the port you want to use for RCON here. This port will be opend.<br>
"RconPassword": Set the password you want to use for RCON here.<br>
"Config": This should contain the config from the Game.ini.<br>
"MapRotation": To this array you can add a map rotation. This can be done in the "Config" too but doing it here makes it more readable an is easier to change.<br>
"AdditionalMods": Same as "MapRotation" but for the ugc mods.<br>
"Mods": Here you can add the id's of Moderators.<br>
"Whitelist": Here you can add the id's of people you want to whitelist.<br>
"Blacklist": Here you can add the id's of people you want to blacklist.<br>
"StandardPorts": These ports will be opend.<br>
"SteamPassword": This is the password of the steam user on the server. Can be changed if wanted.<br>
"StartServerAfterCompletion": true or false. If true it starts the pavlov server using "sudo systemctl start pavlovserver".<br>



//There is also a batch file to easily connect to a server and a batch file to clear all known hosts(ssh). https://github.com/DarkAt26/AutomaticPavlovServerSetup/tree/master/Batch
