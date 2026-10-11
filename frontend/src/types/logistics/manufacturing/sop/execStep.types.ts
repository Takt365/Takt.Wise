// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：execStep.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 工步执行明细实体
 * 对应前端 TaktSopExecStepDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopExecStep
 * @description 对应后端 TaktSopExecStepDto
 */
export interface SopExecStep extends CompanyDtoBase {
  /**
   * SopExecStepID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopExecStepId: string;

  /**
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 执行追溯 名称（填充字段）
   */
  execName?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 工步 名称（填充字段）
   */
  stepName?: string;

  /**
   * 工步序号快照
   */
  stepNo: number;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  stepResult?: number;

  /**
   * 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  confirmedBy?: string;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
   */
  blockNextStep: number;

  /**
   * 执行追溯 （主表：TaktSopExec）
   */
  exec?: SopExec;

}


/**
 * SopExecStep 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopExecStepQuery
 * @description 对应后端 TaktSopExecStepQueryDto
 */
export interface SopExecStepQuery extends TaktPagedQuery {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 工步序号快照
   */
  stepNo?: number;

  /**
   * 开始时间（范围查询-开始）
   */
  startedAtStart?: string;

  /**
   * 开始时间（范围查询-结束）
   */
  startedAtEnd?: string;

  /**
   * 结束时间（范围查询-开始）
   */
  endedAtStart?: string;

  /**
   * 结束时间（范围查询-结束）
   */
  endedAtEnd?: string;

  /**
   * 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  stepResult?: number;

  /**
   * 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  confirmedBy?: string;

  /**
   * 确认时间（范围查询-开始）
   */
  confirmedAtStart?: string;

  /**
   * 确认时间（范围查询-结束）
   */
  confirmedAtEnd?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
   */
  blockNextStep?: number;

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
 * 创建SopExecStep DTO
 * 对应前端 SopExecStepCreate
 * @description 对应后端 TaktSopExecStepCreateDto
 */
export interface SopExecStepCreate {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 工步序号快照
   */
  stepNo: number;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  stepResult?: number;

  /**
   * 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  confirmedBy?: string;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
   */
  blockNextStep: number;

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
 * 更新SopExecStep DTO
 * 继承 TaktSopExecStepCreateDto，添加 SopExecStepId 字段
 * 对应前端 SopExecStepUpdate
 * @description 对应后端 TaktSopExecStepUpdateDto
 */
export interface SopExecStepUpdate extends SopExecStepCreate {
  /**
   * SopExecStepID（标识要更新的实体）
   */
  sopExecStepId: string;

}


/**
 * SopExecStep 导入模板行 DTO
 * 对应前端 SopExecStepTemplate
 * @description 对应后端 TaktSopExecStepTemplateDto
 */
export interface SopExecStepTemplate {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 工步序号快照
   */
  stepNo?: number;

  /**
   * 开始时间
   */
  startedAt?: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  stepResult?: number;

  /**
   * 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  confirmedBy?: string;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
   */
  blockNextStep?: number;

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
 * SopExecStep 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopExecStepImport
 * @description 对应后端 TaktSopExecStepImportDto
 */
export interface SopExecStepImport {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 工步序号快照
   */
  stepNo?: number;

  /**
   * 开始时间
   */
  startedAt?: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  stepResult?: number;

  /**
   * 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  confirmedBy?: string;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
   */
  blockNextStep?: number;

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
 * SopExecStep 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopExecStepExport
 * @description 对应后端 TaktSopExecStepExportDto
 */
export interface SopExecStepExport {
  /**
   * SopExecStepID
   */
  sopExecStepId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 工步序号快照
   */
  stepNo: number;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  stepResult?: number;

  /**
   * 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  confirmedBy?: string;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
   */
  blockNextStep: number;

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

