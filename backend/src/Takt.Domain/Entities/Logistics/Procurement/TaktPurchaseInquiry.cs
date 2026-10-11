// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInquiry.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价实体（主子表主表，含询价明细 Items）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// 采购询价实体
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_inquiry", "采购询价表")]
[SugarIndex("ix_purchase_inquiry_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_inquiry_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_inquiry_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseInquiryCode), OrderByType.Asc, nameof(InquiryDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_inquiry_inquiry_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InquiryDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_inquiry_inquiry_employee_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InquiryEmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_inquiry_supplier_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
public class TaktPurchaseInquiry : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "采购询价编码", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string PurchaseInquiryCode { get; set; } = string.Empty;
    /// <summary>
    /// 询价日期
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_date", ColumnDescription = "询价日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime InquiryDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 报价截止日期
    /// </summary>
    [SugarColumn(ColumnName = "quote_deadline_date", ColumnDescription = "报价截止日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? QuoteDeadlineDate { get; set; }
    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_employee_id", ColumnDescription = "询价人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }
    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_employee_name", ColumnDescription = "询价人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? InquiryEmployeeName { get; set; }
    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "询价供应商编码", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "询价供应商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_type", ColumnDescription = "采购询价类型", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseInquiryType { get; set; }
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
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
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
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;
    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    [SugarColumn(ColumnName = "payment_mode", ColumnDescription = "付款方式", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "vendorpay")]
    public string PaymentMode { get; set; } = "vendorpay";
    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    [SugarColumn(ColumnName = "chain_scheme", ColumnDescription = "采购链路方案", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ChainScheme { get; set; } = 1;
    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "询价总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; }
    /// <summary>
    /// 询价总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "询价总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转价格数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; }
    /// <summary>
    /// 已转价格金额
    /// </summary>
    [SugarColumn(ColumnName = "converted_amount", ColumnDescription = "已转价格金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedAmount { get; set; }
    /// <summary>
    /// 询价原因
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_reason", ColumnDescription = "询价原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? InquiryReason { get; set; }
    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_status", ColumnDescription = "询价状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InquiryStatus { get; set; } = 1;
    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    [SugarColumn(ColumnName = "converted_status", ColumnDescription = "转价格状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConvertedStatus { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 采购询价明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInquiryItem.PurchaseInquiryId))]
    public List<TaktPurchaseInquiryItem>? Items { get; set; }
}
