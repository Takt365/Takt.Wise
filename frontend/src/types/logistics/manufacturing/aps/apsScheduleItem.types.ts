// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：apsScheduleItem.types.ts
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
 * APS排程明细（排程的具体工序任务）
 * 对应前端 TaktApsScheduleItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsScheduleItem
 * @description 对应后端 TaktApsScheduleItemDto
 */
export interface ApsScheduleItem extends CompanyDtoBase {
  /**
   * ApsScheduleItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  apsScheduleItemId: string;

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * APS排程名称（填充字段）
   */
  apsScheduleName?: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode: string;

  /**
   * APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options）
   */
  apsOrderId?: string;

  /**
   * APS 订单 名称（填充字段）
   */
  apsOrderName?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * APS 工序排程 名称（填充字段）
   */
  apsOperationName?: string;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 工艺路线工序 名称（填充字段）
   */
  routingItemName?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产工单编码（关联 TaktProductionOrder.ProdOrderCode，选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  workOrderCode: string;

  /**
   * 产品编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  productCode: string;

  /**
   * 产品名称
   */
  productName: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 工序序号
   */
  processSequence: number;

  /**
   * 工序标准ST值
   */
  processStandardST: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes: number;

  /**
   * 计划数量
   */
  planQuantity: number;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * APS排程主表（主表） （主表：TaktApsSchedule）
   */
  schedule?: ApsSchedule;

}


/**
 * ApsScheduleItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ApsScheduleItemQuery
 * @description 对应后端 TaktApsScheduleItemQueryDto
 */
export interface ApsScheduleItemQuery extends TaktPagedQuery {
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
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId?: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode?: string;

  /**
   * APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产工单编码（关联 TaktProductionOrder.ProdOrderCode，选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  workOrderCode?: string;

  /**
   * 产品编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工序序号
   */
  processSequence?: number;

  /**
   * 工序标准ST值
   */
  processStandardST?: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit?: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes?: number;

  /**
   * 计划数量
   */
  planQuantity?: number;

  /**
   * 计划开始时间（范围查询-开始）
   */
  planStartTimeStart?: string;

  /**
   * 计划开始时间（范围查询-结束）
   */
  planStartTimeEnd?: string;

  /**
   * 计划结束时间（范围查询-开始）
   */
  planEndTimeStart?: string;

  /**
   * 计划结束时间（范围查询-结束）
   */
  planEndTimeEnd?: string;

  /**
   * 实际开始时间（范围查询-开始）
   */
  actualStartTimeStart?: string;

  /**
   * 实际开始时间（范围查询-结束）
   */
  actualStartTimeEnd?: string;

  /**
   * 实际结束时间（范围查询-开始）
   */
  actualEndTimeStart?: string;

  /**
   * 实际结束时间（范围查询-结束）
   */
  actualEndTimeEnd?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus?: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority?: number;

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
 * 创建ApsScheduleItem DTO
 * 对应前端 ApsScheduleItemCreate
 * @description 对应后端 TaktApsScheduleItemCreateDto
 */
export interface ApsScheduleItemCreate {
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
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode: string;

  /**
   * APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产工单编码（关联 TaktProductionOrder.ProdOrderCode，选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  workOrderCode: string;

  /**
   * 产品编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  productCode: string;

  /**
   * 产品名称
   */
  productName: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 工序序号
   */
  processSequence: number;

  /**
   * 工序标准ST值
   */
  processStandardST: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes: number;

  /**
   * 计划数量
   */
  planQuantity: number;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority: number;

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
 * 更新ApsScheduleItem DTO
 * 继承 TaktApsScheduleItemCreateDto，添加 ApsScheduleItemId 字段
 * 对应前端 ApsScheduleItemUpdate
 * @description 对应后端 TaktApsScheduleItemUpdateDto
 */
export interface ApsScheduleItemUpdate extends ApsScheduleItemCreate {
  /**
   * ApsScheduleItemID（标识要更新的实体）
   */
  apsScheduleItemId: string;

}


/**
 * ApsScheduleItem 状态更新 DTO
 * 对应前端 ApsScheduleItemStatus
 * @description 对应后端 TaktApsScheduleItemStatusDto
 */
export interface ApsScheduleItemStatus {
  /**
   * ApsScheduleItemID
   */
  apsScheduleItemId: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus: number;

}


/**
 * ApsScheduleItem 作废/撤销作废 DTO
 * 对应前端 ApsScheduleItemObsolete
 * @description 对应后端 TaktApsScheduleItemObsoleteDto
 */
export interface ApsScheduleItemObsolete {
  /**
   * ApsScheduleItemID
   */
  apsScheduleItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ApsScheduleItem 导入模板行 DTO
 * 对应前端 ApsScheduleItemTemplate
 * @description 对应后端 TaktApsScheduleItemTemplateDto
 */
export interface ApsScheduleItemTemplate {
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
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId?: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode?: string;

  /**
   * APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产工单编码（关联 TaktProductionOrder.ProdOrderCode，选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  workOrderCode?: string;

  /**
   * 产品编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工序序号
   */
  processSequence?: number;

  /**
   * 工序标准ST值
   */
  processStandardST?: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit?: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes?: number;

  /**
   * 计划数量
   */
  planQuantity?: number;

  /**
   * 计划开始时间
   */
  planStartTime?: string;

  /**
   * 计划结束时间
   */
  planEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus?: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority?: number;

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
 * ApsScheduleItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ApsScheduleItemImport
 * @description 对应后端 TaktApsScheduleItemImportDto
 */
export interface ApsScheduleItemImport {
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
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId?: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode?: string;

  /**
   * APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产工单编码（关联 TaktProductionOrder.ProdOrderCode，选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  workOrderCode?: string;

  /**
   * 产品编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工序序号
   */
  processSequence?: number;

  /**
   * 工序标准ST值
   */
  processStandardST?: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit?: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes?: number;

  /**
   * 计划数量
   */
  planQuantity?: number;

  /**
   * 计划开始时间
   */
  planStartTime?: string;

  /**
   * 计划结束时间
   */
  planEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus?: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority?: number;

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
 * ApsScheduleItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsScheduleItemExport
 * @description 对应后端 TaktApsScheduleItemExportDto
 */
export interface ApsScheduleItemExport {
  /**
   * ApsScheduleItemID
   */
  apsScheduleItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode: string;

  /**
   * APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（关联 TaktRoutingItem.Id，选项 TaktRoutingItems/options）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产工单编码（关联 TaktProductionOrder.ProdOrderCode，选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  workOrderCode: string;

  /**
   * 产品编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  productCode: string;

  /**
   * 产品名称
   */
  productName: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 工序序号
   */
  processSequence: number;

  /**
   * 工序标准ST值
   */
  processStandardST: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes: number;

  /**
   * 计划数量
   */
  planQuantity: number;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority: number;

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

