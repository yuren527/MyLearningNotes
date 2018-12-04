# OrbitPawn #
This file shows how to make a orbiting camera pawn in UE4. Next time when a orbiting camera is needed in some case, just copy the code from here.
### OrbitPawn.h
```C++
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Pawn.h"
#include "GameFramework/SpringArmComponent.h"
#include "Camera/CameraComponent.h"
#include "OBS_Pawn.generated.h"

UCLASS()
class OBSPROJECT_API AOrbitPawn : public APawn
{
    GENERATED_BODY()

public:
    AOrbitPawn();
    UPROPERTY(VisibleDefaultsOnly)
        USpringArmComponent* springArm;
    UPROPERTY(VisibleDefaultsOnly)
        UCameraComponent * camera;
    UPROPERTY(EditDefaultsOnly)
        float rotateSpeed = 10.0f;
    UPROPERTY(EditDefaultsOnly)
        float zoomSpeed = 15.0f;
    UPROPERTY(EditDefaultsOnly)
        float minZoomDist = 150.0f;
    UPROPERTY(EditDefaultsOnly)
        float maxZoomDist = 500.0f;

protected:
    virtual void BeginPlay() override;
    UPROPERTY()
        USceneComponent* rootComp;

    void RotateYaw(float axisValue);
    void RotatePitch(float axisValue);
    void Zoom(float axisValue);

public:	
    virtual void Tick(float DeltaTime) override;

    virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;
};
```
### OrbitPawn.cpp
```C++
#include "OrbitPawn.h"
#include "Kismet/KismetMathLibrary.h"

AOrbitPawn::AOrbitPawn()
{  
    PrimaryActorTick.bCanEverTick = true;
    rootComp = CreateDefaultSubobject<USceneComponent>(TEXT("root"));
    springArm = CreateDefaultSubobject<USpringArmComponent>(TEXT("springArm"));
    springArm->AttachToComponent(rootComp, FAttachmentTransformRules::KeepRelativeTransform);
    camera = CreateDefaultSubobject<UCameraComponent>(TEXT("camera"));	
    camera->AttachToComponent(springArm, FAttachmentTransformRules::KeepRelativeTransform);
    springArm->TargetArmLength = 300.0f;
}

void AOrbitPawn::BeginPlay()
{
    Super::BeginPlay();
}

void AOrbitPawn::Tick(float DeltaTime)
{
    Super::Tick(DeltaTime);
}

void AOrbitPawn::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
    Super::SetupPlayerInputComponent(PlayerInputComponent);

    PlayerInputComponent->BindAxis("RotateCameraYaw", this, &AOBS_Pawn::RotateYaw);
    PlayerInputComponent->BindAxis("RotateCameraPitch", this, &AOBS_Pawn::RotatePitch);
    PlayerInputComponent->BindAxis("ZoomCamera", this, &AOBS_Pawn::Zoom);
}

void AOrbitPawn::RotateYaw(float axisValue)
{
    AddActorWorldRotation(FRotator(0.0f, axisValue * rotateSpeed, 0.0f));
}
void AOrbitPawn::RotatePitch(float axisValue)
{	
    float newPitch = FMath::ClampAngle(springArm->RelativeRotation.Pitch + axisValue * rotateSpeed, -89.0f, 89.0f);
    springArm->SetRelativeRotation(FRotator(newPitch, 0.0f, 0.0f));
}
void AOrbitPawn::Zoom(float axisValue)
{
    springArm->TargetArmLength += axisValue * zoomSpeed;
    springArm->TargetArmLength = FMath::Clamp(springArm->TargetArmLength, minZoomDist, maxZoomDist);
}
```
To Make the input binding functional, some input events should be set in project settings.
> RotateCameraYaw  
> RotateCameraPitch  
> ZoomCamera  