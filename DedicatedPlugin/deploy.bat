@echo off
if [%2] == [] goto EOF

echo Parameters: %*

set TARGET=..\..\..\Torch\DedicatedServer64\Plugins
mkdir %TARGET% >NUL 2>&1

set SRC=%~p1
set NAME=%~2

:DEPLOY
echo Deploying binary:
echo From %1 to "%TARGET%\%NAME%.dll"
copy /y %1 "%TARGET%\%NAME%.dll"
copy /y "%SRC%\0Harmony.dll" "%TARGET%\"
echo Done
exit 0

:EOF