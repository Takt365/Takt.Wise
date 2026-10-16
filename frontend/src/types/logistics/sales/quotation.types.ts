// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：quotation.types.ts
// 创建时间：2026-09-04
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
 * Takt销售报价实体
 * 对应前端 TaktSalesQuotationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesQuotation
 * @description 对应后端 TaktSalesQuotationDto
 */
export interface SalesQuotation extends CompanyDtoBase {
  /**
   * SalesQuotationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesQuotationId: string;

  /**
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
   */
  salesQuotationType?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus: number;

  /**
   * 销售报价明细列表（主子表关系） （子表：TaktSalesQuotationItem）
   */
  items?: SalesQuotationItem[];

}


/**
 * SalesQuotation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesQuotationQuery
 * @description 对应后端 TaktSalesQuotationQueryDto
 */
export interface SalesQuotationQuery extends TaktPagedQuery {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 报价日期（范围查询-开始）
   */
  quotationDateStart?: string;

  /**
   * 报价日期（范围查询-结束）
   */
  quotationDateEnd?: string;

  /**
   * 报价有效期至（范围查询-开始）
   */
  validUntilDateStart?: string;

  /**
   * 报价有效期至（范围查询-结束）
   */
  validUntilDateEnd?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
   */
  salesQuotationType?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 报价总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 报价实付金额
   */
  actualAmount?: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus?: number;

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
 * 创建SalesQuotation DTO
 * 对应前端 SalesQuotationCreate
 * @description 对应后端 TaktSalesQuotationCreateDto
 */
export interface SalesQuotationCreate {
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
   * 销售报价编码（唯一索引；前端表单选择编码规则后自动通过 TaktNumbering 销售报价编码规则生成并展示，非手输；单据类型菜单：销售报价）
   */
  salesQuotationCode: string;

  /**
   * 编码规则编码（新增时选择销售报价编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
   */
  salesQuotationType?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus: number;

  /**
   * 销售报价明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesQuotationItemCreate[];

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
 * 更新SalesQuotation DTO
 * 继承 TaktSalesQuotationCreateDto，添加 SalesQuotationId 字段
 * 对应前端 SalesQuotationUpdate
 * @description 对应后端 TaktSalesQuotationUpdateDto
 */
export interface SalesQuotationUpdate extends SalesQuotationCreate {
  /**
   * SalesQuotationID（标识要更新的实体）
   */
  salesQuotationId: string;

  /**
   * 销售报价明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * SalesQuotation 状态更新 DTO
 * 对应前端 SalesQuotationStatus
 * @description 对应后端 TaktSalesQuotationStatusDto
 */
export interface SalesQuotationStatus {
  /**
   * SalesQuotationID
   */
  salesQuotationId: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus: number;

}


/**
 * SalesQuotation 导入模板行 DTO
 * 对应前端 SalesQuotationTemplate
 * @description 对应后端 TaktSalesQuotationTemplateDto
 */
export interface SalesQuotationTemplate {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 报价日期
   */
  quotationDate?: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
   */
  salesQuotationType?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 报价总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 报价实付金额
   */
  actualAmount?: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus?: number;

  /**
   * 销售报价明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesQuotationItemCreate[];

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
 * SalesQuotation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesQuotationImport
 * @description 对应后端 TaktSalesQuotationImportDto
 */
export interface SalesQuotationImport {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 报价日期
   */
  quotationDate?: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
   */
  salesQuotationType?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 报价总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 报价实付金额
   */
  actualAmount?: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus?: number;

  /**
   * 销售报价明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesQuotationItemCreate[];

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
 * SalesQuotation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesQuotationExport
 * @description 对应后端 TaktSalesQuotationExportDto
 */
export interface SalesQuotationExport {
  /**
   * SalesQuotationID
   */
  salesQuotationId: string;

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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售报价类型（字典 logistics_sales_order_type；与销售订单共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项；报价默认 AEBQ）
   */
  salesQuotationType?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 报价状态（字典 logistics_sales_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus: number;

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

