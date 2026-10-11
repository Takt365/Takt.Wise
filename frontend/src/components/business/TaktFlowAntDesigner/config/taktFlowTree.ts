// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/components/business/TaktFlowAntDesigner/config
// 文件名称：taktFlowTree.ts
// 创建时间：2026-04-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程树类型定义与节点工厂（与 AntFlow nodeUtils 对齐，nodeType 1～7）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FlowDesignTranslate } from './taktFlowDesignValidate'

const P = 'workflow.designer.page.'

export function idGenerator(): string {
  let q = Date.now() - new Date('2024-05-01').getTime() + Math.ceil(Math.random() * 1000)
  const chars = '0123456789ABCDEFGHIGKLMNOPQRSTUVWXYZabcdefghigklmnopqrstuvwxyz'
  const radix = chars.length
  const res: string[] = []
  do {
    res.push(chars[q % radix])
    q = Math.floor(q / radix)
  } while (q)
  return res.join('').toUpperCase()
}

/** 审批人/抄送人项 */
export interface NodeApproveItem {
  targetId: string
  name: string
}

/** 条件项（与 AntFlow conditionList 一致） */
export interface ConditionItem {
  formId?: string
  showName?: string
  optType?: string
  zdy1?: string
  zdy2?: string
  columnType?: string
  showType?: string
  fixedDownBoxValue?: string
}

/** 流程树节点（AntFlow 风格） */
export interface FlowTreeNode {
  nodeId: string
  nodeName: string
  nodeDisplayName?: string
  nodeType: 1 | 2 | 3 | 4 | 6 | 7
  childNode?: FlowTreeNode | null
  /** 条件分支时的子条件节点 */
  conditionNodes?: FlowTreeNode[]
  /** 并行审核网关（nodeType 7）下的并行列，每列为 nodeType 4 */
  parallelNodes?: FlowTreeNode[]
  /** 网关 nodeType=2：true 动态条件；false 普通条件 */
  isDynamicCondition?: boolean
  /** 网关 nodeType=2：true 条件并行（子树含并行聚合审批人） */
  isParallel?: boolean
  error?: boolean
  /** 发起人/审核人/抄送：成员列表 */
  nodeApproveList?: NodeApproveItem[]
  /** 审核人：setType 1指定成员 2主管 3角色 4部门 5发起人自己 6层层审批 */
  setType?: number
  signType?: number
  directorLevel?: number
  noHeaderAction?: number
  isSignUp?: number
  /** 条件节点：优先级 */
  priorityLevel?: number
  conditionList?: ConditionItem[]
  isDefault?: number
  ccFlag?: number
}

/** 创建发起人节点 */
export function createStartNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const name = t ? t(`${P}promoter`) : '发起人'
  return {
    nodeId: idGenerator(),
    nodeName: name,
    nodeDisplayName: name,
    nodeType: 1,
    childNode: child ?? null,
    nodeApproveList: []
  }
}

/** 创建审核人节点 */
export function createApproveNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const name = t ? t(`${P}approver`) : '审核人'
  return {
    nodeId: idGenerator(),
    nodeName: name,
    nodeDisplayName: name,
    nodeType: 4,
    setType: 1,
    directorLevel: 1,
    signType: 1,
    noHeaderAction: 1,
    childNode: child ?? null,
    error: true,
    nodeApproveList: []
  }
}

/** 创建抄送人节点 */
export function createCopyNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const name = t ? t(`${P}copyer`) : '抄送人'
  return {
    nodeId: idGenerator(),
    nodeName: name,
    nodeDisplayName: name,
    nodeType: 6,
    childNode: child ?? null,
    error: true,
    ccFlag: 0,
    nodeApproveList: []
  }
}

/** 创建条件节点 */
export function createConditionNode(
  name: string,
  childNode: FlowTreeNode | null,
  priority: number,
  isDefault: number
): FlowTreeNode {
  return {
    nodeId: idGenerator(),
    nodeName: name,
    nodeDisplayName: name,
    nodeType: 3,
    priorityLevel: priority,
    conditionList: [],
    nodeApproveList: [],
    error: true,
    childNode: childNode ?? null,
    isDefault: isDefault ?? 0
  }
}

/** 并行分支上的审批人节点（与 AntFlow createParallelNode 对齐） */
export function createParallelBranchApproverNode(
  name: string,
  childNode: FlowTreeNode | null,
  priority: number,
  isDefault: number,
  t?: FlowDesignTranslate
): FlowTreeNode {
  const defaultName = t ? t(`${P}parallelapproverdefault`) : '并行审核人'
  const nodeName = name || defaultName
  return {
    nodeId: idGenerator(),
    nodeName: nodeName,
    nodeDisplayName: '',
    nodeType: 4,
    priorityLevel: priority,
    setType: 1,
    signType: 1,
    noHeaderAction: 1,
    directorLevel: 1,
    childNode: childNode ?? null,
    error: true,
    nodeApproveList: [],
    isDefault: isDefault ?? 0
  }
}

/** 创建并行审核网关（nodeType 7） */
export function createParallelWayNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const gw = t ? t(`${P}parallelgateway`) : '并行审核网关'
  const agg = t ? t(`${P}parallelaggregatenode`) : '并行聚合节点'
  const br1 = t ? t(`${P}parallelapprovernwithindex`, { n: '1' }) : '并行审核人1'
  const br2 = t ? t(`${P}parallelapprovernwithindex`, { n: '2' }) : '并行审核人2'
  return {
    nodeId: idGenerator(),
    nodeName: gw,
    nodeType: 7,
    childNode: createParallelBranchApproverNode(agg, null, 1, 0, t),
    error: false,
    parallelNodes: [
      createParallelBranchApproverNode(br1, child ?? null, 1, 0, t),
      createParallelBranchApproverNode(br2, null, 2, 0, t)
    ]
  }
}

/** 创建动态条件网关 */
export function createDynamicConditionWayNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const gw = t ? t(`${P}dynamicgateway`) : '动态网关'
  const c1 = t ? t(`${P}dynamicconditionnamewithindex`, { n: '1' }) : '动态条件1'
  const c2 = t ? t(`${P}dynamicconditionnamewithindex`, { n: '2' }) : '动态条件2'
  return {
    nodeId: idGenerator(),
    nodeName: gw,
    nodeType: 2,
    childNode: null,
    isDynamicCondition: true,
    isParallel: false,
    error: false,
    conditionNodes: [
      createConditionNode(c1, child ?? null, 1, 0),
      createConditionNode(c2, null, 2, 1)
    ]
  }
}

/** 创建条件并行网关 */
export function createParallelConditionWayNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const gw = t ? t(`${P}parallelconditiongateway`) : '条件并行网关'
  const agg = t ? t(`${P}parallelconditionaggregatenode`) : '条件并行聚合审批人'
  const c1 = t ? t(`${P}parallelconditionnamewithindex`, { n: '1' }) : '并行条件1'
  const c2 = t ? t(`${P}parallelconditionnamewithindex`, { n: '2' }) : '并行条件2'
  return {
    nodeId: idGenerator(),
    nodeName: gw,
    nodeType: 2,
    childNode: createParallelBranchApproverNode(agg, null, 1, 0, t),
    isDynamicCondition: false,
    isParallel: true,
    error: false,
    conditionNodes: [
      createConditionNode(c1, child ?? null, 1, 0),
      createConditionNode(c2, null, 2, 0)
    ]
  }
}

/** 创建网关（条件分支）节点 — 首条件挂原链 child，网关 childNode 为 null（与 AntFlow createGatewayNode 一致） */
export function createGatewayNode(child?: FlowTreeNode | null, t?: FlowDesignTranslate): FlowTreeNode {
  const gw = t ? t(`${P}gateway`) : '网关'
  const c1 = t ? t(`${P}conditionnamewithindex`, { n: '1' }) : '条件1'
  const c2 = t ? t(`${P}conditionnamewithindex`, { n: '2' }) : '条件2'
  return {
    nodeId: idGenerator(),
    nodeName: gw,
    nodeType: 2,
    childNode: null,
    isDynamicCondition: false,
    isParallel: false,
    error: true,
    conditionNodes: [
      createConditionNode(c1, child ?? null, 1, 0),
      createConditionNode(c2, null, 2, 1)
    ]
  }
}

/** 与 AntFlow addNode.vue createNodeMap 键 1~6 完全一致 */
export function buildCreateNodeMap(t: FlowDesignTranslate): Map<number, (child: FlowTreeNode | null) => FlowTreeNode> {
  return new Map([
    [1, (child) => createApproveNode(child ?? null, t)],
    [2, (child) => createCopyNode(child ?? null, t)],
    [3, (child) => createParallelWayNode(child ?? null, t)],
    [4, (child) => createGatewayNode(child ?? null, t)],
    [5, (child) => createDynamicConditionWayNode(child ?? null, t)],
    [6, (child) => createParallelConditionWayNode(child ?? null, t)]
  ])
}
