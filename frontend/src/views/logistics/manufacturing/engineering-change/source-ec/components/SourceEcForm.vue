<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/components -->
<!-- 文件名称：SourceEcForm.vue -->
<!-- 功能描述：设变来源明细列表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form source-ec-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <TaktFormTabs
      v-model:active-key="activeTab"
      class="source-ec-form-tabs"
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
                :label="pi.label('sourceEcCode')"
                name="sourceEcCode"
              >
                <a-input
                  v-model:value="formState.sourceEcCode"
                  :placeholder="pi.ph('sourceEcCode')"
                  show-count
                  :maxlength="6"
                  allow-clear
                  :disabled="!!formData?.sourceEcId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceModel')"
                name="sourceModel"
              >
                <a-input
                  v-model:value="formState.sourceModel"
                  :placeholder="pi.ph('sourceModel')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceTitle')"
                name="sourceTitle"
              >
                <a-input
                  v-model:value="formState.sourceTitle"
                  :placeholder="pi.ph('sourceTitle')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceStatus')"
                name="sourceStatus"
              >
                <a-input
                  v-model:value="formState.sourceStatus"
                  :placeholder="pi.ph('sourceStatus')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceIssueDate')"
                name="sourceIssueDate"
              >
                <a-date-picker
                  v-model:value="formState.sourceIssueDate"
                  :placeholder="pi.ph('sourceIssueDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceTcjOwner')"
                name="sourceTcjOwner"
              >
                <a-input
                  v-model:value="formState.sourceTcjOwner"
                  :placeholder="pi.ph('sourceTcjOwner')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceTcjDependency')"
                name="sourceTcjDependency"
              >
                <a-input
                  v-model:value="formState.sourceTcjDependency"
                  :placeholder="pi.ph('sourceTcjDependency')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceEcMeeting')"
                name="sourceEcMeeting"
              >
                <a-input
                  v-model:value="formState.sourceEcMeeting"
                  :placeholder="pi.ph('sourceEcMeeting')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                :label="pi.label('sourcePpCode')"
                name="sourcePpCode"
              >
                <a-input
                  v-model:value="formState.sourcePpCode"
                  :placeholder="pi.ph('sourcePpCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.sourceEcId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceTechnicalNoticeCode')"
                name="sourceTechnicalNoticeCode"
              >
                <a-input
                  v-model:value="formState.sourceTechnicalNoticeCode"
                  :placeholder="pi.ph('sourceTechnicalNoticeCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.sourceEcId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceImplementation')"
                name="sourceImplementation"
              >
                <a-input
                  v-model:value="formState.sourceImplementation"
                  :placeholder="pi.ph('sourceImplementation')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceMainChangeReason')"
                name="sourceMainChangeReason"
              >
                <a-input
                  v-model:value="formState.sourceMainChangeReason"
                  :placeholder="pi.ph('sourceMainChangeReason')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceSecondaryChangeReason')"
                name="sourceSecondaryChangeReason"
              >
                <a-input
                  v-model:value="formState.sourceSecondaryChangeReason"
                  :placeholder="pi.ph('sourceSecondaryChangeReason')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceSafetyRegulation')"
                name="sourceSafetyRegulation"
              >
                <a-input
                  v-model:value="formState.sourceSafetyRegulation"
                  :placeholder="pi.ph('sourceSafetyRegulation')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceProgressStatus')"
                name="sourceProgressStatus"
              >
                <a-input
                  v-model:value="formState.sourceProgressStatus"
                  :placeholder="pi.ph('sourceProgressStatus')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceSerialNumberControl')"
                name="sourceSerialNumberControl"
              >
                <a-input
                  v-model:value="formState.sourceSerialNumberControl"
                  :placeholder="pi.ph('sourceSerialNumberControl')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceCustomerApproval')"
                name="sourceCustomerApproval"
              >
                <a-input
                  v-model:value="formState.sourceCustomerApproval"
                  :placeholder="pi.ph('sourceCustomerApproval')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceServiceManualRevision')"
                name="sourceServiceManualRevision"
              >
                <a-input
                  v-model:value="formState.sourceServiceManualRevision"
                  :placeholder="pi.ph('sourceServiceManualRevision')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceUserManualRevision')"
                name="sourceUserManualRevision"
              >
                <a-input
                  v-model:value="formState.sourceUserManualRevision"
                  :placeholder="pi.ph('sourceUserManualRevision')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourcePromotionManualRevision')"
                name="sourcePromotionManualRevision"
              >
                <a-input
                  v-model:value="formState.sourcePromotionManualRevision"
                  :placeholder="pi.ph('sourcePromotionManualRevision')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceStandardDocumentRevision')"
                name="sourceStandardDocumentRevision"
              >
                <a-input
                  v-model:value="formState.sourceStandardDocumentRevision"
                  :placeholder="pi.ph('sourceStandardDocumentRevision')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceInformationRelease')"
                name="sourceInformationRelease"
              >
                <a-input
                  v-model:value="formState.sourceInformationRelease"
                  :placeholder="pi.ph('sourceInformationRelease')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceCostChange')"
                name="sourceCostChange"
              >
                <a-input
                  v-model:value="formState.sourceCostChange"
                  :placeholder="pi.ph('sourceCostChange')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceUnitCost')"
                name="sourceUnitCost"
              >
                <a-input-number
                  v-model:value="formState.sourceUnitCost"
                  :placeholder="pi.ph('sourceUnitCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceMoldModificationCost')"
                name="sourceMoldModificationCost"
              >
                <a-input-number
                  v-model:value="formState.sourceMoldModificationCost"
                  :placeholder="pi.ph('sourceMoldModificationCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceRelatedDrawing')"
                name="sourceRelatedDrawing"
              >
                <a-input
                  v-model:value="formState.sourceRelatedDrawing"
                  :placeholder="pi.ph('sourceRelatedDrawing')"
                  show-count
                  :maxlength="210"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sourceEcContent')"
                name="sourceEcContent"
              >
                <TaktRichEditor
                  v-model:value="formState.sourceEcContent"
                  :placeholder="pi.ph('sourceEcContent')"
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
    </TaktFormTabs>
    <!-- 下：子表 sourceEcDetails -->
    <TaktEditableTable
      ref="sourceEcDetailTableRef"
      v-model="childSourceEcDetailRows"
      :columns="sourceEcDetailFormColumns"
      :title="sourceEcDetailPi.self()"
      :add-button-entity="sourceEcDetailPi.self()"
      id-field="sourceEcDetailId"
      :default-row="createDefaultSourceEcDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-sourceCompatibility="{ record }">
        <a-input
          v-model:value="record.sourceCompatibility"
          class="w-full"
          :placeholder="sourceEcDetailPi.ph('sourceCompatibility')"
          :disabled="loading"
          :maxlength="4"
          allow-clear
        />
      </template>
      <template #cell-source2ndVendor="{ record }">
        <TaktSelect
          v-model:value="record.source2ndVendor"
          dict-type="logistics_manufacturing_ec_2nd_vendor"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sourceEcDetailPi.ph('source2ndVendor')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-sourceInstruction="{ record }">
        <TaktSelect
          v-model:value="record.sourceInstruction"
          dict-type="logistics_manufacturing_ec_source_instruction"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sourceEcDetailPi.ph('sourceInstruction')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-sourceOldPartDisposition="{ record }">
        <TaktSelect
          v-model:value="record.sourceOldPartDisposition"
          dict-type="logistics_manufacturing_ec_old_part_disposition"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sourceEcDetailPi.ph('sourceOldPartDisposition')"
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
          :placeholder="sourceEcDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变来源明细列表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/source-ec/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSourceEcI18n } from '../composables/useSourceEcI18n'

/** 实体字段 i18n */
const pi = useSourceEcI18n()

import type { SourceEcCreate } from '@/types/logistics/manufacturing/engineering-change/sourceEc.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","sourceEcCode","sourceModel","sourceTitle","sourceStatus","sourceIssueDate","sourceTcjOwner","sourceTcjDependency","sourceEcMeeting","sourcePpCode","sourceTechnicalNoticeCode","sourceImplementation","sourceMainChangeReason","sourceSecondaryChangeReason","sourceSafetyRegulation","sourceProgressStatus","sourceSerialNumberControl","sourceCustomerApproval","sourceServiceManualRevision","sourceUserManualRevision","sourcePromotionManualRevision","sourceStandardDocumentRevision","sourceInformationRelease","sourceCostChange","sourceUnitCost","sourceMoldModificationCost","sourceRelatedDrawing","sourceEcContent","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useSourceEcDetailI18n } from '../composables/useSourceEcDetailI18n'

const sourceEcDetailPi = useSourceEcDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSourceEcDetailRows = ref<Record<string, unknown>[]>([])
const sourceEcDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedSourceEcDetailRow(row: Record<string, unknown>): boolean {
  const id = row.sourceEcDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextSourceEcDetailLineNumber(): number {
  const rows = sourceEcDetailTableRef.value?.getRows?.() ?? childSourceEcDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 sourceEcDetail 可编辑列 */
const sourceEcDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: sourceEcDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'sourceRootMaterialCode',
    title: sourceEcDetailPi.label('sourceRootMaterialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'sourceParentMaterialCode',
    title: sourceEcDetailPi.label('sourceParentMaterialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'sourceOldMaterialCode',
    title: sourceEcDetailPi.label('sourceOldMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceOldMaterialCode'),
  },
  {
    key: 'sourceOldMaterialDescription',
    title: sourceEcDetailPi.label('sourceOldMaterialDescription'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceOldMaterialDescription'),
  },
  {
    key: 'sourceOldUsageQuantity',
    title: sourceEcDetailPi.label('sourceOldUsageQuantity'),
    width: 140,
  },
  {
    key: 'sourceOldItemPosition',
    title: sourceEcDetailPi.label('sourceOldItemPosition'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceOldItemPosition'),
  },
  {
    key: 'sourceNewMaterialCode',
    title: sourceEcDetailPi.label('sourceNewMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceNewMaterialCode'),
  },
  {
    key: 'sourceNewMaterialDescription',
    title: sourceEcDetailPi.label('sourceNewMaterialDescription'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceNewMaterialDescription'),
  },
  {
    key: 'sourceNewUsageQuantity',
    title: sourceEcDetailPi.label('sourceNewUsageQuantity'),
    width: 140,
  },
  {
    key: 'sourceNewItemPosition',
    title: sourceEcDetailPi.label('sourceNewItemPosition'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceNewItemPosition'),
  },
  {
    key: 'sourceBomCode',
    title: sourceEcDetailPi.label('sourceBomCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceBomCode'),
  },
  {
    key: 'sourceCompatibility',
    title: sourceEcDetailPi.label('sourceCompatibility'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sourceEcDetailPi.ph('sourceCompatibility'),
  },
  {
    key: 'source2ndVendor',
    title: sourceEcDetailPi.label('source2ndVendor'),
    width: 140,
  },
  {
    key: 'sourceInstruction',
    title: sourceEcDetailPi.label('sourceInstruction'),
    width: 140,
  },
  {
    key: 'sourceOldPartDisposition',
    title: sourceEcDetailPi.label('sourceOldPartDisposition'),
    width: 140,
  },
  {
    key: 'sourceBomEffectiveDate',
    title: sourceEcDetailPi.label('sourceBomEffectiveDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'isObsolete',
    title: sourceEcDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SourceEcCreate & { sourceEcId?: string }> | null | undefined) {
  const rows_sourceEcDetail = ((val as any)?.sourceEcDetails ?? []) as Record<string, unknown>[]
  childSourceEcDetailRows.value = rows_sourceEcDetail
}

function createDefaultSourceEcDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextSourceEcDetailLineNumber(),
    sourceRootMaterialCode: '',
    sourceParentMaterialCode: '',
    sourceOldMaterialCode: '',
    sourceOldMaterialDescription: '',
    sourceOldUsageQuantity: 0,
    sourceOldItemPosition: '',
    sourceNewMaterialCode: '',
    sourceNewMaterialDescription: '',
    sourceNewUsageQuantity: 0,
    sourceNewItemPosition: '',
    sourceBomCode: '',
    sourceCompatibility: '',
    source2ndVendor: '',
    sourceInstruction: '',
    sourceOldPartDisposition: '',
    sourceBomEffectiveDate: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.sourceEcId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    sourceEcDetails: sourceEcDetailTableRef.value?.getRows?.() ?? childSourceEcDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        sourceEcId: masterId,
      }
      if (isUpdate && isPersistedSourceEcDetailRow(row)) {
        normalized.sourceEcDetailId = row.sourceEcDetailId
      } else {
        delete normalized.sourceEcDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SourceEcCreate & { sourceEcId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 sourceEcId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sourceEcId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).sourceEcDetails
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

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.sourceEcId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  sourceEcCode: [
    {
      required: true,
      message: pi.ph('sourceEcCode'),
      trigger: 'blur'
    }
  ],
  sourceModel: [
    {
      required: true,
      message: pi.ph('sourceModel'),
      trigger: 'blur'
    }
  ],
  sourceTitle: [
    {
      required: true,
      message: pi.ph('sourceTitle'),
      trigger: 'blur'
    }
  ],
  sourceStatus: [
    {
      required: true,
      message: pi.ph('sourceStatus'),
      trigger: 'blur'
    }
  ],
  sourceIssueDate: [
    {
      required: true,
      message: pi.ph('sourceIssueDate'),
      trigger: 'change'
    }
  ],
  sourceUnitCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sourceUnitCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sourceUnitCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceMoldModificationCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sourceMoldModificationCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sourceMoldModificationCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceEcContent: [
    {
      required: true,
      message: pi.ph('sourceEcContent'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await sourceEcDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sourceUnitCost' in payload) {
    const rawsourceUnitCost = payload.sourceUnitCost
    if (rawsourceUnitCost === undefined || rawsourceUnitCost === null || rawsourceUnitCost === '') {
      delete payload.sourceUnitCost
    } else {
      const numsourceUnitCost = typeof rawsourceUnitCost === 'number' ? rawsourceUnitCost : Number(rawsourceUnitCost)
      if (Number.isFinite(numsourceUnitCost)) payload.sourceUnitCost = numsourceUnitCost
      else delete payload.sourceUnitCost
    }
  }
  if ('sourceMoldModificationCost' in payload) {
    const rawsourceMoldModificationCost = payload.sourceMoldModificationCost
    if (rawsourceMoldModificationCost === undefined || rawsourceMoldModificationCost === null || rawsourceMoldModificationCost === '') {
      delete payload.sourceMoldModificationCost
    } else {
      const numsourceMoldModificationCost = typeof rawsourceMoldModificationCost === 'number' ? rawsourceMoldModificationCost : Number(rawsourceMoldModificationCost)
      if (Number.isFinite(numsourceMoldModificationCost)) payload.sourceMoldModificationCost = numsourceMoldModificationCost
      else delete payload.sourceMoldModificationCost
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.sourceEcId) {
    payload.sourceEcId = props.formData.sourceEcId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.sourceEcId)
  childSourceEcDetailRows.value = []
  sourceEcDetailTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
