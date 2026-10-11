// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseOrderItem.types.ts
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
 * Takt采购订单明细实体
 * 对应前端 TaktPurchaseOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseOrderItem
 * @description 对应后端 TaktPurchaseOrderItemDto
 */
export interface PurchaseOrderItem extends CompanyDtoBase {
  /**
   * PurchaseOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseOrderItemId: string;

  /**
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
   */
  purchaseOrderId: string;

  /**
   * 采购订单 名称（填充字段）
   */
  purchaseOrderName?: string;

  /**
   * 采购订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购信息记录编码
   */
  purchaseInfoRecordCode?: string;

  /**
   * 供应商物料编码
   */
  supplierMaterialCode?: string;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice: number;

  /**
   * 折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税码（冗余：按 PurchaseOrderId 取 TaktPurchaseOrder.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 采购金额
   */
  purchaseAmount: number;

  /**
   * 价格日期
   */
  pricingDate?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseOrderItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseOrderItemQuery
 * @description 对应后端 TaktPurchaseOrderItemQueryDto
 */
export interface PurchaseOrderItemQuery extends TaktPagedQuery {
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
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
   */
  purchaseOrderId?: string;

  /**
   * 采购订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购信息记录编码
   */
  purchaseInfoRecordCode?: string;

  /**
   * 供应商物料编码
   */
  supplierMaterialCode?: string;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 税码（冗余：按 PurchaseOrderId 取 TaktPurchaseOrder.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 采购金额
   */
  purchaseAmount?: number;

  /**
   * 价格日期（范围查询-开始）
   */
  pricingDateStart?: string;

  /**
   * 价格日期（范围查询-结束）
   */
  pricingDateEnd?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建PurchaseOrderItem DTO
 * 对应前端 PurchaseOrderItemCreate
 * @description 对应后端 TaktPurchaseOrderItemCreateDto
 */
export interface PurchaseOrderItemCreate {
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
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
   */
  purchaseOrderId: string;

  /**
   * 采购订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购信息记录编码
   */
  purchaseInfoRecordCode?: string;

  /**
   * 供应商物料编码
   */
  supplierMaterialCode?: string;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice: number;

  /**
   * 折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税码（冗余：按 PurchaseOrderId 取 TaktPurchaseOrder.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 采购金额
   */
  purchaseAmount: number;

  /**
   * 价格日期
   */
  pricingDate?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新PurchaseOrderItem DTO
 * 继承 TaktPurchaseOrderItemCreateDto，添加 PurchaseOrderItemId 字段
 * 对应前端 PurchaseOrderItemUpdate
 * @description 对应后端 TaktPurchaseOrderItemUpdateDto
 */
export interface PurchaseOrderItemUpdate extends PurchaseOrderItemCreate {
  /**
   * PurchaseOrderItemID（标识要更新的实体）
   */
  purchaseOrderItemId: string;

}


/**
 * PurchaseOrderItem 状态更新 DTO
 * 对应前端 PurchaseOrderItemStatus
 * @description 对应后端 TaktPurchaseOrderItemStatusDto
 */
export interface PurchaseOrderItemStatus {
  /**
   * PurchaseOrderItemID
   */
  purchaseOrderItemId: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

}


/**
 * PurchaseOrderItem 作废/撤销作废 DTO
 * 对应前端 PurchaseOrderItemObsolete
 * @description 对应后端 TaktPurchaseOrderItemObsoleteDto
 */
export interface PurchaseOrderItemObsolete {
  /**
   * PurchaseOrderItemID
   */
  purchaseOrderItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseOrderItem 导入模板行 DTO
 * 对应前端 PurchaseOrderItemTemplate
 * @description 对应后端 TaktPurchaseOrderItemTemplateDto
 */
export interface PurchaseOrderItemTemplate {
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
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
   */
  purchaseOrderId?: string;

  /**
   * 采购订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购信息记录编码
   */
  purchaseInfoRecordCode?: string;

  /**
   * 供应商物料编码
   */
  supplierMaterialCode?: string;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 税码（冗余：按 PurchaseOrderId 取 TaktPurchaseOrder.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 采购金额
   */
  purchaseAmount?: number;

  /**
   * 价格日期
   */
  pricingDate?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * PurchaseOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseOrderItemImport
 * @description 对应后端 TaktPurchaseOrderItemImportDto
 */
export interface PurchaseOrderItemImport {
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
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
   */
  purchaseOrderId?: string;

  /**
   * 采购订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购信息记录编码
   */
  purchaseInfoRecordCode?: string;

  /**
   * 供应商物料编码
   */
  supplierMaterialCode?: string;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 税码（冗余：按 PurchaseOrderId 取 TaktPurchaseOrder.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 采购金额
   */
  purchaseAmount?: number;

  /**
   * 价格日期
   */
  pricingDate?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * PurchaseOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseOrderItemExport
 * @description 对应后端 TaktPurchaseOrderItemExportDto
 */
export interface PurchaseOrderItemExport {
  /**
   * PurchaseOrderItemID
   */
  purchaseOrderItemId: string;

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
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
   */
  purchaseOrderId: string;

  /**
   * 采购订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购信息记录编码
   */
  purchaseInfoRecordCode?: string;

  /**
   * 供应商物料编码
   */
  supplierMaterialCode?: string;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice: number;

  /**
   * 折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税码（冗余：按 PurchaseOrderId 取 TaktPurchaseOrder.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 采购金额
   */
  purchaseAmount: number;

  /**
   * 价格日期
   */
  pricingDate?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 行交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

