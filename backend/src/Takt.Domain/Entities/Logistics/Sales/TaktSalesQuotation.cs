// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesQuotation.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售报价实体，定义销售报价领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售报价实体
/// </summary>
[SugarTable("takt_logistics_sales_quotation", "销售报价表")]
[SugarIndex("ix_sales_quotation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_quotation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesQuotationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_quotation_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_quotation_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuotationDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_sales_quotation_quotation_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuotationStatus), OrderByType.Asc, false)]
public class TaktSalesQuotation : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售报价编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售报价编码规则生成并展示，非手输；单据类型菜单：销售报价）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_code", ColumnDescription = "销售报价编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesQuotationCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "客户名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string CustomerName1 { get; set; } = string.Empty;
    /// <summary>
    /// 报价日期
    /// </summary>
    [SugarColumn(ColumnName = "quotation_date", ColumnDescription = "报价日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime QuotationDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 报价有效期至
    /// </summary>
    [SugarColumn(ColumnName = "valid_until_date", ColumnDescription = "报价有效期至", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ValidUntilDate { get; set; }
    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_group", ColumnDescription = "销售组", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? SalesGroup { get; set; }
    /// <summary>
    /// 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_type", ColumnDescription = "销售报价类型", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SalesQuotationType { get; set; }
    /// <summary>
    /// 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
    /// </summary>
    [SugarColumn(ColumnName = "pricing_procedure", ColumnDescription = "定价过程", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PricingProcedure { get; set; }
    /// <summary>
    /// 定价条件编码
    /// </summary>
    [SugarColumn(ColumnName = "pricing_condition_code", ColumnDescription = "定价条件编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PricingConditionCode { get; set; }
    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "报价总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;
    /// <summary>
    /// 报价总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "报价总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;
    /// <summary>
    /// 折扣金额
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;
    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxCode { get; set; }
    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;
    /// <summary>
    /// 报价实付金额
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "报价实付金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;
    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_code", ColumnDescription = "销售订单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SalesOrderCode { get; set; }
    /// <summary>
    /// 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "quotation_status", ColumnDescription = "报价状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QuotationStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 销售报价明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesQuotationItem.SalesQuotationId))]
    public List<TaktSalesQuotationItem>? Items { get; set; }
}
