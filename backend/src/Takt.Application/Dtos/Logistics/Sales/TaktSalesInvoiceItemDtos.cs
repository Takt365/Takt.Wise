// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesInvoiceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesInvoiceItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesInvoiceItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售发票明细实体（公司级；主子表关系见 SalesInvoiceId）
/// 对应前端 TaktSalesInvoiceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesInvoiceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

    /// <summary>
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 销售发票名称（填充字段）
    /// </summary>
    public string? SalesInvoiceName { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 已出发票数量
    /// </summary>
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期
    /// </summary>
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 提供服务日期
    /// </summary>
    public DateTime? ServiceRenderedDate { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    public string? ReferenceDocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证
    /// </summary>
    public string? SalesDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    public string? SalesDocumentReferenceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 项目类别
    /// </summary>
    public string? SalesItemCategory { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    public string? ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    public string? PlantRegion { get; set; } = string.Empty;

    /// <summary>
    /// 定价
    /// </summary>
    public string? PricingFlag { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 信贷价格
    /// </summary>
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    public string? CustomerGroupSalesOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DestinationCountryOrder { get; set; } = string.Empty;

    /// <summary>
    /// 地区订单
    /// </summary>
    public string? RegionOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    public string? SalesOrganizationOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    public string? DistributionChannelOrder { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 销售发票主表
    /// （主表：TaktSalesInvoice）
    /// </summary>
    public TaktSalesInvoiceDto? SalesInvoice { get; set; }

}

// ========================================
// SalesInvoiceItem 查询 DTO
// ========================================

/// <summary>
/// SalesInvoiceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesInvoiceItemQueryDto : TaktPagedQuery
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
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 已出发票数量
    /// </summary>
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期（范围查询-开始）
    /// </summary>
    public DateTime? PricingDateStart { get; set; }

    /// <summary>
    /// 定价日期（范围查询-结束）
    /// </summary>
    public DateTime? PricingDateEnd { get; set; }

    /// <summary>
    /// 提供服务日期（范围查询-开始）
    /// </summary>
    public DateTime? ServiceRenderedDateStart { get; set; }

    /// <summary>
    /// 提供服务日期（范围查询-结束）
    /// </summary>
    public DateTime? ServiceRenderedDateEnd { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    public string? ReferenceDocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证
    /// </summary>
    public string? SalesDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    public string? SalesDocumentReferenceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 项目类别
    /// </summary>
    public string? SalesItemCategory { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    public string? ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    public string? PlantRegion { get; set; } = string.Empty;

    /// <summary>
    /// 定价
    /// </summary>
    public string? PricingFlag { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 信贷价格
    /// </summary>
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    public string? CustomerGroupSalesOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DestinationCountryOrder { get; set; } = string.Empty;

    /// <summary>
    /// 地区订单
    /// </summary>
    public string? RegionOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    public string? SalesOrganizationOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    public string? DistributionChannelOrder { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期（范围查询-开始）
    /// </summary>
    public DateTime? ExchangeRateDateStart { get; set; }

    /// <summary>
    /// 换算日期（范围查询-结束）
    /// </summary>
    public DateTime? ExchangeRateDateEnd { get; set; }

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
// 创建SalesInvoiceItem DTO
// ========================================

/// <summary>
/// 创建SalesInvoiceItem DTO
/// </summary>
public class TaktSalesInvoiceItemCreateDto
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
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [Required(ErrorMessage = "开票凭证（冗余：按对应 Id 取主数据名称联动）不能为空")]
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 已出发票数量
    /// </summary>
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期
    /// </summary>
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 提供服务日期
    /// </summary>
    public DateTime? ServiceRenderedDate { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    public string? ReferenceDocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证
    /// </summary>
    public string? SalesDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    public string? SalesDocumentReferenceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 项目类别
    /// </summary>
    public string? SalesItemCategory { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    public string? ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    public string? PlantRegion { get; set; } = string.Empty;

    /// <summary>
    /// 定价
    /// </summary>
    public string? PricingFlag { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 信贷价格
    /// </summary>
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    public string? CustomerGroupSalesOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DestinationCountryOrder { get; set; } = string.Empty;

    /// <summary>
    /// 地区订单
    /// </summary>
    public string? RegionOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    public string? SalesOrganizationOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    public string? DistributionChannelOrder { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

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
// 更新SalesInvoiceItem DTO
// ========================================

/// <summary>
/// 更新SalesInvoiceItem DTO
/// 继承 TaktSalesInvoiceItemCreateDto，添加 SalesInvoiceItemId 字段
/// </summary>
public class TaktSalesInvoiceItemUpdateDto : TaktSalesInvoiceItemCreateDto
{
    /// <summary>
    /// SalesInvoiceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

}

// ========================================
// SalesInvoiceItem 作废 DTO
// ========================================

/// <summary>
/// SalesInvoiceItem 作废/撤销作废 DTO
/// </summary>
public class TaktSalesInvoiceItemObsoleteDto
{
    /// <summary>
    /// SalesInvoiceItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesInvoiceItem 导入模板行 DTO
/// </summary>
public class TaktSalesInvoiceItemTemplateDto
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
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 已出发票数量
    /// </summary>
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期
    /// </summary>
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 提供服务日期
    /// </summary>
    public DateTime? ServiceRenderedDate { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    public string? ReferenceDocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证
    /// </summary>
    public string? SalesDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    public string? SalesDocumentReferenceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 项目类别
    /// </summary>
    public string? SalesItemCategory { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    public string? ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    public string? PlantRegion { get; set; } = string.Empty;

    /// <summary>
    /// 定价
    /// </summary>
    public string? PricingFlag { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 信贷价格
    /// </summary>
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    public string? CustomerGroupSalesOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DestinationCountryOrder { get; set; } = string.Empty;

    /// <summary>
    /// 地区订单
    /// </summary>
    public string? RegionOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    public string? SalesOrganizationOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    public string? DistributionChannelOrder { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

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
/// SalesInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesInvoiceItemImportDto
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
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 已出发票数量
    /// </summary>
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期
    /// </summary>
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 提供服务日期
    /// </summary>
    public DateTime? ServiceRenderedDate { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    public string? ReferenceDocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证
    /// </summary>
    public string? SalesDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    public string? SalesDocumentReferenceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 项目类别
    /// </summary>
    public string? SalesItemCategory { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    public string? ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    public string? PlantRegion { get; set; } = string.Empty;

    /// <summary>
    /// 定价
    /// </summary>
    public string? PricingFlag { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 信贷价格
    /// </summary>
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    public string? CustomerGroupSalesOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DestinationCountryOrder { get; set; } = string.Empty;

    /// <summary>
    /// 地区订单
    /// </summary>
    public string? RegionOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    public string? SalesOrganizationOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    public string? DistributionChannelOrder { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

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
/// SalesInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesInvoiceItemExportDto
{
    /// <summary>
    /// SalesInvoiceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

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
    /// 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 已出发票数量
    /// </summary>
    public decimal? BillingQuantity { get; set; }

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 库存单位开票数量
    /// </summary>
    public decimal? BillingQuantitySku { get; set; }

    /// <summary>
    /// 净重量
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 重量单位
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期
    /// </summary>
    public DateTime? PricingDate { get; set; }

    /// <summary>
    /// 提供服务日期
    /// </summary>
    public DateTime? ServiceRenderedDate { get; set; }

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? PricingExchangeRate { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 先前凭证类别
    /// </summary>
    public string? ReferenceDocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证
    /// </summary>
    public string? SalesDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售凭证项目
    /// </summary>
    public int? SalesDocumentItem { get; set; }

    /// <summary>
    /// 销售凭证参考
    /// </summary>
    public string? SalesDocumentReferenceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 项目类别
    /// </summary>
    public string? SalesItemCategory { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（最长 18，故 Length=18）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 装运点/接收点
    /// </summary>
    public string? ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 合作伙伴项目
    /// </summary>
    public int? PartnerItem { get; set; }

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂地区
    /// </summary>
    public string? PlantRegion { get; set; } = string.Empty;

    /// <summary>
    /// 定价
    /// </summary>
    public string? PricingFlag { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal? CostAmount { get; set; }

    /// <summary>
    /// 小计1
    /// </summary>
    public decimal? Subtotal1 { get; set; }

    /// <summary>
    /// 小计2
    /// </summary>
    public decimal? Subtotal2 { get; set; }

    /// <summary>
    /// 小计3
    /// </summary>
    public decimal? Subtotal3 { get; set; }

    /// <summary>
    /// 小计4
    /// </summary>
    public decimal? Subtotal4 { get; set; }

    /// <summary>
    /// 小计5
    /// </summary>
    public decimal? Subtotal5 { get; set; }

    /// <summary>
    /// 小计6
    /// </summary>
    public decimal? Subtotal6 { get; set; }

    /// <summary>
    /// 汇率统计
    /// </summary>
    public decimal? StatisticsExchangeRate { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 信贷价格
    /// </summary>
    public decimal? CreditPrice { get; set; }

    /// <summary>
    /// 客户组销售订单
    /// </summary>
    public string? CustomerGroupSalesOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? DestinationCountryOrder { get; set; } = string.Empty;

    /// <summary>
    /// 地区订单
    /// </summary>
    public string? RegionOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单的销售机构
    /// </summary>
    public string? SalesOrganizationOrder { get; set; } = string.Empty;

    /// <summary>
    /// 订单分销渠道
    /// </summary>
    public string? DistributionChannelOrder { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 税额
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 总值
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

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
