// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/components
// 文件名称：directoryExplore.types.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：通用目录浏览 TaktDirectoryExplore 类型（服务器 / 文件服务器 / FTP）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktFolderExplorerBrowseResult, TaktFolderExplorerItem } from './folder-explorer'

/**
 * 目录浏览方式（三种）
 * - server：API 宿主本机磁盘（服务器目录）
 * - fileserver：UNC 网络文件服务器
 * - ftp：FTP 服务器
 */
export type TaktDirectoryExploreMethod = 'server' | 'fileserver' | 'ftp'

/**
 * 目录项（与 FolderExplorer 对齐）
 */
export type TaktDirectoryExploreItem = TaktFolderExplorerItem

/**
 * 浏览结果
 */
export type TaktDirectoryExploreBrowseResult = TaktFolderExplorerBrowseResult

/**
 * 文件服务器凭据
 */
export interface TaktDirectoryExploreFileServerAuth {
  /** 用户名 */
  userName?: string
  /** 密码 */
  password?: string
  /** 已存配置 Id（可解密密码） */
  configId?: string
}

/**
 * FTP 连接
 */
export interface TaktDirectoryExploreFtpAuth {
  /** 主机 */
  host: string
  /** 端口 */
  port?: number
  /** 用户名 */
  userName: string
  /** 密码 */
  password?: string
  /** 已存配置 Id */
  configId?: string
}

/**
 * 浏览请求（三种方式共用字段，按 method 取用）
 */
export interface TaktDirectoryExploreBrowseRequest {
  /** 浏览方式 */
  method: TaktDirectoryExploreMethod
  /** 当前路径（server/fileserver/ftp） */
  path?: string
  /** 文件服务器凭据 */
  fileServer?: TaktDirectoryExploreFileServerAuth
  /** FTP 连接 */
  ftp?: TaktDirectoryExploreFtpAuth
}

/**
 * 创建目录请求
 */
export interface TaktDirectoryExploreMkdirRequest {
  /** 浏览方式 */
  method: TaktDirectoryExploreMethod
  /** 要创建的完整路径 */
  path: string
  /** 文件服务器凭据 */
  fileServer?: TaktDirectoryExploreFileServerAuth
  /** FTP 连接 */
  ftp?: TaktDirectoryExploreFtpAuth
}
