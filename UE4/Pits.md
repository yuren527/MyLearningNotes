# Func_Implementation not recognized when using network replicated functions
**Solution:**

- Change thie macro `GENERATED_BODY()` to `GENERATED_UCLASS_BODY()`
- Note to define the constructor with parameter `const FObjectInitializer& ObjectInitializer` ONLY in the cpp file, don't declare it in the header file;
- If takes parameters in the replicated function, `FString` and collections must be marked as const reference, for eaxample: `const TArray<Type>&` and `const FString&`;

# Typedef in UE4
The problem here is that the UHT is only able to recognize types that it already knows about, so when it tries to parse the typedef it will fail. The compiler will still accept the typdef though, so you can still use it in your code. Unfortunately you will not be able to use it in any form that the UHT touches, so you can't use it with UPROPERTY, as a UObject base class, as a UFUNCTION parameter, etc. The UHT is fundamentally unable to understand the typedef so it will always fail.

# Use Custom Struct as Key of TMap
To use custom struct as key in a TMap, `operator==` must be overloaded, and `GetTypeHash` function should be overloaded outside the struct; following is the example:
```C++
struct FStructName
 {
     int x;
     int y;
 
      bool operator==(const FStructName& a) const
     {
         return x == a.x && y == a.y;
     }
 }
 // end of struct
 
 FORCEINLINE uint32 GetTypeHash(const FStructName& b)
 {
     return FCrc::MemCrc32(&b, sizeof(FStructName));
 }
```

# FUCKING big PIT!!
A static member variable should be initialized outside the class, without `static` keyword when initializing; Ohterwise, a `unresolved external symbol` will show up;
# Some pits in Replication
Something in Replication worth remembering:
- Replicated variables should be following `UPROPERTY(Replicated)`, **Replication only go from server to client**, so it is not recommended to modify replicated variables locally on a client, modification should always be executed on server side;
- Because replication only go from server to client, so functions in `UPROPERTY(ReplicatedUsing=func)` will only execute on client; maybe it can be called manually, not tested yet; It is good to name the On-Rep function with `OnRep_` prefix, and must be marked as `UFUNCTION()`;

# F**king rubbish interface #
UnrealEngine `UCLASS` just don't support multi-inheritance, it provides `UINTERFACE` and `IINTERFACE`	instead if multi-inheritance;

Don't waste time to get interface of UnrealEngine to work, especially associating RPC functions, it's so rubbish, don't support replication functions, and get rid of your thought to use any multi-inheritance feature in UnrealEngine, use `ActorComponent` instead; 

# Mother fucking C++ native USceneComponent Bug!!
Sometimes, SceneComponents that are added to an actor in C++ code, cannot be recognized in blueprints, even are marked as editAnywhere, cannot be read from blueprint and will not get any error or warning!!!!!  
This is due to hot reload bug, to solve this, simply reparent the blueprint class;

# Dynamically spawned pawn can't receive input
A dynamically spawned pawn can't receive input event, but when placing it in the world, it can receive input event.  
The reason that cause this, is that, when spawning a pawn, it's not posesed by any controller by default, and a input event can only be received when the pawn is posessed. To fix this, just set `AutoPossessAI = EAutoPossessAI::PlacedInWorldOrSpawned;`, or change the posess rule in the blueprint defaults, category Pawn;


