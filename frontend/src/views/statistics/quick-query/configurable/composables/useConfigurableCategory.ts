// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/statistics/quick-query/configurable/composables
// 文件名称：useConfigurableCategory.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表业务域/子分类与侧栏菜单联动（根目录=模块，下级目录=子分类）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Ref } from 'vue'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import { TaktMenuType } from '@/utils/common'
import type { MenuTree } from '@/types/identity/menu.types'
import {
  CONFIGURABLE_DOMAIN_MENU_CODES,
  MENU_CODE_BY_CONFIGURABLE_DOMAIN,
  getRootMenuLabel,
  findConfigurableDomainMenu,
  extractConfigurableSubCategoryFromRoute,
  resolveConfigurableSubCategoryLabel,
  resolveConfigurableDomainLabel,
} from '@/utils/taktModuleRootMenu'

export { CONFIGURABLE_DOMAIN_MENU_CODES, MENU_CODE_BY_CONFIGURABLE_DOMAIN }

/** 菜单展示文案（兼容导出） */
export function getConfigurableMenuLabel(menu: MenuTree): string {
  return getRootMenuLabel(menu)
}

export { extractConfigurableSubCategoryFromRoute, findConfigurableDomainMenu, resolveConfigurableSubCategoryLabel, resolveConfigurableDomainLabel }

/**
 * 定制报表业务域与子分类联动（基于登录菜单树，configurableDomain 存 TaktModule 整型）
 * @param configurableDomainRef 定制报表业务域
 * @param configurableSubCategoryRef 定制报表子分类（路由末段）；传入时在模块切换时校验并清空无效值
 * @returns 模块/子分类下拉选项与解析函数
 */
export function useConfigurableCategory(
  configurableDomainRef: Ref<number | undefined | null>,
  configurableSubCategoryRef?: Ref<string | undefined | null>
) {
  const menuStore = useMenuStore()

  /** 一级目录（ParentId=0、MenuType=目录）→ 模块选项 */
  const moduleOptions = computed(() =>
    menuStore.menuList
      .filter(
        (menu) =>
          menu.menuType === TaktMenuType.Directory &&
          CONFIGURABLE_DOMAIN_MENU_CODES[menu.menuCode] != null
      )
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .map((menu) => ({
        label: getRootMenuLabel(menu),
        value: CONFIGURABLE_DOMAIN_MENU_CODES[menu.menuCode],
      }))
  )

  /** 当前选中模块对应的一级菜单 */
  const selectedModuleMenu = computed(() =>
    findConfigurableDomainMenu(menuStore.menuList, configurableDomainRef.value ?? undefined)
  )

  /** 选中模块下直接子级目录（MenuType=0）→ 子分类选项 */
  const subCategoryOptions = computed(() => {
    const moduleMenu = selectedModuleMenu.value
    if (!moduleMenu?.children?.length) {
      return []
    }
    return moduleMenu.children
      .filter((child) => child.menuType === TaktMenuType.Directory)
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .map((child) => ({
        label: getRootMenuLabel(child),
        value: extractConfigurableSubCategoryFromRoute(child.routePath),
      }))
      .filter((option) => option.value)
  })

  if (configurableSubCategoryRef) {
    watch(configurableDomainRef, () => {
      const sub = configurableSubCategoryRef.value?.trim()
      if (!sub) {
        return
      }
      const valid = subCategoryOptions.value.some((option) => option.value === sub)
      if (!valid) {
        configurableSubCategoryRef.value = ''
      }
    })
  }

  /**
   * 解析子分类展示名（当前菜单树）
   * @param configurableDomain 定制报表业务域
   * @param configurableSubCategory 子分类路由末段
   * @returns {string} 展示文本
   */
  function resolveSubCategoryLabel(configurableDomain?: number | null, configurableSubCategory?: string | null): string {
    return resolveConfigurableSubCategoryLabel(configurableDomain, configurableSubCategory, menuStore.menuList)
  }

  /**
   * 解析业务域展示名（当前菜单树）
   * @param configurableDomain 定制报表业务域
   * @returns {string} 展示文本
   */
  function resolveDomainLabel(configurableDomain?: number | null): string {
    return resolveConfigurableDomainLabel(configurableDomain, menuStore.menuList)
  }

  return {
    moduleOptions,
    subCategoryOptions,
    resolveSubCategoryLabel,
    resolveDomainLabel,
  }
}
