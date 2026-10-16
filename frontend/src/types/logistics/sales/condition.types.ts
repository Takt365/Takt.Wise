// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：condition.types.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt销售条件实体（定价记录；条件类型 + 客户 + 物料 + 有效期；含子表 Items）
 * 对应前端 TaktSalesConditionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesCondition
 * @description 对应后端 TaktSalesConditionDto
 */
export interface SalesCondition extends CompanyDtoBase {
  /**
   * SalesConditionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesConditionId: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  salesConditionCode: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

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
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价 名称（填充字段）
   */
  salesQuotationName?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系） （子表：TaktSalesConditionItem）
   */
  items?: SalesConditionItem[];

}


/**
 * SalesCondition 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesConditionQuery
 * @description 对应后端 TaktSalesConditionQueryDto
 */
export interface SalesConditionQuery extends TaktPagedQuery {
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
  salesConditionCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

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
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

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
 * 创建SalesCondition DTO
 * 对应前端 SalesConditionCreate
 * @description 对应后端 TaktSalesConditionCreateDto
 */
export interface SalesConditionCreate {
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
   * 定价记录号（唯一索引；长度 20；前端表单选择编码规则后自动通过 TaktNumbering 定价记录号规则生成并展示，非手输；单据类型菜单：销售条件）
   */
  salesConditionCode: string;

  /**
   * 编码规则编码（新增时选择销售条件编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

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
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: SalesConditionItemCreate[];

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
 * 更新SalesCondition DTO
 * 继承 TaktSalesConditionCreateDto，添加 SalesConditionId 字段
 * 对应前端 SalesConditionUpdate
 * @description 对应后端 TaktSalesConditionUpdateDto
 */
export interface SalesConditionUpdate extends SalesConditionCreate {
  /**
   * SalesConditionID（标识要更新的实体）
   */
  salesConditionId: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * SalesCondition 导入模板行 DTO
 * 对应前端 SalesConditionTemplate
 * @description 对应后端 TaktSalesConditionTemplateDto
 */
export interface SalesConditionTemplate {
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
  salesConditionCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

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
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: SalesConditionItemCreate[];

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
 * SalesCondition 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesConditionImport
 * @description 对应后端 TaktSalesConditionImportDto
 */
export interface SalesConditionImport {
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
  salesConditionCode?: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

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
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: SalesConditionItemCreate[];

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
 * SalesCondition 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesConditionExport
 * @description 对应后端 TaktSalesConditionExportDto
 */
export interface SalesConditionExport {
  /**
   * SalesConditionID
   */
  salesConditionId: string;

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
  salesConditionCode: string;

  /**
   * 条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

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
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

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

