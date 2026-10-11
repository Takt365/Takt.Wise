// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeAddressDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeAddress 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeAddress 生成，请按需审阅）
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
// EmployeeAddress 响应 DTO
// ========================================

/// <summary>
/// 员工地址（主档子表；同一员工每种地址类型至多一条）
/// 对应前端 TaktEmployeeAddressDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeAddressDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeAddressID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAddressId { get; set; }

    /// <summary>
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    public int AddressType { get; set; } = 0;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

    /// <summary>
    /// 员工主档（多对一）
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? Employee { get; set; }

}

// ========================================
// EmployeeAddress 查询 DTO
// ========================================

/// <summary>
/// EmployeeAddress 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeAddressQueryDto : TaktPagedQuery
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
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    public int? AddressType { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? Country { get; set; } = string.Empty;

    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? City { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

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
// 创建EmployeeAddress DTO
// ========================================

/// <summary>
/// 创建EmployeeAddress DTO
/// </summary>
public class TaktEmployeeAddressCreateDto
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
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    public int AddressType { get; set; } = 0;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "国家（字典 sys_country_code；DictValue=ISO alpha-2）不能为空")]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [Required(ErrorMessage = "省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）不能为空")]
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [Required(ErrorMessage = "市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）不能为空")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    [Required(ErrorMessage = "区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）不能为空")]
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    [Required(ErrorMessage = "地址1（详细地址行1）不能为空")]
    public string Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

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
// 更新EmployeeAddress DTO
// ========================================

/// <summary>
/// 更新EmployeeAddress DTO
/// 继承 TaktEmployeeAddressCreateDto，添加 EmployeeAddressId 字段
/// </summary>
public class TaktEmployeeAddressUpdateDto : TaktEmployeeAddressCreateDto
{
    /// <summary>
    /// EmployeeAddressID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAddressId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeAddress 导入模板行 DTO
/// </summary>
public class TaktEmployeeAddressTemplateDto
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
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    public int? AddressType { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? Country { get; set; } = string.Empty;

    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? City { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

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
/// EmployeeAddress 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeAddressImportDto
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
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    public int? AddressType { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? Country { get; set; } = string.Empty;

    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? Province { get; set; } = string.Empty;

    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? City { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string? District { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string? Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

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
/// EmployeeAddress 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeAddressExportDto
{
    /// <summary>
    /// EmployeeAddressID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeAddressId { get; set; }

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
    /// 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
    /// 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
    /// </summary>
    public int AddressType { get; set; } = 0;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    public string District { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    public string Address1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    public string? Address2 { get; set; } = string.Empty;

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
