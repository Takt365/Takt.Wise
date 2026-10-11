<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/business/TaktEditableTable -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：主子表表单内嵌可编辑子表；增删行、行校验、汇总、Excel 风格键盘单元格导航；defineExpose validate/getRows -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    ref="tableRootRef"
    class="takt-editable-table flex flex-col min-h-0"
    :class="sectionBorder ? 'pt-3 mt-3 border-t border-border' : undefined"
  >
    <div
      v-if="showToolbar"
      class="mb-2 flex items-center justify-between gap-2"
    >
      <span
        v-if="title"
        class="text-sm font-medium text-text"
      >{{ title }}</span>
      <slot name="toolbar" />
      <a-button
        v-if="showAdd"
        class="takt-button-create-row"
        :disabled="disabled || loading"
        @click="handleAddRow"
      >
        <template #icon>
          <RiInsertRowBottom class="takt-remix-icon" />
        </template>
        {{ resolvedAddButtonLabel }}
      </a-button>
    </div>
    <a-table
      :columns="tableColumns"
      :data-source="innerRows"
      :pagination="false"
      :scroll="scrollConfig"
      table-layout="fixed"
      :row-key="rowKeyResolver"
      :size="size"
      :bordered="bordered"
      :loading="loading"
      :locale="tableLocale"
      :virtual="shouldUseVirtual"
      :custom-row="resolveCustomRow"
    >
      <template #emptyText>
        <div class="py-8 text-center text-sm text-text-secondary">
          <slot name="empty">
            {{ resolvedEmptyDescription }}
          </slot>
        </div>
      </template>
      <template
        v-if="hasSummaryRow"
        #summary
      >
        <slot
          name="summary"
          :summary-map="summaryValueMap"
          :rows="innerRows"
        >
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </slot>
      </template>
      <template #bodyCell="{ column, record, index }">
        <template v-if="column.key === ACTION_COLUMN_KEY">
          <template v-if="obsoleteField">
            <a-button
              v-if="!isRowObsolete(record)"
              type="link"
              size="small"
              class="takt-button-void-row px-0"
              :disabled="disabled || loading"
              @click="handleMarkRowObsolete(index)"
            >
              {{ t('common.page.button.void') }}
            </a-button>
            <a-button
              v-else
              type="link"
              size="small"
              class="takt-button-revoke-row px-0"
              :disabled="disabled || loading"
              @click="handleRevokeRowObsolete(index)"
            >
              {{ t('common.page.button.revoke') }}
            </a-button>
          </template>
          <a-tooltip
            v-else
            :title="t('common.page.button.deleterow')"
          >
            <a-button
              class="takt-button-delete-row takt-button-plain-borderless takt-button-plain-icon-only"
              :disabled="disabled || loading"
              @click="handleRemoveRow(index)"
            >
              <template #icon>
                <RiDeleteRow class="takt-remix-icon" />
              </template>
            </a-button>
          </a-tooltip>
        </template>
        <template v-else-if="hasCellSlot(String(column.key))">
          <div
            class="takt-editable-table-cell"
            v-bind="resolveCellNavAttrs(index, String(column.key))"
          >
            <slot
              :name="`cell-${String(column.key)}`"
              :record="record"
              :index="index"
              :column="resolveColumn(String(column.key))"
              :error="getCellError(record, String(column.key))"
              :cell-nav="resolveCellNavAttrs(index, String(column.key))"
            />
            <span
              v-if="getCellError(record, String(column.key))"
              class="text-xs text-red-500"
            >{{ getCellError(record, String(column.key)) }}</span>
          </div>
        </template>
        <template v-else>
          <div
            class="takt-editable-table-cell gap-0.5"
            v-bind="resolveCellNavAttrs(index, String(column.key))"
          >
            <template v-if="isReadonlyColumn(String(column.key)) || isRowObsolete(record)">
              <span class="text-sm">{{ record[resolveDataIndex(String(column.key))] }}</span>
            </template>
            <template v-else-if="resolveEditor(String(column.key)) === 'inputNumber'">
              <a-input-number
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveInputNumberProps(String(column.key), record)"
                :status="resolveCellStatus(record, String(column.key))"
                @update:value="(val) => handleEditorValueChange(record, String(column.key), val)"
              />
            </template>
            <template v-else-if="resolveEditor(String(column.key)) === 'textarea'">
              <a-textarea
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveTextareaProps(String(column.key), record)"
                :status="resolveCellStatus(record, String(column.key))"
                @change="(val) => handleEditorValueChange(record, String(column.key), val)"
              />
            </template>
            <template v-else-if="resolveEditor(String(column.key)) === 'datePicker'">
              <a-date-picker
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveDatePickerProps(String(column.key), record)"
                :status="resolveCellStatus(record, String(column.key))"
                @change="(val) => handleEditorValueChange(record, String(column.key), val)"
              />
            </template>
            <template v-else>
              <a-input
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveInputProps(String(column.key), record)"
                :status="resolveCellStatus(record, String(column.key))"
                @change="(val) => handleEditorValueChange(record, String(column.key), val)"
              />
            </template>
            <span
              v-if="getCellError(record, String(column.key))"
              class="text-xs text-red-500"
            >{{ getCellError(record, String(column.key)) }}</span>
          </div>
        </template>
      </template>
    </a-table>
    <div
      v-if="showFooterRemark"
      class="takt-editable-table__footer-remark shrink-0 pt-2 text-sm leading-relaxed text-text-secondary"
    >
      <slot name="footerRemark">
        {{ footerRemark }}
      </slot>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 主子表表单内嵌可编辑子表
 * @module components/business/TaktEditableTable
 */
import { computed, h, nextTick, ref, useSlots, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { Tooltip } from 'ant-design-vue'
import { RiDeleteRow, RiInsertRowBottom, RiQuestionLine } from '@remixicon/vue'
import type { TableColumnsType } from 'ant-design-vue'
import {
  computeEditableSummaryMap,
  formatSummaryValue,
  mapEditableErrorsToCells,
  validateEditableRows,
  type TaktEditableValidateError,
} from './editableTableUtils'
import {
  resolveTableScrollConfig,
  resolveVerticalScrollY,
  shouldUseTableVirtualScroll,
  type TaktTableScrollLayout,
} from '@/utils/tableScroll'
import { resolveTableSummaryLabelColumnKey } from '@/utils/tableColumns'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import {
  TAKT_EDITABLE_ROW_KEY,
  attachEditableRowKeys,
  createEditableRowKey,
  detachEditableRowKeys,
  type TaktEditableEditorType,
  type TaktEditableRow,
  type TaktEditableTableColumn,
  filterActiveEditableRows,
  isEditableRowObsolete,
} from './types'
import {
  buildEditableCellNavAttrs,
  buildEditableNavColumnKeys,
  focusEditableTableCell,
  resolveNextEditableCell,
  shouldNavigateHorizontalOnArrowKey,
  shouldNavigateOnEnterKey,
  shouldNavigateVerticalOnArrowKey,
  type TaktEditableNavDirection,
} from './editableTableNav'

/** 操作列 key（内置） */
const ACTION_COLUMN_KEY = '__action'

/** i18n 翻译函数 */
const { t } = useI18n()

/**
 * 构建表头标题（可选问号提示、必填红 *）
 * @param title 列标题
 * @param titleHint 提示文案
 * @param required 是否必填
 */
function buildColumnTitle(title: string, titleHint?: string, required?: boolean) {
  const labelBody = titleHint
    ? () =>
        h('span', { class: 'inline-flex items-center gap-1 align-middle' }, [
          h(
            Tooltip,
            { title: titleHint, placement: 'top' },
            {
              default: () =>
                h('span', { class: 'takt-form-label-hint-icon inline-flex cursor-help' }, [
                  h(RiQuestionLine, { class: 'takt-remix-icon' }),
                ]),
            },
          ),
          h('span', null, title),
        ])
    : title
  if (!required) {
    return labelBody
  }
  if (typeof labelBody === 'function') {
    return () =>
      h('span', { class: 'ant-form-item-required inline-flex items-center gap-1 align-middle' }, [
        labelBody(),
      ])
  }
  return h('span', { class: 'ant-form-item-required' }, title)
}

/** 组件 Props */
interface Props {
  /** 子表行（可含或不含 __rowKey） */
  modelValue?: TaktEditableRow[]
  /** 列配置（不含操作列，由 showDelete 自动追加） */
  columns: TaktEditableTableColumn[]
  /** 区块标题 */
  title?: string
  /** 新增按钮文案；省略时用 addButtonEntity 或 title */
  addButtonLabel?: string
  /** 新增按钮实体后缀（拼在 common.page.button.create 后） */
  addButtonEntity?: string
  /** 是否显示工具栏新增按钮 */
  showAdd?: boolean
  /** 是否显示行内删除列 */
  showDelete?: boolean
  /** 是否显示工具栏（标题 + 新增） */
  showToolbar?: boolean
  /** 滚动配置（仅覆盖 x 或显式 y；未传 y 时由 scrollLayout 决定默认高度） */
  scroll?: { x?: number | string | true; y?: number | string }
  /** 表格布局场景（默认 editable） */
  scrollLayout?: TaktTableScrollLayout
  /** 是否启用表体纵向 scroll.y（弹窗内嵌子表可关，与外层表单共用滚动条） */
  enableVerticalScroll?: boolean
  /** @deprecated 请用 scroll.y；显式覆盖纵向滚动高度（px） */
  scrollY?: number
  /** 表格 bordered */
  bordered?: boolean
  /** 表格尺寸 */
  size?: 'small' | 'middle' | 'large'
  /** 表格 loading */
  loading?: boolean
  /** 禁用编辑与增删 */
  disabled?: boolean
  /** 持久化主键字段（用于 row-key 与 attach） */
  idField?: string
  /** 新增行默认字段工厂 */
  defaultRow?: () => Record<string, unknown>
  /** 顶部 border-t 分区（表单上主下从） */
  sectionBorder?: boolean
  /** 空数据提示文案；省略时用 common.status.empty */
  emptyDescription?: string
  /** 是否显示汇总行（省略时按列 summary 自动推断） */
  showSummary?: boolean
  /** 汇总行首列标签 */
  summaryLabel?: string
  /** 表尾备注说明（合计行 / 表体下方） */
  footerRemark?: string
  /**
   * 虚拟滚动（大数据默认开启）；false 仅在行数未超阈值时可关，超阈值强制开
   * @see TAKT_TABLE_AUTO_VIRTUAL_ROW_THRESHOLD
   */
  virtual?: boolean
  /** validate 最少行数 */
  minRows?: number
  /** 是否启用方向键 / 回车键在可编辑单元格间导航（Excel 风格） */
  enableArrowNavigation?: boolean
  /** 作废字段名（如 isObsolete）；设置后操作列为作废/撤销，作废行灰色只读 */
  obsoleteField?: string
  /** 未作废取值，默认 0 */
  obsoleteActiveValue?: number | string
  /** 作废取值，默认 1 */
  obsoleteValue?: number | string
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: () => [],
  title: '',
  addButtonLabel: '',
  addButtonEntity: '',
  showAdd: true,
  showDelete: true,
  showToolbar: true,
  scroll: undefined,
  scrollLayout: 'editable',
  enableVerticalScroll: true,
  bordered: true,
  size: 'middle',
  loading: false,
  disabled: false,
  idField: '',
  defaultRow: undefined,
  sectionBorder: false,
  emptyDescription: '',
  showSummary: undefined,
  summaryLabel: '',
  footerRemark: '',
  virtual: true,
  minRows: 0,
  enableArrowNavigation: true,
  obsoleteField: '',
  obsoleteActiveValue: 0,
  obsoleteValue: 1,
})

const emit = defineEmits<{
  'update:modelValue': [rows: TaktEditableRow[]]
  add: [row: TaktEditableRow]
  remove: [payload: { index: number; row: TaktEditableRow }]
  /** 行标记作废 */
  obsolete: [payload: { index: number; row: TaktEditableRow }]
  /** 行撤销作废 */
  revoke: [payload: { index: number; row: TaktEditableRow }]
  /** 单元格值变更（v-model 已同步后发出，供派生计算等） */
  cellValueChange: [payload: { record: TaktEditableRow; columnKey: string; value: unknown }]
}>()

/** 父级透传的单元格具名插槽（动态名 cell-{columnKey}） */
const cellSlots = useSlots()

/**
 * 是否存在列级自定义插槽
 * @param columnKey 列 key
 * @returns 是否已定义 cell-{columnKey} 插槽
 */
function hasCellSlot(columnKey: string): boolean {
  return typeof cellSlots[`cell-${columnKey}`] === 'function'
}

/** 内部行（始终带 __rowKey） */
const innerRows = ref<TaktEditableRow[]>([])
/** 表格根元素（方向键聚焦） */
const tableRootRef = ref<HTMLElement | null>(null)
/** 单元格校验错误 `${rowKey}:${field}` → message */
const cellErrors = ref<Record<string, string>>({})

/** 可方向键导航的列 key（有序） */
const navigableColumnKeys = computed(() => buildEditableNavColumnKeys(props.columns))

/** 同步外部 modelValue → innerRows（浅监听引用，编辑态不重建内部行） */
watch(
  () => props.modelValue,
  (val) => {
    innerRows.value = attachEditableRowKeys(val ?? [], props.idField || undefined)
    cellErrors.value = {}
  },
  { immediate: true },
)

/** 空数据提示 */
const resolvedEmptyDescription = computed(() => {
  if (props.emptyDescription) {
    return props.emptyDescription
  }
  return t('common.status.empty')
})

/** a-table locale（空态由 #emptyText 渲染） */
const tableLocale = computed(() => ({
  emptyText: resolvedEmptyDescription.value,
}))

/** 新增按钮文案 */
const resolvedAddButtonLabel = computed(() => {
  if (props.addButtonLabel) {
    return props.addButtonLabel
  }
  const entity = props.addButtonEntity || props.title
  if (entity) {
    return `${t('common.page.button.createrow')}${entity}`
  }
  return t('common.page.button.createrow')
})

/** 是否展示汇总行 */
const hasSummaryRow = computed(() => {
  if (props.showSummary === false) {
    return false
  }
  if (props.showSummary === true) {
    return true
  }
  return props.columns.some((col) => !!col.summary)
})

/** 是否展示表尾备注 */
const showFooterRemark = computed(
  () => !!props.footerRemark?.trim() || !!cellSlots.footerRemark,
)

/** 汇总用行（排除作废行） */
const summarySourceRows = computed(() =>
  filterActiveEditableRows(innerRows.value, props.obsoleteField || undefined, props.obsoleteValue),
)

/** 汇总值映射 */
const summaryValueMap = computed(() => computeEditableSummaryMap(summarySourceRows.value, props.columns))

/** 汇总行首列文案 */
const resolvedSummaryLabel = computed(() => {
  if (props.summaryLabel) {
    return props.summaryLabel
  }
  return t('components.business.page.editabletable.summarylabel')
})

/** 汇总行单元格（index 与 a-table columns 列序一致；文案跳过序号列，落在第一个业务数据列） */
const summaryCells = computed(() => {
  const labelKey = resolveTableSummaryLabelColumnKey(props.columns)
  const cells: Array<{ key: string; text: string; index: number }> = []
  props.columns.forEach((column, columnIndex) => {
    const key = String(column.key)
    let text = ''
    if (labelKey && key === labelKey) {
      text = resolvedSummaryLabel.value
    } else if (column.summary) {
      const raw = summaryValueMap.value[column.key]
      text = formatSummaryValue(raw, column.summaryPrecision ?? 2)
    }
    cells.push({
      key,
      index: columnIndex,
      text,
    })
  })
  if (props.showDelete) {
    cells.push({
      key: ACTION_COLUMN_KEY,
      index: props.columns.length,
      text: '',
    })
  }
  return cells
})

/** 合并视图传入的 scroll 与兼容 scrollY */
const mergedScroll = computed(() => {
  const config = props.scroll ? { ...props.scroll } : {}
  if (config.y == null && props.scrollY != null && props.scrollY > 0) {
    config.y = props.scrollY
  }
  return Object.keys(config).length > 0 ? config : undefined
})

/** 视口动态 scroll.y（px）；editable 场景默认固定 240，仍走统一计算入口 */
const viewportScrollYPx = useTaktTableViewportScrollY(() => props.scrollLayout)

/** 是否启用虚拟滚动：显式 true，或行数超过 5000 */
const shouldUseVirtual = computed(() =>
  shouldUseTableVirtualScroll(innerRows.value.length, props.virtual),
)

/** 表格 scroll */
const scrollConfig = computed(() =>
  resolveTableScrollConfig({
    columns: tableColumns.value,
    scroll: mergedScroll.value,
    enableVerticalScroll: props.enableVerticalScroll,
    scrollLayout: props.scrollLayout,
    verticalScrollHeight: resolveVerticalScrollY(mergedScroll.value?.y, viewportScrollYPx.value),
  }),
)

/** a-table columns */
const tableColumns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = props.columns.map((col) => ({
    title: buildColumnTitle(col.title, col.titleHint, col.required),
    dataIndex: col.dataIndex ?? col.key,
    key: col.key,
    width: col.width,
    fixed: col.fixed,
    ellipsis: true,
  }))
  if (props.showDelete) {
    cols.push({
      title: t('common.table.actions'),
      key: ACTION_COLUMN_KEY,
      width: props.obsoleteField ? 72 : 56,
      align: 'center',
      fixed: 'right',
    })
  }
  return cols
})

/**
 * row-key 解析
 * @param row 行数据
 * @returns {string} row-key
 */
function rowKeyResolver(row: TaktEditableRow): string {
  return String(row[TAKT_EDITABLE_ROW_KEY] ?? '')
}

/**
 * 行是否已作废
 * @param row 行数据
 * @returns {boolean} 是否作废
 */
function isRowObsolete(row: TaktEditableRow): boolean {
  if (!props.obsoleteField) {
    return false
  }
  return isEditableRowObsolete(row, props.obsoleteField, props.obsoleteValue)
}

/**
 * 行是否禁用编辑
 * @param row 行数据
 * @returns {boolean} 是否禁用
 */
function isRowEditingDisabled(row: TaktEditableRow): boolean {
  return props.disabled || props.loading || isRowObsolete(row)
}

/**
 * a-table 行 class（作废行灰色）
 * @param record 行数据
 * @returns {object} customRow 配置
 */
function resolveCustomRow(record: TaktEditableRow) {
  if (isRowObsolete(record)) {
    return { class: 'takt-editable-table-row-obsolete' }
  }
  return {}
}

/**
 * 单元格错误 key
 * @param row 行
 * @param columnKey 列 key
 * @returns {string} `${rowKey}:${field}`
 */
function buildCellErrorKey(row: TaktEditableRow, columnKey: string): string {
  const field = resolveDataIndex(columnKey)
  return `${rowKeyResolver(row)}:${field}`
}

/**
 * 读取单元格错误
 * @param row 行
 * @param columnKey 列 key
 * @returns {string | undefined} 错误文案
 */
function getCellError(row: TaktEditableRow, columnKey: string): string | undefined {
  return cellErrors.value[buildCellErrorKey(row, columnKey)]
}

/**
 * 解析单元格校验态
 * @param row 行
 * @param columnKey 列 key
 * @returns {'error' | undefined} a-input status
 */
function resolveCellStatus(row: TaktEditableRow, columnKey: string): 'error' | undefined {
  return getCellError(row, columnKey) ? 'error' : undefined
}

/**
 * 单元格变更时清除对应错误
 * @param row 行
 * @param columnKey 列 key
 */
function handleCellChange(row: TaktEditableRow, columnKey: string) {
  const key = buildCellErrorKey(row, columnKey)
  if (!cellErrors.value[key]) {
    return
  }
  const next = { ...cellErrors.value }
  delete next[key]
  cellErrors.value = next
}

/**
 * 内置编辑器值变更：清校验并通知父级
 * @param row 行
 * @param columnKey 列 key
 * @param value 新值
 */
function handleEditorValueChange(row: TaktEditableRow, columnKey: string, value: unknown) {
  handleCellChange(row, columnKey)
  emit('cellValueChange', { record: row, columnKey, value })
}

/**
 * 键盘在可编辑单元格间移动焦点（方向键；Enter 下一行；Shift+Enter 上一行）
 * @param event 键盘事件
 * @param rowIndex 行索引
 * @param columnKey 列 key
 */
function handleCellNavKeydown(event: KeyboardEvent, rowIndex: number, columnKey: string) {
  if (!props.enableArrowNavigation || props.disabled || props.loading) {
    return
  }
  const row = innerRows.value[rowIndex]
  if (!row || isRowObsolete(row)) {
    return
  }
  if (isReadonlyColumn(columnKey)) {
    return
  }
  const column = resolveColumn(columnKey)
  let direction: TaktEditableNavDirection | null = null
  if (event.key === 'Enter') {
    if (!shouldNavigateOnEnterKey(event, column)) {
      return
    }
    direction = event.shiftKey ? 'up' : 'down'
  } else {
    const keyMap: Record<string, TaktEditableNavDirection | undefined> = {
      ArrowUp: 'up',
      ArrowDown: 'down',
      ArrowLeft: 'left',
      ArrowRight: 'right',
    }
    direction = keyMap[event.key] ?? null
  }
  if (!direction) {
    return
  }
  if (
    (direction === 'left' || direction === 'right')
    && !shouldNavigateHorizontalOnArrowKey(event, direction)
  ) {
    return
  }
  if (
    (direction === 'up' || direction === 'down')
    && !shouldNavigateVerticalOnArrowKey(event)
  ) {
    return
  }
  const next = resolveNextEditableCell(
    rowIndex,
    columnKey,
    direction,
    navigableColumnKeys.value,
    innerRows.value.length,
  )
  if (!next) {
    return
  }
  event.preventDefault()
  event.stopPropagation()
  void nextTick(() => {
    focusEditableTableCell(tableRootRef.value, next.rowIndex, next.columnKey)
  })
}

/**
 * 可编辑单元格导航属性（data 坐标 + keydown）
 * @param rowIndex 行索引
 * @param columnKey 列 key
 * @returns Vue v-bind 对象；只读列返回空对象
 */
function resolveCellNavAttrs(rowIndex: number, columnKey: string): Record<string, unknown> {
  if (!props.enableArrowNavigation || isReadonlyColumn(columnKey)) {
    return {}
  }
  return buildEditableCellNavAttrs(rowIndex, columnKey, (event) => {
    handleCellNavKeydown(event, rowIndex, columnKey)
  })
}

/**
 * 按 key 查找列配置
 * @param key 列 key
 * @returns {TaktEditableTableColumn | undefined} 列配置
 */
function resolveColumn(key: string): TaktEditableTableColumn | undefined {
  return props.columns.find((col) => col.key === key)
}

/**
 * 解析绑定字段名
 * @param key 列 key
 * @returns {string} dataIndex
 */
function resolveDataIndex(key: string): string {
  return resolveColumn(key)?.dataIndex ?? key
}

/**
 * 只读列
 * @param key 列 key
 * @returns {boolean} 只读
 */
function isReadonlyColumn(key: string): boolean {
  const col = resolveColumn(key)
  if (!col) {
    return true
  }
  return col.readonly === true || col.editor === 'readonly'
}

/**
 * 解析列编辑器类型
 * @param key 列 key
 * @returns {TaktEditableEditorType} 编辑器
 */
function resolveEditor(key: string): TaktEditableEditorType {
  const col = resolveColumn(key)
  if (!col || col.readonly) {
    return 'readonly'
  }
  return col.editor ?? 'input'
}

/**
 * 解析列占位符
 * @param col 列配置
 * @returns {string} placeholder
 */
function resolvePlaceholder(col: TaktEditableTableColumn): string {
  if (col.placeholder) {
    return col.placeholder
  }
  const field = col.title
  if (col.editor === 'datePicker') {
    return t('common.page.form.placeholder.select', { field })
  }
  if (col.editor === 'textarea') {
    return t('common.page.form.placeholder.optional', { field })
  }
  return t('common.page.form.placeholder.required', { field })
}

/**
 * 行号 + 字段标签（校验文案）
 * @param rowIndex 行索引
 * @param column 列配置
 * @returns {string} 标签
 */
function buildRowFieldLabel(rowIndex: number, column: TaktEditableTableColumn): string {
  return t('components.business.page.editabletable.rowfield', {
    row: rowIndex + 1,
    field: column.title,
  })
}

/**
 * 解析 a-input props
 * @param key 列 key
 * @returns {Record<string, unknown>} props
 */
function resolveInputProps(key: string, row: TaktEditableRow): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    placeholder: col ? resolvePlaceholder(col) : '',
    size: props.size,
    disabled: isRowEditingDisabled(row),
    allowClear: col?.allowClear !== false,
    style: { width: '100%' },
  }
}

/**
 * 解析 a-input-number props
 * @param key 列 key
 * @param row 行数据
 * @returns {Record<string, unknown>} props
 */
function resolveInputNumberProps(key: string, row: TaktEditableRow): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    min: col?.min ?? 0,
    placeholder: col ? resolvePlaceholder(col) : '',
    size: props.size,
    disabled: isRowEditingDisabled(row),
    style: { width: '100%' },
  }
}

/**
 * 解析 a-textarea props
 * @param key 列 key
 * @param row 行数据
 * @returns {Record<string, unknown>} props
 */
function resolveTextareaProps(key: string, row: TaktEditableRow): Record<string, unknown> {
  const col = resolveColumn(key)
  const rows = col?.rows ?? 1
  const result: Record<string, unknown> = {
    placeholder: col ? resolvePlaceholder(col) : '',
    size: props.size,
    disabled: isRowEditingDisabled(row),
    style: { width: '100%' },
  }
  if (rows <= 1) {
    result.autoSize = { minRows: 1, maxRows: 1 }
  } else {
    result.rows = rows
  }
  return result
}

/**
 * 解析 a-date-picker props
 * @param key 列 key
 * @param row 行数据
 * @returns {Record<string, unknown>} props
 */
function resolveDatePickerProps(key: string, row: TaktEditableRow): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    placeholder: col ? resolvePlaceholder(col) : '',
    valueFormat: col?.valueFormat ?? 'YYYY-MM-DD HH:mm:ss',
    showTime: col?.showTime ?? true,
    size: props.size,
    disabled: isRowEditingDisabled(row),
    style: { width: '100%' },
  }
}

/**
 * 向父级 emit 行变更
 * @param rows 内部行
 */
function emitRows(rows: TaktEditableRow[]) {
  innerRows.value = rows
  emit('update:modelValue', rows)
}

/** 新增一行 */
function handleAddRow() {
  const base = props.defaultRow?.() ?? {}
  const row: TaktEditableRow = {
    ...base,
    [TAKT_EDITABLE_ROW_KEY]: createEditableRowKey(),
  }
  if (props.obsoleteField) {
    row[props.obsoleteField] = props.obsoleteActiveValue
  }
  const next = [...innerRows.value, row]
  emitRows(next)
  emit('add', row)
}

/**
 * 删除指定行
 * @param index 行索引
 */
function handleRemoveRow(index: number) {
  const row = innerRows.value[index]
  if (!row) {
    return
  }
  const next = innerRows.value.filter((_, i) => i !== index)
  emitRows(next)
  emit('remove', { index, row })
}

/**
 * 标记行作废（保留行号，灰色只读）
 * @param index 行索引
 */
function handleMarkRowObsolete(index: number) {
  if (!props.obsoleteField) {
    return
  }
  const row = innerRows.value[index]
  if (!row || isRowObsolete(row)) {
    return
  }
  row[props.obsoleteField] = props.obsoleteValue
  const next = [...innerRows.value]
  emitRows(next)
  emit('obsolete', { index, row })
}

/**
 * 撤销行作废
 * @param index 行索引
 */
function handleRevokeRowObsolete(index: number) {
  if (!props.obsoleteField) {
    return
  }
  const row = innerRows.value[index]
  if (!row || !isRowObsolete(row)) {
    return
  }
  row[props.obsoleteField] = props.obsoleteActiveValue
  const next = [...innerRows.value]
  emitRows(next)
  emit('revoke', { index, row })
}

/**
 * 剥离 __rowKey 后返回提交用行
 * @returns {Record<string, unknown>[]} DTO 行数组
 */
function getRows(): Record<string, unknown>[] {
  return detachEditableRowKeys(innerRows.value)
}

/**
 * 返回各列汇总值
 * @returns {Record<string, unknown>} 汇总映射
 */
function getSummaryValues(): Record<string, unknown> {
  return { ...summaryValueMap.value }
}

/** 清空子表 */
function resetRows() {
  cellErrors.value = {}
  emitRows([])
}

/** 清除校验态 */
function clearValidate() {
  cellErrors.value = {}
}

/**
 * 校验子表行（失败 throw Error，供父级 validate 捕获）
 * @returns {Promise<TaktEditableValidateError[]>} 错误列表（成功为空数组）
 */
async function validate(): Promise<TaktEditableValidateError[]> {
  const entity = props.addButtonEntity || props.title
  const minRowsMessage =
    props.minRows > 0
      ? t('components.business.page.editabletable.minrows', {
          min: props.minRows,
          entity: entity || t('components.business.page.editabletable.row'),
        })
      : ''
  const rawErrors = await validateEditableRows(innerRows.value, props.columns, {
    minRows: props.minRows,
    minRowsMessage,
    obsoleteField: props.obsoleteField || undefined,
    obsoleteValue: props.obsoleteValue,
    rowFieldLabel: (rowIndex, column) =>
      t('common.validation.required', { field: buildRowFieldLabel(rowIndex, column) }),
    uniqueMessage: (rowIndex, column) =>
      t('components.business.page.editabletable.uniquefield', {
        row: rowIndex + 1,
        field: column.title,
      }),
  })
  const normalizedErrors = rawErrors.map((item) => {
    if (item.rowIndex >= 0) {
      return item
    }
    return {
      ...item,
      message: item.message || minRowsMessage,
    }
  })
  cellErrors.value = mapEditableErrorsToCells(normalizedErrors)
  if (normalizedErrors.length) {
    const first = normalizedErrors[0]
    throw new Error(first?.message || t('common.validation.invalid', { field: props.title || '' }))
  }
  return []
}

/**
 * 遍历内部可编辑行（原地修改，供父级派生字段刷新）
 * @param callback 行回调
 */
function forEachRow(callback: (row: TaktEditableRow, index: number) => void) {
  innerRows.value.forEach(callback)
}

/**
 * 将内部行同步至 v-model（剥离 __rowKey）
 */
function syncModelValue() {
  emit('update:modelValue', detachEditableRowKeys(innerRows.value))
}

defineExpose({
  addRow: handleAddRow,
  removeRow: handleRemoveRow,
  markRowObsolete: handleMarkRowObsolete,
  revokeRowObsolete: handleRevokeRowObsolete,
  getRows,
  getSummaryValues,
  resetRows,
  validate,
  clearValidate,
  forEachRow,
  syncModelValue,
})
</script>

<style scoped lang="css">
/* 行内编辑：单元格垂直居中，控件宽度撑满，单行 textarea 不撑高行 */
.takt-editable-table :deep(.ant-table-tbody > tr > td) {
  vertical-align: middle;
  padding-top: 4px;
  padding-bottom: 4px;
}

.takt-editable-table-cell {
  display: flex;
  min-width: 0;
  min-height: 32px;
  flex-direction: column;
  justify-content: center;
}

.takt-editable-table-cell :deep(.ant-input),
.takt-editable-table-cell :deep(.ant-input-affix-wrapper),
.takt-editable-table-cell :deep(.ant-input-number),
.takt-editable-table-cell :deep(.ant-select),
.takt-editable-table-cell :deep(.ant-picker) {
  width: 100%;
}

.takt-editable-table-cell :deep(textarea.ant-input) {
  resize: none;
}

.takt-editable-table-cell :deep(.ant-select-selector) {
  align-items: center;
}

.takt-editable-table :deep(.takt-editable-table-row-obsolete > td) {
  color: var(--ant-color-text-disabled);
  background-color: var(--ant-color-fill-alter);
}
</style>
