// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInquiry 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInquiry 生成，请按需审阅）
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
// PurchaseInquiry 响应 DTO
// ========================================

/// <summary>
/// 采购询价实体
/// 对应前端 TaktPurchaseInquiryDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInquiryDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInquiryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价日期
    /// </summary>
    public DateTime InquiryDate { get; set; }

    /// <summary>
    /// 报价截止日期
    /// </summary>
    public DateTime? QuoteDeadlineDate { get; set; }

    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }

    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? InquiryEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    public string? PurchaseInquiryType { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    public string PaymentMode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    public int ChainScheme { get; set; } = 0;

    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 询价总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转价格金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 询价原因
    /// </summary>
    public string? InquiryReason { get; set; } = string.Empty;

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int InquiryStatus { get; set; } = 0;

    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 采购询价明细列表（主子表关系）
    /// （子表：TaktPurchaseInquiryItem）
    /// </summary>
    public List<TaktPurchaseInquiryItemDto>? Items { get; set; }

}

// ========================================
// PurchaseInquiry 查询 DTO
// ========================================

/// <summary>
/// PurchaseInquiry 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInquiryQueryDto : TaktPagedQuery
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
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价日期（范围查询-开始）
    /// </summary>
    public DateTime? InquiryDateStart { get; set; }

    /// <summary>
    /// 询价日期（范围查询-结束）
    /// </summary>
    public DateTime? InquiryDateEnd { get; set; }

    /// <summary>
    /// 报价截止日期（范围查询-开始）
    /// </summary>
    public DateTime? QuoteDeadlineDateStart { get; set; }

    /// <summary>
    /// 报价截止日期（范围查询-结束）
    /// </summary>
    public DateTime? QuoteDeadlineDateEnd { get; set; }

    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }

    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? InquiryEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    public string? PurchaseInquiryType { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    public string? PaymentMode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    public int? ChainScheme { get; set; }

    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 询价总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转价格金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 询价原因
    /// </summary>
    public string? InquiryReason { get; set; } = string.Empty;

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? InquiryStatus { get; set; }

    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

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
// 创建PurchaseInquiry DTO
// ========================================

/// <summary>
/// 创建PurchaseInquiry DTO
/// </summary>
public class TaktPurchaseInquiryCreateDto
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
    /// 采购询价编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购询价编码规则生成并展示，非手输；单据类型菜单：采购询价）
    /// </summary>
    [Required(ErrorMessage = "采购询价编码（租户+公司+工厂内业务唯一）不能为空")]
    public string PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（新增时选择采购询价编码规则后取号；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 询价日期
    /// </summary>
    public DateTime InquiryDate { get; set; }

    /// <summary>
    /// 报价截止日期
    /// </summary>
    public DateTime? QuoteDeadlineDate { get; set; }

    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }

    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? InquiryEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    [Required(ErrorMessage = "询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [Required(ErrorMessage = "询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）不能为空")]
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    public string? PurchaseInquiryType { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [Required(ErrorMessage = "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    [Required(ErrorMessage = "付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）不能为空")]
    public string PaymentMode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    public int ChainScheme { get; set; } = 0;

    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 询价总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转价格金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 询价原因
    /// </summary>
    public string? InquiryReason { get; set; } = string.Empty;

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int InquiryStatus { get; set; } = 0;

    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 采购询价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInquiryItemCreateDto>? Items { get; set; }

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
// 更新PurchaseInquiry DTO
// ========================================

/// <summary>
/// 更新PurchaseInquiry DTO
/// 继承 TaktPurchaseInquiryCreateDto，添加 PurchaseInquiryId 字段
/// </summary>
public class TaktPurchaseInquiryUpdateDto : TaktPurchaseInquiryCreateDto
{
    /// <summary>
    /// PurchaseInquiryID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseInquiryItemUpdateDto>? Items { get; set; }

}

// ========================================
// PurchaseInquiry 状态 DTO
// ========================================

/// <summary>
/// PurchaseInquiry 状态更新 DTO
/// </summary>
public class TaktPurchaseInquiryStatusDto
{
    /// <summary>
    /// PurchaseInquiryID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "询价状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int InquiryStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInquiry 导入模板行 DTO
/// </summary>
public class TaktPurchaseInquiryTemplateDto
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
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价日期
    /// </summary>
    public DateTime? InquiryDate { get; set; }

    /// <summary>
    /// 报价截止日期
    /// </summary>
    public DateTime? QuoteDeadlineDate { get; set; }

    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }

    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? InquiryEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    public string? PurchaseInquiryType { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    public string? PaymentMode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    public int? ChainScheme { get; set; }

    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 询价总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转价格金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 询价原因
    /// </summary>
    public string? InquiryReason { get; set; } = string.Empty;

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? InquiryStatus { get; set; }

    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 采购询价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInquiryItemCreateDto>? Items { get; set; }

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
/// PurchaseInquiry 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInquiryImportDto
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
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价日期
    /// </summary>
    public DateTime? InquiryDate { get; set; }

    /// <summary>
    /// 报价截止日期
    /// </summary>
    public DateTime? QuoteDeadlineDate { get; set; }

    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }

    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? InquiryEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    public string? PurchaseInquiryType { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    public string? PaymentMode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    public int? ChainScheme { get; set; }

    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 询价总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转价格金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 询价原因
    /// </summary>
    public string? InquiryReason { get; set; } = string.Empty;

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? InquiryStatus { get; set; }

    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 采购询价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInquiryItemCreateDto>? Items { get; set; }

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
/// PurchaseInquiry 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInquiryExportDto
{
    /// <summary>
    /// PurchaseInquiryID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

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
    /// 采购询价编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价日期
    /// </summary>
    public DateTime InquiryDate { get; set; }

    /// <summary>
    /// 报价截止日期
    /// </summary>
    public DateTime? QuoteDeadlineDate { get; set; }

    /// <summary>
    /// 询价人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InquiryEmployeeId { get; set; }

    /// <summary>
    /// 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? InquiryEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
    /// </summary>
    public string? PurchaseInquiryType { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件编码
    /// </summary>
    public string? PricingConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
    /// </summary>
    public string PaymentMode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
    /// </summary>
    public int ChainScheme { get; set; } = 0;

    /// <summary>
    /// 询价总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 询价总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转价格数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转价格金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 询价原因
    /// </summary>
    public string? InquiryReason { get; set; } = string.Empty;

    /// <summary>
    /// 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int InquiryStatus { get; set; } = 0;

    /// <summary>
    /// 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

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
