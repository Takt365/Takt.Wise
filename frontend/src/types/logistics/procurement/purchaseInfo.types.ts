// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseInfo.types.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt采购信息实体（定价记录；条件类型 + 供应商 + 物料 + 有效期；含子表 Items）
 * 对应前端 TaktPurchaseInfoDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInfo
 * @description 对应后端 TaktPurchaseInfoDto
 */
export interface PurchaseInfo extends CompanyDtoBase {
  /**
   * PurchaseInfoID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInfoId: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchaseInfoCode: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 有效起始日
   */
  validFrom: string;

  /**
   * 有效截至日
   */
  validTo: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价 名称（填充字段）
   */
  purchaseInquiryName?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系） （子表：TaktPurchaseInfoItem）
   */
  items?: PurchaseInfoItem[];

}


/**
 * PurchaseInfo 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInfoQuery
 * @description 对应后端 TaktPurchaseInfoQueryDto
 */
export interface PurchaseInfoQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchaseInfoCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 有效起始日（范围查询-开始）
   */
  validFromStart?: string;

  /**
   * 有效起始日（范围查询-结束）
   */
  validFromEnd?: string;

  /**
   * 有效截至日（范围查询-开始）
   */
  validToStart?: string;

  /**
   * 有效截至日（范围查询-结束）
   */
  validToEnd?: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PurchaseInfo DTO
 * 对应前端 PurchaseInfoCreate
 * @description 对应后端 TaktPurchaseInfoCreateDto
 */
export interface PurchaseInfoCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 定价记录号（唯一索引；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：采购信息）
   */
  purchaseInfoCode: string;

  /**
   * 编码规则编码（新增时选择采购信息编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 有效起始日
   */
  validFrom: string;

  /**
   * 有效截至日
   */
  validTo: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInfoItemCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新PurchaseInfo DTO
 * 继承 TaktPurchaseInfoCreateDto，添加 PurchaseInfoId 字段
 * 对应前端 PurchaseInfoUpdate
 * @description 对应后端 TaktPurchaseInfoUpdateDto
 */
export interface PurchaseInfoUpdate extends PurchaseInfoCreate {
  /**
   * PurchaseInfoID（标识要更新的实体）
   */
  purchaseInfoId: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * PurchaseInfo 导入模板行 DTO
 * 对应前端 PurchaseInfoTemplate
 * @description 对应后端 TaktPurchaseInfoTemplateDto
 */
export interface PurchaseInfoTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchaseInfoCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 有效起始日
   */
  validFrom?: string;

  /**
   * 有效截至日
   */
  validTo?: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInfoItemCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PurchaseInfo 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInfoImport
 * @description 对应后端 TaktPurchaseInfoImportDto
 */
export interface PurchaseInfoImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchaseInfoCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 有效起始日
   */
  validFrom?: string;

  /**
   * 有效截至日
   */
  validTo?: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInfoItemCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PurchaseInfo 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInfoExport
 * @description 对应后端 TaktPurchaseInfoExportDto
 */
export interface PurchaseInfoExport {
  /**
   * PurchaseInfoID
   */
  purchaseInfoId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchaseInfoCode: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 有效起始日
   */
  validFrom: string;

  /**
   * 有效截至日
   */
  validTo: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

