<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanDeptDetailPanel.vue -->
<!-- 功能描述：设变看板·采购/受检/部管/物料页签：主表字段 + 本部门实体完整列；列设置按 panelKey 默认可见 -->
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
      :id-column-key="panelConfig.idField"
      table-mode="single"
      scroll-layout="pageWithInnerTabs"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getRowId"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'ecLeader'">
          <TaktDictTag
            v-if="record.ecLeader"
            :value="record.ecLeader"
            :option="ecGroupOption(record.ecLeader)"
          />
        </template>
        <template v-else-if="column.key === 'discontinuedStatus'">
          <TaktDictTag dict-type="logistics_materials_material_discontinued_status" :value="record.discontinuedStatus" />
        </template>
        <template v-else-if="column.key === 'ecNewPurchaseType'">
          <TaktDictTag dict-type="logistics_procurement_type" :value="record.ecNewPurchaseType" />
        </template>
        <template v-else-if="column.key === 'ecOldPartBalance'">
          <span
            v-if="String(record.ecOldPartBalance ?? '').includes(':')"
            class="truncate"
            :title="String(record.ecOldPartBalance ?? '')"
          >{{ record.ecOldPartBalance }}</span>
          <TaktDictTag
            v-else
            dict-type="logistics_manufacturing_ec_old_part_disposition"
            :value="record.ecOldPartBalance"
          />
        </template>
        <template v-else-if="column.key === 'ecImplScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_impl_scope_category" :value="record.ecImplScope" />
        </template>
        <template v-else-if="column.key === 'ecControlScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_control_scope_category" :value="record.ecControlScope" />
        </template>
        <template v-else-if="isYesNoField(String(column.key))">
          <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
        </template>
        <template v-else-if="isCalendarDateColumn(column.key)">
          {{ formatCalendarDate(record[String(column.key)]) }}
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
      :id-column-key="panelConfig.idField"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板部门完整列页签：主表 + 本部门实体字段；按 panelKey 切换采购/受检/部管/物料
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
import type { TaktSelectOption } from '@/types/common.types'
import type { EcKanban } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types'
import { getEcDeptExecLineFields } from '@/constants/logistics/ecDeptExecLineFields.constants'
import { formatCalendarDate } from '@/utils/datetime'
import { resolveEntityFieldI18nKey } from '@/utils/taktEntityI18n'
import { useEcTeI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcTeI18n'
import {
  fetchAllEcKanbanMasters,
  sliceEcKanbanDetailPage,
} from '../utils/fetchAllEcKanbanMasters'

/** 部门完整列页签 */
export type EcKanbanDeptDetailPanelKey = 'pur' | 'iqc' | 'mc' | 'mtl'

/** 页签配置 */
interface PanelConfig {
  slug: string
  idField: string
  linesKey: 'purDetailLines' | 'iqcDetailLines' | 'mcDetailLines' | 'mtlDetailLines'
  tabKey: EcKanbanDeptDetailPanelKey
  defaultVisible: readonly string[]
}

const PANEL_CONFIG: Record<EcKanbanDeptDetailPanelKey, PanelConfig> = {
  pur: {
    slug: 'ecpur',
    idField: 'ecPurId',
    linesKey: 'purDetailLines',
    tabKey: 'pur',
    defaultVisible: [
      'ecIssueDate',
      'ecCode',
      'ecLeader',
      'ecModelCodeGroup',
      'ecNewMaterialCodeGroup',
      'ecNewPurchaseType',
      'isImplemented',
      'inboundDate',
    ],
  },
  iqc: {
    slug: 'eciqc',
    idField: 'ecIqcId',
    linesKey: 'iqcDetailLines',
    tabKey: 'iqc',
    defaultVisible: [
      'ecIssueDate',
      'ecCode',
      'ecLeader',
      'ecModelCodeGroup',
      'ecNewMaterialCodeGroup',
      'ecNewRequiresInspection',
      'isImplemented',
      'inspectionDate',
    ],
  },
  mc: {
    slug: 'ecmc',
    idField: 'ecMcId',
    linesKey: 'mcDetailLines',
    tabKey: 'mc',
    defaultVisible: [
      'ecIssueDate',
      'ecCode',
      'ecLeader',
      'ecModelCodeGroup',
      'ecRootMaterialCodeGroup',
      'ecNewMaterialCodeGroup',
      'ecNewWarehouse',
      'isImplemented',
      'outboundDate',
      'outboundBatch',
    ],
  },
  mtl: {
    slug: 'ecmtl',
    idField: 'ecMtlId',
    linesKey: 'mtlDetailLines',
    tabKey: 'mtl',
    defaultVisible: [
      'ecIssueDate',
      'ecCode',
      'ecLeader',
      'ecModelCodeGroup',
      'ecRootMaterialCodeGroup',
      'ecParentMaterialCode',
      'ecParentMaterialDescription',
      'ecNewMaterialCodeGroup',
      'ecNewWarehouse',
      'isImplemented',
      'outboundDate',
      'outboundBatch',
    ],
  },
}

const props = withDefaults(defineProps<{
  /** 部门完整列页签 */
  panelKey?: EcKanbanDeptDetailPanelKey
}>(), {
  panelKey: 'pur',
})

const { t } = useI18n()
const gi = useEcTeI18n()
const panelConfig = computed(() => PANEL_CONFIG[props.panelKey])
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page'
/** 列表 loading */
const loading = ref(false)
/** 单张设变最多展开行 */
const KANBAN_MAX_EXPAND_LINES = 300
/** 是/否字典列（含新品检验） */
const YES_NO_FIELDS = new Set(['isImplemented', 'isObsolete', 'ecNewRequiresInspection'])
/** 主表附加列 */
const MASTER_FIELDS = ['ecIssueDate', 'ecLeader', 'ecEntryDate'] as const

/** 查询抽屉 storage */
const queryStorageKey = computed(
  () => `takt-query-fields-logistics-manufacturing-engineering-change-kanban-${props.panelKey}`,
)

/** 展开后的全量明细行（分页按此切页） */
const allDetailRows = ref<Record<string, unknown>[]>([])
/** 当前页表格行 */
const dataSource = ref<Record<string, unknown>[]>([])
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
 * 是否是/否字段
 * @param field 列 key
 * @returns 是否
 */
function isYesNoField(field: string): boolean {
  return YES_NO_FIELDS.has(field)
}

/**
 * 是否日历日列
 * @param key 列 key
 * @returns 是否
 */
function isCalendarDateColumn(key: unknown): boolean {
  return String(key ?? '').endsWith('Date')
}

/**
 * 列宽
 * @param field 字段
 * @returns 宽度
 */
function columnWidth(field: string): number {
  if (field.endsWith('Id') || field === 'ecGrainKey') {
    return 170
  }
  if (field === 'execContent' || field === 'supplier') {
    return 180
  }
  if (field === 'ecRootMaterialCodeGroup' || field === 'ecModelCodeGroup') {
    return 220
  }
  if (field.endsWith('Date') || field.endsWith('Code') || field === 'ecLeader' || field.endsWith('Description')) {
    return 140
  }
  if (YES_NO_FIELDS.has(field) || field === 'lineNumber' || field === 'deptCode') {
    return 100
  }
  return 120
}

/**
 * 字段标题
 * @param field 字段
 * @returns 标题
 */
function fieldTitle(field: string): string {
  if (field === 'ecIssueDate') {
    return t(`${localePrefix}.column.issueDate`)
  }
  if (field === 'ecLeader') {
    return t(`${localePrefix}.column.leader`)
  }
  if (field === 'ecEntryDate') {
    return t(`${localePrefix}.column.tech`)
  }
  // 与批次页签「成品」列文案一致（根物料编码组）
  if (field === 'ecRootMaterialCodeGroup') {
    return t(`${localePrefix}.column.finishedProduct`)
  }
  if (field === 'ecModelCodeGroup') {
    return t(`${localePrefix}.column.model`)
  }
  // 执行行主键无独立 entity 种子时用通用「主键ID」
  if (field !== 'ecDetailId' && /^ec[A-Za-z]+Id$/.test(field)) {
    return t('common.page.entity.id')
  }
  return t(resolveEntityFieldI18nKey(panelConfig.value.slug, field))
}

/** 全部列 */
const columns = computed((): TableColumnsType => {
  const deptFields = getEcDeptExecLineFields(panelConfig.value.slug)
  const keys = [...MASTER_FIELDS, ...deptFields.filter((f) => !(MASTER_FIELDS as readonly string[]).includes(f))]
  return keys.map((field) => ({
    title: fieldTitle(field),
    dataIndex: field,
    key: field,
    width: columnWidth(field),
    ellipsis: true,
    align: 'center' as const,
  }))
})

/** 可见列 */
const visibleColumnKeys = ref<string[]>([...PANEL_CONFIG.pur.defaultVisible])

watch(
  () => props.panelKey,
  () => {
    visibleColumnKeys.value = [...panelConfig.value.defaultVisible]
    currentPage.value = 1
    void loadData()
  },
)

watch(
  columns,
  (cols) => {
    const allKeys = new Set(cols.map((c) => String(c.key ?? '')).filter(Boolean))
    visibleColumnKeys.value = visibleColumnKeys.value.filter((key) => allKeys.has(key))
    if (visibleColumnKeys.value.length === 0) {
      visibleColumnKeys.value = panelConfig.value.defaultVisible.filter((key) => allKeys.has(key))
    }
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
 * 按本部门完整行展开
 * @param rows 看板行
 * @returns 表格行
 */
function expandDetailRows(rows: EcKanban[]): Record<string, unknown>[] {
  const lines: Record<string, unknown>[] = []
  const { linesKey, idField } = panelConfig.value
  for (const row of rows) {
    const details = ((row as Record<string, unknown>)[linesKey] as Record<string, unknown>[] | undefined)
      ?? []
    const capped = details.slice(0, KANBAN_MAX_EXPAND_LINES)
    if (capped.length === 0) {
      continue
    }
    capped.forEach((detail, index) => {
      const id = String(detail[idField] ?? index)
      lines.push({
        ...detail,
        rowKey: `${row.ecCode}-${id}`,
        ecTeId: row.ecTeId,
        ecIssueDate: row.ecIssueDate ?? '',
        ecLeader: row.ecLeader ?? '',
        ecEntryDate: row.ecEntryDate ?? '',
        [idField]: id,
        ecCode: String(detail.ecCode || row.ecCode || ''),
      })
    })
  }
  return lines
}

/**
 * 行主键
 * @param record 行
 * @returns 主键
 */
function getRowId(record: Record<string, unknown>) {
  return String(record.rowKey ?? record[panelConfig.value.idField] ?? '')
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
    allDetailRows.value = expandDetailRows(masters)
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
  const allKeys = new Set(columns.value.map((c) => String(c.key ?? '')).filter(Boolean))
  visibleColumnKeys.value = panelConfig.value.defaultVisible.filter((key) => allKeys.has(key))
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
    link.download = `${t(`${localePrefix}.tabs.${panelConfig.value.tabKey}`)}.xlsx`
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
  visibleColumnKeys.value = [...panelConfig.value.defaultVisible]
  void loadEcGroupOptions()
  void loadData()
})
</script>
