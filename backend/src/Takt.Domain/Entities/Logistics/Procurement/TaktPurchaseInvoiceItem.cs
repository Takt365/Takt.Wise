// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItem.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购发票明细实体（字段按 RSEG 业务清单）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购发票明细实体（公司级；主子表关系见 PurchaseInvoiceId）
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_invoice_item", "采购发票明细表")]
[SugarIndex("ix_purchase_invoice_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_invoice_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseInvoiceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_item_purchase_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseOrderCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_item_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktPurchaseInvoiceItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_invoice_id", ColumnDescription = "采购发票ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_invoice_code", ColumnDescription = "凭证编号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "凭证项目", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购凭证", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_item", ColumnDescription = "项目", ColumnDataType = "int", IsNullable = true)]
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    [SugarColumn(ColumnName = "account_assignment_seq", ColumnDescription = "科目分配序号", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? AccountAssignmentSeq { get; set; }

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 评估范围
    /// </summary>
    [SugarColumn(ColumnName = "valuation_area", ColumnDescription = "评估范围", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ValuationArea { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [SugarColumn(ColumnName = "amount", ColumnDescription = "金额", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    [SugarColumn(ColumnName = "debit_credit_indicator", ColumnDescription = "借/贷标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DebitCreditIndicator { get; set; }

    /// <summary>
    /// 税码
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? TaxCode { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnName = "quantity", ColumnDescription = "数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    [SugarColumn(ColumnName = "order_unit", ColumnDescription = "订单单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? OrderUnit { get; set; }

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    [SugarColumn(ColumnName = "po_price_quantity", ColumnDescription = "订单价格单位数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    [SugarColumn(ColumnName = "po_price_unit", ColumnDescription = "订单价格单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? PoPriceUnit { get; set; }

    /// <summary>
    /// 总库存
    /// </summary>
    [SugarColumn(ColumnName = "valuated_stock_quantity", ColumnDescription = "总库存", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    [SugarColumn(ColumnName = "previous_period_stock", ColumnDescription = "上一过账期间库存", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? BaseUnit { get; set; }

    /// <summary>
    /// 评估类
    /// </summary>
    [SugarColumn(ColumnName = "valuation_class", ColumnDescription = "评估类", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ValuationClass { get; set; }

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    [SugarColumn(ColumnName = "update_po_history_flag", ColumnDescription = "标识: 更新采购订单历史", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? UpdatePoHistoryFlag { get; set; }

    /// <summary>
    /// 后续借/贷
    /// </summary>
    [SugarColumn(ColumnName = "subsequent_debit_credit", ColumnDescription = "后续借/贷", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SubsequentDebitCredit { get; set; }

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    [SugarColumn(ColumnName = "block_reason_price", ColumnDescription = "价格冻结原因", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BlockReasonPrice { get; set; }

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    [SugarColumn(ColumnName = "block_reason_quantity", ColumnDescription = "数量冻结原因", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BlockReasonQuantity { get; set; }

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    [SugarColumn(ColumnName = "block_reason_quality", ColumnDescription = "质量冻结原因", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BlockReasonQuality { get; set; }

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    [SugarColumn(ColumnName = "block_reason_enhanced", ColumnDescription = "增强冻结原因", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BlockReasonEnhanced { get; set; }

    /// <summary>
    /// 价值串
    /// </summary>
    [SugarColumn(ColumnName = "value_string", ColumnDescription = "价值串", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ValueString { get; set; }

    /// <summary>
    /// 参照
    /// </summary>
    [SugarColumn(ColumnName = "reference_code", ColumnDescription = "参照", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? ReferenceCode { get; set; }

    /// <summary>
    /// 条件类型
    /// </summary>
    [SugarColumn(ColumnName = "condition_type", ColumnDescription = "条件类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ConditionType { get; set; }

    /// <summary>
    /// 总价值
    /// </summary>
    [SugarColumn(ColumnName = "total_valuated_stock_value", ColumnDescription = "总价值", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    [SugarColumn(ColumnName = "previous_period_value", ColumnDescription = "前期总值", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_code", ColumnDescription = "参考凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ReferenceDocumentCode { get; set; }

    /// <summary>
    /// 当前期间年
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_year", ColumnDescription = "当前期间年", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ReferenceDocumentYear { get; set; }

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_item", ColumnDescription = "参考凭证项目", ColumnDataType = "int", IsNullable = true)]
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    [SugarColumn(ColumnName = "stock_managed_material_code", ColumnDescription = "库存物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? StockManagedMaterialCode { get; set; }

    /// <summary>
    /// 文本
    /// </summary>
    [SugarColumn(ColumnName = "item_text", ColumnDescription = "文本", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ItemText { get; set; }

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    [SugarColumn(ColumnName = "material_document_item", ColumnDescription = "来自到达的发票的存货过帐行", ColumnDataType = "int", IsNullable = true)]
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 采购发票主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PurchaseInvoiceId))]
    public TaktPurchaseInvoice? PurchaseInvoice { get; set; }
}
