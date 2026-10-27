<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：MaterialCostProductPanel.vue -->
<!-- 功能描述：中栏产品子表 + 右栏 Item 明细（左机种选中后；同物理汇总表不过拆实体） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-cost-product-panel flex h-full min-h-0 flex-1 flex-row overflow-hidden">
    <template v-if="hasModelGroup">
      <!-- 中：产品（TaktSingleTable + 外置分页） -->
      <div class="flex h-full min-h-0 w-1/2 min-w-0 shrink-0 flex-col border-r border-border px-3">
        <div
          ref="productTableWrapRef"
          class="min-h-0 flex-1 overflow-hidden"
        >
          <TaktSingleTable
            class="h-full min-h-0"
            entity-scope="company"
            table-mode="single"
            :stripe="true"
            :columns="productColumns"
            :data-source="productDataSource"
            :loading="productLoading"
            :row-key="getProductRowKey"
            :row-selection="productRowSelection"
            :pagination="false"
            :custom-row="onProductClickRow"
            id-column-key="bomMaterialCostId"
            :visible-column-keys="productVisibleKeys"
            :scroll="{ y: productTableScrollY }"
            @resize-column="handleProductResizeColumn"
          >
            <template #bodyCell="{ column, record, text }">
              <template v-if="column.key === 'materialType'">
                <TaktDictTag
                  :value="String((record as BomMaterialCost).materialType ?? '')"
                  dict-type="logistics_materials_material_type"
                />
              </template>
              <template v-else-if="column.key === 'currencyCode'">
                <TaktDictTag
                  :value="String((record as BomMaterialCost).currencyCode ?? '')"
                  dict-type="accounting_financial_currency_code"
                />
              </template>
              <template v-else>
                {{ text }}
              </template>
            </template>
          </TaktSingleTable>
        </div>
        <TaktPagination
          v-model:current="productPage"
          v-model:page-size="productPageSize"
          :total="productTotal"
          :disabled="productLoading"
          @change="loadProductData"
        />
      </div>
      <!-- 右：明细（始终挂载，避免 v-if 卸载导致选中后不刷新） -->
      <div class="relative flex h-full min-h-0 w-1/2 min-w-0 shrink-0 flex-col pl-3">
        <BomMaterialCostItemPanel
          v-show="hasProductSelection"
          ref="itemPanelRef"
          class="h-full min-h-0 flex-1"
        />
        <div
          v-show="!hasProductSelection"
          class="absolute inset-0 z-10 flex items-center justify-center bg-container"
        >
          <a-empty :description="t('logistics.manufacturing.bom.material-cost.page.selectproductfirst')" />
        </div>
      </div>
    </template>
    <div
      v-else
      class="flex min-h-0 flex-1 items-center justify-center"
    >
      <a-empty :description="t('logistics.manufacturing.bom.material-cost.page.selectmasterfirst')" />
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 中栏产品 + 右栏明细（机种组选中后加载）
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { getBomMaterialCostList } from '@/api/logistics/manufacturing/bom/materialCost.api'
import type { BomMaterialCost } from '@/types/logistics/manufacturing/bom/materialCost.types'
import { useBomMaterialCostMasterContext, type BomMaterialCostRowRecord } from '../composables/useMaterialCostMasterContext'
import { useBomMaterialCostI18n } from '../composables/useMaterialCostI18n'
import BomMaterialCostItemPanel from './MaterialCostItemPanel.vue'
import { formatBomMaterialCostAmount } from '../utils/bomMaterialCostItemLineCost'

const { t } = useI18n()
/** 实体字段 i18n */
const pi = useBomMaterialCostI18n()
/** 三层选中上下文 */
const { selecmedModelGroup, selecmedProductRow } = useBomMaterialCostMasterContext()

/** 次子表面板 ref */
const itemPanelRef = ref<InstanceType<typeof BomMaterialCostItemPanel> | null>(null)
/** 产品列表 loading */
const productLoading = ref(false)
/** 产品列表数据 */
const productDataSource = ref<BomMaterialCost[]>([])
/** 产品页码 */
const productPage = ref(getTaktDefaultPageIndex())
/** 产品每页条数 */
const productPageSize = ref(getTaktDefaultPageSize())
/** 产品 total */
const productTotal = ref(0)
/** 产品多选 keys */
const productSelecmedKeys = ref<(string | number)[]>([])
/** 中栏表格容器 */
const productTableWrapRef = ref<HTMLElement | null>(null)
/** 中栏 scroll.y */
const productTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 中栏 ResizeObserver */
let productTableScrollResizeObserver: ResizeObserver | null = null

/** 是否已选机种组 */
const hasModelGroup = computed(() => {
  const g = selecmedModelGroup.value as Record<string, unknown> | null
  return !!(g?.plantCode && g?.modelCode && g?.costingPeriod)
})
/** 是否已选产品行 */
const hasProductSelection = computed(() => !!selecmedProductRow.value)

/** 产品列默认可见 */
const productVisibleKeys = ref([
  'productCode',
  'productDescription',
  'materialType',
  'productMonthlyCost',
  'productMonthlyCalculation',
  'latestPurchaseCost',
  'currencyCode'])

/**
 * 产品行主键
 * @param record 产品行
 * @returns {string} Id
 */
function getProductRowKey(record: BomMaterialCostRowRecord): string {
  const row = record as BomMaterialCost & { id?: string }
  const id = String(row.bomMaterialCostId ?? row.id ?? '').trim()
  if (id) return id
  return `${row.plantCode ?? ''}|${row.materialType ?? ''}|${row.productCode ?? ''}|${row.costingPeriod ?? ''}`
}

/** 产品列定义 */
const productColumns = computed<TableColumnsType>(() => [
  {
    title: pi.label('productCode'),
    dataIndex: 'productCode',
    key: 'productCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: pi.label('productDescription'),
    dataIndex: 'productDescription',
    key: 'productDescription',
    width: 140,
    ellipsis: true,
  },
  {
    title: pi.label('materialType'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 100,
    ellipsis: true,
  },
  {
    title: pi.label('productMonthlyCost'),
    dataIndex: 'productMonthlyCost',
    key: 'productMonthlyCost',
    width: 120,
    ellipsis: true,
    customRender: ({ record }: { record: BomMaterialCost }) =>
      formatBomMaterialCostAmount(record.productMonthlyCost),
  },
  {
    title: pi.label('productMonthlyCalculation'),
    dataIndex: 'productMonthlyCalculation',
    key: 'productMonthlyCalculation',
    width: 120,
    ellipsis: true,
    customRender: ({ record }: { record: BomMaterialCost }) =>
      formatBomMaterialCostAmount(record.productMonthlyCalculation),
  },
  {
    title: pi.label('latestPurchaseCost'),
    dataIndex: 'latestPurchaseCost',
    key: 'latestPurchaseCost',
    width: 120,
    ellipsis: true,
    customRender: ({ record }: { record: BomMaterialCost }) =>
      formatBomMaterialCostAmount(record.latestPurchaseCost),
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 80,
  }])

/**
 * 列宽调整
 * @param width 宽度
 * @param column 列
 */
function handleProductResizeColumn(width: number, column: { width?: number }) {
  column.width = width
}

/**
 * 同步产品选中到上下文
 * @param record 产品行
 */
function syncProductSelection(record: BomMaterialCostRowRecord | null) {
  selecmedProductRow.value = record
}

/** 产品行选择 */
const productRowSelection = computed(() => ({
  type: 'radio' as const,
  selecmedRowKeys: productSelecmedKeys.value,
  onChange: (keys: (string | number)[], rows: BomMaterialCostRowRecord[]) => {
    productSelecmedKeys.value = keys
    syncProductSelection(rows[0] ?? null)
  },
}))

/**
 * 产品行点击
 * @param record 产品行
 */
function onProductClickRow(record: BomMaterialCost) {
  const key = getProductRowKey(record)
  return {
    onClick: () => {
      productSelecmedKeys.value = [key]
      syncProductSelection(record)
    },
    class: productSelecmedKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}

/** 按中栏容器重算 scroll.y */
function recalcProductTableScrollY(): void {
  const wrap = productTableWrapRef.value
  if (!wrap) {
    return
  }
  productTableScrollY.value = measureMasterDetailLrTableScrollY(wrap)
}

/** 监听中栏容器尺寸 */
function startProductTableScrollObserve(): void {
  stopProductTableScrollObserve()
  recalcProductTableScrollY()
  const wrap = productTableWrapRef.value
  if (!wrap) {
    return
  }
  productTableScrollResizeObserver = new ResizeObserver(() => {
    recalcProductTableScrollY()
  })
  productTableScrollResizeObserver.observe(wrap)
}

/** 停止中栏容器监听 */
function stopProductTableScrollObserve(): void {
  productTableScrollResizeObserver?.disconnect()
  productTableScrollResizeObserver = null
}

/**
 * 加载产品子表（同主表实体，按机种组过滤）
 * @returns {Promise<void>}
 */
async function loadProductData() {
  const g = selecmedModelGroup.value as Record<string, unknown> | null
  if (!g?.plantCode || !g?.modelCode || !g?.costingPeriod) {
    productDataSource.value = []
    productTotal.value = 0
    productSelecmedKeys.value = []
    syncProductSelection(null)
    return
  }
  productLoading.value = true
  try {
    const materialType = String(g.materialType ?? '').trim()
    const res = await getBomMaterialCostList({
      pageIndex: productPage.value,
      pageSize: productPageSize.value,
      plantCode: String(g.plantCode),
      modelCode: String(g.modelCode),
      costingPeriod: String(g.costingPeriod),
      ...(materialType ? { materialType } : {}),
    })
    productDataSource.value = res.data ?? []
    productTotal.value = res.total ?? 0
    if (productDataSource.value.length === 1) {
      const only = productDataSource.value[0]!
      productSelecmedKeys.value = [getProductRowKey(only)]
      syncProductSelection(only)
    } else if (
      selecmedProductRow.value
      && !productDataSource.value.some(
        (r) => getProductRowKey(r) === getProductRowKey(selecmedProductRow.value as BomMaterialCostRowRecord),
      )
    ) {
      productSelecmedKeys.value = []
      syncProductSelection(null)
    }
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    productDataSource.value = []
    productTotal.value = 0
  } finally {
    productLoading.value = false
    await nextTick()
    startProductTableScrollObserve()
  }
}

/**
 * 外部刷新（主表重算/导入后）
 * @returns {void}
 */
function reload() {
  productPage.value = getTaktDefaultPageIndex()
  void loadProductData()
  itemPanelRef.value?.reload?.()
}

watch(
  () => {
    const g = selecmedModelGroup.value as Record<string, unknown> | null
    return `${g?.plantCode ?? ''}|${g?.materialType ?? ''}|${g?.modelCode ?? ''}|${g?.costingPeriod ?? ''}`
  },
  () => {
    productPage.value = getTaktDefaultPageIndex()
    productSelecmedKeys.value = []
    syncProductSelection(null)
    void loadProductData()
  },
)

onMounted(async () => {
  await nextTick()
  if (hasModelGroup.value) {
    startProductTableScrollObserve()
  }
})

onBeforeUnmount(() => {
  stopProductTableScrollObserve()
})

defineExpose({ reload, loadProductData })
</script>
