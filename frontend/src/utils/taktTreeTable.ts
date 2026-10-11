// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktTreeTable.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：右表树数据过滤/展开 key；展开后拍平供虚拟表格渲染（不把 children 交给 a-table）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { markRaw, toRaw } from 'vue'

/** 树表展开拍平最大深度（与 07-overflow-vue 树深上限一致） */
export const TAKT_TREE_TABLE_MAX_DEPTH = 10

/** 按 parentId 补齐子孙时的最大并发（防 /tree 打满全局限流 429） */
export const TAKT_TREE_FILL_CONCURRENCY = 3

/** 左树 loadData / 右表 loadChildren 默认并发上限 */
export const TAKT_TREE_LOAD_CONCURRENCY = 3

/** 树拉子节点并发闸门（模块内计数；仅编排 in-flight，不缓存业务数据） */
let taktTreeLoadInFlight = 0
const taktTreeLoadWaiters: Array<() => void> = []

/**
 * 限制树懒加载并发（多节点同时 expand 时排队，避免 429）
 * @param task 实际拉子节点逻辑
 * @param concurrency 最大并发，默认 TAKT_TREE_LOAD_CONCURRENCY
 * @returns {Promise<T>} task 结果
 */
export async function runWithTaktTreeLoadConcurrency<T>(
  task: () => Promise<T>,
  concurrency: number = TAKT_TREE_LOAD_CONCURRENCY,
): Promise<T> {
  const limit = Math.max(1, concurrency)
  if (taktTreeLoadInFlight >= limit) {
    await new Promise<void>((resolve) => {
      taktTreeLoadWaiters.push(resolve)
    })
  }
  taktTreeLoadInFlight += 1
  try {
    return await task()
  } finally {
    taktTreeLoadInFlight -= 1
    const next = taktTreeLoadWaiters.shift()
    if (next) next()
  }
}

/** 右表树节点（内存中保留 children；交给虚拟 a-table 前须拍平并去掉 children） */
export type TaktTreeTableNode = Record<string, unknown> & {
  children?: TaktTreeTableNode[]
  /** 懒加载：尚未拉子节点但应显示展开箭头 */
  _hasChildren?: boolean
}

/** 虚拟树表行：已去掉 children，仅含当前展开路径上的节点 */
export type TaktVirtualTreeTableRow = TaktTreeTableNode & {
  _treeDepth: number
  _hasChildren: boolean
  _treeExpanded: boolean
}

/**
 * 读取树表行 key，与页面 row-key（实体 Id）对齐
 * @param node 树节点
 * @param idField 实体主键字段名（如 menuId）
 * @returns {string} 行 key；无值时空串
 */
export function taktTreeTableNodeKey(
  node: Record<string, unknown> | null | undefined,
  idField: string,
): string {
  if (node == null) return ''
  const id = node[idField]
  if (id != null && String(id) !== '') return String(id)
  if (node.key != null && String(node.key) !== '') return String(node.key)
  if (node.id != null && String(node.id) !== '') return String(node.id)
  return ''
}

/**
 * 按谓词过滤树：命中节点或其子孙时保留该节点，并只保留命中分支的 children
 * @param nodes 树根列表
 * @param predicate 节点是否命中查询
 * @returns {T[]} 过滤后的树（结构仍带 children）
 */
export function filterTaktTreeTableNodes<T extends TaktTreeTableNode>(
  nodes: T[] | null | undefined,
  predicate: (node: T) => boolean,
): T[] {
  if (!nodes?.length) return []
  const result: T[] = []
  for (const node of nodes) {
    const rawChildren = node.children
    const filteredChildren = Array.isArray(rawChildren)
      ? filterTaktTreeTableNodes(rawChildren as T[], predicate)
      : []
    if (predicate(node) || filteredChildren.length > 0) {
      result.push({
        ...node,
        children: filteredChildren.length > 0 ? filteredChildren : undefined,
      })
    }
  }
  return result
}

/**
 * 判断节点是否可展开（已有子节点、或懒加载非叶子）
 * @param node 树节点
 * @param includeUnloaded 为 true 时把 isLeaf=false / _hasChildren 也算可展开（工具栏「全部展开」）
 * @returns {boolean} 是否可展开
 */
function taktTreeNodeIsExpandable(
  node: TaktTreeTableNode,
  includeUnloaded: boolean,
): boolean {
  const children = node.children
  if (Array.isArray(children) && children.length > 0) return true
  if (!includeUnloaded) return false
  if (node._hasChildren === true) return true
  const leaf = node.isLeaf
  if (leaf === false || leaf === 0 || leaf === '0') return true
  return false
}

/**
 * 收集可展开行 key（工具栏「全部展开/收缩」）
 * @param nodes 树根列表
 * @param getKey 行 key（须与 a-table row-key / a-tree key 一致）
 * @param options.includeUnloaded 懒加载：true=含尚未拉子的非叶子（工具栏展开与展开态下数据变化均应 true，才能一次点开逐层 loadData 拉齐）
 * @returns {string[]} 可展开行的 key
 */
export function collectTaktTreeTableExpandableKeys<T extends TaktTreeTableNode>(
  nodes: T[] | null | undefined,
  getKey: (node: T) => string,
  options?: { includeUnloaded?: boolean },
): string[] {
  if (!nodes?.length) return []
  const includeUnloaded = options?.includeUnloaded === true
  const keys: string[] = []
  const stack: T[] = nodes.slice()
  while (stack.length > 0) {
    const node = stack.pop()
    if (node == null) continue
    if (!taktTreeNodeIsExpandable(node, includeUnloaded)) continue
    const key = getKey(node)
    if (key) keys.push(key)
    const children = node.children
    if (!Array.isArray(children) || children.length === 0) continue
    for (let i = 0; i < children.length; i++) {
      const child = children[i] as T
      if (child != null) stack.push(child)
    }
  }
  return keys
}

/**
 * 比较展开 key 列表是否同集合（忽略顺序）
 * @param a 当前 keys
 * @param b 目标 keys
 * @returns {boolean} 是否相同
 */
export function taktTreeExpandedKeysEqual(
  a: ReadonlyArray<string | number> | null | undefined,
  b: ReadonlyArray<string | number> | null | undefined,
): boolean {
  const aa = a ?? []
  const bb = b ?? []
  if (aa.length !== bb.length) return false
  const set = new Set(aa.map(String))
  for (let i = 0; i < bb.length; i++) {
    if (!set.has(String(bb[i]))) return false
  }
  return true
}

/**
 * 收集「可展开但尚未加载子节点」的 parentId（供工具栏一次展开主动拉齐）
 * @param nodes 树根
 * @param getKey 节点主键
 * @returns {string[]} 待 load 的父级 Id
 */
export function collectTaktUnloadedExpandableKeys<T extends TaktTreeTableNode>(
  nodes: T[] | null | undefined,
  getKey: (node: T) => string,
): string[] {
  if (!nodes?.length) return []
  const keys: string[] = []
  const stack: T[] = nodes.slice()
  while (stack.length > 0) {
    const node = stack.pop()
    if (node == null) continue
    const children = node.children
    const hasLoaded = Array.isArray(children) && children.length > 0
    if (hasLoaded) {
      for (let i = 0; i < children.length; i++) {
        const child = children[i] as T
        if (child != null) stack.push(child)
      }
      continue
    }
    if (!taktTreeNodeIsExpandable(node, true)) continue
    const key = getKey(node)
    if (key) keys.push(key)
  }
  return keys
}

/**
 * 工具栏「全部展开」：写入 expandable keys，并主动按层拉取未加载子节点（并发受限），直到稳定或达深度上限。
 * @param options 取树/写 keys/拉子/是否仍展开
 * @returns {Promise<void>}
 */
export async function expandTaktLazyTreeFully(options: {
  getNodes: () => TaktTreeTableNode[]
  getKey: (node: TaktTreeTableNode) => string
  setExpandedKeys: (keys: string[]) => void
  loadChildren: (parentId: string) => Promise<void>
  isActive: () => boolean
  maxDepth?: number
}): Promise<void> {
  const maxDepth = Math.max(1, options.maxDepth ?? TAKT_TREE_TABLE_MAX_DEPTH)
  for (let round = 0; round < maxDepth; round++) {
    if (!options.isActive()) return
    const nodes = options.getNodes()
    const expandKeys = collectTaktTreeTableExpandableKeys(nodes, options.getKey, {
      includeUnloaded: true,
    })
    options.setExpandedKeys(expandKeys)
    const unloaded = collectTaktUnloadedExpandableKeys(nodes, options.getKey)
    if (unloaded.length === 0) return
    await Promise.all(
      unloaded.map((parentId) =>
        runWithTaktTreeLoadConcurrency(async () => {
          if (!options.isActive()) return
          await options.loadChildren(parentId)
        }),
      ),
    )
  }
}

/**
 * 当前页根节点是否应按树表拍平（已有 children，或显式 _hasChildren 待懒加载）
 * @param nodes 树根或平铺行
 * @returns {boolean} 任一根可展开
 */
export function hasTaktTreeTableChildren(
  nodes: TaktTreeTableNode[] | null | undefined,
): boolean {
  if (!nodes?.length) return false
  return nodes.some((row) => {
    if (row._hasChildren === true) return true
    return Array.isArray(row.children) && row.children.length > 0
  })
}

/**
 * 按 expandedKeys 迭代拍平已展开路径（不含未展开子孙）；去掉 children 以便 a-table virtual 只渲染可见行
 * @param nodes 当前页树根（可含 children）
 * @param expandedKeys 已展开行 key（与 row-key 一致）
 * @param getKey 行 key
 * @returns {TaktVirtualTreeTableRow[]} 展开路径上的平铺行
 */
export function flattenExpandedTaktTreeTableRows<T extends TaktTreeTableNode>(
  nodes: T[] | null | undefined,
  expandedKeys: ReadonlyArray<string | number> | null | undefined,
  getKey: (node: T) => string,
): TaktVirtualTreeTableRow[] {
  if (!nodes?.length) return []
  const expanded = new Set((expandedKeys ?? []).map((k) => String(k)))
  const rows: TaktVirtualTreeTableRow[] = []
  const stack: Array<{ node: T; depth: number }> = []
  for (let i = nodes.length - 1; i >= 0; i--) {
    const root = nodes[i]
    if (root != null) stack.push({ node: toRaw(root) as T, depth: 0 })
  }
  while (stack.length > 0) {
    const current = stack.pop()
    if (current == null) continue
    const { depth } = current
    const node = toRaw(current.node) as T
    const children = Array.isArray(node.children) ? (node.children as T[]) : []
    const hasChildren = children.length > 0 || node._hasChildren === true
    const key = getKey(node)
    const expandedNow = hasChildren && key !== '' && expanded.has(key)
    const row = markRaw(Object.assign({}, node, {
      children: undefined,
      _treeDepth: depth,
      _hasChildren: hasChildren,
      _treeExpanded: expandedNow,
    })) as TaktVirtualTreeTableRow
    rows.push(row)
    if (!expandedNow || depth >= TAKT_TREE_TABLE_MAX_DEPTH) continue
    for (let i = children.length - 1; i >= 0; i--) {
      const child = children[i]
      if (child != null) stack.push({ node: toRaw(child) as T, depth: depth + 1 })
    }
  }
  return markRaw(rows)
}

/**
 * 由平铺行按 parentId 组装树（仅用于总数低于懒加载阈值时的全量展示）
 * @param rows 平铺记录
 * @param idField 主键字段
 * @param parentIdField 父级字段，默认 parentId
 * @returns {T[]} 根节点列表（含子级 children）
 */
export function buildTaktTreeFromFlat<T extends TaktTreeTableNode>(
  rows: readonly T[],
  idField: string,
  parentIdField = 'parentId',
): T[] {
  if (!rows?.length) return []
  const clones: T[] = rows.map((row) => ({ ...row, children: [] as T[] }) as T)
  const byId = new Map<string, T>()
  for (const node of clones) {
    const id = taktTreeTableNodeKey(node, idField)
    if (id) byId.set(id, node)
  }
  const roots: T[] = []
  for (const node of clones) {
    const parentId = String(node[parentIdField] ?? '0')
    if (parentId === '0' || parentId === '' || !byId.has(parentId)) {
      roots.push(node)
      continue
    }
    const parent = byId.get(parentId)
    if (parent == null) {
      roots.push(node)
      continue
    }
    const children = Array.isArray(parent.children) ? parent.children as T[] : []
    children.push(node)
    parent.children = children
  }
  for (const node of clones) {
    const children = node.children
    if (!Array.isArray(children) || children.length === 0) {
      delete node.children
    }
  }
  return roots
}

/**
 * 在树中查找指定 key 的节点，返回以该节点为根的单元素数组（含已加载 children）
 * @param nodes 树根
 * @param key 行 key
 * @param idField 主键字段
 * @returns {T[]} 命中子树；未命中空数组
 */
export function findTaktTreeTableSubtree<T extends TaktTreeTableNode>(
  nodes: T[] | null | undefined,
  key: string | number,
  idField: string,
): T[] {
  if (!nodes?.length) return []
  const keyStr = String(key)
  const stack: T[] = nodes.slice()
  while (stack.length > 0) {
    const node = stack.pop()
    if (node == null) continue
    if (taktTreeTableNodeKey(node, idField) === keyStr) return [node]
    const children = node.children
    if (Array.isArray(children) && children.length > 0) {
      for (let i = 0; i < children.length; i++) {
        const child = children[i] as T
        if (child != null) stack.push(child)
      }
    }
  }
  return []
}

/**
 * 有限并发映射（同层兄弟节点拉取 /tree 时限流，避免 Promise.all 打满全局限流）
 * @param items 输入列表
 * @param concurrency 最大并发
 * @param mapper 异步映射
 * @returns {Promise<R[]>} 与输入同序结果
 */
async function mapWithConcurrency<T, R>(
  items: readonly T[],
  concurrency: number,
  mapper: (item: T, index: number) => Promise<R>,
): Promise<R[]> {
  if (!items.length) return []
  const limit = Math.max(1, concurrency)
  const results: R[] = new Array(items.length)
  let nextIndex = 0
  async function worker(): Promise<void> {
    while (nextIndex < items.length) {
      const i = nextIndex
      nextIndex += 1
      results[i] = await mapper(items[i] as T, i)
    }
  }
  const workerCount = Math.min(limit, items.length)
  await Promise.all(Array.from({ length: workerCount }, () => worker()))
  return results
}

/**
 * 为已有一层子节点补齐全部子孙（按 parentId 逐层拉取；同层并发上限 TAKT_TREE_FILL_CONCURRENCY）
 * 宽树（如行政区划）请优先用右表懒加载一层，勿对本函数喂整棵国/省。
 * @param nodes 当前层节点（可已带 children）
 * @param fetchChildren 按父级 Id 拉取直接子级
 * @param getKey 节点主键
 * @param depth 当前深度（根下第一层为 1）
 * @returns {Promise<T[]>} 已填子孙的节点
 */
export async function fillTaktTreeDescendants<T extends TaktTreeTableNode>(
  nodes: T[],
  fetchChildren: (parentId: string) => Promise<T[]>,
  getKey: (node: T) => string,
  depth = 1,
): Promise<T[]> {
  if (!nodes?.length || depth >= TAKT_TREE_TABLE_MAX_DEPTH) return nodes ?? []
  return mapWithConcurrency(nodes, TAKT_TREE_FILL_CONCURRENCY, async (node) => {
    const id = getKey(node)
    const existing = Array.isArray(node.children) ? (node.children as T[]) : []
    if (existing.length > 0) {
      const nested = await fillTaktTreeDescendants(existing, fetchChildren, getKey, depth + 1)
      return { ...node, children: nested, _hasChildren: true }
    }
    const maybeHas = node._hasChildren === true || node.isLeaf === false
    if (!id || !maybeHas) {
      return { ...node, children: undefined, _hasChildren: false }
    }
    const fetched = await fetchChildren(id)
    if (!fetched.length) {
      return { ...node, children: undefined, _hasChildren: false }
    }
    const nested = await fillTaktTreeDescendants(fetched, fetchChildren, getKey, depth + 1)
    return { ...node, children: nested, _hasChildren: true }
  })
}
