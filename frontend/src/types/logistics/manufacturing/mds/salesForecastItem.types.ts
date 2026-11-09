// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：salesForecastItem.types.ts
// 创建时间：2026-11-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt销售预测明细（一行 = 主表物料在某财年某月的一个版本；版本号 1～20；产品/类别/利润中心/机种/物料在主表）
 * 对应前端 TaktSalesForecastItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesForecastItem
 * @description 对应后端 TaktSalesForecastItemDto
 */
export interface SalesForecastItem extends CompanyDtoBase {
  /**
   * SalesForecastItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastItemId: string;

  /**
   * 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId: string;

  /**
   * 销售预测名称（填充字段）
   */
  salesForecastName?: string;

  /**
   * 销售预测编码（冗余：按对应 Id 取主数据名称联动）
   */
  salesForecastCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear: string;

  /**
   * 计划月份（1～12）
   */
  planMonth: number;

  /**
   * 版本号（同一财年同一月 1～20；每个版本一行，挂在销售预测主表下）
   */
  versionNo: number;

  /**
   * 计划数量（本版本）
   */
  planQuantity: number;

  /**
   * 计划增减（本版本数量 − 同月上一版本数量；版本 1 相对 0；服务层写入，禁止手改）
   */
  planQuantityDelta: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesForecastItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesForecastItemQuery
 * @description 对应后端 TaktSalesForecastItemQueryDto
 */
export interface SalesForecastItemQuery extends TaktPagedQuery {
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
   * 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 销售预测编码（冗余：按对应 Id 取主数据名称联动）
   */
  salesForecastCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear?: string;

  /**
   * 计划月份（1～12）
   */
  planMonth?: number;

  /**
   * 版本号（同一财年同一月 1～20；每个版本一行，挂在销售预测主表下）
   */
  versionNo?: number;

  /**
   * 计划数量（本版本）
   */
  planQuantity?: number;

  /**
   * 计划增减（本版本数量 − 同月上一版本数量；版本 1 相对 0；服务层写入，禁止手改）
   */
  planQuantityDelta?: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * 创建SalesForecastItem DTO
 * 对应前端 SalesForecastItemCreate
 * @description 对应后端 TaktSalesForecastItemCreateDto
 */
export interface SalesForecastItemCreate {
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
   * 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId: string;

  /**
   * 销售预测编码（冗余：按对应 Id 取主数据名称联动）
   */
  salesForecastCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear: string;

  /**
   * 计划月份（1～12）
   */
  planMonth: number;

  /**
   * 版本号（同一财年同一月 1～20；每个版本一行，挂在销售预测主表下）
   */
  versionNo: number;

  /**
   * 计划数量（本版本）
   */
  planQuantity: number;

  /**
   * 计划增减（本版本数量 − 同月上一版本数量；版本 1 相对 0；服务层写入，禁止手改）
   */
  planQuantityDelta: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

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
 * 更新SalesForecastItem DTO
 * 继承 TaktSalesForecastItemCreateDto，添加 SalesForecastItemId 字段
 * 对应前端 SalesForecastItemUpdate
 * @description 对应后端 TaktSalesForecastItemUpdateDto
 */
export interface SalesForecastItemUpdate extends SalesForecastItemCreate {
  /**
   * SalesForecastItemID（标识要更新的实体）
   */
  salesForecastItemId: string;

}


/**
 * SalesForecastItem 作废/撤销作废 DTO
 * 对应前端 SalesForecastItemObsolete
 * @description 对应后端 TaktSalesForecastItemObsoleteDto
 */
export interface SalesForecastItemObsolete {
  /**
   * SalesForecastItemID
   */
  salesForecastItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesForecastItem 导入模板行 DTO
 * 对应前端 SalesForecastItemTemplate
 * @description 对应后端 TaktSalesForecastItemTemplateDto
 */
export interface SalesForecastItemTemplate {
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
   * 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 销售预测编码（冗余：按对应 Id 取主数据名称联动）
   */
  salesForecastCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear?: string;

  /**
   * 计划月份（1～12）
   */
  planMonth?: number;

  /**
   * 版本号（同一财年同一月 1～20；每个版本一行，挂在销售预测主表下）
   */
  versionNo?: number;

  /**
   * 计划数量（本版本）
   */
  planQuantity?: number;

  /**
   * 计划增减（本版本数量 − 同月上一版本数量；版本 1 相对 0；服务层写入，禁止手改）
   */
  planQuantityDelta?: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * SalesForecastItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesForecastItemImport
 * @description 对应后端 TaktSalesForecastItemImportDto
 */
export interface SalesForecastItemImport {
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
   * 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 销售预测编码（冗余：按对应 Id 取主数据名称联动）
   */
  salesForecastCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear?: string;

  /**
   * 计划月份（1～12）
   */
  planMonth?: number;

  /**
   * 版本号（同一财年同一月 1～20；每个版本一行，挂在销售预测主表下）
   */
  versionNo?: number;

  /**
   * 计划数量（本版本）
   */
  planQuantity?: number;

  /**
   * 计划增减（本版本数量 − 同月上一版本数量；版本 1 相对 0；服务层写入，禁止手改）
   */
  planQuantityDelta?: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * SalesForecastItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesForecastItemExport
 * @description 对应后端 TaktSalesForecastItemExportDto
 */
export interface SalesForecastItemExport {
  /**
   * SalesForecastItemID
   */
  salesForecastItemId: string;

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
   * 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId: string;

  /**
   * 销售预测编码（冗余：按对应 Id 取主数据名称联动）
   */
  salesForecastCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear: string;

  /**
   * 计划月份（1～12）
   */
  planMonth: number;

  /**
   * 版本号（同一财年同一月 1～20；每个版本一行，挂在销售预测主表下）
   */
  versionNo: number;

  /**
   * 计划数量（本版本）
   */
  planQuantity: number;

  /**
   * 计划增减（本版本数量 − 同月上一版本数量；版本 1 相对 0；服务层写入，禁止手改）
   */
  planQuantityDelta: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

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

