# 编译额外运输设施模组。脚本会查找带有 SDK 的 dotnet，并检查游戏程序集。
[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [Alias('CurrentManagedPath')]
    # 在这里修改默认 Managed 目录，日常直接运行 .\build.ps1 即可。
    [string]$ManagedPath = 'C:\Program Files (x86)\Steam\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed'
)

$ErrorActionPreference = 'Stop'

# 默认采用上方设定的 Steam 路径。显式传入空值时，兼容环境变量配置。
if ([string]::IsNullOrWhiteSpace($ManagedPath)) {
    $ManagedPath = $env:CAPTAIN_INDUSTRY_MANAGED_PATH
}
if ([string]::IsNullOrWhiteSpace($ManagedPath) -and $env:COI_ROOT) {
    $ManagedPath = Join-Path $env:COI_ROOT 'Captain of Industry_Data\Managed'
}
if ([string]::IsNullOrWhiteSpace($ManagedPath)) {
    $ManagedPath = 'C:\Program Files (x86)\Steam\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed'
}

foreach ($file in 'Mafi.dll', 'Mafi.Base.dll', 'Mafi.Core.dll') {
    if (-not (Test-Path -LiteralPath (Join-Path $ManagedPath $file) -PathType Leaf)) {
        throw "找不到 $file。请补齐游戏文件，或修改 build.ps1 顶部的 ManagedPath 默认路径。当前目录为 $ManagedPath"
    }
}
$ManagedPath = (Resolve-Path -LiteralPath $ManagedPath).ProviderPath
$gameVersion = [Reflection.AssemblyName]::GetAssemblyName((Join-Path $ManagedPath 'Mafi.dll')).Version
if ($gameVersion -lt [Version]'0.8.8.0') {
    throw "游戏程序集版本为 $gameVersion，此源码修订需要 0.8.8 或更新版本。"
}

# PATH 中可能只有运行时，因此也检查微软安装脚本常用的用户级 SDK 目录。
$dotnetCandidates = @(Get-Command dotnet.exe -CommandType Application -All -ErrorAction SilentlyContinue |
    Select-Object -ExpandProperty Source)
if ($env:DOTNET_ROOT) {
    $dotnetCandidates += Join-Path $env:DOTNET_ROOT 'dotnet.exe'
}
$dotnetCandidates += Join-Path $env:LOCALAPPDATA 'Microsoft\dotnet\dotnet.exe'
$dotnetCandidates += Join-Path $env:USERPROFILE '.dotnet\dotnet.exe'
$dotnetPath = $null
foreach ($candidate in ($dotnetCandidates | Select-Object -Unique)) {
    if (-not (Test-Path -LiteralPath $candidate -PathType Leaf)) {
        continue
    }
    $sdks = @(& $candidate --list-sdks 2>$null)
    if ($LASTEXITCODE -eq 0 -and ($sdks | Where-Object { $_ -match '^(?:[89]|\d{2,})\.' })) {
        $dotnetPath = $candidate
        break
    }
}
if ($null -eq $dotnetPath) {
    throw '未找到 .NET SDK 8 或更新版本。请安装 SDK，或通过 DOTNET_ROOT 指定已有 SDK 的目录。'
}

$project = Join-Path $PSScriptRoot 'ExtraTransports.csproj'
$manifest = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'manifest.json') -Raw -Encoding UTF8 |
    ConvertFrom-Json
$buildArguments = @(
    'build',
    $project,
    '-c', $Configuration,
    "-p:CaptainIndustryManagedPath=$ManagedPath",
    "-p:Version=$($manifest.version)"
)

Write-Host "使用 SDK $dotnetPath"
Write-Host "游戏程序集目录 $ManagedPath"
Write-Host "正在编译 ExtraTransports $($manifest.version)（$Configuration）"
# 直接传递参数数组，游戏目录含空格时也能正确交给 MSBuild。
& $dotnetPath @buildArguments
if ($LASTEXITCODE -ne 0) {
    throw "编译失败，退出码为 $LASTEXITCODE。请查看上方编译器给出的错误。"
}

$outputDirectory = Join-Path $PSScriptRoot "bin\$Configuration\ExtraTransports"
if (-not (Test-Path -LiteralPath (Join-Path $outputDirectory 'ExtraTransports.dll') -PathType Leaf)) {
    throw "编译命令已结束，但预期目录里没有找到 DLL。请检查 $outputDirectory"
}
Write-Host "编译完成，输出目录为 $outputDirectory"
