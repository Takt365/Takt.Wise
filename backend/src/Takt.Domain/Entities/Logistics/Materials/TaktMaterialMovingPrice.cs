// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialMovingPrice.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格实体（按工厂/物料/评估类别/评估期间维护库存与移动平均价）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// 移动价格实体
/// <para>业务唯一键（新增/更新匹配）：TenantCode+CompanyCode+PlantCode+MaterialCode+ValuationPeriod；Valuation 为业务字段，不参与唯一匹配。</para>
/// </summary>
[SugarTable("takt_logistics_materials_material_moving_price", "移动价格表")]
[SugarIndex("ix_material_moving_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_moving_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_material_moving_price_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(ValuationPeriod), OrderByType.Asc, true)]
[SugarIndex("ix_material_moving_price_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_moving_price_valuation_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ValuationPeriod), OrderByType.Asc, false)]
public class TaktMaterialMovingPrice : TaktCompanyEntityBase
{

    /// <summary>
    /// 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "valuation_period", ColumnDescription = "评估期间", ColumnDataType = "nvarchar", Length = 7, IsNullable = false)]
    public string ValuationPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [SugarColumn(ColumnName = "valuation", ColumnDescription = "评估类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "stock_quantity", ColumnDescription = "库存数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StockQuantity { get; set; } = 0;

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "stock_amount", ColumnDescription = "库存金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StockAmount { get; set; } = 0;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    [SugarColumn(ColumnName = "price_control", ColumnDescription = "价格控制", ColumnDataType = "nvarchar", Length = 1, IsNullable = false, DefaultValue = "V")]
    public string PriceControl { get; set; } = "V";

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    [SugarColumn(ColumnName = "moving_price", ColumnDescription = "移动价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal MovingPrice { get; set; } = 0;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "price_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PriceUnit { get; set; } = 1000;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
}
