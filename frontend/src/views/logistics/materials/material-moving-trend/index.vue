<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-moving-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：物料月移动价格推移（物料×月份转置） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <MaterialMovingTrendQueryForm
      v-model:plant-code="plantCode"
      v-model:period-range="periodRange"
      v-model:valuation="valuation"
      v-model:material-code="materialCode"
      :plant-select-key="plantSelectKey"
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
      :export-disabled="!plantCode?.trim() || !hasRows"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      :right-actions="trendFilterActions"
      export-permission="logistics:materials:material:moving:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <MaterialMovingTrendPanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :trend-filter="trendFilter"
      :plant-code="plantCode"
      :period-range="periodRange"
      :valuation="valuation"
      :material-code="materialCode"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 物料月移动价格推移（仅物料价格 Tab；机种见独立菜单）
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
import { getMaterialMovingTrendPlantOptions } from '@/api/logistics/materials/materialMovingTrend.api'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import MaterialMovingTrendPanel from './components/MaterialMovingTrendPanel.vue'
import MaterialMovingTrendQueryForm from './components/MaterialMovingTrendQueryForm.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'

/** 静态 locales 前缀 */
const localePrefix = 'logistics.materials.material-moving-trend.page'
const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 工厂下拉重挂载键 */
const plantSelectKey = ref(0)
/** 期间年月 */
const periodRange = ref<[string, string] | null>(buildDefaultCostingPeriodRange(3))
/** 评估类别 */
const valuation = ref<string | undefined>()
/** 物料编码（可空） */
const materialCode = ref<string | undefined>()
/** 涨跌筛选：默认空=全部 */
const trendFilter = ref('')
/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 右侧涨跌筛选 */
const trendFilterActions = computed<ToolBarAction[]>(() => [
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
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 查询（工厂/期间必选；评估类别、物料编码可空） */
function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
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

/** 涨跌筛选 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
}

/** 刷新 */
function handleRefresh() {
  void panelRef.value?.reload?.()
}

/**
 * 默认工厂：当前公司 RelatedPlant 仅当出现在本页 plant-options（RelatedPlant∩本表）时选中；无则清空
 * @returns {Promise<void>}
 */
async function applyDefaultPlant(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched: string | undefined
  if (related) {
    try {
      const plants = await getMaterialMovingTrendPlantOptions()
      const hit = (plants ?? []).find(
        (o) => String(o.dictValue ?? '').trim().toLowerCase() === related.toLowerCase(),
      )
      matched = hit ? String(hit.dictValue).trim() : undefined
    } catch {
      matched = undefined
    }
  }
  plantCode.value = matched
  plantSelectKey.value += 1
}

/** 清空评估/物料与结果（工厂由 applyDefaultPlant 再写入） */
function clearDownstreamAndResult() {
  valuation.value = undefined
  materialCode.value = undefined
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 重置 */
async function handleReset() {
  clearDownstreamAndResult()
  periodRange.value = buildDefaultCostingPeriodRange(3)
  trendFilter.value = ''
  await applyDefaultPlant()
}

/** 导出 */
async function handleExport() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${localePrefix}.exportEmpty`))
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
    clearDownstreamAndResult()
    void applyDefaultPlant()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  periodRange.value = buildDefaultCostingPeriodRange(3)
  await applyDefaultPlant()
  void getTaktDefaultPageSize()
})
</script>
