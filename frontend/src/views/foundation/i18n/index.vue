<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/foundation/i18n -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：翻译管理（列表/转置 CRUD、导入导出、高级查询与列设置） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-i18n">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="translationQueryKeyword"
      :placeholder="t('common.page.form.placeholder.search', { keyword: [t('entity.translation.i18nkey'), t('entity.translation.culturecode'), t('entity.translation.text')].join(t('common.tip.or')) })"
      :loading="translationLoading"
      @search="handleTranslationSearch"
      @reset="handleTranslationReset"
    />
    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="foundation:i18n:create"
      update-permission="foundation:i18n:update"
      delete-permission="foundation:i18n:delete"
      export-permission="foundation:i18n:export"
      :left-actions="translationToolbarLeftActions"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-transpose="true"
      :show-refresh="true"
      :create-disabled="false"
      :update-disabled="!translationSelecmedRow"
      :delete-disabled="translationSelecmedRows.length === 0"
      :create-loading="translationLoading"
      :update-loading="translationLoading"
      :delete-loading="translationLoading"
      :export-loading="translationLoading"
      :refresh-loading="translationLoading"
      @create="handleTranslationCreate"
      @update="handleTranslationUpdate"
      @delete="handleTranslationDelete"
      @export="handleTranslationExport"
      @advanced-query="handleTranslationAdvancedQuery"
      @column-setting="handleTranslationColumnSetting"
      @transpose="handleTranslationTranspose"
      @refresh="handleTranslationRefresh"
    />
    <!-- 列表模式 -->
    <div
      v-if="translationViewMode === 'list'"
      class="foundation-i18n-translation-table-wrap"
    >
      <TaktSingleTable
        :scroll="tableScroll"
        entity-scope="tenant-core"
        :columns="translationDisplayColumns"
        :data-source="translationDataSource"
        :loading="translationLoading"
        :stripe="true"
        :row-key="getTranslationListRowKey"
        :row-selection="translationRowSelection"
        :pagination="false"
        @change="() => {}"
      >
        <!-- 字典列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'resourceType'">
            <TaktDictTag
              :value="getTranslationField(record, 'resourceType')"
              dict-type="sys_resource_type"
            />
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <!-- 转置模式 -->
    <div
      v-else
      class="foundation-i18n-transposed-table-wrap"
    >
      <a-table
        :columns="transposedColumns"
        :data-source="transposedTableRows"
        :row-key="getTransposedTableRowKey"
        :loading="translationLoading"
        :pagination="false"
        size="small"
        bordered
        :scroll="transposedTableScroll"
      >
        <!-- 字典列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'resourceType'">
            <TaktDictTag
              :value="getTranslationField(record, 'resourceType')"
              dict-type="sys_resource_type"
            />
          </template>
        </template>
      </a-table>
    </div>
    <!-- 分页 -->
    <TaktPagination
      v-model:current="translationPage"
      v-model:page-size="translationPageSize"
      :total="translationTotal"
      @change="handleTranslationPaginationChange"
      @show-size-change="handleTranslationPageSizeChange"
    />
    <!-- 翻译表单（多语言转置） -->
    <TaktModal
      v-model:open="translationTransposedFormVisible"
      :title="translationTransposedFormTitle"
      :width="720"
      :confirm-loading="translationTransposedFormLoading"
      @ok="handleTranslationTransposedFormSubmit"
      @cancel="handleTranslationTransposedFormCancel"
    >
      <TranslationTransposedForm
        ref="translationTransposedFormRef"
        :form-data="translationTransposedFormData"
        :loading="translationTransposedFormLoading"
      />
    </TaktModal>
    <!-- 高级查询 -->
    <TaktQueryDrawer
      v-model:open="translationAdvancedVisible"
      :form-model="translationAdvancedForm"
      @submit="handleTranslationAdvancedSubmit"
      @reset="handleTranslationAdvancedReset"
    >
      <a-form-item :label="t('entity.translation.i18nkey')">
        <a-input
          v-model:value="translationAdvancedForm.i18nKey"
          :placeholder="t('common.page.form.placeholder.input', { field: t('entity.translation.i18nkey') })"
          :maxlength="140"
        />
      </a-form-item>
      <a-form-item :label="t('entity.translation.culturecode')">
        <a-input
          v-model:value="translationAdvancedForm.cultureCode"
          :placeholder="t('common.page.form.placeholder.input', { field: t('entity.translation.culturecode') })"
        />
      </a-form-item>
      <a-form-item :label="t('entity.translation.resourcetype')">
        <TaktSelect
          v-model:value="translationAdvancedForm.resourceType"
          dict-type="sys_resource_type"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.translation.resourcetype') })"
        />
      </a-form-item>
      <a-form-item :label="t('entity.translation.resourcegroup')">
        <TaktTreeSelect
          v-model:value="translationAdvancedForm.resourceGroup"
          api-url="TaktMenus/tree-options"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.translation.resourcegroup') })"
        />
      </a-form-item>
    </TaktQueryDrawer>
    <!-- 列设置 -->
    <TaktColumnDrawer
      entity-scope="tenant-core"
      v-model:open="translationColumnDrawerVisible"
      :columns="translationListColumns"
      :checked-keys="translationVisibleColumnKeys"
      :id-column-key="'translationId'"
      :action-column-key="'action'"
      @update:checked-keys="handleTranslationColumnKeysChange"
      @reset="handleTranslationColumnSettingReset"
    />
    <!-- 区域代码列表弹窗 -->
    <CultureTable v-model:open="cultureTableVisible" />
  </div>
</template>

<script setup lang="ts">
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
import TranslationTransposedForm from './components/TranslationTransposedForm.vue'
import CultureTable from './components/CultureTable.vue'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'
import {
  getTranslationList,
  getTranslationTransposedList,
  createTranslation,
  updateTranslation,
  deleteTranslationById,
  exportTranslation
} from '@/api/foundation/translation.api'
import type {
  Translation,
  TranslationQuery,
  TranslationTransposed,
  TranslationTransposedQuery,
  TranslationTransposedResult,
  TranslationCreate,
  TranslationUpdate
} from '@/types/foundation/translation.types'
import { CreateActionColumn, type ActionRecord } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiGlobalLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

const { t } = useI18n()
/** 当前租户编码（翻译 Create/Update DTO 必填，与请求头 X-Tenant-Code 一致） */
const tenantStore = useTenantStore()
/** 字典缓存（高级查询 sys_resource_type 等） */
const dictDataStore = useDictDataStore()

/**
 * 转置弹窗标题（多语言后缀）
 * @param mode create | edit
 * @returns {string} 标题
 */
function buildTransposedFormTitle(mode: 'create' | 'edit'): string {
  const entity = t('entity.translation._self') + t('foundation.i18n.page.translation.multilang.suffix')
  return mode === 'create'
    ? t('common.dialog.title.create', { entity })
    : t('common.dialog.title.edit', { entity })
}

const getTranslationListRowKey = (record: unknown): string => {
  if (record == null || typeof record !== 'object') return ''
  const id = (record as Record<string, unknown>)['translationId']
  return id != null && String(id) !== '' ? String(id) : ''
}

/** 供表格单元格（如 TaktDictTag）安全取行字段，安全取行字段 */
const getTranslationField = (record: any, field: string): any => record?.[field]

const getTransposedTableRowKey = (record: unknown): string => {
  if (record == null || typeof record !== 'object') return ''
  const r = record as Record<string, unknown>
  return [r['i18nKey'], r['resourceType'], r['resourceGroup']].filter((x) => x != null && String(x) !== '').join('|')
}

/**
 * 从异常对象提取可展示消息
 * @param error 捕获的异常
 * @returns {string | undefined} 错误文案
 */
function getErrorMessage(error: unknown): string | undefined {
  if (error instanceof Error) return error.message
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const msg = (error as { message?: unknown }).message
    return typeof msg === 'string' ? msg : undefined
  }
  return undefined
}

/**
 * 翻译高级查询：资源类别（字典 sys_resource_type：frontend/backend）
 * @param value 表单值
 * @returns {string | undefined} 资源类别
 */
function parseTranslationResourceType(value: string | undefined): string | undefined {
  const trimmed = value?.trim()
  return trimmed ? trimmed : undefined
}

/**
 * 翻译高级查询：资源分组
 * @param value 表单值
 * @returns {string | undefined} 资源分组
 */
function parseTranslationResourceGroup(value: string | undefined): string | undefined {
  const trimmed = value?.trim()
  return trimmed ? trimmed : undefined
}

/** 翻译高级查询抽屉表单状态 */
type TranslationAdvancedQueryFormState = {
  KeyWords: string
  i18nKey: string
  cultureCode: string
  resourceType?: string
  resourceGroup: string
}

/** 表格 scroll.y */
const tableScroll = { y: 'calc(100vh - 300px)' } as const
/** 转置表 scroll */
const transposedTableScroll = { x: 'max-content', y: 'calc(100vh - 300px)' } as const

/** 列表 / 转置视图 */
const translationViewMode = ref<'list' | 'transposed'>('list')
/** 列表 loading */
const translationLoading = ref(false)
/** 查询关键字 */
const translationQueryKeyword = ref('')
/** 当前页码 */
const translationPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const translationPageSize = ref(getTaktDefaultPageSize())
/** 总条数 */
const translationTotal = ref(0)
/** 列表数据源 */
const translationDataSource = ref<Translation[]>([])
/** 选中行 keys */
const translationSelecmedRowKeys = ref<(string | number)[]>([])
/** 选中行 */
const translationSelecmedRows = ref<Translation[]>([])
/** 单选行（编辑） */
const translationSelecmedRow = ref<Translation | null>(null)
/** 转置表单可见 */
const translationTransposedFormVisible = ref(false)
/** 转置表单标题 */
const translationTransposedFormTitle = ref(buildTransposedFormTitle('create'))
/** 转置表单提交 loading */
const translationTransposedFormLoading = ref(false)
/** 转置表单数据 */
const translationTransposedFormData = ref<Translation[] | null>(null)
/** 转置表单 ref */
const translationTransposedFormRef = ref<InstanceType<typeof TranslationTransposedForm> | null>(null)
/** 高级查询抽屉 */
const translationAdvancedVisible = ref(false)
/** 高级查询表单 */
const translationAdvancedForm = reactive<TranslationAdvancedQueryFormState>({
  KeyWords: '',
  i18nKey: '',
  cultureCode: '',
  resourceType: undefined,
  resourceGroup: ''
})
/** 列设置抽屉 */
const translationColumnDrawerVisible = ref(false)
/** 可见列 keys */
const translationVisibleColumnKeys = ref<string[]>([])
/** 转置查询结果 */
const transposedResult = ref<TranslationTransposedResult | null>(null)
/** 区域代码列表弹窗 */
const cultureTableVisible = ref(false)

/** 工具栏自定义按钮：区域代码（key=tables → takt-button-tables） */
const translationToolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'tables',
    label: t('foundation.i18n.page.culture.button'),
    icon: RiGlobalLine,
    permission: 'foundation:i18n:list',
    onClick: () => {
      cultureTableVisible.value = true
    }
  }
])

type TransposedRow = { i18nKey: string; resourceType: string; resourceGroup: string }

/**
 * 由高级查询表单构建 Translation 列表查询参数
 * @param form 高级查询表单
 * @param pageIndex 页码
 * @param pageSize 每页条数
 * @param keyWords 关键字
 * @returns {TranslationQuery} 查询参数
 */
function buildTranslationQuery(
  form: TranslationAdvancedQueryFormState,
  pageIndex: number,
  pageSize: number,
  keyWords?: string
): TranslationQuery {
  const query: TranslationQuery = {
    pageIndex,
    pageSize,
  }
  const kw = (keyWords ?? '').trim() || (form.KeyWords ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  if (form.i18nKey?.trim()) {
    query.i18nKey = form.i18nKey.trim()
  }
  if (form.cultureCode?.trim()) {
    query.cultureCode = form.cultureCode.trim()
  }
  const resourceType = parseTranslationResourceType(form.resourceType)
  if (resourceType !== undefined) {
    query.resourceType = resourceType
  }
  const resourceGroup = parseTranslationResourceGroup(form.resourceGroup)
  if (resourceGroup !== undefined) {
    query.resourceGroup = resourceGroup
  }
  return query
}

/**
 * 由高级查询表单构建 Translation 转置查询参数
 * @param form 高级查询表单
 * @param pageIndex 页码
 * @param pageSize 每页条数
 * @param keyWords 关键字
 * @returns {TranslationTransposedQuery} 转置查询参数
 */
function buildTranslationTransposedQuery(
  form: TranslationAdvancedQueryFormState,
  pageIndex: number,
  pageSize: number,
  keyWords?: string
): TranslationTransposedQuery {
  const query: TranslationTransposedQuery = {
    pageIndex,
    pageSize,
  }
  const kw = (keyWords ?? '').trim() || (form.KeyWords ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  if (form.i18nKey?.trim()) {
    query.i18nKey = form.i18nKey.trim()
  }
  if (form.cultureCode?.trim()) {
    query.cultureCode = form.cultureCode.trim()
  }
  const resourceType = parseTranslationResourceType(form.resourceType)
  if (resourceType !== undefined) {
    query.resourceType = resourceType
  }
  const resourceGroup = parseTranslationResourceGroup(form.resourceGroup)
  if (resourceGroup !== undefined) {
    query.resourceGroup = resourceGroup
  }
  return query
}

/** 翻译列表列 */
const translationListColumns = computed(() => [
  { title: t('common.page.entity.id'), dataIndex: 'translationId', key: 'translationId', width: 120 },
  { title: t('entity.translation.i18nkey'), dataIndex: 'i18nKey', key: 'i18nKey', width: 200 },
  { title: t('entity.translation.cultureid'), dataIndex: 'cultureId', key: 'cultureId', width: 120 },
  { title: t('entity.translation.culturecode'), dataIndex: 'cultureCode', key: 'cultureCode', width: 120 },
  { title: t('entity.translation.text'), dataIndex: 'translationText', key: 'translationText', width: 240, ellipsis: true },
  { title: t('entity.translation.resourcetype'), dataIndex: 'resourceType', key: 'resourceType', width: 100 },
  { title: t('entity.translation.resourcegroup'), dataIndex: 'resourceGroup', key: 'resourceGroup', width: 120, ellipsis: true },
  CreateActionColumn<Translation>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:i18n:update',
        onClick: (record: Translation) => handleTranslationEditOne(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:i18n:delete',
        onClick: (record: Translation) => handleTranslationDeleteOne(record)
      }
    ]
  })
])

/** 转置表列 */
const transposedColumns = computed(() => {
  const order = transposedResult.value?.cultureCodeOrder ?? []
  const base: any[] = [
    { title: t('entity.translation.i18nkey'), dataIndex: 'i18nKey', key: 'i18nKey', width: 180, fixed: 'left' as const },
    { title: t('entity.translation.resourcetype'), dataIndex: 'resourceType', key: 'resourceType', width: 90, fixed: 'left' as const },
    { title: t('entity.translation.resourcegroup'), dataIndex: 'resourceGroup', key: 'resourceGroup', width: 100, fixed: 'left' as const }
  ]
  order.forEach((c) => {
    base.push({ title: c, dataIndex: `translations.${c}`, key: `lang_${c}`, width: 120, ellipsis: true })
  })
  base.push(
    CreateActionColumn<ActionRecord>({
      actions: [
        {
          key: 'update',
          label: t('common.page.button.edit'),
          shape: 'plain',
          icon: RiEditLine,
          permission: 'foundation:i18n:update',
          onClick: (record: ActionRecord) => handleTransposedEdit(record as TransposedRow)
        },
        {
          key: 'delete',
          label: t('common.page.button.delete'),
          shape: 'plain',
          icon: RiDeleteBinLine,
          permission: 'foundation:i18n:delete',
          onClick: (record: ActionRecord) => handleTransposedDelete(record as TransposedRow)
        }
      ]
    })
  )
  return base
})

/** 转置表行 */
const transposedTableRows = computed(() => {
  const list = transposedResult.value?.paged?.data ?? []
  const order = transposedResult.value?.cultureCodeOrder ?? []
  const rows: Record<string, unknown>[] = []
  list.forEach((item: TranslationTransposed) => {
    const translations = item.translations
    const row: Record<string, unknown> = {
      i18nKey: item.i18nKey,
      resourceType: item.resourceType,
      resourceGroup: item.resourceGroup ?? ''
    }
    order.forEach((c) => {
      row[`translations.${c}`] = (translations ?? {})[c] ?? ''
    })
    rows.push(row)
  })
  return rows
})

/** 行选择 */
const translationRowSelection = computed(() => ({
  selecmedRowKeys: translationSelecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Translation[]) => {
    translationSelecmedRowKeys.value = keys
    translationSelecmedRows.value = rows
    translationSelecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  }
}))

/** 列设置过滤后的可见列 */
const translationDisplayColumns = computed((): any[] => {
  const keys = translationVisibleColumnKeys.value || []
  const cols: any[] = translationListColumns.value
  if (keys.length === 0) return cols
  const getColumnKey = (col: any): string => {
    const k = col.key || col.dataIndex || col.title
    return k ? String(k) : ''
  }
  const keysSet = new Set(keys.map((k) => String(k)))
  return cols.filter((col: any) => {
    const ck = getColumnKey(col)
    return ck && keysSet.has(ck)
  })
})

/**
 * 按当前视图模式加载列表
 */
function refreshPageData() {
  if (translationViewMode.value === 'list') loadTranslationList()
  else loadTranslationTransposed()
}

/** 租户/公司切换时重载列表 */
useTableRefresh(refreshPageData)

/**
 * 加载翻译分页列表
 */
const loadTranslationList = async () => {
  try {
    translationLoading.value = true
    const query = buildTranslationQuery(translationAdvancedForm, translationPage.value, translationPageSize.value, translationQueryKeyword.value || undefined)
    const result = await getTranslationList(query)
    translationDataSource.value = result?.data ?? []
    translationTotal.value = result?.total ?? 0
  } catch (e) {
    logger.error('[Translation] 加载列表失败', undefined, e)
    message.error(getErrorMessage(e) || t('common.feedback.load.failed', { target: t('entity.translation._self') }))
  } finally {
    translationLoading.value = false
  }
}

/**
 * 加载转置列表
 */
const loadTranslationTransposed = async () => {
  try {
    translationLoading.value = true
    const query = buildTranslationTransposedQuery(
      translationAdvancedForm,
      translationPage.value,
      translationPageSize.value,
      translationQueryKeyword.value || undefined
    )
    const result = await getTranslationTransposedList(query)
    transposedResult.value = result
    translationTotal.value = result.paged?.total ?? 0
  } catch (e) {
    logger.error('[Translation] 加载转置失败', undefined, e)
    message.error(t('common.feedback.load.failed', { target: t('entity.translation._self') }))
  } finally {
    translationLoading.value = false
  }
}

const handleTranslationViewModeChange = () => {
  translationPage.value = getTaktDefaultPageIndex()
  refreshPageData()
}

const handleTranslationTranspose = (isTransposed: boolean) => {
  translationViewMode.value = isTransposed ? 'transposed' : 'list'
  handleTranslationViewModeChange()
}

const handleTranslationSearch = () => {
  translationPage.value = getTaktDefaultPageIndex()
  refreshPageData()
}

const handleTranslationReset = () => {
  translationQueryKeyword.value = ''
  translationPage.value = getTaktDefaultPageIndex()
  Object.assign(translationAdvancedForm, {
    KeyWords: '',
    i18nKey: '',
    cultureCode: '',
    resourceType: undefined,
    resourceGroup: ''
  })
  refreshPageData()
}

const handleTranslationRefresh = () => {
  refreshPageData()
}

const handleTranslationAdvancedQuery = () => { translationAdvancedVisible.value = true }
const handleTranslationColumnSetting = () => { translationColumnDrawerVisible.value = true }
const handleTranslationColumnKeysChange = (keys: (string | number)[]) => {
  translationVisibleColumnKeys.value = keys.map((k) => String(k))
}
const handleTranslationColumnSettingReset = () => { translationVisibleColumnKeys.value = [] }
const handleTranslationAdvancedSubmit = () => {
  translationAdvancedVisible.value = false
  translationPage.value = getTaktDefaultPageIndex()
  refreshPageData()
}
const handleTranslationAdvancedReset = () => {
  Object.assign(translationAdvancedForm, {
    i18nKey: '',
    cultureCode: '',
    resourceType: undefined,
    resourceGroup: ''
  })
}

const handleTranslationPaginationChange = (page: number, size: number) => {
  translationPage.value = page
  translationPageSize.value = size
  refreshPageData()
}
const handleTranslationPageSizeChange = (_current: number, size: number) => {
  translationPage.value = getTaktDefaultPageIndex()
  translationPageSize.value = size
  refreshPageData()
}

const handleTranslationCreate = () => {
  translationTransposedFormTitle.value = buildTransposedFormTitle('create')
  translationTransposedFormData.value = null
  translationTransposedFormVisible.value = true
}

const handleTranslationUpdate = async () => {
  if (!translationSelecmedRow.value) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.translation._self') }))
    return
  }
  await openTransposedEditFromRecord(translationSelecmedRow.value)
}

const handleTranslationEditOne = (record: Translation) => {
  openTransposedEditFromRecord(record)
}

const handleTranslationDeleteOne = async (record: Translation) => {
  if (!record?.translationId) {
    message.warning(t('common.validation.invalid', { field: t('common.page.entity.id') }))
    return
  }
  try {
    translationLoading.value = true
    await deleteTranslationById(record.translationId)
    message.success(t('common.feedback.deleted'))
    refreshPageData()
    const k = record.translationId
    translationSelecmedRowKeys.value = translationSelecmedRowKeys.value.filter((x) => x !== k)
    translationSelecmedRows.value = translationSelecmedRows.value.filter((r) => r.translationId !== k)
    if (translationSelecmedRow.value?.translationId === k) translationSelecmedRow.value = null
  } catch (e) {
    logger.error('[Translation] 删除失败', undefined, e)
    message.error(t('common.feedback.delete.failed'))
  } finally {
    translationLoading.value = false
  }
}

const handleTranslationDelete = async () => {
  if (translationSelecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.translation._self') }))
    return
  }
  const ids = translationSelecmedRows.value.map((r) => r.translationId).filter(Boolean) as string[]
  try {
    translationLoading.value = true
    for (const id of ids) await deleteTranslationById(id)
    message.success(t('common.feedback.deleted'))
    refreshPageData()
    translationSelecmedRowKeys.value = []
    translationSelecmedRows.value = []
    translationSelecmedRow.value = null
  } catch (e) {
    logger.error('[Translation] 删除失败', undefined, e)
    message.error(t('common.feedback.delete.failed'))
  } finally {
    translationLoading.value = false
  }
}

const handleTranslationExport = async () => {
  try {
    translationLoading.value = true
    const query = buildTranslationQuery(translationAdvancedForm, 1, 100000, translationQueryKeyword.value || undefined)
    const exportLabel = t('entity.translation._self')
    const blob = await exportTranslation(query, undefined, exportLabel)
    const ts = new Date()
    const padNum = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `${exportLabel}_${ts.getFullYear()}${padNum(ts.getMonth() + 1)}${padNum(ts.getDate())}${padNum(ts.getHours())}${padNum(ts.getMinutes())}${padNum(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const a = document.createElement('a')
    a.href = url
    a.download = fileName
    document.body.appendChild(a)
    a.click()
    document.body.removeChild(a)
    window.URL.revokeObjectURL(url)
    message.success(t('common.feedback.export.success', { target: t('entity.translation._self') }))
  } catch (e) {
    logger.error('[Translation] 导出失败', undefined, e)
    message.error(t('common.feedback.export.failed', { target: t('entity.translation._self') }))
  } finally {
    translationLoading.value = false
  }
}

/**
 * 按 i18n 键组合加载同组翻译并打开转置编辑表单
 * @param row i18nKey + resourceType + resourceGroup
 */
async function openTransposedEditFromKey(row: TransposedRow) {
  if (!row?.i18nKey) return
  try {
    translationLoading.value = true
    const q: TranslationQuery = {
      pageIndex: 1,
      pageSize: 10000,
      i18nKey: row.i18nKey,
      resourceType: row.resourceType,
      resourceGroup: row.resourceGroup,
    }
    const { data } = await getTranslationList(q)
    if (!data || data.length === 0) {
      message.warning(t('common.validation.not.found', { field: t('entity.translation.i18nkey') }))
      return
    }
    translationTransposedFormTitle.value = buildTransposedFormTitle('edit')
    translationTransposedFormData.value = data
    translationTransposedFormVisible.value = true
  } catch (e) {
    logger.error('[Translation] 获取翻译失败', undefined, e)
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    translationLoading.value = false
  }
}

/**
 * 从列表单条记录打开转置编辑表单
 * @param record 翻译记录
 */
async function openTransposedEditFromRecord(record: Translation) {
  if (!record?.i18nKey) return
  await openTransposedEditFromKey({
    i18nKey: record.i18nKey,
    resourceType: record.resourceType ?? 'frontend',
    resourceGroup: record.resourceGroup ?? ''
  })
}

const handleTranslationTransposedFormSubmit = async () => {
  if (!translationTransposedFormRef.value) return
  try {
    await translationTransposedFormRef.value.validate()
    translationTransposedFormLoading.value = true
    const formData = translationTransposedFormRef.value.getFormData()
    const { i18nKey, resourceType, resourceGroup, remark, translations, translationIds, cultureIds } = formData
    const results: { success: number; fail: number } = { success: 0, fail: 0 }
    for (const [cultureCode, translationText] of Object.entries(translations)) {
      if (!translationText) continue
      const existingId = translationIds[cultureCode]
      const cultureIdStr = String(cultureIds[cultureCode] ?? '')
      const payload: TranslationCreate = {
        tenantCode: tenantStore.tenantCode?.trim() || '',
        i18nKey,
        cultureCode,
        translationText,
        resourceType,
        cultureId: cultureIdStr,
        resourceGroup,
        remark: remark?.trim() || undefined,
      }
      try {
        if (existingId) {
          await updateTranslation(existingId, { ...payload, translationId: existingId } as TranslationUpdate)
        } else {
          await createTranslation(payload)
        }
        results.success++
      } catch {
        results.fail++
      }
    }
    if (results.fail > 0) {
      message.warning(
        t('common.feedback.partial.success', {
          success: results.success,
          fail: results.fail
        })
      )
    } else {
      message.success(t('common.feedback.saved'))
    }
    translationTransposedFormVisible.value = false
    translationTransposedFormData.value = null
    refreshPageData()
  } catch (err: any) {
    if (err?.errorFields) message.warning(t('common.feedback.failed'))
    else {
      logger.error('[Translation] 保存失败', undefined, err)
      message.error(t('common.feedback.failed'))
    }
  } finally {
    translationTransposedFormLoading.value = false
  }
}

const handleTranslationTransposedFormCancel = () => {
  translationTransposedFormVisible.value = false
  translationTransposedFormData.value = null
}

const handleTransposedEdit = (row: TransposedRow) => {
  openTransposedEditFromKey(row)
}

const handleTransposedDelete = async (row: TransposedRow) => {
  if (!row?.i18nKey) return
  try {
    translationLoading.value = true
    const q: TranslationQuery = {
      pageIndex: 1,
      pageSize: 10000,
      i18nKey: row.i18nKey,
      resourceType: row.resourceType,
      resourceGroup: row.resourceGroup,
    }
    const { data } = await getTranslationList(q)
    const ids = (data ?? []).map((r: Translation) => r.translationId).filter(Boolean) as string[]
    if (ids.length === 0) {
      message.warning(t('common.validation.not.found', { field: t('entity.translation.i18nkey') }))
      return
    }
    for (const id of ids) await deleteTranslationById(id)
    message.success(t('common.feedback.deleted'))
    loadTranslationTransposed()
  } catch (e) {
    logger.error('[Translation] 删除失败', undefined, e)
    message.error(t('common.feedback.delete.failed'))
  } finally {
    translationLoading.value = false
  }
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  translationPage.value = getTaktDefaultPageIndex()
  translationPageSize.value = getTaktDefaultPageSize()
  loadTranslationList()
})
</script>

<style scoped lang="css">
.foundation-i18n {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}
.foundation-i18n-translation-table-wrap,
.foundation-i18n-transposed-table-wrap {
  flex: 1;
  min-height: 0;
  min-width: 0;
}
</style>
