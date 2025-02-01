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
./APSS '{"StandardPorts":[7777,8177],"RconPort":9100,"RconPassword":"","Config":"[/Script/Pavlov.DedicatedServer]\nbEnabled=true\nServerName=\"AutomaticPavlovServerSetup\" \nMaxPlayers=10\nApiKey=\"ABC123FALSEKEYDONTUSEME\"\nbSecured=true\nbCustomServer=true \nbVerboseLogging=false \nbCompetitive=false\nbWhitelist=false \nRefreshListTime=120 \nLimitedAmmoType=0 \nTickRate=90\nTimeLimit=60\nAFKTimeLimit=300\n#Password=0000 \n#BalanceTableURL=\"vankruptgames/BalancingTable/main\"","MapRotation":["UGC1758245796 GUN","datacenter SND","sand DM"],"AdditionalMods":["UGC3462586"],"Mods":[],"Whitelist":[],"Blacklist":[],"StartServerAfterCompletion":true,"SteamPassword":"pwdSt3am","Platform":"-beta shack"}'
```
//The "wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS", "chmod +x APSS" and "./APSS" + optionally "update" or a config can be combined like this, to install the code, make it an executeable and run it with just one line of commands.
```
wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS && chmod +x APSS && ./APSS
```

//This can then also look like this using the default setup config. A single command line to setup a full pavlov server.
```
wget -O APSS https://github.com/DarkAt26/AutomaticPavlovServerSetup/releases/latest/download/APSS && chmod +x APSS && ./APSS '{"StandardPorts":[7777,8177],"RconPort":9100,"RconPassword":"","Config":"[/Script/Pavlov.DedicatedServer]\nbEnabled=true\nServerName=\"AutomaticPavlovServerSetup\" \nMaxPlayers=10\nApiKey=\"ABC123FALSEKEYDONTUSEME\"\nbSecured=true\nbCustomServer=true \nbVerboseLogging=false \nbCompetitive=false\nbWhitelist=false \nRefreshListTime=120 \nLimitedAmmoType=0 \nTickRate=90\nTimeLimit=60\nAFKTimeLimit=300\n#Password=0000 \n#BalanceTableURL=\"vankruptgames/BalancingTable/main\"","MapRotation":["UGC1758245796 GUN","datacenter SND","sand DM"],"AdditionalMods":["UGC3462586"],"Mods":[],"Whitelist":[],"Blacklist":[],"StartServerAfterCompletion":true,"SteamPassword":"pwdSt3am","Platform":"-beta shack"}'
```

//SetupConfig explaination:
"Platform": Here you set what type of server it should install. "" <- PC; "-beta shack" <- Shack
"RconPort": Set the number of the port you want to use for RCON here. This port will be opend.
"RconPassword": Set the password you want to use for RCON here.
"Config": This should contain the config from the Game.ini.
"MapRotation": To this array you can add a map rotation. This can be done in the "Config" too but doing it here makes it more readable an is easier to change.
"AdditionalMods": Same as "MapRotation" but for the ugc mods.
"Mods": Here you can add the id's of Moderators.
"Whitelist": Here you can add the id's of people you want to whitelist.
"Blacklist": Here you can add the id's of people you want to blacklist.
"StandardPorts": These ports will be opend.
"SteamPassword": This is the password of the steam user on the server. Can be changed if wanted.
"StartServerAfterCompletion": true or false. If true it starts the pavlov server using "sudo systemctl start pavlovserver".



//There is also a batch file to easily connect to a server and a batch file to clear all known hosts(ssh). https://github.com/DarkAt26/AutomaticPavlovServerSetup/tree/master/Batch
