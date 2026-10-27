<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-zero-price -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：BOM 零价格清单 + 成本计算操作（原 BOM计算页按钮迁入；defineExpose 无） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏：工厂 + 核算月 + 可选机种（左侧固定标签，与成本差异推移一致） -->
    <div class="takt-query-bar">
      <div class="min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.plantCode`) }}</span>
          <TaktSelect
            v-model:value="queryForm.plantCode"
            :api-url="plantOptionsUrl"
            :placeholder="t('common.page.form.placeholder.selectonly')"
            allow-clear
            show-search
            class="min-w-[160px]"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.costingMonth`) }}</span>
          <a-date-picker
            v-model:value="queryForm.costingMonth"
            picker="month"
            value-format="YYYY-MM"
            class="min-w-[140px]"
            :disabled-date="isCostingPeriodMonthDisabled"
            :placeholder="t(`${localePrefix}.costingMonthPlaceholder`)"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.modelCode`) }}</span>
          <TaktSelect
            :key="`model-${modelSelectKey}-${queryForm.plantCode}-${queryForm.costingMonth}`"
            v-model="queryForm.modelCodes"
            :api-url="modelOptionsUrl"
            :api-params="modelApiParams"
            multiple
            allow-clear
            show-search
            :disabled="!canSelectModel"
            class="min-w-[220px]"
            :placeholder="t(`${localePrefix}.modelCodesOptional`)"
          />
        </div>
      </div>
      <a-space class="shrink-0">
        <a-button class="takt-button-query" :loading="loading" @click="handleSearch">
          <template #icon>
            <RiSearchLine class="takt-remix-icon" />
          </template>
          {{ t('common.page.button.query') }}
        </a-button>
        <a-button class="takt-button-reset" :disabled="loading" @click="handleReset">
          <template #icon>
            <RiRefreshLine class="takt-remix-icon" />
          </template>
          {{ t('common.page.button.reset') }}
        </a-button>
      </a-space>
    </div>

    <!-- 工具栏：前导0、PCB SECT标识、计算、回填、机种平均、批量回填替代价格、导出；右侧列设置/刷新 -->
    <TaktToolsBar
      update-permission="logistics:manufacturing:bom:material:zeroprice:update"
      export-permission="logistics:manufacturing:bom:material:zeroprice:export"
      :left-actions="toolbarLeftActions"
      :show-import="false"
      :show-export="false"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      :export-loading="exportLoading"
      @export="handleExport"
      @column-setting="columnSettingVisible = true"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      table-mode="single"
      :columns="columns"
      :data-source="rows"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getRowKey"
      :pagination="false"
      :show-row-selection="false"
      id-column-key="componentCode"
      action-column-key="action"
      :visible-column-keys="visibleColumnKeys"
      :scroll="{ x: 'max-content' }"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'movingAveragePrice'">
          {{ formatPrice((record as BomMaterialZeroPrice).movingAveragePrice) }}
        </template>
        <template v-else-if="column.key === 'suggestedComponentCode'">
          {{ (record as BomMaterialZeroPrice).suggestedComponentCode?.trim() || '—' }}
        </template>
        <template v-else-if="column.key === 'suggestedMovingPrice'">
          {{ formatOptionalPrice((record as BomMaterialZeroPrice).suggestedMovingPrice) }}
        </template>
      </template>
    </TaktSingleTable>

    <!-- 底栏：左说明完整折行+字号随可用宽度缩放；右分页独立列不压缩 -->
    <div class="mt-2 grid grid-cols-[minmax(0,1fr)_auto] items-start gap-x-3 gap-y-1">
      <div
        ref="hintRef"
        class="min-w-0 break-words leading-snug text-text-secondary"
        :style="{ fontSize: `${hintFontPx}px` }"
      >
        {{
          t(`${localePrefix}.hint`, {
            month: costingMonthLabel,
            productCount,
            componentCount: total,
          })
        }}
      </div>
      <div v-if="total > 0" class="justify-self-end self-start">
        <TaktPagination
          :current="pageIndex"
          :page-size="pageSize"
          :total="total"
          :disabled="loading"
          @change="handlePageChange"
        />
      </div>
    </div>

    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'componentCode'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

    <!-- 手工替换更新移动价格：原组件/价 → 新组件/价/单位/币种 -->
    <TaktModal
      v-model:open="manualMovingVisible"
      :title="t(`${localePrefix}.movingPriceManualTitle`)"
      :confirm-loading="manualMovingPending"
      :use-viewport-size="false"
      width="560px"
      @ok="handleManualMovingSubmit"
    >
      <p class="mb-3 text-sm text-text-secondary">
        {{ t(`${localePrefix}.movingPriceManualHint`) }}
      </p>
      <a-form layout="vertical" :model="manualMovingForm" class="flex flex-col gap-1">
        <div class="mb-2 rounded border border-border px-3 py-2">
          <div class="mb-2 text-sm font-medium text-text">
            {{ t(`${localePrefix}.movingPriceManualOriginal`) }}
          </div>
          <a-row :gutter="16">
            <a-col :span="12">
              <a-form-item :label="t('entity.bommaterialcostitem.componentcode')">
                <a-input :value="manualMovingForm.componentCode" disabled />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="t(`${localePrefix}.movingPriceManualPrice`)">
                <a-input-number
                  :value="manualMovingForm.originalMovingPrice"
                  class="w-full"
                  disabled
                  :precision="5"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
        <div class="rounded border border-border px-3 py-2">
          <div class="mb-2 text-sm font-medium text-text">
            {{ t(`${localePrefix}.movingPriceManualReplace`) }}
          </div>
          <a-row :gutter="16">
            <a-col :span="12">
              <a-form-item :label="t(`${localePrefix}.movingPriceManualSourceComponent`)" required>
                <a-input
                  v-model:value="manualMovingForm.sourceComponentCode"
                  allow-clear
                  :placeholder="t(`${localePrefix}.movingPriceManualSourceRequired`)"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="t(`${localePrefix}.movingPriceManualPrice`)" required>
                <a-input-number
                  v-model:value="manualMovingForm.movingAveragePrice"
                  class="w-full"
                  :min="0"
                  :step="0.00001"
                  :precision="5"
                  :placeholder="t(`${localePrefix}.movingPriceManualPriceRequired`)"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="t(`${localePrefix}.movingPriceManualUnit`)" required>
                <TaktSelect
                  v-model="manualMovingForm.movingPriceUnit"
                  dict-type="logistics_materials_price_unit_param"
                  class="w-full"
                  :allow-clear="false"
                  show-search
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="t(`${localePrefix}.movingPriceManualCurrency`)" required>
                <TaktSelect
                  v-model="manualMovingForm.movingPriceCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  class="w-full"
                  :allow-clear="false"
                  show-search
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-form>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * BOM 零价格清单 + 原 BOM计算操作（计算/重算/平均/回填采购价/最近采购成本）
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'
import { useI18n } from 'vue-i18n'
import {
  getBomCostOptionPlantOptions,
  getBomCostOptionPlantOptionsUrl,
  getBomCostOptionModelOptionsUrl,
} from '@/api/logistics/manufacturing/bom/costOption.api'
import {
  exportBomMaterialZeroPriceData,
  getBomMaterialZeroPriceList,
  backfillBomMaterialZeroPriceMoving,
  manualUpdateBomMaterialZeroPriceMoving,
  markBomMaterialZeroPricePcbSect,
  stripBomMaterialZeroPriceLeadingZeros,
} from '@/api/logistics/manufacturing/bom/materialZeroPrice.api'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import {
  backfillBomCalculatePurchaseInfo,
  calculateBomCalculateAverage,
  recalculateBomCalculateCost,
  sumBomCalculateCost,
  sumBomCalculateLatestPurchaseCost,
} from '@/api/logistics/manufacturing/bom/calculate.api'
import type {
  BomMaterialZeroPrice,
  BomMaterialZeroPriceQuery,
} from '@/types/logistics/manufacturing/bom/materialZeroPrice.types'
import type { BomCalculateQuery } from '@/types/logistics/manufacturing/bom/calculate.types'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import {
  formatBomMaterialCostItemRecalculateDuration,
  useBomMaterialCostItemRecalculateSignalR,
} from '@/composables/useBomMaterialCostItemRecalculateSignalr'
import { RiCalculatorLine, RiCoinLine, RiEditLine, RiEraserLine, RiExportLine, RiFlagLine, RiFundsLine, RiPriceTag3Line, RiRefreshLine, RiSearchLine, RiShoppingCart2Line } from '@remixicon/vue'
import {
  buildDefaultCostingMonth,
  costingMonthToDateQuery,
  isCostingPeriodMonthDisabled,
} from './utils/bomMaterialCostPeriod'
import { BOM_ANALYSIS_PREFERRED_MATERIAL_TYPE } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialTypeOptions'
import { buildBomCostOptionParams } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomCostOptionParams'

/** 静态文案键前缀 */
const localePrefix = 'logistics.manufacturing.bom.material-zero-price.page'
/** i18n */
const { t } = useI18n()
/** 租户/公司 */
const tenantStore = useTenantStore()
/** Excel 命名 */
const excelNames = taktExcelEntityNames('TaktBomMaterialZeroPrice')
/** 工厂下拉 */
const plantOptionsUrl = getBomCostOptionPlantOptionsUrl()
/** 机种下拉（本表 FERT 去重） */
const modelOptionsUrl = getBomCostOptionModelOptionsUrl()
/** 机种下拉刷新 key（工厂/核算月变更时重建） */
const modelSelectKey = ref(0)
/** 计算按钮权限 */
const calculateUpdatePermission = 'logistics:manufacturing:bom:material:zeroprice:update'
/** 导出按钮权限 */
const exportPermission = 'logistics:manufacturing:bom:material:zeroprice:export'

/** 查询条件 */
const queryForm = reactive({
  plantCode: '' as string,
  costingMonth: buildDefaultCostingMonth(),
  modelCodes: [] as string[],
})

/** 已选机种（去空白） */
const selecmedModelCodes = computed(() =>
  (queryForm.modelCodes ?? []).map((c) => String(c).trim()).filter(Boolean),
)

/** 可选机种（须工厂 + 核算月） */
const canSelectModel = computed(
  () => !!queryForm.plantCode?.trim() && !!queryForm.costingMonth?.trim(),
)

/** 机种下拉参数（工厂 + 单月 + FERT） */
const modelApiParams = computed(() =>
  buildBomCostOptionParams({
    plantCode: queryForm.plantCode,
    costingMonth: queryForm.costingMonth,
    materialType: BOM_ANALYSIS_PREFERRED_MATERIAL_TYPE,
  }),
)

/**
 * 机种查询串（空=不传=全部）
 * @returns {string | undefined}
 */
function modelCodesQueryParam(): string | undefined {
  const joined = selecmedModelCodes.value.join(',')
  return joined || undefined
}

/** 合并行 */
const rows = ref<BomMaterialZeroPrice[]>([])
/** 列表 loading */
const loading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 计算/重算提交中 */
const sumPending = ref(false)
/** 平均成本提交中 */
const averagePending = ref(false)
/** 回填采购价提交中 */
const purchaseBackfillPending = ref(false)
/** 批量回填移动价提交中 */
const movingBackfillBatchPending = ref(false)
/** 行内回填移动价中的组件编码 */
const movingBackfillComponentCode = ref('')
/** 手工更新弹窗 */
const manualMovingVisible = ref(false)
/** 手工更新提交中 */
const manualMovingPending = ref(false)
/** 手工更新表单（原组件只读 + 替换新组件价/单位/币种） */
const manualMovingForm = reactive({
  componentCode: '',
  originalMovingPrice: 0 as number,
  sourceComponentCode: '',
  movingAveragePrice: null as number | null,
  movingPriceUnit: '1000',
  movingPriceCurrencyCode: 'CNY',
})
/** 最近采购成本提交中 */
const latestPurchaseCostPending = ref(false)
/** PCB SECT 打标提交中 */
const pcbSectMarkPending = ref(false)
/** 去前导 0 */
const stripLeadingZeroPending = ref(false)
/** 页码 */
const pageIndex = ref(getTaktDefaultPageIndex())
/** 页大小 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 产品数 */
const productCount = ref(0)
/** 列设置 */
const columnSettingVisible = ref(false)
/** 可见列 */
const visibleColumnKeys = ref<string[]>([])
/** 底栏说明区（按可用宽度缩放字号） */
const hintRef = ref<HTMLElement | null>(null)
/** 说明字号（px；随说明区宽度在 10～14 间缩放） */
const hintFontPx = ref(13)
/** 说明区 ResizeObserver */
let hintResizeObserver: ResizeObserver | null = null

/**
 * 按说明区可用宽度缩放字号，全文折行完整显示且不侵占右侧分页
 */
function syncHintFontSize(): void {
  const el = hintRef.value
  if (!el) {
    return
  }
  const width = el.clientWidth
  if (width <= 0) {
    return
  }
  // 约 320px→10px，960px→14px
  hintFontPx.value = Math.min(14, Math.max(10, Math.round(10 + (width - 320) * (4 / 640))))
}

/** 核算月展示 */
const costingMonthLabel = computed(() => queryForm.costingMonth?.trim() || '—')

/** 工具栏：前导0、PCB SECT标识、计算成本、重算成本、回填采购价、最近采购成本、机种平均成本、批量回填替代价格、导出 */
const toolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'strip-leading-zeros',
    label: t(`${localePrefix}.stripLeadingZeros`),
    icon: RiEraserLine,
    buttonClass: 'takt-button-sync',
    permission: calculateUpdatePermission,
    loading: stripLeadingZeroPending.value,
    onClick: () => handleStripLeadingZeros(),
  },
  {
    key: 'pcb-sect-mark',
    label: t(`${localePrefix}.pcbSectMark`),
    icon: RiFlagLine,
    buttonClass: 'takt-button-sync',
    permission: calculateUpdatePermission,
    loading: pcbSectMarkPending.value,
    onClick: () => handleMarkPcbSect(),
  },
  {
    key: 'cost-sum',
    label: t(`${localePrefix}.costSum`),
    icon: RiCalculatorLine,
    buttonClass: 'takt-button-calculate',
    permission: calculateUpdatePermission,
    loading: sumPending.value,
    onClick: () => handleSumOrRecalculate(false),
  },
  {
    key: 'cost-recalculate',
    label: t(`${localePrefix}.costRecalculate`),
    icon: RiRefreshLine,
    buttonClass: 'takt-button-restart',
    permission: calculateUpdatePermission,
    loading: sumPending.value,
    onClick: () => handleSumOrRecalculate(true),
  },
  {
    key: 'purchase-price-backfill',
    label: t(`${localePrefix}.purchaseInfoBackfill`),
    icon: RiShoppingCart2Line,
    buttonClass: 'takt-button-sync',
    permission: calculateUpdatePermission,
    loading: purchaseBackfillPending.value,
    onClick: () => handlePurchaseInfoBackfill(),
  },
  {
    key: 'latest-purchase-cost',
    label: t(`${localePrefix}.latestPurchaseCost`),
    icon: RiFundsLine,
    buttonClass: 'takt-button-calculate',
    permission: calculateUpdatePermission,
    loading: latestPurchaseCostPending.value,
    onClick: () => handleLatestPurchaseCost(),
  },
  {
    key: 'cost-average',
    label: t(`${localePrefix}.costAverage`),
    icon: RiPriceTag3Line,
    buttonClass: 'takt-button-sync',
    permission: calculateUpdatePermission,
    loading: averagePending.value,
    onClick: () => handleAverage(),
  },
  {
    key: 'moving-price-backfill-batch',
    label: t(`${localePrefix}.movingPriceBackfillBatch`),
    icon: RiCoinLine,
    buttonClass: 'takt-button-sync',
    permission: calculateUpdatePermission,
    loading: movingBackfillBatchPending.value,
    onClick: () => handleBackfillMovingPriceBatch(),
  },
  {
    key: 'export',
    label: t('common.page.button.export'),
    icon: RiExportLine,
    buttonClass: 'takt-button-export',
    permission: exportPermission,
    loading: exportLoading.value,
    onClick: () => handleExport(),
  },
])

/** 列定义 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 90,
    ellipsis: true,
  },
  {
    title: t('entity.bommaterialcost.modelcode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.bommaterialcostitem.componentcode'),
    dataIndex: 'componentCode',
    key: 'componentCode',
    width: 130,
    ellipsis: true,
  },
  {
    title: t('entity.bommaterialcostitem.componentdescription'),
    dataIndex: 'componentDescription',
    key: 'componentDescription',
    width: 160,
    ellipsis: true,
  },
  {
    title: t(`${localePrefix}.productCodes`),
    dataIndex: 'productCodes',
    key: 'productCodes',
    width: 240,
    ellipsis: true,
  },
  {
    title: t(`${localePrefix}.productCount`),
    dataIndex: 'productCount',
    key: 'productCount',
    width: 90,
    align: 'right',
  },
  {
    title: t('entity.bommaterialcostitem.movingaverageprice'),
    dataIndex: 'movingAveragePrice',
    key: 'movingAveragePrice',
    width: 110,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.suggestedComponentCode`),
    dataIndex: 'suggestedComponentCode',
    key: 'suggestedComponentCode',
    width: 140,
    ellipsis: true,
  },
  {
    title: t(`${localePrefix}.suggestedMovingPrice`),
    dataIndex: 'suggestedMovingPrice',
    key: 'suggestedMovingPrice',
    width: 120,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.costingMonth`),
    dataIndex: 'costingPeriod',
    key: 'costingPeriod',
    width: 100,
  },
  CreateActionColumn<BomMaterialZeroPrice>({
    width: 200,
    actions: [
      {
        key: 'backfill-moving-price',
        label: t(`${localePrefix}.movingPriceBackfillRow`),
        shape: 'plain',
        icon: RiCoinLine,
        buttonClass: 'takt-button-sync',
        permission: calculateUpdatePermission,
        visible: (record) => !!record.suggestedComponentCode?.trim(),
        loading: (record) =>
          movingBackfillComponentCode.value === String(record.componentCode ?? '').trim(),
        onClick: (record) => {
          void handleBackfillMovingPrice(record)
        },
      },
      {
        key: 'manual-moving-price',
        label: t(`${localePrefix}.movingPriceManualRow`),
        shape: 'plain',
        icon: RiEditLine,
        buttonClass: 'takt-button-update',
        permission: calculateUpdatePermission,
        visible: (record) => !record.suggestedComponentCode?.trim(),
        loading: () =>
          manualMovingPending.value
          && manualMovingVisible.value
          && !!manualMovingForm.componentCode,
        onClick: (record) => {
          openManualMovingModal(record)
        },
      },
    ],
  }),
])

/**
 * 行主键
 * @param record 行
 * @returns key
 */
function getRowKey(record: BomMaterialZeroPrice): string {
  return `${record.plantCode}|${record.modelCode}|${record.componentCode}|${record.costingPeriod}`
}

/**
 * 格式化价格
 * @param value 价格
 * @returns 文本
 */
function formatPrice(value: unknown): string {
  const n = Number(value)
  if (!Number.isFinite(n)) {
    return '—'
  }
  return n.toFixed(5)
}

/**
 * 格式化可选价格
 * @param value 价格
 * @returns 文本
 */
function formatOptionalPrice(value: unknown): string {
  if (value === null || value === undefined || value === '') {
    return '—'
  }
  return formatPrice(value)
}

/**
 * 构建零价清单查询
 * @param overrides 覆盖
 * @returns 查询或 null
 */
function buildQuery(
  overrides?: Partial<BomMaterialZeroPriceQuery>,
): BomMaterialZeroPriceQuery | null {
  const plant = queryForm.plantCode?.trim() ?? ''
  const month = queryForm.costingMonth?.trim() ?? ''
  if (!plant || !month) {
    return null
  }
  const modelCodes = modelCodesQueryParam()
  return {
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    plantCode: plant,
    ...(modelCodes ? { modelCodes } : {}),
    ...costingMonthToDateQuery(month),
    ...overrides,
  }
}

/**
 * 按表单构建计算查询（不传 materialType = 全部类型）
 * @returns 查询；非法时 null
 */
function buildCostQuery(): BomCalculateQuery | null {
  const month = String(queryForm.costingMonth ?? '').trim()
  const dates = costingMonthToDateQuery(month)
  if (!dates.costingDateStart || !dates.costingDateEnd) {
    message.warning(t(`${localePrefix}.costNeedMonth`))
    return null
  }
  const plantCode = String(queryForm.plantCode ?? '').trim()
  if (!plantCode) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return null
  }
  const query: BomCalculateQuery = {
    plantCode,
    costingDateStart: dates.costingDateStart,
    costingDateEnd: dates.costingDateEnd,
    processRecordCount: 0,
  }
  const modelCodes = modelCodesQueryParam()
  if (modelCodes) {
    query.modelCode = modelCodes
  }
  return query
}

/** 加载清单 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    rows.value = []
    total.value = 0
    productCount.value = 0
    return
  }
  loading.value = true
  try {
    const res = await getBomMaterialZeroPriceList(query)
    rows.value = res.paged?.data ?? []
    total.value = res.paged?.total ?? res.componentCount ?? 0
    productCount.value = res.productCodes?.length ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    rows.value = []
    total.value = 0
    productCount.value = 0
  } finally {
    loading.value = false
  }
}

/** 查询 */
async function handleSearch() {
  if (!queryForm.plantCode?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!queryForm.costingMonth?.trim()) {
    message.warning(t(`${localePrefix}.costNeedMonth`))
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData()
}

/**
 * 默认工厂：取公司关联工厂，仅当其存在于本表 plant-options 时选中（对齐成本分析）
 * @returns {Promise<void>}
 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched = ''
  if (related) {
    try {
      const plants = await getBomCostOptionPlantOptions()
      const hit = (plants ?? []).find(
        (o) => String(o.dictValue ?? '').trim().toLowerCase() === related.toLowerCase(),
      )
      matched = hit ? String(hit.dictValue).trim() : ''
    } catch {
      matched = ''
    }
  }
  queryForm.plantCode = matched
  queryForm.modelCodes = []
  modelSelectKey.value += 1
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  queryForm.costingMonth = buildDefaultCostingMonth()
  queryForm.modelCodes = []
  modelSelectKey.value += 1
  pageIndex.value = getTaktDefaultPageIndex()
  rows.value = []
  total.value = 0
  productCount.value = 0
}

/** 刷新 */
async function handleRefresh() {
  await loadData()
}

/**
 * 分页
 * @param page 页码
 * @param size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/**
 * 列可见性
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置重置 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = columns.value.map((c) => String(c.key))
}

/**
 * 计算成本或重算成本
 * @param forceRecalculate true=重算
 */
async function handleSumOrRecalculate(forceRecalculate: boolean): Promise<void> {
  const query = buildCostQuery()
  if (!query) return
  if (forceRecalculate) {
    const confirmed = await new Promise<boolean>((resolve) => {
      Modal.confirm({
        title: t(`${localePrefix}.costRecalculateConfirmTitle`),
        content: t(`${localePrefix}.costRecalculateConfirmContent`),
        okText: t('common.page.button.ok'),
        cancelText: t('common.page.button.cancel'),
        onOk: () => resolve(true),
        onCancel: () => resolve(false),
      })
    })
    if (!confirmed) return
  }
  sumPending.value = true
  try {
    const submitted = forceRecalculate
      ? await recalculateBomCalculateCost(query)
      : await sumBomCalculateCost(query)
    const msgKey = forceRecalculate
      ? `${localePrefix}.costRecalculateSubmitted`
      : `${localePrefix}.costSumSubmitted`
    message.success(t(msgKey, { month: submitted.processedMonth }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.costRecalculateFailed`))
  } finally {
    sumPending.value = false
  }
}

/** 计算平均成本 */
async function handleAverage(): Promise<void> {
  const month = String(queryForm.costingMonth ?? '').trim()
  if (!month) {
    message.warning(t(`${localePrefix}.costNeedMonth`))
    return
  }
  const plant = String(queryForm.plantCode ?? '').trim()
  if (!plant) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  const modelCode = modelCodesQueryParam()
  averagePending.value = true
  try {
    const result = await calculateBomCalculateAverage({
      plantCode: plant,
      costingPeriod: month,
      modelCode,
    })
    message.success(
      t(`${localePrefix}.costAverageSuccess`, {
        month: result.costingPeriod,
        scanned: result.scannedRowCount,
        positiveCostRows: result.positiveProductCostRowCount,
        modelUpdated: result.modelCodeUpdatedCount,
        typeUpdated: result.materialTypeUpdatedCount,
        averageUpdated: result.averageUpdatedCount,
        groups: result.modelGroupCount,
        groupsWithCost: result.groupsWithProductCostCount,
        groupsNoCost: result.groupsWithoutProductCostCount,
      }),
    )
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.costAverageFailed`))
  } finally {
    averagePending.value = false
  }
}

/** 回填采购价 */
async function handlePurchaseInfoBackfill(): Promise<void> {
  const query = buildCostQuery()
  if (!query) return
  purchaseBackfillPending.value = true
  try {
    const result = await backfillBomCalculatePurchaseInfo(query)
    message.success(
      t(`${localePrefix}.purchaseInfoBackfillSuccess`, {
        month: result.processedMonth,
        scanned: result.scannedRowCount,
        updated: result.updatedRowCount,
        skipped: result.skippedNoPriceCount,
        unchanged: result.unchangedRowCount,
      }),
    )
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.purchaseInfoBackfillFailed`))
  } finally {
    purchaseBackfillPending.value = false
  }
}

/**
 * 工具栏：按当前工厂/核算月/机种批量回填全部零价组件移动价
 */
function handleBackfillMovingPriceBatch() {
  const query = buildQuery({ pageIndex: 1, pageSize: 1 })
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!queryForm.costingMonth?.trim()) {
    message.warning(t(`${localePrefix}.costNeedMonth`))
    return
  }
  Modal.confirm({
    title: t(`${localePrefix}.movingPriceBackfillBatchConfirmTitle`),
    content: t(`${localePrefix}.movingPriceBackfillBatchConfirmContent`, {
      month: queryForm.costingMonth.trim(),
    }),
    onOk: async () => {
      movingBackfillBatchPending.value = true
      try {
        const result = await backfillBomMaterialZeroPriceMoving({
          plantCode: query.plantCode,
          ...(query.modelCodes ? { modelCodes: query.modelCodes } : {}),
          costingDateStart: query.costingDateStart,
          costingDateEnd: query.costingDateEnd,
        })
        message.success(
          t(`${localePrefix}.movingPriceBackfillBatchSuccess`, {
            month: result.processedMonth,
            components: result.componentProcessedCount,
            scanned: result.scannedRowCount,
            updated: result.updatedRowCount,
            skipped: result.skippedNoPriceCount,
            unchanged: result.unchangedRowCount,
            productCost: result.productMonthlyCostUpdatedCount,
            modelAverage: result.modelMonthlyAverageUpdatedCount,
          }),
        )
        await loadData()
      } catch (error: unknown) {
        const err = error as { message?: string }
        message.error(err?.message || t(`${localePrefix}.movingPriceBackfillFailed`))
      } finally {
        movingBackfillBatchPending.value = false
      }
    },
  })
}

/**
 * 操作列：按当前查询条件回填该组件明细移动平均价/单位/货币
 * @param record 零价行
 */
async function handleBackfillMovingPrice(record: BomMaterialZeroPrice) {
  const componentCode = String(record.componentCode ?? '').trim()
  const suggested = String(record.suggestedComponentCode ?? '').trim()
  if (!componentCode || !suggested) {
    message.warning(t(`${localePrefix}.movingPriceBackfillNoSuggested`))
    return
  }
  const query = buildQuery({ pageIndex: 1, pageSize: 1 })
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  Modal.confirm({
    title: t(`${localePrefix}.movingPriceBackfillConfirmTitle`),
    content: t(`${localePrefix}.movingPriceBackfillConfirmContent`, {
      component: componentCode,
      suggested,
    }),
    onOk: async () => {
      movingBackfillComponentCode.value = componentCode
      try {
        const result = await backfillBomMaterialZeroPriceMoving({
          plantCode: query.plantCode,
          componentCode,
          ...(query.modelCodes ? { modelCodes: query.modelCodes } : {}),
          costingDateStart: query.costingDateStart,
          costingDateEnd: query.costingDateEnd,
        })
        message.success(
          t(`${localePrefix}.movingPriceBackfillSuccess`, {
            month: result.processedMonth,
            scanned: result.scannedRowCount,
            updated: result.updatedRowCount,
            skipped: result.skippedNoPriceCount,
            unchanged: result.unchangedRowCount,
            productCost: result.productMonthlyCostUpdatedCount,
            modelAverage: result.modelMonthlyAverageUpdatedCount,
            priceInfo: result.priceInfo || '—',
          }),
        )
        await loadData()
      } catch (error: unknown) {
        const err = error as { message?: string }
        message.error(err?.message || t(`${localePrefix}.movingPriceBackfillFailed`))
      } finally {
        movingBackfillComponentCode.value = ''
      }
    },
  })
}

/**
 * 打开手工替换更新弹窗（原组件/价 + 新组件/价/单位/币种）
 * @param record 零价行
 */
function openManualMovingModal(record: BomMaterialZeroPrice) {
  const componentCode = String(record.componentCode ?? '').trim()
  if (!componentCode) {
    return
  }
  const originalPrice = Number(record.movingAveragePrice)
  manualMovingForm.componentCode = componentCode
  manualMovingForm.originalMovingPrice = Number.isFinite(originalPrice) ? originalPrice : 0
  manualMovingForm.sourceComponentCode = ''
  manualMovingForm.movingAveragePrice = null
  manualMovingForm.movingPriceUnit = '1000'
  manualMovingForm.movingPriceCurrencyCode = 'CNY'
  manualMovingVisible.value = true
}

/**
 * 提交手工替换：新组件价/单位/币种回填到原组件明细
 */
async function handleManualMovingSubmit() {
  const componentCode = String(manualMovingForm.componentCode ?? '').trim()
  const sourceComponentCode = String(manualMovingForm.sourceComponentCode ?? '').trim()
  if (!componentCode) {
    return
  }
  if (!sourceComponentCode) {
    message.warning(t(`${localePrefix}.movingPriceManualSourceRequired`))
    return
  }
  const price = Number(manualMovingForm.movingAveragePrice)
  if (!Number.isFinite(price) || price <= 0) {
    message.warning(t(`${localePrefix}.movingPriceManualPriceRequired`))
    return
  }
  const unitRaw = String(manualMovingForm.movingPriceUnit ?? '').trim()
  const unit = Number(unitRaw)
  if (!Number.isFinite(unit) || unit <= 0) {
    message.warning(t(`${localePrefix}.movingPriceManualPriceRequired`))
    return
  }
  const currency = String(manualMovingForm.movingPriceCurrencyCode ?? '').trim()
  if (!currency) {
    message.warning(t(`${localePrefix}.movingPriceManualPriceRequired`))
    return
  }
  const query = buildQuery({ pageIndex: 1, pageSize: 1 })
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  manualMovingPending.value = true
  try {
    const result = await manualUpdateBomMaterialZeroPriceMoving({
      plantCode: query.plantCode,
      componentCode,
      sourceComponentCode,
      costingDateStart: query.costingDateStart,
      costingDateEnd: query.costingDateEnd,
      movingAveragePrice: price,
      movingPriceUnit: unit,
      movingPriceCurrencyCode: currency,
    })
    message.success(
      t(`${localePrefix}.movingPriceManualSuccess`, {
        month: result.processedMonth,
        component: componentCode,
        source: sourceComponentCode,
        scanned: result.scannedRowCount,
        updated: result.updatedRowCount,
        unchanged: result.unchangedRowCount,
        productCost: result.productMonthlyCostUpdatedCount,
        modelAverage: result.modelMonthlyAverageUpdatedCount,
        priceInfo: result.priceInfo || '—',
      }),
    )
    manualMovingVisible.value = false
    await loadData()
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.movingPriceManualFailed`))
  } finally {
    manualMovingPending.value = false
  }
}

/** 计算最近采购成本 */
async function handleLatestPurchaseCost(): Promise<void> {
  const query = buildCostQuery()
  if (!query) return
  latestPurchaseCostPending.value = true
  try {
    const result = await sumBomCalculateLatestPurchaseCost(query)
    message.success(
      t(`${localePrefix}.latestPurchaseCostSuccess`, {
        month: result.processedMonth,
        scanned: result.scannedRowCount,
        refreshed: result.refreshedGroupCount,
        skipped: result.skippedGroupCount,
      }),
    )
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.latestPurchaseCostFailed`))
  } finally {
    latestPurchaseCostPending.value = false
  }
}

/**
 * PCB SECT 整树 ExtField 打标（pcbSect=X）
 */
async function handleMarkPcbSect(): Promise<void> {
  const query = buildQuery({ pageIndex: 1, pageSize: 1 })
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  const month = queryForm.costingMonth?.trim() ?? ''
  const confirmed = await new Promise<boolean>((resolve) => {
    Modal.confirm({
      title: t(`${localePrefix}.pcbSectMarkConfirmTitle`),
      content: t(`${localePrefix}.pcbSectMarkConfirmContent`, { month }),
      okText: t('common.page.button.ok'),
      cancelText: t('common.page.button.cancel'),
      onOk: () => resolve(true),
      onCancel: () => resolve(false),
    })
  })
  if (!confirmed) {
    return
  }
  pcbSectMarkPending.value = true
  try {
    const result = await markBomMaterialZeroPricePcbSect({
      plantCode: query.plantCode,
      modelCodes: query.modelCodes,
      costingDateStart: query.costingDateStart,
      costingDateEnd: query.costingDateEnd,
    })
    message.success(
      t(`${localePrefix}.pcbSectMarkSuccess`, {
        month: result.processedMonth,
        scanned: result.scannedRowCount,
        pcbSect: result.pcbSectRowCount,
        updated: result.updatedRowCount,
        unchanged: result.unchangedRowCount,
      }),
    )
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.pcbSectMarkFailed`))
  } finally {
    pcbSectMarkPending.value = false
  }
}

/**
 * 去掉当前工厂纯数字产品/组件编码的前导 0
 */
async function handleStripLeadingZeros(): Promise<void> {
  const plant = queryForm.plantCode?.trim() ?? ''
  if (!plant) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  const confirmed = await new Promise<boolean>((resolve) => {
    Modal.confirm({
      title: t(`${localePrefix}.stripLeadingZerosConfirmTitle`),
      content: t(`${localePrefix}.stripLeadingZerosConfirmContent`, { plant }),
      okText: t('common.page.button.ok'),
      cancelText: t('common.page.button.cancel'),
      onOk: () => resolve(true),
      onCancel: () => resolve(false),
    })
  })
  if (!confirmed) {
    return
  }
  stripLeadingZeroPending.value = true
  try {
    const result = await stripBomMaterialZeroPriceLeadingZeros({ plantCode: plant })
    message.success(
      t(`${localePrefix}.stripLeadingZerosSuccess`, {
        headerProduct: result.headerProductUpdatedCount ?? 0,
        itemProduct: result.itemProductUpdatedCount ?? 0,
        itemComponent: result.itemComponentUpdatedCount ?? 0,
        skipped: result.skippedConflictCount ?? 0,
      }),
    )
    await loadData()
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.stripLeadingZerosFailed`))
  } finally {
    stripLeadingZeroPending.value = false
  }
}

/** 导出 */
async function handleExport() {
  const plant = queryForm.plantCode?.trim() ?? ''
  const month = queryForm.costingMonth?.trim() ?? ''
  const query = buildQuery({
    pageIndex: 1,
    pageSize: 100000,
  })
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  exportLoading.value = true
  try {
    const exportMeta = await exportBomMaterialZeroPriceData(
      query,
      excelNames.sheet,
      `${excelNames.fileBase}_${plant}_${month}`,
    )
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase: `${excelNames.fileBase}_${plant}_${month}`,
    })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t(`${localePrefix}.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.exportFailed`))
  } finally {
    exportLoading.value = false
  }
}

/** 后台计算/重算完成 SignalR */
useBomMaterialCostItemRecalculateSignalR(async (event) => {
  if (event.executeStatus === 1) {
    message.success(
      t(`${localePrefix}.costRecalculateCompleted`, {
        month: event.processedMonth,
        duration: formatBomMaterialCostItemRecalculateDuration(event.executeDuration),
        refreshed: event.refreshedGroupCount,
        skipped: event.skippedGroupCount,
      }),
    )
    await loadData()
    return
  }
  message.error(event.errorMessage || t(`${localePrefix}.costRecalculateFailed`))
})

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  visibleColumnKeys.value = columns.value.map((c) => String(c.key))
  await applyDefaultPlantFromCompany()
  await nextTick()
  if (typeof ResizeObserver !== 'undefined' && hintRef.value) {
    hintResizeObserver = new ResizeObserver(() => {
      syncHintFontSize()
    })
    hintResizeObserver.observe(hintRef.value)
    syncHintFontSize()
  }
})

onUnmounted(() => {
  hintResizeObserver?.disconnect()
  hintResizeObserver = null
})

watch(
  () => tenantStore.companyCode,
  () => {
    void (async () => {
      await applyDefaultPlantFromCompany()
      queryForm.costingMonth = buildDefaultCostingMonth()
      queryForm.modelCodes = []
      modelSelectKey.value += 1
      pageIndex.value = getTaktDefaultPageIndex()
      rows.value = []
      total.value = 0
      productCount.value = 0
    })()
  },
)

watch(
  () => [queryForm.plantCode, queryForm.costingMonth] as const,
  () => {
    queryForm.modelCodes = []
    modelSelectKey.value += 1
  },
)

watch(total, async () => {
  await nextTick()
  syncHintFontSize()
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
</style>
