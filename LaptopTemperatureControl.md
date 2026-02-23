下面是把今天整个 **Alienware 游戏本调教 + 自动化方案**整理成的一份完整 Markdown，可直接保存使用 👍

---

# 🧊 Alienware Area-51 Laptop 游戏本散热与电源自动化方案

## 📌 目标

实现以下效果：

* 🎮 游戏时自动进入低温高效模式
* 💻 日常使用保持默认体验
* 🌡 GPU 温度明显下降
* 🔇 噪音降低
* 🤖 自动化无人值守运行
* 🔁 可随时手动覆盖

---

# ⭐ 一、散热与性能优化

## ✅ 1. AWCC 模式选择

**使用：Balanced**

### 原因

* 动态 CPU/GPU 功率分配
* 控制温度目标区间（约 80–85°C）
* 降低风扇波动
* Sustained performance 更优

---

## ✅ 2. CPU Power Shaping

### 设置路径

```
控制面板 → 电源选项 → GameCool → 高级设置 → 处理器电源管理
```

### 参数

* 最大处理器状态：**95–99%**
* 系统散热策略：Active（如存在）

### 效果

* CPU Turbo 功耗下降
* GPU 散热余量增加
* 整机温度下降 2–5°C

---

## ✅ 3. GPU Undervolt（核心优化）

### 工具

**MSI Afterburner**

### 设置

* Voltage point：**0.90 V**
* Frequency：≈ **2500 MHz**

### 操作流程

1. 打开 Afterburner
2. `Ctrl + F` 打开 Curve Editor
3. 找到 900 mV 点
4. 拖至 ~2500 MHz
5. Apply

### 结果

* GPU 温度：85°C → ~74°C
* 噪音下降
* 性能基本不变

---

# ⭐ 二、电源计划体系

## ✅ 4. 创建 GameCool 电源计划

### 方法

基于 Balanced 复制

### 核心修改

* CPU 最大状态：95–99%

### 作用

* 作为游戏专用 power profile

---

## ✅ 5. 手动切换脚本（单 BAT 参数方案）

### PowerMode.bat

```bat
@echo off

if /I "%1"=="game" (
    powercfg /setactive GAME_GUID
    timeout /t 5 /nobreak >nul
    goto :eof
)

if /I "%1"=="normal" (
    powercfg /setactive BALANCED_GUID
    timeout /t 5 /nobreak >nul
    goto :eof
)
```

### 使用

```
PowerMode.bat game
PowerMode.bat normal
```

---

# ⭐ 三、自动化（AutoHotkey）

## ✅ 6. 多游戏自动电源切换（AHK v2）

### GamePower.ahk

```ahk
#Requires AutoHotkey v2

Games := [
    "ArkAscended.exe",
    "UnrealEditor.exe"
]

GameGUID := "GAME_GUID"
NormalGUID := "BALANCED_GUID"
CheckInterval := 3000
EnableToast := true

InGameMode := false

SetTimer(CheckGames, CheckInterval)

CheckGames() {
    global Games, GameGUID, NormalGUID, InGameMode, EnableToast

    anyRunning := false
    runningName := ""

    for exe in Games {
        if ProcessExist(exe) {
            anyRunning := true
            runningName := exe
            break
        }
    }

    if anyRunning && !InGameMode {
        Run('powercfg /setactive ' GameGUID, , "Hide")
        InGameMode := true
        if EnableToast
            TrayTip("🎮 GameCool Enabled", runningName " detected", 3)
    }
    else if !anyRunning && InGameMode {
        Run('powercfg /setactive ' NormalGUID, , "Hide")
        InGameMode := false
        if EnableToast
            TrayTip("💻 Normal Mode", "All games closed", 3)
    }
}
```

### 特性

* 多游戏支持
* 状态机防抖
* Toast 通知
* 低资源占用

---

# ⭐ 四、开机自动运行

## 方法

```
Win + R → shell:startup
```

将 **AHK 快捷方式** 放入即可。

### 推荐

放快捷方式而不是脚本本体。

---

# ⭐ 五、最终系统架构

```
AWCC Balanced
      ↓
CPU Power Shaping
      ↓
GPU Undervolt
      ↓
GameCool Power Plan
      ↓
AHK Auto Switching
```

👉 Laptop power orchestration pipeline

---

# ⭐ 六、实际收益

* GPU 温度：~74°C
* 风扇噪音降低
* 自动游戏识别
* 自动电源切换
* 可手动 override
* 系统无感运行

---

# ⭐ 七、可选进阶方向

## 🔥 1. 全屏检测触发 GameCool

无需维护 exe 列表

## 🔥 2. Hybrid 触发

进程 + GPU load

## 🔥 3. Afterburner 自动 profile

完全无人值守

## 🔥 4. AHK 编译为 EXE

作为后台服务运行

---

# ⭐ ⭐ ⭐ 总结

本方案实现：

> 🎮 **高性能 + 低温 + 自动化 + 可控性**

属于：

👉 高端游戏本长期稳定配置

---

如果你愿意，我可以继续帮你整理：

* 📄 **UE5 工作站模式电源自动化**
* 🧊 **夏季降温策略模板**
* 🤖 **全屏触发 GameCool 自动化脚本**

直接形成你的个人调教库 😎
