// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowEngine.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 发起流程请求
 * 对应前端 FlowStart
 * @description 对应后端 TaktFlowStartDto
 */
/**
 * 按业务表提交审批
 * 对应前端 FlowSubmitByTable
 * @description 对应后端 TaktFlowSubmitByTableDto
 */
export interface FlowSubmitByTable {
  /**
   * 表单 RelatedTableName
   */
  relatedTableName: string;

  /**
   * 业务主键
   */
  entityId: string;

  /**
   * 可选流程键
   */
  processKey?: string;
}

/**
 * 可发起流程方案摘要
 * 对应前端 FlowStartableScheme
 * @description 对应后端 TaktFlowStartableSchemeDto
 */
export interface FlowStartableScheme {
  /**
   * 流程键
   */
  processKey: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 关联表单 ID
   */
  formId: string;

  /**
   * 关联表单编码
   */
  formCode: string;

  /**
   * 是否绑定数据源
   */
  isDatasource: number;

  /**
   * 关联物理表名
   */
  relatedTableName?: string;
}

export interface FlowStart {
  /**
   * 流程键
   */
  processKey: string;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 表单 JSON
   */
  frmData?: string;

  /**
   * 业务主键
   */
  businessKey?: string;

  /**
   * 业务类型
   */
  businessType?: string;

}

/**
 * 办结任务请求
 * 对应前端 FlowCompleteTask
 * @description 对应后端 TaktFlowCompleteTaskDto
 */
export interface FlowCompleteTask {
  /**
   * 流程实例 ID
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode?: string;

  /**
   * 是否通过
   */
  approved: boolean;

  /**
   * 审批意见
   */
  comment?: string;

  /**
   * 驳回到指定节点 ID
   */
  nodeRejectStep?: string;

  /**
   * 更新后的表单 JSON
   */
  frmData?: string;

}

/**
 * 转办请求
 * 对应前端 FlowTransfer
 * @description 对应后端 TaktFlowTransferDto
 */
export interface FlowTransfer {
  /**
   * 流程实例 ID
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode?: string;

  /**
   * 目标用户 ID
   */
  toUserId: string;

  /**
   * 目标用户姓名
   */
  toUserName?: string;

  /**
   * 转办说明
   */
  comment?: string;

}

/**
 * 加签人项
 * 对应前端 FlowAddApproverItem
 * @description 对应后端 TaktFlowAddApproverItemDto
 */
export interface FlowAddApproverItem {
  /**
   * 加签人用户 ID
   */
  approverUserId: string;

  /**
   * 加签人姓名
   */
  approverUserName?: string;

}

/**
 * 加签请求
 * 对应前端 FlowAddApprovers
 * @description 对应后端 TaktFlowAddApproversDto
 */
export interface FlowAddApprovers {
  /**
   * 流程实例 ID
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode?: string;

  /**
   * 加签人列表
   */
  approvers: FlowAddApproverItem[];

  /**
   * 加签方式（sequential / all / one）
   */
  approveType: string;

  /**
   * 完成后回到加签节点
   */
  returnToSignNode: boolean;

  /**
   * 加签原因
   */
  reason?: string;

}

/**
 * 减签请求
 * 对应前端 FlowReduceApproval
 * @description 对应后端 TaktFlowReduceApprovalDto
 */
export interface FlowReduceApproval {
  /**
   * 流程实例 ID
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode?: string;

  /**
   * 加签记录 ID
   */
  flowAddSignId: string;

}

/**
 * 实例操作请求（挂起/恢复/终止/撤回/撤销审批）
 * 对应前端 FlowInstanceOperate
 * @description 对应后端 TaktFlowInstanceOperateDto
 */
export interface FlowInstanceOperate {
  /**
   * 流程实例 ID
   */
  flowInstanceId: string;

  /**
   * 原因说明
   */
  reason?: string;

}

/**
 * 待办/已办查询（分页与关键词见 TaktPagedQuery）
 * 对应前端 FlowTodoQuery
 * @description 对应后端 TaktFlowTodoQueryDto
 */
export interface FlowTodoQuery extends TaktPagedQuery {
  /**
   * 实例编码
   */
  instanceCode?: string;

  /**
   * 流程键
   */
  processKey?: string;

  /**
   * 流程名称
   */
  processName?: string;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 流程定义 ID
   */
  processDefinitionId?: string;

  /**
   * 当前节点/任务名称
   */
  taskName?: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 发起时间（范围起）
   */
  startTimeStart?: string;

  /**
   * 发起时间（范围止）
   */
  startTimeEnd?: string;

}

/**
 * 当前用户待办数量
 * @description 对应后端 TaktFlowTodoCountDto
 */
export interface FlowTodoCount {
  /**
   * 待办数量
   */
  todoCount: number;
}

/**
 * 待办列表项
 * 对应前端 FlowTodoItem
 * @description 对应后端 TaktFlowTodoItemDto
 */
export interface FlowTodoItem {
  /**
   * 流程实例 ID（对应 takt_workflow_instance 主键 Id）
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 当前节点名称（任务名称或实例当前活动名）
   */
  taskName?: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 发起时间
   */
  startTime?: string;

  /**
   * 任务 ID（适配 TaktFlowTask Id）
   */
  flowTaskId: string;

}

/**
 * 流转历史项（前端 history）
 * 对应前端 FlowHistoryItem
 * @description 对应后端 TaktFlowHistoryItemDto
 */
export interface FlowHistoryItem {
  /**
   * 源节点名称
   */
  fromNodeName: string;

  /**
   * 目标节点名称
   */
  toNodeName: string;

  /**
   * 操作人姓名
   */
  transitionUserName: string;

  /**
   * 操作时间
   */
  transitionTime: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

}

/**
 * 未处理加签项
 * 对应前端 FlowPendingAddApprover
 * @description 对应后端 TaktFlowPendingAddApproverDto
 */
export interface FlowPendingAddApprover {
  /**
   * 加签记录 ID（适配 TaktFlowAddSign Id）
   */
  flowAddSignId: string;

  /**
   * 加签人姓名
   */
  approverUserName: string;

}

/**
 * 流程实例详情（前端 FlowInstanceDetail）
 * 对应前端 FlowInstanceDetail
 * @description 对应后端 TaktFlowInstanceDetailDto
 */
export interface FlowInstanceDetail {
  /**
   * 流程实例 ID（对应 takt_workflow_instance 主键 Id）
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode: string;

  /**
   * 流程定义 ID
   */
  processDefinitionId: string;

  /**
   * 流程键
   */
  processKey: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

  /**
   * 当前节点 ID
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 表单 JSON
   */
  frmData?: string;

  /**
   * 流转历史
   */
  history: FlowHistoryItem[];

  /**
   * 未处理加签
   */
  pendingAddApprovers: FlowPendingAddApprover[];

  /**
   * 当前用户是否可审批（含减签）
   */
  canVerify: boolean;

}

/**
 * 我的/已办列表项（前端 FlowInstance 列表形态）
 * 对应前端 FlowInstanceListItem
 * @description 对应后端 TaktFlowInstanceListItemDto
 */
export interface FlowInstanceListItem {
  /**
   * 流程实例 ID（对应 takt_workflow_instance 主键 Id）
   */
  flowInstanceId: string;

  /**
   * 实例编码
   */
  instanceCode: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 发起时间
   */
  startTime?: string;

  /**
   * 表单 JSON
   */
  frmData?: string;

}

/**
 * 我的流程查询扩展
 * 对应前端 FlowMyInstanceQuery
 * @description 对应后端 TaktFlowMyInstanceQueryDto
 */
export interface FlowMyInstanceQuery extends FlowInstanceQuery {
  /**
   * 仅我发起
   */
  myStartedOnly: boolean;

}

