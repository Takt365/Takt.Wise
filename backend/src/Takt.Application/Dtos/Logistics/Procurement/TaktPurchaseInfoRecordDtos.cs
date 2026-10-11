// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordDtos.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInfoRecord 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInfoRecord 生成，请按需审阅）
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
// PurchaseInfoRecord 响应 DTO
// ========================================

/// <summary>
/// Takt采购信息记录实体（定价记录；条件类型 + 供应商 + 物料 + 有效期；含子表 Items）
/// 对应前端 TaktPurchaseInfoRecordDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInfoRecordDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInfoRecordID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInfoRecordId { get; set; }

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

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
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）
    /// （子表：TaktPurchaseInfoRecordItem）
    /// </summary>
    public List<TaktPurchaseInfoRecordItemDto>? Items { get; set; }

}

// ========================================
// PurchaseInfoRecord 查询 DTO
// ========================================

/// <summary>
/// PurchaseInfoRecord 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInfoRecordQueryDto : TaktPagedQuery
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
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string? PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 有效起始日（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 有效起始日（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 有效截至日（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 有效截至日（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

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
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

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
// 创建PurchaseInfoRecord DTO
// ========================================

/// <summary>
/// 创建PurchaseInfoRecord DTO
/// </summary>
public class TaktPurchaseInfoRecordCreateDto
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
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（唯一索引；长度 20）不能为空")]
    public string PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    [Required(ErrorMessage = "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）不能为空")]
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [Required(ErrorMessage = "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

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
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInfoRecordItemCreateDto>? Items { get; set; }

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
// 更新PurchaseInfoRecord DTO
// ========================================

/// <summary>
/// 更新PurchaseInfoRecord DTO
/// 继承 TaktPurchaseInfoRecordCreateDto，添加 PurchaseInfoRecordId 字段
/// </summary>
public class TaktPurchaseInfoRecordUpdateDto : TaktPurchaseInfoRecordCreateDto
{
    /// <summary>
    /// PurchaseInfoRecordID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInfoRecordId { get; set; }

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseInfoRecordItemUpdateDto>? Items { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInfoRecord 导入模板行 DTO
/// </summary>
public class TaktPurchaseInfoRecordTemplateDto
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
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string? PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime? ValidTo { get; set; }

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
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInfoRecordItemCreateDto>? Items { get; set; }

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
/// PurchaseInfoRecord 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInfoRecordImportDto
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
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string? PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime? ValidTo { get; set; }

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
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInfoRecordItemCreateDto>? Items { get; set; }

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
/// PurchaseInfoRecord 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInfoRecordExportDto
{
    /// <summary>
    /// PurchaseInfoRecordID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInfoRecordId { get; set; }

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
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string PurchaseInfoRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

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
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

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
