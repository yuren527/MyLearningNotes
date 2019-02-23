# A very simple starter example
> .h
```C++
#include <CryEntitySystem/IEntityComponent>

class MyTestComponent : public IEntityComponent
{
public:
    MyTestComponent() = default;
    virtual ~MyTestComponent(){}

    static void ReflectType(Schematyc::CTypeDesc<MyTestComponent>& desc) {
		desc.SetGUID("{0A0BB4DA-0150-4AC1-B26B-F33129783E5D}"_cry_guid);
		desc.SetLabel("MyTestComponentLabel");
		desc.AddMember(&MyTestComponent::m_Force, 'for', "Force", "JumpForce", "Force of jump", 10.0f);
	}

    virtual void Initialize() override;

    virtual uint64 GetEventMask() const override;
    virtual void ProcessEvent(const SEntityEvent& event) override;
};
```
> .cpp
```C++
#include "StdAfx.h"
#include "MyTestComponent.h"
#include "DefaultComponents/Input/InputComponent.h"
#include "DefaultComponents/Physics/RigidBodyComponent.h"

#include <CrySchematyc/Reflection/TypeDesc.h>
#include <CrySchematyc/Utils/EnumFlags.h>
#include <CrySchematyc/Env/IEnvRegistry.h>
#include <CrySchematyc/Env/IEnvRegistrar.h>
#include <CrySchematyc/Env/Elements/EnvComponent.h>
#include <CrySchematyc/Env/Elements/EnvFunction.h>
#include <CrySchematyc/Env/Elements/EnvSignal.h>
#include <CrySchematyc/ResourceTypes.h>
#include <CrySchematyc/MathTypes.h>
#include <CrySchematyc/Utils/SharedString.h>

static void RegisterMyTestComponent(Schematyc::IEnvRegistrar& registrar)
{
	Schematyc::CEnvRegistrationScope scope = registrar.Scope(IEntity::GetEntityScopeGUID());
	{
		Schematyc::CEnvRegistrationScope componentScope = scope.Register(SCHEMATYC_MAKE_ENV_COMPONENT(MyTestComponent));
		// Functions
		{
		}
	}
}
CRY_STATIC_AUTO_REGISTER_FUNCTION(&RegisterMyTestComponent)

void MyTestComponent::Initialize()
{
	CryLog("MyTestComponent initializing");
	Cry::DefaultComponents::CInputComponent* input = m_pEntity->GetOrCreateComponent<Cry::DefaultComponents::CInputComponent>();

	input->RegisterAction("myTestComponent", "jump", [this](int activationMode, float value)
	{
		Cry::DefaultComponents::CRigidBodyComponent* rigid = m_pEntity->GetOrCreateComponent<Cry::DefaultComponents::CRigidBodyComponent>();
		rigid->ApplyImpulse(Vec3(0, 0, 1) * m_Force);
	});

	input->BindAction("myTestComponent", "jump", eAID_KeyboardMouse, eKI_Mouse1);
}

uint64 MyTestComponent::GetEventMask() const
{
	return BIT64(uint64(ENTITY_EVENT_UPDATE));
}

void MyTestComponent::ProcessEvent(const SEntityEvent & event)
{
	switch (event.event) {
	case ENTITY_EVENT_UPDATE:
	{
		float color[4]{ 1,0,0,1 };
		gEnv->pAuxGeomRenderer->Draw2dLabel(50, 50, 2, color, true, "I was updated");		
	}
	break;
	}
}
```
This example demonstrated:
- How to create a new subclass of `IEntityComponent`, which is something similar with  `UActorComponent` in UnealEngine;
- How to make our new class interactive with the sandbox editor;
- How to bind a simple input to the component;
- How to reveive and process events;

**Breakdown:**
- To create a custom `IEntityComponent`, just create a new class inherited from `IEntityComponent`, this is almost the same with creating a new ActorComponent in UnrealEngine, fortunately, we don't need the UCLASS() or something like that in UnrealEngine; All we need is to include the header file `CryEntitySystem/IEntityComponent.h` in our header and `StdAfx.h` in the cpp file for obvious reason;
- Initialize the class in `virtual void Initialize() override` function which is a interface function inherited from `IEntityComponent`, this is pretty the same with `virtual void BeginPlay()` in UnrealEngine;
- It is a little complicated to make our new class interactive with the sandbox editor, we need two major steps as following:
    - Use `static void ReflectType(Schematyc::CTypeDesc<MyTestComponent>& desc)` function to set the `CtypeDesc` of the class, this is some kind of struct that include all the informations that the editor need to know how to display and place our custom class; In the function body, call the functions of `desc` to set up the informations;
    -  After we set up the informations, we still need a function to register our new class to the editor, the syntax is `static void RegisterMyTestComponent(Schematyc::IEnvRegistrar& registrar)` and all of its body, this may be almost the same in every custom classes, so we just need to simply copy and paste it from some existing classes; Then call the macro function `CRY_STATIC_AUTO_REGISTER_FUNCTION(&RegisterMyTestComponent)`, until this we finished register our class, and now we can use it in the sandbox editor;
    -  There is one more thing that matters, don't forget to include all the header files that are needed to register, just copy them and paste;
 -  Binding input is shown above, no more to say here;
 -  To receive and process events, we need to override `virtual uint64 GetEventMask() const` and `virtual void ProcessEvent(const SEntityEvent& event)` of the base class, see exmple above, notice that all the qualifiers should be correct;

