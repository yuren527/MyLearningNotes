By default vscode cannot find the cpp standard library and its include files, even if having C/C++ extension installed.
When compiling cpp file(myself using cl.exe), it comes an error saying "no include path set", to fix this, an argument can be added to the compile args, `"/I{Path_To_Include}"`,
but even having this fixed, it comes another error that cannot open the library of some standard lib, after adding another arg of "/LIBPATH:"C:/{Path_To_Lib}"", it comes more weird errors like cannot recognize space in args.

Then realized to compile cpp project in vscode, the feasible way is to set the env variables using vs batch command before running actual compiling process, below is the example:
```
		// build release
		{
			"type": "shell",
			"label": "Build SPFCore Release cl",
			"command": "cl.exe",
			"args": [
				"/std:c++20",
				"/MD",
				"/LD",
				"/DSPF_MAKE_DLL",
				"/DNDEBUG",
				"/O2",
				"/EHsc",
				"/nologo",
				"/Fo${workspaceFolder}/Intermediate/",
				"/I${workspaceFolder}/Public",
				"/Fe${workspaceFolder:SPFHost}/Plugins/SpatialPathfindingPlugin/Binaries/ThirdParty/SPFCore/Win64/SPFCore",
				"/Fd${workspaceFolder}/Intermediate/",
				"${workspaceFolder}/Private/Cell.cpp",
				"${workspaceFolder}/Private/Coordinate.cpp",
				"${workspaceFolder}/Private/GridCoord.cpp",
				"${workspaceFolder}/Private/MultiThreadPathfinding.cpp",
				"${workspaceFolder}/Private/SPFMatrix.cpp",
				"/link${workspaceFolder}/version.res",
			],
			"options": {
				//"cwd": "${fileDirname}"
			},
			"problemMatcher": [
				"$msCompile"
			],
			"group": "build",
			"windows": {
				"options": {
					"shell": {
						"executable": "cmd.exe",
						"args": [
							"/C",
							"\"C:/Program Files/Microsoft Visual Studio/2022/Community/VC/Auxiliary/Build/Vcvarsall.bat\"",
							"x64",
							"&&",
						]
					}
				}
			},
			"dependsOn": [
				"compile versioninfo",
				//"copy headers"
			],
			"dependsOrder": "sequence",
		},
```

In this way, no need to add any troublesome including or linking args anymore.

Additionally, backup the task command and source code of versioninfo:
```
// compile the versioninfo
		{
			"type": "shell",
			"label": "compile versioninfo",
			"command": "rc.exe",
			"args": [
				"${workspaceFolder}\\version.rc"
			],
			"options": {
				"cwd": "${workspaceFolder}/"
			}
		}
```

```
1 VERSIONINFO
FILEVERSION 2,0,0,0
PRODUCTVERSION 2,0,0,0
BEGIN
    BLOCK "StringFileInfo"
    BEGIN
        BLOCK "040904b0"
        BEGIN
            VALUE "CompanyName", ""
            VALUE "FileDescription", "This DLL is an integal part of SpatialPathfindingPlugin, distributed along with the product" 
            VALUE "FileVersion", "2.0.0.0"
            VALUE "InternalName", "SPFCore"
            VALUE "LegalCopyright", "Copyright (C) 2022 Yusong Gao - All rights reserved. This plugin is downloadable from UnrealEngine Marketplace"
            VALUE "OriginalName", "SPFCore"
            VALUE "ProductName", "SpatialPathfindingPlugin"
            VALUE "ProductVersion", "2.0.0.0"
        END
    END
    BLOCK "VarFileInfo"
    BEGIN
        VALUE "Translation", 0x0409, 1200
    END
END
```

