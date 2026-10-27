<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/business/TaktMasterDetailTableLr -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：主子表左右布局；默认左右表体等高（共享 scroll.y，与 takt-single-table masterDetailLr 对齐） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    ref="rootRef"
    class="takt-master-detail-table-lr flex h-full min-h-0 flex-1 flex-row items-stretch gap-0"
    :style="rootScrollStyle"
  >
    <!-- 左：主表（结构与右栏对称：toolbar + __table-body + TaktSingleTable 内嵌分页） -->
    <div class="takt-master-detail-table-lr__pane master-pane flex h-full min-h-0 w-2/5 shrink-0 flex-col border-r border-border pr-3">
      <div
        ref="masterToolbarWrapRef"
        class="takt-master-detail-table-lr__chrome shrink-0"
      >
        <slot name="master-toolbar" />
      </div>
      <div
        ref="masterTableBodyRef"
        class="takt-master-detail-table-lr__table-body min-h-0 flex-1"
      >
        <TaktSingleTable
          class="h-full min-h-0"
          :columns="masterColumns"
          :data-source="masterDataSource"
          :loading="masterLoading"
          :row-key="masterRowKey"
          :row-selection="masterRowSelection"
          :show-row-selection="showMasterRowSelection"
          :entity-scope="masterEntityScope"
          :visible-column-keys="masterVisibleColumnKeys"
          :id-column-key="masterIdColumnKey"
          :action-column-key="masterActionColumnKey"
          :table-mode="masterTableMode"
          :stripe="masterStripe"
          :virtual="shouldUseMasterVirtual"
          :scroll-layout="masterScrollLayout"
          :scroll="masterScroll"
          :size="size"
          :bordered="bordered"
          :show-pagination="showMasterPagination"
          :current="masterCurrent"
          :page-size="masterPageSize"
          :total="masterTotal"
          :custom-row="masterCustomRow"
          :footer-remark="masterFooterRemark"
          @change="(p, f, s) => emit('master-change', p, f, s)"
          @pagination-change="handleMasterPaginationChange"
          @resize-column="(w, col) => emit('master-resize-column', w, col)"
        >
        <template
          v-for="name in masterForwardSlotNames"
          #[name]="slotData"
        >
          <slot
            :name="name"
            v-bind="slotData || {}"
          />
        </template>
        </TaktSingleTable>
      </div>
    </div>
    <!-- 右：从表（toolbar 放 detailChromeWrap；#detail 根节点须 h-full flex flex-col min-h-0 填满 __table-body） -->
    <div class="takt-master-detail-table-lr__pane detail-pane flex h-full min-h-0 flex-1 flex-col overflow-hidden pl-3">
      <div
        ref="detailChromeWrapRef"
        class="takt-master-detail-table-lr__chrome shrink-0"
      >
        <div
          v-if="detailTitle"
          class="mb-2 text-sm font-medium text-text"
        >
          {{ detailTitle }}
        </div>
        <slot name="detail-toolbar" />
      </div>
      <template v-if="$slots.detail">
        <div
          v-show="hasMasterSelection"
          ref="detailTableBodyRef"
          class="takt-master-detail-table-lr__table-body min-h-0 flex-1"
        >
          <slot name="detail" />
        </div>
        <div
          v-show="!hasMasterSelection"
          class="takt-master-detail-table-lr__table-body takt-master-detail-table-lr__detail-empty min-h-0 flex-1"
        >
          <slot name="detail-empty">
            <div class="flex h-full min-h-0 items-center justify-center text-sm text-text-secondary">
              {{ resolvedDetailEmptyDescription }}
            </div>
          </slot>
        </div>
      </template>
      <template v-else-if="hasMasterSelection">
        <div
          ref="detailTableBodyRef"
          class="takt-master-detail-table-lr__table-body min-h-0 flex-1"
        >
          <TaktSingleTable
            class="h-full min-h-0"
            :columns="detailColumns"
            :data-source="detailDataSource"
            :loading="detailLoading"
            :row-key="detailRowKey"
            :row-selection="detailRowSelection"
            :show-row-selection="showDetailRowSelection"
            :entity-scope="detailEntityScope"
            :visible-column-keys="detailVisibleColumnKeys"
            :id-column-key="detailIdColumnKey"
            :action-column-key="detailActionColumnKey"
            :table-mode="detailTableMode"
            :stripe="detailStripe"
            :virtual="shouldUseDetailVirtual"
            :scroll-layout="detailScrollLayout"
            :scroll="detailScroll"
            :size="size"
            :bordered="bordered"
            :show-pagination="false"
            :custom-row="detailCustomRow"
            :footer-remark="detailFooterRemark"
            @change="(p, f, s) => emit('detail-change', p, f, s)"
            @resize-column="(w, col) => emit('detail-resize-column', w, col)"
          >
          <template
            v-for="name in detailForwardSlotNames"
            #[name]="slotData"
          >
            <slot
              :name="detailSlotKey(name)"
              v-bind="slotData || {}"
            />
          </template>
          </TaktSingleTable>
        </div>
      </template>
      <div
        v-else
        class="takt-master-detail-table-lr__table-body takt-master-detail-table-lr__detail-empty min-h-0 flex-1"
      >
        <slot name="detail-empty">
          <div class="flex h-full min-h-0 items-center justify-center text-sm text-text-secondary">
            {{ resolvedDetailEmptyDescription }}
          </div>
        </slot>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 主子表左右布局（左主右从）；默认 provide 共享 scroll.y，子级 TaktSingleTable scrollLayout=masterDetailLr 自动 inject
 * @module components/business/TaktMasterDetailTableLr
 */
import { computed, ref, useSlots } from 'vue'
import { useI18n } from 'vue-i18n'
import type { TableColumnsType, TableProps } from 'ant-design-vue'
import type { FilterValue } from 'ant-design-vue/es/table/interface'
import type { TaktEntityScope, TaktTableLayoutMode } from '@/utils/tableColumns'
import { shouldUseTableVirtualScroll, type TaktTableScrollLayout } from '@/utils/tableScroll'
import { provideTaktMasterDetailLrScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { createLogger } from '@/utils/logger'

/** 表格行 */
type TableRecord = Record<string, unknown>
/** 分页摘要 */
type TablePagination = { current?: number; pageSize?: number; total?: number }
/** 排序摘要 */
type TableSorter = {
  field?: string | number | readonly (string | number)[]
  order?: string
}
/** 筛选摘要 */
type TableFilters = Record<string, FilterValue | null>

const masterDetailLrLogger = createLogger('takt-master-detail-table-lr')
const { t } = useI18n()
const slots = useSlots()

/** 保留给主表转发的插槽名（排除 layout / detail-*） */
const RESERVED_SLOT_NAMES = new Set(['master-toolbar', 'detail-toolbar', 'detail-empty'])

/** 组件 Props */
interface Props {
  masterColumns: TableColumnsType
  masterDataSource?: TableRecord[]
  masterLoading?: boolean
  masterRowKey?: string | ((record: TableRecord) => string)
  masterRowSelection?: TableProps['rowSelection']
  showMasterRowSelection?: boolean
  masterEntityScope?: TaktEntityScope
  masterVisibleColumnKeys?: string[]
  masterIdColumnKey?: string | number
  masterActionColumnKey?: string | number
  masterTableMode?: TaktTableLayoutMode
  masterStripe?: boolean
  /** 是否启用主表虚拟滚动；默认 true，行数/pageSize 超阈值时亦自动开启 */
  masterVirtual?: boolean
  masterScroll?: { x?: number | string | true; y?: number | string }
  /** 主表 scrollLayout；默认 masterDetailLr，与 provide 共享 scroll.y 配套 */
  masterScrollLayout?: TaktTableScrollLayout
  showMasterPagination?: boolean
  masterCurrent?: number
  masterPageSize?: number
  masterTotal?: number
  detailColumns?: TableColumnsType
  detailDataSource?: TableRecord[]
  detailLoading?: boolean
  detailRowKey?: string | ((record: TableRecord) => string)
  detailRowSelection?: TableProps['rowSelection']
  showDetailRowSelection?: boolean
  detailEntityScope?: TaktEntityScope
  detailVisibleColumnKeys?: string[]
  detailIdColumnKey?: string | number
  detailActionColumnKey?: string | number
  detailTableMode?: TaktTableLayoutMode
  detailStripe?: boolean
  /** 是否启用从表虚拟滚动；默认 true，行数超阈值时亦自动开启 */
  detailVirtual?: boolean
  detailScroll?: { x?: number | string | true; y?: number | string }
  /** 从表 scrollLayout；默认 masterDetailLr，与 provide 共享 scroll.y 配套 */
  detailScrollLayout?: TaktTableScrollLayout
  size?: TableProps['size']
  bordered?: boolean
  /** 当前选中主表 row-key */
  selecmedMasterKey?: string
  detailTitle?: string
  detailEmptyDescription?: string
  /** 主表表尾备注（合计下方；也可用 #footerRemark） */
  masterFooterRemark?: string
  /** 从表表尾备注（合计下方；也可用 #detail-footerRemark） */
  detailFooterRemark?: string
  /** 选中主表行后加载从表（不分页） */
  loadDetailData?: (masterRecord: TableRecord) => void | Promise<void>
}

const props = withDefaults(defineProps<Props>(), {
  masterDataSource: () => [],
  masterLoading: false,
  masterRowKey: 'id',
  showMasterRowSelection: true,
  masterEntityScope: 'company',
  masterVisibleColumnKeys: () => [],
  masterIdColumnKey: 'id',
  masterActionColumnKey: 'action',
  masterTableMode: 'masterDetailMaster',
  masterStripe: true,
  masterVirtual: true,
  masterScrollLayout: 'masterDetailLr',
  showMasterPagination: true,
  masterCurrent: 1,
  masterPageSize: 10,
  masterTotal: 0,
  detailColumns: () => [],
  detailDataSource: () => [],
  detailLoading: false,
  detailRowKey: 'id',
  showDetailRowSelection: true,
  detailEntityScope: 'company',
  detailVisibleColumnKeys: () => [],
  detailIdColumnKey: 'id',
  detailActionColumnKey: 'action',
  detailTableMode: 'masterDetailDetail',
  detailStripe: true,
  detailVirtual: true,
  detailScrollLayout: 'masterDetailLr',
  size: 'middle',
  bordered: false,
  selecmedMasterKey: '',
  detailTitle: '',
  detailEmptyDescription: '',
  masterFooterRemark: '',
  detailFooterRemark: '',
})

const emit = defineEmits<{
  'update:masterCurrent': [page: number]
  'update:masterPageSize': [pageSize: number]
  'update:selecmedMasterKey': [key: string]
  'master-change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter | TableSorter[]]
  'detail-change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter | TableSorter[]]
  'master-select': [record: TableRecord]
  'master-pagination-change': [page: number, pageSize: number]
  'master-resize-column': [width: number, column: Record<string, unknown>]
  'detail-resize-column': [width: number, column: Record<string, unknown>]
}>()

/** 主表转发插槽（非 detail-*） */
const masterForwardSlotNames = computed(() =>
  Object.keys(slots).filter(
    (name) => !RESERVED_SLOT_NAMES.has(name) && !name.startsWith('detail-'),
  ),
)

/** 从表 a-table 插槽名（去掉 detail- 前缀） */
const detailForwardSlotNames = computed(() =>
  Object.keys(slots)
    .filter((name) => name.startsWith('detail-'))
    .map((name) => name.slice('detail-'.length)),
)

/**
 * 从表插槽对外名称
 * @param innerName a-table 插槽名
 * @returns {string} 组件插槽名 detail-{inner}
 */
function detailSlotKey(innerName: string): string {
  return `detail-${innerName}`
}

/** 未选主表时的从表空态 */
const resolvedDetailEmptyDescription = computed(() => {
  if (props.detailEmptyDescription) {
    return props.detailEmptyDescription
  }
  return t('common.status.empty')
})

/** 是否已选中主表行 */
const hasMasterSelection = computed(() => !!props.selecmedMasterKey)

/** 组件根 DOM */
const rootRef = ref<HTMLElement | null>(null)

/** 左侧 master-toolbar 包裹层 */
const masterToolbarWrapRef = ref<HTMLElement | null>(null)

/** 右侧 detailTitle + detail-toolbar 包裹层 */
const detailChromeWrapRef = ref<HTMLElement | null>(null)

/** 主表 __table-body */
const masterTableBodyRef = ref<HTMLElement | null>(null)

/** 从表 __table-body（布局用，不参与 scroll.y 重算） */
const detailTableBodyRef = ref<HTMLElement | null>(null)

/** 左右共享 scroll.y（仅主表区测量；选中从表不改变主表高度） */
const sharedScrollYPx = provideTaktMasterDetailLrScrollY({
  rootRef,
  masterToolbarWrapRef,
  masterTableBodyRef,
  includeMasterPagination: computed(() => props.showMasterPagination),
})

/** 根节点 CSS 变量（与 takt-single-table --takt-table-scroll-y 语义对齐，便于调试） */
const rootScrollStyle = computed(() => ({
  '--takt-master-detail-lr-scroll-y': `${sharedScrollYPx.value}px`,
}))

/** 主表是否启用虚拟滚动（行数或 pageSize 超过 5000 强制开） */
const shouldUseMasterVirtual = computed(() => {
  const len = props.masterDataSource?.length ?? 0
  const pageSize = props.masterPageSize ?? 10
  return shouldUseTableVirtualScroll(Math.max(len, pageSize), props.masterVirtual)
})

/** 从表是否启用虚拟滚动（行数超过 5000 强制开） */
const shouldUseDetailVirtual = computed(() =>
  shouldUseTableVirtualScroll(props.detailDataSource?.length ?? 0, props.detailVirtual),
)

/**
 * 解析主表 row-key
 * @param record 行
 * @returns {string} row-key
 */
function resolveMasterRowKey(record: TableRecord): string {
  if (typeof props.masterRowKey === 'function') {
    return props.masterRowKey(record)
  }
  return String(record[props.masterRowKey] ?? '')
}

/**
 * 解析从表 row-key
 * @param record 行
 * @returns {string} row-key
 */
function resolveDetailRowKey(record: TableRecord): string {
  const rowKey = props.detailRowKey ?? 'id'
  if (typeof rowKey === 'function') {
    return rowKey(record)
  }
  return String(record[rowKey] ?? '')
}

/**
 * 从表行点击选中（与主表 masterCustomRow 对称，联动 detailRowSelection）
 * @param record 从表行
 * @returns {Record<string, unknown>} 行属性
 */
function detailCustomRow(record: TableRecord): Record<string, unknown> {
  const key = resolveDetailRowKey(record)
  const selecmedKeys = props.detailRowSelection?.selecmedRowKeys ?? []
  const isSelecmed = selecmedKeys.some((k) => String(k) === key)
  return {
    onClick: () => {
      const selection = props.detailRowSelection
      if (!selection?.onChange) {
        return
      }
      selection.onChange([key], [record])
    },
    class: isSelecmed ? 'takt-master-detail-table-row-selecmed cursor-pointer' : 'cursor-pointer',
  }
}

/**
 * 主表行点击选中并加载从表
 * @param record 主表行
 */
async function handleMasterSelect(record: TableRecord) {
  const key = resolveMasterRowKey(record)
  emit('update:selecmedMasterKey', key)
  emit('master-select', record)
  if (!props.loadDetailData) {
    return
  }
  try {
    await props.loadDetailData(record)
  } catch (error) {
    masterDetailLrLogger.error('加载从表数据失败', { action: 'loadDetailData' }, error)
  }
}

/**
 * 主表 custom-row：点击行选中
 * @param record 行
 * @returns {Record<string, unknown>} 行属性
 */
function masterCustomRow(record: TableRecord): Record<string, unknown> {
  const key = resolveMasterRowKey(record)
  const selecmed = key === props.selecmedMasterKey
  return {
    onClick: () => {
      void handleMasterSelect(record)
    },
    class: selecmed ? 'takt-master-detail-table-row-selecmed cursor-pointer' : 'cursor-pointer',
  }
}

/**
 * 主表分页变更
 * @param page 页码
 * @param pageSize 每页条数
 */
function handleMasterPaginationChange(page: number, pageSize: number) {
  emit('update:masterCurrent', page)
  emit('update:masterPageSize', pageSize)
  emit('master-pagination-change', page, pageSize)
}
</script>

<style scoped>
.takt-master-detail-table-lr {
  width: 100%;
  min-width: 0;
  overflow: hidden;
}

.takt-master-detail-table-lr__pane {
  height: 100%;
  min-height: 0;
}

.takt-master-detail-table-lr__table-body {
  display: flex;
  flex-direction: column;
  min-width: 0;
  overflow: hidden;
  flex: 1 1 auto;
  min-height: 0;
}

/** #detail 槽根节点默认撑满 __table-body（与左侧 TaktSingleTable h-full 对齐） */
.takt-master-detail-table-lr__table-body :deep(> *) {
  display: flex;
  flex-direction: column;
  height: 100%;
  min-height: 0;
  flex: 1 1 auto;
}

.takt-master-detail-table-lr__table-body :deep(.takt-single-table) {
  flex: 1 1 auto;
  min-height: 0;
  margin: 0;
  height: 100%;
}

.takt-master-detail-table-lr__table-body :deep(.ant-table-wrapper),
.takt-master-detail-table-lr__table-body :deep(.ant-table-container) {
  min-width: 0;
}

.takt-master-detail-table-lr__detail-empty {
  min-width: 0;
}
</style>
