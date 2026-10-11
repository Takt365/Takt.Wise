<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/database-info -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：当前登录租户业务库物理表浏览（不可切换租户，服务端分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="code-database-database-info p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="tenant"
      :columns="columns"
      :include-audit-fields="false"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
      :id-column-key="'tableName'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTableRowKey"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :scroll="tableScroll"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 数据库表信息浏览页：仅当前登录租户业务库物理表（租户隔离，服务端分页）
 * @module views/code/database/database-info
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getDatabaseTableInfoPageList } from '@/api/code/database/databaseInfo.api'
import type { DatabaseTableInfo, DatabaseTableInfoQuery } from '@/types/code/database/databaseInfo.types'

/** i18n 翻译函数 */
const { t } = useI18n()

/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.gentable.tablename') })
)
/** 快捷查询关键字（表名/表注释） */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 当前页表格数据 */
const dataSource = ref<DatabaseTableInfo[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数（与标准 CRUD 列表一致，默认 20） */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<DatabaseTableInfo | null>(null)
/** 表格多选行 */
const selectedRows = ref<DatabaseTableInfo[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])
/** 表格纵向滚动（与 identity/user 及 TaktSingleTable 分页列表一致） */
const tableScroll = { y: 'calc(100vh - 300px)' }

/** 页面挂载：加载当前登录租户表信息 */
onMounted(() => {
  loadData()
})

/** 表格列定义 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('entity.gentable.tablename'),
    dataIndex: 'tableName',
    key: 'tableName',
    width: 240,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: DatabaseTableInfo }) => record.tableName ?? ''
  },
  {
    title: t('entity.gentable.tablecomment'),
    dataIndex: 'tableComment',
    key: 'tableComment',
    width: 280,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DatabaseTableInfo }) => record.tableComment ?? ''
  }
])

/**
 * 表格 row-key（物理表名）
 * @param record 行数据
 * @returns 表名
 */
function getTableRowKey(record: unknown): string {
  if (!record || typeof record !== 'object' || !('tableName' in record)) {
    return ''
  }
  const tableName = (record as { tableName?: unknown }).tableName
  return typeof tableName === 'string' ? tableName : ''
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: DatabaseTableInfo[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: DatabaseTableInfo, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getTableRowKey(selectedRow.value as DatabaseTableInfo) === getTableRowKey(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: DatabaseTableInfo[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/**
 * 行点击切换选中
 * @param record 行数据
 */
const onClickRow = (record: DatabaseTableInfo) => ({
  onClick: () => {
    const key = getTableRowKey(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTableRowKey(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载当前登录租户下物理表分页列表 */
async function loadData() {
  loading.value = true
  try {
    const params: DatabaseTableInfoQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value
    }
    const kw = queryKeyword.value?.trim()
    if (kw) {
      params.keyWords = kw
    }
    const response = await getDatabaseTableInfoPageList(params)
    const responseAny = response as { Data?: DatabaseTableInfo[]; Total?: number }
    const items = response?.data ?? responseAny?.Data ?? []
    const totalCount = response?.total ?? responseAny?.Total ?? 0
    dataSource.value = items
    total.value = totalCount
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
  } catch (error: unknown) {
    logger.error('[DatabaseInfo] 加载表信息失败', { error })
    const errMsg = error instanceof Error ? error.message : ''
    message.error(errMsg || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动按新租户重载 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = 1
  loadData()
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = 1
  pageSize.value = size
  loadData()
}
</script>
