<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-usage/components -->
<!-- 文件名称：MaterialUsagePanel.vue -->
<!-- 功能描述：通用物料 / 专用物料表（期间、机种、组件、用量清单、行成本） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-1 flex-col overflow-hidden">
    <p class="mb-2 shrink-0 text-sm text-text-secondary">
      {{ summaryText }}
    </p>
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
        entity-scope="company"
        :columns="columns"
        :id-column-key="'usageKey'"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="column.key === 'lineCost'">
            {{ formatCost((record as BomMaterialUsageRow).lineCost) }}
          </template>
          <template v-else-if="column.key === 'usageText'">
            <a-tooltip
              v-if="String(text ?? '').trim()"
              placement="topLeft"
              :overlay-style="longTextTooltipStyle"
            >
              <template #title>
                <div class="max-h-80 max-w-md overflow-auto whitespace-pre-wrap break-all text-xs">
                  {{ text }}
                </div>
              </template>
              <span class="block max-w-full truncate">{{ text }}</span>
            </a-tooltip>
            <span v-else>—</span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 通用物料 / 专用物料表：服务端分页，用量清单悬停查看全文
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportBomCommonMaterial,
  exportBomDedicatedMaterial,
  getBomCommonMaterialList,
  getBomDedicatedMaterialList,
} from '@/api/logistics/manufacturing/bom/materialUsage.api'
import type { BomMaterialUsageQuery, BomMaterialUsageRow } from '@/types/logistics/manufacturing/bom/materialUsage.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomExportFileName'

/** 列表分类 */
type MaterialUsageKind = 'common' | 'dedicated'

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const props = defineProps<{
  /** common 通用物料，dedicated 专用物料 */
  kind: MaterialUsageKind
  /** 静态文案前缀 */
  localePrefix: string
  /** 工厂 */
  plantCode?: string
  /** 物料，空=不按物料收窄 */
  componentCode?: string
  /** 机种，空=全部 */
  modelCode?: string
  /** 核算年月 yyyy-MM */
  costingMonth?: string
}>()

/** 长文本 Tooltip 样式 */
const longTextTooltipStyle = { maxWidth: '480px' } as const
const { t } = useI18n()
/** 表体 */
const rows = ref<BomMaterialUsageRow[]>([])
/** 总行数 */
const total = ref(0)
/** 页码 */
const pageIndex = ref(getTaktDefaultPageIndex())
/** 页大小 */
const pageSize = ref(getTaktDefaultPageSize())
/** 是否已经查询过 */
const searched = ref(false)
/** 表格滚动高度 */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 表格容器 */
const tableWrapRef = ref<HTMLElement | null>(null)

/** 列 */
const columns = computed<TableColumnsType>(() => [
  { title: t(`${props.localePrefix}.columns.period`), dataIndex: 'costingPeriod', key: 'costingPeriod', width: 110, ellipsis: true },
  { title: t(`${props.localePrefix}.columns.model`), dataIndex: 'modelCode', key: 'modelCode', width: 140, ellipsis: true },
  { title: t(`${props.localePrefix}.columns.component`), dataIndex: 'componentCode', key: 'componentCode', width: 160, ellipsis: true },
  { title: t(`${props.localePrefix}.columns.description`), dataIndex: 'componentDescription', key: 'componentDescription', width: 220, ellipsis: true },
  { title: t(`${props.localePrefix}.columns.productCount`), dataIndex: 'productCount', key: 'productCount', width: 90, align: 'right' },
  { title: t(`${props.localePrefix}.columns.usage`), dataIndex: 'usageText', key: 'usageText', width: 280, ellipsis: true },
  { title: t(`${props.localePrefix}.columns.lineCost`), dataIndex: 'lineCost', key: 'lineCost', width: 120, align: 'right' },
])

/** 查询摘要 */
const summaryText = computed(() => {
  if (!searched.value) {
    return t(`${props.localePrefix}.hint`)
  }
  return t(`${props.localePrefix}.summary`, {
    plant: props.plantCode?.trim() || '—',
    period: props.costingMonth?.trim() || '—',
    material: props.componentCode?.trim() || '—',
    model: props.modelCode?.trim() || '—',
    total: total.value,
  })
})

/**
 * 行键
 * @param record 行
 * @returns 机种|组件
 */
function getRowKey(record: BomMaterialUsageRow): string {
  return record.usageKey
}

/**
 * 行成本展示
 * @param value 金额
 * @returns 文本
 */
function formatCost(value: number | null | undefined): string {
  if (value == null || Number.isNaN(Number(value))) {
    return '—'
  }
  return String(value)
}

/**
 * 组装查询
 * @returns 查询；条件不足时 undefined
 */
function buildQuery(): BomMaterialUsageQuery | undefined {
  const plant = props.plantCode?.trim()
  const costingPeriod = props.costingMonth?.trim()
  if (!plant || !costingPeriod) {
    return undefined
  }
  const component = props.componentCode?.trim()
  const model = component ? undefined : props.modelCode?.trim()
  return {
    plantCode: plant,
    componentCode: component || undefined,
    modelCode: model || undefined,
    costingPeriod,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
  }
}

/** 清空表 */
function clear() {
  rows.value = []
  total.value = 0
  searched.value = false
  hasRows.value = false
}

/** 从第一页重新查询 */
async function reload() {
  pageIndex.value = getTaktDefaultPageIndex()
  searched.value = true
  await loadData()
}

/** 加载当前页 */
async function loadData() {
  const query = buildQuery()
  if (!query || !searched.value) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = props.kind === 'common'
      ? await getBomCommonMaterialList(query)
      : await getBomDedicatedMaterialList(query)
    rows.value = result?.data ?? []
    total.value = result?.total ?? 0
    hasRows.value = total.value > 0
  } catch (error: unknown) {
    rows.value = []
    total.value = 0
    hasRows.value = false
    const err = error as { message?: string }
    message.error(err?.message || t(`${props.localePrefix}.queryFailed`))
  } finally {
    loading.value = false
    nextTick(() => recalcTableScrollY())
  }
}

/**
 * 翻页
 * @param page 页码
 * @param size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 导出当前查询的全量行 */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${props.localePrefix}.selectPlantRequired`))
    return
  }
  try {
    const sheetTitle = t(`${props.localePrefix}.title`)
    const nameParts = [query.plantCode, query.componentCode, query.modelCode]
    const exportBase = buildBomExportBaseName(sheetTitle, nameParts)
    const exportCall = props.kind === 'common' ? exportBomCommonMaterial : exportBomDedicatedMaterial
    const exportMeta = await exportCall(
      { ...query, pageIndex: 1, pageSize: 100000 },
      sheetTitle,
      buildBomExportFileName(sheetTitle, nameParts),
    )
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase: exportBase,
    })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t(`${props.localePrefix}.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${props.localePrefix}.exportFailed`))
  }
}

/** 按容器高度计算表格滚动 */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) {
    return
  }
  const y = Math.floor(wrap.clientHeight - 47)
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, y)
}

useTableRefresh(loadData)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageSize.value = getTaktDefaultPageSize()
  nextTick(() => recalcTableScrollY())
})

defineExpose({ reload, handleExport, clear })
</script>
