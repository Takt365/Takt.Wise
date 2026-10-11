<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/file/components -->
<!-- 文件名称：FileFormUpload.vue -->
<!-- 功能描述：文件新增上传表单；集成 takt-upload-file 与存储元数据；defineExpose 提供 validate、resetFields、uploadFiles -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="file-form-upload">
    <TaktUploadFile
      ref="uploadFileRef"
      tabs-type="files"
      :files-auto-upload="false"
      :files-multiple="false"
      :files-max-count="1"
      :files-disabled="loading"
      :files-max-size="taktFileMaxSizeMb"
      :files-accept="taktFileAccept"
      :files-hint="t('foundation.file.page.upload.hint', { max: taktFileMaxSizeMb })"
      :files-before-upload="handleFilesBeforeUpload"
      :files-custom-request="handleFilesCustomRequest"
      v-model:files-file-list="filesFileList"
    />
    <a-form
      ref="createMetaFormRef"
      :model="formState"
      :rules="createMetaRules"
      layout="horizontal"
      label-align="right"
      class="mt-4"
    >
      <a-row :gutter="24">
        <a-col
          v-for="field in storageSelectFields"
          :key="field.name"
          :span="24"
        >
          <a-form-item :label="t(field.labelKey)" :name="field.name">
            <a-select
              v-if="isMenuUploadPathField(field)"
              v-model:value="formState.uploadPath"
              :options="uploadPathOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t(field.labelKey) })"
              size="small"
              allow-clear
            />
            <TaktSelect
              v-else
              v-model:value="formState[field.name]"
              :dict-type="field.dictType!"
              :placeholder="t('common.page.form.placeholder.select', { field: t(field.labelKey) })"
              size="small"
            />
          </a-form-item>
        </a-col>
        <a-col v-if="showOssProviderField" :span="24">
          <a-form-item :label="t('foundation.file.page.oss.provider')" name="ossProvider">
            <TaktSelect
              v-model:value="formState.ossProvider"
              :options="ossProviderSelectOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('foundation.file.page.oss.provider') })"
              size="small"
            />
          </a-form-item>
        </a-col>
        <a-col v-if="showFtpProviderField" :span="24">
          <a-form-item :label="t('foundation.file.page.ftp.provider')" name="ftpProvider">
            <TaktSelect
              v-model:value="formState.ftpProvider"
              dict-type="sys_ftp_provider"
              :placeholder="t('common.page.form.placeholder.select', { field: t('foundation.file.page.ftp.provider') })"
              size="small"
            />
          </a-form-item>
        </a-col>
        <a-col v-if="isCustomStorageNaming" :span="24">
          <a-form-item :label="t('entity.file.name')" name="fileName">
            <a-input
              v-model:value="formState.fileName"
              :placeholder="t('foundation.file.page.custom.filename.placeholder')"
              size="small"
              allow-clear
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item :label="t('entity.file.ispublic')" name="isPublic">
            <TaktSelect
              v-model:value="formState.isPublic"
              dict-type="sys_public_type"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.ispublic') })"
              size="small"
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item :label="t('entity.file.status')" name="fileStatus">
            <TaktSelect
              v-model:value="formState.fileStatus"
              dict-type="sys_normal_disable"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.status') })"
              size="small"
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item :label="t('entity.file.tags')" name="fileTags">
            <FileTagEditor v-model="formState.fileTags" :disabled="loading" />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item :label="t('common.page.entity.remark')" name="remark">
            <a-textarea
              v-model:value="formState.remark"
              :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
              :rows="2"
              size="small"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
    <TaktModal
      v-model:open="chunkUploadVisible"
      :title="t('components.common.page.upload.chunkmodaltitle')"
      :use-viewport-size="false"
      :confirm-loading="chunkUploadLoading"
      :ok-button-props="{ style: { display: 'none' } }"
      :cancel-text="t('components.common.page.upload.cancel')"
      @cancel="handleChunkUploadCancel"
    >
      <div class="flex flex-col gap-3">
        <a-typography-text>{{ chunkUploadFileName }}</a-typography-text>
        <a-progress :percent="chunkUploadPercent" :status="chunkUploadProgressStatus" />
        <a-typography-text type="secondary">{{ chunkUploadStatusText }}</a-typography-text>
        <div class="flex gap-2 justify-end">
          <a-button
            v-if="chunkUploadCanPause"
            @click="handleChunkUploadPause"
          >
            {{ t('components.common.page.upload.chunkpause') }}
          </a-button>
          <a-button
            v-if="chunkUploadCanResume"
            type="primary"
            @click="handleChunkUploadResume"
          >
            {{ t('components.common.page.upload.chunkresume') }}
          </a-button>
        </div>
      </div>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 文件新增上传表单：takt-upload-file + 存储元数据；仅新增，不支持更新
 * @module views/foundation/file/components
 */
import { message,  } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { FileUploadMeta } from '@/types/foundation/file.types'
import { uploadFile } from '@/api/foundation/file.api'
import TaktSelect from '@/components/business/TaktSelect/index.vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import {
  TaktFileChunkUploader,
  TaktFileChunkUploadStatus,
  TaktFileChunkUploadPausedError,
  shouldUseTaktFileChunkUpload,
} from '@/utils/taktFileChunkUpload'
import {
  applyFileStorageDefaults,
  buildFileStorageConfigJson,
  filterOssProviderSelectOptions,
  normalizeOssProvider,
  resolveFileCategoryPathForUpload,
} from '@/utils/taktFileStorageConfig'
import { joinFileTags } from '@/utils/taktFileTags'
import { buildDefaultFileTagsFromUploadPath } from '@/utils/taktModuleRootMenu'
import {
  TAKT_FILE_STATUS_ENABLED,
  normalizeFileStatus,
} from '@/utils/taktFileStatus'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
import FileTagEditor from '@/views/foundation/file/components/FileTagEditor.vue'
import { useMenuUploadPath } from '@/views/foundation/file/composables/useMenuUploadPath'
import { useMenuStore } from '@/stores/identity/useMenuStore'

/** 上传 accept 属性（挂载后由后端 AllowedExtensions 填充） */
const taktFileAccept = ref('')
/** 单文件最大 MB（挂载后由后端 MaxFileSizeBytes 填充） */
const taktFileMaxSizeMb = ref(500)
/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：字典缓存（OSS 提供商下拉过滤） */
const dictDataStore = useDictDataStore()
/** 一级目录菜单 → 上传路径选项 */
const { uploadPathOptions } = useMenuUploadPath()
/** 菜单树（默认标签依赖一级目录展示名） */
const menuStore = useMenuStore()
/** OSS 提供商下拉（仅后端已实现的 aliyun） */
const ossProviderSelectOptions = computed(() =>
  filterOssProviderSelectOptions(
    dictDataStore.getDictOptionsForSelect('sys_oss_provider', {
      labelField: 'dictLabel',
      valueField: 'dictValue',
    }),
  ),
)
/** 存储相关表单项（uploadPath 完全由一级菜单推导，替代已删除的 sys_upload_path） */
type StorageSelectField =
  | { name: 'uploadPath'; labelKey: 'entity.file.path'; menuUploadPath: true }
  | { name: 'storageNaming'; labelKey: 'entity.file.name'; dictType: 'sys_storage_naming' }
  | { name: 'storageType'; labelKey: 'entity.file.storagetype'; dictType: 'sys_storage_type' }

const storageSelectFields: StorageSelectField[] = [
  { name: 'uploadPath', labelKey: 'entity.file.path', menuUploadPath: true },
  { name: 'storageNaming', dictType: 'sys_storage_naming', labelKey: 'entity.file.name' },
  { name: 'storageType', dictType: 'sys_storage_type', labelKey: 'entity.file.storagetype' }]

/**
 * 是否为菜单推导的上传路径字段
 * @param field 表单项配置
 * @returns 是否 uploadPath
 */
function isMenuUploadPathField(field: StorageSelectField): field is { name: 'uploadPath'; labelKey: 'entity.file.path'; menuUploadPath: true } {
  return field.name === 'uploadPath'
}

/** 上传元数据表单模型 */
interface FileUploadFormModel {
  fileName?: string
  fileTags?: string
  isPublic?: number
  fileStatus?: number
  remark?: string
  uploadPath?: string
  storageNaming?: string | number
  storageType?: number
  ossProvider?: string
  ftpProvider?: string
}

/** 父级提交 loading，禁用表单项 */
interface Props {
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  loading: false,
})

/** 新增态元数据 a-form 实例 ref */
const createMetaFormRef = ref()
/** takt-upload-file 实例 ref */
type TaktUploadFileExpose = {
  uploadFiles: () => Promise<void>
  clearFiles: () => void
}
const uploadFileRef = ref<TaktUploadFileExpose | null>(null)
/** 待上传文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 分片上传弹窗 */
const chunkUploadVisible = ref(false)
/** 分片上传进行中 */
const chunkUploadLoading = ref(false)
/** 当前上传文件名 */
const chunkUploadFileName = ref('')
/** 分片上传进度 0-100 */
const chunkUploadPercent = ref(0)
/** 分片上传状态文案 */
const chunkUploadStatusText = ref('')
/** 分片上传器实例 */
const chunkUploaderRef = ref<TaktFileChunkUploader | null>(null)
/** 当前分片上传状态 */
const chunkUploadStatus = ref<TaktFileChunkUploadStatus>(TaktFileChunkUploadStatus.Waiting)
/** 分片上传 customRequest 回调（恢复成功后通知 Upload） */
const pendingChunkUploadOptions = ref<Parameters<NonNullable<UploadProps['customRequest']>>[0] | null>(null)
/** 表单双向绑定模型 */
const formState = reactive<Partial<FileUploadFormModel>>({})
/** 分片上传进度条状态 */
const chunkUploadProgressStatus = computed(() => {
  if (chunkUploadStatus.value === TaktFileChunkUploadStatus.Error) {
    return 'exception'
  }
  if (chunkUploadStatus.value === TaktFileChunkUploadStatus.Success) {
    return 'success'
  }
  return 'active'
})
/** 是否可暂停分片上传 */
const chunkUploadCanPause = computed(
  () => chunkUploadStatus.value === TaktFileChunkUploadStatus.Uploading
)
/** 是否可恢复分片上传 */
const chunkUploadCanResume = computed(
  () =>
    chunkUploadStatus.value === TaktFileChunkUploadStatus.Paused
    || chunkUploadStatus.value === TaktFileChunkUploadStatus.Error
)
/** 是否选中存储命名「自定义」（字典 sys_storage_naming=2） */
const isCustomStorageNaming = computed(() => coerceStorageType(formState.storageNaming) === 2)
/** 当前存储类型（字典 sys_storage_type） */
const selectedStorageType = computed(() => coerceStorageType(formState.storageType))
/** 是否展示 OSS 提供商（StorageType=1） */
const showOssProviderField = computed(() => selectedStorageType.value === 1)
/** 是否展示 FTP 提供商（StorageType=2） */
const showFtpProviderField = computed(() => selectedStorageType.value === 2)

/**
 * 新增态公开范围、文件状态默认值
 * @param target 目标对象
 * @param force 是否强制覆盖
 */
function applyFileMetaDefaults(target: Record<string, unknown>, force = false): void {
  if (force || target.isPublic == null || target.isPublic === '') {
    target.isPublic = 0
  }
  if (force || target.fileStatus == null || target.fileStatus === '') {
    target.fileStatus = TAKT_FILE_STATUS_ENABLED
  }
}

/**
 * 按当前上传路径同步默认文件标签（菜单展示名 + 路径 slug）
 */
function syncFileTagsFromUploadPath(): void {
  const uploadPath = formState.uploadPath
  if (!uploadPath?.trim()) {
    formState.fileTags = ''
    return
  }
  formState.fileTags = joinFileTags(
    buildDefaultFileTagsFromUploadPath(uploadPath, menuStore.menuList)
  )
}

/** 挂载后拉取后端上传基础策略（accept / maxSize 提示） */
onMounted(async () => {
  applyFileStorageDefaults(formState as Record<string, unknown>, true)
  applyFileMetaDefaults(formState as Record<string, unknown>, true)
  syncFileTagsFromUploadPath()
  try {
    const policy = await loadTaktFileUploadBasePolicy()
    taktFileAccept.value = buildTaktFileAcceptAttribute(policy.allowedExtensions ?? [])
    taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
  } catch {
    // 提示文案回退默认值；实际上传校验仍由后端 API 返回
  }
})

/** 切换上传路径时同步默认标签（如日常事务 + routine） */
watch(
  () => formState.uploadPath,
  () => {
    syncFileTagsFromUploadPath()
  }
)

/** 菜单加载完成后补全默认标签展示名 */
watch(
  () => menuStore.menuList,
  () => {
    if (formState.uploadPath?.trim()) {
      syncFileTagsFromUploadPath()
    }
  },
  { deep: true }
)

/** 切换存储类型时补齐默认 OSS/FTP 提供商 */
watch(
  () => formState.storageType,
  () => {
    const storageType = coerceStorageType(formState.storageType)
    if (storageType === 1) {
      formState.ossProvider = normalizeOssProvider(formState.ossProvider)
    }
    if (storageType === 2 && !formState.ftpProvider) {
      formState.ftpProvider = 'teac_cn'
    }
  },
)

/** 新增态元数据表单校验 */
const createMetaRules = computed<Record<string, Rule[]>>(() => {
  const base: Record<string, Rule[]> = {
    uploadPath: [
      {
        required: true,
        message: t('common.page.form.placeholder.select', { field: t('entity.file.path') }),
        trigger: 'change',
      }],
    storageNaming: [
      {
        required: true,
        message: t('common.page.form.placeholder.select', { field: t('entity.file.name') }),
        trigger: 'change',
      }],
    storageType: [
      {
        required: true,
        message: t('common.page.form.placeholder.select', { field: t('entity.file.storagetype') }),
        trigger: 'change',
      }],
  }
  if (showFtpProviderField.value) {
    base.ftpProvider = [
      {
        required: true,
        message: t('common.page.form.placeholder.select', { field: t('foundation.file.page.ftp.provider') }),
        trigger: 'change',
      }]
  }
  if (showOssProviderField.value) {
    base.ossProvider = [
      {
        required: true,
        message: t('common.page.form.placeholder.select', { field: t('foundation.file.page.oss.provider') }),
        trigger: 'change',
      }]
  }
  if (isCustomStorageNaming.value) {
    base.fileName = [
      {
        required: true,
        message: t('common.page.form.placeholder.required', { field: t('entity.file.name') }),
        trigger: 'blur',
      }]
  }
  return base
})

/**
 * 将表单 storageType 转为 number
 * @param value 表单值
 * @returns 有效整数或 undefined
 */
function coerceStorageType(value: unknown): number | undefined {
  if (value === undefined || value === null || value === '') {
    return undefined
  }
  const num = typeof value === 'number' ? value : Number(value)
  return Number.isFinite(num) ? Math.trunc(num) : undefined
}

/**
 * 构建 StorageConfig 载荷
 * @returns JSON 字符串或 null
 */
function buildStorageConfigPayload(): string | null {
  const storageType = coerceStorageType(formState.storageType)
  return buildFileStorageConfigJson({
    uploadPath: formState.uploadPath,
    storageNaming: formState.storageNaming,
    ossProvider: storageType === 1 ? normalizeOssProvider(formState.ossProvider) : undefined,
    ftpProvider: storageType === 2 ? (formState.ftpProvider || 'teac_cn') : undefined,
  })
}

/**
 * 构建上传元数据
 * @param uploadSourceFile 待上传文件（用于按类型推断 images/documents 等子目录）
 * @returns 上传附带业务字段
 */
function buildUploadMeta(uploadSourceFile?: File): FileUploadMeta {
  const storageType = coerceStorageType(formState.storageType)
  const storageConfig = buildStorageConfigPayload()
  const meta: FileUploadMeta = {
    fileTags: formState.fileTags || undefined,
    isPublic: formState.isPublic,
    fileStatus: normalizeFileStatus(formState.fileStatus),
    categoryPath: resolveFileCategoryPathForUpload(formState.uploadPath, uploadSourceFile),
    storageType,
    storageNaming: coerceStorageType(formState.storageNaming) ?? 0,
    storageConfig: storageConfig ?? undefined,
  }
  if (isCustomStorageNaming.value) {
    const customName = formState.fileName?.trim()
    if (customName) {
      meta.targetFileName = customName
    }
  }
  return meta
}

/**
 * 更新分片上传状态文案
 * @param status 状态
 * @param uploaded 已上传分片数
 * @param total 总分片数
 */
function updateChunkUploadStatusText(
  status: TaktFileChunkUploadStatus,
  uploaded: number,
  total: number
) {
  chunkUploadStatus.value = status
  if (status === TaktFileChunkUploadStatus.Hashing) {
    chunkUploadStatusText.value = t('components.common.page.upload.chunkhashing')
    return
  }
  if (status === TaktFileChunkUploadStatus.Merging) {
    chunkUploadStatusText.value = t('components.common.page.upload.chunkmerging')
    return
  }
  if (status === TaktFileChunkUploadStatus.Paused) {
    chunkUploadStatusText.value = t('components.common.page.upload.chunkpaused')
    return
  }
  if (total > 0) {
    chunkUploadStatusText.value = t('components.common.page.upload.chunkuploading', {
      uploaded,
      total,
    })
  }
}

/**
 * 分片上传完成回调
 * @param result 上传结果
 * @param options Upload customRequest 选项
 */
function finishChunkUploadSuccess(
  result: unknown,
  options: Parameters<NonNullable<UploadProps['customRequest']>>[0]
) {
  chunkUploadVisible.value = false
  pendingChunkUploadOptions.value = null
  options.onSuccess?.(result)
}

/**
 * 分片上传失败回调
 * @param error 错误
 * @param options Upload customRequest 选项
 */
function finishChunkUploadError(
  error: Error,
  options: Parameters<NonNullable<UploadProps['customRequest']>>[0]
) {
  if (chunkUploadStatus.value !== TaktFileChunkUploadStatus.Cancelled) {
    options.onError?.(error)
  }
  chunkUploadVisible.value = false
  pendingChunkUploadOptions.value = null
}

/**
 * 启动分片上传
 * @param file 文件
 * @param options Upload customRequest 选项
 */
async function startChunkUpload(
  file: globalThis.File,
  options: Parameters<NonNullable<UploadProps['customRequest']>>[0]
) {
  chunkUploadFileName.value = file.name
  chunkUploadPercent.value = 0
  chunkUploadVisible.value = true
  chunkUploadLoading.value = true
  pendingChunkUploadOptions.value = options
  const meta = buildUploadMeta(file)
  const uploader = new TaktFileChunkUploader(file, {
    meta,
    onProgress: (progress) => {
      chunkUploadPercent.value = progress.percent
      updateChunkUploadStatusText(
        progress.status,
        progress.uploadedChunks,
        progress.totalChunks
      )
      options.onProgress?.({ percent: progress.percent })
    },
  })
  chunkUploaderRef.value = uploader
  try {
    const result = await uploader.start()
    finishChunkUploadSuccess(result, options)
  } catch (error: unknown) {
    if (error instanceof TaktFileChunkUploadPausedError) {
      chunkUploadLoading.value = false
      return
    }
    const err = error instanceof Error ? error : new Error(String(error))
    finishChunkUploadError(err, options)
  } finally {
    if (chunkUploadStatus.value !== TaktFileChunkUploadStatus.Paused) {
      chunkUploadLoading.value = false
      chunkUploaderRef.value = null
    }
  }
}

/** takt-upload-file 自定义上传 */
const handleFilesCustomRequest: UploadProps['customRequest'] = (options) => {
  const originFile = options.file as globalThis.File
  const meta = buildUploadMeta(originFile)
  void (async () => {
    try {
      const useChunk = await shouldUseTaktFileChunkUpload(originFile.size)
      if (!useChunk) {
        const result = await uploadFile(originFile, meta)
        options.onSuccess?.(result)
        return
      }
      await startChunkUpload(originFile, options)
    } catch (error: unknown) {
      const err = error instanceof Error ? error : new Error(String(error))
      options.onError?.(err)
    }
  })()
}

/**
 * 阻止 a-upload 自动提交
 * @param _file 待选文件
 * @returns false 阻止自动上传
 */
const handleFilesBeforeUpload: UploadProps['beforeUpload'] = (_file) => false

/** 暂停分片上传 */
function handleChunkUploadPause() {
  chunkUploaderRef.value?.pause()
}

/** 恢复分片上传 */
async function handleChunkUploadResume() {
  const uploader = chunkUploaderRef.value
  const options = pendingChunkUploadOptions.value
  if (!uploader || !options) return
  chunkUploadLoading.value = true
  try {
    const result = await uploader.resume()
    finishChunkUploadSuccess(result, options)
  } catch (error: unknown) {
    if (error instanceof TaktFileChunkUploadPausedError) {
      return
    }
    const err = error instanceof Error ? error : new Error(String(error))
    finishChunkUploadError(err, options)
  } finally {
    if (chunkUploadStatus.value !== TaktFileChunkUploadStatus.Paused) {
      chunkUploadLoading.value = false
      chunkUploaderRef.value = null
    }
  }
}

/** 取消分片上传 */
async function handleChunkUploadCancel() {
  await chunkUploaderRef.value?.cancel()
  if (pendingChunkUploadOptions.value) {
    pendingChunkUploadOptions.value.onError?.(new Error('cancelled'))
  }
  chunkUploadVisible.value = false
  chunkUploaderRef.value = null
  pendingChunkUploadOptions.value = null
}

/** 执行待上传文件（新增提交时由父级调用） */
async function uploadFiles() {
  await uploadFileRef.value?.uploadFiles()
}

/**
 * 校验表单（失败 throw）
 * @returns 表单模型
 */
async function validate() {
  const hasFile = filesFileList.value.some(
    (item) => item.originFileObj && item.status !== 'removed'
  )
  if (!hasFile) {
    message.warning(
      t('common.tip.select.to.action', {
        action: t('common.page.button.upload'),
        entity: t('entity.file._self'),
      })
    )
    throw new Error('NO_FILE_SELECTED')
  }
  await createMetaFormRef.value?.validate()
  return formState
}

/** 重置表单与上传列表 */
function resetFields() {
  createMetaFormRef.value?.resetFields()
  uploadFileRef.value?.clearFiles()
  filesFileList.value = []
  Object.keys(formState).forEach((k) => delete formState[k as keyof FileUploadFormModel])
  applyFileStorageDefaults(formState as Record<string, unknown>, true)
  applyFileMetaDefaults(formState as Record<string, unknown>, true)
  syncFileTagsFromUploadPath()
  chunkUploadVisible.value = false
  chunkUploaderRef.value = null
  pendingChunkUploadOptions.value = null
}

defineExpose({ validate, resetFields, uploadFiles })
</script>