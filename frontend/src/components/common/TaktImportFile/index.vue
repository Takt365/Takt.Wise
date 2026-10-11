<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktImportFile -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：文件导入组件，支持模板下载、上传、预览和导入结果展示 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-import-file">
    <!-- 下载模板按钮 -->
    <a-button
      v-if="showTemplate"
      :loading="templateLoading"
      :disabled="disabled"
      class="takt-import-file-template"
      @click="handleDownloadTemplate"
    >
      <template #icon>
        <download-outlined />
      </template>
      {{ t('common.tip.import.template', { entity: t(entityI18nKey) }) }}
    </a-button>

    <!-- 上传区域 -->
    <a-upload-dragger
      v-model:file-list="fileList"
      :name="name"
      :accept="accept"
      :max-count="1"
      :disabled="disabled"
      :before-upload="handleBeforeUpload"
      :custom-request="handleCustomRequest"
      :show-upload-list="showUploadList"
      v-bind="$attrs"
      class="takt-import-file-upload"
      @change="handleChange"
      @preview="handlePreview"
      @drop="handleDrop"
    >
      <p class="ant-upload-drag-icon">
        <slot name="icon">
          <inbox-outlined />
        </slot>
      </p>
      <p class="ant-upload-text">
        <slot name="text">
          {{ t('common.tip.import.upload') }}
        </slot>
      </p>
      <p class="ant-upload-hint">
        <slot name="hint">
          {{ t('common.tip.import.hint') }}
        </slot>
      </p>
    </a-upload-dragger>

    <!-- 上传数据预览 -->
    <a-modal
      v-model:open="previewVisible"
      :title="previewTitle"
      :footer="null"
      width="90%"
      centered
      class="takt-import-file-preview"
      @cancel="handleCancelPreview"
    >
      <div
        v-if="previewType === 'xlsx' || previewType === 'csv' || previewType === 'txt'"
        class="takt-import-file-preview-file"
      >
        <a-alert
          :message="getFileTypeText() + ' ' + t('common.page.button.preview')"
          :description="getPreviewDescription()"
          type="info"
          show-icon
          class="takt-import-file-preview-tip"
        />
        <div class="takt-import-file-preview-info">
          <p><strong>{{ t('components.common.page.import.filename') }}</strong>{{ previewFileName }}</p>
          <p><strong>{{ t('components.common.page.import.filesize') }}</strong>{{ previewFileSize }}</p>
          <p><strong>{{ t('components.common.page.import.filetype') }}</strong>{{ previewFileType }}</p>
        </div>
        <a-button
          type="primary"
          class="takt-import-file-preview-download"
          @click="handleDownloadPreviewFile"
        >
          <template #icon>
            <download-outlined />
          </template>
          {{ t('components.common.page.import.downloadpreviewfile') }}
        </a-button>
      </div>
      <div
        v-else
        class="takt-import-file-preview-error"
      >
        <a-result
          status="error"
          :title="t('components.common.page.import.cannotpreview')"
          :sub-title="t('components.common.page.import.previewexcelonly')"
        />
      </div>
    </a-modal>

    <!-- 导入结果 -->
    <a-alert
      v-if="importResult"
      :type="importResult.fail > 0 || (importResult.errors.length > 0 && importResult.success === 0) ? 'warning' : 'success'"
      :message="getResultMessage()"
      :description="getResultDescription()"
      show-icon
      closable
      class="takt-import-file-result"
      @close="handleCloseResult"
    />
  </div>
</template>

<script setup lang="ts">
import { InboxOutlined, DownloadOutlined } from '@ant-design/icons-vue'
import { message } from 'ant-design-vue'
import type { UploadChangeParam, UploadFile, UploadProps } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { TaktBinaryDownload } from '@/types/common.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'

const { t } = useI18n()

function isTaktBinaryDownload(v: unknown): v is TaktBinaryDownload {
  return !!v && typeof v === 'object' && v !== null && 'blob' in v && (v as TaktBinaryDownload).blob instanceof Blob
}

type FileType = 'txt' | 'csv' | 'xlsx'

interface Props {
  /** 文件类型(只能选择一种) */
  fileType?: FileType
  /** 实体显示名 i18n 键（动态 `entity.*._self` 或页面级键，供 `common.tip.import.template` 插值） */
  entityI18nKey: string
  /** 是否显示下载模板按钮 */
  showTemplate?: boolean
  /** 下载模板:返回 Blob;若 API 配置 blobWithHeaders 则返回 TaktBinaryDownload,下载名优先用服务端 Content-Disposition(与导出一致) */
  downloadTemplate?: ((sheetName?: string, fileName?: string) => Promise<Blob | TaktBinaryDownload>) | undefined
  /** 导入文件的函数 */
  importFile?: ((file: File, sheetName?: string) => Promise<TaktImportResult | Record<string, unknown>>) | undefined
  /** 上传的文件字段名 */
  name?: string
  /** 是否禁用 */
  disabled?: boolean
  /** 是否显示上传列表 */
  showUploadList?: boolean | UploadProps['showUploadList']
  /** 工作表名称(仅 xlsx 格式需要) */
  sheetName?: string | undefined
  /** 模板文件名(仅名称,不含 .xlsx;后端自动拼接 名称_时间戳.xlsx) */
  templateFileName?: string | undefined
  /** 文件大小限制(MB) */
  maxSize?: number
  /** 最大记录数限制(默认 1000 条) */
  maxRows?: number
}

const props = withDefaults(defineProps<Props>(), {
  fileType: 'xlsx',
  showTemplate: true,
  name: 'file',
  disabled: false,
  showUploadList: true,
  maxSize: 10,
  maxRows: 1000
})

// 根据文件类型获取 accept 属性
const accept = computed(() => {
  switch (props.fileType) {
    case 'txt':
      return '.txt'
    case 'csv':
      return '.csv'
    case 'xlsx':
      return '.xlsx'
    default:
      return '.txt,.csv,.xlsx'
  }
})

const emit = defineEmits<{
  'update:fileList': [fileList: UploadFile[]]
  'change': [info: UploadChangeParam]
  'preview': [file: UploadFile]
  'drop': [e: DragEvent]
  'success': [result: TaktImportResult]
  'error': [error: Error]
}>()

const fileList = ref<UploadFile[]>([])
const templateLoading = ref(false)
const uploading = ref(false)
const importResult = ref<TaktImportResult | null>(null)
const previewVisible = ref(false)
const previewFile = ref<File | null>(null)
const previewTitle = ref('')
const previewFileName = ref('')
const previewFileSize = ref('')
const previewFileType = ref('')

// 预览文件类型
const previewType = computed(() => {
  if (!previewFile.value) return ''
  const fileName = previewFile.value.name || ''
  if (fileName.endsWith('.xlsx')) {
    return 'xlsx'
  }
  if (fileName.endsWith('.csv')) {
    return 'csv'
  }
  if (fileName.endsWith('.txt')) {
    return 'txt'
  }
  return 'unknown'
})

// 获取预览描述
const getPreviewDescription = (): string => {
  switch (previewType.value) {
    case 'xlsx':
      return t('components.common.page.import.openwithexcel')
    case 'csv':
      return t('components.common.page.import.openwithcsv')
    case 'txt':
      return t('components.common.page.import.openwitheditor')
    default:
      return t('components.common.page.import.openwithapp')
  }
}

// 格式化文件大小
const formatFileSize = (bytes: number): string => {
  if (bytes === 0) return '0 B'
  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i]
}

// 时间戳格式：yyyyMMddHHmmss，与后端一致
const formatTimestamp = () => {
  const d = new Date()
  const y = d.getFullYear()
  const m = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  const h = String(d.getHours()).padStart(2, '0')
  const min = String(d.getMinutes()).padStart(2, '0')
  const s = String(d.getSeconds()).padStart(2, '0')
  return `${y}${m}${day}${h}${min}${s}`
}

// 下载模板：优先服务端 Content-Disposition；否则沿用本地 名称_时间戳.{fileType}
const handleDownloadTemplate = async () => {
  if (!props.downloadTemplate) {
    message.warning(t('components.common.page.import.notemplatefn'))
    return
  }

  try {
    templateLoading.value = true
    const result = await props.downloadTemplate(props.sheetName, props.templateFileName)
    const blob = isTaktBinaryDownload(result) ? result.blob : result
    const baseName =
      (props.templateFileName && !props.templateFileName.endsWith(`.${props.fileType}`)
        ? props.templateFileName
        : undefined) || t('common.page.button.template')
    const fallbackStamped = `${baseName}_${formatTimestamp()}`
    const fileName = isTaktBinaryDownload(result)
      ? resolveExportDownloadFileName({
          contentDisposition: result.contentDisposition ?? null,
          contentType: result.contentType ?? null,
          fallbackBase: fallbackStamped
        })
      : `${fallbackStamped}.${props.fileType}`

    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)

    message.success(t('components.common.page.import.templatedownloadsuccess'))
  } catch (error: unknown) {
    console.error('[TaktImportFile] 下载模板失败:', error)
    const err = error instanceof Error ? error : new Error(String(error))
    message.error(err.message || t('components.common.page.import.templatedownloadfail'))
    emit('error', err)
  } finally {
    templateLoading.value = false
  }
}

// 验证文件类型
const validateFileType = (fileName: string): boolean => {
  const fileExtension = fileName.substring(fileName.lastIndexOf('.')).toLowerCase()
  const expectedExtension = `.${props.fileType}`
  return fileExtension === expectedExtension
}

// 获取文件类型文本
const getFileTypeText = (): string => {
  switch (props.fileType) {
    case 'txt':
      return t('components.common.page.import.filetypetxt')
    case 'csv':
      return t('components.common.page.import.filetypecsv')
    case 'xlsx':
      return t('components.common.page.import.filetypexlsx')
    default:
      return t('components.common.page.import.filetypefile')
  }
}

// 读取文本文件行数
const countTextFileRows = async (file: File): Promise<number> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = (e) => {
      try {
        const text = e.target?.result as string
        const lines = text.split(/\r?\n/).filter(line => line.trim().length > 0)
        resolve(lines.length)
      } catch (error) {
        reject(error)
      }
    }
    reader.onerror = reject
    reader.readAsText(file, 'UTF-8')
  })
}

// 读取 CSV 文件行数
const countCsvFileRows = async (file: File): Promise<number> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = (e) => {
      try {
        const text = e.target?.result as string
        const lines = text.split(/\r?\n/).filter(line => line.trim().length > 0)
        // CSV 第一行可能是标题行，所以实际数据行数可能减1
        const dataRows = lines.length > 0 ? lines.length - 1 : 0
        resolve(dataRows)
      } catch (error) {
        reject(error)
      }
    }
    reader.onerror = reject
    reader.readAsText(file, 'UTF-8')
  })
}


// 验证文件记录数
const validateFileRows = async (file: File): Promise<boolean> => {
  try {
    let rowCount = 0

    switch (props.fileType) {
      case 'txt':
        rowCount = await countTextFileRows(file)
        if (rowCount > props.maxRows) {
          message.error(t('common.data.import.row.limit.exceeded', { count: rowCount, max: props.maxRows }))
          return false
        }
        if (rowCount === 0) {
          message.warning(t('common.file.empty'))
          return false
        }
        break
      case 'csv':
        rowCount = await countCsvFileRows(file)
        if (rowCount > props.maxRows) {
          message.error(t('common.data.import.row.limit.exceeded', { count: rowCount, max: props.maxRows }))
          return false
        }
        if (rowCount === 0) {
          message.warning(t('common.file.empty'))
          return false
        }
        break
      case 'xlsx':
        // xlsx 行数上限由服务端校验（文案 common.data.import.row.limit.exceeded）
        return true
      default:
        message.error(t('components.common.page.import.unsupportedfiletype', { type: props.fileType }))
        return false
    }

    return true
  } catch (error: unknown) {
    console.error('[TaktImportFile] 验证文件记录数失败:', error)
    const err = error instanceof Error ? error : new Error(String(error))
    message.error(err.message || t('common.file.parse.error'))
    return false
  }
}

// 上传前的钩子
const handleBeforeUpload = async (file: UploadFile | File) => {
  // 获取原生 File 对象
  const originFile = (file as UploadFile).originFileObj || (file as File)
  
  // 如果设置了文件大小限制
  if (props.maxSize && originFile.size) {
    const fileSizeMB = originFile.size / 1024 / 1024
    if (fileSizeMB > props.maxSize) {
      message.error(t('common.file.too.large', { max: `${props.maxSize}MB` }))
      return false
    }
  }

  // 检查文件类型
  const fileName = originFile.name || ''
  if (!validateFileType(fileName)) {
    message.error(t('common.file.invalid.format', { formats: `.${props.fileType}` }))
    return false
  }

  // 验证文件记录数
  const isValidRows = await validateFileRows(originFile)
  if (!isValidRows) {
    return false
  }

  return true
}

// 自定义上传请求
const handleCustomRequest: NonNullable<UploadProps['customRequest']> = async (options) => {
  const { file, onSuccess, onError, onProgress } = options

  if (!props.importFile) {
    const error = new Error(t('components.common.page.import.noimportfn'))
    onError?.(error)
    message.error(t('components.common.page.import.noimportfn'))
    return
  }

  try {
    uploading.value = true
    onProgress?.({ percent: 50 })

    // 处理不同类型的 file: string | RcFile | Blob
    // string 类型通常表示远程文件URL，本地上传应为 RcFile 或 Blob
    let uploadFile: File
    if (file instanceof Blob && !(file instanceof File)) {
      // Blob 转 File
      uploadFile = new File([file], 'uploaded-file', { type: file.type })
    } else if (file instanceof File) {
      uploadFile = file
    } else {
      // string 类型或其他情况，抛出错误
      throw new Error(t('components.common.page.import.invalidfile'))
    }
    
    const raw = await props.importFile(uploadFile, props.sheetName)
    const result = normalizeImportResult(raw)

    onProgress?.({ percent: 100 })
    onSuccess?.(result)

    // 保存导入结果
    importResult.value = result

    // 触发成功事件
    emit('success', result)

    // 显示成功消息
    const toastMessage = buildImportFeedbackMessage(result)
    if (result.fail > 0 || (result.errors.length > 0 && result.success === 0)) {
      message.warning(toastMessage)
    } else {
      message.success(toastMessage)
    }
  } catch (error: unknown) {
    console.error('[TaktImportFile] 导入失败:', error)
    const err = error instanceof Error ? error : new Error(String(error))
    onError?.(err)
    message.error(err.message || t('common.feedback.import.failed'))
    emit('error', err)
  } finally {
    uploading.value = false
  }
}

// 文件状态改变时的回调
const handleChange = (info: UploadChangeParam) => {
  // 如果文件被移除，清空导入结果和预览
  if (info.file.status === 'removed') {
    importResult.value = null
    previewFile.value = null
  }

  fileList.value = info.fileList
  emit('change', info)
  emit('update:fileList', info.fileList)
}

// 预览文件
const handlePreview = (file: UploadFile) => {
  const originFile = file.originFileObj || file
  
  if (!originFile || !(originFile instanceof File)) {
    message.warning(t('components.common.page.import.cannotpreview'))
    return
  }

  previewFile.value = originFile
  previewTitle.value = t('components.common.page.import.previewtitle', { name: file.name || originFile.name || t('components.common.page.import.unknownfile') })
  previewFileName.value = file.name || originFile.name || t('components.common.page.import.unknownfile')
  previewFileSize.value = formatFileSize(originFile.size || 0)
  
  const fileName = previewFileName.value
  if (fileName.endsWith('.xlsx')) {
    previewFileType.value = t('components.common.page.import.excelworkbook')
  } else if (fileName.endsWith('.csv')) {
    previewFileType.value = t('components.common.page.import.csvfile')
  } else if (fileName.endsWith('.txt')) {
    previewFileType.value = t('components.common.page.import.filetypetxt')
  } else {
    previewFileType.value = t('components.common.page.import.unknowntype')
  }

  previewVisible.value = true
  emit('preview', file)
}

// 取消预览
const handleCancelPreview = () => {
  previewVisible.value = false
  previewFile.value = null
  previewTitle.value = ''
  previewFileName.value = ''
  previewFileSize.value = ''
  previewFileType.value = ''
}

// 下载预览文件
const handleDownloadPreviewFile = () => {
  if (!previewFile.value) {
    message.warning(t('components.common.page.import.previewnotexist'))
    return
  }

  const url = window.URL.createObjectURL(previewFile.value)
  const link = document.createElement('a')
  link.href = url
  link.download = previewFileName.value
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
  window.URL.revokeObjectURL(url)
  
  message.success(t('common.feedback.success'))
}

// 拖拽放下时的回调
const handleDrop = (e: DragEvent) => {
  console.log('[TaktImportFile] 拖拽放下:', e)
  emit('drop', e)
}

// 关闭导入结果
const handleCloseResult = () => {
  importResult.value = null
}

/**
 * 构建导入结果提示文案
 * @param result 归一化导入结果
 * @returns 用户可见提示
 */
function buildImportFeedbackMessage(result: TaktImportResult): string {
  if (result.fail > 0 || (result.errors.length > 0 && result.success === 0)) {
    return t('common.feedback.import.summary', {
      success: result.success,
      duplicate: result.duplicate,
      error: result.error,
    })
  }
  return t('common.feedback.import.success', { count: result.success })
}

// 获取结果消息
const getResultMessage = (): string => {
  if (!importResult.value) return ''
  return buildImportFeedbackMessage(importResult.value)
}

// 获取结果描述
const getResultDescription = (): string => {
  if (!importResult.value || !importResult.value.errors || importResult.value.errors.length === 0) {
    return ''
  }
  const errorCount = importResult.value.errors.length
  const displayedErrors = importResult.value.errors.slice(0, 5)
  const remaining = errorCount > 5 ? t('components.common.page.import.errorsremaining', { count: errorCount }) : ''
  return displayedErrors.join('；') + remaining
}
</script>

<style scoped lang="css">
.takt-import-file {
  display: flex;
  flex-direction: column;
  gap: 16px;

  .takt-import-file-template {
    align-self: flex-start;
  }

  .takt-import-file-upload {
    width: 100%;
  }

  .takt-import-file-preview {
    :deep(.ant-modal-body) {
      padding: 24px;
    }

    .takt-import-file-preview-file {
      .takt-import-file-preview-tip {
        margin-bottom: 16px;
      }

      .takt-import-file-preview-info {
        margin: 16px 0;
        padding: 16px;
        background: #f5f5f5;
        border-radius: 4px;

        p {
          margin: 8px 0;
          color: #666;

          strong {
            color: #333;
          }
        }
      }

      .takt-import-file-preview-download {
        width: 100%;
      }
    }

    .takt-import-file-preview-error {
      text-align: center;
    }
  }

  .takt-import-file-result {
    margin-top: 8px;
  }
}
</style>
