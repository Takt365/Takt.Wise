<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/so-price-scr-deviation/components -->
<!-- 文件名称：SoPriceScrDeviationQueryForm.vue -->
<!-- 功能描述：SO价偏离SCR查询栏（工厂/期间必选；客户、物料按工厂+期间级联；物料 remote-search） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar so-price-scr-deviation-query-bar">
    <div class="so-price-scr-deviation-query-bar__fields min-w-0 flex flex-1 flex-nowrap items-center gap-x-3 overflow-x-auto">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :disabled-date="disabledDate"
          class="so-price-scr-deviation-query-bar__control so-price-scr-deviation-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`),
          ]"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.salescondition.customercode') }}</span>
        <TaktSelect
          :key="customerSelectKey"
          v-model:value="customerCode"
          :api-url="customerOptionsUrl"
          :api-params="customerApiParams"
          :disabled="!canSelectPartner"
          class="so-price-scr-deviation-query-bar__control so-price-scr-deviation-query-bar__control--customer"
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
          :disabled="!canSelectMaterial"
          class="so-price-scr-deviation-query-bar__control so-price-scr-deviation-query-bar__control--material"
          allow-clear
          show-search
          remote-search
          virtual
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
 * SO价偏离SCR查询栏：工厂/期间必选；客户、物料按工厂+期间（+客户）级联
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getSoPriceScrDeviationMaterialOptionsUrl,
  getSoPriceScrDeviationPlantOptionsUrl,
  getSoPriceScrDeviationCustomerOptionsUrl,
} from '@/api/logistics/sales/soPriceScrDeviation.api'

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

/** 工厂代码（必选） */
const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 客户 */
const customerCode = defineModel<string | undefined>('customerCode')
/** 物料 */
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
const localePrefix = 'logistics.sales.so-price-scr-deviation.page'
/** 客户下拉刷新键 */
const customerSelectKey = ref(0)
/** 物料下拉刷新键 */
const materialSelectKey = ref(0)
const plantOptionsUrl = getSoPriceScrDeviationPlantOptionsUrl()
const customerOptionsUrl = getSoPriceScrDeviationCustomerOptionsUrl()
const materialOptionsUrl = getSoPriceScrDeviationMaterialOptionsUrl()

/** 是否已选工厂+完整期间 */
const hasPeriod = computed(
  () => !!periodRange.value?.[0]?.trim() && !!periodRange.value?.[1]?.trim(),
)

/** 客户可选：工厂 + 期间 */
const canSelectPartner = computed(() => !!plantCode.value?.trim() && hasPeriod.value)

/** 物料可选：工厂 + 期间 */
const canSelectMaterial = computed(() => !!plantCode.value?.trim() && hasPeriod.value)

/** 期间起止（与分析 API 一致：YYYY-MM-01） */
const periodApiParams = computed(() => {
  const start = periodRange.value?.[0]?.trim()
  const end = periodRange.value?.[1]?.trim()
  if (!start || !end) {
    return undefined
  }
  return {
    periodDateStart: `${start}-01`,
    periodDateEnd: `${end}-01`,
  }
})

/** 客户级联：工厂 + 期间 */
const customerApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const period = periodApiParams.value
  if (!plant || !period) {
    return undefined
  }
  return { plantCode: plant, ...period }
})

/** 物料级联：工厂 + 期间；选了客户再收窄 */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const period = periodApiParams.value
  if (!plant || !period) {
    return undefined
  }
  const customer = customerCode.value?.trim()
  return customer
    ? { plantCode: plant, customerCode: customer, ...period }
    : { plantCode: plant, ...period }
})

watch(
  () => plantCode.value,
  () => {
    customerCode.value = undefined
    materialCode.value = undefined
    customerSelectKey.value += 1
    materialSelectKey.value += 1
  },
)

watch(
  () => [periodRange.value?.[0], periodRange.value?.[1]] as const,
  () => {
    customerCode.value = undefined
    materialCode.value = undefined
    customerSelectKey.value += 1
    materialSelectKey.value += 1
  },
)

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
  flex-wrap: nowrap;
}

.so-price-scr-deviation-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.so-price-scr-deviation-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.so-price-scr-deviation-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.so-price-scr-deviation-query-bar__control--material {
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
</style>
