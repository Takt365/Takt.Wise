<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-actual-trend/components -->
<!-- 文件名称：PurchaseActualTrendQueryForm.vue -->
<!-- 功能描述：采购实绩推移查询栏（工厂/过账年月必选；供应商、物料可选） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar purchase-actual-trend-query-bar">
    <div class="purchase-actual-trend-query-bar__fields min-w-0 flex flex-1 flex-nowrap items-center gap-x-3 overflow-x-auto">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          :api-url="plantOptionsUrl"
          class="purchase-actual-trend-query-bar__control purchase-actual-trend-query-bar__control--plant"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :disabled-date="disabledDate"
          class="purchase-actual-trend-query-bar__control purchase-actual-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.materialdocumentitem.suppliercode') }}</span>
        <TaktSelect
          :key="supplierSelectKey"
          v-model:value="supplierCode"
          :api-url="supplierOptionsUrl"
          :api-params="supplierApiParams"
          :disabled="!plantCode?.trim()"
          class="purchase-actual-trend-query-bar__control purchase-actual-trend-query-bar__control--supplier"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.materialdocumentitem.materialcode') }}</span>
        <TaktSelect
          :key="materialSelectKey"
          v-model:value="materialCode"
          :api-url="materialOptionsUrl"
          :api-params="materialApiParams"
          :disabled="!plantCode?.trim()"
          class="purchase-actual-trend-query-bar__control purchase-actual-trend-query-bar__control--material"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
    </div>
    <a-space class="query-actions">
      <a-button
        class="takt-button-query"
        :loading="props.loading"
        @click="emit('search')"
      >
        <template #icon>
          <RiSearchLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.query') }}
      </a-button>
      <a-button
        class="takt-button-reset"
        :disabled="props.loading"
        @click="emit('reset')"
      >
        <template #icon>
          <RiRefreshLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.reset') }}
      </a-button>
    </a-space>
  </div>
</template>

<script setup lang="ts">
/**
 * 采购实绩推移查询栏：工厂/过账年月必选；供应商、物料可选
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getPurchaseActualTrendMaterialOptionsUrl,
  getPurchaseActualTrendPlantOptionsUrl,
  getPurchaseActualTrendSupplierOptionsUrl,
} from '@/api/logistics/procurement/purchaseActualTrend.api'

/** 工厂代码（必选） */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 供应商编码（可选） */
const supplierCode = defineModel<string | undefined>('supplierCode')
/** 物料编码（可选） */
const materialCode = defineModel<string | undefined>('materialCode')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.procurement.purchase-actual-trend.page'
/** 供应商下拉刷新键 */
const supplierSelectKey = ref(0)
/** 物料下拉刷新键 */
const materialSelectKey = ref(0)
/** 推移本表级联选项 URL（TaktPurchaseActualTrends） */
const plantOptionsUrl = getPurchaseActualTrendPlantOptionsUrl()
const supplierOptionsUrl = getPurchaseActualTrendSupplierOptionsUrl()
const materialOptionsUrl = getPurchaseActualTrendMaterialOptionsUrl()

/** 工厂 → 供应商 */
const supplierApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 物料选项：工厂；选了供应商则再收窄 */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  const supplier = supplierCode.value?.trim()
  return supplier
    ? { plantCode: plant, supplierCode: supplier }
    : { plantCode: plant }
})

/** 工厂变更：清空下游并强制重挂载 */
watch(
  () => plantCode.value,
  () => {
    supplierCode.value = undefined
    materialCode.value = undefined
    supplierSelectKey.value += 1
    materialSelectKey.value += 1
  },
)

/** 供应商变更：清空物料并强制重挂载 */
watch(
  () => supplierCode.value,
  () => {
    materialCode.value = undefined
    materialSelectKey.value += 1
  },
)
</script>

<style scoped>
.takt-query-bar {
  margin: 4px;
  padding: 4px;
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  box-sizing: border-box;
  flex-wrap: nowrap;
}

.purchase-actual-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.purchase-actual-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.purchase-actual-trend-query-bar__control--supplier {
  width: 12rem;
  min-width: 9rem;
}

.purchase-actual-trend-query-bar__control--material {
  width: 12rem;
  min-width: 9rem;
}

.query-actions {
  flex-shrink: 0;
}

.query-actions :deep(.ant-btn) {
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.query-actions :deep(.ant-btn .anticon) {
  margin-inline-end: 0 !important;
}
</style>
