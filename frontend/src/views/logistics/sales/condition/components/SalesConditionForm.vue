<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/condition/components -->
<!-- 文件名称：SalesConditionForm.vue -->
<!-- 功能描述：Takt销售条件实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form condition-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="condition-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="condition-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '销售条件' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.salesConditionId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesConditionCode')"
                name="salesConditionCode"
              >
                <a-input
                  v-model:value="formState.salesConditionCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceType')"
                name="priceType"
              >
                <TaktSelect
                  v-model:value="formState.priceType"
                  dict-type="logistics_procurement_price_type"
                  :placeholder="pi.ph('priceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <TaktSelect
                  v-model:value="formState.customerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerCode')"
                  :disabled="!!formData?.salesConditionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.salesConditionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDescription')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="pi.ph('materialDescription')"
                  :rows="2"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesGroup')"
                name="salesGroup"
              >
                <TaktSelect
                  v-model:value="formState.salesGroup"
                  api-url="TaktSalesGroups/options"
                  :placeholder="pi.ph('salesGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxCode')"
                name="taxCode"
              >
                <TaktSelect
                  v-model:value="formState.taxCode"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('taxCode')"
                  :disabled="!!formData?.salesConditionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grBasedInvoiceInspection')"
                name="grBasedInvoiceInspection"
              >
                <TaktSelect
                  v-model:value="formState.grBasedInvoiceInspection"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('grBasedInvoiceInspection')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('pricingDateControl')"
                name="pricingDateControl"
              >
                <TaktSelect
                  v-model:value="formState.pricingDateControl"
                  dict-type="logistics_procurement_pricing_date_control"
                  :placeholder="pi.ph('pricingDateControl')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('validFrom')"
                name="validFrom"
              >
                <a-date-picker
                  v-model:value="formState.validFrom"
                  :placeholder="pi.ph('validFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('validTo')"
                name="validTo"
              >
                <a-date-picker
                  v-model:value="formState.validTo"
                  :placeholder="pi.ph('validTo')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('salesQuotationId')"
                name="salesQuotationId"
              >
                <TaktSelect
                  v-model:value="formState.salesQuotationId"
                  api-url="TaktSalesQuotations/options"
                  :placeholder="pi.ph('salesQuotationId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('salesQuotationCode')"
                name="salesQuotationCode"
              >
                <a-input
                  v-model:value="formState.salesQuotationCode"
                  :placeholder="pi.ph('salesQuotationCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('variableKey')"
                name="variableKey"
              >
                <a-input
                  v-model:value="formState.variableKey"
                  :placeholder="pi.ph('variableKey')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    </div>
    <!-- 下：子表（弹窗视口约 1/2） -->
    <div
      ref="detailHostRef"
      class="condition-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="salesConditionItemTableRef"
      v-model="childSalesConditionItemRows"
      :columns="salesConditionItemFormColumns"
      :title="salesConditionItemPi.self()"
      :add-button-entity="salesConditionItemPi.self()"
      id-field="salesConditionItemId"
      :default-row="createDefaultSalesConditionItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-scaleType="{ record }">
        <TaktSelect
          v-model:value="record.scaleType"
          dict-type="logistics_procurement_scale"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('scaleType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleBasis="{ record }">
        <TaktSelect
          v-model:value="record.scaleBasis"
          dict-type="logistics_procurement_scale_basis"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('scaleBasis')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleUnit="{ record }">
        <TaktSelect
          v-model:value="record.scaleUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('scaleUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleCurrencyCode="{ record }">
        <TaktSelect
          v-model:value="record.scaleCurrencyCode"
          dict-type="accounting_financial_currency_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('scaleCurrencyCode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-calculationType="{ record }">
        <TaktSelect
          v-model:value="record.calculationType"
          dict-type="logistics_procurement_calculation_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('calculationType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-conditionCurrencyCode="{ record }">
        <TaktSelect
          v-model:value="record.conditionCurrencyCode"
          dict-type="accounting_financial_currency_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('conditionCurrencyCode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-priceUnit="{ record }">
        <TaktSelect
          v-model:value="record.priceUnit"
          dict-type="logistics_materials_price_unit_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('priceUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-unitOfMeasure="{ record }">
        <TaktSelect
          v-model:value="record.unitOfMeasure"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('unitOfMeasure')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesConditionItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售条件实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/condition/components
 */

import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesConditionI18n } from '../composables/useConditionI18n'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'

/** 实体字段 i18n */
const pi = useSalesConditionI18n()

import type { SalesConditionCreate } from '@/types/logistics/sales/condition.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","salesConditionCode","priceType","customerCode","materialCode","materialDescription","salesGroup","taxCode","grBasedInvoiceInspection","pricingDateControl","validFrom","validTo","salesQuotationId","salesQuotationCode","variableKey","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useSalesConditionItemI18n } from '../composables/useConditionItemI18n'

const salesConditionItemPi = useSalesConditionItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSalesConditionItemRows = ref<Record<string, unknown>[]>([])
const salesConditionItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 salesConditionItem 可编辑列 */
const salesConditionItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'salesConditionSeq',
    title: salesConditionItemPi.label('salesConditionSeq'),
    width: 140,
  },
  {
    key: 'scaleType',
    title: salesConditionItemPi.label('scaleType'),
    width: 140,
  },
  {
    key: 'scaleBasis',
    title: salesConditionItemPi.label('scaleBasis'),
    width: 140,
  },
  {
    key: 'scaleQuantity',
    title: salesConditionItemPi.label('scaleQuantity'),
    width: 140,
  },
  {
    key: 'scaleUnit',
    title: salesConditionItemPi.label('scaleUnit'),
    width: 140,
  },
  {
    key: 'scaleValue',
    title: salesConditionItemPi.label('scaleValue'),
    width: 140,
  },
  {
    key: 'scaleCurrencyCode',
    title: salesConditionItemPi.label('scaleCurrencyCode'),
    width: 140,
  },
  {
    key: 'calculationType',
    title: salesConditionItemPi.label('calculationType'),
    width: 140,
  },
  {
    key: 'price',
    title: salesConditionItemPi.label('price'),
    width: 140,
  },
  {
    key: 'conditionCurrencyCode',
    title: salesConditionItemPi.label('conditionCurrencyCode'),
    width: 140,
  },
  {
    key: 'priceUnit',
    title: salesConditionItemPi.label('priceUnit'),
    width: 140,
  },
  {
    key: 'unitOfMeasure',
    title: salesConditionItemPi.label('unitOfMeasure'),
    width: 140,
  },
  {
    key: 'minOrderQuantity',
    title: salesConditionItemPi.label('minOrderQuantity'),
    width: 140,
  },
  {
    key: 'roundingValue',
    title: salesConditionItemPi.label('roundingValue'),
    width: 140,
  },
  {
    key: 'plannedDeliveryTimeDays',
    title: salesConditionItemPi.label('plannedDeliveryTimeDays'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: salesConditionItemPi.label('isObsolete'),
    width: 140,
  },
  {
    key: 'scaleQuantities',
    title: salesConditionItemPi.label('scaleQuantities'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesConditionItemPi.ph('scaleQuantities'),
  },
  {
    key: 'scaleValues',
    title: salesConditionItemPi.label('scaleValues'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesConditionItemPi.ph('scaleValues'),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesConditionCreate & { salesConditionId?: string }> | null | undefined) {
  const rows_salesConditionItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSalesConditionItemRows.value = rows_salesConditionItem
}

function createDefaultSalesConditionItemRow(): Record<string, unknown> {
  return {
    salesConditionSeq: 0,
    scaleType: '',
    scaleBasis: '',
    scaleQuantity: 0,
    scaleUnit: '',
    scaleValue: 0,
    scaleCurrencyCode: '',
    calculationType: '',
    price: 0,
    conditionCurrencyCode: '',
    priceUnit: 0,
    unitOfMeasure: '',
    minOrderQuantity: 0,
    roundingValue: 0,
    plannedDeliveryTimeDays: 0,
    isObsolete: 0,
    scaleQuantities: '',
    scaleValues: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesConditionId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: salesConditionItemTableRef.value?.getRows?.() ?? childSalesConditionItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
      salesConditionId: isUpdate ? masterId : 0,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesConditionCreate & { salesConditionId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
} from '@/utils/tableScroll'

/** 子表半区宿主（弹窗视口约 1/2） */
const detailHostRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（半区内扣除标题/表头/汇总） */
const detailScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailHostResizeObserver: ResizeObserver | null = null

/**
 * 按子表半区实测 scroll.y
 */
function recalcDetailScrollYPx(): void {
  const host = detailHostRef.value
  if (host == null || host.clientHeight <= 0) {
    return
  }
  const tableRoot = host.querySelector('.takt-editable-table') as HTMLElement | null
  const toolbar = tableRoot?.querySelector(':scope > .mb-2') as HTMLElement | null
  const toolbarH = toolbar?.offsetHeight ?? 0
  const sectionPad = 12
  const next = Math.floor(
    host.clientHeight
      - toolbarH
      - sectionPad
      - TAKT_TABLE_HEADER_FALLBACK_PX
      - TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
  )
  detailScrollYPx.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, next)
}

/** 监听弹窗/半区尺寸变化 */
function bindDetailHostResizeObserver(): void {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  const host = detailHostRef.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const target =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
    ?? host
  detailHostResizeObserver = new ResizeObserver(() => {
    recalcDetailScrollYPx()
  })
  detailHostResizeObserver.observe(target)
  detailHostResizeObserver.observe(host)
}

onMounted(() => {
  void nextTick(() => {
    recalcDetailScrollYPx()
    bindDetailHostResizeObserver()
  })
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', recalcDetailScrollYPx)
  }
})

onBeforeUnmount(() => {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', recalcDetailScrollYPx)
  }
})

/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  priceType: "PB00",
  taxCode: "J2",
  pricingDateControl: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})



/** 编辑态灌入 formData；新增态恢复默认值（须含 salesConditionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesConditionId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

watch(
  () => props.formData,
  () => {
    void nextTick(() => {
      recalcDetailScrollYPx()
      bindDetailHostResizeObserver()
    })
  },
)


/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.salesConditionId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.salesConditionId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'salesConditionCode',
})

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  salesConditionCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  priceType: [
    {
      required: true,
      message: pi.ph('priceType'),
      trigger: 'change'
    }
  ],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  grBasedInvoiceInspection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('grBasedInvoiceInspection'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('grBasedInvoiceInspection'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pricingDateControl: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('pricingDateControl'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('pricingDateControl'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  validFrom: [
    {
      required: true,
      message: pi.ph('validFrom'),
      trigger: 'change'
    }
  ],
  validTo: [
    {
      required: true,
      message: pi.ph('validTo'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await salesConditionItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('grBasedInvoiceInspection' in payload) {
    const rawgrBasedInvoiceInspection = payload.grBasedInvoiceInspection
    if (rawgrBasedInvoiceInspection === undefined || rawgrBasedInvoiceInspection === null || rawgrBasedInvoiceInspection === '') {
      delete payload.grBasedInvoiceInspection
    } else {
      const numgrBasedInvoiceInspection = typeof rawgrBasedInvoiceInspection === 'number' ? rawgrBasedInvoiceInspection : Number(rawgrBasedInvoiceInspection)
      if (Number.isFinite(numgrBasedInvoiceInspection)) payload.grBasedInvoiceInspection = numgrBasedInvoiceInspection
      else delete payload.grBasedInvoiceInspection
    }
  }
  if ('pricingDateControl' in payload) {
    const rawpricingDateControl = payload.pricingDateControl
    if (rawpricingDateControl === undefined || rawpricingDateControl === null || rawpricingDateControl === '') {
      delete payload.pricingDateControl
    } else {
      const numpricingDateControl = typeof rawpricingDateControl === 'number' ? rawpricingDateControl : Number(rawpricingDateControl)
      if (Number.isFinite(numpricingDateControl)) payload.pricingDateControl = numpricingDateControl
      else delete payload.pricingDateControl
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.salesConditionId) {
    payload.salesConditionId = props.formData.salesConditionId
    delete payload.numberingRuleCode
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesConditionId)
  childSalesConditionItemRows.value = []
  salesConditionItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.condition-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.condition-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.condition-form__master :deep(.condition-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.condition-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.condition-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.condition-form__master :deep(.ant-tabs-content),
.condition-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
