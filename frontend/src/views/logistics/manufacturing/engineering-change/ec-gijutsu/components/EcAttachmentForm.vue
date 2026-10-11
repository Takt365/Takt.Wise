<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：EcAttachmentForm.vue -->
<!-- 功能描述：设变子表 ecAttachment 独立 CRUD 弹窗表单；集成 takt-upload-file 上传至 TaktFile 后回写附件字段；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-attachment-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-attachment-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="ai.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('tenantCode') })"
                  show-count
                  :maxlength="3"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('companyCode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('cultureCode') })"
                  show-count
                  :maxlength="5"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('plantCode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('ecCode')"
                name="ecCode"
              >
                <a-input
                  v-model:value="formState.ecCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('ecCode') })"
                  show-count
                  :maxlength="10"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('lineNumber') })"
                  style="width: 100%"
                  :min="0"
                  :disabled="loading || fileUploading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('attachmentType')"
                name="attachmentType"
              >
                <TaktSelect
                  v-model:value="formState.attachmentType"
                  dict-type="logistics_manufacturing_ec_attachment_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: ai.label('attachmentType') })"
                  allow-clear
                  class="w-full"
                  :disabled="loading || fileUploading"
                  @change="handleAttachmentTypeChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('docCode')"
                name="docCode"
              >
                <a-input
                  v-model:value="formState.docCode"
                  :placeholder="docCodePlaceholder"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="loading || fileUploading || isDocCodeLocked"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="ai.label('fileName')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('fileName') })"
                  show-count
                  :maxlength="200"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="ai.label('accessUrl')"
                name="accessUrl"
              >
                <TaktUploadFile
                  tabs-type="files"
                  :files-auto-upload="true"
                  :files-multiple="false"
                  :files-max-count="1"
                  :files-disabled="loading || fileUploading"
                  :files-max-size="taktFileMaxSizeMb"
                  :files-accept="taktFileAccept"
                  :files-hint="t(`${ATTACHMENT_UPLOAD_I18N}.hint`, { max: taktFileMaxSizeMb })"
                  :files-before-upload="handleFilesBeforeUpload"
                  :files-custom-request="handleFilesCustomRequest"
                  v-model:files-file-list="filesFileList"
                  @files:remove="handleFileRemove"
                />
                <a-input
                  v-if="formState.accessUrl"
                  v-model:value="formState.accessUrl"
                  class="mt-2"
                  :placeholder="t('common.page.form.placeholder.required', { field: ai.label('accessUrl') })"
                  show-count
                  :maxlength="500"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="ai.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: ai.label('remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                  :disabled="loading || fileUploading"
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
 * 设变子表 ecAttachment 维护表单 · takt-upload-file 上传至 TaktFile 后自动回填 fileName / accessUrl
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { useI18n } from 'vue-i18n'
import { message, Upload } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcAttachmentCreate } from '@/types/logistics/manufacturing/engineering-change/ecAttachment.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { getFileById, getFileList } from '@/api/foundation/file.api'
import { getEcAttachmentList } from '@/api/logistics/manufacturing/engineering-change/ecAttachment.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import { buildEcAttachmentFileUploadMeta } from '@/utils/taktEcAttachmentStorage'
import {
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
import {
  buildEcAttachmentFileName,
  getEcAttachmentDocCodeHintKey,
  isEcAttachmentDocCodeLockedToEcCode,
  isEcAttachmentPdfFileName,
  isValidEcAttachmentDocCode,
} from '@/utils/taktEcAttachmentDocCode'
import { useEcAttachmentI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcAttachmentI18n'

/** i18n 翻译函数 */
const { t } = useI18n()
const ai = useEcAttachmentI18n()
/** 附件 DocCode 文案前缀 */
const DOC_CODE_I18N = 'logistics.manufacturing.engineering-change.ec-gijutsu.page.attachment.docCode'
/** 附件文件名称文案前缀 */
const FILE_NAME_I18N = 'logistics.manufacturing.engineering-change.ec-gijutsu.page.attachment.fileName'
/** 附件上传文案前缀 */
const ATTACHMENT_UPLOAD_I18N = 'logistics.manufacturing.engineering-change.ec-gijutsu.page.attachment.upload'
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode', 'companyCode', 'cultureCode', 'plantCode', 'ecCode', 'lineNumber', 'attachmentType', 'docCode', 'fileName', 'accessUrl', 'remark']

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcAttachmentCreate & { ecAttachmentId?: string; __rowKey?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /**
   * 同主表已有附件的文件编码（不含当前编辑行），用于前端查重
   */
  siblingDocCodes?: string[]
  /**
   * 同主表已有附件的文件名称（不含当前编辑行），用于前端查重
   */
  siblingFileNames?: string[]
  /**
   * 主表当前设变号码（实时）；类型为 EC 时文件编码强制等于此值
   */
  currentEcCode?: string
  /** 主表当前工厂代码（新增态回填） */
  currentPlantCode?: string
  /** 主表当前区域文化（新增态回填） */
  currentCultureCode?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  siblingDocCodes: () => [],
  siblingFileNames: () => [],
  currentEcCode: '',
  currentPlantCode: '',
  currentCultureCode: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 文件上传 loading */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 同一重复提示键只弹一次（编码输入防连发） */
const lastDuplicateToastKey = ref('')
/** 上传 accept（仅 PDF） */
const taktFileAccept = ref('.pdf,application/pdf')
/** 上传体积上限 MB（后端策略） */
const taktFileMaxSizeMb = ref(500)

/**
 * 上下文隔离字段：租户 / 公司 / 区域文化 / 工厂
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
    target.cultureCode = String(props.currentCultureCode ?? '').trim()
      || userStore.userInfo?.companyDefaultCulture
      || userStore.userInfo?.cultureCode
      || ''
  }
  if (formFields.includes('plantCode') && (force || !target.plantCode)) {
    target.plantCode = String(props.currentPlantCode ?? '').trim()
      || tenantStore.currentCompanyRelatedPlant
      || ''
  }
}

/** 表单字段默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (target.lineNumber === undefined || target.lineNumber === null || target.lineNumber === '') {
    target.lineNumber = 10
  }
  if (!String(target.attachmentType ?? '').trim()) {
    target.attachmentType = 'EC'
  }
  syncEcCodeOntoTarget(target)
  syncDocCodeIfEcType(target)
}

/**
 * 解析当前设变号码（主表 prop > 表单 ecCode/ecNo）
 * @returns 去空格后的设变单号
 */
function resolveCurrentEcCode(): string {
  const fromProp = String(props.currentEcCode ?? '').trim()
  if (fromProp) {
    return fromProp
  }
  return String(formState.ecCode ?? formState.ecNo ?? '').trim()
}

/**
 * 将当前设变号码写入目标对象的 ecCode
 * @param target 表单数据
 */
function syncEcCodeOntoTarget(target: Record<string, unknown>) {
  const code = String(props.currentEcCode ?? target.ecCode ?? target.ecNo ?? '').trim()
  if (code) {
    target.ecCode = code
  }
}

/**
 * 类型为 EC 时，文件编码强制等于当前设变号码
 * @param target 表单数据（默认 formState）
 */
function syncDocCodeIfEcType(target: Record<string, unknown> = formState) {
  const type = String(target.attachmentType ?? '').trim()
  if (!isEcAttachmentDocCodeLockedToEcCode(type)) {
    return
  }
  const code = String(props.currentEcCode ?? target.ecCode ?? target.ecNo ?? '').trim()
  target.ecCode = code
  target.docCode = code
}

/**
 * 文件类别变更：EC 自动赋文件编码；离开 EC 时若仍是设变号则清空
 * @param type 新文件类别（TaktSelect change 首参）
 */
function handleAttachmentTypeChange(type: string | number | (string | number)[] | null | undefined) {
  const nextType = Array.isArray(type)
    ? String(type[0] ?? '')
    : (type == null ? '' : String(type))
  formState.attachmentType = nextType
  if (isEcAttachmentDocCodeLockedToEcCode(nextType)) {
    syncDocCodeIfEcType(formState)
    syncFileNameFromDocCode()
    const dup = getLocalDuplicateMessage(resolveDocCode(), String(formState.fileName ?? '').trim())
    if (dup) {
      promptDuplicateNow(dup, `docCode:${resolveDocCode()}`)
    } else {
      lastDuplicateToastKey.value = ''
      formRef.value?.clearValidate(['docCode', 'fileName'])
    }
    return
  }
  const ec = resolveCurrentEcCode()
  if (ec && String(formState.docCode ?? '').trim() === ec) {
    formState.docCode = ''
  }
  formRef.value?.validateFields(['docCode']).catch(() => undefined)
}

/**
 * 解析当前文件编码（EC 类型锁定为设变单号）
 * @returns {string} 去空格后的文件编码
 */
function resolveDocCode(): string {
  const attachmentType = String(formState.attachmentType ?? '').trim()
  const ecCode = resolveCurrentEcCode() || String(formState.ecCode ?? '').trim()
  if (isEcAttachmentDocCodeLockedToEcCode(attachmentType)) {
    return ecCode
  }
  return String(formState.docCode ?? '').trim()
}

/**
 * 规范化文件名称（去空格、小写）便于查重
 * @param value 文件名称或编码
 * @returns 规范化后的字符串
 */
function normalizeAttachmentName(value: unknown): string {
  return String(value ?? '').trim().toLowerCase()
}

/**
 * 当前编辑行以外的文件编码是否已占用
 * @param docCode 文件编码
 * @returns 是否重复
 */
function isSiblingDocCodeDuplicate(docCode: string): boolean {
  const code = String(docCode ?? '').trim()
  if (!code) {
    return false
  }
  return (props.siblingDocCodes ?? [])
    .map((x) => String(x ?? '').trim())
    .filter(Boolean)
    .includes(code)
}

/**
 * 当前编辑行以外的文件名称是否已占用（忽略大小写）
 * @param fileName 文件名称（DocCode + 扩展名）
 * @returns 是否重复
 */
function isSiblingFileNameDuplicate(fileName: string): boolean {
  const name = normalizeAttachmentName(fileName)
  if (!name) {
    return false
  }
  return (props.siblingFileNames ?? [])
    .map((x) => normalizeAttachmentName(x))
    .filter(Boolean)
    .includes(name)
}

/**
 * 本地同单附件重复提示（文件编码或文件名称）
 * @param docCode 文件编码
 * @param fileName 文件名称（可选）
 * @returns 提示文案；无重复时为空串
 */
function getLocalDuplicateMessage(docCode: string, fileName?: string): string {
  const name = String(fileName ?? '').trim()
  if (name && isSiblingFileNameDuplicate(name)) {
    return t(`${FILE_NAME_I18N}.duplicate`, { name })
  }
  const code = String(docCode ?? '').trim()
  if (code && isSiblingDocCodeDuplicate(code)) {
    return name
      ? t(`${FILE_NAME_I18N}.duplicate`, { name })
      : t(`${DOC_CODE_I18N}.duplicate`, { code })
  }
  return ''
}

/**
 * 查询同设变下附件及当日原始文件名是否重复（唯一键：EcGijutsuId+DocCode+LineNumber；跨设变允许同文件编码）
 * @param docCode 文件编码
 * @param fileName 目标文件名称
 * @param originalName 上传原始文件名
 * @returns 提示文案；无重复时为空串
 */
async function findServerDuplicateMessage(
  docCode: string,
  fileName: string,
  originalName: string,
): Promise<string> {
  const editingId = String(props.formData?.ecAttachmentId ?? '')
  const currentEcId = String(props.masterId || formState.ecGijutsuId || props.formData?.ecGijutsuId || '').trim()
  const currentLine = Number(formState.lineNumber ?? props.formData?.lineNumber ?? 0)
  const code = String(docCode ?? '').trim()
  const name = String(fileName ?? '').trim()
  try {
    const [byCode, byName] = await Promise.all([
      code
        ? getEcAttachmentList({
            docCode: code,
            ...(currentEcId ? { ecGijutsuId: currentEcId } : {}),
            pageIndex: 1,
            pageSize: 50,
          })
        : Promise.resolve({ data: [] }),
      name
        ? getEcAttachmentList({ fileName: name, pageIndex: 1, pageSize: 50 })
        : Promise.resolve({ data: [] }),
    ])
    const rows = [...(byCode.data ?? []), ...(byName.data ?? [])]
    const hit = rows.find((row) => {
      const id = String(row.ecAttachmentId ?? '')
      if (editingId && id === editingId) {
        return false
      }
      const rowEcId = String(row.ecGijutsuId ?? '').trim()
      if (code && String(row.docCode ?? '').trim() === code) {
        if (!currentEcId || rowEcId !== currentEcId) {
          return false
        }
        const rowLine = Number(row.lineNumber ?? 0)
        if (currentLine > 0 && rowLine > 0 && rowLine !== currentLine) {
          return false
        }
        return true
      }
      return Boolean(name) && normalizeAttachmentName(row.fileName) === normalizeAttachmentName(name)
    })
    if (hit) {
      const hitName = String(hit.fileName ?? '').trim() || name || code
      return t(`${FILE_NAME_I18N}.duplicate`, { name: hitName })
    }
  } catch {
    // 列表查询失败不阻断选择，最终以上传/保存接口为准
  }
  const original = String(originalName ?? '').trim()
  if (!original) {
    return ''
  }
  try {
    const dayStart = new Date()
    dayStart.setHours(0, 0, 0, 0)
    const files = await getFileList({
      fileOriginalName: original,
      createdAtStart: dayStart.toISOString(),
      pageIndex: 1,
      pageSize: 50,
    })
    const originalHit = (files.data ?? []).some((item) => String(item.fileOriginalName ?? '').trim() === original)
    if (originalHit) {
      return t('validation.file.upload.duplicate.original.name.today', { fileName: original })
    }
  } catch {
    // 文件库查询失败不阻断选择
  }
  return ''
}

/**
 * 立即提示重复并刷新编码/名称校验
 * @param messageText 提示文案
 * @param onceKey 同一键只弹一次（编码输入 watch 防连发）；选择文件时不传
 */
function promptDuplicateNow(messageText: string, onceKey?: string) {
  if (!messageText) {
    return
  }
  if (onceKey && lastDuplicateToastKey.value === onceKey) {
    formRef.value?.validateFields(['docCode', 'fileName']).catch(() => undefined)
    return
  }
  if (onceKey) {
    lastDuplicateToastKey.value = onceKey
  }
  message.error(messageText)
  formRef.value?.validateFields(['docCode', 'fileName']).catch(() => undefined)
}

/**
 * 按文件编码回写 fileName（保留原扩展名；仅已上传时改写，避免未上传就填满 fileName）
 */
function syncFileNameFromDocCode() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url || url === '-') {
    return
  }
  const docCode = resolveDocCode()
  if (!docCode) {
    return
  }
  const next = buildEcAttachmentFileName(docCode, formState.fileName, formState.accessUrl)
  if (next) {
    formState.fileName = next
  }
}

/**
 * 根据 accessUrl 同步 takt-upload-file 列表展示
 */
function syncUploadFileListFromState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url || url === '-') {
    filesFileList.value = []
    return
  }
  filesFileList.value = [{
    uid: '-1',
    name: String(formState.fileName ?? url.split('/').pop() ?? 'file'),
    status: 'done',
  }]
}

/**
 * 将 TaktFiles 上传结果回填至表单
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
  formState.fileName = buildEcAttachmentFileName(resolveDocCode(), file.name, result.fileName || accessUrl)
  syncUploadFileListFromState()
  formRef.value?.validateFields(['accessUrl', 'fileName']).catch(() => undefined)
}

/**
 * 选择文件后立即按文件名称/编码查重；重复则中止上传
 * @param file 待上传文件
 * @returns 通过则 true；重复或缺少编码则 LIST_IGNORE
 */
const handleFilesBeforeUpload: UploadProps['beforeUpload'] = async (file) => {
  if (props.loading || fileUploading.value) {
    return Upload.LIST_IGNORE
  }
  const originFile = ((file as { originFileObj?: File }).originFileObj ?? file) as File
  if (!isEcAttachmentPdfFileName(originFile.name)) {
    message.error(t(`${ATTACHMENT_UPLOAD_I18N}.pdfOnly`))
    return Upload.LIST_IGNORE
  }
  syncDocCodeIfEcType(formState)
  const docCode = resolveDocCode()
  if (!docCode) {
    message.error(t('common.page.form.placeholder.required', { field: ai.label('docCode') }))
    return Upload.LIST_IGNORE
  }
  const targetFileName = buildEcAttachmentFileName(docCode, originFile.name)
  const localDup = getLocalDuplicateMessage(docCode, targetFileName)
  if (localDup) {
    promptDuplicateNow(localDup)
    return Upload.LIST_IGNORE
  }
  const serverDup = await findServerDuplicateMessage(docCode, targetFileName, originFile.name)
  if (serverDup) {
    promptDuplicateNow(serverDup)
    return Upload.LIST_IGNORE
  }
  return true
}

/** takt-upload-file 自定义上传：落库 TaktFile 后回写 accessUrl / fileName */
const handleFilesCustomRequest: UploadProps['customRequest'] = (options) => {
  if (props.loading || fileUploading.value) {
    options.onError?.(new Error('upload disabled'))
    return
  }
  const originFile = options.file as globalThis.File
  if (!isEcAttachmentPdfFileName(originFile.name)) {
    const pdfOnly = t(`${ATTACHMENT_UPLOAD_I18N}.pdfOnly`)
    message.error(pdfOnly)
    options.onError?.(new Error(pdfOnly))
    return
  }
  syncDocCodeIfEcType(formState)
  const docCode = resolveDocCode()
  if (!docCode) {
    message.error(t('common.page.form.placeholder.required', { field: ai.label('docCode') }))
    options.onError?.(new Error('docCode required'))
    return
  }
  const targetFileName = buildEcAttachmentFileName(docCode, originFile.name)
  const localDup = getLocalDuplicateMessage(docCode, targetFileName)
  if (localDup) {
    promptDuplicateNow(localDup)
    options.onError?.(new Error(localDup))
    return
  }
  fileUploading.value = true
  void (async () => {
    try {
      const result = await uploadTaktFileSmart(
        originFile,
        buildEcAttachmentFileUploadMeta(targetFileName),
      )
      await applyUploadResultToForm(originFile, result)
      options.onSuccess?.(result)
    } catch (error: unknown) {
      const err = error instanceof Error ? error : new Error(String(error))
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

/** 挂载后加载后端上传体积上限（类型固定 PDF，不走全局扩展名白名单） */
onMounted(async () => {
  try {
    const policy = await loadTaktFileUploadBasePolicy()
    taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
  } catch {
    // 回退默认值；实际上传校验仍由后端 API 返回
  }
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecAttachmentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecAttachmentId) {
      const next = { ...val } as Record<string, unknown>
      if (next.ExtField != null && next.extField == null) {
        next.extField = next.ExtField
        delete next.ExtField
      }
      if (!next.ecCode && next.ecNo) {
        next.ecCode = next.ecNo
      }
      if (!next.docCode && next.docNo) {
        next.docCode = next.docNo
      }
      Object.keys(formState).forEach((k) => delete formState[k])
      applyScopeDefaults(next)
      syncEcCodeOntoTarget(next)
      Object.assign(formState, next)
      syncDocCodeIfEcType(formState)
      syncFileNameFromDocCode()
      syncUploadFileListFromState()
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        if (next.ExtField != null && next.extField == null) {
          next.extField = next.ExtField
          delete next.ExtField
        }
        if (!next.ecCode && next.ecNo) {
          next.ecCode = next.ecNo
        }
        Object.assign(formState, next)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      syncDocCodeIfEcType(formState)
      syncFileNameFromDocCode()
      syncUploadFileListFromState()
      formRef.value?.clearValidate()
    }
  },
  { immediate: true, deep: true },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [
    tenantStore.tenantCode,
    tenantStore.companyCode,
    userStore.userInfo?.companyDefaultCulture,
    userStore.userInfo?.cultureCode,
    props.currentPlantCode,
    props.currentCultureCode,
  ] as const,
  () => {
    const isCreate = !props.formData?.ecAttachmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 文件类别为设变(EC)时锁定 DocCode=设变单号 */
const isDocCodeLocked = computed(() => isEcAttachmentDocCodeLockedToEcCode(formState.attachmentType))

/** 按文件类别动态占位提示 */
const docCodePlaceholder = computed(() => {
  const hintKey = getEcAttachmentDocCodeHintKey(formState.attachmentType)
  return t(`${DOC_CODE_I18N}.hint.${hintKey}`)
})

/** 主表设变号码变化时：同步 ecCode；EC 类型同步文件编码 */
watch(
  () => props.currentEcCode,
  () => {
    const code = resolveCurrentEcCode()
    if (code) {
      formState.ecCode = code
    }
    syncDocCodeIfEcType(formState)
  },
)

/** 设变单号变化时，EC 类型同步文件编码 */
watch(
  () => formState.ecCode,
  () => {
    syncDocCodeIfEcType(formState)
  },
)

/** 文件编码变化时，已上传文件的展示名跟随编码；重复立即提示 */
watch(
  () => formState.docCode,
  (value) => {
    if (String(formState.accessUrl ?? '').trim() && String(formState.accessUrl ?? '').trim() !== '-') {
      syncFileNameFromDocCode()
      syncUploadFileListFromState()
    }
    const code = String(value ?? '').trim()
    if (!code) {
      return
    }
    const nextFileName = buildEcAttachmentFileName(code, formState.fileName, formState.accessUrl)
    const dup = getLocalDuplicateMessage(code, nextFileName)
    if (dup) {
      promptDuplicateNow(dup, `docCode:${code}`)
    } else {
      lastDuplicateToastKey.value = ''
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ecCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: ai.label('ecCode') }),
      trigger: 'blur',
    },
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: ai.label('lineNumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: ai.label('lineNumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  attachmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: ai.label('attachmentType') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  docCode: [
    {
      validator: async (_rule, value) => {
        const code = String(value ?? '').trim()
        const type = String(formState.attachmentType ?? '').trim()
        const ec = String(formState.ecCode ?? '').trim()
        if (!code) {
          return Promise.reject(t('common.page.form.placeholder.required', { field: ai.label('docCode') }))
        }
        if (!isValidEcAttachmentDocCode(type, code, ec)) {
          const hintKey = getEcAttachmentDocCodeHintKey(type)
          return Promise.reject(t(`${DOC_CODE_I18N}.formatInvalid`, {
            hint: t(`${DOC_CODE_I18N}.hint.${hintKey}`),
          }))
        }
        const siblings = (props.siblingDocCodes ?? [])
          .map((x) => String(x ?? '').trim())
          .filter(Boolean)
        if (siblings.includes(code)) {
          return Promise.reject(t(`${DOC_CODE_I18N}.duplicate`, { code }))
        }
        return Promise.resolve()
      },
      trigger: ['blur', 'change'],
    },
  ],
  fileName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: ai.label('fileName') }),
      trigger: 'change',
    },
    {
      validator: async (_rule, value) => {
        const name = String(value ?? '').trim()
        if (!name) {
          return Promise.resolve()
        }
        if (!isEcAttachmentPdfFileName(name)) {
          return Promise.reject(t(`${ATTACHMENT_UPLOAD_I18N}.pdfOnly`))
        }
        const code = resolveDocCode()
        const dup = getLocalDuplicateMessage(code, name)
        if (dup) {
          return Promise.reject(dup)
        }
        return Promise.resolve()
      },
      trigger: 'change',
    },
  ],
  accessUrl: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: ai.label('accessUrl') }),
      trigger: 'change',
    },
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 ecGijutsuId） */
function getValues(): Record<string, any> {
  syncDocCodeIfEcType(formState)
  const ecCode = resolveCurrentEcCode() || String(formState.ecCode ?? '').trim()
  const attachmentType = String(formState.attachmentType ?? '').trim()
  const docCode = isEcAttachmentDocCodeLockedToEcCode(attachmentType)
    ? ecCode
    : String(formState.docCode ?? '').trim()
  const payload: Record<string, unknown> = {
    tenantCode: formState.tenantCode,
    companyCode: formState.companyCode,
    cultureCode: formState.cultureCode,
    plantCode: formState.plantCode,
    ecGijutsuId: props.masterId,
    ecCode,
    lineNumber: typeof formState.lineNumber === 'number' ? formState.lineNumber : Number(formState.lineNumber),
    attachmentType,
    docCode,
    fileName: buildEcAttachmentFileName(docCode, formState.fileName, formState.accessUrl),
    accessUrl: String(formState.accessUrl ?? '').trim(),
    extField: formState.extField,
    remark: formState.remark,
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    const next = { ...props.formData } as Record<string, unknown>
    if (next.ExtField != null && next.extField == null) {
      next.extField = next.ExtField
      delete next.ExtField
    }
    Object.assign(formState, next)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecAttachmentId)
  syncUploadFileListFromState()
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
