@echo off
echo ==========================================
echo Developer PC Cache Cleanup Script
echo ==========================================
echo.

echo Cleaning Windows Temp...
del /s /q "%TEMP%*" >nul 2>&1

echo Cleaning Windows System Temp...
del /s /q "C:\Windows\Temp*" >nul 2>&1

echo Cleaning Unreal Engine DerivedDataCache...
rd /s /q "%LOCALAPPDATA%\UnrealEngine\Common\DerivedDataCache" 2>nul

echo Cleaning NVIDIA Shader Cache...
rd /s /q "%LOCALAPPDATA%\NVIDIA\DXCache" 2>nul
rd /s /q "%LOCALAPPDATA%\NVIDIA\GLCache" 2>nul

echo Cleaning DirectX Shader Cache...
rd /s /q "%LOCALAPPDATA%\D3DSCache" 2>nul

echo Cleaning Visual Studio Cache...
rd /s /q "%LOCALAPPDATA%\Microsoft\VisualStudio\ComponentModelCache" 2>nul

echo Cleaning Epic Launcher Cache...
rd /s /q "%LOCALAPPDATA%\EpicGamesLauncher\Saved\webcache" 2>nul

echo Cleaning Steam Shader Cache...
rd /s /q "C:\Program Files (x86)\Steam\steamapps\shadercache" 2>nul

echo.
echo Cleanup Finished.
pause
