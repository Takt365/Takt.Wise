// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/training
// 文件名称：plan.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 培训计划（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktTrainingPlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TrainingPlan
 * @description 对应后端 TaktTrainingPlanDto
 */
export interface TrainingPlan extends ApprovalDtoBase {
  /**
   * TrainingPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  trainingPlanId: string;

  /**
   * 计划编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 计划名称
   */
  planName: string;

  /**
   * 计划年度
   */
  planYear: number;

  /**
   * 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
   */
  planType: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 计划开始日期
   */
  startDate: string;

  /**
   * 计划结束日期
   */
  endDate: string;

  /**
   * 培训目标
   */
  trainingObjectives: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount: number;

  /**
   * 培训预算（元）
   */
  trainingBudget: number;

  /**
   * 计划说明
   */
  trainingPlanDescription: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus: number;

}


/**
 * TrainingPlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TrainingPlanQuery
 * @description 对应后端 TaktTrainingPlanQueryDto
 */
export interface TrainingPlanQuery extends TaktPagedQuery {
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
   * 计划编码（租户+公司内唯一）
   */
  planCode?: string;

  /**
   * 计划名称
   */
  planName?: string;

  /**
   * 计划年度
   */
  planYear?: number;

  /**
   * 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
   */
  planType?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 计划开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 计划开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 计划结束日期（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 计划结束日期（范围查询-结束）
   */
  endDateEnd?: string;

  /**
   * 培训目标
   */
  trainingObjectives?: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount?: number;

  /**
   * 培训预算（元）
   */
  trainingBudget?: number;

  /**
   * 计划说明
   */
  trainingPlanDescription?: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus?: number;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建TrainingPlan DTO
 * 对应前端 TrainingPlanCreate
 * @description 对应后端 TaktTrainingPlanCreateDto
 */
export interface TrainingPlanCreate {
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
   * 计划编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 计划名称
   */
  planName: string;

  /**
   * 计划年度
   */
  planYear: number;

  /**
   * 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
   */
  planType: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 计划开始日期
   */
  startDate: string;

  /**
   * 计划结束日期
   */
  endDate: string;

  /**
   * 培训目标
   */
  trainingObjectives: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount: number;

  /**
   * 培训预算（元）
   */
  trainingBudget: number;

  /**
   * 计划说明
   */
  trainingPlanDescription: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus: number;

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
 * 更新TrainingPlan DTO
 * 继承 TaktTrainingPlanCreateDto，添加 TrainingPlanId 字段
 * 对应前端 TrainingPlanUpdate
 * @description 对应后端 TaktTrainingPlanUpdateDto
 */
export interface TrainingPlanUpdate extends TrainingPlanCreate {
  /**
   * TrainingPlanID（标识要更新的实体）
   */
  trainingPlanId: string;

}


/**
 * TrainingPlan 状态更新 DTO
 * 对应前端 TrainingPlanStatus
 * @description 对应后端 TaktTrainingPlanStatusDto
 */
export interface TrainingPlanStatus {
  /**
   * TrainingPlanID
   */
  trainingPlanId: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus: number;

}


/**
 * TrainingPlan 导入模板行 DTO
 * 对应前端 TrainingPlanTemplate
 * @description 对应后端 TaktTrainingPlanTemplateDto
 */
export interface TrainingPlanTemplate {
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
   * 计划编码（租户+公司内唯一）
   */
  planCode?: string;

  /**
   * 计划名称
   */
  planName?: string;

  /**
   * 计划年度
   */
  planYear?: number;

  /**
   * 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
   */
  planType?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 计划开始日期
   */
  startDate?: string;

  /**
   * 计划结束日期
   */
  endDate?: string;

  /**
   * 培训目标
   */
  trainingObjectives?: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount?: number;

  /**
   * 培训预算（元）
   */
  trainingBudget?: number;

  /**
   * 计划说明
   */
  trainingPlanDescription?: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus?: number;

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
 * TrainingPlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TrainingPlanImport
 * @description 对应后端 TaktTrainingPlanImportDto
 */
export interface TrainingPlanImport {
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
   * 计划编码（租户+公司内唯一）
   */
  planCode?: string;

  /**
   * 计划名称
   */
  planName?: string;

  /**
   * 计划年度
   */
  planYear?: number;

  /**
   * 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
   */
  planType?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 计划开始日期
   */
  startDate?: string;

  /**
   * 计划结束日期
   */
  endDate?: string;

  /**
   * 培训目标
   */
  trainingObjectives?: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount?: number;

  /**
   * 培训预算（元）
   */
  trainingBudget?: number;

  /**
   * 计划说明
   */
  trainingPlanDescription?: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus?: number;

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
 * TrainingPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrainingPlanExport
 * @description 对应后端 TaktTrainingPlanExportDto
 */
export interface TrainingPlanExport {
  /**
   * TrainingPlanID
   */
  trainingPlanId: string;

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
   * 计划编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 计划名称
   */
  planName: string;

  /**
   * 计划年度
   */
  planYear: number;

  /**
   * 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
   */
  planType: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 计划开始日期
   */
  startDate: string;

  /**
   * 计划结束日期
   */
  endDate: string;

  /**
   * 培训目标
   */
  trainingObjectives: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount: number;

  /**
   * 培训预算（元）
   */
  trainingBudget: number;

  /**
   * 计划说明
   */
  trainingPlanDescription: string;

  /**
   * 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  trainingPlanStatus: number;

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

