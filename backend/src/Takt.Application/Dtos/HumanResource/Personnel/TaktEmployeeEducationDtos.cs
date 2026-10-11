// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeEducationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeEducation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeEducation 生成，请按需审阅）
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
// EmployeeEducation 响应 DTO
// ========================================

/// <summary>
/// 员工教育经历
/// 对应前端 TaktEmployeeEducationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeEducationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeEducationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeEducationId { get; set; }

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
    /// 学校名称
    /// </summary>
    public string SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationLevel { get; set; }

    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    public int? DegreeLevel { get; set; }

    /// <summary>
    /// 专业名称
    /// </summary>
    public string? MajorName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsHighest { get; set; } = 0;

    /// <summary>
    /// 员工主档（多对一）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? Employee { get; set; }

}

// ========================================
// EmployeeEducation 查询 DTO
// ========================================

/// <summary>
/// EmployeeEducation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeEducationQueryDto : TaktPagedQuery
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
    /// 学校名称
    /// </summary>
    public string? SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationLevel { get; set; }

    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    public int? DegreeLevel { get; set; }

    /// <summary>
    /// 专业名称
    /// </summary>
    public string? MajorName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

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
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsHighest { get; set; }

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
// 创建EmployeeEducation DTO
// ========================================

/// <summary>
/// 创建EmployeeEducation DTO
/// </summary>
public class TaktEmployeeEducationCreateDto
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
    /// 学校名称
    /// </summary>
    [Required(ErrorMessage = "学校名称不能为空")]
    public string SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationLevel { get; set; }

    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    public int? DegreeLevel { get; set; }

    /// <summary>
    /// 专业名称
    /// </summary>
    public string? MajorName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsHighest { get; set; } = 0;

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
// 更新EmployeeEducation DTO
// ========================================

/// <summary>
/// 更新EmployeeEducation DTO
/// 继承 TaktEmployeeEducationCreateDto，添加 EmployeeEducationId 字段
/// </summary>
public class TaktEmployeeEducationUpdateDto : TaktEmployeeEducationCreateDto
{
    /// <summary>
    /// EmployeeEducationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeEducationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeEducation 导入模板行 DTO
/// </summary>
public class TaktEmployeeEducationTemplateDto
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
    /// 学校名称
    /// </summary>
    public string? SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationLevel { get; set; }

    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    public int? DegreeLevel { get; set; }

    /// <summary>
    /// 专业名称
    /// </summary>
    public string? MajorName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsHighest { get; set; }

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
/// EmployeeEducation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeEducationImportDto
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
    /// 学校名称
    /// </summary>
    public string? SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationLevel { get; set; }

    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    public int? DegreeLevel { get; set; }

    /// <summary>
    /// 专业名称
    /// </summary>
    public string? MajorName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsHighest { get; set; }

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
/// EmployeeEducation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeEducationExportDto
{
    /// <summary>
    /// EmployeeEducationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeEducationId { get; set; }

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
    /// 学校名称
    /// </summary>
    public string SchoolName { get; set; } = string.Empty;

    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationLevel { get; set; }

    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    public int? DegreeLevel { get; set; }

    /// <summary>
    /// 专业名称
    /// </summary>
    public string? MajorName { get; set; } = string.Empty;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsHighest { get; set; } = 0;

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
