// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeDelegation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeDelegation 生成，请按需审阅）
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
// EmployeeDelegation 响应 DTO
// ========================================

/// <summary>
/// 员工代理关系实体 独立记录所有代理场景（部门代理、岗位代理、审批代理等）
/// 对应前端 TaktEmployeeDelegationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeDelegationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeDelegationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDelegationId { get; set; }

    /// <summary>
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string ProxyEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string ProxyEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string OriginalEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string OriginalEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    public int DelegationType { get; set; } = 0;

    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    public int ScopeType { get; set; } = 0;

    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理范围 名称（填充字段）
    /// </summary>
    public string? ScopeName { get; set; }

    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? OriginalEmployee { get; set; }

    /// <summary>
    /// 代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? ProxyEmployee { get; set; }

}

// ========================================
// EmployeeDelegation 查询 DTO
// ========================================

/// <summary>
/// EmployeeDelegation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeDelegationQueryDto : TaktPagedQuery
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
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? ProxyEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? ProxyEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? OriginalEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? OriginalEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    public int? DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    public int? ScopeType { get; set; }

    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 代理开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

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
// 创建EmployeeDelegation DTO
// ========================================

/// <summary>
/// 创建EmployeeDelegation DTO
/// </summary>
public class TaktEmployeeDelegationCreateDto
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
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string ProxyEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string ProxyEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string OriginalEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string OriginalEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    public int DelegationType { get; set; } = 0;

    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    public int ScopeType { get; set; } = 0;

    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    [Required(ErrorMessage = "代理原因（如休假、出差、培训、岗位空缺、病假等）不能为空")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）
    /// </summary>
    public DateTime? EndDate { get; set; }

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
// 更新EmployeeDelegation DTO
// ========================================

/// <summary>
/// 更新EmployeeDelegation DTO
/// 继承 TaktEmployeeDelegationCreateDto，添加 EmployeeDelegationId 字段
/// </summary>
public class TaktEmployeeDelegationUpdateDto : TaktEmployeeDelegationCreateDto
{
    /// <summary>
    /// EmployeeDelegationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDelegationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeDelegation 导入模板行 DTO
/// </summary>
public class TaktEmployeeDelegationTemplateDto
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
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? ProxyEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? ProxyEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? OriginalEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? OriginalEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    public int? DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    public int? ScopeType { get; set; }

    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）
    /// </summary>
    public DateTime? EndDate { get; set; }

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
/// EmployeeDelegation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeDelegationImportDto
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
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? ProxyEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? ProxyEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string? OriginalEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string? OriginalEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    public int? DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    public int? ScopeType { get; set; }

    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）
    /// </summary>
    public DateTime? EndDate { get; set; }

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
/// EmployeeDelegation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeDelegationExportDto
{
    /// <summary>
    /// EmployeeDelegationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDelegationId { get; set; }

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
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string ProxyEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string ProxyEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    public string OriginalEmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    public string OriginalEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    public int DelegationType { get; set; } = 0;

    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    public int ScopeType { get; set; } = 0;

    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）
    /// </summary>
    public DateTime? EndDate { get; set; }

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
