""" Replaces project GUIDs and renames the solution

Tested on Python 3.9, should work on any recent 3.x

"""

import os
import re
import sys
import uuid

DRY_RUN = False

PT_PROJECT_NAME = r'^([A-Z][a-z_0-9]+)+$'
RX_PROJECT_NAME = re.compile(PT_PROJECT_NAME)

PROJECT_NAMES = (
    'ClientPlugin',
    'TorchPlugin',
    'DedicatedPlugin',
    'Shared',
)


def generate_guid():
    return str(uuid.uuid4())


def replace_text_in_file(replacements, path):
    is_project = path.endswith('.sln') or path.endswith('.csproj') or path.endswith('.shproj')
    encoding = 'utf-8-sig' if is_project else 'utf-8'

    with open(path, 'rt', encoding=encoding) as f:
        text = f.read()

    original = text

    for k, v in replacements.items():
        text = text.replace(k, v)

    if DRY_RUN or text == original:
        return

    with open(path, 'wt', encoding=encoding) as f:
        f.write(text)


def input_plugin_name():
    while 1:
        plugin_name = input('Name of the plugin (in CapitalizedWords format): ')
        if not plugin_name:
            break

        if RX_PROJECT_NAME.match(plugin_name):
            break

        print('Invalid plugin name, it must match regexp: ' + PT_PROJECT_NAME)

    return plugin_name


def main():
    if not os.path.isfile('PluginTemplate.sln'):
        print('Run this script only once from the working copy (solution) folder')
        sys.exit(-1)

    plugin_name = input_plugin_name()
    if not plugin_name:
        return

    torch_guid = generate_guid()
    replacements = {
        'PluginTemplate': plugin_name,
        'A061FC6C-713E-42CD-B413-151AC8A5074C': generate_guid().upper(),
        'FFB7FCA3-B168-43F4-8DBF-6247C0D331C8': generate_guid().upper(),
        'C5784FE0-CF0A-4870-9DEF-7BEA8B64C01A': generate_guid().upper(),
        'C889318F-9835-4814-B26E-979242CAEB0C': torch_guid.upper(),
        'c889318f-9835-4814-b26e-979242caeb0c': torch_guid,
    }

    def iter_paths():
        print('Solution:')
        yield 'PluginTemplate.sln', 'PluginTemplate.sln'

        if os.path.exists('PluginTemplate.sln.DotSettings.user'):
            yield 'PluginTemplate.sln.DotSettings.user', 'PluginTemplate.sln.DotSettings.user'

        for project_name in PROJECT_NAMES:

            print()
            print(f'{project_name}:')

            for dirpath, dirnames, filenames in os.walk(project_name):
                dirpath2 = dirpath + '\\'
                if '\\obj\\' in dirpath2 or '\\bin\\' in dirpath2:
                    continue

                for filename in filenames:
                    ext = filename.rsplit('.')[-1]
                    if ext in ('xml', 'xaml', 'cs', 'sln', 'csproj', 'shproj'):
                        path = os.path.join(dirpath, filename)
                        yield filename, path

    rename_files = []
    for filename, path in iter_paths():
        print(f'  {filename}')
        replace_text_in_file(replacements, path)
        if 'PluginTemplate' in filename:
            rename_files.append((filename, path))

    if not DRY_RUN:
        for filename, path in rename_files:
            dir_path = os.path.dirname(path)
            dst_name = filename.replace('PluginTemplate', plugin_name)
            dst_path = os.path.join(dir_path, dst_name)
            os.rename(path, dst_path)

    print('Done.')


if __name__ == '__main__':
    main()