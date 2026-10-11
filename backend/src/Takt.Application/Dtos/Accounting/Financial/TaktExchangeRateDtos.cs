// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktExchangeRateDtos.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：ExchangeRate 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktExchangeRate 生成，请按需审阅）
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
// ExchangeRate 响应 DTO
// ========================================

/// <summary>
/// 汇率实体（租户级主数据；租户内各公司共用同一套汇率；维护自币种至目标币种的折算汇率及生效区间）
/// 对应前端 TaktExchangeRateDto
/// 继承 TaktTenantCoreDtoBase（组合 4）
/// </summary>
public class TaktExchangeRateDto : TaktTenantCoreDtoBase
{

    /// <summary>
    /// ExchangeRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExchangeRateId { get; set; }

    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    public string FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    public string ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    public string ExchangeRateType { get; set; } = string.Empty;

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    public int RatioFrom { get; set; } = 0;

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    public int RatioTo { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int ExchangeRateStatus { get; set; } = 0;

}

// ========================================
// ExchangeRate 查询 DTO
// ========================================

/// <summary>
/// ExchangeRate 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktExchangeRateQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    public string? FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    public string? ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    public string? ExchangeRateType { get; set; } = string.Empty;

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    public int? RatioFrom { get; set; }

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    public int? RatioTo { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? ExchangeRateStatus { get; set; }

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
// 创建ExchangeRate DTO
// ========================================

/// <summary>
/// 创建ExchangeRate DTO
/// </summary>
public class TaktExchangeRateCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    [Required(ErrorMessage = "源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）不能为空")]
    public string FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    [Required(ErrorMessage = "目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）不能为空")]
    public string ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    [Required(ErrorMessage = "汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）不能为空")]
    public string ExchangeRateType { get; set; } = string.Empty;

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    public int RatioFrom { get; set; } = 0;

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    public int RatioTo { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int ExchangeRateStatus { get; set; } = 0;

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
// 更新ExchangeRate DTO
// ========================================

/// <summary>
/// 更新ExchangeRate DTO
/// 继承 TaktExchangeRateCreateDto，添加 ExchangeRateId 字段
/// </summary>
public class TaktExchangeRateUpdateDto : TaktExchangeRateCreateDto
{
    /// <summary>
    /// ExchangeRateID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExchangeRateId { get; set; }

}

// ========================================
// ExchangeRate 状态 DTO
// ========================================

/// <summary>
/// ExchangeRate 状态更新 DTO
/// </summary>
public class TaktExchangeRateStatusDto
{
    /// <summary>
    /// ExchangeRateID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExchangeRateId { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "汇率状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int ExchangeRateStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ExchangeRate 导入模板行 DTO
/// </summary>
public class TaktExchangeRateTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    public string? FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    public string? ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    public string? ExchangeRateType { get; set; } = string.Empty;

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    public int? RatioFrom { get; set; }

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    public int? RatioTo { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? ExchangeRateStatus { get; set; }

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
/// ExchangeRate 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktExchangeRateImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    public string? FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    public string? ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    public string? ExchangeRateType { get; set; } = string.Empty;

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    public int? RatioFrom { get; set; }

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    public int? RatioTo { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? ExchangeRateStatus { get; set; }

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
/// ExchangeRate 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktExchangeRateExportDto
{
    /// <summary>
    /// ExchangeRateID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExchangeRateId { get; set; }

    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    public string FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    public string ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    public string ExchangeRateType { get; set; } = string.Empty;

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    public int RatioFrom { get; set; } = 0;

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    public int RatioTo { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int ExchangeRateStatus { get; set; } = 0;

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
