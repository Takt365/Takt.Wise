<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/file/components -->
<!-- 文件名称：FileDetail.vue -->
<!-- 功能描述：文件实体只读详情表单，全部控件 disabled -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="detailState" class="file-detail">
    <a-form layout="horizontal" label-align="right" disabled>
      <a-tabs v-model:active-key="activeTab" class="file-detail-tabs">
        <a-tab-pane
          key="tab-0"
          :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
          force-render
        >
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.tenantcode')">
                  <a-input :value="text(detailState.tenantCode)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.companycode')">
                  <a-input :value="text(detailState.companyCode)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.code')">
                  <a-input :value="text(detailState.fileCode)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.name')">
                  <a-typography-link
                    v-if="canDownloadDetailFile"
                    :disabled="downloadLoading"
                    @click="handleDownloadDetailFile"
                  >
                    {{ getDetailDisplayName() || '-' }}
                  </a-typography-link>
                  <a-input
                    v-else
                    :value="getDetailDisplayName()"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.originalname')">
                  <a-input :value="text(detailState.fileOriginalName)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.path')">
                  <a-input :value="text(detailState.filePath)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.size')">
                  <a-input :value="formatFileSize(detailState.fileSize)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.type')">
                  <a-input :value="text(detailState.fileType)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.extension')">
                  <a-input :value="text(detailState.fileExtension)" size="small" disabled />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
        <a-tab-pane
          key="tab-1"
          :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
          force-render
        >
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('entity.file.hash')">
                  <a-input :value="text(detailState.fileHash)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.category')">
                  <a-input :value="formatFileCategory(detailState.fileCategory)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.path')">
                  <a-input
                    :value="detailState.uploadPath ? resolveUploadPathLabel(detailState.uploadPath) : ''"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.name')">
                  <TaktSelect
                    v-if="hasDictDisplayValue(detailState.storageNaming)"
                    :model-value="detailState.storageNaming"
                    dict-type="sys_storage_naming"
                    size="small"
                    disabled
                    :allow-clear="false"
                  />
                  <a-input v-else value="" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.storagetype')">
                  <TaktSelect
                    :model-value="detailState.storageType"
                    dict-type="sys_storage_type"
                    size="small"
                    disabled
                    :allow-clear="false"
                  />
                </a-form-item>
              </a-col>
              <a-col v-if="showOssProviderField" :span="24">
                <a-form-item :label="t('foundation.file.page.oss.provider')">
                  <TaktSelect
                    :model-value="detailState.ossProvider"
                    dict-type="sys_oss_provider"
                    size="small"
                    disabled
                    :allow-clear="false"
                  />
                </a-form-item>
              </a-col>
              <a-col v-if="showFtpProviderField" :span="24">
                <a-form-item :label="t('foundation.file.page.ftp.provider')">
                  <TaktSelect
                    :model-value="detailState.ftpProvider"
                    dict-type="sys_ftp_provider"
                    size="small"
                    disabled
                    :allow-clear="false"
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.storageconfig')">
                  <a-textarea
                    :value="text(detailState.storageConfig)"
                    :rows="2"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.accessurl')">
                  <a-input :value="text(detailState.accessUrl)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.downloadcount')">
                  <a-input :value="text(detailState.downloadCount)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.lastdownloadtime')">
                  <a-input :value="text(detailState.lastDownloadTime)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.status')">
                  <TaktSelect
                    :model-value="detailState.fileStatus"
                    dict-type="sys_normal_disable"
                    size="small"
                    disabled
                    :allow-clear="false"
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.ispublic')">
                  <TaktSelect
                    :model-value="detailState.isPublic"
                    dict-type="sys_public_type"
                    size="small"
                    disabled
                    :allow-clear="false"
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
        <a-tab-pane
          key="tab-2"
          :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
          force-render
        >
          <div class="takt-form-content-rows-5">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('entity.file.tags')">
                  <TaktTagColor :value="detailState.fileTags" />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.description')">
                  <a-textarea
                    :value="text(detailState.fileDescription)"
                    :rows="2"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.ipaddress')">
                  <a-textarea
                    :value="text(detailState.ipAddress)"
                    :rows="2"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.file.location')">
                  <a-input :value="text(detailState.location)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.extfield')">
                  <a-textarea
                    :value="text(detailState.ExtField)"
                    :rows="2"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.remark')">
                  <a-textarea
                    :value="text(detailState.remark)"
                    :rows="2"
                    size="small"
                    disabled
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
      </a-tabs>
    </a-form>
  </div>
</template>

<script setup lang="ts">
/**
 * 文件实体只读详情表单（全部 disabled）
 * @module views/foundation/file/components
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { downloadFileById } from '@/api/foundation/file.api'
import type { File } from '@/types/foundation/file.types'
import { taktFileCategoryI18nKey } from '@/utils/taktFileCategory'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import {
  normalizeOssProvider,
  parseFileStorageConfig,
} from '@/utils/taktFileStorageConfig'
import { isFileStatusEnabled, normalizeFileStatus } from '@/utils/taktFileStatus'
import { useMenuUploadPath } from '@/views/foundation/file/composables/useMenuUploadPath'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 文件下载权限 */
const permissionStore = usePermissionStore()
/** 一级目录菜单 → 上传路径展示名 */
const { resolveUploadPathLabel } = useMenuUploadPath()

/** 详情展示模型（含 storageConfig 解析字段） */
interface FileDetailViewModel extends Partial<File> {
  uploadPath?: string
  storageNaming?: string | number
  ossProvider?: string
  ftpProvider?: string
}

const props = defineProps<{
  detail?: Partial<File> | null
}>()

const emit = defineEmits<{
  /** 下载成功后通知父级刷新详情/列表 */
  downloaded: [fileId: string]
}>()

/** 当前激活 Tab */
const activeTab = ref('tab-0')
/** 详情视图模型 */
const detailState = ref<FileDetailViewModel | null>(null)
/** 详情内下载进行中 */
const downloadLoading = ref(false)
/** 是否可点击文件名称下载 */
const canDownloadDetailFile = computed(() => {
  const state = detailState.value
  if (!state?.fileId) {
    return false
  }
  if (!permissionStore.hasPermission('foundation:file:download')) {
    return false
  }
  return isFileStatusEnabled(state.fileStatus)
})

/**
 * 详情文件名称展示（与列表一致：优先原始名）
 * @returns {string} 展示名
 */
function getDetailDisplayName(): string {
  const state = detailState.value
  if (!state) {
    return ''
  }
  const original = text(state.fileOriginalName)
  const stored = text(state.fileName)
  return original || stored
}

/**
 * 只读展示文本
 * @param value 字段值
 * @returns {string} 展示字符串
 */
function text(value: unknown): string {
  if (value == null || value === '') {
    return ''
  }
  return String(value)
}

/**
 * 将 storageType 转为 number
 * @param value 原始值
 * @returns 有效整数或 undefined
 */
function coerceStorageType(value: unknown): number | undefined {
  if (value === undefined || value === null || value === '') {
    return undefined
  }
  const num = typeof value === 'number' ? value : Number(value)
  return Number.isFinite(num) ? Math.trunc(num) : undefined
}

/** 当前存储类型 */
const selectedStorageType = computed(() => coerceStorageType(detailState.value?.storageType))
/** 是否展示 OSS 提供商 */
const showOssProviderField = computed(() => selectedStorageType.value === 1)
/** 是否展示 FTP 提供商 */
const showFtpProviderField = computed(() => selectedStorageType.value === 2)

/**
 * 文件大小展示
 * @param value 字节数
 * @returns 展示文案
 */
function formatFileSize(value: unknown): string {
  if (value == null || value === '') {
    return ''
  }
  const num = typeof value === 'number' ? value : Number(value)
  if (!Number.isFinite(num)) {
    return String(value)
  }
  if (num < 1024) {
    return `${num} B`
  }
  if (num < 1024 * 1024) {
    return `${(num / 1024).toFixed(2)} KB`
  }
  if (num < 1024 * 1024 * 1024) {
    return `${(num / (1024 * 1024)).toFixed(2)} MB`
  }
  return `${(num / (1024 * 1024 * 1024)).toFixed(2)} GB`
}

/**
 * 文件分类只读展示
 * @param value 分类码
 * @returns i18n 文案
 */
function formatFileCategory(value: unknown): string {
  const num = typeof value === 'number' ? value : Number(value)
  if (!Number.isFinite(num)) {
    return ''
  }
  return t(taktFileCategoryI18nKey(Math.trunc(num)))
}

/**
 * 字典展示值是否非空（兼容 number / string）
 * @param value 字典值
 * @returns 是否可展示
 */
function hasDictDisplayValue(value: unknown): boolean {
  return value != null && String(value).trim() !== ''
}

/**
 * 详情下载用文件名（与列表、后端一致：优先原始名）
 * @returns {string} 下载文件名
 */
function resolveDetailDownloadName(): string {
  const name = getDetailDisplayName()
  return name || 'download'
}

/**
 * 点击文件名称下载
 */
async function handleDownloadDetailFile() {
  const state = detailState.value
  const fileId = state?.fileId
  if (!fileId || !canDownloadDetailFile.value) {
    return
  }
  downloadLoading.value = true
  try {
    const blob = await downloadFileById(fileId)
    const fallbackName = resolveDetailDownloadName()
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fallbackName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.success'))
    if (state) {
      const current = Number(state.downloadCount ?? 0)
      state.downloadCount = Number.isFinite(current) ? current + 1 : 1
    }
    emit('downloaded', fileId)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.failed'))
  } finally {
    downloadLoading.value = false
  }
}

/** 灌入详情并解析 storageConfig */
watch(
  () => props.detail,
  (val) => {
    activeTab.value = 'tab-0'
    if (!val) {
      detailState.value = null
      return
    }
    const next: FileDetailViewModel = { ...val }
    const parsedStorage = parseFileStorageConfig(next.storageConfig as string | undefined)
    if (parsedStorage.uploadPath) {
      next.uploadPath = parsedStorage.uploadPath
    }
    if (hasDictDisplayValue(parsedStorage.storageNaming)) {
      next.storageNaming = parsedStorage.storageNaming
    }
    if (parsedStorage.ossProvider) {
      next.ossProvider = normalizeOssProvider(parsedStorage.ossProvider)
    }
    if (parsedStorage.ftpProvider) {
      next.ftpProvider = parsedStorage.ftpProvider
    }
    if (next.fileStatus != null) {
      next.fileStatus = normalizeFileStatus(next.fileStatus)
    }
    detailState.value = next
  },
  { immediate: true, deep: true },
)
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>