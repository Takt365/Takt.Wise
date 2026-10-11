<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeader
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:页面顶部组件,包含菜单折叠、公司切换、通知、语言、主题等快捷操作

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-layout-header
    class="takt-header"
    :style="headerStyle"
    :data-height="height"
  >
    <div class="header-left">
      <slot name="left">
        <a-button
          type="text"
          class="trigger"
          @click.stop="handleToggle"
        >
          <template #icon>
            <RiMenuUnfoldLine class="takt-remix-icon" v-if="props.collapsed" />
            <RiMenuFoldLine class="takt-remix-icon" v-else />
          </template>
        </a-button>
        <TaktBreadcrumb v-if="settingSafe.showBreadcrumb" />
      </slot>
    </div>
    <div class="header-right">
      <slot name="right">
        <TaktHeaderQuery />
        <TaktCompanyToggle type="icon" size="small" />       
        <TaktHeaderFull />
        <TaktHeaderFont />
        <TaktHeaderNotification
          :notifications="notifications"
          :dot="notificationDot"
          :overflow-count="notificationOverflowCount"
          @click="handleNotificationClick"
          @read="handleNotificationRead"
          @delete="handleNotificationDelete"
          @clear-all="handleNotificationClearAll"
        />
        <TaktLocaleToggle type="icon" />
        <TaktThemeToggle type="icon" />
        <TaktHeaderSetting type="icon" />
        <TaktHeaderUser 
          type="icon"
          @profile="handleProfile"
          @logout="handleLogout"
        />
      </slot>
    </div>
  </a-layout-header>
</template>

<script setup lang="ts">
import { RiMenuUnfoldLine, RiMenuFoldLine } from '@remixicon/vue'
import { defaultSetting, useSettingStore } from '@/stores/common/useSettingStore'
import { useHeaderNotificationStore } from '@/stores/navigation/useHeaderNotificationStore'
import { createLogger } from '@/utils/logger'

const headerLogger = createLogger('takt-header')
const headerNotificationStore = useHeaderNotificationStore()

type HeaderHeight = 32 | 40 | 48

interface Props {
  collapsed?: boolean
  fixed?: boolean
  leftOffset?: string
  height?: HeaderHeight
  notificationDot?: boolean
  notificationOverflowCount?: number
}

const props = withDefaults(defineProps<Props>(), {
  collapsed: false,
  fixed: true,
  leftOffset: '0px',
  height: 40,
  notificationDot: false,
  notificationOverflowCount: 99
})

/** 顶栏通知列表（来自 header-notification Store） */
const notifications = computed(() =>
  headerNotificationStore.items.map((item) => ({
    id: item.id,
    title: item.title,
    content: item.content,
    time: item.time,
    read: item.read,
  })),
)

const emit = defineEmits<{
  'update:collapsed': [value: boolean]
  'toggle': []
  'profile': []
  'logout': []
  'notification-click': []
  'notification-read': [id: string]
  'notification-delete': [id: string]
  'notification-clear-all': []
}>()

const { setting } = storeToRefs(useSettingStore())
const settingSafe = computed(() => setting.value ?? defaultSetting)

// 视口宽度
const viewportWidth = ref(typeof window !== 'undefined' ? window.innerWidth : 0)

// 计算 header 宽度：视口宽度 - leftOffset
const headerWidth = computed(() => {
  const leftOffsetValue = parseFloat(props.leftOffset) || 0
  return viewportWidth.value - leftOffsetValue
})


// 监听视口宽度变化
const handleResize = () => {
  if (typeof window !== 'undefined') {
    viewportWidth.value = window.innerWidth
  }
}

onMounted(() => {
  if (typeof window !== 'undefined') {
    viewportWidth.value = window.innerWidth
    window.addEventListener('resize', handleResize)
  }
})

onUnmounted(() => {
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', handleResize)
  }
})

const headerStyle = computed(() => {
  const isFixed = (props.fixed || settingSafe.value.fixedHeader)
  return {
    position: isFixed ? 'fixed' : 'relative',
    top: isFixed ? 0 : undefined,
    right: isFixed ? 0 : undefined,
    left: isFixed ? props.leftOffset : undefined,
    width: isFixed ? `${headerWidth.value}px` : undefined,
    zIndex: isFixed ? 100 : undefined,
    '--header-height': `${props.height}px`,
    '--header-width': isFixed ? `${headerWidth.value}px` : undefined
  }
})

const handleToggle = () => {
  if (import.meta.env.DEV) {
    headerLogger.debug('侧边栏折叠切换', { action: 'toggle', collapsed: props.collapsed })
  }
  const newCollapsed = !props.collapsed
  emit('update:collapsed', newCollapsed)
  emit('toggle')
}

const handleProfile = () => {
  emit('profile')
}

const handleLogout = () => {
  emit('logout')
}

const handleNotificationClick = () => {
  emit('notification-click')
}

const handleNotificationRead = (id: string) => {
  void headerNotificationStore.markNotificationReadAsync(id).catch(() => undefined)
  emit('notification-read', id)
}

const handleNotificationDelete = (id: string) => {
  headerNotificationStore.removeNotification(id)
  emit('notification-delete', id)
}

const handleNotificationClearAll = () => {
  headerNotificationStore.clearAllNotifications()
  emit('notification-clear-all')
}
</script>

<style scoped>
.takt-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: var(--header-height, 40px);
  line-height: var(--header-height, 40px);
  padding: 0;
  /* 确保 header 能够正确适配主题系统 */
  /* 使用 Ant Design Vue 的主题变量，确保背景色能够根据主题自动切换 */
  background: var(--ant-color-bg-container) !important;
  .header-left {
    display: flex;
    align-items: center;
    padding-left: 8px;
    gap: 8px;
  }

  .header-right {
    display: flex;
    align-items: center;
    padding-right: 8px;
  }
}
</style>
