<template>
  <div class="dashboard-data-board">
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
            class="databoard-module-context-target"
            @mouseenter="hoveredModuleId = item.id"
            @mouseleave="hoveredModuleId = null"
          >
            <DataBoardModuleCard
              :module="item"
              :hovered="hoveredModuleId === item.id"
              @remove="removeModule(item.id)"
              @change-span="updateModuleSpan"
            >
              <component :is="moduleComponents[item.moduleKey]" />
            </DataBoardModuleCard>
          </div>
          <template #overlay>
            <a-menu @click="onModuleContextMenuClick(item, $event)">
              <a-menu-item key="add">
                <template #icon>
                  <RiAddLine />
                </template>
                {{ t('dashboard.data-board.page.addmodule') }}
              </a-menu-item>
              <a-menu-item
                key="remove"
                danger
              >
                <template #icon>
                  <RiDeleteBinLine />
                </template>
                {{ t('dashboard.data-board.page.removemodule') }}
              </a-menu-item>
            </a-menu>
          </template>
        </a-dropdown>
      </a-col>
    </a-row>

    <a-modal
      v-model:open="showAddModal"
      :title="t('dashboard.data-board.page.addmodule')"
      :width="520"
      @ok="onAddModule"
    >
      <p class="databoard-add-tip">
        {{ t('dashboard.data-board.page.selectmoduletype') }}
      </p>
      <a-checkbox-group v-model:value="addingKeys">
        <a-row :gutter="[8, 8]">
          <a-col
            v-for="m in DATA_BOARD_AVAILABLE_MODULES"
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
import { RiAddLine, RiDeleteBinLine } from '@remixicon/vue'
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import Sortable from 'sortablejs'
import DataBoardModuleCard from './components/DataBoardModuleCard.vue'
import StatsOverviewModule from './modules/StatsOverviewModule.vue'
import StatsChangeModule from './modules/StatsChangeModule.vue'
import StatsOnlineModule from './modules/StatsOnlineModule.vue'
import StatsSalesInvoiceModule from './modules/StatsSalesInvoiceModule.vue'
import StatsSalesOrderModule from './modules/StatsSalesOrderModule.vue'
import StatsAssyOphModule from './modules/StatsAssyOphModule.vue'
import StatsPcbaOphModule from './modules/StatsPcbaOphModule.vue'
import StatsAssyDefectModule from './modules/StatsAssyDefectModule.vue'
import StatsPcbaModule from './modules/StatsPcbaModule.vue'
import StatsQualityCostModule from './modules/StatsQualityCostModule.vue'
import StatsQualityOperationModule from './modules/StatsQualityOperationModule.vue'
import StatsQualityComplaintModule from './modules/StatsQualityComplaintModule.vue'
import StatsHelpDeskModule from './modules/StatsHelpDeskModule.vue'
import StatsMeetingNoticeModule from './modules/StatsMeetingNoticeModule.vue'
import StatsCustomerServiceModule from './modules/StatsCustomerServiceModule.vue'
import StatsPurchaseInvoiceModule from './modules/StatsPurchaseInvoiceModule.vue'
import StatsPurchaseOrderModule from './modules/StatsPurchaseOrderModule.vue'
import StatsPurchaseRequestModule from './modules/StatsPurchaseRequestModule.vue'
import StatsInventoryModule from './modules/StatsInventoryModule.vue'
import StatsCustomModule from './modules/StatsCustomModule.vue'
import type { DataBoardModuleItem, DataBoardModuleKey } from '@/types/dashboard/dataBoard.types'
import {
  DATA_BOARD_STORAGE_KEY,
  DATA_BOARD_AVAILABLE_MODULES,
  getDefaultDataBoardModules,
  generateModuleId
} from './config'

const { t } = useI18n()

const moduleComponents: Record<DataBoardModuleKey, unknown> = {
  overview: markRaw(StatsOverviewModule),
  change: markRaw(StatsChangeModule),
  online: markRaw(StatsOnlineModule),
  salesInvoice: markRaw(StatsSalesInvoiceModule),
  salesOrder: markRaw(StatsSalesOrderModule),
  assyOph: markRaw(StatsAssyOphModule),
  pcbaOph: markRaw(StatsPcbaOphModule),
  assyDefect: markRaw(StatsAssyDefectModule),
  pcba: markRaw(StatsPcbaModule),
  qualityCost: markRaw(StatsQualityCostModule),
  qualityOperation: markRaw(StatsQualityOperationModule),
  qualityComplaint: markRaw(StatsQualityComplaintModule),
  helpDesk: markRaw(StatsHelpDeskModule),
  meetingNotice: markRaw(StatsMeetingNoticeModule),
  customerService: markRaw(StatsCustomerServiceModule),
  purchaseInvoice: markRaw(StatsPurchaseInvoiceModule),
  purchaseOrder: markRaw(StatsPurchaseOrderModule),
  purchaseRequest: markRaw(StatsPurchaseRequestModule),
  inventory: markRaw(StatsInventoryModule),
  custom: markRaw(StatsCustomModule)
}

function loadModules(): DataBoardModuleItem[] {
  try {
    const raw = localStorage.getItem(DATA_BOARD_STORAGE_KEY)
    if (raw) {
      const parsed = JSON.parse(raw) as Array<DataBoardModuleItem & { moduleKey: string }>
      if (Array.isArray(parsed) && parsed.length > 0) {
        const validKeys = new Set(Object.keys(moduleComponents))
        const migrated = parsed.map((item) => {
          if (item.moduleKey === 'pcbaInspection' || item.moduleKey === 'pcbaRepair') {
            return { ...item, moduleKey: 'pcba' as DataBoardModuleKey }
          }
          return item as DataBoardModuleItem
        })
        const deduped: DataBoardModuleItem[] = []
        let hasPcba = false
        for (const item of migrated) {
          if (!validKeys.has(item.moduleKey as string)) {
            continue
          }
          if (item.moduleKey === 'pcba') {
            if (hasPcba) {
              continue
            }
            hasPcba = true
          }
          deduped.push(item)
        }
        return deduped
      }
    }
  } catch {
    // ignore
  }
  return getDefaultDataBoardModules()
}

function saveModules(list: DataBoardModuleItem[]) {
  localStorage.setItem(DATA_BOARD_STORAGE_KEY, JSON.stringify(list))
}

const modules = ref<DataBoardModuleItem[]>(loadModules())
/** 当前悬停的模块 Id（控制卡片头按钮显隐） */
const hoveredModuleId = ref<string | null>(null)

const rowRef = ref<{ $el: HTMLElement } | null>(null)
let sortableInstance: Sortable | null = null

const showAddModal = ref(false)
const addingKeys = ref<DataBoardModuleKey[]>([])

const addedModuleKeys = computed(() => new Set(modules.value.map(m => m.moduleKey)))

function openAddModal() {
  showAddModal.value = true
  addingKeys.value = []
}

function reorderModulesByDomOrder(container: HTMLElement) {
  const ids = [...container.children]
    .map(el => el.getAttribute('data-id'))
    .filter((id): id is string => !!id)
  const idToItem = new Map(modules.value.map(m => [m.id, m]))
  const ordered = ids.map(id => idToItem.get(id)).filter((m): m is DataBoardModuleItem => !!m)
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
      ghostClass: 'databoard-module-card-ghost',
      onEnd(evt: Sortable.SortableEvent) {
        reorderModulesByDomOrder(evt.from)
      }
    })
  })
})

onBeforeUnmount(() => {
  sortableInstance?.destroy()
})

function addModule(key: DataBoardModuleKey) {
  if (key !== 'custom' && addedModuleKeys.value.has(key)) return
  const meta = DATA_BOARD_AVAILABLE_MODULES.find(m => m.key === key)
  const span = meta?.defaultSpan ?? 24
  const newItem: DataBoardModuleItem = {
    id: generateModuleId(key),
    moduleKey: key,
    span
  }
  modules.value = [...modules.value, newItem]
  saveModules(modules.value)
  message.success(t('dashboard.data-board.page.addsuccess'))
}

function removeModule(id: string) {
  modules.value = modules.value.filter(m => m.id !== id)
  saveModules(modules.value)
  message.success(t('dashboard.data-board.page.removesuccess'))
}

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

function onModuleContextMenuClick(item: DataBoardModuleItem, ev: MenuInfo) {
  if (ev.key === 'add') openAddModal()
  else if (ev.key === 'remove') removeModule(item.id)
}
</script>

<style scoped lang="css">
.dashboard-data-board {
  padding: 24px 0;
}
.dashboard-data-board :deep(.ant-row) {
  align-items: stretch;
}
.dashboard-data-board :deep(.ant-col) {
  display: flex;
  flex-direction: column;
}
.databoard-module-context-target {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-height: 160px;
  height: 100%;
  width: 100%;
}
.databoard-add-tip {
  margin-bottom: 8px;
  color: var(--ant-color-text-secondary);
  font-size: 12px;
}
:deep(.databoard-module-card-ghost) {
  opacity: 0.5;
  background: var(--ant-color-fill-quaternary);
}
</style>
