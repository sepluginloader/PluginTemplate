@echo off
if [%2] == [] goto EOF

echo Parameters: %*

set TARGET=..\..\..\Bin64\Plugins\Local
mkdir %TARGET% >NUL 2>&1

set NAME=%~2

echo Killing Space Engineers process...
taskkill /im SpaceEngineers.exe 2>NUL
IF %ERRORLEVEL% EQU 0 (
    ping
)

echo Deploying binary:
echo From %1 to "%TARGET%\%NAME%.dll"
:RETRY
ping -n 2 127.0.0.1 >NUL 2>&1
copy /y %1 "%TARGET%\%NAME%.dll"
IF %ERRORLEVEL% NEQ 0 GOTO :RETRY
echo Done
exit 0

:EOF