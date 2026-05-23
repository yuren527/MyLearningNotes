Here is a concise technical summary you can keep for your UE5 notes:

---

# UE5 Config Persistence Issue Summary

In Unreal Engine 5, using:

```cpp
UCLASS(Config=Game, DefaultConfig)
```

with:

```cpp
SaveConfig()
```

works mainly for:

* developer/project defaults
* editor configuration
* config hierarchy initialization

but is unreliable for persistent runtime user settings in packaged builds.

Observed issues included:

* custom config files not being recognized consistently
* runtime overrides not persisting
* `Saved/Config/.../Game.ini` being regenerated or deleted
* packaged behavior differing from editor behavior
* custom config names (e.g. `Config=SniperSysSettings`) not behaving like built-in config targets

Root cause:
UE distinguishes between:

* developer configuration (`Config=Game`, `UDeveloperSettings`)
* runtime user settings (`UGameUserSettings`)

`UGameUserSettings` is handled through a dedicated persistence pipeline:

* auto-loaded on startup
* reliably saved in packaged builds
* stored in `GameUserSettings.ini`
* intended specifically for runtime user overrides

Final conclusion:

* Use `UDeveloperSettings` for packaged default tuning and Project Settings exposure
* Use `UGameUserSettings` for persistent runtime/player settings
* Avoid relying on generic `SaveConfig()` for important runtime persistence behavior in packaged games
