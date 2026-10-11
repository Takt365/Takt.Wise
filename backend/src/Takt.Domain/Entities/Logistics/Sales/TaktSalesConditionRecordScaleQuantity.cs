// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesConditionRecordScaleQuantity.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售阶梯数量实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售阶梯数量实体（主子表：TaktSalesConditionRecordItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
/// </summary>
[SugarTable("takt_logistics_sales_condition_record_scale_quantity", "销售阶梯数量表")]
[SugarIndex("ix_sales_condition_record_scale_quantity_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_condition_record_scale_quantity_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_condition_record_scale_quantity_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesConditionRecordItemId), OrderByType.Asc, nameof(SalesConditionRecordCode), OrderByType.Asc, nameof(SalesConditionRecordSeq), OrderByType.Asc, nameof(SalesScaleSeq), OrderByType.Asc, nameof(ScaleQuantity), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_condition_record_scale_quantity_code_seq", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesConditionRecordCode), OrderByType.Asc, nameof(SalesConditionRecordSeq), OrderByType.Asc, false)]
public class TaktSalesConditionRecordScaleQuantity : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sales_condition_record_item_id", ColumnDescription = "销售条件记录行ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesConditionRecordItemId { get; set; }

    /// <summary>
    /// 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "sales_condition_record_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "sales_condition_record_seq", ColumnDescription = "定价序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int SalesConditionRecordSeq { get; set; } = 10;

    /// <summary>
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "sales_scale_seq", ColumnDescription = "等级序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int SalesScaleSeq { get; set; } = 10;

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    [SugarColumn(ColumnName = "scale_quantity", ColumnDescription = "等级数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ScaleQuantity { get; set; } = 0;

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
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
