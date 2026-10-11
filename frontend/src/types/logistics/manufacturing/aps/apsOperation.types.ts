// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：apsOperation.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * APS 工序排程（APS_Order → Operation，关联 RoutingItem 与 WC/Resource）
 * 对应前端 TaktApsOperationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsOperation
 * @description 对应后端 TaktApsOperationDto
 */
export interface ApsOperation extends CompanyDtoBase {
  /**
   * ApsOperationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  apsOperationId: string;

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId: string;

  /**
   * APS 订单 名称（填充字段）
   */
  apsOrderName?: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode: string;

  /**
   * 行号（工序序号）
   */
  lineNumber: number;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工艺路线工序 名称（填充字段）
   */
  routingItemName?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（关联 TaktWorkCenterResource.Id，选项 TaktWorkCenterResources/options）
   */
  workCenterResourceId?: string;

  /**
   * 工作中心资源 名称（填充字段）
   */
  workCenterResourceName?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ApsOperation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ApsOperationQuery
 * @description 对应后端 TaktApsOperationQueryDto
 */
export interface ApsOperationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId?: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode?: string;

  /**
   * 行号（工序序号）
   */
  lineNumber?: number;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（关联 TaktWorkCenterResource.Id，选项 TaktWorkCenterResources/options）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间（范围查询-开始）
   */
  plannedStartTimeStart?: string;

  /**
   * 计划开始时间（范围查询-结束）
   */
  plannedStartTimeEnd?: string;

  /**
   * 计划结束时间（范围查询-开始）
   */
  plannedEndTimeStart?: string;

  /**
   * 计划结束时间（范围查询-结束）
   */
  plannedEndTimeEnd?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes?: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes?: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * 创建ApsOperation DTO
 * 对应前端 ApsOperationCreate
 * @description 对应后端 TaktApsOperationCreateDto
 */
export interface ApsOperationCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode: string;

  /**
   * 行号（工序序号）
   */
  lineNumber: number;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（关联 TaktWorkCenterResource.Id，选项 TaktWorkCenterResources/options）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * 更新ApsOperation DTO
 * 继承 TaktApsOperationCreateDto，添加 ApsOperationId 字段
 * 对应前端 ApsOperationUpdate
 * @description 对应后端 TaktApsOperationUpdateDto
 */
export interface ApsOperationUpdate extends ApsOperationCreate {
  /**
   * ApsOperationID（标识要更新的实体）
   */
  apsOperationId: string;

}


/**
 * ApsOperation 状态更新 DTO
 * 对应前端 ApsOperationStatus
 * @description 对应后端 TaktApsOperationStatusDto
 */
export interface ApsOperationStatus {
  /**
   * ApsOperationID
   */
  apsOperationId: string;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus: number;

}


/**
 * ApsOperation 作废/撤销作废 DTO
 * 对应前端 ApsOperationObsolete
 * @description 对应后端 TaktApsOperationObsoleteDto
 */
export interface ApsOperationObsolete {
  /**
   * ApsOperationID
   */
  apsOperationId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ApsOperation 导入模板行 DTO
 * 对应前端 ApsOperationTemplate
 * @description 对应后端 TaktApsOperationTemplateDto
 */
export interface ApsOperationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId?: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode?: string;

  /**
   * 行号（工序序号）
   */
  lineNumber?: number;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（关联 TaktWorkCenterResource.Id，选项 TaktWorkCenterResources/options）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes?: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes?: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * ApsOperation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ApsOperationImport
 * @description 对应后端 TaktApsOperationImportDto
 */
export interface ApsOperationImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId?: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode?: string;

  /**
   * 行号（工序序号）
   */
  lineNumber?: number;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（关联 TaktWorkCenterResource.Id，选项 TaktWorkCenterResources/options）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes?: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes?: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * ApsOperation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsOperationExport
 * @description 对应后端 TaktApsOperationExportDto
 */
export interface ApsOperationExport {
  /**
   * ApsOperationID
   */
  apsOperationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode: string;

  /**
   * 行号（工序序号）
   */
  lineNumber: number;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（关联 TaktWorkCenterResource.Id，选项 TaktWorkCenterResources/options）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

