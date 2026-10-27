// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesInvoiceItem.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售发票明细实体（必要字段按开票凭证行清单）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售发票明细实体（公司级；主子表关系见 SalesInvoiceId）
/// </summary>
[SugarTable("takt_logistics_sales_invoice_item", "销售发票明细表")]
[SugarIndex("ix_sales_invoice_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_invoice_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_item_invoice_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesInvoiceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_invoice_item_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktSalesInvoiceItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sales_invoice_id", ColumnDescription = "销售发票ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "billing_document_code", ColumnDescription = "开票凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "项目", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 已出发票数量
    /// </summary>
    [SugarColumn(ColumnName = "billing_quantity", ColumnDescription = "已出发票数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    [SugarColumn(ColumnName = "sales_unit", ColumnDescription = "销售单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? SalesUnit { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? BaseUnit { get; set; }

    /// <summary>
    /// 等级数量
    /// </summary>
    [SugarColumn(ColumnName = "scale_quantity", ColumnDescription = "等级数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    [SugarColumn(ColumnName = "billing_quantity_sku", ColumnDescription = "库存单位开票数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    [SugarColumn(ColumnName = "net_weight", ColumnDescription = "净重量", ColumnDataType = "decimal", Length = 15, DecimalDigits = 3, IsNullable = true)]
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    [SugarColumn(ColumnName = "gross_weight", ColumnDescription = "毛重", ColumnDataType = "decimal", Length = 15, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    [SugarColumn(ColumnName = "weight_unit", ColumnDescription = "重量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? WeightUnit { get; set; }

    /// <summary>
    /// 业务范围
    /// </summary>
    [SugarColumn(ColumnName = "business_area_code", ColumnDescription = "业务范围", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? BusinessAreaCode { get; set; }

    /// <summary>
    /// 定价日期
    /// </summary>
    [SugarColumn(ColumnName = "pricing_date", ColumnDescription = "定价日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 提供服务日期
    /// </summary>
    [SugarColumn(ColumnName = "service_rendered_date", ColumnDescription = "提供服务日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ServiceRenderedDate { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    [SugarColumn(ColumnName = "pricing_exchange_rate", ColumnDescription = "汇率", ColumnDataType = "decimal", Length = 9, DecimalDigits = 5, IsNullable = true)]
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    [SugarColumn(ColumnName = "net_amount", ColumnDescription = "净价值", ColumnDataType = "decimal", Length = 15, DecimalDigits = 2, IsNullable = true)]
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_code", ColumnDescription = "参考凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ReferenceDocumentCode { get; set; }

    /// <summary>
    /// 参考项目
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_item", ColumnDescription = "参考项目", ColumnDataType = "int", IsNullable = true)]
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_category", ColumnDescription = "先前凭证类别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ReferenceDocumentCategory { get; set; }

    /// <summary>
    /// 销售凭证
    /// </summary>
    [SugarColumn(ColumnName = "sales_document_code", ColumnDescription = "销售凭证", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SalesDocumentCode { get; set; }

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    [SugarColumn(ColumnName = "sales_document_item", ColumnDescription = "销售凭证项目", ColumnDataType = "int", IsNullable = true)]
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    [SugarColumn(ColumnName = "sales_document_reference_flag", ColumnDescription = "销售凭证参考", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SalesDocumentReferenceFlag { get; set; }

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? MaterialDescription { get; set; }

    /// <summary>
    /// 定价参考物料
    /// </summary>
    [SugarColumn(ColumnName = "pricing_reference_material_code", ColumnDescription = "定价参考物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PricingReferenceMaterialCode { get; set; }

    /// <summary>
    /// 批次
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? BatchCode { get; set; }

    /// <summary>
    /// 物料组
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "物料组", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? MaterialGroup { get; set; }

    /// <summary>
    /// 项目类别
    /// </summary>
    [SugarColumn(ColumnName = "sales_item_category", ColumnDescription = "项目类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SalesItemCategory { get; set; }

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    [SugarColumn(ColumnName = "product_hierarchy", ColumnDescription = "产品层次", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductHierarchy { get; set; }

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    [SugarColumn(ColumnName = "shipping_point", ColumnDescription = "装运点/接收点", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ShippingPoint { get; set; }

    /// <summary>
    /// 产品组
    /// </summary>
    [SugarColumn(ColumnName = "division", ColumnDescription = "产品组", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? Division { get; set; }

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    [SugarColumn(ColumnName = "partner_item", ColumnDescription = "合作伙伴项目", ColumnDataType = "int", IsNullable = true)]
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "departure_country", ColumnDescription = "国家", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DepartureCountry { get; set; }

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    [SugarColumn(ColumnName = "plant_region", ColumnDescription = "交货工厂地区", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? PlantRegion { get; set; }

    /// <summary>
    /// 定价
    /// </summary>
    [SugarColumn(ColumnName = "pricing_flag", ColumnDescription = "定价", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? PricingFlag { get; set; }

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "库存地点", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? WarehouseCode { get; set; }

    /// <summary>
    /// 成本
    /// </summary>
    [SugarColumn(ColumnName = "cost_amount", ColumnDescription = "成本", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    [SugarColumn(ColumnName = "subtotal1", ColumnDescription = "小计1", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    [SugarColumn(ColumnName = "subtotal2", ColumnDescription = "小计2", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    [SugarColumn(ColumnName = "subtotal3", ColumnDescription = "小计3", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    [SugarColumn(ColumnName = "subtotal4", ColumnDescription = "小计4", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    [SugarColumn(ColumnName = "subtotal5", ColumnDescription = "小计5", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    [SugarColumn(ColumnName = "subtotal6", ColumnDescription = "小计6", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    [SugarColumn(ColumnName = "statistics_exchange_rate", ColumnDescription = "汇率统计", ColumnDataType = "decimal", Length = 9, DecimalDigits = 5, IsNullable = true)]
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ProfitCenterCode { get; set; }

    /// <summary>
    /// 信贷价格
    /// </summary>
    [SugarColumn(ColumnName = "credit_price", ColumnDescription = "信贷价格", ColumnDataType = "decimal", Length = 11, DecimalDigits = 2, IsNullable = true)]
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    [SugarColumn(ColumnName = "customer_group_sales_order", ColumnDescription = "客户组销售订单", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CustomerGroupSalesOrder { get; set; }

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "destination_country_order", ColumnDescription = "订单目的地国家", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DestinationCountryOrder { get; set; }

    /// <summary>
    /// 地区订单
    /// </summary>
    [SugarColumn(ColumnName = "region_order", ColumnDescription = "地区订单", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? RegionOrder { get; set; }

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    [SugarColumn(ColumnName = "sales_organization_order", ColumnDescription = "订单的销售机构", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SalesOrganizationOrder { get; set; }

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    [SugarColumn(ColumnName = "distribution_channel_order", ColumnDescription = "订单分销渠道", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? DistributionChannelOrder { get; set; }

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    [SugarColumn(ColumnName = "document_category", ColumnDescription = "SD 凭证类别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DocumentCategory { get; set; }

    /// <summary>
    /// 税额
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税额", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    [SugarColumn(ColumnName = "gross_amount", ColumnDescription = "总值", ColumnDataType = "decimal", Length = 15, DecimalDigits = 2, IsNullable = true)]
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate_date", ColumnDescription = "换算日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 销售发票主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SalesInvoiceId))]
    public TaktSalesInvoice? SalesInvoice { get; set; }
}
