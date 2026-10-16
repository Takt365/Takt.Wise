// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseOrderDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// PurchaseOrder 响应 DTO
// ========================================

/// <summary>
/// Takt采购订单实体
/// 对应前端 TaktPurchaseOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }

    /// <summary>
    /// 来源采购申请 名称（填充字段）
    /// </summary>
    public string? PurchaseRequestName { get; set; }

    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    public string? PurchaseOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）
    /// （子表：TaktPurchaseOrderItem）
    /// </summary>
    public List<TaktPurchaseOrderItemDto>? Items { get; set; }

}

// ========================================
// PurchaseOrder 查询 DTO
// ========================================

/// <summary>
/// PurchaseOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseOrderQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }

    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期（范围查询-开始）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围查询-结束）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 要求到货日期（范围查询-开始）
    /// </summary>
    public DateTime? RequiredArrivalDateStart { get; set; }

    /// <summary>
    /// 要求到货日期（范围查询-结束）
    /// </summary>
    public DateTime? RequiredArrivalDateEnd { get; set; }

    /// <summary>
    /// 实际到货日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualArrivalDateStart { get; set; }

    /// <summary>
    /// 实际到货日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualArrivalDateEnd { get; set; }

    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    public string? PurchaseOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal? ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建PurchaseOrder DTO
// ========================================

/// <summary>
/// 创建PurchaseOrder DTO
/// </summary>
public class TaktPurchaseOrderCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）
    /// </summary>
    [Required(ErrorMessage = "采购订单编码（唯一索引）不能为空")]
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（新增时选择采购订单编码规则后取号；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }

    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [Required(ErrorMessage = "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）不能为空")]
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    public string? PurchaseOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [Required(ErrorMessage = "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新PurchaseOrder DTO
// ========================================

/// <summary>
/// 更新PurchaseOrder DTO
/// 继承 TaktPurchaseOrderCreateDto，添加 PurchaseOrderId 字段
/// </summary>
public class TaktPurchaseOrderUpdateDto : TaktPurchaseOrderCreateDto
{
    /// <summary>
    /// PurchaseOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseOrderItemUpdateDto>? Items { get; set; }

}

// ========================================
// PurchaseOrder 状态 DTO
// ========================================

/// <summary>
/// PurchaseOrder 状态更新 DTO
/// </summary>
public class TaktPurchaseOrderStatusDto
{
    /// <summary>
    /// PurchaseOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "订单状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseOrder 导入模板行 DTO
/// </summary>
public class TaktPurchaseOrderTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }

    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    public string? PurchaseOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal? ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// PurchaseOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseOrderImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }

    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    public string? PurchaseOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal? ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    public int? DeliveryMethod { get; set; }

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// PurchaseOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseOrderExportDto
{
    /// <summary>
    /// PurchaseOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码（唯一索引）
    /// </summary>
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseRequestId { get; set; }

    /// <summary>
    /// 来源采购申请编码（冗余）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 实际到货日期
    /// </summary>
    public DateTime? ActualArrivalDate { get; set; }

    /// <summary>
    /// 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
    /// </summary>
    public string? PurchaseOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 订单总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 订单实付金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// 已入库金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// 已付款金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
    /// </summary>
    public int DeliveryMethod { get; set; } = 0;

    /// <summary>
    /// 交货地址
    /// </summary>
    public string? DeliveryAddress { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
