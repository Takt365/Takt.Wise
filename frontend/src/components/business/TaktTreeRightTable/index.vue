<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktTreeRightTable
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:右表区域：外框与左树同高撑满父级；表体 scroll.y 按容器扣除表头实测；无分页

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->

<template>
  <div class="takt-tree-right-table">
    <div
      ref="bodyRef"
      class="takt-tree-right-table__body"
      :class="{ 'takt-tree-right-table__body--fixed-y': hasFixedScrollY }"
      :style="tableBodyStyle"
    >
      <a-table
        class="ant-table-striped"
        table-layout="fixed"
        :columns="resolvedDisplayColumns"
        :data-source="tableDataSource"
        :loading="loading"
        :pagination="false"
        :row-key="rowKey"
        :row-class-name="rowClassName"
        :virtual="shouldUseVirtual"
        :size="size"
        :bordered="bordered"
        v-bind="{ ...$attrs, ...(effectiveRowSelection ? { 'row-selection': effectiveRowSelection } : {}) }"
        :scroll="scrollConfig"
        :locale="tableLocale"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="slotData">
          <div
            v-if="isTreeExpandAnchorColumn(slotData.column)"
            class="flex min-w-0 items-center"
          >
            <span
              class="inline-flex shrink-0 items-center"
              :style="{ paddingLeft: `${Number(slotData.record?._treeDepth ?? 0) * indentSize}px` }"
            >
              <button
                v-if="slotData.record?._hasChildren"
                type="button"
                class="inline-flex h-6 w-6 shrink-0 cursor-pointer items-center justify-center border-0 bg-transparent p-0 text-text-secondary"
                :title="slotData.record?._treeExpanded ? t('common.page.button.collapse') : t('common.page.button.expand')"
                @click.stop="handleTreeExpandClick(slotData.record)"
              >
                <RiArrowDownSLine
                  v-if="slotData.record?._treeExpanded"
                  class="takt-remix-icon"
                  :size="16"
                />
                <RiArrowRightSLine
                  v-else
                  class="takt-remix-icon"
                  :size="16"
                />
              </button>
              <span
                v-else
                class="inline-block h-6 w-6 shrink-0"
              />
            </span>
            <div class="min-w-0 flex-1">
              <div
                v-if="isTableColumnEllipsisEnabled(slotData.column)"
                :class="TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS"
                :title="resolveTableCellEllipsisTitle(slotData.text)"
              >
                <slot
                  name="bodyCell"
                  v-bind="slotData"
                >
                  <TaktTableBodyCellFallback :slot-data="slotData" />
                </slot>
              </div>
              <slot
                v-else
                name="bodyCell"
                v-bind="slotData"
              >
                <TaktTableBodyCellFallback :slot-data="slotData" />
              </slot>
            </div>
          </div>
          <div
            v-else-if="isTableColumnEllipsisEnabled(slotData.column)"
            :class="TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS"
            :title="resolveTableCellEllipsisTitle(slotData.text)"
          >
            <slot
              name="bodyCell"
              v-bind="slotData"
            >
              <TaktTableBodyCellFallback :slot-data="slotData" />
            </slot>
          </div>
          <slot
            v-else
            name="bodyCell"
            v-bind="slotData"
          >
            <TaktTableBodyCellFallback :slot-data="slotData" />
          </slot>
        </template>
        <template
          v-for="name in passthroughSlotNames"
          #[name]="slotData"
        >
          <slot
            :name="name"
            v-bind="slotData"
          />
        </template>
        <template
          v-if="$slots.summary"
          #summary
        >
          <slot name="summary" />
        </template>
      </a-table>
    </div>
    <div
      v-if="showFooterRemark"
      class="takt-tree-right-table__footer-remark shrink-0 px-1 pt-2 text-sm leading-relaxed text-text-secondary"
    >
      <slot name="footerRemark">
        {{ footerRemark }}
      </slot>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { TableColumnsType, TableProps, TablePaginationConfig } from 'ant-design-vue'
import type { ColumnType, SorterResult, FilterValue, TableCurrentDataSource } from 'ant-design-vue/es/table/interface'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
  type TaktEntityScope,
  type TaktTableLayoutMode,
} from '@/utils/tableColumns'
import {
  isTableColumnEllipsisEnabled,
  resolveTableCellEllipsisTitle,
  resolveTableScrollConfig,
  resolveVerticalScrollY,
  shouldUseTableVirtualScroll,
  TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS,
  type TaktTableScrollLayout,
} from '@/utils/tableScroll'
import TaktTableBodyCellFallback from '@/components/business/TaktTableBodyCellFallback/index'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { useTaktFillHeightScrollY } from '@/composables/useTaktFillHeightScrollY'
import { useTableColumnResize } from '@/composables/useTableColumnResize'
import {
  flattenExpandedTaktTreeTableRows,
  hasTaktTreeTableChildren,
} from '@/utils/taktTreeTable'
import { RiArrowDownSLine, RiArrowRightSLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

type TableRecord = Record<string, unknown>
type TableSorter = { field?: string | number | readonly (string | number)[]; order?: string }
type TableFilters = Record<string, unknown>
type TablePagination = { current?: number; pageSize?: number; total?: number }
type ResizableColumn = { width?: number | string }

interface Props {
  /** 业务列配置（不含实体基座字段，由 entityScope 自动合并） */
  columns: TableColumnsType
  /** 实体基类作用域（tenant/company/approval，必填） */
  entityScope: TaktEntityScope
  /** 是否合并审计字段列 */
  includeAuditFields?: boolean
  /** 可见列键（列设置抽屉）；空数组时按 tableMode 默认显隐 */
  visibleColumnKeys?: string[]
  /** ID 列键（默认 id） */
  idColumnKey?: string | number
  /** 操作列键（默认 action） */
  actionColumnKey?: string | number
  /** 左树右表默认 4 个业务列（默认 tree） */
  tableMode?: TaktTableLayoutMode
  /** 数据源（树表传入带 children 的根；组件内按展开路径拍平后虚拟渲染） */
  dataSource?: TableRecord[]
  /** 加载状态 */
  loading?: boolean
  /** 行键 */
  rowKey?: string | ((record: TableRecord) => string)
  /** 自定义行类名(斑马纹等) */
  rowClassName?: string | ((record: TableRecord, index: number) => string)
  /** 是否启用斑马纹 */
  stripe?: boolean
  /** 是否启用虚拟滚动 */
  virtual?: boolean
  /** 滚动配置（仅覆盖 x 或显式 y；未传 y 时由 scrollLayout 决定默认高度） */
  scroll?: { x?: number | string | true; y?: number | string }
  /** 表格布局场景（默认 treeRight，与左树右表页面对齐） */
  scrollLayout?: TaktTableScrollLayout
  /** 表格尺寸 */
  size?: TableProps['size']
  /** 是否显示边框 */
  bordered?: boolean
  /** 行选择配置 */
  rowSelection?: TableProps['rowSelection']
  /** 是否默认显示行选择列（默认 true） */
  showRowSelection?: boolean
  /** 展开的行 key 列表（树表展开用，v-model:expanded-row-keys；须与 row-key 一致） */
  expandedRowKeys?: (string | number)[]
  /** 树表缩进像素（有 children 时生效，默认 16） */
  indentSize?: number
  /** 是否默认省略 */
  defaultEllipsis?: boolean
  /** 表尾备注说明（合计行 / 表体下方） */
  footerRemark?: string
  /**
   * 懒加载子节点（展开时调用）。未传则只切换 expandedRowKeys（全量树已带 children）。
   */
  loadChildren?: (record: TableRecord) => Promise<void>
}

const props = withDefaults(defineProps<Props>(), {
  dataSource: () => [],
  loading: false,
  rowKey: 'id',
  stripe: true,
  virtual: true,
  scroll: undefined,
  scrollLayout: 'treeRight',
  size: 'middle',
  bordered: false,
  showRowSelection: true,
  expandedRowKeys: () => [],
  defaultEllipsis: true,
  includeAuditFields: true,
  visibleColumnKeys: () => [],
  idColumnKey: 'id',
  actionColumnKey: 'action',
  tableMode: 'tree',
  footerRemark: '',
  indentSize: 16,
  loadChildren: undefined,
})

const emit = defineEmits<{
  'change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter]
  'resize-column': [width: number, column: ResizableColumn]
  'update:expandedRowKeys': [keys: (string | number)[]]
}>()

const { t } = useI18n()
const slots = useSlots()

/** 表体容器 DOM（撑满父级后实测 scroll.y） */
const bodyRef = ref<HTMLElement | null>(null)

/** 本组件承接、不向 a-table 透传的插槽 */
const OWNED_SLOT_NAMES = new Set(['bodyCell', 'summary', 'footerRemark'])

/** 透传给 a-table 的插槽名 */
const passthroughSlotNames = computed(() =>
  Object.keys(slots).filter((name) => !OWNED_SLOT_NAMES.has(name)),
)

/** 是否展示表尾备注 */
const showFooterRemark = computed(
  () => !!props.footerRemark?.trim() || !!slots.footerRemark,
)

/** 空数据占位（scroll.y 固定布局高度，与行数无关） */
const tableLocale = computed(() => ({
  emptyText: t('common.status.empty'),
}))

/**
 * 解析行 key（与 a-table row-key 一致）
 * @param record 行数据
 * @returns {string} 行 key
 */
function resolveRowKey(record: TableRecord): string {
  const rk = props.rowKey
  if (typeof rk === 'function') {
    return String(rk(record) ?? '')
  }
  if (typeof rk === 'string' && rk.length > 0) {
    const value = record[rk]
    return value == null ? '' : String(value)
  }
  return record.id == null ? '' : String(record.id)
}

/**
 * 取列配置中第一个叶子列的 key（树表展开图标落在此列）
 * @param columns 列配置
 * @returns {string | number | undefined} 列 key
 */
function getFirstLeafColumnKey(columns: TableColumnsType): string | number | undefined {
  if (!columns?.length) return undefined
  for (const col of columns) {
    if (col == null || typeof col !== 'object') continue
    const nested = (col as { children?: TableColumnsType }).children
    if (Array.isArray(nested) && nested.length > 0) {
      const childKey = getFirstLeafColumnKey(nested)
      if (childKey !== undefined) return childKey
      continue
    }
    const key =
      (col as { key?: string | number; dataIndex?: string | number }).key
      ?? (col as { dataIndex?: string | number }).dataIndex
    if (key !== undefined && key !== null && String(key) !== '') return key
  }
  return undefined
}

/** 传入 dataSource 已带 children 时走虚拟树表拍平 */
const isTreeDataSource = computed(() => hasTaktTreeTableChildren(props.dataSource))

/** 绑定到 a-table 的行：树表为「已展开路径」拍平结果（无 children），否则原列表 */
const tableDataSource = computed(() => {
  if (!isTreeDataSource.value) return props.dataSource ?? []
  return flattenExpandedTaktTreeTableRows(
    props.dataSource,
    props.expandedRowKeys ?? [],
    resolveRowKey,
  )
})

/**
 * 是否在该列绘制树表展开按钮
 * @param column bodyCell 列
 * @returns {boolean} 是否为首数据列
 */
function isTreeExpandAnchorColumn(column: { key?: string | number; dataIndex?: string | number } | undefined): boolean {
  if (!isTreeDataSource.value || column == null) return false
  const first = firstDataColumnKey.value
  if (first === undefined) return false
  return String(column.key ?? column.dataIndex ?? '') === String(first)
}

/**
 * 切换树行展开：收起只改 keys；展开时若有 loadChildren 则先拉一层子节点，再拍平 virtual 渲染
 * @param record 当前行
 * @returns {Promise<void>}
 */
async function handleTreeExpandClick(record: TableRecord) {
  if (!record?._hasChildren) return
  const key = resolveRowKey(record)
  if (!key) return
  const next = new Set((props.expandedRowKeys ?? []).map((k) => String(k)))
  if (next.has(key)) {
    next.delete(key)
    emit('update:expandedRowKeys', Array.from(next))
    return
  }
  if (props.loadChildren) {
    await props.loadChildren(record)
  }
  next.add(key)
  emit('update:expandedRowKeys', Array.from(next))
}

/** 是否启用虚拟滚动：展开后拍平行一律 virtual，只渲染视口内行 */
const shouldUseVirtual = computed(() => shouldUseTableVirtualScroll(1, true))

/** 行选择：默认显示选择列（showRowSelection 为 true 且未传 rowSelection 时用空对象） */
const effectiveRowSelection = computed(() => {
  if (!props.showRowSelection) return undefined
  return props.rowSelection !== undefined && props.rowSelection !== null ? props.rowSelection : {}
})

const rowClassName = computed(() => {
  if (props.rowClassName) return props.rowClassName
  if (props.stripe) {
    return (_record: TableRecord, index: number) => (index % 2 === 1 ? 'table-striped' : '')
  }
  return ''
})

const userColumns = computed((): TableColumnsType => normalizeUserTableColumns(props.columns))

const mergedColumns = computed(() =>
  mergeDefaultColumns(
    userColumns.value,
    t,
    props.includeAuditFields,
    props.entityScope,
    props.idColumnKey,
  ),
)

const displayColumnSource = computed((): TableColumnsType => {
  const keys = props.visibleColumnKeys ?? []
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(mergedColumns.value, keys, mergedColumns.value)
  }
  return filterMergedColumnsByDefaultVisible(mergedColumns.value, userColumns.value, {
    idColumnKey: props.idColumnKey,
    actionColumnKey: props.actionColumnKey,
    tableMode: props.tableMode,
    entityScope: props.entityScope,
  })
})

/** 列宽拖拽：稳定列引用，拖动时就地改 width（不重建列数组） */
const {
  displayColumns: resolvedDisplayColumns,
  rebuildDisplayColumns,
  handleResizeColumn: applyResizeColumnWidth,
} = useTableColumnResize()

/** 首列 key：展开/缩进画在该列 */
const firstDataColumnKey = computed(() => getFirstLeafColumnKey(resolvedDisplayColumns.value))

watch(
  [displayColumnSource, () => props.defaultEllipsis],
  ([source, defaultEllipsis]) => {
    rebuildDisplayColumns(source, defaultEllipsis)
  },
  { immediate: true },
)

/** 窗口回退 scroll.y（容器尚未布局时） */
const viewportScrollYPx = useTaktTableViewportScrollY(computed(() => props.scrollLayout))

/** 填满父级后扣除表头的表体高度（与左树外框同高） */
const fillHeightScrollYPx = useTaktFillHeightScrollY(bodyRef, {
  fallbackPx: viewportScrollYPx,
  subtractTableHeader: true,
  recalcToken: computed(() => [
    props.loading,
    resolvedDisplayColumns.value.length,
    showFooterRemark.value,
  ]),
})

const scrollConfig = computed(() =>
  resolveTableScrollConfig({
    columns: resolvedDisplayColumns.value,
    scroll: props.scroll,
    includeRowSelection: effectiveRowSelection.value != null,
    enableVerticalScroll: true,
    scrollLayout: props.scrollLayout,
    verticalScrollHeight: resolveVerticalScrollY(props.scroll?.y, fillHeightScrollYPx.value),
  }),
)

/** 是否已配置固定纵向滚动高度 */
const hasFixedScrollY = computed(() => {
  const y = scrollConfig.value.y
  return y != null && y !== ''
})

/**
 * 表格体 CSS 变量（兜底固定高度，与 scroll.y 一致）
 * @returns 绑定到 __body 的 style
 */
const tableBodyStyle = computed(() => {
  const y = scrollConfig.value.y
  if (y == null || y === '') {
    return undefined
  }
  const px = typeof y === 'number' ? `${y}px` : String(y)
  return { '--takt-table-scroll-y': px } as Record<string, string>
})

const handleTableChange = (pagination: TablePaginationConfig, filters: Record<string, FilterValue | null>, sorter: SorterResult<any> | SorterResult<any>[], _extra: TableCurrentDataSource<any>) => {
  const finalSorter = Array.isArray(sorter) ? sorter[0] : sorter
  emit('change',
    { current: pagination.current, pageSize: pagination.pageSize, total: pagination.total } as TablePagination,
    filters as TableFilters,
    (finalSorter || {}) as TableSorter
  )
}

/**
 * a-table 列宽拖拽：就地改宽并通知父级
 * @param w 新宽度
 * @param col 当前列
 */
const handleResizeColumn = (w: number, col: ColumnType<any>) => {
  if (!applyResizeColumnWidth(w, col)) {
    return
  }
  emit('resize-column', w, col as any)
}

</script>

<style scoped>
.takt-tree-right-table {
  flex: 1;
  min-width: 0;
  margin: 0;
  width: auto;
  max-width: none;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
  align-self: stretch;
  box-sizing: border-box;
}

.takt-tree-right-table__body {
  flex: 1;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
  height: 100%;
}

.takt-tree-right-table__body :deep(.ant-table-wrapper) {
  width: 100%;
  min-width: 0;
  height: 100%;
}

.takt-tree-right-table__body :deep(.ant-table) {
  height: 100%;
}

.takt-tree-right-table__body :deep(.ant-table-container) {
  min-width: 0;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-container) {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

/** scroll.y 兜底：空数据/少行时亦保持固定表体高度（不随数据撑开） */
.takt-tree-right-table__body--fixed-y :deep(.ant-table-header) {
  overflow-y: scroll !important;
  scrollbar-gutter: stable;
  scrollbar-width: none;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-header::-webkit-scrollbar) {
  width: 0;
  height: 0;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-body) {
  min-height: var(--takt-table-scroll-y);
  max-height: var(--takt-table-scroll-y);
  overflow-x: auto !important;
  overflow-y: scroll !important;
  scrollbar-gutter: stable;
  flex: 1 1 auto;
  min-width: 0;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-tbody-virtual-scrollbar-horizontal) {
  display: block !important;
  visibility: visible !important;
  opacity: 1 !important;
  height: 10px !important;
  bottom: 0 !important;
  z-index: 10;
  pointer-events: auto !important;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-tbody-virtual-scrollbar-horizontal .ant-table-tbody-virtual-scrollbar-thumb) {
  height: 8px !important;
  border-radius: 4px;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-tbody-virtual-holder) {
  min-width: 0;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-placeholder) {
  min-height: calc(var(--takt-table-scroll-y) - 8px);
}
</style>
