// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：routingItemArgument.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 工艺路线工序参数定义实体
 * 对应前端 TaktRoutingItemArgumentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 RoutingItemArgument
 * @description 对应后端 TaktRoutingItemArgumentDto
 */
export interface RoutingItemArgument extends CompanyDtoBase {
  /**
   * RoutingItemArgumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemArgumentId: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 工艺路线明细 名称（填充字段）
   */
  routingItemName?: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 参数名称
   */
  paramName: string;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  paramUnit?: string;

  /**
   * 标准值
   */
  standardValue?: number;

  /**
   * 下限
   */
  lowerLimit?: number;

  /**
   * 上限
   */
  upperLimit?: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 工序 （主表：TaktRoutingItem）
   */
  routingItem?: RoutingItem;

}


/**
 * RoutingItemArgument 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 RoutingItemArgumentQuery
 * @description 对应后端 TaktRoutingItemArgumentQueryDto
 */
export interface RoutingItemArgumentQuery extends TaktPagedQuery {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 参数名称
   */
  paramName?: string;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  paramUnit?: string;

  /**
   * 标准值
   */
  standardValue?: number;

  /**
   * 下限
   */
  lowerLimit?: number;

  /**
   * 上限
   */
  upperLimit?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * 创建RoutingItemArgument DTO
 * 对应前端 RoutingItemArgumentCreate
 * @description 对应后端 TaktRoutingItemArgumentCreateDto
 */
export interface RoutingItemArgumentCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 参数名称
   */
  paramName: string;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  paramUnit?: string;

  /**
   * 标准值
   */
  standardValue?: number;

  /**
   * 下限
   */
  lowerLimit?: number;

  /**
   * 上限
   */
  upperLimit?: number;

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
 * 更新RoutingItemArgument DTO
 * 继承 TaktRoutingItemArgumentCreateDto，添加 RoutingItemArgumentId 字段
 * 对应前端 RoutingItemArgumentUpdate
 * @description 对应后端 TaktRoutingItemArgumentUpdateDto
 */
export interface RoutingItemArgumentUpdate extends RoutingItemArgumentCreate {
  /**
   * RoutingItemArgumentID（标识要更新的实体）
   */
  routingItemArgumentId: string;

}


/**
 * RoutingItemArgument 排序更新 DTO
 * 对应前端 RoutingItemArgumentSort
 * @description 对应后端 TaktRoutingItemArgumentSortDto
 */
export interface RoutingItemArgumentSort {
  /**
   * RoutingItemArgumentID
   */
  routingItemArgumentId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * RoutingItemArgument 导入模板行 DTO
 * 对应前端 RoutingItemArgumentTemplate
 * @description 对应后端 TaktRoutingItemArgumentTemplateDto
 */
export interface RoutingItemArgumentTemplate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 参数名称
   */
  paramName?: string;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  paramUnit?: string;

  /**
   * 标准值
   */
  standardValue?: number;

  /**
   * 下限
   */
  lowerLimit?: number;

  /**
   * 上限
   */
  upperLimit?: number;

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
 * RoutingItemArgument 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 RoutingItemArgumentImport
 * @description 对应后端 TaktRoutingItemArgumentImportDto
 */
export interface RoutingItemArgumentImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 参数名称
   */
  paramName?: string;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  paramUnit?: string;

  /**
   * 标准值
   */
  standardValue?: number;

  /**
   * 下限
   */
  lowerLimit?: number;

  /**
   * 上限
   */
  upperLimit?: number;

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
 * RoutingItemArgument 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 RoutingItemArgumentExport
 * @description 对应后端 TaktRoutingItemArgumentExportDto
 */
export interface RoutingItemArgumentExport {
  /**
   * RoutingItemArgumentID
   */
  routingItemArgumentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 参数名称
   */
  paramName: string;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  paramUnit?: string;

  /**
   * 标准值
   */
  standardValue?: number;

  /**
   * 下限
   */
  lowerLimit?: number;

  /**
   * 上限
   */
  upperLimit?: number;

  /**
   * 排序号
   */
  sortOrder: number;

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

