<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktUploadFile/avatar -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：头像上传组件，支持图片裁剪（1:1比例）和预览 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-upload-avatar">
    <div
      class="takt-upload-avatar-preview"
      :class="{ 'takt-upload-avatar-preview--actions-only': !showPreview }"
    >
      <a-avatar
        v-if="showPreview"
        :size="avatarSize"
        :src="avatarUrl"
        class="takt-upload-avatar-img"
      >
        <template #icon>
          <user-outlined />
        </template>
      </a-avatar>
      <div class="takt-upload-avatar-actions">
        <a-upload
          :show-upload-list="false"
          :before-upload="handleBeforeUpload"
          :custom-request="handleCustomRequest"
          accept="image/*"
          v-bind="$attrs"
        >
          <a-button
            type="primary"
            :loading="uploading"
          >
            <template #icon>
              <upload-outlined />
            </template>
            {{ uploading ? t('common.action.uploading') : t('components.common.page.upload.avatarupload') }}
          </a-button>
        </a-upload>
        <a-button
          v-if="avatarUrl"
          danger
          @click="handleRemove"
        >
          <template #icon>
            <delete-outlined />
          </template>
          {{ t('common.page.button.delete') }}
        </a-button>
      </div>
    </div>

    <!-- 裁剪弹窗：画布默认 512，裁剪框/输出为 cropSize（默认 256）居中 -->
    <a-modal
      v-model:open="cropperVisible"
      :title="t('components.common.page.upload.cropavatar')"
      :width="resolvedCanvasSize + 80"
      :ok-text="t('common.page.button.confirm')"
      :cancel-text="t('common.page.button.cancel')"
      :confirm-loading="cropping"
      class="takt-upload-avatar-cropper-modal"
      @ok="handleCropConfirm"
      @cancel="handleCropCancel"
    >
      <div class="flex justify-center">
        <TaktCropper
          ref="cropperRef"
          :image-src="cropperImage"
          :aspect-ratio="1"
          :width="resolvedCanvasSize"
          :height="resolvedCanvasSize"
          :crop-width="resolvedCropSize"
          :crop-height="resolvedCropSize"
          @crop="handleCropperCrop"
          @error="handleCropperError"
        />
      </div>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { UserOutlined, UploadOutlined, DeleteOutlined } from '@ant-design/icons-vue'
import { Modal, message } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import TaktCropper from '../cropper/index.vue'

const { t } = useI18n()
type CustomRequestOptions = Parameters<NonNullable<UploadProps['customRequest']>>[0]
type UploadResultLike = { url?: string; data?: { url?: string } }

interface Props {
  /** 头像地址 */
  modelValue?: string
  /** 头像尺寸(默认 120) */
  avatarSize?: number
  /** 裁剪框与输出边长(默认 256)；须落在 minPixelSize～maxPixelSize */
  cropSize?: number
  /** 裁剪画布边长(默认 512) */
  canvasSize?: number
  /** 源图/输出最小边长像素（默认 128） */
  minPixelSize?: number
  /** 源图/输出最大边长像素（默认 512） */
  maxPixelSize?: number
  /** 文件大小限制(MB,默认 2) */
  maxSize?: number
  /** 是否显示左侧/上方头像预览（弹窗内可隐藏，仅保留操作按钮） */
  showPreview?: boolean
  /** 自定义上传请求 */
  customRequest?: UploadProps['customRequest'] | undefined
  /** 上传地址 */
  action?: string | undefined
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: '',
  avatarSize: 120,
  cropSize: 256,
  canvasSize: 512,
  minPixelSize: 128,
  maxPixelSize: 512,
  maxSize: 2,
  showPreview: true,
  action: '',
  customRequest: undefined
})

/**
 * 裁剪框 / 输出边长：clamp 到 [minPixelSize, maxPixelSize]
 */
const resolvedCropSize = computed(() => {
  const min = Math.max(1, props.minPixelSize)
  const max = Math.max(min, props.maxPixelSize)
  return Math.min(max, Math.max(min, props.cropSize))
})

/**
 * 裁剪画布边长：至少覆盖裁剪框，默认 512
 */
const resolvedCanvasSize = computed(() => {
  const canvas = Math.max(1, props.canvasSize)
  return Math.max(canvas, resolvedCropSize.value)
})

const emit = defineEmits<{
  'update:modelValue': [url: string]
  /** 上传成功事件 */
  'success': [url: string, file: File]
  /** 上传失败事件 */
  'error': [error: Error]
  /** 删除事件 */
  'remove': []
}>()

const avatarUrl = ref(props.modelValue)
const uploading = ref(false)
const cropperVisible = ref(false)
const cropperImage = ref('')
const cropping = ref(false)
const cropperRef = ref<InstanceType<typeof TaktCropper> | null>(null)
const currentFile = ref<File | null>(null)

function getUploadUrl(result: unknown, fallbackUrl: string): string {
  const data = result as UploadResultLike
  return data?.url ?? data?.data?.url ?? fallbackUrl
}

function toError(error: unknown): Error {
  return error instanceof Error ? error : new Error(String(error))
}

// 监听 modelValue 变化
watch(() => props.modelValue, (newValue) => {
  avatarUrl.value = newValue
}, { immediate: true })

// 监听 avatarUrl 变化
watch(avatarUrl, (newValue) => {
  emit('update:modelValue', newValue)
})

// 上传前的钩子
const handleBeforeUpload = (file: UploadFile | File) => {
  // 获取原生 File 对象
  const originFile = (file as UploadFile).originFileObj || (file as File)

  // 检查是否为图片文件
  if (!originFile.type?.startsWith('image/')) {
    message.error(t('components.common.page.upload.imageonly'))
    return false
  }

  // 检查文件大小
  if (props.maxSize && originFile.size) {
    const fileSizeMB = originFile.size / 1024 / 1024
    if (fileSizeMB > props.maxSize) {
      message.error(t('components.common.page.upload.imagesizeexceed', { max: props.maxSize }))
      return false
    }
  }

  // 读取图片：校验像素范围后打开裁剪弹窗
  const reader = new FileReader()
  reader.onload = (e) => {
    const dataUrl = e.target?.result as string
    const probe = new Image()
    probe.onload = () => {
      const w = probe.naturalWidth || probe.width
      const h = probe.naturalHeight || probe.height
      const min = Math.max(1, props.minPixelSize)
      if (w < min || h < min) {
        message.error(t('components.common.page.upload.imagepixelmin', { min }))
        return
      }
      // 输出边长由 cropSize clamp 到 [minPixelSize, maxPixelSize]；源图可大于 max，由裁剪框截取
      cropperImage.value = dataUrl
      currentFile.value = originFile
      cropperVisible.value = true
    }
    probe.onerror = () => {
      message.error(t('components.common.page.upload.imagereadfail'))
    }
    probe.src = dataUrl
  }
  reader.onerror = () => {
    message.error(t('components.common.page.upload.imagereadfail'))
  }
  reader.readAsDataURL(originFile)

  // 阻止自动上传，等待裁剪完成
  return false
}

// 裁剪器裁剪事件
const handleCropperCrop = (_blob: Blob, _dataUrl: string) => {
  // 裁剪完成，这里可以预览裁剪后的图片
  // dataUrl 可以用于预览，blob 用于实际上传
}

// 裁剪器错误事件
const handleCropperError = (error: Error) => {
  message.error(t('components.common.page.upload.cropfail') + '：' + error.message)
}

// 确认裁剪并上传
const handleCropConfirm = async () => {
  if (!cropperRef.value) {
    message.error(t('components.common.page.upload.croppernotinit'))
    return
  }

  try {
    cropping.value = true

    // 获取裁剪后的图片
    const { blob, dataUrl } = await cropperRef.value.getCroppedImage()

    // 创建 File 对象
    const croppedFile = new File(
      [blob],
      currentFile.value?.name || `avatar-${Date.now()}.png`,
      { type: 'image/png' }
    )

    // 如果有自定义上传请求,使用自定义请求
    if (props.customRequest) {
      await new Promise<void>((resolve, reject) => {
        uploading.value = true
    
        const requestOptions: CustomRequestOptions = {
          file: croppedFile,
          action: props.action || '',
          method: 'post',
          onSuccess: (result: unknown) => {
            // 假设返回结果中有 url 字段
            const url = getUploadUrl(result, dataUrl)
            avatarUrl.value = url
            uploading.value = false
            cropperVisible.value = false
            cropperImage.value = ''
            currentFile.value = null
            message.success(t('components.common.page.upload.uploadsuccess'))
            emit('success', url, croppedFile)
            resolve()
          },
          onError: (error: unknown) => {
            uploading.value = false
            const err = toError(error)
            message.error(t('components.common.page.upload.uploadfail') + ':' + err.message)
            emit('error', err)
            reject(err)
          },
          onProgress: () => {
            // 进度处理
          }
        }
        props.customRequest!(requestOptions)
      })
    } else if (props.action) {
      // 使用默认上传请求
      uploading.value = true

      const formData = new FormData()
      formData.append('file', croppedFile)

      try {
        const response = await fetch(props.action, {
          method: 'POST',
          body: formData
        })

        if (!response.ok) {
          throw new Error(`上传失败：${response.statusText}`)
        }

        const result = await response.json()
        const url = result?.url || result?.data?.url || dataUrl
        avatarUrl.value = url
        uploading.value = false
        cropperVisible.value = false
        cropperImage.value = ''
        currentFile.value = null
        message.success(t('components.common.page.upload.uploadsuccess'))
        emit('success', url, croppedFile)
      } catch (error: unknown) {
        uploading.value = false
        const err = toError(error)
        message.error(t('components.common.page.upload.uploadfail') + '：' + err.message)
        emit('error', err)
      }
    } else {
      // 没有上传配置，直接使用 base64
      avatarUrl.value = dataUrl
      cropperVisible.value = false
      cropperImage.value = ''
      currentFile.value = null
      message.success(t('components.common.page.upload.cropsuccess'))
      emit('success', dataUrl, croppedFile)
    }
  } catch (error: unknown) {
    console.error('[TaktUploadAvatar] 裁剪上传失败:', error)
    const err = toError(error)
    message.error(t('components.common.page.upload.operationfail') + '：' + err.message)
    emit('error', err)
  } finally {
    cropping.value = false
  }
}

// 取消裁剪
const handleCropCancel = () => {
  cropperVisible.value = false
  cropperImage.value = ''
  currentFile.value = null
}

// 自定义上传请求（如果不提供，使用默认的）
const handleCustomRequest = (options: CustomRequestOptions) => {
  // 这个函数实际上不会被调用，因为我们在 beforeUpload 中返回了 false
  // 但为了完整性，这里提供一个实现
  if (props.customRequest) {
    props.customRequest(options)
  }
}

// 删除头像
const handleRemove = () => {
  Modal.confirm({
    title: t('components.common.page.upload.confirmdeleteavatar'),
    content: t('components.common.page.upload.confirmdeleteavatarcontent'),
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => {
      avatarUrl.value = ''
      message.success(t('components.common.page.upload.deletesuccess'))
      emit('remove')
    }
  })
}
</script>

<style scoped lang="css">
.takt-upload-avatar {
  .takt-upload-avatar-preview {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 16px;

    .takt-upload-avatar-img {
      border: 2px solid var(--ant-color-border, #d9d9d9);
      cursor: pointer;
      transition: border-color 0.3s;

      &:hover {
        border-color: var(--ant-color-primary, #1677ff);
      }
    }

    .takt-upload-avatar-actions {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
    }
  }

  .takt-upload-avatar-preview--actions-only {
    align-items: flex-start;

    .takt-upload-avatar-actions {
      flex-direction: column;
      align-items: stretch;
    }
  }
}

.takt-upload-avatar-cropper-modal {
  :deep(.ant-modal-body) {
    padding: 24px;
  }
}
</style>
