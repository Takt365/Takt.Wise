// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeFamilyDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeFamily 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeFamily 生成，请按需审阅）
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
// EmployeeFamily 响应 DTO
// ========================================

/// <summary>
/// 员工家庭成员
/// 对应前端 TaktEmployeeFamilyDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeFamilyDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeFamilyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeFamilyId { get; set; }

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
    /// 成员姓名
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    public int RelationType { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 工作单位
    /// </summary>
    public string? WorkUnit { get; set; } = string.Empty;

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsEmergencyContact { get; set; } = 0;

    /// <summary>
    /// 员工主档（多对一）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? Employee { get; set; }

}

// ========================================
// EmployeeFamily 查询 DTO
// ========================================

/// <summary>
/// EmployeeFamily 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeFamilyQueryDto : TaktPagedQuery
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
    /// 成员姓名
    /// </summary>
    public string? MemberName { get; set; } = string.Empty;

    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    public int? RelationType { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 工作单位
    /// </summary>
    public string? WorkUnit { get; set; } = string.Empty;

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期（范围查询-开始）
    /// </summary>
    public DateTime? BirthDateStart { get; set; }

    /// <summary>
    /// 出生日期（范围查询-结束）
    /// </summary>
    public DateTime? BirthDateEnd { get; set; }

    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsEmergencyContact { get; set; }

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
// 创建EmployeeFamily DTO
// ========================================

/// <summary>
/// 创建EmployeeFamily DTO
/// </summary>
public class TaktEmployeeFamilyCreateDto
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
    /// 成员姓名
    /// </summary>
    [Required(ErrorMessage = "成员姓名不能为空")]
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    public int RelationType { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 工作单位
    /// </summary>
    public string? WorkUnit { get; set; } = string.Empty;

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsEmergencyContact { get; set; } = 0;

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
// 更新EmployeeFamily DTO
// ========================================

/// <summary>
/// 更新EmployeeFamily DTO
/// 继承 TaktEmployeeFamilyCreateDto，添加 EmployeeFamilyId 字段
/// </summary>
public class TaktEmployeeFamilyUpdateDto : TaktEmployeeFamilyCreateDto
{
    /// <summary>
    /// EmployeeFamilyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeFamilyId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeFamily 导入模板行 DTO
/// </summary>
public class TaktEmployeeFamilyTemplateDto
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
    /// 成员姓名
    /// </summary>
    public string? MemberName { get; set; } = string.Empty;

    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    public int? RelationType { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 工作单位
    /// </summary>
    public string? WorkUnit { get; set; } = string.Empty;

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsEmergencyContact { get; set; }

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
/// EmployeeFamily 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeFamilyImportDto
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
    /// 成员姓名
    /// </summary>
    public string? MemberName { get; set; } = string.Empty;

    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    public int? RelationType { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 工作单位
    /// </summary>
    public string? WorkUnit { get; set; } = string.Empty;

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsEmergencyContact { get; set; }

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
/// EmployeeFamily 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeFamilyExportDto
{
    /// <summary>
    /// EmployeeFamilyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeFamilyId { get; set; }

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
    /// 成员姓名
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    public int RelationType { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 工作单位
    /// </summary>
    public string? WorkUnit { get; set; } = string.Empty;

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsEmergencyContact { get; set; } = 0;

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
