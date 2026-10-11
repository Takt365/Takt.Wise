// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktAdminDivisionDtos.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：AdminDivision 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAdminDivision 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// AdminDivision 响应 DTO
// ========================================

/// <summary>
/// 行政区划实体（租户级共享；世界通用六级树） 层级：1=国家，2=州省，3=地市，4=区县，5=乡镇街道，6=行政村（字典 sys_admin_division_level） 编码可对齐 ISO 3166、ISO 3166-2、GB/T 2260、JIS 等；子节点 CountryCode 冗余自根国家便于过滤 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// 对应前端 TaktAdminDivisionDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktAdminDivisionDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// AdminDivisionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AdminDivisionId { get; set; }

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
    /// </summary>
    public string DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    public string DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（字典 sys_admin_division_level；1～6）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    public string DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（字典 sys_yes_no）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    public string PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    public int DivisionStatus { get; set; } = 0;

}

// ========================================
// AdminDivision 树形响应 DTO
// ========================================

/// <summary>
/// AdminDivision 树形列表/树选择 DTO（含子节点）
/// 对应 GetAdminDivisionTreeAsync 等接口
/// </summary>
public class TaktAdminDivisionTreeDto : TaktAdminDivisionDto
{
    /// <summary>
    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
    /// </summary>
    public List<TaktAdminDivisionTreeDto>? Children { get; set; }
}

// ========================================
// AdminDivision 查询 DTO
// ========================================

/// <summary>
/// AdminDivision 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAdminDivisionQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
    /// </summary>
    public string? DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    public string? DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 层级（字典 sys_admin_division_level；1～6）
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    public string? DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（字典 sys_yes_no）
    /// </summary>
    public int? IsLeaf { get; set; }

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    public string? PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    public int? DivisionStatus { get; set; }

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
// 创建AdminDivision DTO
// ========================================

/// <summary>
/// 创建AdminDivision DTO
/// </summary>
public class TaktAdminDivisionCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）不能为空")]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
    /// </summary>
    [Required(ErrorMessage = "区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）不能为空")]
    public string DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    [Required(ErrorMessage = "区划名称（本国语言官方/本地显示名）不能为空")]
    public string DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    [Required(ErrorMessage = "区划路径（如 /1/3/5/，用于快速查询子孙）不能为空")]
    public string DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    [Required(ErrorMessage = "电话区号（国际电话区号，如 +86、+81）不能为空")]
    public string PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    public int DivisionStatus { get; set; } = 0;

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
// 更新AdminDivision DTO
// ========================================

/// <summary>
/// 更新AdminDivision DTO
/// 继承 TaktAdminDivisionCreateDto，添加 AdminDivisionId 字段
/// </summary>
public class TaktAdminDivisionUpdateDto : TaktAdminDivisionCreateDto
{
    /// <summary>
    /// AdminDivisionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AdminDivisionId { get; set; }

}

// ========================================
// AdminDivision 状态 DTO
// ========================================

/// <summary>
/// AdminDivision 状态更新 DTO
/// </summary>
public class TaktAdminDivisionStatusDto
{
    /// <summary>
    /// AdminDivisionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AdminDivisionId { get; set; }

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "区划状态（字典 sys_normal_disable）不能为空")]
    public int DivisionStatus { get; set; } = 0;
}

// ========================================
// AdminDivision 排序 DTO
// ========================================

/// <summary>
/// AdminDivision 排序更新 DTO
/// </summary>
public class TaktAdminDivisionSortDto
{
    /// <summary>
    /// AdminDivisionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AdminDivisionId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AdminDivision 导入模板行 DTO
/// </summary>
public class TaktAdminDivisionTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
    /// </summary>
    public string? DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    public string? DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    public string? DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    public string? PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    public int? DivisionStatus { get; set; }

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
/// AdminDivision 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAdminDivisionImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
    /// </summary>
    public string? DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    public string? DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    public string? DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    public string? PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    public int? DivisionStatus { get; set; }

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
/// AdminDivision 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAdminDivisionExportDto
{
    /// <summary>
    /// AdminDivisionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AdminDivisionId { get; set; }

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
    /// </summary>
    public string DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    public string DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（字典 sys_admin_division_level；1～6）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    public string DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（字典 sys_yes_no）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    public string PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    public int DivisionStatus { get; set; } = 0;

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
