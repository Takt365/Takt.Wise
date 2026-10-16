// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：order.types.ts
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
 * Takt销售订单实体
 * 对应前端 TaktSalesOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesOrder
 * @description 对应后端 TaktSalesOrderDto
 */
export interface SalesOrder extends CompanyDtoBase {
  /**
   * SalesOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesOrderId: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
   */
  salesOrderType?: string;

  /**
   * 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
   */
  orderReason?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
   */
  invoiceType?: string;

  /**
   * 采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 订单日期
   */
  purchaseOrderDate?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细） （子表：TaktSalesOrderItem）
   */
  items?: SalesOrderItem[];

}


/**
 * SalesOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesOrderQuery
 * @description 对应后端 TaktSalesOrderQueryDto
 */
export interface SalesOrderQuery extends TaktPagedQuery {
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
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  orderDateEnd?: string;

  /**
   * 要求交货日期（范围查询-开始）
   */
  requiredDeliveryDateStart?: string;

  /**
   * 要求交货日期（范围查询-结束）
   */
  requiredDeliveryDateEnd?: string;

  /**
   * 实际交货日期（范围查询-开始）
   */
  actualDeliveryDateStart?: string;

  /**
   * 实际交货日期（范围查询-结束）
   */
  actualDeliveryDateEnd?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
   */
  salesOrderType?: string;

  /**
   * 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
   */
  orderReason?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
   */
  invoiceType?: string;

  /**
   * 采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  purchaseOrderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  purchaseOrderDateEnd?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount?: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
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
 * 创建SalesOrder DTO
 * 对应前端 SalesOrderCreate
 * @description 对应后端 TaktSalesOrderCreateDto
 */
export interface SalesOrderCreate {
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
   * 销售订单编码（唯一索引；前端表单选择编码规则后自动通过 TaktNumbering 销售订单编码规则生成并展示，非手输；单据类型菜单：销售订单）
   */
  salesOrderCode: string;

  /**
   * 编码规则编码（新增时选择销售订单编码规则后取号；不落库）
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
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
   */
  salesOrderType?: string;

  /**
   * 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
   */
  orderReason?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
   */
  invoiceType?: string;

  /**
   * 采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 订单日期
   */
  purchaseOrderDate?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: SalesOrderItemCreate[];

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
 * 更新SalesOrder DTO
 * 继承 TaktSalesOrderCreateDto，添加 SalesOrderId 字段
 * 对应前端 SalesOrderUpdate
 * @description 对应后端 TaktSalesOrderUpdateDto
 */
export interface SalesOrderUpdate extends SalesOrderCreate {
  /**
   * SalesOrderID（标识要更新的实体）
   */
  salesOrderId: string;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: any;

}


/**
 * SalesOrder 状态更新 DTO
 * 对应前端 SalesOrderStatus
 * @description 对应后端 TaktSalesOrderStatusDto
 */
export interface SalesOrderStatus {
  /**
   * SalesOrderID
   */
  salesOrderId: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus: number;

}


/**
 * SalesOrder 导入模板行 DTO
 * 对应前端 SalesOrderTemplate
 * @description 对应后端 TaktSalesOrderTemplateDto
 */
export interface SalesOrderTemplate {
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
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
   */
  salesOrderType?: string;

  /**
   * 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
   */
  orderReason?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
   */
  invoiceType?: string;

  /**
   * 采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 订单日期
   */
  purchaseOrderDate?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount?: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: SalesOrderItemCreate[];

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
 * SalesOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesOrderImport
 * @description 对应后端 TaktSalesOrderImportDto
 */
export interface SalesOrderImport {
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
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
   */
  salesOrderType?: string;

  /**
   * 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
   */
  orderReason?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
   */
  invoiceType?: string;

  /**
   * 采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 订单日期
   */
  purchaseOrderDate?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount?: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: SalesOrderItemCreate[];

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
 * SalesOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesOrderExport
 * @description 对应后端 TaktSalesOrderExportDto
 */
export interface SalesOrderExport {
  /**
   * SalesOrderID
   */
  salesOrderId: string;

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
   * 销售订单编码（唯一索引）
   */
  salesOrderCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 销售订单类型（字典 logistics_sales_order_type；与销售报价共用；DictValue=AG/QT/AEBQ/ZQT/Z800/Z801/Z850/Z851/ZCR/ZDR/ZOR/ZOR1；ExtLabel=凭证类别 A询价/B报价/C订单/H退货/K贷项/L借项）
   */
  salesOrderType?: string;

  /**
   * 订单原因（字典 logistics_sales_order_reason；DictValue=001～008/100～105/200）
   */
  orderReason?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 定价过程（字典 logistics_sales_pricing_procedure；DictValue=Z10010～Z91001/ZCAA01/ZVAA97/ZVAA98/ZVAA99；ExtLabel=A；ExtValue=V；默认 ZVAA99）
   */
  pricingProcedure?: string;

  /**
   * 定价条件编码
   */
  pricingConditionCode?: string;

  /**
   * 发票类型（字典 logistics_sales_invoice_type；DictValue=B1/F2/G2/RE 等）
   */
  invoiceType?: string;

  /**
   * 采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 订单日期
   */
  purchaseOrderDate?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 交货方式（字典 logistics_sales_delivery_method；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）
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

