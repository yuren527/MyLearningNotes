## GPU Profiling
#### UnrealInsights
A standalone programe named `UnrealInsights.exe` is located in Unreal Engine binary folder, which can record the profiling data in files, use command-line argument `-trace=default,gpu` in standalone mode or packaged program. Or, use console command `Trace.start default,gpu` in gameplay.

#### ProfileGPU Command
The ProfileGPU command provides capabilities to identify the GPU cost of the various passes, sometimes down to the draw calls. You can either use the mouse-based UI or the text version. You can suppress the UI with `r.ProfileGPU.ShowUI`. The data is based on GPU timestamps and is usually quite accurate.


Use console command `ProfileGPU` or short-cut `Ctrl-Shift + ,` to open.

#### Live GPU profiling
Console command `stat gpu` to open text GPU profiling.

## ShadowMap VS Virtual ShadowMap
They are both used to render shadow into a distance field map, but ShadowMap is a traditional way which is less effective with high poly scenes, while virtual shadow map is suitable for nanite scenes.
