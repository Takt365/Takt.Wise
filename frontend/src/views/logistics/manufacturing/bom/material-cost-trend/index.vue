<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：产品成本推移报表：必选单个产品 → 明细表组件月材料成本转置涨跌 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <material-cost-trend-query-form
      v-model:plant-code="queryPlantCode"
      v-model:material-type="queryMaterialType"
      v-model:model-code="queryModelCode"
      v-model:product-code="queryProductCode"
      v-model:period-range="periodRange"
      :loading="panelLoading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!canExport"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      :right-actions="toolbarRightActions"
      export-permission="logistics:manufacturing:bom:material:cost:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <MaterialCostTrendPanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 min-w-0 flex-1"
      :trend-filter="trendFilter"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 产品成本推移报表：必选单个产品，展示明细表组件行转置
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiListCheck,
} from '@remixicon/vue'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { getBomCostOptionPlantOptions } from '@/api/logistics/manufacturing/bom/costOption.api'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import MaterialCostTrendPanel from './components/MaterialCostTrendPanel.vue'
import MaterialCostTrendQueryForm from './components/MaterialCostItemAnalysisQueryForm.vue'
import { MATERIAL_COST_ANALYSIS_LOCALE_PREFIX } from './composables/useMaterialCostItemAnalysis'
import { provideBomMaterialCostAnalysisMasterContext } from './composables/useMaterialCostAnalysisMasterContext'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const tenantStore = useTenantStore()
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCode,
  queryProductCode,
  periodRange,
} = provideBomMaterialCostAnalysisMasterContext()

/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 涨跌筛选 */
const trendFilter = ref('')
/** 右侧：涨跌筛选（行序固定 ProductCode + 行号升序） */
const toolbarRightActions = computed<ToolBarAction[]>(() => [
  {
    key: 'trend-all',
    icon: RiListCheck,
    tooltip: t(`${localePrefix}.filter.all`),
    active: trendFilter.value === '',
    onClick: () => setTrendFilter(''),
  },
  {
    key: 'trend-changed',
    icon: RiArrowUpDownLine,
    tooltip: t(`${localePrefix}.filter.changed`),
    active: trendFilter.value === 'changed',
    onClick: () => setTrendFilter('changed'),
  },
  {
    key: 'trend-up',
    icon: RiArrowUpLine,
    tooltip: t(`${localePrefix}.trend.up`),
    active: trendFilter.value === 'up',
    onClick: () => setTrendFilter('up'),
  },
  {
    key: 'trend-down',
    icon: RiArrowDownLine,
    tooltip: t(`${localePrefix}.trend.down`),
    active: trendFilter.value === 'down',
    onClick: () => setTrendFilter('down'),
  }])
/** 明细面板 */
const panelRef = ref<{
  reload?: (trendFilterOverride?: string) => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 可否导出 */
const canExport = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && !!queryProductCode.value?.trim()
    && hasRows.value,
)

/** 查询 */
function handleSearch() {
  if (!queryPlantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!queryMaterialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return
  }
  if (!queryProductCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectProductRequired`))
    return
  }
  if (!periodRange.value?.[0]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  void panelRef.value?.reload?.()
}

/**
 * 涨跌筛选：点涨→up 列表，点跌→down 列表；显式把筛选码传给面板请求
 * @param value 空 / changed / up / down
 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
  void panelRef.value?.reload?.(value)
}

/** 刷新 */
function handleRefresh() {
  void panelRef.value?.reload?.()
}

/** 默认核算年月 */
function applyDefaultPeriodRange() {
  periodRange.value = buildDefaultCostingPeriodRange(3)
}

/**
 * 默认工厂：当前公司 RelatedPlant 仅当出现在 plant-options（RelatedPlant∩本表）时选中；无则清空；并清空机种/产品
 * @returns {Promise<void>}
 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched: string | undefined
  if (related) {
    try {
      const plants = await getBomCostOptionPlantOptions()
      const hit = (plants ?? []).find(
        (o) => String(o.dictValue ?? '').trim().toLowerCase() === related.toLowerCase(),
      )
      matched = hit ? String(hit.dictValue).trim() : undefined
    } catch {
      matched = undefined
    }
  }
  queryPlantCode.value = matched
  queryMaterialType.value = undefined
  queryModelCode.value = undefined
  queryProductCode.value = undefined
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  queryMaterialType.value = undefined
  applyDefaultPeriodRange()
  trendFilter.value = ''
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 导出 */
async function handleExport() {
  if (!canExport.value) {
    message.warning(t(`${localePrefix}.selectProductRequired`))
    return
  }
  exportLoading.value = true
  try {
    await panelRef.value?.handleExport?.()
  } finally {
    exportLoading.value = false
  }
}

watch(
  () => tenantStore.companyCode,
  () => {
    void (async () => {
      await applyDefaultPlantFromCompany()
      hasRows.value = false
      panelRef.value?.clear?.()
    })()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  applyDefaultPeriodRange()
  await applyDefaultPlantFromCompany()
  void getTaktDefaultPageSize()
})
</script>
