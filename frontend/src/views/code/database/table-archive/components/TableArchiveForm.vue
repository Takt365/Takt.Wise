<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/table-archive/components -->
<!-- 文件名称：TableArchiveForm.vue -->
<!-- 功能描述：数据表归档配置表单；目标租户/库只读锁定当前登录租户；defineExpose validate/getValues/resetFields -->
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
          :label="t('entity.tablearchive.targettenantcode')"
          name="targetTenantCode"
        >
          <a-input
            :value="lockedTargetTenantCode"
            disabled
            :placeholder="pi.ph('targetTenantCode')"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.tablearchive.targetdatabasename')"
          name="targetDatabaseName"
        >
          <a-input
            :value="lockedTargetDatabaseName"
            disabled
            :placeholder="pi.ph('targetDatabaseName')"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.tablearchive.tablename')"
          name="tableName"
        >
          <a-select
            v-model:value="formState.tableName"
            :placeholder="pi.ph('tableName')"
            :loading="tablesLoading"
            show-search
            option-filter-prop="label"
            allow-clear
            class="w-full"
            @change="handleTableNameChange"
          >
            <a-select-option
              v-for="tbl in tableOptions"
              :key="tbl.tableName"
              :value="tbl.tableName"
              :label="tbl.tableName"
            >
              {{ tbl.tableName }}
              <span v-if="tbl.tableComment" class="text-text-secondary"> — {{ tbl.tableComment }}</span>
            </a-select-option>
          </a-select>
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item name="archiveKeyColumn">
          <template #label>
            <span class="takt-form-ext-field-label">
              <a-tooltip :title="t('code.database.table-archive.page.tip.archivekeycolumn')" placement="top">
                <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
              </a-tooltip>
              <span>{{ t('entity.tablearchive.archivekeycolumn') }}</span>
            </span>
          </template>
          <a-select
            v-model:value="formState.archiveKeyColumn"
            :placeholder="pi.ph('archiveKeyColumn')"
            :disabled="!formState.tableName"
            :loading="columnsLoading"
            show-search
            option-filter-prop="label"
            allow-clear
            class="w-full"
            @change="handleArchiveKeyColumnChange"
          >
            <a-select-option
              v-for="col in columnOptions"
              :key="col.databaseColumnName"
              :value="col.databaseColumnName"
              :label="columnOptionLabel(col)"
            >
              {{ columnOptionLabel(col) }}
            </a-select-option>
          </a-select>
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item name="archiveKeyKind">
          <template #label>
            <span class="takt-form-ext-field-label">
              <a-tooltip :title="t('code.database.table-archive.page.tip.archivekeykind')" placement="top">
                <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
              </a-tooltip>
              <span>{{ t('entity.tablearchive.archivekeykind') }}</span>
            </span>
          </template>
          <TaktSelect
            v-model:value="formState.archiveKeyKind"
            dict-type="sys_archive_key_kind"
            :placeholder="pi.ph('archiveKeyKind')"
            class="w-full"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item name="retainHotYears">
          <template #label>
            <span class="takt-form-ext-field-label">
              <a-tooltip :title="t('code.database.table-archive.page.tip.retainhotyears')" placement="top">
                <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
              </a-tooltip>
              <span>{{ t('entity.tablearchive.retainhotyears') }}</span>
            </span>
          </template>
          <a-input-number
            :value="1"
            :min="1"
            :max="1"
            :precision="0"
            disabled
            class="w-full"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.tablearchive.archivename')"
          name="archiveName"
        >
          <a-input
            :value="archiveNamePreview"
            disabled
            :placeholder="pi.ph('archiveName')"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.tablearchive.archivestatus')"
          name="archiveStatus"
        >
          <TaktSelect
            v-model:value="formState.archiveStatus"
            dict-type="sys_normal_disable"
            :placeholder="pi.ph('archiveStatus')"
            class="w-full"
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
            :rows="3"
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
            :rows="3"
            show-count
            :maxlength="400"
            allow-clear
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 数据表归档配置表单（目标租户/库锁定当前登录租户）
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { RiQuestionLine } from '@remixicon/vue'
import { getDatabaseTableColumnInfoList } from '@/api/code/database/databaseInfo.api'
import { useDatabaseInfoCatalog } from '@/composables/useDatabaseInfoCatalog'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import type { DatabaseTableColumnInfo } from '@/types/code/database/databaseInfo.types'
import type { TableArchive, TableArchiveCreate, TableArchiveUpdate } from '@/types/code/database/tableArchive.types'
import { useTableArchiveI18n } from '../composables/useTableArchiveI18n'

const { t } = useI18n()
const pi = useTableArchiveI18n()
const tenantStore = useTenantStore()
const userStore = useUserStore()
const {
  loadDatabaseInfoList,
  loadTablesForTenant,
  resolveDatabaseDisplayName,
  isTablesLoading,
} = useDatabaseInfoCatalog()

interface Props {
  formData?: Partial<TableArchive> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

const formRef = ref()
const formState = reactive<Record<string, unknown>>({})
const columnOptions = ref<DatabaseTableColumnInfo[]>([])
const columnsLoading = ref(false)
const tableOptions = ref<{ tableName: string; tableComment?: string }[]>([])

const FORM_FIELD_DEFAULTS: Record<string, string | number | undefined> = {
  targetTenantCode: '',
  targetDatabaseName: '',
  tableName: '',
  archiveKeyColumn: '',
  archiveKeyKind: 3,
  retainHotYears: 1,
  archiveName: '',
  archiveStatus: 1,
  extField: '',
  remark: '',
}

/** 当前租户物理表 loading */
const tablesLoading = computed(() => isTablesLoading(tenantStore.tenantCode || ''))

/** 目标租户：固定当前登录租户（只读） */
const lockedTargetTenantCode = computed(() => (tenantStore.tenantCode || '').trim())

/** 目标库：当前租户对应的数据库展示名（只读） */
const lockedTargetDatabaseName = computed(
  () => resolveDatabaseDisplayName(lockedTargetTenantCode.value) || '',
)

/**
 * 归档名称预览：{table}_{yyyyMMddHHmmss|yyyyMM|yyyy}
 */
const archiveNamePreview = computed(() =>
  buildArchiveNamePreview(String(formState.tableName || ''), Number(formState.archiveKeyKind) || 3),
)

/**
 * 列下拉展示文案
 * @param col 列摘要
 */
function columnOptionLabel(col: DatabaseTableColumnInfo): string {
  const name = col.databaseColumnName || ''
  const comment = (col.columnComment || '').trim()
  const type = (col.databaseDataType || '').trim()
  const suffix = [comment, type].filter(Boolean).join(' · ')
  return suffix ? `${name} (${suffix})` : name
}

/**
 * 根据列数据类型推断归档键类型
 * @param dataType 数据库类型
 */
function inferArchiveKeyKind(dataType: string): number {
  const normalized = (dataType || '').toLowerCase()
  if (normalized.includes('date') || normalized.includes('time')) {
    return 1
  }
  if (
    normalized.includes('char')
    || normalized === 'text'
    || normalized === 'ntext'
    || normalized.includes('string')
  ) {
    return 2
  }
  if (
    normalized.includes('int')
    || normalized === 'decimal'
    || normalized === 'numeric'
    || normalized === 'float'
    || normalized === 'real'
  ) {
    return 3
  }
  return 1
}

/**
 * 生成归档名称预览
 * @param tableName 物理表名
 * @param archiveKeyKind 归档键类型
 */
function buildArchiveNamePreview(tableName: string, archiveKeyKind: number): string {
  const safeTable = (tableName || '').trim().toLowerCase()
  if (!safeTable) {
    return ''
  }
  const kindCode =
    archiveKeyKind === 1 ? 'yyyyMMddHHmmss' : archiveKeyKind === 2 ? 'yyyyMM' : 'yyyy'
  const name = `${safeTable}_${kindCode}`
  return name.length <= 200 ? name : name.slice(0, 200)
}

/**
 * 上下文隔离字段
 * @param target 表单数据
 * @param force 强制覆盖
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}

/**
 * 目标租户/库固定为当前登录租户及其对应库（不可选其它租户）
 * @param target 表单数据
 */
function applyCurrentTargetTenant(target: Record<string, unknown>) {
  const code = (tenantStore.tenantCode || '').trim()
  target.targetTenantCode = code
  target.targetDatabaseName = resolveDatabaseDisplayName(code) || ''
}

function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/**
 * 加载当前租户物理表列表
 */
async function loadCurrentTenantTables() {
  const code = (tenantStore.tenantCode || '').trim()
  if (!code) {
    tableOptions.value = []
    return
  }
  const tables = await loadTablesForTenant(code)
  tableOptions.value = (tables ?? []).map((item) => ({
    tableName: item.tableName,
    tableComment: item.tableComment,
  }))
}

/**
 * 加载选中表的列列表
 * @param tableName 物理表名
 */
async function loadColumnsForTable(tableName: string) {
  const tenantCode = (tenantStore.tenantCode || '').trim()
  const name = (tableName || '').trim()
  if (!tenantCode || !name) {
    columnOptions.value = []
    return
  }
  columnsLoading.value = true
  try {
    const list = await getDatabaseTableColumnInfoList(tenantCode, name)
    columnOptions.value = [...(list ?? [])].sort((a, b) =>
      String(a.databaseColumnName).localeCompare(String(b.databaseColumnName), undefined, { sensitivity: 'base' }),
    )
  } catch (error) {
    logger.error('[TableArchiveForm] load columns failed', { error, tableName: name })
    columnOptions.value = []
  } finally {
    columnsLoading.value = false
  }
}

/**
 * 物理表变更：清空列并重新加载
 * @param value 表名
 */
async function handleTableNameChange(value: unknown) {
  formState.archiveKeyColumn = ''
  const tableName = String(value ?? '').trim()
  if (!tableName) {
    columnOptions.value = []
    return
  }
  await loadColumnsForTable(tableName)
}

/**
 * 归档键列变更：按数据类型联动归档键类型
 * @param value 列名
 */
function handleArchiveKeyColumnChange(value: unknown) {
  const columnName = String(value ?? '').trim()
  if (!columnName) {
    return
  }
  const col = columnOptions.value.find((item) => item.databaseColumnName === columnName)
  if (col?.databaseDataType) {
    formState.archiveKeyKind = inferArchiveKeyKind(col.databaseDataType)
  }
}

watch(
  () => props.formData,
  async (val) => {
    Object.keys(formState).forEach((key) => delete formState[key])
    if (val?.tableArchiveId) {
      Object.assign(formState, { ...val })
      formState.retainHotYears = 1
      applyScopeDefaults(formState)
      applyCurrentTargetTenant(formState)
      if (formState.tableName) {
        await loadColumnsForTable(String(formState.tableName))
      }
    } else {
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formState.retainHotYears = 1
      applyScopeDefaults(formState, true)
      applyCurrentTargetTenant(formState)
    }
    formRef.value?.clearValidate()
  },
  { immediate: true },
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  async () => {
    applyScopeDefaults(formState, true)
    applyCurrentTargetTenant(formState)
    if (!props.formData?.tableArchiveId) {
      formState.tableName = ''
      formState.archiveKeyColumn = ''
      columnOptions.value = []
    }
    await loadCurrentTenantTables()
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  targetTenantCode: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.tablearchive.targettenantcode') }),
    trigger: 'change',
  }],
  targetDatabaseName: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.tablearchive.targetdatabasename') }),
    trigger: 'blur',
  }],
  tableName: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.tablearchive.tablename') }),
    trigger: 'change',
  }],
  archiveKeyColumn: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.tablearchive.archivekeycolumn') }),
    trigger: 'change',
  }],
  archiveKeyKind: [{
    validator: async (_rule, value) => {
      const kind = Number(value)
      if (kind !== 1 && kind !== 2 && kind !== 3) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.tablearchive.archivekeykind') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  retainHotYears: [{
    validator: async (_rule, value) => {
      if (Number(value) !== 1) {
        return Promise.reject(t('common.page.form.placeholder.required', { field: t('entity.tablearchive.retainhotyears') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  archiveStatus: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.tablearchive.archivestatus') }),
    trigger: 'change',
  }],
}))

async function validate() {
  applyCurrentTargetTenant(formState)
  formState.retainHotYears = 1
  await formRef.value?.validate()
  return formState
}

function getValues(): TableArchiveCreate | TableArchiveUpdate {
  applyCurrentTargetTenant(formState)
  applyScopeDefaults(formState, true)
  const payload: Record<string, unknown> = {
    targetTenantCode: lockedTargetTenantCode.value,
    targetDatabaseName: lockedTargetDatabaseName.value
      || resolveDatabaseDisplayName(tenantStore.tenantCode || '')
      || '',
    tableName: String(formState.tableName ?? '').trim().toLowerCase(),
    archiveKeyColumn: String(formState.archiveKeyColumn ?? '').trim().toLowerCase(),
    archiveKeyKind: Number(formState.archiveKeyKind) || 3,
    retainHotYears: 1,
    archiveName: buildArchiveNamePreview(
      String(formState.tableName ?? ''),
      Number(formState.archiveKeyKind) || 3,
    ),
    archiveStatus: Number(formState.archiveStatus ?? 1),
    tenantCode: tenantStore.tenantCode,
    companyCode: tenantStore.companyCode,
    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
  }
  if (typeof formState.extField === 'string') {
    payload.extField = formState.extField.trim() || undefined
  }
  if (typeof formState.remark === 'string') {
    payload.remark = formState.remark.trim() || undefined
  }
  if (props.formData?.tableArchiveId) {
    return {
      ...(payload as TableArchiveCreate),
      tableArchiveId: props.formData.tableArchiveId,
    }
  }
  return payload as TableArchiveCreate
}

function resetFields() {
  Object.keys(formState).forEach((key) => delete formState[key])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  formState.retainHotYears = 1
  applyScopeDefaults(formState, !props.formData?.tableArchiveId)
  applyCurrentTargetTenant(formState)
  formRef.value?.clearValidate()
}

onMounted(async () => {
  await loadDatabaseInfoList()
  applyCurrentTargetTenant(formState)
  await loadCurrentTenantTables()
  if (formState.tableName) {
    await loadColumnsForTable(String(formState.tableName))
  }
})

defineExpose({ validate, getValues, resetFields })
</script>
