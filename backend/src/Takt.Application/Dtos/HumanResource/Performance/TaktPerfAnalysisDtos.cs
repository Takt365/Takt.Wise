// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktPerfAnalysisDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfAnalysis 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPerfAnalysis 生成，请按需审阅）
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
// PerfAnalysis 响应 DTO
// ========================================

/// <summary>
/// 分析改进
/// 对应前端 TaktPerfAnalysisDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktPerfAnalysisDto : TaktApprovalDtoBase
{
    /// <summary>
    /// PerfAnalysisID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAnalysisId { get; set; }


    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

}

// ========================================
// PerfAnalysis 查询 DTO
// ========================================

/// <summary>
/// PerfAnalysis 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPerfAnalysisQueryDto : TaktPagedQuery
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
    /// 关联考核评估 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssessmentId { get; set; }

    /// <summary>
    /// 改进计划标题
    /// </summary>
    public string? PlanTitle { get; set; } = string.Empty;

    /// <summary>
    /// 改进领域
    /// </summary>
    public string? ImprovementArea { get; set; } = string.Empty;

    /// <summary>
    /// 当前状况描述
    /// </summary>
    public string? CurrentSituation { get; set; } = string.Empty;

    /// <summary>
    /// 改进目标
    /// </summary>
    public string? ImprovementGoal { get; set; } = string.Empty;

    /// <summary>
    /// 改进措施
    /// </summary>
    public string? ImprovementActions { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanDateStart { get; set; }

    /// <summary>
    /// 计划制定日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanDateEnd { get; set; }

    /// <summary>
    /// 目标完成日期（范围查询-开始）
    /// </summary>
    public DateTime? TargetCompletionDateStart { get; set; }

    /// <summary>
    /// 目标完成日期（范围查询-结束）
    /// </summary>
    public DateTime? TargetCompletionDateEnd { get; set; }

    /// <summary>
    /// 进度百分比（%）
    /// </summary>
    public decimal? ProgressPercentage { get; set; }

    /// <summary>
    /// 改进结果说明
    /// </summary>
    public string? ResultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 指导老师 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MentorId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    public int? ImprovementStatus { get; set; }

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
// 创建PerfAnalysis DTO
// ========================================

/// <summary>
/// 创建PerfAnalysis DTO
/// </summary>
public class TaktPerfAnalysisCreateDto
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
    /// 关联考核评估 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }

    /// <summary>
    /// 改进计划标题
    /// </summary>
    [Required(ErrorMessage = "改进计划标题不能为空")]
    public string PlanTitle { get; set; } = string.Empty;

    /// <summary>
    /// 改进领域
    /// </summary>
    [Required(ErrorMessage = "改进领域不能为空")]
    public string ImprovementArea { get; set; } = string.Empty;

    /// <summary>
    /// 当前状况描述
    /// </summary>
    [Required(ErrorMessage = "当前状况描述不能为空")]
    public string CurrentSituation { get; set; } = string.Empty;

    /// <summary>
    /// 改进目标
    /// </summary>
    [Required(ErrorMessage = "改进目标不能为空")]
    public string ImprovementGoal { get; set; } = string.Empty;

    /// <summary>
    /// 改进措施
    /// </summary>
    [Required(ErrorMessage = "改进措施不能为空")]
    public string ImprovementActions { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 目标完成日期
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }

    /// <summary>
    /// 进度百分比（%）
    /// </summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>
    /// 改进结果说明
    /// </summary>
    [Required(ErrorMessage = "改进结果说明不能为空")]
    public string ResultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 指导老师 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MentorId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

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
// 更新PerfAnalysis DTO
// ========================================

/// <summary>
/// 更新PerfAnalysis DTO
/// 继承 TaktPerfAnalysisCreateDto，添加 PerfAnalysisId 字段
/// </summary>
public class TaktPerfAnalysisUpdateDto : TaktPerfAnalysisCreateDto
{
    /// <summary>
    /// PerfAnalysisID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAnalysisId { get; set; }

}

// ========================================
// PerfAnalysis 状态 DTO
// ========================================

/// <summary>
/// PerfAnalysis 状态更新 DTO
/// </summary>
public class TaktPerfAnalysisStatusDto
{
    /// <summary>
    /// PerfAnalysisID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAnalysisId { get; set; }

    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    [Required(ErrorMessage = "业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）不能为空")]
    public int ImprovementStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PerfAnalysis 导入模板行 DTO
/// </summary>
public class TaktPerfAnalysisTemplateDto
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
    /// 关联考核评估 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssessmentId { get; set; }

    /// <summary>
    /// 改进计划标题
    /// </summary>
    public string? PlanTitle { get; set; } = string.Empty;

    /// <summary>
    /// 改进领域
    /// </summary>
    public string? ImprovementArea { get; set; } = string.Empty;

    /// <summary>
    /// 当前状况描述
    /// </summary>
    public string? CurrentSituation { get; set; } = string.Empty;

    /// <summary>
    /// 改进目标
    /// </summary>
    public string? ImprovementGoal { get; set; } = string.Empty;

    /// <summary>
    /// 改进措施
    /// </summary>
    public string? ImprovementActions { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 目标完成日期
    /// </summary>
    public DateTime? TargetCompletionDate { get; set; }

    /// <summary>
    /// 进度百分比（%）
    /// </summary>
    public decimal? ProgressPercentage { get; set; }

    /// <summary>
    /// 改进结果说明
    /// </summary>
    public string? ResultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 指导老师 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MentorId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    public int? ImprovementStatus { get; set; }

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
/// PerfAnalysis 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPerfAnalysisImportDto
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
    /// 关联考核评估 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssessmentId { get; set; }

    /// <summary>
    /// 改进计划标题
    /// </summary>
    public string? PlanTitle { get; set; } = string.Empty;

    /// <summary>
    /// 改进领域
    /// </summary>
    public string? ImprovementArea { get; set; } = string.Empty;

    /// <summary>
    /// 当前状况描述
    /// </summary>
    public string? CurrentSituation { get; set; } = string.Empty;

    /// <summary>
    /// 改进目标
    /// </summary>
    public string? ImprovementGoal { get; set; } = string.Empty;

    /// <summary>
    /// 改进措施
    /// </summary>
    public string? ImprovementActions { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 目标完成日期
    /// </summary>
    public DateTime? TargetCompletionDate { get; set; }

    /// <summary>
    /// 进度百分比（%）
    /// </summary>
    public decimal? ProgressPercentage { get; set; }

    /// <summary>
    /// 改进结果说明
    /// </summary>
    public string? ResultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 指导老师 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MentorId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    public int? ImprovementStatus { get; set; }

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
/// PerfAnalysis 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPerfAnalysisExportDto
{
    /// <summary>
    /// PerfAnalysisID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAnalysisId { get; set; }

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
    /// 关联考核评估 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }

    /// <summary>
    /// 改进计划标题
    /// </summary>
    public string PlanTitle { get; set; } = string.Empty;

    /// <summary>
    /// 改进领域
    /// </summary>
    public string ImprovementArea { get; set; } = string.Empty;

    /// <summary>
    /// 当前状况描述
    /// </summary>
    public string CurrentSituation { get; set; } = string.Empty;

    /// <summary>
    /// 改进目标
    /// </summary>
    public string ImprovementGoal { get; set; } = string.Empty;

    /// <summary>
    /// 改进措施
    /// </summary>
    public string ImprovementActions { get; set; } = string.Empty;

    /// <summary>
    /// 计划制定日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 目标完成日期
    /// </summary>
    public DateTime TargetCompletionDate { get; set; }

    /// <summary>
    /// 进度百分比（%）
    /// </summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>
    /// 改进结果说明
    /// </summary>
    public string ResultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 指导老师 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MentorId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

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
