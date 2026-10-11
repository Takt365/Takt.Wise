// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：schedule.types.ts
// 创建时间：2026-07-24
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
 * APS排程主表（高级计划与排程）
 * 对应前端 TaktApsScheduleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsSchedule
 * @description 对应后端 TaktApsScheduleDto
 */
export interface ApsSchedule extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 工厂编码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode?: string;

  /**
   * 排程名称
   */
  scheduleName?: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType?: number;

  /**
   * 计划日期
   */
  planDate?: string;

  /**
   * 计划开始时间
   */
  planStartTime?: string;

  /**
   * 计划结束时间
   */
  planEndTime?: string;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle?: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产班组编码
   */
  productionLineCode?: string;

  /**
   * 生产班组名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy?: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm?: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective?: number;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus?: number;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

  /**
   * 发布人ID（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）
   */
  publishUserId?: string;

  /**
   * 发布人姓名
   */
  publishUserName?: string;

  /**
   * 排程说明
   */
  scheduleDescription?: string;

  /**
   * APS 排程订单列表（排程批次关联的订单）（子表，级联保存）
   */
  orders?: ApsOrderCreate[];

  /**
   * 排程明细列表（主子表关系）（子表，级联保存）
   */
  items?: ApsScheduleItemCreate[];

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
 * ApsSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsScheduleExport
 * @description 对应后端 TaktApsScheduleExportDto
 */
export interface ApsScheduleExport {
  /**
   * ApsScheduleID
   */
  apsScheduleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 工厂编码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode: string;

  /**
   * 排程名称
   */
  scheduleName: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType: number;

  /**
   * 计划日期
   */
  planDate: string;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产班组编码
   */
  productionLineCode?: string;

  /**
   * 生产班组名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective: number;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus: number;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

  /**
   * 发布人ID（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）
   */
  publishUserId?: string;

  /**
   * 发布人姓名
   */
  publishUserName?: string;

  /**
   * 排程说明
   */
  scheduleDescription?: string;

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

