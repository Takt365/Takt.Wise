// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialDocumentItem.types.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt物料凭证行项目实体（公司级；主子表关系见 MaterialDocumentId）
 * 对应前端 TaktMaterialDocumentItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialDocumentItem
 * @description 对应后端 TaktMaterialDocumentItemDto
 */
export interface MaterialDocumentItem extends CompanyDtoBase {
  /**
   * MaterialDocumentItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialDocumentItemId: string;

  /**
   * 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
   */
  materialDocumentId: string;

  /**
   * 物料凭证名称（填充字段）
   */
  materialDocumentName?: string;

  /**
   * 物料凭证（冗余：按对应 Id 取主数据名称联动）
   */
  materialDocumentCode: string;

  /**
   * 物料凭证项目（行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 行标识
   */
  lineId?: string;

  /**
   * 行标识
   */
  lineName?: string;

  /**
   * 上级行 ID
   */
  parentLineId?: string;

  /**
   * 上级行 名称（填充字段）
   */
  parentLineName?: string;

  /**
   * 层次结构级别
   */
  lineDepth?: string;

  /**
   * 移动类型（字典 logistics_materials_movement_type）
   */
  movementType: string;

  /**
   * 项目自动创建
   */
  autoCreatedFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 库存类型（字典 logistics_stock_type）
   */
  stockType?: string;

  /**
   * 批次限制
   */
  restrictedStockFlag?: string;

  /**
   * 特殊库存（字典 logistics_materials_special_stock_type）
   */
  specialStock?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 金额
   */
  alternativeAmount?: number;

  /**
   * 数量（基本计量单位）
   */
  quantity: number;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code）
   */
  baseUnit?: string;

  /**
   * 输入单位数量
   */
  entryQuantity?: number;

  /**
   * 条目单位
   */
  entryUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 采购订单
   */
  purchaseOrderCode?: string;

  /**
   * 采购订单项目
   */
  purchaseOrderItem?: number;

  /**
   * 参考凭证会计年度
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 冲销物料凭证的年份
   */
  originalMaterialDocumentYear?: string;

  /**
   * 冲销物料凭证
   */
  originalMaterialDocumentCode?: string;

  /**
   * 冲销物料凭证项目
   */
  originalLineNumber?: number;

  /**
   * 交货已完成
   */
  deliveryCompletedFlag?: string;

  /**
   * 文本（项目文本最长 50，故 Length=50）
   */
  itemText?: string;

  /**
   * 设备
   */
  equipmentCode?: string;

  /**
   * 收货方（最长 12，故 Length=12）
   */
  goodsRecipient?: string;

  /**
   * 卸货点（最长 25，故 Length=25）
   */
  unloadingPoint?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 成本控制域
   */
  controllingAreaCode?: string;

  /**
   * 伙伴业务范围
   */
  tradingPartnerBusinessArea?: string;

  /**
   * 订单
   */
  productionOrderCode?: string;

  /**
   * 资产
   */
  assetCode?: string;

  /**
   * 次级编号
   */
  assetSubCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 允许前期记帐
   */
  postToPreviousPeriodFlag?: string;

  /**
   * 上年度记帐
   */
  postToPreviousYearFlag?: string;

  /**
   * 会计凭证编号
   */
  accountingDocumentCode?: string;

  /**
   * 会计凭证行项目
   */
  accountingDocumentItem?: number;

  /**
   * 再评估凭证编号
   */
  revaluationDocumentCode?: string;

  /**
   * 再评估凭证行项目
   */
  revaluationDocumentItem?: string;

  /**
   * 预留编号
   */
  reservationCode?: string;

  /**
   * 项目编号库存转储预留
   */
  reservationItem?: number;

  /**
   * 最终发货标识
   */
  finalIssueFlag?: string;

  /**
   * 预留已处理数量
   */
  reservationQuantity?: number;

  /**
   * 接收物料
   */
  receivingMaterialCode?: string;

  /**
   * 收货工厂
   */
  receivingPlantCode?: string;

  /**
   * 收货库存地点
   */
  receivingWarehouseCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 过帐前总计估价库存
   */
  valuatedStockQuantity?: number;

  /**
   * 过帐前总计评估的库存的价值
   */
  totalValuatedStockValue?: number;

  /**
   * 价格控制
   */
  priceControl?: string;

  /**
   * 制造商物料编码
   */
  manufacturerPartMaterialCode?: string;

  /**
   * 参考（最长 32，故 Length=32）
   */
  mkpfReferenceCode?: string;

  /**
   * 交货
   */
  imDeliveryCode?: string;

  /**
   * 交货项目
   */
  imDeliveryItem?: number;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 物料凭证主表 （主表：TaktMaterialDocument）
   */
  materialDocument?: MaterialDocument;

}


/**
 * MaterialDocumentItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialDocumentItemQuery
 * @description 对应后端 TaktMaterialDocumentItemQueryDto
 */
export interface MaterialDocumentItemQuery extends TaktPagedQuery {
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
   * 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
   */
  materialDocumentId?: string;

  /**
   * 物料凭证（冗余：按对应 Id 取主数据名称联动）
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证项目（行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 行标识
   */
  lineId?: string;

  /**
   * 上级行 ID
   */
  parentLineId?: string;

  /**
   * 层次结构级别
   */
  lineDepth?: string;

  /**
   * 移动类型（字典 logistics_materials_movement_type）
   */
  movementType?: string;

  /**
   * 项目自动创建
   */
  autoCreatedFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 库存类型（字典 logistics_stock_type）
   */
  stockType?: string;

  /**
   * 批次限制
   */
  restrictedStockFlag?: string;

  /**
   * 特殊库存（字典 logistics_materials_special_stock_type）
   */
  specialStock?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 金额
   */
  alternativeAmount?: number;

  /**
   * 数量（基本计量单位）
   */
  quantity?: number;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code）
   */
  baseUnit?: string;

  /**
   * 输入单位数量
   */
  entryQuantity?: number;

  /**
   * 条目单位
   */
  entryUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 采购订单
   */
  purchaseOrderCode?: string;

  /**
   * 采购订单项目
   */
  purchaseOrderItem?: number;

  /**
   * 参考凭证会计年度
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 冲销物料凭证的年份
   */
  originalMaterialDocumentYear?: string;

  /**
   * 冲销物料凭证
   */
  originalMaterialDocumentCode?: string;

  /**
   * 冲销物料凭证项目
   */
  originalLineNumber?: number;

  /**
   * 交货已完成
   */
  deliveryCompletedFlag?: string;

  /**
   * 文本（项目文本最长 50，故 Length=50）
   */
  itemText?: string;

  /**
   * 设备
   */
  equipmentCode?: string;

  /**
   * 收货方（最长 12，故 Length=12）
   */
  goodsRecipient?: string;

  /**
   * 卸货点（最长 25，故 Length=25）
   */
  unloadingPoint?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 成本控制域
   */
  controllingAreaCode?: string;

  /**
   * 伙伴业务范围
   */
  tradingPartnerBusinessArea?: string;

  /**
   * 订单
   */
  productionOrderCode?: string;

  /**
   * 资产
   */
  assetCode?: string;

  /**
   * 次级编号
   */
  assetSubCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 允许前期记帐
   */
  postToPreviousPeriodFlag?: string;

  /**
   * 上年度记帐
   */
  postToPreviousYearFlag?: string;

  /**
   * 会计凭证编号
   */
  accountingDocumentCode?: string;

  /**
   * 会计凭证行项目
   */
  accountingDocumentItem?: number;

  /**
   * 再评估凭证编号
   */
  revaluationDocumentCode?: string;

  /**
   * 再评估凭证行项目
   */
  revaluationDocumentItem?: string;

  /**
   * 预留编号
   */
  reservationCode?: string;

  /**
   * 项目编号库存转储预留
   */
  reservationItem?: number;

  /**
   * 最终发货标识
   */
  finalIssueFlag?: string;

  /**
   * 预留已处理数量
   */
  reservationQuantity?: number;

  /**
   * 接收物料
   */
  receivingMaterialCode?: string;

  /**
   * 收货工厂
   */
  receivingPlantCode?: string;

  /**
   * 收货库存地点
   */
  receivingWarehouseCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 过帐前总计估价库存
   */
  valuatedStockQuantity?: number;

  /**
   * 过帐前总计评估的库存的价值
   */
  totalValuatedStockValue?: number;

  /**
   * 价格控制
   */
  priceControl?: string;

  /**
   * 制造商物料编码
   */
  manufacturerPartMaterialCode?: string;

  /**
   * 参考（最长 32，故 Length=32）
   */
  mkpfReferenceCode?: string;

  /**
   * 交货
   */
  imDeliveryCode?: string;

  /**
   * 交货项目
   */
  imDeliveryItem?: number;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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
 * 创建MaterialDocumentItem DTO
 * 对应前端 MaterialDocumentItemCreate
 * @description 对应后端 TaktMaterialDocumentItemCreateDto
 */
export interface MaterialDocumentItemCreate {
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
   * 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
   */
  materialDocumentId: string;

  /**
   * 物料凭证（冗余：按对应 Id 取主数据名称联动）
   */
  materialDocumentCode: string;

  /**
   * 物料凭证项目（行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 行标识
   */
  lineId?: string;

  /**
   * 上级行 ID
   */
  parentLineId?: string;

  /**
   * 层次结构级别
   */
  lineDepth?: string;

  /**
   * 移动类型（字典 logistics_materials_movement_type）
   */
  movementType: string;

  /**
   * 项目自动创建
   */
  autoCreatedFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 库存类型（字典 logistics_stock_type）
   */
  stockType?: string;

  /**
   * 批次限制
   */
  restrictedStockFlag?: string;

  /**
   * 特殊库存（字典 logistics_materials_special_stock_type）
   */
  specialStock?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 金额
   */
  alternativeAmount?: number;

  /**
   * 数量（基本计量单位）
   */
  quantity: number;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code）
   */
  baseUnit?: string;

  /**
   * 输入单位数量
   */
  entryQuantity?: number;

  /**
   * 条目单位
   */
  entryUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 采购订单
   */
  purchaseOrderCode?: string;

  /**
   * 采购订单项目
   */
  purchaseOrderItem?: number;

  /**
   * 参考凭证会计年度
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 冲销物料凭证的年份
   */
  originalMaterialDocumentYear?: string;

  /**
   * 冲销物料凭证
   */
  originalMaterialDocumentCode?: string;

  /**
   * 冲销物料凭证项目
   */
  originalLineNumber?: number;

  /**
   * 交货已完成
   */
  deliveryCompletedFlag?: string;

  /**
   * 文本（项目文本最长 50，故 Length=50）
   */
  itemText?: string;

  /**
   * 设备
   */
  equipmentCode?: string;

  /**
   * 收货方（最长 12，故 Length=12）
   */
  goodsRecipient?: string;

  /**
   * 卸货点（最长 25，故 Length=25）
   */
  unloadingPoint?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 成本控制域
   */
  controllingAreaCode?: string;

  /**
   * 伙伴业务范围
   */
  tradingPartnerBusinessArea?: string;

  /**
   * 订单
   */
  productionOrderCode?: string;

  /**
   * 资产
   */
  assetCode?: string;

  /**
   * 次级编号
   */
  assetSubCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 允许前期记帐
   */
  postToPreviousPeriodFlag?: string;

  /**
   * 上年度记帐
   */
  postToPreviousYearFlag?: string;

  /**
   * 会计凭证编号
   */
  accountingDocumentCode?: string;

  /**
   * 会计凭证行项目
   */
  accountingDocumentItem?: number;

  /**
   * 再评估凭证编号
   */
  revaluationDocumentCode?: string;

  /**
   * 再评估凭证行项目
   */
  revaluationDocumentItem?: string;

  /**
   * 预留编号
   */
  reservationCode?: string;

  /**
   * 项目编号库存转储预留
   */
  reservationItem?: number;

  /**
   * 最终发货标识
   */
  finalIssueFlag?: string;

  /**
   * 预留已处理数量
   */
  reservationQuantity?: number;

  /**
   * 接收物料
   */
  receivingMaterialCode?: string;

  /**
   * 收货工厂
   */
  receivingPlantCode?: string;

  /**
   * 收货库存地点
   */
  receivingWarehouseCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 过帐前总计估价库存
   */
  valuatedStockQuantity?: number;

  /**
   * 过帐前总计评估的库存的价值
   */
  totalValuatedStockValue?: number;

  /**
   * 价格控制
   */
  priceControl?: string;

  /**
   * 制造商物料编码
   */
  manufacturerPartMaterialCode?: string;

  /**
   * 参考（最长 32，故 Length=32）
   */
  mkpfReferenceCode?: string;

  /**
   * 交货
   */
  imDeliveryCode?: string;

  /**
   * 交货项目
   */
  imDeliveryItem?: number;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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
 * 更新MaterialDocumentItem DTO
 * 继承 TaktMaterialDocumentItemCreateDto，添加 MaterialDocumentItemId 字段
 * 对应前端 MaterialDocumentItemUpdate
 * @description 对应后端 TaktMaterialDocumentItemUpdateDto
 */
export interface MaterialDocumentItemUpdate extends MaterialDocumentItemCreate {
  /**
   * MaterialDocumentItemID（标识要更新的实体）
   */
  materialDocumentItemId: string;

}


/**
 * MaterialDocumentItem 作废/撤销作废 DTO
 * 对应前端 MaterialDocumentItemObsolete
 * @description 对应后端 TaktMaterialDocumentItemObsoleteDto
 */
export interface MaterialDocumentItemObsolete {
  /**
   * MaterialDocumentItemID
   */
  materialDocumentItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * MaterialDocumentItem 导入模板行 DTO
 * 对应前端 MaterialDocumentItemTemplate
 * @description 对应后端 TaktMaterialDocumentItemTemplateDto
 */
export interface MaterialDocumentItemTemplate {
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
   * 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
   */
  materialDocumentId?: string;

  /**
   * 物料凭证（冗余：按对应 Id 取主数据名称联动）
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证项目（行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 行标识
   */
  lineId?: string;

  /**
   * 上级行 ID
   */
  parentLineId?: string;

  /**
   * 层次结构级别
   */
  lineDepth?: string;

  /**
   * 移动类型（字典 logistics_materials_movement_type）
   */
  movementType?: string;

  /**
   * 项目自动创建
   */
  autoCreatedFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 库存类型（字典 logistics_stock_type）
   */
  stockType?: string;

  /**
   * 批次限制
   */
  restrictedStockFlag?: string;

  /**
   * 特殊库存（字典 logistics_materials_special_stock_type）
   */
  specialStock?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 金额
   */
  alternativeAmount?: number;

  /**
   * 数量（基本计量单位）
   */
  quantity?: number;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code）
   */
  baseUnit?: string;

  /**
   * 输入单位数量
   */
  entryQuantity?: number;

  /**
   * 条目单位
   */
  entryUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 采购订单
   */
  purchaseOrderCode?: string;

  /**
   * 采购订单项目
   */
  purchaseOrderItem?: number;

  /**
   * 参考凭证会计年度
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 冲销物料凭证的年份
   */
  originalMaterialDocumentYear?: string;

  /**
   * 冲销物料凭证
   */
  originalMaterialDocumentCode?: string;

  /**
   * 冲销物料凭证项目
   */
  originalLineNumber?: number;

  /**
   * 交货已完成
   */
  deliveryCompletedFlag?: string;

  /**
   * 文本（项目文本最长 50，故 Length=50）
   */
  itemText?: string;

  /**
   * 设备
   */
  equipmentCode?: string;

  /**
   * 收货方（最长 12，故 Length=12）
   */
  goodsRecipient?: string;

  /**
   * 卸货点（最长 25，故 Length=25）
   */
  unloadingPoint?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 成本控制域
   */
  controllingAreaCode?: string;

  /**
   * 伙伴业务范围
   */
  tradingPartnerBusinessArea?: string;

  /**
   * 订单
   */
  productionOrderCode?: string;

  /**
   * 资产
   */
  assetCode?: string;

  /**
   * 次级编号
   */
  assetSubCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 允许前期记帐
   */
  postToPreviousPeriodFlag?: string;

  /**
   * 上年度记帐
   */
  postToPreviousYearFlag?: string;

  /**
   * 会计凭证编号
   */
  accountingDocumentCode?: string;

  /**
   * 会计凭证行项目
   */
  accountingDocumentItem?: number;

  /**
   * 再评估凭证编号
   */
  revaluationDocumentCode?: string;

  /**
   * 再评估凭证行项目
   */
  revaluationDocumentItem?: string;

  /**
   * 预留编号
   */
  reservationCode?: string;

  /**
   * 项目编号库存转储预留
   */
  reservationItem?: number;

  /**
   * 最终发货标识
   */
  finalIssueFlag?: string;

  /**
   * 预留已处理数量
   */
  reservationQuantity?: number;

  /**
   * 接收物料
   */
  receivingMaterialCode?: string;

  /**
   * 收货工厂
   */
  receivingPlantCode?: string;

  /**
   * 收货库存地点
   */
  receivingWarehouseCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 过帐前总计估价库存
   */
  valuatedStockQuantity?: number;

  /**
   * 过帐前总计评估的库存的价值
   */
  totalValuatedStockValue?: number;

  /**
   * 价格控制
   */
  priceControl?: string;

  /**
   * 制造商物料编码
   */
  manufacturerPartMaterialCode?: string;

  /**
   * 参考（最长 32，故 Length=32）
   */
  mkpfReferenceCode?: string;

  /**
   * 交货
   */
  imDeliveryCode?: string;

  /**
   * 交货项目
   */
  imDeliveryItem?: number;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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
 * MaterialDocumentItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialDocumentItemImport
 * @description 对应后端 TaktMaterialDocumentItemImportDto
 */
export interface MaterialDocumentItemImport {
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
   * 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
   */
  materialDocumentId?: string;

  /**
   * 物料凭证（冗余：按对应 Id 取主数据名称联动）
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证项目（行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 行标识
   */
  lineId?: string;

  /**
   * 上级行 ID
   */
  parentLineId?: string;

  /**
   * 层次结构级别
   */
  lineDepth?: string;

  /**
   * 移动类型（字典 logistics_materials_movement_type）
   */
  movementType?: string;

  /**
   * 项目自动创建
   */
  autoCreatedFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 库存类型（字典 logistics_stock_type）
   */
  stockType?: string;

  /**
   * 批次限制
   */
  restrictedStockFlag?: string;

  /**
   * 特殊库存（字典 logistics_materials_special_stock_type）
   */
  specialStock?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 金额
   */
  alternativeAmount?: number;

  /**
   * 数量（基本计量单位）
   */
  quantity?: number;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code）
   */
  baseUnit?: string;

  /**
   * 输入单位数量
   */
  entryQuantity?: number;

  /**
   * 条目单位
   */
  entryUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 采购订单
   */
  purchaseOrderCode?: string;

  /**
   * 采购订单项目
   */
  purchaseOrderItem?: number;

  /**
   * 参考凭证会计年度
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 冲销物料凭证的年份
   */
  originalMaterialDocumentYear?: string;

  /**
   * 冲销物料凭证
   */
  originalMaterialDocumentCode?: string;

  /**
   * 冲销物料凭证项目
   */
  originalLineNumber?: number;

  /**
   * 交货已完成
   */
  deliveryCompletedFlag?: string;

  /**
   * 文本（项目文本最长 50，故 Length=50）
   */
  itemText?: string;

  /**
   * 设备
   */
  equipmentCode?: string;

  /**
   * 收货方（最长 12，故 Length=12）
   */
  goodsRecipient?: string;

  /**
   * 卸货点（最长 25，故 Length=25）
   */
  unloadingPoint?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 成本控制域
   */
  controllingAreaCode?: string;

  /**
   * 伙伴业务范围
   */
  tradingPartnerBusinessArea?: string;

  /**
   * 订单
   */
  productionOrderCode?: string;

  /**
   * 资产
   */
  assetCode?: string;

  /**
   * 次级编号
   */
  assetSubCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 允许前期记帐
   */
  postToPreviousPeriodFlag?: string;

  /**
   * 上年度记帐
   */
  postToPreviousYearFlag?: string;

  /**
   * 会计凭证编号
   */
  accountingDocumentCode?: string;

  /**
   * 会计凭证行项目
   */
  accountingDocumentItem?: number;

  /**
   * 再评估凭证编号
   */
  revaluationDocumentCode?: string;

  /**
   * 再评估凭证行项目
   */
  revaluationDocumentItem?: string;

  /**
   * 预留编号
   */
  reservationCode?: string;

  /**
   * 项目编号库存转储预留
   */
  reservationItem?: number;

  /**
   * 最终发货标识
   */
  finalIssueFlag?: string;

  /**
   * 预留已处理数量
   */
  reservationQuantity?: number;

  /**
   * 接收物料
   */
  receivingMaterialCode?: string;

  /**
   * 收货工厂
   */
  receivingPlantCode?: string;

  /**
   * 收货库存地点
   */
  receivingWarehouseCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 过帐前总计估价库存
   */
  valuatedStockQuantity?: number;

  /**
   * 过帐前总计评估的库存的价值
   */
  totalValuatedStockValue?: number;

  /**
   * 价格控制
   */
  priceControl?: string;

  /**
   * 制造商物料编码
   */
  manufacturerPartMaterialCode?: string;

  /**
   * 参考（最长 32，故 Length=32）
   */
  mkpfReferenceCode?: string;

  /**
   * 交货
   */
  imDeliveryCode?: string;

  /**
   * 交货项目
   */
  imDeliveryItem?: number;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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
 * MaterialDocumentItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialDocumentItemExport
 * @description 对应后端 TaktMaterialDocumentItemExportDto
 */
export interface MaterialDocumentItemExport {
  /**
   * MaterialDocumentItemID
   */
  materialDocumentItemId: string;

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
   * 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
   */
  materialDocumentId: string;

  /**
   * 物料凭证（冗余：按对应 Id 取主数据名称联动）
   */
  materialDocumentCode: string;

  /**
   * 物料凭证项目（行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 行标识
   */
  lineId?: string;

  /**
   * 上级行 ID
   */
  parentLineId?: string;

  /**
   * 层次结构级别
   */
  lineDepth?: string;

  /**
   * 移动类型（字典 logistics_materials_movement_type）
   */
  movementType: string;

  /**
   * 项目自动创建
   */
  autoCreatedFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 库存类型（字典 logistics_stock_type）
   */
  stockType?: string;

  /**
   * 批次限制
   */
  restrictedStockFlag?: string;

  /**
   * 特殊库存（字典 logistics_materials_special_stock_type）
   */
  specialStock?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 金额
   */
  alternativeAmount?: number;

  /**
   * 数量（基本计量单位）
   */
  quantity: number;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code）
   */
  baseUnit?: string;

  /**
   * 输入单位数量
   */
  entryQuantity?: number;

  /**
   * 条目单位
   */
  entryUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 采购订单
   */
  purchaseOrderCode?: string;

  /**
   * 采购订单项目
   */
  purchaseOrderItem?: number;

  /**
   * 参考凭证会计年度
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 冲销物料凭证的年份
   */
  originalMaterialDocumentYear?: string;

  /**
   * 冲销物料凭证
   */
  originalMaterialDocumentCode?: string;

  /**
   * 冲销物料凭证项目
   */
  originalLineNumber?: number;

  /**
   * 交货已完成
   */
  deliveryCompletedFlag?: string;

  /**
   * 文本（项目文本最长 50，故 Length=50）
   */
  itemText?: string;

  /**
   * 设备
   */
  equipmentCode?: string;

  /**
   * 收货方（最长 12，故 Length=12）
   */
  goodsRecipient?: string;

  /**
   * 卸货点（最长 25，故 Length=25）
   */
  unloadingPoint?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 成本控制域
   */
  controllingAreaCode?: string;

  /**
   * 伙伴业务范围
   */
  tradingPartnerBusinessArea?: string;

  /**
   * 订单
   */
  productionOrderCode?: string;

  /**
   * 资产
   */
  assetCode?: string;

  /**
   * 次级编号
   */
  assetSubCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 允许前期记帐
   */
  postToPreviousPeriodFlag?: string;

  /**
   * 上年度记帐
   */
  postToPreviousYearFlag?: string;

  /**
   * 会计凭证编号
   */
  accountingDocumentCode?: string;

  /**
   * 会计凭证行项目
   */
  accountingDocumentItem?: number;

  /**
   * 再评估凭证编号
   */
  revaluationDocumentCode?: string;

  /**
   * 再评估凭证行项目
   */
  revaluationDocumentItem?: string;

  /**
   * 预留编号
   */
  reservationCode?: string;

  /**
   * 项目编号库存转储预留
   */
  reservationItem?: number;

  /**
   * 最终发货标识
   */
  finalIssueFlag?: string;

  /**
   * 预留已处理数量
   */
  reservationQuantity?: number;

  /**
   * 接收物料
   */
  receivingMaterialCode?: string;

  /**
   * 收货工厂
   */
  receivingPlantCode?: string;

  /**
   * 收货库存地点
   */
  receivingWarehouseCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 过帐前总计估价库存
   */
  valuatedStockQuantity?: number;

  /**
   * 过帐前总计评估的库存的价值
   */
  totalValuatedStockValue?: number;

  /**
   * 价格控制
   */
  priceControl?: string;

  /**
   * 制造商物料编码
   */
  manufacturerPartMaterialCode?: string;

  /**
   * 参考（最长 32，故 Length=32）
   */
  mkpfReferenceCode?: string;

  /**
   * 交货
   */
  imDeliveryCode?: string;

  /**
   * 交货项目
   */
  imDeliveryItem?: number;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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

