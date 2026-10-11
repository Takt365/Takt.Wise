// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/foundation/file/composables
// 文件名称：useMenuUploadPath.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传路径与一级目录菜单联动（uploads/{RoutePath首段}，如 uploads/human-resource）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useMenuStore } from '@/stores/identity/useMenuStore'
import {
  buildUploadPathSelectOptions,
  resolveUploadPathLabel as resolveUploadPathLabelFromMenus,
} from '@/utils/taktModuleRootMenu'

/**
 * 文件上传路径下拉（一级目录菜单 → uploads/{RoutePath首段}）
 * @returns 上传路径选项与展示名解析
 */
export function useMenuUploadPath() {
  const menuStore = useMenuStore()

  /** 一级目录菜单对应的上传路径选项 */
  const uploadPathOptions = computed(() => buildUploadPathSelectOptions(menuStore.menuList))

  /**
   * 解析上传路径展示名
   * @param uploadPath 上传路径
   * @returns {string} 展示文本
   */
  function resolveUploadPathLabel(uploadPath?: string | null): string {
    return resolveUploadPathLabelFromMenus(uploadPath, menuStore.menuList)
  }

  return {
    uploadPathOptions,
    resolveUploadPathLabel,
  }
}