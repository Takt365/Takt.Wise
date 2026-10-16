<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/so-price-scr-deviation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：SO价偏离SCR（同生效周期订单折算单价与销售条件不一致） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <SoPriceScrDeviationQueryForm
      v-model:plant-code="plantCode"
      v-model:period-range="periodRange"
      v-model:customer-code="customerCode"
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
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!plantCode?.trim() || !hasRows"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      export-permission="logistics:sales:so:price:scr:deviation:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <SoPriceScrDeviationPanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :plant-code="plantCode"
      :period-range="periodRange"
      :customer-code="customerCode"
      :material-code="materialCode"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * SO价偏离SCR
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import SoPriceScrDeviationQueryForm from './components/SoPriceScrDeviationQueryForm.vue'
import SoPriceScrDeviationPanel from './components/SoPriceScrDeviationPanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.sales.so-price-scr-deviation.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)

/** 默认工厂=公司 RelatedPlant；期间=前三月（对齐成本差异推移） */
const { applyDefaultPeriodRange, applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 客户 */
const customerCode = ref<string | undefined>()
/** 物料 */
const materialCode = ref<string | undefined>()
/** 面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据 */
const hasRows = ref(false)
/** 面板 ref */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/**
 * 查询
 * @returns {Promise<void>}
 */
async function handleSearch(): Promise<void> {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  await panelRef.value?.reload?.()
}

/**
 * 刷新
 */
function handleRefresh(): void {
  void panelRef.value?.reload?.()
}

/**
 * 默认核算年月
 */

/**
 * 清空工厂及下游
 */
function clearPlantCascade(): void {
  customerCode.value = undefined
  materialCode.value = undefined
}

/**
 * 重置
 */
async function handleReset() {
  clearPlantCascade()
  await applyAnalysisQueryDefaults()
  hasRows.value = false
  panelRef.value?.clear?.()
}

/**
 * 导出
 * @returns {Promise<void>}
 */
async function handleExport(): Promise<void> {
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
