# Some tips
## Test for nullptr to avoid crashing the whole game and editor
```C++
if(!ensure(trigglerVolume != nullptr)) return;
rootComponent = triggerVolume;
```