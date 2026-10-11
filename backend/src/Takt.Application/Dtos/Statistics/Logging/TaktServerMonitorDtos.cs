// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktServerMonitorDtos.cs
// 创建时间：2026-05-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务器监控相关 DTO 定义
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Statistics.Logging;

/// <summary>
/// 服务器硬件信息 DTO
/// </summary>
public class TaktServerHardwareDto
{
    /// <summary>
    /// 主机编码（系统序列号）
    /// </summary>
    public string HostSerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 硬盘编码（首个磁盘序列号）
    /// </summary>
    public string DriveSerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// MAC 地址（首个网卡）
    /// </summary>
    public string MacAddress { get; set; } = string.Empty;

    /// <summary>
    /// CPU 型号标识
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
    public TaktOperatingSystemLanguageDto OperatingSystemLanguage { get; set; } = new();

    /// <summary>
    /// 主板信息
    /// </summary>
    public TaktMotherboardInfoDto Motherboard { get; set; } = new();

    /// <summary>
    /// BIOS 信息
    /// </summary>
    public TaktBiosInfoDto Bios { get; set; } = new();

    /// <summary>
    /// CPU 信息列表
    /// </summary>
    public List<TaktCpuInfoDto> CpuList { get; set; } = new();

    /// <summary>
    /// 显卡信息列表
    /// </summary>
    public List<TaktGpuInfoDto> GpuList { get; set; } = new();

    /// <summary>
    /// 内存信息
    /// </summary>
    public List<TaktMemoryModuleDto> MemoryModuleList { get; set; } = new();

    /// <summary>
    /// 内存汇总
    /// </summary>
    public TaktMemoryInfoDto Memory { get; set; } = new();

    /// <summary>
    /// 磁盘信息列表
    /// </summary>
    public List<TaktDriveInfoDto> DriveList { get; set; } = new();

    /// <summary>
    /// 网络适配器信息列表
    /// </summary>
    public List<TaktNetworkAdapterDto> NetworkAdapterList { get; set; } = new();

    /// <summary>
    /// 计算机系统信息列表
    /// </summary>
    public List<TaktComputerSystemInfoDto> ComputerSystemList { get; set; } = new();
}

/// <summary>
/// 操作系统语言信息 DTO
/// </summary>
public class TaktOperatingSystemLanguageDto
{
    /// <summary>
    /// 当前文化代码（如：zh-CN）
    /// </summary>
    public string CurrentCulture { get; set; } = string.Empty;

    /// <summary>
    /// 当前文化显示名称
    /// </summary>
    public string CurrentCultureDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文化本地化名称
    /// </summary>
    public string CurrentCultureNativeName { get; set; } = string.Empty;

    /// <summary>
    /// 当前 UI 文化代码
    /// </summary>
    public string CurrentUICulture { get; set; } = string.Empty;

    /// <summary>
    /// 当前 UI 文化显示名称
    /// </summary>
    public string CurrentUICultureDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 当前 UI 文化本地化名称
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
    public List<TaktInstalledLanguageDto> InstalledLanguages { get; set; } = new();
}

/// <summary>
/// 已安装的语言信息 DTO
/// </summary>
public class TaktInstalledLanguageDto
{
    /// <summary>
    /// 文化代码（如：zh-CN）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化名称
    /// </summary>
    public string NativeName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名称
    /// </summary>
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 是否为中性文化
    /// </summary>
    public bool IsNeutralCulture { get; set; }

    /// <summary>
    /// 是否已安装 Win32 文化
    /// </summary>
    public bool IsInstalledWin32Culture { get; set; }
}

/// <summary>
/// CPU 信息 DTO
/// </summary>
public class TaktCpuInfoDto
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
    /// 核心数
    /// </summary>
    public uint NumberOfCores { get; set; }

    /// <summary>
    /// 逻辑处理器数
    /// </summary>
    public uint NumberOfLogicalProcessors { get; set; }

    /// <summary>
    /// 处理器 ID
    /// </summary>
    public string ProcessorId { get; set; } = string.Empty;

    /// <summary>
    /// 插槽标识（SocketDesignation）
    /// </summary>
    public string SocketDesignation { get; set; } = string.Empty;

    /// <summary>
    /// 物理 CPU 总使用率（%），PercentProcessorTime
    /// </summary>
    public double UsagePercent { get; set; }

    /// <summary>
    /// 各逻辑核心使用率（CpuCoreList；逻辑处理器，非物理核心）
    /// </summary>
    public List<TaktCpuCoreInfoDto> CoreList { get; set; } = new();
}

/// <summary>
/// CPU 核心使用率 DTO
/// </summary>
public class TaktCpuCoreInfoDto
{
    /// <summary>
    /// 逻辑核心名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 逻辑核心使用率（%）
    /// </summary>
    public double UsagePercent { get; set; }
}

/// <summary>
/// 显卡信息 DTO
/// </summary>
public class TaktGpuInfoDto
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
    public ulong AdapterRam { get; set; }

    /// <summary>
    /// 驱动版本
    /// </summary>
    public string DriverVersion { get; set; } = string.Empty;
}

/// <summary>
/// 内存信息 DTO
/// </summary>
public class TaktMemoryInfoDto
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

    /// <summary>
    /// 内存使用率（%）
    /// </summary>
    public double MemoryUsagePercent => TotalPhysicalMemory > 0
        ? Math.Round((double)UsedPhysicalMemory / TotalPhysicalMemory * 100, 2)
        : 0;
}

/// <summary>
/// 物理内存条（DIMM）信息 DTO
/// </summary>
public class TaktMemoryModuleDto
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
/// 磁盘信息 DTO
/// </summary>
public class TaktDriveInfoDto
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
    /// 磁盘序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 磁盘型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 磁盘使用率（%）
    /// </summary>
    public double UsagePercent => TotalSize > 0
        ? Math.Round((double)UsedSpace / TotalSize * 100, 2)
        : 0;
}

/// <summary>
/// 网络适配器信息 DTO
/// </summary>
public class TaktNetworkAdapterDto
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
    /// MAC 地址
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
    /// 联网状态：Down / NoInternet / DnsFault / Online
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// 主板信息 DTO
/// </summary>
public class TaktMotherboardInfoDto
{
    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 型号（Product，主板型号/ID）
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 机器 UUID（SMBIOS）
    /// </summary>
    public string Uuid { get; set; } = string.Empty;
}

/// <summary>
/// BIOS 信息 DTO
/// </summary>
public class TaktBiosInfoDto
{
    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// 发布日期
    /// </summary>
    public string ReleaseDate { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;
}

/// <summary>
/// 计算机系统信息 DTO
/// </summary>
public class TaktComputerSystemInfoDto
{
    /// <summary>
    /// 系统名称
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
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 系统类型
    /// </summary>
    public string SystemType { get; set; } = string.Empty;

    /// <summary>
    /// 机器 UUID
    /// </summary>
    public string Uuid { get; set; } = string.Empty;
}

/// <summary>
/// 应用运行状态 DTO
/// </summary>
public class TaktAppStatusDto
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>
    /// 应用版本
    /// </summary>
    public string ApplicationVersion { get; set; } = string.Empty;

    /// <summary>
    /// 运行环境
    /// </summary>
    public string Environment { get; set; } = string.Empty;

    /// <summary>
    /// 机器名称
    /// </summary>
    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// 启动时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 运行时长
    /// </summary>
    public TimeSpan Uptime { get; set; }

    /// <summary>
    /// .NET 版本
    /// </summary>
    public string DotNetVersion { get; set; } = string.Empty;

    /// <summary>
    /// 工作集内存（字节）
    /// </summary>
    public long WorkingSet { get; set; }

    /// <summary>
    /// 处理器数量
    /// </summary>
    public int ProcessorCount { get; set; }

    /// <summary>
    /// 进程架构（RuntimeInformation.ProcessArchitecture，如 X64、Arm64）
    /// </summary>
    public string ProcessArchitecture { get; set; } = string.Empty;
}
