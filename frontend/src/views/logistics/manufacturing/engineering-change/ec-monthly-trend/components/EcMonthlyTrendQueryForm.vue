<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-monthly-trend/components -->
<!-- 文件名称：EcMonthlyTrendQueryForm.vue -->
<!-- 功能描述：月设变/月实施推移查询栏：TaktQueryBar + 录入期间 #leading -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktQueryBar
    v-model="keywordModel"
    :loading="props.loading"
    :placeholder="t('common.page.form.placeholder.search')"
    @search="emit('search')"
    @reset="handleReset"
  >
    <template #leading>
      <span class="whitespace-nowrap text-sm text-text-secondary">{{ t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange') }}</span>
      <a-range-picker
        v-model:value="periodRange"
        picker="month"
        format="YYYY-MM"
        value-format="YYYY-MM"
        class="w-64 min-w-56"
        :placeholder="[
          t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
          t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
        ]"
        :disabled-date="disabledDate"
        @calendar-change="onCalendarChange"
        @open-change="onOpenChange"
      />
    </template>
  </TaktQueryBar>
</template>

<script setup lang="ts">
/**
 * 月设变/月实施推移查询栏：录入期间固定宽度，关键词占剩余宽度
 */
import { useI18n } from 'vue-i18n'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { TAKT_ANALYSIS_PERIOD_MAX_MONTHS } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'

/** 期间最多 12 个月，可选到本月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })

/** 年月区间，对应录入期间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 关键词，按设变号模糊查询 */
const ecCode = defineModel<string | undefined>('ecCode')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()

/** TaktQueryBar 关键字与可空 ecCode 互转 */
const keywordModel = computed({
  get: () => ecCode.value ?? '',
  set: (value: string) => {
    ecCode.value = value.trim() ? value : undefined
  },
})

/** 重置：关键字已由 TaktQueryBar 清空，转发给父级 */
function handleReset() {
  emit('reset')
}
</script>
