// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowVariable.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 流程变量实体
 * 对应前端 TaktFlowVariableDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowVariable
 * @description 对应后端 TaktFlowVariableDto
 */
export interface FlowVariable extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName?: string;

  /**
   * 变量类型
   */
  variableType?: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * FlowVariable 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowVariableExport
 * @description 对应后端 TaktFlowVariableExportDto
 */
export interface FlowVariableExport {
  /**
   * FlowVariableID
   */
  flowVariableId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName: string;

  /**
   * 变量类型
   */
  variableType: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

  /**
   * 双精度值
   */
  doubleValue?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

