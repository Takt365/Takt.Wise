<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint/components -->
<!-- 文件名称：CustomerComplaintItemForm.vue -->
<!-- 功能描述：客诉主表实体子表 customerComplaintItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form customer-complaint-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-complaint-item-form-tabs"
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
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productCode')"
                name="productCode"
              >
                <TaktSelect
                  v-model:value="formState.productCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('productCode')"
                  :disabled="!!formData?.customerComplaintItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productName')"
                name="productName"
              >
                <a-input
                  v-model:value="formState.productName"
                  :placeholder="pi.ph('productName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchCode')"
                name="batchCode"
              >
                <a-input
                  v-model:value="formState.batchCode"
                  :placeholder="pi.ph('batchCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.customerComplaintItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('itemType')"
                name="itemType"
              >
                <TaktSelect
                  v-model:value="formState.itemType"
                  dict-type="logistics_quality_complaint_item_type"
                  :placeholder="pi.ph('itemType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('defectDescription')"
                name="defectDescription"
              >
                <a-textarea
                  v-model:value="formState.defectDescription"
                  :placeholder="pi.ph('defectDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectLevel')"
                name="defectLevel"
              >
                <TaktSelect
                  v-model:value="formState.defectLevel"
                  dict-type="logistics_quality_defect_severity_code"
                  :placeholder="pi.ph('defectLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectQuantity')"
                name="defectQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectQuantity"
                  :placeholder="pi.ph('defectQuantity')"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectRate')"
                name="defectRate"
              >
                <a-input-number
                  v-model:value="formState.defectRate"
                  :placeholder="pi.ph('defectRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('causeAnalysis')"
                name="causeAnalysis"
              >
                <a-input
                  v-model:value="formState.causeAnalysis"
                  :placeholder="pi.ph('causeAnalysis')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('improvementAction')"
                name="improvementAction"
              >
                <a-input
                  v-model:value="formState.improvementAction"
                  :placeholder="pi.ph('improvementAction')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('improvementResponsibleGroupCode')"
                name="improvementResponsibleGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.improvementResponsibleGroupCode"
                  api-url="TaktComplaintGroups/options"
                  :api-params="{ complaintGroupCategory: 3 }"
                  :placeholder="pi.ph('improvementResponsibleGroupCode')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedCompletionDate')"
                name="plannedCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedCompletionDate"
                  :placeholder="pi.ph('plannedCompletionDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualCompletionDate')"
                name="actualCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.actualCompletionDate"
                  :placeholder="pi.ph('actualCompletionDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                  :maxlength="20"
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
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('improvementStatus')"
                name="improvementStatus"
              >
                <TaktSelect
                  v-model:value="formState.improvementStatus"
                  dict-type="logistics_quality_improvement_status"
                  :placeholder="pi.ph('improvementStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isObsolete')"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客诉主表实体子表 customerComplaintItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/complaint/customer-complaint/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerComplaintItemI18n } from '../composables/useCustomerComplaintItemI18n'

/** 实体字段 i18n */
const pi = useCustomerComplaintItemI18n()

import type { CustomerComplaintItemCreate } from '@/types/logistics/quality/complaint/customerComplaintItem.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { message } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","productCode","productName","batchCode","itemType","defectDescription","defectLevel","defectQuantity","defectRate","causeAnalysis","improvementAction","improvementResponsibleGroupCode","plannedCompletionDate","actualCompletionDate","fileName","accessUrl","improvementStatus","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerComplaintItemCreate & { customerComplaintItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterRow: null,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  itemType: 0,
  improvementStatus: 0
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

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 挂载后加载后端上传策略（accept / maxSize） */
onMounted(() => {
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerComplaintItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerComplaintItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    if (!props.formData?.customerComplaintItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  itemType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('itemType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('itemType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectDescription: [
    {
      required: true,
      message: pi.ph('defectDescription'),
      trigger: 'blur'
    }
  ],
  defectLevel: [
    {
      required: true,
      message: pi.ph('defectLevel'),
      trigger: 'change'
    }
  ],
  defectQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  improvementStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('improvementStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('improvementStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 complaintId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    if (rawlineNumber === undefined || rawlineNumber === null || rawlineNumber === '') {
      delete payload.lineNumber
    } else {
      const numlineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
      if (Number.isFinite(numlineNumber)) payload.lineNumber = numlineNumber
      else delete payload.lineNumber
    }
  }
  if ('itemType' in payload) {
    const rawitemType = payload.itemType
    if (rawitemType === undefined || rawitemType === null || rawitemType === '') {
      delete payload.itemType
    } else {
      const numitemType = typeof rawitemType === 'number' ? rawitemType : Number(rawitemType)
      if (Number.isFinite(numitemType)) payload.itemType = numitemType
      else delete payload.itemType
    }
  }
  if ('defectQuantity' in payload) {
    const rawdefectQuantity = payload.defectQuantity
    if (rawdefectQuantity === undefined || rawdefectQuantity === null || rawdefectQuantity === '') {
      delete payload.defectQuantity
    } else {
      const numdefectQuantity = typeof rawdefectQuantity === 'number' ? rawdefectQuantity : Number(rawdefectQuantity)
      if (Number.isFinite(numdefectQuantity)) payload.defectQuantity = numdefectQuantity
      else delete payload.defectQuantity
    }
  }
  if ('defectRate' in payload) {
    const rawdefectRate = payload.defectRate
    if (rawdefectRate === undefined || rawdefectRate === null || rawdefectRate === '') {
      delete payload.defectRate
    } else {
      const numdefectRate = typeof rawdefectRate === 'number' ? rawdefectRate : Number(rawdefectRate)
      if (Number.isFinite(numdefectRate)) payload.defectRate = numdefectRate
      else delete payload.defectRate
    }
  }
  if ('improvementStatus' in payload) {
    const rawimprovementStatus = payload.improvementStatus
    if (rawimprovementStatus === undefined || rawimprovementStatus === null || rawimprovementStatus === '') {
      delete payload.improvementStatus
    } else {
      const numimprovementStatus = typeof rawimprovementStatus === 'number' ? rawimprovementStatus : Number(rawimprovementStatus)
      if (Number.isFinite(numimprovementStatus)) payload.improvementStatus = numimprovementStatus
      else delete payload.improvementStatus
    }
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    if (rawisObsolete === undefined || rawisObsolete === null || rawisObsolete === '') {
      delete payload.isObsolete
    } else {
      const numisObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
      if (Number.isFinite(numisObsolete)) payload.isObsolete = numisObsolete
      else delete payload.isObsolete
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.customerComplaintItemId) {
    payload.customerComplaintItemId = props.formData.customerComplaintItemId
    delete payload.numberingRuleCode
  }
  payload.complaintId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.customerComplaintCode ?? masterRow.CustomerComplaintCode
    const masterName = masterRow.customerComplaintName ?? masterRow.CustomerComplaintName
    if (masterCode != null && masterCode !== '' && !payload.customerComplaintCode) {
      payload.customerComplaintCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.customerComplaintName) {
      payload.customerComplaintName = masterName
    }
    if (masterCode != null && masterCode !== '' && !payload.complaintCode) {
      payload.complaintCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.complaintName) {
      payload.complaintName = masterName
    }
    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerComplaintItemId)
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
