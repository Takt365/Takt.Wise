// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktServHelper.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：服务器硬件信息帮助类，使用Hardware.Info获取系统硬件信息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Hardware.Info;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Takt.Shared.Helpers;

/// <summary>
/// 服务器硬件信息帮助类（Hardware.Info）。
/// </summary>
/// <remarks>
/// 非纯工具网关：内部缓存 IHardwareInfo 及系统 I/O 刷新结果；硬件清单来自 Hardware.Info，网卡联网状态使用跨平台 System.Net.NetworkInformation + Ping + DNS。
/// </remarks>
public static class TaktServHelper
{
    private static readonly IHardwareInfo _hardwareInfo = new HardwareInfo();
    private static bool _initialized = false;
    private static readonly object _lockObject = new object();
    /// <summary>
    /// CPU 性能计数器两次 Refresh 间隔（毫秒）；WMI/perf 差值采样必须 ≥1000
    /// </summary>
    private const int CpuUsageSampleDelayMs = 1000;
    /// <summary>
    /// 互联网 Ping 探测目标
    /// </summary>
    private const string InternetPingHost = "8.8.8.8";
    /// <summary>
    /// 互联网 Ping 超时（毫秒）
    /// </summary>
    private const int InternetPingTimeoutMs = 2000;
    /// <summary>
    /// DNS 探测主机名
    /// </summary>
    private const string DnsProbeHost = "www.microsoft.com";

    /// <summary>
    /// 初始化硬件信息（首次调用时会刷新所有硬件信息）
    /// </summary>
    private static void EnsureInitialized()
    {
        if (!_initialized)
        {
            lock (_lockObject)
            {
                if (!_initialized)
                {
                    try
                    {
                        // 按照官方示例，分开刷新各个硬件信息
                        _hardwareInfo.RefreshOperatingSystem();
                        _hardwareInfo.RefreshMemoryStatus();
                        _hardwareInfo.RefreshBatteryList();
                        _hardwareInfo.RefreshBIOSList();
                        _hardwareInfo.RefreshComputerSystemList();
                        _hardwareInfo.RefreshCPUList(includePercentProcessorTime: false);
                        _hardwareInfo.RefreshDriveList();
                        _hardwareInfo.RefreshKeyboardList();
                        _hardwareInfo.RefreshMemoryList();
                        _hardwareInfo.RefreshMonitorList();
                        _hardwareInfo.RefreshMotherboardList();
                        _hardwareInfo.RefreshMouseList();
                        _hardwareInfo.RefreshNetworkAdapterList();
                        _hardwareInfo.RefreshPrinterList();
                        _hardwareInfo.RefreshSoundDeviceList();
                        _hardwareInfo.RefreshVideoControllerList();
                        _initialized = true;
                    }
                    catch (Exception ex)
                    {
                        TaktLogger.Warning(ex, "初始化硬件信息失败");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 刷新所有硬件信息
    /// </summary>
    public static void RefreshAll()
    {
        lock (_lockObject)
        {
            try
            {
                // 按照官方示例，分开刷新各个硬件信息
                _hardwareInfo.RefreshOperatingSystem();
                _hardwareInfo.RefreshMemoryStatus();
                _hardwareInfo.RefreshBatteryList();
                _hardwareInfo.RefreshBIOSList();
                _hardwareInfo.RefreshComputerSystemList();
                _hardwareInfo.RefreshCPUList(includePercentProcessorTime: false);
                _hardwareInfo.RefreshDriveList();
                _hardwareInfo.RefreshKeyboardList();
                _hardwareInfo.RefreshMemoryList();
                _hardwareInfo.RefreshMonitorList();
                _hardwareInfo.RefreshMotherboardList();
                _hardwareInfo.RefreshMouseList();
                _hardwareInfo.RefreshNetworkAdapterList();
                _hardwareInfo.RefreshPrinterList();
                _hardwareInfo.RefreshSoundDeviceList();
                _hardwareInfo.RefreshVideoControllerList();
                _initialized = true;
            }
            catch (Exception ex)
            {
                TaktLogger.Error(ex, "刷新硬件信息失败");
            }
        }
    }

    /// <summary>
    /// 从 RuntimeInformation.OSDescription 提取数字版本段（如 10.0.26200）
    /// </summary>
    private static readonly Regex OsDescriptionVersionRegex = new(
        @"(\d+\.\d+\.\d+(?:\.\d+)?)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// 获取操作系统版本（RuntimeInformation 平台分发 + 各平台原生读取 + OSDescription 兜底）
    /// </summary>
    /// <returns>版本字符串；无法解析时返回空串</returns>
    public static string GetOsVersion()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var version = GetWindowsVersion();
                if (!string.IsNullOrWhiteSpace(version))
                {
                    return version.Trim();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var version = GetLinuxVersion();
                if (!string.IsNullOrWhiteSpace(version))
                {
                    return version.Trim();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var version = GetMacVersion();
                if (!string.IsNullOrWhiteSpace(version))
                {
                    return version.Trim();
                }
            }
            var fallback = RuntimeInformation.OSDescription;
            return string.IsNullOrWhiteSpace(fallback) ? string.Empty : fallback.Trim();
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "GetOsVersion 平台分发失败，回退 OSDescription");
            var fallback = RuntimeInformation.OSDescription;
            return string.IsNullOrWhiteSpace(fallback) ? string.Empty : fallback.Trim();
        }
    }

    /// <summary>
    /// Windows 版本（RuntimeInformation.OSDescription 解析 + Hardware.Info Version，无 Win32 注册表）
    /// </summary>
    /// <returns>如 10.0.26200；失败返回空串</returns>
    private static string GetWindowsVersion()
    {
        if (TryExtractNumericOsVersion(RuntimeInformation.OSDescription, out var parsed))
        {
            return parsed;
        }
        try
        {
            EnsureInitialized();
            var os = _hardwareInfo.OperatingSystem;
            if (os != null)
            {
                var (_, version) = ResolveOperatingSystemNameAndVersion(os);
                if (!string.IsNullOrWhiteSpace(version))
                {
                    return version.Trim();
                }
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "Hardware.Info 读取 Windows 版本失败");
        }
        var description = RuntimeInformation.OSDescription;
        return string.IsNullOrWhiteSpace(description) ? string.Empty : description.Trim();
    }

    /// <summary>
    /// 从 RuntimeInformation.OSDescription 提取数字版本（如 Microsoft Windows 10.0.26200.0 → 10.0.26200.0）
    /// </summary>
    /// <param name="osDescription">OSDescription 原文</param>
    /// <param name="version">解析出的版本</param>
    /// <returns>是否解析成功</returns>
    private static bool TryExtractNumericOsVersion(string? osDescription, out string version)
    {
        version = string.Empty;
        if (string.IsNullOrWhiteSpace(osDescription))
        {
            return false;
        }
        var match = OsDescriptionVersionRegex.Match(osDescription);
        if (!match.Success)
        {
            return false;
        }
        version = match.Groups[1].Value.Trim();
        return !string.IsNullOrWhiteSpace(version);
    }

    /// <summary>
    /// 从 /etc/os-release 读取 Linux 发行版版本
    /// </summary>
    /// <returns>VERSION_ID 或 PRETTY_NAME；失败返回空串</returns>
    private static string GetLinuxVersion()
    {
        const string osReleasePath = "/etc/os-release";
        try
        {
            if (!File.Exists(osReleasePath))
            {
                return string.Empty;
            }
            var lines = File.ReadAllLines(osReleasePath);
            var versionId = ParseOsReleaseValue(lines, "VERSION_ID");
            if (!string.IsNullOrWhiteSpace(versionId))
            {
                return versionId;
            }
            var prettyName = ParseOsReleaseValue(lines, "PRETTY_NAME");
            if (!string.IsNullOrWhiteSpace(prettyName))
            {
                return prettyName;
            }
            var version = ParseOsReleaseValue(lines, "VERSION");
            if (!string.IsNullOrWhiteSpace(version))
            {
                return version;
            }
            var name = ParseOsReleaseValue(lines, "NAME");
            return name ?? string.Empty;
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "读取 /etc/os-release 失败");
            return string.Empty;
        }
    }

    /// <summary>
    /// 解析 /etc/os-release 键值
    /// </summary>
    /// <param name="lines">文件行</param>
    /// <param name="key">键名（如 VERSION_ID）</param>
    /// <returns>去引号后的值；未找到返回 null</returns>
    private static string? ParseOsReleaseValue(IReadOnlyList<string> lines, string key)
    {
        ArgumentNullException.ThrowIfNull(lines);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var prefix = key + "=";
        foreach (var line in lines)
        {
            if (!line.StartsWith(prefix, StringComparison.Ordinal))
            {
                continue;
            }
            var value = line[prefix.Length..].Trim();
            if (value.Length >= 2 && value.StartsWith('"') && value.EndsWith('"'))
            {
                value = value[1..^1];
            }
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        return null;
    }

    /// <summary>
    /// 通过 sw_vers 读取 macOS 版本
    /// </summary>
    /// <returns>如 14.2.1；失败返回空串</returns>
    private static string GetMacVersion()
    {
        try
        {
            var productVersion = RunSwVersArgument("-productVersion");
            if (!string.IsNullOrWhiteSpace(productVersion))
            {
                return productVersion;
            }
            return RunSwVersArgument("-productName") ?? string.Empty;
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "执行 sw_vers 失败");
            return string.Empty;
        }
    }

    /// <summary>
    /// 执行 sw_vers 子命令并返回标准输出
    /// </summary>
    /// <param name="argument">sw_vers 参数（如 -productVersion）</param>
    /// <returns>trim 后的输出；失败返回 null</returns>
    private static string? RunSwVersArgument(string argument)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(argument);
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "sw_vers",
            Arguments = argument,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        if (!process.Start())
        {
            return null;
        }
        var output = process.StandardOutput.ReadToEnd().Trim();
        if (!process.WaitForExit(5000))
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (Exception killEx)
            {
                TaktLogger.Debug(killEx, "终止 sw_vers 超时进程失败");
            }
            return null;
        }
        return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output) ? output : null;
    }

    /// <summary>
    /// 获取操作系统名称（不含版本号；禁止 OperatingSystem.ToString 整段输出）
    /// </summary>
    /// <returns>操作系统名称，如 Microsoft Windows 11 家庭版 中文版</returns>
    public static string GetOperatingSystem()
    {
        EnsureInitialized();
        try
        {
            var os = _hardwareInfo.OperatingSystem;
            if (os == null)
            {
                return "Unknown";
            }
            var (name, _) = ResolveOperatingSystemNameAndVersion(os);
            return string.IsNullOrWhiteSpace(name) ? "Unknown" : name;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取操作系统信息失败");
            return "Unknown";
        }
    }

    /// <summary>
    /// 获取操作系统语言信息
    /// </summary>
    /// <returns>操作系统语言信息</returns>
    public static OperatingSystemLanguageInfo GetOperatingSystemLanguage()
    {
        EnsureInitialized();
        try
        {
            var osLanguage = new OperatingSystemLanguageInfo();
            
            // 获取当前系统文化
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            var currentUICulture = System.Globalization.CultureInfo.CurrentUICulture;
            
            osLanguage.CurrentCulture = currentCulture.Name;
            osLanguage.CurrentCultureDisplayName = currentCulture.DisplayName;
            osLanguage.CurrentCultureNativeName = currentCulture.NativeName;
            
            osLanguage.CurrentUICulture = currentUICulture.Name;
            osLanguage.CurrentUICultureDisplayName = currentUICulture.DisplayName;
            osLanguage.CurrentUICultureNativeName = currentUICulture.NativeName;
            
            // 获取所有安装的语言
            osLanguage.InstalledLanguages = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
                .Select(c => new InstalledLanguage
                {
                    CultureCode = c.Name,
                    DisplayName = c.DisplayName,
                    NativeName = c.NativeName,
                    EnglishName = c.EnglishName,
                    IsNeutralCulture = c.IsNeutralCulture,
                    IsInstalledWin32Culture = c.LCID != 4096 // LCID 4096 表示未安装的文化
                })
                .OrderBy(c => c.CultureCode)
                .ToList();
            
            osLanguage.OsVersion = GetOsVersion();

            // 从 Hardware.Info 读取系统默认语言
            try
            {
                var os = _hardwareInfo.OperatingSystem;
                if (os != null)
                {
                    var systemLanguage = GetPropertyValueNullable<string>(os, "SystemDefaultLanguage");
                    if (!string.IsNullOrEmpty(systemLanguage))
                    {
                        osLanguage.SystemDefaultLanguage = systemLanguage;
                    }
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Debug(ex, "读取操作系统默认语言失败");
            }
            
            return osLanguage;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取操作系统语言信息失败");
            return new OperatingSystemLanguageInfo();
        }
    }

    /// <summary>
    /// 刷新 CPU 使用率快照（Hardware.Info 标准：两次 RefreshCPUList + Sleep，仅一次 Refresh 常为 0）
    /// </summary>
    /// <remarks>
    /// WMI / Linux /proc/stat 基于时间差；第一次预热，Sleep ≥1000ms 后第二次取真实 PercentProcessorTime。
    /// v110 入口为 CpuList；物理 CPU 总使用率用 CPU.PercentProcessorTime，逻辑核心用 CpuCoreList（禁止对核心 Sum）。
    /// </remarks>
    private static void RefreshCpuUsageSnapshot()
    {
        lock (_lockObject)
        {
            _hardwareInfo.RefreshCPUList(includePercentProcessorTime: true);
            Thread.Sleep(CpuUsageSampleDelayMs);
            _hardwareInfo.RefreshCPUList(includePercentProcessorTime: true);
        }
    }

    /// <summary>
    /// 将 Hardware.Info 逻辑核心列表映射为 CpuCoreInfo（排除 _Total 汇总项）
    /// </summary>
    /// <param name="cpu">Hardware.Info 物理 CPU（Socket）</param>
    /// <returns>各逻辑核心使用率；CpuCoreList 项数等于逻辑处理器数，无法区分物理核与超线程</returns>
    private static List<CpuCoreInfo> MapCpuCoreList(Hardware.Info.CPU cpu)
    {
        ArgumentNullException.ThrowIfNull(cpu);
        return cpu.CpuCoreList
            .Where(core => !string.Equals(core.Name, "_Total", StringComparison.OrdinalIgnoreCase))
            .Select(core => new CpuCoreInfo
            {
                Name = core.Name ?? string.Empty,
                UsagePercent = NormalizeCpuUsagePercent(core.PercentProcessorTime)
            })
            .ToList();
    }

    /// <summary>
    /// 将 Hardware.Info 物理 CPU 映射为 CpuInfo
    /// </summary>
    /// <param name="cpu">Hardware.Info 物理 CPU（Socket）</param>
    /// <returns>物理 CPU 总使用率取自 PercentProcessorTime，禁止对 CpuCoreList 求和</returns>
    private static CpuInfo MapCpuInfoFromHardware(Hardware.Info.CPU cpu)
    {
        ArgumentNullException.ThrowIfNull(cpu);
        return new CpuInfo
        {
            Name = cpu.Name ?? "Unknown",
            Manufacturer = cpu.Manufacturer ?? "Unknown",
            NumberOfCores = cpu.NumberOfCores,
            NumberOfLogicalProcessors = cpu.NumberOfLogicalProcessors,
            ProcessorId = cpu.ProcessorId ?? string.Empty,
            SocketDesignation = GetPropertyValueNullable<string>(cpu, "SocketDesignation") ?? string.Empty,
            UsagePercent = NormalizeCpuUsagePercent(cpu.PercentProcessorTime),
            CoreList = MapCpuCoreList(cpu)
        };
    }

    /// <summary>
    /// 获取 CPU 信息（含各物理 CPU 与逻辑核心使用率）
    /// </summary>
    /// <param name="systemCpuUsagePercent">整机 CPU 使用率（%）：各物理 CPU PercentProcessorTime 算术平均</param>
    /// <returns>每个 Hardware.Info CpuList 项对应一个物理 CPU（Socket）</returns>
    public static List<CpuInfo> GetCpuInfo(out double systemCpuUsagePercent)
    {
        EnsureInitialized();
        systemCpuUsagePercent = 0;
        try
        {
            RefreshCpuUsageSnapshot();
            var hardwareCpuList = _hardwareInfo.CpuList;
            var result = hardwareCpuList.Select(MapCpuInfoFromHardware).ToList();
            systemCpuUsagePercent = CalculateAggregateCpuUsagePercent(result);
            return result;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取CPU信息失败");
            return new List<CpuInfo>();
        }
    }

    /// <summary>
    /// 获取CPU信息
    /// </summary>
    /// <returns>CPU信息列表</returns>
    public static List<CpuInfo> GetCpuInfo()
    {
        return GetCpuInfo(out _);
    }

    /// <summary>
    /// 获取内存信息
    /// </summary>
    /// <returns>内存信息</returns>
    public static MemoryInfo GetMemoryInfo()
    {
        EnsureInitialized();
        try
        {
            var status = _hardwareInfo.MemoryStatus;
            return new MemoryInfo
            {
                TotalPhysicalMemory = status?.TotalPhysical ?? 0,
                AvailablePhysicalMemory = status?.AvailablePhysical ?? 0,
                UsedPhysicalMemory = (status?.TotalPhysical ?? 0) - (status?.AvailablePhysical ?? 0),
                TotalVirtualMemory = status?.TotalVirtual ?? 0,
                AvailableVirtualMemory = status?.AvailableVirtual ?? 0,
                UsedVirtualMemory = (status?.TotalVirtual ?? 0) - (status?.AvailableVirtual ?? 0)
            };
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取内存信息失败");
            return new MemoryInfo();
        }
    }

    /// <summary>
    /// 获取物理内存条列表（Hardware.Info MemoryList）
    /// </summary>
    /// <returns>各 DIMM 容量与标识；不含单条使用率（Hardware.Info 不提供）</returns>
    public static List<MemoryModuleInfo> GetMemoryModuleList()
    {
        EnsureInitialized();
        try
        {
            return _hardwareInfo.MemoryList.Select(module => new MemoryModuleInfo
            {
                BankLabel = module.BankLabel ?? string.Empty,
                Capacity = module.Capacity,
                Speed = module.Speed,
                Manufacturer = module.Manufacturer ?? string.Empty,
                PartNumber = GetPropertyValueNullable<string>(module, "PartNumber") ?? string.Empty,
                SerialNumber = GetPropertyValueNullable<string>(module, "SerialNumber") ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取内存条信息失败");
            return new List<MemoryModuleInfo>();
        }
    }

    /// <summary>
    /// 获取磁盘信息
    /// </summary>
    /// <returns>磁盘信息列表</returns>
    public static List<DriveInfo> GetDriveInfo()
    {
        EnsureInitialized();
        try
        {
            var result = new List<DriveInfo>();
            foreach (var drive in _hardwareInfo.DriveList)
            {
                var hasVolume = false;
                foreach (var partition in drive.PartitionList)
                {
                    foreach (var volume in partition.VolumeList)
                    {
                        hasVolume = true;
                        var totalSize = volume.Size;
                        var freeSpace = volume.FreeSpace;
                        result.Add(new DriveInfo
                        {
                            Name = !string.IsNullOrWhiteSpace(volume.Name) ? volume.Name : drive.Name ?? "Unknown",
                            DriveType = !string.IsNullOrWhiteSpace(drive.MediaType) ? drive.MediaType : "Unknown",
                            VolumeLabel = volume.VolumeName ?? string.Empty,
                            FileSystem = volume.FileSystem ?? string.Empty,
                            TotalSize = totalSize,
                            FreeSpace = freeSpace,
                            UsedSpace = totalSize >= freeSpace ? totalSize - freeSpace : 0UL,
                            SerialNumber = !string.IsNullOrWhiteSpace(volume.VolumeSerialNumber)
                                ? volume.VolumeSerialNumber
                                : drive.SerialNumber ?? string.Empty,
                            Model = drive.Model ?? string.Empty
                        });
                    }
                }
                if (!hasVolume && drive.Size > 0UL)
                {
                    result.Add(new DriveInfo
                    {
                        Name = drive.Name ?? "Unknown",
                        DriveType = !string.IsNullOrWhiteSpace(drive.MediaType) ? drive.MediaType : "Unknown",
                        VolumeLabel = string.Empty,
                        FileSystem = string.Empty,
                        TotalSize = drive.Size,
                        FreeSpace = 0UL,
                        UsedSpace = drive.Size,
                        SerialNumber = drive.SerialNumber ?? string.Empty,
                        Model = drive.Model ?? string.Empty
                    });
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取磁盘信息失败");
            return new List<DriveInfo>();
        }
    }

    /// <summary>
    /// 获取网络适配器信息
    /// </summary>
    /// <returns>网络适配器信息列表</returns>
    public static List<NetworkAdapterInfo> GetNetworkAdapterInfo()
    {
        EnsureInitialized();
        try
        {
            _hardwareInfo.RefreshNetworkAdapterList(includeNetworkAdapterConfiguration: true);
            var connectivity = ProbeNetworkConnectivity();
            return _hardwareInfo.NetworkAdapterList.Select(adapter => new NetworkAdapterInfo
            {
                Name = adapter.Name ?? "Unknown",
                Description = adapter.Description ?? string.Empty,
                MACAddress = adapter.MACAddress ?? string.Empty,
                IpAddress = ResolveNetworkAdapterIpAddresses(adapter),
                Speed = adapter.Speed,
                Status = ResolveNetworkAdapterStatus(adapter, connectivity)
            }).ToList();
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取网络适配器信息失败");
            return new List<NetworkAdapterInfo>();
        }
    }

    /// <summary>
    /// 探测本机互联网与 DNS 连通性（每轮 GetNetworkAdapterInfo 调用一次）
    /// </summary>
    /// <returns>互联网 Ping 与 DNS 解析结果</returns>
    private static NetworkConnectivitySnapshot ProbeNetworkConnectivity()
    {
        return new NetworkConnectivitySnapshot
        {
            HasInternet = HasInternet(),
            DnsWorks = DnsWorks()
        };
    }

    /// <summary>
    /// 是否存在可用网络接口（OperationalStatus.Up，排除 Loopback/Tunnel）
    /// </summary>
    /// <returns>是否存在至少一块可用网卡</returns>
    private static bool HasActiveNic()
    {
        return NetworkInterface.GetAllNetworkInterfaces().Any(IsActiveNetworkInterface);
    }

    /// <summary>
    /// 判断网络接口是否可用（已连接且非回环/隧道）
    /// </summary>
    /// <param name="nic">网络接口</param>
    /// <returns>是否可用</returns>
    private static bool IsActiveNetworkInterface(NetworkInterface nic)
    {
        ArgumentNullException.ThrowIfNull(nic);
        return nic.OperationalStatus == OperationalStatus.Up
            && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
            && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel;
    }

    /// <summary>
    /// 是否能访问互联网（ICMP Ping 8.8.8.8）
    /// </summary>
    /// <returns>Ping 成功返回 true</returns>
    private static bool HasInternet()
    {
        try
        {
            using var ping = new Ping();
            var reply = ping.Send(InternetPingHost, InternetPingTimeoutMs);
            return reply.Status == IPStatus.Success;
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "Ping 探测互联网连通性失败");
            return false;
        }
    }

    /// <summary>
    /// DNS 是否可用（解析探测主机名）
    /// </summary>
    /// <returns>解析成功返回 true</returns>
    private static bool DnsWorks()
    {
        try
        {
            _ = Dns.GetHostEntry(DnsProbeHost);
            return true;
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "DNS 探测失败");
            return false;
        }
    }

    /// <summary>
    /// 是否已联网（可用网卡 + 互联网 Ping + DNS）
    /// </summary>
    /// <returns>三项均满足时返回 true</returns>
    public static bool IsOnline()
    {
        return HasActiveNic() && HasInternet() && DnsWorks();
    }

    /// <summary>
    /// 解析网卡联网状态（匹配 NetworkInterface + 互联网/DNS 探测）
    /// </summary>
    /// <param name="adapter">Hardware.Info 网卡</param>
    /// <param name="connectivity">本机互联网/DNS 探测快照</param>
    /// <returns>Down / NoInternet / DnsFault / Online</returns>
    private static string ResolveNetworkAdapterStatus(NetworkAdapter adapter, NetworkConnectivitySnapshot connectivity)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        ArgumentNullException.ThrowIfNull(connectivity);
        var nic = TryFindMatchingNetworkInterface(adapter);
        if (nic == null || !IsActiveNetworkInterface(nic))
        {
            return "Down";
        }
        if (!connectivity.HasInternet)
        {
            return "NoInternet";
        }
        if (!connectivity.DnsWorks)
        {
            return "DnsFault";
        }
        return "Online";
    }

    /// <summary>
    /// 将 Hardware.Info 网卡与 System.Net.NetworkInformation 网卡对齐（优先 MAC，其次名称/描述）
    /// </summary>
    /// <param name="adapter">Hardware.Info 网卡</param>
    /// <returns>匹配到的 NetworkInterface；未匹配返回 null</returns>
    private static NetworkInterface? TryFindMatchingNetworkInterface(NetworkAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        var targetMac = NormalizeMacAddress(adapter.MACAddress);
        if (!string.IsNullOrEmpty(targetMac))
        {
            var byMac = interfaces.FirstOrDefault(nic =>
                string.Equals(NormalizeMacAddress(nic.GetPhysicalAddress().ToString()), targetMac, StringComparison.OrdinalIgnoreCase));
            if (byMac != null)
            {
                return byMac;
            }
        }
        var name = adapter.Name?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(name))
        {
            var byName = interfaces.FirstOrDefault(nic =>
                string.Equals(nic.Name, name, StringComparison.OrdinalIgnoreCase)
                || string.Equals(nic.Description, name, StringComparison.OrdinalIgnoreCase));
            if (byName != null)
            {
                return byName;
            }
        }
        var description = adapter.Description?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(description))
        {
            return interfaces.FirstOrDefault(nic =>
                string.Equals(nic.Description, description, StringComparison.OrdinalIgnoreCase));
        }
        return null;
    }

    /// <summary>
    /// 规范化 MAC 地址为无分隔符大写十六进制
    /// </summary>
    /// <param name="macAddress">MAC 字符串</param>
    /// <returns>规范化 MAC；无效时返回空串</returns>
    private static string NormalizeMacAddress(string? macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
        {
            return string.Empty;
        }
        var chars = macAddress.Where(static c => char.IsAsciiHexDigit(c)).ToArray();
        return chars.Length == 0 ? string.Empty : new string(chars).ToUpperInvariant();
    }

    /// <summary>
    /// 本机互联网/DNS 连通性快照
    /// </summary>
    private sealed class NetworkConnectivitySnapshot
    {
        /// <summary>
        /// 互联网 Ping 是否成功
        /// </summary>
        public bool HasInternet { get; init; }
        /// <summary>
        /// DNS 解析是否成功
        /// </summary>
        public bool DnsWorks { get; init; }
    }

    /// <summary>
    /// 解析网卡 IPv4 地址（Hardware.Info IPAddressList）
    /// </summary>
    /// <param name="adapter">Hardware.Info 网卡对象</param>
    /// <returns>逗号分隔的 IPv4 地址；无有效地址时返回空字符串</returns>
    private static string ResolveNetworkAdapterIpAddresses(NetworkAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        if (adapter.IPAddressList == null || adapter.IPAddressList.Count == 0)
        {
            return string.Empty;
        }
        var ipAddresses = adapter.IPAddressList
            .Select(ip => ip?.ToString())
            .Where(IsDisplayableIpv4)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        return ipAddresses.Count > 0
            ? string.Join(", ", ipAddresses)
            : string.Empty;
    }

    /// <summary>
    /// 网卡是否含可展示 IPv4（Hardware.Info IPAddressList）
    /// </summary>
    /// <param name="adapter">Hardware.Info 网卡对象</param>
    /// <returns>是否含有效 IPv4</returns>
    private static bool HasDisplayableIpv4(NetworkAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        return adapter.IPAddressList?.Any(ip => IsDisplayableIpv4(ip?.ToString())) == true;
    }

    /// <summary>
    /// 判断是否为可展示的 IPv4（非回环、非 0.0.0.0）
    /// </summary>
    /// <param name="ipAddress">IP 字符串</param>
    /// <returns>是否为有效 IPv4</returns>
    private static bool IsDisplayableIpv4(string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            return false;
        }
        var text = ipAddress.Trim();
        if (text is "0.0.0.0" or "127.0.0.1")
        {
            return false;
        }
        if (text.Contains(':', StringComparison.Ordinal))
        {
            return false;
        }
        var parts = text.Split('.');
        if (parts.Length != 4)
        {
            return false;
        }
        foreach (var part in parts)
        {
            if (!byte.TryParse(part, out _))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取所有硬件信息（汇总）
    /// </summary>
    /// <returns>服务器硬件信息汇总</returns>
    public static ServerHardwareInfo GetAllInfo()
    {
        EnsureInitialized();
        
        var firstComputerSystem = _hardwareInfo.ComputerSystemList.FirstOrDefault();
        var cpuList = GetCpuInfo(out var systemCpuUsagePercent);
        return new ServerHardwareInfo
        {
            // 硬件唯一标识
            HostSerialNumber = firstComputerSystem != null
                ? GetPropertyValueNullable<string>(firstComputerSystem, "SerialNumber") ?? string.Empty
                : string.Empty,
            DriveSerialNumber = _hardwareInfo.DriveList.FirstOrDefault()?.SerialNumber ?? string.Empty,
            MacAddress = _hardwareInfo.NetworkAdapterList.FirstOrDefault()?.MACAddress ?? string.Empty,
            CpuModel = _hardwareInfo.CpuList.FirstOrDefault()?.Name ?? string.Empty,
            CpuUsagePercent = systemCpuUsagePercent,
            // 基础信息
            OsArchitecture = RuntimeInformation.OSArchitecture.ToString(),
            OperatingSystem = GetOperatingSystem(),
            OperatingSystemLanguage = GetOperatingSystemLanguage(),
            Motherboard = GetMotherboardInfo(),
            Bios = GetBiosInfo(),
            CpuList = cpuList,
            GpuList = GetGpuInfo(),
            Memory = GetMemoryInfo(),
            MemoryModuleList = GetMemoryModuleList(),
            DriveList = GetDriveInfo(),
            NetworkAdapterList = GetNetworkAdapterInfo(),
            ComputerSystemList = GetComputerSystemList()
        };
    }

    /// <summary>
    /// 获取计算机系统信息（包含主机序列号）
    /// </summary>
    /// <returns>计算机系统信息列表</returns>
    public static List<ComputerSystemInfo> GetComputerSystemList()
    {
        EnsureInitialized();
        try
        {
            return _hardwareInfo.ComputerSystemList.Select(system => new ComputerSystemInfo
            {
                Name = system.Name ?? string.Empty,
                Manufacturer = GetPropertyValueNullable<string>(system, "Manufacturer") ?? string.Empty,
                Model = GetPropertyValueNullable<string>(system, "Model") ?? string.Empty,
                SerialNumber = GetPropertyValueNullable<string>(system, "SerialNumber") ?? string.Empty,
                SystemType = GetPropertyValueNullable<string>(system, "SystemType") ?? string.Empty,
                Uuid = GetPropertyValueNullable<string>(system, "UUID")
                    ?? GetPropertyValueNullable<string>(system, "Uuid")
                    ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取计算机系统信息失败");
            return new List<ComputerSystemInfo>();
        }
    }

    /// <summary>
    /// 获取主板信息（Win32_BaseBoard）及机器 UUID（Win32_ComputerSystemProduct）
    /// </summary>
    /// <returns>主板信息</returns>
    public static MotherboardInfo GetMotherboardInfo()
    {
        EnsureInitialized();
        try
        {
            var motherboard = _hardwareInfo.MotherboardList.FirstOrDefault();
            var systemProduct = _hardwareInfo.ComputerSystemList.FirstOrDefault();
            var uuid = GetPropertyValueNullable<string>(systemProduct, "UUID")
                ?? GetPropertyValueNullable<string>(systemProduct, "Uuid")
                ?? string.Empty;
            if (motherboard != null)
            {
                return new MotherboardInfo
                {
                    Manufacturer = motherboard.Manufacturer ?? string.Empty,
                    Product = motherboard.Product ?? string.Empty,
                    SerialNumber = motherboard.SerialNumber ?? string.Empty,
                    Version = GetPropertyValueNullable<string>(motherboard, "Version") ?? string.Empty,
                    Uuid = uuid
                };
            }
            return new MotherboardInfo
            {
                Uuid = uuid
            };
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取主板信息失败");
            return new MotherboardInfo();
        }
    }

    /// <summary>
    /// 获取BIOS信息
    /// </summary>
    /// <returns>BIOS信息</returns>
    public static BiosInfo GetBiosInfo()
    {
        EnsureInitialized();
        try
        {
            var bios = _hardwareInfo.BiosList.FirstOrDefault();
            if (bios != null)
            {
                return new BiosInfo
                {
                    Manufacturer = bios.Manufacturer ?? string.Empty,
                    Version = bios.Version ?? string.Empty,
                    ReleaseDate = bios.ReleaseDate?.ToString() ?? string.Empty,
                    SerialNumber = bios.SerialNumber ?? string.Empty
                };
            }
            
            return new BiosInfo();
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取BIOS信息失败");
            return new BiosInfo();
        }
    }

    /// <summary>
    /// 获取显卡信息
    /// </summary>
    /// <returns>显卡信息列表</returns>
    public static List<GpuInfo> GetGpuInfo()
    {
        EnsureInitialized();
        try
        {
            return _hardwareInfo.VideoControllerList.Select(gpu => new GpuInfo
            {
                Name = gpu.Name ?? "Unknown",
                Manufacturer = gpu.Manufacturer ?? string.Empty,
                AdapterRAM = GetPropertyValue<ulong>(gpu, "AdapterRAM") ?? 0UL,
                DriverVersion = gpu.DriverVersion ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "获取显卡信息失败");
            return new List<GpuInfo>();
        }
    }

    /// <summary>
    /// 将 Hardware.Info PercentProcessorTime 转为使用率（%）
    /// </summary>
    /// <param name="percentProcessorTime">PercentProcessorTime 原始值（0~100）</param>
    /// <returns>使用率百分比</returns>
    private static double NormalizeCpuUsagePercent(ulong percentProcessorTime)
    {
        return Math.Round(Math.Clamp((double)percentProcessorTime, 0, 100), 2);
    }

    /// <summary>
    /// 计算整机 CPU 平均使用率（%）
    /// </summary>
    /// <param name="cpuList">物理 CPU 列表</param>
    /// <returns>各 Socket 的 PercentProcessorTime 算术平均；多路服务器为多颗物理 CPU 的平均值</returns>
    private static double CalculateAggregateCpuUsagePercent(IReadOnlyList<CpuInfo> cpuList)
    {
        ArgumentNullException.ThrowIfNull(cpuList);
        return cpuList.Count > 0
            ? Math.Round(cpuList.Average(cpu => cpu.UsagePercent), 2)
            : 0;
    }

    /// <summary>
    /// 从 Hardware.Info OperatingSystem 解析名称与版本（分别对应 Win32_OperatingSystem Caption / Version）
    /// </summary>
    /// <param name="os">Hardware.Info OperatingSystem 实例</param>
    /// <returns>名称与版本；均经 Trim，可能为空串</returns>
    private static (string Name, string Version) ResolveOperatingSystemNameAndVersion(object os)
    {
        ArgumentNullException.ThrowIfNull(os);
        var name = GetPropertyValueNullable<string>(os, "Name") ?? string.Empty;
        var version = GetPropertyValueNullable<string>(os, "Version") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name))
        {
            name = GetPropertyValueNullable<string>(os, "Caption") ?? string.Empty;
        }
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(version))
        {
            return (name.Trim(), version.Trim());
        }
        if (TryParseOperatingSystemToString(os.ToString() ?? string.Empty, out var parsedName, out var parsedVersion))
        {
            return (parsedName, parsedVersion);
        }
        return (name.Trim(), version.Trim());
    }

    /// <summary>
    /// 解析 Hardware.Info OperatingSystem.ToString 格式（Name: … Version: …）
    /// </summary>
    /// <param name="raw">ToString 原始文本</param>
    /// <param name="name">解析出的名称</param>
    /// <param name="version">解析出的版本</param>
    /// <returns>至少解析出名称或版本之一时返回 true</returns>
    private static bool TryParseOperatingSystemToString(string raw, out string name, out string version)
    {
        name = string.Empty;
        version = string.Empty;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }
        const string namePrefix = "Name:";
        const string versionPrefix = "Version:";
        var nameIndex = raw.IndexOf(namePrefix, StringComparison.OrdinalIgnoreCase);
        var versionIndex = raw.IndexOf(versionPrefix, StringComparison.OrdinalIgnoreCase);
        if (nameIndex < 0)
        {
            return false;
        }
        var nameStart = nameIndex + namePrefix.Length;
        if (versionIndex > nameStart)
        {
            name = raw[nameStart..versionIndex].Trim();
            version = raw[(versionIndex + versionPrefix.Length)..].Trim();
            return !string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(version);
        }
        name = raw[nameStart..].Trim();
        return !string.IsNullOrWhiteSpace(name);
    }

    /// <summary>
    /// 使用反射获取属性值（安全访问，值类型）
    /// </summary>
    private static T? GetPropertyValue<T>(object? obj, string propertyName) where T : struct
    {
        if (obj == null)
        {
            return null;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        try
        {
            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "反射读取属性失败: {PropertyName}", propertyName);
        }
        return null;
    }

    /// <summary>
    /// 使用反射获取属性值（安全访问，引用类型和可空类型）
    /// </summary>
    private static T? GetPropertyValueNullable<T>(object? obj, string propertyName) where T : class
    {
        if (obj == null)
        {
            return null;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        try
        {
            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                var value = prop.GetValue(obj);
                return value as T;
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "反射读取属性失败: {PropertyName}", propertyName);
        }
        return null;
    }

    /// <summary>
    /// 使用反射获取属性值（安全访问，可空值类型）
    /// </summary>
    private static T? GetPropertyValueNullableStruct<T>(object? obj, string propertyName) where T : struct
    {
        if (obj == null)
        {
            return null;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        try
        {
            var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "反射读取属性失败: {PropertyName}", propertyName);
        }
        return null;
    }
}

/// <summary>
/// CPU 逻辑核心使用率（Hardware.Info CpuCoreList 项，对应任务管理器「逻辑处理器」）
/// </summary>
public class CpuCoreInfo
{
    /// <summary>
    /// 逻辑核心名称（如 0、1、2…；非物理核心编码）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 逻辑核心使用率（%），取自 PercentProcessorTime
    /// </summary>
    public double UsagePercent { get; set; }
}

/// <summary>
/// 物理 CPU（Socket）信息及下属逻辑核心使用率
/// </summary>
public class CpuInfo
{
    /// <summary>
    /// CPU 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 物理核心数（NumberOfCores；与逻辑核心数不同）
    /// </summary>
    public uint NumberOfCores { get; set; }

    /// <summary>
    /// 逻辑处理器数（含超线程；与 CpuCoreList 项数一致）
    /// </summary>
    public uint NumberOfLogicalProcessors { get; set; }

    /// <summary>
    /// 处理器 ID（CPU 唯一标识替代方案之一）
    /// </summary>
    public string ProcessorId { get; set; } = string.Empty;

    /// <summary>
    /// 插槽标识（SocketDesignation；平台支持时由 WMI 提供）
    /// </summary>
    public string SocketDesignation { get; set; } = string.Empty;

    /// <summary>
    /// 物理 CPU 总使用率（%），取自 PercentProcessorTime；禁止对 CoreList 求和替代
    /// </summary>
    public double UsagePercent { get; set; }

    /// <summary>
    /// 各逻辑核心使用率（CpuCoreList；无法区分物理核与超线程）
    /// </summary>
    public List<CpuCoreInfo> CoreList { get; set; } = new();
}

/// <summary>
/// 内存信息
/// </summary>
public class MemoryInfo
{
    /// <summary>
    /// 总物理内存（字节）
    /// </summary>
    public ulong TotalPhysicalMemory { get; set; }

    /// <summary>
    /// 可用物理内存（字节）
    /// </summary>
    public ulong AvailablePhysicalMemory { get; set; }

    /// <summary>
    /// 已用物理内存（字节）
    /// </summary>
    public ulong UsedPhysicalMemory { get; set; }

    /// <summary>
    /// 总虚拟内存（字节）
    /// </summary>
    public ulong TotalVirtualMemory { get; set; }

    /// <summary>
    /// 可用虚拟内存（字节）
    /// </summary>
    public ulong AvailableVirtualMemory { get; set; }

    /// <summary>
    /// 已用虚拟内存（字节）
    /// </summary>
    public ulong UsedVirtualMemory { get; set; }
}

/// <summary>
/// 物理内存条（DIMM）信息
/// </summary>
public class MemoryModuleInfo
{
    /// <summary>
    /// 插槽 / Bank 标识
    /// </summary>
    public string BankLabel { get; set; } = string.Empty;

    /// <summary>
    /// 容量（字节）
    /// </summary>
    public ulong Capacity { get; set; }

    /// <summary>
    /// 频率（MHz）
    /// </summary>
    public uint Speed { get; set; }

    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 部件号
    /// </summary>
    public string PartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;
}

/// <summary>
/// 磁盘信息
/// </summary>
public class DriveInfo
{
    /// <summary>
    /// 驱动器名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 驱动器类型
    /// </summary>
    public string DriveType { get; set; } = string.Empty;

    /// <summary>
    /// 卷标
    /// </summary>
    public string VolumeLabel { get; set; } = string.Empty;

    /// <summary>
    /// 文件系统
    /// </summary>
    public string FileSystem { get; set; } = string.Empty;

    /// <summary>
    /// 总容量（字节）
    /// </summary>
    public ulong TotalSize { get; set; }

    /// <summary>
    /// 可用空间（字节）
    /// </summary>
    public ulong FreeSpace { get; set; }

    /// <summary>
    /// 已用空间（字节）
    /// </summary>
    public ulong UsedSpace { get; set; }

    /// <summary>
    /// 磁盘序列号（硬盘唯一标识）
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 磁盘型号
    /// </summary>
    public string Model { get; set; } = string.Empty;
}

/// <summary>
/// 网络适配器信息
/// </summary>
public class NetworkAdapterInfo
{
    /// <summary>
    /// 适配器名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// MAC地址（网卡唯一标识）
    /// </summary>
    public string MACAddress { get; set; } = string.Empty;

    /// <summary>
    /// IPv4 地址（多个以逗号分隔）
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 速度（比特/秒，WMI Speed）
    /// </summary>
    public ulong Speed { get; set; }

    /// <summary>
    /// 联网状态：Down（链路断开）/ NoInternet（网卡可用无外网）/ DnsFault（有外网 DNS 失败）/ Online（已联网）
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// 主板信息（Hardware.Info Motherboard / Win32_BaseBoard）
/// </summary>
public class MotherboardInfo
{
    /// <summary>
    /// 主板制造商（Manufacturer）
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 主板型号 / ID（Product，Win32_BaseBoard.Product）
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// 主板序列号（SerialNumber）
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主板版本（Version）
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 机器 UUID（Win32_ComputerSystemProduct.UUID，SMBIOS 唯一标识）
    /// </summary>
    public string Uuid { get; set; } = string.Empty;
}

/// <summary>
/// BIOS信息
/// </summary>
public class BiosInfo
{
    /// <summary>
    /// BIOS制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// BIOS版本
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// BIOS发布日期
    /// </summary>
    public string ReleaseDate { get; set; } = string.Empty;

    /// <summary>
    /// BIOS序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;
}

/// <summary>
/// 显卡信息
/// </summary>
public class GpuInfo
{
    /// <summary>
    /// 显卡名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 显卡制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 显存大小（字节）
    /// </summary>
    public ulong AdapterRAM { get; set; }

    /// <summary>
    /// 显卡驱动版本
    /// </summary>
    public string DriverVersion { get; set; } = string.Empty;
}

/// <summary>
/// 操作系统语言信息
/// </summary>
public class OperatingSystemLanguageInfo
{
    /// <summary>
    /// 当前文化代码（如：zh-CN）
    /// </summary>
    public string CurrentCulture { get; set; } = string.Empty;

    /// <summary>
    /// 当前文化显示名称（如：中文(简体，中国)）
    /// </summary>
    public string CurrentCultureDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文化本地化名称（如：中文(简体，中国)）
    /// </summary>
    public string CurrentCultureNativeName { get; set; } = string.Empty;

    /// <summary>
    /// 当前UI文化代码（如：zh-CN）
    /// </summary>
    public string CurrentUICulture { get; set; } = string.Empty;

    /// <summary>
    /// 当前UI文化显示名称（如：中文(简体，中国)）
    /// </summary>
    public string CurrentUICultureDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 当前UI文化本地化名称（如：中文(简体，中国)）
    /// </summary>
    public string CurrentUICultureNativeName { get; set; } = string.Empty;

    /// <summary>
    /// 系统默认语言
    /// </summary>
    public string SystemDefaultLanguage { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统版本（GetOsVersion：平台分发 + OSDescription 兜底）
    /// </summary>
    public string OsVersion { get; set; } = string.Empty;

    /// <summary>
    /// 已安装的语言列表
    /// </summary>
    public List<InstalledLanguage> InstalledLanguages { get; set; } = new();
}

/// <summary>
/// 计算机系统信息（包含主机序列号）
/// </summary>
public class ComputerSystemInfo
{
    /// <summary>
    /// 计算机系统名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 系统序列号（主机唯一标识，来源于 SMBIOS）
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 系统类型
    /// </summary>
    public string SystemType { get; set; } = string.Empty;

    /// <summary>
    /// 机器 UUID（Win32_ComputerSystemProduct.UUID）
    /// </summary>
    public string Uuid { get; set; } = string.Empty;
}

/// <summary>
/// 已安装的语言信息
/// </summary>
public class InstalledLanguage
{
    /// <summary>
    /// 文化代码（如：zh-CN）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（如：中文(简体，中国)）
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化名称（如：中文(简体，中国)）
    /// </summary>
    public string NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名称（如：Chinese (Simplified, China)）
    /// </summary>
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 是否为中性文化（如：zh 而不是 zh-CN）
    /// </summary>
    public bool IsNeutralCulture { get; set; }

    /// <summary>
    /// 是否已安装 Win32 文化
    /// </summary>
    public bool IsInstalledWin32Culture { get; set; }
}

/// <summary>
/// 服务器硬件信息汇总
/// </summary>
public class ServerHardwareInfo
{
    /// <summary>
    /// 主机编码（系统序列号，来源于 SMBIOS）
    /// </summary>
    public string HostSerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 硬盘编码（第一个磁盘的序列号）
    /// </summary>
    public string DriveSerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// MAC地址（第一个网卡的MAC地址）
    /// </summary>
    public string MacAddress { get; set; } = string.Empty;

    /// <summary>
    /// CPU型号标识（用于区分CPU型号，非唯一序列号）
    /// </summary>
    public string CpuModel { get; set; } = string.Empty;

    /// <summary>
    /// CPU 平均使用率（%）
    /// </summary>
    public double CpuUsagePercent { get; set; }

    /// <summary>
    /// 操作系统架构（RuntimeInformation.OSArchitecture，如 X64、Arm64）
    /// </summary>
    public string OsArchitecture { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统信息
    /// </summary>
    public string OperatingSystem { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统语言信息
    /// </summary>
    public OperatingSystemLanguageInfo OperatingSystemLanguage { get; set; } = new();

    /// <summary>
    /// 主板信息
    /// </summary>
    public MotherboardInfo Motherboard { get; set; } = new();

    /// <summary>
    /// BIOS信息
    /// </summary>
    public BiosInfo Bios { get; set; } = new();

    /// <summary>
    /// CPU信息列表
    /// </summary>
    public List<CpuInfo> CpuList { get; set; } = new();

    /// <summary>
    /// 显卡信息列表
    /// </summary>
    public List<GpuInfo> GpuList { get; set; } = new();

    /// <summary>
    /// 内存汇总
    /// </summary>
    public MemoryInfo Memory { get; set; } = new();

    /// <summary>
    /// 物理内存条列表
    /// </summary>
    public List<MemoryModuleInfo> MemoryModuleList { get; set; } = new();

    /// <summary>
    /// 磁盘信息列表
    /// </summary>
    public List<DriveInfo> DriveList { get; set; } = new();

    /// <summary>
    /// 网络适配器信息列表
    /// </summary>
    public List<NetworkAdapterInfo> NetworkAdapterList { get; set; } = new();

    /// <summary>
    /// 计算机系统信息列表（包含主机序列号）
    /// </summary>
    public List<ComputerSystemInfo> ComputerSystemList { get; set; } = new();
}
