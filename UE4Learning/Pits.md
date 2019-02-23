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

# FUCKING big PIT!!
类的静态成员变量，必须在类外进行初始化，初始化的类型前不加`static`关键字;
不初始化的情况下，如果用其他成员变量进行访问，就会出现`unresolved external symbol`，根本找不到原因，然而奇怪的是，函数的本地变量却可以进行访问，但如果再把此本地变量赋予成员变量，便又出现错误；
# Some pits in Replication
Something in Replication worth remembering:
- Replicated variables should be following `UPROPERTY(Replicated)`, **Replication only go from server to client**, so it is not recommended to modify replicated variables locally on a client, modification should always be executed on server side;
- Because replication only go from server to client, so functions in `UPROPERTY(ReplicatedUsing=func)` will only execute on client; maybe it can be called manually, not tested yet; It is good to name the On-Rep function with `OnRep_` prefix, and must be marked as `UFUNCTION()`;