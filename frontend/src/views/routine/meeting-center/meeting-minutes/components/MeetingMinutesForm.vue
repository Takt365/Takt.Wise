<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/meeting-center/meeting-minutes/components -->
<!-- 文件名称：MeetingMinutesForm.vue -->
<!-- 功能描述：会后纪要维护弹窗内嵌表单；defineExpose 提供 validate、getValues、resetFields -->
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
    <div class="takt-form-content-rows-10">
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
            :label="t('entity.meetingminutes.meetingid')"
            name="meetingId"
          >
            <a-input
              v-model:value="formState.meetingId"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.meetingid') })"
              show-count
              :maxlength="20"
              allow-clear
              :disabled="!!props.masterId"
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.meetingminutes.meetingtitle')"
            name="meetingTitle"
          >
            <a-input
              v-model:value="formState.meetingTitle"
              disabled
              :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.meetingtitle') })"
              show-count
              :maxlength="200"
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.meetingminutes.linenumber')"
            name="lineNumber"
          >
            <a-input-number
              v-model:value="formState.lineNumber"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.linenumber') })"
              :min="0"
              style="width: 100%"
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item
            :label="t('entity.meetingminutes.meetingminutes')"
            name="meetingMinutes"
          >
            <TaktRichEditor
              v-model:value="formState.meetingMinutes"
              :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.meetingminutes') })"
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item
            :label="t('entity.meetingminutes.meetingsummary')"
            name="meetingSummary"
          >
            <a-textarea
              v-model:value="formState.meetingSummary"
              :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.meetingsummary') })"
              :rows="3"
              show-count
              :maxlength="2000"
              allow-clear
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.meetingminutes.recorderid')"
            name="recorderId"
          >
            <TaktSelect
              v-model:value="formState.recorderId"
              api-url="TaktUsers/options"
              allow-clear
              :disabled="!!loading"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.meetingminutes.recorderid') })"
              @change="handleRecorderChange"
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.meetingminutes.recordername')"
            name="recorderName"
          >
            <a-input
              v-model:value="formState.recorderName"
              disabled
              :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.recordername') })"
              show-count
              :maxlength="40"
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.meetingminutes.filename')"
            name="fileName"
          >
            <a-input
              v-model:value="formState.fileName"
              disabled
              :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.filename') })"
              show-count
              :maxlength="200"
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item
            :label="t('entity.meetingminutes.accessurl')"
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
              :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.accessurl') })"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会后纪要维护表单
 * @module views/routine/meeting-center/meeting-minutes/components
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import type { MeetingMinutesCreate } from '@/types/routine/meeting-center/meetingMinutes.types'
import { RiQuestionLine } from '@remixicon/vue'
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
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = [
  'tenantCode', 'companyCode', 'cultureCode', 'plantCode',
  'meetingId', 'meetingTitle', 'lineNumber',
  'meetingMinutes', 'meetingSummary',
  'recorderId', 'recorderName',
  'fileName', 'accessUrl',
  'extField', 'remark',
]

/**
 * 上下文隔离字段：租户 / 公司 / 语言 / 工厂
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
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

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MeetingMinutesCreate & { meetingMinutesId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余标题等） */
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
/** 文件上传中 */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传 accept */
const taktFileAccept = ref('')
/** 上传体积上限 MB */
const taktFileMaxSizeMb = ref(500)

/** 表单字段默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (target.lineNumber === undefined || target.lineNumber === null || target.lineNumber === '') {
    target.lineNumber = 10
  }
}

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
 * 将 TaktFile 上传结果回填至表单
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

/**
 * 记录人下拉变更：回填记录员姓名
 * @param value 选中用户 Id
 * @param option 选项（含 label）
 */
function handleRecorderChange(
  value: string | number | (string | number)[] | undefined,
  option: { label?: string; dictLabel?: string } | { label?: string; dictLabel?: string }[] | null,
) {
  if (value === undefined || value === null || value === '') {
    formState.recorderName = ''
    return
  }
  const opt = Array.isArray(option) ? option[0] : option
  formState.recorderName = String(opt?.label ?? opt?.dictLabel ?? '').trim()
}

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 从主表快照回填 meetingTitle */
function applyMasterRowDefaults() {
  if (props.masterId) {
    formState.meetingId = props.masterId
  }
  const masterRow = props.masterRow
  if (!masterRow) {
    return
  }
  const masterTitle = masterRow.meetingTitle ?? masterRow.MeetingTitle
  if (masterTitle != null && masterTitle !== '') {
    formState.meetingTitle = masterTitle
  }
  const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
  if (masterPlant != null && masterPlant !== '' && !formState.plantCode) {
    formState.plantCode = masterPlant
  }
  const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
  if (masterCulture != null && masterCulture !== '' && !formState.cultureCode) {
    formState.cultureCode = masterCulture
  }
}

/** 编辑态灌入 formData；新增态恢复默认值 */
watch(
  () => props.formData,
  (val) => {
    Object.keys(formState).forEach((k) => delete formState[k])
    if (val?.meetingMinutesId) {
      const next = { ...val } as Record<string, unknown>
      applyScopeDefaults(next)
      Object.assign(formState, next)
    } else {
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
    }
    applyMasterRowDefaults()
    syncFilesFileListFromFormState()
    formRef.value?.clearValidate()
  },
  { immediate: true },
)

/** 主表切换时同步外键与冗余标题 */
watch(
  () => [props.masterId, props.masterRow] as const,
  () => {
    applyMasterRowDefaults()
  },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.meetingMinutesId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 挂载后加载上传策略 */
onMounted(() => {
  void (async () => {
    const policy = await loadTaktFileUploadBasePolicy()
    taktFileAccept.value = buildTaktFileAcceptAttribute(policy)
    taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
  })()
  syncFilesFileListFromFormState()
})

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  meetingId: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.meetingid') }),
    trigger: 'blur',
  }],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.meetingminutes.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.meetingminutes.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
}))

/** 校验表单（失败 throw） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const raw = payload.lineNumber
    payload.lineNumber = typeof raw === 'number' ? raw : Number(raw)
  }
  if (props.formData?.meetingMinutesId) {
    payload.meetingMinutesId = props.formData.meetingMinutesId
  }
  if (props.masterId) {
    payload.meetingId = props.masterId
  }
  applyMasterRowDefaults()
  if (formState.meetingTitle) {
    payload.meetingTitle = formState.meetingTitle
  }
  return payload
}

/** 重置表单 */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.meetingMinutesId)
  applyMasterRowDefaults()
  syncFilesFileListFromFormState()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
