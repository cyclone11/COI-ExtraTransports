# 额外运输设施（Extra Transports）

为《工业巨头》（Captain of Industry）添加四级、五级传送带和五级管道，通过两项研究解锁。

原作者为 Indra Kusuma（Luminiel / not-muted）。此源码修订面向更新 4.3（0.8.8），修订版本为 0.0.6。已使用本机游戏程序集完成 Release 编译及原型注册、初始化检查。

## 外观与菜单图标

四级平面、散料传送带使用紫色标记，五级平面带、散料带和管道使用亮绿色标记（`#54DF3B`）。选择菜单使用对应配色的无角标图标。

`Assets/Textures` 保存四张传送带贴图和一张管道贴图，`Assets/Icons` 保存五张菜单图标。项目将这些 PNG 嵌入 DLL。`TransportVisuals.cs` 在游戏的 `EarlyInit` 阶段克隆原版材质，替换颜色贴图，并向 `AssetsDb` 注册独立资源路径。`Data/TransportsData.cs` 将设施原型指向这些路径。管道贴图通过原版遮罩固定箭头和饰条的颜色，管身其余部分和流量指示器继续使用游戏的流体着色。

再次修改外观时，编辑对应的 PNG 后重新编译。颜色贴图必须保持原始尺寸和 UV 布局。已有设施重新加载存档后使用新版材质。

## 设施与研究

| 设施 | 运输速度参数 | 耗电基准 |
|---|---|---|
| 平面传送带 IV | 2.9 格/秒 | 原版三级传送带的 1.5 倍 |
| 散料传送带 IV | 2.9 格/秒 | 原版三级传送带的 1.5 倍 |
| 平面传送带 V | 5.8 格/秒 | 原版三级传送带的 2.25 倍 |
| 散料传送带 V | 5.8 格/秒 | 原版三级传送带的 2.25 倍 |
| 管道 V | 5.8 格/秒 | 零 |

研究顺序为研究实验室 III → 传送带 IV 与管道 V → 传送带 V。第一项模组研究解锁两种四级传送带与五级管道，第二项解锁两种五级传送带。两项研究分别采用原版三级传送带研究成本的两倍和三倍。

## 此次源码修订

- 添加游戏加载所需的 manifest.json，并使用原模组 ID ExtraTransports。
- 修正五级平面带、五级散料带、五级管道的名称与说明共用翻译键的问题。
- 加入英文及简体中文翻译，设施名称、说明和两项研究都可翻译。
- 克隆原版材质并替换涂装贴图，为四、五级传送带提供不同颜色。
- 使用内嵌 PNG 和显式图标路径，补齐五种新增设施的菜单图标。
- 每个新增设施使用独立的渲染缓存。
- 改用当前官方示例的 net48 目标框架，支持配置游戏安装目录。
- 编译结果按 ExtraTransports 目录整理，同时复制清单、译文、说明和许可证。

## 编译环境

需要 .NET SDK 8 或更新版本，以及目标游戏同一构建中的 Managed 目录。项目通过 NuGet 获取 .NET Framework 4.8 的引用程序集。

项目直接引用 Mafi.dll、Mafi.Base.dll、Mafi.Core.dll、Mafi.Unity.dll、UnityEngine.CoreModule.dll、UnityEngine.ImageConversionModule.dll 和游戏自带的 netstandard.dll。游戏更新后应重新检查接口和实机运行情况。

在源码根目录的 PowerShell 中直接运行：

~~~powershell
.\build.ps1
~~~

脚本默认采用 Steam 的标准 Managed 路径：

~~~text
C:\Program Files (x86)\Steam\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed
~~~

本机需要先准备该目录下的游戏程序集。游戏安装在其他位置时，修改 build.ps1 顶部的 ManagedPath 默认值即可。

脚本默认编译 Release，并自动查找 .NET SDK。需要调试版本时加 -Configuration Debug。

Windows 如提示脚本被执行策略阻止，可以仅为本次调用使用以下命令：

~~~powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\build.ps1
~~~

.NET SDK 8 下脚本直接编译 .csproj，兼容仓库自带 .slnx 尚未受当前工具链支持的情况。

编译结果位于 bin\Release\ExtraTransports\，包含以下文件：

~~~text
ExtraTransports/
├── ExtraTransports.dll
├── manifest.json
├── LICENSE
├── README.md
└── Translations/
    ├── en.json
    └── zh_Hans.json
~~~

## 安装与存档

将编译所得的 ExtraTransports 整个文件夹复制到 %APPDATA%\Captain of Industry\Mods。安装文件夹名称须与清单中的 ExtraTransports 完全一致。

模组新增的设施和研究会写入存档。清单设为允许加入已有存档，并将可从存档移除设为 false，直到完成移除与恢复流程的验证。

沿用原版已发布模组的设施与研究 ID，以维持已有内容的对应关系。根据官方 0.8.8 迁移说明，旧原型的未加前缀 ID 可以暂时保留，新的原型应使用 ExtraTransports. 前缀。下一次破坏存档兼容的游戏更新需要重新检查这项迁移安排。

## 翻译

游戏随玩家语言加载 Translations/zh_Hans.json 或 Translations/en.json。源码中的英文作为默认文本。

设施和研究词条由 ExtraTransportsTexts.cs 登记，使用 ExtraTransports. 前缀。存档使用原有设施和研究 ID。

启用模组后，可以在游戏控制台导出完整模板，并检查原生本地化系统报告的问题：

~~~text
export_mod_translations ExtraTransports
print_translation_errors
~~~

## 待实机确认

- 新游戏和已有存档都能启用模组。
- 两项研究可见，五种运输设施按研究正确解锁。
- 原版到四级、四级到五级的升级与降级操作可用。
- 紫色、亮绿色材质、无角标的图标和矿物运输动画正确显示。
- 输送速度、耗电和造价与参数一致，分流器、升降机和现有物流设施连接正常。
- 简体中文与英文的设施名称、说明和研究内容显示正确。
- 保存并重新加载后，既有运输设施和研究状态能恢复。

## 开发参考

- [官方模组目录结构](https://coigame.com/Modding/Mod-Structure)
- [官方清单字段说明](https://coigame.com/Modding/Manifest)
- [官方本地化与旧 ID 兼容说明](https://coigame.com/Modding/Translations)
- [更新 4.3 的 API 变动](https://coigame.com/Modding/Api-Changes/4.3)
- [原作者源码](https://github.com/not-muted/coi-extratransports)

## 许可证

源码沿用原作者的 [MIT 许可证](LICENSE)。新增贴图和图标基于本机游戏素材修改，原始素材的权利归 MaFi Games 所有。游戏程序集、着色器、法线及金属贴图由本机游戏提供。
