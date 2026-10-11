// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktServerMonitorService.cs
// 创建时间：2026-05-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务器监控应用服务，基于 TaktServHelper 提供硬件与应用状态查询
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 服务器监控应用服务
/// </summary>
public class TaktServerMonitorService : TaktServiceBase, ITaktServerMonitorService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="userContext">用户上下文（可选）</param>
    /// <param name="localizationService">本地化服务（可选）</param>
    public TaktServerMonitorService(
        IConfiguration configuration,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 获取服务器硬件信息
    /// </summary>
    /// <returns>服务器硬件信息 DTO</returns>
    public Task<TaktServerHardwareDto> GetServerHardwareAsync()
    {
        return Task.FromResult(MapHardwareDto(TaktServHelper.GetAllInfo()));
    }

    /// <summary>
    /// 获取应用运行状态
    /// </summary>
    /// <returns>应用运行状态 DTO</returns>
    public Task<TaktAppStatusDto> GetAppStatusAsync()
    {
        var loggingOptions = _configuration.GetSection(TaktLoggingOptions.SectionName).Get<TaktLoggingOptions>();
        var systemOptions = _configuration.GetSection(TaktSystemOptions.SectionName).Get<TaktSystemOptions>();
        var process = System.Diagnostics.Process.GetCurrentProcess();
        var startTime = process.StartTime;
        var dto = new TaktAppStatusDto
        {
            ApplicationName = loggingOptions?.AppName ?? "TaktWise",
            ApplicationVersion = loggingOptions?.AppVersion
                ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
                ?? "1.0.0.0",
            Environment = systemOptions?.Environment
                ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Production",
            MachineName = Environment.MachineName,
            StartTime = startTime,
            Uptime = DateTime.Now - startTime,
            DotNetVersion = Environment.Version.ToString(),
            WorkingSet = process.WorkingSet64,
            ProcessorCount = Environment.ProcessorCount,
            ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString()
        };
        return Task.FromResult(dto);
    }

    /// <summary>
    /// 刷新硬件信息缓存
    /// </summary>
    public void RefreshHardwareCache()
    {
        TaktServHelper.RefreshAll();
    }

    /// <summary>
    /// 将 ServerHardwareInfo 映射为 API DTO
    /// </summary>
    /// <param name="hardwareInfo">硬件汇总信息</param>
    /// <returns>服务器硬件信息 DTO</returns>
    private static TaktServerHardwareDto MapHardwareDto(ServerHardwareInfo hardwareInfo)
    {
        return new TaktServerHardwareDto
        {
            HostSerialNumber = hardwareInfo.HostSerialNumber,
            DriveSerialNumber = hardwareInfo.DriveSerialNumber,
            MacAddress = hardwareInfo.MacAddress,
            CpuModel = hardwareInfo.CpuModel,
            CpuUsagePercent = hardwareInfo.CpuUsagePercent,
            OsArchitecture = hardwareInfo.OsArchitecture,
            OperatingSystem = hardwareInfo.OperatingSystem,
            OperatingSystemLanguage = new TaktOperatingSystemLanguageDto
            {
                CurrentCulture = hardwareInfo.OperatingSystemLanguage.CurrentCulture,
                CurrentCultureDisplayName = hardwareInfo.OperatingSystemLanguage.CurrentCultureDisplayName,
                CurrentCultureNativeName = hardwareInfo.OperatingSystemLanguage.CurrentCultureNativeName,
                CurrentUICulture = hardwareInfo.OperatingSystemLanguage.CurrentUICulture,
                CurrentUICultureDisplayName = hardwareInfo.OperatingSystemLanguage.CurrentUICultureDisplayName,
                CurrentUICultureNativeName = hardwareInfo.OperatingSystemLanguage.CurrentUICultureNativeName,
                SystemDefaultLanguage = hardwareInfo.OperatingSystemLanguage.SystemDefaultLanguage,
                OsVersion = hardwareInfo.OperatingSystemLanguage.OsVersion,
                InstalledLanguages = hardwareInfo.OperatingSystemLanguage.InstalledLanguages.Select(lang => new TaktInstalledLanguageDto
                {
                    CultureCode = lang.CultureCode,
                    DisplayName = lang.DisplayName,
                    NativeName = lang.NativeName,
                    EnglishName = lang.EnglishName,
                    IsNeutralCulture = lang.IsNeutralCulture,
                    IsInstalledWin32Culture = lang.IsInstalledWin32Culture
                }).ToList()
            },
            Motherboard = new TaktMotherboardInfoDto
            {
                Manufacturer = hardwareInfo.Motherboard.Manufacturer,
                Product = hardwareInfo.Motherboard.Product,
                SerialNumber = hardwareInfo.Motherboard.SerialNumber,
                Version = hardwareInfo.Motherboard.Version,
                Uuid = hardwareInfo.Motherboard.Uuid
            },
            Bios = new TaktBiosInfoDto
            {
                Manufacturer = hardwareInfo.Bios.Manufacturer,
                Version = hardwareInfo.Bios.Version,
                ReleaseDate = hardwareInfo.Bios.ReleaseDate,
                SerialNumber = hardwareInfo.Bios.SerialNumber
            },
            CpuList = hardwareInfo.CpuList.Select(cpu => new TaktCpuInfoDto
            {
                Name = cpu.Name,
                Manufacturer = cpu.Manufacturer,
                NumberOfCores = cpu.NumberOfCores,
                NumberOfLogicalProcessors = cpu.NumberOfLogicalProcessors,
                ProcessorId = cpu.ProcessorId,
                SocketDesignation = cpu.SocketDesignation,
                UsagePercent = cpu.UsagePercent,
                CoreList = cpu.CoreList.Select(core => new TaktCpuCoreInfoDto
                {
                    Name = core.Name,
                    UsagePercent = core.UsagePercent
                }).ToList()
            }).ToList(),
            GpuList = hardwareInfo.GpuList.Select(gpu => new TaktGpuInfoDto
            {
                Name = gpu.Name,
                Manufacturer = gpu.Manufacturer,
                AdapterRam = gpu.AdapterRAM,
                DriverVersion = gpu.DriverVersion
            }).ToList(),
            Memory = new TaktMemoryInfoDto
            {
                TotalPhysicalMemory = hardwareInfo.Memory.TotalPhysicalMemory,
                AvailablePhysicalMemory = hardwareInfo.Memory.AvailablePhysicalMemory,
                UsedPhysicalMemory = hardwareInfo.Memory.UsedPhysicalMemory,
                TotalVirtualMemory = hardwareInfo.Memory.TotalVirtualMemory,
                AvailableVirtualMemory = hardwareInfo.Memory.AvailableVirtualMemory,
                UsedVirtualMemory = hardwareInfo.Memory.UsedVirtualMemory
            },
            MemoryModuleList = hardwareInfo.MemoryModuleList.Select(module => new TaktMemoryModuleDto
            {
                BankLabel = module.BankLabel,
                Capacity = module.Capacity,
                Speed = module.Speed,
                Manufacturer = module.Manufacturer,
                PartNumber = module.PartNumber,
                SerialNumber = module.SerialNumber
            }).ToList(),
            DriveList = hardwareInfo.DriveList.Select(drive => new TaktDriveInfoDto
            {
                Name = drive.Name,
                DriveType = drive.DriveType,
                VolumeLabel = drive.VolumeLabel,
                FileSystem = drive.FileSystem,
                TotalSize = drive.TotalSize,
                FreeSpace = drive.FreeSpace,
                UsedSpace = drive.UsedSpace,
                SerialNumber = drive.SerialNumber,
                Model = drive.Model
            }).ToList(),
            NetworkAdapterList = hardwareInfo.NetworkAdapterList.Select(adapter => new TaktNetworkAdapterDto
            {
                Name = adapter.Name,
                Description = adapter.Description,
                MACAddress = adapter.MACAddress,
                IpAddress = adapter.IpAddress,
                Speed = adapter.Speed,
                Status = adapter.Status
            }).ToList(),
            ComputerSystemList = hardwareInfo.ComputerSystemList.Select(system => new TaktComputerSystemInfoDto
            {
                Name = system.Name,
                Manufacturer = system.Manufacturer,
                Model = system.Model,
                SerialNumber = system.SerialNumber,
                SystemType = system.SystemType,
                Uuid = system.Uuid
            }).ToList()
        };
    }
}
