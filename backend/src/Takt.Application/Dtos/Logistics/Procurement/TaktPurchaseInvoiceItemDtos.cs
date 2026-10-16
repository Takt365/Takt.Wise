// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItemDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInvoiceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInvoiceItem 生成，请按需审阅）
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
// PurchaseInvoiceItem 响应 DTO
// ========================================

/// <summary>
/// Takt采购发票明细实体（公司级；主子表关系见 PurchaseInvoiceId）
/// 对应前端 TaktPurchaseInvoiceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInvoiceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

    /// <summary>
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票名称（填充字段）
    /// </summary>
    public string? PurchaseInvoiceName { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    public string? AccountAssignmentSeq { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估范围
    /// </summary>
    public string? ValuationArea { get; set; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 总库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 评估类
    /// </summary>
    public string? ValuationClass { get; set; } = string.Empty;

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    public string? UpdatePoHistoryFlag { get; set; } = string.Empty;

    /// <summary>
    /// 后续借/贷
    /// </summary>
    public string? SubsequentDebitCredit { get; set; } = string.Empty;

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    public string? BlockReasonPrice { get; set; } = string.Empty;

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    public string? BlockReasonQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    public string? BlockReasonQuality { get; set; } = string.Empty;

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    public string? BlockReasonEnhanced { get; set; } = string.Empty;

    /// <summary>
    /// 价值串
    /// </summary>
    public string? ValueString { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string? ConditionType { get; set; } = string.Empty;

    /// <summary>
    /// 总价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前期间年
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    public string? StockManagedMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 文本
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 采购发票主表
    /// （主表：TaktPurchaseInvoice）
    /// </summary>
    public TaktPurchaseInvoiceDto? PurchaseInvoice { get; set; }

}

// ========================================
// PurchaseInvoiceItem 查询 DTO
// ========================================

/// <summary>
/// PurchaseInvoiceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInvoiceItemQueryDto : TaktPagedQuery
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
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    public string? AccountAssignmentSeq { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估范围
    /// </summary>
    public string? ValuationArea { get; set; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 总库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 评估类
    /// </summary>
    public string? ValuationClass { get; set; } = string.Empty;

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    public string? UpdatePoHistoryFlag { get; set; } = string.Empty;

    /// <summary>
    /// 后续借/贷
    /// </summary>
    public string? SubsequentDebitCredit { get; set; } = string.Empty;

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    public string? BlockReasonPrice { get; set; } = string.Empty;

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    public string? BlockReasonQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    public string? BlockReasonQuality { get; set; } = string.Empty;

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    public string? BlockReasonEnhanced { get; set; } = string.Empty;

    /// <summary>
    /// 价值串
    /// </summary>
    public string? ValueString { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string? ConditionType { get; set; } = string.Empty;

    /// <summary>
    /// 总价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前期间年
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    public string? StockManagedMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 文本
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建PurchaseInvoiceItem DTO
// ========================================

/// <summary>
/// 创建PurchaseInvoiceItem DTO
/// </summary>
public class TaktPurchaseInvoiceItemCreateDto
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
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    public string? AccountAssignmentSeq { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估范围
    /// </summary>
    public string? ValuationArea { get; set; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 总库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 评估类
    /// </summary>
    public string? ValuationClass { get; set; } = string.Empty;

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    public string? UpdatePoHistoryFlag { get; set; } = string.Empty;

    /// <summary>
    /// 后续借/贷
    /// </summary>
    public string? SubsequentDebitCredit { get; set; } = string.Empty;

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    public string? BlockReasonPrice { get; set; } = string.Empty;

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    public string? BlockReasonQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    public string? BlockReasonQuality { get; set; } = string.Empty;

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    public string? BlockReasonEnhanced { get; set; } = string.Empty;

    /// <summary>
    /// 价值串
    /// </summary>
    public string? ValueString { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string? ConditionType { get; set; } = string.Empty;

    /// <summary>
    /// 总价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前期间年
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    public string? StockManagedMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 文本
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新PurchaseInvoiceItem DTO
// ========================================

/// <summary>
/// 更新PurchaseInvoiceItem DTO
/// 继承 TaktPurchaseInvoiceItemCreateDto，添加 PurchaseInvoiceItemId 字段
/// </summary>
public class TaktPurchaseInvoiceItemUpdateDto : TaktPurchaseInvoiceItemCreateDto
{
    /// <summary>
    /// PurchaseInvoiceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

}

// ========================================
// PurchaseInvoiceItem 作废 DTO
// ========================================

/// <summary>
/// PurchaseInvoiceItem 作废/撤销作废 DTO
/// </summary>
public class TaktPurchaseInvoiceItemObsoleteDto
{
    /// <summary>
    /// PurchaseInvoiceItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInvoiceItem 导入模板行 DTO
/// </summary>
public class TaktPurchaseInvoiceItemTemplateDto
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
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    public string? AccountAssignmentSeq { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估范围
    /// </summary>
    public string? ValuationArea { get; set; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 总库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 评估类
    /// </summary>
    public string? ValuationClass { get; set; } = string.Empty;

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    public string? UpdatePoHistoryFlag { get; set; } = string.Empty;

    /// <summary>
    /// 后续借/贷
    /// </summary>
    public string? SubsequentDebitCredit { get; set; } = string.Empty;

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    public string? BlockReasonPrice { get; set; } = string.Empty;

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    public string? BlockReasonQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    public string? BlockReasonQuality { get; set; } = string.Empty;

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    public string? BlockReasonEnhanced { get; set; } = string.Empty;

    /// <summary>
    /// 价值串
    /// </summary>
    public string? ValueString { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string? ConditionType { get; set; } = string.Empty;

    /// <summary>
    /// 总价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前期间年
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    public string? StockManagedMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 文本
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// PurchaseInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInvoiceItemImportDto
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
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    public string? AccountAssignmentSeq { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估范围
    /// </summary>
    public string? ValuationArea { get; set; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 总库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 评估类
    /// </summary>
    public string? ValuationClass { get; set; } = string.Empty;

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    public string? UpdatePoHistoryFlag { get; set; } = string.Empty;

    /// <summary>
    /// 后续借/贷
    /// </summary>
    public string? SubsequentDebitCredit { get; set; } = string.Empty;

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    public string? BlockReasonPrice { get; set; } = string.Empty;

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    public string? BlockReasonQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    public string? BlockReasonQuality { get; set; } = string.Empty;

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    public string? BlockReasonEnhanced { get; set; } = string.Empty;

    /// <summary>
    /// 价值串
    /// </summary>
    public string? ValueString { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string? ConditionType { get; set; } = string.Empty;

    /// <summary>
    /// 总价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前期间年
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    public string? StockManagedMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 文本
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// PurchaseInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInvoiceItemExportDto
{
    /// <summary>
    /// PurchaseInvoiceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceItemId { get; set; }

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
    /// 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 凭证编号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（采购凭证项目）
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 科目分配序号
    /// </summary>
    public string? AccountAssignmentSeq { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估范围
    /// </summary>
    public string? ValuationArea { get; set; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 订单单位
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 总库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 上一过账期间库存
    /// </summary>
    public decimal? PreviousPeriodStock { get; set; }

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 评估类
    /// </summary>
    public string? ValuationClass { get; set; } = string.Empty;

    /// <summary>
    /// 标识: 更新采购订单历史
    /// </summary>
    public string? UpdatePoHistoryFlag { get; set; } = string.Empty;

    /// <summary>
    /// 后续借/贷
    /// </summary>
    public string? SubsequentDebitCredit { get; set; } = string.Empty;

    /// <summary>
    /// 价格冻结原因
    /// </summary>
    public string? BlockReasonPrice { get; set; } = string.Empty;

    /// <summary>
    /// 数量冻结原因
    /// </summary>
    public string? BlockReasonQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 质量冻结原因
    /// </summary>
    public string? BlockReasonQuality { get; set; } = string.Empty;

    /// <summary>
    /// 增强冻结原因
    /// </summary>
    public string? BlockReasonEnhanced { get; set; } = string.Empty;

    /// <summary>
    /// 价值串
    /// </summary>
    public string? ValueString { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string? ConditionType { get; set; } = string.Empty;

    /// <summary>
    /// 总价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 前期总值
    /// </summary>
    public decimal? PreviousPeriodValue { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前期间年
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 库存物料
    /// </summary>
    public string? StockManagedMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 文本
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 来自到达的发票的存货过帐行
    /// </summary>
    public int? MaterialDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
