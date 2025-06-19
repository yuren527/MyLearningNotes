

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

# Right way to create a server-only module
- Make a new module with the `[ModuleName].cpp` including `IMPLEMENT_MODULE( FDefaultGameModuleImpl, [ModuleName]);`
- In `.uproject` file, add the newly created module with `"WhitelistTargets/BlacklistTargets"` property set to `"Server/Client"`, include or exclude the module for compiling on the listed build targets. The available build targets are `Game`, `Server`, `Client`, `Editor`, and `Program`.
- In primary module build rules, add the code below:
```
if (Target.bWithServerCode == true)
    {
        PublicDependencyModuleNames.Add("[ServerModuleName]");
    }
```

# Building target macros
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

# Show SpriteComponent of C++ implemented actor in editor
Blueprint classes derived from actor will have a sprite shown in scene, while C++ classes do not. 

To make C++ implemented actors show the sprite for the ease of navigation and locating, there are several simple steps that should be followed in code.

1. In the constructor of the class, add `RootComponent->bVisualizeComponent = true;`, this will display a sprite at the location of the component in the editor.
2. If the sprite is too small for some extensive scenes, add another line in constructor: `SpriteScale = 5.0f`, this change will make the sprites would be 5 times larger.
3. Finally, wrap the code added above with `#if WITH_EDITORONLY_DATA` and `#endif` to let the compiler know that the code should only be compiled with editor builds. Otherwise, it wouldn't compile for game builds since the variables referred to above are also wrapped with the macro; they just don't exist in the code of builds other than editor builds.


# Build UnrealEngine from source using VSCode
After cloning UnrealEngine source code from github and running setup.bat, run `.\GenerateProjectFiles.bat -vscode' in powershell, it will generate the .workspace file for vscode, then we can use command 'code .' or double click the .workspace file to open the vscode and press ctrl+shift+B to bring up the command palette, find and select "UnrealEditor Win64 Development Build", it will start building the editor.


## Differences between AddDynamic, AddStatic, AddUObject, AddUFunction
In Unreal Engine, delegates are a powerful feature that allows you to bind functions to events. There are several ways to bind functions to delegates, each with its own use case and requirements. Here are the differences between `AddDynamic`, `AddStatic`, `AddUObject`, and `AddUFunction`:

### `AddDynamic`

- **Usage**: Used to bind a UFUNCTION to a delegate.
- **Requirements**: The function must be marked with the [`UFUNCTION`](command:_github.copilot.openSymbolFromReferences?%5B%22%22%2C%5B%7B%22uri%22%3A%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FC%3A%2FRepos%2FIntelliBlueForce%2FSource%2FIntelliBlueForce%2FUserInterface%2FUnitListItem_Deployment.h%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%2C%22pos%22%3A%7B%22line%22%3A33%2C%22character%22%3A1%7D%7D%5D%2C%229cba47ec-6c07-4b98-acc2-925a1b64b2b5%22%5D "Go to definition") macro.
- **Example**:
  ```cpp
  MyDelegate.AddDynamic(this, &MyClass::MyFunction);
  ```

### `AddStatic`

- **Usage**: Used to bind a static function to a delegate.
- **Requirements**: The function must be static.
- **Example**:
  ```cpp
  MyDelegate.AddStatic(&MyStaticFunction);
  ```

### `AddUObject`

- **Usage**: Used to bind a member function of a UObject-derived class to a delegate.
- **Requirements**: The function must be a member of a class derived from [`UObject`](command:_github.copilot.openSymbolFromReferences?%5B%22%22%2C%5B%7B%22uri%22%3A%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FC%3A%2FRepos%2FIntelliBlueForce%2FSource%2FIntelliBlueForce%2FUserInterface%2FUnitListItem_Deployment.h%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%2C%22pos%22%3A%7B%22line%22%3A27%2C%22character%22%3A40%7D%7D%5D%2C%229cba47ec-6c07-4b98-acc2-925a1b64b2b5%22%5D "Go to definition").
- **Example**:
  ```cpp
  MyDelegate.AddUObject(this, &MyClass::MyFunction);
  ```

### `AddUFunction`

- **Usage**: Used to bind a UFUNCTION to a delegate by name.
- **Requirements**: The function must be marked with the [`UFUNCTION`](command:_github.copilot.openSymbolFromReferences?%5B%22%22%2C%5B%7B%22uri%22%3A%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FC%3A%2FRepos%2FIntelliBlueForce%2FSource%2FIntelliBlueForce%2FUserInterface%2FUnitListItem_Deployment.h%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%2C%22pos%22%3A%7B%22line%22%3A33%2C%22character%22%3A1%7D%7D%5D%2C%229cba47ec-6c07-4b98-acc2-925a1b64b2b5%22%5D "Go to definition") macro, and you need to provide the function name as a string.
- **Example**:
  ```cpp
  MyDelegate.AddUFunction(this, FName("MyFunction"));
  ```

### Detailed Explanation

1. **`AddDynamic`**:
   - **Purpose**: Binds a UFUNCTION to a delegate.
   - **Use Case**: When you need to bind a function that is marked with the [`UFUNCTION`](command:_github.copilot.openSymbolFromReferences?%5B%22%22%2C%5B%7B%22uri%22%3A%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FC%3A%2FRepos%2FIntelliBlueForce%2FSource%2FIntelliBlueForce%2FUserInterface%2FUnitListItem_Deployment.h%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%2C%22pos%22%3A%7B%22line%22%3A33%2C%22character%22%3A1%7D%7D%5D%2C%229cba47ec-6c07-4b98-acc2-925a1b64b2b5%22%5D "Go to definition") macro, typically for Blueprint-exposed functions.
   - **Example**:
     ```cpp
     DECLARE_DYNAMIC_MULTICAST_DELEGATE(FMyDelegate);

     UCLASS()
     class MYPROJECT_API AMyActor : public AActor
     {
         GENERATED_BODY()

     public:
         FMyDelegate OnMyEvent;

         UFUNCTION()
         void MyFunction()
         {
             // Function implementation
         }

         void BindFunction()
         {
             OnMyEvent.AddDynamic(this, &AMyActor::MyFunction);
         }
     };
     ```

2. **`AddStatic`**:
   - **Purpose**: Binds a static function to a delegate.
   - **Use Case**: When you need to bind a function that does not belong to an instance of a class.
   - **Example**:
     ```cpp
     DECLARE_DELEGATE(FMyDelegate);

     void MyStaticFunction()
     {
         // Function implementation
     }

     void BindFunction()
     {
         FMyDelegate MyDelegate;
         MyDelegate.AddStatic(&MyStaticFunction);
     }
     ```

3. **`AddUObject`**:
   - **Purpose**: Binds a member function of a UObject-derived class to a delegate.
   - **Use Case**: When you need to bind a function that belongs to an instance of a class derived from [`UObject`](command:_github.copilot.openSymbolFromReferences?%5B%22%22%2C%5B%7B%22uri%22%3A%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FC%3A%2FRepos%2FIntelliBlueForce%2FSource%2FIntelliBlueForce%2FUserInterface%2FUnitListItem_Deployment.h%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%2C%22pos%22%3A%7B%22line%22%3A27%2C%22character%22%3A40%7D%7D%5D%2C%229cba47ec-6c07-4b98-acc2-925a1b64b2b5%22%5D "Go to definition").
   - **Example**:
     ```cpp
     DECLARE_DELEGATE(FMyDelegate);

     UCLASS()
     class MYPROJECT_API AMyActor : public AActor
     {
         GENERATED_BODY()

     public:
         FMyDelegate OnMyEvent;

         void MyFunction()
         {
             // Function implementation
         }

         void BindFunction()
         {
             OnMyEvent.AddUObject(this, &AMyActor::MyFunction);
         }
     };
     ```

4. **`AddUFunction`**:
   - **Purpose**: Binds a UFUNCTION to a delegate by name.
   - **Use Case**: When you need to bind a function dynamically by its name, typically for more dynamic or reflective scenarios.
   - **Example**:
     ```cpp
     DECLARE_DYNAMIC_MULTICAST_DELEGATE(FMyDelegate);

     UCLASS()
     class MYPROJECT_API AMyActor : public AActor
     {
         GENERATED_BODY()

     public:
         FMyDelegate OnMyEvent;

         UFUNCTION()
         void MyFunction()
         {
             // Function implementation
         }

         void BindFunction()
         {
             OnMyEvent.AddUFunction(this, FName("MyFunction"));
         }
     };
     ```

### Summary

- **`AddDynamic`**: Use for UFUNCTIONs, typically for Blueprint-exposed functions.
- **`AddStatic`**: Use for static functions.
- **`AddUObject`**: Use for member functions of [`UObject`](command:_github.copilot.openSymbolFromReferences?%5B%22%22%2C%5B%7B%22uri%22%3A%7B%22scheme%22%3A%22file%22%2C%22authority%22%3A%22%22%2C%22path%22%3A%22%2FC%3A%2FRepos%2FIntelliBlueForce%2FSource%2FIntelliBlueForce%2FUserInterface%2FUnitListItem_Deployment.h%22%2C%22query%22%3A%22%22%2C%22fragment%22%3A%22%22%7D%2C%22pos%22%3A%7B%22line%22%3A27%2C%22character%22%3A40%7D%7D%5D%2C%229cba47ec-6c07-4b98-acc2-925a1b64b2b5%22%5D "Go to definition")-derived classes.
- **`AddUFunction`**: Use for binding UFUNCTIONs by name, useful for dynamic or reflective scenarios.

Choose the appropriate method based on your specific use case and the type of function you need to bind to the delegate.

### Summary of Delegate Functions

| Delegate Type       | Bind Function Types                          | Unbind Function Types       | Execute Function Types       |
|---------------------|----------------------------------------------|-----------------------------|------------------------------|
| Single-cast Delegate| BindDynamic, BindStatic, BindUObject, BindRaw| Unbind                      | Execute, ExecuteIfBound      |
| Multi-cast Delegate | AddDynamic, AddStatic, AddUObject, AddRaw    | RemoveDynamic, Remove       | Broadcast                    |
| Dynamic Delegate    | AddDynamic                                   | RemoveDynamic               | Broadcast                    |

# Forward rendering Issues

## Forward rendering in UE5:
- Is designed for performance-focused use cases like VR, mobile, or stylized visuals.
- Uses simpler shading, fewer dynamic lights, and no GBuffer.
- Supports MSAA (which deferred doesn’t).
- Is not compatible with many advanced features, such as:
	- Lumen
	- Nanite
	- Screen Space Global Illumination
	- SSAO (replaced by simpler ambient models)

## SM6 (Shader Model 6)
SM6 is a DirectX 12+ feature offering:
- Better HLSL support (wave intrinsics, etc.)
- Potential for advanced material logic and compute shaders
- Used by Lumen, Nanite, Virtual Shadow Maps, etc.
