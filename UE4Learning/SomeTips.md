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