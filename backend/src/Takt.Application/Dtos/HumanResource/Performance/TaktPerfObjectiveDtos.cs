// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktPerfObjectiveDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfObjective 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPerfObjective 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Performance;

// ========================================
// PerfObjective 响应 DTO
// ========================================

/// <summary>
/// 员工绩效目标
/// 对应前端 TaktPerfObjectiveDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktPerfObjectiveDto : TaktApprovalDtoBase
{
    /// <summary>
    /// PerfObjectiveID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfObjectiveId { get; set; }


    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int ObjectiveStatus { get; set; } = 0;

}

// ========================================
// PerfObjective 查询 DTO
// ========================================

/// <summary>
/// PerfObjective 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPerfObjectiveQueryDto : TaktPagedQuery
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string? ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal? TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal? CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal? ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 截止日期（范围查询-开始）
    /// </summary>
    public DateTime? DueDateStart { get; set; }

    /// <summary>
    /// 截止日期（范围查询-结束）
    /// </summary>
    public DateTime? DueDateEnd { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string? AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int? ObjectiveStatus { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建PerfObjective DTO
// ========================================

/// <summary>
/// 创建PerfObjective DTO
/// </summary>
public class TaktPerfObjectiveCreateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    [Required(ErrorMessage = "员工姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    [Required(ErrorMessage = "目标周期（如 2026-Q1、2026-Annual）不能为空")]
    public string ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    [Required(ErrorMessage = "目标描述不能为空")]
    public string ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    [Required(ErrorMessage = "目标达成说明不能为空")]
    public string AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int ObjectiveStatus { get; set; } = 0;

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
// 更新PerfObjective DTO
// ========================================

/// <summary>
/// 更新PerfObjective DTO
/// 继承 TaktPerfObjectiveCreateDto，添加 PerfObjectiveId 字段
/// </summary>
public class TaktPerfObjectiveUpdateDto : TaktPerfObjectiveCreateDto
{
    /// <summary>
    /// PerfObjectiveID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfObjectiveId { get; set; }

}

// ========================================
// PerfObjective 状态 DTO
// ========================================

/// <summary>
/// PerfObjective 状态更新 DTO
/// </summary>
public class TaktPerfObjectiveStatusDto
{
    /// <summary>
    /// PerfObjectiveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfObjectiveId { get; set; }

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    [Required(ErrorMessage = "业务状态（0=待确认 1=进行中 2=已完成）不能为空")]
    public int ObjectiveStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PerfObjective 导入模板行 DTO
/// </summary>
public class TaktPerfObjectiveTemplateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string? ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal? TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal? CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal? ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string? AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int? ObjectiveStatus { get; set; }

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
/// PerfObjective 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPerfObjectiveImportDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string? ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal? TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal? ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal? CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal? ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string? AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int? ObjectiveStatus { get; set; }

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
/// PerfObjective 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPerfObjectiveExportDto
{
    /// <summary>
    /// PerfObjectiveID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfObjectiveId { get; set; }

    /// <summary>
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string ObjectivePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 目标描述
    /// </summary>
    public string ObjectiveDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标值
    /// </summary>
    public decimal TargetValue { get; set; }

    /// <summary>
    /// 实际完成值
    /// </summary>
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// 目标权重（%）
    /// </summary>
    public decimal ObjectiveWeight { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 目标达成说明
    /// </summary>
    public string AchievementNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待确认 1=进行中 2=已完成）
    /// </summary>
    public int ObjectiveStatus { get; set; } = 0;

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
