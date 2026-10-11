// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionEquipment.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产设备主数据（SMT/插件/冲压/注塑/装配等；MPS RCCP、APS 有限产能排程资源）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产设备主数据（排程资源；粗能力 StdEquipHourlyCapacity=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate；多穴=(60÷StdMinutesPerCycle)×CavityCount×AvailabilityRate）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_production_equipment", "生产设备表")]
[SugarIndex("ix_production_equipment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_equipment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_production_equipment_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(StorageLocation), OrderByType.Asc, nameof(ProdEquipCode), OrderByType.Asc, true)]
public class TaktProductionEquipment : TaktCompanyEntityBase
{
    // ---- 基础标识 ----
    /// <summary>
    /// 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
    /// </summary>
    [SugarColumn(ColumnName = "equip_category", ColumnDescription = "设备类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EquipCategory { get; set; } = 1;
    /// <summary>
    /// 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
    /// </summary>
    [SugarColumn(ColumnName = "prod_equip_code", ColumnDescription = "生产设备编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = false)]
    public string ProdEquipCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产设备名称（列表展示名）
    /// </summary>
    [SugarColumn(ColumnName = "prod_equip_name", ColumnDescription = "生产设备名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ProdEquipName { get; set; } = string.Empty;
    /// <summary>
    /// 制造商
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer", ColumnDescription = "制造商", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Manufacturer { get; set; }
    /// <summary>
    /// 设备品牌（铭牌 Brand）
    /// </summary>
    [SugarColumn(ColumnName = "equip_brand", ColumnDescription = "设备品牌", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? EquipBrand { get; set; }
    /// <summary>
    /// 机型名称（铭牌 Machine Type，如 SP18P-L）
    /// </summary>
    [SugarColumn(ColumnName = "machine_type", ColumnDescription = "机型名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MachineType { get; set; } = string.Empty;
    /// <summary>
    /// 型号（铭牌 Model No，如 NM-EJP1A）
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "型号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ModelCode { get; set; }
    /// <summary>
    /// 序列号（铭牌 Serial No，如 1P8V0336）
    /// </summary>
    [SugarColumn(ColumnName = "serial_code", ColumnDescription = "序列号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SerialCode { get; set; }
    /// <summary>
    /// 出厂日期（Manufacturing Date）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturing_date", ColumnDescription = "出厂日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ManufacturingDate { get; set; }
    /// <summary>
    /// 设备规格
    /// </summary>
    [SugarColumn(ColumnName = "equip_specification", ColumnDescription = "设备规格", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? EquipSpecification { get; set; }

    // ---- 产能与节拍 ----
    /// <summary>
    /// 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
    /// </summary>
    [SugarColumn(ColumnName = "std_cycle_time_seconds", ColumnDescription = "理论周期时间秒", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StdCycleTimeSeconds { get; set; } = 0;
    /// <summary>
    /// 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
    /// </summary>
    [SugarColumn(ColumnName = "std_minutes_per_unit", ColumnDescription = "标准分钟每件", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StdMinutesPerUnit { get; set; } = 0;
    /// <summary>
    /// 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
    /// </summary>
    [SugarColumn(ColumnName = "std_minutes_per_cycle", ColumnDescription = "标准分钟每周期", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StdMinutesPerCycle { get; set; } = 0;
    /// <summary>
    /// 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
    /// </summary>
    [SugarColumn(ColumnName = "theoretical_spm", ColumnDescription = "理论模次每小时", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TheoreticalSpm { get; set; } = 0;
    /// <summary>
    /// 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
    /// </summary>
    [SugarColumn(ColumnName = "theoretical_cycle_time_seconds", ColumnDescription = "理论成型周期秒", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TheoreticalCycleTimeSeconds { get; set; } = 0;
    /// <summary>
    /// 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
    /// </summary>
    [SugarColumn(ColumnName = "std_equip_hourly_capacity", ColumnDescription = "设备标准小时产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StdEquipHourlyCapacity { get; set; } = 0;
    /// <summary>
    /// 设备时间稼动率（AvailabilityRate，0–1）
    /// </summary>
    [SugarColumn(ColumnName = "availability_rate", ColumnDescription = "设备时间稼动率", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal AvailabilityRate { get; set; } = 1;
    /// <summary>
    /// 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
    /// </summary>
    [SugarColumn(ColumnName = "performance_rate", ColumnDescription = "性能稼动率", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal PerformanceRate { get; set; } = 1;
    /// <summary>
    /// 准备时间（分钟；通用调试）
    /// </summary>
    [SugarColumn(ColumnName = "setup_minutes", ColumnDescription = "准备时间分钟", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SetupMinutes { get; set; } = 0;
    /// <summary>
    /// 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
    /// </summary>
    [SugarColumn(ColumnName = "mold_change_minutes", ColumnDescription = "换模时间分钟", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MoldChangeMinutes { get; set; } = 0;
    /// <summary>
    /// 换料时间（分钟；注塑料筒清洗等）
    /// </summary>
    [SugarColumn(ColumnName = "material_change_minutes", ColumnDescription = "换料时间分钟", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MaterialChangeMinutes { get; set; } = 0;
    /// <summary>
    /// 平均无故障时间 MTBF（小时）
    /// </summary>
    [SugarColumn(ColumnName = "mtbf_hours", ColumnDescription = "平均无故障时间小时", ColumnDataType = "decimal", Length = 12, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MtbfHours { get; set; } = 0;
    /// <summary>
    /// 平均修复时间 MTTR（小时）
    /// </summary>
    [SugarColumn(ColumnName = "mttr_hours", ColumnDescription = "平均修复时间小时", ColumnDataType = "decimal", Length = 12, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MttrHours { get; set; } = 0;

    // ---- 精度与工艺能力 ----
    /// <summary>
    /// 重复定位精度（mm；滑块/模板 ±0.01~0.05）
    /// </summary>
    [SugarColumn(ColumnName = "repeatability_accuracy", ColumnDescription = "重复定位精度", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? RepeatabilityAccuracy { get; set; }
    /// <summary>
    /// 闭合高度精度（mm；冲压）
    /// </summary>
    [SugarColumn(ColumnName = "shut_height_accuracy", ColumnDescription = "闭合高度精度", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? ShutHeightAccuracy { get; set; }
    /// <summary>
    /// 注射精度（%；注塑计量 ±0.5%）
    /// </summary>
    [SugarColumn(ColumnName = "injection_accuracy", ColumnDescription = "注射精度", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? InjectionAccuracy { get; set; }
    /// <summary>
    /// 温控精度（℃；注塑 ±1℃）
    /// </summary>
    [SugarColumn(ColumnName = "temperature_control_accuracy", ColumnDescription = "温控精度", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? TemperatureControlAccuracy { get; set; }
    /// <summary>
    /// 压力控制精度（%；冲压/注塑 ±1–2%）
    /// </summary>
    [SugarColumn(ColumnName = "pressure_control_accuracy", ColumnDescription = "压力控制精度", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? PressureControlAccuracy { get; set; }
    /// <summary>
    /// 工艺能力 Cpk（关键尺寸）
    /// </summary>
    [SugarColumn(ColumnName = "process_capability_cpk", ColumnDescription = "工艺能力Cpk", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? ProcessCapabilityCpk { get; set; }
    /// <summary>
    /// 最大成型公差（mm）
    /// </summary>
    [SugarColumn(ColumnName = "max_dimensional_tolerance", ColumnDescription = "最大成型公差", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = true)]
    public decimal? MaxDimensionalTolerance { get; set; }

    // ---- 模具/工装 ----
    /// <summary>
    /// 最大模具尺寸（L×W×H）
    /// </summary>
    [SugarColumn(ColumnName = "max_mold_dimension", ColumnDescription = "最大模具尺寸", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? MaxMoldDimension { get; set; }
    /// <summary>
    /// 最小模具尺寸（L×W×H）
    /// </summary>
    [SugarColumn(ColumnName = "min_mold_dimension", ColumnDescription = "最小模具尺寸", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? MinMoldDimension { get; set; }
    /// <summary>
    /// 模具重量上限（ton）
    /// </summary>
    [SugarColumn(ColumnName = "max_mold_weight_ton", ColumnDescription = "模具重量上限吨", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = true)]
    public decimal? MaxMoldWeightTon { get; set; }
    /// <summary>
    /// 模具厚度范围（冲压闭合高度/注塑模板间距）
    /// </summary>
    [SugarColumn(ColumnName = "mold_height_range", ColumnDescription = "模具厚度范围", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? MoldHeightRange { get; set; }
    /// <summary>
    /// 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
    /// </summary>
    [SugarColumn(ColumnName = "ejection_type", ColumnDescription = "顶出方式", ColumnDataType = "int", IsNullable = true)]
    public int? EjectionType { get; set; }
    /// <summary>
    /// 顶出行程（mm）
    /// </summary>
    [SugarColumn(ColumnName = "ejection_stroke_mm", ColumnDescription = "顶出行程毫米", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? EjectionStrokeMm { get; set; }
    /// <summary>
    /// 工位数/穴数（CavityCount；一出几，产能折算关键）
    /// </summary>
    [SugarColumn(ColumnName = "cavity_count", ColumnDescription = "工位穴数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CavityCount { get; set; } = 1;
    /// <summary>
    /// 快速换模（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "quick_mold_change", ColumnDescription = "快速换模", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QuickMoldChange { get; set; } = 0;
    /// <summary>
    /// 模具编码（模具主数据关联）
    /// </summary>
    [SugarColumn(ColumnName = "mold_code", ColumnDescription = "模具编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? MoldCode { get; set; }

    // ---- 资源与约束 ----
    /// <summary>
    /// 额定吨位（ton；冲压）
    /// </summary>
    [SugarColumn(ColumnName = "rated_tonnage", ColumnDescription = "额定吨位", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = true)]
    public decimal? RatedTonnage { get; set; }
    /// <summary>
    /// 锁模力（kN；注塑）
    /// </summary>
    [SugarColumn(ColumnName = "clamping_force_kn", ColumnDescription = "锁模力千牛", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = true)]
    public decimal? ClampingForceKn { get; set; }
    /// <summary>
    /// 最大行程（mm）
    /// </summary>
    [SugarColumn(ColumnName = "max_stroke_mm", ColumnDescription = "最大行程毫米", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? MaxStrokeMm { get; set; }
    /// <summary>
    /// 开模行程（mm；注塑）
    /// </summary>
    [SugarColumn(ColumnName = "open_stroke_mm", ColumnDescription = "开模行程毫米", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? OpenStrokeMm { get; set; }
    /// <summary>
    /// 模板尺寸（mm）
    /// </summary>
    [SugarColumn(ColumnName = "platen_size", ColumnDescription = "模板尺寸", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PlatenSize { get; set; }
    /// <summary>
    /// 使用电压（V）
    /// </summary>
    [SugarColumn(ColumnName = "rated_voltage", ColumnDescription = "额定电压", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? RatedVoltage { get; set; }
    /// <summary>
    /// 额定功率（kW）
    /// </summary>
    [SugarColumn(ColumnName = "rated_power_kw", ColumnDescription = "额定功率千瓦", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = true)]
    public decimal? RatedPowerKw { get; set; }
    /// <summary>
    /// 耗气量（L/min）
    /// </summary>
    [SugarColumn(ColumnName = "air_consumption_lpm", ColumnDescription = "耗气量升每分钟", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = true)]
    public decimal? AirConsumptionLpm { get; set; }
    /// <summary>
    /// 冷却水流量（L/min）
    /// </summary>
    [SugarColumn(ColumnName = "cooling_water_flow_lpm", ColumnDescription = "冷却水流量升每分钟", ColumnDataType = "decimal", Length = 12, DecimalDigits = 4, IsNullable = true)]
    public decimal? CoolingWaterFlowLpm { get; set; }
    /// <summary>
    /// 操作人员数（标准配人）
    /// </summary>
    [SugarColumn(ColumnName = "operator_count", ColumnDescription = "操作人员数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int OperatorCount { get; set; } = 1;
    /// <summary>
    /// 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
    /// </summary>
    [SugarColumn(ColumnName = "is_critical_resource", ColumnDescription = "是否关键设备", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsCriticalResource { get; set; } = 0;
    /// <summary>
    /// 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
    /// </summary>
    [SugarColumn(ColumnName = "parallel_capacity", ColumnDescription = "并行能力", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ParallelCapacity { get; set; } = 1;
    /// <summary>
    /// 是否允许插单（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "allow_rush_order", ColumnDescription = "是否允许插单", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int AllowRushOrder { get; set; } = 1;

    // ---- 电气环境与运行状态 ----
    /// <summary>
    /// 开机预热时间（分钟；注塑螺杆预热）
    /// </summary>
    [SugarColumn(ColumnName = "warmup_minutes", ColumnDescription = "开机预热时间分钟", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal WarmupMinutes { get; set; } = 0;
    /// <summary>
    /// 工作温度范围（℃）
    /// </summary>
    [SugarColumn(ColumnName = "operating_temp_range", ColumnDescription = "工作温度范围", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? OperatingTempRange { get; set; }
    /// <summary>
    /// 湿度范围（%RH）
    /// </summary>
    [SugarColumn(ColumnName = "operating_humidity_range", ColumnDescription = "工作湿度范围", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? OperatingHumidityRange { get; set; }
    /// <summary>
    /// 噪音水平（dB）
    /// </summary>
    [SugarColumn(ColumnName = "noise_level_db", ColumnDescription = "噪音水平分贝", ColumnDataType = "decimal", Length = 8, DecimalDigits = 2, IsNullable = true)]
    public decimal? NoiseLevelDb { get; set; }
    /// <summary>
    /// 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_run_status", ColumnDescription = "设备运行状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EquipmentRunStatus { get; set; } = 1;
    /// <summary>
    /// 保养周期（小时）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_interval_hours", ColumnDescription = "保养周期小时", ColumnDataType = "decimal", Length = 12, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MaintenanceIntervalHours { get; set; } = 0;
    /// <summary>
    /// 累计运行时间（小时；寿命/PM）
    /// </summary>
    [SugarColumn(ColumnName = "cumulative_run_hours", ColumnDescription = "累计运行时间小时", ColumnDataType = "decimal", Length = 14, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal CumulativeRunHours { get; set; } = 0;
    /// <summary>
    /// 车间集成接口（SMEMA/PLC 等）
    /// </summary>
    [SugarColumn(ColumnName = "interface_type", ColumnDescription = "车间集成接口", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? InterfaceType { get; set; }
    /// <summary>
    /// 投产日期（Commissioning Date；设备正式投产日期）
    /// </summary>
    [SugarColumn(ColumnName = "commissioning_date", ColumnDescription = "投产日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CommissioningDate { get; set; }
    /// <summary>
    /// 停产日期（Decommissioning Date；设备停止生产日期）
    /// </summary>
    [SugarColumn(ColumnName = "decommissioning_date", ColumnDescription = "停产日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DecommissioningDate { get; set; }
    /// <summary>
    /// 报废日期（资产注销 / Scrap Date）
    /// </summary>
    [SugarColumn(ColumnName = "scrap_date", ColumnDescription = "报废日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ScrapDate { get; set; }
    /// <summary>
    /// 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
    /// </summary>
    [SugarColumn(ColumnName = "storage_location", ColumnDescription = "存放位置", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string StorageLocation { get; set; } = string.Empty;
    /// <summary>
    /// 设备管理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "equip_administrator_id", ColumnDescription = "设备管理员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipAdministratorId { get; set; }
    /// <summary>
    /// 设备管理员名称（冗余：按 EquipAdministratorId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "equip_administrator_name", ColumnDescription = "设备管理员名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EquipAdministratorName { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "prod_equip_status", ColumnDescription = "生产设备状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProdEquipStatus { get; set; } = 1;
}
