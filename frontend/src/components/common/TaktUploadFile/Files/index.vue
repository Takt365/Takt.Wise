<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktUploadFile/files -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：通用文件上传组件，支持拖拽上传、分片上传和文件验证 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-upload-files">
    <a-upload-dragger
      v-model:file-list="fileList"
      :name="name"
      :multiple="multiple"
      :action="autoUpload && action ? action : ''"
      :accept="accept || ''"
      :max-count="maxCount || 0"
      :disabled="disabled"
      :before-upload="handleBeforeUpload"
      v-bind="autoUpload && customRequest ? { customRequest } : {}"
      :show-upload-list="showUploadList"
      class="takt-upload-files-dragger"
      @change="handleChange"
      @drop="handleDrop"
      @remove="handleRemove"
      @preview="handlePreview"
    >
      <p class="ant-upload-drag-icon">
        <slot name="icon">
          <inbox-outlined />
        </slot>
      </p>
      <p class="ant-upload-text">
        <slot name="text">
          {{ text ?? t('components.common.page.upload.filestext') }}
        </slot>
      </p>
      <p class="ant-upload-hint">
        <slot name="hint">
          {{ hint ?? t('components.common.page.upload.fileshint') }}
        </slot>
      </p>
    </a-upload-dragger>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { InboxOutlined } from '@ant-design/icons-vue'
import { message, Upload } from 'ant-design-vue'
import type { UploadChangeParam, UploadFile, UploadProps } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { logger } from '@/utils/logger'

const { t } = useI18n()
type BeforeUploadFn = NonNullable<UploadProps['beforeUpload']>
type CustomRequestOptions = Parameters<NonNullable<UploadProps['customRequest']>>[0]
type UploadProgress = { percent?: number }

interface Props {
  /** 文件列表 */
  modelValue?: UploadFile[] | undefined
  /** 上传的文件字段名 */
  name?: string
  /** 是否支持多选 */
  multiple?: boolean
  /** 上传的地址 */
  action?: string | undefined
  /** 接受上传的文件类型 */
  accept?: string | undefined
  /** 最大上传数量 */
  maxCount?: number | undefined
  /** 是否禁用 */
  disabled?: boolean
  /** 是否显示上传列表 */
  showUploadList?: boolean | UploadProps['showUploadList']
  /** 上传前的钩子 */
  beforeUpload?: UploadProps['beforeUpload'] | undefined
  /** 自定义上传请求 */
  customRequest?: UploadProps['customRequest'] | undefined
  /** 提示文本 */
  text?: string | undefined
  /** 提示说明 */
  hint?: string | undefined
  /** 文件大小限制(MB) */
  maxSize?: number | undefined
  /** 是否启用分片上传 */
  enableChunked?: boolean
  /** 分片大小(MB,默认 2) */
  chunkSize?: number
  /** 是否自动上传(true=自动上传,false=手动上传,默认 true) */
  autoUpload?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: undefined,
  name: 'file',
  multiple: true,
  action: '',
  accept: undefined,
  maxCount: undefined,
  disabled: false,
  showUploadList: true,
  text: undefined,
  hint: undefined,
  maxSize: undefined,
  enableChunked: false,
  chunkSize: 2,
  beforeUpload: undefined,
  customRequest: undefined,
  autoUpload: true
})

const emit = defineEmits<{
  'update:modelValue': [fileList: UploadFile[]]
  'change': [info: UploadChangeParam]
  'drop': [e: DragEvent]
  'remove': [file: UploadFile]
  'preview': [file: UploadFile]
}>()

const fileList = ref<UploadFile[]>(props.modelValue || [])

// 监听 modelValue 变化
watch(() => props.modelValue, (newValue) => {
  if (newValue !== fileList.value) {
    fileList.value = newValue || []
  }
}, { deep: true })

// 监听 fileList 变化
watch(fileList, (newValue) => {
  emit('update:modelValue', newValue)
}, { deep: true })

// 上传前的钩子（须 await 父级 beforeUpload，否则异步查重无法拦住上传）
const handleBeforeUpload = async (file: UploadFile | File) => {
  const originFile = (file as UploadFile).originFileObj || (file as File)

  if (props.maxSize && originFile.size) {
    const fileSizeMB = originFile.size / 1024 / 1024
    if (fileSizeMB > props.maxSize) {
      message.error(t('components.common.page.upload.filesizeexceed', { max: props.maxSize }))
      return Upload.LIST_IGNORE
    }
  }

  if (props.beforeUpload) {
    const beforeUpload = props.beforeUpload
    const result = await Promise.resolve(beforeUpload(
      originFile as Parameters<BeforeUploadFn>[0],
      fileList.value as Parameters<BeforeUploadFn>[1],
    ))
    if (result === Upload.LIST_IGNORE || result === false) {
      return result
    }
  }

  if (!props.autoUpload) {
    return false
  }

  return true
}

// 文件状态改变时的回调
const handleChange = (info: UploadChangeParam) => {
  fileList.value = info.fileList
  emit('change', info)

  const status = info.file.status
  if (status !== 'uploading') {
    console.log('[TaktUploadFiles] 文件状态变化:', info.file, info.fileList)
  }
  if (status === 'done') {
    message.success(t('components.common.page.upload.fileuploadsuccess', { name: info.file.name }))
  } else if (status === 'error') {
    const detail = info.file.error instanceof Error
      ? info.file.error.message
      : typeof info.file.error === 'string'
        ? info.file.error
        : ''
    message.error(detail || t('components.common.page.upload.fileuploadfail', { name: info.file.name }))
  }
}

// 拖拽放下时的回调
const handleDrop = (e: DragEvent) => {
  console.log('[TaktUploadFiles] 拖拽放下:', e)
  emit('drop', e)
}

// 移除文件
const handleRemove = (file: UploadFile) => {
  emit('remove', file)
}

// 预览文件
const handlePreview = (file: UploadFile) => {
  emit('preview', file)
  
  if (file.url) {
    window.open(file.url, '_blank')
  } else if (file.preview) {
    window.open(file.preview, '_blank')
  } else {
    message.warning(t('components.common.page.upload.previewfail'))
  }
}

// 手动上传所有未上传的文件
const uploadFiles = async (): Promise<void> => {
  // 如果没有提供上传方法，直接返回（不报错，允许没有上传方法的情况）
  if (!props.customRequest) {
    return
  }
  
  // 获取所有未上传的文件（状态为 ready、error 或未设置状态的文件，排除已完成的）
  const filesToUpload = fileList.value.filter(
    file => file.status !== 'done' && file.status !== 'removed'
  )
  
  // 如果没有需要上传的文件，直接返回
  if (filesToUpload.length === 0) {
    return
  }
  
  // 逐个上传文件
  const uploadPromises = filesToUpload.map((file) => {
    return new Promise<void>((resolve, reject) => {
      const originFile = file.originFileObj as File | undefined
      if (!originFile) {
        reject(new Error(t('components.common.page.upload.fileinvalid', { name: file.name })))
        return
      }
      
      // 更新文件状态为 uploading
      file.status = 'uploading'
      file.percent = 0
      
      // 调用自定义上传请求
      const requestOptions: CustomRequestOptions = {
        file: originFile,
        action: props.action || '',
        method: 'post',
        onSuccess: (response: unknown) => {
          file.status = 'done'
          file.percent = 100
          file.response = response
          // 手动触发 change 事件，确保父组件能收到文件上传完成的通知
          emit('change', {
            file,
            fileList: fileList.value
          } as UploadChangeParam)
          resolve()
        },
        onError: (error: unknown) => {
          const err = error instanceof Error ? error : new Error(String(error))
          file.status = 'error'
          file.error = err
          
          // 记录错误日志
          logger.error('[TaktUploadFiles] 文件上传失败:', {
            fileName: file.name,
            error: err.message,
            file: file
          })
          
          // 手动触发 change 事件，通知父组件上传失败
          emit('change', {
            file,
            fileList: fileList.value
          } as UploadChangeParam)
          reject(err)
        },
        onProgress: (event: UploadProgress) => {
          file.percent = event.percent ?? 0
        }
      }
      props.customRequest?.(requestOptions)
    })
  })
  
  try {
    await Promise.all(uploadPromises)
    message.success(t('components.common.page.upload.alluploadsuccess'))
  } catch (error: unknown) {
    const err = error instanceof Error ? error : new Error(String(error))
    const errorMessage = err.message || t('components.common.page.upload.partialuploadfail')
    logger.error('[TaktUploadFiles] 批量上传失败:', {
      error: errorMessage,
      filesCount: filesToUpload.length,
      fullError: err
    })
    message.error(errorMessage)
    throw err
  }
}

// 清空文件列表
const clearFiles = () => {
  fileList.value = []
  emit('update:modelValue', [])
}

// 暴露方法给父组件
defineExpose({
  uploadFiles,
  clearFiles
})
</script>
