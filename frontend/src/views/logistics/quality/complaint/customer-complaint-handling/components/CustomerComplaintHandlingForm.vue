<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint-handling/components -->
<!-- 文件名称：CustomerComplaintHandlingForm.vue -->
<!-- 功能描述：客诉处理记录实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-complaint-handling-form-tabs"
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
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.complainthandlingcode')"
                name="complaintHandlingCode"
              >
                <a-input
                  v-model:value="formState.complaintHandlingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complainthandlingcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerComplaintHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.complaintid')"
                name="complaintId"
              >
                <a-input
                  v-model:value="formState.complaintId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.complaintCode')"
                name="complaintCode"
              >
                <a-input
                  v-model:value="formState.complaintCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintCode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.complaintitemid')"
                name="complaintItemId"
              >
                <a-input
                  v-model:value="formState.complaintItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintitemid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.handlingstage')"
                name="handlingStage"
              >
                <a-input-number
                  v-model:value="formState.handlingStage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingstage') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.handlingmethod')"
                name="handlingMethod"
              >
                <a-input-number
                  v-model:value="formState.handlingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingmethod') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customercomplainthandling.handlingdescription')"
                name="handlingDescription"
              >
                <a-textarea
                  v-model:value="formState.handlingDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customercomplainthandling.handlingdescription') })"
                  :rows="2"
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
                :label="t('entity.customercomplainthandling.causeanalysis')"
                name="causeAnalysis"
              >
                <a-input
                  v-model:value="formState.causeAnalysis"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.causeanalysis') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.correctiveaction')"
                name="correctiveAction"
              >
                <a-input
                  v-model:value="formState.correctiveAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.correctiveaction') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.preventiveaction')"
                name="preventiveAction"
              >
                <a-input
                  v-model:value="formState.preventiveAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.preventiveaction') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.responsibledept')"
                name="responsibleDept"
              >
                <a-input
                  v-model:value="formState.responsibleDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.responsibledept') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.responsibleby')"
                name="responsibleBy"
              >
                <a-input
                  v-model:value="formState.responsibleBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.responsibleby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.handlerby')"
                name="handlerBy"
              >
                <a-input
                  v-model:value="formState.handlerBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlerby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.handlingat')"
                name="handlingAt"
              >
                <a-date-picker
                  v-model:value="formState.handlingAt"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingat') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.plannedcompletiondate')"
                name="plannedCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedCompletionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.plannedcompletiondate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.actualcompletiondate')"
                name="actualCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.actualCompletionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.actualcompletiondate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.handlingstatus')"
                name="handlingStatus"
              >
                <a-input-number
                  v-model:value="formState.handlingStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingstatus') })"
                  style="width: 100%"
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
                :label="t('entity.customercomplainthandling.handlingcost')"
                name="handlingCost"
              >
                <a-input-number
                  v-model:value="formState.handlingCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customercomplainthandling.customerfeedback')"
                name="customerFeedback"
              >
                <a-input
                  v-model:value="formState.customerFeedback"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.customerfeedback') })"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customercomplainthandling.customersatisfaction')"
                name="customerSatisfaction"
              >
                <a-input-number
                  v-model:value="formState.customerSatisfaction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.customersatisfaction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplainthandling.filename')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.filename') })"
                  show-count
                  :maxlength="200"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customercomplainthandling.accessurl')"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.accessurl') })"
                  show-count
                  :maxlength="1000"
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
                    <span>{{ t('common.page.entity.extfield') }}</span>
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
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客诉处理记录实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint-handling/components
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import type { CustomerComplaintHandlingCreate } from '@/types/logistics/quality/complaint/customerComplaintHandling.types'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","complaintHandlingCode","complaintId","complaintCode","complaintItemId","handlingStage","handlingMethod","handlingDescription","causeAnalysis","correctiveAction","preventiveAction","responsibleDept","responsibleBy","handlerBy","handlingAt","plannedCompletionDate","actualCompletionDate","handlingStatus","handlingCost","customerFeedback","customerSatisfaction","fileName","accessUrl","extField","remark"]

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

onMounted(() => {
  void loadTaktFileUploadBasePolicy().then((policy) => {
    taktFileAccept.value = buildTaktFileAcceptAttribute(policy)
    taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
  })
})

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerComplaintHandlingCreate & { customerComplaintHandlingId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerComplaintHandlingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerComplaintHandlingId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      syncFilesFileListFromFormState()
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.customerComplaintHandlingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  complaintHandlingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complainthandlingcode') }),
      trigger: 'blur'
    }
  ],
  complaintId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintid') }),
      trigger: 'blur'
    }
  ],
  complaintCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintCode') }),
      trigger: 'blur'
    }
  ],
  handlingStage: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingstage') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingstage') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingmethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingdescription') }),
      trigger: 'blur'
    }
  ],
  handlingStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.handlingstatus') }))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('handlingStage' in payload) {
    const rawhandlingStage = payload.handlingStage
    payload.handlingStage = typeof rawhandlingStage === 'number' ? rawhandlingStage : Number(rawhandlingStage)
  }
  if ('handlingMethod' in payload) {
    const rawhandlingMethod = payload.handlingMethod
    payload.handlingMethod = typeof rawhandlingMethod === 'number' ? rawhandlingMethod : Number(rawhandlingMethod)
  }
  if ('handlingStatus' in payload) {
    const rawhandlingStatus = payload.handlingStatus
    payload.handlingStatus = typeof rawhandlingStatus === 'number' ? rawhandlingStatus : Number(rawhandlingStatus)
  }
  if ('handlingCost' in payload) {
    const rawhandlingCost = payload.handlingCost
    payload.handlingCost = typeof rawhandlingCost === 'number' ? rawhandlingCost : Number(rawhandlingCost)
  }
  if ('customerSatisfaction' in payload) {
    const rawcustomerSatisfaction = payload.customerSatisfaction
    payload.customerSatisfaction = typeof rawcustomerSatisfaction === 'number' ? rawcustomerSatisfaction : Number(rawcustomerSatisfaction)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerComplaintHandlingId)

  activeTab.value = 'tab-0'
  filesFileList.value = []
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
