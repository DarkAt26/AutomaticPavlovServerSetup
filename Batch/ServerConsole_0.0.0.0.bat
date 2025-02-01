@echo off
setlocal enabledelayedexpansion

:: Get the batch file's name (without path)
set filename=%~nx0

:: Remove the ".bat" extension
set fileWithoutExt=%filename:.bat=%

:: Split the filename at the underscore and get the part after the underscore
for /f "tokens=2 delims=_" %%A in ("%fileWithoutExt%") do (
    set ip=%%A
)

:: Now use the extracted IP address in the ssh command
echo Using IP: !ip!
START cmd.exe /K "ssh -o ServerAliveInterval=60 root@!ip!"

endlocal