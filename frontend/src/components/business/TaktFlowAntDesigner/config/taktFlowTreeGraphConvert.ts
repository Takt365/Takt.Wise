// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/components/business/TaktFlowAntDesigner/config
// 文件名称：taktFlowTreeGraphConvert.ts
// 创建时间：2026-04-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程树与后端 processContent 的 nodes/edges 互转
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FlowTreeNode } from './taktFlowTree'

export interface GraphNode {
  id: string
  name?: string
  type: string
  assigneeType?: string
  roles?: string[]
  departments?: string[]
  assigneeUserIds?: string[]
  copyUserIds?: string[]
}

export interface GraphEdge {
  from: string
  to: string
  condition?: string
  label?: string
  priority?: number
}

/** 取分支上第一个“实体”节点 id（start/userTask/copy）；若是网关则取第一个条件的第一个节点；并行网关取第一列起点 */
function firstRealNodeId(node: FlowTreeNode | null | undefined): string | null {
  if (!node) return null
  if (node.nodeType === 1 || node.nodeType === 4 || node.nodeType === 6) return node.nodeId
  if (node.nodeType === 7 && node.parallelNodes?.length) {
    const first = node.parallelNodes[0]
    return first ? first.nodeId : null
  }
  if (node.nodeType === 2 && node.conditionNodes?.length) {
    const first = node.conditionNodes[0]
    return firstRealNodeId(first?.childNode ?? null)
  }
  if (node.nodeType === 3) return firstRealNodeId(node.childNode ?? null)
  return null
}

/**
 * 取分支上最后一个实体节点 id
 * @param node 流程树节点
 * @returns 节点 id
 */
function lastRealNodeId(node: FlowTreeNode | null | undefined): string | null {
  if (!node) return null
  if (node.nodeType === 1 || node.nodeType === 4 || node.nodeType === 6) {
    if (!node.childNode) return node.nodeId
    return lastRealNodeId(node.childNode)
  }
  if (node.nodeType === 7 && node.parallelNodes?.length) {
    const lastP = node.parallelNodes[node.parallelNodes.length - 1]
    return lastRealNodeId(lastP ?? null)
  }
  if (node.nodeType === 2 && node.conditionNodes?.length) {
    const lastCond = node.conditionNodes[node.conditionNodes.length - 1]
    return lastRealNodeId(lastCond?.childNode ?? null)
  }
  if (node.nodeType === 3) return lastRealNodeId(node.childNode ?? null)
  return null
}

/**
 * 条件列表 → 简单表达式字符串
 * @param conditionList 条件项
 * @returns 表达式
 */
function buildConditionExpr(conditionList: { showName?: string; optType?: string; zdy1?: string; zdy2?: string }[]): string {
  const parts = conditionList
    .filter((c) => c.zdy1 != null)
    .map((c) => {
      const v = c.zdy1 ?? ''
      const op = (c.optType ?? '') === '1' ? '<' : (c.optType ?? '') === '2' ? '>' : (c.optType ?? '') === '4' ? '>=' : (c.optType ?? '') === '5' ? '<=' : '=='
      const key = (c.showName ?? '').replace(/\s/g, '_')
      return key ? `${key} ${op} ${v}` : ''
    })
    .filter(Boolean)
  return parts.length ? parts.join(' && ') : ''
}

/**
 * 解析 condition 字符串为简单 conditionList（仅支持单条件时反向）
 * @param condition 条件表达式
 * @returns 条件项列表
 */
function parseConditionExpr(condition?: string): { showName?: string; optType?: string; zdy1?: string }[] {
  if (!condition?.trim()) return []
  const m = condition.match(/^(\S+)\s*(<|>|<=|>=|==)\s*(.+)$/)
  if (!m) return []
  const [, key, op, val] = m
  const optMap: Record<string, string> = { '<': '1', '>': '2', '>=': '4', '<=': '5', '==': '3' }
  return [{ showName: key?.replace(/_/g, ' '), optType: optMap[op ?? ''] ?? '3', zdy1: val?.trim() }]
}

/**
 * 树 → nodes + edges
 * @param root 流程树根
 * @returns nodes/edges
 */
export function treeToGraph(root: FlowTreeNode | null): { nodes: GraphNode[]; edges: GraphEdge[] } {
  const nodes: GraphNode[] = []
  const edges: GraphEdge[] = []
  const seen = new Set<string>()

  /**
   * 写入图节点（去重）
   * @param node 树节点
   * @param type 图节点类型
   * @param extra 附加字段
   */
  function addNode(node: FlowTreeNode, type: string, extra: Partial<GraphNode> = {}) {
    if (seen.has(node.nodeId)) return
    seen.add(node.nodeId)
    const n: GraphNode = {
      id: node.nodeId,
      name: node.nodeName,
      type,
      ...extra,
    }
    nodes.push(n)
  }

  /**
   * 深度优先遍历树并生成边
   * @param node 当前节点
   * @param fromId 入边来源
   */
  function walk(node: FlowTreeNode | null | undefined, fromId: string | null) {
    if (!node) return
    if (node.nodeType === 1) {
      addNode(node, 'start')
      if (fromId) edges.push({ from: fromId, to: node.nodeId })
      if (node.childNode) walk(node.childNode, node.nodeId)
      return
    }
    if (node.nodeType === 4) {
      const assigneeType =
        node.setType === 1
          ? 'assignee'
          : node.setType === 2
            ? 'director'
            : node.setType === 3
              ? 'role'
              : node.setType === 4
                ? 'dept'
                : node.setType === 5
                  ? 'selfSelect'
                  : 'director'
      const roles = node.setType === 3 ? (node.nodeApproveList ?? []).map((x) => x.targetId) : undefined
      const assigneeUserIds = node.setType === 1 ? (node.nodeApproveList ?? []).map((x) => x.targetId) : undefined
      addNode(node, 'userTask', { assigneeType, roles, assigneeUserIds })
      if (fromId) edges.push({ from: fromId, to: node.nodeId })
      if (node.childNode) walk(node.childNode, node.nodeId)
      return
    }
    if (node.nodeType === 6) {
      const copyUserIds = (node.nodeApproveList ?? []).map((x) => x.targetId)
      addNode(node, 'copy', { copyUserIds })
      if (fromId) edges.push({ from: fromId, to: node.nodeId })
      if (node.childNode) walk(node.childNode, node.nodeId)
      return
    }
    if (node.nodeType === 7) {
      const mergeRoot = node.childNode
      if (node.parallelNodes?.length) {
        for (const p of node.parallelNodes) {
          const colStart = p?.nodeId
          if (fromId && colStart) edges.push({ from: fromId, to: colStart })
          if (p) walk(p, null)
        }
      }
      if (mergeRoot) {
        const mergeTarget = firstRealNodeId(mergeRoot)
        if (mergeTarget && node.parallelNodes?.length) {
          for (const p of node.parallelNodes) {
            const branchLast = lastRealNodeId(p ?? null)
            if (branchLast) edges.push({ from: branchLast, to: mergeTarget })
          }
        }
        walk(mergeRoot, null)
      }
      return
    }
    if (node.nodeType === 2) {
      if (!node.conditionNodes?.length) {
        if (fromId && node.childNode) {
          const toId = firstRealNodeId(node.childNode)
          if (toId) edges.push({ from: fromId, to: toId })
        }
        if (node.childNode) walk(node.childNode, fromId)
        return
      }
      for (let i = 0; i < node.conditionNodes.length; i++) {
        const cond = node.conditionNodes[i]
        const toId = firstRealNodeId(cond?.childNode ?? null)
        if (fromId && toId) {
          edges.push({
            from: fromId,
            to: toId,
            condition: cond?.conditionList?.length ? buildConditionExpr(cond.conditionList) : undefined,
            label: cond?.nodeName,
            priority: cond?.priorityLevel ?? i + 1,
          })
        }
        if (cond?.childNode) walk(cond.childNode, null)
      }
      if (node.childNode) {
        const mergeTarget = firstRealNodeId(node.childNode)
        if (mergeTarget) {
          for (const cond of node.conditionNodes) {
            const branchLast = lastRealNodeId(cond?.childNode ?? null)
            if (branchLast) edges.push({ from: branchLast, to: mergeTarget })
          }
        }
        walk(node.childNode, null)
      }
      return
    }
    if (node.nodeType === 3) {
      if (fromId && node.childNode) {
        const toId = firstRealNodeId(node.childNode)
        if (toId) {
          edges.push({
            from: fromId,
            to: toId,
            condition: node.conditionList?.length ? buildConditionExpr(node.conditionList) : undefined,
            label: node.nodeName,
            priority: node.priorityLevel,
          })
        }
      }
      if (node.childNode) walk(node.childNode, fromId)
    }
  }

  if (root) walk(root, null)
  return { nodes, edges }
}

/**
 * nodes + edges → 树（根为发起人节点）
 * @param nodes 图节点
 * @param edges 图边
 * @returns 流程树根；无法还原时 null
 */
export function graphToTree(nodes: GraphNode[], edges: GraphEdge[]): FlowTreeNode | null {
  if (!nodes.length) return null
  const nodeMap = new Map<string, GraphNode>()
  nodes.forEach((n) => nodeMap.set(n.id, n))
  const outEdges = new Map<string, GraphEdge[]>()
  edges.forEach((e) => {
    const list = outEdges.get(e.from) ?? []
    list.push(e)
    outEdges.set(e.from, list)
  })

  const start = nodes.find((n) => n.type === 'start')
  if (!start) return null

  const idGen = () => Math.random().toString(36).slice(2, 12).toUpperCase()

  /**
   * 图节点 → 树节点骨架
   * @param n 图节点
   * @returns 树节点
   */
  function toTreeNode(n: GraphNode): FlowTreeNode {
    if (n.type === 'start') {
      return { nodeId: n.id, nodeName: n.name ?? '发起人', nodeType: 1, nodeApproveList: [], childNode: null }
    }
    if (n.type === 'userTask') {
      const setType =
        n.assigneeType === 'assignee'
          ? 1
          : n.assigneeType === 'director'
            ? 2
            : n.assigneeType === 'role'
              ? 3
              : n.assigneeType === 'dept'
                ? 4
                : n.assigneeType === 'selfSelect'
                  ? 5
                  : 1
      return {
        nodeId: n.id,
        nodeName: n.name ?? '审核人',
        nodeType: 4,
        setType,
        signType: 1,
        directorLevel: 1,
        nodeApproveList: (n.assigneeUserIds ?? n.roles ?? []).map((id) => ({ targetId: id, name: id })),
        childNode: null,
      }
    }
    if (n.type === 'copy') {
      return {
        nodeId: n.id,
        nodeName: n.name ?? '抄送人',
        nodeType: 6,
        nodeApproveList: (n.copyUserIds ?? []).map((id) => ({ targetId: id, name: id })),
        childNode: null,
      }
    }
    return { nodeId: n.id, nodeName: n.name ?? '', nodeType: 4, nodeApproveList: [], childNode: null }
  }

  /**
   * 从指定图节点重建子树
   * @param fromNodeId 起点
   * @param visited 防环
   * @returns 树节点
   */
  function buildTree(fromNodeId: string, visited: Set<string>): FlowTreeNode | null {
    if (visited.has(fromNodeId)) return null
    const from = nodeMap.get(fromNodeId)
    if (!from) return null
    visited.add(fromNodeId)
    const list = (outEdges.get(fromNodeId) ?? []).sort((a, b) => (a.priority ?? 0) - (b.priority ?? 0))

    if (list.length === 0) {
      return toTreeNode(from)
    }
    if (list.length === 1) {
      const child = buildTree(list[0].to, new Set(visited))
      const node = toTreeNode(from)
      node.childNode = child ?? null
      return node
    }
    // 多出边：当前节点后接网关，网关的 conditionNodes 为各分支
    const gatewayId = idGen()
    const conditionNodes: FlowTreeNode[] = list.map((e, i) => ({
      nodeId: idGen(),
      nodeName: e.label ?? `条件${i + 1}`,
      nodeType: 3 as const,
      priorityLevel: e.priority ?? i + 1,
      conditionList: parseConditionExpr(e.condition),
      nodeApproveList: [],
      childNode: buildTree(e.to, new Set(visited)) ?? null,
    }))
    const currentNode = toTreeNode(from)
    currentNode.childNode = {
      nodeId: gatewayId,
      nodeName: '网关',
      nodeType: 2,
      childNode: null,
      conditionNodes,
    }
    return currentNode
  }

  const tree = buildTree(start.id, new Set<string>())
  return tree && tree.nodeType === 1 ? tree : null
}
