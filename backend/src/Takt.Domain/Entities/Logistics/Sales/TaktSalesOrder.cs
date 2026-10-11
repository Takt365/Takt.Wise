// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesOrder.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售订单实体，定义销售订单领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售订单实体
/// </summary>
[SugarTable("takt_logistics_sales_order", "销售订单表")]
[SugarIndex("ix_sales_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_order_so_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_order_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_order_order_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OrderDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_sales_order_order_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OrderStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_order_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSalesOrder : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售订单编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_code", ColumnDescription = "销售订单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesOrderCode { get; set; } = string.Empty;
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
    /// 订单日期
    /// </summary>
    [SugarColumn(ColumnName = "order_date", ColumnDescription = "订单日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 要求交货日期
    /// </summary>
    [SugarColumn(ColumnName = "required_delivery_date", ColumnDescription = "要求交货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RequiredDeliveryDate { get; set; }
    /// <summary>
    /// 实际交货日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_delivery_date", ColumnDescription = "实际交货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualDeliveryDate { get; set; }
    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_group", ColumnDescription = "销售组", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? SalesGroup { get; set; }
    /// <summary>
    /// 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_type", ColumnDescription = "销售订单类型", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SalesOrderType { get; set; }
    /// <summary>
    /// 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
    /// </summary>
    [SugarColumn(ColumnName = "order_reason", ColumnDescription = "订单原因", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? OrderReason { get; set; }
    /// <summary>
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_organization", ColumnDescription = "销售组织", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SalesOrganization { get; set; }
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
    /// 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
    /// </summary>
    [SugarColumn(ColumnName = "invoice_type", ColumnDescription = "发票类型", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? InvoiceType { get; set; }
    /// <summary>
    /// 采购订单编码
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }
    /// <summary>
    /// 订单日期
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_date", ColumnDescription = "订单日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PurchaseOrderDate { get; set; }
    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "订单总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;
    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "订单总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;
    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;
    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 汇率
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate", ColumnDescription = "汇率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "1.00000")]
    public decimal ExchangeRate { get; set; } = 1.00000m;
    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxCode { get; set; }
    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;
    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "订单实付金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;
    /// <summary>
    /// 已发货数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "shipped_quantity", ColumnDescription = "已发货数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ShippedQuantity { get; set; } = 0;
    /// <summary>
    /// 已发货金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "shipped_amount", ColumnDescription = "已发货金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ShippedAmount { get; set; } = 0;
    /// <summary>
    /// 已收款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "received_amount", ColumnDescription = "已收款金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ReceivedAmount { get; set; } = 0;
    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_method", ColumnDescription = "交货方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryMethod { get; set; } = 0;
    /// <summary>
    /// 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "收款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentMethod { get; set; } = 0;
    /// <summary>
    /// 交货地址
    /// </summary>
    [SugarColumn(ColumnName = "delivery_address", ColumnDescription = "交货地址", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? DeliveryAddress { get; set; }
    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "order_status", ColumnDescription = "订单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int OrderStatus { get; set; } = 1;
    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_status", ColumnDescription = "交货状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 销售订单明细列表（主子表关系，一个订单可以有多个明细）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesOrderItem.SalesOrderId))]
    public List<TaktSalesOrderItem>? Items { get; set; }
}
