// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesConditionRecord.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售条件记录实体（定价记录：条件类型+客户+物料+有效期）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售条件记录实体（定价记录；条件类型 + 客户 + 物料 + 有效期；含子表 Items）
/// </summary>
[SugarTable("takt_logistics_sales_condition_record", "销售条件记录表")]
[SugarIndex("ix_sales_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_condition_record_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesConditionRecordCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_condition_record_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PriceType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_condition_record_customer_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_condition_record_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSalesConditionRecord : TaktCompanyEntityBase
{

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    [SugarColumn(ColumnName = "sales_condition_record_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    [SugarColumn(ColumnName = "price_type", ColumnDescription = "条件类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "PB00")]
    public string PriceType { get; set; } = "PR00";

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;

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
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_group", ColumnDescription = "销售组", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? SalesGroup { get; set; }

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
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
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_id", ColumnDescription = "来源销售报价ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_code", ColumnDescription = "来源销售报价编码", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? SalesQuotationCode { get; set; }

    /// <summary>
    /// 可变关键字
    /// </summary>
    [SugarColumn(ColumnName = "variable_key", ColumnDescription = "可变关键字", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? VariableKey { get; set; }

    /// <summary>
    /// 定价条件行列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesConditionRecordItem.SalesConditionRecordId))]
    public List<TaktSalesConditionRecordItem>? Items { get; set; }
}
