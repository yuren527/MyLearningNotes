- First of all, the http functionality should be created in an Actor or a Component, other ways have not been tested;
  
- Add dependencies for HTTP and JSON to `ProjectName.Build.cs` file;  
Modify the line below:
    > PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore"});

    to:
    > PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore", "Http", "Json", "JsonUtilities" });

- Next, in `Project/Saved/Config/Engine.ini` file, you need to add the following section to setup the default values for the HTTP Request Module:
    ```
    [HTTP]
    HttpTimeout=300
    HttpConnectionTimeout=-1
    HttpReceiveTimeout=-1
    HttpSendTimeout=-1
    HttpMaxConnectionsPerServer=16
    bEnableHttp=true
    bUseNullHttp=false
    HttpDelayTime=0
    ```

- Then we can do the coding:

### ActorName.h ###
```C++
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "Runtime/Online/HTTP/Public/Http.h"
#include "TestGameMode.generated.h"

UCLASS(minimalapi)
class ATestGameMode : public AGameModeBase
{
    GENERATED_BODY()

public:
    FHttpModule* Http;

    ATestGameMode();
    //Actual Http call
    void MyHttpCall();
    //Assign this function to call when the GET request processes successfully
    void OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bWasSuccessful);

    virtual void BeginPlay() override;
};
```
### ActorName.cpp ###
```C++
#include "TestGameMode.h"
#include "TestCharacter.h"
#include "UObject/ConstructorHelpers.h"
#include "Runtime/Json/Public/Json.h"
#include "Engine/Engine.h"

ATestGameMode::ATestGameMode()
{
    // set default pawn class to our Blueprinted character
    static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/ThirdPersonCPP/Blueprints/ThirdPersonCharacter"));
    if (PlayerPawnBPClass.Class != NULL)
    {
    	DefaultPawnClass = PlayerPawnBPClass.Class;
    }
    //When the object is constructed, Get the HTTP module
    Http = &FHttpModule::Get();
}

void ATestGameMode::BeginPlay() {
    MyHttpCall();
    Super::BeginPlay();
}

//HTTP Call
void ATestGameMode::MyHttpCall() {
    TSharedRef<IHttpRequest> Request = Http->CreateRequest();
    Request->OnProcessRequestComplete().BindUObject(this, &ATestGameMode::OnResponseReceived);

    Request->SetURL("http://localhost:8081/WebApi/getInt.php");
    Request->SetVerb("GET");
    Request->SetHeader(TEXT("User-Agent"), "X-UnrealEngine-Agent");
    Request->SetHeader("Content-Type", TEXT("application/json"));
    Request->ProcessRequest();
}

void ATestGameMode::OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bWasSuccessful) {
    //Create a pointer to hold the json serialized data
    TSharedPtr<FJsonObject> JsonObject;
    //Create a reader pointer to read the json data
    TSharedRef<TJsonReader<>> Reader = TJsonReaderFactory<>::Create(Response->GetContentAsString());
    //Deserialize the json data given Reader and the actual object to deserialize
    if (FJsonSerializer::Deserialize(Reader, JsonObject)) {
        //Get the value of the json object by field name
    	int32 receivedInt = JsonObject->GetIntegerField("customInt");
        //Output to the engine
    	GEngine->AddOnScreenDebugMessage(1, 2.0f, FColor::Green, FString::FromInt(receivedInt));
    }
}
```
