// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowTask.types.ts
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
 * 流程用户任务实体
 * 对应前端 TaktFlowTaskDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowTask
 * @description 对应后端 TaktFlowTaskDto
 */
export interface FlowTask extends CompanyDtoBase {
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
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId?: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 会签类型
   */
  signType?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 是否加签任务
   */
  isAddSign?: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
   */
  sortOrder?: number;

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
 * FlowTask 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowTaskExport
 * @description 对应后端 TaktFlowTaskExportDto
 */
export interface FlowTaskExport {
  /**
   * FlowTaskID
   */
  flowTaskId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 会签类型
   */
  signType: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 到期时间
   */
  dueDate?: string;

  /**
   * 认领时间
   */
  claimTime?: string;

  /**
   * 办结时间
   */
  completedAt?: string;

  /**
   * 是否加签任务
   */
  isAddSign: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
   */
  sortOrder: number;

  /**
   * 审批意见
   */
  comment?: string;

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

