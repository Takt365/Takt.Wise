// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceDtos.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialMovingPrice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialMovingPrice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// MaterialMovingPrice 响应 DTO
// ========================================

/// <summary>
/// 移动价格实体 <para>业务唯一键（新增/更新匹配）：TenantCode+CompanyCode+PlantCode+MaterialCode+ValuationPeriod；Valuation 为业务字段，不参与唯一匹配。</para>
/// 对应前端 TaktMaterialMovingPriceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialMovingPriceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialMovingPriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialMovingPriceId { get; set; }

    /// <summary>
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    public string ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

}

// ========================================
// MaterialMovingPrice 查询 DTO
// ========================================

/// <summary>
/// MaterialMovingPrice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialMovingPriceQueryDto : TaktPagedQuery
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
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    public string? ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal? StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

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
// 创建MaterialMovingPrice DTO
// ========================================

/// <summary>
/// 创建MaterialMovingPrice DTO
/// </summary>
public class TaktMaterialMovingPriceCreateDto
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
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    [Required(ErrorMessage = "评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）不能为空")]
    public string ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [Required(ErrorMessage = "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）不能为空")]
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    [Required(ErrorMessage = "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）不能为空")]
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

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
// 更新MaterialMovingPrice DTO
// ========================================

/// <summary>
/// 更新MaterialMovingPrice DTO
/// 继承 TaktMaterialMovingPriceCreateDto，添加 MaterialMovingPriceId 字段
/// </summary>
public class TaktMaterialMovingPriceUpdateDto : TaktMaterialMovingPriceCreateDto
{
    /// <summary>
    /// MaterialMovingPriceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialMovingPriceId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialMovingPrice 导入模板行 DTO
/// </summary>
public class TaktMaterialMovingPriceTemplateDto
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
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    public string? ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal? StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

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
/// MaterialMovingPrice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialMovingPriceImportDto
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
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    public string? ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal? StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

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
/// MaterialMovingPrice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialMovingPriceExportDto
{
    /// <summary>
    /// MaterialMovingPriceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialMovingPriceId { get; set; }

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
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    public string ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

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
