// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeOnboarding 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeOnboarding 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Personnel;

// ========================================
// EmployeeOnboarding 响应 DTO
// ========================================

/// <summary>
/// 入职待办（办理待办单，非审批单；状态见 TodoStatus）
/// 对应前端 TaktEmployeeOnboardingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeOnboardingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeOnboardingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingId { get; set; }

    /// <summary>
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OfferId { get; set; }

    /// <summary>
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    public string? OfferName { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    public string TodoCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    public string? EmployeeJoinedName { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    public int TodoStatus { get; set; } = 0;

    /// <summary>
    /// 员工主档（多对一；建档回填后可有值）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? Employee { get; set; }

    /// <summary>
    /// 录用信息
    /// （主表：TaktTalentOffer）
    /// </summary>
    public TaktTalentOfferDto? Offer { get; set; }

    /// <summary>
    /// 入职上岗单
    /// （主表：TaktEmployeeJoined）
    /// </summary>
    public TaktEmployeeJoinedDto? EmployeeJoined { get; set; }

}

// ========================================
// EmployeeOnboarding 查询 DTO
// ========================================

/// <summary>
/// EmployeeOnboarding 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeOnboardingQueryDto : TaktPagedQuery
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
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    public string? TodoCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）（范围查询-开始）
    /// </summary>
    public DateTime? PlannedJoinedDateStart { get; set; }

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）（范围查询-结束）
    /// </summary>
    public DateTime? PlannedJoinedDateEnd { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    public int? TodoStatus { get; set; }

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
// 创建EmployeeOnboarding DTO
// ========================================

/// <summary>
/// 创建EmployeeOnboarding DTO
/// </summary>
public class TaktEmployeeOnboardingCreateDto
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
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    [Required(ErrorMessage = "待办单号（租户+公司内业务编码）不能为空")]
    public string TodoCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    [Required(ErrorMessage = "候选人姓名（快照）不能为空")]
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    public int TodoStatus { get; set; } = 0;

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
// 更新EmployeeOnboarding DTO
// ========================================

/// <summary>
/// 更新EmployeeOnboarding DTO
/// 继承 TaktEmployeeOnboardingCreateDto，添加 EmployeeOnboardingId 字段
/// </summary>
public class TaktEmployeeOnboardingUpdateDto : TaktEmployeeOnboardingCreateDto
{
    /// <summary>
    /// EmployeeOnboardingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingId { get; set; }

}

// ========================================
// EmployeeOnboarding 状态 DTO
// ========================================

/// <summary>
/// EmployeeOnboarding 状态更新 DTO
/// </summary>
public class TaktEmployeeOnboardingStatusDto
{
    /// <summary>
    /// EmployeeOnboardingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingId { get; set; }

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    [Required(ErrorMessage = "待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）不能为空")]
    public int TodoStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeOnboarding 导入模板行 DTO
/// </summary>
public class TaktEmployeeOnboardingTemplateDto
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
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    public string? TodoCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime? PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    public int? TodoStatus { get; set; }

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
/// EmployeeOnboarding 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeOnboardingImportDto
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
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    public string? TodoCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime? PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    public int? TodoStatus { get; set; }

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
/// EmployeeOnboarding 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeOnboardingExportDto
{
    /// <summary>
    /// EmployeeOnboardingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingId { get; set; }

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
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    public string TodoCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    public int TodoStatus { get; set; } = 0;

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
