// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialCostItem.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery,
} from '@/types/common.types';

/**
 * BOM 物料成本明细行（业务源数据：先导入/维护明细，再按工厂+产品+核算月聚合写入 TaktBomMaterialCost；无线上主表外键）
 * 对应前端 TaktBomMaterialCostItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BomMaterialCostItem
 * @description 对应后端 TaktBomMaterialCostItemDto
 */
export interface BomMaterialCostItem extends CompanyDtoBase {
  /**
   * BomMaterialCostItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bomMaterialCostItemId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemCode: string;

  /**
   * 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode: string;

  /**
   * 组件描述
   */
  componentDescription: string;

  /**
   * 组件数量
   */
  componentQuantity: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
   */
  pcbSectIndicator?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
   */
  profitCenterCode: string;

  /**
   * 移动平均价（5 位小数）
   */
  movingAveragePrice: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit: number;

  /**
   * 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  movingPriceCurrencyCode: string;

  /**
   * 采购组织
   */
  purchaseOrganization: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
   */
  purchaseGroup: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 净价（采购价格，5 位小数）
   */
  netPurchasePrice: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit: number;

  /**
   * 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  purchaseCurrencyCode: string;

  /**
   * 核算日期
   */
  costingDate: string;

}


/**
 * BomMaterialCostItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BomMaterialCostItemQuery
 * @description 对应后端 TaktBomMaterialCostItemQueryDto
 */
export interface BomMaterialCostItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
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
   * 机种编码（分析/合计查询；回填 productCodes 后用于明细过滤）
   */
  modelCode?: string;

  /**
   * 共用产品编码集合（由 modelCode 回填；不显式指定产品时用于缩小范围）
   */
  productCodes?: string[];

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel?: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemCode?: string;

  /**
   * 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode?: string;

  /**
   * 组件描述
   */
  componentDescription?: string;

  /**
   * 组件数量
   */
  componentQuantity?: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
   */
  pcbSectIndicator?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType?: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
   */
  profitCenterCode?: string;

  /**
   * 移动平均价（5 位小数）
   */
  movingAveragePrice?: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit?: number;

  /**
   * 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  movingPriceCurrencyCode?: string;

  /**
   * 采购组织
   */
  purchaseOrganization?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
   */
  purchaseGroup?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 净价（采购价格，5 位小数）
   */
  netPurchasePrice?: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit?: number;

  /**
   * 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  purchaseCurrencyCode?: string;

  /**
   * 核算日期（范围查询-开始）
   */
  costingDateStart?: string;

  /**
   * 核算日期（范围查询-结束）
   */
  costingDateEnd?: string;

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
 * 创建BomMaterialCostItem DTO
 * 对应前端 BomMaterialCostItemCreate
 * @description 对应后端 TaktBomMaterialCostItemCreateDto
 */
export interface BomMaterialCostItemCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemCode: string;

  /**
   * 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode: string;

  /**
   * 组件描述
   */
  componentDescription: string;

  /**
   * 组件数量
   */
  componentQuantity: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
   */
  pcbSectIndicator?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
   */
  profitCenterCode: string;

  /**
   * 移动平均价（5 位小数）
   */
  movingAveragePrice: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit: number;

  /**
   * 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  movingPriceCurrencyCode: string;

  /**
   * 采购组织
   */
  purchaseOrganization: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
   */
  purchaseGroup: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 净价（采购价格，5 位小数）
   */
  netPurchasePrice: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit: number;

  /**
   * 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  purchaseCurrencyCode: string;

  /**
   * 核算日期
   */
  costingDate: string;

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
 * 更新BomMaterialCostItem DTO
 * 继承 TaktBomMaterialCostItemCreateDto，添加 BomMaterialCostItemId 字段
 * 对应前端 BomMaterialCostItemUpdate
 * @description 对应后端 TaktBomMaterialCostItemUpdateDto
 */
export interface BomMaterialCostItemUpdate extends BomMaterialCostItemCreate {
  /**
   * BomMaterialCostItemID（标识要更新的实体）
   */
  bomMaterialCostItemId: string;

}


/**
 * BomMaterialCostItem 导入模板行 DTO
 * 对应前端 BomMaterialCostItemTemplate
 * @description 对应后端 TaktBomMaterialCostItemTemplateDto
 */
export interface BomMaterialCostItemTemplate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel?: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemCode?: string;

  /**
   * 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode?: string;

  /**
   * 组件描述
   */
  componentDescription?: string;

  /**
   * 组件数量
   */
  componentQuantity?: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
   */
  pcbSectIndicator?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType?: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
   */
  profitCenterCode?: string;

  /**
   * 移动平均价（5 位小数）
   */
  movingAveragePrice?: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit?: number;

  /**
   * 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  movingPriceCurrencyCode?: string;

  /**
   * 采购组织
   */
  purchaseOrganization?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
   */
  purchaseGroup?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 净价（采购价格，5 位小数）
   */
  netPurchasePrice?: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit?: number;

  /**
   * 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  purchaseCurrencyCode?: string;

  /**
   * 核算日期
   */
  costingDate?: string;

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
 * BomMaterialCostItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BomMaterialCostItemImport
 * @description 对应后端 TaktBomMaterialCostItemImportDto
 */
export interface BomMaterialCostItemImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel?: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemCode?: string;

  /**
   * 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode?: string;

  /**
   * 组件描述
   */
  componentDescription?: string;

  /**
   * 组件数量
   */
  componentQuantity?: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
   */
  pcbSectIndicator?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType?: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
   */
  profitCenterCode?: string;

  /**
   * 移动平均价（5 位小数）
   */
  movingAveragePrice?: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit?: number;

  /**
   * 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  movingPriceCurrencyCode?: string;

  /**
   * 采购组织
   */
  purchaseOrganization?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
   */
  purchaseGroup?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 净价（采购价格，5 位小数）
   */
  netPurchasePrice?: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit?: number;

  /**
   * 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  purchaseCurrencyCode?: string;

  /**
   * 核算日期
   */
  costingDate?: string;

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
 * BomMaterialCostItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BomMaterialCostItemExport
 * @description 对应后端 TaktBomMaterialCostItemExportDto
 */
export interface BomMaterialCostItemExport {
  /**
   * BomMaterialCostItemID
   */
  bomMaterialCostItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemCode: string;

  /**
   * 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode: string;

  /**
   * 组件描述
   */
  componentDescription: string;

  /**
   * 组件数量
   */
  componentQuantity: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
   */
  pcbSectIndicator?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
   */
  profitCenterCode: string;

  /**
   * 移动平均价（5 位小数）
   */
  movingAveragePrice: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit: number;

  /**
   * 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  movingPriceCurrencyCode: string;

  /**
   * 采购组织
   */
  purchaseOrganization: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
   */
  purchaseGroup: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 净价（采购价格，5 位小数）
   */
  netPurchasePrice: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit: number;

  /**
   * 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  purchaseCurrencyCode: string;

  /**
   * 核算日期
   */
  costingDate: string;

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

