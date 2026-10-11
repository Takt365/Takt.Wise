// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购信息记录行实体（定价记录条件行：等级/价格/舍入）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购信息记录行实体（定价记录条件行；主子表：TaktPurchaseInfoRecord → Items → ScaleQuantities / ScaleValues）
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_info_record_item", "采购信息记录行表")]
[SugarIndex("ix_purchase_info_record_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_info_record_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_info_record_item_seq_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseInfoRecordId), OrderByType.Asc, nameof(PurchaseInfoRecordSeq), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_info_record_item_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseInfoRecordCode), OrderByType.Asc, false)]
public class TaktPurchaseInfoRecordItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购价格 ID（主子表关系；选项 TaktPurchaseInfoRecords/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_info_record_id", ColumnDescription = "采购价格ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInfoRecordId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_info_record_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_info_record_seq", ColumnDescription = "定价序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int PurchaseInfoRecordSeq { get; set; } = 10;

    /// <summary>
    /// 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    [SugarColumn(ColumnName = "price_type", ColumnDescription = "条件类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "PB00")]
    public string PriceType { get; set; } = "PB00";

    /// <summary>
    /// 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    [SugarColumn(ColumnName = "scale_type", ColumnDescription = "等级类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ScaleType { get; set; }

    /// <summary>
    /// 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    [SugarColumn(ColumnName = "scale_basis", ColumnDescription = "等级基础", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ScaleBasis { get; set; }

    /// <summary>
    /// 等级数量
    /// </summary>
    [SugarColumn(ColumnName = "scale_quantity", ColumnDescription = "等级数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ScaleQuantity { get; set; } = 0;

    /// <summary>
    /// 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    [SugarColumn(ColumnName = "scale_unit", ColumnDescription = "等级单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = true)]
    public string? ScaleUnit { get; set; }

    /// <summary>
    /// 等级值
    /// </summary>
    [SugarColumn(ColumnName = "scale_value", ColumnDescription = "等级值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ScaleValue { get; set; } = 0;

    /// <summary>
    /// 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "scale_currency_code", ColumnDescription = "等级货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? ScaleCurrencyCode { get; set; }

    /// <summary>
    /// 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
    /// </summary>
    [SugarColumn(ColumnName = "calculation_type", ColumnDescription = "计算类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = false, DefaultValue = "A")]
    public string CalculationType { get; set; } = "A";

    /// <summary>
    /// 价格
    /// </summary>
    [SugarColumn(ColumnName = "price", ColumnDescription = "价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal Price { get; set; } = 0;

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    [SugarColumn(ColumnName = "untaxed_price", ColumnDescription = "未税价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UntaxedPrice { get; set; } = 0;

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    [SugarColumn(ColumnName = "tax_included_price", ColumnDescription = "含税价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxIncludedPrice { get; set; } = 0;
    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    [SugarColumn(ColumnName = "condition_currency_code", ColumnDescription = "条件货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string ConditionCurrencyCode { get; set; } = "CNY";

    /// <summary>
    /// 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "price_unit", ColumnDescription = "定价单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PriceUnit { get; set; } = 1000;

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "unit_of_measure", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = false, DefaultValue = "PC")]
    public string UnitOfMeasure { get; set; } = "PC";

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    [SugarColumn(ColumnName = "min_order_quantity", ColumnDescription = "最小起订量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    [SugarColumn(ColumnName = "rounding_value", ColumnDescription = "舍入值", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    [SugarColumn(ColumnName = "planned_delivery_time_days", ColumnDescription = "计划交货时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 数量等级行列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInfoRecordScaleQuantity.PurchaseInfoRecordItemId))]
    public List<TaktPurchaseInfoRecordScaleQuantity>? ScaleQuantities { get; set; }

    /// <summary>
    /// 价值等级行列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInfoRecordScaleValue.PurchaseInfoRecordItemId))]
    public List<TaktPurchaseInfoRecordScaleValue>? ScaleValues { get; set; }
}
