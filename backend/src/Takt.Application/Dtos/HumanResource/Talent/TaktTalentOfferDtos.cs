// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Talent
// 文件名称：TaktTalentOfferDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentOffer 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTalentOffer 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Talent;

// ========================================
// TalentOffer 响应 DTO
// ========================================

/// <summary>
/// 录用信息（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// 对应前端 TaktTalentOfferDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktTalentOfferDto : TaktApprovalDtoBase
{
    /// <summary>
    /// TalentOfferID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentOfferId { get; set; }

    /// <summary>
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    public string? JobPostingName { get; set; }

    /// <summary>
    /// 录用编码（租户+公司内业务编码）
    /// </summary>
    public string OfferCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（确认录用/发 offer）
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布
    /// （主表：TaktTalentJobPosting）
    /// </summary>
    public TaktTalentJobPostingDto? JobPosting { get; set; }

    /// <summary>
    /// 入职待办
    /// （子表：TaktEmployeeOnboarding）
    /// </summary>
    public List<TaktEmployeeOnboardingDto>? EmployeeOnboardings { get; set; }

}

// ========================================
// TalentOffer 查询 DTO
// ========================================

/// <summary>
/// TalentOffer 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTalentOfferQueryDto : TaktPagedQuery
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
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JobPostingId { get; set; }

    /// <summary>
    /// 录用编码（租户+公司内业务编码）
    /// </summary>
    public string? OfferCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（确认录用/发 offer）（范围查询-开始）
    /// </summary>
    public DateTime? HireDateStart { get; set; }

    /// <summary>
    /// 录用日期（确认录用/发 offer）（范围查询-结束）
    /// </summary>
    public DateTime? HireDateEnd { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
// 创建TalentOffer DTO
// ========================================

/// <summary>
/// 创建TalentOffer DTO
/// </summary>
public class TaktTalentOfferCreateDto
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
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 录用编码（租户+公司内业务编码）
    /// </summary>
    [Required(ErrorMessage = "录用编码（租户+公司内业务编码）不能为空")]
    public string OfferCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（确认录用/发 offer）
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingCreateDto>? EmployeeOnboardings { get; set; }

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
// 更新TalentOffer DTO
// ========================================

/// <summary>
/// 更新TalentOffer DTO
/// 继承 TaktTalentOfferCreateDto，添加 TalentOfferId 字段
/// </summary>
public class TaktTalentOfferUpdateDto : TaktTalentOfferCreateDto
{
    /// <summary>
    /// TalentOfferID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentOfferId { get; set; }

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeOnboardingUpdateDto>? EmployeeOnboardings { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TalentOffer 导入模板行 DTO
/// </summary>
public class TaktTalentOfferTemplateDto
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
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JobPostingId { get; set; }

    /// <summary>
    /// 录用编码（租户+公司内业务编码）
    /// </summary>
    public string? OfferCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（确认录用/发 offer）
    /// </summary>
    public DateTime? HireDate { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingCreateDto>? EmployeeOnboardings { get; set; }

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
/// TalentOffer 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTalentOfferImportDto
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
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JobPostingId { get; set; }

    /// <summary>
    /// 录用编码（租户+公司内业务编码）
    /// </summary>
    public string? OfferCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（确认录用/发 offer）
    /// </summary>
    public DateTime? HireDate { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingCreateDto>? EmployeeOnboardings { get; set; }

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
/// TalentOffer 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTalentOfferExportDto
{
    /// <summary>
    /// TalentOfferID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentOfferId { get; set; }

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
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 录用编码（租户+公司内业务编码）
    /// </summary>
    public string OfferCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（确认录用/发 offer）
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
