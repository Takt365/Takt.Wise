<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend/components -->
<!-- 文件名称：ModelCostTrendQueryForm.vue -->
<!-- 功能描述：机种成本推移查询栏（工厂、期间、物料类型可空、机种可空） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <TaktQueryBar
    :show-keyword="false"
    :loading="props.loading"
    @search="emit('search')"
    @reset="emit('reset')"
  >
    <template #leading>
<div class="model-cost-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :disabled-date="disabledDate"
          class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
          @change="handlePeriodChange"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.bommaterialcost.materialtype') }}</span>
        <TaktSelect
          v-model:value="materialType"
          :options="materialTypeOptions"
          class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--type"
          allow-clear
          show-search
          :disabled="!canSelectType || materialTypeOptionsLoading"
          :placeholder="t(`${localePrefix}.materialTypeOptional`)"
          @change="handleMaterialTypeChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.bommaterialcost.modelcode') }}</span>
        <TaktSelect
          :key="`model-${modelSelectKey}-${materialType || ''}-${periodKey}`"
          v-model="modelCode"
          :api-url="modelOptionsUrl"
          :api-params="modelApiParams"
          class="model-cost-trend-query-bar__control model-cost-trend-query-bar__control--model"
          allow-clear
          show-search
          :disabled="!canSelectModel"
          :placeholder="t(`${localePrefix}.modelOptional`)"
        />
      </div>
    </div>
    </template>
  </TaktQueryBar>
</template>

<script setup lang="ts">
/**
 * 机种成本推移查询栏：工厂、期间、物料类型可空（空=全部类型，初次加载默认 FERT）、机种单选可空（空=全部机种）
 */
import { useI18n } from 'vue-i18n'
import {
  getBomCostOptionModelOptionsUrl,
  getBomCostOptionPlantOptionsUrl,
} from '@/api/logistics/manufacturing/bom/costOption.api'
import type { TaktSelectOption } from '@/types/common.types'
import { buildBomCostOptionParams, hasBomCostOptionPeriod } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomCostOptionParams'
import { loadBomMaterialTypeOptionsWithDefault } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialTypeOptions'

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 物料类型（可空=全部类型；初次加载默认 FERT） */
const materialType = defineModel<string | undefined>('materialType')
/** 机种编码（空=全部机种） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
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
const localePrefix = 'logistics.manufacturing.bom.model-cost-trend.page'
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)
/** 本表物料类型全量选项 */
const materialTypeOptions = ref<TaktSelectOption[]>([])
/** 物料类型选项 loading */
const materialTypeOptionsLoading = ref(false)
/** 当前 options 对应的工厂 */
const materialTypeOptionsPlant = ref('')
/** 选项请求序号 */
let materialTypeLoadToken = 0
const plantOptionsUrl = getBomCostOptionPlantOptionsUrl()
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()
/** 期间键 */
const periodKey = computed(
  () => `${periodRange.value?.[0] || ''}_${periodRange.value?.[1] || ''}`,
)
/** 期间是否可解析 */
const hasPeriod = computed(() => hasBomCostOptionPeriod(periodRange.value))
/** 类型：工厂 + 期间 */
const canSelectType = computed(() => !!plantCode.value?.trim() && hasPeriod.value)
/** 机种：工厂 + 期间即可；类型空则不过滤机种 */
const canSelectModel = computed(() => canSelectType.value)

/** 机种下拉参数（工厂 + 整个期间 + 类型） */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    materialType: materialType.value,
  }),
)

/** 刷新机种下拉 */
function refreshModelSelect() {
  modelSelectKey.value += 1
}

/**
 * 按工厂拉取物料类型全量选项
 * @param {string} plant 工厂
 * @returns {Promise<string | undefined>} 默认类型（优先 FERT）
 */
async function ensureMaterialTypeOptions(plant: string): Promise<string | undefined> {
  const token = ++materialTypeLoadToken
  materialTypeOptionsLoading.value = true
  try {
    const { options, defaultType } = await loadBomMaterialTypeOptionsWithDefault(
      plant,
      periodRange.value,
    )
    if (token !== materialTypeLoadToken) {
      return undefined
    }
    materialTypeOptions.value = options
    materialTypeOptionsPlant.value = plant
    return defaultType
  } finally {
    if (token === materialTypeLoadToken) {
      materialTypeOptionsLoading.value = false
    }
  }
}

/** 工厂变更：清空机种 */
function handlePlantChange() {
  modelCode.value = undefined
  refreshModelSelect()
}

/** 期间变更：重拉类型并清空机种 */
async function handlePeriodChange() {
  modelCode.value = undefined
  const p = plantCode.value?.trim()
  if (p && hasBomCostOptionPeriod(periodRange.value)) {
    await ensureMaterialTypeOptions(p)
  }
  refreshModelSelect()
}

/** 物料类型变更：清空机种；允许清空为全部类型 */
function handleMaterialTypeChange() {
  modelCode.value = undefined
  refreshModelSelect()
}

watch(
  plantCode,
  async (plant) => {
    const p = plant?.trim()
    if (!p || !hasBomCostOptionPeriod(periodRange.value)) {
      materialTypeLoadToken += 1
      materialTypeOptions.value = []
      materialTypeOptionsPlant.value = ''
      materialTypeOptionsLoading.value = false
      if (!p) {
        materialType.value = undefined
      }
      modelCode.value = undefined
      refreshModelSelect()
      return
    }
    modelCode.value = undefined
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
    refreshModelSelect()
  },
  { immediate: true },
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

.model-cost-trend-query-bar__control--plant,
.model-cost-trend-query-bar__control--model {
  width: 12rem;
  min-width: 8rem;
}

.model-cost-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.model-cost-trend-query-bar__control--type {
  width: 9rem;
  min-width: 7rem;
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
