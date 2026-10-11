<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/databaseBackup.types/components -->
<!-- 文件名称：BackupFtpFolderDialog.vue -->
<!-- 功能描述：FTP 目录选择（TaktDirectoryExplore method=ftp） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="openProxy"
    :title="t('code.database.database-backup.page.dialog.ftptitle')"
    :confirm-loading="confirmLoading"
    :use-viewport-size="false"
    width="960px"
    @ok="handleOk"
  >
    <TaktDirectoryExplore
      v-if="openProxy"
      ref="exploreRef"
      method="ftp"
      :initial-host="initialHost"
      :initial-port="initialPort"
      :initial-path="initialPath"
      :initial-user-name="initialUserName"
      :has-password="hasPassword"
      :config-id="databaseBackupId"
      :panel-height="400"
      :table-scroll-y="260"
    />
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * FTP 备份目录：通用目录浏览（FTP 服务器）
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'

/** i18n */
const { t } = useI18n()

const props = defineProps<{
  open: boolean
  initialHost?: string
  initialPort?: number | null
  initialPath?: string
  initialUserName?: string
  hasPassword?: boolean
  databaseBackupId?: string
}>()

const emit = defineEmits<{
  'update:open': [value: boolean]
  confirm: [payload: {
    host: string
    port: number
    path: string
    userName: string
    password?: string
  }]
}>()

const openProxy = computed({
  get: () => props.open,
  set: (v: boolean) => emit('update:open', v),
})

const confirmLoading = ref(false)
const exploreRef = ref<{
  resolveConfirmPath: (ensure?: boolean) => Promise<string>
  getValues: () => {
    path: string
    userName: string
    password: string
    host: string
    port: number
  }
} | null>(null)

async function handleOk() {
  confirmLoading.value = true
  try {
    const values = exploreRef.value?.getValues()
    if (!values?.host?.trim() || !values?.userName?.trim()) {
      message.warning(t('code.database.database-backup.page.message.ftprequired'))
      return
    }
    const path = await exploreRef.value?.resolveConfirmPath(true)
    const fullPath = String(path || values.path || '/').trim()
    if (!fullPath.startsWith('/')) {
      message.warning(t('code.database.database-backup.page.message.pathrequired'))
      return
    }
    emit('confirm', {
      host: values.host.trim(),
      port: values.port || 21,
      path: fullPath,
      userName: values.userName.trim(),
      password: values.password || undefined,
    })
    openProxy.value = false
  } catch (error) {
    logger.error('[BackupFtpDialog] confirm failed', { error })
    message.error(
      (error instanceof Error && error.message)
        || t('code.database.database-backup.page.dialog.createdirectoryfailed'),
    )
  } finally {
    confirmLoading.value = false
  }
}

watch(
  () => props.open,
  () => {
    confirmLoading.value = false
  },
)
</script>
