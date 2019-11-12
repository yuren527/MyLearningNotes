# Some tips
## Test for nullptr to avoid crashing the whole game and editor
```C++
if(!ensure(trigglerVolume != nullptr)) return;
rootComponent = triggerVolume;
```
# Two-way operator overloading
**Just overload it outside of the class itself;**
```C++
inline bool operator ==(const UBuildingGroupID& lhs, const UBuildingGroupID& rhs)
{
	return lhs.GetID() == rhs.GetID() ? true : false;
}
```
# TMap replication
Currently, up to UE 4.21, `TMap` as parameter of a replicated function is not supported; So is `TSet`, only `TArray` can be used as replicated function parameters;
# .generated.h file
If a `.h` file contains any `UCLASS` or `USTRUCT`, `.generated.h` file should be included at the end of included file list; otherwise the UE macros will not be recognized;
# How to initialize a member without copy constructor and assignment operator
```C++
BuildingNodeGroup::BuildingNodeGroup(const FString & owner, FTransform trans) : ownerID(owner), baseTransform(trans), groupID(), nodes() {};
```
Need to use initializer-list, `groupID()` in the init-list can initialize the variable with a temporary nameless object which calls the default constructor;

For more information, see the [Value Initialization](https://en.cppreference.com/w/cpp/language/value_initialization);
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
