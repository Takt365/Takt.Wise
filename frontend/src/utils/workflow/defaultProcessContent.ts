// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils/workflow
// 文件名称：defaultProcessContent.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程方案默认 ProcessContent（与设计器 toProcessContent / 引擎 ParseRoot 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { createStartNode } from '@/components/business/TaktFlowAntDesigner/config/taktFlowTree'

/**
 * 生成可保存的默认流程设计 JSON（含 flowTree 包装，引擎 ParseRoot 可解析）
 * @returns ProcessContent 字符串
 */
export function buildDefaultProcessContent(): string {
  const flowTree = createStartNode(null)
  return JSON.stringify({ nodes: [], edges: [], flowTree })
}

/** 新建方案时的默认 ProcessContent（固定引用，避免重复生成随机 nodeId） */
export const DEFAULT_PROCESS_CONTENT = buildDefaultProcessContent()
