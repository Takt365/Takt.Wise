<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/defect-monthly/components -->
<!-- 文件名称：DefectMonthlyQueryForm.vue -->
<!-- 功能描述：不良推移查询栏（当前页签的工厂与机种） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar defect-monthly-query-bar">
    <div class="defect-monthly-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          class="defect-monthly-query-bar__control defect-monthly-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.modelCode`) }}</span>
        <TaktSelect
          :key="`${props.statTab}-model`"
          v-model:value="modelCode"
          :api-url="modelOptionsUrl"
          :api-params="modelApiParams"
          :disabled="!plantCode?.trim()"
          class="defect-monthly-query-bar__control defect-monthly-query-bar__control--model"
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
 * 不良推移查询栏：工厂、期间必填，机种取自当前页签
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getDefectMonthlyTrendModelOptionsUrl,
  getDefectMonthlyTrendPlantOptionsUrl,
} from '@/api/logistics/manufacturing/defect/defectMonthly.api'

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 机种（可空） */
const modelCode = defineModel<string | undefined>('modelCode')
const props = defineProps<{
  /** 页签 assy / pcba */
  statTab: string
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.defect.defect-monthly.page'
const plantOptionsUrl = getDefectMonthlyTrendPlantOptionsUrl()
const modelOptionsUrl = getDefectMonthlyTrendModelOptionsUrl()

/** 工厂只取当前页签 */
const plantApiParams = computed(() => ({ statTab: props.statTab }))

/** 机种只取当前页签的产出不良表 */
const modelApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant, defectCategory: props.statTab }
})

/** 工厂变更：清空机种 */
watch(
  () => plantCode.value,
  () => {
    modelCode.value = undefined
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

.defect-monthly-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.defect-monthly-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.defect-monthly-query-bar__control--category {
  width: 12rem;
  min-width: 9rem;
}

.defect-monthly-query-bar__control--model {
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
