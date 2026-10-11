<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktDownloadFile -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：通用文件下载组件，支持多种下载方式 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-button
    :loading="effectiveLoading"
    :disabled="disabled || effectiveLoading"
    :type="type"
    :size="size"
    :icon="icon"
    :block="block"
    v-bind="$attrs"
    class="takt-download-file"
    @click="handleDownload"
  >
    <slot>{{ text ?? t('common.page.button.download') }}</slot>
  </a-button>
</template>

<script setup lang="ts">
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

interface Props {
  /** 下载函数(返回 Blob 或 Promise<Blob>) */
  download?: (() => Blob | Promise<Blob>) | undefined
  /** 文件 URL(直接下载 URL 指向的文件) */
  url?: string | undefined
  /** 文件名(如果不提供,将尝试从 URL 或响应头中获取) */
  fileName?: string | undefined
  /** 文件类型(MIME 类型,用于 Blob 下载) */
  mimeType?: string
  /** 按钮文本 */
  text?: string | undefined
  /** 按钮类型 */
  type?: 'primary' | 'default' | 'dashed' | 'text' | 'link'
  /** 按钮尺寸 */
  size?: 'large' | 'middle' | 'small'
  /** 按钮图标 */
  icon?: unknown | undefined
  /** 是否块级按钮 */
  block?: boolean
  /** 是否禁用 */
  disabled?: boolean
  /** 是否显示加载状态 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  mimeType: 'application/octet-stream',
  type: 'default',
  size: 'middle',
  block: false,
  disabled: false,
  loading: false
})

const emit = defineEmits<{
  'success': [fileName: string]
  'error': [error: Error]
}>()

const internalLoading = ref(false)
const effectiveLoading = computed(() => props.loading || internalLoading.value)

// 从 URL 获取文件名
const getFileNameFromUrl = (url: string): string => {
  try {
    const urlObj = new URL(url)
    const pathname = urlObj.pathname
    const fileName = pathname.substring(pathname.lastIndexOf('/') + 1)
    return fileName || 'downloaded_file'
  } catch {
    // 如果不是完整 URL，尝试从路径中提取
    const fileName = url.substring(url.lastIndexOf('/') + 1)
    return fileName || 'downloaded_file'
  }
}

// 从响应头获取文件名
const getFileNameFromHeaders = (headers: Headers): string | null => {
  const contentDisposition = headers.get('content-disposition')
  if (contentDisposition) {
    const matches = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/)
    if (matches && matches[1]) {
      let fileName = matches[1].replace(/['"]/g, '')
      // 处理 URL 编码的文件名
      if (fileName.includes('%')) {
        try {
          fileName = decodeURIComponent(fileName)
        } catch {
          // 忽略解码错误
        }
      }
      return fileName
    }
  }
  return null
}

// 下载 Blob 文件
const downloadBlob = (blob: Blob, fileName: string) => {
  const url = window.URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = fileName
  link.style.display = 'none'
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
  
  // 延迟清理 URL，确保下载完成
  setTimeout(() => {
    window.URL.revokeObjectURL(url)
  }, 100)
}

// 处理下载
const handleDownload = async () => {
  if (!props.download && !props.url) {
    message.warning(t('common.tip.feature.unavailable', { feature: t('common.page.button.download') }))
    return
  }

  try {
    internalLoading.value = true

    let blob: Blob
    let finalFileName = props.fileName

    if (props.download) {
      // 从函数获取 Blob
      const result = props.download()
      blob = result instanceof Promise ? await result : result
      
      // 如果未提供文件名，使用默认名称
      if (!finalFileName) {
        finalFileName = `文件_${new Date().getTime()}.${getFileExtension(blob.type)}`
      }
    } else if (props.url) {
      // 从 URL 下载
      const response = await fetch(props.url)
      
      if (!response.ok) {
        throw new Error(t('common.feedback.failed'))
      }

      blob = await response.blob()

      // 尝试从响应头获取文件名
      if (!finalFileName) {
        finalFileName = getFileNameFromHeaders(response.headers) || getFileNameFromUrl(props.url)
      }
    } else {
      throw new Error(t('common.tip.feature.unavailable', { feature: t('common.page.button.download') }))
    }

    // 确保有文件名
    if (!finalFileName) {
      finalFileName = `文件_${new Date().getTime()}`
    }

    // 执行下载
    downloadBlob(blob, finalFileName)

    message.success(t('common.feedback.success'))
    emit('success', finalFileName)
  } catch (error: unknown) {
    console.error('[TaktDownloadFile] 下载失败:', error)
    const err = error instanceof Error ? error : new Error(String(error))
    message.error(err.message || t('common.feedback.failed'))
    emit('error', err)
  } finally {
    internalLoading.value = false
  }
}

// 从 MIME 类型获取文件扩展名
const getFileExtension = (mimeType: string): string => {
  const mimeMap: Record<string, string> = {
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': 'xlsx',
    'application/vnd.ms-excel': 'xls',
    'application/pdf': 'pdf',
    'application/json': 'json',
    'text/csv': 'csv',
    'text/plain': 'txt',
    'image/png': 'png',
    'image/jpeg': 'jpg',
    'image/gif': 'gif',
    'image/svg+xml': 'svg',
    'application/zip': 'zip',
    'application/x-rar-compressed': 'rar'
  }
  return mimeMap[mimeType] || 'bin'
}
</script>

<style scoped lang="css">

</style>
