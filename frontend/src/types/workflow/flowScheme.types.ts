// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowScheme.types.ts
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
 * 流程定义实体（前端流程方案 FlowScheme）
 * 对应前端 TaktFlowSchemeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowScheme
 * @description 对应后端 TaktFlowSchemeDto
 */
export interface FlowScheme extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey?: string;

  /**
   * 流程名称
   */
  processName?: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion?: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion?: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest?: number;

  /**
   * 流程分类
   */
  processCategory?: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus?: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState?: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

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
 * FlowScheme 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowSchemeExport
 * @description 对应后端 TaktFlowSchemeExportDto
 */
export interface FlowSchemeExport {
  /**
   * FlowSchemeID
   */
  flowSchemeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest: number;

  /**
   * 流程分类
   */
  processCategory: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 排序号
   */
  sortOrder: number;

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

/**
 * 方案编辑弹窗表单模型（FlowSchemeCreate 字段 + 可选 flowSchemeId / processContent）
 */
export type FlowSchemeFormModel = Partial<FlowSchemeCreate> & {
  flowSchemeId?: string
  processContent?: string
}

