// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopExecStep.types.ts
// 创建时间：2026-06-15
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
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 工步 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  stepId?: string;

  /**
   * 工步序号快照
   */
  stepNo?: number;

  /**
   * 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_manufacturing_sop_check_result）
   */
  stepResult?: number;

  /**
   * 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  confirmedBy?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no，扫码 NG 等）
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
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId: string;

  /**
   * 工步 ID（序列化为 string 以避免 Javascript 精度问题）
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
   * 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_manufacturing_sop_check_result）
   */
  stepResult?: number;

  /**
   * 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  confirmedBy?: string;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否禁止下一步（字典 sys_yes_no，扫码 NG 等）
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

