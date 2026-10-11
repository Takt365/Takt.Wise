// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Training
// 文件名称：TaktTrainingPlanDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingPlan 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTrainingPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Training;

// ========================================
// TrainingPlan 响应 DTO
// ========================================

/// <summary>
/// 培训计划（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// 对应前端 TaktTrainingPlanDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktTrainingPlanDto : TaktApprovalDtoBase
{
    /// <summary>
    /// TrainingPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int PlanYear { get; set; } = 0;

    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    public string PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int PlannedHeadcount { get; set; } = 0;

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string TrainingPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int TrainingPlanStatus { get; set; } = 0;

}

// ========================================
// TrainingPlan 查询 DTO
// ========================================

/// <summary>
/// TrainingPlan 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTrainingPlanQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int? PlanYear { get; set; }

    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    public string? PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 计划开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 计划结束日期（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 计划结束日期（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string? TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int? PlannedHeadcount { get; set; }

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal? TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? TrainingPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? TrainingPlanStatus { get; set; }

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
// 创建TrainingPlan DTO
// ========================================

/// <summary>
/// 创建TrainingPlan DTO
/// </summary>
public class TaktTrainingPlanCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "计划编码（租户+公司内唯一）不能为空")]
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    [Required(ErrorMessage = "计划名称不能为空")]
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int PlanYear { get; set; } = 0;

    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    [Required(ErrorMessage = "计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）不能为空")]
    public string PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    [Required(ErrorMessage = "适用部门不能为空")]
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    [Required(ErrorMessage = "培训目标不能为空")]
    public string TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int PlannedHeadcount { get; set; } = 0;

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    [Required(ErrorMessage = "计划说明不能为空")]
    public string TrainingPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int TrainingPlanStatus { get; set; } = 0;

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
// 更新TrainingPlan DTO
// ========================================

/// <summary>
/// 更新TrainingPlan DTO
/// 继承 TaktTrainingPlanCreateDto，添加 TrainingPlanId 字段
/// </summary>
public class TaktTrainingPlanUpdateDto : TaktTrainingPlanCreateDto
{
    /// <summary>
    /// TrainingPlanID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

}

// ========================================
// TrainingPlan 状态 DTO
// ========================================

/// <summary>
/// TrainingPlan 状态更新 DTO
/// </summary>
public class TaktTrainingPlanStatusDto
{
    /// <summary>
    /// TrainingPlanID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int TrainingPlanStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TrainingPlan 导入模板行 DTO
/// </summary>
public class TaktTrainingPlanTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int? PlanYear { get; set; }

    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    public string? PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string? TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int? PlannedHeadcount { get; set; }

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal? TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? TrainingPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? TrainingPlanStatus { get; set; }

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
/// TrainingPlan 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTrainingPlanImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int? PlanYear { get; set; }

    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    public string? PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string? TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int? PlannedHeadcount { get; set; }

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal? TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? TrainingPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? TrainingPlanStatus { get; set; }

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
/// TrainingPlan 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTrainingPlanExportDto
{
    /// <summary>
    /// TrainingPlanID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingPlanId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划名称
    /// </summary>
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 计划年度
    /// </summary>
    public int PlanYear { get; set; } = 0;

    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    public string PlanType { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 计划结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 培训目标
    /// </summary>
    public string TrainingObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 计划培训人数
    /// </summary>
    public int PlannedHeadcount { get; set; } = 0;

    /// <summary>
    /// 培训预算（元）
    /// </summary>
    public decimal TrainingBudget { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string TrainingPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int TrainingPlanStatus { get; set; } = 0;

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
