<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/outbound-actual-trend/components -->
<!-- 文件名称：OutboundActualTrendQueryForm.vue -->
<!-- 功能描述：物料出库推移查询栏（工厂/过账年月必选；客户、物料可选） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar outbound-actual-trend-query-bar">
    <div class="outbound-actual-trend-query-bar__fields min-w-0 flex flex-1 flex-nowrap items-center gap-x-3 overflow-x-auto">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :disabled-date="disabledDate"
          class="outbound-actual-trend-query-bar__control outbound-actual-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.materialdocumentitem.customercode') }}</span>
        <TaktSelect
          :key="customerSelectKey"
          v-model:value="customerCode"
          :api-url="customerOptionsUrl"
          :api-params="customerApiParams"
          :disabled="!plantCode?.trim()"
          class="outbound-actual-trend-query-bar__control outbound-actual-trend-query-bar__control--customer"
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
          class="outbound-actual-trend-query-bar__control outbound-actual-trend-query-bar__control--material"
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
 * 物料出库推移查询栏：工厂/过账年月必选；客户、物料可选
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getOutboundActualTrendMaterialOptionsUrl,
  getOutboundActualTrendPlantOptionsUrl,
  getOutboundActualTrendCustomerOptionsUrl,
} from '@/api/logistics/materials/outboundActualTrend.api'

/** 工厂代码（必选） */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
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
const localePrefix = 'logistics.materials.outbound-actual-trend.page'
/** 客户下拉刷新键 */
const customerSelectKey = ref(0)
/** 物料下拉刷新键 */
const materialSelectKey = ref(0)
/** 推移本表级联选项 URL（TaktOutboundActualTrends） */
const plantOptionsUrl = getOutboundActualTrendPlantOptionsUrl()
const customerOptionsUrl = getOutboundActualTrendCustomerOptionsUrl()
const materialOptionsUrl = getOutboundActualTrendMaterialOptionsUrl()

/** 工厂 → 客户 */
const customerApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 物料选项：工厂；选了客户则再收窄 */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  const customer = customerCode.value?.trim()
  return customer
    ? { plantCode: plant, customerCode: customer }
    : { plantCode: plant }
})

/** 工厂变更：清空下游并强制重挂载 */
watch(
  () => plantCode.value,
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
  flex-wrap: nowrap;
}

.outbound-actual-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.outbound-actual-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.outbound-actual-trend-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.outbound-actual-trend-query-bar__control--material {
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
