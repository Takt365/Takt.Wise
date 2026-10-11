// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/workflow
// 文件名称：flowEngine.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎运行时 API（路由与 TaktFlowEngineController 一一对应，修改须同步后端）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import type {
  FlowStart,
  FlowSubmitByTable,
  FlowStartableScheme,
  FlowCompleteTask,
  FlowTransfer,
  FlowAddApprovers,
  FlowReduceApproval,
  FlowInstanceOperate,
  FlowInstanceDetail,
  FlowTodoItem,
  FlowTodoQuery,
  FlowInstanceListItem,
  FlowTodoCount,
} from '@/types/workflow/flowEngine.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowEngine
 */
const FLOW_ENGINE_API_BASE = 'TaktFlowEngine';

// ========================================
// 待办（列表 + 详情 + 计数）
// ========================================

/**
 * 获取当前用户待办数量 — GetFlowInstanceTodoCountAsync
 * @returns {Promise<FlowTodoCount>} 待办数量
 */
export function getFlowEngineTodoCount(): Promise<FlowTodoCount> {
  return request<FlowTodoCount>({
    url: `${FLOW_ENGINE_API_BASE}/todo/count`,
    method: 'get',
  });
}

/**
 * 获取待办列表（分页）— GetFlowInstanceTodoListAsync
 * @param {FlowTodoQuery} queryDto 查询 DTO
 * @returns {Promise<TaktPagedResult<FlowTodoItem>>} 分页结果
 */
export function getFlowEngineTodoList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowTodoItem>> {
  return request<TaktPagedResult<FlowTodoItem>>({
    url: `${FLOW_ENGINE_API_BASE}/todo/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取待办流程实例运行时详情 — GetFlowInstanceTodoDetailByIdAsync
 * @param {string} id 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function getFlowEngineTodoById(id: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/todo/${id}`,
    method: 'get',
  });
}

// ========================================
// 我的流程（列表 + 详情）
// ========================================

/**
 * 获取我发起的流程列表 — GetFlowInstanceMyListAsync
 * @param {FlowTodoQuery} queryDto 查询 DTO
 * @returns {Promise<TaktPagedResult<FlowInstanceListItem>>} 分页结果
 */
export function getFlowEngineMyList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowInstanceListItem>> {
  return request<TaktPagedResult<FlowInstanceListItem>>({
    url: `${FLOW_ENGINE_API_BASE}/my/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取我发起的流程实例运行时详情 — GetFlowInstanceMyDetailByIdAsync
 * @param {string} id 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function getFlowEngineMyById(id: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/my/${id}`,
    method: 'get',
  });
}

// ========================================
// 已办（列表 + 详情）
// ========================================

/**
 * 获取已办流程列表 — GetFlowInstanceProcessedListAsync
 * @param {FlowTodoQuery} queryDto 查询 DTO
 * @returns {Promise<TaktPagedResult<FlowInstanceListItem>>} 分页结果
 */
export function getFlowEngineProcessedList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowInstanceListItem>> {
  return request<TaktPagedResult<FlowInstanceListItem>>({
    url: `${FLOW_ENGINE_API_BASE}/processed/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取已办流程实例运行时详情 — GetFlowInstanceProcessedDetailByIdAsync
 * @param {string} id 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function getFlowEngineProcessedById(id: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/processed/${id}`,
    method: 'get',
  });
}

// ========================================
// 实例管理（运行时详情）
// ========================================

/**
 * 获取流程实例运行时详情（实例管理页）— GetFlowInstanceDetailByIdAsync
 * @param {string} id 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function getFlowEngineDetailById(id: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/${id}`,
    method: 'get',
  });
}

// ========================================
// 发起与草稿
// ========================================

/**
 * 可发起流程方案列表 — GetStartableSchemeListAsync
 * @returns {Promise<FlowStartableScheme[]>} 已发布且未挂起的方案
 */
export function getFlowEngineStartableSchemes(): Promise<FlowStartableScheme[]> {
  return request<FlowStartableScheme[]>({
    url: `${FLOW_ENGINE_API_BASE}/startable-schemes`,
    method: 'get',
  });
}

/**
 * 审批业务表白名单 — GetApprovalFlowTableNamesAsync
 * @returns {Promise<string[]>} 物理表名列表
 */
export function getFlowEngineApprovalTables(): Promise<string[]> {
  return request<string[]>({
    url: `${FLOW_ENGINE_API_BASE}/approval-tables`,
    method: 'get',
  });
}

/**
 * 按业务表提交审批 — SubmitFlowApprovalByTableAsync
 * @param {FlowSubmitByTable} dto 表名与业务主键
 * @returns {Promise<FlowInstanceDetail>} 流程实例详情
 */
export function submitFlowApprovalByTable(dto: FlowSubmitByTable): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/submit-by-table`,
    method: 'post',
    data: dto,
  });
}

/**
 * 发起流程 — StartFlowInstanceAsync
 * @param {FlowStart} dto 发起参数
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function startFlowEngineInstance(dto: FlowStart): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/start`,
    method: 'post',
    data: dto,
  });
}

/**
 * 保存草稿 — CreateFlowInstanceDraftAsync
 * @param {FlowStart} dto 发起参数
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function createFlowEngineDraft(dto: FlowStart): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/draft`,
    method: 'post',
    data: dto,
  });
}

/**
 * 从草稿启动 — StartFlowInstanceFromDraftAsync
 * @param {string} instanceId 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function startFlowEngineFromDraft(instanceId: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/${instanceId}/start-from-draft`,
    method: 'post',
  });
}

// ========================================
// 审批操作
// ========================================

/**
 * 办结任务（通过/驳回）— CompleteFlowInstanceTaskAsync
 * @param {FlowCompleteTask} dto 办结参数
 * @returns {Promise<void>}
 */
export function completeFlowEngineTask(dto: FlowCompleteTask): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/complete`,
    method: 'post',
    data: dto,
  });
}

/**
 * 撤回流程 — RevokeFlowInstanceAsync
 * @param {string} instanceCode 实例编码
 * @returns {Promise<void>}
 */
export function withdrawFlowEngineInstance(instanceCode: string): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/withdraw`,
    method: 'post',
    params: { instanceCode },
  });
}

/**
 * 转办 — TransferFlowInstanceAsync
 * @param {FlowTransfer} dto 转办参数
 * @returns {Promise<void>}
 */
export function transferFlowEngineTask(dto: FlowTransfer): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/transfer`,
    method: 'post',
    data: dto,
  });
}

/**
 * 加签 — AddFlowInstanceApproversAsync
 * @param {FlowAddApprovers} dto 加签参数
 * @returns {Promise<void>}
 */
export function addFlowEngineApprovers(dto: FlowAddApprovers): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/add-approvers`,
    method: 'post',
    data: dto,
  });
}

/**
 * 减签 — ReduceFlowInstanceApprovalAsync
 * @param {FlowReduceApproval} dto 减签参数
 * @returns {Promise<void>}
 */
export function reduceFlowEngineSign(dto: FlowReduceApproval): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/reduce-sign`,
    method: 'post',
    data: dto,
  });
}

/**
 * 挂起流程 — SuspendFlowInstanceAsync
 * @param {FlowInstanceOperate} dto 参数
 * @returns {Promise<void>}
 */
export function suspendFlowEngineInstance(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/suspend`,
    method: 'post',
    data: dto,
  });
}

/**
 * 恢复流程 — ResumeFlowInstanceAsync
 * @param {FlowInstanceOperate} dto 参数
 * @returns {Promise<void>}
 */
export function resumeFlowEngineInstance(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/resume`,
    method: 'post',
    data: dto,
  });
}

/**
 * 终止流程 — TerminateFlowInstanceAsync
 * @param {FlowInstanceOperate} dto 参数
 * @returns {Promise<void>}
 */
export function terminateFlowEngineInstance(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/terminate`,
    method: 'post',
    data: dto,
  });
}

/**
 * 撤销当前节点审批 — UndoFlowInstanceVerificationAsync
 * @param {FlowInstanceOperate} dto 参数
 * @returns {Promise<void>}
 */
export function undoFlowEngineVerification(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/undo-verification`,
    method: 'post',
    data: dto,
  });
}
