<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/databaseBackup.types/components -->
<!-- 文件名称：BackupNetworkFolderDialog.vue -->
<!-- 功能描述：文件服务器 UNC 目录选择（TaktDirectoryExplore method=fileserver） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="openProxy"
    :title="t('code.database.database-backup.page.dialog.networktitle')"
    :confirm-loading="confirmLoading"
    :use-viewport-size="false"
    width="960px"
    @ok="handleOk"
  >
    <TaktDirectoryExplore
      v-if="openProxy"
      ref="exploreRef"
      method="fileserver"
      :initial-path="initialPath"
      :initial-user-name="initialUserName"
      :has-password="hasPassword"
      :config-id="databaseBackupId"
      :panel-height="420"
      :table-scroll-y="280"
    />
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * 网络备份目录：通用目录浏览（文件服务器）
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'

/** i18n */
const { t } = useI18n()

const props = defineProps<{
  open: boolean
  initialPath?: string
  initialUserName?: string
  hasPassword?: boolean
  databaseBackupId?: string
}>()

const emit = defineEmits<{
  'update:open': [value: boolean]
  confirm: [payload: { path: string; userName?: string; password?: string }]
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
  }
} | null>(null)

async function handleOk() {
  confirmLoading.value = true
  try {
    const path = await exploreRef.value?.resolveConfirmPath(true)
    if (!path || !/^\\\\[^\\]+\\[^\\]+/.test(String(path).replace(/\//g, '\\'))) {
      message.warning(t('code.database.database-backup.page.message.pathrequired'))
      return
    }
    const values = exploreRef.value?.getValues()
    emit('confirm', {
      path,
      userName: values?.userName?.trim() || undefined,
      password: values?.password || undefined,
    })
    openProxy.value = false
  } catch (error) {
    logger.error('[BackupNetworkDialog] confirm failed', { error })
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
