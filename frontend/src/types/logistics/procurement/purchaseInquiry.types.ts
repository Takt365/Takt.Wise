// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseInquiry.types.ts
// 创建时间：2026-09-04
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
 * 采购询价实体
 * 对应前端 TaktPurchaseInquiryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInquiry
 * @description 对应后端 TaktPurchaseInquiryDto
 */
export interface PurchaseInquiry extends CompanyDtoBase {
  /**
   * PurchaseInquiryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInquiryId: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryEmployeeId?: string;

  /**
   * 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  inquiryEmployeeName?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
   */
  purchaseInquiryType?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

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
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 采购询价明细列表（主子表关系） （子表：TaktPurchaseInquiryItem）
   */
  items?: PurchaseInquiryItem[];

}


/**
 * PurchaseInquiry 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInquiryQuery
 * @description 对应后端 TaktPurchaseInquiryQueryDto
 */
export interface PurchaseInquiryQuery extends TaktPagedQuery {
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
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期（范围查询-开始）
   */
  inquiryDateStart?: string;

  /**
   * 询价日期（范围查询-结束）
   */
  inquiryDateEnd?: string;

  /**
   * 报价截止日期（范围查询-开始）
   */
  quoteDeadlineDateStart?: string;

  /**
   * 报价截止日期（范围查询-结束）
   */
  quoteDeadlineDateEnd?: string;

  /**
   * 询价人员工（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryEmployeeId?: string;

  /**
   * 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  inquiryEmployeeName?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode?: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
   */
  purchaseInquiryType?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

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
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode?: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme?: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

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
 * 创建PurchaseInquiry DTO
 * 对应前端 PurchaseInquiryCreate
 * @description 对应后端 TaktPurchaseInquiryCreateDto
 */
export interface PurchaseInquiryCreate {
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
   * 采购询价编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购询价编码规则生成并展示，非手输；单据类型菜单：采购询价）
   */
  purchaseInquiryCode: string;

  /**
   * 编码规则编码（新增时选择采购询价编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryEmployeeId?: string;

  /**
   * 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  inquiryEmployeeName?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
   */
  purchaseInquiryType?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

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
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * 更新PurchaseInquiry DTO
 * 继承 TaktPurchaseInquiryCreateDto，添加 PurchaseInquiryId 字段
 * 对应前端 PurchaseInquiryUpdate
 * @description 对应后端 TaktPurchaseInquiryUpdateDto
 */
export interface PurchaseInquiryUpdate extends PurchaseInquiryCreate {
  /**
   * PurchaseInquiryID（标识要更新的实体）
   */
  purchaseInquiryId: string;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * PurchaseInquiry 状态更新 DTO
 * 对应前端 PurchaseInquiryStatus
 * @description 对应后端 TaktPurchaseInquiryStatusDto
 */
export interface PurchaseInquiryStatus {
  /**
   * PurchaseInquiryID
   */
  purchaseInquiryId: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus: number;

}


/**
 * PurchaseInquiry 导入模板行 DTO
 * 对应前端 PurchaseInquiryTemplate
 * @description 对应后端 TaktPurchaseInquiryTemplateDto
 */
export interface PurchaseInquiryTemplate {
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
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期
   */
  inquiryDate?: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryEmployeeId?: string;

  /**
   * 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  inquiryEmployeeName?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode?: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
   */
  purchaseInquiryType?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

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
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode?: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme?: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * PurchaseInquiry 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInquiryImport
 * @description 对应后端 TaktPurchaseInquiryImportDto
 */
export interface PurchaseInquiryImport {
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
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期
   */
  inquiryDate?: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryEmployeeId?: string;

  /**
   * 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  inquiryEmployeeName?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode?: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
   */
  purchaseInquiryType?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

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
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode?: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme?: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * PurchaseInquiry 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInquiryExport
 * @description 对应后端 TaktPurchaseInquiryExportDto
 */
export interface PurchaseInquiryExport {
  /**
   * PurchaseInquiryID
   */
  purchaseInquiryId: string;

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
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryEmployeeId?: string;

  /**
   * 询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  inquiryEmployeeName?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）
   */
  purchaseInquiryType?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

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
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

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

