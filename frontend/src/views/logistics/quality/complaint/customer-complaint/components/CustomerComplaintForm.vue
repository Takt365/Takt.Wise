<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint/components -->
<!-- 文件名称：CustomerComplaintForm.vue -->
<!-- 功能描述：客诉主表实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form customer-complaint-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="customer-complaint-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-complaint-form-tabs"
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
                :label="pi.label('customerComplaintCode')"
                name="customerComplaintCode"
              >
                <a-input
                  v-model:value="formState.customerComplaintCode"
                  :placeholder="pi.ph('customerComplaintCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.customerComplaintId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerId')"
                name="customerId"
              >
                <TaktSelect
                  v-model:value="formState.customerId"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerName1')"
                name="customerName1"
              >
                <a-input
                  v-model:value="formState.customerName1"
                  :placeholder="pi.ph('customerName1')"
                  show-count
                  :maxlength="140"
                  disabled
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
                  :disabled="!!formData?.customerComplaintId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('complaintDate')"
                name="complaintDate"
              >
                <a-date-picker
                  v-model:value="formState.complaintDate"
                  :placeholder="pi.ph('complaintDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('complaintMethod')"
                name="complaintMethod"
              >
                <TaktSelect
                  v-model:value="formState.complaintMethod"
                  dict-type="logistics_quality_complaint_method"
                  :placeholder="pi.ph('complaintMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('complaintType')"
                name="complaintType"
              >
                <TaktSelect
                  v-model:value="formState.complaintType"
                  dict-type="logistics_quality_complaint_type"
                  :placeholder="pi.ph('complaintType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('complaintLevel')"
                name="complaintLevel"
              >
                <TaktSelect
                  v-model:value="formState.complaintLevel"
                  dict-type="logistics_quality_complaint_level"
                  :placeholder="pi.ph('complaintLevel')"
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
                :label="pi.label('responsibleDeptId')"
                name="responsibleDeptId"
              >
                <TaktSelect
                  v-model:value="formState.responsibleDeptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('responsibleDeptId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('responsibleDeptName')"
                name="responsibleDeptName"
              >
                <a-input
                  v-model:value="formState.responsibleDeptName"
                  :placeholder="pi.ph('responsibleDeptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('responsibleGroupCode')"
                name="responsibleGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.responsibleGroupCode"
                  api-url="TaktComplaintGroups/options"
                  :api-params="{ complaintGroupCategory: 1 }"
                  :placeholder="pi.ph('responsibleGroupCode')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('responsibleGroupName')"
                name="responsibleGroupName"
              >
                <a-input
                  v-model:value="formState.responsibleGroupName"
                  :placeholder="pi.ph('responsibleGroupName')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requiredReplyDate')"
                name="requiredReplyDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredReplyDate"
                  :placeholder="pi.ph('requiredReplyDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualReplyDate')"
                name="actualReplyDate"
              >
                <a-date-picker
                  v-model:value="formState.actualReplyDate"
                  :placeholder="pi.ph('actualReplyDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('complaintDescription')"
                name="complaintDescription"
              >
                <a-textarea
                  v-model:value="formState.complaintDescription"
                  :placeholder="pi.ph('complaintDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('handlingResult')"
                name="handlingResult"
              >
                <TaktRichEditor
                  v-model:value="formState.handlingResult"
                  :placeholder="pi.ph('handlingResult')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerSatisfaction')"
                name="customerSatisfaction"
              >
                <TaktSelect
                  v-model:value="formState.customerSatisfaction"
                  dict-type="logistics_quality_customer_satisfaction"
                  :placeholder="pi.ph('customerSatisfaction')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('attachments')"
                name="attachments"
              >
                <a-input
                  v-model:value="formState.attachments"
                  :placeholder="pi.ph('attachments')"
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
                :label="pi.label('complaintStatus')"
                name="complaintStatus"
              >
                <TaktSelect
                  v-model:value="formState.complaintStatus"
                  dict-type="logistics_quality_complaint_status"
                  :placeholder="pi.ph('complaintStatus')"
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
      class="customer-complaint-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="customerComplaintItemTableRef"
      v-model="childCustomerComplaintItemRows"
      :columns="customerComplaintItemFormColumns"
      :title="customerComplaintItemPi.self()"
      :add-button-entity="customerComplaintItemPi.self()"
      id-field="customerComplaintItemId"
      :default-row="createDefaultCustomerComplaintItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-productCode="{ record }">
        <TaktSelect
          v-model:value="record.productCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.queryPh('productCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-itemType="{ record }">
        <TaktSelect
          v-model:value="record.itemType"
          dict-type="logistics_quality_complaint_item_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.ph('itemType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectLevel="{ record }">
        <TaktSelect
          v-model:value="record.defectLevel"
          dict-type="logistics_quality_defect_severity_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.ph('defectLevel')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-improvementResponsibleGroupCode="{ record }">
        <TaktSelect
          v-model:value="record.improvementResponsibleGroupCode"
          api-url="TaktComplaintGroups/options"
          :api-params="{ complaintGroupCategory: 3 }"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.queryPh('improvementResponsibleGroupCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-improvementStatus="{ record }">
        <TaktSelect
          v-model:value="record.improvementStatus"
          dict-type="logistics_quality_improvement_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.ph('improvementStatus')"
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
          :placeholder="customerComplaintItemPi.ph('isObsolete')"
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
 * 客诉主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerComplaintI18n } from '../composables/useCustomerComplaintI18n'

/** 实体字段 i18n */
const pi = useCustomerComplaintI18n()

import type { CustomerComplaintCreate } from '@/types/logistics/quality/complaint/customerComplaint.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","customerComplaintCode","customerId","customerName1","customerCode","complaintDate","complaintMethod","complaintType","complaintLevel","responsibleDeptId","responsibleDeptName","responsibleGroupCode","responsibleGroupName","requiredReplyDate","actualReplyDate","complaintDescription","handlingResult","customerSatisfaction","attachments","complaintStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useCustomerComplaintItemI18n } from '../composables/useCustomerComplaintItemI18n'

const customerComplaintItemPi = useCustomerComplaintItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childCustomerComplaintItemRows = ref<Record<string, unknown>[]>([])
const customerComplaintItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedCustomerComplaintItemRow(row: Record<string, unknown>): boolean {
  const id = row.customerComplaintItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextCustomerComplaintItemLineNumber(): number {
  const rows = customerComplaintItemTableRef.value?.getRows?.() ?? childCustomerComplaintItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 customerComplaintItem 可编辑列 */
const customerComplaintItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: customerComplaintItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'productCode',
    title: customerComplaintItemPi.label('productCode'),
    width: 140,
  },
  {
    key: 'productName',
    title: customerComplaintItemPi.label('productName'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('productName'),
  },
  {
    key: 'batchCode',
    title: customerComplaintItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('batchCode'),
  },
  {
    key: 'itemType',
    title: customerComplaintItemPi.label('itemType'),
    width: 140,
  },
  {
    key: 'defectDescription',
    title: customerComplaintItemPi.label('defectDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: customerComplaintItemPi.ph('defectDescription'),
    width: 180,
  },
  {
    key: 'defectLevel',
    title: customerComplaintItemPi.label('defectLevel'),
    width: 140,
  },
  {
    key: 'defectQuantity',
    title: customerComplaintItemPi.label('defectQuantity'),
    width: 140,
  },
  {
    key: 'defectRate',
    title: customerComplaintItemPi.label('defectRate'),
    width: 140,
  },
  {
    key: 'causeAnalysis',
    title: customerComplaintItemPi.label('causeAnalysis'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('causeAnalysis'),
  },
  {
    key: 'improvementAction',
    title: customerComplaintItemPi.label('improvementAction'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('improvementAction'),
  },
  {
    key: 'improvementResponsibleGroupCode',
    title: customerComplaintItemPi.label('improvementResponsibleGroupCode'),
    width: 140,
  },
  {
    key: 'plannedCompletionDate',
    title: customerComplaintItemPi.label('plannedCompletionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'actualCompletionDate',
    title: customerComplaintItemPi.label('actualCompletionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'fileName',
    title: customerComplaintItemPi.label('fileName'),
    readonly: true,
    width: 140,
  },
  {
    key: 'accessUrl',
    title: customerComplaintItemPi.label('accessUrl'),
    editor: 'input',
    width: 180, allowClear: true, placeholder: customerComplaintItemPi.ph('accessUrl'),
  },
  {
    key: 'improvementStatus',
    title: customerComplaintItemPi.label('improvementStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: customerComplaintItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CustomerComplaintCreate & { customerComplaintId?: string }> | null | undefined) {
  const rows_customerComplaintItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childCustomerComplaintItemRows.value = rows_customerComplaintItem
}

function createDefaultCustomerComplaintItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextCustomerComplaintItemLineNumber(),
    productCode: '',
    productName: '',
    batchCode: '',
    itemType: 0,
    defectDescription: '',
    defectLevel: '',
    defectQuantity: 0,
    defectRate: 0,
    causeAnalysis: '',
    improvementAction: '',
    improvementResponsibleGroupCode: '',
    plannedCompletionDate: '',
    actualCompletionDate: '',
    fileName: '',
    accessUrl: '',
    improvementStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.customerComplaintId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: customerComplaintItemTableRef.value?.getRows?.() ?? childCustomerComplaintItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        complaintId: isUpdate ? masterId : 0,
      }
      if (isUpdate && isPersistedCustomerComplaintItemRow(row)) {
        normalized.customerComplaintItemId = row.customerComplaintItemId
      } else {
        delete normalized.customerComplaintItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerComplaintCreate & { customerComplaintId?: string }> | null
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
  complaintMethod: 0,
  complaintType: 0,
  complaintLevel: 0,
  complaintStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerComplaintId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerComplaintId) {
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
    if (!props.formData?.customerComplaintId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  customerComplaintCode: [
    {
      required: true,
      message: pi.ph('customerComplaintCode'),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: pi.ph('customerId'),
      trigger: 'change'
    }
  ],
  complaintDate: [
    {
      required: true,
      message: pi.ph('complaintDate'),
      trigger: 'change'
    }
  ],
  complaintMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintDescription: [
    {
      required: true,
      message: pi.ph('complaintDescription'),
      trigger: 'blur'
    }
  ],
  complaintStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await customerComplaintItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('complaintMethod' in payload) {
    const rawcomplaintMethod = payload.complaintMethod
    if (rawcomplaintMethod === undefined || rawcomplaintMethod === null || rawcomplaintMethod === '') {
      delete payload.complaintMethod
    } else {
      const numcomplaintMethod = typeof rawcomplaintMethod === 'number' ? rawcomplaintMethod : Number(rawcomplaintMethod)
      if (Number.isFinite(numcomplaintMethod)) payload.complaintMethod = numcomplaintMethod
      else delete payload.complaintMethod
    }
  }
  if ('complaintType' in payload) {
    const rawcomplaintType = payload.complaintType
    if (rawcomplaintType === undefined || rawcomplaintType === null || rawcomplaintType === '') {
      delete payload.complaintType
    } else {
      const numcomplaintType = typeof rawcomplaintType === 'number' ? rawcomplaintType : Number(rawcomplaintType)
      if (Number.isFinite(numcomplaintType)) payload.complaintType = numcomplaintType
      else delete payload.complaintType
    }
  }
  if ('complaintLevel' in payload) {
    const rawcomplaintLevel = payload.complaintLevel
    if (rawcomplaintLevel === undefined || rawcomplaintLevel === null || rawcomplaintLevel === '') {
      delete payload.complaintLevel
    } else {
      const numcomplaintLevel = typeof rawcomplaintLevel === 'number' ? rawcomplaintLevel : Number(rawcomplaintLevel)
      if (Number.isFinite(numcomplaintLevel)) payload.complaintLevel = numcomplaintLevel
      else delete payload.complaintLevel
    }
  }
  if ('customerSatisfaction' in payload) {
    const rawcustomerSatisfaction = payload.customerSatisfaction
    if (rawcustomerSatisfaction === undefined || rawcustomerSatisfaction === null || rawcustomerSatisfaction === '') {
      delete payload.customerSatisfaction
    } else {
      const numcustomerSatisfaction = typeof rawcustomerSatisfaction === 'number' ? rawcustomerSatisfaction : Number(rawcustomerSatisfaction)
      if (Number.isFinite(numcustomerSatisfaction)) payload.customerSatisfaction = numcustomerSatisfaction
      else delete payload.customerSatisfaction
    }
  }
  if ('complaintStatus' in payload) {
    const rawcomplaintStatus = payload.complaintStatus
    if (rawcomplaintStatus === undefined || rawcomplaintStatus === null || rawcomplaintStatus === '') {
      delete payload.complaintStatus
    } else {
      const numcomplaintStatus = typeof rawcomplaintStatus === 'number' ? rawcomplaintStatus : Number(rawcomplaintStatus)
      if (Number.isFinite(numcomplaintStatus)) payload.complaintStatus = numcomplaintStatus
      else delete payload.complaintStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.customerComplaintId) {
    payload.customerComplaintId = props.formData.customerComplaintId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerComplaintId)
  childCustomerComplaintItemRows.value = []
  customerComplaintItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.customer-complaint-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.customer-complaint-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.customer-complaint-form__master :deep(.customer-complaint-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.customer-complaint-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.customer-complaint-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.customer-complaint-form__master :deep(.ant-tabs-content),
.customer-complaint-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
