<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/expense/components -->
<!-- 文件名称：ExpenseForm.vue -->
<!-- 功能描述：费用单实体维护弹窗内嵌表单（上主下从级联保存）。defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form expense-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="expense-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="pi.label('expenseCode')"
                name="expenseCode"
              >
                <a-input
                  v-model:value="formState.expenseCode"
                  :placeholder="pi.ph('expenseCode')"
                  show-count
                  :maxlength="40"
                  :disabled="!!formData?.expenseId"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseTitle')"
                name="expenseTitle"
              >
                <a-input
                  v-model:value="formState.expenseTitle"
                  :placeholder="pi.ph('expenseTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseType')"
                name="expenseType"
              >
                <TaktSelect
                  v-model:value="formState.expenseType"
                  dict-type="accounting_financial_expense_type"
                  :placeholder="pi.ph('expenseType')"
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
                  allow-clear
                  @change="handleSupplierChange"
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
                :label="pi.label('costCenter')"
                name="costCenter"
              >
                <TaktTreeSelect
                  v-model:value="formState.costCenter"
                  api-url="TaktCostCenters/tree-options"
                  :lazy="true"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="pi.ph('costCenter')"
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
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseAmount')"
                name="expenseAmount"
              >
                <a-input-number
                  v-model:value="formState.expenseAmount"
                  :placeholder="pi.ph('expenseAmount')"
                  style="width: 100%"
                  :min="0"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxRate')"
                name="taxRate"
              >
                <TaktSelect
                  v-model:value="formState.taxRate"
                  dict-type="accounting_financial_tax_rate_param"
                  :placeholder="pi.ph('taxRate')"
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
                  :min="0"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseDate')"
                name="expenseDate"
              >
                <a-date-picker
                  v-model:value="formState.expenseDate"
                  value-format="YYYY-MM-DDTHH:mm:ss"
                  style="width: 100%"
                  :placeholder="pi.ph('expenseDate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseStatus')"
                name="expenseStatus"
              >
                <TaktSelect
                  v-model:value="formState.expenseStatus"
                  dict-type="sys_approval_status"
                  :placeholder="pi.ph('expenseStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('applicationReason')"
                name="applicationReason"
              >
                <a-textarea
                  v-model:value="formState.applicationReason"
                  :placeholder="pi.ph('applicationReason')"
                  :rows="3"
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
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 expenseDetails -->
    <TaktEditableTable
      ref="expenseDetailTableRef"
      v-model="childExpenseDetailRows"
      :columns="expenseDetailFormColumns"
      :title="expenseDetailPi.self()"
      :add-button-entity="expenseDetailPi.self()"
      id-field="expenseDetailId"
      :default-row="createDefaultExpenseDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 费用单实体维护表单
 * @module views/accounting/financial/expense/components
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { useExpenseI18n } from '../composables/useExpenseI18n'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
import type { ExpenseCreate } from '@/types/accounting/financial/expense.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useExpenseDetailI18n } from '../composables/useExpenseDetailI18n'

/** 实体字段 i18n */
const pi = useExpenseI18n()
const expenseDetailPi = useExpenseDetailI18n()

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()

/**
 * 上下文隔离字段注入
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
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

/** 表单内容区高度 class */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表 */
const formFields = ['tenantCode', 'companyCode', 'cultureCode', 'plantCode', 'expenseCode', 'expenseTitle', 'expenseType', 'supplierCode', 'supplierName1', 'applicantBy', 'applicantName', 'applicationDeptId', 'applicationDeptName', 'costBearerDeptId', 'costBearerDeptName', 'costCenter', 'countersignId', 'purchaseOrderCode', 'purchaseRequestCode', 'expenseAmount', 'taxRate', 'taxAmount', 'expenseDate', 'applicationReason', 'fileName', 'accessUrl', 'expenseStatus', 'extField', 'remark']

const childExpenseDetailRows = ref<Record<string, unknown>[]>([])
const expenseDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 expenseDetail 可编辑列（待业务细化） */
const expenseDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ExpenseCreate & { expenseId?: string }> | null | undefined) {
  const rows_expenseDetail = ((val as any)?.expenseDetails ?? []) as Record<string, unknown>[]
  childExpenseDetailRows.value = rows_expenseDetail
}

function createDefaultExpenseDetailRow(): Record<string, unknown> {
  return {}
}

/** 选项变更时回填冗余名称 */
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

/** 供应商变更：回填名称 */
function handleSupplierChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.supplierName1 = resolveOptionName(value, option, true)
}

/** 申请人变更：回填员工姓名 */
function handleApplicantChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.applicantName = resolveOptionName(value, option, true)
}

/** 申请部门变更：回填部门名称 */
function handleApplicationDeptChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.applicationDeptName = resolveOptionName(value, option)
}

/** 经费负担部门变更：回填部门名称 */
function handleCostBearerDeptChange(
  value: string | number | (string | number)[] | undefined,
  option: SelectOptionLike | SelectOptionLike[] | unknown,
) {
  formState.costBearerDeptName = resolveOptionName(value, option)
}

/** 文件上传中 */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传 accept */
const taktFileAccept = ref('')
/** 上传体积上限 MB */
const taktFileMaxSizeMb = ref(500)

/** 按 fileName / accessUrl 同步上传列表展示 */
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

/** 将 TaktFile 上传结果回填至表单 */
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

/** takt-upload-file 自定义上传 */
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

/** 组装 Create/Update 载荷 */
function buildSubmitPayload() {
  const masterId = props.formData?.expenseId ?? ''
  return {
    ...formState,
    expenseDetails: expenseDetailTableRef.value?.getRows?.() ?? childExpenseDetailRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      expenseId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO */
interface Props {
  formData?: Partial<ExpenseCreate & { expenseId?: string }> | null
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

/** 表单字段默认值 */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  expenseType: 1,
  expenseStatus: 0,
  taxRate: 0,
  expenseAmount: 0,
  taxAmount: 0,
}

/** 写入表单默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** 表单挂载时预加载字典与上传策略 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  void (async () => {
    try {
      const policy = await loadTaktFileUploadBasePolicy()
      taktFileAccept.value = buildTaktFileAcceptAttribute(policy.allowedExtensions ?? [])
      taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
    } catch {
      // 回退默认值
    }
  })()
})

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 编辑态灌入 formData；新增态恢复默认值 */
watch(
  () => props.formData,
  (val) => {
    if (val?.expenseId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as any).expenseDetails
      applyScopeDefaults(next)
      Object.assign(formState, next)
      syncChildRowsFromFormData(val)
      syncFilesFileListFromFormState()
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      syncFilesFileListFromFormState()
      formRef.value?.clearValidate()
    }
  },
  { immediate: true },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.expenseId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  expenseCode: [{ required: true, message: pi.ph('expenseCode'), trigger: 'blur' }],
  expenseTitle: [{ required: true, message: pi.ph('expenseTitle'), trigger: 'blur' }],
  expenseType: [{ required: true, message: pi.ph('expenseType'), trigger: 'change' }],
  applicantBy: [{ required: true, message: pi.ph('applicantBy'), trigger: 'change' }],
  expenseAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('expenseAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('expenseAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  expenseDate: [{ required: true, message: pi.ph('expenseDate'), trigger: 'change' }],
}))

/** 校验表单 */
async function validate() {
  await formRef.value?.validate()
  await expenseDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder
  if (props.formData?.expenseId) {
    payload.expenseId = props.formData.expenseId
  }
  return payload
}

/** 重置表单与子表行 */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.expenseId)
  childExpenseDetailRows.value = []
  expenseDetailTableRef.value?.resetRows?.()
  syncFilesFileListFromFormState()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
