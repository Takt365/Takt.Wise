// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseOrder.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购订单实体，定义采购订单领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购订单实体
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_order", "采购订单表")]
[SugarIndex("ix_purchase_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_order_po_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseOrderCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, nameof(OrderDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_order_order_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OrderDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_order_order_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OrderStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_order_purchase_group", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseGroup), OrderByType.Asc, false)]
public class TaktPurchaseOrder : TaktCompanyEntityBase
{

    /// <summary>
    /// 采购订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchaseOrderCode { get; set; } = string.Empty;
    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_id", ColumnDescription = "来源采购申请ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }
    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_code", ColumnDescription = "来源采购申请编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseRequestCode { get; set; }
    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "供应商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 订单日期
    /// </summary>
    [SugarColumn(ColumnName = "order_date", ColumnDescription = "订单日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 要求到货日期
    /// </summary>
    [SugarColumn(ColumnName = "required_arrival_date", ColumnDescription = "要求到货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RequiredArrivalDate { get; set; }
    /// <summary>
    /// 实际到货日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_arrival_date", ColumnDescription = "实际到货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualArrivalDate { get; set; }
    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组代码", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? PurchaseGroup { get; set; }
    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_type", ColumnDescription = "采购订单类型", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseOrderType { get; set; }
    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? PaymentTerms { get; set; }
    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    [SugarColumn(ColumnName = "pricing_procedure", ColumnDescription = "定价过程", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PricingProcedure { get; set; }
    /// <summary>
    /// 定价条件编码
    /// </summary>
    [SugarColumn(ColumnName = "pricing_condition_code", ColumnDescription = "定价条件编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PricingConditionCode { get; set; }
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
    /// 已入库数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "received_quantity", ColumnDescription = "已入库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ReceivedQuantity { get; set; } = 0;
    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "received_amount", ColumnDescription = "已入库金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ReceivedAmount { get; set; } = 0;
    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "paid_amount", ColumnDescription = "已付款金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PaidAmount { get; set; } = 0;
    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "支付方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentMethod { get; set; } = 0;
    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_method", ColumnDescription = "交货方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryMethod { get; set; } = 0;
    /// <summary>
    /// 交货地址
    /// </summary>
    [SugarColumn(ColumnName = "delivery_address", ColumnDescription = "交货地址", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? DeliveryAddress { get; set; }
    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "order_status", ColumnDescription = "订单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int OrderStatus { get; set; } = 1;
    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_status", ColumnDescription = "交货状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseOrderItem.PurchaseOrderId))]
    public List<TaktPurchaseOrderItem>? Items { get; set; }
}
