<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-request/components -->
<!-- 文件名称：PurchaseRequestForm.vue -->
<!-- 功能描述：Takt采购申请实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-request-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="purchase-request-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-request-form-tabs"
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
                  :api-params="{ documentType: '采购申请' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.purchaseRequestId || loading"
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
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseInquiryId')"
                name="purchaseInquiryId"
              >
                <TaktSelect
                  v-model:value="formState.purchaseInquiryId"
                  api-url="TaktPurchaseInquirys/options"
                  :placeholder="pi.ph('purchaseInquiryId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseInquiryCode')"
                name="purchaseInquiryCode"
              >
                <a-input
                  v-model:value="formState.purchaseInquiryCode"
                  :placeholder="pi.ph('purchaseInquiryCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchasePlanId')"
                name="purchasePlanId"
              >
                <a-input
                  v-model:value="formState.purchasePlanId"
                  :placeholder="pi.ph('purchasePlanId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchasePlanCode')"
                name="purchasePlanCode"
              >
                <a-input
                  v-model:value="formState.purchasePlanCode"
                  :placeholder="pi.ph('purchasePlanCode')"
                  show-count
                  :maxlength="40"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('chainScheme')"
                name="chainScheme"
              >
                <TaktSelect
                  v-model:value="formState.chainScheme"
                  dict-type="logistics_procurement_chain_scheme"
                  :placeholder="pi.ph('chainScheme')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('poDecision')"
                name="poDecision"
              >
                <a-input-number
                  v-model:value="formState.poDecision"
                  :placeholder="pi.ph('poDecision')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countersignId')"
                name="countersignId"
              >
                <TaktSelect
                  v-model:value="formState.countersignId"
                  api-url="TaktCountersigns/options"
                  :placeholder="pi.ph('countersignId')"
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
                :label="pi.label('countersignCode')"
                name="countersignCode"
              >
                <a-input
                  v-model:value="formState.countersignCode"
                  :placeholder="pi.ph('countersignCode')"
                  show-count
                  :maxlength="50"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestDate')"
                name="requestDate"
              >
                <a-date-picker
                  v-model:value="formState.requestDate"
                  :placeholder="pi.ph('requestDate')"
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
                :label="pi.label('requestEmployeeId')"
                name="requestEmployeeId"
              >
                <TaktSelect
                  v-model:value="formState.requestEmployeeId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('requestEmployeeId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestEmployeeName')"
                name="requestEmployeeName"
              >
                <a-input
                  v-model:value="formState.requestEmployeeName"
                  :placeholder="pi.ph('requestEmployeeName')"
                  show-count
                  :maxlength="80"
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
                  :disabled="!!formData?.purchaseRequestId"
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
                :label="pi.label('purchaseRequestType')"
                name="purchaseRequestType"
              >
                <TaktSelect
                  v-model:value="formState.purchaseRequestType"
                  dict-type="logistics_procurement_purchase_order_type"
                  :placeholder="pi.ph('purchaseRequestType')"
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
                  :disabled="!!formData?.purchaseRequestId"
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
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.purchaseRequestId"
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
                  :disabled="!!formData?.purchaseRequestId"
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
                :label="pi.label('convertedQuantity')"
                name="convertedQuantity"
              >
                <a-input-number
                  v-model:value="formState.convertedQuantity"
                  :placeholder="pi.ph('convertedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('convertedAmount')"
                name="convertedAmount"
              >
                <a-input-number
                  v-model:value="formState.convertedAmount"
                  :placeholder="pi.ph('convertedAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestReason')"
                name="requestReason"
              >
                <a-input
                  v-model:value="formState.requestReason"
                  :placeholder="pi.ph('requestReason')"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestStatus')"
                name="requestStatus"
              >
                <TaktSelect
                  v-model:value="formState.requestStatus"
                  dict-type="sys_approval_status"
                  :placeholder="pi.ph('requestStatus')"
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
                :label="pi.label('convertedStatus')"
                name="convertedStatus"
              >
                <TaktSelect
                  v-model:value="formState.convertedStatus"
                  dict-type="sys_convert_status"
                  :placeholder="pi.ph('convertedStatus')"
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
      class="purchase-request-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="purchaseRequestItemTableRef"
      v-model="childPurchaseRequestItemRows"
      :columns="purchaseRequestItemFormColumns"
      :title="purchaseRequestItemPi.self()"
      :add-button-entity="purchaseRequestItemPi.self()"
      id-field="purchaseRequestItemId"
      :default-row="createDefaultPurchaseRequestItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-allocationCategory="{ record }">
        <TaktSelect
          v-model:value="record.allocationCategory"
          dict-type="logistics_sales_allocation_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.ph('allocationCategory')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-requestUnit="{ record }">
        <TaktSelect
          v-model:value="record.requestUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.ph('requestUnit')"
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
          :placeholder="purchaseRequestItemPi.ph('purchasePerUnit')"
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
          :placeholder="purchaseRequestItemPi.ph('weightUnit')"
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
          :placeholder="purchaseRequestItemPi.ph('volumeUnit')"
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
          :placeholder="purchaseRequestItemPi.queryPh('profitCenterCode', 'select')"
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
          :placeholder="purchaseRequestItemPi.ph('isObsolete')"
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
 * Takt采购申请实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-request/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseRequestI18n } from '../composables/usePurchaseRequestI18n'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'

/** 实体字段 i18n */
const pi = usePurchaseRequestI18n()

import type { PurchaseRequestCreate } from '@/types/logistics/procurement/purchaseRequest.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseRequestCode","purchaseInquiryId","purchaseInquiryCode","purchasePlanId","purchasePlanCode","chainScheme","poDecision","countersignId","countersignCode","requestDate","requiredArrivalDate","requestEmployeeId","requestEmployeeName","supplierCode","supplierName1","purchaseRequestType","pricingProcedure","pricingConditionCode","currencyCode","taxCode","taxRate","taxAmount","totalQuantity","totalAmount","convertedQuantity","convertedAmount","requestReason","requestStatus","convertedStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { usePurchaseRequestItemI18n } from '../composables/usePurchaseRequestItemI18n'

const purchaseRequestItemPi = usePurchaseRequestItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPurchaseRequestItemRows = ref<Record<string, unknown>[]>([])
const purchaseRequestItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPurchaseRequestItemRow(row: Record<string, unknown>): boolean {
  const id = row.purchaseRequestItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPurchaseRequestItemLineNumber(): number {
  const rows = purchaseRequestItemTableRef.value?.getRows?.() ?? childPurchaseRequestItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 purchaseRequestItem 可编辑列 */
const purchaseRequestItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'purchasePlanItemId',
    title: purchaseRequestItemPi.label('purchasePlanItemId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseRequestItemPi.ph('purchasePlanItemId'),
  },
  {
    key: 'lineNumber',
    title: purchaseRequestItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'allocationCategory',
    title: purchaseRequestItemPi.label('allocationCategory'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: purchaseRequestItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'requestUnit',
    title: purchaseRequestItemPi.label('requestUnit'),
    width: 140,
  },
  {
    key: 'requestQuantity',
    title: purchaseRequestItemPi.label('requestQuantity'),
    width: 140,
  },
  {
    key: 'convertedQuantity',
    title: purchaseRequestItemPi.label('convertedQuantity'),
    width: 140,
  },
  {
    key: 'purchasePerUnit',
    title: purchaseRequestItemPi.label('purchasePerUnit'),
    width: 140,
  },
  {
    key: 'purchaseRequestUnitPrice',
    title: purchaseRequestItemPi.label('purchaseRequestUnitPrice'),
    width: 140,
  },
  {
    key: 'taxIncludedAmount',
    title: purchaseRequestItemPi.label('taxIncludedAmount'),
    width: 140,
  },
  {
    key: 'untaxedAmount',
    title: purchaseRequestItemPi.label('untaxedAmount'),
    width: 140,
  },
  {
    key: 'taxAmount',
    title: purchaseRequestItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'requestAmount',
    title: purchaseRequestItemPi.label('requestAmount'),
    width: 140,
  },
  {
    key: 'pricingDate',
    title: purchaseRequestItemPi.label('pricingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'grossWeight',
    title: purchaseRequestItemPi.label('grossWeight'),
    width: 140,
  },
  {
    key: 'netWeight',
    title: purchaseRequestItemPi.label('netWeight'),
    width: 140,
  },
  {
    key: 'weightUnit',
    title: purchaseRequestItemPi.label('weightUnit'),
    width: 140,
  },
  {
    key: 'volume',
    title: purchaseRequestItemPi.label('volume'),
    width: 140,
  },
  {
    key: 'volumeUnit',
    title: purchaseRequestItemPi.label('volumeUnit'),
    width: 140,
  },
  {
    key: 'profitCenterCode',
    title: purchaseRequestItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: purchaseRequestItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseRequestCreate & { purchaseRequestId?: string }> | null | undefined) {
  const rows_purchaseRequestItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childPurchaseRequestItemRows.value = rows_purchaseRequestItem
}

function createDefaultPurchaseRequestItemRow(): Record<string, unknown> {
  return {
    purchasePlanItemId: '',
    lineNumber: allocateNextPurchaseRequestItemLineNumber(),
    allocationCategory: '',
    materialCode: '',
    requestUnit: '',
    requestQuantity: 0,
    convertedQuantity: 0,
    purchasePerUnit: 0,
    purchaseRequestUnitPrice: 0,
    taxIncludedAmount: 0,
    untaxedAmount: 0,
    taxAmount: 0,
    requestAmount: 0,
    pricingDate: '',
    grossWeight: 0,
    netWeight: 0,
    weightUnit: '',
    volume: 0,
    volumeUnit: '',
    profitCenterCode: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseRequestId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: purchaseRequestItemTableRef.value?.getRows?.() ?? childPurchaseRequestItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        purchaseRequestId: isUpdate ? masterId : 0,
      }
      if (isUpdate && isPersistedPurchaseRequestItemRow(row)) {
        normalized.purchaseRequestItemId = row.purchaseRequestItemId
      } else {
        delete normalized.purchaseRequestItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseRequestCreate & { purchaseRequestId?: string }> | null
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
  chainScheme: 1,
  purchaseRequestType: "F-NB",
  pricingProcedure: "ZRM001",
  currencyCode: "CNY",
  taxCode: "J2",
  requestStatus: 0,
  convertedStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseRequestId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseRequestId) {
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
    if (!props.formData?.purchaseRequestId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.purchaseRequestId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'purchaseRequestCode',
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
  purchaseRequestCode: [{
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
  chainScheme: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('chainScheme'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('chainScheme'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestDate: [
    {
      required: true,
      message: pi.ph('requestDate'),
      trigger: 'change'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
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
  convertedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requestStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requestStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseRequestItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('chainScheme' in payload) {
    const rawchainScheme = payload.chainScheme
    if (rawchainScheme === undefined || rawchainScheme === null || rawchainScheme === '') {
      delete payload.chainScheme
    } else {
      const numchainScheme = typeof rawchainScheme === 'number' ? rawchainScheme : Number(rawchainScheme)
      if (Number.isFinite(numchainScheme)) payload.chainScheme = numchainScheme
      else delete payload.chainScheme
    }
  }
  if ('poDecision' in payload) {
    const rawpoDecision = payload.poDecision
    if (rawpoDecision === undefined || rawpoDecision === null || rawpoDecision === '') {
      delete payload.poDecision
    } else {
      const numpoDecision = typeof rawpoDecision === 'number' ? rawpoDecision : Number(rawpoDecision)
      if (Number.isFinite(numpoDecision)) payload.poDecision = numpoDecision
      else delete payload.poDecision
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
  if ('convertedQuantity' in payload) {
    const rawconvertedQuantity = payload.convertedQuantity
    if (rawconvertedQuantity === undefined || rawconvertedQuantity === null || rawconvertedQuantity === '') {
      delete payload.convertedQuantity
    } else {
      const numconvertedQuantity = typeof rawconvertedQuantity === 'number' ? rawconvertedQuantity : Number(rawconvertedQuantity)
      if (Number.isFinite(numconvertedQuantity)) payload.convertedQuantity = numconvertedQuantity
      else delete payload.convertedQuantity
    }
  }
  if ('convertedAmount' in payload) {
    const rawconvertedAmount = payload.convertedAmount
    if (rawconvertedAmount === undefined || rawconvertedAmount === null || rawconvertedAmount === '') {
      delete payload.convertedAmount
    } else {
      const numconvertedAmount = typeof rawconvertedAmount === 'number' ? rawconvertedAmount : Number(rawconvertedAmount)
      if (Number.isFinite(numconvertedAmount)) payload.convertedAmount = numconvertedAmount
      else delete payload.convertedAmount
    }
  }
  if ('requestStatus' in payload) {
    const rawrequestStatus = payload.requestStatus
    if (rawrequestStatus === undefined || rawrequestStatus === null || rawrequestStatus === '') {
      delete payload.requestStatus
    } else {
      const numrequestStatus = typeof rawrequestStatus === 'number' ? rawrequestStatus : Number(rawrequestStatus)
      if (Number.isFinite(numrequestStatus)) payload.requestStatus = numrequestStatus
      else delete payload.requestStatus
    }
  }
  if ('convertedStatus' in payload) {
    const rawconvertedStatus = payload.convertedStatus
    if (rawconvertedStatus === undefined || rawconvertedStatus === null || rawconvertedStatus === '') {
      delete payload.convertedStatus
    } else {
      const numconvertedStatus = typeof rawconvertedStatus === 'number' ? rawconvertedStatus : Number(rawconvertedStatus)
      if (Number.isFinite(numconvertedStatus)) payload.convertedStatus = numconvertedStatus
      else delete payload.convertedStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.purchaseRequestId) {
    payload.purchaseRequestId = props.formData.purchaseRequestId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseRequestId)
  childPurchaseRequestItemRows.value = []
  purchaseRequestItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.purchase-request-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.purchase-request-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.purchase-request-form__master :deep(.purchase-request-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.purchase-request-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.purchase-request-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.purchase-request-form__master :deep(.ant-tabs-content),
.purchase-request-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
