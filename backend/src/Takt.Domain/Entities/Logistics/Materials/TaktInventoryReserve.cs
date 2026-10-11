// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktInventoryReserve.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：存货跌价准备实体（对齐中国《企业会计准则第1号——存货》与 IAS 2 Inventories：成本与可变现净值孰低；允许在原计提范围内转回）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// 存货跌价准备实体（CAS 存货准则 / IAS 2）
/// 计量原则：资产负债表日按「成本与可变现净值孰低」；成本高于可变现净值时计提跌价准备；
/// 可变现净值回升时，在原已计提金额内转回（CAS/IFRS 允许；与 US GAAP ASC 330 一般禁止转回不同）。
/// 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别（期间存当月首日表示年月）
/// </summary>
[SugarTable("takt_logistics_materials_inventory_reserve", "存货跌价准备表")]
[SugarIndex("ix_inventory_reserve_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_inventory_reserve_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_inventory_reserve_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PeriodDate), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(Valuation), OrderByType.Asc, true)]
[SugarIndex("ix_inventory_reserve_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_inventory_reserve_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PeriodDate), OrderByType.Asc, false)]
[SugarIndex("ix_inventory_reserve_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktInventoryReserve : TaktCompanyEntityBase
{
    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    [SugarColumn(ColumnName = "period_date", ColumnDescription = "会计期间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PeriodDate { get; set; }
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? MaterialDescription { get; set; }
    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [SugarColumn(ColumnName = "valuation", ColumnDescription = "评估类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string Valuation { get; set; } = string.Empty;
    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    [SugarColumn(ColumnName = "provision_scope", ColumnDescription = "计提范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProvisionScope { get; set; } = 1;
    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "stock_quantity", ColumnDescription = "库存数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StockQuantity { get; set; } = 0;
    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "unit_cost", ColumnDescription = "单位成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UnitCost { get; set; } = 0;
    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    [SugarColumn(ColumnName = "inventory_cost", ColumnDescription = "存货成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal InventoryCost { get; set; } = 0;
    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    [SugarColumn(ColumnName = "estimated_selling_price", ColumnDescription = "估计售价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedSellingPrice { get; set; } = 0;
    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    [SugarColumn(ColumnName = "estimated_completion_cost", ColumnDescription = "至完工估计成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedCompletionCost { get; set; } = 0;
    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    [SugarColumn(ColumnName = "estimated_selling_cost", ColumnDescription = "估计销售费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedSellingCost { get; set; } = 0;
    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    [SugarColumn(ColumnName = "net_realizable_value", ColumnDescription = "可变现净值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal NetRealizableValue { get; set; } = 0;
    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    [SugarColumn(ColumnName = "unit_net_realizable_value", ColumnDescription = "单位可变现净值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UnitNetRealizableValue { get; set; } = 0;
    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    [SugarColumn(ColumnName = "opening_provision", ColumnDescription = "期初跌价准备", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OpeningProvision { get; set; } = 0;
    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    [SugarColumn(ColumnName = "provision_amount", ColumnDescription = "本期计提金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ProvisionAmount { get; set; } = 0;
    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    [SugarColumn(ColumnName = "reversal_amount", ColumnDescription = "本期转回金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ReversalAmount { get; set; } = 0;
    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    [SugarColumn(ColumnName = "closing_provision", ColumnDescription = "期末跌价准备", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ClosingProvision { get; set; } = 0;
    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    [SugarColumn(ColumnName = "impairment_loss", ColumnDescription = "本期净损益影响", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ImpairmentLoss { get; set; } = 0;
    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    [SugarColumn(ColumnName = "carrying_amount", ColumnDescription = "账面价值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal CarryingAmount { get; set; } = 0;
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    [SugarColumn(ColumnName = "impairment_reason", ColumnDescription = "跌价原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ImpairmentReason { get; set; }
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [SugarColumn(ColumnName = "provision_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProvisionStatus { get; set; } = 1;
}
