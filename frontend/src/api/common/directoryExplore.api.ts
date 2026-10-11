// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/common
// 文件名称：directoryExplore.api.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：通用目录浏览 API（服务器目录 / 文件服务器 / FTP）；当前对接 TaktDatabaseBackups browse/mkdir
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  TaktDirectoryExploreBrowseRequest,
  TaktDirectoryExploreBrowseResult,
  TaktDirectoryExploreItem,
  TaktDirectoryExploreMkdirRequest,
} from '@/types/components/directoryExplore.types'

/** 当前后端落点（目录浏览挂在备份控制器；通用组件不直接依赖业务模块） */
const DIRECTORY_EXPLORE_API_BASE = 'TaktDatabaseBackups'

/**
 * @param it 后端项
 * @returns 前端项
 */
function mapItem(it: Record<string, unknown>): TaktDirectoryExploreItem {
  const modified = it.modifiedTime ?? it.ModifiedTime
  const full = String(it.fullPath || it.FullPath || '').trim()
  const name = String(it.name || it.Name || '').trim()
  return {
    name: name || full,
    fullPath: full || name,
    isDirectory: it.isDirectory !== false && it.IsDirectory !== false,
    modifiedTime: modified == null ? null : String(modified),
  }
}

/**
 * 解包 mkdir 响应中的完整路径
 * @param res 接口 data
 * @param fallback 请求时路径
 * @returns 完整路径
 */
function unwrapMkdirPath(res: { path?: string; Path?: string } | string | null | undefined, fallback: string): string {
  if (typeof res === 'string' && res.trim()) {
    return res.trim()
  }
  if (res && typeof res === 'object') {
    const p = String(res.path ?? res.Path ?? '').trim()
    if (p) {
      return p
    }
  }
  return String(fallback || '').trim()
}

/**
 * @param res 原始结果
 * @returns 规范化结果
 */
function mapBrowseResult(res: {
  currentPath?: string
  CurrentPath?: string
  parentPath?: string | null
  ParentPath?: string | null
  items?: Record<string, unknown>[]
  Items?: Record<string, unknown>[]
}): TaktDirectoryExploreBrowseResult {
  const items = (res.items ?? res.Items ?? []).map((it) => mapItem(it))
  return {
    currentPath: String(res.currentPath ?? res.CurrentPath ?? ''),
    parentPath: res.parentPath === undefined && res.ParentPath === undefined
      ? null
      : (res.parentPath ?? res.ParentPath ?? null),
    items,
  }
}

/**
 * 浏览服务器目录（API 宿主本机磁盘）
 * @param path 当前路径；空=盘符列表
 * @returns 浏览结果
 */
export function browseServerDirectory(path?: string): Promise<TaktDirectoryExploreBrowseResult> {
  return request({
    url: `${DIRECTORY_EXPLORE_API_BASE}/browse/local`,
    method: 'post',
    data: { currentPath: path || undefined },
  }).then((res) => mapBrowseResult(res as Parameters<typeof mapBrowseResult>[0]))
}

/**
 * 在服务器上创建目录
 * @param path 完整路径
 * @returns 创建后路径
 */
export function createServerDirectory(path: string): Promise<string> {
  return request<{ path?: string; Path?: string } | string>({
    url: `${DIRECTORY_EXPLORE_API_BASE}/mkdir/local`,
    method: 'post',
    data: { path },
  }).then((res) => unwrapMkdirPath(res, path))
}

/**
 * 浏览文件服务器目录（UNC）
 * @param path UNC 路径
 * @param auth 凭据
 * @returns 浏览结果
 */
export function browseFileServerDirectory(
  path: string,
  auth?: { userName?: string; password?: string; configId?: string },
): Promise<TaktDirectoryExploreBrowseResult> {
  return request({
    url: `${DIRECTORY_EXPLORE_API_BASE}/browse/network`,
    method: 'post',
    data: {
      path,
      userName: auth?.userName,
      password: auth?.password,
      databaseBackupId: auth?.configId,
    },
  }).then((res) => mapBrowseResult(res as Parameters<typeof mapBrowseResult>[0]))
}

/**
 * 在文件服务器上创建目录
 * @param path UNC 路径
 * @param auth 凭据
 * @returns 创建后路径
 */
export function createFileServerDirectory(
  path: string,
  auth?: { userName?: string; password?: string; configId?: string },
): Promise<string> {
  return request<{ path?: string; Path?: string } | string>({
    url: `${DIRECTORY_EXPLORE_API_BASE}/mkdir/network`,
    method: 'post',
    data: {
      path,
      userName: auth?.userName,
      password: auth?.password,
      databaseBackupId: auth?.configId,
    },
  }).then((res) => unwrapMkdirPath(res, path))
}

/**
 * 浏览 FTP 服务器目录
 * @param path 远程路径
 * @param auth FTP 连接
 * @returns 浏览结果
 */
export function browseFtpDirectory(
  path: string | undefined,
  auth: { host: string; port?: number; userName: string; password?: string; configId?: string },
): Promise<TaktDirectoryExploreBrowseResult> {
  return request({
    url: `${DIRECTORY_EXPLORE_API_BASE}/browse/ftp`,
    method: 'post',
    data: {
      host: auth.host,
      port: auth.port,
      path: path || '/',
      userName: auth.userName,
      password: auth.password,
      databaseBackupId: auth.configId,
    },
  }).then((res) => mapBrowseResult(res as Parameters<typeof mapBrowseResult>[0]))
}

/**
 * 在 FTP 上创建目录
 * @param path 远程路径
 * @param auth FTP 连接
 * @returns 创建后路径
 */
export function createFtpDirectory(
  path: string,
  auth: { host: string; port?: number; userName: string; password?: string; configId?: string },
): Promise<string> {
  return request<{ path?: string; Path?: string } | string>({
    url: `${DIRECTORY_EXPLORE_API_BASE}/mkdir/ftp`,
    method: 'post',
    data: {
      host: auth.host,
      port: auth.port,
      path,
      userName: auth.userName,
      password: auth.password,
      databaseBackupId: auth.configId,
    },
  }).then((res) => unwrapMkdirPath(res, path))
}

/**
 * 按方式统一浏览（三种方法入口）
 * @param req 请求
 * @returns 浏览结果
 */
export async function browseDirectory(
  req: TaktDirectoryExploreBrowseRequest,
): Promise<TaktDirectoryExploreBrowseResult> {
  if (req.method === 'server') {
    return browseServerDirectory(req.path)
  }
  if (req.method === 'fileserver') {
    const path = String(req.path || '').trim()
    if (!path) {
      throw new Error('fileserver path required')
    }
    return browseFileServerDirectory(path, req.fileServer)
  }
  const ftp = req.ftp
  if (!ftp?.host || !ftp?.userName) {
    throw new Error('ftp auth required')
  }
  return browseFtpDirectory(req.path, ftp)
}

/**
 * 按方式统一创建目录
 * @param req 请求
 * @returns 创建后路径
 */
export async function createDirectory(req: TaktDirectoryExploreMkdirRequest): Promise<string> {
  if (req.method === 'server') {
    return createServerDirectory(req.path)
  }
  if (req.method === 'fileserver') {
    return createFileServerDirectory(req.path, req.fileServer)
  }
  const ftp = req.ftp
  if (!ftp?.host || !ftp?.userName) {
    throw new Error('ftp auth required')
  }
  return createFtpDirectory(req.path, ftp)
}
