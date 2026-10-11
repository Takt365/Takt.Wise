<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/profile/components -->
<!-- 文件名称：ProfileLoginLogPanel.vue -->
<!-- 功能描述：个人中心登录日志面板（me/list；QueryBar+ToolsBar；无 CRUD） -->
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
      :id-column-key="'loginLogId'"
      table-mode="single"
      scroll-layout="pageWithInnerTabs"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getLoginLogId"
      :pagination="false"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'browser'">
          <TaktConstTag
            category="browserType"
            :value="getLoginLogField(record, 'browser')"
          />
        </template>
        <template v-else-if="column.key === 'os'">
          <TaktConstTag
            category="operatingSystem"
            :value="getLoginLogField(record, 'os')"
          />
        </template>
        <template v-else-if="column.key === 'loginType'">
          <TaktConstTag
            category="loginType"
            :value="getLoginLogField(record, 'loginType')"
          />
        </template>
        <template v-else-if="column.key === 'loginResult'">
          <TaktConstTag
            category="loginResult"
            :value="getLoginLogField(record, 'loginResult')"
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
      :storage-key="'takt-query-fields-identity-profile-login-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('loginType')">
          <a-form-item :label="t('entity.loginlog.logintype')">
            <TaktSelect
              v-model:value="advancedQueryForm.loginType"
              :options="loginTypeOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.logintype') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('browser')">
          <a-form-item :label="t('entity.loginlog.browser')">
            <TaktSelect
              v-model:value="advancedQueryForm.browser"
              :options="browserTypeOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.browser') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('os')">
          <a-form-item :label="t('entity.loginlog.os')">
            <TaktSelect
              v-model:value="advancedQueryForm.os"
              :options="operatingSystemOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.os') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('userAgent')">
          <a-form-item :label="t('entity.loginlog.useragent')">
            <a-input
              v-model:value="advancedQueryForm.userAgent"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.useragent') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('loginResult')">
          <a-form-item :label="t('entity.loginlog.loginresult')">
            <TaktSelect
              v-model:value="advancedQueryForm.loginResult"
              :options="loginResultOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.loginresult') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('loginMessage')">
          <a-form-item :label="t('entity.loginlog.loginmessage')">
            <a-input
              v-model:value="advancedQueryForm.loginMessage"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.loginmessage') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('loginIp')">
          <a-form-item :label="t('entity.loginlog.loginip')">
            <a-input
              v-model:value="advancedQueryForm.loginIp"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.loginip') })"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('loginLocation')">
          <a-form-item :label="t('entity.loginlog.loginlocation')">
            <a-input
              v-model:value="advancedQueryForm.loginLocation"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.loginlocation') })"
              allow-clear
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
      :id-column-key="'loginLogId'"
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
 * 个人中心 · 当前用户登录日志列表（me/list）
 */
import {
  browserTypeOptions,
  loginResultOptions,
  loginTypeOptions,
  operatingSystemOptions,
} from '@/constants/takt.constants'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getMyLoginLogList } from '@/api/statistics/logging/loginLog.api'
import type { LoginLog } from '@/types/statistics/logging/loginLog.types'
import { createLogger } from '@/utils/logger'

const panelLogger = createLogger('identity-profile-login-log')
const { t } = useI18n()

/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.loginlog._self') }),
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<LoginLog[]>([])
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
  { key: 'loginType', label: t('entity.loginlog.logintype') },
  { key: 'browser', label: t('entity.loginlog.browser') },
  { key: 'os', label: t('entity.loginlog.os') },
  { key: 'userAgent', label: t('entity.loginlog.useragent') },
  { key: 'loginResult', label: t('entity.loginlog.loginresult') },
  { key: 'loginMessage', label: t('entity.loginlog.loginmessage') },
  { key: 'loginIp', label: t('entity.loginlog.loginip') },
  { key: 'loginLocation', label: t('entity.loginlog.loginlocation') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（storage: takt-column-identity-profile-login-log） */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名 */
const entityIdName = 'loginLogId'

/**
 * 空高级查询表单
 */
function createEmptyAdvancedForm() {
  return {
    loginType: undefined as string | undefined,
    browser: undefined as string | undefined,
    os: undefined as string | undefined,
    userAgent: '',
    loginResult: undefined as string | undefined,
    loginMessage: '',
    loginIp: '',
    loginLocation: '',
    createdAtStart: '',
    createdAtEnd: '',
    remark: '',
  }
}

/** 可 trim 的高级查询字符串字段 */
type LoginLogQueryTrimmedKey =
  | 'userAgent'
  | 'loginMessage'
  | 'loginIp'
  | 'loginLocation'
  | 'createdAtStart'
  | 'createdAtEnd'
  | 'remark'

/**
 * 组装 me/list 查询参数（不下发 userName）
 */
function buildListQuery() {
  const form = advancedQueryForm.value
  const query: Parameters<typeof getMyLoginLogList>[0] = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: LoginLogQueryTrimmedKey, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v
    }
  }
  assignTrimmed('userAgent', form.userAgent)
  assignTrimmed('loginMessage', form.loginMessage)
  assignTrimmed('loginIp', form.loginIp)
  assignTrimmed('loginLocation', form.loginLocation)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('remark', form.remark)
  if (form.loginType?.trim()) {
    query.loginType = form.loginType.trim()
  }
  if (form.browser?.trim()) {
    query.browser = form.browser.trim()
  }
  if (form.os?.trim()) {
    query.os = form.os.trim()
  }
  if (form.loginResult?.trim()) {
    query.loginResult = form.loginResult.trim()
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
    customRender: ({ record }: { record: LoginLog }) => getLoginLogField(record, 'plantCode') ?? '',
  },
  {
    title: t('entity.loginlog.logintype'),
    dataIndex: 'loginType',
    key: 'loginType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.loginresult'),
    dataIndex: 'loginResult',
    key: 'loginResult',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.browser'),
    dataIndex: 'browser',
    key: 'browser',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.os'),
    dataIndex: 'os',
    key: 'os',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.loginip'),
    dataIndex: 'loginIp',
    key: 'loginIp',
    width: 130,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: LoginLog }) => getLoginLogField(record, 'loginIp') ?? '',
  },
  {
    title: t('entity.loginlog.loginlocation'),
    dataIndex: 'loginLocation',
    key: 'loginLocation',
    width: 180,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: LoginLog }) => getLoginLogField(record, 'loginLocation') ?? '',
  },
  {
    title: t('entity.loginlog.loginmessage'),
    dataIndex: 'loginMessage',
    key: 'loginMessage',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: LoginLog }) => getLoginLogField(record, 'loginMessage') ?? '',
  },
  {
    title: t('common.page.entity.createdat'),
    dataIndex: 'createdAt',
    key: 'createdAt',
    width: 180,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: LoginLog }) => getLoginLogField(record, 'createdAt') ?? '',
  },
])

/**
 * 表格 row-key
 * @param record 行
 */
function getLoginLogId(record: LoginLog): string {
  const row = record as LoginLog & { loginLogId?: string; id?: string }
  return String(row[entityIdName] ?? row.id ?? '')
}

/**
 * 读取行字段
 * @param record 行
 * @param field 字段名
 */
function getLoginLogField(record: LoginLog, field: string): unknown {
  return (record as Record<string, unknown>)?.[field]
}

/**
 * 加载当前用户登录日志
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const res = await getMyLoginLogList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    panelLogger.error('load login logs failed', { action: 'loadData' }, error)
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
