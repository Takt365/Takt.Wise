// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/components/business/TaktFlowAntDesigner/config
// 文件名称：taktFlowConditionStr.ts
// 创建时间：2026-04-07
// 创建人：Takt365(Cursor AI)
// 功能描述：条件分支/审批人/抄送节点展示文案（AntFlow conditionStr 语义）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FlowDesignTranslate } from './taktFlowDesignValidate'
import type { FlowTreeNode } from './taktFlowTree'

const P = 'workflow.designer.page.'

function directorLevelLabel(level: number, t: FlowDesignTranslate): string {
  if (level === 1) return t(`${P}directorleveldirect`)
  if (level === 2) return t(`${P}directorlevelsecond`)
  if (level === 3) return t(`${P}directorlevelthird`)
  return t(`${P}directorleveln`, { n: String(level) })
}

export function setApproverStr(node: FlowTreeNode, t: FlowDesignTranslate): string {
  if (!node || node.nodeType !== 4) return ''
  const list = node.nodeApproveList ?? []
  const names = list.map((x) => x.name).join('、')
  if (node.setType === 1) {
    if (list.length === 0) return ''
    if (list.length === 1) return list[0].name ?? ''
    return node.signType === 2
      ? `${t(`${P}peoplecount`, { count: String(list.length), names })}${t(`${P}orsign`)}`
      : names
  }
  if (node.setType === 2) {
    const level = directorLevelLabel(node.directorLevel ?? 1, t)
    return node.signType === 2 ? `${level}${t(`${P}orsign`)}` : level
  }
  if (node.setType === 3) return list.length ? t(`${P}assigntorole`, { names }) : ''
  if (node.setType === 4) return t(`${P}assigndept`)
  if (node.setType === 5) return t(`${P}selfselect`)
  if (node.setType === 6) {
    return t(`${P}layerapproval`, { level: String(node.directorLevel ?? 1) })
  }
  return names || ''
}

export function copyerStr(node: FlowTreeNode, t: FlowDesignTranslate): string {
  if (!node || node.nodeType !== 6) return ''
  const list = node.nodeApproveList ?? []
  if (list.length) return list.map((x) => x.name).join('、')
  if (node.ccFlag === 1) return t(`${P}selfselect`)
  return ''
}

export function conditionStr(nodeConfig: FlowTreeNode, index: number, t: FlowDesignTranslate): string {
  const nodes = nodeConfig.conditionNodes
  if (!nodes?.length || index < 0 || index >= nodes.length) return t(`${P}setcondition`)
  const item = nodes[index]
  const list = item.conditionList ?? []
  if (list.length === 0) {
    const isLast = index === nodes.length - 1
    return isLast ? t(`${P}defaultcondition`) : t(`${P}setcondition`)
  }
  const parts = list
    .filter((c) => c.zdy1 != null)
    .map((c) => {
      const op = (c.optType ?? '') === '1' ? '<' : (c.optType ?? '') === '2' ? '>' : (c.optType ?? '') === '4' ? '>=' : (c.optType ?? '') === '5' ? '<=' : '=='
      return `${c.showName ?? ''} ${op} ${c.zdy1 ?? ''}`
    })
  return parts.join(t(`${P}andconj`)) || t(`${P}setcondition`)
}
