<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktTabs
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:多标签页组件,支持标签管理、刷新、关闭等操作

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div
    v-if="show"
    class="takt-tabs-container"
  >
    <div class="tabs-wrapper">
      <a-tabs
        v-model:active-key="activeKey"
        :class="['takt-tabs', tabStyleClass]"
        type="editable-card"
        hide-add
        @edit="handleEdit"
        @change="handleChange"
      >
        <a-tab-pane
          v-for="tab in displayTabs"
          :key="tab.key"
          :closable="tab.closable !== false"
        >
          <template #tab>
            <span class="tab-content">
              <component
                :is="tab.icon"
                v-if="tab.icon"
                :class="['tab-icon', TAKT_REMIX_ICON_CLASS]"
              />
              <span>{{ tab.title }}</span>
            </span>
          </template>
          <slot
            :name="tab.key"
            :tab="tab"
          >
            <router-view v-if="tab.component" />
          </slot>
        </a-tab-pane>
        <template #rightExtra>
          <div class="tabs-extra">
            <a-dropdown
              :trigger="['click']"
              placement="bottomRight"
            >
              <a-button
                type="text"
                class="tabs-dropdown-btn"
              >
                <template #icon>
                  <RiArrowDownSLine class="takt-remix-icon" />
                </template>
              </a-button>
              <template #overlay>
                <a-menu @click="handleMenuClick">
                  <a-menu-item key="refresh">
                    {{ t('components.navigation.page.tabs.refreshcurrent') }}
                  </a-menu-item>
                  <a-menu-item
                    key="close-current"
                    :disabled="activeKey === '/dashboard/workspace' || displayTabs.length <= 1"
                  >
                    {{ t('components.navigation.page.tabs.closecurrent') }}
                  </a-menu-item>
                  <a-menu-item
                    key="close-right"
                    :disabled="!hasRightTabs"
                  >
                    {{ t('components.navigation.page.tabs.closeright') }}
                  </a-menu-item>
                  <a-menu-item
                    key="close-left"
                    :disabled="!hasLeftTabs"
                  >
                    {{ t('components.navigation.page.tabs.closeleft') }}
                  </a-menu-item>
                  <a-menu-item
                    key="close-other"
                    :disabled="displayTabs.length <= 1"
                  >
                    {{ t('components.navigation.page.tabs.closeothers') }}
                  </a-menu-item>
                  <a-menu-item
                    key="close-all"
                    :disabled="displayTabs.length <= 1"
                  >
                    {{ t('components.navigation.page.tabs.closeall') }}
                  </a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>
            <a-button
              type="text"
              class="tabs-fullscreen-btn"
              :title="isFullscreen ? t('common.page.button.exitfullscreen') : t('common.page.button.fullscreen')"
              @click="handleToggleFullscreen"
            >
              <template #icon>
                <RiFullscreenExitLine class="takt-remix-icon" v-if="isFullscreen" />
                <RiFullscreenLine class="takt-remix-icon" v-else />
              </template>
            </a-button>
          </div>
        </template>
      </a-tabs>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Component } from 'vue';
import type { RouteMeta } from 'vue-router';
import { useI18n } from 'vue-i18n'
import { RiArrowDownSLine, RiFullscreenLine, RiFullscreenExitLine } from '@remixicon/vue'
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import { defaultSetting, useSettingStore } from '@/stores/common/useSettingStore'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import type { MenuTree } from '@/types/identity/menu.types'
import {
  getRemixIconComponent,
  preloadRemixIcons,
} from '@/utils/taktRemixIcon'
import { TAKT_REMIX_ICON_CLASS } from '@/utils/common'
import { normalizeRoutePath } from '@/utils/permission'
import { NOT_FOUND_ROUTE_NAME } from '@/router/menuRoutes'
import { TAKT_TABS_STORAGE_KEY } from '@/bootstrap/taktLogoutFlow'
import { EventBus } from '@/utils/eventBus'

interface Tab {
  key: string
  title: string
  path: string
  closable?: boolean
  component?: Component
  icon?: Component | undefined
}
type StoredTab = {
  key: string
  title: string
  path: string
  closable?: boolean
}

interface Props {
  tabs?: Tab[]
  maxTabs?: number
}

const props = withDefaults(defineProps<Props>(), {
  tabs: () => [],
  maxTabs: 10
})

const emit = defineEmits<{
  'update:activeKey': [key: string]
  'change': [key: string]
  'edit': [key: string, action: 'add' | 'remove']
  'close-current': [key: string]
  'close-right': [key: string]
  'close-left': [key: string]
  'close-other': [key: string]
  'close-all': []
  'refresh': [key: string]
}>()

const route = useRoute()
const router = useRouter()
const { setting } = storeToRefs(useSettingStore())
const settingSafe = computed(() => setting.value ?? defaultSetting)
const { t, locale: i18nLocale } = useI18n()
const menuStore = useMenuStore()

/** 是否显示标签栏（仅由 showTabs 控制；multiTab 控制是否累积多页签） */
const show = computed(() => !!settingSafe.value.showTabs)
/** 标签风格 class（card / google） */
const tabStyleClass = computed(() => {
  const style = settingSafe.value.tabStyle === 'card' ? 'card' : 'google'
  return `tab-style-${style}`
})
const activeKey = ref<string>('')
const tabsList = ref<Tab[]>([])
const isFullscreen = ref(false)

/**
 * 从菜单树中按 routePath 查找节点
 * @param menus 菜单树
 * @param path 路由 path
 */
const findMenuByPath = (menus: MenuTree[], path: string): MenuTree | null => {
  const normalized = normalizeRoutePath(path)

  for (const menu of menus) {
    const menuPath = menu.routePath?.trim()
      ? normalizeRoutePath(menu.routePath)
      : ''

    if (menuPath && menuPath === normalized) {
      return menu
    }

    if (menu.children?.length) {
      const found = findMenuByPath(menu.children, normalized)
      if (found) {
        return found
      }
    }
  }

  return null
}

/**
 * 获取翻译后的标签标题
 * @param menu 菜单节点
 * @param routeMeta 路由 meta
 */
/**
 * 动态路由已注册时解析 tab 对应 route meta；未注册时跳过 resolve（避免登出/清路由后 Vue Router 警告）
 * @param path 标签 path
 */
function resolveTabRouteMeta(path: string): RouteMeta | null {
  if (!menuStore.isDynamicRoutesRegistered) {
    return null
  }

  const resolved = router.resolve(path)
  if (resolved.matched.length === 0 || resolved.name === NOT_FOUND_ROUTE_NAME) {
    return null
  }

  return resolved.matched[resolved.matched.length - 1]?.meta ?? null
}

/** 登出后清空内存标签与持久化，避免恢复已注销的动态路由 path */
function resetTabsForLogout(): void {
  tabsList.value = []
  activeKey.value = ''
  localStorage.removeItem(TAKT_TABS_STORAGE_KEY)
}

const getTranslatedTitle = (menu: MenuTree | null, routeMeta: RouteMeta | null | undefined): string => {
  if (menu?.i18nKey) {
    const translated = t(menu.i18nKey)
    if (translated && translated !== menu.i18nKey) {
      return translated
    }
  }

  if (menu?.menuName) {
    return menu.menuName
  }

  const titleKey = (routeMeta?.title || routeMeta?.titleKey || '') as string
  return titleKey ? t(titleKey) : ''
}

// 管理标签列表
const manageTabs = async () => {
  const currentPath = route.path
  const matchedRoute = route.matched[route.matched.length - 1]
  const routeMeta = matchedRoute?.meta
  
  // 从菜单 store 中查找对应的菜单项（后端已统一转换为 camelCase）
  const menu = findMenuByPath(menuStore.menuList, currentPath)
  const menuIcon = menu?.icon?.trim() || (routeMeta?.icon as string | undefined)
  
  const landingPath = '/dashboard/workspace'
  const landingMenu = findMenuByPath(menuStore.menuList, landingPath)
  const landingMenuIcon = landingMenu?.icon?.trim()
  
  const toPreload = [menuIcon, landingMenuIcon].filter(Boolean) as string[]
  const unique = [...new Set(toPreload)]
  if (unique.length > 0) {
    await preloadRemixIcons(unique)
  }
  
  const iconComponent = menuIcon ? getRemixIconComponent(menuIcon) : undefined
  const landingIconComponent = landingMenuIcon ? getRemixIconComponent(landingMenuIcon) : undefined
  
  // 获取翻译后的标题（固定落地页用工作台菜单名；禁止用已隐藏的 HOME/tabs.home）
  const currentTitle = getTranslatedTitle(menu, routeMeta ?? null) || routeMeta?.title as string || route.name as string || t('components.navigation.page.tabs.unnamed')
  const landingTitle = getTranslatedTitle(landingMenu, null) || t('menu.workspace')
  
  const landingTab = tabsList.value.find(t => t.key === landingPath)
  
  if (!landingTab) {
    // 如果落地页签不存在，添加到第一个位置
    tabsList.value.unshift({
      key: landingPath,
      title: landingTitle,
      path: landingPath,
      closable: false,
      icon: landingIconComponent
    })
  } else {
    // 确保落地页签在第一个位置
    const landingIndex = tabsList.value.findIndex(t => t.key === landingPath)
    if (landingIndex > 0) {
      tabsList.value.splice(landingIndex, 1)
      tabsList.value.unshift(landingTab)
    }
    // 确保落地页签不可关闭，并更新标题和图标
    landingTab.closable = false
    landingTab.title = landingTitle
    landingTab.icon = landingIconComponent
  }
  
  // 检查当前路由标签是否已存在（排除落地页）
  const existingTab = tabsList.value.find(t => t.key === currentPath && t.key !== landingPath)
  
  if (!existingTab && currentPath !== landingPath) {
    // 未启用多页签：只保留落地页 + 当前页（替换非落地页签）
    if (!settingSafe.value.multiTab) {
      tabsList.value = tabsList.value.filter((t) => t.key === landingPath)
    } else {
      // 如果标签数量达到上限，移除最后一个标签（但保留落地页）
      const maxTabsValue = props.maxTabs || settingSafe.value.maxTabs || 10
      if (tabsList.value.length >= maxTabsValue) {
        const lastTab = tabsList.value[tabsList.value.length - 1]
        if (lastTab && lastTab.key !== landingPath) {
          tabsList.value.pop()
        } else {
          // 如果最后一个是落地页，移除倒数第二个
          tabsList.value.splice(tabsList.value.length - 2, 1)
        }
      }
    }

    // 添加新标签（在落地页之后）
    tabsList.value.push({
      key: currentPath,
      title: currentTitle,
      path: currentPath,
      closable: true,
      icon: iconComponent
    })
  } else if (existingTab) {
    // 如果标签已存在，更新标题和图标
    existingTab.title = currentTitle
    existingTab.icon = iconComponent
  }
  
  // 更新当前活动标签
  activeKey.value = currentPath
}

// 显示的标签（最多 maxTabs 个）
const displayTabs = computed(() => {
  const maxTabsValue = props.maxTabs || settingSafe.value.maxTabs || 10
  if (props.tabs.length > 0) {
    return props.tabs.slice(0, maxTabsValue)
  }
  return tabsList.value.slice(0, maxTabsValue)
})

// 判断当前标签右侧是否有其他标签（除了首页）
const hasRightTabs = computed(() => {
  const currentIndex = tabsList.value.findIndex(t => t.key === activeKey.value)
  if (currentIndex < 0) return false
  // 如果当前标签不是最后一个，且右侧还有标签（排除首页），则有右侧标签
  if (currentIndex < tabsList.value.length - 1) {
    // 检查右侧是否有非首页的标签
    for (let i = currentIndex + 1; i < tabsList.value.length; i++) {
      const tab = tabsList.value[i]
      if (tab && tab.key !== '/dashboard/workspace') {
        return true
      }
    }
  }
  return false
})

// 判断当前标签左侧是否有其他标签（除了首页）
const hasLeftTabs = computed(() => {
  const currentIndex = tabsList.value.findIndex(t => t.key === activeKey.value)
  if (currentIndex <= 0) return false // 索引0是首页，没有左侧标签
  // 检查左侧是否有非首页的标签
  for (let i = currentIndex - 1; i > 0; i--) {
    const tab = tabsList.value[i]
    if (tab && tab.key !== '/dashboard/workspace') {
      return true
    }
  }
  return false
})

// 持久化标签页到 localStorage
const saveTabsToStorage = () => {
  if (settingSafe.value.persistTabs) {
    const tabsData = tabsList.value.map(tab => ({
      key: tab.key,
      title: tab.title,
      path: tab.path,
      closable: tab.closable
    }))
    localStorage.setItem(TAKT_TABS_STORAGE_KEY, JSON.stringify({
      tabs: tabsData,
      activeKey: activeKey.value
    }))
  }
}

// 从 localStorage 恢复标签页
const loadTabsFromStorage = () => {
  if (settingSafe.value.persistTabs) {
    const stored = localStorage.getItem(TAKT_TABS_STORAGE_KEY)
    if (stored) {
      try {
        const data = JSON.parse(stored)
        if (data.tabs && Array.isArray(data.tabs)) {
          const landingPath = '/dashboard/workspace'
          // 纠正历史错误路径；丢弃已隐藏的 /home（HOME 菜单 MenuStatus/IsVisible=0）
          const normalizeTabPath = (path: string): string | null => {
            const p = String(path || '').trim()
            if (!p || p === '/home' || p.startsWith('/home/')) {
              return null
            }
            if (p === '/dashboard/workspace.types.types' || p === '/dashboard/workspace.types') {
              return landingPath
            }
            return p
          }
          tabsList.value = data.tabs
            .map((tab: StoredTab) => {
              const path = normalizeTabPath(tab.path || tab.key)
              if (!path) {
                return null
              }
              return {
                key: path,
                title: path === landingPath
                  ? (findMenuByPath(menuStore.menuList, landingPath)?.menuName || tab.title)
                  : tab.title,
                path,
                closable: path === landingPath ? false : tab.closable !== false,
                icon: undefined
              }
            })
            .filter(Boolean) as typeof tabsList.value
          // 恢复活动标签
          const active = normalizeTabPath(String(data.activeKey || ''))
          if (active && tabsList.value.find(t => t.key === active)) {
            activeKey.value = active
            router.push(active)
          }
        }
      } catch {
        // 解析失败，忽略
      }
    }
  }
}

// 监听路由变化
watch(() => route.path, () => {
  manageTabs()
  saveTabsToStorage()
}, { immediate: true })

// 重新计算所有标签页的标题（用于语言切换后更新）
const refreshTabsTitles = () => {
  if (tabsList.value.length === 0) {
    return
  }

  tabsList.value.forEach(tab => {
    const menu = findMenuByPath(menuStore.menuList, tab.path)
    const routeMeta = resolveTabRouteMeta(tab.path)
    const newTitle = getTranslatedTitle(menu, routeMeta) || tab.title
    tab.title = newTitle
  })
  saveTabsToStorage()
}

// 监听语言变化，重新计算所有标签页的标题
// 延迟更新以确保翻译数据已经加载完成
watch(() => i18nLocale.value, async (newLocale, oldLocale) => {
  // 如果语言没有变化，跳过
  if (newLocale === oldLocale) return
  
  // 先立即更新一次（使用静态翻译或 fallback）
  refreshTabsTitles()
  
  // 等待 Vue 的响应式更新完成
  await nextTick()
  
  // 延迟再次更新，确保后端翻译数据已加载完成
  // 使用多次延迟更新，确保在所有异步操作完成后更新
  setTimeout(() => {
    refreshTabsTitles()
  }, 300)
  
  // 再次延迟更新，处理可能的网络延迟
  setTimeout(() => {
    refreshTabsTitles()
  }, 1000)
  
  // 最后延迟更新，确保所有翻译数据都已加载
  setTimeout(() => {
    refreshTabsTitles()
  }, 2000)
})

// 监听设置变化，如果关闭持久化则清除存储
watch(() => settingSafe.value.persistTabs, (enabled) => {
  if (!enabled) {
    localStorage.removeItem(TAKT_TABS_STORAGE_KEY)
  } else {
    saveTabsToStorage()
  }
})

// 监听标签列表变化，保存到存储
watch(tabsList, () => {
  saveTabsToStorage()
}, { deep: true })

// 监听活动标签变化，保存到存储
watch(activeKey, () => {
  saveTabsToStorage()
})

// 组件挂载时恢复标签页；登出时清标签（须在 performLogout 清路由之后仍可兜底）
onMounted(() => {
  EventBus.on('user:logout', resetTabsForLogout)
  if (settingSafe.value.persistTabs) {
    loadTabsFromStorage()
  }
})

onBeforeUnmount(() => {
  EventBus.off('user:logout', resetTabsForLogout)
})

// 标签切换
const handleChange = (key: string | number) => {
  const keyStr = String(key)
  activeKey.value = keyStr
  const tab = displayTabs.value.find(t => t.key === keyStr)
  if (tab && tab.path) {
    router.push(tab.path)
  }
  emit('change', keyStr)
}

// 标签编辑（关闭）
const handleEdit = (targetKey: string | number | MouseEvent | KeyboardEvent, action: 'add' | 'remove') => {
  if (action === 'remove') {
    const keyStr = typeof targetKey === 'string' || typeof targetKey === 'number' ? String(targetKey) : ''
    if (keyStr) {
      // 不允许关闭首页标签
      if (keyStr === '/dashboard/workspace') {
        return
      }
      
      const index = tabsList.value.findIndex(t => t.key === keyStr)
      if (index > -1) {
        tabsList.value.splice(index, 1)
        
        // 如果关闭的是当前标签，切换到其他标签（优先切换到首页）
        if (keyStr === activeKey.value && tabsList.value.length > 0) {
          const homeTab = tabsList.value.find(t => t.key === '/dashboard/workspace')
          const nextTab = homeTab || tabsList.value[tabsList.value.length - 1]
          if (nextTab) {
            activeKey.value = nextTab.key
            router.push(nextTab.path)
          }
        }
        
        emit('edit', keyStr, action)
      }
    }
  }
}

// 下拉菜单点击
const handleMenuClick = async (info: MenuInfo) => {
  const key = String(info.key)
  const homePath = '/dashboard/workspace'
  switch (key) {
    case 'refresh':
      // 刷新当前标签
      emit('refresh', activeKey.value)
      // 重新加载当前路由
      router.go(0)
      break
    case 'close-current':
      // 关闭当前标签（不允许关闭首页）
      if (activeKey.value !== homePath) {
        handleEdit(activeKey.value, 'remove')
        emit('close-current', activeKey.value)
      }
      break
    case 'close-right': {
      // 关闭右侧标签
      const currentIndex = tabsList.value.findIndex(t => t.key === activeKey.value)
      if (currentIndex >= 0 && currentIndex < tabsList.value.length - 1) {
        // 从右侧开始删除，保留首页
        const tabsToRemove: string[] = []
        for (let i = tabsList.value.length - 1; i > currentIndex; i--) {
          const tab = tabsList.value[i]
          if (tab && tab.key !== homePath) {
            tabsToRemove.push(tab.key)
            tabsList.value.splice(i, 1)
          }
        }
        emit('close-right', activeKey.value)
      }
      break
    }
    case 'close-left': {
      // 关闭左侧标签（保留首页和当前标签）
      const currentIndexLeft = tabsList.value.findIndex(t => t.key === activeKey.value)
      if (currentIndexLeft > 1) {
        // 从索引1开始删除到当前索引之前（索引0是首页，保留）
        const tabsToRemove: string[] = []
        for (let i = currentIndexLeft - 1; i > 0; i--) {
          const tab = tabsList.value[i]
          if (tab && tab.key !== homePath) {
            tabsToRemove.push(tab.key)
            tabsList.value.splice(i, 1)
          }
        }
        emit('close-left', activeKey.value)
      }
      break
    }
    case 'close-other': {
      // 关闭其他标签，只保留当前标签和首页标签
      const currentTab = tabsList.value.find(t => t.key === activeKey.value)
      const homeTab = tabsList.value.find(t => t.key === homePath)
      if (currentTab && homeTab) {
        if (activeKey.value === homePath) {
          // 如果当前是首页，只保留首页
          tabsList.value.splice(0, tabsList.value.length, homeTab)
        } else {
          // 如果当前不是首页，保留首页和当前标签
          tabsList.value.splice(0, tabsList.value.length, homeTab, currentTab)
        }
        emit('close-other', activeKey.value)
      }
      break
    }
    case 'close-all': {
      // 关闭所有标签，但保留首页标签
      const homeTabOnly = tabsList.value.find(t => t.key === homePath)
      if (homeTabOnly) {
        tabsList.value.splice(0, tabsList.value.length, homeTabOnly)
        activeKey.value = homePath
        router.push(homePath)
      } else {
        // 如果首页标签不存在，创建它（后端已统一转换为 camelCase）
        const homeMenu = findMenuByPath(menuStore.menuList, homePath)
        const homeMenuIcon = homeMenu?.icon?.trim()
        
        if (homeMenuIcon) {
          await preloadRemixIcons([homeMenuIcon])
        }
        const homeIconComponent = homeMenuIcon ? getRemixIconComponent(homeMenuIcon) : undefined
        const homeTitle = getTranslatedTitle(homeMenu, null) || t('menu.workspace')
        
        tabsList.value.splice(0, tabsList.value.length, {
          key: homePath,
          title: homeTitle,
          path: homePath,
          closable: false,
          icon: homeIconComponent
        })
        activeKey.value = homePath
        router.push(homePath)
      }
      emit('close-all')
      break
    }
  }
}

// 全屏切换
const handleToggleFullscreen = () => {
  if (!document.fullscreenElement) {
    document.documentElement.requestFullscreen().then(() => {
      isFullscreen.value = true
    }).catch(() => {
      // 全屏失败，忽略
    })
  } else {
    document.exitFullscreen().then(() => {
      isFullscreen.value = false
    }).catch(() => {
      // 退出全屏失败，忽略
    })
  }
}

// 监听全屏状态变化
const handleFullscreenChange = () => {
  isFullscreen.value = !!document.fullscreenElement
}

onMounted(() => {
  document.addEventListener('fullscreenchange', handleFullscreenChange)
  manageTabs()
})

onUnmounted(() => {
  document.removeEventListener('fullscreenchange', handleFullscreenChange)
})
</script>

<style scoped>
.takt-tabs-container {
  height: 40px;
  display: flex;
  align-items: center;
  padding: 0 8px;
  transition: border-bottom-color 0.25s ease, background-color 0.25s ease;
}

.tabs-wrapper {
  flex: 1;
  display: flex;
  align-items: center;
  height: 40px;
  overflow: hidden;
}

.takt-tabs {
  flex: 1;
  height: 40px;

  :deep(.ant-tabs-nav) {
    transition: background-color 0.25s ease;
  }
  :deep(.ant-tabs-tab) {
    transition: background-color 0.25s ease, border-color 0.25s ease, border-radius 0.25s ease, margin 0.25s ease;
  }

  :deep(.ant-tabs-content-holder) {
    display: none;
  }

  :deep(.ant-tabs-tab .ant-tabs-tab-btn) {
    .tab-content {
      display: inline-flex;
      align-items: center;

      .tab-icon {
        margin-right: 4px;
        margin-left: 0;
      }
    }
  }

  &.tab-style-card {
    :deep(.ant-tabs-nav) {
      background: transparent;
    }
    :deep(.ant-tabs-tab) {
      border-radius: 5px 5px 0 0;
      border-bottom: none;
      margin-right: 4px;
    }
  }

  &.tab-style-google {
    :deep(.ant-tabs-nav) {
      background: transparent;
    }
    :deep(.ant-tabs-ink-bar) {
      display: none;
    }
    :deep(.ant-tabs-tab) {
      position: relative;
      border-radius: 12px 12px 0 0;
      border: 1px solid transparent;
      margin-right: 2px;
      background-color: transparent;
      overflow: visible;

      &::before,
      &::after {
        position: absolute;
        bottom: 0;
        content: '';
        width: 20px;
        height: 20px;
        border-radius: 100%;
        box-shadow: 0 0 0 40px transparent;
        transition: box-shadow 0.25s ease;
      }
      &::before {
        left: -20px;
        clip-path: inset(50% -10px 0 50%);
      }
      &::after {
        right: -20px;
        clip-path: inset(50% 50% 0 -10px);
      }
    }
  }
}

.tabs-extra {
  display: flex;
  align-items: center;
  height: 40px;
  gap: 4px;

  .tabs-dropdown-btn,
  .tabs-fullscreen-btn {
    height: 32px;
    width: 32px;
    padding: 0;
    display: inline-flex;
    align-items: center;
    justify-content: center;
  }
}
</style>

<!-- 非 scoped：须匹配 html[data-theme]，并压过 Ant Design editable-card 默认样式 -->
<style>
html[data-theme='light'] .takt-tabs-container {
  background: #f5f5f5;
  border-bottom: 1px solid #d9d9d9;
}

html[data-theme='dark'] .takt-tabs-container {
  background: #1f1f1f;
  border-bottom: 1px solid rgba(255, 255, 255, 0.12);
}

html[data-theme='light'] .takt-tabs-container:has(.takt-tabs.tab-style-google) {
  border-bottom-color: #f5f5f5;
}

html[data-theme='dark'] .takt-tabs-container:has(.takt-tabs.tab-style-google) {
  border-bottom-color: #1f1f1f;
}

html[data-theme='light'] .takt-tabs.tab-style-card.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab {
  border: 1px solid #d9d9d9 !important;
  background: transparent !important;
}

html[data-theme='light'] .takt-tabs.tab-style-card.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active {
  background: #fff !important;
  border-bottom-color: #fff !important;
}

html[data-theme='dark'] .takt-tabs.tab-style-card.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab {
  border: 1px solid rgba(255, 255, 255, 0.12) !important;
  background: transparent !important;
}

html[data-theme='dark'] .takt-tabs.tab-style-card.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active {
  background: #141414 !important;
  border-bottom-color: #141414 !important;
}

html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab {
  border-color: #f5f5f5 !important;
  background: transparent !important;
}

html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab:hover {
  background-color: #f0f0f0 !important;
}

html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab:hover::before,
html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab:hover::after {
  box-shadow: 0 0 0 30px #f0f0f0;
}

html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active {
  background-color: #fff !important;
  border-color: #fff !important;
}

html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active::before,
html[data-theme='light'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active::after {
  box-shadow: 0 0 0 30px #fff;
}

html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab {
  border-color: #1f1f1f !important;
  background: transparent !important;
}

html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab:hover {
  background-color: #2a2a2a !important;
}

html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab:hover::before,
html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab:hover::after {
  box-shadow: 0 0 0 30px #2a2a2a;
}

html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active {
  background-color: #262626 !important;
  border-color: #262626 !important;
}

html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active::before,
html[data-theme='dark'] .takt-tabs.tab-style-google.ant-tabs-card > .ant-tabs-nav .ant-tabs-tab-active::after {
  box-shadow: 0 0 0 30px #262626;
}
</style>
