# Space Engineers Plugin Template

Prerequisites:
- Space Engineers
- Python 3.x (tested with 3.9)
- [Plugin Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=2407984968)
- [Torch Server](https://torchapi.net/) in `C:\Torch`, run `Torch.Server.exe` once to prepare

To create your plugin project:
1. Click on "Use this template" (top right corner on GitHub) and follow the wizard to create your repository
2. Clone your repository to have a local working copy
3. Run `ReplaceGuidsAndRename.py`, enter the name of your plugin project in `CapitalizedWords` format
4. Edit `Edit-and-run-before-opening-solution.bat` to match your local paths, then run it
5. Open the solution in Visual Studio or Rider
6. Make a test build, it should deploy the resulting files to the respective target folders 
7. Test that the empty plugin can be enabled in Plugin Loader (client), Torch Server's UI and the Dedicated Server's UI
8. You can delete the `ReplaceGuidsAndRename.py` from the project
9. Replace this file with the description of your plugin

_You can skip steps depending on your specific targets._