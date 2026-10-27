// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCost.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本汇总表（物理一行=工厂+产品+核算日；UI 三层浏览同表聚合，不拆实体）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本汇总表（由明细按工厂+产品+核算日聚合写入；UI 机种组→产品行→Item，实体不拆分，无对明细导航）
/// <para>业务唯一键（新增/更新匹配）：TenantCode+CompanyCode+PlantCode+ProductCode+CostingDate；ModelCode/CostingPeriod 为业务字段，不参与唯一匹配。</para>
/// <para>软删除：本表 IsDeleted 可为 0/1；列表读隔离不显示已删；合计/重算/机种平均须 includeSoftDeleted 含已删主表。</para>
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_material_cost", "材料成本表")]
[SugarIndex("ix_bom_material_cost_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bom_material_cost_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProductCode), OrderByType.Asc, nameof(CostingDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_model_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ModelCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_product_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProductCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_costing_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CostingPeriod), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_material_cost_costing_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CostingDate), OrderByType.Asc, false)]
public class TaktBomMaterialCost : TaktCompanyEntityBase
{

    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode）
    /// <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均；
    /// 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
    /// </summary>
    [SugarColumn(ColumnName = "model_monthly_average_cost", ColumnDescription = "机种月成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ModelMonthlyAverageCost { get; set; } = 0;

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码）
    /// <para>CRUD 表单：字典 logistics_materials_material_type。</para>
    /// <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "FERT")]
    public string MaterialType { get; set; } = "FERT";

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一）
    /// <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para>
    /// <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    [SugarColumn(ColumnName = "product_code", ColumnDescription = "产品编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    [SugarColumn(ColumnName = "product_description", ColumnDescription = "产品描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
    /// </summary>
    [SugarColumn(ColumnName = "product_monthly_cost", ColumnDescription = "产品月成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ProductMonthlyCost { get; set; } = 0;

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "product_monthly_calculation", ColumnDescription = "产品月计算", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ProductMonthlyCalculation { get; set; } = 0;

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    [SugarColumn(ColumnName = "latest_purchase_cost", ColumnDescription = "最近采购成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal LatestPurchaseCost { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false)]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
    /// </summary>
    [SugarColumn(ColumnName = "costing_period", ColumnDescription = "核算期间", ColumnDataType = "nvarchar", Length = 7, IsNullable = false)]
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）
    /// </summary>
    [SugarColumn(ColumnName = "costing_date", ColumnDescription = "核算日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CostingDate { get; set; }
}
