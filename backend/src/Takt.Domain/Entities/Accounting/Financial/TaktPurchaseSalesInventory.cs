// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktPurchaseSalesInventory.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：进销存表实体（对齐中国存货成本流转与 IAS 2：期初+入库−出库成本=期末；区分销售收入与结转成本）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 进销存表实体（存货数量金额账；CAS《存货》成本流转 / IAS 2 inventory movement）
/// 勾稽：期末数量/成本 = 期初 + 采购入库 + 生产入库 + 其他入库调整 − 出库成本结转；
/// 销售收入单独列示，不计入存货成本等式（避免收入与成本混淆）。
/// 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别
/// </summary>
[SugarTable("takt_accounting_financial_purchase_sales_inventory", "进销存表")]
[SugarIndex("ix_purchase_sales_inventory_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_sales_inventory_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_psi_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(Valuation), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_psi_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Desc, false)]
[SugarIndex("ix_takt_accounting_financial_psi_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktPurchaseSalesInventory : TaktCompanyEntityBase
{
    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    [SugarColumn(ColumnName = "period_code", ColumnDescription = "会计期间", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string PeriodCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options 或 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;
    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [SugarColumn(ColumnName = "valuation", ColumnDescription = "评估类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string Valuation { get; set; } = string.Empty;
    /// <summary>
    /// 计量单位
    /// </summary>
    [SugarColumn(ColumnName = "unit_code", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? UnitCode { get; set; }
    /// <summary>
    /// 期初数量
    /// </summary>
    [SugarColumn(ColumnName = "opening_qty", ColumnDescription = "期初数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal OpeningQty { get; set; } = 0;
    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    [SugarColumn(ColumnName = "opening_amount", ColumnDescription = "期初成本金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OpeningAmount { get; set; } = 0;
    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    [SugarColumn(ColumnName = "purchase_qty", ColumnDescription = "采购入库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PurchaseQty { get; set; } = 0;
    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    [SugarColumn(ColumnName = "purchase_amount", ColumnDescription = "采购入库成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PurchaseAmount { get; set; } = 0;
    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    [SugarColumn(ColumnName = "production_qty", ColumnDescription = "生产入库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ProductionQty { get; set; } = 0;
    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    [SugarColumn(ColumnName = "production_amount", ColumnDescription = "生产入库成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ProductionAmount { get; set; } = 0;
    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    [SugarColumn(ColumnName = "issue_qty", ColumnDescription = "出库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal IssueQty { get; set; } = 0;
    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    [SugarColumn(ColumnName = "issue_cost_amount", ColumnDescription = "出库结转成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal IssueCostAmount { get; set; } = 0;
    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    [SugarColumn(ColumnName = "sales_revenue_amount", ColumnDescription = "销售收入", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SalesRevenueAmount { get; set; } = 0;
    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    [SugarColumn(ColumnName = "adjust_qty", ColumnDescription = "调整数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal AdjustQty { get; set; } = 0;
    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    [SugarColumn(ColumnName = "adjust_amount", ColumnDescription = "调整成本金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AdjustAmount { get; set; } = 0;
    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    [SugarColumn(ColumnName = "closing_qty", ColumnDescription = "期末数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ClosingQty { get; set; } = 0;
    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    [SugarColumn(ColumnName = "closing_amount", ColumnDescription = "期末成本金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ClosingAmount { get; set; } = 0;
    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    [SugarColumn(ColumnName = "closing_unit_cost", ColumnDescription = "期末单位成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ClosingUnitCost { get; set; } = 0;
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [SugarColumn(ColumnName = "psi_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PsiStatus { get; set; } = 1;
}
