// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：quartzTask.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Quartz 定时任务实体
 * 对应前端 TaktQuartzTaskDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QuartzTask
 * @description 对应后端 TaktQuartzTaskDto
 */
export interface QuartzTask extends CompanyDtoBase {
  /**
   * QuartzTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  quartzTaskId: string;

  /**
   * 任务编码（租户+公司内唯一）
   */
  taskCode: string;

  /**
   * 任务名称
   */
  taskName: string;

  /**
   * Quartz Job 名称
   */
  jobName: string;

  /**
   * Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
   */
  taskType: string;

  /**
   * 程序集名称（任务类型为程序集时使用）
   */
  assemblyName: string;

  /**
   * 任务类名（任务类型为程序集时使用）
   */
  className: string;

  /**
   * API 执行地址（任务类型为网络请求时使用）
   */
  apiUrl?: string;

  /**
   * 网络请求方式（GET/POST 等）
   */
  requestMethod?: string;

  /**
   * SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
   */
  sqlScript?: string;

  /**
   * 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
   */
  triggerType: number;

  /**
   * Cron 表达式（触发器类型为 Cron 时使用）
   */
  cronExpression: string;

  /**
   * 执行间隔时间（秒，触发器类型为 Simple 时使用）
   */
  intervalSeconds: number;

  /**
   * 执行参数
   */
  executeParams?: string;

  /**
   * 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
   */
  concurrent: number;

  /**
   * Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
   */
  misfirePolicy: number;

  /**
   * 首次执行（调度生效开始时间）
   */
  firstRunAt?: string;

  /**
   * 执行次数
   */
  executeCount: number;

  /**
   * 上次执行
   */
  lastRunAt?: string;

  /**
   * 下次执行
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  taskDescription?: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus: number;

}


/**
 * QuartzTask 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QuartzTaskQuery
 * @description 对应后端 TaktQuartzTaskQueryDto
 */
export interface QuartzTaskQuery extends TaktPagedQuery {
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
   * 任务编码（租户+公司内唯一）
   */
  taskCode?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * Quartz Job 名称
   */
  jobName?: string;

  /**
   * Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup?: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
   */
  taskType?: string;

  /**
   * 程序集名称（任务类型为程序集时使用）
   */
  assemblyName?: string;

  /**
   * 任务类名（任务类型为程序集时使用）
   */
  className?: string;

  /**
   * API 执行地址（任务类型为网络请求时使用）
   */
  apiUrl?: string;

  /**
   * 网络请求方式（GET/POST 等）
   */
  requestMethod?: string;

  /**
   * SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
   */
  sqlScript?: string;

  /**
   * 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
   */
  triggerType?: number;

  /**
   * Cron 表达式（触发器类型为 Cron 时使用）
   */
  cronExpression?: string;

  /**
   * 执行间隔时间（秒，触发器类型为 Simple 时使用）
   */
  intervalSeconds?: number;

  /**
   * 执行参数
   */
  executeParams?: string;

  /**
   * 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
   */
  concurrent?: number;

  /**
   * Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
   */
  misfirePolicy?: number;

  /**
   * 首次执行（调度生效开始时间）（范围查询-开始）
   */
  firstRunAtStart?: string;

  /**
   * 首次执行（调度生效开始时间）（范围查询-结束）
   */
  firstRunAtEnd?: string;

  /**
   * 执行次数
   */
  executeCount?: number;

  /**
   * 上次执行（范围查询-开始）
   */
  lastRunAtStart?: string;

  /**
   * 上次执行（范围查询-结束）
   */
  lastRunAtEnd?: string;

  /**
   * 下次执行（范围查询-开始）
   */
  nextRunAtStart?: string;

  /**
   * 下次执行（范围查询-结束）
   */
  nextRunAtEnd?: string;

  /**
   * 任务描述
   */
  taskDescription?: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus?: number;

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
 * 创建QuartzTask DTO
 * 对应前端 QuartzTaskCreate
 * @description 对应后端 TaktQuartzTaskCreateDto
 */
export interface QuartzTaskCreate {
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
   * 任务编码（租户+公司内唯一）
   */
  taskCode: string;

  /**
   * 任务名称
   */
  taskName: string;

  /**
   * Quartz Job 名称
   */
  jobName: string;

  /**
   * Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
   */
  taskType: string;

  /**
   * 程序集名称（任务类型为程序集时使用）
   */
  assemblyName: string;

  /**
   * 任务类名（任务类型为程序集时使用）
   */
  className: string;

  /**
   * API 执行地址（任务类型为网络请求时使用）
   */
  apiUrl?: string;

  /**
   * 网络请求方式（GET/POST 等）
   */
  requestMethod?: string;

  /**
   * SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
   */
  sqlScript?: string;

  /**
   * 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
   */
  triggerType: number;

  /**
   * Cron 表达式（触发器类型为 Cron 时使用）
   */
  cronExpression: string;

  /**
   * 执行间隔时间（秒，触发器类型为 Simple 时使用）
   */
  intervalSeconds: number;

  /**
   * 执行参数
   */
  executeParams?: string;

  /**
   * 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
   */
  concurrent: number;

  /**
   * Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
   */
  misfirePolicy: number;

  /**
   * 首次执行（调度生效开始时间）
   */
  firstRunAt?: string;

  /**
   * 执行次数
   */
  executeCount: number;

  /**
   * 上次执行
   */
  lastRunAt?: string;

  /**
   * 下次执行
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  taskDescription?: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus: number;

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
 * 更新QuartzTask DTO
 * 继承 TaktQuartzTaskCreateDto，添加 QuartzTaskId 字段
 * 对应前端 QuartzTaskUpdate
 * @description 对应后端 TaktQuartzTaskUpdateDto
 */
export interface QuartzTaskUpdate extends QuartzTaskCreate {
  /**
   * QuartzTaskID（标识要更新的实体）
   */
  quartzTaskId: string;

}


/**
 * QuartzTask 状态更新 DTO
 * 对应前端 QuartzTaskStatus
 * @description 对应后端 TaktQuartzTaskStatusDto
 */
export interface QuartzTaskStatus {
  /**
   * QuartzTaskID
   */
  quartzTaskId: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus: number;

}


/**
 * QuartzTask 导入模板行 DTO
 * 对应前端 QuartzTaskTemplate
 * @description 对应后端 TaktQuartzTaskTemplateDto
 */
export interface QuartzTaskTemplate {
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
   * 任务编码（租户+公司内唯一）
   */
  taskCode?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * Quartz Job 名称
   */
  jobName?: string;

  /**
   * Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup?: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
   */
  taskType?: string;

  /**
   * 程序集名称（任务类型为程序集时使用）
   */
  assemblyName?: string;

  /**
   * 任务类名（任务类型为程序集时使用）
   */
  className?: string;

  /**
   * API 执行地址（任务类型为网络请求时使用）
   */
  apiUrl?: string;

  /**
   * 网络请求方式（GET/POST 等）
   */
  requestMethod?: string;

  /**
   * SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
   */
  sqlScript?: string;

  /**
   * 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
   */
  triggerType?: number;

  /**
   * Cron 表达式（触发器类型为 Cron 时使用）
   */
  cronExpression?: string;

  /**
   * 执行间隔时间（秒，触发器类型为 Simple 时使用）
   */
  intervalSeconds?: number;

  /**
   * 执行参数
   */
  executeParams?: string;

  /**
   * 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
   */
  concurrent?: number;

  /**
   * Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
   */
  misfirePolicy?: number;

  /**
   * 首次执行（调度生效开始时间）
   */
  firstRunAt?: string;

  /**
   * 执行次数
   */
  executeCount?: number;

  /**
   * 上次执行
   */
  lastRunAt?: string;

  /**
   * 下次执行
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  taskDescription?: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus?: number;

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
 * QuartzTask 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QuartzTaskImport
 * @description 对应后端 TaktQuartzTaskImportDto
 */
export interface QuartzTaskImport {
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
   * 任务编码（租户+公司内唯一）
   */
  taskCode?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * Quartz Job 名称
   */
  jobName?: string;

  /**
   * Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup?: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
   */
  taskType?: string;

  /**
   * 程序集名称（任务类型为程序集时使用）
   */
  assemblyName?: string;

  /**
   * 任务类名（任务类型为程序集时使用）
   */
  className?: string;

  /**
   * API 执行地址（任务类型为网络请求时使用）
   */
  apiUrl?: string;

  /**
   * 网络请求方式（GET/POST 等）
   */
  requestMethod?: string;

  /**
   * SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
   */
  sqlScript?: string;

  /**
   * 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
   */
  triggerType?: number;

  /**
   * Cron 表达式（触发器类型为 Cron 时使用）
   */
  cronExpression?: string;

  /**
   * 执行间隔时间（秒，触发器类型为 Simple 时使用）
   */
  intervalSeconds?: number;

  /**
   * 执行参数
   */
  executeParams?: string;

  /**
   * 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
   */
  concurrent?: number;

  /**
   * Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
   */
  misfirePolicy?: number;

  /**
   * 首次执行（调度生效开始时间）
   */
  firstRunAt?: string;

  /**
   * 执行次数
   */
  executeCount?: number;

  /**
   * 上次执行
   */
  lastRunAt?: string;

  /**
   * 下次执行
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  taskDescription?: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus?: number;

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
 * QuartzTask 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QuartzTaskExport
 * @description 对应后端 TaktQuartzTaskExportDto
 */
export interface QuartzTaskExport {
  /**
   * QuartzTaskID
   */
  quartzTaskId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 任务编码（租户+公司内唯一）
   */
  taskCode: string;

  /**
   * 任务名称
   */
  taskName: string;

  /**
   * Quartz Job 名称
   */
  jobName: string;

  /**
   * Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
   */
  jobGroup: string;

  /**
   * 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
   */
  taskType: string;

  /**
   * 程序集名称（任务类型为程序集时使用）
   */
  assemblyName: string;

  /**
   * 任务类名（任务类型为程序集时使用）
   */
  className: string;

  /**
   * API 执行地址（任务类型为网络请求时使用）
   */
  apiUrl?: string;

  /**
   * 网络请求方式（GET/POST 等）
   */
  requestMethod?: string;

  /**
   * SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
   */
  sqlScript?: string;

  /**
   * 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
   */
  triggerType: number;

  /**
   * Cron 表达式（触发器类型为 Cron 时使用）
   */
  cronExpression: string;

  /**
   * 执行间隔时间（秒，触发器类型为 Simple 时使用）
   */
  intervalSeconds: number;

  /**
   * 执行参数
   */
  executeParams?: string;

  /**
   * 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
   */
  concurrent: number;

  /**
   * Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
   */
  misfirePolicy: number;

  /**
   * 首次执行（调度生效开始时间）
   */
  firstRunAt?: string;

  /**
   * 执行次数
   */
  executeCount: number;

  /**
   * 上次执行
   */
  lastRunAt?: string;

  /**
   * 下次执行
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  taskDescription?: string;

  /**
   * 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
   */
  taskStatus: number;

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

