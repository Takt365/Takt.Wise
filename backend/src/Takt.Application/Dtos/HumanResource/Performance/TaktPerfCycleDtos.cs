// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktPerfCycleDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfCycle 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPerfCycle 生成，请按需审阅）
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
// PerfCycle 响应 DTO
// ========================================

/// <summary>
/// 绩效考核周期日程安排
/// 对应前端 TaktPerfCycleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPerfCycleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PerfCycleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfCycleId { get; set; }


    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    public int CycleScheduleStatus { get; set; } = 0;

}

// ========================================
// PerfCycle 查询 DTO
// ========================================

/// <summary>
/// PerfCycle 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPerfCycleQueryDto : TaktPagedQuery
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
    /// 周期编码（租户+公司内唯一）
    /// </summary>
    public string? CycleCode { get; set; } = string.Empty;

    /// <summary>
    /// 周期名称
    /// </summary>
    public string? CycleName { get; set; } = string.Empty;

    /// <summary>
    /// 周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 周期年度
    /// </summary>
    public int? CycleYear { get; set; }

    /// <summary>
    /// 周期序号
    /// </summary>
    public int? CycleSequence { get; set; }

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
    /// 目标设定截止日期（范围查询-开始）
    /// </summary>
    public DateTime? GoalSettingDueDateStart { get; set; }

    /// <summary>
    /// 目标设定截止日期（范围查询-结束）
    /// </summary>
    public DateTime? GoalSettingDueDateEnd { get; set; }

    /// <summary>
    /// 自评截止日期（范围查询-开始）
    /// </summary>
    public DateTime? SelfEvaluationDueDateStart { get; set; }

    /// <summary>
    /// 自评截止日期（范围查询-结束）
    /// </summary>
    public DateTime? SelfEvaluationDueDateEnd { get; set; }

    /// <summary>
    /// 主管评审截止日期（范围查询-开始）
    /// </summary>
    public DateTime? SupervisorReviewDueDateStart { get; set; }

    /// <summary>
    /// 主管评审截止日期（范围查询-结束）
    /// </summary>
    public DateTime? SupervisorReviewDueDateEnd { get; set; }

    /// <summary>
    /// 面谈截止日期（范围查询-开始）
    /// </summary>
    public DateTime? InterviewDueDateStart { get; set; }

    /// <summary>
    /// 面谈截止日期（范围查询-结束）
    /// </summary>
    public DateTime? InterviewDueDateEnd { get; set; }

    /// <summary>
    /// 结果确认截止日期（范围查询-开始）
    /// </summary>
    public DateTime? ResultConfirmationDueDateStart { get; set; }

    /// <summary>
    /// 结果确认截止日期（范围查询-结束）
    /// </summary>
    public DateTime? ResultConfirmationDueDateEnd { get; set; }

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 周期说明
    /// </summary>
    public string? PerfCycleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    public int? CycleScheduleStatus { get; set; }

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
// 创建PerfCycle DTO
// ========================================

/// <summary>
/// 创建PerfCycle DTO
/// </summary>
public class TaktPerfCycleCreateDto
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
    /// 周期编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "周期编码（租户+公司内唯一）不能为空")]
    public string CycleCode { get; set; } = string.Empty;

    /// <summary>
    /// 周期名称
    /// </summary>
    [Required(ErrorMessage = "周期名称不能为空")]
    public string CycleName { get; set; } = string.Empty;

    /// <summary>
    /// 周期类型（月度/季度/半年度/年度）
    /// </summary>
    [Required(ErrorMessage = "周期类型（月度/季度/半年度/年度）不能为空")]
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 周期年度
    /// </summary>
    public int CycleYear { get; set; } = 0;

    /// <summary>
    /// 周期序号
    /// </summary>
    public int CycleSequence { get; set; } = 0;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 目标设定截止日期
    /// </summary>
    public DateTime GoalSettingDueDate { get; set; }

    /// <summary>
    /// 自评截止日期
    /// </summary>
    public DateTime SelfEvaluationDueDate { get; set; }

    /// <summary>
    /// 主管评审截止日期
    /// </summary>
    public DateTime SupervisorReviewDueDate { get; set; }

    /// <summary>
    /// 面谈截止日期
    /// </summary>
    public DateTime InterviewDueDate { get; set; }

    /// <summary>
    /// 结果确认截止日期
    /// </summary>
    public DateTime ResultConfirmationDueDate { get; set; }

    /// <summary>
    /// 适用部门
    /// </summary>
    [Required(ErrorMessage = "适用部门不能为空")]
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 周期说明
    /// </summary>
    [Required(ErrorMessage = "周期说明不能为空")]
    public string PerfCycleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    public int CycleScheduleStatus { get; set; } = 0;

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
// 更新PerfCycle DTO
// ========================================

/// <summary>
/// 更新PerfCycle DTO
/// 继承 TaktPerfCycleCreateDto，添加 PerfCycleId 字段
/// </summary>
public class TaktPerfCycleUpdateDto : TaktPerfCycleCreateDto
{
    /// <summary>
    /// PerfCycleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfCycleId { get; set; }

}

// ========================================
// PerfCycle 状态 DTO
// ========================================

/// <summary>
/// PerfCycle 状态更新 DTO
/// </summary>
public class TaktPerfCycleStatusDto
{
    /// <summary>
    /// PerfCycleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfCycleId { get; set; }

    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    [Required(ErrorMessage = "状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）不能为空")]
    public int CycleScheduleStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PerfCycle 导入模板行 DTO
/// </summary>
public class TaktPerfCycleTemplateDto
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
    /// 周期编码（租户+公司内唯一）
    /// </summary>
    public string? CycleCode { get; set; } = string.Empty;

    /// <summary>
    /// 周期名称
    /// </summary>
    public string? CycleName { get; set; } = string.Empty;

    /// <summary>
    /// 周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 周期年度
    /// </summary>
    public int? CycleYear { get; set; }

    /// <summary>
    /// 周期序号
    /// </summary>
    public int? CycleSequence { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 目标设定截止日期
    /// </summary>
    public DateTime? GoalSettingDueDate { get; set; }

    /// <summary>
    /// 自评截止日期
    /// </summary>
    public DateTime? SelfEvaluationDueDate { get; set; }

    /// <summary>
    /// 主管评审截止日期
    /// </summary>
    public DateTime? SupervisorReviewDueDate { get; set; }

    /// <summary>
    /// 面谈截止日期
    /// </summary>
    public DateTime? InterviewDueDate { get; set; }

    /// <summary>
    /// 结果确认截止日期
    /// </summary>
    public DateTime? ResultConfirmationDueDate { get; set; }

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 周期说明
    /// </summary>
    public string? PerfCycleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    public int? CycleScheduleStatus { get; set; }

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
/// PerfCycle 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPerfCycleImportDto
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
    /// 周期编码（租户+公司内唯一）
    /// </summary>
    public string? CycleCode { get; set; } = string.Empty;

    /// <summary>
    /// 周期名称
    /// </summary>
    public string? CycleName { get; set; } = string.Empty;

    /// <summary>
    /// 周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 周期年度
    /// </summary>
    public int? CycleYear { get; set; }

    /// <summary>
    /// 周期序号
    /// </summary>
    public int? CycleSequence { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 目标设定截止日期
    /// </summary>
    public DateTime? GoalSettingDueDate { get; set; }

    /// <summary>
    /// 自评截止日期
    /// </summary>
    public DateTime? SelfEvaluationDueDate { get; set; }

    /// <summary>
    /// 主管评审截止日期
    /// </summary>
    public DateTime? SupervisorReviewDueDate { get; set; }

    /// <summary>
    /// 面谈截止日期
    /// </summary>
    public DateTime? InterviewDueDate { get; set; }

    /// <summary>
    /// 结果确认截止日期
    /// </summary>
    public DateTime? ResultConfirmationDueDate { get; set; }

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 周期说明
    /// </summary>
    public string? PerfCycleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    public int? CycleScheduleStatus { get; set; }

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
/// PerfCycle 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPerfCycleExportDto
{
    /// <summary>
    /// PerfCycleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfCycleId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 周期编码（租户+公司内唯一）
    /// </summary>
    public string CycleCode { get; set; } = string.Empty;

    /// <summary>
    /// 周期名称
    /// </summary>
    public string CycleName { get; set; } = string.Empty;

    /// <summary>
    /// 周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 周期年度
    /// </summary>
    public int CycleYear { get; set; } = 0;

    /// <summary>
    /// 周期序号
    /// </summary>
    public int CycleSequence { get; set; } = 0;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 目标设定截止日期
    /// </summary>
    public DateTime GoalSettingDueDate { get; set; }

    /// <summary>
    /// 自评截止日期
    /// </summary>
    public DateTime SelfEvaluationDueDate { get; set; }

    /// <summary>
    /// 主管评审截止日期
    /// </summary>
    public DateTime SupervisorReviewDueDate { get; set; }

    /// <summary>
    /// 面谈截止日期
    /// </summary>
    public DateTime InterviewDueDate { get; set; }

    /// <summary>
    /// 结果确认截止日期
    /// </summary>
    public DateTime ResultConfirmationDueDate { get; set; }

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 周期说明
    /// </summary>
    public string PerfCycleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    public int CycleScheduleStatus { get; set; } = 0;

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
