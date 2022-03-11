# Space Engineers Plugin Template

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Python 3.x](https://python.org) (tested with 3.9)
- [Plugin Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=2407984968)
- [Torch Server](https://torchapi.com/) in `C:\Torch`, run `Torch.Server.exe` once to prepare

## Create your plugin project

1. Click on **Use this template** (top right corner on GitHub) and follow the wizard to create your repository
2. Clone your repository to have a local working copy
3. Run `ReplaceGuidsAndRename.py`, enter the name of your plugin project in `CapitalizedWords` format
4. Edit `Edit-and-run-before-opening-solution.bat` to match your local paths, then run it
5. Open the solution in Visual Studio or Rider
6. Make a test build, it should deploy the resulting files to their respective target folders (see them in the build log) 
7. Test that the empty plugin can be enabled in Plugin Loader (client), Torch Server's UI and the Dedicated Server's UI
8. Delete the `ReplaceGuidsAndRename.py` from the Shared project and the working copy folder (not needed anymore)
9. Replace the contents of this file with the description of your plugin
10. Follow the TODO comments in the source code

## Remarks

### Conditional compilation

- DedicatedPlugin defines `DEDICATED`, TorchPlugin defines `TORCH`. 
  You can use those names for conditional compilation by `#if` blocks in the Shared project.
  For example if you want your code to compile for client and dedicated server plugins, but 
  not for the Torch plugin, then put it into a `#if !TORCH` ... `#endif` block. 

### Shared project

- Put any code you can share between the plugin projects into the Shared project. 
  Try to keep the redundancy at the minimum.

- The DLLs required by your Shared code need to be added as a dependency to all the projects, 
  even if some of the code is not used by one of the projects.

- You can delete the projects you don't need. If you want only a single project, 
  then move over what is in the Shared one, then you can delete Shared.

### Torch plugin

- For Torch plugins see also the official
  [Torch Plugin Template](https://torchapi.com/wiki/index.php/Torch_Plugin_Template),
  it has some additional information in its `README.txt` file.

- If you don't need the config UI in Torch for your plugin, then remove the IWpfPlugin
  from the Plugin class and the `xaml` and `xaml.cs` files. Also remove the now unused
  `GetControl` method.
 
- Torch plugins should not use Harmony for patching, ideally. 
  Torch has its own patching mechanism, which is more compatible with other plugins, 
  but less convenient to use. If you want to remove Harmony from the Torch plugin, 
  then search for USE_HARMONY in all files, which will show you where to make changes. 
  Also remove Lib.Harmony from the TorchPlugin project's NuGet package dependencies.

### Debugging

- Always use a debug build if you want to set breakpoints and see variable values.
- A debug build defines `DEBUG`, so you can add conditional code in `#if DEBUG` blocks.
- While debugging a specific target unload the other two. It prevents the IDE to be confused.
- If breakpoints do not "stick" or do not work, then make sure that:
  - Other projects are unloaded, only the debugged one and Shared are loaded.
  - Debugger is attached to the running process.
  - You are debugging the code which is running (no code changes made since the build).

### Troubleshooting

- If the IDE looks confused, then restarting it and the debugged game usually works.
- If the restart did not work, then try to delete caches used by your IDE and restart.
- If your build cannot deploy (just runs in a loop), then something locks the DLL file.
- Look for running game processes (maybe stuck running in the background) and kill them.

### Release

- Always make your final release from a RELEASE build. (More optimized, removes debug code.)
- Always test your RELEASE build before publishing. Sometimes is behaves differently.
- In case of client plugins the Plugin Loader compiles your code, watch out for differences.

### Communication

- In your documentation always include how players or server admins should report bugs.
- Try to be reachable and respond on a timely manner over your communication channels.
- Be open for constructive critics.

### Abandoning your project

- Always consider finding a new maintainer, ask around at least once.
- If you ever abandon the project, then make it clear on its GitHub page.
- Abandoned projects should be made hidden on PluginHub and Torch's plugin list.
- Keep the code available on GitHub, so it can be forked and continued by others.