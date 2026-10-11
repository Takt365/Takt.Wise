// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseRequestDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseRequest 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseRequest 生成，请按需审阅）
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
// PurchaseRequest 响应 DTO
// ========================================

/// <summary>
/// Takt采购申请实体
/// 对应前端 TaktPurchaseRequestDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktPurchaseRequestDto : TaktApprovalDtoBase
{
    /// <summary>
    /// PurchaseRequestID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseRequestId { get; set; }

    /// <summary>
    /// 采购申请编码（唯一索引）
    /// </summary>
    public string PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价 名称（填充字段）
    /// </summary>
    public string? PurchaseInquiryName { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 来源采购计划 名称（填充字段）
    /// </summary>
    public string? PurchasePlanName { get; set; }

    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    public int ChainScheme { get; set; } = 0;

    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    public int? PoDecision { get; set; }

    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// PR 会签单 名称（填充字段）
    /// </summary>
    public string? CountersignName { get; set; }

    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? RequestEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）
    /// </summary>
    public string? PurchaseRequestType { get; set; } = string.Empty;

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
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? RequestReason { get; set; } = string.Empty;

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 采购申请明细列表（主子表关系，一个申请可以有多个明细）
    /// （子表：TaktPurchaseRequestItem）
    /// </summary>
    public List<TaktPurchaseRequestItemDto>? Items { get; set; }

}

// ========================================
// PurchaseRequest 查询 DTO
// ========================================

/// <summary>
/// PurchaseRequest 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseRequestQueryDto : TaktPagedQuery
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
    /// 采购申请编码（唯一索引）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    public int? ChainScheme { get; set; }

    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    public int? PoDecision { get; set; }

    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请日期（范围查询-开始）
    /// </summary>
    public DateTime? RequestDateStart { get; set; }

    /// <summary>
    /// 申请日期（范围查询-结束）
    /// </summary>
    public DateTime? RequestDateEnd { get; set; }

    /// <summary>
    /// 要求到货日期（范围查询-开始）
    /// </summary>
    public DateTime? RequiredArrivalDateStart { get; set; }

    /// <summary>
    /// 要求到货日期（范围查询-结束）
    /// </summary>
    public DateTime? RequiredArrivalDateEnd { get; set; }

    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? RequestEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）
    /// </summary>
    public string? PurchaseRequestType { get; set; } = string.Empty;

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
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? RequestReason { get; set; } = string.Empty;

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? RequestStatus { get; set; }

    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建PurchaseRequest DTO
// ========================================

/// <summary>
/// 创建PurchaseRequest DTO
/// </summary>
public class TaktPurchaseRequestCreateDto
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
    /// 采购申请编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "采购申请编码（唯一索引）不能为空")]
    public string PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    public int ChainScheme { get; set; } = 0;

    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    public int? PoDecision { get; set; }

    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? RequestEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [Required(ErrorMessage = "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）不能为空")]
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）
    /// </summary>
    public string? PurchaseRequestType { get; set; } = string.Empty;

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
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? RequestReason { get; set; } = string.Empty;

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 采购申请明细列表（主子表关系，一个申请可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseRequestItemCreateDto>? Items { get; set; }

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
// 更新PurchaseRequest DTO
// ========================================

/// <summary>
/// 更新PurchaseRequest DTO
/// 继承 TaktPurchaseRequestCreateDto，添加 PurchaseRequestId 字段
/// </summary>
public class TaktPurchaseRequestUpdateDto : TaktPurchaseRequestCreateDto
{
    /// <summary>
    /// PurchaseRequestID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseRequestId { get; set; }

    /// <summary>
    /// 采购申请明细列表（主子表关系，一个申请可以有多个明细）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseRequestItemUpdateDto>? Items { get; set; }

}

// ========================================
// PurchaseRequest 状态 DTO
// ========================================

/// <summary>
/// PurchaseRequest 状态更新 DTO
/// </summary>
public class TaktPurchaseRequestStatusDto
{
    /// <summary>
    /// PurchaseRequestID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseRequestId { get; set; }

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    [Required(ErrorMessage = "申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）不能为空")]
    public int RequestStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseRequest 导入模板行 DTO
/// </summary>
public class TaktPurchaseRequestTemplateDto
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
    /// 采购申请编码（唯一索引）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    public int? ChainScheme { get; set; }

    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    public int? PoDecision { get; set; }

    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? RequestDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? RequestEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）
    /// </summary>
    public string? PurchaseRequestType { get; set; } = string.Empty;

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
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? RequestReason { get; set; } = string.Empty;

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? RequestStatus { get; set; }

    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 采购申请明细列表（主子表关系，一个申请可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseRequestItemCreateDto>? Items { get; set; }

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
/// PurchaseRequest 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseRequestImportDto
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
    /// 采购申请编码（唯一索引）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    public int? ChainScheme { get; set; }

    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    public int? PoDecision { get; set; }

    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime? RequestDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? RequestEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）
    /// </summary>
    public string? PurchaseRequestType { get; set; } = string.Empty;

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
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? RequestReason { get; set; } = string.Empty;

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? RequestStatus { get; set; }

    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 采购申请明细列表（主子表关系，一个申请可以有多个明细）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseRequestItemCreateDto>? Items { get; set; }

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
/// PurchaseRequest 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseRequestExportDto
{
    /// <summary>
    /// PurchaseRequestID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseRequestId { get; set; }

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
    /// 采购申请编码（唯一索引）
    /// </summary>
    public string PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    public int ChainScheme { get; set; } = 0;

    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    public int? PoDecision { get; set; }

    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请日期
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// 要求到货日期
    /// </summary>
    public DateTime? RequiredArrivalDate { get; set; }

    /// <summary>
    /// 申请人员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestEmployeeId { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 RequestEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? RequestEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请类型（字典 logistics_procurement_purchase_order_type；与采购订单/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；申请默认 B-NB）
    /// </summary>
    public string? PurchaseRequestType { get; set; } = string.Empty;

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
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? RequestReason { get; set; } = string.Empty;

    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int RequestStatus { get; set; } = 0;

    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
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
