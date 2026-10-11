<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：MaterialDocumentForm.vue -->
<!-- 功能描述：Takt物料凭证主表实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-document-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="material-document-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="material-document-form-tabs"
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
                :label="pi.label('materialDocumentCode')"
                name="materialDocumentCode"
              >
                <a-input
                  v-model:value="formState.materialDocumentCode"
                  :placeholder="pi.ph('materialDocumentCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialDocumentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialDocumentYear')"
                name="materialDocumentYear"
              >
                <a-input
                  v-model:value="formState.materialDocumentYear"
                  :placeholder="pi.ph('materialDocumentYear')"
                  show-count
                  :maxlength="4"
                  allow-clear
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
                  dict-type="logistics_material_document_transaction_event_type"
                  :placeholder="pi.ph('transactionEventType')"
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
                  dict-type="logistics_material_document_type"
                  :placeholder="pi.ph('documentType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revaluationType')"
                name="revaluationType"
              >
                <a-input
                  v-model:value="formState.revaluationType"
                  :placeholder="pi.ph('revaluationType')"
                  show-count
                  :maxlength="2"
                  allow-clear
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
                :label="pi.label('referenceCode')"
                name="referenceCode"
              >
                <a-input
                  v-model:value="formState.referenceCode"
                  :placeholder="pi.ph('referenceCode')"
                  show-count
                  :maxlength="16"
                  allow-clear
                  :disabled="!!formData?.materialDocumentId"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('billOfLadingCode')"
                name="billOfLadingCode"
              >
                <a-input
                  v-model:value="formState.billOfLadingCode"
                  :placeholder="pi.ph('billOfLadingCode')"
                  show-count
                  :maxlength="16"
                  allow-clear
                  :disabled="!!formData?.materialDocumentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deliveryCode')"
                name="deliveryCode"
              >
                <a-input
                  v-model:value="formState.deliveryCode"
                  :placeholder="pi.ph('deliveryCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialDocumentId"
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
                  :disabled="!!formData?.materialDocumentId"
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
      class="material-document-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="materialDocumentItemTableRef"
      v-model="childMaterialDocumentItemRows"
      :columns="materialDocumentItemFormColumns"
      :title="materialDocumentItemPi.self()"
      :add-button-entity="materialDocumentItemPi.self()"
      id-field="materialDocumentItemId"
      :default-row="createDefaultMaterialDocumentItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-movementType="{ record }">
        <TaktSelect
          v-model:value="record.movementType"
          dict-type="logistics_materials_movement_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('movementType')"
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
          :placeholder="materialDocumentItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-warehouseCode="{ record }">
        <TaktSelect
          v-model:value="record.warehouseCode"
          api-url="TaktWarehouses/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('warehouseCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-stockType="{ record }">
        <TaktSelect
          v-model:value="record.stockType"
          dict-type="logistics_stock_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('stockType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-specialStock="{ record }">
        <TaktSelect
          v-model:value="record.specialStock"
          dict-type="logistics_materials_special_stock_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('specialStock')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-supplierCode="{ record }">
        <TaktSelect
          v-model:value="record.supplierCode"
          api-url="TaktSuppliers/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('supplierCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-customerCode="{ record }">
        <TaktSelect
          v-model:value="record.customerCode"
          api-url="TaktCustomers/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('customerCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-currencyCode="{ record }">
        <TaktSelect
          v-model:value="record.currencyCode"
          dict-type="accounting_financial_currency_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('currencyCode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-baseUnit="{ record }">
        <TaktSelect
          v-model:value="record.baseUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('baseUnit')"
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
          :placeholder="materialDocumentItemPi.queryPh('profitCenterCode', 'select')"
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
          :placeholder="materialDocumentItemPi.ph('isObsolete')"
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
 * Takt物料凭证主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-document/components
 */

import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialDocumentI18n } from '../composables/useMaterialDocumentI18n'

/** 实体字段 i18n */
const pi = useMaterialDocumentI18n()

import type { MaterialDocumentCreate } from '@/types/logistics/materials/materialDocument.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","materialDocumentCode","materialDocumentYear","transactionEventType","documentType","revaluationType","documentDate","postingDate","referenceCode","headerText","billOfLadingCode","deliveryCode","transactionCode","postedBy","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useMaterialDocumentItemI18n } from '../composables/useMaterialDocumentItemI18n'

const materialDocumentItemPi = useMaterialDocumentItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaterialDocumentItemRows = ref<Record<string, unknown>[]>([])
const materialDocumentItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaterialDocumentItemRow(row: Record<string, unknown>): boolean {
  const id = row.materialDocumentItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaterialDocumentItemLineNumber(): number {
  const rows = materialDocumentItemTableRef.value?.getRows?.() ?? childMaterialDocumentItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 materialDocumentItem 可编辑列 */
const materialDocumentItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: materialDocumentItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'lineId',
    title: materialDocumentItemPi.label('lineId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('lineId'),
  },
  {
    key: 'parentLineId',
    title: materialDocumentItemPi.label('parentLineId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('parentLineId'),
  },
  {
    key: 'lineDepth',
    title: materialDocumentItemPi.label('lineDepth'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('lineDepth'),
  },
  {
    key: 'movementType',
    title: materialDocumentItemPi.label('movementType'),
    width: 140,
  },
  {
    key: 'autoCreatedFlag',
    title: materialDocumentItemPi.label('autoCreatedFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('autoCreatedFlag'),
  },
  {
    key: 'materialCode',
    title: materialDocumentItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'warehouseCode',
    title: materialDocumentItemPi.label('warehouseCode'),
    width: 140,
  },
  {
    key: 'batchCode',
    title: materialDocumentItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('batchCode'),
  },
  {
    key: 'stockType',
    title: materialDocumentItemPi.label('stockType'),
    width: 140,
  },
  {
    key: 'restrictedStockFlag',
    title: materialDocumentItemPi.label('restrictedStockFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('restrictedStockFlag'),
  },
  {
    key: 'specialStock',
    title: materialDocumentItemPi.label('specialStock'),
    width: 140,
  },
  {
    key: 'supplierCode',
    title: materialDocumentItemPi.label('supplierCode'),
    width: 140,
  },
  {
    key: 'customerCode',
    title: materialDocumentItemPi.label('customerCode'),
    width: 140,
  },
  {
    key: 'debitCreditIndicator',
    title: materialDocumentItemPi.label('debitCreditIndicator'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('debitCreditIndicator'),
  },
  {
    key: 'currencyCode',
    title: materialDocumentItemPi.label('currencyCode'),
    width: 140,
  },
  {
    key: 'localCurrencyAmount',
    title: materialDocumentItemPi.label('localCurrencyAmount'),
    width: 140,
  },
  {
    key: 'alternativeAmount',
    title: materialDocumentItemPi.label('alternativeAmount'),
    width: 140,
  },
  {
    key: 'quantity',
    title: materialDocumentItemPi.label('quantity'),
    width: 140,
  },
  {
    key: 'baseUnit',
    title: materialDocumentItemPi.label('baseUnit'),
    width: 140,
  },
  {
    key: 'entryQuantity',
    title: materialDocumentItemPi.label('entryQuantity'),
    width: 140,
  },
  {
    key: 'entryUnit',
    title: materialDocumentItemPi.label('entryUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('entryUnit'),
  },
  {
    key: 'poPriceQuantity',
    title: materialDocumentItemPi.label('poPriceQuantity'),
    width: 140,
  },
  {
    key: 'poPriceUnit',
    title: materialDocumentItemPi.label('poPriceUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('poPriceUnit'),
  },
  {
    key: 'purchaseOrderCode',
    title: materialDocumentItemPi.label('purchaseOrderCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('purchaseOrderCode'),
  },
  {
    key: 'purchaseOrderItem',
    title: materialDocumentItemPi.label('purchaseOrderItem'),
    width: 140,
  },
  {
    key: 'referenceDocumentYear',
    title: materialDocumentItemPi.label('referenceDocumentYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('referenceDocumentYear'),
  },
  {
    key: 'referenceDocumentCode',
    title: materialDocumentItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'referenceDocumentItem',
    title: materialDocumentItemPi.label('referenceDocumentItem'),
    width: 140,
  },
  {
    key: 'originalMaterialDocumentYear',
    title: materialDocumentItemPi.label('originalMaterialDocumentYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('originalMaterialDocumentYear'),
  },
  {
    key: 'originalMaterialDocumentCode',
    title: materialDocumentItemPi.label('originalMaterialDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('originalMaterialDocumentCode'),
  },
  {
    key: 'originalLineNumber',
    title: materialDocumentItemPi.label('originalLineNumber'),
    width: 140,
  },
  {
    key: 'deliveryCompletedFlag',
    title: materialDocumentItemPi.label('deliveryCompletedFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('deliveryCompletedFlag'),
  },
  {
    key: 'itemText',
    title: materialDocumentItemPi.label('itemText'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('itemText'),
  },
  {
    key: 'equipmentCode',
    title: materialDocumentItemPi.label('equipmentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('equipmentCode'),
  },
  {
    key: 'goodsRecipient',
    title: materialDocumentItemPi.label('goodsRecipient'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('goodsRecipient'),
  },
  {
    key: 'unloadingPoint',
    title: materialDocumentItemPi.label('unloadingPoint'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('unloadingPoint'),
  },
  {
    key: 'businessAreaCode',
    title: materialDocumentItemPi.label('businessAreaCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('businessAreaCode'),
  },
  {
    key: 'controllingAreaCode',
    title: materialDocumentItemPi.label('controllingAreaCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('controllingAreaCode'),
  },
  {
    key: 'tradingPartnerBusinessArea',
    title: materialDocumentItemPi.label('tradingPartnerBusinessArea'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('tradingPartnerBusinessArea'),
  },
  {
    key: 'productionOrderCode',
    title: materialDocumentItemPi.label('productionOrderCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('productionOrderCode'),
  },
  {
    key: 'assetCode',
    title: materialDocumentItemPi.label('assetCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('assetCode'),
  },
  {
    key: 'assetSubCode',
    title: materialDocumentItemPi.label('assetSubCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('assetSubCode'),
  },
  {
    key: 'fiscalYear',
    title: materialDocumentItemPi.label('fiscalYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('fiscalYear'),
  },
  {
    key: 'postToPreviousPeriodFlag',
    title: materialDocumentItemPi.label('postToPreviousPeriodFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('postToPreviousPeriodFlag'),
  },
  {
    key: 'postToPreviousYearFlag',
    title: materialDocumentItemPi.label('postToPreviousYearFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('postToPreviousYearFlag'),
  },
  {
    key: 'accountingDocumentCode',
    title: materialDocumentItemPi.label('accountingDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('accountingDocumentCode'),
  },
  {
    key: 'accountingDocumentItem',
    title: materialDocumentItemPi.label('accountingDocumentItem'),
    width: 140,
  },
  {
    key: 'revaluationDocumentCode',
    title: materialDocumentItemPi.label('revaluationDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('revaluationDocumentCode'),
  },
  {
    key: 'revaluationDocumentItem',
    title: materialDocumentItemPi.label('revaluationDocumentItem'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('revaluationDocumentItem'),
  },
  {
    key: 'reservationCode',
    title: materialDocumentItemPi.label('reservationCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('reservationCode'),
  },
  {
    key: 'reservationItem',
    title: materialDocumentItemPi.label('reservationItem'),
    width: 140,
  },
  {
    key: 'finalIssueFlag',
    title: materialDocumentItemPi.label('finalIssueFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('finalIssueFlag'),
  },
  {
    key: 'reservationQuantity',
    title: materialDocumentItemPi.label('reservationQuantity'),
    width: 140,
  },
  {
    key: 'receivingMaterialCode',
    title: materialDocumentItemPi.label('receivingMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('receivingMaterialCode'),
  },
  {
    key: 'receivingPlantCode',
    title: materialDocumentItemPi.label('receivingPlantCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('receivingPlantCode'),
  },
  {
    key: 'receivingWarehouseCode',
    title: materialDocumentItemPi.label('receivingWarehouseCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('receivingWarehouseCode'),
  },
  {
    key: 'profitCenterCode',
    title: materialDocumentItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'valuatedStockQuantity',
    title: materialDocumentItemPi.label('valuatedStockQuantity'),
    width: 140,
  },
  {
    key: 'totalValuatedStockValue',
    title: materialDocumentItemPi.label('totalValuatedStockValue'),
    width: 140,
  },
  {
    key: 'priceControl',
    title: materialDocumentItemPi.label('priceControl'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('priceControl'),
  },
  {
    key: 'manufacturerPartMaterialCode',
    title: materialDocumentItemPi.label('manufacturerPartMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('manufacturerPartMaterialCode'),
  },
  {
    key: 'mkpfReferenceCode',
    title: materialDocumentItemPi.label('mkpfReferenceCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('mkpfReferenceCode'),
  },
  {
    key: 'imDeliveryCode',
    title: materialDocumentItemPi.label('imDeliveryCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('imDeliveryCode'),
  },
  {
    key: 'imDeliveryItem',
    title: materialDocumentItemPi.label('imDeliveryItem'),
    width: 140,
  },
  {
    key: 'postedBy',
    title: materialDocumentItemPi.label('postedBy'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('postedBy'),
  },
  {
    key: 'isObsolete',
    title: materialDocumentItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialDocumentCreate & { materialDocumentId?: string }> | null | undefined) {
  const rows_materialDocumentItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childMaterialDocumentItemRows.value = rows_materialDocumentItem
}

function createDefaultMaterialDocumentItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextMaterialDocumentItemLineNumber(),
    lineId: '',
    parentLineId: '',
    lineDepth: '',
    movementType: '',
    autoCreatedFlag: '',
    materialCode: '',
    warehouseCode: '',
    batchCode: '',
    stockType: '',
    restrictedStockFlag: '',
    specialStock: '',
    supplierCode: '',
    customerCode: '',
    debitCreditIndicator: '',
    currencyCode: '',
    localCurrencyAmount: 0,
    alternativeAmount: 0,
    quantity: 0,
    baseUnit: '',
    entryQuantity: 0,
    entryUnit: '',
    poPriceQuantity: 0,
    poPriceUnit: '',
    purchaseOrderCode: '',
    purchaseOrderItem: 0,
    referenceDocumentYear: '',
    referenceDocumentCode: '',
    referenceDocumentItem: 0,
    originalMaterialDocumentYear: '',
    originalMaterialDocumentCode: '',
    originalLineNumber: 0,
    deliveryCompletedFlag: '',
    itemText: '',
    equipmentCode: '',
    goodsRecipient: '',
    unloadingPoint: '',
    businessAreaCode: '',
    controllingAreaCode: '',
    tradingPartnerBusinessArea: '',
    productionOrderCode: '',
    assetCode: '',
    assetSubCode: '',
    fiscalYear: '',
    postToPreviousPeriodFlag: '',
    postToPreviousYearFlag: '',
    accountingDocumentCode: '',
    accountingDocumentItem: 0,
    revaluationDocumentCode: '',
    revaluationDocumentItem: '',
    reservationCode: '',
    reservationItem: 0,
    finalIssueFlag: '',
    reservationQuantity: 0,
    receivingMaterialCode: '',
    receivingPlantCode: '',
    receivingWarehouseCode: '',
    profitCenterCode: '',
    valuatedStockQuantity: 0,
    totalValuatedStockValue: 0,
    priceControl: '',
    manufacturerPartMaterialCode: '',
    mkpfReferenceCode: '',
    imDeliveryCode: '',
    imDeliveryItem: 0,
    postedBy: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialDocumentId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: materialDocumentItemTableRef.value?.getRows?.() ?? childMaterialDocumentItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        materialDocumentId: isUpdate ? masterId : 0,
      }
      if (isUpdate && isPersistedMaterialDocumentItemRow(row)) {
        normalized.materialDocumentItemId = row.materialDocumentItemId
      } else {
        delete normalized.materialDocumentItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialDocumentCreate & { materialDocumentId?: string }> | null
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

/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})



/** 编辑态灌入 formData；新增态恢复默认值（须含 materialDocumentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialDocumentId) {
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
    if (!props.formData?.materialDocumentId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialDocumentCode: [
    {
      required: true,
      message: pi.ph('materialDocumentCode'),
      trigger: 'blur'
    }
  ],
  materialDocumentYear: [
    {
      required: true,
      message: pi.ph('materialDocumentYear'),
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
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialDocumentItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.materialDocumentId) {
    payload.materialDocumentId = props.formData.materialDocumentId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialDocumentId)
  childMaterialDocumentItemRows.value = []
  materialDocumentItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.material-document-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.material-document-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.material-document-form__master :deep(.material-document-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.material-document-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.material-document-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.material-document-form__master :deep(.ant-tabs-content),
.material-document-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
