<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/databaseBackup.types/components -->
<!-- 文件名称：BackupForm.vue -->
<!-- 功能描述：数据库备份配置表单；选中路径类型弹出对应选择器并回填；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="24">
        <a-form-item
          :label="t('entity.databasebackup.targettenantcode')"
          name="targetTenantCode"
        >
          <a-input
            v-model:value="formState.targetTenantCode"
            disabled
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.databasebackup.targettenantcode') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.databasebackup.targetdatabasename')"
          name="targetDatabaseName"
        >
          <a-input
            v-model:value="formState.targetDatabaseName"
            disabled
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.databasebackup.targetdatabasename') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item name="backupType">
          <template #label>
            <span class="takt-form-ext-field-label">
              <a-tooltip :title="t('code.database.database-backup.page.tip.delta')" placement="top">
                <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
              </a-tooltip>
              <span>{{ t('entity.databasebackup.backuptype') }}</span>
            </span>
          </template>
          <a-radio-group v-model:value="formState.backupType">
            <a-radio :value="1">{{ t('code.database.database-backup.page.backuptype.full') }}</a-radio>
            <a-radio :value="2">{{ t('code.database.database-backup.page.backuptype.delta') }}</a-radio>
          </a-radio-group>
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.databasebackup.backupcode')"
          name="backupCode"
        >
          <a-input
            v-model:value="formState.backupCode"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.databasebackup.backupcode') })"
            show-count
            :maxlength="40"
            allow-clear
            :disabled="!!formData?.databaseBackupId"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.databasebackup.backuppathtype')"
          name="backupPathType"
        >
          <a-radio-group
            :value="formState.backupPathType"
            @update:value="onPathTypeSelect"
          >
            <a-radio :value="4">{{ t('code.database.database-backup.page.pathtype.client') }}</a-radio>
            <a-radio :value="1">{{ t('code.database.database-backup.page.pathtype.local') }}</a-radio>
            <a-radio :value="2">{{ t('code.database.database-backup.page.pathtype.network') }}</a-radio>
            <a-radio :value="3">{{ t('code.database.database-backup.page.pathtype.ftp') }}</a-radio>
          </a-radio-group>
          <div class="mt-2 flex flex-wrap items-center gap-2">
            <a-typography-text type="secondary">
              {{ pathSummary || t('code.database.database-backup.page.dialog.notselected') }}
            </a-typography-text>
            <a-button type="link" class="!px-0" @click="reopenPathDialog">
              {{ t('code.database.database-backup.page.dialog.reselect') }}
            </a-button>
          </div>
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.databasebackup.backupfilename')"
          name="backupFileName"
        >
          <a-input
            v-model:value="formState.backupFileName"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.databasebackup.backupfilename') })"
            show-count
            :maxlength="200"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item name="extField" class="takt-form-item-ext-field">
          <template #label>
            <span class="takt-form-ext-field-label">
              <a-tooltip :title="t('common.page.entity.extfieldhint')" placement="top">
                <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
              </a-tooltip>
              <span>{{ t('common.page.entity.extfield') }}</span>
            </span>
          </template>
          <a-textarea
            v-model:value="formState.extField"
            :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item :label="t('common.page.entity.remark')" name="remark">
          <a-textarea
            v-model:value="formState.remark"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>

  <BackupLocalFolderDialog
    v-model:open="localDialogOpen"
    :initial-path="formState.backupPath"
    @confirm="onLocalConfirm"
  />
  <BackupClientFolderDialog
    v-model:open="clientDialogOpen"
    :initial-path="formState.backupPath"
    @confirm="onClientConfirm"
  />
  <BackupNetworkFolderDialog
    v-model:open="networkDialogOpen"
    :initial-path="formState.backupPath"
    :initial-user-name="formState.backupUserName"
    :has-password="!!formState.hasBackupPassword"
    :database-backup-id="formData?.databaseBackupId"
    @confirm="onNetworkConfirm"
  />
  <BackupFtpFolderDialog
    v-model:open="ftpDialogOpen"
    :initial-host="formState.backupHost"
    :initial-port="formState.backupPort"
    :initial-path="formState.backupPath"
    :initial-user-name="formState.backupUserName"
    :has-password="!!formState.hasBackupPassword"
    :database-backup-id="formData?.databaseBackupId"
    @confirm="onFtpConfirm"
  />
</template>

<script setup lang="ts">
/**
 * 数据库备份配置表单（路径类型弹窗回填）
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { RiQuestionLine } from '@remixicon/vue'
import { useDatabaseInfoCatalog } from '@/composables/useDatabaseInfoCatalog'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import type { DatabaseBackup } from '@/types/code/database/backup.types'
import { resolveFolderExplorerConfirmPath, isClientAbsolutePath, isLocalAbsolutePath, isUncAbsolutePath, isFtpAbsolutePath, normalizeClientAbsolutePath } from '@/utils/taktFolderExplorerPath'
import BackupLocalFolderDialog from './BackupLocalFolderDialog.vue'
import BackupClientFolderDialog from './BackupClientFolderDialog.vue'
import BackupNetworkFolderDialog from './BackupNetworkFolderDialog.vue'
import BackupFtpFolderDialog from './BackupFtpFolderDialog.vue'

/** i18n */
const { t } = useI18n()
const tenantStore = useTenantStore()
const userStore = useUserStore()
const {
  loadDatabaseInfoList,
  resolveDatabaseDisplayName,
} = useDatabaseInfoCatalog()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}

/**
 * 目标租户固定为当前登录租户，并回填对应库展示名（不可选其它租户）
 * @param target 表单数据
 * @param refreshDefaults 是否按库名刷新默认备份编码/文件名
 */
function applyCurrentTargetTenant(target: Record<string, unknown>, refreshDefaults = false) {
  const code = (tenantStore.tenantCode || '').trim()
  target.targetTenantCode = code
  const dbName = resolveDatabaseDisplayName(code) || ''
  if (dbName) {
    target.targetDatabaseName = dbName
  }
  if (refreshDefaults) {
    fillDefaultBackupCode(target, true)
    target.backupFileName = buildDefaultFileName(
      String(target.targetDatabaseName || ''),
      Number(target.backupType) || 1,
    )
  }
}

const formFields = [
  'tenantCode',
  'companyCode','cultureCode',
  'backupCode',
  'targetTenantCode',
  'targetDatabaseName',
  'backupType',
  'backupPathType',
  'backupPath',
  'backupHost',
  'backupPort',
  'backupUserName',
  'backupPassword',
  'backupFileName',
  'hasBackupPassword',
  'extField',
  'remark']

interface Props {
  formData?: Partial<DatabaseBackup> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})
const FORM_FIELD_DEFAULTS: Record<string, string | number | boolean | undefined> = {
  backupType: 1,
  backupPathType: 4,
  backupCode: '',
  targetTenantCode: '',
  targetDatabaseName: '',
  backupPath: '',
  backupHost: '',
  backupPort: 21,
  backupUserName: '',
  backupPassword: '',
  backupFileName: '',
  hasBackupPassword: false,
  extField: '',
  remark: '',
}

const localDialogOpen = ref(false)
const clientDialogOpen = ref(false)
const networkDialogOpen = ref(false)
const ftpDialogOpen = ref(false)
/** 切换类型时若取消弹窗可回退（默认客户端） */
const previousPathType = ref(4)

/** 默认路径类型：客户端 */
const DEFAULT_PATH_TYPE = 4

/**
 * 规范化路径类型（非法值回退客户端）
 * @param value 原始值
 * @returns {number} 1|2|3|4
 */
function normalizePathType(value: unknown): number {
  const n = Number(value)
  return n === 1 || n === 2 || n === 3 || n === 4 ? n : DEFAULT_PATH_TYPE
}

const pathSummary = computed(() => {
  const type = normalizePathType(formState.backupPathType)
  const path = String(formState.backupPath || '').trim()
  if (!path || path === '\\') return ''
  if (type === 3) {
    const host = formState.backupHost || ''
    const user = formState.backupUserName || ''
    return `${host}${path} (${user})`
  }
  if (type === 2) {
    const user = formState.backupUserName ? ` (${formState.backupUserName})` : ''
    return `${path}${user}`
  }
  return path
})

function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  fillDefaultBackupCode(target)
  fillDefaultFileName(target)
}

/**
 * 本地时间 yyyyMMddHHmmss
 * @returns {string} 14 位时间戳
 */
function formatLocalStamp(): string {
  const d = new Date()
  const pad = (n: number) => String(n).padStart(2, '0')
  return `${d.getFullYear()}${pad(d.getMonth() + 1)}${pad(d.getDate())}${pad(d.getHours())}${pad(d.getMinutes())}${pad(d.getSeconds())}`
}

/**
 * 安全库名片段（仅字母数字下划线点短横）
 * @param dbName 库展示名
 * @returns {string} 安全片段
 */
function sanitizeDbNamePart(dbName: string): string {
  return (dbName || 'db').replace(/[^A-Za-z0-9_\-.]/g, '_')
}

/**
 * 默认备份编码：{库名}_{yyyyMMddHHmmss}（最长 40）
 * @param dbName 库展示名
 * @returns {string} 备份编码
 */
function buildDefaultBackupCode(dbName: string): string {
  const stamp = formatLocalStamp()
  const maxDbLen = Math.max(1, 40 - 1 - stamp.length)
  let safe = sanitizeDbNamePart(dbName)
  if (safe.length > maxDbLen) {
    safe = safe.slice(0, maxDbLen)
  }
  return `${safe}_${stamp}`
}

/**
 * 是否为自动生成的备份编码
 * @param code 备份编码
 * @returns {boolean} 是否匹配默认模式
 */
function isAutoGeneratedBackupCode(code: unknown): boolean {
  const name = String(code ?? '').trim()
  return /^[A-Za-z0-9_\-.]{1,25}_\d{14}$/.test(name)
}

/**
 * 空名或仍为自动编码时按库名刷新
 * @param target 表单数据
 * @param force 强制重生成
 */
function fillDefaultBackupCode(target: Record<string, unknown>, force = false) {
  const current = String(target.backupCode ?? '').trim()
  if (!force && current && !isAutoGeneratedBackupCode(current)) {
    return
  }
  if (!current || force || isAutoGeneratedBackupCode(current)) {
    target.backupCode = buildDefaultBackupCode(String(target.targetDatabaseName || ''))
  }
}

/**
 * 默认备份文件名：z{库名}_{Full|Delta}_{时间戳}.bak
 * @param dbName 库展示名
 * @param backupType 1=Full 2=Delta
 * @returns {string} 文件名
 */
function buildDefaultFileName(dbName: string, backupType: number = 1): string {
  const safe = sanitizeDbNamePart(dbName)
  const typeLabel = backupType === 2 ? 'Delta' : 'Full'
  return `z${safe}_${typeLabel}_${formatLocalStamp()}.bak`
}

/**
 * 是否为自动生成的默认文件名
 * @param fileName 文件名
 * @returns {boolean} 是否匹配默认模式
 */
function isAutoGeneratedFileName(fileName: unknown): boolean {
  const name = String(fileName ?? '').trim()
  return /^z[A-Za-z0-9_\-.]+_(Full|Delta)_\d{14}\.bak$/i.test(name)
}

/**
 * 空名或仍为自动名时按库名/类型刷新默认文件名
 * @param target 表单数据
 * @param force 强制按当前类型/库名重生成
 */
function fillDefaultFileName(target: Record<string, unknown>, force = false) {
  const current = String(target.backupFileName ?? '').trim()
  if (!force && current && !isAutoGeneratedFileName(current)) {
    return
  }
  if (!current || force || isAutoGeneratedFileName(current)) {
    target.backupFileName = buildDefaultFileName(
      String(target.targetDatabaseName || ''),
      Number(target.backupType) || 1,
    )
  }
}

/**
 * 按路径类型打开对应选择弹窗
 * @param type 4=客户端 1=服务器本地 2=文件服务器 3=FTP
 */
function openDialogForType(type: number) {
  const normalized = normalizePathType(type)
  if (normalized === 4) clientDialogOpen.value = true
  else if (normalized === 1) localDialogOpen.value = true
  else if (normalized === 2) networkDialogOpen.value = true
  else if (normalized === 3) ftpDialogOpen.value = true
}

/**
 * 选择路径类型并弹出对应目录浏览器（切换时清空旧路径，避免残缺回填）
 * @param type 路径类型
 */
function onPathTypeSelect(type: number) {
  previousPathType.value = normalizePathType(formState.backupPathType)
  formState.backupPathType = normalizePathType(type)
  formState.backupPath = ''
  formState.backupHost = ''
  formState.backupPort = type === 3 ? 21 : undefined
  formState.backupUserName = ''
  formState.backupPassword = ''
  openDialogForType(formState.backupPathType)
}

/** 重新打开当前类型的路径选择弹窗 */
function reopenPathDialog() {
  openDialogForType(normalizePathType(formState.backupPathType))
}

/**
 * 本地（服务器端）路径确认 — 必须完整盘符绝对路径
 * @param path 服务器绝对路径
 */
function onLocalConfirm(path: string) {
  const full = resolveFolderExplorerConfirmPath('local', '', path)
  if (!isLocalAbsolutePath(full)) {
    return
  }
  formState.backupPathType = 1
  formState.backupPath = full
  formState.backupHost = ''
  formState.backupPort = undefined
  formState.backupUserName = ''
  formState.backupPassword = ''
  previousPathType.value = 1
}

/**
 * 客户端路径确认 — 必须完整本机绝对路径
 * @param path 本机绝对路径
 */
function onClientConfirm(path: string) {
  const full = normalizeClientAbsolutePath(path)
  if (!isClientAbsolutePath(full)) {
    return
  }
  formState.backupPathType = 4
  formState.backupPath = full
  formState.backupHost = ''
  formState.backupPort = undefined
  formState.backupUserName = ''
  formState.backupPassword = ''
  previousPathType.value = 4
}

/**
 * 文件服务器（UNC）路径确认 — 必须完整 \\\\server\\share...
 * @param payload 路径与凭据
 */
function onNetworkConfirm(payload: { path: string; userName?: string; password?: string }) {
  const full = resolveFolderExplorerConfirmPath('unc', '', payload.path)
  if (!isUncAbsolutePath(full)) {
    return
  }
  formState.backupPathType = 2
  formState.backupPath = full
  formState.backupUserName = payload.userName || ''
  if (payload.password) {
    formState.backupPassword = payload.password
  }
  formState.backupHost = ''
  formState.backupPort = undefined
  previousPathType.value = 2
}

/**
 * FTP 路径确认 — 必须完整远程路径（以 / 开头）
 * @param payload 主机/端口/路径/凭据
 */
function onFtpConfirm(payload: {
  host: string
  port: number
  path: string
  userName: string
  password?: string
}) {
  const full = resolveFolderExplorerConfirmPath('ftp', '/', payload.path)
  if (!isFtpAbsolutePath(full)) {
    return
  }
  formState.backupPathType = 3
  formState.backupHost = payload.host
  formState.backupPort = payload.port
  formState.backupPath = full
  formState.backupUserName = payload.userName
  if (payload.password) {
    formState.backupPassword = payload.password
  }
  previousPathType.value = 3
}

watch(
  () => props.formData,
  (val) => {
    if (val?.databaseBackupId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      applyScopeDefaults(next)
      Object.assign(formState, next)
      applyCurrentTargetTenant(formState)
      formState.backupPassword = ''
      previousPathType.value = normalizePathType(formState.backupPathType)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      applyCurrentTargetTenant(formState, true)
      formState.backupPathType = DEFAULT_PATH_TYPE
      previousPathType.value = DEFAULT_PATH_TYPE
      formRef.value?.clearValidate()
    }
  },
  { immediate: true },
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.databaseBackupId) {
      applyScopeDefaults(formState, true)
      applyCurrentTargetTenant(formState, true)
    }
  },
)

/** 切换 Full/Delta 时，若仍为自动文件名则刷新 Full/Delta 段 */
watch(
  () => formState.backupType,
  () => {
    if (props.formData?.databaseBackupId && !isAutoGeneratedFileName(formState.backupFileName)) {
      return
    }
    fillDefaultFileName(formState, true)
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  targetTenantCode: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.databasebackup.targettenantcode') }),
    trigger: 'change',
  }],
  targetDatabaseName: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.databasebackup.targetdatabasename') }),
    trigger: 'blur',
  }],
  backupType: [{
    validator: async (_rule, value) => {
      if (value !== 1 && value !== 2) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.databasebackup.backuptype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  backupPathType: [{
    validator: async () => {
      if (!formState.backupPath) {
        return Promise.reject(t('code.database.database-backup.page.message.pathrequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  const payload = { ...formState }
  payload.backupType = Number(payload.backupType)
  payload.backupPathType = normalizePathType(payload.backupPathType)
  let backupCode = String(payload.backupCode ?? '').trim()
  if (!backupCode && !props.formData?.databaseBackupId) {
    backupCode = buildDefaultBackupCode(String(
      payload.targetDatabaseName || resolveDatabaseDisplayName(payload.targetTenantCode) || '',
    ))
  }
  payload.backupCode = backupCode
  payload.targetTenantCode = (tenantStore.tenantCode || '').trim()
  payload.targetDatabaseName = String(
    payload.targetDatabaseName || resolveDatabaseDisplayName(payload.targetTenantCode) || '',
  ).trim()
  if (!payload.backupCode) {
    payload.backupCode = buildDefaultBackupCode(payload.targetDatabaseName)
  }
  payload.backupPath = String(payload.backupPath ?? '').trim()
  payload.backupFileName = String(payload.backupFileName ?? '').trim()
    || buildDefaultFileName(payload.targetDatabaseName, payload.backupType)
  if (payload.backupPathType !== 3) {
    payload.backupHost = undefined
    payload.backupPort = undefined
  } else {
    payload.backupHost = String(payload.backupHost ?? '').trim()
    payload.backupPort = Number(payload.backupPort) || 21
  }
  // 1=服务器本地、4=客户端：无凭据；2=文件服务器、3=FTP：可带用户名密码
  if (payload.backupPathType === 1 || payload.backupPathType === 4) {
    payload.backupUserName = undefined
    payload.backupPassword = undefined
  } else {
    payload.backupUserName = String(payload.backupUserName ?? '').trim() || undefined
    if (!payload.backupPassword) {
      delete payload.backupPassword
    }
  }
  delete payload.hasBackupPassword
  if (typeof payload.extField === 'string') {
    payload.extField = payload.extField.trim() || undefined
  }
  if (typeof payload.remark === 'string') {
    payload.remark = payload.remark.trim() || undefined
  }
  applyScopeDefaults(payload, true)
  if (props.formData?.databaseBackupId) {
    payload.databaseBackupId = props.formData.databaseBackupId
  }
  return payload
}

function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.databaseBackupId)
  applyCurrentTargetTenant(formState, !props.formData?.databaseBackupId)
  if (!props.formData?.databaseBackupId) {
    formState.backupPathType = DEFAULT_PATH_TYPE
    previousPathType.value = DEFAULT_PATH_TYPE
  } else {
    previousPathType.value = normalizePathType(formState.backupPathType)
  }
  formRef.value?.clearValidate()
}

onMounted(async () => {
  await loadDatabaseInfoList()
  applyCurrentTargetTenant(formState, !props.formData?.databaseBackupId)
  // 新建默认客户端：挂载后弹出客户端目录浏览器
  if (!props.formData?.databaseBackupId) {
    nextTick(() => openDialogForType(DEFAULT_PATH_TYPE))
  }
})

defineExpose({ validate, getValues, resetFields })
</script>
