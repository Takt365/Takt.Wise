<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanBatchPanel.vue -->
<!-- 功能描述：设变看板·批次页签：panelKey 区分生产组(生管/制二/制一/品管)与采购/受检/部管/物料独立脊柱；defineExpose 无 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="box-border flex w-full min-w-0 max-w-full flex-col overflow-x-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    >
      <template #leading>
        <a-range-picker
          v-model:value="entryPeriod"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :placeholder="[
            t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
            t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
          ]"
          :disabled-date="entryPeriodLimit.disabledDate"
          @calendar-change="entryPeriodLimit.onCalendarChange"
          @open-change="entryPeriodLimit.onOpenChange"
        />
      </template>
    </TaktQueryBar>
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      export-permission="logistics:manufacturing:engineering:change:kanban:export"
      :export-loading="loading"
      :refresh-loading="loading"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="loadData"
    />
    <TaktSingleTable
      analysis-table
      entity-scope="company"
      :show-row-selection="false"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecTeId'"
      table-mode="single"
      scroll-layout="pageWithInnerTabs"
      :scroll="{ y: batchTableScrollY }"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getEcId"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'ecIssueDate' || column.key === 'ecEntryDate'">
          {{ formatIssueDate(record[column.key]) }}
        </template>
        <template v-else-if="column.key === 'ecLeader'">
          <TaktDictTag
            v-if="record.ecLeader"
            :value="record.ecLeader"
            :option="ecGroupOption(record.ecLeader)"
          />
        </template>
        <template v-else-if="isStageColumn(column.key)">
          <span>{{ (record as BatchTableRow)[String(column.key)] || '—' }}</span>
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      :show-total="true"
      @change="handlePaginationChange"
    />
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="queryStorageKey"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('ecCode')">
          <a-form-item :label="gi.label('ecCode')">
            <a-input v-model:value="advancedQueryForm.ecCode" allow-clear :maxlength="10" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('changeStatus')">
          <a-form-item :label="gi.label('changeStatus')">
            <TaktSelect v-model:value="advancedQueryForm.changeStatus" dict-type="logistics_manufacturing_ec_status" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecStatus')">
          <a-form-item :label="gi.label('ecStatus')">
            <TaktSelect v-model:value="advancedQueryForm.ecStatus" dict-type="logistics_manufacturing_ec_te_status" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('cultureCode')">
          <a-form-item :label="t('common.page.entity.culturecode')">
            <TaktSelect v-model:value="advancedQueryForm.cultureCode" dict-type="sys_culture_code" allow-clear />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecTeId"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板批次页签：按 panelKey 展开脊柱（生产组=生管行；采购/受检/部管/物料=本部门库表行）
 */
import '../../ec-list-layout.css'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import { exportEcKanbanData } from '@/api/logistics/manufacturing/engineering-change/ecKanban.api'
import { getEcGroupOptions } from '@/api/logistics/manufacturing/engineering-change/ecGroup.api'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import type { TaktSelectOption } from '@/types/common.types'
import type {
  EcKanban,
  EcKanbanDeptLine,
} from '@/types/logistics/manufacturing/engineering-change/ecKanban.types'
import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
} from '@/utils/tableScroll'
import { useEcTeI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcTeI18n'
import {
  fetchAllEcKanbanMasters,
  sliceEcKanbanDetailPage,
} from '../utils/fetchAllEcKanbanMasters'

/** 批次页签：生产组 / 采购 / 受检 / 部管 / 物料 */
export type EcKanbanBatchPanelKey = 'production' | 'pur' | 'iqc' | 'mc' | 'mtl'

const props = withDefaults(defineProps<{
  /** 页签脊柱：production=生管+制二+制一+品管；其余为本部门单列 */
  panelKey?: EcKanbanBatchPanelKey
}>(), {
  panelKey: 'production',
})

const { t } = useI18n()
const gi = useEcTeI18n()
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page'
const batchPrefix = `${localePrefix}.batch`
/** 视口标准表体高度（已扣内嵌页签栏） */
const viewportScrollYPx = useTaktTableViewportScrollY('pageWithInnerTabs')
/**
 * 批次表为分组双层表头；scroll.y 只约束表体，须再扣一层表头高度，
 * 总高度才与采购/受检等单层表头页签对齐
 */
const batchTableScrollY = computed(() =>
  Math.max(TAKT_TABLE_SCROLL_Y_MIN, viewportScrollYPx.value - TAKT_TABLE_HEADER_FALLBACK_PX),
)
/** 列表 loading */
const loading = ref(false)
/** 单张设变最多展开行数（与后端 KanbanDeptLineCap 对齐） */
const KANBAN_MAX_EXPAND_LINES = 300

/** 展开后的一行 */
interface BatchTableRow {
  rowKey: string
  lineIndex: number
  ecIssueDate: string
  ecLeader: string
  ecCode: string
  ecModelCode: string
  ecRootMaterialCode: string
  ecEntryDate: string
  [key: string]: string | number
}

/** 部门列配置 */
interface BatchDeptColumn {
  linesKey: string
  group: string
  dateKey: string
  batchKey: string
}

/** 生产组：生管→制二→制一→品管（同生管脊柱） */
const PRODUCTION_DEPT_COLUMNS: BatchDeptColumn[] = [
  { linesKey: 'pcLines', group: 'pc', dateKey: 'scheduledInput', batchKey: 'productionSchedule' },
  { linesKey: 'mfg2Lines', group: 'mfg2', dateKey: 'productionDate', batchKey: 'productionBatch' },
  { linesKey: 'mfg1Lines', group: 'mfg1', dateKey: 'productionDate', batchKey: 'productionBatch' },
  { linesKey: 'qcLines', group: 'qc', dateKey: 'sampleDate', batchKey: 'sampleBatch' },
]

/** 单部门页签列 */
const SINGLE_DEPT_COLUMNS: Record<Exclude<EcKanbanBatchPanelKey, 'production'>, BatchDeptColumn[]> = {
  pur: [{ linesKey: 'purLines', group: 'pur', dateKey: 'inboundDate', batchKey: '' }],
  iqc: [{ linesKey: 'iqcLines', group: 'iqc', dateKey: 'inspectionDate', batchKey: '' }],
  mc: [{ linesKey: 'mcLines', group: 'mc', dateKey: 'outboundDate', batchKey: 'outboundBatch' }],
  mtl: [{ linesKey: 'mtlLines', group: 'mtl', dateKey: 'outboundDate', batchKey: 'outboundBatch' }],
}

/** 当前页签部门列 */
const activeDeptColumns = computed((): BatchDeptColumn[] => {
  if (props.panelKey === 'production') {
    return PRODUCTION_DEPT_COLUMNS
  }
  return SINGLE_DEPT_COLUMNS[props.panelKey]
})

/** 展开脊柱字段（生产=生管；其余=本部门） */
const spineLinesKey = computed(() => {
  if (props.panelKey === 'production') {
    return 'pcLines'
  }
  return SINGLE_DEPT_COLUMNS[props.panelKey][0].linesKey
})

/** 查询抽屉 storage */
const queryStorageKey = computed(
  () => `takt-query-fields-logistics-manufacturing-engineering-change-kanban-batch-${props.panelKey}`,
)

/** 展开后的全量明细行（分页按此切页） */
const allDetailRows = ref<BatchTableRow[]>([])
/** 当前页表格行 */
const dataSource = ref<BatchTableRow[]>([])
/** 当前页 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 总数（展开后明细行数） */
const total = ref(0)
/** 关键词 */
const queryKeyword = ref('')
/** 录入期间 */
const entryPeriod = ref<[string, string] | null>(buildDefaultThroughCurrentPeriodRange(3))
/** 录入期间限制 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })
/** 列设置 */
const columnSettingVisible = ref(false)
/** 高级查询 */
const advancedQueryVisible = ref(false)
/** 高级查询字段 */
const KANBAN_QUERY_FIELDS = ['ecCode', 'changeStatus', 'ecStatus', 'cultureCode'] as const
/** 高级查询可见字段 */
const visibleQueryFieldKeys = ref<string[]>([])

/**
 * 空高级查询表单
 */
function createEmptyAdvancedQueryForm() {
  return {
    ecCode: '',
    cultureCode: '',
    changeStatus: undefined as number | undefined,
    ecStatus: undefined as number | undefined,
  }
}

/** 高级查询表单 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段标题 */
const queryFieldsMeta = computed(() => KANBAN_QUERY_FIELDS.map((key) => ({
  key,
  label: key === 'cultureCode' ? t('common.page.entity.culturecode') : gi.label(key),
})))

/**
 * 叶子列
 * @param title 标题
 * @param key 列键
 * @param width 宽度
 * @returns 列
 */
function leaf(title: string, key: string, width: number) {
  return {
    title,
    dataIndex: key,
    key,
    width,
    align: 'center' as const,
  }
}

/**
 * 部门分组列
 * @param group 分组键
 * @param dateKey 日期列文案键
 * @param batchKey 批次列文案键；空则仅日期
 * @returns 分组列
 */
function deptGroup(group: string, dateKey: string, batchKey: string) {
  const children = [
    leaf(t(`${batchPrefix}.column.${dateKey}`), `date_${group}`, 110),
  ]
  if (batchKey) {
    children.push(leaf(t(`${batchPrefix}.column.${batchKey}`), `batch_${group}`, 120))
  }
  return {
    title: t(`${batchPrefix}.group.${group}`),
    key: `group_${group}`,
    align: 'center' as const,
    children,
  }
}

/** 列：设变信息 + 当前页签部门日期/批次 */
const columns = computed((): TableColumnsType => [
  leaf(t(`${localePrefix}.column.issueDate`), 'ecIssueDate', 120),
  leaf(t(`${localePrefix}.column.leader`), 'ecLeader', 110),
  leaf(t(`${localePrefix}.column.ecCode`), 'ecCode', 130),
  leaf(t(`${localePrefix}.column.model`), 'ecModelCode', 120),
  leaf(t(`${localePrefix}.column.finishedProduct`), 'ecRootMaterialCode', 150),
  leaf(t(`${localePrefix}.column.tech`), 'ecEntryDate', 120),
  ...activeDeptColumns.value.map((column) => deptGroup(column.group, column.dateKey, column.batchKey)),
])

/** 默认可见列 */
const BATCH_DEFAULT_COLUMN_KEYS = computed(() =>
  columns.value.map((column) => String(column.key ?? '')).filter((key) => key.length > 0),
)
/** 可见列 */
const visibleColumnKeys = ref<string[]>([])

watch(
  BATCH_DEFAULT_COLUMN_KEYS,
  (keys) => {
    visibleColumnKeys.value = [...keys]
  },
  { immediate: true },
)

/** 设变组选项 */
const ecGroupOptionByCode = ref<Record<string, TaktSelectOption>>({})

/**
 * 设变担当选项
 * @param value 编码
 * @returns 选项
 */
function ecGroupOption(value: unknown): TaktSelectOption | undefined {
  if (value == null || value === '') {
    return undefined
  }
  return ecGroupOptionByCode.value[String(value)]
}

/** 预加载设变组 */
async function loadEcGroupOptions(): Promise<void> {
  try {
    const list = await getEcGroupOptions()
    const map: Record<string, TaktSelectOption> = {}
    for (const item of list ?? []) {
      if (item.dictValue != null && String(item.dictValue) !== '') {
        map[String(item.dictValue)] = item
      }
    }
    ecGroupOptionByCode.value = map
  } catch {
    ecGroupOptionByCode.value = {}
  }
}

/**
 * 是否部门日期/批次列
 * @param key 列键
 * @returns 是否
 */
function isStageColumn(key: unknown): boolean {
  const text = String(key ?? '')
  return text.startsWith('date_') || text.startsWith('batch_')
}

/**
 * 取部门执行行列表
 * @param row 看板行
 * @param linesKey 字段名
 * @returns 行列表
 */
function deptLines(row: EcKanban, linesKey: string): EcKanbanDeptLine[] {
  const value = (row as Record<string, unknown>)[linesKey]
  return Array.isArray(value) ? (value as EcKanbanDeptLine[]) : []
}

/**
 * 阶段日期展示
 * @param line 执行行
 * @returns 文本
 */
function lineDateText(line: EcKanbanDeptLine | undefined): string {
  if (!line) {
    return ''
  }
  if (line.dateDisplayText?.trim()) {
    return line.dateDisplayText.trim()
  }
  if (line.stageDate) {
    return String(line.stageDate).slice(0, 10).replace(/-/g, '')
  }
  return ''
}

/**
 * 阶段批次展示
 * @param line 执行行
 * @returns 文本
 */
function lineBatchText(line: EcKanbanDeptLine | undefined): string {
  return line?.prodBatch?.trim() ? line.prodBatch.trim() : ''
}

/**
 * 按页签脊柱展开：生产组按生管行；单部门按本部门库表行（机种/成品取脊柱行）
 * @param rows 看板行
 * @returns 表格行
 */
function expandBatchRows(rows: EcKanban[]): BatchTableRow[] {
  const lines: BatchTableRow[] = []
  const deptCols = activeDeptColumns.value
  const spineKey = spineLinesKey.value
  for (const row of rows) {
    const spine = deptLines(row, spineKey)
    const lineCount = Math.min(KANBAN_MAX_EXPAND_LINES, Math.max(1, spine.length))
    const groups = deptCols.map((column) => deptLines(row, column.linesKey))
    for (let index = 0; index < lineCount; index += 1) {
      const spineLine = spine[index]
      const line: BatchTableRow = {
        rowKey: `${row.ecCode}-${props.panelKey}-${index}`,
        lineIndex: index,
        ecIssueDate: row.ecIssueDate ?? '',
        ecLeader: row.ecLeader ?? '',
        ecCode: row.ecCode ?? '',
        ecModelCode: String(spineLine?.ecModelCode ?? ''),
        ecRootMaterialCode: String(spineLine?.ecRootMaterialCode ?? ''),
        ecEntryDate: row.ecEntryDate ?? '',
      }
      deptCols.forEach((column, groupIndex) => {
        // 单部门页签：列与脊柱同一列表，按下标取；生产组：各部门已对齐生管下标
        const item = props.panelKey === 'production'
          ? groups[groupIndex]?.[index]
          : spineLine
        if (!item || item.isImplemented === -1) {
          line[`date_${column.group}`] = ''
          if (column.batchKey) {
            line[`batch_${column.group}`] = ''
          }
          return
        }
        line[`date_${column.group}`] = lineDateText(item)
        if (column.batchKey) {
          line[`batch_${column.group}`] = lineBatchText(item)
        }
      })
      lines.push(line)
    }
  }
  return lines
}

/**
 * 发行/录入日期只显示年月日
 * @param value 日期
 * @returns yyyy-MM-dd
 */
function formatIssueDate(value: unknown): string {
  const text = String(value ?? '')
  return text.length >= 10 ? text.slice(0, 10) : text
}

/**
 * 行主键
 * @param record 行
 * @returns 主键
 */
function getEcId(record: Record<string, unknown>) {
  return String(record.rowKey ?? record.ecCode ?? '')
}

/**
 * 录入期间 → 查询起止
 * @param range 年月区间
 * @returns 日期起止
 */
function entryPeriodToQuery(range: [string, string] | null): { ecEntryDateStart?: string; ecEntryDateEnd?: string } {
  if (!range?.[0] || !range?.[1]) {
    return {}
  }
  const parts = range[1].split('-').map(Number)
  const year = parts[0]
  const month = parts[1]
  if (!year || !month) {
    return { ecEntryDateStart: `${range[0]}-01` }
  }
  const lastDay = new Date(year, month, 0).getDate()
  return {
    ecEntryDateStart: `${range[0]}-01`,
    ecEntryDateEnd: `${range[1]}-${String(lastDay).padStart(2, '0')}`,
  }
}

/**
 * 构建筛选参数（不含分页；明细行在前端展开后分页）
 * @returns 查询 DTO
 */
function buildQueryParams() {
  const form = advancedQueryForm.value
  return {
    keyWords: queryKeyword.value || undefined,
    ...entryPeriodToQuery(entryPeriod.value),
    ecCode: form.ecCode.trim() || undefined,
    cultureCode: form.cultureCode.trim() || undefined,
    changeStatus: form.changeStatus === undefined || form.changeStatus === null ? undefined : Number(form.changeStatus),
    ecStatus: form.ecStatus === undefined || form.ecStatus === null ? undefined : Number(form.ecStatus),
  }
}

/** 按明细行切当前页 */
function applyDetailPageSlice() {
  const sliced = sliceEcKanbanDetailPage(allDetailRows.value, currentPage.value, pageSize.value)
  currentPage.value = sliced.pageIndex
  total.value = sliced.total
  dataSource.value = sliced.pageRows
}

/** 加载列表（拉全量主表 → 展开明细 → 按明细行分页） */
async function loadData() {
  loading.value = true
  try {
    const masters = await fetchAllEcKanbanMasters(buildQueryParams())
    allDetailRows.value = expandBatchRows(masters)
    applyDetailPageSlice()
  } finally {
    loading.value = false
  }
}

/** 搜索 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置 */
function handleReset() {
  entryPeriod.value = buildDefaultThroughCurrentPeriodRange(3)
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = 1
  loadData()
}

/** 打开高级查询 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 提交高级查询 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

/** 重置高级查询 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = 1
  loadData()
}

/** 打开列设置 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/**
 * 列设置变更
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置恢复默认 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = [...BATCH_DEFAULT_COLUMN_KEYS.value]
}

/** 分页变化（仅切明细页，不重拉主表） */
function handlePaginationChange() {
  applyDetailPageSlice()
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽变化 */
function handleResizeColumn() {}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true
    const blob = await exportEcKanbanData(buildQueryParams())
    const url = window.URL.createObjectURL(blob as Blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `${t(`${localePrefix}.tabs.${props.panelKey}`)}.xlsx`
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
  } finally {
    loading.value = false
  }
}

useTableRefresh(loadData)
onMounted(() => {
  void loadEcGroupOptions()
  void loadData()
})
</script>
