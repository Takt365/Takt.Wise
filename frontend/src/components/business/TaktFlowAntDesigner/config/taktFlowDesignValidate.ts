// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/components/business/TaktFlowAntDesigner/config
// 文件名称：taktFlowDesignValidate.ts
// 创建时间：2026-04-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程树设计校验（发布前错误列表，与 AntFlow Process.setIsTried 类校验对齐的简化版）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FlowTreeNode } from './taktFlowTree'

export type FlowDesignTranslate = (key: string, values?: Record<string, string>) => string

export interface FlowDesignErrorRow {
  nodeName: string
  message: string
}

function nodeLabel(node: FlowTreeNode): string {
  return String(node.nodeDisplayName || node.nodeName || node.nodeId)
}

function visit(node: FlowTreeNode | null | undefined, t: FlowDesignTranslate, out: FlowDesignErrorRow[]): void {
  if (!node) return

  const name = nodeLabel(node)

  if (node.nodeType === 4) {
    const st = node.setType ?? 1
    if (st === 1 && !(node.nodeApproveList?.length)) {
      out.push({ nodeName: name, message: t('workflow.designer.page.validateapproverneedusers', { name }) })
    }
    if (st === 3 && !(node.nodeApproveList?.length)) {
      out.push({ nodeName: name, message: t('workflow.designer.page.validateapproverneedroles', { name }) })
    }
    if (st === 4 && !(node.nodeApproveList?.length)) {
      out.push({ nodeName: name, message: t('workflow.designer.page.validateapproverneeddepts', { name }) })
    }
  }

  if (node.nodeType === 6 && !(node.nodeApproveList?.length)) {
    out.push({ nodeName: name, message: t('workflow.designer.page.validatecopyneedusers', { name }) })
  }

  if (node.nodeType === 3 && node.isDefault !== 1 && !(node.conditionList?.length)) {
    out.push({ nodeName: name, message: t('workflow.designer.page.validateconditionneedexpr', { name }) })
  }

  visit(node.childNode, t, out)
  node.conditionNodes?.forEach((c) => visit(c, t, out))
  node.parallelNodes?.forEach((p) => visit(p, t, out))
}

/**
 * 收集流程设计错误（用于错误弹窗）。根节点为 null 时不报错，由上层提示「未初始化」。
 */
export function collectFlowDesignErrors(root: FlowTreeNode | null, t: FlowDesignTranslate): FlowDesignErrorRow[] {
  const out: FlowDesignErrorRow[] = []
  if (!root) return out
  visit(root, t, out)
  return out
}
