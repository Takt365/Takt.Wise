// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mps
// 文件名称：TaktEquipmentOperationRateDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：EquipmentOperationRate 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEquipmentOperationRate 生成，请按需审阅）
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
// EquipmentOperationRate 响应 DTO
// ========================================

/// <summary>
/// 机器稼动率实体（生产设备运行效率记录） 时间稼动率(%) = 稼动时间 ÷ 负荷时间 × 100%；为 OEE（设备综合效率）基础之一。
/// 对应前端 TaktEquipmentOperationRateDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEquipmentOperationRateDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EquipmentOperationRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }


    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    public int TimeCategory { get; set; } = 0;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
    /// </summary>
    public int EquipmentType { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
    /// </summary>
    public decimal PlannedRuntime { get; set; }

    /// <summary>
    /// 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
    /// </summary>
    public decimal ActualRuntime { get; set; }

    /// <summary>
    /// 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
    /// </summary>
    public decimal Downtime { get; set; }

    /// <summary>
    /// 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
    /// </summary>
    public decimal EquipmentOperationRate { get; set; }

    /// <summary>
    /// 计划产量
    /// </summary>
    public decimal PlannedOutput { get; set; }

    /// <summary>
    /// 实际产量
    /// </summary>
    public decimal ActualOutput { get; set; }

    /// <summary>
    /// 合格品数量
    /// </summary>
    public decimal QualifiedQuantity { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal DefectiveQuantity { get; set; }

    /// <summary>
    /// 良品率（%）
    /// </summary>
    public decimal YieldRate { get; set; }

    /// <summary>
    /// 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
    /// </summary>
    public int? DowntimeReasonType { get; set; }

    /// <summary>
    /// 停机原因描述（自由文本，与 DowntimeReasonType 配合）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentOperator { get; set; } = string.Empty;

    /// <summary>
    /// 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentMaintainer { get; set; } = string.Empty;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int RateStatus { get; set; } = 0;

}

// ========================================
// EquipmentOperationRate 查询 DTO
// ========================================

/// <summary>
/// EquipmentOperationRate 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEquipmentOperationRateQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    public int? TimeCategory { get; set; }

    /// <summary>
    /// 开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 结束日期（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 结束日期（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
    /// </summary>
    public int? EquipmentType { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
    /// </summary>
    public decimal? PlannedRuntime { get; set; }

    /// <summary>
    /// 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
    /// </summary>
    public decimal? ActualRuntime { get; set; }

    /// <summary>
    /// 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
    /// </summary>
    public decimal? Downtime { get; set; }

    /// <summary>
    /// 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
    /// </summary>
    public decimal? EquipmentOperationRate { get; set; }

    /// <summary>
    /// 计划产量
    /// </summary>
    public decimal? PlannedOutput { get; set; }

    /// <summary>
    /// 实际产量
    /// </summary>
    public decimal? ActualOutput { get; set; }

    /// <summary>
    /// 合格品数量
    /// </summary>
    public decimal? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal? DefectiveQuantity { get; set; }

    /// <summary>
    /// 良品率（%）
    /// </summary>
    public decimal? YieldRate { get; set; }

    /// <summary>
    /// 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
    /// </summary>
    public int? DowntimeReasonType { get; set; }

    /// <summary>
    /// 停机原因描述（自由文本，与 DowntimeReasonType 配合）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentOperator { get; set; } = string.Empty;

    /// <summary>
    /// 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentMaintainer { get; set; } = string.Empty;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int? RateStatus { get; set; }

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
// 创建EquipmentOperationRate DTO
// ========================================

/// <summary>
/// 创建EquipmentOperationRate DTO
/// </summary>
public class TaktEquipmentOperationRateCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=Id）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    public int TimeCategory { get; set; } = 0;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "设备编码（选项 TaktProductionEquipments/options；DictValue=Id）不能为空")]
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    [Required(ErrorMessage = "设备名称不能为空")]
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
    /// </summary>
    public int EquipmentType { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
    /// </summary>
    public decimal PlannedRuntime { get; set; }

    /// <summary>
    /// 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
    /// </summary>
    public decimal ActualRuntime { get; set; }

    /// <summary>
    /// 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
    /// </summary>
    public decimal Downtime { get; set; }

    /// <summary>
    /// 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
    /// </summary>
    public decimal EquipmentOperationRate { get; set; }

    /// <summary>
    /// 计划产量
    /// </summary>
    public decimal PlannedOutput { get; set; }

    /// <summary>
    /// 实际产量
    /// </summary>
    public decimal ActualOutput { get; set; }

    /// <summary>
    /// 合格品数量
    /// </summary>
    public decimal QualifiedQuantity { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal DefectiveQuantity { get; set; }

    /// <summary>
    /// 良品率（%）
    /// </summary>
    public decimal YieldRate { get; set; }

    /// <summary>
    /// 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
    /// </summary>
    public int? DowntimeReasonType { get; set; }

    /// <summary>
    /// 停机原因描述（自由文本，与 DowntimeReasonType 配合）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentOperator { get; set; } = string.Empty;

    /// <summary>
    /// 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentMaintainer { get; set; } = string.Empty;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int RateStatus { get; set; } = 0;

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
// 更新EquipmentOperationRate DTO
// ========================================

/// <summary>
/// 更新EquipmentOperationRate DTO
/// 继承 TaktEquipmentOperationRateCreateDto，添加 EquipmentOperationRateId 字段
/// </summary>
public class TaktEquipmentOperationRateUpdateDto : TaktEquipmentOperationRateCreateDto
{
    /// <summary>
    /// EquipmentOperationRateID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }

}

// ========================================
// EquipmentOperationRate 状态 DTO
// ========================================

/// <summary>
/// EquipmentOperationRate 状态更新 DTO
/// </summary>
public class TaktEquipmentOperationRateStatusDto
{
    /// <summary>
    /// EquipmentOperationRateID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（0=正常，1=停用）不能为空")]
    public int RateStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EquipmentOperationRate 导入模板行 DTO
/// </summary>
public class TaktEquipmentOperationRateTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    public int? TimeCategory { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
    /// </summary>
    public int? EquipmentType { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
    /// </summary>
    public decimal? PlannedRuntime { get; set; }

    /// <summary>
    /// 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
    /// </summary>
    public decimal? ActualRuntime { get; set; }

    /// <summary>
    /// 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
    /// </summary>
    public decimal? Downtime { get; set; }

    /// <summary>
    /// 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
    /// </summary>
    public decimal? EquipmentOperationRate { get; set; }

    /// <summary>
    /// 计划产量
    /// </summary>
    public decimal? PlannedOutput { get; set; }

    /// <summary>
    /// 实际产量
    /// </summary>
    public decimal? ActualOutput { get; set; }

    /// <summary>
    /// 合格品数量
    /// </summary>
    public decimal? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal? DefectiveQuantity { get; set; }

    /// <summary>
    /// 良品率（%）
    /// </summary>
    public decimal? YieldRate { get; set; }

    /// <summary>
    /// 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
    /// </summary>
    public int? DowntimeReasonType { get; set; }

    /// <summary>
    /// 停机原因描述（自由文本，与 DowntimeReasonType 配合）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentOperator { get; set; } = string.Empty;

    /// <summary>
    /// 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentMaintainer { get; set; } = string.Empty;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int? RateStatus { get; set; }

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
/// EquipmentOperationRate 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEquipmentOperationRateImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    public int? TimeCategory { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
    /// </summary>
    public int? EquipmentType { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
    /// </summary>
    public decimal? PlannedRuntime { get; set; }

    /// <summary>
    /// 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
    /// </summary>
    public decimal? ActualRuntime { get; set; }

    /// <summary>
    /// 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
    /// </summary>
    public decimal? Downtime { get; set; }

    /// <summary>
    /// 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
    /// </summary>
    public decimal? EquipmentOperationRate { get; set; }

    /// <summary>
    /// 计划产量
    /// </summary>
    public decimal? PlannedOutput { get; set; }

    /// <summary>
    /// 实际产量
    /// </summary>
    public decimal? ActualOutput { get; set; }

    /// <summary>
    /// 合格品数量
    /// </summary>
    public decimal? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal? DefectiveQuantity { get; set; }

    /// <summary>
    /// 良品率（%）
    /// </summary>
    public decimal? YieldRate { get; set; }

    /// <summary>
    /// 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
    /// </summary>
    public int? DowntimeReasonType { get; set; }

    /// <summary>
    /// 停机原因描述（自由文本，与 DowntimeReasonType 配合）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentOperator { get; set; } = string.Empty;

    /// <summary>
    /// 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentMaintainer { get; set; } = string.Empty;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int? RateStatus { get; set; }

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
/// EquipmentOperationRate 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEquipmentOperationRateExportDto
{
    /// <summary>
    /// EquipmentOperationRateID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=Id）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    public int TimeCategory { get; set; } = 0;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
    /// </summary>
    public int EquipmentType { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
    /// </summary>
    public decimal PlannedRuntime { get; set; }

    /// <summary>
    /// 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
    /// </summary>
    public decimal ActualRuntime { get; set; }

    /// <summary>
    /// 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
    /// </summary>
    public decimal Downtime { get; set; }

    /// <summary>
    /// 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
    /// </summary>
    public decimal EquipmentOperationRate { get; set; }

    /// <summary>
    /// 计划产量
    /// </summary>
    public decimal PlannedOutput { get; set; }

    /// <summary>
    /// 实际产量
    /// </summary>
    public decimal ActualOutput { get; set; }

    /// <summary>
    /// 合格品数量
    /// </summary>
    public decimal QualifiedQuantity { get; set; }

    /// <summary>
    /// 不良品数量
    /// </summary>
    public decimal DefectiveQuantity { get; set; }

    /// <summary>
    /// 良品率（%）
    /// </summary>
    public decimal YieldRate { get; set; }

    /// <summary>
    /// 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
    /// </summary>
    public int? DowntimeReasonType { get; set; }

    /// <summary>
    /// 停机原因描述（自由文本，与 DowntimeReasonType 配合）
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentOperator { get; set; } = string.Empty;

    /// <summary>
    /// 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? EquipmentMaintainer { get; set; } = string.Empty;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int RateStatus { get; set; } = 0;

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
