// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：serverMonitor.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 服务器硬件信息 DTO
 * 对应前端 ServerHardware
 * @description 对应后端 TaktServerHardwareDto
 */
export interface ServerHardware {
  /**
   * 主机编码（系统序列号）
   */
  hostSerialNumber: string;

  /**
   * 硬盘编码（首个磁盘序列号）
   */
  driveSerialNumber: string;

  /**
   * MAC 地址（首个网卡）
   */
  macAddress: string;

  /**
   * CPU 型号标识
   */
  cpuModel: string;

  /**
   * CPU 平均使用率（%）
   */
  cpuUsagePercent: number;

  /**
   * 操作系统架构（RuntimeInformation.OSArchitecture，如 X64、Arm64）
   */
  osArchitecture: string;

  /**
   * 操作系统信息
   */
  operatingSystem: string;

  /**
   * 操作系统语言信息
   */
  operatingSystemLanguage: OperatingSystemLanguage;

  /**
   * 主板信息
   */
  motherboard: MotherboardInfo;

  /**
   * BIOS 信息
   */
  bios: BiosInfo;

  /**
   * CPU 信息列表
   */
  cpuList: CpuInfo[];

  /**
   * 显卡信息列表
   */
  gpuList: GpuInfo[];

  /**
   * 内存汇总
   */
  memory: MemoryInfo;

  /**
   * 物理内存条列表
   */
  memoryModuleList: MemoryModule[];

  /**
   * 磁盘信息列表
   */
  driveList: DriveInfo[];

  /**
   * 网络适配器信息列表
   */
  networkAdapterList: NetworkAdapter[];

  /**
   * 计算机系统信息列表
   */
  computerSystemList: ComputerSystemInfo[];

}


/**
 * 操作系统语言信息 DTO
 * 对应前端 OperatingSystemLanguage
 * @description 对应后端 TaktOperatingSystemLanguageDto
 */
export interface OperatingSystemLanguage {
  /**
   * 当前文化代码（如：zh-CN）
   */
  currentCulture: string;

  /**
   * 当前文化显示名称
   */
  currentCultureDisplayName: string;

  /**
   * 当前文化本地化名称
   */
  currentCultureNativeName: string;

  /**
   * 当前 UI 文化代码
   */
  currentUICulture: string;

  /**
   * 当前 UI 文化显示名称
   */
  currentUICultureDisplayName: string;

  /**
   * 当前 UI 文化本地化名称
   */
  currentUICultureNativeName: string;

  /**
   * 系统默认语言
   */
  systemDefaultLanguage: string;

  /**
   * 操作系统版本（GetOsVersion：平台分发 + OSDescription 兜底）
   */
  osVersion: string;

  /**
   * 已安装的语言列表
   */
  installedLanguages: InstalledLanguage[];

}


/**
 * 已安装的语言信息 DTO
 * 对应前端 InstalledLanguage
 * @description 对应后端 TaktInstalledLanguageDto
 */
export interface InstalledLanguage {
  /**
   * 文化代码（如：zh-CN）
   */
  cultureCode: string;

  /**
   * 显示名称
   */
  displayName: string;

  /**
   * 本地化名称
   */
  nativeName: string;

  /**
   * 英文名称
   */
  englishName: string;

  /**
   * 是否为中性文化
   */
  isNeutralCulture: boolean;

  /**
   * 是否已安装 Win32 文化
   */
  isInstalledWin32Culture: boolean;

}


/**
 * CPU 信息 DTO
 * 对应前端 CpuInfo
 * @description 对应后端 TaktCpuInfoDto
 */
export interface CpuInfo {
  /**
   * CPU 名称
   */
  name: string;

  /**
   * 制造商
   */
  manufacturer: string;

  /**
   * 核心数
   */
  numberOfCores: any;

  /**
   * 逻辑处理器数
   */
  numberOfLogicalProcessors: any;

  /**
   * 处理器 ID
   */
  processorId: string;

  /**
   * 插槽标识（SocketDesignation）
   */
  socketDesignation: string;

  /**
   * 物理 CPU 总使用率（%）
   */
  usagePercent: number;

  /**
   * 各逻辑核心使用率（CpuCoreList）
   */
  coreList: CpuCoreInfo[];

}


/**
 * CPU 核心使用率 DTO
 */
export interface CpuCoreInfo {
  /**
   * 逻辑核心名称
   */
  name: string;

  /**
   * 逻辑核心使用率（%）
   */
  usagePercent: number;

}


/**
 * 显卡信息 DTO
 * 对应前端 GpuInfo
 * @description 对应后端 TaktGpuInfoDto
 */
export interface GpuInfo {
  /**
   * 显卡名称
   */
  name: string;

  /**
   * 显卡制造商
   */
  manufacturer: string;

  /**
   * 显存大小（字节）
   */
  adapterRam: any;

  /**
   * 驱动版本
   */
  driverVersion: string;

}


/**
 * 内存信息 DTO
 * 对应前端 MemoryInfo
 * @description 对应后端 TaktMemoryInfoDto
 */
export interface MemoryInfo {
  /**
   * 总物理内存（字节）
   */
  totalPhysicalMemory: any;

  /**
   * 可用物理内存（字节）
   */
  availablePhysicalMemory: any;

  /**
   * 已用物理内存（字节）
   */
  usedPhysicalMemory: any;

  /**
   * 总虚拟内存（字节）
   */
  totalVirtualMemory: any;

  /**
   * 可用虚拟内存（字节）
   */
  availableVirtualMemory: any;

  /**
   * 已用虚拟内存（字节）
   */
  usedVirtualMemory: any;

}


/**
 * 物理内存条 DTO
 */
export interface MemoryModule {
  /**
   * 插槽 / Bank 标识
   */
  bankLabel: string;

  /**
   * 容量（字节）
   */
  capacity: any;

  /**
   * 频率（MHz）
   */
  speed: any;

  /**
   * 制造商
   */
  manufacturer: string;

  /**
   * 部件号
   */
  partNumber: string;

  /**
   * 序列号
   */
  serialNumber: string;

}


/**
 * 磁盘信息 DTO
 * 对应前端 DriveInfo
 * @description 对应后端 TaktDriveInfoDto
 */
export interface DriveInfo {
  /**
   * 驱动器名称
   */
  name: string;

  /**
   * 驱动器类型
   */
  driveType: string;

  /**
   * 卷标
   */
  volumeLabel: string;

  /**
   * 文件系统
   */
  fileSystem: string;

  /**
   * 总容量（字节）
   */
  totalSize: any;

  /**
   * 可用空间（字节）
   */
  freeSpace: any;

  /**
   * 已用空间（字节）
   */
  usedSpace: any;

  /**
   * 磁盘序列号
   */
  serialNumber: string;

  /**
   * 磁盘型号
   */
  model: string;

}


/**
 * 网络适配器信息 DTO
 * 对应前端 NetworkAdapter
 * @description 对应后端 TaktNetworkAdapterDto
 */
export interface NetworkAdapter {
  /**
   * 适配器名称
   */
  name: string;

  /**
   * 描述
   */
  description: string;

  /**
   * MAC 地址
   */
  macAddress: string;

  /**
   * IPv4 地址（多个以逗号分隔）
   */
  ipAddress: string;

  /**
   * 速度（比特/秒）
   */
  speed: any;

  /**
   * 联网状态：Down / NoInternet / DnsFault / Online
   */
  status: string;

}


/**
 * 主板信息 DTO
 * 对应前端 MotherboardInfo
 * @description 对应后端 TaktMotherboardInfoDto
 */
export interface MotherboardInfo {
  /**
   * 制造商
   */
  manufacturer: string;

  /**
   * 型号
   */
  product: string;

  /**
   * 序列号
   */
  serialNumber: string;

  /**
   * 版本
   */
  version: string;

  /**
   * 机器 UUID（SMBIOS）
   */
  uuid: string;

}


/**
 * BIOS 信息 DTO
 * 对应前端 BiosInfo
 * @description 对应后端 TaktBiosInfoDto
 */
export interface BiosInfo {
  /**
   * 制造商
   */
  manufacturer: string;

  /**
   * 版本
   */
  version: string;

  /**
   * 发布日期
   */
  releaseDate: string;

  /**
   * 序列号
   */
  serialNumber: string;

}


/**
 * 计算机系统信息 DTO
 * 对应前端 ComputerSystemInfo
 * @description 对应后端 TaktComputerSystemInfoDto
 */
export interface ComputerSystemInfo {
  /**
   * 系统名称
   */
  name: string;

  /**
   * 制造商
   */
  manufacturer: string;

  /**
   * 型号
   */
  model: string;

  /**
   * 序列号
   */
  serialNumber: string;

  /**
   * 系统类型
   */
  systemType: string;

}


/**
 * 应用运行状态 DTO
 * 对应前端 AppStatus
 * @description 对应后端 TaktAppStatusDto
 */
export interface AppStatus {
  /**
   * 应用名称
   */
  applicationName: string;

  /**
   * 应用版本
   */
  applicationVersion: string;

  /**
   * 运行环境
   */
  environment: string;

  /**
   * 机器名称
   */
  machineName: string;

  /**
   * 启动时间
   */
  startTime: string;

  /**
   * 运行时长
   */
  uptime: any;

  /**
   * .NET 版本
   */
  dotNetVersion: string;

  /**
   * 工作集内存（字节）
   */
  workingSet: string;

  /**
   * 处理器数量
   */
  processorCount: number;

  /**
   * 进程架构（RuntimeInformation.ProcessArchitecture，如 X64、Arm64）
   */
  processArchitecture: string;

}

