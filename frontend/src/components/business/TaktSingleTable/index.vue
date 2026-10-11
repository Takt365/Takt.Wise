<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktSingleTable
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:单表格组件,支持虚拟滚动、列宽调整、排序、筛选、列溢出省略；始终设置 scroll.y（布局高度，与数据有无无关）

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div
    class="takt-single-table"
    :class="[rootExtraClass, analysisTable ? 'takt-analysis-table' : undefined]"
  >
    <div
      ref="tableBodyRef"
      class="takt-single-table__body"
      :class="{ 'takt-single-table__body--fixed-y': hasFixedScrollY }"
      :style="tableBodyStyle"
    >
      <a-table
        class="ant-table-striped"
        table-layout="fixed"
        :columns="analysisDisplayColumns"
        :data-source="dataSource"
        :loading="loading"
        :pagination="false"
        :row-key="rowKey"
        :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : '')"
        :virtual="shouldUseVirtual"
        :size="size"
        :bordered="bordered"
        v-bind="{
          ...tablePassthroughAttrs,
          ...(effectiveRowSelection ? { 'row-selection': effectiveRowSelection } : {})
        }"
        :custom-row="customRow"
        :scroll="scrollConfig"
        :locale="tableLocale"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
        @expand="(expanded, record) => emit('expand', expanded, record)"
      >
        <template #bodyCell="slotData">
          <template v-if="String(slotData.column?.key) === 'approvalStatus' && props.entityScope === 'approval'">
            <TaktDictTag
              dict-type="sys_approval_status"
              :value="slotData.record?.approvalStatus as string | number | undefined"
            />
          </template>
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
          v-for="(_, name) in passthroughSlotNames"
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
      ref="footerRemarkRef"
      class="takt-single-table__footer-remark shrink-0 px-1 pt-2 text-sm leading-relaxed text-text-secondary"
    >
      <slot name="footerRemark">
        {{ footerRemark }}
      </slot>
    </div>
    <TaktPagination
      v-if="showPagination"
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
  </div>
</template>

<script setup lang="ts">
defineOptions({ inheritAttrs: false })

import type { TableColumnsType, TableProps } from 'ant-design-vue'
import type {
  ColumnType,
  FilterValue,
  SorterResult,
  TableCurrentDataSource,
  TablePaginationConfig
} from 'ant-design-vue/es/table/interface'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
  type TaktEntityScope,
  type TaktTableLayoutMode,
} from '@/utils/tableColumns'
import {
  applyAnalysisTableColumnAlign,
  isTableColumnEllipsisEnabled,
  resolveTableCellEllipsisTitle,
  resolveTableScrollConfig,
  resolveVerticalScrollY,
  shouldUseTableVirtualScroll,
  TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS,
  TAKT_TABLE_SCROLL_Y_MIN,
  type TaktTableScrollLayout,
} from '@/utils/tableScroll'
import TaktTableBodyCellFallback from '@/components/business/TaktTableBodyCellFallback/index'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { useTaktMasterDetailLrScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { useTableColumnResize } from '@/composables/useTableColumnResize'
import { useI18n } from 'vue-i18n'
import { useAttrs, computed, nextTick, onBeforeUnmount, onMounted, ref, useSlots, watch } from 'vue'

type TableRecord = Record<string, unknown>
type TableSorter = {
  field?: string | number | readonly (string | number)[]
  order?: string
}
type TableFilters = Record<string, FilterValue | null>
type TablePagination = { current?: number; pageSize?: number; total?: number }
type ResizableColumn = { width?: string | number } & Record<string, unknown>

interface Props {
  /** 业务列配置（不含实体基座字段，由 entityScope 自动合并） */
  columns: TableColumnsType
  /** 数据源 */
  dataSource?: TableRecord[]
  /** 加载状态 */
  loading?: boolean
  /** 行键 */
  rowKey?: string | ((record: TableRecord) => string)
  /** 自定义行类名 */
  rowClassName?: string | ((record: TableRecord, index: number) => string) | undefined
  /** 是否启用斑马纹 */
  stripe?: boolean
  /**
   * 虚拟滚动（大数据默认开启）。true 强制开；false 仅在行数未超过阈值时关闭；超阈值一律自动开启
   * @see TAKT_TABLE_AUTO_VIRTUAL_ROW_THRESHOLD
   */
  virtual?: boolean
  /** 滚动配置（仅覆盖 x 或显式 y；未传 y 时 innerHeight - 固定 header/footer 与页壳 300px） */
  scroll?: { x?: number | string | true; y?: number | string } | undefined
  /**
   * 表格布局场景（决定视口预留与 scroll.y 计算策略）
   * page：整页列表；pageWithInnerTabs：整页 + 内容区内嵌 a-tabs；treeLeft/treeRight：左树右表；masterDetailLr/Tb*：主子表；editable：表单内嵌可编辑表
   */
  scrollLayout?: TaktTableScrollLayout
  /** 表格尺寸 */
  size?: TableProps['size']
  /** 是否显示边框 */
  bordered?: boolean
  /** 行选择配置 */
  rowSelection?: TableProps['rowSelection'] | undefined
  /** 自定义行属性（点击选中等，对应 a-table customRow） */
  customRow?: TableProps['customRow']
  /** 是否默认显示行选择列 */
  showRowSelection?: boolean
  /** 是否默认启用列文本省略 */
  defaultEllipsis?: boolean
  /** 是否显示底部分页 */
  showPagination?: boolean
  /** 当前页码 */
  current?: number
  /** 每页条数 */
  pageSize?: number
  /** 总条数 */
  total?: number
  /**
   * 实体基类作用域（与 DTO 继承 TenantDtoBase / CompanyDtoBase / ApprovalDtoBase 对齐，必填）
   * tenant → TaktTenantEntityBase；company → TaktCompanyEntityBase；approval → TaktApprovalEntityBase
   */
  entityScope: TaktEntityScope
  /** 是否合并审计字段列 */
  includeAuditFields?: boolean
  /** 可见列键（列设置抽屉）；空数组时仅展示业务列 columns */
  visibleColumnKeys?: string[]
  /** ID 列键（默认 id） */
  idColumnKey?: string | number
  /** 操作列键（默认 action） */
  actionColumnKey?: string | number
  /** 单表 8 个业务列 / 树表 4 个 / 主子表左 2 个 / 主子表右 4 个（默认 single） */
  tableMode?: TaktTableLayoutMode
  /** @deprecated 请使用 tableMode；保留兼容旧页，不再从 merge 列截取 */
  largeScreenColumnCount?: number
  /** @deprecated 请使用 tableMode */
  smallScreenColumnCount?: number
  /** @deprecated 请使用 tableMode */
  largeScreenBreakpoint?: number
  /**
   * 表尾备注说明（渲染在合计行 / 表体下方、分页上方）
   * 复杂多行内容请用 #footerRemark 插槽
   */
  footerRemark?: string
  /**
   * 分析/推移表：表头居中；表体仍按列 align（文本左、数值右）
   * 样式见 styles/table-analysis-base.css
   */
  analysisTable?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  dataSource: () => [],
  loading: false,
  rowKey: 'id',
  rowClassName: undefined,
  stripe: true,
  virtual: true,
  scroll: undefined,
  scrollLayout: 'page',
  size: 'middle',
  bordered: false,
  rowSelection: undefined,
  customRow: undefined,
  showRowSelection: true,
  defaultEllipsis: true,
  showPagination: false,
  current: 1,
  pageSize: 20,
  total: 0,
  includeAuditFields: true,
  visibleColumnKeys: () => [],
  idColumnKey: 'id',
  actionColumnKey: 'action',
  tableMode: 'single',
  smallScreenColumnCount: 5,
  largeScreenBreakpoint: 1200,
  footerRemark: '',
  analysisTable: false,
})

const emit = defineEmits<{
  'change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter | TableSorter[]]
  'resize-column': [width: number, column: ResizableColumn]
  'update:current': [page: number]
  'update:pageSize': [size: number]
  'pagination-change': [page: number, pageSize: number]
  'expand': [expanded: boolean, record: TableRecord]
}>()

const { t } = useI18n()
const attrs = useAttrs()
const slots = useSlots()

/** 不向 a-table 透传的插槽（由本组件承接） */
const OWNED_SLOT_NAMES = new Set(['bodyCell', 'summary', 'footerRemark'])

/** 透传给 a-table 的插槽名（bodyCell 由组件内统一处理 approvalStatus 字典展示） */
const passthroughSlotNames = computed(() =>
  Object.keys(slots).filter((name) => !OWNED_SLOT_NAMES.has(name)),
)

/** 是否展示表尾备注（prop 或插槽） */
const showFooterRemark = computed(
  () => !!props.footerRemark?.trim() || !!slots.footerRemark,
)

/** 表尾备注 DOM（用于从 scroll.y 扣减高度） */
const footerRemarkRef = ref<HTMLElement | null>(null)

/** 表尾备注占用高度（px） */
const footerRemarkHeightPx = ref(0)

/** 表尾备注高度观测 */
let footerRemarkResizeObserver: ResizeObserver | null = null

/**
 * 测量表尾备注高度，避免与显式 scroll.y 叠高溢出
 */
function measureFooterRemarkHeight(): void {
  const el = footerRemarkRef.value
  footerRemarkHeightPx.value = el ? Math.ceil(el.getBoundingClientRect().height) : 0
}

watch(showFooterRemark, async (visible) => {
  if (!visible) {
    footerRemarkHeightPx.value = 0
    return
  }
  await nextTick()
  measureFooterRemarkHeight()
})

onMounted(() => {
  if (typeof ResizeObserver === 'undefined') {
    return
  }
  footerRemarkResizeObserver = new ResizeObserver(() => {
    measureFooterRemarkHeight()
  })
  watch(
    footerRemarkRef,
    (el, prev) => {
      if (prev) {
        footerRemarkResizeObserver?.unobserve(prev)
      }
      if (el) {
        footerRemarkResizeObserver?.observe(el)
        measureFooterRemarkHeight()
      }
    },
    { immediate: true },
  )
})

onBeforeUnmount(() => {
  footerRemarkResizeObserver?.disconnect()
  footerRemarkResizeObserver = null
})

/** 页面传入的 class 挂到根节点（inheritAttrs: false 时不会自动合并） */
const rootExtraClass = computed(() => attrs.class)

/** 透传给 a-table 的属性（排除 class/style/customRow，避免破坏 scroll 布局或重复绑定） */
const tablePassthroughAttrs = computed(() => {
  const { class: _class, style: _style, customRow: _customRow, ...rest } = attrs
  return rest
})

/** 空数据占位（高度由 scroll.y 固定，文案居中显示） */
const tableLocale = computed(() => ({
  emptyText: t('common.status.empty'),
}))

/** 是否启用虚拟滚动：显式 true，或行数超过 TAKT_TABLE_AUTO_VIRTUAL_ROW_THRESHOLD（5000） */
const shouldUseVirtual = computed(() =>
  shouldUseTableVirtualScroll(props.dataSource?.length ?? 0, props.virtual),
)

const currentPage = computed({
  get: () => props.current ?? 1,
  set: (val) => emit('update:current', val),
})

const pageSize = computed({
  get: () => props.pageSize ?? 20,
  set: (val) => emit('update:pageSize', val),
})

/** 行选择:默认显示选择列 */
const effectiveRowSelection = computed(() => {
  if (!props.showRowSelection) return undefined
  return props.rowSelection !== undefined && props.rowSelection !== null ? props.rowSelection : {}
})

/** 页面业务列（解包 Ref/ComputedRef） */
const userColumns = computed((): TableColumnsType => normalizeUserTableColumns(props.columns))

/** 合并实体基座字段后的完整列 */
const mergedColumns = computed((): TableColumnsType =>
  mergeDefaultColumns(
    userColumns.value,
    t,
    props.includeAuditFields,
    props.entityScope,
    props.idColumnKey,
  ),
)

/** 列设置过滤前的展示列源 */
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

watch(
  [displayColumnSource, () => props.defaultEllipsis],
  ([source, defaultEllipsis]) => {
    rebuildDisplayColumns(source, defaultEllipsis)
  },
  { immediate: true },
)

/**
 * 分析表：表头居中、表体按 align 写 inline（覆盖 Ant Design 默认左对齐）
 */
const analysisDisplayColumns = computed((): TableColumnsType => {
  if (!props.analysisTable) {
    return resolvedDisplayColumns.value
  }
  return applyAnalysisTableColumnAlign(resolvedDisplayColumns.value)
})

/**
 * a-table 列宽拖拽：就地改宽并通知父级
 * @param w 新宽度
 * @param col 当前列
 */
const handleResizeColumn = (w: number, col: ColumnType<unknown>) => {
  const mutableCol = col as ResizableColumn & { resizable?: boolean }
  if (!applyResizeColumnWidth(w, mutableCol)) {
    return
  }
  emit('resize-column', w, mutableCol)
}

/** 表格体 DOM（masterDetailLr 布局内由父级统一测量 scroll.y） */
const tableBodyRef = ref<HTMLElement | null>(null)

/** 左右主子表：使用 takt-master-detail-table-lr provide 的共享 scroll.y */
const masterDetailLrScrollYPx = useTaktMasterDetailLrScrollY()

/** 视口动态 scroll.y（px）；与 dataSource 行数无关，空表亦保持同一高度 */
const viewportScrollYPx = useTaktTableViewportScrollY(computed(() => props.scrollLayout))

const scrollConfig = computed(() => {
  const resolved = resolveTableScrollConfig({
    columns: analysisDisplayColumns.value,
    scroll: props.scroll,
    includeRowSelection: effectiveRowSelection.value != null,
    enableVerticalScroll: true,
    scrollLayout: props.scrollLayout,
    verticalScrollHeight:
      props.scroll?.y != null && props.scroll.y !== ''
        ? props.scroll.y
        : props.scrollLayout === 'masterDetailLr' && masterDetailLrScrollYPx != null
          ? masterDetailLrScrollYPx.value
          : resolveVerticalScrollY(undefined, viewportScrollYPx.value),
  })
  const y = resolved.y
  const footerH = footerRemarkHeightPx.value
  if (y == null || y === '' || footerH <= 0) {
    return resolved
  }
  const yNum = typeof y === 'number' ? y : Number.parseFloat(String(y))
  if (!Number.isFinite(yNum)) {
    return resolved
  }
  return {
    ...resolved,
    y: Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor(yNum - footerH)),
  }
})

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

/**
 * 分页页码变更
 * @param page 页码
 * @param size 每页条数
 */
const handlePaginationChange = (page: number, size: number) => {
  emit('update:current', page)
  emit('update:pageSize', size)
  emit('pagination-change', page, size)
}

/**
 * 分页每页条数变更
 * @param _page 当前页
 * @param size 每页条数
 */
const handlePaginationSizeChange = (_page: number, size: number) => {
  emit('update:current', 1)
  emit('update:pageSize', size)
  emit('pagination-change', 1, size)
}

const handleTableChange = (
  pagination: TablePaginationConfig,
  filters: TableFilters,
  sorter: SorterResult<TableRecord> | SorterResult<TableRecord>[],
  _extra: TableCurrentDataSource<TableRecord>,
) => {
  emit('change', pagination as TablePagination, filters, sorter as TableSorter | TableSorter[])
}

defineExpose({
  mergedColumns,
})
</script>

<style scoped>
.takt-single-table {
  margin: 0 4px 4px 4px;
  width: 100%;
  min-width: 0;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 0;
  flex: 1 1 auto;
}

.takt-single-table.h-full {
  height: 100%;
}

.takt-single-table__body {
  flex: 1 1 auto;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
  height: 100%;
}

.takt-single-table__body :deep(.ant-table-wrapper) {
  width: 100%;
  max-width: 100%;
  min-width: 0;
}

.takt-single-table__body :deep(.ant-table-container) {
  min-width: 0;
  max-width: 100%;
}

/** scroll.y 兜底：空数据/少行时亦保持固定表体高度（不随数据撑开） */
.takt-single-table__body--fixed-y :deep(.ant-table-container) {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

/**
 * 表头预留与表体相同的纵向滚动条槽位，避免横向滚动时列宽错位
 * （勿在 .ant-table-content 上另开 overflow-x，否则破坏 Ant Design scroll.x 同步）
 */
.takt-single-table__body--fixed-y :deep(.ant-table-header) {
  overflow-y: scroll !important;
  scrollbar-gutter: stable;
  scrollbar-width: none;
}

.takt-single-table__body--fixed-y :deep(.ant-table-header::-webkit-scrollbar) {
  width: 0;
  height: 0;
}

.takt-single-table__body--fixed-y :deep(.ant-table-body) {
  min-height: var(--takt-table-scroll-y);
  max-height: var(--takt-table-scroll-y);
  overflow-x: auto !important;
  overflow-y: scroll !important;
  scrollbar-gutter: stable;
  flex: 1 1 auto;
  min-width: 0;
}

/**
 * 虚拟表格横向滚动条（Ant Design Vue virtual）：勿被 overflow:hidden 裁切
 */
.takt-single-table__body--fixed-y :deep(.ant-table-tbody-virtual-scrollbar-horizontal) {
  display: block !important;
  visibility: visible !important;
  opacity: 1 !important;
  height: 10px !important;
  bottom: 0 !important;
  z-index: 10;
  pointer-events: auto !important;
}

.takt-single-table__body--fixed-y :deep(.ant-table-tbody-virtual-scrollbar-horizontal .ant-table-tbody-virtual-scrollbar-thumb) {
  height: 8px !important;
  border-radius: 4px;
}

.takt-single-table__body--fixed-y :deep(.ant-table-tbody-virtual-holder) {
  min-width: 0;
}

.takt-single-table__body--fixed-y :deep(.ant-table-summary) {
  flex-shrink: 0;
}

.takt-single-table__body--fixed-y :deep(.ant-table-placeholder) {
  min-height: calc(var(--takt-table-scroll-y) - 8px);
}
</style>
