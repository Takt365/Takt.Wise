<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/form/components -->
<!-- 文件名称：FormForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程表单新增/编辑表单组件，含步骤与表单设计 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    layout="vertical"
    :model="form"
    :rules="formRules"
  >
    <a-steps
      :current="currentStep"
      :items="stepItems"
      class="form-steps"
    />
    <div class="steps-content">
      <!-- 第一步：表单信息 -->
      <div
        v-show="currentStep === 0"
        class="step-content"
      >
        <a-row :gutter="16">
          <a-col :span="12">
            <a-form-item
              v-if="!form.flowFormId"
              :label="t('common.page.form.numberingRule')"
              name="numberingRuleCode"
              required
            >
              <TaktSelect
                v-model:value="form.numberingRuleCode"
                api-url="TaktNumberings/options"
                :api-params="{ documentType: '表单管理' }"
                style="width: 100%"
                :placeholder="t('common.page.form.placeholder.selectonly')"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item
              :label="t('entity.flowform.formcode')"
              name="formCode"
              required
            >
              <a-input
                v-model:value="form.formCode"
                :placeholder="t('common.page.form.numberingCodePreview')"
                disabled
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item
              :label="t('entity.flowform.formname')"
              name="formName"
              required
            >
              <a-input
                v-model:value="form.formName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.flowform.formname') })"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.flowform.formcategory')">
              <TaktSelect
                v-model="form.formCategory"
                dict-type="sys_form_category"
                style="width: 100%"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowform.formcategory') })"
                allow-clear
                :show-search="true"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.flowform.formtype')">
              <TaktSelect
                v-model="form.formType"
                dict-type="sys_form_type"
                style="width: 100%"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowform.formtype') })"
                allow-clear
                :show-search="true"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.flowform.formversion')">
              <a-input
                v-model:value="form.formVersion"
                :placeholder="t('workflow.form.page.version.placeholder')"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.flowform.formstatus')">
              <TaktSelect
                v-model="form.formStatus"
                dict-type="sys_scheme_status"
                style="width: 100%"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowform.formstatus') })"
                allow-clear
                :show-search="true"
              />
            </a-form-item>
          </a-col>

        </a-row>
      </div>
      <!-- 第二步（新增/编辑一致）：数据源 + 数据表 + 字段网格 -->
      <div
        v-show="currentStep === 1"
        class="step-content"
      >
        <a-form-item :label="t('workflow.form.page.is.datasource.label')">
          <a-switch
            :checked="form.isDatasource === 1"
            :checked-children="t('common.status.yes')"
            :un-checked-children="t('common.status.no')"
            @change="onIsDatasourceChange"
          />
          <div class="text-text-secondary text-sm mt-1">
            {{ t('workflow.form.page.is.datasource.hint') }}
          </div>
        </a-form-item>
        <a-row
          v-if="form.isDatasource === 1"
          :gutter="8"
          class="form-form__ds-row"
        >
          <a-col :span="12">
            <a-form-item :label="t('workflow.form.page.step.data.source')">
              <TaktSelect
                v-model="relatedDataBaseNameModel"
                :options="databaseConfigOptions"
                :placeholder="t('workflow.form.page.data.source.placeholder')"
                :allow-clear="true"
                :show-search="true"
                :filter-option="filterDataSourceOption"
                :loading="databaseConfigLoading"
                style="width: 100%"
                @focus="loadDatabaseConfigs"
                @change="onDataSourceChange"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('workflow.form.page.step.data.table.list')">
              <TaktSelect
                v-model="relatedTableNameModel"
                :options="databaseTableOptions"
                :placeholder="t('workflow.form.page.data.table.placeholder')"
                :allow-clear="true"
                :show-search="true"
                :filter-option="filterDataTableOption"
                :loading="databaseTableLoading"
                :disabled="!form.relatedDataBaseName"
                style="width: 100%"
                @focus="loadDatabaseTables"
                @change="onDataTableChange"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <TaktSingleTable
          v-if="form.isDatasource === 1"
          entity-scope="company"
          class="form-form__field-grid"
          :columns="dataTableColumns"
          :data-source="tableColumnList"
          :pagination="false"
          :stripe="true"
          :show-row-selection="false"
          :include-audit-fields="false"
          row-key="dbColumnName"
          :large-screen-column-count="10"
          :small-screen-column-count="5"
        >
          <template #bodyCell="{ column, record }">
            <!-- C#类型：只读显示（由 DB 类型自动映射） -->
            <template v-if="column.key === 'csharpType'">
              <span>{{ record.csharpType }}</span>
            </template>
            <!-- FrmData 字段名：camelCase（与后端 FrmData 对齐） -->
            <template v-else-if="column.key === 'csharpColumnName'">
              <span>{{ record.csharpColumnName }}</span>
            </template>
            <!-- 必填：开关，1=是，0=否 -->
            <template v-else-if="column.key === 'isRequired'">
              <a-switch
                :checked="record.isRequired === 0 || record.isRequired === '0'"
                :checked-children="t('common.status.yes')"
                :un-checked-children="t('common.status.no')"
                @change="(checked) => { record.isRequired = checked ? 0 : 1 }"
              />
            </template>
            <!-- 显示类型：code_generator_display_type 字典 -->
            <template v-else-if="column.key === 'displayType'">
              <TaktSelect
                v-model="record.displayType"
                dict-type="code_generator_display_type"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.gentablecolumn.htmltype') })"
                allow-clear
                size="small"
                style="width: 100%"
                @change="(v: unknown) => { const t = Array.isArray(v) ? v[0] : v; if (!['select','checkbox','radio'].includes(String(t))) record.dictTypeCode = '' }"
              />
            </template>
            <!-- 字典：选择绑定的字典类型编码 -->
            <template v-else-if="column.key === 'dictTypeCode'">
              <TaktSelect
                v-model="record.dictTypeCode"
                :options="dictTypeOptions"
                :field-names="{ label: 'dictLabel', value: 'extLabel' }"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.gentablecolumn.dicttype') })"
                allow-clear
                size="small"
                style="width: 100%"
                :show-search="true"
                :filter-option="filterDictTypeOption"
              />
            </template>
          </template>
        </TaktSingleTable>
        <div
          v-if="form.isDatasource === 1"
          class="form-form__entity-hint"
        >
          {{ t('workflow.form.page.entity.table.hint') }}
        </div>
        <a-divider
          v-if="form.isDatasource === 1"
          orientation="left"
        >
          {{ t('workflow.form.page.business.binding.title') }}
        </a-divider>
        <a-row
          v-if="form.isDatasource === 1"
          :gutter="16"
        >
          <a-col :span="12">
            <a-form-item :label="t('workflow.form.page.business.status.column.label')">
              <a-select
                v-model:value="businessBinding.businessStatusColumn"
                :options="statusColumnOptions"
                :placeholder="t('workflow.form.page.business.status.column.placeholder')"
                allow-clear
                show-search
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
          <a-col :span="6">
            <a-form-item :label="t('workflow.form.page.status.in.progress')">
              <a-input-number
                v-model:value="businessBinding.statusInProgress"
                :min="0"
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
          <a-col :span="6">
            <a-form-item :label="t('workflow.form.page.status.approved')">
              <a-input-number
                v-model:value="businessBinding.statusApproved"
                :min="0"
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
          <a-col :span="6">
            <a-form-item :label="t('workflow.form.page.status.rejected')">
              <a-input-number
                v-model:value="businessBinding.statusRejected"
                :min="0"
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
          <a-col :span="6">
            <a-form-item :label="t('workflow.form.page.status.cancelled')">
              <a-input-number
                v-model:value="businessBinding.statusCancelled"
                :min="0"
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('workflow.form.page.submit.allowed.statuses.label')">
              <a-select
                v-model:value="submitAllowedStatusTags"
                mode="tags"
                :placeholder="t('workflow.form.page.submit.allowed.statuses.placeholder')"
                style="width: 100%"
              />
            </a-form-item>
          </a-col>
        </a-row>
      </div>
      <!-- 第三步（新增/编辑一致）：表单设计 -->
      <div
        v-show="currentStep === 2"
        class="step-content"
      >
        <a-form-item :label="t('entity.flowform.formconfig')">
          <TaktFormDesigner
            :key="'form-designer-' + locale + '-' + (form.flowFormId ?? 'new')"
            ref="designerRef"
            v-model="formConfigModel"
            height="480px"
            :designer-config="formDesignerConfig"
          />
        </a-form-item>
      </div>
    </div>
    <div class="steps-action">
      <a-button
        v-if="currentStep > 0"
        style="margin-right: 8px"
        @click="prev"
      >
        {{ t('workflow.form.page.step.prev') }}
      </a-button>
      <a-button
        v-if="currentStep < steps.length - 1"
        type="primary"
        @click="next"
      >
        {{ t('workflow.form.page.step.next') }}
      </a-button>
      <a-button
        v-if="currentStep === steps.length - 1"
        type="primary"
        @click="handleDone"
      >
        {{ t('workflow.form.page.step.done') }}
      </a-button>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 流程表单新增/编辑：步骤 1 基本信息、步骤 2 表单设计（takt-form-designer）；对外暴露 validate、getFormData。
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  buildRelatedFormFieldJson,
  parseRelatedFormField,
  snakeColumnToCamelCase,
  type TaktFlowFormBusinessBinding
} from '@/utils/taktFlowFormBinding'
import type { TaktSelectOption } from '@/types/common.types'
import {
  getDatabaseInfoList,
  getDatabaseTableInfoList,
  getDatabaseTableColumnInfoList
} from '@/api/code/database/databaseInfo.api'
import type { DatabaseTableColumnInfo } from '@/types/code/database/databaseInfo.types'

/** 流程表单字段网格行（由 database introspect 映射） */
interface TableColumnItem {
  dbColumnName: string
  columnDescription?: string
  dataType?: string
  length?: number
  decimalDigits?: number
  csharpColumnName?: string
  csharpType?: string
  isRequired?: number
  displayType?: string
  dictTypeCode?: string
  isNullable?: boolean
}
import { getDictTypeOptions } from '@/api/foundation/dictType.api'
import { getFlowEngineApprovalTables } from '@/api/workflow/flowEngine.api'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'
import type { FlowFormCreate } from '@/types/workflow/flowForm.types'
const getStringValue = (obj: unknown, key: string): string | undefined => {
  if (!obj || typeof obj !== 'object') return undefined
  const value = (obj as Record<string, unknown>)[key]
  return value == null ? undefined : String(value)
}
const getBooleanValue = (obj: unknown, key: string): boolean | undefined => {
  if (!obj || typeof obj !== 'object') return undefined
  const value = (obj as Record<string, unknown>)[key]
  return typeof value === 'boolean' ? value : undefined
}
const pickString = (obj: unknown, ...keys: string[]): string => {
  for (const key of keys) {
    const value = getStringValue(obj, key)
    if (value != null && value !== '') return value
  }
  return ''
}
const toErrorMessage = (error: unknown): string => (error instanceof Error ? error.message : String(error))

const { t, locale } = useI18n()
const tenantStore = useTenantStore()

/** 历史种子误写 SqlSugar 上下文类名，API 需 3 位租户码 */
const LEGACY_DATABASE_CONTEXT_NAMES = new Set(['TaktSqlSugarContext'])

/**
 * 解析数据源租户码（relatedDataBaseName → TaktDatabaseInfos tenantCode）
 * @param raw 表单 relatedDataBaseName
 */
function resolveDatabaseTenantCode(raw?: string | null): string {
  const value = raw?.trim() ?? ''
  if (!value || LEGACY_DATABASE_CONTEXT_NAMES.has(value)) {
    return tenantStore.tenantCode?.trim() ?? ''
  }
  return value
}

/** 父组件传入的表单数据（含 flowFormId 表示编辑） */
interface Props {
  form: FlowFormCreate & { flowFormId?: string }
}

const props = defineProps<Props>()

const form = props.form
const relatedDataBaseNameModel = computed<string>({
  get: () => form.relatedDataBaseName ?? '',
  set: (value) => { form.relatedDataBaseName = value }
})
const relatedTableNameModel = computed<string>({
  get: () => form.relatedTableName ?? '',
  set: (value) => { form.relatedTableName = value }
})
const formConfigModel = computed<string>({
  get: () => form.formConfig ?? '',
  set: (value) => { form.formConfig = value }
})

/** 是否编辑态 */
const isEditMode = computed(() => !!form.flowFormId)

/**
 * 按表单分类同步编码规则（WF-FORM0/1/2 ↔ sys_form_category）
 * @param category 表单分类
 */
function resolveFlowFormNumberingRule(category: number | undefined | null): string {
  const n = Number(category)
  if (!Number.isFinite(n) || n < 0 || n > 2) {
    return 'WF-FORM0'
  }
  return `WF-FORM${n}`
}

watch(
  () => form.formCategory,
  (category) => {
    if (isEditMode.value) {
      return
    }
    form.numberingRuleCode = resolveFlowFormNumberingRule(category)
  },
  { immediate: true },
)

useTaktFormNumbering({
  formState: form as Record<string, unknown>,
  isEdit: isEditMode,
  businessCodeField: 'formCode',
})

/** 当前步骤（0=表单信息，1=数据源+字段网格，2=表单设计），提前声明避免 watch 注册时 TDZ */
const currentStep = ref(0)

/** 数据库源下拉：选项与加载状态 */
const databaseConfigOptions = ref<{ value: string; label: string }[]>([])
const databaseConfigLoading = ref(false)
function loadDatabaseConfigs() {
  if (databaseConfigOptions.value.length > 0) return
  databaseConfigLoading.value = true
  getDatabaseInfoList()
    .then((list) => {
      databaseConfigOptions.value = (list ?? []).map((item) => ({
        value: item.tenantCode,
        label: item.displayName ? `${item.displayName} (${item.tenantCode})` : item.tenantCode
      }))
    })
    .catch(() => { databaseConfigOptions.value = [] })
    .finally(() => { databaseConfigLoading.value = false })
}
function filterDataSourceOption(input: string, option?: unknown) {
  const label = String((option as { label?: string })?.label ?? '').toLowerCase()
  return label.includes((input ?? '').trim().toLowerCase())
}

/** 审批业务表白名单（引擎 API，与 TaktApprovalEntityBase 对齐） */
const approvalTableNames = ref<string[]>([])
/** 是否已拉取审批表白名单 */
let approvalTablesLoaded = false

/**
 * 拉取审批业务表白名单（仅一次）
 */
async function ensureApprovalTablesLoaded(): Promise<void> {
  if (approvalTablesLoaded) return
  try {
    approvalTableNames.value = await getFlowEngineApprovalTables()
  } catch {
    approvalTableNames.value = []
  }
  approvalTablesLoaded = true
}

/** 数据表下拉：选项与加载状态（依赖 form.relatedDataBaseName）；仅显示审批白名单内表 */
const databaseTableOptions = ref<{ value: string; label: string }[]>([])
const databaseTableLoading = ref(false)
function loadDatabaseTables() {
  const tenantCode = resolveDatabaseTenantCode(form.relatedDataBaseName)
  if (!tenantCode) { databaseTableOptions.value = []; return }
  databaseTableLoading.value = true
  ensureApprovalTablesLoaded()
    .then(() => getDatabaseTableInfoList(tenantCode))
    .then((list) => {
      const allowed = approvalTableNames.value
      const source = allowed.length
        ? (list ?? []).filter((item) => allowed.includes(item.tableName))
        : (list ?? [])
      databaseTableOptions.value = source.map((item) => ({
        value: item.tableName,
        label: item.tableComment ? `${item.tableName} - ${item.tableComment}` : item.tableName
      }))
    })
    .catch(() => { databaseTableOptions.value = [] })
    .finally(() => { databaseTableLoading.value = false })
}
function filterDataTableOption(input: string, option?: unknown) {
  const label = String((option as { label?: string })?.label ?? '').toLowerCase()
  return label.includes((input ?? '').trim().toLowerCase())
}

/** 表列多选：选项与加载状态（依赖 form.relatedDataBaseName + form.relatedTableName） */
const tableColumnOptions = ref<{ value: string; label: string }[]>([])
const tableColumnLoading = ref(false)
/** 表列网格数据：用于第二步展示字段列表 */
const tableColumnList = ref<TableColumnItem[]>([])
/** 字典类型选项：供「字典」列选择绑定的字典类型编码 */
const dictTypeOptions = ref<TaktSelectOption[]>([])

/** 审计字段 / 基类通用字段 / 实例字段：在字段网格中隐藏 */
/** 可选显隐的审计/通用列（在字段网格中隐藏）；ext_field、remark 必须显示，不在此集合中 */
const AUDIT_DB_COLUMNS = new Set([
  'id',
  'tenant_code',
  'company_code',
  'created_id',
  'created_by',
  'created_at',
  'updated_id',
  'updated_by',
  'updated_at',
  'is_deleted',
  'deleted_id',
  'deleted_by',
  'deleted_at',
  'flow_instance_id'
])

/** DB 类型 -> C# 类型 级联映射（与代码生成器保持一致的精简版） */
const DB_TYPE_TO_CSHARP: Record<string, string> = {
  bigint: 'long',
  bit: 'bool',
  datetime: 'DateTime',
  decimal: 'decimal',
  int: 'int',
  ntext: 'string',
  nvarchar: 'string',
  text: 'string',
  uniqueidentifier: 'Guid',
  varchar: 'string'
}

/** 业务状态列下拉（含 *_status 列） */
const statusColumnOptions = ref<{ value: string; label: string }[]>([])

/** 业务状态与提交规则（写入 RelatedFormField.business） */
const businessBinding = reactive<TaktFlowFormBusinessBinding>({})

/** 允许提交的业务状态（标签输入，同步为 number[]） */
const submitAllowedStatusTags = computed({
  get: () => businessBinding.submitAllowedBusinessStatuses?.map((v) => String(v)) ?? [],
  set: (tags: string[]) => {
    businessBinding.submitAllowedBusinessStatuses = tags
      .map((tag) => parseInt(tag, 10))
      .filter((n) => !Number.isNaN(n))
  }
})

/**
 * 从库表列摘要提取业务状态列选项
 * @param list 列摘要列表
 */
function extractStatusColumnOptions(list: DatabaseTableColumnInfo[]) {
  statusColumnOptions.value = (list ?? [])
    .filter((item) => {
      const name = (item.databaseColumnName ?? '').toLowerCase()
      return name === 'status' || name.endsWith('_status')
    })
    .map((item) => {
      const col = item.databaseColumnName ?? ''
      const label = item.columnComment ? `${col} - ${item.columnComment}` : col
      return { value: col, label }
    })
}

/** 清空业务绑定配置 */
function resetBusinessBinding() {
  businessBinding.businessStatusColumn = undefined
  businessBinding.statusInProgress = undefined
  businessBinding.statusApproved = undefined
  businessBinding.statusRejected = undefined
  businessBinding.statusCancelled = undefined
  businessBinding.submitAllowedBusinessStatuses = undefined
  statusColumnOptions.value = []
}

/**
 * 从 RelatedFormField 还原 business 段
 * @param json RelatedFormField JSON
 */
function applyBusinessBindingFromRelatedFormField(json: string | undefined) {
  resetBusinessBinding()
  const root = parseRelatedFormField(json)
  if (!root.business) return
  businessBinding.businessStatusColumn = root.business.businessStatusColumn
  businessBinding.statusInProgress = root.business.statusInProgress
  businessBinding.statusApproved = root.business.statusApproved
  businessBinding.statusRejected = root.business.statusRejected
  businessBinding.statusCancelled = root.business.statusCancelled
  businessBinding.submitAllowedBusinessStatuses = root.business.submitAllowedBusinessStatuses
    ? [...root.business.submitAllowedBusinessStatuses]
    : undefined
}

/** 是否数据源开关 */
function onIsDatasourceChange(checked: boolean | string | number) {
  const on = checked === true || checked === 1
  form.isDatasource = on ? 1 : 0
  if (!on) {
    form.relatedDataBaseName = ''
    form.relatedTableName = ''
    form.relatedFormField = ''
    tableColumnList.value = []
    resetBusinessBinding()
  }
}

/** 将 introspect 列摘要映射为字段网格行 */
function mapDatabaseColumns(list: DatabaseTableColumnInfo[]): TableColumnItem[] {
  const raw = (list ?? []).filter((item) => {
    const name = (item.databaseColumnName ?? '').toLowerCase()
    if (AUDIT_DB_COLUMNS.has(name)) return false
    if (name === 'status' || name.endsWith('_status')) return false
    return true
  })
  return raw.map((item) => {
    const dbType = (item.databaseDataType ?? '').toLowerCase()
    const mappedCsharp = DB_TYPE_TO_CSHARP[dbType] ?? 'string'
    const dbName = item.databaseColumnName ?? ''
    const csharpName = snakeColumnToCamelCase(dbName)
    const notNullable = item.isNullable === false
    const isRequired = notNullable ? 0 : 1
    return {
      dbColumnName: dbName,
      columnDescription: item.columnComment ?? '',
      dataType: dbType,
      length: item.length,
      decimalDigits: item.decimalDigits,
      csharpType: mappedCsharp,
      csharpColumnName: csharpName,
      isRequired,
      displayType: mappedCsharp === 'DateTime'
        ? 'date'
        : (mappedCsharp === 'int' || mappedCsharp === 'long' || mappedCsharp === 'decimal')
          ? 'InputNumber'
          : 'input',
      dictTypeCode: '',
      isNullable: item.isNullable
    }
  })
}

function loadTableColumns() {
  const tenantCode = resolveDatabaseTenantCode(form.relatedDataBaseName)
  const tableName = form.relatedTableName?.trim()
  if (!tenantCode || !tableName) { tableColumnOptions.value = []; tableColumnList.value = []; return }
  tableColumnLoading.value = true
  getDatabaseTableColumnInfoList(tenantCode, tableName)
    .then((list) => {
      extractStatusColumnOptions(list ?? [])
      // 编辑且已有 form.relatedFormField 字段元数据时，不覆盖（由进入第二步时从 form.relatedFormField 还原）
      if (form.flowFormId && form.relatedFormField?.trim()) {
        const parsedRoot = parseRelatedFormField(form.relatedFormField)
        if (parsedRoot.fields.length > 0) {
          const first = parsedRoot.fields[0]
          if (first?.dbColumnName != null || first?.csharpColumnName != null) return
        }
      }
      const cols = mapDatabaseColumns(list ?? [])
      tableColumnList.value = cols
      tableColumnOptions.value = cols.map((item) => ({
        value: item.dbColumnName,
        label: item.columnDescription ? `${item.dbColumnName} - ${item.columnDescription}` : item.dbColumnName
      }))
    })
    .catch(() => { tableColumnOptions.value = []; tableColumnList.value = [] })
    .finally(() => { tableColumnLoading.value = false })
}
watch(() => form.isDatasource, (v) => {
  if (v !== 1) {
    form.relatedDataBaseName = ''
    form.relatedTableName = ''
    form.relatedFormField = ''
    databaseTableOptions.value = []
    tableColumnOptions.value = []
    tableColumnList.value = []
  }
})
const previousDataSource = ref('')
watch(() => form.relatedDataBaseName, (tenantCode) => {
  if (!tenantCode) {
    form.relatedTableName = ''
    form.relatedFormField = ''
    previousDataSource.value = ''
    databaseTableOptions.value = []
    tableColumnOptions.value = []
    tableColumnList.value = []
    return
  }
  if (previousDataSource.value && previousDataSource.value !== tenantCode) {
    form.relatedTableName = ''
    form.relatedFormField = ''
  }
  previousDataSource.value = tenantCode
  databaseTableOptions.value = []
  tableColumnOptions.value = []
  loadDatabaseTables()
}, { immediate: true })
watch(() => form.relatedTableName, (tableName) => {
  if (!tableName || !form.relatedDataBaseName) {
    tableColumnOptions.value = []
    tableColumnList.value = []
    return
  }
  tableColumnOptions.value = []
  loadTableColumns()
}, { immediate: true })

// 加载字典类型选项（用于「字典」列下拉）
getDictTypeOptions()
  .then((list: TaktSelectOption[]) => { dictTypeOptions.value = list ?? [] })
  .catch(() => { dictTypeOptions.value = [] })

function filterDictTypeOption(input: string, option?: unknown) {
  const label = String((option as { label?: string })?.label ?? '').toLowerCase()
  return label.includes((input ?? '').trim().toLowerCase())
}


/** 是否编辑（有 flowFormId 为编辑，反之为新增）；编辑时表单设计器由 form.formConfig 还原 */
const isEdit = computed(() => !!form.flowFormId)

/** 进入第二步：从 RelatedFormField 还原字段网格与 business 配置 */
watch(currentStep, (step) => {
  if (step !== 1 || !form.relatedFormField?.trim()) return
  const parsedRoot = parseRelatedFormField(form.relatedFormField)
  if (parsedRoot.fields.length > 0) {
    const first = parsedRoot.fields[0]
    if (first?.dbColumnName != null || first?.csharpColumnName != null) {
      tableColumnList.value = parsedRoot.fields as TableColumnItem[]
      tableColumnOptions.value = parsedRoot.fields.map((item) => ({
        value: pickString(item, 'dbColumnName', 'DbColumnName'),
        label: pickString(item, 'columnDescription', 'ColumnDescription')
          ? `${pickString(item, 'dbColumnName', 'DbColumnName')} - ${pickString(item, 'columnDescription', 'ColumnDescription')}`
          : pickString(item, 'dbColumnName', 'DbColumnName')
      }))
    }
  }
  applyBusinessBindingFromRelatedFormField(form.relatedFormField)
})
const formRef = ref()
const designerRef = ref<{ syncToModel?: () => void } | null>(null)

/** 第二步（新增）：数据源变更时清空数据表与字段列表 */
function onDataSourceChange() {
  form.relatedTableName = ''
  tableColumnList.value = []
}
/** 第二步（新增）：选中数据表后获取所有列项生成 FormConfig，并在网格中展示字段 */
async function onDataTableChange() {
  const tenantCode = resolveDatabaseTenantCode(form.relatedDataBaseName)
  const tableName = form.relatedTableName?.trim()
  if (!tenantCode || !tableName) return
  try {
    tableColumnLoading.value = true
    const list = await getDatabaseTableColumnInfoList(tenantCode, tableName)
    extractStatusColumnOptions(list ?? [])
    const cols = mapDatabaseColumns(list ?? [])
    tableColumnList.value = cols
    tableColumnOptions.value = cols.map((item) => ({
      value: item.dbColumnName,
      label: item.columnDescription ? `${item.dbColumnName} - ${item.columnDescription}` : item.dbColumnName
    }))
    form.formConfig = JSON.stringify(cols)
    form.isDatasource = 1
    message.success(t('workflow.form.page.step.data.table.loaded'))
  } catch (err: unknown) {
    const msg =
      (typeof (err as { response?: { data?: unknown } })?.response?.data === 'string' &&
      ((err as { response?: { data?: string } }).response?.data?.trim() ?? '') !== ''
        ? (err as { response?: { data?: string } }).response?.data?.trim()
        : null) ??
      ((err as { response?: { data?: { message?: string } } }).response?.data?.message) ??
      toErrorMessage(err) ??
      t('workflow.form.page.load.form.config.failed')
    message.error(msg)
  } finally {
    tableColumnLoading.value = false
  }
}

/** 设计器 config：语言跟随 vue-i18n（TaktFormDesigner 内映射），关闭设计器自带语言切换 */
const formDesignerConfig = {
  showSaveBtn: true,
  showPreviewBtn: true,
  showJsonPreview: true,
  showLanguage: false,
  showInputData: true
}

/** 步骤配置（新增/编辑一致）：第一步 表单信息 → 第二步 数据源+数据表+字段网格 → 第三步 表单设计 */
const steps = computed(() => [
  { title: t('workflow.form.page.step.form.info'), content: 0 },
  { title: t('workflow.form.page.step.data.source'), content: 1 },
  { title: t('workflow.form.page.step.form.design'), content: 2 }
])
/** 供 a-steps 使用的 items（title 列表） */
const stepItems = computed(() => steps.value.map(item => ({ key: item.title, title: item.title })))

/** 第二步字段网格列配置：列名、描述、DB类型 等 */
const dataTableColumns = computed(() => [
  { title: t('entity.gentablecolumn.databasecolumnname'), dataIndex: 'dbColumnName', key: 'dbColumnName', width: 160 },
  { title: t('entity.gentablecolumn.columncomment'), dataIndex: 'columnDescription', key: 'columnDescription', width: 200 },
  { title: t('entity.gentablecolumn.databasedatatype'), dataIndex: 'dataType', key: 'dataType', width: 120 },
  { title: t('entity.gentablecolumn.csharpdatatype'), dataIndex: 'csharpType', key: 'csharpType', width: 120 },
  { title: t('entity.gentablecolumn.csharpcolumnname'), dataIndex: 'csharpColumnName', key: 'csharpColumnName', width: 160 },
  { title: t('entity.gentablecolumn.length'), dataIndex: 'length', key: 'length', width: 80 },
  { title: t('entity.gentablecolumn.decimaldigits'), dataIndex: 'decimalDigits', key: 'decimalDigits', width: 80 },
  { title: t('entity.gentablecolumn.isrequired'), dataIndex: 'isRequired', key: 'isRequired', width: 80 },
  { title: t('entity.gentablecolumn.htmltype'), dataIndex: 'displayType', key: 'displayType', width: 120 },
  { title: t('entity.gentablecolumn.dicttype'), dataIndex: 'dictTypeCode', key: 'dictTypeCode', width: 160 }
])

/** 当前步骤需要校验的字段名：第一步 表单信息（新增含编码规则） */
const stepFieldNames = computed<Record<number, string[]>>(
  () => ({
    0: isEditMode.value
      ? ['formCode', 'formName']
      : ['numberingRuleCode', 'formCode', 'formName'],
    1: [],
    2: [],
  })
)

/** 表单校验规则：新增须选规则；编码只读预览 */
const formRules = computed(() => ({
  numberingRuleCode: [{
    validator: async (_rule: unknown, value: unknown) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  formCode: [{
    validator: async (_rule: unknown, value: unknown) => {
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  formName: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.flowform.formname') }),
  }],
}))

/**
 * 将当前字段网格(tableColumnList)同步到 form.relatedFormField / form.formConfig：
 * - relatedFormField：保存完整字段元数据数组(JSON)
 * - formConfig：根据字段元数据生成第三步表单配置规则(JSON)，供设计器与发起流程表单使用
 */
function syncFieldsToFormModel() {
  if (!tableColumnList.value.length) return
  const fieldDefs = tableColumnList.value.map((col) => {
    return {
      dbColumnName: pickString(col, 'dbColumnName', 'DbColumnName'),
      columnDescription: pickString(col, 'columnDescription', 'ColumnDescription'),
      dataType: pickString(col, 'dataType', 'DataType'),
      length: col.length,
      decimalDigits: col.decimalDigits,
      isRequired: col.isRequired,
      displayType: col.displayType,
      dictTypeCode: col.dictTypeCode,
      csharpType: col.csharpType,
      csharpColumnName: col.csharpColumnName
    }
  })

  const businessPayload: TaktFlowFormBusinessBinding = {
    businessStatusColumn: businessBinding.businessStatusColumn?.trim() || undefined,
    statusInProgress: businessBinding.statusInProgress,
    statusApproved: businessBinding.statusApproved,
    statusRejected: businessBinding.statusRejected,
    statusCancelled: businessBinding.statusCancelled,
    submitAllowedBusinessStatuses: businessBinding.submitAllowedBusinessStatuses?.length
      ? [...businessBinding.submitAllowedBusinessStatuses]
      : undefined
  }
  form.relatedFormField = buildRelatedFormFieldJson(fieldDefs, businessPayload)

  // 生成 FormConfig 规则：与 FlowStartForm / FlowTaskForm 使用的 FormConfigRule 结构兼容
  const formConfigRule = fieldDefs.map((f) => {
    const field = (f.csharpColumnName || f.dbColumnName || '').toString()
    const title = (f.columnDescription || field || '').toString()
    const dt = (f.dataType || '').toString().toLowerCase()
    const displayType = (f.displayType || '').toString()

    let type: string
    if (displayType === 'textarea') {
      type = 'textarea'
    } else if (displayType === 'select' || displayType === 'checkbox' || displayType === 'radio') {
      type = 'select'
    } else if (displayType === 'date' || dt === 'date' || dt === 'datetime' || dt === 'datetime2') {
      type = 'datePicker'
    } else {
      type = 'input'
    }

    const props: Record<string, unknown> = {}
    if (type === 'textarea') {
      props.rows = 3
    }

    return {
      field,
      title,
      type,
      props
    }
  })

  form.formConfig = JSON.stringify(formConfigRule)
}

/**
 * 校验当前步骤需要校验的字段（见 stepFieldNames）。
 * @returns 通过返回 true，否则返回 false
 */
async function validateCurrentStep(): Promise<boolean> {
  const fields = stepFieldNames.value[currentStep.value]
  if (!fields?.length) return true
  try {
    await formRef.value?.validateFields(fields)
    return true
  } catch {
    return false
  }
}

/**
 * 下一步：先校验当前步骤，通过则将 currentStep 加 1。
 */
async function next() {
  const ok = await validateCurrentStep()
  if (!ok) return
  // 从第二步(字段网格)进入第三步(表单设计)前，同步字段元数据到 RelatedFormField / FormConfig（新增/编辑一致）
  if (currentStep.value === 1) {
    syncFieldsToFormModel()
  }
  currentStep.value++
}

/**
 * 上一步：将 currentStep 减 1。
 */
function prev() {
  currentStep.value--
}

/**
 * 完成：校验当前步骤，通过则提示成功（父组件负责实际提交）。
 */
async function handleDone() {
  const ok = await validateCurrentStep()
  if (!ok) return
  message.success(t('workflow.form.page.step.done'))
}

/**
 * 校验所有步骤的必填字段；未通过时切换到对应步骤并提示，返回 false。
 * @returns 全部通过返回 true，否则 false
 */
async function validateAllSteps(): Promise<boolean> {
  for (let i = 0; i < steps.value.length; i++) {
    const fields = stepFieldNames.value[i]
    if (fields?.length) {
      try {
        await formRef.value?.validateFields(fields)
      } catch {
        currentStep.value = i
        message.warning(t('workflow.form.page.step.validate.fail', { step: i + 1 }))
        return false
      }
    }
  }
  if (form.isDatasource === 1) {
    if (!form.relatedTableName?.trim() || tableColumnList.value.length === 0) {
      currentStep.value = 1
      message.warning(t('workflow.form.page.require.data.table'))
      return false
    }
    syncFieldsToFormModel()
  }
  syncDesignerToModel()
  if (!form.formConfig?.trim()) {
    currentStep.value = 2
    message.warning(t('workflow.form.page.require.form.config'))
    return false
  }
  return true
}

/**
 * 将表单设计器当前数据同步到 form.formConfig（提交前由父组件调用）。
 */
function syncDesignerToModel() {
  designerRef.value?.syncToModel?.()
}

/**
 * 重置步骤与子组件内部状态。
 * 仅由父组件在「打开弹窗」时调用（新增或编辑打开后 nextTick 调用），用于：
 * - 步骤归零，从第一步开始；
 * - 清空数据源/数据表/字段列表的缓存与加载状态，避免上次操作残留。
 * 关闭弹窗时不要调用：下次打开为新增会 resetForm + 再 open + 再调本方法，为编辑会回填 form + open + 再调本方法。
 */
function resetSteps() {
  currentStep.value = 0
  tableColumnList.value = []
  tableColumnOptions.value = []
  databaseConfigOptions.value = []
  databaseTableOptions.value = []
  previousDataSource.value = ''
  databaseConfigLoading.value = false
  databaseTableLoading.value = false
  tableColumnLoading.value = false
  resetBusinessBinding()
  if (form.relatedFormField?.trim()) {
    applyBusinessBindingFromRelatedFormField(form.relatedFormField)
  }
}

defineExpose({
  syncDesignerToModel,
  currentStep,
  validateAllSteps,
  resetSteps
})
</script>

<style scoped lang="css">
.form-steps {
  margin-bottom: 16px;
}
.steps-content {
  margin-top: 16px;
  min-height: 200px;
}
.step-content {
  margin-top: 0;
}
.form-form__entity-hint {
  margin-top: 8px;
  color: var(--ant-color-text-tertiary);
  font-size: 12px;
}
.steps-action {
  margin-top: 24px;
}
</style>

