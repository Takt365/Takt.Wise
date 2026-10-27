<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-usage/components -->
<!-- 文件名称：MaterialUsageQueryForm.vue -->
<!-- 功能描述：通用物料 / 专用物料查询栏（工厂、核算年月 yyyy-MM、机种可空、物料可空；二者互斥） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-usage-query-bar">
    <div class="material-usage-query-bar__fields min-w-0 flex flex-1 flex-nowrap items-center gap-x-3">
      <div class="flex items-center gap-1">
        <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.costingMonth`) }}</span>
        <a-date-picker
          v-model:value="costingMonth"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :disabled-date="isCostingPeriodMonthDisabled"
          class="material-usage-query-bar__control material-usage-query-bar__control--period"
          :placeholder="t(`${localePrefix}.costingMonthPlaceholder`)"
          @change="handlePeriodChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t('entity.bommaterialcost.modelcode') }}</span>
        <TaktSelect
          :key="`model-${modelSelectKey}-${periodKey}`"
          v-model="modelCode"
          :api-url="modelOptionsUrl"
          :api-params="modelApiParams"
          class="material-usage-query-bar__control material-usage-query-bar__control--model"
          allow-clear
          show-search
          :disabled="!canSelectModel"
          :placeholder="t(`${localePrefix}.modelOptional`)"
          @change="handleModelChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.material`) }}</span>
        <TaktSelect
          :key="`material-${materialSelectKey}-${periodKey}`"
          v-model="componentCode"
          :api-url="materialOptionsUrl"
          :api-params="materialApiParams"
          class="material-usage-query-bar__control material-usage-query-bar__control--model"
          allow-clear
          show-search
          :disabled="!canSelectComponent"
          :placeholder="t(`${localePrefix}.materialOptional`)"
          @change="handleComponentChange"
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
 * 通用物料 / 专用物料查询栏：工厂、核算年月、机种、物料。机种与物料均可空且互斥。
 */
import { RiRefreshLine, RiSearchLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getBomCostOptionModelOptionsUrl,
  getBomCostOptionPlantOptionsUrl,
} from '@/api/logistics/manufacturing/bom/costOption.api'
import {
  getBomCommonMaterialComponentOptionsUrl,
  getBomDedicatedMaterialComponentOptionsUrl,
} from '@/api/logistics/manufacturing/bom/materialUsage.api'
import { buildBomCostOptionParams, hasBomCostOptionPeriod } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomCostOptionParams'
import { isCostingPeriodMonthDisabled } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'

/** 成品类型：机种下拉与列表对比一致 */
const FERT_MATERIAL_TYPE = 'FERT'

/** 工厂代码 */
const plantCode = defineModel<string | undefined>('plantCode')
/** 组件物料（空=不按物料收窄；有值时机种禁用） */
const componentCode = defineModel<string | undefined>('componentCode')
/** 机种编码（空=全部机种；有值时物料禁用） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 核算年月 yyyy-MM */
const costingMonth = defineModel<string | undefined>('costingMonth')

const props = defineProps<{
  /** common 通用物料，dedicated 专用物料 */
  kind: 'common' | 'dedicated'
  /** 静态文案前缀 */
  localePrefix: string
  /** 查询 loading */
  loading?: boolean
}>()

const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 文案前缀 */
const localePrefix = computed(() => props.localePrefix)
/** 物料下拉刷新键 */
const materialSelectKey = ref(0)
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)
const plantOptionsUrl = getBomCostOptionPlantOptionsUrl()
/** 物料下拉：明细 ComponentCode */
const materialOptionsUrl = computed(() =>
  props.kind === 'dedicated'
    ? getBomDedicatedMaterialComponentOptionsUrl()
    : getBomCommonMaterialComponentOptionsUrl(),
)
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()
/** 年月键 */
const periodKey = computed(() => costingMonth.value?.trim() || '')
/** 已选物料 */
const hasComponent = computed(() => !!componentCode.value?.trim())
/** 已选机种 */
const hasModel = computed(() => !!modelCode.value?.trim())
/** 工厂与年月齐全，且未选机种，才选物料 */
const canSelectComponent = computed(
  () => !!plantCode.value?.trim() && hasBomCostOptionPeriod(null, costingMonth.value) && !hasModel.value,
)
/** 工厂与年月齐全，且未选物料，才选机种 */
const canSelectModel = computed(
  () => !!plantCode.value?.trim() && hasBomCostOptionPeriod(null, costingMonth.value) && !hasComponent.value,
)

/** 物料下拉参数（工厂 + 核算年月，选项来自明细 ComponentCode） */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  const period = costingMonth.value?.trim()
  if (!plant || !period) {
    return undefined
  }
  return { plantCode: plant, costingPeriod: period }
})

/** 机种下拉参数（工厂 + 年月 + FERT） */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: plantCode.value,
    costingMonth: costingMonth.value,
    materialType: FERT_MATERIAL_TYPE,
  }),
)

/** 选中物料后清空并禁用机种 */
function handleComponentChange() {
  if (componentCode.value?.trim()) {
    modelCode.value = undefined
  }
}

/** 选中机种后清空并禁用物料 */
function handleModelChange() {
  if (modelCode.value?.trim()) {
    componentCode.value = undefined
  }
}

/** 工厂变化后清空物料、机种并刷新下拉 */
function handlePlantChange() {
  componentCode.value = undefined
  modelCode.value = undefined
  materialSelectKey.value += 1
  modelSelectKey.value += 1
}

/** 年月变化后清空物料、机种并刷新下拉 */
function handlePeriodChange() {
  componentCode.value = undefined
  modelCode.value = undefined
  materialSelectKey.value += 1
  modelSelectKey.value += 1
}
</script>

<style scoped>
.takt-query-bar {
  margin: 4px;
  padding: 4px;
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
  gap: 8px;
  width: 100%;
  box-sizing: border-box;
}

.material-usage-query-bar__control--plant,
.material-usage-query-bar__control--model {
  width: 12rem;
  min-width: 8rem;
}

.material-usage-query-bar__control--period {
  width: 10rem;
  min-width: 8rem;
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
