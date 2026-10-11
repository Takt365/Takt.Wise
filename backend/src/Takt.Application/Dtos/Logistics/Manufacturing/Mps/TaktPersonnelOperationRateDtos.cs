// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mps
// 文件名称：TaktPersonnelOperationRateDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PersonnelOperationRate 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPersonnelOperationRate 生成，请按需审阅）
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
// PersonnelOperationRate 响应 DTO
// ========================================

/// <summary>
/// 人员稼动率实体（生产线人员作业效率记录） 人员稼动率(%) = 在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
/// 对应前端 TaktPersonnelOperationRateDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPersonnelOperationRateDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PersonnelOperationRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PersonnelOperationRateId { get; set; }


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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? TeamCodeName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    public int PlannedDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    public int ActualDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    public int PlannedIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    public int ActualIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    public decimal PlannedWorkTime { get; set; }

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    public decimal ActualWorkTime { get; set; }

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    public decimal BreakTime { get; set; }

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    public decimal IdleTime { get; set; }

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    public decimal PersonnelOperationRate { get; set; }

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
    /// 工作效率（%）
    /// </summary>
    public decimal WorkEfficiency { get; set; }

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    public string? IdleReason { get; set; } = string.Empty;

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    public decimal OvertimeHours { get; set; }

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 主管（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? Supervisor { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    public int RateStatus { get; set; } = 0;

}

// ========================================
// PersonnelOperationRate 查询 DTO
// ========================================

/// <summary>
/// PersonnelOperationRate 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPersonnelOperationRateQueryDto : TaktPagedQuery
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? TeamCodeName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    public int? PlannedDirectPersonnelCount { get; set; }

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    public int? ActualDirectPersonnelCount { get; set; }

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    public int? PlannedIndirectPersonnelCount { get; set; }

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    public int? ActualIndirectPersonnelCount { get; set; }

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    public decimal? PlannedWorkTime { get; set; }

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    public decimal? ActualWorkTime { get; set; }

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    public decimal? BreakTime { get; set; }

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    public decimal? IdleTime { get; set; }

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    public decimal? PersonnelOperationRate { get; set; }

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
    /// 工作效率（%）
    /// </summary>
    public decimal? WorkEfficiency { get; set; }

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    public string? IdleReason { get; set; } = string.Empty;

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    public decimal? OvertimeHours { get; set; }

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 主管（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? Supervisor { get; set; } = string.Empty;

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
// 创建PersonnelOperationRate DTO
// ========================================

/// <summary>
/// 创建PersonnelOperationRate DTO
/// </summary>
public class TaktPersonnelOperationRateCreateDto
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? TeamCodeName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    public int PlannedDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    public int ActualDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    public int PlannedIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    public int ActualIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    public decimal PlannedWorkTime { get; set; }

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    public decimal ActualWorkTime { get; set; }

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    public decimal BreakTime { get; set; }

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    public decimal IdleTime { get; set; }

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    public decimal PersonnelOperationRate { get; set; }

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
    /// 工作效率（%）
    /// </summary>
    public decimal WorkEfficiency { get; set; }

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    public string? IdleReason { get; set; } = string.Empty;

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    public decimal OvertimeHours { get; set; }

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 主管（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? Supervisor { get; set; } = string.Empty;

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
// 更新PersonnelOperationRate DTO
// ========================================

/// <summary>
/// 更新PersonnelOperationRate DTO
/// 继承 TaktPersonnelOperationRateCreateDto，添加 PersonnelOperationRateId 字段
/// </summary>
public class TaktPersonnelOperationRateUpdateDto : TaktPersonnelOperationRateCreateDto
{
    /// <summary>
    /// PersonnelOperationRateID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PersonnelOperationRateId { get; set; }

}

// ========================================
// PersonnelOperationRate 状态 DTO
// ========================================

/// <summary>
/// PersonnelOperationRate 状态更新 DTO
/// </summary>
public class TaktPersonnelOperationRateStatusDto
{
    /// <summary>
    /// PersonnelOperationRateID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PersonnelOperationRateId { get; set; }

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
/// PersonnelOperationRate 导入模板行 DTO
/// </summary>
public class TaktPersonnelOperationRateTemplateDto
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? TeamCodeName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    public int? PlannedDirectPersonnelCount { get; set; }

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    public int? ActualDirectPersonnelCount { get; set; }

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    public int? PlannedIndirectPersonnelCount { get; set; }

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    public int? ActualIndirectPersonnelCount { get; set; }

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    public decimal? PlannedWorkTime { get; set; }

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    public decimal? ActualWorkTime { get; set; }

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    public decimal? BreakTime { get; set; }

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    public decimal? IdleTime { get; set; }

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    public decimal? PersonnelOperationRate { get; set; }

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
    /// 工作效率（%）
    /// </summary>
    public decimal? WorkEfficiency { get; set; }

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    public string? IdleReason { get; set; } = string.Empty;

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    public decimal? OvertimeHours { get; set; }

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 主管（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? Supervisor { get; set; } = string.Empty;

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
/// PersonnelOperationRate 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPersonnelOperationRateImportDto
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? TeamCodeName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    public int? PlannedDirectPersonnelCount { get; set; }

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    public int? ActualDirectPersonnelCount { get; set; }

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    public int? PlannedIndirectPersonnelCount { get; set; }

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    public int? ActualIndirectPersonnelCount { get; set; }

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    public decimal? PlannedWorkTime { get; set; }

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    public decimal? ActualWorkTime { get; set; }

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    public decimal? BreakTime { get; set; }

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    public decimal? IdleTime { get; set; }

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    public decimal? PersonnelOperationRate { get; set; }

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
    /// 工作效率（%）
    /// </summary>
    public decimal? WorkEfficiency { get; set; }

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    public string? IdleReason { get; set; } = string.Empty;

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    public decimal? OvertimeHours { get; set; }

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 主管（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? Supervisor { get; set; } = string.Empty;

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
/// PersonnelOperationRate 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPersonnelOperationRateExportDto
{
    /// <summary>
    /// PersonnelOperationRateID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PersonnelOperationRateId { get; set; }

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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? TeamCodeName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    public int PlannedDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    public int ActualDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    public int PlannedIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    public int ActualIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    public decimal PlannedWorkTime { get; set; }

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    public decimal ActualWorkTime { get; set; }

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    public decimal BreakTime { get; set; }

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    public decimal IdleTime { get; set; }

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    public decimal PersonnelOperationRate { get; set; }

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
    /// 工作效率（%）
    /// </summary>
    public decimal WorkEfficiency { get; set; }

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    public string? IdleReason { get; set; } = string.Empty;

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    public decimal OvertimeHours { get; set; }

    /// <summary>
    /// 班组长（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeader { get; set; } = string.Empty;

    /// <summary>
    /// 主管（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? Supervisor { get; set; } = string.Empty;

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
