
<template>
  <div class="shortcut-module">
    <a-row :gutter="[16, 16]">
      <a-col
        v-for="item in shortcuts"
        :key="item.path"
        :span="3"
      >
        <a-tooltip
          :title="item.title"
          placement="top"
        >
          <a
            class="shortcut-item"
            @click="item.path && go(item.path)"
          >
            <span
              v-if="item.icon"
              class="shortcut-icon"
              :style="item.iconColor ? { color: item.iconColor } : undefined"
            ><component :is="item.icon" :class="[TAKT_REMIX_ICON_CLASS, 'takt-remix-icon-xl']" /></span>
          </a>
        </a-tooltip>
      </a-col>
    </a-row>
    <a-modal
      v-model:open="manageVisible"
      :title="t('dashboard.workspace.page.manageshortcuts')"
      :width="520"
      @ok="handleManageOk"
      @cancel="handleManageCancel"
    >
      <p class="shortcut-tip">
        {{ t('dashboard.workspace.page.shortcuttip') }}
      </p>
      <a-checkbox-group v-model:value="editingPaths">
        <a-row :gutter="[8, 8]">
          <a-col
            v-for="item in availableShortcuts"
            :key="item.path || ''"
            :span="6"
          >
            <a-checkbox
              :value="item.path || ''"
              :disabled="editingPaths.length >= 16 && !editingPaths.includes(item.path || '')"
            >
              {{ item.title }}
            </a-checkbox>
          </a-col>
        </a-row>
      </a-checkbox-group>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import { useWorkspaceShortcutStore } from '@/stores/dashboard/useWorkspaceShortcutStore'
import { TAKT_REMIX_ICON_CLASS } from '@/utils/common'
import { resolveThemeColorCssVar } from '@/utils/theme'

const router = useRouter()
const { t } = useI18n()
const menuStore = useMenuStore()
const shortcutStore = useWorkspaceShortcutStore()

const availableShortcuts = computed(() => menuStore.leafMenus)

function getColorForPath(path: string): string {
  let n = 0
  const safePath = path || ''
  for (let i = 0; i < safePath.length; i++) n = (n * 31 + safePath.charCodeAt(i)) >>> 0
  return resolveThemeColorCssVar(n)
}

watch(
  availableShortcuts,
  list => {
    if (list.length > 0) {
      const paths = list.map(x => x.path).filter((p): p is string => p !== undefined)
      shortcutStore.initDefaultsIfEmpty(paths)
    }
  },
  { immediate: true }
)

const shortcuts = computed(() => {
  const list = availableShortcuts.value
  const selecmed = shortcutStore.selecmedPaths
  return list
    .filter(x => x.path && selecmed.includes(x.path))
    .slice(0, 16)
    .map(x => ({
      path: x.path,
      title: x.title || '',
      icon: x.icon,
      iconColor: getColorForPath(x.path)
    }))
})

const manageVisible = ref(false)
const editingPaths = ref<string[]>([])

function openManage() {
  editingPaths.value = [...shortcutStore.selecmedPaths]
  manageVisible.value = true
}

function handleManageOk() {
  const unique = Array.from(new Set(editingPaths.value))
  const finalList = unique.slice(0, 16)
  shortcutStore.setPaths(finalList)
  manageVisible.value = false
}

function handleManageCancel() {
  manageVisible.value = false
}

function go(path: string) {
  router.push(path)
}

defineExpose({ openManage })
</script>

<style scoped lang="css">
.shortcut-module {
  padding: 16px 0;
  min-height: 140px;
}
.shortcut-item {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 52px;
  height: 52px;
  padding: 0;
  border-radius: 8px;
  color: var(--ant-color-text);
  text-decoration: none;
  transition: background-color 0.2s, color 0.2s, transform 0.2s ease;
  cursor: pointer;
  user-select: none;
  border: 1px solid transparent;
  perspective: 420px;
  &:hover {
    background: var(--ant-color-fill-tertiary);
    color: var(--ant-color-primary);
    .shortcut-icon {
      color: var(--ant-color-primary) !important;
    }
  }
  &:active {
    background: var(--ant-color-fill-secondary);
    transform: scale(0.98);
  }
  &:focus-visible {
    outline: 2px solid var(--ant-color-primary);
    outline-offset: 2px;
  }
  .shortcut-icon {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    transform: rotateY(0deg);
    transition: color 0.2s ease, transform 0.3s ease;
    will-change: transform;
  }
  &:hover .shortcut-icon {
    transform: rotateY(180deg);
  }
  &:active .shortcut-icon {
    transform: rotateY(180deg) scale(0.95);
  }
}
.shortcut-tip {
  margin-bottom: 8px;
  color: var(--ant-color-text-secondary);
  font-size: 12px;
}
</style>