<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/code/generator -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：代码生成表配置 CRUD 页：分页列表、表单弹窗(gen-form)、从库导入(import-table)、代码预览(code-preview)、生成/同步/克隆/初始化。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar
      v-model="queryKeyword"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <TaktToolsBar
      create-permission="code:generator:create"
      update-permission="code:generator:update"
      delete-permission="code:generator:delete"
      import-permission="code:generator:import"
      export-permission="code:generator:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :update-disabled="!selectedRow"
      :delete-disabled="selectedRows.length === 0"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="() => (importVisible = true)"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <TaktSingleTable
      entity-scope="tenant"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getGenTableRowKey"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      @change="handleTableChange"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'genTemplateCategory'">
          <a-tag>{{ record.genTemplateCategory || 'crud' }}</a-tag>
        </template>
      </template>
    </TaktSingleTable>

    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑表单弹窗：宽度 80%，高度 75% -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="80%"
      :centered="true"
      :body-style="{ height: '75vh', overflow: 'auto' }"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <GenForm
        ref="genFormRef"
        :form-data="formData"
        :database-info-list="databaseInfoList"
        :database-tables="databaseTables"
        :database-tables-loading="databaseTablesLoading"
        @config-change="handleImportConfigChange"
      />
    </TaktModal>

    <!-- 导入表弹窗：宽度 80%，高度 75% -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.importfromdb')"
      width="80%"
      :centered="true"
      :body-style="{ height: '75vh', overflow: 'auto' }"
      :footer="null"
      @cancel="importVisible = false"
    >
      <ImportTable
        :open="importVisible"
        :database-info-list="databaseInfoList"
        :database-tables="databaseTables"
        :database-tables-loading="databaseTablesLoading"
        :import-loading="importLoading"
        @config-change="handleImportConfigChange"
        @submit="handleImportSubmit"
      />
    </TaktModal>

    <!-- 代码预览弹窗 -->
    <CodePreview
      v-model="previewVisible"
      :files="previewFiles"
      :loading="previewLoading"
      :validation-issues="previewValidationIssues"
    />

    <!-- 另存为：输入生成路径后确定生成 -->
    <a-modal
      v-model:open="saveAsVisible"
      :title="t('common.dialog.title.saveas')"
      :ok-text="t('common.page.button.ok')"
      :cancel-text="t('common.page.button.cancel')"
      :confirm-loading="loading"
      @ok="handleSaveAsOk"
      @cancel="saveAsVisible = false"
    >
      <p style="margin-bottom: 8px">
        {{ t('code.generator.page.saveaspathhint') }}
      </p>
      <a-input
        v-model:value="saveAsPath"
        :placeholder="t('code.generator.page.saveaspathplaceholder')"
        allow-clear
        @press-enter="handleSaveAsOk"
      />
    </a-modal>

    <!-- 高级查询抽屉 -->
    <a-drawer
      v-model:open="advancedQueryVisible"
      :title="t('common.dialog.title.advancedquery')"
      placement="right"
      width="360"
      @close="advancedQueryVisible = false"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('common.page.form.keyword')">
          <a-input
            v-model:value="queryKeyword"
            :placeholder="t('common.page.form.placeholder.search', { keyword: t('common.page.form.keyword') })"
            allow-clear
          />
        </a-form-item>
        <a-form-item>
          <a-space>
            <a-button
              type="primary"
              @click="handleAdvancedQuerySubmit"
            >
              {{ t('common.page.button.query') }}
            </a-button>
            <a-button @click="handleAdvancedQueryReset">
              {{ t('common.page.button.reset') }}
            </a-button>
          </a-space>
        </a-form-item>
      </a-form>
    </a-drawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      entity-scope="tenant"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 代码生成表配置列表页：TaktGenTables CRUD + TaktGenEngines（导入/生成/预览/初始化）。
 * 子组件：gen-form（表+列配置）、import-table（从库导入）、code-preview（生成预览）。
 */
import { useI18n } from 'vue-i18n'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { RiEditLine, RiDeleteBinLine, RiEyeLine, RiCodeSSlashLine, RiRefreshLine, RiFileCopyLine, RiRestartLine } from '@remixicon/vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import TaktColumnDrawer from '@/components/business/TaktColumnDrawer/index.vue'
import {
  getGenTableList,
  getGenTableById,
  createGenTable,
  updateGenTable,
  deleteGenTableById,
} from '@/api/code/generator/genTable.api'
import type {
  GenTable,
  GenTableQuery,
  GenTableCreate,
  GenTableUpdate,
} from '@/types/code/generator/genTable.types'
import {
  importTableFromDatabase,
  initializeTableFromEntity,
  generateCode,
  previewCode,
  syncTableColumnsFromDatabase,
} from '@/api/code/generator/genEngine.api'
import type { CodeGenGenerateResult, CodeGenPreviewFile, CodeGenPreviewResult, CodeGenPreviewValidationIssue } from '@/types/code/generator/genEngine.types'
import type { TaktBinaryDownload } from '@/types/common.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { getDatabaseInfoList, getDatabaseTableInfoList } from '@/api/code/database/databaseInfo.api'
import type { DatabaseInfo, DatabaseTableInfo } from '@/types/code/database/databaseInfo.types'
import GenForm from './components/GenForm.vue'
import ImportTable from './components/ImportTable.vue'
import CodePreview from './components/CodePreview.vue'
import type { PreviewFile, PreviewValidationIssue } from './components/CodePreview.vue'

/** i18n 翻译函数 */
const { t } = useI18n()

/**
 * 实体展示名（与 entity.gentable._self 一致）
 * @returns {string} 表配置实体名称
 */
const tableConfig = () => t('entity.gentable._self')

/** 查询栏关键词 */
const queryKeyword = ref('')
/** 列表/操作全局 loading */
const loading = ref(false)
/** 当前页表格数据 */
const dataSource = ref<GenTable[]>([])
/** 当前页码（从 1 开始） */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总记录数 */
const total = ref(0)
/** 当前单选行（多选时为空） */
const selectedRow = ref<GenTable | null>(null)
/** 当前多选行 */
const selectedRows = ref<GenTable[]>([])
/** 表格选中行主键 */
const selectedRowKeys = ref<(string | number)[]>([])
/** 新增/编辑表单弹窗显隐 */
const formVisible = ref(false)
/** 表单弹窗标题 */
const formTitle = ref('')
/** 表单提交 loading */
const formLoading = ref(false)
/** gen-form 组件实例 */
const genFormRef = ref<InstanceType<typeof GenForm>>()
/** 传入 gen-form 的表配置（null 为新增） */
const formData = ref<Partial<GenTable> | null>(null)
/** 从库导入弹窗显隐 */
const importVisible = ref(false)
/** 可 introspect 的租户业务库列表 */
const databaseInfoList = ref<DatabaseInfo[]>([])
/** 当前租户下可选物理表（导入选表） */
const databaseTables = ref<DatabaseTableInfo[]>([])
/** 物理表列表加载中 */
const databaseTablesLoading = ref(false)
/** 导入请求进行中 */
const importLoading = ref(false)
/** 代码预览弹窗显隐 */
const previewVisible = ref(false)
/** 预览文件列表 */
const previewFiles = ref<PreviewFile[]>([])
/** 预览请求 loading */
const previewLoading = ref(false)
/** 预览前校验问题 */
const previewValidationIssues = ref<PreviewValidationIssue[]>([])
/** 高级查询抽屉显隐 */
const advancedQueryVisible = ref(false)
/** 列设置抽屉显隐 */
const columnSettingVisible = ref(false)
/** 表格可见列 key 列表 */
const visibleColumnKeys = ref<string[]>([])
/** 另存为（自定义路径生成）弹窗显隐 */
const saveAsVisible = ref(false)
/** 另存为输入的生成路径 */
const saveAsPath = ref('')
/** 另存为操作对应的表配置行 */
const saveAsRecord = ref<GenTable | null>(null)

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
 * 归一化预览 API 响应（兼容 camelCase / PascalCase 字段名）
 * @param raw 解包后的 preview 数据
 * @returns {CodeGenPreviewResult} 标准预览结果
 */
function normalizePreviewResult(raw: CodeGenPreviewResult | Record<string, unknown> | null | undefined): CodeGenPreviewResult {
  if (!raw || typeof raw !== 'object') {
    return { isValid: false, previewFiles: [], validationIssues: [] }
  }
  const bag = raw as Record<string, unknown>
  const rawFiles = (bag.previewFiles ?? bag.PreviewFiles ?? []) as Array<Record<string, unknown>>
  const rawIssues = (bag.validationIssues ?? bag.ValidationIssues ?? []) as Array<Record<string, unknown>>
  return {
    isValid: Boolean(bag.isValid ?? bag.IsValid ?? false),
    previewFiles: rawFiles.map((item) => ({
      path: String(item.path ?? item.Path ?? ''),
      content: String(item.content ?? item.Content ?? ''),
      isExisting: Boolean(item.isExisting ?? item.IsExisting ?? false),
    })),
    validationIssues: rawIssues.map((item) => ({
      templateKey: String(item.templateKey ?? item.TemplateKey ?? ''),
      targetPath: item.targetPath != null ? String(item.targetPath) : (item.TargetPath != null ? String(item.TargetPath) : undefined),
      message: String(item.message ?? item.Message ?? ''),
    })) as CodeGenPreviewValidationIssue[],
  }
}

/**
 * 读取表配置主键（优先 genTableId，兼容历史 id）
 * @param record 表配置行
 * @returns {string | number | undefined} 主键
 */
function getTableId(record: GenTable): string | number | undefined {
  if (record.genTableId != null && String(record.genTableId) !== '') return String(record.genTableId)
  const legacyId = (record as unknown as Record<string, unknown>)['id']
  if (typeof legacyId === 'string' || typeof legacyId === 'number') return legacyId
  return undefined
}

/**
 * 表格 rowKey（与 TaktSingleTable 一致）
 * @param record 表格行 Record
 * @returns {string} genTableId 字符串
 */
function getGenTableRowKey(record: Record<string, unknown>): string {
  return String(record['genTableId'] ?? '')
}

/**
 * 由表配置解析实体类型全名（供 initializeTableFromEntity 使用）
 * @param record 表配置行
 * @returns {string} 实体类型全名
 */
function resolveEntityTypeFullName(record: GenTable): string {
  const entityClassName = String(record.entityClassName ?? '').trim()
  if (!entityClassName) return ''
  if (entityClassName.includes('.')) return entityClassName
  const entityNamespace = String(record.entityNamespace ?? 'Takt.Domain.Entities').trim()
  return `${entityNamespace}.${entityClassName}`
}

/** 表格列定义（含操作列与权限码 code:generator:*） */
const columns = computed(() => [
  { title: t('entity.gentable.tablename'), dataIndex: 'tableName', key: 'tableName', width: 180, ellipsis: true },
  { title: t('entity.gentable.tablecomment'), dataIndex: 'tableComment', key: 'tableComment', width: 140, ellipsis: true },
  { title: t('entity.gentable.entityclassname'), dataIndex: 'entityClassName', key: 'entityClassName', width: 140 },
  { title: t('entity.gentable.genmodulename'), dataIndex: 'genModuleName', key: 'genModuleName', width: 100 },
  { title: t('entity.gentable.genbusinessname'), dataIndex: 'genBusinessName', key: 'genBusinessName', width: 100 },
  { title: t('entity.gentable.gentemplatecategory'), dataIndex: 'genTemplateCategory', key: 'genTemplateCategory', width: 80 },
  CreateActionColumn<GenTable>({
    actions: [
      {
        key: 'preview',
        label: t('common.page.button.preview'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'code:generator:preview',
        onClick: (record: GenTable) => handlePreviewOne(record)
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'code:generator:update',
        onClick: (record: GenTable) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'code:generator:delete',
        onClick: (record: GenTable) => handleDeleteOne(record)
      },
      {
        key: 'generate',
        label: t('common.page.button.generate'),
        shape: 'plain',
        icon: RiCodeSSlashLine,
        permission: 'code:generator:generate',
        onClick: (record: GenTable) => handleGenerateOne(record)
      },
      {
        key: 'sync',
        label: t('common.page.button.sync'),
        shape: 'plain',
        icon: RiRefreshLine,
        permission: 'code:generator:sync',
        onClick: (record: GenTable) => handleSync(record)
      },
      {
        key: 'initialize',
        label: t('common.page.button.initialize'),
        shape: 'plain',
        icon: RiRestartLine,
        permission: 'code:generator:initialize',
        visible: (record: GenTable) => record.inDatabase === 1,
        onClick: (record: GenTable) => handleInitialize(record)
      },
      {
        key: 'clone',
        label: t('common.page.button.clone'),
        shape: 'plain',
        icon: RiFileCopyLine,
        permission: 'code:generator:clone',
        onClick: (record: GenTable) => handleClone(record)
      }
    ]
  })
])

/** 按列设置过滤后的展示列 */
const displayColumns = computed<TableColumnsType>(() => {
  const keys = visibleColumnKeys.value || []
  const cols = (columns.value || []) as Array<{ key?: unknown }>
  if (keys.length === 0) return cols as TableColumnsType
  const keySet = new Set(keys)
  return cols.filter((c) => keySet.has(String(c.key))) as TableColumnsType
})

/** 表格行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: GenTable[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  }
}))

/**
 * 行点击切换选中（自定义行事件）
 * @param record 点击的行
 * @returns 行 onClick 配置
 */
const onClickRow = (record: GenTable) => ({
  onClick: () => {
    const key = String(record.genTableId ?? '')
    const idx = selectedRowKeys.value.indexOf(key)
    if (idx > -1) selectedRowKeys.value.splice(idx, 1)
    else selectedRowKeys.value.push(key)
    selectedRows.value = dataSource.value.filter((item: GenTable) => selectedRowKeys.value.includes(String(item.genTableId ?? '')))
    selectedRow.value = selectedRows.value.length === 1 ? (selectedRows.value[0] ?? null) : null
  }
})

/**
 * 分页加载表配置列表
 * @returns {Promise<void>}
 */
async function loadData() {
  try {
    loading.value = true
    const params: GenTableQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      genBusinessName: ''
    }
    const kw = (queryKeyword.value ?? '').trim()
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const response = await getGenTableList(params)
    dataSource.value = response?.data ?? []
    total.value = response?.total ?? 0
  } catch (error: unknown) {
    logger.error('[GenTable] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/**
 * 查询：重置到第 1 页并加载
 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/**
 * 重置查询条件并加载
 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = 1
  loadData()
}

/**
 * 表格 change 占位（排序/筛选由后端扩展时可实现）
 * @param _pagination 分页
 * @param _filters 筛选
 * @param _sorter 排序
 */
function handleTableChange(
  _pagination: unknown,
  _filters: unknown,
  _sorter: unknown
) {}

/**
 * 分页页码变更
 * @param page 新页码
 * @param size 每页条数
 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/**
 * 每页条数变更
 * @param current 当前页
 * @param size 新每页条数
 */
function handlePaginationSizeChange(current: number, size: number) {
  currentPage.value = current
  pageSize.value = size
  loadData()
}

/**
 * 打开新增表单弹窗
 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: tableConfig() })
  formData.value = null
  formVisible.value = true
}

/**
 * 打开编辑表单（先拉详情）
 * @param record 列表行
 * @returns {Promise<void>}
 */
async function handleEdit(record: GenTable) {
  formTitle.value = t('common.dialog.title.edit', { entity: tableConfig() })
  const id = getTableId(record)
  if (!id) {
    formData.value = { ...record, genTableId: record.genTableId }
    formVisible.value = true
    return
  }
  try {
    loading.value = true
    const detail = await getGenTableById(String(id))
    const fallbackTableId = String(id)
    formData.value = {
      ...detail,
      genTableId: detail.genTableId != null ? String(detail.genTableId) : (fallbackTableId || undefined)
    } as Partial<GenTable>
    formVisible.value = true
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.load.failed', { target: tableConfig() }))
  } finally {
    loading.value = false
  }
}

/**
 * 工具栏编辑：编辑当前单选行
 */
function handleUpdate() {
  if (selectedRow.value) handleEdit(selectedRow.value)
  else message.warning(t('common.page.action.warnselecttoaction', { action: t('common.page.button.edit'), entity: tableConfig() }))
}

/**
 * 删除单行（确认框）
 * @param record 待删行
 */
function handleDeleteOne(record: GenTable) {
  Modal.confirm({
    title: t('common.page.action.confirmdelete'),
    content: t('common.page.confirm.deleteentity', { entity: tableConfig(), name: record.tableName ?? '' }),
    onOk: async () => {
      try {
        loading.value = true
        await deleteGenTableById(String(record.genTableId))
        message.success(t('common.feedback.deleted'))
        loadData()
      } catch (e: unknown) {
        message.error(getErrorMessage(e) || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

/**
 * 批量删除选中行
 */
function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.page.action.warnselecttoaction', { action: t('common.page.button.delete'), entity: tableConfig() }))
    return
  }
  Modal.confirm({
    title: t('common.page.action.confirmdelete'),
    content: t('common.page.confirm.deletecountentity', { count: selectedRows.value.length, entity: tableConfig() }),
    onOk: async () => {
      try {
        loading.value = true
        await Promise.all(selectedRows.value.map((r: GenTable) => deleteGenTableById(String(r.genTableId))))
        message.success(t('common.feedback.deleted'))
        selectedRowKeys.value = []
        selectedRows.value = []
        selectedRow.value = null
        loadData()
      } catch (e: unknown) {
        message.error(getErrorMessage(e) || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

/**
 * 解析预览/覆盖检测用的目标根路径（GenMethod=1/2；zip 模式不传）
 * @param record 表配置
 * @returns 目标根路径或 undefined（由后端按 GenMethod 解析）
 */
function resolvePreviewTargetBasePath(record: GenTable): string | undefined {
  const genMethod = record.genMethod != null ? Number(record.genMethod) : 0
  if (genMethod !== 1 && genMethod !== 2) return undefined
  const genPathRaw = String(record.genPath ?? '').trim()
  if (!genPathRaw || genPathRaw === '/') return undefined
  return genPathRaw
}

/**
 * 构建生成 API 请求体
 * @param record 表配置
 * @returns 生成请求 DTO
 */
function buildGenerateRequest(record: GenTable) {
  const genMethod = record.genMethod != null ? Number(record.genMethod) : 0
  const genPath = String(record.genPath ?? '').trim()
  return {
    templates: {} as Record<string, string>,
    genMethod,
    ...(genMethod === 1 && genPath && genPath !== '/' ? { genPath } : {}),
  }
}

/**
 * 调用生成 API 并按 genMethod 处理结果（zip 下载或落盘成功提示）
 * @param record 表配置（可含覆盖后的 genPath）
 * @returns {Promise<void>}
 */
async function doGenerateCode(record: GenTable) {
  const id = String(record.genTableId)
  const genMethod = record.genMethod != null ? Number(record.genMethod) : 0
  const payload = buildGenerateRequest(record)

  if (genMethod === 0) {
    const exportMeta = await generateCode(id, payload) as TaktBinaryDownload
    const ts = new Date().toISOString().replace(/[-:T]/g, '').slice(0, 14)
    const fallbackBase = `${record.tableName ?? id}_${ts}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: exportMeta.contentDisposition ?? null,
      contentType: exportMeta.contentType ?? null,
      fallbackBase,
    })
    const url = window.URL.createObjectURL(exportMeta.blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('code.generator.page.codegenerateddownload'))
    return
  }

  const delivery = await generateCode(id, payload) as CodeGenGenerateResult
  if (!delivery?.fileCount) {
    message.warning(t('code.generator.page.nocodegenerated'))
    return
  }
  message.success(
    t('code.generator.page.gensuccesscount', { count: delivery.fileCount })
      + (delivery.basePath ? ` → ${delivery.basePath}` : ''),
  )
}

/**
 * 打开另存为路径弹窗（自定义路径生成）
 * @param record 当前表配置
 */
function showSaveAsPathModal(record: GenTable) {
  saveAsRecord.value = record
  saveAsPath.value = String(record.genPath ?? '').trim() || '/'
  saveAsVisible.value = true
}

/**
 * 另存为确认：使用新路径执行生成
 * @returns {Promise<void>}
 */
async function handleSaveAsOk() {
  const record = saveAsRecord.value
  const newPath = saveAsPath.value?.trim() || ''
  if (!record) {
    saveAsVisible.value = false
    return
  }
  if (!newPath) {
    message.warning(t('common.page.form.placeholder.required', { field: t('entity.gentable.genpath') }))
    return
  }
  try {
    loading.value = true
    await doGenerateCode({ ...record, genPath: newPath })
    saveAsVisible.value = false
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.action.failed', { action: t('common.page.button.generate') }))
  } finally {
    loading.value = false
  }
}

/**
 * 单行生成代码（含覆盖确认、另存为分支）
 * @param record 表配置行
 * @returns {Promise<void>}
 */
async function handleGenerateOne(record: GenTable) {
  const genMethod = record.genMethod != null ? Number(record.genMethod) : 0
  if (genMethod !== 1 && genMethod !== 2) {
    try {
      loading.value = true
      await doGenerateCode(record)
    } catch (e: unknown) {
      message.error(getErrorMessage(e) || t('common.feedback.action.failed', { action: t('common.page.button.generate') }))
    } finally {
      loading.value = false
    }
    return
  }
  try {
    loading.value = true
    const id = String(record.genTableId)
    const targetBasePath = resolvePreviewTargetBasePath(record)
    const preview = normalizePreviewResult(await previewCode(id, { templates: {}, targetBasePath }))
    const existingFiles = preview.previewFiles
      .filter((f) => f.isExisting)
      .map((f) => f.path)
    loading.value = false
    if (existingFiles.length > 0) {
      const fileList = existingFiles.slice(0, 20).join('\n') + (existingFiles.length > 20 ? '\n... ' + t('code.generator.page.existingfilessuffix', { count: existingFiles.length }) : '')
      Modal.confirm({
        title: t('common.dialog.title.overwrite'),
        content: t('code.generator.page.overwriteconfirmcontent') + '\n\n' + fileList,
        okText: t('code.generator.page.overwrite'),
        cancelText: genMethod === 2 ? t('common.page.button.cancel') : t('code.generator.page.saveascancel'),
        onOk: async () => {
          try {
            loading.value = true
            await doGenerateCode(record)
          } catch (e: unknown) {
            message.error(getErrorMessage(e) || t('common.feedback.action.failed', { action: t('common.page.button.generate') }))
          } finally {
            loading.value = false
          }
        },
        onCancel: () => {
          if (genMethod !== 2) showSaveAsPathModal(record)
        }
      })
    } else {
      try {
        loading.value = true
        await doGenerateCode(record)
      } finally {
        loading.value = false
      }
    }
  } catch (e: unknown) {
    loading.value = false
    message.error(getErrorMessage(e) || t('common.feedback.action.failed', { action: t('common.page.button.generate') }))
  }
}

/**
 * 同步：从数据库增量同步列元数据，打开表单供用户确认保存
 * @param record 列表行
 * @returns {Promise<void>}
 */
async function handleSync(record: GenTable) {
  const id = getTableId(record)
  if (!id) {
    message.warning(t('code.generator.page.notableidsync'))
    return
  }
  try {
    loading.value = true
    const detail = await syncTableColumnsFromDatabase(String(id))
    const fallbackTableId = String(id)
    formData.value = {
      ...detail,
      genTableId: detail.genTableId != null ? String(detail.genTableId) : (fallbackTableId || undefined)
    } as Partial<GenTable>
    formTitle.value = t('common.dialog.title.sync', { entity: tableConfig() })
    formVisible.value = true
    message.info(t('code.generator.page.syncformhint'))
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.load.failed', { target: tableConfig() }))
  } finally {
    loading.value = false
  }
}

/**
 * 按表配置对应实体初始化物理表（TaktGenEngines/entities/initialize）
 * @param record 列表行
 * @returns {Promise<void>}
 */
async function handleInitialize(record: GenTable) {
  const tenantCode = String(record.tenantCode ?? '').trim()
  const entityTypeFullName = resolveEntityTypeFullName(record)
  if (!tenantCode || !entityTypeFullName) {
    message.warning(t('code.generator.page.notableidinit'))
    return
  }
  try {
    loading.value = true
    await initializeTableFromEntity({ tenantCode, entityTypeFullName })
    message.success(t('common.feedback.action.success', { action: t('common.page.button.initialize') }))
    loadData()
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.action.failed', { action: t('common.page.button.initialize') }))
  } finally {
    loading.value = false
  }
}

/**
 * 克隆表配置（复制详情并改表名，去掉主键/列 id）
 * @param record 源行
 * @returns {Promise<void>}
 */
async function handleClone(record: GenTable) {
  const id = getTableId(record)
  if (!id) {
    const { genTableId: _omitId, ...recordWithoutId } = record
    formData.value = { ...recordWithoutId, tableName: `${record.tableName ?? 'table'}_copy` }
    formTitle.value = t('common.dialog.title.clone', { entity: tableConfig() })
    formVisible.value = true
    return
  }
  try {
    loading.value = true
    const detail = await getGenTableById(String(id))
    const columns = (detail as { columns?: unknown }).columns
    const { genTableId: _omitDetailId, ...detailWithoutId } = detail as GenTable
    const cloneData: Partial<GenTable> = { ...detailWithoutId, tableName: `${detail.tableName ?? 'table'}_copy` }
    if (Array.isArray(columns)) {
      ;(cloneData as { columns?: Record<string, unknown>[] }).columns = columns.map((col) => {
        const c = { ...(col as Record<string, unknown>) }
        delete c.genTableColumnId
        delete c.columnId
        delete c.genTableId
        delete c.tableId
        return c
      })
    }
    formData.value = cloneData
    formTitle.value = t('common.dialog.title.clone', { entity: tableConfig() })
    formVisible.value = true
    message.success(t('code.generator.page.clonesuccess'))
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.load.failed', { target: tableConfig() }))
  } finally {
    loading.value = false
  }
}

/**
 * 刷新列表
 */
function handleRefresh() {
  loadData()
}

/**
 * 导出当前页数据为 CSV（前端拼装）
 */
function handleExport() {
  if (dataSource.value.length === 0) {
    message.warning(t('code.generator.page.nodataexport'))
    return
  }
  try {
    const headers = [
      t('entity.gentable.tablename'),
      t('entity.gentable.tablecomment'),
      t('entity.gentable.entityclassname'),
      t('entity.gentable.genmodulename'),
      t('entity.gentable.genbusinessname'),
      t('entity.gentable.gentemplatecategory')
    ]
    const rows = dataSource.value.map((r: GenTable) =>
      [r.tableName ?? '', r.tableComment ?? '', r.entityClassName ?? '', r.genModuleName ?? '', r.genBusinessName ?? '', r.genTemplateCategory ?? 'crud'].join(',')
    )
    const csv = '\uFEFF' + [headers.join(','), ...rows].join('\n')
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `${tableConfig()}_${new Date().toISOString().slice(0, 10)}.csv`
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    message.success(t('common.feedback.export.success'))
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.export.failed'))
  }
}

/**
 * 打开高级查询抽屉
 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/**
 * 高级查询确定
 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  handleSearch()
}

/**
 * 高级查询重置
 */
function handleAdvancedQueryReset() {
  queryKeyword.value = ''
  advancedQueryVisible.value = false
  handleSearch()
}

/**
 * 打开列设置抽屉
 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/**
 * 列设置勾选变更
 * @param keys 可见列 key
 */
function handleColumnKeysChange(keys: (string | number)[]) {
  visibleColumnKeys.value = keys.map(k => String(k))
}

/**
 * 列设置恢复默认（显示全部列）
 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/**
 * 表单弹窗确定：校验后 create 或 update
 * @returns {Promise<void>}
 */
async function handleFormSubmit() {
  try {
    await genFormRef.value?.validate()
    const values = genFormRef.value?.getValues()
    if (!values) return
    formLoading.value = true
    if (values.genTableId) {
      const tableId = String(values.genTableId)
      await updateGenTable(tableId, { ...values, genTableId: tableId } as unknown as GenTableUpdate)
      message.success(t('common.feedback.updated'))
    } else {
      await createGenTable(values as unknown as GenTableCreate)
      message.success(t('common.feedback.created'))
    }
    genFormRef.value?.reset()
    formVisible.value = false
    formData.value = null
    loadData()
  } catch (e: unknown) {
    if (typeof e === 'object' && e !== null && 'errorFields' in e) return
    message.error(getErrorMessage(e) || t('common.feedback.failed'))
  } finally {
    formLoading.value = false
  }
}

/**
 * 表单弹窗取消
 */
function handleFormCancel() {
  genFormRef.value?.reset()
  formVisible.value = false
  formData.value = null
}

/**
 * 预览生成代码（不落盘）
 * @param record 列表行
 * @returns {Promise<void>}
 */
async function handlePreviewOne(record: GenTable) {
  const id = getTableId(record)
  if (!id) {
    message.warning(t('code.generator.page.notableidpreview'))
    return
  }

  selectedRow.value = record
  previewFiles.value = []
  previewValidationIssues.value = []
  previewVisible.value = true
  previewLoading.value = true
  try {
    const targetBasePath = resolvePreviewTargetBasePath(record)
    const preview = normalizePreviewResult(await previewCode(String(id), { templates: {}, targetBasePath }))
    previewValidationIssues.value = preview.validationIssues
    const previewItems = preview.previewFiles
    if (previewItems.length > 0) {
      previewFiles.value = previewItems.map((item: CodeGenPreviewFile) => ({
        name: item.path,
        content: item.content,
        isExisting: item.isExisting
      }))
    } else {
      previewFiles.value = []
      if (previewValidationIssues.value.length === 0) {
        message.warning(t('code.generator.page.preview.empty'))
      }
    }
    if (previewValidationIssues.value.length > 0) {
      message.warning(t('code.generator.page.preview.validationissuetoast', { count: previewValidationIssues.value.length }))
    }
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('code.generator.page.preview.loadfail'))
    previewFiles.value = []
  } finally {
    previewLoading.value = false
  }
}

/**
 * 导入弹窗切换租户：加载可选物理表
 * @param tenantCode 租户编码（3 位）
 * @returns {Promise<void>}
 */
async function handleImportConfigChange(tenantCode: string) {
  databaseTables.value = []
  if (!tenantCode) return
  try {
    databaseTablesLoading.value = true
    // 获取指定租户库下可选物理表（TaktDatabaseInfos/tables）
    const list = await getDatabaseTableInfoList(tenantCode)
    databaseTables.value = list ?? []
  } catch (e: unknown) {
    message.error(getErrorMessage(e) || t('common.feedback.load.data.failed'))
    databaseTables.value = []
  } finally {
    databaseTablesLoading.value = false
  }
}

/**
 * 从数据库导入表结构到代码生成配置
 * @param payload 租户编码与表名
 * @returns {Promise<void>}
 */
async function handleImportSubmit(payload: { tenantCode: string; tableName: string }) {
  try {
    importLoading.value = true
    
    if (!payload.tenantCode || !payload.tableName) {
      message.warning(t('code.generator.page.importwithdbvalidation'))
      return
    }
    
    const imported = await importTableFromDatabase({
      tenantCode: payload.tenantCode,
      tableName: payload.tableName,
    })
    message.success(t('common.feedback.created'))
    
    importVisible.value = false
    databaseTables.value = []
    await loadData()
    
    const id = String(imported?.genTableId ?? imported?.tableName ?? '')
    if (id) {
      selectedRowKeys.value = [id]
      const found = dataSource.value.find((r: GenTable) => String(r.genTableId ?? '') === id || String(r.tableName) === (payload.tableName || ''))
      selectedRows.value = found ? [found] : (imported ? [imported as GenTable] : [])
      selectedRow.value = found ?? (imported as GenTable) ?? null
    }
  } catch (e: unknown) {
    const msg =
      (e as { response?: { data?: { message?: string } } }).response?.data?.message ??
      getErrorMessage(e) ??
      t('common.feedback.action.failed', { action: t('common.page.button.import') })
    message.error(msg)
  } finally {
    importLoading.value = false
  }
}

/**
 * 挂载：加载列表与租户业务库下拉数据
 */
onMounted(() => {
  loadData()
  getDatabaseInfoList().then(list => {
    databaseInfoList.value = list ?? []
  }).catch(() => {
    databaseInfoList.value = []
  })
})
</script>
