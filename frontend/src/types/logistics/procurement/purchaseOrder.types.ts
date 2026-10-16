// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseOrder.types.ts
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
 * Takt采购订单实体
 * 对应前端 TaktPurchaseOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseOrder
 * @description 对应后端 TaktPurchaseOrderDto
 */
export interface PurchaseOrder extends CompanyDtoBase {
  /**
   * PurchaseOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseOrderId: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请 名称（填充字段）
   */
  purchaseRequestName?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
   */
  purchaseOrderType?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate: number;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount: number;

  /**
   * 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细） （子表：TaktPurchaseOrderItem）
   */
  items?: PurchaseOrderItem[];

}


/**
 * PurchaseOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseOrderQuery
 * @description 对应后端 TaktPurchaseOrderQueryDto
 */
export interface PurchaseOrderQuery extends TaktPagedQuery {
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
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  orderDateEnd?: string;

  /**
   * 要求到货日期（范围查询-开始）
   */
  requiredArrivalDateStart?: string;

  /**
   * 要求到货日期（范围查询-结束）
   */
  requiredArrivalDateEnd?: string;

  /**
   * 实际到货日期（范围查询-开始）
   */
  actualArrivalDateStart?: string;

  /**
   * 实际到货日期（范围查询-结束）
   */
  actualArrivalDateEnd?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
   */
  purchaseOrderType?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount?: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount?: number;

  /**
   * 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

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
 * 创建PurchaseOrder DTO
 * 对应前端 PurchaseOrderCreate
 * @description 对应后端 TaktPurchaseOrderCreateDto
 */
export interface PurchaseOrderCreate {
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
   * 采购订单编码（唯一索引；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）
   */
  purchaseOrderCode: string;

  /**
   * 编码规则编码（新增时选择采购订单编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
   */
  purchaseOrderType?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate: number;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount: number;

  /**
   * 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseOrderItemCreate[];

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
 * 更新PurchaseOrder DTO
 * 继承 TaktPurchaseOrderCreateDto，添加 PurchaseOrderId 字段
 * 对应前端 PurchaseOrderUpdate
 * @description 对应后端 TaktPurchaseOrderUpdateDto
 */
export interface PurchaseOrderUpdate extends PurchaseOrderCreate {
  /**
   * PurchaseOrderID（标识要更新的实体）
   */
  purchaseOrderId: string;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: any;

}


/**
 * PurchaseOrder 状态更新 DTO
 * 对应前端 PurchaseOrderStatus
 * @description 对应后端 TaktPurchaseOrderStatusDto
 */
export interface PurchaseOrderStatus {
  /**
   * PurchaseOrderID
   */
  purchaseOrderId: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus: number;

}


/**
 * PurchaseOrder 导入模板行 DTO
 * 对应前端 PurchaseOrderTemplate
 * @description 对应后端 TaktPurchaseOrderTemplateDto
 */
export interface PurchaseOrderTemplate {
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
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
   */
  purchaseOrderType?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount?: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount?: number;

  /**
   * 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseOrderItemCreate[];

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
 * PurchaseOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseOrderImport
 * @description 对应后端 TaktPurchaseOrderImportDto
 */
export interface PurchaseOrderImport {
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
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
   */
  purchaseOrderType?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount?: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount?: number;

  /**
   * 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseOrderItemCreate[];

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
 * PurchaseOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseOrderExport
 * @description 对应后端 TaktPurchaseOrderExportDto
 */
export interface PurchaseOrderExport {
  /**
   * PurchaseOrderID
   */
  purchaseOrderId: string;

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
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）
   */
  purchaseOrderType?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate: number;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount: number;

  /**
   * 支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

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

