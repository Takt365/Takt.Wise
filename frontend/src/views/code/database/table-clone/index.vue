<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/table-clone -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：跨租户整表克隆页（一次 1~5 张表；备份窗口确认后执行） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col gap-4 min-h-0">
    <a-typography-title :level="4" class="!mb-0">
      {{ t('code.database.table-data-clone.page.title') }}
    </a-typography-title>
    <a-typography-text type="secondary">
      {{ t('code.database.table-data-clone.page.subtitle') }}
    </a-typography-text>

    <a-card :title="t('code.database.table-data-clone.page.section.scope')">
      <a-row :gutter="16">
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.table-data-clone.page.field.sourcetenant')" required>
            <a-select
              v-model:value="form.sourceTenantCode"
              :placeholder="t('common.page.form.placeholder.select', { field: t('code.database.table-data-clone.page.field.sourcetenant') })"
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
          <a-form-item :label="t('code.database.table-data-clone.page.field.targettenant')" required>
            <a-select
              v-model:value="form.targetTenantCode"
              :placeholder="t('common.page.form.placeholder.select', { field: t('code.database.table-data-clone.page.field.targettenant') })"
              :loading="databaseInfoLoading"
              show-search
              option-filter-prop="label"
              class="w-full"
              @change="handleTargetTenantChange"
            >
              <a-select-option
                v-for="item in databaseInfoList"
                :key="`target-${item.tenantCode}`"
                :value="item.tenantCode"
                :label="`${item.displayName} (${item.tenantCode})`"
              >
                {{ item.displayName }} ({{ item.tenantCode }})
              </a-select-option>
            </a-select>
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.table-data-clone.page.field.sourcedatabase')">
            <a-input :value="sourceDatabaseName" disabled />
          </a-form-item>
        </a-col>
        <a-col :xs="24" :md="12">
          <a-form-item :label="t('code.database.table-data-clone.page.field.targetdatabase')">
            <a-input :value="targetDatabaseName" disabled />
          </a-form-item>
        </a-col>
      </a-row>
    </a-card>

    <a-card :title="t('code.database.table-data-clone.page.section.tables')">
      <div class="flex flex-col gap-3">
        <a-table
          :columns="mappingColumns"
          :data-source="tableMappings"
          :pagination="false"
          :row-key="(row: TableMappingRow) => row.clientKey"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'sourceTableName'">
              <a-select
                v-model:value="record.sourceTableName"
                :placeholder="t('code.database.table-data-clone.page.field.sourcetable')"
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
                  {{ tbl.tableName }}
                </a-select-option>
              </a-select>
            </template>
            <template v-else-if="column.key === 'targetTableName'">
              <a-select
                v-model:value="record.targetTableName"
                :placeholder="t('code.database.table-data-clone.page.field.targettable')"
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
                  {{ tbl.tableName }}
                </a-select-option>
              </a-select>
            </template>
            <template v-else-if="column.key === 'action'">
              <a-button
                type="link"
                danger
                :disabled="tableMappings.length <= 1"
                @click="handleRemoveMappingRow(record.clientKey)"
              >
                {{ t('code.database.table-data-clone.page.tablemapping.removerow') }}
              </a-button>
            </template>
          </template>
        </a-table>
        <div class="flex items-center gap-3">
          <a-button
            :disabled="tableMappings.length >= MAX_TABLE_COUNT"
            @click="handleAddMappingRow"
          >
            {{ t('code.database.table-data-clone.page.tablemapping.addrow') }}
          </a-button>
          <a-typography-text type="secondary">
            {{ t('code.database.table-data-clone.page.tablemapping.maxhint') }}
          </a-typography-text>
        </div>
      </div>
    </a-card>

    <a-card :title="t('code.database.table-data-clone.page.section.options')">
      <a-checkbox v-model:checked="form.preserveIdentityValues">
        {{ t('code.database.table-data-clone.page.field.preserveidentity') }}
      </a-checkbox>
      <div class="mt-4">
        <a-button
          v-permission="'code:database:table:preview'"
          type="primary"
          :loading="previewLoading"
          :disabled="!canSubmitForm"
          @click="handleOpenBackupWindow"
        >
          {{ t('code.database.table-data-clone.page.action.startclone') }}
        </a-button>
      </div>
    </a-card>

    <a-card
      v-if="cloneResult"
      :title="t('code.database.table-data-clone.page.section.result')"
    >
      <a-descriptions bordered :column="3" size="small" class="mb-4">
        <a-descriptions-item :label="t('code.database.table-data-clone.page.result.tablecount')">
          {{ cloneResult.tableCount }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.table-data-clone.page.result.totalsourcerows')">
          {{ cloneResult.totalSourceRowCount }}
        </a-descriptions-item>
        <a-descriptions-item :label="t('code.database.table-data-clone.page.result.totalclonedrows')">
          {{ cloneResult.totalClonedRowCount }}
        </a-descriptions-item>
      </a-descriptions>
      <a-table
        :columns="resultColumns"
        :data-source="cloneResult.tables"
        :pagination="false"
        row-key="targetTableName"
        size="small"
        bordered
      />
    </a-card>

    <TaktModal
      v-model:open="backupModalVisible"
      :title="t('code.database.table-data-clone.page.backupmodaltitle')"
      :use-viewport-size="false"
      width="720px"
      @cancel="handleCloseBackupModal"
    >
      <div v-if="backupPreview" class="flex flex-col gap-3">
        <a-alert
          type="warning"
          :message="t('code.database.table-data-clone.page.preview.summary', { count: backupPreview.targets.length })"
          show-icon
        />
        <a-checkbox v-model:checked="backupConfirmed">
          {{ t('code.database.table-data-clone.page.preview.confirmhint') }}
        </a-checkbox>
        <a-collapse>
          <a-collapse-panel
            v-for="item in backupPreview.targets"
            :key="item.targetTableName"
            :header="item.targetTableName"
          >
            <a-alert
              type="warning"
              :message="t('code.database.table-data-clone.page.preview.warning', { tableName: item.targetTableName })"
              show-icon
              class="mb-2"
            />
            <a-typography-paragraph>{{ formatTargetBackupDesc(item) }}</a-typography-paragraph>
            <a-typography-paragraph>{{ formatTargetClearDesc(item) }}</a-typography-paragraph>
            <a-descriptions bordered size="small" :column="1">
              <a-descriptions-item :label="t('code.database.table-data-clone.page.result.targetrows')">
                {{ item.targetRowCount }}
              </a-descriptions-item>
              <a-descriptions-item :label="t('code.database.table-data-clone.page.result.plannedbackuptable')">
                {{ item.plannedBackupTableName }}
              </a-descriptions-item>
            </a-descriptions>
          </a-collapse-panel>
        </a-collapse>
      </div>
      <template #footer>
        <div class="flex justify-end gap-2">
          <a-button @click="handleCloseBackupModal">
            {{ t('common.page.button.cancel') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:clone'"
            type="primary"
            :loading="cloneLoading"
            :disabled="!backupConfirmed"
            @click="handleConfirmClone"
          >
            {{ t('code.database.table-data-clone.page.action.confirmexecute') }}
          </a-button>
        </div>
      </template>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 跨租户整表克隆页：配置源/目标租户与表映射 → 备份窗口 → 确认后执行
 * @module views/code/database/table-clone
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { cloneTable, getTableClonePreview } from '@/api/code/database/tableClone.api'
import { useDatabaseInfoCatalog } from '@/composables/useDatabaseInfoCatalog'
import type {
  TableClone,
  TableClonePreview,
  TableCloneResult,
  TableCloneTargetPreviewItem,
} from '@/types/code/database/tableClone.types'

/** 备份预览文案键前缀 */
const TABLE_PREVIEW_KEY = 'code.database.table-data-clone.page.preview'

/** 单次最多克隆表数量（与后端 ITaktTableCloneService.MaxTableCountPerRequest 一致） */
const MAX_TABLE_COUNT = 5

/** 表映射行（客户端 row-key） */
interface TableMappingRow {
  clientKey: string
  sourceTableName: string
  targetTableName: string
}

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

/** 租户与选项 */
const form = reactive({
  sourceTenantCode: '',
  targetTenantCode: '',
  preserveIdentityValues: true,
})

/** 表映射行 */
const tableMappings = ref<TableMappingRow[]>([createMappingRow()])
/** 备份弹窗 */
const backupModalVisible = ref(false)
/** 预览 loading */
const previewLoading = ref(false)
/** 克隆 loading */
const cloneLoading = ref(false)
/** 备份预览 */
const backupPreview = ref<TableClonePreview | null>(null)
/** 用户确认勾选 */
const backupConfirmed = ref(false)
/** 克隆结果 */
const cloneResult = ref<TableCloneResult | null>(null)

/** 源库展示名 */
const sourceDatabaseName = computed(() => resolveDatabaseDisplayName(form.sourceTenantCode))
/** 目标库展示名 */
const targetDatabaseName = computed(() => resolveDatabaseDisplayName(form.targetTenantCode))
/** 源租户物理表 */
const sourceTables = computed(() => tablesByTenant.value[form.sourceTenantCode] ?? [])
/** 目标租户物理表 */
const targetTables = computed(() => tablesByTenant.value[form.targetTenantCode] ?? [])

/** 表映射列 */
const mappingColumns = computed<TableColumnsType>(() => [
  {
    title: t('code.database.table-data-clone.page.field.sourcetable'),
    key: 'sourceTableName',
    dataIndex: 'sourceTableName',
  },
  {
    title: t('code.database.table-data-clone.page.field.targettable'),
    key: 'targetTableName',
    dataIndex: 'targetTableName',
  },
  {
    title: t('code.database.table-data-clone.page.tablemapping.actioncolumn'),
    key: 'action',
    width: 100,
  }])

/** 结果明细列 */
const resultColumns = computed<TableColumnsType>(() => [
  { title: t('code.database.table-data-clone.page.field.sourcetable'), dataIndex: 'sourceTableName', key: 'sourceTableName' },
  { title: t('code.database.table-data-clone.page.field.targettable'), dataIndex: 'targetTableName', key: 'targetTableName' },
  { title: t('code.database.table-data-clone.page.result.backuptable'), dataIndex: 'backupTableName', key: 'backupTableName', ellipsis: true },
  { title: t('code.database.table-data-clone.page.result.backeduprows'), dataIndex: 'backedUpRowCount', key: 'backedUpRowCount', width: 100 },
  { title: t('code.database.table-data-clone.page.result.clearedrows'), dataIndex: 'clearedRowCount', key: 'clearedRowCount', width: 100 },
  { title: t('code.database.table-data-clone.page.result.sourcerows'), dataIndex: 'sourceRowCount', key: 'sourceRowCount', width: 100 },
  { title: t('code.database.table-data-clone.page.result.clonedrows'), dataIndex: 'clonedRowCount', key: 'clonedRowCount', width: 100 }])

/** 是否可提交 */
const canSubmitForm = computed(() => {
  if (!form.sourceTenantCode || !form.targetTenantCode) return false
  if (form.sourceTenantCode.trim() === form.targetTenantCode.trim()) return false
  if (!sourceDatabaseName.value || !targetDatabaseName.value) return false
  if (tableMappings.value.length < 1 || tableMappings.value.length > MAX_TABLE_COUNT) return false
  return tableMappings.value.every((row) => row.sourceTableName.trim() && row.targetTableName.trim())
})

/**
 * 创建空映射行
 * @returns 映射行
 */
function createMappingRow(): TableMappingRow {
  return {
    clientKey: `client-${crypto.randomUUID()}`,
    sourceTableName: '',
    targetTableName: '',
  }
}

/**
 * 单张目标表备份步骤说明
 * @param item 目标表预览项
 * @returns 备份步骤文案
 */
function formatTargetBackupDesc(item: TableCloneTargetPreviewItem): string {
  if (item.targetRowCount > 0) {
    return t(`${TABLE_PREVIEW_KEY}.backupwithrows`, {
      tableName: item.targetTableName,
      rowCount: item.targetRowCount,
      backupTable: item.plannedBackupTableName,
    })
  }
  return t(`${TABLE_PREVIEW_KEY}.backupempty`, {
    tableName: item.targetTableName,
    backupTable: item.plannedBackupTableName,
  })
}

/**
 * 单张目标表清空步骤说明
 * @param item 目标表预览项
 * @returns 清空步骤文案
 */
function formatTargetClearDesc(item: TableCloneTargetPreviewItem): string {
  return t(`${TABLE_PREVIEW_KEY}.cleartruncate`, {
    tableName: item.targetTableName,
    rowCount: item.targetRowCount,
  })
}

/** 挂载加载业务库 */
onMounted(async () => {
  try {
    await loadDatabaseInfoList()
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
})

/**
 * 构建克隆请求
 * @param confirm 是否确认备份
 */
function buildRequest(confirm: boolean): TableClone {
  return {
    sourceTenantCode: form.sourceTenantCode.trim(),
    sourceDatabaseName: sourceDatabaseName.value.trim(),
    targetTenantCode: form.targetTenantCode.trim(),
    targetDatabaseName: targetDatabaseName.value.trim(),
    tables: tableMappings.value.map((row) => ({
      sourceTableName: row.sourceTableName.trim(),
      targetTableName: row.targetTableName.trim(),
    })),
    preserveIdentityValues: form.preserveIdentityValues,
    confirmTargetBackupAndClear: confirm,
  }
}

/** 源租户变更 */
async function handleSourceTenantChange() {
  tableMappings.value.forEach((row) => { row.sourceTableName = '' })
  if (!form.sourceTenantCode) return
  try {
    await loadTablesForTenant(form.sourceTenantCode)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 目标租户变更 */
async function handleTargetTenantChange() {
  tableMappings.value.forEach((row) => { row.targetTableName = '' })
  if (!form.targetTenantCode) return
  try {
    await loadTablesForTenant(form.targetTenantCode)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 添加映射行 */
function handleAddMappingRow() {
  if (tableMappings.value.length >= MAX_TABLE_COUNT) return
  tableMappings.value.push(createMappingRow())
}

/**
 * 删除映射行
 * @param clientKey 客户端 row-key
 */
function handleRemoveMappingRow(clientKey: string) {
  if (tableMappings.value.length <= 1) return
  tableMappings.value = tableMappings.value.filter((row) => row.clientKey !== clientKey)
}

/** 打开备份窗口 */
async function handleOpenBackupWindow() {
  if (!canSubmitForm.value) {
    if (form.sourceTenantCode.trim() === form.targetTenantCode.trim()) {
      message.warning(t('code.database.table-data-clone.page.crosstenantrequired'))
    } else if (!tableMappings.value.every((row) => row.sourceTableName && row.targetTableName)) {
      message.warning(t('code.database.table-data-clone.page.tablerequired'))
    }
    return
  }
  previewLoading.value = true
  backupConfirmed.value = false
  try {
    backupPreview.value = await getTableClonePreview(buildRequest(false))
    backupModalVisible.value = true
  } catch (error: unknown) {
    const msg = error instanceof Error ? error.message : t('code.database.table-data-clone.page.clonefailed')
    message.error(msg)
  } finally {
    previewLoading.value = false
  }
}

/** 关闭备份弹窗 */
function handleCloseBackupModal() {
  backupModalVisible.value = false
  backupConfirmed.value = false
}

/** 确认并执行克隆 */
async function handleConfirmClone() {
  if (!backupConfirmed.value || !backupPreview.value) {
    message.warning(t('code.database.table-data-clone.page.previewrequired'))
    return
  }
  cloneLoading.value = true
  try {
    cloneResult.value = await cloneTable(buildRequest(true))
    message.success(t('code.database.table-data-clone.page.clonesuccess'))
    backupModalVisible.value = false
    backupConfirmed.value = false
  } catch (error: unknown) {
    const msg = error instanceof Error ? error.message : t('code.database.table-data-clone.page.clonefailed')
    message.error(msg)
  } finally {
    cloneLoading.value = false
  }
}
</script>
