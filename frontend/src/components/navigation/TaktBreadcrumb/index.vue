<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktBreadcrumb
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:面包屑按菜单树祖先链展示（父目录→…→当前页）；仅当前页时不显示，避免与标签页重复

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-breadcrumb
    v-if="show && breadcrumbItems.length > 1"
    class="takt-breadcrumb"
  >
    <a-breadcrumb-item
      v-for="(item, index) in breadcrumbItems"
      :key="`${item.key}-${index}`"
    >
      <router-link
        v-if="item.path && index < breadcrumbItems.length - 1"
        :to="item.path"
        class="breadcrumb-link"
      >
        <component
          :is="item.icon"
          v-if="showIcon && item.icon"
          :class="TAKT_REMIX_ICON_CLASS"
        />
        <span class="breadcrumb-title">{{ item.title }}</span>
      </router-link>
      <span
        v-else
        class="breadcrumb-plain"
      >
        <component
          :is="item.icon"
          v-if="showIcon && item.icon"
          :class="TAKT_REMIX_ICON_CLASS"
        />
        <span class="breadcrumb-title">{{ item.title }}</span>
      </span>
    </a-breadcrumb-item>
  </a-breadcrumb>
</template>

<script setup lang="ts">
import type { Component } from 'vue'
import { useI18n } from 'vue-i18n'
import { defaultSetting, useSettingStore } from '@/stores/common/useSettingStore'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import type { MenuTree } from '@/types/identity/menu.types'
import {
  getRemixIconComponent,
  preloadRemixIcons,
} from '@/utils/taktRemixIcon'
import { TAKT_REMIX_ICON_CLASS, TaktMenuType } from '@/utils/common'
import { normalizeRoutePath } from '@/utils/permission'

/** 面包屑最大祖先深度（含当前页） */
const MAX_BREADCRUMB_DEPTH = 10

interface BreadcrumbItem {
  /** 稳定 key（menuId 或 path） */
  key: string
  title: string
  /** 可导航时才有 path；目录节点无路由则不可点 */
  path?: string
  icon?: Component
}

const route = useRoute()
const { setting } = storeToRefs(useSettingStore())
const settingSafe = computed(() => setting.value ?? defaultSetting)
const { t } = useI18n()
const menuStore = useMenuStore()

/** 图标缓存版本（预加载后递增） */
const iconRevision = ref(0)

/** 是否开启面包屑偏好 */
const show = computed(() => !!settingSafe.value.showBreadcrumb)

/** 是否在面包屑项上显示图标 */
const showIcon = computed(() => !!settingSafe.value.breadcrumbIcon)

/**
 * 在菜单树中查找匹配 path 的祖先链（根 → 当前）
 * @param menus 菜单树
 * @param path 当前路由 path
 * @param trail 已走过的祖先
 * @returns {MenuTree[] | null} 祖先链（含当前节点）
 */
function findMenuAncestorTrail(
  menus: MenuTree[],
  path: string,
  trail: MenuTree[] = [],
): MenuTree[] | null {
  if (trail.length >= MAX_BREADCRUMB_DEPTH) {
    return null
  }

  const normalized = normalizeRoutePath(path)

  for (const menu of menus) {
    if (menu.menuType === TaktMenuType.Button) {
      continue
    }

    const nextTrail = [...trail, menu]
    const menuPath = menu.routePath?.trim()
      ? normalizeRoutePath(menu.routePath)
      : ''

    if (menuPath && menuPath === normalized) {
      return nextTrail
    }

    if (menu.children?.length) {
      const found = findMenuAncestorTrail(menu.children, normalized, nextTrail)
      if (found) {
        return found
      }
    }
  }

  return null
}

/**
 * 菜单节点展示标题
 * @param menu 菜单节点
 */
function getMenuTitle(menu: MenuTree): string {
  if (menu.i18nKey) {
    const translated = t(menu.i18nKey)
    if (translated && translated !== menu.i18nKey) {
      return translated
    }
  }
  return menu.menuName || menu.menuCode || ''
}

/**
 * 菜单节点是否可作为面包屑中间项的跳转目标
 * @param menu 菜单节点
 */
function resolveNavigablePath(menu: MenuTree): string | undefined {
  if (menu.menuType !== TaktMenuType.Menu) {
    return undefined
  }
  const raw = menu.routePath?.trim()
  if (!raw) {
    return undefined
  }
  return normalizeRoutePath(raw)
}

/**
 * 当前路由对应的菜单祖先链
 */
const menuTrail = computed(() => {
  const path = route.path
  if (!path || !menuStore.menuList?.length) {
    return [] as MenuTree[]
  }
  return findMenuAncestorTrail(menuStore.menuList, path) ?? []
})

/**
 * 预加载祖先链图标；仅写入新缓存时 bump，避免死循环
 */
watch(
  () => [route.fullPath, menuTrail.value, show.value, showIcon.value] as const,
  async () => {
    if (!show.value || !showIcon.value || menuTrail.value.length === 0) {
      return
    }
    const names = menuTrail.value
      .map((m) => m.icon?.trim())
      .filter((name): name is string => !!name)
    const missing = names.filter((name) => !getRemixIconComponent(name))
    if (missing.length === 0) {
      return
    }
    await preloadRemixIcons(missing)
    if (missing.some((name) => getRemixIconComponent(name))) {
      iconRevision.value += 1
    }
  },
  { immediate: true },
)

/**
 * 面包屑项：菜单祖先链（至少 2 级才有展示意义）
 */
const breadcrumbItems = computed(() => {
  void iconRevision.value

  const trail = menuTrail.value
  if (trail.length < 2) {
    return [] as BreadcrumbItem[]
  }

  return trail.map((menu, index) => {
    const isLast = index === trail.length - 1
    const navigablePath = resolveNavigablePath(menu)
    const menuIcon = menu.icon?.trim()
    const iconComponent =
      showIcon.value && menuIcon ? getRemixIconComponent(menuIcon) : undefined

    const item: BreadcrumbItem = {
      key: menu.menuId || menu.menuCode || `${index}`,
      title: getMenuTitle(menu),
    }

    // 末级不可点；中间项仅页面菜单且有 routePath 时可跳转
    if (!isLast && navigablePath) {
      item.path = navigablePath
    }

    if (iconComponent) {
      item.icon = iconComponent
    }

    return item
  })
})
</script>

<style scoped>
.takt-breadcrumb {
  display: inline-flex;
  align-items: center;
  min-height: 24px;
  line-height: 1;
}

/* Ant Design ol/li + 分隔符与文字垂直居中平齐 */
:deep(ol) {
  display: inline-flex;
  align-items: center;
  flex-wrap: wrap;
  margin: 0;
  padding: 0;
  list-style: none;
}

:deep(li) {
  display: inline-flex;
  align-items: center;
  line-height: 1;
}

:deep(.ant-breadcrumb-link),
:deep(.ant-breadcrumb-separator) {
  display: inline-flex;
  align-items: center;
  line-height: 1;
}

:deep(.ant-breadcrumb-separator) {
  margin-inline: 8px;
  /* 抵消默认基线偏移，与标签文本中线对齐 */
  position: relative;
  top: 0;
  height: 1em;
}

:deep(.breadcrumb-link),
:deep(.breadcrumb-plain) {
  display: inline-flex;
  align-items: center;
  line-height: 1;
  gap: 6px;
}

:deep(.breadcrumb-title) {
  line-height: 1;
  margin-left: 0;
}

:deep(.takt-remix-icon) {
  display: inline-flex;
  flex-shrink: 0;
  line-height: 1;
  vertical-align: middle;
}
</style>
