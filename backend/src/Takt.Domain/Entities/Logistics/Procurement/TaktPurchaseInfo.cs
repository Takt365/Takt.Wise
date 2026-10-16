// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInfo.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购信息实体（定价记录：条件类型+供应商+物料+有效期）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购信息实体（定价记录；条件类型 + 供应商 + 物料 + 有效期；含子表 Items）
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_info", "采购信息表")]
[SugarIndex("ix_purchase_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_info_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseInfoCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_info_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PriceType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_info_supplier_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_info_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktPurchaseInfo : TaktCompanyEntityBase
{

    /// <summary>
    /// 定价记录号（租户+公司+工厂内唯一；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：采购信息）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_info_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchaseInfoCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    [SugarColumn(ColumnName = "price_type", ColumnDescription = "条件类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "PB00")]
    public string PriceType { get; set; } = "PB00";

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? PurchaseGroup { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxCode { get; set; }

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "gr_based_invoice_inspection", ColumnDescription = "基于收货的发票检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "pricing_date_control", ColumnDescription = "定价日期控制", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PricingDateControl { get; set; } = 1;

    /// <summary>
    /// 有效起始日
    /// </summary>
    [SugarColumn(ColumnName = "valid_from", ColumnDescription = "有效起始日", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// 有效截至日
    /// </summary>
    [SugarColumn(ColumnName = "valid_to", ColumnDescription = "有效截至日", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidTo { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_id", ColumnDescription = "来源采购询价ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "来源采购询价编码", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? PurchaseInquiryCode { get; set; }

    /// <summary>
    /// 可变关键字
    /// </summary>
    [SugarColumn(ColumnName = "variable_key", ColumnDescription = "可变关键字", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? VariableKey { get; set; }

    /// <summary>
    /// 定价条件行列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInfoItem.PurchaseInfoId))]
    public List<TaktPurchaseInfoItem>? Items { get; set; }
}
