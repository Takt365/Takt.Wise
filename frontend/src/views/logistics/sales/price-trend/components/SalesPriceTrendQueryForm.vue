<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price-trend/components -->
<!-- 文件名称：SalesPriceTrendQueryForm.vue -->
<!-- 功能描述：销售价格推移查询栏（工厂/期间/条件类型必选；客户、物料可选） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar sales-price-trend-query-bar">
    <div class="sales-price-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          :api-url="plantOptionsUrl"
          class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--plant"
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
          class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.salescondition.pricetype') }}</span>
        <TaktSelect
          :key="priceTypeSelectKey"
          v-model:value="priceType"
          :api-url="priceTypeOptionsUrl"
          :api-params="priceTypeApiParams"
          :disabled="!plantCode?.trim()"
          class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--price-type"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.salescondition.customercode') }}</span>
        <TaktSelect
          :key="customerSelectKey"
          v-model:value="customerCode"
          :api-url="customerOptionsUrl"
          :api-params="customerApiParams"
          :disabled="!plantCode?.trim() || !priceType?.trim()"
          class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--customer"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.salescondition.materialcode') }}</span>
        <TaktSelect
          :key="materialSelectKey"
          v-model:value="materialCode"
          :api-url="materialOptionsUrl"
          :api-params="materialApiParams"
          :disabled="!plantCode?.trim() || !priceType?.trim()"
          class="sales-price-trend-query-bar__control sales-price-trend-query-bar__control--material"
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
 * 销售价格推移查询栏：工厂/期间/条件类型必选；客户、物料可选
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getSalesPriceTrendCustomerOptionsUrl,
  getSalesPriceTrendMaterialOptionsUrl,
  getSalesPriceTrendPlantOptionsUrl,
  getSalesPriceTrendPriceTypeOptionsUrl,
} from '@/api/logistics/sales/priceTrend.api'

/** 工厂代码（必选） */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 条件类型（必选） */
const priceType = defineModel<string | undefined>('priceType')
/** 客户编码（可选） */
const customerCode = defineModel<string | undefined>('customerCode')
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
const localePrefix = 'logistics.sales.price-trend.page'
/** 条件类型下拉刷新键 */
const priceTypeSelectKey = ref(0)
/** 客户下拉刷新键 */
const customerSelectKey = ref(0)
/** 物料下拉刷新键 */
const materialSelectKey = ref(0)
/** 推移本表级联选项 URL（TaktSalesPriceTrends） */
const plantOptionsUrl = getSalesPriceTrendPlantOptionsUrl()
const priceTypeOptionsUrl = getSalesPriceTrendPriceTypeOptionsUrl()
const customerOptionsUrl = getSalesPriceTrendCustomerOptionsUrl()
const materialOptionsUrl = getSalesPriceTrendMaterialOptionsUrl()

/** 工厂 → 条件类型 */
const priceTypeApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 工厂 + 条件类型 → 客户 */
const customerApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = priceType.value?.trim()
  if (!plant || !type) {
    return undefined
  }
  return { plantCode: plant, priceType: type }
})

/** 物料选项：工厂 + 条件类型；选了客户则再收窄 */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const type = priceType.value?.trim()
  if (!plant || !type) {
    return undefined
  }
  const customer = customerCode.value?.trim()
  return customer
    ? { plantCode: plant, priceType: type, customerCode: customer }
    : { plantCode: plant, priceType: type }
})

/** 工厂变更：清空下游并强制重挂载 */
watch(
  () => plantCode.value,
  () => {
    priceType.value = undefined
    customerCode.value = undefined
    materialCode.value = undefined
    priceTypeSelectKey.value += 1
    customerSelectKey.value += 1
    materialSelectKey.value += 1
  },
)

/** 条件类型变更：清空客户/物料并强制重挂载 */
watch(
  () => priceType.value,
  () => {
    customerCode.value = undefined
    materialCode.value = undefined
    customerSelectKey.value += 1
    materialSelectKey.value += 1
  },
)

/** 客户变更：清空物料并强制重挂载 */
watch(
  () => customerCode.value,
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
}

.sales-price-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.sales-price-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.sales-price-trend-query-bar__control--price-type {
  width: 10rem;
  min-width: 8rem;
}

.sales-price-trend-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.sales-price-trend-query-bar__control--material {
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
