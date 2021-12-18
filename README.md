# Space Engineers Plugin Template

1. Install Space Engineers from Steam (if you don't have it already)
2. Install [Plugin Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=2407984968) into Space Engineers
3. [Download latest Torch Server](https://torchapi.net/)
4. Extract Torch and move it to `C:\Torch`.
5. Run `C:\Torch\Torch.Server.exe` to download the Dedicated Server from Steam
6. Click on "Use this template" on GitHub to create your project
7. Clone your project to have a local working copy
8. Replace the GUID value in `AssemblyInfo.cs` of each project with a new random one, the GUID must be unique for each project separately. Replace them consistently in all files, including project files.
9. Make sure `manifest.xml` has the right information in the `TorchPlugin` project
10. Search for `MyPlugin` in all C# source files and replace with your plugin's name accordingly
11. Optionally change the top level namespace name to yours
12. Rename your projects and solution if/as needed, with replace all in files or with rename refactoring if available in your IDE
13. Edit `Edit-and-run-before-opening-solution.bat` in the working copy folder to match your local paths
14. Open the solution
15. Make a test build, it should deploy the resulting DLL files, 0Harmony.dll and manifest.xml to the respective targets
16. Test that the empty plugin can be enabled in Plugin Loader (client), Torch Server's UI and the Dedicated Server's UI
17. Replace this file with the description of your plugin

_You can skip steps depending on which kinds of plugin you plan to develop._

_The ClientPlugin build kills SpaceEngineers.exe, so the DLL can be overwritten._
