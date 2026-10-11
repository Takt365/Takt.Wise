<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-invoice/components -->
<!-- 文件名称：PurchaseInvoiceForm.vue -->
<!-- 功能描述：Takt采购发票主表实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-invoice-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="purchase-invoice-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-invoice-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="pi.label('purchaseInvoiceCode')"
                name="purchaseInvoiceCode"
              >
                <a-input
                  v-model:value="formState.purchaseInvoiceCode"
                  :placeholder="pi.ph('purchaseInvoiceCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiscalYear')"
                name="fiscalYear"
              >
                <a-input
                  v-model:value="formState.fiscalYear"
                  :placeholder="pi.ph('fiscalYear')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentType')"
                name="documentType"
              >
                <TaktSelect
                  v-model:value="formState.documentType"
                  dict-type="logistics_purchase_invoice_document_type"
                  :placeholder="pi.ph('documentType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentDate')"
                name="documentDate"
              >
                <a-date-picker
                  v-model:value="formState.documentDate"
                  :placeholder="pi.ph('documentDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postingDate')"
                name="postingDate"
              >
                <a-date-picker
                  v-model:value="formState.postingDate"
                  :placeholder="pi.ph('postingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('transactionEventType')"
                name="transactionEventType"
              >
                <TaktSelect
                  v-model:value="formState.transactionEventType"
                  dict-type="logistics_purchase_invoice_transaction_event_type"
                  :placeholder="pi.ph('transactionEventType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceCode')"
                name="referenceCode"
              >
                <a-input
                  v-model:value="formState.referenceCode"
                  :placeholder="pi.ph('referenceCode')"
                  show-count
                  :maxlength="16"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceId"
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
                  :disabled="!!formData?.purchaseInvoiceId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
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
                  :disabled="!!formData?.purchaseInvoiceId"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossAmount')"
                name="grossAmount"
              >
                <a-input-number
                  v-model:value="formState.grossAmount"
                  :placeholder="pi.ph('grossAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vatAmount')"
                name="vatAmount"
              >
                <a-input-number
                  v-model:value="formState.vatAmount"
                  :placeholder="pi.ph('vatAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxJurisdictionCode')"
                name="taxJurisdictionCode"
              >
                <a-input
                  v-model:value="formState.taxJurisdictionCode"
                  :placeholder="pi.ph('taxJurisdictionCode')"
                  show-count
                  :maxlength="15"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cashDiscountDays1')"
                name="cashDiscountDays1"
              >
                <a-input-number
                  v-model:value="formState.cashDiscountDays1"
                  :placeholder="pi.ph('cashDiscountDays1')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('invoiceFlag')"
                name="invoiceFlag"
              >
                <a-input
                  v-model:value="formState.invoiceFlag"
                  :placeholder="pi.ph('invoiceFlag')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('headerText')"
                name="headerText"
              >
                <a-input
                  v-model:value="formState.headerText"
                  :placeholder="pi.ph('headerText')"
                  show-count
                  :maxlength="25"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reversalDocumentCode')"
                name="reversalDocumentCode"
              >
                <a-input
                  v-model:value="formState.reversalDocumentCode"
                  :placeholder="pi.ph('reversalDocumentCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reversalFiscalYear')"
                name="reversalFiscalYear"
              >
                <a-input
                  v-model:value="formState.reversalFiscalYear"
                  :placeholder="pi.ph('reversalFiscalYear')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxCode')"
                name="taxCode"
              >
                <a-input
                  v-model:value="formState.taxCode"
                  :placeholder="pi.ph('taxCode')"
                  show-count
                  :maxlength="2"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('supplyingCountry')"
                name="supplyingCountry"
              >
                <TaktSelect
                  v-model:value="formState.supplyingCountry"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('supplyingCountry')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxExchangeRate')"
                name="taxExchangeRate"
              >
                <a-input-number
                  v-model:value="formState.taxExchangeRate"
                  :placeholder="pi.ph('taxExchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('baselineDate')"
                name="baselineDate"
              >
                <a-date-picker
                  v-model:value="formState.baselineDate"
                  :placeholder="pi.ph('baselineDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('exchangeRateDate')"
                name="exchangeRateDate"
              >
                <a-date-picker
                  v-model:value="formState.exchangeRateDate"
                  :placeholder="pi.ph('exchangeRateDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('transactionCode')"
                name="transactionCode"
              >
                <a-input
                  v-model:value="formState.transactionCode"
                  :placeholder="pi.ph('transactionCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('postedBy')"
                name="postedBy"
              >
                <a-input
                  v-model:value="formState.postedBy"
                  :placeholder="pi.ph('postedBy')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
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
      class="purchase-invoice-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="purchaseInvoiceItemTableRef"
      v-model="childPurchaseInvoiceItemRows"
      :columns="purchaseInvoiceItemFormColumns"
      :title="purchaseInvoiceItemPi.self()"
      :add-button-entity="purchaseInvoiceItemPi.self()"
      id-field="purchaseInvoiceItemId"
      :default-row="createDefaultPurchaseInvoiceItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-purchaseOrderCode="{ record }">
        <TaktSelect
          v-model:value="record.purchaseOrderCode"
          api-url="TaktPurchaseOrders/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseInvoiceItemPi.queryPh('purchaseOrderCode', 'select')"
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
          :placeholder="purchaseInvoiceItemPi.queryPh('materialCode', 'select')"
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
          :placeholder="purchaseInvoiceItemPi.ph('isObsolete')"
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
 * Takt采购发票主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-invoice/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseInvoiceI18n } from '../composables/usePurchaseInvoiceI18n'

/** 实体字段 i18n */
const pi = usePurchaseInvoiceI18n()

import type { PurchaseInvoiceCreate } from '@/types/logistics/procurement/purchaseInvoice.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseInvoiceCode","fiscalYear","documentType","documentDate","postingDate","transactionEventType","referenceCode","supplierCode","currencyCode","exchangeRate","grossAmount","vatAmount","taxJurisdictionCode","cashDiscountDays1","invoiceFlag","headerText","reversalDocumentCode","reversalFiscalYear","taxCode","supplyingCountry","taxExchangeRate","baselineDate","exchangeRateDate","transactionCode","postedBy","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { usePurchaseInvoiceItemI18n } from '../composables/usePurchaseInvoiceItemI18n'

const purchaseInvoiceItemPi = usePurchaseInvoiceItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPurchaseInvoiceItemRows = ref<Record<string, unknown>[]>([])
const purchaseInvoiceItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPurchaseInvoiceItemRow(row: Record<string, unknown>): boolean {
  const id = row.purchaseInvoiceItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPurchaseInvoiceItemLineNumber(): number {
  const rows = purchaseInvoiceItemTableRef.value?.getRows?.() ?? childPurchaseInvoiceItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 purchaseInvoiceItem 可编辑列 */
const purchaseInvoiceItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: purchaseInvoiceItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'purchaseOrderCode',
    title: purchaseInvoiceItemPi.label('purchaseOrderCode'),
    width: 140,
  },
  {
    key: 'purchaseOrderItem',
    title: purchaseInvoiceItemPi.label('purchaseOrderItem'),
    width: 140,
  },
  {
    key: 'accountAssignmentSeq',
    title: purchaseInvoiceItemPi.label('accountAssignmentSeq'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('accountAssignmentSeq'),
  },
  {
    key: 'materialCode',
    title: purchaseInvoiceItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'valuationArea',
    title: purchaseInvoiceItemPi.label('valuationArea'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('valuationArea'),
  },
  {
    key: 'amount',
    title: purchaseInvoiceItemPi.label('amount'),
    width: 140,
  },
  {
    key: 'debitCreditIndicator',
    title: purchaseInvoiceItemPi.label('debitCreditIndicator'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('debitCreditIndicator'),
  },
  {
    key: 'taxCode',
    title: purchaseInvoiceItemPi.label('taxCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('taxCode'),
  },
  {
    key: 'quantity',
    title: purchaseInvoiceItemPi.label('quantity'),
    width: 140,
  },
  {
    key: 'orderUnit',
    title: purchaseInvoiceItemPi.label('orderUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('orderUnit'),
  },
  {
    key: 'poPriceQuantity',
    title: purchaseInvoiceItemPi.label('poPriceQuantity'),
    width: 140,
  },
  {
    key: 'poPriceUnit',
    title: purchaseInvoiceItemPi.label('poPriceUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('poPriceUnit'),
  },
  {
    key: 'valuatedStockQuantity',
    title: purchaseInvoiceItemPi.label('valuatedStockQuantity'),
    width: 140,
  },
  {
    key: 'previousPeriodStock',
    title: purchaseInvoiceItemPi.label('previousPeriodStock'),
    width: 140,
  },
  {
    key: 'baseUnit',
    title: purchaseInvoiceItemPi.label('baseUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('baseUnit'),
  },
  {
    key: 'valuationClass',
    title: purchaseInvoiceItemPi.label('valuationClass'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('valuationClass'),
  },
  {
    key: 'updatePoHistoryFlag',
    title: purchaseInvoiceItemPi.label('updatePoHistoryFlag'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'subsequentDebitCredit',
    title: purchaseInvoiceItemPi.label('subsequentDebitCredit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('subsequentDebitCredit'),
  },
  {
    key: 'blockReasonPrice',
    title: purchaseInvoiceItemPi.label('blockReasonPrice'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonPrice'),
  },
  {
    key: 'blockReasonQuantity',
    title: purchaseInvoiceItemPi.label('blockReasonQuantity'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonQuantity'),
  },
  {
    key: 'blockReasonQuality',
    title: purchaseInvoiceItemPi.label('blockReasonQuality'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonQuality'),
  },
  {
    key: 'blockReasonEnhanced',
    title: purchaseInvoiceItemPi.label('blockReasonEnhanced'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonEnhanced'),
  },
  {
    key: 'valueString',
    title: purchaseInvoiceItemPi.label('valueString'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('valueString'),
  },
  {
    key: 'referenceCode',
    title: purchaseInvoiceItemPi.label('referenceCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('referenceCode'),
  },
  {
    key: 'conditionType',
    title: purchaseInvoiceItemPi.label('conditionType'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('conditionType'),
  },
  {
    key: 'totalValuatedStockValue',
    title: purchaseInvoiceItemPi.label('totalValuatedStockValue'),
    width: 140,
  },
  {
    key: 'previousPeriodValue',
    title: purchaseInvoiceItemPi.label('previousPeriodValue'),
    width: 140,
  },
  {
    key: 'referenceDocumentCode',
    title: purchaseInvoiceItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'referenceDocumentYear',
    title: purchaseInvoiceItemPi.label('referenceDocumentYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('referenceDocumentYear'),
  },
  {
    key: 'referenceDocumentItem',
    title: purchaseInvoiceItemPi.label('referenceDocumentItem'),
    width: 140,
  },
  {
    key: 'stockManagedMaterialCode',
    title: purchaseInvoiceItemPi.label('stockManagedMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('stockManagedMaterialCode'),
  },
  {
    key: 'itemText',
    title: purchaseInvoiceItemPi.label('itemText'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('itemText'),
  },
  {
    key: 'materialDocumentItem',
    title: purchaseInvoiceItemPi.label('materialDocumentItem'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: purchaseInvoiceItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseInvoiceCreate & { purchaseInvoiceId?: string }> | null | undefined) {
  const rows_purchaseInvoiceItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childPurchaseInvoiceItemRows.value = rows_purchaseInvoiceItem
}

function createDefaultPurchaseInvoiceItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextPurchaseInvoiceItemLineNumber(),
    purchaseOrderCode: '',
    purchaseOrderItem: 0,
    accountAssignmentSeq: '',
    materialCode: '',
    valuationArea: '',
    amount: 0,
    debitCreditIndicator: '',
    taxCode: '',
    quantity: 0,
    orderUnit: '',
    poPriceQuantity: 0,
    poPriceUnit: '',
    valuatedStockQuantity: 0,
    previousPeriodStock: 0,
    baseUnit: '',
    valuationClass: '',
    updatePoHistoryFlag: '',
    subsequentDebitCredit: '',
    blockReasonPrice: '',
    blockReasonQuantity: '',
    blockReasonQuality: '',
    blockReasonEnhanced: '',
    valueString: '',
    referenceCode: '',
    conditionType: '',
    totalValuatedStockValue: 0,
    previousPeriodValue: 0,
    referenceDocumentCode: '',
    referenceDocumentYear: '',
    referenceDocumentItem: 0,
    stockManagedMaterialCode: '',
    itemText: '',
    materialDocumentItem: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseInvoiceId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: purchaseInvoiceItemTableRef.value?.getRows?.() ?? childPurchaseInvoiceItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        purchaseInvoiceId: isUpdate ? masterId : 0,
      }
      if (isUpdate && isPersistedPurchaseInvoiceItemRow(row)) {
        normalized.purchaseInvoiceItemId = row.purchaseInvoiceItemId
      } else {
        delete normalized.purchaseInvoiceItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseInvoiceCreate & { purchaseInvoiceId?: string }> | null
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
  currencyCode: "CNY",
  supplyingCountry: "CN"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseInvoiceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseInvoiceId) {
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
    if (!props.formData?.purchaseInvoiceId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  purchaseInvoiceCode: [
    {
      required: true,
      message: pi.ph('purchaseInvoiceCode'),
      trigger: 'blur'
    }
  ],
  fiscalYear: [
    {
      required: true,
      message: pi.ph('fiscalYear'),
      trigger: 'blur'
    }
  ],
  documentDate: [
    {
      required: true,
      message: pi.ph('documentDate'),
      trigger: 'change'
    }
  ],
  postingDate: [
    {
      required: true,
      message: pi.ph('postingDate'),
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
  grossAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('grossAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('grossAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseInvoiceItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
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
  if ('grossAmount' in payload) {
    const rawgrossAmount = payload.grossAmount
    if (rawgrossAmount === undefined || rawgrossAmount === null || rawgrossAmount === '') {
      delete payload.grossAmount
    } else {
      const numgrossAmount = typeof rawgrossAmount === 'number' ? rawgrossAmount : Number(rawgrossAmount)
      if (Number.isFinite(numgrossAmount)) payload.grossAmount = numgrossAmount
      else delete payload.grossAmount
    }
  }
  if ('vatAmount' in payload) {
    const rawvatAmount = payload.vatAmount
    if (rawvatAmount === undefined || rawvatAmount === null || rawvatAmount === '') {
      delete payload.vatAmount
    } else {
      const numvatAmount = typeof rawvatAmount === 'number' ? rawvatAmount : Number(rawvatAmount)
      if (Number.isFinite(numvatAmount)) payload.vatAmount = numvatAmount
      else delete payload.vatAmount
    }
  }
  if ('cashDiscountDays1' in payload) {
    const rawcashDiscountDays1 = payload.cashDiscountDays1
    if (rawcashDiscountDays1 === undefined || rawcashDiscountDays1 === null || rawcashDiscountDays1 === '') {
      delete payload.cashDiscountDays1
    } else {
      const numcashDiscountDays1 = typeof rawcashDiscountDays1 === 'number' ? rawcashDiscountDays1 : Number(rawcashDiscountDays1)
      if (Number.isFinite(numcashDiscountDays1)) payload.cashDiscountDays1 = numcashDiscountDays1
      else delete payload.cashDiscountDays1
    }
  }
  if ('taxExchangeRate' in payload) {
    const rawtaxExchangeRate = payload.taxExchangeRate
    if (rawtaxExchangeRate === undefined || rawtaxExchangeRate === null || rawtaxExchangeRate === '') {
      delete payload.taxExchangeRate
    } else {
      const numtaxExchangeRate = typeof rawtaxExchangeRate === 'number' ? rawtaxExchangeRate : Number(rawtaxExchangeRate)
      if (Number.isFinite(numtaxExchangeRate)) payload.taxExchangeRate = numtaxExchangeRate
      else delete payload.taxExchangeRate
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.purchaseInvoiceId) {
    payload.purchaseInvoiceId = props.formData.purchaseInvoiceId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseInvoiceId)
  childPurchaseInvoiceItemRows.value = []
  purchaseInvoiceItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.purchase-invoice-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.purchase-invoice-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.purchase-invoice-form__master :deep(.purchase-invoice-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.purchase-invoice-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.purchase-invoice-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.purchase-invoice-form__master :deep(.ant-tabs-content),
.purchase-invoice-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
