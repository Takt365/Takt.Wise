<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktUploadFile -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：文件上传组件，支持头像、图片、文件上传切换 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-upload-file">
    <!-- 如果只有一个标签页，不显示 tabs -->
    <template v-if="visibleTabs.length === 1">
      <TaktUploadAvatar
        v-if="visibleTabs.includes('avatar')"
        v-model="avatarUrl"
        :avatar-size="avatarSize"
        :crop-size="avatarCropSize"
        :canvas-size="avatarCanvasSize"
        :min-pixel-size="avatarMinPixelSize"
        :max-pixel-size="avatarMaxPixelSize"
        :max-size="avatarMaxSize"
        :show-preview="avatarShowPreview"
        :custom-request="avatarCustomRequest"
        :action="avatarAction"
        @success="handleAvatarSuccess"
        @error="handleAvatarError"
        @remove="handleAvatarRemove"
      />
      <TaktUploadImages
        v-if="visibleTabs.includes('images')"
        v-model="imagesFileList"
        :name="imagesName"
        :multiple="imagesMultiple"
        :action="imagesAction"
        :accept="imagesAccept"
        :max-count="imagesMaxCount"
        :disabled="imagesDisabled"
        :list-type="imagesListType"
        :before-upload="imagesBeforeUpload"
        :custom-request="imagesCustomRequest"
        :show-upload-list="imagesShowUploadList"
        :upload-text="imagesUploadText"
        :max-size="imagesMaxSize"
        :enable-crop="imagesEnableCrop"
        :aspect-ratio="imagesAspectRatio"
        :crop-width="imagesCropWidth"
        :crop-height="imagesCropHeight"
        @change="handleImagesChange"
        @preview="handleImagesPreview"
        @remove="handleImagesRemove"
        @crop="handleImagesCrop"
      />
      <TaktUploadFiles
        v-if="visibleTabs.includes('files')"
        ref="filesUploadRef"
        v-model="filesFileList"
        :name="filesName"
        :multiple="filesMultiple"
        :action="filesAutoUpload ? filesAction : undefined"
        :accept="filesAccept"
        :max-count="filesMaxCount"
        :disabled="filesDisabled"
        :show-upload-list="filesShowUploadList"
        :before-upload="filesBeforeUpload"
        :custom-request="filesAutoUpload ? filesCustomRequest : filesCustomRequest"
        :auto-upload="filesAutoUpload"
        :text="filesText"
        :hint="filesHint"
        :max-size="filesMaxSize"
        :enable-chunked="filesEnableChunked"
        :chunk-size="filesChunkSize"
        @change="handleFilesChange"
        @drop="handleFilesDrop"
        @remove="handleFilesRemove"
        @preview="handleFilesPreview"
      />
    </template>

    <!-- 多个标签页时显示 tabs -->
    <a-tabs
      v-else
      v-model:active-key="currentActiveKey"
      type="card"
      class="takt-upload-file-tabs"
    >
      <a-tab-pane
        v-if="visibleTabs.includes('avatar')"
        key="avatar"
        :tab="t('components.common.page.upload.avatar')"
      >
        <TaktUploadAvatar
          v-model="avatarUrl"
          :avatar-size="avatarSize"
          :crop-size="avatarCropSize"
          :canvas-size="avatarCanvasSize"
          :min-pixel-size="avatarMinPixelSize"
          :max-pixel-size="avatarMaxPixelSize"
          :max-size="avatarMaxSize"
          :show-preview="avatarShowPreview"
          :custom-request="avatarCustomRequest"
          :action="avatarAction"
          @success="handleAvatarSuccess"
          @error="handleAvatarError"
          @remove="handleAvatarRemove"
        />
      </a-tab-pane>

      <a-tab-pane
        v-if="visibleTabs.includes('images')"
        key="images"
        :tab="t('components.common.page.upload.images')"
      >
        <TaktUploadImages
          v-model="imagesFileList"
          :name="imagesName"
          :multiple="imagesMultiple"
          :action="imagesAction"
          :accept="imagesAccept"
          :max-count="imagesMaxCount"
          :disabled="imagesDisabled"
          :list-type="imagesListType"
          :before-upload="imagesBeforeUpload"
          :custom-request="imagesCustomRequest"
          :show-upload-list="imagesShowUploadList"
          :upload-text="imagesUploadText"
          :max-size="imagesMaxSize"
          :enable-crop="imagesEnableCrop"
          :aspect-ratio="imagesAspectRatio"
          :crop-width="imagesCropWidth"
          :crop-height="imagesCropHeight"
          @change="handleImagesChange"
          @preview="handleImagesPreview"
          @remove="handleImagesRemove"
          @crop="handleImagesCrop"
        />
      </a-tab-pane>

      <a-tab-pane
        v-if="visibleTabs.includes('files')"
        key="files"
        :tab="t('components.common.page.upload.files')"
      >
        <TaktUploadFiles
          ref="filesUploadRef"
          v-model="filesFileList"
          :name="filesName"
          :multiple="filesMultiple"
          :action="filesAutoUpload ? filesAction : undefined"
          :accept="filesAccept"
          :max-count="filesMaxCount"
          :disabled="filesDisabled"
          :show-upload-list="filesShowUploadList"
          :before-upload="filesBeforeUpload"
          :custom-request="filesAutoUpload ? filesCustomRequest : filesCustomRequest"
          :auto-upload="filesAutoUpload"
          :text="filesText"
          :hint="filesHint"
          :max-size="filesMaxSize"
          :enable-chunked="filesEnableChunked"
          :chunk-size="filesChunkSize"
          @change="handleFilesChange"
          @drop="handleFilesDrop"
          @remove="handleFilesRemove"
          @preview="handleFilesPreview"
        />
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
import type { UploadChangeParam, UploadFile, UploadProps } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import TaktUploadAvatar from './avatar/index.vue'
import TaktUploadImages from './images/index.vue'
import TaktUploadFiles from './files/index.vue'

const { t } = useI18n()

interface Props {
  /** 当前激活的标签页 */
  activeKey?: 'avatar' | 'images' | 'files'
  /** 显示的标签页类型,可选值:'all' | 'avatar' | 'images' | 'files' | ['avatar', 'images'] 等 */
  tabsType?: 'all' | 'avatar' | 'images' | 'files' | Array<'avatar' | 'images' | 'files'>
  
  // Avatar 相关属性
  /** 头像地址 */
  avatarUrl?: string
  /** 头像尺寸(默认 120) */
  avatarSize?: number
  /** 头像裁剪框与输出边长(默认 256) */
  avatarCropSize?: number
  /** 头像裁剪画布边长(默认 512) */
  avatarCanvasSize?: number
  /** 头像源图最小边长像素（默认 128） */
  avatarMinPixelSize?: number
  /** 头像裁剪输出最大边长像素（默认 512） */
  avatarMaxPixelSize?: number
  /** 头像文件大小限制(MB,默认 2) */
  avatarMaxSize?: number
  /** 是否显示头像组件内预览（默认 true；弹窗内可 false） */
  avatarShowPreview?: boolean
  /** 头像自定义上传请求 */
  avatarCustomRequest?: UploadProps['customRequest'] | undefined
  /** 头像上传地址 */
  avatarAction?: string | undefined

  // Images 相关属性
  /** 图片文件列表 */
  imagesFileList?: UploadFile[]
  /** 图片上传字段名 */
  imagesName?: string
  /** 图片是否支持多选 */
  imagesMultiple?: boolean
  /** 图片上传地址 */
  imagesAction?: string | undefined
  /** 图片接受类型 */
  imagesAccept?: string
  /** 图片最大数量 */
  imagesMaxCount?: number
  /** 图片是否禁用 */
  imagesDisabled?: boolean
  /** 图片列表类型 */
  imagesListType?: UploadProps['listType']
  /** 图片上传前钩子 */
  imagesBeforeUpload?: UploadProps['beforeUpload'] | undefined
  /** 图片自定义上传请求 */
  imagesCustomRequest?: UploadProps['customRequest'] | undefined
  /** 图片是否显示上传列表 */
  imagesShowUploadList?: boolean | UploadProps['showUploadList']
  /** 图片上传文本 */
  imagesUploadText?: string | undefined
  /** 图片文件大小限制(MB) */
  imagesMaxSize?: number
  /** 图片是否启用裁剪 */
  imagesEnableCrop?: boolean
  /** 图片裁剪比例 */
  imagesAspectRatio?: number | 'free'
  /** 图片裁剪宽度 */
  imagesCropWidth?: number | undefined
  /** 图片裁剪高度 */
  imagesCropHeight?: number | undefined

  // Files 相关属性
  /** 文件列表 */
  filesFileList?: UploadFile[]
  /** 文件上传字段名 */
  filesName?: string
  /** 文件是否支持多选 */
  filesMultiple?: boolean
  /** 文件上传地址 */
  filesAction?: string
  /** 文件接受类型 */
  filesAccept?: string | undefined
  /** 文件最大数量 */
  filesMaxCount?: number | undefined
  /** 文件是否禁用 */
  filesDisabled?: boolean
  /** 文件是否显示上传列表 */
  filesShowUploadList?: boolean | UploadProps['showUploadList']
  /** 文件上传前钩子 */
  filesBeforeUpload?: UploadProps['beforeUpload'] | undefined
  /** 文件自定义上传请求 */
  filesCustomRequest?: UploadProps['customRequest'] | undefined
  /** 文件提示文本 */
  filesText?: string | undefined
  /** 文件提示说明 */
  filesHint?: string | undefined
  /** 文件大小限制(MB) */
  filesMaxSize?: number | undefined
  /** 文件是否启用分片上传 */
  filesEnableChunked?: boolean
  /** 文件分片大小(MB,默认 2) */
  filesChunkSize?: number
  /** 文件是否自动上传(true=自动上传,false=手动上传,默认 true) */
  filesAutoUpload?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  activeKey: 'avatar',
  tabsType: 'all',
  // Avatar
  avatarUrl: '',
  avatarSize: 120,
  avatarCropSize: 256,
  avatarCanvasSize: 512,
  avatarMinPixelSize: 128,
  avatarMaxPixelSize: 512,
  avatarMaxSize: 2,
  avatarShowPreview: true,
  avatarAction: '',
  // Images
  imagesFileList: () => [],
  imagesName: 'file',
  imagesMultiple: true,
  imagesAction: '',
  imagesAccept: 'image/*',
  imagesMaxCount: 8,
  imagesDisabled: false,
  imagesListType: 'picture-card',
  imagesShowUploadList: true,
  imagesMaxSize: 2,
  imagesEnableCrop: false,
  imagesAspectRatio: 'free',
  // Files
  filesFileList: () => [],
  filesName: 'file',
  filesMultiple: true,
  filesAction: '',
  filesDisabled: false,
  filesShowUploadList: true,
  filesEnableChunked: false,
  filesChunkSize: 2,
  filesAutoUpload: true
})

type FilesUploadRef = {
  uploadFiles: () => Promise<void>
  clearFiles: () => void
}

const emit = defineEmits<{
  /** 当前激活标签页变化 */
  'update:activeKey': [key: 'avatar' | 'images' | 'files']
  /** 头像地址变化 */
  'update:avatarUrl': [url: string]
  /** 头像上传成功 */
  'avatar:success': [url: string, file: File]
  /** 头像上传失败 */
  'avatar:error': [error: Error]
  /** 头像删除 */
  'avatar:remove': []
  /** 图片文件列表变化 */
  'update:imagesFileList': [fileList: UploadFile[]]
  /** 图片上传变化 */
  'images:change': [info: UploadChangeParam]
  /** 图片预览 */
  'images:preview': [file: UploadFile]
  /** 图片移除 */
  'images:remove': [file: UploadFile]
  /** 图片裁剪 */
  'images:crop': [file: File, croppedFile: File]
  /** 文件列表变化 */
  'update:filesFileList': [fileList: UploadFile[]]
  /** 文件上传变化 */
  'files:change': [info: UploadChangeParam]
  /** 文件拖拽 */
  'files:drop': [e: DragEvent]
  /** 文件移除 */
  'files:remove': [file: UploadFile]
  /** 文件预览 */
  'files:preview': [file: UploadFile]
}>()

// 计算可见的标签页
const visibleTabs = computed<Array<'avatar' | 'images' | 'files'>>(() => {
  if (props.tabsType === 'all') {
    return ['avatar', 'images', 'files']
  } else if (typeof props.tabsType === 'string') {
    return [props.tabsType]
  } else if (Array.isArray(props.tabsType)) {
    return props.tabsType
  }
  return ['avatar', 'images', 'files']
})

// 计算默认激活的标签页（如果指定的标签页不可见，则使用第一个可见的标签页）
const defaultActiveKey = computed<'avatar' | 'images' | 'files'>(() => {
  const specified = props.activeKey || 'avatar'
  if (visibleTabs.value.includes(specified)) {
    return specified
  }
  return visibleTabs.value[0] || 'avatar'
})

const currentActiveKey = ref<'avatar' | 'images' | 'files'>(defaultActiveKey.value)
const avatarUrl = ref(props.avatarUrl)
const imagesFileList = ref<UploadFile[]>(props.imagesFileList || [])
const filesFileList = ref<UploadFile[]>(props.filesFileList || [])
const filesUploadRef = ref<FilesUploadRef | null>(null)

// 监听 visibleTabs 变化，自动调整 activeKey
watch(visibleTabs, (newTabs) => {
  if (!newTabs.includes(currentActiveKey.value)) {
    currentActiveKey.value = newTabs[0] || 'avatar'
  }
}, { immediate: true })

// 监听 activeKey 变化
watch(() => props.activeKey, (newValue) => {
  if (newValue) {
    currentActiveKey.value = newValue
  }
}, { immediate: true })

watch(currentActiveKey, (newValue) => {
  emit('update:activeKey', newValue)
})

// 监听 avatarUrl 变化
watch(() => props.avatarUrl, (newValue) => {
  avatarUrl.value = newValue || ''
}, { immediate: true })

watch(avatarUrl, (newValue) => {
  emit('update:avatarUrl', newValue)
})

// 监听 imagesFileList 变化
watch(() => props.imagesFileList, (newValue) => {
  imagesFileList.value = newValue || []
}, { deep: true, immediate: true })

watch(imagesFileList, (newValue) => {
  emit('update:imagesFileList', newValue)
}, { deep: true })

// 监听 filesFileList 变化
watch(() => props.filesFileList, (newValue) => {
  filesFileList.value = newValue || []
}, { deep: true, immediate: true })

watch(filesFileList, (newValue) => {
  emit('update:filesFileList', newValue)
}, { deep: true })

// Avatar 事件处理
const handleAvatarSuccess = (url: string, file: File) => {
  emit('avatar:success', url, file)
}

const handleAvatarError = (error: Error) => {
  emit('avatar:error', error)
}

const handleAvatarRemove = () => {
  emit('avatar:remove')
}

// Images 事件处理
const handleImagesChange = (info: UploadChangeParam) => {
  emit('images:change', info)
}

const handleImagesPreview = (file: UploadFile) => {
  emit('images:preview', file)
}

const handleImagesRemove = (file: UploadFile) => {
  emit('images:remove', file)
}

const handleImagesCrop = (file: File, croppedFile: File) => {
  emit('images:crop', file, croppedFile)
}

// Files 事件处理
const handleFilesChange = (info: UploadChangeParam) => {
  emit('files:change', info)
}

const handleFilesDrop = (e: DragEvent) => {
  emit('files:drop', e)
}

const handleFilesRemove = (file: UploadFile) => {
  emit('files:remove', file)
}

const handleFilesPreview = (file: UploadFile) => {
  emit('files:preview', file)
}

// 手动上传文件（仅在手动上传模式下使用）
const uploadFiles = async (): Promise<void> => {
  if (filesUploadRef.value) {
    await filesUploadRef.value.uploadFiles()
  }
}

// 清空文件列表
const clearFiles = () => {
  if (filesUploadRef.value) {
    filesUploadRef.value.clearFiles()
  }
  filesFileList.value = []
  emit('update:filesFileList', [])
}

// 暴露方法给父组件
defineExpose({
  uploadFiles,
  clearFiles
})
</script>

<style scoped lang="css">
.takt-upload-file {
  .takt-upload-file-tabs {
    :deep(.ant-tabs-content-holder) {
      padding-top: 16px;
    }
  }
}
</style>
