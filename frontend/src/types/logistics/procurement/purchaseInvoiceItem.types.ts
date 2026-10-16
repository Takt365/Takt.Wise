// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseInvoiceItem.types.ts
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
 * Takt采购发票明细实体（公司级；主子表关系见 PurchaseInvoiceId）
 * 对应前端 TaktPurchaseInvoiceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInvoiceItem
 * @description 对应后端 TaktPurchaseInvoiceItemDto
 */
export interface PurchaseInvoiceItem extends CompanyDtoBase {
  /**
   * PurchaseInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInvoiceItemId: string;

  /**
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 采购发票名称（填充字段）
   */
  purchaseInvoiceName?: string;

  /**
   * 凭证编号（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseInvoiceCode: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 采购发票主表 （主表：TaktPurchaseInvoice）
   */
  purchaseInvoice?: PurchaseInvoice;

}


/**
 * PurchaseInvoiceItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInvoiceItemQuery
 * @description 对应后端 TaktPurchaseInvoiceItemQueryDto
 */
export interface PurchaseInvoiceItemQuery extends TaktPagedQuery {
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
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId?: string;

  /**
   * 凭证编号（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseInvoiceCode?: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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
 * 创建PurchaseInvoiceItem DTO
 * 对应前端 PurchaseInvoiceItemCreate
 * @description 对应后端 TaktPurchaseInvoiceItemCreateDto
 */
export interface PurchaseInvoiceItemCreate {
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
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 凭证编号（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseInvoiceCode: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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
 * 更新PurchaseInvoiceItem DTO
 * 继承 TaktPurchaseInvoiceItemCreateDto，添加 PurchaseInvoiceItemId 字段
 * 对应前端 PurchaseInvoiceItemUpdate
 * @description 对应后端 TaktPurchaseInvoiceItemUpdateDto
 */
export interface PurchaseInvoiceItemUpdate extends PurchaseInvoiceItemCreate {
  /**
   * PurchaseInvoiceItemID（标识要更新的实体）
   */
  purchaseInvoiceItemId: string;

}


/**
 * PurchaseInvoiceItem 作废/撤销作废 DTO
 * 对应前端 PurchaseInvoiceItemObsolete
 * @description 对应后端 TaktPurchaseInvoiceItemObsoleteDto
 */
export interface PurchaseInvoiceItemObsolete {
  /**
   * PurchaseInvoiceItemID
   */
  purchaseInvoiceItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInvoiceItem 导入模板行 DTO
 * 对应前端 PurchaseInvoiceItemTemplate
 * @description 对应后端 TaktPurchaseInvoiceItemTemplateDto
 */
export interface PurchaseInvoiceItemTemplate {
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
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId?: string;

  /**
   * 凭证编号（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseInvoiceCode?: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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
 * PurchaseInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInvoiceItemImport
 * @description 对应后端 TaktPurchaseInvoiceItemImportDto
 */
export interface PurchaseInvoiceItemImport {
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
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId?: string;

  /**
   * 凭证编号（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseInvoiceCode?: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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
 * PurchaseInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInvoiceItemExport
 * @description 对应后端 TaktPurchaseInvoiceItemExportDto
 */
export interface PurchaseInvoiceItemExport {
  /**
   * PurchaseInvoiceItemID
   */
  purchaseInvoiceItemId: string;

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
   * 采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 凭证编号（冗余：按对应 Id 取主数据名称联动）
   */
  purchaseInvoiceCode: string;

  /**
   * 发票项目（发票行项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 项目（采购凭证项目）
   */
  purchaseOrderItem?: number;

  /**
   * 科目分配序号
   */
  accountAssignmentSeq?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估范围
   */
  valuationArea?: string;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 借/贷标识
   */
  debitCreditIndicator?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 订单单位
   */
  orderUnit?: string;

  /**
   * 订单价格单位数量
   */
  poPriceQuantity?: number;

  /**
   * 订单价格单位
   */
  poPriceUnit?: string;

  /**
   * 总库存
   */
  valuatedStockQuantity?: number;

  /**
   * 上一过账期间库存
   */
  previousPeriodStock?: number;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 评估类
   */
  valuationClass?: string;

  /**
   * 标识: 更新采购订单历史
   */
  updatePoHistoryFlag?: string;

  /**
   * 后续借/贷
   */
  subsequentDebitCredit?: string;

  /**
   * 价格冻结原因
   */
  blockReasonPrice?: string;

  /**
   * 数量冻结原因
   */
  blockReasonQuantity?: string;

  /**
   * 质量冻结原因
   */
  blockReasonQuality?: string;

  /**
   * 增强冻结原因
   */
  blockReasonEnhanced?: string;

  /**
   * 价值串
   */
  valueString?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 条件类型
   */
  conditionType?: string;

  /**
   * 总价值
   */
  totalValuatedStockValue?: number;

  /**
   * 前期总值
   */
  previousPeriodValue?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 当前期间年
   */
  referenceDocumentYear?: string;

  /**
   * 参考凭证项目
   */
  referenceDocumentItem?: number;

  /**
   * 库存物料
   */
  stockManagedMaterialCode?: string;

  /**
   * 文本
   */
  itemText?: string;

  /**
   * 来自到达的发票的存货过帐行
   */
  materialDocumentItem?: number;

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

