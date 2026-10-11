// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseForecastItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseForecastItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseForecastItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// PurchaseForecastItem 响应 DTO
// ========================================

/// <summary>
/// Takt采购预测明细（一行 = 主表物料在某财年某月的 001/002 计划量；产品/类别/利润中心/机种/物料在主表）
/// 对应前端 TaktPurchaseForecastItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseForecastItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseForecastItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastItemId { get; set; }

    /// <summary>
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测名称（填充字段）
    /// </summary>
    public string? PurchaseForecastName { get; set; }

    /// <summary>
    /// 采购预测编码（冗余字段，便于查询）
    /// </summary>
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    public int PlanMonth { get; set; } = 0;

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    public decimal PlanQuantity001 { get; set; }

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    public decimal PlanQuantity002 { get; set; }

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    public decimal PlanQuantityDelta { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// PurchaseForecastItem 查询 DTO
// ========================================

/// <summary>
/// PurchaseForecastItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseForecastItemQueryDto : TaktPagedQuery
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
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    public int? PlanMonth { get; set; }

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    public decimal? PlanQuantity001 { get; set; }

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    public decimal? PlanQuantity002 { get; set; }

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    public decimal? PlanQuantityDelta { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建PurchaseForecastItem DTO
// ========================================

/// <summary>
/// 创建PurchaseForecastItem DTO
/// </summary>
public class TaktPurchaseForecastItemCreateDto
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
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（冗余字段，便于查询）
    /// </summary>
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    [Required(ErrorMessage = "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）不能为空")]
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    public int PlanMonth { get; set; } = 0;

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    public decimal PlanQuantity001 { get; set; }

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    public decimal PlanQuantity002 { get; set; }

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    public decimal PlanQuantityDelta { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新PurchaseForecastItem DTO
// ========================================

/// <summary>
/// 更新PurchaseForecastItem DTO
/// 继承 TaktPurchaseForecastItemCreateDto，添加 PurchaseForecastItemId 字段
/// </summary>
public class TaktPurchaseForecastItemUpdateDto : TaktPurchaseForecastItemCreateDto
{
    /// <summary>
    /// PurchaseForecastItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastItemId { get; set; }

}

// ========================================
// PurchaseForecastItem 作废 DTO
// ========================================

/// <summary>
/// PurchaseForecastItem 作废/撤销作废 DTO
/// </summary>
public class TaktPurchaseForecastItemObsoleteDto
{
    /// <summary>
    /// PurchaseForecastItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseForecastItem 导入模板行 DTO
/// </summary>
public class TaktPurchaseForecastItemTemplateDto
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
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    public int? PlanMonth { get; set; }

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    public decimal? PlanQuantity001 { get; set; }

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    public decimal? PlanQuantity002 { get; set; }

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    public decimal? PlanQuantityDelta { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// PurchaseForecastItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseForecastItemImportDto
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
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    public int? PlanMonth { get; set; }

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    public decimal? PlanQuantity001 { get; set; }

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    public decimal? PlanQuantity002 { get; set; }

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    public decimal? PlanQuantityDelta { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// PurchaseForecastItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseForecastItemExportDto
{
    /// <summary>
    /// PurchaseForecastItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastItemId { get; set; }

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
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（冗余字段，便于查询）
    /// </summary>
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    public int PlanMonth { get; set; } = 0;

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    public decimal PlanQuantity001 { get; set; }

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    public decimal PlanQuantity002 { get; set; }

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    public decimal PlanQuantityDelta { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
