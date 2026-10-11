// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionEquipmentDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionEquipment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionEquipment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mps;

// ========================================
// ProductionEquipment 响应 DTO
// ========================================

/// <summary>
/// 生产设备主数据（排程资源；粗能力 StdEquipHourlyCapacity=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate；多穴=(60÷StdMinutesPerCycle)×CavityCount×AvailabilityRate）
/// 对应前端 TaktProductionEquipmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionEquipmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionEquipmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionEquipmentId { get; set; }


    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    public int EquipCategory { get; set; } = 0;

    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    public string ProdEquipName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    public string MachineType { get; set; } = string.Empty;

    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出厂日期（Manufacturing Date）
    /// </summary>
    public DateTime? ManufacturingDate { get; set; }

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    public decimal StdCycleTimeSeconds { get; set; }

    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    public decimal StdMinutesPerUnit { get; set; }

    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    public decimal StdMinutesPerCycle { get; set; }

    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    public decimal TheoreticalSpm { get; set; }

    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    public decimal TheoreticalCycleTimeSeconds { get; set; }

    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    public decimal StdEquipHourlyCapacity { get; set; }

    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    public decimal AvailabilityRate { get; set; }

    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    public decimal PerformanceRate { get; set; }

    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    public decimal SetupMinutes { get; set; }

    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    public decimal MoldChangeMinutes { get; set; }

    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    public decimal MaterialChangeMinutes { get; set; }

    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    public decimal MtbfHours { get; set; }

    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    public decimal MttrHours { get; set; }

    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    public decimal? RepeatabilityAccuracy { get; set; }

    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    public decimal? ShutHeightAccuracy { get; set; }

    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    public decimal? InjectionAccuracy { get; set; }

    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    public decimal? TemperatureControlAccuracy { get; set; }

    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    public decimal? PressureControlAccuracy { get; set; }

    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    public decimal? ProcessCapabilityCpk { get; set; }

    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    public decimal? MaxDimensionalTolerance { get; set; }

    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    public string? MaxMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    public string? MinMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    public decimal? MaxMoldWeightTon { get; set; }

    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    public string? MoldHeightRange { get; set; } = string.Empty;

    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    public int? EjectionType { get; set; }

    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    public decimal? EjectionStrokeMm { get; set; }

    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    public int CavityCount { get; set; } = 0;

    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    public int QuickMoldChange { get; set; } = 0;

    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    public string? MoldCode { get; set; } = string.Empty;

    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    public decimal? RatedTonnage { get; set; }

    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    public decimal? ClampingForceKn { get; set; }

    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    public decimal? MaxStrokeMm { get; set; }

    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    public decimal? OpenStrokeMm { get; set; }

    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    public string? PlatenSize { get; set; } = string.Empty;

    /// <summary>
    /// 使用电压（V）
    /// </summary>
    public decimal? RatedVoltage { get; set; }

    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    public decimal? RatedPowerKw { get; set; }

    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    public decimal? AirConsumptionLpm { get; set; }

    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    public decimal? CoolingWaterFlowLpm { get; set; }

    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    public int OperatorCount { get; set; } = 0;

    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    public int IsCriticalResource { get; set; } = 0;

    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    public int ParallelCapacity { get; set; } = 0;

    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    public int AllowRushOrder { get; set; } = 0;

    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    public decimal WarmupMinutes { get; set; }

    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    public string? OperatingTempRange { get; set; } = string.Empty;

    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    public string? OperatingHumidityRange { get; set; } = string.Empty;

    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    public decimal? NoiseLevelDb { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    public int EquipmentRunStatus { get; set; } = 0;

    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    public decimal MaintenanceIntervalHours { get; set; }

    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    public decimal CumulativeRunHours { get; set; }

    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    public string? InterfaceType { get; set; } = string.Empty;

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）
    /// </summary>
    public DateTime? CommissioningDate { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）
    /// </summary>
    public DateTime? DecommissioningDate { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipAdministratorName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int ProdEquipStatus { get; set; } = 0;

}

// ========================================
// ProductionEquipment 查询 DTO
// ========================================

/// <summary>
/// ProductionEquipment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionEquipmentQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    public int? EquipCategory { get; set; }

    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    public string? ProdEquipName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    public string? MachineType { get; set; } = string.Empty;

    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出厂日期（Manufacturing Date）（范围查询-开始）
    /// </summary>
    public DateTime? ManufacturingDateStart { get; set; }

    /// <summary>
    /// 出厂日期（Manufacturing Date）（范围查询-结束）
    /// </summary>
    public DateTime? ManufacturingDateEnd { get; set; }

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    public decimal? StdCycleTimeSeconds { get; set; }

    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    public decimal? StdMinutesPerUnit { get; set; }

    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    public decimal? StdMinutesPerCycle { get; set; }

    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    public decimal? TheoreticalSpm { get; set; }

    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    public decimal? TheoreticalCycleTimeSeconds { get; set; }

    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    public decimal? StdEquipHourlyCapacity { get; set; }

    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    public decimal? AvailabilityRate { get; set; }

    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    public decimal? PerformanceRate { get; set; }

    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    public decimal? SetupMinutes { get; set; }

    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    public decimal? MoldChangeMinutes { get; set; }

    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    public decimal? MaterialChangeMinutes { get; set; }

    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    public decimal? MtbfHours { get; set; }

    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    public decimal? MttrHours { get; set; }

    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    public decimal? RepeatabilityAccuracy { get; set; }

    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    public decimal? ShutHeightAccuracy { get; set; }

    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    public decimal? InjectionAccuracy { get; set; }

    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    public decimal? TemperatureControlAccuracy { get; set; }

    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    public decimal? PressureControlAccuracy { get; set; }

    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    public decimal? ProcessCapabilityCpk { get; set; }

    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    public decimal? MaxDimensionalTolerance { get; set; }

    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    public string? MaxMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    public string? MinMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    public decimal? MaxMoldWeightTon { get; set; }

    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    public string? MoldHeightRange { get; set; } = string.Empty;

    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    public int? EjectionType { get; set; }

    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    public decimal? EjectionStrokeMm { get; set; }

    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    public int? CavityCount { get; set; }

    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    public int? QuickMoldChange { get; set; }

    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    public string? MoldCode { get; set; } = string.Empty;

    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    public decimal? RatedTonnage { get; set; }

    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    public decimal? ClampingForceKn { get; set; }

    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    public decimal? MaxStrokeMm { get; set; }

    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    public decimal? OpenStrokeMm { get; set; }

    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    public string? PlatenSize { get; set; } = string.Empty;

    /// <summary>
    /// 使用电压（V）
    /// </summary>
    public decimal? RatedVoltage { get; set; }

    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    public decimal? RatedPowerKw { get; set; }

    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    public decimal? AirConsumptionLpm { get; set; }

    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    public decimal? CoolingWaterFlowLpm { get; set; }

    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    public int? OperatorCount { get; set; }

    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    public int? IsCriticalResource { get; set; }

    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    public int? ParallelCapacity { get; set; }

    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    public int? AllowRushOrder { get; set; }

    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    public decimal? WarmupMinutes { get; set; }

    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    public string? OperatingTempRange { get; set; } = string.Empty;

    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    public string? OperatingHumidityRange { get; set; } = string.Empty;

    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    public decimal? NoiseLevelDb { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    public int? EquipmentRunStatus { get; set; }

    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    public decimal? MaintenanceIntervalHours { get; set; }

    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    public decimal? CumulativeRunHours { get; set; }

    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    public string? InterfaceType { get; set; } = string.Empty;

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）（范围查询-开始）
    /// </summary>
    public DateTime? CommissioningDateStart { get; set; }

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）（范围查询-结束）
    /// </summary>
    public DateTime? CommissioningDateEnd { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）（范围查询-开始）
    /// </summary>
    public DateTime? DecommissioningDateStart { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）（范围查询-结束）
    /// </summary>
    public DateTime? DecommissioningDateEnd { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）（范围查询-开始）
    /// </summary>
    public DateTime? ScrapDateStart { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）（范围查询-结束）
    /// </summary>
    public DateTime? ScrapDateEnd { get; set; }

    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipAdministratorName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? ProdEquipStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ProductionEquipment DTO
// ========================================

/// <summary>
/// 创建ProductionEquipment DTO
/// </summary>
public class TaktProductionEquipmentCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    public int EquipCategory { get; set; } = 0;

    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    [Required(ErrorMessage = "生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）不能为空")]
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    [Required(ErrorMessage = "生产设备名称（列表展示名）不能为空")]
    public string ProdEquipName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    [Required(ErrorMessage = "机型名称（铭牌 Machine Type，如 SP18P-L）不能为空")]
    public string MachineType { get; set; } = string.Empty;

    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出厂日期（Manufacturing Date）
    /// </summary>
    public DateTime? ManufacturingDate { get; set; }

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    public decimal StdCycleTimeSeconds { get; set; }

    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    public decimal StdMinutesPerUnit { get; set; }

    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    public decimal StdMinutesPerCycle { get; set; }

    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    public decimal TheoreticalSpm { get; set; }

    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    public decimal TheoreticalCycleTimeSeconds { get; set; }

    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    public decimal StdEquipHourlyCapacity { get; set; }

    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    public decimal AvailabilityRate { get; set; }

    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    public decimal PerformanceRate { get; set; }

    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    public decimal SetupMinutes { get; set; }

    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    public decimal MoldChangeMinutes { get; set; }

    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    public decimal MaterialChangeMinutes { get; set; }

    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    public decimal MtbfHours { get; set; }

    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    public decimal MttrHours { get; set; }

    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    public decimal? RepeatabilityAccuracy { get; set; }

    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    public decimal? ShutHeightAccuracy { get; set; }

    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    public decimal? InjectionAccuracy { get; set; }

    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    public decimal? TemperatureControlAccuracy { get; set; }

    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    public decimal? PressureControlAccuracy { get; set; }

    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    public decimal? ProcessCapabilityCpk { get; set; }

    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    public decimal? MaxDimensionalTolerance { get; set; }

    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    public string? MaxMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    public string? MinMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    public decimal? MaxMoldWeightTon { get; set; }

    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    public string? MoldHeightRange { get; set; } = string.Empty;

    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    public int? EjectionType { get; set; }

    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    public decimal? EjectionStrokeMm { get; set; }

    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    public int CavityCount { get; set; } = 0;

    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    public int QuickMoldChange { get; set; } = 0;

    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    public string? MoldCode { get; set; } = string.Empty;

    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    public decimal? RatedTonnage { get; set; }

    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    public decimal? ClampingForceKn { get; set; }

    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    public decimal? MaxStrokeMm { get; set; }

    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    public decimal? OpenStrokeMm { get; set; }

    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    public string? PlatenSize { get; set; } = string.Empty;

    /// <summary>
    /// 使用电压（V）
    /// </summary>
    public decimal? RatedVoltage { get; set; }

    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    public decimal? RatedPowerKw { get; set; }

    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    public decimal? AirConsumptionLpm { get; set; }

    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    public decimal? CoolingWaterFlowLpm { get; set; }

    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    public int OperatorCount { get; set; } = 0;

    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    public int IsCriticalResource { get; set; } = 0;

    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    public int ParallelCapacity { get; set; } = 0;

    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    public int AllowRushOrder { get; set; } = 0;

    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    public decimal WarmupMinutes { get; set; }

    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    public string? OperatingTempRange { get; set; } = string.Empty;

    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    public string? OperatingHumidityRange { get; set; } = string.Empty;

    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    public decimal? NoiseLevelDb { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    public int EquipmentRunStatus { get; set; } = 0;

    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    public decimal MaintenanceIntervalHours { get; set; }

    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    public decimal CumulativeRunHours { get; set; }

    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    public string? InterfaceType { get; set; } = string.Empty;

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）
    /// </summary>
    public DateTime? CommissioningDate { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）
    /// </summary>
    public DateTime? DecommissioningDate { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    [Required(ErrorMessage = "存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）不能为空")]
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipAdministratorName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int ProdEquipStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新ProductionEquipment DTO
// ========================================

/// <summary>
/// 更新ProductionEquipment DTO
/// 继承 TaktProductionEquipmentCreateDto，添加 ProductionEquipmentId 字段
/// </summary>
public class TaktProductionEquipmentUpdateDto : TaktProductionEquipmentCreateDto
{
    /// <summary>
    /// ProductionEquipmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionEquipmentId { get; set; }

}

// ========================================
// ProductionEquipment 状态 DTO
// ========================================

/// <summary>
/// ProductionEquipment 状态更新 DTO
/// </summary>
public class TaktProductionEquipmentStatusDto
{
    /// <summary>
    /// ProductionEquipmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionEquipmentId { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    [Required(ErrorMessage = "设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）不能为空")]
    public int EquipmentRunStatus { get; set; } = 0;
}

// ========================================
// ProductionEquipment 排序 DTO
// ========================================

/// <summary>
/// ProductionEquipment 排序更新 DTO
/// </summary>
public class TaktProductionEquipmentSortDto
{
    /// <summary>
    /// ProductionEquipmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionEquipmentId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionEquipment 导入模板行 DTO
/// </summary>
public class TaktProductionEquipmentTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    public int? EquipCategory { get; set; }

    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    public string? ProdEquipName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    public string? MachineType { get; set; } = string.Empty;

    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出厂日期（Manufacturing Date）
    /// </summary>
    public DateTime? ManufacturingDate { get; set; }

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    public decimal? StdCycleTimeSeconds { get; set; }

    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    public decimal? StdMinutesPerUnit { get; set; }

    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    public decimal? StdMinutesPerCycle { get; set; }

    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    public decimal? TheoreticalSpm { get; set; }

    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    public decimal? TheoreticalCycleTimeSeconds { get; set; }

    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    public decimal? StdEquipHourlyCapacity { get; set; }

    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    public decimal? AvailabilityRate { get; set; }

    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    public decimal? PerformanceRate { get; set; }

    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    public decimal? SetupMinutes { get; set; }

    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    public decimal? MoldChangeMinutes { get; set; }

    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    public decimal? MaterialChangeMinutes { get; set; }

    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    public decimal? MtbfHours { get; set; }

    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    public decimal? MttrHours { get; set; }

    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    public decimal? RepeatabilityAccuracy { get; set; }

    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    public decimal? ShutHeightAccuracy { get; set; }

    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    public decimal? InjectionAccuracy { get; set; }

    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    public decimal? TemperatureControlAccuracy { get; set; }

    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    public decimal? PressureControlAccuracy { get; set; }

    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    public decimal? ProcessCapabilityCpk { get; set; }

    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    public decimal? MaxDimensionalTolerance { get; set; }

    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    public string? MaxMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    public string? MinMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    public decimal? MaxMoldWeightTon { get; set; }

    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    public string? MoldHeightRange { get; set; } = string.Empty;

    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    public int? EjectionType { get; set; }

    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    public decimal? EjectionStrokeMm { get; set; }

    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    public int? CavityCount { get; set; }

    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    public int? QuickMoldChange { get; set; }

    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    public string? MoldCode { get; set; } = string.Empty;

    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    public decimal? RatedTonnage { get; set; }

    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    public decimal? ClampingForceKn { get; set; }

    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    public decimal? MaxStrokeMm { get; set; }

    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    public decimal? OpenStrokeMm { get; set; }

    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    public string? PlatenSize { get; set; } = string.Empty;

    /// <summary>
    /// 使用电压（V）
    /// </summary>
    public decimal? RatedVoltage { get; set; }

    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    public decimal? RatedPowerKw { get; set; }

    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    public decimal? AirConsumptionLpm { get; set; }

    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    public decimal? CoolingWaterFlowLpm { get; set; }

    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    public int? OperatorCount { get; set; }

    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    public int? IsCriticalResource { get; set; }

    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    public int? ParallelCapacity { get; set; }

    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    public int? AllowRushOrder { get; set; }

    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    public decimal? WarmupMinutes { get; set; }

    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    public string? OperatingTempRange { get; set; } = string.Empty;

    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    public string? OperatingHumidityRange { get; set; } = string.Empty;

    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    public decimal? NoiseLevelDb { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    public int? EquipmentRunStatus { get; set; }

    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    public decimal? MaintenanceIntervalHours { get; set; }

    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    public decimal? CumulativeRunHours { get; set; }

    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    public string? InterfaceType { get; set; } = string.Empty;

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）
    /// </summary>
    public DateTime? CommissioningDate { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）
    /// </summary>
    public DateTime? DecommissioningDate { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipAdministratorName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? ProdEquipStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// ProductionEquipment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionEquipmentImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    public int? EquipCategory { get; set; }

    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    public string? ProdEquipName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    public string? MachineType { get; set; } = string.Empty;

    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出厂日期（Manufacturing Date）
    /// </summary>
    public DateTime? ManufacturingDate { get; set; }

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    public decimal? StdCycleTimeSeconds { get; set; }

    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    public decimal? StdMinutesPerUnit { get; set; }

    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    public decimal? StdMinutesPerCycle { get; set; }

    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    public decimal? TheoreticalSpm { get; set; }

    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    public decimal? TheoreticalCycleTimeSeconds { get; set; }

    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    public decimal? StdEquipHourlyCapacity { get; set; }

    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    public decimal? AvailabilityRate { get; set; }

    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    public decimal? PerformanceRate { get; set; }

    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    public decimal? SetupMinutes { get; set; }

    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    public decimal? MoldChangeMinutes { get; set; }

    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    public decimal? MaterialChangeMinutes { get; set; }

    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    public decimal? MtbfHours { get; set; }

    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    public decimal? MttrHours { get; set; }

    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    public decimal? RepeatabilityAccuracy { get; set; }

    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    public decimal? ShutHeightAccuracy { get; set; }

    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    public decimal? InjectionAccuracy { get; set; }

    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    public decimal? TemperatureControlAccuracy { get; set; }

    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    public decimal? PressureControlAccuracy { get; set; }

    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    public decimal? ProcessCapabilityCpk { get; set; }

    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    public decimal? MaxDimensionalTolerance { get; set; }

    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    public string? MaxMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    public string? MinMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    public decimal? MaxMoldWeightTon { get; set; }

    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    public string? MoldHeightRange { get; set; } = string.Empty;

    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    public int? EjectionType { get; set; }

    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    public decimal? EjectionStrokeMm { get; set; }

    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    public int? CavityCount { get; set; }

    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    public int? QuickMoldChange { get; set; }

    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    public string? MoldCode { get; set; } = string.Empty;

    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    public decimal? RatedTonnage { get; set; }

    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    public decimal? ClampingForceKn { get; set; }

    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    public decimal? MaxStrokeMm { get; set; }

    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    public decimal? OpenStrokeMm { get; set; }

    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    public string? PlatenSize { get; set; } = string.Empty;

    /// <summary>
    /// 使用电压（V）
    /// </summary>
    public decimal? RatedVoltage { get; set; }

    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    public decimal? RatedPowerKw { get; set; }

    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    public decimal? AirConsumptionLpm { get; set; }

    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    public decimal? CoolingWaterFlowLpm { get; set; }

    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    public int? OperatorCount { get; set; }

    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    public int? IsCriticalResource { get; set; }

    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    public int? ParallelCapacity { get; set; }

    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    public int? AllowRushOrder { get; set; }

    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    public decimal? WarmupMinutes { get; set; }

    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    public string? OperatingTempRange { get; set; } = string.Empty;

    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    public string? OperatingHumidityRange { get; set; } = string.Empty;

    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    public decimal? NoiseLevelDb { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    public int? EquipmentRunStatus { get; set; }

    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    public decimal? MaintenanceIntervalHours { get; set; }

    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    public decimal? CumulativeRunHours { get; set; }

    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    public string? InterfaceType { get; set; } = string.Empty;

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）
    /// </summary>
    public DateTime? CommissioningDate { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）
    /// </summary>
    public DateTime? DecommissioningDate { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipAdministratorName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? ProdEquipStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// ProductionEquipment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionEquipmentExportDto
{
    /// <summary>
    /// ProductionEquipmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionEquipmentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    public int EquipCategory { get; set; } = 0;

    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    public string ProdEquipName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    public string? EquipBrand { get; set; } = string.Empty;

    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    public string MachineType { get; set; } = string.Empty;

    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    public string? SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出厂日期（Manufacturing Date）
    /// </summary>
    public DateTime? ManufacturingDate { get; set; }

    /// <summary>
    /// 设备规格
    /// </summary>
    public string? EquipSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    public decimal StdCycleTimeSeconds { get; set; }

    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    public decimal StdMinutesPerUnit { get; set; }

    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    public decimal StdMinutesPerCycle { get; set; }

    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    public decimal TheoreticalSpm { get; set; }

    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    public decimal TheoreticalCycleTimeSeconds { get; set; }

    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    public decimal StdEquipHourlyCapacity { get; set; }

    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    public decimal AvailabilityRate { get; set; }

    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    public decimal PerformanceRate { get; set; }

    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    public decimal SetupMinutes { get; set; }

    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    public decimal MoldChangeMinutes { get; set; }

    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    public decimal MaterialChangeMinutes { get; set; }

    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    public decimal MtbfHours { get; set; }

    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    public decimal MttrHours { get; set; }

    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    public decimal? RepeatabilityAccuracy { get; set; }

    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    public decimal? ShutHeightAccuracy { get; set; }

    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    public decimal? InjectionAccuracy { get; set; }

    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    public decimal? TemperatureControlAccuracy { get; set; }

    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    public decimal? PressureControlAccuracy { get; set; }

    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    public decimal? ProcessCapabilityCpk { get; set; }

    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    public decimal? MaxDimensionalTolerance { get; set; }

    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    public string? MaxMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    public string? MinMoldDimension { get; set; } = string.Empty;

    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    public decimal? MaxMoldWeightTon { get; set; }

    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    public string? MoldHeightRange { get; set; } = string.Empty;

    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    public int? EjectionType { get; set; }

    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    public decimal? EjectionStrokeMm { get; set; }

    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    public int CavityCount { get; set; } = 0;

    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    public int QuickMoldChange { get; set; } = 0;

    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    public string? MoldCode { get; set; } = string.Empty;

    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    public decimal? RatedTonnage { get; set; }

    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    public decimal? ClampingForceKn { get; set; }

    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    public decimal? MaxStrokeMm { get; set; }

    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    public decimal? OpenStrokeMm { get; set; }

    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    public string? PlatenSize { get; set; } = string.Empty;

    /// <summary>
    /// 使用电压（V）
    /// </summary>
    public decimal? RatedVoltage { get; set; }

    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    public decimal? RatedPowerKw { get; set; }

    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    public decimal? AirConsumptionLpm { get; set; }

    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    public decimal? CoolingWaterFlowLpm { get; set; }

    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    public int OperatorCount { get; set; } = 0;

    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    public int IsCriticalResource { get; set; } = 0;

    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    public int ParallelCapacity { get; set; } = 0;

    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    public int AllowRushOrder { get; set; } = 0;

    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    public decimal WarmupMinutes { get; set; }

    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    public string? OperatingTempRange { get; set; } = string.Empty;

    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    public string? OperatingHumidityRange { get; set; } = string.Empty;

    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    public decimal? NoiseLevelDb { get; set; }

    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    public int EquipmentRunStatus { get; set; } = 0;

    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    public decimal MaintenanceIntervalHours { get; set; }

    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    public decimal CumulativeRunHours { get; set; }

    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    public string? InterfaceType { get; set; } = string.Empty;

    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）
    /// </summary>
    public DateTime? CommissioningDate { get; set; }

    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）
    /// </summary>
    public DateTime? DecommissioningDate { get; set; }

    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）
    /// </summary>
    public DateTime? ScrapDate { get; set; }

    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipAdministratorName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int ProdEquipStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
