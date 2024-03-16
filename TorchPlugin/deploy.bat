@echo off
if [%2] == [] goto EOF

echo Parameters: %*

set SRC=%~p1
set NAME=%~2

set TARGET=..\..\..\Torch\Plugins\%NAME%
mkdir %TARGET% >NUL 2>&1

echo.
echo Deploying TORCH SERVER plugin binary:
echo.
:RETRY
ping -n 2 127.0.0.1 >NUL 2>&1
echo From %1 to "%TARGET%\%NAME%.dll"
copy /y %1 "%TARGET%\%NAME%.dll"
IF %ERRORLEVEL% NEQ 0 GOTO :RETRY
echo Copying "%SRC%\manifest.xml" into "%TARGET%\"
copy /y "%SRC%\manifest.xml" "%TARGET%\"

rem TODO: If your plugin depends on any unsafe C# code, then uncomment the next line:
rem copy /y "%SRC%\System.Runtime.CompilerServices.Unsafe.dll" "%TARGET%\"

REM USE_HARMONY
REM TODO: Comment out or remove the next two lines if
REM       your plugin does not use Harmony for patching:
echo Copying "%SRC%\0Harmony.dll" into "%TARGET%\"
copy /y "%SRC%\0Harmony.dll" "%TARGET%\"

echo Done
echo.
exit 0

:EOF