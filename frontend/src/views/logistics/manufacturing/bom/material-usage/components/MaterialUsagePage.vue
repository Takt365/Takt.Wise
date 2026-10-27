<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-usage/components -->
<!-- 文件名称：MaterialUsagePage.vue -->
<!-- 功能描述：通用物料 / 专用物料页壳（查询工厂、核算年月、机种可选，导出） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <MaterialUsageQueryForm
      v-model:plant-code="queryPlantCode"
      v-model:component-code="queryComponentCode"
      v-model:model-code="queryModelCode"
      v-model:costing-month="costingMonth"
      :kind="kind"
      :locale-prefix="localePrefix"
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
      :export-permission="exportPermission"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <MaterialUsagePanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 min-w-0 flex-1"
      :kind="kind"
      :locale-prefix="localePrefix"
      :plant-code="queryPlantCode"
      :component-code="queryComponentCode"
      :model-code="queryModelCode"
      :costing-month="costingMonth"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 通用物料 / 专用物料页：查询后才加载，导出沿用成本报表文件名
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getBomCostOptionPlantOptions } from '@/api/logistics/manufacturing/bom/costOption.api'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import MaterialUsagePanel from './MaterialUsagePanel.vue'
import MaterialUsageQueryForm from './MaterialUsageQueryForm.vue'

/** 列表分类 */
type MaterialUsageKind = 'common' | 'dedicated'

const props = defineProps<{
  /** common 通用物料，dedicated 专用物料 */
  kind: MaterialUsageKind
  /** 静态文案前缀 */
  localePrefix: string
  /** 导出权限码 */
  exportPermission: string
}>()

const { t } = useI18n()
const tenantStore = useTenantStore()
/** 工厂 */
const queryPlantCode = ref<string | undefined>(undefined)
/** 物料，空=不按物料收窄 */
const queryComponentCode = ref<string | undefined>(undefined)
/** 机种，空=全部 */
const queryModelCode = ref<string | undefined>(undefined)
/** 核算年月 yyyy-MM */
const costingMonth = ref<string | undefined>(undefined)
/** 列表 loading */
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

/** 可否导出 */
const canExport = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && !!costingMonth.value?.trim()
    && hasRows.value,
)

/** 文案前缀 */
const localePrefix = computed(() => props.localePrefix)
/** 分类 */
const kind = computed(() => props.kind)
/** 导出权限 */
const exportPermission = computed(() => props.exportPermission)

/**
 * 校验查询条件
 * @returns 是否通过
 */
function validateQuery(): boolean {
  if (!queryPlantCode.value?.trim()) {
    message.warning(t(`${props.localePrefix}.selectPlantRequired`))
    return false
  }
  if (!costingMonth.value?.trim()) {
    message.warning(t(`${props.localePrefix}.selectPeriodRequired`))
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

/** 默认核算年月：上月（当月无实绩） */
function applyDefaultCostingMonth() {
  costingMonth.value = buildDefaultCostingPeriodRange(1)[1]
}

/**
 * 默认工厂取当前公司关联工厂
 * @returns 无
 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched: string | undefined
  if (related) {
    try {
      const plants = await getBomCostOptionPlantOptions()
      const hit = (plants ?? []).find(
        (item) => String(item.dictValue ?? '').trim().toLowerCase() === related.toLowerCase(),
      )
      matched = hit ? String(hit.dictValue).trim() : undefined
    } catch {
      matched = undefined
    }
  }
  queryPlantCode.value = matched
  queryComponentCode.value = undefined
  queryModelCode.value = undefined
}

/** 重置为默认工厂与期间，并清空表 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  applyDefaultCostingMonth()
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 导出 */
async function handleExport() {
  if (!validateQuery()) {
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${props.localePrefix}.exportFailed`))
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
  applyDefaultCostingMonth()
  await applyDefaultPlantFromCompany()
})
</script>
