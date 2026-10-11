<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：机种成本推移（工厂、期间、物料类型、机种可选；一行一个成品） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <ModelCostTrendQueryForm
      v-model:plant-code="queryPlantCode"
      v-model:material-type="queryMaterialType"
      v-model:model-code="queryModelCode"
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
      export-permission="logistics:manufacturing:bom:model:cost:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <ModelCostTrendPanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 min-w-0 flex-1"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 机种成本推移 · 查询栏 + 工具栏
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { getBomCostOptionPlantOptions } from '@/api/logistics/manufacturing/bom/costOption.api'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import ModelCostTrendQueryForm from './components/ModelCostTrendQueryForm.vue'
import { provideBomMaterialCostAnalysisMasterContext } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/useMaterialCostAnalysisMasterContext'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import ModelCostTrendPanel from './components/ModelCostTrendPanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.bom.model-cost-trend.page'
const tenantStore = useTenantStore()
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCode,
  periodRange,
} = provideBomMaterialCostAnalysisMasterContext()

/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 明细面板 */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 查询条件是否满足 */
const canQuery = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && !!queryMaterialType.value?.trim()
    && !!periodRange.value?.[0]
    && !!periodRange.value?.[1],
)

/** 可否导出 */
const canExport = computed(() => canQuery.value && hasRows.value)

/**
 * 校验查询条件
 * @returns 是否通过
 */
function validateQuery(): boolean {
  if (!queryPlantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return false
  }
  if (!queryMaterialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return false
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return false
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return false
  }
  return true
}

/** 查询 */
function handleSearch() {
  if (!validateQuery()) {
    return
  }
  void panelRef.value?.reload?.()
}

/** 刷新 */
function handleRefresh() {
  if (!validateQuery()) {
    return
  }
  void panelRef.value?.reload?.()
}

/** 默认核算年月 */
function applyDefaultPeriodRange() {
  periodRange.value = buildDefaultCostingPeriodRange(3)
}

/**
 * 默认工厂：当前公司 RelatedPlant 仅当出现在 plant-options（RelatedPlant∩本表）时选中；无则清空
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
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  queryMaterialType.value = undefined
  applyDefaultPeriodRange()
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 导出 */
async function handleExport() {
  if (!validateQuery()) {
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${localePrefix}.exportFailed`))
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
