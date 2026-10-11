// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils
// 文件名称：taktEcAttachmentStorage.ts
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件默认上传路径（与后端 TaktEcAttachmentStorageConstants 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FileUploadMeta } from '@/types/foundation/file.types'
import { buildFileStorageConfigJson } from '@/utils/taktFileStorageConfig'

/** 一级菜单后勤管理对应上传根路径 */
export const EC_ATTACHMENT_MENU_UPLOAD_PATH = 'uploads/logistics'

/**
 * 引擎 CategoryPath。最终磁盘路径：
 * uploads/logistics/ec/{租户}/{公司}/{年}/{月}/{日}/{文件名}
 */
export const EC_ATTACHMENT_CATEGORY_PATH = 'logistics/ec'

/** 字典 sys_storage_naming=2 自定义磁盘文件名 */
const TAKT_STORAGE_NAMING_CUSTOM = 2

/**
 * 组装设变附件上传元数据（业务目录 logistics/ec，命名用文件编码）
 * @param targetFileName 磁盘文件名（DocCode + 原扩展名）
 * @param storageNaming 存储命名规则，默认 2=自定义
 * @returns 传给 uploadTaktFileSmart 的 meta
 */
export function buildEcAttachmentFileUploadMeta(
  targetFileName: string,
  storageNaming: number = TAKT_STORAGE_NAMING_CUSTOM,
): FileUploadMeta {
  const name = String(targetFileName ?? '').trim()
  if (!name) {
    throw new Error('targetFileName is required')
  }
  return {
    categoryPath: EC_ATTACHMENT_CATEGORY_PATH,
    storageNaming,
    targetFileName: name,
    storageConfig: buildFileStorageConfigJson({
      uploadPath: EC_ATTACHMENT_MENU_UPLOAD_PATH,
      storageNaming,
    }) ?? undefined,
  }
}
