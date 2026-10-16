// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktTaxCodeDtos.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaxCode 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTaxCode 生成，请按需审阅）
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
// TaxCode 响应 DTO
// ========================================

/// <summary>
/// 税收代码实体（公司级；租户内公司 + 过程 + 税码 + 税类别唯一）
/// 对应前端 TaktTaxCodeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTaxCodeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TaxCodeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCodeId { get; set; }

    /// <summary>
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    public string TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一公司、过程、税类别下唯一）
    /// </summary>
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    public string TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TaxCodeStatus { get; set; } = 0;

}

// ========================================
// TaxCode 查询 DTO
// ========================================

/// <summary>
/// TaxCode 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTaxCodeQueryDto : TaktPagedQuery
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
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    public string? TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一公司、过程、税类别下唯一）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    public string? TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TaxCodeStatus { get; set; }

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
// 创建TaxCode DTO
// ========================================

/// <summary>
/// 创建TaxCode DTO
/// </summary>
public class TaktTaxCodeCreateDto
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
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    [Required(ErrorMessage = "过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）不能为空")]
    public string TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一公司、过程、税类别下唯一）
    /// </summary>
    [Required(ErrorMessage = "税码（长度 2；同一公司、过程、税类别下唯一）不能为空")]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    [Required(ErrorMessage = "税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）不能为空")]
    public string TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    [Required(ErrorMessage = "描述（税码文本，如销项增值税、进项税档位说明；Length=50）不能为空")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TaxCodeStatus { get; set; } = 0;

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
// 更新TaxCode DTO
// ========================================

/// <summary>
/// 更新TaxCode DTO
/// 继承 TaktTaxCodeCreateDto，添加 TaxCodeId 字段
/// </summary>
public class TaktTaxCodeUpdateDto : TaktTaxCodeCreateDto
{
    /// <summary>
    /// TaxCodeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCodeId { get; set; }

}

// ========================================
// TaxCode 状态 DTO
// ========================================

/// <summary>
/// TaxCode 状态更新 DTO
/// </summary>
public class TaktTaxCodeStatusDto
{
    /// <summary>
    /// TaxCodeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCodeId { get; set; }

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int TaxCodeStatus { get; set; } = 0;
}

// ========================================
// TaxCode 排序 DTO
// ========================================

/// <summary>
/// TaxCode 排序更新 DTO
/// </summary>
public class TaktTaxCodeSortDto
{
    /// <summary>
    /// TaxCodeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCodeId { get; set; }

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
/// TaxCode 导入模板行 DTO
/// </summary>
public class TaktTaxCodeTemplateDto
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
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    public string? TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一公司、过程、税类别下唯一）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    public string? TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TaxCodeStatus { get; set; }

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
/// TaxCode 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTaxCodeImportDto
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
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    public string? TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一公司、过程、税类别下唯一）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    public string? TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TaxCodeStatus { get; set; }

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
/// TaxCode 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTaxCodeExportDto
{
    /// <summary>
    /// TaxCodeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaxCodeId { get; set; }

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
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    public string TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一公司、过程、税类别下唯一）
    /// </summary>
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    public string TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TaxCodeStatus { get; set; } = 0;

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
