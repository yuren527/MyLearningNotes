- Three files need to be modified in plugin source code;
- The .Dll and .Lib files should be copy to `[PluginBaseDirPath]/ThirdParty/`, the directory does not exist by default, so we need to create it;  

Blow is the test project I made, now we use it as example here:  
- In the Plugin.build.cs file, we need to add two line of code:  
```C#
using UnrealBuildTool;
using System.IO;

public class FSDKPlugin : ModuleRules
{
	public FSDKPlugin(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		PublicIncludePaths.AddRange(
			new string[] {
				// ... add public include paths required here ...
			}
			);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				// ... add other private include paths required here ...
			}
			);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				"Projects",
				// ... add other public dependencies that you statically link with here ...
			}
			);
			
		
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
                "CoreUObject",
                "Engine",
                "Slate",
                "SlateCore",
				// ... add private dependencies that you statically link with here ...	
			}
			);
		
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);

        PublicDelayLoadDLLs.Add("facesdk.dll");
        PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "../../ThirdParty/facesdk.lib"));

        bEnableExceptions = true;
    }
}
```
