// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFolderExplorerPath.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktFolderExplorer 面包屑路径拆分（纯函数，与 UI 无关）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktFolderExplorerCrumb,
  TaktFolderExplorerPathMode,
} from '@/types/components/folderExplorer.types'

/**
 * 本地模式「此电脑」根路径哨兵（空串在部分事件链路中易丢失，统一用此常量）
 */
export const TAKT_FOLDER_EXPLORER_LOCAL_ROOT = '__takt_local_root__'

/**
 * 是否为本地「此电脑」根
 * @param path 路径
 * @returns 是否根
 */
export function isFolderExplorerLocalRoot(path: string | null | undefined): boolean {
  const p = String(path ?? '').trim()
  return !p || p === TAKT_FOLDER_EXPLORER_LOCAL_ROOT
}

/**
 * 按路径模式生成面包屑
 * @param currentPath 当前路径
 * @param mode 路径模式
 * @param rootLabel 根标签（如「此电脑」）；local 模式首节
 * @returns 面包屑列表
 */
export function buildFolderExplorerCrumbs(
  currentPath: string | null | undefined,
  mode: TaktFolderExplorerPathMode,
  rootLabel?: string,
): TaktFolderExplorerCrumb[] {
  const path = String(currentPath || '').trim()
  const list: TaktFolderExplorerCrumb[] = []

  if (mode === 'local') {
    list.push({
      label: rootLabel || 'Computer',
      path: TAKT_FOLDER_EXPLORER_LOCAL_ROOT,
    })
    if (!path || path === TAKT_FOLDER_EXPLORER_LOCAL_ROOT) {
      return list
    }
    const normalized = path.replace(/\//g, '\\')
    const root = normalized.match(/^[A-Za-z]:\\?/)?.[0]
    if (root) {
      const rootPath = root.endsWith('\\') ? root : `${root}\\`
      list.push({ label: rootPath.replace(/\\$/, ''), path: rootPath })
      const rest = normalized.slice(rootPath.length).split('\\').filter(Boolean)
      let acc = rootPath.replace(/\\$/, '')
      for (const part of rest) {
        acc += `\\${part}`
        list.push({ label: part, path: acc })
      }
    } else {
      list.push({ label: path, path })
    }
    return list
  }

  if (mode === 'ftp') {
    list.push({ label: '/', path: '/' })
    if (!path || path === '/') {
      return list
    }
    const parts = path.split('/').filter(Boolean)
    let acc = ''
    for (const p of parts) {
      acc += `/${p}`
      list.push({ label: p, path: acc })
    }
    return list
  }

  if (mode === 'unc') {
    if (!path) {
      return list
    }
    if (path.startsWith('\\\\')) {
      const body = path.replace(/^\\\\/, '')
      const parts = body.split('\\').filter(Boolean)
      if (parts.length >= 2) {
        const shareRoot = `\\\\${parts[0]}\\${parts[1]}`
        list.push({ label: shareRoot, path: shareRoot })
        let acc = shareRoot
        for (let i = 2; i < parts.length; i += 1) {
          acc += `\\${parts[i]}`
          list.push({ label: parts[i], path: acc })
        }
      } else {
        list.push({ label: path, path })
      }
    } else {
      list.push({ label: path, path })
    }
    return list
  }

  // custom
  if (path) {
    list.push({ label: path, path })
  }
  return list
}

/**
 * 解析确认回填用的完整路径（优先选中项，相对名则拼到当前目录）
 * @param mode 路径模式
 * @param currentPath 当前浏览目录
 * @param selecmedPath 选中路径或名称
 * @returns 完整绝对路径；无法解析时返回空串
 */
export function resolveFolderExplorerConfirmPath(
  mode: TaktFolderExplorerPathMode,
  currentPath: string | null | undefined,
  selecmedPath: string | null | undefined,
): string {
  const selecmed = String(selecmedPath || '').trim()
  const current = String(currentPath || '').trim()
  const raw = selecmed || current
  if (!raw) {
    return mode === 'ftp' ? '/' : ''
  }
  // 禁止把单独的 \ 当成有效路径回填
  if (raw === '\\') {
    return ''
  }
  if (mode === 'local') {
    if (isFolderExplorerLocalRoot(raw)) {
      return ''
    }
    if (/^[A-Za-z]:[\\/]/.test(raw) || raw.startsWith('\\\\')) {
      return normalizeLocalAbsolutePath(raw)
    }
    if (current && /^[A-Za-z]:/.test(current)) {
      return normalizeLocalAbsolutePath(`${trimTrailingSeparators(current, '\\')}\\${raw}`)
    }
    // 相对名且无法拼出盘符 → 不回填残缺路径
    return ''
  }
  if (mode === 'unc') {
    if (raw.startsWith('\\\\')) {
      const normalized = raw.replace(/\//g, '\\').replace(/\\+$/, '') || raw
      return isUncAbsolutePath(normalized) ? normalized : ''
    }
    if (current.startsWith('\\\\')) {
      const joined = `${trimTrailingSeparators(current, '\\')}\\${raw}`.replace(/\//g, '\\')
      return isUncAbsolutePath(joined) ? joined : ''
    }
    return ''
  }
  if (mode === 'ftp') {
    if (raw.startsWith('/')) {
      return raw === '/' ? '/' : raw.replace(/\/+$/, '') || '/'
    }
    const base = !current || current === '/' ? '' : current.replace(/\/+$/, '')
    return `${base}/${raw}`.replace(/\/+/g, '/')
  }
  return raw
}

/**
 * 是否为本地（盘符）完整绝对路径
 * @param path 路径
 * @returns 是否完整
 */
export function isLocalAbsolutePath(path: string | null | undefined): boolean {
  const p = String(path || '').trim()
  if (!p || p === '\\' || p === '/') {
    return false
  }
  return /^[A-Za-z]:[\\/]/.test(p)
}

/**
 * 是否为 UNC 完整绝对路径（至少 \\server\share）
 * @param path 路径
 * @returns 是否完整
 */
export function isUncAbsolutePath(path: string | null | undefined): boolean {
  const p = String(path || '').trim().replace(/\//g, '\\')
  if (!p || p === '\\') {
    return false
  }
  return /^\\\\[^\\]+\\[^\\]+/.test(p)
}

/**
 * 是否为 FTP 远程完整路径
 * @param path 路径
 * @returns 是否完整
 */
export function isFtpAbsolutePath(path: string | null | undefined): boolean {
  const p = String(path || '').trim()
  return !!p && p.startsWith('/')
}

/**
 * 客户端本机路径是否为可回填的完整绝对路径（盘符/UNC/Unix）
 * @param path 路径
 * @returns 是否完整
 */
export function isClientAbsolutePath(path: string | null | undefined): boolean {
  const p = String(path || '').trim()
  if (!p || p === '\\' || p === '/') {
    return false
  }
  if (isLocalAbsolutePath(p) || isUncAbsolutePath(p)) {
    return true
  }
  // Unix 绝对路径（至少 /x）
  return /^\/[^/]/.test(p)
}

/**
 * 规范化客户端本机绝对路径（统一反斜杠，去掉多余末尾分隔；保留盘符根 D:\）
 * @param path 路径
 * @returns 规范化路径；非绝对则原样 trim
 */
export function normalizeClientAbsolutePath(path: string | null | undefined): string {
  const p = String(path || '').trim()
  if (!p) {
    return ''
  }
  if (isUncAbsolutePath(p)) {
    return p.replace(/\//g, '\\').replace(/\\+$/, '')
  }
  if (isLocalAbsolutePath(p)) {
    return normalizeLocalAbsolutePath(p)
  }
  if (/^\/[^/]/.test(p)) {
    return p.replace(/\/+$/, '') || p
  }
  return p
}

/**
 * 从浏览器/Electron 选中的 FileList 解析目录绝对路径
 * @param files 目录选择产生的文件列表
 * @returns 目录绝对路径；无法解析返回空串
 */
export function resolveClientDirectoryAbsolutePath(
  files: FileList | null | undefined,
): string {
  if (!files?.length) {
    return ''
  }
  const file = files[0] as File & { path?: string; webkitRelativePath?: string }
  // Electron / 部分宿主会暴露完整文件路径
  const filePath = String(file.path || '').trim()
  if (filePath) {
    const normalized = filePath.replace(/\//g, '\\')
    const idx = normalized.lastIndexOf('\\')
    const dir = idx > 0 ? normalized.slice(0, idx) : normalized
    return isClientAbsolutePath(dir) ? normalizeClientAbsolutePath(dir) : ''
  }
  return ''
}

/**
 * 从列表项读取完整路径（兼容 fullPath / FullPath）
 * @param item 目录项
 * @returns 完整路径
 */
export function readFolderExplorerItemFullPath(item: {
  fullPath?: string
  FullPath?: string
  name?: string
  Name?: string
} | null | undefined): string {
  if (!item) {
    return ''
  }
  const full = String(item.fullPath || item.FullPath || '').trim()
  if (full) {
    return full
  }
  return String(item.name || item.Name || '').trim()
}

/**
 * @param path 本地路径
 * @returns 规范化反斜杠绝对路径
 */
function normalizeLocalAbsolutePath(path: string): string {
  let p = path.replace(/\//g, '\\')
  if (!p || p === '\\') {
    return ''
  }
  if (/^[A-Za-z]:\\?$/.test(p)) {
    return `${p.replace(/\\+$/, '')}\\`
  }
  return p.replace(/\\+$/, '')
}

/**
 * @param path 路径
 * @param sep 分隔符
 * @returns 去掉末尾分隔
 */
function trimTrailingSeparators(path: string, sep: string): string {
  let p = path
  while (p.length > 1 && p.endsWith(sep)) {
    p = p.slice(0, -1)
  }
  return p
}
