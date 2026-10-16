<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-order/components -->
<!-- 文件名称：PurchaseOrderForm.vue -->
<!-- 功能描述：Takt采购订单实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-order-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="purchase-order-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-order-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
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
                  :api-params="{ documentType: '采购订单' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.purchaseOrderId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrderCode')"
                name="purchaseOrderCode"
              >
                <a-input
                  v-model:value="formState.purchaseOrderCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseRequestId')"
                name="purchaseRequestId"
              >
                <TaktSelect
                  v-model:value="formState.purchaseRequestId"
                  api-url="TaktPurchaseRequests/options"
                  :placeholder="pi.ph('purchaseRequestId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseRequestCode')"
                name="purchaseRequestCode"
              >
                <a-input
                  v-model:value="formState.purchaseRequestCode"
                  :placeholder="pi.ph('purchaseRequestCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.purchaseOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierName1')"
                name="supplierName1"
              >
                <a-input
                  v-model:value="formState.supplierName1"
                  :placeholder="pi.ph('supplierName1')"
                  show-count
                  :maxlength="140"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderDate')"
                name="orderDate"
              >
                <a-date-picker
                  v-model:value="formState.orderDate"
                  :placeholder="pi.ph('orderDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requiredArrivalDate')"
                name="requiredArrivalDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredArrivalDate"
                  :placeholder="pi.ph('requiredArrivalDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualArrivalDate')"
                name="actualArrivalDate"
              >
                <a-date-picker
                  v-model:value="formState.actualArrivalDate"
                  :placeholder="pi.ph('actualArrivalDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseGroup')"
                name="purchaseGroup"
              >
                <TaktSelect
                  v-model:value="formState.purchaseGroup"
                  api-url="TaktPurchaseGroups/options"
                  :placeholder="pi.ph('purchaseGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrderType')"
                name="purchaseOrderType"
              >
                <TaktSelect
                  v-model:value="formState.purchaseOrderType"
                  dict-type="logistics_procurement_purchase_order_type"
                  :placeholder="pi.ph('purchaseOrderType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paymentTerms')"
                name="paymentTerms"
              >
                <TaktSelect
                  v-model:value="formState.paymentTerms"
                  dict-type="accounting_financial_payment_terms_param"
                  :placeholder="pi.ph('paymentTerms')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingProcedure')"
                name="pricingProcedure"
              >
                <TaktSelect
                  v-model:value="formState.pricingProcedure"
                  dict-type="logistics_procurement_pricing_procedure"
                  :placeholder="pi.ph('pricingProcedure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingConditionCode')"
                name="pricingConditionCode"
              >
                <a-input
                  v-model:value="formState.pricingConditionCode"
                  :placeholder="pi.ph('pricingConditionCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalQuantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="pi.ph('totalQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalAmount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="pi.ph('totalAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountAmount')"
                name="discountAmount"
              >
                <a-input-number
                  v-model:value="formState.discountAmount"
                  :placeholder="pi.ph('discountAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.purchaseOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('exchangeRate')"
                name="exchangeRate"
              >
                <a-input-number
                  v-model:value="formState.exchangeRate"
                  :placeholder="pi.ph('exchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxCode')"
                name="taxCode"
              >
                <TaktSelect
                  v-model:value="formState.taxCode"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('taxCode')"
                  :disabled="!!formData?.purchaseOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxRate')"
                name="taxRate"
              >
                <a-input-number
                  v-model:value="formState.taxRate"
                  :placeholder="pi.ph('taxRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxAmount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="pi.ph('taxAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualAmount')"
                name="actualAmount"
              >
                <a-input-number
                  v-model:value="formState.actualAmount"
                  :placeholder="pi.ph('actualAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('receivedQuantity')"
                name="receivedQuantity"
              >
                <a-input-number
                  v-model:value="formState.receivedQuantity"
                  :placeholder="pi.ph('receivedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('receivedAmount')"
                name="receivedAmount"
              >
                <a-input-number
                  v-model:value="formState.receivedAmount"
                  :placeholder="pi.ph('receivedAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paidAmount')"
                name="paidAmount"
              >
                <a-input-number
                  v-model:value="formState.paidAmount"
                  :placeholder="pi.ph('paidAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paymentMethod')"
                name="paymentMethod"
              >
                <TaktSelect
                  v-model:value="formState.paymentMethod"
                  dict-type="accounting_financial_payment_method"
                  :placeholder="pi.ph('paymentMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deliveryMethod')"
                name="deliveryMethod"
              >
                <TaktSelect
                  v-model:value="formState.deliveryMethod"
                  dict-type="logistics_sales_delivery_method"
                  :placeholder="pi.ph('deliveryMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deliveryAddress')"
                name="deliveryAddress"
              >
                <a-textarea
                  v-model:value="formState.deliveryAddress"
                  :placeholder="pi.ph('deliveryAddress')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('orderStatus')"
                name="orderStatus"
              >
                <TaktSelect
                  v-model:value="formState.orderStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('orderStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deliveryStatus')"
                name="deliveryStatus"
              >
                <TaktSelect
                  v-model:value="formState.deliveryStatus"
                  dict-type="logistics_sales_delivery_status"
                  :placeholder="pi.ph('deliveryStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
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
      class="purchase-order-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="purchaseOrderItemTableRef"
      v-model="childPurchaseOrderItemRows"
      :columns="purchaseOrderItemFormColumns"
      :title="purchaseOrderItemPi.self()"
      :add-button-entity="purchaseOrderItemPi.self()"
      id-field="purchaseOrderItemId"
      :default-row="createDefaultPurchaseOrderItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-purchaseUnit="{ record }">
        <TaktSelect
          v-model:value="record.purchaseUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('purchaseUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-purchasePerUnit="{ record }">
        <TaktSelect
          v-model:value="record.purchasePerUnit"
          dict-type="logistics_materials_price_unit_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('purchasePerUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-discountRate="{ record }">
        <TaktSelect
          v-model:value="record.discountRate"
          dict-type="logistics_sales_discount_rate_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('discountRate')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-weightUnit="{ record }">
        <TaktSelect
          v-model:value="record.weightUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('weightUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-volumeUnit="{ record }">
        <TaktSelect
          v-model:value="record.volumeUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('volumeUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-profitCenterCode="{ record }">
        <TaktSelect
          v-model:value="record.profitCenterCode"
          api-url="TaktProfitCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.queryPh('profitCenterCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-deliveryStatus="{ record }">
        <TaktSelect
          v-model:value="record.deliveryStatus"
          dict-type="logistics_sales_delivery_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('deliveryStatus')"
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
          :placeholder="purchaseOrderItemPi.ph('isObsolete')"
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
 * Takt采购订单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseOrderI18n } from '../composables/usePurchaseOrderI18n'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'

/** 实体字段 i18n */
const pi = usePurchaseOrderI18n()

import type { PurchaseOrderCreate } from '@/types/logistics/procurement/purchaseOrder.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseOrderCode","purchaseRequestId","purchaseRequestCode","supplierCode","supplierName1","orderDate","requiredArrivalDate","actualArrivalDate","purchaseGroup","purchaseOrderType","paymentTerms","pricingProcedure","pricingConditionCode","totalQuantity","totalAmount","discountAmount","currencyCode","exchangeRate","taxCode","taxRate","taxAmount","actualAmount","receivedQuantity","receivedAmount","paidAmount","paymentMethod","deliveryMethod","deliveryAddress","orderStatus","deliveryStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { usePurchaseOrderItemI18n } from '../composables/usePurchaseOrderItemI18n'

const purchaseOrderItemPi = usePurchaseOrderItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPurchaseOrderItemRows = ref<Record<string, unknown>[]>([])
const purchaseOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPurchaseOrderItemRow(row: Record<string, unknown>): boolean {
  const id = row.purchaseOrderItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPurchaseOrderItemLineNumber(): number {
  const rows = purchaseOrderItemTableRef.value?.getRows?.() ?? childPurchaseOrderItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 purchaseOrderItem 可编辑列 */
const purchaseOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: purchaseOrderItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'requestCode',
    title: purchaseOrderItemPi.label('requestCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseOrderItemPi.ph('requestCode'),
  },
  {
    key: 'requestLineNumber',
    title: purchaseOrderItemPi.label('requestLineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: purchaseOrderItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'purchaseInfoCode',
    title: purchaseOrderItemPi.label('purchaseInfoCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseOrderItemPi.ph('purchaseInfoCode'),
  },
  {
    key: 'supplierMaterialCode',
    title: purchaseOrderItemPi.label('supplierMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseOrderItemPi.ph('supplierMaterialCode'),
  },
  {
    key: 'purchaseUnit',
    title: purchaseOrderItemPi.label('purchaseUnit'),
    width: 140,
  },
  {
    key: 'orderQuantity',
    title: purchaseOrderItemPi.label('orderQuantity'),
    width: 140,
  },
  {
    key: 'receivedQuantity',
    title: purchaseOrderItemPi.label('receivedQuantity'),
    width: 140,
  },
  {
    key: 'purchasePerUnit',
    title: purchaseOrderItemPi.label('purchasePerUnit'),
    width: 140,
  },
  {
    key: 'purchaseUnitPrice',
    title: purchaseOrderItemPi.label('purchaseUnitPrice'),
    width: 140,
  },
  {
    key: 'discountRate',
    title: purchaseOrderItemPi.label('discountRate'),
    width: 140,
  },
  {
    key: 'discountAmount',
    title: purchaseOrderItemPi.label('discountAmount'),
    width: 140,
  },
  {
    key: 'taxIncludedAmount',
    title: purchaseOrderItemPi.label('taxIncludedAmount'),
    width: 140,
  },
  {
    key: 'untaxedAmount',
    title: purchaseOrderItemPi.label('untaxedAmount'),
    width: 140,
  },
  {
    key: 'taxAmount',
    title: purchaseOrderItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'purchaseAmount',
    title: purchaseOrderItemPi.label('purchaseAmount'),
    width: 140,
  },
  {
    key: 'pricingDate',
    title: purchaseOrderItemPi.label('pricingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'grossWeight',
    title: purchaseOrderItemPi.label('grossWeight'),
    width: 140,
  },
  {
    key: 'netWeight',
    title: purchaseOrderItemPi.label('netWeight'),
    width: 140,
  },
  {
    key: 'weightUnit',
    title: purchaseOrderItemPi.label('weightUnit'),
    width: 140,
  },
  {
    key: 'volume',
    title: purchaseOrderItemPi.label('volume'),
    width: 140,
  },
  {
    key: 'volumeUnit',
    title: purchaseOrderItemPi.label('volumeUnit'),
    width: 140,
  },
  {
    key: 'profitCenterCode',
    title: purchaseOrderItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'deliveryStatus',
    title: purchaseOrderItemPi.label('deliveryStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: purchaseOrderItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseOrderCreate & { purchaseOrderId?: string }> | null | undefined) {
  const rows_purchaseOrderItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childPurchaseOrderItemRows.value = rows_purchaseOrderItem
}

function createDefaultPurchaseOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextPurchaseOrderItemLineNumber(),
    requestCode: '',
    requestLineNumber: 0,
    materialCode: '',
    purchaseInfoCode: '',
    supplierMaterialCode: '',
    purchaseUnit: '',
    orderQuantity: 0,
    receivedQuantity: 0,
    purchasePerUnit: 0,
    purchaseUnitPrice: 0,
    discountRate: 0,
    discountAmount: 0,
    taxIncludedAmount: 0,
    untaxedAmount: 0,
    taxAmount: 0,
    purchaseAmount: 0,
    pricingDate: '',
    grossWeight: 0,
    netWeight: 0,
    weightUnit: '',
    volume: 0,
    volumeUnit: '',
    profitCenterCode: '',
    deliveryStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: purchaseOrderItemTableRef.value?.getRows?.() ?? childPurchaseOrderItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        purchaseOrderId: isUpdate ? masterId : 0,
      }
      if (isUpdate && isPersistedPurchaseOrderItemRow(row)) {
        normalized.purchaseOrderItemId = row.purchaseOrderItemId
      } else {
        delete normalized.purchaseOrderItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseOrderCreate & { purchaseOrderId?: string }> | null
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
  purchaseOrderType: "F-NB",
  paymentTerms: "PREPAYSHIP",
  pricingProcedure: "ZRM001",
  currencyCode: "CNY",
  taxCode: "J2",
  paymentMethod: 0,
  deliveryMethod: 0,
  orderStatus: 1,
  deliveryStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseOrderId) {
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
    if (!props.formData?.purchaseOrderId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.purchaseOrderId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'purchaseOrderCode',
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
  purchaseOrderCode: [{
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
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  orderDate: [
    {
      required: true,
      message: pi.ph('orderDate'),
      trigger: 'change'
    }
  ],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  discountAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('discountAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('discountAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  exchangeRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('exchangeRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('exchangeRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  receivedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('receivedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('receivedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  receivedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('receivedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('receivedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  paidAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paidAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paidAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  paymentMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paymentMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paymentMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  deliveryMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('deliveryMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('deliveryMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  orderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  deliveryStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('deliveryStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('deliveryStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseOrderItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalQuantity' in payload) {
    const rawtotalQuantity = payload.totalQuantity
    if (rawtotalQuantity === undefined || rawtotalQuantity === null || rawtotalQuantity === '') {
      delete payload.totalQuantity
    } else {
      const numtotalQuantity = typeof rawtotalQuantity === 'number' ? rawtotalQuantity : Number(rawtotalQuantity)
      if (Number.isFinite(numtotalQuantity)) payload.totalQuantity = numtotalQuantity
      else delete payload.totalQuantity
    }
  }
  if ('totalAmount' in payload) {
    const rawtotalAmount = payload.totalAmount
    if (rawtotalAmount === undefined || rawtotalAmount === null || rawtotalAmount === '') {
      delete payload.totalAmount
    } else {
      const numtotalAmount = typeof rawtotalAmount === 'number' ? rawtotalAmount : Number(rawtotalAmount)
      if (Number.isFinite(numtotalAmount)) payload.totalAmount = numtotalAmount
      else delete payload.totalAmount
    }
  }
  if ('discountAmount' in payload) {
    const rawdiscountAmount = payload.discountAmount
    if (rawdiscountAmount === undefined || rawdiscountAmount === null || rawdiscountAmount === '') {
      delete payload.discountAmount
    } else {
      const numdiscountAmount = typeof rawdiscountAmount === 'number' ? rawdiscountAmount : Number(rawdiscountAmount)
      if (Number.isFinite(numdiscountAmount)) payload.discountAmount = numdiscountAmount
      else delete payload.discountAmount
    }
  }
  if ('exchangeRate' in payload) {
    const rawexchangeRate = payload.exchangeRate
    if (rawexchangeRate === undefined || rawexchangeRate === null || rawexchangeRate === '') {
      delete payload.exchangeRate
    } else {
      const numexchangeRate = typeof rawexchangeRate === 'number' ? rawexchangeRate : Number(rawexchangeRate)
      if (Number.isFinite(numexchangeRate)) payload.exchangeRate = numexchangeRate
      else delete payload.exchangeRate
    }
  }
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    if (rawtaxRate === undefined || rawtaxRate === null || rawtaxRate === '') {
      delete payload.taxRate
    } else {
      const numtaxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
      if (Number.isFinite(numtaxRate)) payload.taxRate = numtaxRate
      else delete payload.taxRate
    }
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    if (rawtaxAmount === undefined || rawtaxAmount === null || rawtaxAmount === '') {
      delete payload.taxAmount
    } else {
      const numtaxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
      if (Number.isFinite(numtaxAmount)) payload.taxAmount = numtaxAmount
      else delete payload.taxAmount
    }
  }
  if ('actualAmount' in payload) {
    const rawactualAmount = payload.actualAmount
    if (rawactualAmount === undefined || rawactualAmount === null || rawactualAmount === '') {
      delete payload.actualAmount
    } else {
      const numactualAmount = typeof rawactualAmount === 'number' ? rawactualAmount : Number(rawactualAmount)
      if (Number.isFinite(numactualAmount)) payload.actualAmount = numactualAmount
      else delete payload.actualAmount
    }
  }
  if ('receivedQuantity' in payload) {
    const rawreceivedQuantity = payload.receivedQuantity
    if (rawreceivedQuantity === undefined || rawreceivedQuantity === null || rawreceivedQuantity === '') {
      delete payload.receivedQuantity
    } else {
      const numreceivedQuantity = typeof rawreceivedQuantity === 'number' ? rawreceivedQuantity : Number(rawreceivedQuantity)
      if (Number.isFinite(numreceivedQuantity)) payload.receivedQuantity = numreceivedQuantity
      else delete payload.receivedQuantity
    }
  }
  if ('receivedAmount' in payload) {
    const rawreceivedAmount = payload.receivedAmount
    if (rawreceivedAmount === undefined || rawreceivedAmount === null || rawreceivedAmount === '') {
      delete payload.receivedAmount
    } else {
      const numreceivedAmount = typeof rawreceivedAmount === 'number' ? rawreceivedAmount : Number(rawreceivedAmount)
      if (Number.isFinite(numreceivedAmount)) payload.receivedAmount = numreceivedAmount
      else delete payload.receivedAmount
    }
  }
  if ('paidAmount' in payload) {
    const rawpaidAmount = payload.paidAmount
    if (rawpaidAmount === undefined || rawpaidAmount === null || rawpaidAmount === '') {
      delete payload.paidAmount
    } else {
      const numpaidAmount = typeof rawpaidAmount === 'number' ? rawpaidAmount : Number(rawpaidAmount)
      if (Number.isFinite(numpaidAmount)) payload.paidAmount = numpaidAmount
      else delete payload.paidAmount
    }
  }
  if ('paymentMethod' in payload) {
    const rawpaymentMethod = payload.paymentMethod
    if (rawpaymentMethod === undefined || rawpaymentMethod === null || rawpaymentMethod === '') {
      delete payload.paymentMethod
    } else {
      const numpaymentMethod = typeof rawpaymentMethod === 'number' ? rawpaymentMethod : Number(rawpaymentMethod)
      if (Number.isFinite(numpaymentMethod)) payload.paymentMethod = numpaymentMethod
      else delete payload.paymentMethod
    }
  }
  if ('deliveryMethod' in payload) {
    const rawdeliveryMethod = payload.deliveryMethod
    if (rawdeliveryMethod === undefined || rawdeliveryMethod === null || rawdeliveryMethod === '') {
      delete payload.deliveryMethod
    } else {
      const numdeliveryMethod = typeof rawdeliveryMethod === 'number' ? rawdeliveryMethod : Number(rawdeliveryMethod)
      if (Number.isFinite(numdeliveryMethod)) payload.deliveryMethod = numdeliveryMethod
      else delete payload.deliveryMethod
    }
  }
  if ('orderStatus' in payload) {
    const raworderStatus = payload.orderStatus
    if (raworderStatus === undefined || raworderStatus === null || raworderStatus === '') {
      delete payload.orderStatus
    } else {
      const numorderStatus = typeof raworderStatus === 'number' ? raworderStatus : Number(raworderStatus)
      if (Number.isFinite(numorderStatus)) payload.orderStatus = numorderStatus
      else delete payload.orderStatus
    }
  }
  if ('deliveryStatus' in payload) {
    const rawdeliveryStatus = payload.deliveryStatus
    if (rawdeliveryStatus === undefined || rawdeliveryStatus === null || rawdeliveryStatus === '') {
      delete payload.deliveryStatus
    } else {
      const numdeliveryStatus = typeof rawdeliveryStatus === 'number' ? rawdeliveryStatus : Number(rawdeliveryStatus)
      if (Number.isFinite(numdeliveryStatus)) payload.deliveryStatus = numdeliveryStatus
      else delete payload.deliveryStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.purchaseOrderId) {
    payload.purchaseOrderId = props.formData.purchaseOrderId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseOrderId)
  childPurchaseOrderItemRows.value = []
  purchaseOrderItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.purchase-order-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.purchase-order-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.purchase-order-form__master :deep(.purchase-order-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.purchase-order-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.purchase-order-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.purchase-order-form__master :deep(.ant-tabs-content),
.purchase-order-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
