<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktMixMenu
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:混合菜单组件,支持内联和折叠模式

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-menu
    v-model:selecmed-keys="selecmedKeys"
    v-model:open-keys="openKeys"
    mode="inline"
    :theme="theme"
    :items="menuItems"
    :inline-collapsed="collapsed"
    :inline-indent="TAKT_MENU_INLINE_INDENT"
    :accordion="setting.menuAccordion"
    :class="['takt-mix-menu', menuStyleClass]"
    @click="handleMenuClick"
    @openChange="handleOpenChange"
  />
</template>

<script setup lang="ts">
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import { useThemeStore } from '@/stores/common/useThemeStore'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import { useSettingStore } from '@/stores/common/useSettingStore'
import { TAKT_MENU_INLINE_INDENT } from '@/utils/common'
import {
  buildMenuParentKeyMap,
  normalizeMenuOpenKeys,
  resolveMenuOpenKeysForPath,
} from '@/utils/taktMenuOpenKeys'

interface Props {
  collapsed?: boolean
  theme?: 'light' | 'dark'
}

const props = withDefaults(defineProps<Props>(), {
  collapsed: false,
  theme: 'light'
})

const emit = defineEmits<{
  'click': [key: string]
}>()

const route = useRoute()
const router = useRouter()
const themeStore = useThemeStore()
const menuStore = useMenuStore()
const { setting } = storeToRefs(useSettingStore())
/** 菜单风格 class（plain / rounded） */
const menuStyleClass = computed(() => {
  const style = setting.value.menuStyle === 'rounded' ? 'rounded' : 'plain'
  return `menu-style-${style}`
})

const selecmedKeys = ref<string[]>([])
const openKeys = ref<string[]>([])

const menuItems = computed(() => menuStore.menuItems ?? [])

/** 菜单 key → 父 key（收起父级时级联清理子孙 openKeys） */
const menuParentKeyMap = computed(() => buildMenuParentKeyMap(menuItems.value))

const theme = computed(() => {
  if (props.theme === 'dark') return 'dark'
  return themeStore.resolvedTheme === 'dark' ? 'dark' : 'light'
})

const handleMenuClick = (info: MenuInfo) => {
  const key = String(info.key)
  router.push(key)
  emit('click', key)
}

/**
 * SubMenu 展开/收拢：收起父级时级联移除子孙 openKeys
 * @param keys 变更后的 openKeys
 */
const handleOpenChange = (keys: string[]) => {
  openKeys.value = normalizeMenuOpenKeys(keys, menuParentKeyMap.value, setting.value.menuAccordion)
}

watch(
  () => [route.path, menuItems.value] as const,
  ([path]) => {
    selecmedKeys.value = [path]
    openKeys.value = resolveMenuOpenKeysForPath(menuItems.value, path)
  },
  { immediate: true }
)
</script>

<style scoped>
.takt-mix-menu {
  border-right: 0;
  /* 朴素/圆润风格见 menu-base.css html[data-menu-style] */
}
</style>
