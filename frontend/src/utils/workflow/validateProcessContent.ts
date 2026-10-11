// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils/workflow
// 文件名称：validateProcessContent.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程方案 ProcessContent 保存前校验（JSON 可解析、flowTree 根节点、深度/分支上限）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FlowTreeNode } from '@/components/business/TaktFlowAntDesigner/config/taktFlowTree'

/** ProcessContent 校验结果 */
export interface ProcessContentValidationResult {
  /** 是否可保存 */
  ok: boolean
  /** 失败原因码（调试/日志用，页面文案走 i18n） */
  reason?: string
}

const MAX_TREE_DEPTH = 10
const MAX_BRANCH_COUNT = 20

/**
 * 读取流程树根 nodeType（兼容 camelCase / PascalCase / 字符串数字）
 * @param ft 节点或包装对象
 * @returns nodeType 或 undefined
 */
function flowTreeRootType(ft: unknown): number | undefined {
  if (ft == null || typeof ft !== 'object') return undefined
  const o = ft as Record<string, unknown>
  const v = o.nodeType ?? o.NodeType
  if (typeof v === 'number' && !Number.isNaN(v)) return v
  if (typeof v === 'string' && v.trim() !== '') {
    const n = Number(v)
    return Number.isNaN(n) ? undefined : n
  }
  return undefined
}

/**
 * 是否为 AntFlow 发起人根节点
 * @param ft 待判定对象
 */
function isFlowTreeRoot(ft: unknown): ft is FlowTreeNode {
  return flowTreeRootType(ft) === 1
}

/**
 * 宽松解析 ProcessContent JSON（与设计器 parseLenientProcessContentJson 一致）
 * @param val JSON 字符串
 * @returns 解析结果；失败返回 null
 */
function parseLenientProcessContentJson(val: string): unknown | null {
  const trimmed = val.trim()
  try {
    return JSON.parse(trimmed)
  } catch {
    const cut = trimmed.indexOf('}{')
    if (cut >= 0) {
      try {
        return JSON.parse(trimmed.slice(0, cut + 1))
      } catch {
        return null
      }
    }
    return null
  }
}

/**
 * 从 ProcessContent 提取 flowTree 根（支持直存树或 { flowTree } 包装）
 * @param parsed 已解析 JSON
 */
function extractFlowTree(parsed: unknown): FlowTreeNode | null {
  if (parsed == null || typeof parsed !== 'object') return null
  if (isFlowTreeRoot(parsed)) return parsed
  const obj = parsed as Record<string, unknown>
  const rawFt = obj.flowTree ?? obj.FlowTree
  if (rawFt != null && typeof rawFt === 'object' && isFlowTreeRoot(rawFt)) {
    return rawFt as FlowTreeNode
  }
  return null
}

/**
 * 计算流程树最大深度（根为第 1 层）
 * @param node 当前节点
 * @param depth 当前深度
 */
function measureTreeDepth(node: FlowTreeNode | null | undefined, depth = 1): number {
  if (!node) return Math.max(0, depth - 1)
  let maxDepth = depth
  const visit = (n: FlowTreeNode | null | undefined, d: number) => {
    if (!n) return
    maxDepth = Math.max(maxDepth, d)
    visit(n.childNode, d + 1)
    n.conditionNodes?.forEach((c) => {
      visit(c, d + 1)
      visit(c.childNode, d + 2)
    })
    n.parallelNodes?.forEach((p) => {
      visit(p, d + 1)
      visit(p.childNode, d + 2)
    })
  }
  visit(node.childNode, depth + 1)
  node.conditionNodes?.forEach((c) => {
    visit(c, depth + 1)
    visit(c.childNode, depth + 2)
  })
  node.parallelNodes?.forEach((p) => {
    visit(p, depth + 1)
    visit(p.childNode, depth + 2)
  })
  return maxDepth
}

/**
 * 校验网关/并行分支数量不超过上限
 * @param node 当前节点
 */
function validateBranchLimits(node: FlowTreeNode | null | undefined): boolean {
  if (!node) return true
  if (node.nodeType === 2 || node.nodeType === 7) {
    const branchCount = Math.max(node.conditionNodes?.length ?? 0, node.parallelNodes?.length ?? 0)
    if (branchCount > MAX_BRANCH_COUNT) return false
  }
  if (node.childNode && !validateBranchLimits(node.childNode)) return false
  for (const branch of node.conditionNodes ?? []) {
    if (!validateBranchLimits(branch)) return false
    if (branch.childNode && !validateBranchLimits(branch.childNode)) return false
  }
  for (const branch of node.parallelNodes ?? []) {
    if (!validateBranchLimits(branch)) return false
    if (branch.childNode && !validateBranchLimits(branch.childNode)) return false
  }
  return true
}

/**
 * 保存/回填前校验 ProcessContent 是否可解析且结构合法（与引擎 ParseRoot / 设计器约定对齐）
 * @param processContent 流程设计 JSON 字符串
 * @returns 校验结果；仅检查结构，不含审批人等业务项（见设计器 collectFlowDesignErrors）
 */
export function validateProcessContentForSave(processContent?: string | null): ProcessContentValidationResult {
  const raw = processContent?.trim()
  if (!raw) {
    return { ok: false, reason: 'empty' }
  }
  let parsed = parseLenientProcessContentJson(raw)
  if (parsed == null) {
    return { ok: false, reason: 'invalid_json' }
  }
  if (typeof parsed === 'string' && parsed.trim()) {
    parsed = parseLenientProcessContentJson(parsed)
    if (parsed == null) {
      return { ok: false, reason: 'invalid_json' }
    }
  }
  const tree = extractFlowTree(parsed)
  if (tree) {
    if (measureTreeDepth(tree) > MAX_TREE_DEPTH) {
      return { ok: false, reason: 'max_depth' }
    }
    if (!validateBranchLimits(tree)) {
      return { ok: false, reason: 'max_branches' }
    }
    return { ok: true }
  }
  return { ok: false, reason: 'no_flow_tree' }
}
