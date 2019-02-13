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