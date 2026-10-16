// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktTaxConditionDtos.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaxCondition 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTaxCondition 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// TaxCondition 响应 DTO
// ========================================

/// <summary>
/// 税务条件实体（公司级；租户内公司 + 应用程序 + 国家 + 税码 + 条件记录号唯一）。应用程序用字典 accounting_financial_application，条件类型用字典 accounting_financial_condition_type（ExtValue=应用程序）。
/// 对应前端 TaktTaxConditionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTaxConditionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TaxConditionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxConditionId { get; set; }

    /// <summary>
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    public string ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    public string ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10）
    /// </summary>
    public string ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TaxConditionStatus { get; set; } = 0;

}

// ========================================
// TaxCondition 查询 DTO
// ========================================

/// <summary>
/// TaxCondition 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTaxConditionQueryDto : TaktPagedQuery
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
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    public string? ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    public string? ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10）
    /// </summary>
    public string? ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TaxConditionStatus { get; set; }

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
// 创建TaxCondition DTO
// ========================================

/// <summary>
/// 创建TaxCondition DTO
/// </summary>
public class TaktTaxConditionCreateDto
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
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    [Required(ErrorMessage = "应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）不能为空")]
    public string ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    [Required(ErrorMessage = "条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）不能为空")]
    public string ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "国家（字典 sys_country_code；DictValue=ISO alpha-2）不能为空")]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [Required(ErrorMessage = "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）不能为空")]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10）
    /// </summary>
    [Required(ErrorMessage = "条件记录号（长度 10）不能为空")]
    public string ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TaxConditionStatus { get; set; } = 0;

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
// 更新TaxCondition DTO
// ========================================

/// <summary>
/// 更新TaxCondition DTO
/// 继承 TaktTaxConditionCreateDto，添加 TaxConditionId 字段
/// </summary>
public class TaktTaxConditionUpdateDto : TaktTaxConditionCreateDto
{
    /// <summary>
    /// TaxConditionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxConditionId { get; set; }

}

// ========================================
// TaxCondition 状态 DTO
// ========================================

/// <summary>
/// TaxCondition 状态更新 DTO
/// </summary>
public class TaktTaxConditionStatusDto
{
    /// <summary>
    /// TaxConditionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxConditionId { get; set; }

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int TaxConditionStatus { get; set; } = 0;
}

// ========================================
// TaxCondition 排序 DTO
// ========================================

/// <summary>
/// TaxCondition 排序更新 DTO
/// </summary>
public class TaktTaxConditionSortDto
{
    /// <summary>
    /// TaxConditionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxConditionId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TaxCondition 导入模板行 DTO
/// </summary>
public class TaktTaxConditionTemplateDto
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
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    public string? ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    public string? ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10）
    /// </summary>
    public string? ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TaxConditionStatus { get; set; }

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
/// TaxCondition 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTaxConditionImportDto
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
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    public string? ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    public string? ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10）
    /// </summary>
    public string? ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TaxConditionStatus { get; set; }

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
/// TaxCondition 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTaxConditionExportDto
{
    /// <summary>
    /// TaxConditionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxConditionId { get; set; }

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
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    public string ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    public string ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10）
    /// </summary>
    public string ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TaxConditionStatus { get; set; } = 0;

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
