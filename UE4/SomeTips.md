

# Create a basic shape in actor constructor
- First, include `ConstructorHelpers.h`;  
- Second, use `static ConstructorHelpers::FObjectFinder<UStaticMesh>` to load the mesh;

See example below:
```C++
this->SphereMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("SphereMesh"));
this->SphereMesh->AttachTo(GetRootComponent());

static ConstructorHelpers::FObjectFinder<UStaticMesh>SphereMeshAsset(TEXT("StaticMesh'/Engine/BasicShapes/Sphere.Sphere'"));
this->SphereMesh->SetStaticMesh(SphereMeshAsset.Object);
```
# Install Code Plugin to Source-built Engine
In order to be able to use a code plugin from the Marketplace in an Engine that you built from source code, all you need to do is copy the plugin folder from the `Engine\Plugins\Marketplace` folder in your binary engine installation to the same folder in your source engine installation (you may need to create the Marketplace folder). You'll need to generate project files and build the project again, but you will then be able to open the project successfully. Once your project is open, you will probably see some errors in any Blueprints using nodes from the plugin. Just refresh these nodes and build the Blueprint again and the errors should disappear.
# Convert between FString and std::string
```C++
FString UE4Str = "UE4 C++"; 

//FString to std::string
std::string cstr(TCHAR_TO_UTF8(*UE4Str));

//std::string to FString
ClientMessage(FString(cstr.c_str()));
```
# Why is Caracter Movement automatically replicated to server
Characters using CharacterMovementComponent automatically have client-server networking built in. Here's how player movement prediction, replication and correction works in networked games through CharacterMovementComponent for each frame:  
- TickComponent function is called
- The acceleration and rotation change for the frame are calculted
- Either PerformMovement (for locally controlled Characters) or ReplicateMoveToServer (for network clients) is called  

ReplicateMoveToServer saves the move (into a list of pending moves), calls PerformMovement locally, and then replicates the move to the server by calling the replicated function ServerMove. ServerMove receives the movement parameters, including the client's final position and a timestamp. It executes on the server, where it decodes the movement parameters and causes the appropriate movement to occur. It then looks at the resulting position and evaluates the time since the last client response and the difference between the client's stated position and the position determined by the server. If the difference is big enough, the server calls ClientAdjustPosition, which replicates to the client and passes the corrected position along.

When TickComponent is called on the client again, if a correction from the server has been received, the client will call ClientUpdatePosition before calling PerformMovement. This process will replay all of the moves in the pending move list which occurred after the timestamp of the move the server adjusted.  

For more information, see [Character Movement Component](https://docs.unrealengine.com/en-US/Gameplay/Networking/CharacterMovementComponent/index.html)
# Simplified guide to make a property replicated
- Add `UPROPERTY(Replicated)`, if want a notification, add `UPROPERTY(Replicated, ReplicatedUsing=OnRep_FuncName)`;  
- Declare a function `void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const;` in header file; 
- in cpp file define the function added in header file: 
```C++
void ClassName::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override
{
	Super::GetLifetimeReplicatedProps(OutLifetimeProps);
	DOREPLIFETIME(ThisClassName, PropertyName);
}
``` 
- remember to include `Net/UnrealNetwork.h`, otherwise the Macro used in definition will not be recognized;

# Use "Delay" in C++
**Example:**  
```C++
#include "Engine/Public/TimerManager.h"
...
...
...
FTimerDelegate timerDelegate;//Attach pawns to airplane in 5 seconds
	FTimerHandle timerHandle;
	float attachTimeInterval = 5.f;
	timerDelegate.BindUFunction(this, FName("AttachPawnsToPlane"), plane);
	GetWorldTimerManager().SetTimer(timerHandle, timerDelegate, attachTimeInterval, false);
 ```
 ```C++
 void APTSGameMode::AttachPawnsToPlane(const AAirplaneBase* plane)
{
	if (plane != nullptr) {
		int playerNum = GetNumPlayers();
		for (int i = 0; i < playerNum; i++) {
			ACharacter* p = Cast<ACharacter>(UGameplayStatics::GetPlayerPawn(this, i));
			p->AttachToComponent(plane->GetAttachPoint(), FAttachmentTransformRules::SnapToTargetNotIncludingScale);
			p->GetCharacterMovement()->GravityScale = 0.f;
		}
	}
}
```

# Format FString
Use `FString::Printf()`, Example:
```C++
GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Red, FString::Printf(TEXT("%s %f"), *Msg, Value));
```

# Custom config file
To Add a custom config file, create a C++ class derived from `UDeveloperSettings`:
```C++
UCLASS(config = UDPConfig, DefaultConfig)
class PTS_API UUDPSettings : public UDeveloperSettings
{
	GENERATED_BODY()
public:
	UPROPERTY(config, EditAnywhere)
		FString ListenPort = FString("8888");
	UPROPERTY(config, EditAnywhere)//listenPort=8888?senderIP=192.168.0.100?senderPort=9999
		FString SenderIP = FString("192.168.0.100");
	UPROPERTY(config, EditAnywhere)
		FString SenderPort = FString("9999");
	
};
```
`config = configName`, determines the file name, if use `Game` or `Engine`, the values will then be inside the coresponding existing config files;  

To Access the settings values, do like below:
```C++
const UUDPSettings* udpSettings = GetDefault<UUDPSettings>();
	listenPort = udpSettings->ListenPort;
	senderIP = udpSettings->SenderIP;
	senderPort = udpSettings->SenderPort;
```

# Singleton in UE4
**.h:**
```C++
UCLASS()
class UFPPRequestLib : public UObject
{
	GENERATED_BODY()

public:
	static UFPPRequestLib* GetInstance();
	
protected:
	virtual void BeginDestroy() override;
}
```
**.cpp:**
```C++
static UFPPRequestLib* Singleton;

UFPPRequestLib* UFPPRequestLib::GetInstance() {
	if (Singleton == nullptr) {
		Singleton = NewObject<UFPPRequestLib>();
	}
	return Singleton;
}

void UFPPRequestObject::BeginDestroy()
{
	Super::BeginDestroy();
#if WITH_EDITOR
	UE_LOG(LogTemp, Warning, TEXT("Begin destroy"));
#endif
	Singleton = nullptr;
}
```
**In this case, we define a pointer to singleton of the class in cpp file; We can also define a private static field of the singleton inside the class, but then we have to initialize it in cpp file too, like below:**
```C++
//.Cpp
UFPPRequestLib* UFPPRequestLib::Singleton = nullptr;
```

Be ware that, a UObject is added to the GC system, so it can be destroyed automaticaaly, but if it's collected by GC then the Singleton will point to somewhere unknown, so we should override the `BeginDestroy` function to make the Singleton pointing to nullptr;

# Delegate declaration
Example:
```C++
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FRequestStructOutputSignature, const FThousandLandmarksResponseStruct&, output);
DECLARE_EVENT_OneParam(UFPPRequestObject, TestSignature, float);
DECLARE_DYNAMIC_DELEGATE_OneParam(FTestSignature_1, float, float_1);
DECLARE_DELEGATE_OneParam(FTestSignature_2, float);
```
- A delegate with a dynamic feature should be declared with a parameter type and a parameter name in the macro variation for each parameter, while a delegate without dynamic only need a parameter type declared; 

- A event should be specified a owning class at the first parameter of the macro variation; 

- Any delegate signature should be named starting with a "F"; 

- If exposing the delegate to blueprint, the delegate should be DYNAMIC_MULTICAST, and the signature should be `UPROPERTY(BlueprintAssignable, Category = "CategoryName")`; 

- The receiving function should be marked `UFUNCTION()`;

- If declaring a dynamic multicast delegate, make sure the .generated.h file is included.

# Example for DataTable Asset Linker
```C++
#include "CoreMinimal.h"
#include "Engine/DataTable.h"
#include "ResourceLinkTable.generated.h"

#define LINKER_PATH "DataTable'/Game/ResourceLinkTable.ResourceLinkTable'"

USTRUCT(BlueprintType)
struct FResourceLinkTableRow : public FTableRowBase
{
	GENERATED_BODY()
public:
	UPROPERTY(EditAnywhere)
		TSoftObjectPtr<UObject> Asset; 
};

template<typename T>
T* GetResourceAsset(FName rowName) {
	UDataTable* pTable = LoadObject<UDataTable>(NULL, UTF8_TO_TCHAR(LINKER_PATH));

	if (!pTable) {
		UE_LOG(LogTemp, Warning, TEXT("Linker table not found"));
		return nullptr;
	}

	FString contextString;
	FResourceLinkTableRow* row = pTable->FindRow<FResourceLinkTableRow>(rowName, contextString);
	if (!row) {
		UE_LOG(LogTemp, Warning, TEXT("Linker row not found, row name: %s"), *rowName.ToString());
		return nullptr;
	}

	T* ptr = Cast<T>(row->Asset.LoadSynchronous());
	if (!ptr) {
		UE_LOG(LogTemp, Warning, TEXT("Linker object cast fail"));
		return nullptr;
	}

	return ptr;	
}

```

## Right way to create a server-only module
- Make a new module with the `[ModuleName].cpp` including `IMPLEMENT_MODULE( FDefaultGameModuleImpl, [ModuleName]);`
- In `.uproject` file, add the newly created module with `"WhitelistTargets/BlacklistTargets"` property set to `"Server/Client"`, include or exclude the module for compiling on the listed build targets. The available build targets are `Game`, `Server`, `Client`, `Editor`, and `Program`.
- In primary module build rules, add the code below:
```
if (Target.bWithServerCode == true)
    {
        PublicDependencyModuleNames.Add("[ServerModuleName]");
    }
```

## Building target macros
If you want to be able to tag any BP function as `DevelopmentOnly`; go to the editor preferences > Blueprint editor > Experimental > Allow explicit impure node disabling.  

To declare custom made BP functions in code, add the following meta data; `UFUNCTION(..., meta=(DevelopmentOnly))`.  

For C++ functions, simply wrap their implementation with the following;
```
void Foo()
{
#if !(UE_BUILD_SHIPPING || UE_BUILD_TEST)
...
#endif
}
```
