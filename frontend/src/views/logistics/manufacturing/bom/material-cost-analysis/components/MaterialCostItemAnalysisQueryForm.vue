<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-analysis/components -->
<!-- 文件名称：MaterialCostItemAnalysisQueryForm.vue -->
<!-- 功能描述：BOM 成本分析查询栏：工厂 → 期间 → 物料类型(先拉全量再默认 FERT) → 机种 → 产品 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-cost-analysis-query-bar">
    <div class="material-cost-analysis-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          :api-url="plantOptionsUrl"
          class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--plant"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
          @change="handlePlantChange"
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
          class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--period"
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
          class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--type"
          :allow-clear="false"
          show-search
          :disabled="!canSelectType || materialTypeOptionsLoading"
          :placeholder="t('common.page.form.placeholder.selectonly')"
          @change="handleMaterialTypeChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.bommaterialcost.modelcode') }}</span>
        <TaktSelect
          :key="`model-${modelSelectKey}-${materialType || ''}-${periodKey}`"
          v-model:value="modelCode"
          :api-url="modelOptionsUrl"
          :api-params="modelApiParams"
          class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--model"
          allow-clear
          show-search
          :disabled="!canSelectModel"
          :placeholder="t('common.page.form.placeholder.selectonly')"
          @change="handleModelChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.bommaterialcost.productcode') }}</span>
        <TaktSelect
          :key="`product-${productSelectKey}-${materialType || ''}-${modelCode || ''}-${periodKey}`"
          v-model:value="productCode"
          :api-url="productOptionsUrl"
          :api-params="productApiParams"
          class="material-cost-analysis-query-bar__control material-cost-analysis-query-bar__control--product"
          allow-clear
          show-search
          :disabled="!canSelectProduct"
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
 * BOM 成本分析查询栏：
 * - 物料类型：先 get material-type-options 全量，再默认选中 FERT
 * - 机种/产品：本表 model-options / product-options（按已选类型过滤）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getBomCostOptionModelOptionsUrl,
  getBomCostOptionPlantOptionsUrl,
  getBomCostOptionProductOptionsUrl,
} from '@/api/logistics/manufacturing/bom/costOption.api'
import type { TaktSelectOption } from '@/types/common.types'
import { buildBomCostOptionParams, hasBomCostOptionPeriod } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomCostOptionParams'
import {
  loadBomMaterialTypeOptionsWithDefault,
  pickDefaultBomMaterialType,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialTypeOptions'
import { MATERIAL_COST_ANALYSIS_LOCALE_PREFIX } from '../composables/useMaterialCostItemAnalysis'

/** 工厂代码 */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 物料类型（本表 MaterialType；加载后默认 FERT） */
const materialType = defineModel<string | undefined>('materialType')
/** 机种编码 */
const modelCode = defineModel<string | undefined>('modelCode')
/** 产品编码 */
const productCode = defineModel<string | undefined>('productCode')
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
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)
/** 产品下拉刷新键 */
const productSelectKey = ref(0)
/** 期间键（驱动下拉刷新） */
const periodKey = computed(
  () => `${periodRange.value?.[0] || ''}_${periodRange.value?.[1] || ''}`,
)
/** 本表物料类型全量选项（先拉列表，再默认 FERT） */
const materialTypeOptions = ref<TaktSelectOption[]>([])
/** 物料类型选项 loading */
const materialTypeOptionsLoading = ref(false)
/** 当前 options 对应的工厂 */
const materialTypeOptionsPlant = ref('')
/** 选项请求序号（防竞态） */
let materialTypeLoadToken = 0
const plantOptionsUrl = getBomCostOptionPlantOptionsUrl()
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()
const productOptionsUrl = getBomCostOptionProductOptionsUrl()

/** 期间是否可解析（起止均有，或单端补齐） */
const hasPeriod = computed(() => hasBomCostOptionPeriod(periodRange.value))

/** 类型：工厂 + 期间 */
const canSelectType = computed(() => !!plantCode.value?.trim() && hasPeriod.value)

/** 机种：工厂 + 期间 + 类型；机种本身可空 */
const canSelectModel = computed(
  () => canSelectType.value && !!materialType.value?.trim(),
)

/** 产品：与机种同前置；机种可空时产品不过滤机种 */
const canSelectProduct = computed(() => canSelectModel.value)

/**
 * 机种下拉参数（工厂 + 期间 + 必选物料类型）
 */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    materialType: materialType.value,
  }),
)

/**
 * 产品下拉参数（工厂 + 期间 + 必选物料类型；机种可选）
 */
const productApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    periodRange: periodRange.value,
    materialType: materialType.value,
    modelCode: modelCode.value,
  }),
)

/** 刷新机种/产品下拉 */
function refreshModelProductSelects() {
  modelSelectKey.value += 1
  productSelectKey.value += 1
}

/**
 * 按工厂拉取物料类型全量选项，返回默认选中值（优先 FERT）
 * @param {string} plant 工厂
 * @returns {Promise<string | undefined>} 默认类型
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

/**
 * 空类型时回填默认 FERT（选项已就绪时）
 */
function applyDefaultMaterialTypeIfEmpty() {
  if (materialType.value?.trim()) {
    return
  }
  const def = pickDefaultBomMaterialType(materialTypeOptions.value)
  if (def) {
    materialType.value = def
  }
}

/** 工厂变更：清空下游（类型由 watch 重拉并默认 FERT） */
function handlePlantChange() {
  modelCode.value = undefined
  productCode.value = undefined
  refreshModelProductSelects()
}

/** 期间变更：重拉类型并清空机种/产品 */
async function handlePeriodChange() {
  modelCode.value = undefined
  productCode.value = undefined
  const p = plantCode.value?.trim()
  if (p && hasBomCostOptionPeriod(periodRange.value)) {
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
  }
  refreshModelProductSelects()
}

/** 物料类型变更：清空下游；清空时回填 FERT */
function handleMaterialTypeChange() {
  applyDefaultMaterialTypeIfEmpty()
  modelCode.value = undefined
  productCode.value = undefined
  refreshModelProductSelects()
}

/** 机种变更 */
function handleModelChange() {
  productCode.value = undefined
  productSelectKey.value += 1
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
      productCode.value = undefined
      refreshModelProductSelects()
      return
    }
    modelCode.value = undefined
    productCode.value = undefined
    const defaultType = await ensureMaterialTypeOptions(p)
    materialType.value = defaultType
    refreshModelProductSelects()
  },
  { immediate: true },
)

/** 父级重置把类型清空、工厂未变时：重新默认 FERT */
watch(materialType, async (type) => {
  if (type?.trim()) {
    return
  }
  const p = plantCode.value?.trim()
  if (!p) {
    return
  }
  if (materialTypeOptionsPlant.value !== p) {
    const defaultType = await ensureMaterialTypeOptions(p)
    if (!materialType.value?.trim()) {
      materialType.value = defaultType
    }
    return
  }
  applyDefaultMaterialTypeIfEmpty()
})
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

.material-cost-analysis-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.material-cost-analysis-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.material-cost-analysis-query-bar__control--type {
  width: 9rem;
  min-width: 7rem;
}

.material-cost-analysis-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.material-cost-analysis-query-bar__control--product {
  width: 14rem;
  min-width: 10rem;
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
