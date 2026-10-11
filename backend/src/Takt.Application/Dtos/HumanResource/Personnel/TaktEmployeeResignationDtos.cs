// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeResignationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeResignation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeResignation 生成，请按需审阅）
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
// EmployeeResignation 响应 DTO
// ========================================

/// <summary>
/// 员工离职办理记录（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// 对应前端 TaktEmployeeResignationDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktEmployeeResignationDto : TaktApprovalDtoBase
{
    /// <summary>
    /// EmployeeResignationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeResignationId { get; set; }

    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
    /// </summary>
    public int ResignationType { get; set; } = 0;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? ApplyDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 实际离职日期
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 工作交接说明
    /// </summary>
    public string? HandoverNotes { get; set; } = string.Empty;

    /// <summary>
    /// 员工主档（多对一）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? Employee { get; set; }

}

// ========================================
// EmployeeResignation 查询 DTO
// ========================================

/// <summary>
/// EmployeeResignation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeResignationQueryDto : TaktPagedQuery
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 申请日期（范围查询-开始）
    /// </summary>
    public DateTime? ApplyDateStart { get; set; }

    /// <summary>
    /// 申请日期（范围查询-结束）
    /// </summary>
    public DateTime? ApplyDateEnd { get; set; }

    /// <summary>
    /// 最后工作日（范围查询-开始）
    /// </summary>
    public DateTime? LastWorkDateStart { get; set; }

    /// <summary>
    /// 最后工作日（范围查询-结束）
    /// </summary>
    public DateTime? LastWorkDateEnd { get; set; }

    /// <summary>
    /// 实际离职日期（范围查询-开始）
    /// </summary>
    public DateTime? TerminationDateStart { get; set; }

    /// <summary>
    /// 实际离职日期（范围查询-结束）
    /// </summary>
    public DateTime? TerminationDateEnd { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 工作交接说明
    /// </summary>
    public string? HandoverNotes { get; set; } = string.Empty;

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
// 创建EmployeeResignation DTO
// ========================================

/// <summary>
/// 创建EmployeeResignation DTO
/// </summary>
public class TaktEmployeeResignationCreateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
    /// </summary>
    public int ResignationType { get; set; } = 0;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? ApplyDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 实际离职日期
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 工作交接说明
    /// </summary>
    public string? HandoverNotes { get; set; } = string.Empty;

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
// 更新EmployeeResignation DTO
// ========================================

/// <summary>
/// 更新EmployeeResignation DTO
/// 继承 TaktEmployeeResignationCreateDto，添加 EmployeeResignationId 字段
/// </summary>
public class TaktEmployeeResignationUpdateDto : TaktEmployeeResignationCreateDto
{
    /// <summary>
    /// EmployeeResignationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeResignationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeResignation 导入模板行 DTO
/// </summary>
public class TaktEmployeeResignationTemplateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? ApplyDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 实际离职日期
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 工作交接说明
    /// </summary>
    public string? HandoverNotes { get; set; } = string.Empty;

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
/// EmployeeResignation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeResignationImportDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? ApplyDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 实际离职日期
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 工作交接说明
    /// </summary>
    public string? HandoverNotes { get; set; } = string.Empty;

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
/// EmployeeResignation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeResignationExportDto
{
    /// <summary>
    /// EmployeeResignationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeResignationId { get; set; }

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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
    /// </summary>
    public int ResignationType { get; set; } = 0;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? ApplyDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 实际离职日期
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 工作交接说明
    /// </summary>
    public string? HandoverNotes { get; set; } = string.Empty;

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
