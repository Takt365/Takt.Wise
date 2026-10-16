// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseRequestItem.types.ts
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
 * Takt采购申请明细实体
 * 对应前端 TaktPurchaseRequestItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseRequestItem
 * @description 对应后端 TaktPurchaseRequestItemDto
 */
export interface PurchaseRequestItem extends CompanyDtoBase {
  /**
   * PurchaseRequestItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseRequestItemId: string;

  /**
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId: string;

  /**
   * 采购申请 名称（填充字段）
   */
  purchaseRequestName?: string;

  /**
   * 采购申请编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseRequestCode: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 来源采购计划明细 名称（填充字段）
   */
  purchasePlanItemName?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

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
   * 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice: number;

  /**
   * 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）
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
   * 请购金额
   */
  requestAmount: number;

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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseRequestItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseRequestItemQuery
 * @description 对应后端 TaktPurchaseRequestItemQueryDto
 */
export interface PurchaseRequestItemQuery extends TaktPagedQuery {
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
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseRequestCode?: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

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
   * 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice?: number;

  /**
   * 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）
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
   * 请购金额
   */
  requestAmount?: number;

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
 * 创建PurchaseRequestItem DTO
 * 对应前端 PurchaseRequestItemCreate
 * @description 对应后端 TaktPurchaseRequestItemCreateDto
 */
export interface PurchaseRequestItemCreate {
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
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId: string;

  /**
   * 采购申请编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseRequestCode: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

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
   * 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice: number;

  /**
   * 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）
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
   * 请购金额
   */
  requestAmount: number;

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
 * 更新PurchaseRequestItem DTO
 * 继承 TaktPurchaseRequestItemCreateDto，添加 PurchaseRequestItemId 字段
 * 对应前端 PurchaseRequestItemUpdate
 * @description 对应后端 TaktPurchaseRequestItemUpdateDto
 */
export interface PurchaseRequestItemUpdate extends PurchaseRequestItemCreate {
  /**
   * PurchaseRequestItemID（标识要更新的实体）
   */
  purchaseRequestItemId: string;

}


/**
 * PurchaseRequestItem 作废/撤销作废 DTO
 * 对应前端 PurchaseRequestItemObsolete
 * @description 对应后端 TaktPurchaseRequestItemObsoleteDto
 */
export interface PurchaseRequestItemObsolete {
  /**
   * PurchaseRequestItemID
   */
  purchaseRequestItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseRequestItem 导入模板行 DTO
 * 对应前端 PurchaseRequestItemTemplate
 * @description 对应后端 TaktPurchaseRequestItemTemplateDto
 */
export interface PurchaseRequestItemTemplate {
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
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseRequestCode?: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

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
   * 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice?: number;

  /**
   * 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）
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
   * 请购金额
   */
  requestAmount?: number;

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
 * PurchaseRequestItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseRequestItemImport
 * @description 对应后端 TaktPurchaseRequestItemImportDto
 */
export interface PurchaseRequestItemImport {
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
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseRequestCode?: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

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
   * 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice?: number;

  /**
   * 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）
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
   * 请购金额
   */
  requestAmount?: number;

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
 * PurchaseRequestItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseRequestItemExport
 * @description 对应后端 TaktPurchaseRequestItemExportDto
 */
export interface PurchaseRequestItemExport {
  /**
   * PurchaseRequestItemID
   */
  purchaseRequestItemId: string;

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
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId: string;

  /**
   * 采购申请编码（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseRequestCode: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

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
   * 申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice: number;

  /**
   * 税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）
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
   * 请购金额
   */
  requestAmount: number;

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

