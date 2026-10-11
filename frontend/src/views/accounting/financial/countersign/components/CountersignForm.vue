<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/countersign/components -->
<!-- 文件名称：CountersignForm.vue -->
<!-- 功能描述：会签单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form countersign-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="countersign-form-tabs"
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
                :label="pi.label('purchaseInquiryId')"
                name="purchaseInquiryId"
              >
                <TaktSelect
                  v-model:value="formState.purchaseInquiryId"
                  api-url="TaktPurchaseInquiries/options"
                  :placeholder="pi.ph('purchaseInquiryId')"
                  allow-clear
                  @change="handlePurchaseInquiryChange"
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
                :label="pi.label('businessType')"
                name="businessType"
              >
                <TaktSelect
                  v-model:value="formState.businessType"
                  dict-type="accounting_financial_countersign_business_type"
                  :placeholder="pi.ph('businessType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessKey')"
                name="businessKey"
              >
                <a-input
                  v-model:value="formState.businessKey"
                  :placeholder="pi.ph('businessKey')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stepNo')"
                name="stepNo"
              >
                <a-input-number
                  v-model:value="formState.stepNo"
                  :placeholder="pi.ph('stepNo')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countersignDepts')"
                name="countersignDepts"
              >
                <TaktTreeSelect
                  v-model:value="formState.countersignDepts"
                  api-url="TaktDepts/tree-options"
                  :lazy="true"
                  multiple
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="pi.ph('countersignDepts')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('financeDept')"
                name="financeDept"
              >
                <a-input
                  v-model:value="formState.financeDept"
                  :placeholder="pi.ph('financeDept')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetReviewComment')"
                name="budgetReviewComment"
              >
                <a-input
                  v-model:value="formState.budgetReviewComment"
                  :placeholder="pi.ph('budgetReviewComment')"
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
                :label="pi.label('executiveOffice')"
                name="executiveOffice"
              >
                <a-input
                  v-model:value="formState.executiveOffice"
                  :placeholder="pi.ph('executiveOffice')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantBy')"
                name="applicantBy"
              >
                <TaktSelect
                  v-model:value="formState.applicantBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('applicantBy')"
                  @change="handleApplicantChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantName')"
                name="applicantName"
              >
                <a-input
                  v-model:value="formState.applicantName"
                  :placeholder="pi.ph('applicantName')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationDeptId')"
                name="applicationDeptId"
              >
                <TaktTreeSelect
                  v-model:value="formState.applicationDeptId"
                  api-url="TaktDepts/tree-options"
                  :lazy="true"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="pi.ph('applicationDeptId')"
                  @change="handleApplicationDeptChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationDeptName')"
                name="applicationDeptName"
              >
                <a-input
                  v-model:value="formState.applicationDeptName"
                  :placeholder="pi.ph('applicationDeptName')"
                  show-count
                  :maxlength="40"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costBearerDeptId')"
                name="costBearerDeptId"
              >
                <TaktTreeSelect
                  v-model:value="formState.costBearerDeptId"
                  api-url="TaktDepts/tree-options"
                  :lazy="true"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="pi.ph('costBearerDeptId')"
                  @change="handleCostBearerDeptChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costBearerDeptName')"
                name="costBearerDeptName"
              >
                <a-input
                  v-model:value="formState.costBearerDeptName"
                  :placeholder="pi.ph('costBearerDeptName')"
                  show-count
                  :maxlength="40"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isBudget')"
                name="isBudget"
              >
                <TaktSelect
                  v-model:value="formState.isBudget"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isBudget')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetItemId')"
                name="budgetItemId"
              >
                <TaktSelect
                  v-model:value="formState.budgetItemId"
                  api-url="TaktBudgetActuals/options"
                  :placeholder="pi.ph('budgetItemId')"
                  allow-clear
                  @change="handleBudgetItemChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetItem')"
                name="budgetItem"
              >
                <a-input
                  v-model:value="formState.budgetItem"
                  :placeholder="pi.ph('budgetItem')"
                  show-count
                  :maxlength="200"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetAmount')"
                name="budgetAmount"
              >
                <a-input-number
                  v-model:value="formState.budgetAmount"
                  :placeholder="pi.ph('budgetAmount')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationAmount')"
                name="applicationAmount"
              >
                <a-input-number
                  v-model:value="formState.applicationAmount"
                  :placeholder="pi.ph('applicationAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countersignTitle')"
                name="countersignTitle"
              >
                <a-input
                  v-model:value="formState.countersignTitle"
                  :placeholder="pi.ph('countersignTitle')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationReason')"
                name="applicationReason"
              >
                <a-input
                  v-model:value="formState.applicationReason"
                  :placeholder="pi.ph('applicationReason')"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('budgetUsageDescription')"
                name="budgetUsageDescription"
              >
                <a-textarea
                  v-model:value="formState.budgetUsageDescription"
                  :placeholder="pi.ph('budgetUsageDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetAndExpectedBenefit')"
                name="targetAndExpectedBenefit"
              >
                <a-input
                  v-model:value="formState.targetAndExpectedBenefit"
                  :placeholder="pi.ph('targetAndExpectedBenefit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fileName')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="pi.ph('fileName')"
                  show-count
                  :maxlength="200"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('accessUrl')"
                name="accessUrl"
              >
                <TaktUploadFile
                  tabs-type="files"
                  :files-auto-upload="true"
                  :files-multiple="false"
                  :files-max-count="1"
                  :files-disabled="!!loading || fileUploading"
                  :files-max-size="taktFileMaxSizeMb"
                  :files-accept="taktFileAccept"
                  :files-hint="t('foundation.file.page.upload.hint', { max: taktFileMaxSizeMb })"
                  :files-custom-request="handleFilesCustomRequest"
                  v-model:files-file-list="filesFileList"
                  @files:remove="handleFileRemove"
                />
                <a-input
                  v-if="formState.accessUrl"
                  v-model:value="formState.accessUrl"
                  class="mt-2"
                  :placeholder="pi.ph('accessUrl')"
                  show-count
                  :maxlength="1000"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('countersignStatus')"
                name="countersignStatus"
              >
                <TaktSelect
                  v-model:value="formState.countersignStatus"
                  dict-type="sys_approval_status"
                  :placeholder="pi.ph('countersignStatus')"
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
    <!-- 下：子表 countersignDetails -->
    <TaktEditableTable
      ref="countersignDetailTableRef"
      v-model="childCountersignDetailRows"
      :columns="countersignDetailFormColumns"
      :title="countersignDetailPi.self()"
      :add-button-entity="countersignDetailPi.self()"
      id-field="countersignDetailId"
      :default-row="createDefaultCountersignDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-allocationCategory="{ record }">
        <TaktSelect
          v-model:value="record.allocationCategory"
          dict-type="logistics_sales_allocation_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="countersignDetailPi.ph('allocationCategory')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-accountTitle="{ record }">
        <TaktSelect
          v-model:value="record.accountTitle"
          api-url="TaktAccountTitles/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="countersignDetailPi.queryPh('accountTitle', 'select')"
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
          :placeholder="countersignDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会签单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/countersign/components
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { useCountersignI18n } from '../composables/useCountersignI18n'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'

/** 实体字段 i18n */
const pi = useCountersignI18n()

import type { CountersignCreate } from '@/types/accounting/financial/countersign.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseInquiryId","purchaseInquiryCode","businessType","businessKey","stepNo","countersignDepts","financeDept","budgetReviewComment","executiveOffice","applicantBy","applicantName","applicationDeptId","applicationDeptName","costBearerDeptId","costBearerDeptName","isBudget","budgetItemId","budgetItem","budgetAmount","applicationAmount","countersignTitle","applicationReason","budgetUsageDescription","targetAndExpectedBenefit","fileName","accessUrl","countersignStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useCountersignDetailI18n } from '../composables/useCountersignDetailI18n'

const countersignDetailPi = useCountersignDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childCountersignDetailRows = ref<Record<string, unknown>[]>([])
const countersignDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedCountersignDetailRow(row: Record<string, unknown>): boolean {
  const id = row.countersignDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextCountersignDetailLineNumber(): number {
  const rows = countersignDetailTableRef.value?.getRows?.() ?? childCountersignDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 countersignDetail 可编辑列 */
const countersignDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: countersignDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'allocationCategory',
    title: countersignDetailPi.label('allocationCategory'),
    width: 140,
  },
  {
    key: 'accountTitle',
    title: countersignDetailPi.label('accountTitle'),
    width: 140,
  },
  {
    key: 'itemName',
    title: countersignDetailPi.label('itemName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: countersignDetailPi.label('itemDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: countersignDetailPi.ph('itemDescription'),
    width: 180,
  },
  {
    key: 'itemQuantity',
    title: countersignDetailPi.label('itemQuantity'),
    width: 140,
  },
  {
    key: 'itemAmount',
    title: countersignDetailPi.label('itemAmount'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: countersignDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CountersignCreate & { countersignId?: string }> | null | undefined) {
  const rows_countersignDetail = ((val as any)?.countersignDetails ?? []) as Record<string, unknown>[]
  childCountersignDetailRows.value = rows_countersignDetail
}

function createDefaultCountersignDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextCountersignDetailLineNumber(),
    allocationCategory: '',
    accountTitle: '',
    itemName: '',
    itemDescription: '',
    itemQuantity: 0,
    itemAmount: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.countersignId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    countersignDetails: countersignDetailTableRef.value?.getRows?.() ?? childCountersignDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        countersignId: masterId,
      }
      if (isUpdate && isPersistedCountersignDetailRow(row)) {
        normalized.countersignDetailId = row.countersignDetailId
      } else {
        delete normalized.countersignDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CountersignCreate & { countersignId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  businessType: "INQUIRY",
  countersignStatus: 0
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  if (!Array.isArray(target.countersignDepts)) {
    target.countersignDepts = parseCountersignDeptIds(target.countersignDepts)
  }
}

/** 选项变更时回填冗余名称 / 金额 */
type SelectOptionLike = { label?: string; dictLabel?: string; extValue?: string } | null

/**
 * 从选项解析展示文案
 * @param value 选中值
 * @param option 选项或选项数组
 * @param preferExtValue 为 true 时优先 ExtValue
 * @returns {string} 冗余名称
 */
function resolveOptionName(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
  preferExtValue = false,
): string {
  if (value === undefined || value === null || value === '') {
    return ''
  }
  const opt = Array.isArray(option) ? option[0] : option
  const rec = opt && typeof opt === 'object' ? (opt as { label?: string; dictLabel?: string; extValue?: string }) : undefined
  if (preferExtValue && rec?.extValue) {
    return String(rec.extValue).trim()
  }
  return String(rec?.label ?? rec?.dictLabel ?? '').trim()
}

/**
 * 解析会签部门 JSON 为树选择多选值
 * @param raw 后端 JSON 字符串或已解析数组
 * @returns {string[]} 部门 Id
 */
function parseCountersignDeptIds(raw: unknown): string[] {
  if (Array.isArray(raw)) {
    return raw.map((item) => String(item)).filter((item) => item.length > 0)
  }
  if (typeof raw !== 'string' || !raw.trim()) {
    return []
  }
  try {
    const parsed = JSON.parse(raw) as unknown
    if (Array.isArray(parsed)) {
      return parsed.map((item) => String(item)).filter((item) => item.length > 0)
    }
  } catch {
    return raw.trim() ? [raw.trim()] : []
  }
  return []
}

/**
 * 来源采购询价变更：回填询价编码（ExtValue / DictLabel）
 * @param value 询价 Id
 * @param option 选项
 */
function handlePurchaseInquiryChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.purchaseInquiryCode = resolveOptionName(value, option, true)
}

/**
 * 申请人变更：回填员工姓名
 * @param value 员工 Id
 * @param option 选项
 */
function handleApplicantChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.applicantName = resolveOptionName(value, option, true)
}

/**
 * 申请部门变更：回填部门名称
 * @param value 部门 Id
 * @param option 选项
 */
function handleApplicationDeptChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.applicationDeptName = resolveOptionName(value, option)
}

/**
 * 经费负担部门变更：回填部门名称
 * @param value 部门 Id
 * @param option 选项
 */
function handleCostBearerDeptChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.costBearerDeptName = resolveOptionName(value, option)
}

/**
 * 预算项目变更：回填项目名称与预算金额
 * @param value 预算实绩 Id
 * @param option 选项（ExtValue=BudgetAmount）
 */
function handleBudgetItemChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.budgetItem = resolveOptionName(value, option)
  if (value === undefined || value === null || value === '') {
    formState.budgetAmount = 0
    return
  }
  const opt = Array.isArray(option) ? option[0] : option
  const rec = opt && typeof opt === 'object' ? (opt as { extValue?: string }) : undefined
  const amount = Number(rec?.extValue)
  formState.budgetAmount = Number.isFinite(amount) ? amount : 0
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 文件上传中 */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传 accept */
const taktFileAccept = ref('')
/** 上传体积上限 MB */
const taktFileMaxSizeMb = ref(500)

/**
 * 按 fileName / accessUrl 同步上传列表展示
 */
function syncFilesFileListFromFormState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url) {
    filesFileList.value = []
    return
  }
  filesFileList.value = [{
    uid: '-1',
    name: String(formState.fileName ?? url.split('/').pop() ?? 'file'),
    status: 'done',
    url,
  }]
}

/**
 * 将 TaktFile 上传结果回填至表单（文件名由上传结果回填，禁止手输）
 * @param file 本地文件
 * @param result 上传结果
 */
async function applyUploadResultToForm(file: globalThis.File, result: Awaited<ReturnType<typeof uploadTaktFileSmart>>) {
  let accessUrl = result.accessUrl?.trim() ?? ''
  if (!accessUrl && result.fileId) {
    const detail = await getFileById(result.fileId)
    accessUrl = detail.accessUrl?.trim() ?? ''
  }
  if (!accessUrl) {
    throw new Error('accessUrl empty')
  }
  formState.accessUrl = accessUrl
  formState.fileName = result.fileOriginalName?.trim()
    || result.fileName?.trim()
    || file.name
  syncFilesFileListFromFormState()
  formRef.value?.validateFields(['accessUrl', 'fileName']).catch(() => undefined)
}

/** takt-upload-file 自定义上传：落库 TaktFile 后回写 accessUrl / fileName */
const handleFilesCustomRequest: UploadProps['customRequest'] = (options) => {
  if (props.loading || fileUploading.value) {
    options.onError?.(new Error('upload disabled'))
    return
  }
  const originFile = options.file as globalThis.File
  fileUploading.value = true
  void (async () => {
    try {
      const result = await uploadTaktFileSmart(originFile)
      await applyUploadResultToForm(originFile, result)
      options.onSuccess?.(result)
    } catch (error: unknown) {
      const err = error instanceof Error ? error : new Error(String(error))
      message.error(t('common.feedback.failed'))
      options.onError?.(err)
    } finally {
      fileUploading.value = false
    }
  })()
}

/** 移除已上传文件 */
function handleFileRemove() {
  formState.accessUrl = ''
  formState.fileName = ''
  filesFileList.value = []
}

/** 表单挂载时预加载全量字典与上传策略 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  void (async () => {
    try {
      const policy = await loadTaktFileUploadBasePolicy()
      taktFileAccept.value = buildTaktFileAcceptAttribute(policy.allowedExtensions ?? [])
      taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
    } catch {
      // 回退默认值；实际上传校验仍由后端 API 返回
    }
  })()
})

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 编辑态灌入 formData；新增态恢复默认值（须含 countersignId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.countersignId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).countersignDetails
      applyScopeDefaults(next)
      next.countersignDepts = parseCountersignDeptIds(next.countersignDepts)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      syncFilesFileListFromFormState()
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        next.countersignDepts = parseCountersignDeptIds(next.countersignDepts)
        Object.assign(formState, next)
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
    if (!props.formData?.countersignId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  businessType: [
    {
      required: true,
      message: pi.ph('businessType'),
      trigger: 'change'
    }
  ],
  stepNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stepNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stepNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  applicantBy: [
    {
      required: true,
      message: pi.ph('applicantBy'),
      trigger: 'change'
    }
  ],
  isBudget: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isBudget'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isBudget'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  budgetAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('budgetAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('budgetAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  applicationAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('applicationAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('applicationAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  countersignStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('countersignStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('countersignStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await countersignDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('countersignDepts' in payload) {
    const rawDepts = payload.countersignDepts
    if (Array.isArray(rawDepts)) {
      payload.countersignDepts = rawDepts.length > 0 ? JSON.stringify(rawDepts.map((item) => String(item))) : ''
    }
  }
  if ('stepNo' in payload) {
    const rawstepNo = payload.stepNo
    if (rawstepNo === undefined || rawstepNo === null || rawstepNo === '') {
      delete payload.stepNo
    } else {
      const numstepNo = typeof rawstepNo === 'number' ? rawstepNo : Number(rawstepNo)
      if (Number.isFinite(numstepNo)) payload.stepNo = numstepNo
      else delete payload.stepNo
    }
  }
  if ('isBudget' in payload) {
    const rawisBudget = payload.isBudget
    if (rawisBudget === undefined || rawisBudget === null || rawisBudget === '') {
      delete payload.isBudget
    } else {
      const numisBudget = typeof rawisBudget === 'number' ? rawisBudget : Number(rawisBudget)
      if (Number.isFinite(numisBudget)) payload.isBudget = numisBudget
      else delete payload.isBudget
    }
  }
  if ('budgetAmount' in payload) {
    const rawbudgetAmount = payload.budgetAmount
    if (rawbudgetAmount === undefined || rawbudgetAmount === null || rawbudgetAmount === '') {
      delete payload.budgetAmount
    } else {
      const numbudgetAmount = typeof rawbudgetAmount === 'number' ? rawbudgetAmount : Number(rawbudgetAmount)
      if (Number.isFinite(numbudgetAmount)) payload.budgetAmount = numbudgetAmount
      else delete payload.budgetAmount
    }
  }
  if ('applicationAmount' in payload) {
    const rawapplicationAmount = payload.applicationAmount
    if (rawapplicationAmount === undefined || rawapplicationAmount === null || rawapplicationAmount === '') {
      delete payload.applicationAmount
    } else {
      const numapplicationAmount = typeof rawapplicationAmount === 'number' ? rawapplicationAmount : Number(rawapplicationAmount)
      if (Number.isFinite(numapplicationAmount)) payload.applicationAmount = numapplicationAmount
      else delete payload.applicationAmount
    }
  }
  if ('countersignStatus' in payload) {
    const rawcountersignStatus = payload.countersignStatus
    if (rawcountersignStatus === undefined || rawcountersignStatus === null || rawcountersignStatus === '') {
      delete payload.countersignStatus
    } else {
      const numcountersignStatus = typeof rawcountersignStatus === 'number' ? rawcountersignStatus : Number(rawcountersignStatus)
      if (Number.isFinite(numcountersignStatus)) payload.countersignStatus = numcountersignStatus
      else delete payload.countersignStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.countersignId) {
    payload.countersignId = props.formData.countersignId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    const next = { ...props.formData } as Record<string, unknown>
    next.countersignDepts = parseCountersignDeptIds(next.countersignDepts)
    Object.assign(formState, next)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.countersignId)
  childCountersignDetailRows.value = []
  countersignDetailTableRef.value?.resetRows?.()
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
