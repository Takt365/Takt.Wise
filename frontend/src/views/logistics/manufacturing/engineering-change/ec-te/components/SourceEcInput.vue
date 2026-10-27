<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/components -->
<!-- 文件名称：SourceEcInput.vue -->
<!-- 功能描述：来源设变录入：查询尚未导入设变主的来源设变，加载草稿至 ec-form（不落库）；列表铺满弹窗剩余区，表高按弹出窗体 × 5/4 且不超过剩余区（分页在表内，仅表格滚动、弹出窗体无滚动条）；defineExpose 提供 resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="source-ec-input flex h-full min-h-0 min-w-0 flex-col overflow-hidden"
  >
    <!-- 工厂与查询 -->
    <a-form
      layout="inline"
      label-align="right"
      class="source-ec-input-query flex flex-wrap gap-y-2"
    >
      <a-form-item
        :label="pi.label('plantCode')"
      >
        <a-input
          :value="mappedPlantCode"
          disabled
          class="w-28"
        />
        <div class="text-xs text-text-secondary mt-1">
          {{ t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.plantFromCompany', { company: mappedCompanyCode || '—', plant: mappedPlantCode || '—' }) }}
        </div>
      </a-form-item>
      <a-form-item :label="t('common.page.button.query')">
        <a-input-search
          v-model:value="queryKeyword"
          :placeholder="t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.searchPlaceholder')"
          allow-clear
          class="w-64"
          :loading="listLoading"
          @search="handleSearch"
        />
      </a-form-item>
    </a-form>
    <!-- 未导入来源设变列表（分页在表内，与来源设变导入明细 Tab 同一套铺满） -->
    <div
      ref="tableWrapEl"
      class="source-ec-input-table-wrap min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'sourceEcId'"
        table-mode="single"
        :data-source="dataSource"
        :loading="listLoading"
        :stripe="true"
        :row-key="getSourceEcId"
        :row-selection="rowSelection"
        :include-audit-fields="false"
        scroll-layout="editable"
        :scroll="sourceEcTableScroll"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        @change="handleTableChange"
        @pagination-change="handlePaginationChange"
      />
    </div>
    <div class="source-ec-input-actions flex justify-end gap-2">
      <a-button @click="handleReset">
        {{ t('common.page.button.reset') }}
      </a-button>
      <a-button
        v-permission="'logistics:manufacturing:engineering:change:te:create'"
        type="primary"
        :loading="draftLoading"
        :disabled="!mappedPlantCode || !selecmedSourceEcId"
        @click="handleLoadToForm"
      >
        {{ t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.loadToForm') }}
      </a-button>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 来源设变录入：展示尚未导入设变主的来源设变，加载草稿至 ec-form（不落库）
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import type { SorterResult } from 'ant-design-vue/es/table/interface'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import {
  computeFormHostRatioScrollYPx,
  TAKT_TABLE_SCROLL_Y_MIN,
} from '@/utils/tableScroll'
import { getEcTeDraftFromSourceEc, getEcTeSourcePlantCode, getUnimportedSourceEcTeList } from '@/api/logistics/manufacturing/engineering-change/ecTe.api'
import type { EcTeSourceEcInputItem } from '@/types/logistics/manufacturing/engineering-change/ecTeSourceInput.types'
import type { EcTeFormData } from '@/types/logistics/manufacturing/engineering-change/ecTe.types'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { taktNormalizeEcControlScope } from '@/constants/logistics/ecScope.constants'
import {
  getSourceEcInputField,
  SOURCE_EC_INPUT_EXTRA_FIELDS,
  SOURCE_EC_INPUT_SOURCE_FIELDS,
  useSourceEcInputI18n,
  type SourceEcInputListField,
  type SourceEcInputSourceField,
} from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useSourceEcInputFields'

const emit = defineEmits<{
  /** 草稿已就绪，父级打开 ec-form */
  'draft-ready': [draft: EcTeFormData]
}>()

const { t } = useI18n()
const pi = useSourceEcInputI18n()
const userStore = useUserStore()
const tenantStore = useTenantStore()

/**
 * 本地当天日期（YYYY-MM-DD）；来源导入录入日期固定为此值
 * @returns 当天日期字符串
 */
function formatLocalTodayYmd(): string {
  const now = new Date()
  const y = String(now.getFullYear())
  const m = String(now.getMonth() + 1).padStart(2, '0')
  const d = String(now.getDate()).padStart(2, '0')
  return `${y}-${m}-${d}`
}

/** 映射后的目标工厂代码（Database:CompanyCodes/PlantCodes 同序） */
const mappedPlantCode = ref('')
/** 映射来源公司代码 */
const mappedCompanyCode = ref('')
/** 关键词 */
const queryKeyword = ref('')
/** 列表 loading */
const listLoading = ref(false)
/** 加载草稿 loading */
const draftLoading = ref(false)
/** 列表数据 */
const dataSource = ref<EcTeSourceEcInputItem[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 服务端排序字段 */
const sortField = ref('sourceIssueDate')
/** 服务端是否降序。默认升序，与原先发行日期排序一致 */
const sortDesc = ref(false)
/** 允许服务端排序的列：设变号码、明细行数、发行日期 */
const SOURCE_EC_INPUT_SORT_FIELDS = new Set<string>([
  'sourceEcCode',
  'detailCount',
  'sourceIssueDate',
])
/** 选中来源设变 ID（单选） */
const selecmedSourceEcId = ref('')
/** 默认可见列（弹窗内嵌列表，不含主键列） */
const visibleColumnKeys = ref<string[]>([
  ...SOURCE_EC_INPUT_SOURCE_FIELDS,
  ...SOURCE_EC_INPUT_EXTRA_FIELDS,
])

/** 表格宿主（定位弹出窗体并实测剩余高度） */
const tableWrapEl = ref<HTMLElement | null>(null)
/** 弹出窗体 ResizeObserver */
let formHostResizeObserver: ResizeObserver | null = null

/**
 * 列表 scroll.y：与来源设变导入明细表相同——目标为弹出窗体高度 × 5/4，且不得超过表格区剩余高度
 * （超出则弹出窗体出现第二套滚动条；❌ 禁止用浏览器视口）
 */
function computeSourceEcTableScrollYPx(): number {
  const host = tableWrapEl.value
  const ratioY = computeFormHostRatioScrollYPx(host, 5, 4)
  if (host == null || host.clientHeight <= 0) {
    return ratioY
  }
  const tableBody = host.querySelector('.takt-single-table__body') as HTMLElement | null
  if (tableBody == null || tableBody.clientHeight <= 0) {
    return ratioY
  }
  const fittedY = measureMasterDetailLrTableScrollY(host, { reserveSummaryRow: false })
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.min(ratioY, fittedY))
}

/** 来源设变列表纵向滚动高度（px） */
const sourceEcTableScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)

/** 来源设变列表 scroll 配置 */
const sourceEcTableScroll = computed(() => ({ y: sourceEcTableScrollYPx.value }))

/** 按弹出窗体与表格剩余区重算列表高度 */
function recalcSourceEcTableScrollY(): void {
  sourceEcTableScrollYPx.value = computeSourceEcTableScrollYPx()
}

/**
 * 绑定弹出窗体 ResizeObserver（全屏/拖拽改高时重算）
 */
function bindFormHostResizeObserver(): void {
  formHostResizeObserver?.disconnect()
  formHostResizeObserver = null
  const host = tableWrapEl.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const modalTarget =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
  formHostResizeObserver = new ResizeObserver(() => {
    recalcSourceEcTableScrollY()
  })
  if (modalTarget != null) {
    formHostResizeObserver.observe(modalTarget)
  }
  formHostResizeObserver.observe(host)
}

onMounted(() => {
  void nextTick(() => {
    recalcSourceEcTableScrollY()
    bindFormHostResizeObserver()
    void nextTick(() => {
      recalcSourceEcTableScrollY()
    })
  })
})

onBeforeUnmount(() => {
  formHostResizeObserver?.disconnect()
  formHostResizeObserver = null
})

/** 来源设变列表列宽 */
const SOURCE_EC_INPUT_COLUMN_WIDTH: Partial<Record<SourceEcInputListField, number>> = {
  sourceEcCode: 120,
  sourceModel: 120,
  sourceTitle: 180,
  sourceStatus: 120,
  sourceIssueDate: 120,
  detailCount: 90,
}

/**
 * 表格日期统一为 yyyy-MM-dd
 * @param value 单元格原值
 * @returns {string} 日期文本
 */
function formatSourceEcTableDate(value: unknown): string {
  if (value == null || value === '') {
    return ''
  }
  const text = String(value).trim()
  const matched = /^(\d{4}-\d{2}-\d{2})/.exec(text)
  return matched?.[1] ?? text
}

/**
 * 构建来源设变实体列
 * @param field 字段名
 */
function buildSourceEcInputSourceColumn(field: SourceEcInputSourceField) {
  const width = SOURCE_EC_INPUT_COLUMN_WIDTH[field] ?? 120
  const sortable = SOURCE_EC_INPUT_SORT_FIELDS.has(field)
  const base = {
    title: pi.label(field),
    dataIndex: field,
    key: field,
    width,
    resizable: true,
    ellipsis: true,
    sorter: sortable,
    sortOrder: sortable && sortField.value === field ? (sortDesc.value ? 'descend' : 'ascend') : null,
    customRender: ({ record }: { record: EcTeSourceEcInputItem }) => {
      const value = getSourceEcInputField(record, field)
      if (field.endsWith('Date')) {
        return formatSourceEcTableDate(value)
      }
      return value ?? ''
    },
  }
  return base
}

/** 表格列（与 TaktSourceEc / TaktEcTeSourceEcInputItemDto 对齐） */
const columns = computed<TableColumnsType>(() => [
  ...SOURCE_EC_INPUT_SOURCE_FIELDS.map((field) => buildSourceEcInputSourceColumn(field)),
  {
    title: t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.detailCount'),
    dataIndex: 'detailCount',
    key: 'detailCount',
    width: SOURCE_EC_INPUT_COLUMN_WIDTH.detailCount ?? 90,
    resizable: true,
    align: 'right',
    sorter: true,
    sortOrder: sortField.value === 'detailCount' ? (sortDesc.value ? 'descend' : 'ascend') : null,
    customRender: ({ record }: { record: EcTeSourceEcInputItem }) =>
      getSourceEcInputField(record, 'detailCount') ?? '',
  },
])

/** 行选择配置（单选） */
const rowSelection = computed(() => ({
  type: 'radio' as const,
  selecmedRowKeys: selecmedSourceEcId.value ? [selecmedSourceEcId.value] : [],
  onChange: (keys: (string | number)[]) => {
    selecmedSourceEcId.value = keys.length > 0 ? String(keys[0]) : ''
  },
}))

/**
 * 获取来源设变行主键
 * @param record 行数据
 * @returns 主键
 */
function getSourceEcId(record: any): string {
  return String(record?.sourceEcId ?? '')
}

/**
 * 加载当前公司对应的工厂代码映射
 * @returns {Promise<boolean>} 是否成功解析映射
 */
async function loadPlantMapping(): Promise<boolean> {
  if (!tenantStore.companyCode?.trim()) {
    mappedPlantCode.value = ''
    mappedCompanyCode.value = ''
    return false
  }
  try {
    const result = await getEcTeSourcePlantCode()
    mappedCompanyCode.value = result.companyCode ?? ''
    mappedPlantCode.value = result.plantCode ?? ''
    return !!mappedPlantCode.value
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    mappedPlantCode.value = ''
    mappedCompanyCode.value = ''
    return false
  }
}

/**
 * 加载未导入来源设变列表
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  if (!mappedPlantCode.value) {
    dataSource.value = []
    total.value = 0
    return
  }
  listLoading.value = true
  try {
    const result = await getUnimportedSourceEcTeList({
      plantCode: mappedPlantCode.value,
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value?.trim() || undefined,
      sortField: sortField.value,
      sortDesc: sortDesc.value,
    })
    dataSource.value = result.data ?? []
    total.value = result.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
  } finally {
    listLoading.value = false
  }
}

/** 查询（重置到第一页） */
function handleSearch(): void {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/**
 * 分页变更
 * @param {number} page 页码
 * @param {number} size 每页条数
 */
function handlePaginationChange(page: number, size: number): void {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

/**
 * 表头排序走服务端，只重查当前页
 * @param _pagination 表格分页（本页使用外置分页）
 * @param _filters 筛选
 * @param sorter 排序
 */
function handleTableChange(
  _pagination: unknown,
  _filters: unknown,
  sorter: SorterResult<EcTeSourceEcInputItem> | SorterResult<EcTeSourceEcInputItem>[],
): void {
  const current = Array.isArray(sorter) ? sorter[0] : sorter
  const field = String(current?.field ?? current?.columnKey ?? '')
  const order = current?.order
  const nextField = order && SOURCE_EC_INPUT_SORT_FIELDS.has(field) ? field : 'sourceIssueDate'
  const nextDesc = order === 'descend' && SOURCE_EC_INPUT_SORT_FIELDS.has(field)
  if (nextField === sortField.value && nextDesc === sortDesc.value) {
    return
  }
  sortField.value = nextField
  sortDesc.value = nextDesc
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 重置表单与选择 */
function resetFields(): void {
  queryKeyword.value = ''
  dataSource.value = []
  total.value = 0
  currentPage.value = getTaktDefaultPageIndex()
  sortField.value = 'sourceIssueDate'
  sortDesc.value = false
  selecmedSourceEcId.value = ''
  mappedPlantCode.value = ''
  mappedCompanyCode.value = ''
}

/** 重置并清空列表 */
function handleReset(): void {
  resetFields()
}

/** 加载选中来源设变草稿至 ec-form（不落库） */
async function handleLoadToForm(): Promise<void> {
  if (!mappedPlantCode.value) {
    message.warning(t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.companyRequired'))
    return
  }
  if (!selecmedSourceEcId.value) {
    message.warning(t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.selectRequired'))
    return
  }
  draftLoading.value = true
  try {
    const draft = await getEcTeDraftFromSourceEc({
      plantCode: mappedPlantCode.value,
      sourceEcId: selecmedSourceEcId.value,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
    })
    const formDraft: EcTeFormData = {
      ...draft,
      plantCode: draft.plantCode ?? mappedPlantCode.value,
      ecLeader: draft.ecLeader ?? '',
      ecImplScope: draft.ecImplScope >= 1 && draft.ecImplScope <= 4
        ? draft.ecImplScope
        : 1,
      ecControlScope: taktNormalizeEcControlScope(
        draft.ecImplScope >= 1 && draft.ecImplScope <= 4 ? draft.ecImplScope : 1,
        draft.ecControlScope,
      ),
      ecStatus: draft.ecStatus === 1 || draft.ecStatus === 2 || draft.ecStatus === 3 ? draft.ecStatus : 1,
      ecEntryDate: formatLocalTodayYmd(),
      ecDetails: draft.ecDetails ?? [],
      attachments: [],
      sourceEcId: draft.sourceEcId ?? selecmedSourceEcId.value,
      detailsDeferred: draft.detailsDeferred,
      deferredDetailCount: draft.deferredDetailCount,
    }
    delete (formDraft as Record<string, unknown>).notifications
    if (draft.detailsDeferred) {
      message.info(
        t('logistics.manufacturing.engineering-change.ec-te.page.sourceEcInput.detailsDeferred', {
          count: draft.deferredDetailCount ?? 0,
        }),
      )
    }
    emit('draft-ready', formDraft)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.operation.failed'))
  } finally {
    draftLoading.value = false
  }
}

watch(
  () => tenantStore.companyCode,
  async () => {
    const mapped = await loadPlantMapping()
    if (mapped) {
      currentPage.value = getTaktDefaultPageIndex()
      selecmedSourceEcId.value = ''
      void loadData()
      return
    }
    dataSource.value = []
    total.value = 0
  },
  { immediate: true },
)

defineExpose({
  resetFields,
  loadData,
})
</script>

<style scoped lang="css">
.source-ec-input {
  height: 100%;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
}

.source-ec-input-query {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.source-ec-input-actions {
  flex-shrink: 0;
  margin-top: 8px;
}

.source-ec-input-table-wrap {
  flex: 1 1 auto;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
}
</style>
