# Func_Implementation not recognized when using network replicated functions
**Solution:**

- Change thie macro `GENERATED_BODY()` to `GENERATED_UCLASS_BODY()`
- Note to define the constructor with parameter `const FObjectInitializer& ObjectInitializer` ONLY in the cpp file, don't declare it in the header file;
- If takes parameters in the replicated function, `FString` and collections must be marked as const reference, for eaxample: `const TArray<Type>&` and `const FString&`;

# Typedef in UE4
The problem here is that the UHT is only able to recognize types that it already knows about, so when it tries to parse the typedef it will fail. The compiler will still accept the typdef though, so you can still use it in your code. Unfortunately you will not be able to use it in any form that the UHT touches, so you can't use it with UPROPERTY, as a UObject base class, as a UFUNCTION parameter, etc. The UHT is fundamentally unable to understand the typedef so it will always fail.


# FUCKING big PIT!!
A static member variable should be initialized outside the class, without `static` keyword when initializing; Ohterwise, a `unresolved external symbol` will show up;

# F**king rubbish interface #
UnrealEngine `UCLASS` just don't support multi-inheritance, it provides `UINTERFACE` and `IINTERFACE`	instead if multi-inheritance;

Don't waste time to get interface of UnrealEngine to work, especially associating RPC functions, it's so rubbish, don't support replication functions, and get rid of your thought to use any multi-inheritance feature in UnrealEngine, use `ActorComponent` instead; 

# Mother fucking C++ native USceneComponent Bug!!
**After a Long time of struggling, I found this is due to the hot load problem, first, check the blueprint class to see if the values are default, second restart the editor, if these two steps doesn't fix the problem, rebase the blueprint class or create a new one;**

# Dynamically spawned pawn can't receive input
A dynamically spawned pawn can't receive input event, but when placing it in the world, it can receive input event.  
The reason that cause this, is that, when spawning a pawn, it's not posesed by any controller by default, and a input event can only be received when the pawn is posessed. To fix this, just set `AutoPossessAI = EAutoPossessAI::PlacedInWorldOrSpawned;`, or change the posess rule in the blueprint defaults, category Pawn;

# USTRUCT Pit! #
UProperty in Ustruct cannot be marked `BlueprintReadWrite` or `BlueprintReadOnly`, in VS it will not show the correct error message, but compiling in Unreal Engine, will get the error message;

# AutonomousProxy Replication Pit #
It seems that, actor movement don't replicates from server to autonomousProxy client, so we have to move the actor in local function and server function when we want to move the actor itself from autonomousProxy client. Otherwise, the movement of actor will differ between server and autonomous client.

# Spawn Actor From TSubClassOf<>
if
```
TSubClassOf<AActor> subClass;
```
Use `subClass.Get()`, do not use `subClass->StaticClass()`

# BlueprintImplementableEvent pits #
### BlueprintImplementableEvent function parameters 
```
UFUNCTION(BlueprintImplementableEvent)
		void SetResult(const FText& t);
```
Use  `const FText&` instead of `FText`
### BlueprintImplementableEvent cannot be private, use protected instead

# Pit in Component field replicateion
If want fields in component to be replicated, besides marking them as `UPROPERTY(Replicated)`, **DO NOT** forget to set the component replicated too `comp->SetIsReplicated(true)`, otherwise, the component itself is not replicated, so the fields are not replicated neither.
