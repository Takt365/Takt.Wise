// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseRequestItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购申请明细实体，定义采购申请明细领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购申请明细实体
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_request_item", "采购申请明细表")]
[SugarIndex("ix_purchase_request_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_request_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_request_item_request_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseRequestId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_request_item_purchase_request_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseRequestCode), OrderByType.Asc, false)]
public class TaktPurchaseRequestItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_id", ColumnDescription = "采购申请ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseRequestId { get; set; }

    /// <summary>
    /// 采购申请编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_code", ColumnDescription = "采购申请编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_item_id", ColumnDescription = "来源采购计划明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    [SugarColumn(ColumnName = "allocation_category", ColumnDescription = "分配类别", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string AllocationCategory { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "request_unit", ColumnDescription = "申请单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string RequestUnit { get; set; } = "PC";

    /// <summary>
    /// 申请数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "request_quantity", ColumnDescription = "申请数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal RequestQuantity { get; set; } = 0;

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_per_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PurchasePerUnit { get; set; } = 1000;

    /// <summary>
    /// 请购单价
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_unit_price", ColumnDescription = "请购单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal PurchaseRequestUnitPrice { get; set; } = 0;

    /// <summary>
    /// 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxCode { get; set; }

    /// <summary>
    /// 含税金额
    /// </summary>
    [SugarColumn(ColumnName = "tax_included_amount", ColumnDescription = "含税金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxIncludedAmount { get; set; } = 0;
    /// <summary>
    /// 未税金额
    /// </summary>
    [SugarColumn(ColumnName = "untaxed_amount", ColumnDescription = "未税金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UntaxedAmount { get; set; } = 0;
    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 请购金额
    /// </summary>
    [SugarColumn(ColumnName = "request_amount", ColumnDescription = "请购金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal RequestAmount { get; set; } = 0;

    /// <summary>
    /// 价格日期
    /// </summary>
    [SugarColumn(ColumnName = "pricing_date", ColumnDescription = "价格日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    [SugarColumn(ColumnName = "gross_weight", ColumnDescription = "毛重", ColumnDataType = "decimal", Length = 18, DecimalDigits = 10, IsNullable = true)]
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    [SugarColumn(ColumnName = "net_weight", ColumnDescription = "净重", ColumnDataType = "decimal", Length = 18, DecimalDigits = 10, IsNullable = true)]
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    [SugarColumn(ColumnName = "weight_unit", ColumnDescription = "重量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? WeightUnit { get; set; }

    /// <summary>
    /// 体积
    /// </summary>
    [SugarColumn(ColumnName = "volume", ColumnDescription = "体积", ColumnDataType = "decimal", Length = 18, DecimalDigits = 10, IsNullable = true)]
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    [SugarColumn(ColumnName = "volume_unit", ColumnDescription = "体积单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? VolumeUnit { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ProfitCenterCode { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
