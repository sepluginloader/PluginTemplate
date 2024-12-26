"""
Replaces project GUIDs and renames the solution
Tested on Python 3.9, should work on any recent 3.x
"""

import json
import os
import re
import typing
import uuid
import winreg
import xml.etree.ElementTree as ET

DRY_RUN = False

PT_PROJECT_NAME = r"^([A-Z][a-z_0-9]+)+$"
RX_PROJECT_NAME = re.compile(PT_PROJECT_NAME)

PROJECT_NAMES = (
    "ClientPlugin",
    "TorchPlugin",
    "DedicatedPlugin",
    "Shared",
)


def _generate_guid() -> str:
    return str(uuid.uuid4())


def _replace_text_in_file(replacements: dict[str, str], path: str) -> None:
    is_project = (
        path.endswith(".sln") or path.endswith(".csproj") or path.endswith(".shproj")
    )
    encoding = "utf-8-sig" if is_project else "utf-8"

    with open(path, "rt", encoding=encoding) as f:
        text = f.read()

    original = text

    for k, v in replacements.items():
        text = text.replace(k, v)

    if DRY_RUN or text == original:
        return

    with open(path, "wt", encoding=encoding) as f:
        f.write(text)


def _input_plugin_name() -> str:
    while True:
        plugin_name = input("Name of the plugin (in CapitalizedWords format): ")
        if not plugin_name:
            break

        if RX_PROJECT_NAME.match(plugin_name):
            break

        print("Invalid plugin name, it must match regexp: " + PT_PROJECT_NAME)

    return plugin_name


def _input_question(prompt: str, default: bool | None = None) -> bool:
    while True:
        response = input(prompt).lower()

        if default is not None and len(response) == 0:
            return default

        if response in ["n", "no"]:
            return False

        if response in ["y", "yes"]:
            return True

        print("Unknown response (Y/N)")


def _rename_project(name: str) -> None:
    torch_guid = _generate_guid()
    replacements = {
        "PluginTemplate": name,
        "A061FC6C-713E-42CD-B413-151AC8A5074C": _generate_guid().upper(),
        "FFB7FCA3-B168-43F4-8DBF-6247C0D331C8": _generate_guid().upper(),
        "C5784FE0-CF0A-4870-9DEF-7BEA8B64C01A": _generate_guid().upper(),
        "C889318F-9835-4814-B26E-979242CAEB0C": torch_guid.upper(),
        "c889318f-9835-4814-b26e-979242caeb0c": torch_guid,
    }

    def iter_paths() -> typing.Iterator[tuple[str, str]]:
        print("Solution:")
        yield "PluginTemplate.sln", "PluginTemplate.sln"

        if os.path.exists("PluginTemplate.sln.DotSettings.user"):
            yield "PluginTemplate.sln.DotSettings.user", "PluginTemplate.sln.DotSettings.user"

        for project_name in PROJECT_NAMES:

            print()
            print(f"{project_name}:")

            for dirpath, _, filenames in os.walk(project_name):
                dirpath2 = dirpath + "\\"
                if "\\obj\\" in dirpath2 or "\\bin\\" in dirpath2:
                    continue

                for filename in filenames:
                    ext = filename.rsplit(".")[-1]
                    if ext in ("xml", "xaml", "cs", "sln", "csproj", "shproj"):
                        path = os.path.join(dirpath, filename)
                        yield filename, path

    rename_files: list[tuple[str, str]] = []
    for filename, path in iter_paths():
        print(f"  {filename}")
        _replace_text_in_file(replacements, path)
        if "PluginTemplate" in filename:
            rename_files.append((filename, path))

    if not DRY_RUN:
        for filename, path in rename_files:
            dir_path = os.path.dirname(path)
            dst_name = filename.replace("PluginTemplate", name)
            dst_path = os.path.join(dir_path, dst_name)
            os.rename(path, dst_path)


def _get_steam_path() -> str:
    reg = winreg.ConnectRegistry(None, winreg.HKEY_LOCAL_MACHINE)
    key = winreg.OpenKey(reg, r"SOFTWARE\WOW6432Node\Valve\Steam")
    (path, _) = winreg.QueryValueEx(key, "InstallPath")
    return path


def _valve_to_json(vdf: str) -> dict[str, dict[str, str | dict]]:
    vdf = re.sub(r'"\n\t*\{', r'": {', vdf)
    vdf = re.sub(r'"\t\t"', r'": "', vdf)
    vdf = re.sub(r"\}\n", r"},\n", vdf)
    vdf = re.sub(r'"\n', r'",\n', vdf)
    vdf = re.sub(r'",\n(\t+)\}', r'"\n\1}', vdf)
    vdf = re.sub(r"\},\n(\t*)(?=})", r"}\n\1", vdf)
    vdf = f"{{{vdf[:-2]}}}"
    return json.loads(vdf)


def _get_install_locations(vdf_path: str, ids: list[str]) -> dict[str, str | None]:
    with open(vdf_path, "r", encoding="UTF-8") as file:
        vdf = file.read()

    game_drives: dict[str, str | None] = {}
    for folder in _valve_to_json(vdf)["libraryfolders"].values():
        assert isinstance(folder, dict)
        assert isinstance(folder["apps"], dict)
        assert isinstance(folder["path"], str)

        for game in ids:
            if game in folder["apps"].keys():
                game_drives[game] = folder["path"]
            else:
                game_drives[game] = None

    game_install: dict[str, str | None] = {}
    for game_id, drive in game_drives.items():

        if drive is None:
            game_install[game_id] = None

        else:
            path = f"{drive}\\steamapps\\appmanifest_{game_id}.acf"
            with open(path, "r", encoding="UTF-8") as file:
                manifest = _valve_to_json(file.read())

            game_install[game_id] = (
                f"{drive}\\steamapps\\common\\{manifest["AppState"]["installdir"]}"
            )

    return game_install


def _update_props(
    game_dir: str | None = None,
    server_dir: str | None = None,
    torch_dir: str | None = None,
) -> None:
    parser = ET.XMLParser(target=ET.TreeBuilder(insert_comments=True))
    tree = ET.parse("Directory.Build.props", parser)
    root = tree.getroot()
    group = root.find("PropertyGroup")
    assert group is not None

    if game_dir:
        bin64 = group.find("Bin64")
        assert bin64 is not None
        bin64.text = f"{game_dir}\\Bin64"

    if server_dir:
        dedicated64 = group.find("Dedicated64")
        assert dedicated64 is not None
        dedicated64.text = f"{server_dir}\\DedicatedServer64"

    if torch_dir:
        torch = group.find("Torch")
        assert torch is not None
        torch.text = torch_dir

    tree.write("Directory.Build.props")


def main() -> None:
    """Run the setup."""

    if os.path.isfile("PluginTemplate.sln"):
        plugin_name = _input_plugin_name()

        if plugin_name:
            _rename_project(plugin_name)
        else:
            print("Skipping project rename")

    if _input_question("Auto-detect reference locations? (Y/N) [Y]: ", True):
        vdf_path = f"{_get_steam_path()}\\steamapps\\libraryfolders.vdf"
        locations = _get_install_locations(vdf_path, ["244850", "298740"])

        if locations["244850"] is not None:
            print(f"Found Space Engineers Under {locations["244850"]}")
        else:
            print("Could not find Space Engineers install location.")

        if locations["298740"] is not None:
            print(f"Found Dedicated Server Under {locations["298740"]}")
        else:
            print("Could not find Dedicated Server install location.")

        locations["torch"] = (
            input("Enter Torch path (leave blank if installed into DS): ")
            or locations["298740"]
        )
        _update_props(locations["244850"], locations["298740"], locations["torch"])
    else:
        print("Please add the paths manually to 'Directory.Build.props'")

    input("Done. (Press any key to exit)")


if __name__ == "__main__":
    main()
