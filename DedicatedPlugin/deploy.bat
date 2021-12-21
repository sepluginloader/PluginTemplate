@echo off
if [%2] == [] goto EOF

echo Parameters: %*

set TARGET=..\..\..\Torch\DedicatedServer64\Plugins
mkdir %TARGET% >NUL 2>&1

set SRC=%~p1
set NAME=%~2

echo Deploying binary:
echo From %1 to "%TARGET%\%NAME%.dll"
:RETRY
ping -n 2 127.0.0.1 >NUL 2>&1
copy /y %1 "%TARGET%\%NAME%.dll"
IF %ERRORLEVEL% NEQ 0 GOTO :RETRY
copy /y "%SRC%\0Harmony.dll" "%TARGET%\"
echo Done
exit 0

:EOF