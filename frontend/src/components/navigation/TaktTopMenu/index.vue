<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktTopMenu
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:顶部水平菜单组件

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-menu
    v-model:selected-keys="selectedKeys"
    mode="horizontal"
    :theme="theme"
    :items="menuItems"
    class="takt-top-menu"
    @click="handleMenuClick"
  />
</template>

<script setup lang="ts">
import type { MenuClickEventHandler } from 'ant-design-vue/es/menu/src/interface'
import { useThemeStore } from '@/stores/common/useThemeStore'
import { useMenuStore } from '@/stores/identity/useMenuStore'

interface Props {
  theme?: 'light' | 'dark'
}

const props = withDefaults(defineProps<Props>(), {
  theme: 'light'
})

const emit = defineEmits<{
  'click': [key: string]
}>()

const route = useRoute()
const router = useRouter()
const themeStore = useThemeStore()
const menuStore = useMenuStore()

const selectedKeys = ref<string[]>([])

const menuItems = computed(() => menuStore.menuItems ?? [])

const theme = computed(() => {
  if (props.theme === 'dark') return 'dark'
  return themeStore.resolvedTheme === 'dark' ? 'dark' : 'light'
})

const handleMenuClick: MenuClickEventHandler = (info) => {
  const key = String(info.key)
  router.push(key)
  emit('click', key)
}

watch(
  () => route.path,
  (path) => {
    selectedKeys.value = [path]
  },
  { immediate: true }
)
</script>

<style scoped>
.takt-top-menu {
  border-bottom: none;
  flex: 1;
}
</style>
