<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-actual-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：采购实绩推移（各月因单价涨跌造成的采购金额浮动） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <PurchaseActualTrendQueryForm
      v-model:plant-code="plantCode"
      v-model:period-range="periodRange"
      v-model:supplier-code="supplierCode"
      v-model:material-code="materialCode"
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
      :show-column-setting="true"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!plantCode?.trim() || !hasRows"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      :right-actions="trendFilterActions"
      export-permission="logistics:procurement:purchase:actual:trend:export"
      @export="handleExport"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <PurchaseActualTrendPanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :trend-filter="trendFilter"
      :plant-code="plantCode"
      :period-range="periodRange"
      :supplier-code="supplierCode"
      :material-code="materialCode"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 采购实绩推移（各月因单价涨跌造成的采购金额浮动）
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
import { useTenantStore } from '@/stores/identity/useTenantStore'
import PurchaseActualTrendQueryForm from './components/PurchaseActualTrendQueryForm.vue'
import PurchaseActualTrendPanel from './components/PurchaseActualTrendPanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.procurement.purchase-actual-trend.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)

/** 默认工厂=公司 RelatedPlant；期间=前三月（对齐成本差异推移） */
const { applyDefaultPeriodRange, applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 供应商编码 */
const supplierCode = ref<string | undefined>()
/** 物料编码 */
const materialCode = ref<string | undefined>()
/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 涨跌筛选：默认空=全部 */
const trendFilter = ref('')
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
  },
])
/** 明细面板 */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
  openDetailColumnSetting?: () => void
} | null>(null)

/** 打开明细列设置 */
function handleColumnSetting() {
  panelRef.value?.openDetailColumnSetting?.()
}

/** 查询（工厂/期间必选，供应商与物料可空） */
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

/**
 * 涨跌筛选
 * @param {string} value 筛选码
 */
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


/** 清空工厂及下游级联 */
function clearPlantCascade() {
  supplierCode.value = undefined
  materialCode.value = undefined
}

/** 重置 */
async function handleReset() {
  clearPlantCascade()
  await applyAnalysisQueryDefaults()
  trendFilter.value = ''
  hasRows.value = false
  panelRef.value?.clear?.()
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
    clearPlantCascade()
    hasRows.value = false
    panelRef.value?.clear?.()
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  await applyAnalysisQueryDefaults()
  void getTaktDefaultPageSize()
})
</script>
