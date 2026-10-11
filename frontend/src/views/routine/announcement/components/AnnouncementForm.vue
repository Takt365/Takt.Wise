<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/announcement/components -->
<!-- 文件名称：AnnouncementForm.vue -->
<!-- 功能描述：公告通知实体 用于发布系统公告、通知等信息维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="announcement-form-tabs"
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
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '公告通知' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.announcementId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('announcementCode')"
                name="announcementCode"
              >
                <a-input
                  v-model:value="formState.announcementCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('numberingRuleCode')"
                name="numberingRuleCode"
              >
                <a-input
                  v-model:value="formState.numberingRuleCode"
                  :placeholder="pi.ph('numberingRuleCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.announcementId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('announcementTitle')"
                name="announcementTitle"
              >
                <a-input
                  v-model:value="formState.announcementTitle"
                  :placeholder="pi.ph('announcementTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('announcementType')"
                name="announcementType"
              >
                <TaktSelect
                  v-model:value="formState.announcementType"
                  dict-type="sys_announcement_category"
                  :placeholder="pi.ph('announcementType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('content')"
                name="content"
              >
                <TaktRichEditor
                  v-model:value="formState.content"
                  :placeholder="pi.ph('content')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('summary')"
                name="summary"
              >
                <a-input
                  v-model:value="formState.summary"
                  :placeholder="pi.ph('summary')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="pi.ph('tags')"
                  show-count
                  :maxlength="500"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('publishTime')"
                name="publishTime"
              >
                <a-date-picker
                  v-model:value="formState.publishTime"
                  :placeholder="pi.ph('publishTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isScheduled')"
                name="isScheduled"
              >
                <TaktSelect
                  v-model:value="formState.isScheduled"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isScheduled')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isTop')"
                name="isTop"
              >
                <TaktSelect
                  v-model:value="formState.isTop"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isTop')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('topPriority')"
                name="topPriority"
              >
                <a-input-number
                  v-model:value="formState.topPriority"
                  :placeholder="pi.ph('topPriority')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expireTime')"
                name="expireTime"
              >
                <a-date-picker
                  v-model:value="formState.expireTime"
                  :placeholder="pi.ph('expireTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('viewCount')"
                name="viewCount"
              >
                <a-input-number
                  v-model:value="formState.viewCount"
                  :placeholder="pi.ph('viewCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('targetScope')"
                name="targetScope"
              >
                <TaktSelect
                  v-model:value="formState.targetScope"
                  dict-type="sys_publish_scope"
                  :placeholder="pi.ph('targetScope')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('targetDepartments')"
                name="targetDepartments"
              >
                <a-input
                  v-model:value="formState.targetDepartments"
                  :placeholder="pi.ph('targetDepartments')"
                  show-count
                  :maxlength="1000"
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
                :label="pi.label('targetUsers')"
                name="targetUsers"
              >
                <a-input
                  v-model:value="formState.targetUsers"
                  :placeholder="pi.ph('targetUsers')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('announcementStatus')"
                name="announcementStatus"
              >
                <TaktSelect
                  v-model:value="formState.announcementStatus"
                  dict-type="sys_publish_status"
                  :placeholder="pi.ph('announcementStatus')"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 公告通知实体 用于发布系统公告、通知等信息维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/routine/announcement/components
 */

import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAnnouncementI18n } from '../composables/useAnnouncementI18n'

/** 实体字段 i18n */
const pi = useAnnouncementI18n()
import type { AnnouncementCreate } from '@/types/routine/announcement.types'
import { RiQuestionLine } from '@remixicon/vue'
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
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'
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
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AnnouncementCreate & { announcementId?: string }> | null
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
  announcementType: 1,
  announcementStatus: 0
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

/** 是否编辑态（编码规则取号仅新增） */
const isEditMode = computed(() => !!props.formData?.announcementId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'announcementCode',
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 announcementId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.announcementId) {
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
    if (!props.formData?.announcementId) {
      applyScopeDefaults(formState, true)
    }
  },
)

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
  announcementCode: [{
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
  announcementTitle: [
    {
      required: true,
      message: pi.ph('announcementTitle'),
      trigger: 'blur'
    }
  ],
  announcementType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('announcementType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('announcementType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  content: [
    {
      required: true,
      message: pi.ph('content'),
      trigger: 'blur'
    }
  ],
  isScheduled: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isScheduled'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isScheduled'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isTop: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isTop'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isTop'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  topPriority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('topPriority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('topPriority'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  viewCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('viewCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('viewCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  targetScope: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('targetScope'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('targetScope'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  announcementStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('announcementStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('announcementStatus'))
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
  if ('announcementType' in payload) {
    const rawannouncementType = payload.announcementType
    if (rawannouncementType === undefined || rawannouncementType === null || rawannouncementType === '') {
      delete payload.announcementType
    } else {
      const numannouncementType = typeof rawannouncementType === 'number' ? rawannouncementType : Number(rawannouncementType)
      if (Number.isFinite(numannouncementType)) payload.announcementType = numannouncementType
      else delete payload.announcementType
    }
  }
  if ('isScheduled' in payload) {
    const rawisScheduled = payload.isScheduled
    if (rawisScheduled === undefined || rawisScheduled === null || rawisScheduled === '') {
      delete payload.isScheduled
    } else {
      const numisScheduled = typeof rawisScheduled === 'number' ? rawisScheduled : Number(rawisScheduled)
      if (Number.isFinite(numisScheduled)) payload.isScheduled = numisScheduled
      else delete payload.isScheduled
    }
  }
  if ('isTop' in payload) {
    const rawisTop = payload.isTop
    if (rawisTop === undefined || rawisTop === null || rawisTop === '') {
      delete payload.isTop
    } else {
      const numisTop = typeof rawisTop === 'number' ? rawisTop : Number(rawisTop)
      if (Number.isFinite(numisTop)) payload.isTop = numisTop
      else delete payload.isTop
    }
  }
  if ('topPriority' in payload) {
    const rawtopPriority = payload.topPriority
    if (rawtopPriority === undefined || rawtopPriority === null || rawtopPriority === '') {
      delete payload.topPriority
    } else {
      const numtopPriority = typeof rawtopPriority === 'number' ? rawtopPriority : Number(rawtopPriority)
      if (Number.isFinite(numtopPriority)) payload.topPriority = numtopPriority
      else delete payload.topPriority
    }
  }
  if ('viewCount' in payload) {
    const rawviewCount = payload.viewCount
    if (rawviewCount === undefined || rawviewCount === null || rawviewCount === '') {
      delete payload.viewCount
    } else {
      const numviewCount = typeof rawviewCount === 'number' ? rawviewCount : Number(rawviewCount)
      if (Number.isFinite(numviewCount)) payload.viewCount = numviewCount
      else delete payload.viewCount
    }
  }
  if ('targetScope' in payload) {
    const rawtargetScope = payload.targetScope
    if (rawtargetScope === undefined || rawtargetScope === null || rawtargetScope === '') {
      delete payload.targetScope
    } else {
      const numtargetScope = typeof rawtargetScope === 'number' ? rawtargetScope : Number(rawtargetScope)
      if (Number.isFinite(numtargetScope)) payload.targetScope = numtargetScope
      else delete payload.targetScope
    }
  }
  if ('announcementStatus' in payload) {
    const rawannouncementStatus = payload.announcementStatus
    if (rawannouncementStatus === undefined || rawannouncementStatus === null || rawannouncementStatus === '') {
      delete payload.announcementStatus
    } else {
      const numannouncementStatus = typeof rawannouncementStatus === 'number' ? rawannouncementStatus : Number(rawannouncementStatus)
      if (Number.isFinite(numannouncementStatus)) payload.announcementStatus = numannouncementStatus
      else delete payload.announcementStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.announcementId) {
    payload.announcementId = props.formData.announcementId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.announcementId)

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
