// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktPerfAssessmentDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfAssessment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPerfAssessment 生成，请按需审阅）
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
// PerfAssessment 响应 DTO
// ========================================

/// <summary>
/// 员工绩效考核
/// 对应前端 TaktPerfAssessmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPerfAssessmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PerfAssessmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAssessmentId { get; set; }


    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int AssessmentStatus { get; set; } = 0;

}

// ========================================
// PerfAssessment 查询 DTO
// ========================================

/// <summary>
/// PerfAssessment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPerfAssessmentQueryDto : TaktPagedQuery
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期（范围查询-开始）
    /// </summary>
    public DateTime? AssessmentDateStart { get; set; }

    /// <summary>
    /// 考核日期（范围查询-结束）
    /// </summary>
    public DateTime? AssessmentDateEnd { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal? SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string? SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal? SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string? SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal? FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string? PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期（范围查询-开始）
    /// </summary>
    public DateTime? InterviewDateStart { get; set; }

    /// <summary>
    /// 面谈日期（范围查询-结束）
    /// </summary>
    public DateTime? InterviewDateEnd { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string? InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int? AssessmentStatus { get; set; }

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
// 创建PerfAssessment DTO
// ========================================

/// <summary>
/// 创建PerfAssessment DTO
/// </summary>
public class TaktPerfAssessmentCreateDto
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    [Required(ErrorMessage = "考核周期（如 2026-Q1、2026-Annual）不能为空")]
    public string AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    [Required(ErrorMessage = "自评说明不能为空")]
    public string SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    [Required(ErrorMessage = "主管评语不能为空")]
    public string SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    [Required(ErrorMessage = "绩效等级（A/B/C/D/E）不能为空")]
    public string PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    [Required(ErrorMessage = "面谈记录不能为空")]
    public string InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int AssessmentStatus { get; set; } = 0;

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
// 更新PerfAssessment DTO
// ========================================

/// <summary>
/// 更新PerfAssessment DTO
/// 继承 TaktPerfAssessmentCreateDto，添加 PerfAssessmentId 字段
/// </summary>
public class TaktPerfAssessmentUpdateDto : TaktPerfAssessmentCreateDto
{
    /// <summary>
    /// PerfAssessmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAssessmentId { get; set; }

}

// ========================================
// PerfAssessment 状态 DTO
// ========================================

/// <summary>
/// PerfAssessment 状态更新 DTO
/// </summary>
public class TaktPerfAssessmentStatusDto
{
    /// <summary>
    /// PerfAssessmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAssessmentId { get; set; }

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    [Required(ErrorMessage = "状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）不能为空")]
    public int AssessmentStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PerfAssessment 导入模板行 DTO
/// </summary>
public class TaktPerfAssessmentTemplateDto
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime? AssessmentDate { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal? SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string? SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal? SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string? SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal? FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string? PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime? InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string? InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int? AssessmentStatus { get; set; }

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
/// PerfAssessment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPerfAssessmentImportDto
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string? AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime? AssessmentDate { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal? SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string? SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal? SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string? SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal? FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string? PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime? InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string? InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int? AssessmentStatus { get; set; }

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
/// PerfAssessment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPerfAssessmentExportDto
{
    /// <summary>
    /// PerfAssessmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfAssessmentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    public string AssessmentPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 考核日期
    /// </summary>
    public DateTime AssessmentDate { get; set; }

    /// <summary>
    /// 方案指标 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }

    /// <summary>
    /// 自评分数
    /// </summary>
    public decimal SelfScore { get; set; }

    /// <summary>
    /// 自评说明
    /// </summary>
    public string SelfEvaluationNotes { get; set; } = string.Empty;

    /// <summary>
    /// 主管评分
    /// </summary>
    public decimal SupervisorScore { get; set; }

    /// <summary>
    /// 主管评语
    /// </summary>
    public string SupervisorComments { get; set; } = string.Empty;

    /// <summary>
    /// 综合得分
    /// </summary>
    public decimal FinalScore { get; set; }

    /// <summary>
    /// 绩效等级（A/B/C/D/E）
    /// </summary>
    public string PerformanceGrade { get; set; } = string.Empty;

    /// <summary>
    /// 评审人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReviewerId { get; set; }

    /// <summary>
    /// 面谈日期
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面谈记录
    /// </summary>
    public string InterviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    public int AssessmentStatus { get; set; } = 0;

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
