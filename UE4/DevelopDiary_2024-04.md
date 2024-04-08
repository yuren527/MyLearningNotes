# 2024-04-04
## UObject unexpectly deleted
Only `UObject`s that are marked as `UPROPERTY()` will be considered strong references, do not use raw pointers on any UObject, 
otherwise it will not be collected by GC system correctly, sometimes they could also be deleted unexpectly.

## Incorrect shadow casting for small moving objects
Small moving object could cast shadow incorrectly under `virtual shadow map` settings, use `shadow map` for rendering settings instead, `virtual shadow map` is designated for nanite meshes

## Unexpected Occlusion culling
Turn off `Occlusion Culling` in `Project Settings` to prevent objects culled unexpectly when it goes behind something else.

## ComposeRotators
When use `ComposeRotators` function, mind that the order of rotators passed to parameters matters, A is first applied, then B. Wrong order will lead to unexpected result.

# 2024-04-08
## TScriptInterface
TScriptInterface is not explicitly designated to hold a strong reference to the object, so make sure to use it with `UPROPERTY()` to prevent it being gargage collected prematurely.
