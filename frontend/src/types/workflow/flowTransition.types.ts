// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowTransition.types.ts
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
 * 流程流转历史实体
 * 对应前端 TaktFlowTransitionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowTransition
 * @description 对应后端 TaktFlowTransitionDto
 */
export interface FlowTransition extends CompanyDtoBase {
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
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId?: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

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
 * FlowTransition 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowTransitionExport
 * @description 对应后端 TaktFlowTransitionExportDto
 */
export interface FlowTransitionExport {
  /**
   * FlowTransitionID
   */
  flowTransitionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  transitionTime: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

  /**
   * 动作类型
   */
  actionType: number;

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

