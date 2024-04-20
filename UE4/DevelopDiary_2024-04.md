
## UObject unexpectly deleted
Only `UObject`s that are marked as `UPROPERTY()` will be considered strong references, do not use raw pointers on any UObject, 
otherwise it will not be collected by GC system correctly, sometimes they could also be deleted unexpectly.

## Incorrect shadow casting for small moving objects
Small moving object could cast shadow incorrectly under `virtual shadow map` settings, use `shadow map` for rendering settings instead, `virtual shadow map` is designated for nanite meshes

## Unexpected Occlusion culling
Turn off `Occlusion Culling` in `Project Settings` to prevent objects culled unexpectly when it goes behind something else.

## ComposeRotators
When use `ComposeRotators` function, mind that the order of rotators passed to parameters matters, A is first applied, then B. Wrong order will lead to unexpected result.

## TScriptInterface
TScriptInterface is not explicitly designated to hold a strong reference to the object, so make sure to use it with `UPROPERTY()` to prevent it being gargage collected prematurely.

## Casting between TSubclassOf\<UObject\> and TSubclassOf\<SomeClass\>
Direct casting between `TSubclassOf` types for different classes is not directly supported because `TSubclassOf` is a template class designed to provide type safety around UClass pointers. However, you can achieve the desired behavior by checking if the `UClass` is indeed a subclass of the target type and then performing the assignment if the check passes.
```C++
// Assume GenericClass is of type TSubclassOf<UObject>
TSubclassOf<UObject> GenericClass = /* Some assignment */;

// Target variable for the casted class
TSubclassOf<ABP_MyActor> SpecificActorClass;

// Check if the GenericClass is a subclass of ABP_MyActor
if (GenericClass->IsChildOf<ABP_MyActor>())
{
    // It's safe to assign GenericClass to SpecificActorClass
    SpecificActorClass = *GenericClass; // Direct assignment is possible after ensuring type compatibility
}
else
{
    // Handle the case where GenericClass is not a subclass of ABP_MyActor
}
```

## Be ware that if you rename a `BlueprintImplementableEvent` or `BlueprintNativeEvent` function in cpp, you should re-implement it in blueprint as well, cause the implementation in blueprint doesn't change its name automatically, it just become another custom event.
