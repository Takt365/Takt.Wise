<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/data-clone -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：公司级数据克隆页（一次一公司一表；备份窗口确认后执行） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col gap-4 min-h-0">
    <a-typography-title :level="4" class="!mb-0">
      {{ t('code.database.data-clone.page.title') }}
    </a-typography-title>
    <a-typography-text type="secondary">
      {{ t('code.database.data-clone.page.subtitle') }}
    </a-typography-text>

    <a-card :title="t('code.database.data-clone.page.section.source')">
      <a-row :gutter="16">
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.tenant')" required>
            <a-select
              v-model:value="form.sourceTenantCode"
              :placeholder="t('common.page.form.placeholder.select', { field: t('code.database.data-clone.page.field.tenant') })"
              :loading="databaseInfoLoading"
              show-search
              option-filter-prop="label"
              class="w-full"
              @change="handleSourceTenantChange"
            >
              <a-select-option
                v-for="item in databaseInfoList"
                :key="item.tenantCode"
                :value="item.tenantCode"
                :label="`${item.displayName} (${item.tenantCode})`"
              >
                {{ item.displayName }} ({{ item.tenantCode }})
              </a-select-option>
            </a-select>
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.database')">
            <a-input :value="sourceDatabaseName" disabled />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.table')" required>
            <a-select
              v-model:value="form.sourceTableName"
              :placeholder="t('common.page.form.placeholder.select', { field: t('code.database.data-clone.page.field.table') })"
              :disabled="!form.sourceTenantCode"
              :loading="isTablesLoading(form.sourceTenantCode)"
              show-search
              option-filter-prop="label"
              class="w-full"
            >
              <a-select-option
                v-for="tbl in sourceTables"
                :key="tbl.tableName"
                :value="tbl.tableName"
                :label="tbl.tableName"
              >
                {{ tbl.tableName }}{{ tbl.tableComment ? ` - ${tbl.tableComment}` : '' }}
              </a-select-option>
            </a-select>
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.company')" required>
            <a-input
              v-model:value="form.sourceCompanyCode"
              :maxlength="4"
              show-count
              :placeholder="t('common.page.form.placeholder.required', { field: t('code.database.data-clone.page.field.company') })"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-card>

    <a-card :title="t('code.database.data-clone.page.section.target')">
      <a-row :gutter="16">
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.tenant')" required>
            <a-select
              v-model:value="form.targetTenantCode"
              :placeholder="t('common.page.form.placeholder.select', { field: t('code.database.data-clone.page.field.tenant') })"
              :loading="databaseInfoLoading"
              show-search
              option-filter-prop="label"
              class="w-full"
              @change="handleTargetTenantChange"
            >
              <a-select-option
                v-for="item in databaseInfoList"
                :key="item.tenantCode"
                :value="item.tenantCode"
                :label="`${item.displayName} (${item.tenantCode})`"
              >
                {{ item.displayName }} ({{ item.tenantCode }})
              </a-select-option>
            </a-select>
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.database')">
            <a-input :value="targetDatabaseName" disabled />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.table')" required>
            <a-select
              v-model:value="form.targetTableName"
              :placeholder="t('common.page.form.placeholder.select', { field: t('code.database.data-clone.page.field.table') })"
              :disabled="!form.targetTenantCode"
              :loading="isTablesLoading(form.targetTenantCode)"
              show-search
              option-filter-prop="label"
              class="w-full"
            >
              <a-select-option
                v-for="tbl in targetTables"
                :key="tbl.tableName"
                :value="tbl.tableName"
                :label="tbl.tableName"
              >
                {{ tbl.tableName }}{{ tbl.tableComment ? ` - ${tbl.tableComment}` : '' }}
              </a-select-option>
            </a-select>
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.data-clone.page.field.company')" required>
            <a-input
              v-model:value="form.targetCompanyCode"
              :maxlength="4"
              show-count
              :placeholder="t('common.page.form.placeholder.required', { field: t('code.database.data-clone.page.field.company') })"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-card>

    <a-card :title="t('code.database.data-clone.page.section.options')">
      <a-checkbox v-model:checked="form.preserveIdentityValues">
        {{ t('code.database.data-clone.page.field.preserveidentity') }}
      </a-checkbox>
      <div class="mt-4">
        <a-button
          v-permission="'code:database:data:preview'"
          type="primary"
          :loading="previewLoading"
          :disabled="!canSubmitForm"
          @click="handleOpenBackupWindow"
        >
          {{ t('code.database.data-clone.page.action.startclone') }}
        </a-button>
      </div>
    </a-card>

    <a-card
      v-if="cloneResult"
      :title="t('code.database.data-clone.page.section.result')"
    >
      <a-descriptions bordered :column="2" size="small">
        <a-descriptions-item :label="t('code.database.data-clone.page.result.backuptable')">
          {{ cloneResult.backupTableName }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.data-clone.page.result.backeduprows')">
          {{ cloneResult.backedUpRowCount }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.data-clone.page.result.clearedrows')">
          {{ cloneResult.clearedRowCount }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.data-clone.page.result.sourcerows')">
          {{ cloneResult.sourceRowCount }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.data-clone.page.result.clonedrows')">
          {{ cloneResult.clonedRowCount }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.data-clone.page.result.commoncolumns')">
          {{ cloneResult.commonColumnCount }}
        </a-descriptions-item>
        <a-descriptions-item :span="2" :label="t('code.database.data-clone.page.result.summary')">
          {{ cloneResultSummaryText }}
        </a-descriptions-item>
      </a-descriptions>
    </a-card>

    <TaktModal
      v-model:open="backupModalVisible"
      :title="t('code.database.data-clone.page.backupmodaltitle')"
      :use-viewport-size="false"
      width="640px"
      @cancel="handleCloseBackupModal"
    >
      <div v-if="backupPreview" class="flex flex-col gap-3">
        <a-alert type="warning" :message="backupWarningText" show-icon />
        <a-typography-text strong>{{ t('code.database.data-clone.page.backupsummary') }}</a-typography-text>
        <a-typography-paragraph>{{ backupDescText }}</a-typography-paragraph>
        <a-typography-paragraph>{{ clearDescText }}</a-typography-paragraph>
        <a-descriptions bordered size="small" :column="1">
          <a-descriptions-item :label="t('code.database.data-clone.page.field.table')">
            {{ backupPreview.targetTableName }}
          </a-descriptions-item>
          <a-descriptions-item :label="t('code.database.data-clone.page.field.company')">
            {{ backupPreview.targetCompanyCode }}
          </a-descriptions-item>
          <a-descriptions-item :label="t('code.database.data-clone.page.result.targetrows')">
            {{ backupPreview.targetRowCount }}
          </a-descriptions-item>
          <a-descriptions-item :label="t('code.database.data-clone.page.result.plannedbackuptable')">
            {{ backupPreview.plannedBackupTableName }}
          </a-descriptions-item>
        </a-descriptions>
        <a-checkbox v-model:checked="backupConfirmed">
          {{ t('code.database.data-clone.page.preview.confirmhint') }}
        </a-checkbox>
      </div>
      <template #footer>
        <div class="flex justify-end gap-2">
          <a-button @click="handleCloseBackupModal">
            {{ t('common.page.button.cancel') }}
          </a-button>
          <a-button
            v-permission="'code:database:data:clone'"
            type="primary"
            :loading="cloneLoading"
            :disabled="!backupConfirmed"
            @click="handleConfirmClone"
          >
            {{ t('code.database.data-clone.page.action.confirmexecute') }}
          </a-button>
        </div>
      </template>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 公司级数据克隆页：配置源/目标公司与表 → 备份窗口预览 → 确认后执行
 * @module views/code/database/data-clone
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { cloneData, getDataClonePreview } from '@/api/code/database/dataClone.api'
import { useDatabaseInfoCatalog } from '@/composables/useDatabaseInfoCatalog'
import type { DataClonePreview, DataCloneRequest, DataCloneResult } from '@/types/code/database/dataClone.types'

/** 备份预览文案键前缀 */
const DATA_PREVIEW_KEY = 'code.database.data-clone.page.preview'

/** i18n */
const { t } = useI18n()

/** 租户库/表目录 */
const {
  databaseInfoList,
  databaseInfoLoading,
  tablesByTenant,
  loadDatabaseInfoList,
  loadTablesForTenant,
  resolveDatabaseDisplayName,
  isTablesLoading,
} = useDatabaseInfoCatalog()

/** 克隆表单 */
const form = reactive({
  sourceTenantCode: '',
  sourceTableName: '',
  sourceCompanyCode: '',
  targetTenantCode: '',
  targetTableName: '',
  targetCompanyCode: '',
  preserveIdentityValues: true,
})

/** 备份预览弹窗 */
const backupModalVisible = ref(false)
/** 备份预览 loading */
const previewLoading = ref(false)
/** 克隆执行 loading */
const cloneLoading = ref(false)
/** 备份预览数据 */
const backupPreview = ref<DataClonePreview | null>(null)
/** 用户勾选备份确认 */
const backupConfirmed = ref(false)
/** 克隆结果 */
const cloneResult = ref<DataCloneResult | null>(null)

/** 克隆结果备份摘要 */
const cloneResultSummaryText = computed(() => {
  const result = cloneResult.value
  if (!result) return ''
  const targetTableName = form.targetTableName.trim()
  const targetCompanyCode = form.targetCompanyCode.trim()
  const backupPart =
    result.backedUpRowCount > 0
      ? t(`${DATA_PREVIEW_KEY}.resultbackupwithrows`, {
          tableName: targetTableName,
          companyCode: targetCompanyCode,
          rowCount: result.backedUpRowCount,
          backupTable: result.backupTableName,
        })
      : t(`${DATA_PREVIEW_KEY}.resultbackupempty`, {
          tableName: targetTableName,
          companyCode: targetCompanyCode,
          backupTable: result.backupTableName,
        })
  const clearPart = t(`${DATA_PREVIEW_KEY}.resultdeleted`, {
    tableName: targetTableName,
    companyCode: targetCompanyCode,
    rowCount: result.clearedRowCount,
  })
  return t(`${DATA_PREVIEW_KEY}.resultsummary`, { backupPart, clearPart })
})

/** 备份窗口风险提示 */
const backupWarningText = computed(() => {
  const preview = backupPreview.value
  if (!preview) return ''
  return t(`${DATA_PREVIEW_KEY}.warning`, {
    tableName: preview.targetTableName,
    companyCode: preview.targetCompanyCode,
  })
})

/** 备份窗口备份步骤说明 */
const backupDescText = computed(() => {
  const preview = backupPreview.value
  if (!preview) return ''
  if (preview.targetRowCount > 0) {
    return t(`${DATA_PREVIEW_KEY}.backupwithrows`, {
      tableName: preview.targetTableName,
      companyCode: preview.targetCompanyCode,
      rowCount: preview.targetRowCount,
      backupTable: preview.plannedBackupTableName,
    })
  }
  return t(`${DATA_PREVIEW_KEY}.backupempty`, {
    tableName: preview.targetTableName,
    companyCode: preview.targetCompanyCode,
    backupTable: preview.plannedBackupTableName,
  })
})

/** 备份窗口清空步骤说明 */
const clearDescText = computed(() => {
  const preview = backupPreview.value
  if (!preview) return ''
  return t(`${DATA_PREVIEW_KEY}.cleardelete`, {
    tableName: preview.targetTableName,
    companyCode: preview.targetCompanyCode,
    rowCount: preview.targetRowCount,
  })
})

/** 源库展示名 */
const sourceDatabaseName = computed(() => resolveDatabaseDisplayName(form.sourceTenantCode))
/** 目标库展示名 */
const targetDatabaseName = computed(() => resolveDatabaseDisplayName(form.targetTenantCode))
/** 源租户物理表 */
const sourceTables = computed(() => tablesByTenant.value[form.sourceTenantCode] ?? [])
/** 目标租户物理表 */
const targetTables = computed(() => tablesByTenant.value[form.targetTenantCode] ?? [])

/** 表单是否可提交预览 */
const canSubmitForm = computed(() => {
  if (!form.sourceTenantCode || !form.targetTenantCode) return false
  if (!form.sourceTableName || !form.targetTableName) return false
  if (form.sourceCompanyCode.trim().length !== 4 || form.targetCompanyCode.trim().length !== 4) return false
  if (isSameScope()) return false
  return true
})

/** 挂载时加载业务库列表 */
onMounted(async () => {
  try {
    await loadDatabaseInfoList()
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
})

/**
 * 判断源与目标是否完全相同
 * @returns 是否同范围
 */
function isSameScope(): boolean {
  return (
    form.sourceTenantCode.trim().toLowerCase() === form.targetTenantCode.trim().toLowerCase()
    && sourceDatabaseName.value.trim().toLowerCase() === targetDatabaseName.value.trim().toLowerCase()
    && form.sourceTableName.trim().toLowerCase() === form.targetTableName.trim().toLowerCase()
    && form.sourceCompanyCode.trim().toLowerCase() === form.targetCompanyCode.trim().toLowerCase()
  )
}

/**
 * 构建克隆请求 DTO
 * @param confirm 是否携带确认标记
 * @returns 请求体
 */
function buildRequest(confirm: boolean): DataCloneRequest {
  return {
    sourceTenantCode: form.sourceTenantCode.trim(),
    sourceDatabaseName: sourceDatabaseName.value.trim(),
    sourceTableName: form.sourceTableName.trim(),
    sourceCompanyCode: form.sourceCompanyCode.trim(),
    targetTenantCode: form.targetTenantCode.trim(),
    targetDatabaseName: targetDatabaseName.value.trim(),
    targetTableName: form.targetTableName.trim(),
    targetCompanyCode: form.targetCompanyCode.trim(),
    preserveIdentityValues: form.preserveIdentityValues,
    confirmTargetBackupAndClear: confirm,
  }
}

/**
 * 源租户变更：清空表名并加载物理表
 */
async function handleSourceTenantChange() {
  form.sourceTableName = ''
  if (!form.sourceTenantCode) return
  try {
    await loadTablesForTenant(form.sourceTenantCode)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/**
 * 目标租户变更：清空表名并加载物理表
 */
async function handleTargetTenantChange() {
  form.targetTableName = ''
  if (!form.targetTenantCode) return
  try {
    await loadTablesForTenant(form.targetTenantCode)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/**
 * 打开备份窗口：先校验再请求 preview
 */
async function handleOpenBackupWindow() {
  if (!canSubmitForm.value) {
    if (isSameScope()) {
      message.warning(t('code.database.data-clone.page.samescopeerror'))
    }
    return
  }
  previewLoading.value = true
  backupConfirmed.value = false
  try {
    backupPreview.value = await getDataClonePreview(buildRequest(false))
    backupModalVisible.value = true
  } catch (error: unknown) {
    const msg = error instanceof Error ? error.message : t('code.database.data-clone.page.clonefailed')
    message.error(msg)
  } finally {
    previewLoading.value = false
  }
}

/**
 * 关闭备份弹窗
 */
function handleCloseBackupModal() {
  backupModalVisible.value = false
  backupConfirmed.value = false
}

/**
 * 确认并执行克隆
 */
async function handleConfirmClone() {
  if (!backupConfirmed.value || !backupPreview.value) {
    message.warning(t('code.database.data-clone.page.previewrequired'))
    return
  }
  cloneLoading.value = true
  try {
    cloneResult.value = await cloneData(buildRequest(true))
    message.success(t('code.database.data-clone.page.clonesuccess'))
    backupModalVisible.value = false
    backupConfirmed.value = false
  } catch (error: unknown) {
    const msg = error instanceof Error ? error.message : t('code.database.data-clone.page.clonefailed')
    message.error(msg)
  } finally {
    cloneLoading.value = false
  }
}
</script>
