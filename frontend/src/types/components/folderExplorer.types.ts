// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/components
// 文件名称：folderExplorer.types.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：通用资源管理器式目录浏览器类型（TaktFolderExplorer）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 目录项
 */
export interface TaktFolderExplorerItem {
  /** 显示名 */
  name: string
  /** 完整路径 */
  fullPath: string
  /** 是否目录 */
  isDirectory: boolean
  /**
   * 最后修改时间（ISO 字符串，可空）
   */
  modifiedTime?: string | null
}

/**
 * 目录浏览结果（与后端 BrowseResult 对齐）
 */
export interface TaktFolderExplorerBrowseResult {
  /** 当前路径；空表示根（如「此电脑」） */
  currentPath: string
  /**
   * 上级路径；
   * null=已在根不可再上；
   * 空串=本地盘符根的上级（回到此电脑）
   */
  parentPath?: string | null
  /** 子项 */
  items: TaktFolderExplorerItem[]
}

/**
 * 路径模式：决定面包屑拆分与左树根
 * - local：Windows 盘符路径
 * - unc：\\\\server\\share
 * - ftp：POSIX /
 * - custom：仅整段当前路径一节面包屑
 */
export type TaktFolderExplorerPathMode = 'local' | 'unc' | 'ftp' | 'custom'

/**
 * 面包屑节点
 */
export interface TaktFolderExplorerCrumb {
  label: string
  path: string
}

/**
 * 左导航树节点（Ant Tree）
 */
export interface TaktFolderExplorerTreeNode {
  key: string
  title: string
  isLeaf?: boolean
  children?: TaktFolderExplorerTreeNode[]
}
