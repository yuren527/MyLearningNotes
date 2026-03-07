# Unreal Engine 开发效率优化指南

适用于同时使用多个 Unreal Engine 项目和多个引擎版本的开发环境。

优化目标：

* 减少 Shader 重新编译
* 减少 Derived Data Cache 重复生成
* 提高 Shader 编译速度
* 集中管理缓存

---

# 一、共享本机 Unreal Engine 缓存（DDC）

## 原理

Unreal Engine 会为资源生成 **Derived Data Cache (DDC)**，例如：

* Shader
* Texture 压缩
* Mesh 数据
* Nanite 数据

如果每个项目都有自己的缓存：

```
ProjectA → 编译 shader
ProjectB → 再编译 shader
ProjectC → 再编译 shader
```

共享缓存后：

```
所有项目 → 共用一份缓存
```

可以大幅减少 Shader 编译时间。

---

## 1 创建缓存目录

例如：

```
D:\UECache
```

建议：

* 使用 SSD / NVMe
* 预留 100GB+ 空间

---

## 2 设置环境变量

打开：

```
Win + R
```

输入：

```
sysdm.cpl
```

进入：

```
高级 → 环境变量
```

新增 **系统变量**：

```
变量名：
UE-LocalDataCachePath
```

```
变量值：
D:\UECache
```

---

## 3 重启 Unreal Engine

设置后需要：

```
重启 UE 或重启电脑
```

---

## 4 验证是否成功

打开任意 UE 项目后，缓存目录中会出现：

```
D:\UECache
 ├─ Zen
 └─ TestData
```

说明缓存已经正常工作。

---

## 5 清理缓存

缓存可以随时删除：

```
D:\UECache
```

删除后：

* UE 会重新生成缓存
* 第一次打开项目会重新编译 Shader

不会影响：

* 项目文件
* 资源
* C++
* 蓝图

---

# 二、提升 Shader 编译多线程能力

UE 默认会保留部分 CPU 线程，因此 Shader 编译不会使用全部 CPU。

可以通过配置提高并行度。

---

## 1 创建用户 Engine 配置

路径：

```
C:\Users\用户名\AppData\Local\UnrealEngine\Common\Config
```

创建文件：

```
Engine.ini
```

如果已经存在则直接编辑。

---

## 2 添加 Shader 编译配置

```
[DevOptions.Shaders]
NumUnusedShaderCompilingThreads=1
NumUnusedShaderCompilingThreadsDuringGame=1
ShaderCompilerCoreCountThreshold=999
```

---

## 参数说明

```
NumUnusedShaderCompilingThreads
```

UE 默认会保留多个 CPU 核心。

设置为：

```
1
```

表示只保留一个线程，其余全部参与 Shader 编译。

---

```
ShaderCompilerCoreCountThreshold
```

关闭 UE 对 CPU 核心数的限制。

---

## 3 生效方式

保存后：

```
重启 Unreal Engine
```

---

## 4 验证是否成功

打开任务管理器。

编译 Shader 时会看到多个进程：

```
ShaderCompileWorker.exe
```

CPU 使用率接近：

```
80%~100%
```

说明多线程编译已经生效。

---

# 三、推荐开发目录结构

建议统一管理 Unreal 开发资源：

```
D:\UE
 ├─ Engines
 │     ├─ UE_5.2
 │     ├─ UE_5.3
 │     └─ UE_5.4
 │
 ├─ Projects
 │     ├─ Personal
 │     └─ Work
 │
 └─ Cache
       └─ UE_DDC
```

然后设置：

```
UE-LocalDataCachePath = D:\UE\Cache\UE_DDC
```

---

# 四、版本兼容说明

多个 Unreal Engine 版本可以共享缓存。

原因：

DDC Key 包含：

```
EngineVersion
ShaderVersion
Platform
BuildSettings
```

不同版本的缓存会自动隔离。

---

# 五、建议忽略的项目目录

以下目录不应该提交到 Git / Perforce：

```
DerivedDataCache
Intermediate
Saved
Binaries
.vs
```

这些目录都是自动生成的。

---

# 六、推荐清理策略

建议定期清理缓存：

```
当缓存 > 100GB
```

清理方法：

```
删除 D:\UECache
```

Unreal Engine 会自动重建缓存。
