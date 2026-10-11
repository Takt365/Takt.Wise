<template>
  <div class="dashboard-workspace">
    <a-row
      ref="rowRef"
      :gutter="[16, 16]"
    >
      <a-col
        v-for="item in modules"
        :key="item.id"
        :data-id="item.id"
        :span="item.span ?? 24"
      >
        <a-dropdown trigger="contextmenu">
          <div
            class="workspace-module-context-target"
            @mouseenter="hoveredModuleId = item.id"
            @mouseleave="hoveredModuleId = null"
          >
            <WorkspaceModuleCard
              :module="item"
              :hovered="hoveredModuleId === item.id"
              @remove="removeModule(item.id)"
              @change-span="updateModuleSpan"
            >
              <template #headActions>
                <a-tooltip
                  v-if="item.moduleKey === 'shortcut'"
                  :title="t('dashboard.workspace.page.manageshortcuts')"
                  placement="right"
                >
                  <a-button
                    type="text"
                    class="takt-dashboard-module-card-head-btn"
                    @click="shortcutRefs[item.id]?.openManage?.()"
                  >
                    <template #icon>
                      <RiSettings3Line class="takt-dashboard-module-card__head-icon" />
                    </template>
                  </a-button>
                </a-tooltip>
              </template>
              <component
                :is="moduleComponents[item.moduleKey]"
                v-bind="getModuleProps(item)"
                :ref="(el: unknown) => setShortcutRef(item.id, item.moduleKey, el)"
              />
            </WorkspaceModuleCard>
          </div>
          <template #overlay>
            <a-menu @click="onModuleContextMenuClick(item, $event)">
              <a-menu-item key="add">
                <template #icon>
                  <RiAddLine />
                </template>
                {{ t('dashboard.workspace.page.addmodule') }}
              </a-menu-item>
              <a-menu-item
                v-if="item.moduleKey !== 'welcome' && item.moduleKey !== 'shortcut'"
                key="remove"
                danger
              >
                <template #icon>
                  <RiDeleteBinLine />
                </template>
                {{ t('dashboard.workspace.page.removemodule') }}
              </a-menu-item>
            </a-menu>
          </template>
        </a-dropdown>
      </a-col>
    </a-row>

    <a-modal
      v-model:open="showAddModal"
      :title="t('dashboard.workspace.page.addmodule')"
      :width="520"
      @ok="onAddModule"
    >
      <p class="workspace-add-tip">
        {{ t('dashboard.workspace.page.selectmoduletype') }}
      </p>
      <a-checkbox-group v-model:value="addingKeys">
        <a-row :gutter="[8, 8]">
          <a-col
            v-for="m in WORKSPACE_AVAILABLE_MODULES"
            :key="m.key"
            :span="8"
          >
            <a-checkbox
              :value="m.key"
              :disabled="m.key !== 'custom' && addedModuleKeys.has(m.key)"
            >
              {{ t(m.titleKey) }}
            </a-checkbox>
          </a-col>
        </a-row>
      </a-checkbox-group>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { RiAddLine, RiDeleteBinLine, RiSettings3Line } from '@remixicon/vue'
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import Sortable from 'sortablejs'
import WorkspaceModuleCard from './components/WorkspaceModuleCard.vue'
import WelcomeModule from './modules/WelcomeModule.vue'
import ShortcutModule from './modules/ShortcutModule.vue'
import TodoModule from './modules/TodoModule.vue'
import NoticeModule from './modules/NoticeModule.vue'
import CustomModule from './modules/CustomModule.vue'
import type { WorkspaceModuleItem, WorkspaceModuleKey } from '@/types/dashboard/workspace.types'
import {
  WORKSPACE_STORAGE_KEY,
  WORKSPACE_AVAILABLE_MODULES,
  getDefaultWorkspaceModules,
  generateModuleId
} from './config'

const { t } = useI18n()

const moduleComponents: Record<WorkspaceModuleKey, unknown> = {
  welcome: markRaw(WelcomeModule),
  shortcut: markRaw(ShortcutModule),
  todo: markRaw(TodoModule),
  Announcement: markRaw(NoticeModule),
  custom: markRaw(CustomModule)
}

function loadModules(): WorkspaceModuleItem[] {
  try {
    const raw = localStorage.getItem(WORKSPACE_STORAGE_KEY)
    if (raw) {
      const parsed = JSON.parse(raw) as WorkspaceModuleItem[]
      if (Array.isArray(parsed) && parsed.length > 0) return parsed
    }
  } catch {
    // ignore
  }
  return getDefaultWorkspaceModules()
}

function saveModules(list: WorkspaceModuleItem[]) {
  localStorage.setItem(WORKSPACE_STORAGE_KEY, JSON.stringify(list))
}

const modules = ref<WorkspaceModuleItem[]>(loadModules())
/** 当前悬停的模块 Id（控制卡片头按钮显隐） */
const hoveredModuleId = ref<string | null>(null)

const rowRef = ref<{ $el: HTMLElement } | null>(null)
let sortableInstance: Sortable | null = null

const shortcutRefs = ref<Record<string, { openManage: () => void }>>({})

function setShortcutRef(id: string, moduleKey: WorkspaceModuleKey, el: unknown) {
  if (moduleKey !== 'shortcut') return
  if (el && typeof el === 'object' && 'openManage' in el && typeof (el as { openManage?: unknown }).openManage === 'function') {
    shortcutRefs.value[id] = el as { openManage: () => void }
    return
  }
  else delete shortcutRefs.value[id]
}

const showAddModal = ref(false)
/** 勾选要添加的模块类型（多选 + 已添加的禁用） */
const addingKeys = ref<WorkspaceModuleKey[]>([])

/** 已添加的 moduleKey 集合（custom 允许多个，其余每种仅允许一个） */
const addedModuleKeys = computed(() => new Set(modules.value.map(m => m.moduleKey)))

function openAddModal() {
  showAddModal.value = true
  addingKeys.value = []
}

/** 按拖拽后的 DOM 顺序重排 modules 并持久化 */
function reorderModulesByDomOrder(container: HTMLElement) {
  const ids = [...container.children]
    .map(el => el.getAttribute('data-id'))
    .filter((id): id is string => !!id)
  const idToItem = new Map(modules.value.map(m => [m.id, m]))
  const ordered = ids.map(id => idToItem.get(id)).filter((m): m is WorkspaceModuleItem => !!m)
  if (ordered.length === modules.value.length) {
    modules.value = ordered
    saveModules(modules.value)
  }
}

onMounted(() => {
  nextTick(() => {
    const el = rowRef.value?.$el
    if (!el || !(el instanceof HTMLElement)) return
    sortableInstance = Sortable.create(el, {
      handle: '.takt-dashboard-module-card-drag-handle',
      animation: 150,
      ghostClass: 'workspace-module-card-ghost',
      onEnd(evt: Sortable.SortableEvent) {
        reorderModulesByDomOrder(evt.from)
      }
    })
  })
})

onBeforeUnmount(() => {
  sortableInstance?.destroy()
})

function getModuleProps(item: WorkspaceModuleItem) {
  if (item.moduleKey === 'custom') {
    return { customTitle: item.customTitle, customContent: item.customContent }
  }
  return {}
}

function addModule(key: WorkspaceModuleKey) {
  if (key !== 'custom' && addedModuleKeys.value.has(key)) return
  const meta = WORKSPACE_AVAILABLE_MODULES.find(m => m.key === key)
  const span = meta?.defaultSpan ?? 24
  const newItem: WorkspaceModuleItem = {
    id: generateModuleId(key),
    moduleKey: key,
    span
  }
  if (key === 'custom') {
    newItem.customTitle = t('dashboard.workspace.page.modules.custom')
    newItem.customContent = ''
  }
  modules.value = [...modules.value, newItem]
  saveModules(modules.value)
  message.success(t('dashboard.workspace.page.addsuccess'))
}

function removeModule(id: string) {
  modules.value = modules.value.filter(m => m.id !== id)
  saveModules(modules.value)
  message.success(t('dashboard.workspace.page.removesuccess'))
}

/** 切换模块占位：独占一行(24) 或 半行/一行两列(12) */
function updateModuleSpan(id: string, span: number) {
  const item = modules.value.find(m => m.id === id)
  if (item && (span === 24 || span === 12)) {
    item.span = span
    saveModules(modules.value)
  }
}

function onAddModule() {
  const added = addedModuleKeys.value
  for (const key of addingKeys.value) {
    if (key !== 'custom' && added.has(key)) continue
    addModule(key)
  }
  showAddModal.value = false
  addingKeys.value = []
}

function onModuleContextMenuClick(
  item: WorkspaceModuleItem,
  ev: MenuInfo
) {
  if (ev.key === 'add') openAddModal()
  else if (ev.key === 'remove') removeModule(item.id)
}
</script>

<style scoped lang="css">
.dashboard-workspace {
  padding: 24px 0;
}
.dashboard-workspace :deep(.ant-row) {
  align-items: stretch;
}
.dashboard-workspace :deep(.ant-col) {
  display: flex;
  flex-direction: column;
}
.workspace-module-context-target {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-height: 160px;
  height: 100%;
  width: 100%;
}
.workspace-add-tip {
  margin-bottom: 8px;
  color: var(--ant-color-text-secondary);
  font-size: 12px;
}
:deep(.workspace-module-card-ghost) {
  opacity: 0.5;
  background: var(--ant-color-fill-quaternary);
}
</style>