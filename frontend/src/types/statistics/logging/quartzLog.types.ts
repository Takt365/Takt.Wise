// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：quartzLog.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Quartz 任务执行日志实体
 * 对应前端 TaktQuartzLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QuartzLog
 * @description 对应后端 TaktQuartzLogDto
 */
export interface QuartzLog extends CompanyDtoBase {
  /**
   * QuartzLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  quartzLogId: string;

  /**
   * 关联定时任务 ID
   */
  quartzTaskId: string;

  /**
   * 关联定时任务 名称（填充字段）
   */
  quartzTaskName?: string;

  /**
   * 任务名称（执行时快照）
   */
  taskName: string;

  /**
   * 任务组名（执行时快照；字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue，如 assembly、http、sql）
   */
  taskType: string;

  /**
   * 执行时间
   */
  executeTime: string;

  /**
   * 执行耗时（毫秒）
   */
  executeDuration: string;

  /**
   * 执行参数（无参数为空串）
   */
  executeParams: string;

  /**
   * 执行消息（无消息为空串）
   */
  executeMessage: string;

  /**
   * 错误信息（成功为空串）
   */
  errorInfo: string;

  /**
   * 执行机器 IP
   */
  executeIp: string;

  /**
   * 执行机器名
   */
  executeHost: string;

  /**
   * 执行状态（0=失败，1=成功）
   */
  executeStatus: number;

  /**
   * 关联的定时任务 （主表：TaktQuartzTask）
   */
  quartzTask?: QuartzTask;

}


/**
 * QuartzLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QuartzLogQuery
 * @description 对应后端 TaktQuartzLogQueryDto
 */
export interface QuartzLogQuery extends TaktPagedQuery {
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
   * 关联定时任务 ID
   */
  quartzTaskId?: string;

  /**
   * 任务名称（执行时快照）
   */
  taskName?: string;

  /**
   * 任务组名（执行时快照；字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup?: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue，如 assembly、http、sql）
   */
  taskType?: string;

  /**
   * 执行时间（范围查询-开始）
   */
  executeTimeStart?: string;

  /**
   * 执行时间（范围查询-结束）
   */
  executeTimeEnd?: string;

  /**
   * 执行耗时（毫秒）
   */
  executeDuration?: string;

  /**
   * 执行参数（无参数为空串）
   */
  executeParams?: string;

  /**
   * 执行消息（无消息为空串）
   */
  executeMessage?: string;

  /**
   * 错误信息（成功为空串）
   */
  errorInfo?: string;

  /**
   * 执行机器 IP
   */
  executeIp?: string;

  /**
   * 执行机器名
   */
  executeHost?: string;

  /**
   * 执行状态（0=失败，1=成功）
   */
  executeStatus?: number;

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
 * 创建QuartzLog DTO
 * 对应前端 QuartzLogCreate
 * @description 对应后端 TaktQuartzLogCreateDto
 */
export interface QuartzLogCreate {
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
   * 关联定时任务 ID
   */
  quartzTaskId: string;

  /**
   * 任务名称（执行时快照）
   */
  taskName: string;

  /**
   * 任务组名（执行时快照；字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue，如 assembly、http、sql）
   */
  taskType: string;

  /**
   * 执行时间
   */
  executeTime: string;

  /**
   * 执行耗时（毫秒）
   */
  executeDuration: string;

  /**
   * 执行参数（无参数为空串）
   */
  executeParams: string;

  /**
   * 执行消息（无消息为空串）
   */
  executeMessage: string;

  /**
   * 错误信息（成功为空串）
   */
  errorInfo: string;

  /**
   * 执行机器 IP
   */
  executeIp: string;

  /**
   * 执行机器名
   */
  executeHost: string;

  /**
   * 执行状态（0=失败，1=成功）
   */
  executeStatus: number;

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
 * 更新QuartzLog DTO
 * 继承 TaktQuartzLogCreateDto，添加 QuartzLogId 字段
 * 对应前端 QuartzLogUpdate
 * @description 对应后端 TaktQuartzLogUpdateDto
 */
export interface QuartzLogUpdate extends QuartzLogCreate {
  /**
   * QuartzLogID（标识要更新的实体）
   */
  quartzLogId: string;

}


/**
 * QuartzLog 状态更新 DTO
 * 对应前端 QuartzLogStatus
 * @description 对应后端 TaktQuartzLogStatusDto
 */
export interface QuartzLogStatus {
  /**
   * QuartzLogID
   */
  quartzLogId: string;

  /**
   * 执行状态（0=失败，1=成功）
   */
  executeStatus: number;

}


/**
 * QuartzLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QuartzLogExport
 * @description 对应后端 TaktQuartzLogExportDto
 */
export interface QuartzLogExport {
  /**
   * QuartzLogID
   */
  quartzLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 关联定时任务 ID
   */
  quartzTaskId: string;

  /**
   * 任务名称（执行时快照）
   */
  taskName: string;

  /**
   * 任务组名（执行时快照；字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue，如 assembly、http、sql）
   */
  taskType: string;

  /**
   * 执行时间
   */
  executeTime: string;

  /**
   * 执行耗时（毫秒）
   */
  executeDuration: string;

  /**
   * 执行参数（无参数为空串）
   */
  executeParams: string;

  /**
   * 执行消息（无消息为空串）
   */
  executeMessage: string;

  /**
   * 错误信息（成功为空串）
   */
  errorInfo: string;

  /**
   * 执行机器 IP
   */
  executeIp: string;

  /**
   * 执行机器名
   */
  executeHost: string;

  /**
   * 执行状态（0=失败，1=成功）
   */
  executeStatus: number;

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

