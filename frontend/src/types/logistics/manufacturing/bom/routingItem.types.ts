// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：routingItem.types.ts
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
 * 工艺路线明细表实体（工序序列）
 * 对应前端 TaktRoutingItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 RoutingItem
 * @description 对应后端 TaktRoutingItemDto
 */
export interface RoutingItem extends CompanyDtoBase {
  /**
   * RoutingItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId: string;

  /**
   * 工艺路线主表名称（填充字段）
   */
  routingName?: string;

  /**
   * 工艺路线编码（冗余字段，便于查询）
   */
  routingCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 基本数量
   */
  baseQuantity: number;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
   */
  pointsToMinutesRate: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 准备时间（分钟），如换模、调试等
   */
  setupMinutes: number;

  /**
   * 清理时间（分钟），如清洁、整理等
   */
  teardownMinutes: number;

  /**
   * 检验（字典 sys_yes_no：0=否，1=是）
   */
  isInspection: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 工序说明
   */
  processDescription?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType: number;

  /**
   * 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
   */
  extJson?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 工艺路线主表（主表） （主表：TaktRouting）
   */
  routing?: Routing;

  /**
   * 工序参数定义 （子表：TaktRoutingItemArgument）
   */
  arguments?: RoutingItemArgument[];

}


/**
 * RoutingItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 RoutingItemQuery
 * @description 对应后端 TaktRoutingItemQueryDto
 */
export interface RoutingItemQuery extends TaktPagedQuery {
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
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId?: string;

  /**
   * 工艺路线编码（冗余字段，便于查询）
   */
  routingCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 基本数量
   */
  baseQuantity?: number;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
   */
  pointsToMinutesRate?: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 准备时间（分钟），如换模、调试等
   */
  setupMinutes?: number;

  /**
   * 清理时间（分钟），如清洁、整理等
   */
  teardownMinutes?: number;

  /**
   * 检验（字典 sys_yes_no：0=否，1=是）
   */
  isInspection?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 工序说明
   */
  processDescription?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType?: number;

  /**
   * 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
   */
  extJson?: string;

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
 * 创建RoutingItem DTO
 * 对应前端 RoutingItemCreate
 * @description 对应后端 TaktRoutingItemCreateDto
 */
export interface RoutingItemCreate {
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
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId: string;

  /**
   * 工艺路线编码（冗余字段，便于查询）
   */
  routingCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 基本数量
   */
  baseQuantity: number;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
   */
  pointsToMinutesRate: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 准备时间（分钟），如换模、调试等
   */
  setupMinutes: number;

  /**
   * 清理时间（分钟），如清洁、整理等
   */
  teardownMinutes: number;

  /**
   * 检验（字典 sys_yes_no：0=否，1=是）
   */
  isInspection: number;

  /**
   * 工序说明
   */
  processDescription?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType: number;

  /**
   * 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
   */
  extJson?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 工序参数定义（子表，级联保存）
   */
  arguments?: RoutingItemArgumentCreate[];

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
 * 更新RoutingItem DTO
 * 继承 TaktRoutingItemCreateDto，添加 RoutingItemId 字段
 * 对应前端 RoutingItemUpdate
 * @description 对应后端 TaktRoutingItemUpdateDto
 */
export interface RoutingItemUpdate extends RoutingItemCreate {
  /**
   * RoutingItemID（标识要更新的实体）
   */
  routingItemId: string;

  /**
   * 工序参数定义（子表，级联保存）
   */
  arguments?: any;

}


/**
 * RoutingItem 排序更新 DTO
 * 对应前端 RoutingItemSort
 * @description 对应后端 TaktRoutingItemSortDto
 */
export interface RoutingItemSort {
  /**
   * RoutingItemID
   */
  routingItemId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * RoutingItem 作废/撤销作废 DTO
 * 对应前端 RoutingItemObsolete
 * @description 对应后端 TaktRoutingItemObsoleteDto
 */
export interface RoutingItemObsolete {
  /**
   * RoutingItemID
   */
  routingItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * RoutingItem 导入模板行 DTO
 * 对应前端 RoutingItemTemplate
 * @description 对应后端 TaktRoutingItemTemplateDto
 */
export interface RoutingItemTemplate {
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
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId?: string;

  /**
   * 工艺路线编码（冗余字段，便于查询）
   */
  routingCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 基本数量
   */
  baseQuantity?: number;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
   */
  pointsToMinutesRate?: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 准备时间（分钟），如换模、调试等
   */
  setupMinutes?: number;

  /**
   * 清理时间（分钟），如清洁、整理等
   */
  teardownMinutes?: number;

  /**
   * 检验（字典 sys_yes_no：0=否，1=是）
   */
  isInspection?: number;

  /**
   * 工序说明
   */
  processDescription?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType?: number;

  /**
   * 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
   */
  extJson?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 工序参数定义（子表，级联保存）
   */
  arguments?: RoutingItemArgumentCreate[];

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
 * RoutingItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 RoutingItemImport
 * @description 对应后端 TaktRoutingItemImportDto
 */
export interface RoutingItemImport {
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
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId?: string;

  /**
   * 工艺路线编码（冗余字段，便于查询）
   */
  routingCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 基本数量
   */
  baseQuantity?: number;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
   */
  pointsToMinutesRate?: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 准备时间（分钟），如换模、调试等
   */
  setupMinutes?: number;

  /**
   * 清理时间（分钟），如清洁、整理等
   */
  teardownMinutes?: number;

  /**
   * 检验（字典 sys_yes_no：0=否，1=是）
   */
  isInspection?: number;

  /**
   * 工序说明
   */
  processDescription?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType?: number;

  /**
   * 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
   */
  extJson?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 工序参数定义（子表，级联保存）
   */
  arguments?: RoutingItemArgumentCreate[];

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
 * RoutingItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 RoutingItemExport
 * @description 对应后端 TaktRoutingItemExportDto
 */
export interface RoutingItemExport {
  /**
   * RoutingItemID
   */
  routingItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId: string;

  /**
   * 工艺路线编码（冗余字段，便于查询）
   */
  routingCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 基本数量
   */
  baseQuantity: number;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
   */
  pointsToMinutesRate: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 准备时间（分钟），如换模、调试等
   */
  setupMinutes: number;

  /**
   * 清理时间（分钟），如清洁、整理等
   */
  teardownMinutes: number;

  /**
   * 检验（字典 sys_yes_no：0=否，1=是）
   */
  isInspection: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 工序说明
   */
  processDescription?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType: number;

  /**
   * 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
   */
  extJson?: string;

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

