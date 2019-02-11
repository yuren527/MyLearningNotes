# Func_Implementation not recognized when using network replicated functions
**Solution:**

- Change thie macro `GENERATED_BODY()` to `GENERATED_UCLASS_BODY()`
- Note to add a parameter `FObjectInitializer ObjectInitializer` to the class constructor ONLY in the cpp file;
- If takes parameters in the replicated function, `FString` and collections	must be marked as const reference, for eaxample: `const TArray<Type>&` and `const FString&`;

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