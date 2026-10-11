<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktUploadFile/images -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：图片上传组件，支持图片卡片展示、裁剪和预览功能 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-upload-images">
    <a-upload
      v-model:file-list="fileList"
      :name="name"
      :multiple="multiple"
      :action="action"
      :accept="accept"
      :max-count="maxCount"
      :disabled="disabled"
      :list-type="listType"
      :before-upload="handleBeforeUpload"
      :custom-request="customRequest"
      :show-upload-list="showUploadList"
      v-bind="$attrs"
      class="takt-upload-images-upload"
      @change="handleChange"
      @preview="handlePreview"
      @remove="handleRemove"
    >
      <div v-if="fileList.length < (maxCount || 8)">
        <plus-outlined />
        <div style="margin-top: 8px">
          {{ uploadText ?? t('components.common.page.upload.upload') }}
        </div>
      </div>
    </a-upload>
    
    <!-- 图片裁剪弹窗 -->
    <a-modal
      v-model:open="cropperVisible"
      :title="t('components.common.page.upload.cropimage')"
      :width="900"
      :ok-text="t('common.page.button.ok')"
      :cancel-text="t('common.page.button.cancel')"
      class="takt-upload-images-cropper"
      @ok="handleCropConfirm"
      @cancel="handleCropCancel"
    >
      <TaktCropper
        ref="cropperRef"
        :image-src="cropperImage"
        :aspect-ratio="aspectRatio"
        :width="800"
        :height="600"
        :crop-width="cropWidth"
        :crop-height="cropHeight"
        @crop="handleCropperCrop"
        @error="handleCropperError"
      />
    </a-modal>

    <!-- 图片预览弹窗 -->
    <a-modal
      v-model:open="previewVisible"
      :title="previewTitle"
      :footer="null"
      class="takt-upload-images-preview"
      @cancel="handleCancel"
    >
      <img
        alt="preview"
        style="width: 100%"
        :src="previewImage"
      >
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { PlusOutlined } from '@ant-design/icons-vue'
import { message } from 'ant-design-vue'
import type { UploadChangeParam, UploadFile, UploadProps } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import TaktCropper from '../cropper/index.vue'

const { t } = useI18n()
type BeforeUploadFn = NonNullable<UploadProps['beforeUpload']>
type CustomRequestOptions = Parameters<NonNullable<UploadProps['customRequest']>>[0]
type UploadResultLike = { url?: string; data?: { url?: string } }
type UploadProgress = { percent?: number }

interface Props {
  /** 文件列表 */
  modelValue?: UploadFile[] | undefined
  /** 上传的文件字段名 */
  name?: string
  /** 是否支持多选 */
  multiple?: boolean
  /** 上传的地址 */
  action?: string
  /** 接受上传的文件类型 */
  accept?: string
  /** 最大上传数量 */
  maxCount?: number
  /** 是否禁用 */
  disabled?: boolean
  /** 上传列表的内建样式 */
  listType?: UploadProps['listType']
  /** 上传前的钩子 */
  beforeUpload?: UploadProps['beforeUpload'] | undefined
  /** 自定义上传请求 */
  customRequest?: UploadProps['customRequest'] | undefined
  /** 是否显示上传列表 */
  showUploadList?: boolean | UploadProps['showUploadList']
  /** 上传文本 */
  uploadText?: string | undefined
  /** 文件大小限制(MB) */
  maxSize?: number
  /** 是否启用裁剪 */
  enableCrop?: boolean
  /** 裁剪比例(如 1:1, 16:9, 'free') */
  aspectRatio?: number | 'free'
  /** 裁剪框宽度 */
  cropWidth?: number | undefined
  /** 裁剪框高度 */
  cropHeight?: number | undefined
}

const props = withDefaults(defineProps<Props>(), {
  name: 'file',
  multiple: true,
  action: '',
  accept: 'image/*',
  maxCount: 8,
  disabled: false,
  listType: 'picture-card',
  showUploadList: true,
  maxSize: 2,
  enableCrop: false,
  aspectRatio: 'free'
})

const emit = defineEmits<{
  'update:modelValue': [fileList: UploadFile[]]
  'change': [info: UploadChangeParam]
  'preview': [file: UploadFile]
  'remove': [file: UploadFile]
  'crop': [file: File, croppedFile: File]
}>()

const fileList = ref<UploadFile[]>(props.modelValue || [])
const previewVisible = ref(false)
const previewImage = ref('')
const previewTitle = ref('')
const cropperVisible = ref(false)
const cropperImage = ref('')
const cropperRef = ref<InstanceType<typeof TaktCropper> | null>(null)
const currentCropFile = ref<File | null>(null)

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

function getUploadUrl(result: unknown): string | undefined {
  const data = result as UploadResultLike
  return data?.url ?? data?.data?.url
}

function toError(error: unknown): Error {
  return error instanceof Error ? error : new Error(String(error))
}

// 获取文件的 base64 预览
function getBase64(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.readAsDataURL(file)
    reader.onload = () => resolve(reader.result as string)
    reader.onerror = (error) => reject(error)
  })
}

// 上传前的钩子
const handleBeforeUpload = (file: UploadFile | File) => {
  // 获取原生 File 对象
  const originFile = (file as UploadFile).originFileObj || (file as File)
  
  // 检查是否为图片文件
  if (!originFile.type?.startsWith('image/')) {
    message.error(t('components.common.page.upload.imageonly'))
    return false
  }

  // 如果设置了文件大小限制
  if (props.maxSize && originFile.size) {
    const fileSizeMB = originFile.size / 1024 / 1024
    if (fileSizeMB > props.maxSize) {
      message.error(t('components.common.page.upload.imagesizeexceed', { max: props.maxSize }))
      return false
    }
  }

  // 如果启用了裁剪，显示裁剪弹窗
  if (props.enableCrop) {
    const reader = new FileReader()
    reader.onload = (e) => {
      cropperImage.value = e.target?.result as string
      currentCropFile.value = originFile
      cropperVisible.value = true
    }
    reader.readAsDataURL(originFile)
    return false // 阻止自动上传，等待裁剪完成
  }

  // 如果提供了自定义 beforeUpload
  if (props.beforeUpload) {
    // 将 UploadFile 或 File 转换为 File 类型传给 beforeUpload
    const fileForUpload = (file as UploadFile).originFileObj || (file as File)
    const beforeUpload = props.beforeUpload
    return beforeUpload(
      fileForUpload as Parameters<BeforeUploadFn>[0],
      fileList.value as Parameters<BeforeUploadFn>[1]
    )
  }

  return true
}

// 裁剪器裁剪事件
const handleCropperCrop = (_blob: Blob, _dataUrl: string) => {
  // 裁剪完成事件（可选，用于预览）
}

// 裁剪器错误事件
const handleCropperError = (error: Error) => {
  message.error(t('components.common.page.upload.cropfail') + '：' + error.message)
}

// 确认裁剪
const handleCropConfirm = async () => {
  if (!cropperRef.value || !currentCropFile.value) {
    message.error(t('components.common.page.upload.croppernotinit'))
    return
  }

  try {
    // 获取裁剪后的图片
    const { blob, dataUrl } = await cropperRef.value.getCroppedImage()

    const croppedFile = new File(
      [blob],
      currentCropFile.value.name,
      { type: currentCropFile.value.type || 'image/png' }
    )

    // 触发裁剪事件
    emit('crop', currentCropFile.value, croppedFile)

    // 如果有自定义上传请求，使用裁剪后的文件上传
    if (props.customRequest) {
      // 创建一个临时的 UploadFile 对象
      const uploadFile: UploadFile = {
        uid: Date.now().toString(),
        name: croppedFile.name,
        status: 'uploading',
        originFileObj: croppedFile
      } as UploadFile

      // 添加到文件列表
      fileList.value.push(uploadFile)

      // 手动触发上传
      const requestOptions: CustomRequestOptions = {
        file: croppedFile,
        action: props.action || '',
        method: 'post',
        onSuccess: (result: unknown) => {
          const index = fileList.value.findIndex(f => f.uid === uploadFile.uid)
          if (index !== -1) {
            const fileItem = fileList.value[index]
            if (fileItem) {
              fileItem.status = 'done'
              fileItem.response = result
              const uploadedUrl = getUploadUrl(result)
              if (uploadedUrl) {
                fileItem.url = uploadedUrl
              }
            }
          }
          handleChange({
            file: fileList.value[index] || uploadFile,
            fileList: fileList.value
          } as UploadChangeParam)
        },
        onError: (error: unknown) => {
          const index = fileList.value.findIndex(f => f.uid === uploadFile.uid)
          if (index !== -1) {
            const fileItem = fileList.value[index]
            if (fileItem) {
              fileItem.status = 'error'
            }
          }
          const errorMessage = toError(error).message || t('components.common.page.upload.uploadfail')
          message.error(t('components.common.page.upload.uploadimagefail') + '：' + errorMessage)
        },
        onProgress: (event: UploadProgress) => {
          const index = fileList.value.findIndex(f => f.uid === uploadFile.uid)
          if (index !== -1) {
            const fileItem = fileList.value[index]
            if (fileItem) {
              fileItem.percent = event.percent ?? 0
              fileItem.status = 'uploading'
            }
          }
        }
      }
      props.customRequest(requestOptions)
    } else {
      // 如果没有自定义上传请求，直接添加到文件列表
      const uploadFile: UploadFile = {
        uid: Date.now().toString(),
        name: croppedFile.name,
        status: 'done',
        originFileObj: croppedFile,
        url: dataUrl
      } as UploadFile

      fileList.value.push(uploadFile)
      handleChange({
        file: uploadFile,
        fileList: fileList.value
      } as UploadChangeParam)
    }

    // 关闭裁剪弹窗
    cropperVisible.value = false
    cropperImage.value = ''
    currentCropFile.value = null
  } catch (error: unknown) {
    console.error('[TaktUploadImages] 裁剪失败:', error)
    message.error(toError(error).message || t('components.common.page.upload.cropfail'))
  }
}

// 取消裁剪
const handleCropCancel = () => {
  cropperVisible.value = false
  cropperImage.value = ''
  currentCropFile.value = null
}

// 文件状态改变时的回调
const handleChange = (info: UploadChangeParam) => {
  fileList.value = info.fileList
  emit('change', info)

  const { status } = info.file
  if (status === 'done') {
    message.success(t('components.common.page.upload.fileuploadsuccess', { name: info.file.name }))
  } else if (status === 'error') {
    message.error(t('components.common.page.upload.fileuploadfail', { name: info.file.name }))
  }
}

// 预览图片
const handlePreview = async (file: UploadFile) => {
  // 如果文件没有 url 和 preview，则生成 base64 预览
  if (!file.url && !file.preview && file.originFileObj) {
    try {
      file.preview = await getBase64(file.originFileObj)
    } catch (error) {
      console.error('[TaktUploadImages] 生成预览失败:', error)
      message.error(t('components.common.page.upload.generatepreviewfail'))
      return
    }
  }
  
  previewImage.value = file.url || file.preview || ''
  previewVisible.value = true
  previewTitle.value = file.name || (file.url ? file.url.substring(file.url.lastIndexOf('/') + 1) : t('components.common.page.upload.preview'))
  emit('preview', file)
}

// 取消预览
const handleCancel = () => {
  previewVisible.value = false
  previewTitle.value = ''
  previewImage.value = ''
}

// 移除图片
const handleRemove = (file: UploadFile) => {
  emit('remove', file)
}
</script>

<style scoped lang="css">
.takt-upload-images {
  .takt-upload-images-upload {
    :deep(.ant-upload-select-picture-card) {
      width: 104px;
      height: 104px;
      
      i {
        font-size: 32px;
        color: #999;
      }
      
      .ant-upload-text {
        margin-top: 8px;
        color: #666;
      }
    }

    :deep(.ant-upload-list-picture-card) {
      .ant-upload-list-item {
        width: 104px;
        height: 104px;
      }
    }

    :deep(.ant-upload-list-picture) {
      .ant-upload-list-item {
        padding: 8px;
        border: 1px solid #d9d9d9;
        border-radius: 4px;
      }
    }
  }

  .takt-upload-images-cropper {
    :deep(.ant-modal-body) {
      padding: 24px;
    }
  }

  .takt-upload-images-preview {
    :deep(.ant-modal-body) {
      padding: 0;
      text-align: center;
    }
  }
}
</style>
