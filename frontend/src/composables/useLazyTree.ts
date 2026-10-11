// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useLazyTree.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：懒加载树编排（API 一层 DTO → Ant Design Tree 节点；供 admin-division / dept / account / menu 复用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * Ant Design Tree / TreeSelect 懒加载节点（key/title/isLeaf；非叶子 children 为 undefined 以显示展开图标）
 */
export interface TaktLazyTreeNode {
  key: string
  title: string
  isLeaf?: boolean
  children?: TaktLazyTreeNode[]
  [field: string]: unknown
}

/**
 * mapLazyTreeNodes 字段映射
 */
export interface MapLazyTreeNodesOptions<T> {
  /**
   * 节点 key（string）
   * @param row 源行
   */
  getKey: (row: T) => string
  /**
   * 节点标题
   * @param row 源行
   */
  getTitle: (row: T) => string
  /**
   * 是否叶子；true 不可展开
   * @param row 源行
   */
  isLeaf: (row: T) => boolean
}

/**
 * mergeLoadedChildren 字段名配置
 */
export interface MergeLazyTreeOptions {
  /** key 字段名，默认 key */
  keyField?: string
  /** children 字段名，默认 children */
  childrenField?: string
}

/**
 * 将 API 一层树 DTO 映射为 Ant Design 懒加载节点。
 * 非叶子：children 为 undefined（展开图标可见，待 loadData）；叶子：isLeaf=true。
 * @template T 源行类型
 * @param {ReadonlyArray<T>} rows 一层子节点
 * @param {MapLazyTreeNodesOptions<T>} options 字段映射
 * @returns {TaktLazyTreeNode[]} Ant 树节点
 */
export function mapLazyTreeNodes<T>(
  rows: ReadonlyArray<T> | null | undefined,
  options: MapLazyTreeNodesOptions<T>,
): TaktLazyTreeNode[] {
  if (!rows?.length) return []
  return rows.map((row) => {
    const key = options.getKey(row)
    const title = options.getTitle(row)
    const leaf = options.isLeaf(row)
    const base: TaktLazyTreeNode = {
      ...(row as Record<string, unknown>),
      key,
      title,
      isLeaf: leaf,
    }
    if (leaf) {
      return { ...base, isLeaf: true, _hasChildren: false }
    }
    // 非叶子：去掉 children（含 API 空数组），Ant Design 才显示展开并触发 loadData
    const { children: _omit, ...rest } = base
    return { ...rest, isLeaf: false, children: undefined, _hasChildren: true }
  })
}

/**
 * 将已加载的子节点合并进父节点（不可变更新整棵树）。
 * @template T 树节点类型（须含 key/children）
 * @param {T[]} treeData 当前树
 * @param {string | number} parentKey 父节点 key
 * @param {T[]} children 新子节点
 * @param {MergeLazyTreeOptions} [options] 字段名
 * @returns {T[]} 新树
 */
export function mergeLoadedChildren<T extends Record<string, unknown>>(
  treeData: readonly T[],
  parentKey: string | number,
  children: readonly T[],
  options?: MergeLazyTreeOptions,
): T[] {
  const keyField = options?.keyField ?? 'key'
  const childrenField = options?.childrenField ?? 'children'
  const parentKeyStr = String(parentKey)

  /**
   * 递归合并
   * @param nodes 当前层
   */
  function walk(nodes: readonly T[]): T[] {
    return nodes.map((node) => {
      const nodeKey = node[keyField]
      if (nodeKey != null && String(nodeKey) === parentKeyStr) {
        return {
          ...node,
          [childrenField]: [...children],
          isLeaf: children.length === 0,
          _hasChildren: children.length > 0,
        } as T
      }
      const existing = node[childrenField] as T[] | undefined
      if (existing?.length) {
        return {
          ...node,
          [childrenField]: walk(existing),
        } as T
      }
      return node
    })
  }

  return walk(treeData)
}

/**
 * 判断实体 IsLeaf（0/1 或 boolean）是否为叶子
 * @param {unknown} value isLeaf 字段
 * @returns {boolean} 是否叶子
 */
export function taktIsLeafFlag(value: unknown): boolean {
  if (value === true || value === 1 || value === '1') return true
  return false
}
