# Space Engineers Plugin Template

1. Install Space Engineers from Steam (if you don't have it already)
2. Install [Plugin Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=2407984968) into Space Engineers
3. [Download latest Torch Server](https://torchapi.net/)
4. Extract Torch and move it to `C:\Torch`.
5. Run `C:\Torch\Torch.Server.exe` to download the Dedicated Server from Steam
6. Edit `Edit-and-run-before-opening-solution.bat` to match your local paths
7. Open the solution
8. Make a test build
9. Test that the empty plugins can be enabled in the client, Torch and the Dedicated Server
10. Replace the GUID value in `AssemblyInfo.cs` of each project with a new random one. Replace them consistently in all files, including project files. Make sure `manifest.xml` is good in the `TorchPlugin` project
11. Search for `MyPlugin` in all C# source files and replace with your plugin's name accordingly
12. Optionally change the top level namespace name to yours
13. Rename your projects and solution if/as needed, with replace all in files or with rename refactoring if available in your IDE
14. Replace this file with the description of your plugin

_You can skip steps depending on which kinds of plugin you plan to develop._
