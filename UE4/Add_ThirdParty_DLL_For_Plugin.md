- Three files need to be modified in plugin source code;
- The .Dll and .Lib files should be copy to `[PluginBaseDirPath]/ThirdParty/`, the directory does not exist by default, so we need to create it;  

Blow is the test project I made, now we use it as example here:  
- **In the Plugin.build.cs file, we need to add two line at the end of class:** 
> PublicDelayLoadDLLs.Add("facesdk.dll");  
> PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "../../ThirdParty/facesdk.lib"));
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
- **Then in the plugin.h file, we should add a library handler, as below:**
```C++
#pragma once
#include "Modules/ModuleManager.h"

class FFSDKPluginModule : public IModuleInterface
{
public:

	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;

private:
	/** Handle to the test dll we will load */
	void* FSDKLibraryHandle;
};
```
- **Finally, we should change the definitions of the two functions above:**
```C++
#include "FSDKPlugin.h"
#include "Core.h"
#include "Modules/ModuleManager.h"
#include "Interfaces/IPluginManager.h"

#define LOCTEXT_NAMESPACE "FFSDKPluginModule"

void FFSDKPluginModule::StartupModule()
{
	// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module

	// Get the base directory of this plugin
	FString BaseDir = IPluginManager::Get().FindPlugin("FSDKPlugin")->GetBaseDir();

	// Add on the relative location of the third party dll and load it
	FString FSDKPath;
#if PLATFORM_WINDOWS
	FSDKPath = FPaths::Combine(*BaseDir, TEXT("ThirdParty/facesdk.dll"));
#elif PLATFORM_MAC
	FSDKPath = FPaths::Combine(*BaseDir, TEXT("Source/ThirdParty/FacePluginLibrary/Mac/Release/libExampleLibrary.dylib"));
#endif // PLATFORM_WINDOWS

	FSDKLibraryHandle = !FSDKPath.IsEmpty() ? FPlatformProcess::GetDllHandle(*FSDKPath) : nullptr;
	if (FSDKLibraryHandle)
	{
		FMessageDialog::Open(EAppMsgType::Ok, LOCTEXT("ThirdPartyLibraryError", "FSDK successfully loaded!"));
	}
	else
	{
		FMessageDialog::Open(EAppMsgType::Ok, LOCTEXT("ThirdPartyLibraryError", "Failed to load Facesdk third party library"));
	}


}

void FFSDKPluginModule::ShutdownModule()
{
	// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,
	// we call this function before unloading the module.

	// Free the dll handle
	FPlatformProcess::FreeDllHandle(FSDKLibraryHandle);
	FSDKLibraryHandle = nullptr;
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FFSDKPluginModule, FSDKPlugin)
```
- **Now, the third-party DLL can be successfully loaded, with a dialog window pop up, as we used the Function `FMessageDialog::Open(...)` here;**
