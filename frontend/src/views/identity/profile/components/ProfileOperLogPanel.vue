<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/profile/components -->
<!-- 文件名称：ProfileOperLogPanel.vue -->
<!-- 功能描述：个人中心操作日志面板（me/list；QueryBar+ToolsBar；无 CRUD） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'operLogId'"
      table-mode="single"
      scroll-layout="pageWithInnerTabs"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getOperLogId"
      :pagination="false"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'operType'">
          <TaktConstTag
            category="operType"
            :value="getOperLogField(record, 'operType')"
          />
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

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-identity-profile-oper-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('operModule')">
          <a-form-item :label="t('entity.operlog.opermodule')">
            <a-input
              v-model:value="advancedQueryForm.operModule"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.opermodule') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operType')">
          <a-form-item :label="t('entity.operlog.opertype')">
            <TaktSelect
              v-model:value="advancedQueryForm.operType"
              :options="operTypeOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.operlog.opertype') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operMethod')">
          <a-form-item :label="t('entity.operlog.opermethod')">
            <a-input
              v-model:value="advancedQueryForm.operMethod"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.opermethod') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('requestMethod')">
          <a-form-item :label="t('entity.operlog.requestmethod')">
            <a-input
              v-model:value="advancedQueryForm.requestMethod"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.requestmethod') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operUrl')">
          <a-form-item :label="t('entity.operlog.operurl')">
            <a-input
              v-model:value="advancedQueryForm.operUrl"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operurl') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operStatus')">
          <a-form-item :label="t('entity.operlog.operstatus')">
            <a-input-number
              v-model:value="advancedQueryForm.operStatus"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operstatus') })"
              style="width: 100%"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('errorMsg')">
          <a-form-item :label="t('entity.operlog.errormsg')">
            <a-input
              v-model:value="advancedQueryForm.errorMsg"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.errormsg') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operIp')">
          <a-form-item :label="t('entity.operlog.operip')">
            <a-input
              v-model:value="advancedQueryForm.operIp"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operip') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operLocation')">
          <a-form-item :label="t('entity.operlog.operlocation')">
            <a-input
              v-model:value="advancedQueryForm.operLocation"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operlocation') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operTimeStart')">
          <a-form-item :label="t('entity.operlog.opertimestart')">
            <a-date-picker
              v-model:value="advancedQueryForm.operTimeStart"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.operlog.opertimestart') })"
              value-format="YYYY-MM-DD HH:mm:ss"
              show-time
              style="width: 100%"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('operTimeEnd')">
          <a-form-item :label="t('entity.operlog.opertimeend')">
            <a-date-picker
              v-model:value="advancedQueryForm.operTimeEnd"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.operlog.opertimeend') })"
              value-format="YYYY-MM-DD HH:mm:ss"
              show-time
              style="width: 100%"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('createdAtStart')">
          <a-form-item :label="t('common.page.entity.createdatstart')">
            <a-date-picker
              v-model:value="advancedQueryForm.createdAtStart"
              :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
              value-format="YYYY-MM-DD HH:mm:ss"
              show-time
              style="width: 100%"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('createdAtEnd')">
          <a-form-item :label="t('common.page.entity.createdatend')">
            <a-date-picker
              v-model:value="advancedQueryForm.createdAtEnd"
              :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
              value-format="YYYY-MM-DD HH:mm:ss"
              show-time
              style="width: 100%"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('remark')">
          <a-form-item :label="t('common.page.entity.remark')">
            <a-textarea
              v-model:value="advancedQueryForm.remark"
              :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
              :rows="2"
              allow-clear
            />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>

    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'operLogId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 个人中心 · 当前用户操作日志列表（me/list）
 */
import { operTypeOptions } from '@/constants/takt.constants'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getMyOperLogList } from '@/api/statistics/logging/operLog.api'
import type { OperLog } from '@/types/statistics/logging/operLog.types'
import { createLogger } from '@/utils/logger'

const panelLogger = createLogger('identity-profile-oper-log')
const { t } = useI18n()

/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.operlog._self') }),
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<OperLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 是否已按条件查询（未查询前不请求接口，表格保持空） */
const hasQueried = ref(false)

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单（不含 userName，始终当前用户） */
const advancedQueryForm = ref(createEmptyAdvancedForm())
/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'operModule', label: t('entity.operlog.opermodule') },
  { key: 'operType', label: t('entity.operlog.opertype') },
  { key: 'operMethod', label: t('entity.operlog.opermethod') },
  { key: 'requestMethod', label: t('entity.operlog.requestmethod') },
  { key: 'operUrl', label: t('entity.operlog.operurl') },
  { key: 'operStatus', label: t('entity.operlog.operstatus') },
  { key: 'errorMsg', label: t('entity.operlog.errormsg') },
  { key: 'operIp', label: t('entity.operlog.operip') },
  { key: 'operLocation', label: t('entity.operlog.operlocation') },
  { key: 'operTimeStart', label: t('entity.operlog.opertimestart') },
  { key: 'operTimeEnd', label: t('entity.operlog.opertimeend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（storage: takt-column-identity-profile-oper-log） */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名 */
const entityIdName = 'operLogId'

/**
 * 空高级查询表单
 */
function createEmptyAdvancedForm() {
  return {
    operModule: '',
    operType: undefined as string | undefined,
    operMethod: '',
    requestMethod: '',
    operUrl: '',
    operStatus: undefined as number | undefined,
    errorMsg: '',
    operIp: '',
    operLocation: '',
    operTimeStart: '',
    operTimeEnd: '',
    createdAtStart: '',
    createdAtEnd: '',
    remark: '',
  }
}

/** 可 trim 的高级查询字符串字段 */
type OperLogQueryTrimmedKey =
  | 'operModule'
  | 'operMethod'
  | 'requestMethod'
  | 'operUrl'
  | 'errorMsg'
  | 'operIp'
  | 'operLocation'
  | 'operTimeStart'
  | 'operTimeEnd'
  | 'createdAtStart'
  | 'createdAtEnd'
  | 'remark'

/**
 * 组装 me/list 查询参数（不下发 userName）
 */
function buildListQuery() {
  const form = advancedQueryForm.value
  const query: Parameters<typeof getMyOperLogList>[0] = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: OperLogQueryTrimmedKey, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v
    }
  }
  assignTrimmed('operModule', form.operModule)
  assignTrimmed('operMethod', form.operMethod)
  assignTrimmed('requestMethod', form.requestMethod)
  assignTrimmed('operUrl', form.operUrl)
  assignTrimmed('errorMsg', form.errorMsg)
  assignTrimmed('operIp', form.operIp)
  assignTrimmed('operLocation', form.operLocation)
  assignTrimmed('operTimeStart', form.operTimeStart)
  assignTrimmed('operTimeEnd', form.operTimeEnd)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('remark', form.remark)
  if (form.operType?.trim()) {
    query.operType = form.operType.trim()
  }
  if (form.operStatus !== undefined && form.operStatus !== null) {
    query.operStatus = form.operStatus
  }
  return query
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
})

/**
 * 是否存在有效查询条件（关键词或高级查询任一非空）
 * @returns 有条件为 true
 */
function hasActiveQueryConditions(): boolean {
  if (queryKeyword.value.trim()) {
    return true
  }
  const form = advancedQueryForm.value as Record<string, unknown>
  for (const value of Object.values(form)) {
    if (value === undefined || value === null) {
      continue
    }
    if (typeof value === 'string' && value.trim()) {
      return true
    }
    if (typeof value === 'number') {
      return true
    }
  }
  return false
}

/** 清空表格数据（不请求） */
function clearTableData(): void {
  dataSource.value = []
  total.value = 0
}

/** 表格列（个人范围：无 userName / 操作列） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'plantCode') ?? '',
  },
  {
    title: t('entity.operlog.opermodule'),
    dataIndex: 'operModule',
    key: 'operModule',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'operModule') ?? '',
  },
  {
    title: t('entity.operlog.opertype'),
    dataIndex: 'operType',
    key: 'operType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.operlog.requestmethod'),
    dataIndex: 'requestMethod',
    key: 'requestMethod',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'requestMethod') ?? '',
  },
  {
    title: t('entity.operlog.operurl'),
    dataIndex: 'operUrl',
    key: 'operUrl',
    width: 200,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'operUrl') ?? '',
  },
  {
    title: t('entity.operlog.operstatus'),
    dataIndex: 'operStatus',
    key: 'operStatus',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'operStatus') ?? '',
  },
  {
    title: t('entity.operlog.operip'),
    dataIndex: 'operIp',
    key: 'operIp',
    width: 130,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'operIp') ?? '',
  },
  {
    title: t('entity.operlog.operlocation'),
    dataIndex: 'operLocation',
    key: 'operLocation',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'operLocation') ?? '',
  },
  {
    title: t('entity.operlog.opertime'),
    dataIndex: 'operTime',
    key: 'operTime',
    width: 180,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'operTime') ?? '',
  },
  {
    title: t('entity.operlog.elapsedtime'),
    dataIndex: 'elapsedTime',
    key: 'elapsedTime',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OperLog }) => getOperLogField(record, 'elapsedTime') ?? '',
  },
])

/**
 * 表格 row-key
 * @param record 行
 */
function getOperLogId(record: OperLog): string {
  const row = record as OperLog & { operLogId?: string; id?: string }
  return String(row[entityIdName] ?? row.id ?? '')
}

/**
 * 读取行字段
 * @param record 行
 * @param field 字段名
 */
function getOperLogField(record: OperLog, field: string): unknown {
  return (record as Record<string, unknown>)?.[field]
}

/**
 * 加载当前用户操作日志
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const res = await getMyOperLogList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    panelLogger.error('load oper logs failed', { action: 'loadData' }, error)
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 快捷查询 */
function handleSearch(): void {
  if (!hasActiveQueryConditions()) {
    message.warning(t('identity.profile.page.log.queryRequired'))
    hasQueried.value = false
    clearTableData()
    return
  }
  hasQueried.value = true
  currentPage.value = 1
  void loadData()
}

/** 重置查询 */
function handleReset(): void {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedForm()
  currentPage.value = 1
  hasQueried.value = false
  clearTableData()
}

/** 打开高级查询 */
function handleAdvancedQuery(): void {
  advancedQueryVisible.value = true
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit(): void {
  advancedQueryVisible.value = false
  if (!hasActiveQueryConditions()) {
    message.warning(t('identity.profile.page.log.queryRequired'))
    hasQueried.value = false
    clearTableData()
    return
  }
  hasQueried.value = true
  currentPage.value = 1
  void loadData()
}

/** 高级查询重置表单 */
function handleAdvancedQueryReset(): void {
  advancedQueryForm.value = createEmptyAdvancedForm()
}

/** 打开列设置 */
function handleColumnSetting(): void {
  columnSettingVisible.value = true
}

/**
 * 列设置：更新可见列
 * @param keys 可见列 key
 */
function handleColumnKeysChange(keys: string[]): void {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认 */
function handleColumnSettingReset(): void {
  visibleColumnKeys.value = []
}

/** 刷新（仅已查询过时请求） */
function handleRefresh(): void {
  if (!hasQueried.value) {
    return
  }
  void loadData()
}

/** 表格 change 占位 */
function handleTableChange(): void {}

/** 列宽拖拽占位 */
function handleResizeColumn(): void {}

/**
 * 分页页码变更
 * @param page 页码
 */
function handlePaginationChange(page: number): void {
  if (!hasQueried.value) {
    return
  }
  currentPage.value = page
  void loadData()
}

/**
 * 分页每页条数变更
 * @param _current 当前页
 * @param size 每页条数
 */
function handlePaginationSizeChange(_current: number, size: number): void {
  if (!hasQueried.value) {
    return
  }
  pageSize.value = size
  currentPage.value = 1
  void loadData()
}
</script>
