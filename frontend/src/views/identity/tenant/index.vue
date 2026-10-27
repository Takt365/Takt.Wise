<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/tenant -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：租户实体 代表系统中的独立租户管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      update-permission="identity:tenant:update"
      delete-permission="identity:tenant:delete"
      export-permission="identity:tenant:export"
      :show-create="false"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @delete="handleDelete"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      :columns="columns"
      entity-scope="tenant"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'tenantId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTenantId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isBuiltIn'">
          <TaktDictTag
            :value="getTenantField(record, 'isBuiltIn')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'tenantStatus'">
          <TaktDictTag
            :value="getTenantField(record, 'tenantStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
        <template v-else-if="column.key === 'userTenants'">
          {{ formatTenantUserCount(record) }}
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <TenantForm
        :key="formData?.tenantId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <!-- 分配可访问用户（RBAC：api/TaktRbacs/tenants/{tenantCode}/users） -->
    <AssignTenantUsers
      v-model:open="assignTenantUsersVisible"
      :tenant="currentAssignTenant"
      @success="handleAssignTenantUsersSuccess"
    />

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-identity-tenant'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('tenantName')">
      <a-form-item :label="t('entity.tenant.name')">
        <a-input
          v-model:value="advancedQueryForm.tenantName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subscriptionStartTimeStart')">
      <a-form-item :label="t('entity.tenant.subscriptionstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.subscriptionStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subscriptionStartTimeEnd')">
      <a-form-item :label="t('entity.tenant.subscriptionstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.subscriptionStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subscriptionEndTimeStart')">
      <a-form-item :label="t('entity.tenant.subscriptionendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.subscriptionEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subscriptionEndTimeEnd')">
      <a-form-item :label="t('entity.tenant.subscriptionendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.subscriptionEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactName')">
      <a-form-item :label="t('entity.tenant.contactname')">
        <a-input
          v-model:value="advancedQueryForm.contactName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPhone')">
      <a-form-item :label="t('entity.tenant.contactphone')">
        <a-input
          v-model:value="advancedQueryForm.contactPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactphone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactEmail')">
      <a-form-item :label="t('entity.tenant.contactemail')">
        <a-input
          v-model:value="advancedQueryForm.contactEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactemail') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="t('entity.tenant.isbuiltin')">
        <a-input-number
          v-model:value="advancedQueryForm.isBuiltIn"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.isbuiltin') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tenantStatus')">
      <a-form-item :label="t('entity.tenant.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.tenantStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.status') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.extfield')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfield') })"
          allow-clear
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

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'tenantId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 租户实体 代表系统中的独立租户管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/identity/tenant
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import TenantForm from './components/TenantForm.vue'
import AssignTenantUsers from './components/AssignTenantUsers.vue'
import { getTenantList,createTenant, updateTenant, deleteTenantById, deleteTenantBatch, exportTenant } from '@/api/identity/tenant.api'
import { getTenantUserIds } from '@/api/identity/rbac.api'
import type { Tenant, TenantQuery} from '@/types/identity/tenant.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiUserSettingsLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTenant')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.tenant._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Tenant[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<Tenant | null>(null)
/** 表格多选行 */
const selecmedRows = ref<Tenant[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Tenant>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  tenantName: '',
  subscriptionStartTimeStart: '',
  subscriptionStartTimeEnd: '',
  subscriptionEndTimeStart: '',
  subscriptionEndTimeEnd: '',
  contactName: '',
  contactPhone: '',
  contactEmail: '',
  isBuiltIn: undefined as number | undefined,
  tenantStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'tenantName', label: t('entity.tenant.name') },
  { key: 'subscriptionStartTimeStart', label: t('entity.tenant.subscriptionstarttimestart') },
  { key: 'subscriptionStartTimeEnd', label: t('entity.tenant.subscriptionstarttimeend') },
  { key: 'subscriptionEndTimeStart', label: t('entity.tenant.subscriptionendtimestart') },
  { key: 'subscriptionEndTimeEnd', label: t('entity.tenant.subscriptionendtimeend') },
  { key: 'contactName', label: t('entity.tenant.contactname') },
  { key: 'contactPhone', label: t('entity.tenant.contactphone') },
  { key: 'contactEmail', label: t('entity.tenant.contactemail') },
  { key: 'isBuiltIn', label: t('entity.tenant.isbuiltin') },
  { key: 'tenantStatus', label: t('entity.tenant.status') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 分配租户用户弹窗 */
const assignTenantUsersVisible = ref(false)
const currentAssignTenant = ref<Tenant | null>(null)
/** 列表行：租户已绑定用户数（key=tenantId，数据来自 getTenantUserIds） */
const tenantUserCountMap = ref<Record<string, number>>({})
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'tenantId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {TenantQuery} 查询 DTO
 */
type TenantQueryTrimmedKey =
  | 'tenantName'
  | 'subscriptionStartTimeStart'
  | 'subscriptionStartTimeEnd'
  | 'subscriptionEndTimeStart'
  | 'subscriptionEndTimeEnd'
  | 'contactName'
  | 'contactPhone'
  | 'contactEmail'
  | 'createdAtStart'
  | 'createdAtEnd'
  | 'ExtField'
  | 'remark'

function buildListQuery(overrides?: Partial<TenantQuery>): TenantQuery {
  const form = advancedQueryForm.value
  const query: TenantQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: TenantQueryTrimmedKey, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v
    }
  }
  assignTrimmed('tenantName', form.tenantName)
  assignTrimmed('subscriptionStartTimeStart', form.subscriptionStartTimeStart)
  assignTrimmed('subscriptionStartTimeEnd', form.subscriptionStartTimeEnd)
  assignTrimmed('subscriptionEndTimeStart', form.subscriptionEndTimeStart)
  assignTrimmed('subscriptionEndTimeEnd', form.subscriptionEndTimeEnd)
  assignTrimmed('contactName', form.contactName)
  assignTrimmed('contactPhone', form.contactPhone)
  assignTrimmed('contactEmail', form.contactEmail)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
  assignTrimmed('remark', form.remark)
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    query.isBuiltIn = form.isBuiltIn
  }
  if (form.tenantStatus !== undefined && form.tenantStatus !== null) {
    query.tenantStatus = form.tenantStatus
  }
  return query
}

/** 页面挂载：加载分页配置后拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'tenantId',
    key: 'tenantId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTenantField(record, 'tenantId') ?? ''
  },
  {
    title: t('entity.tenant.name'),
    dataIndex: 'tenantName',
    key: 'tenantName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTenantField(record, 'tenantName') ?? ''
  },
  {
    title: t('entity.tenant.subscriptionstarttime'),
    dataIndex: 'subscriptionStartTime',
    key: 'subscriptionStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTenantField(record, 'subscriptionStartTime') ?? ''
  },
  {
    title: t('entity.tenant.subscriptionendtime'),
    dataIndex: 'subscriptionEndTime',
    key: 'subscriptionEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTenantField(record, 'subscriptionEndTime') ?? ''
  },
  {
    title: t('entity.tenant.contactname'),
    dataIndex: 'contactName',
    key: 'contactName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTenantField(record, 'contactName') ?? ''
  },
  {
    title: t('entity.tenant.contactphone'),
    dataIndex: 'contactPhone',
    key: 'contactPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTenantField(record, 'contactPhone') ?? ''
  },
  {
    title: t('entity.tenant.contactemail'),
    dataIndex: 'contactEmail',
    key: 'contactEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTenantField(record, 'contactEmail') ?? ''
  },
  {
    title: t('entity.tenant.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.tenant.status'),
    dataIndex: 'tenantStatus',
    key: 'tenantStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.tenant.usertenants'),
    dataIndex: 'userTenants',
    key: 'userTenants',
    width: 140,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'identity:tenant:update',
        onClick: (record: Tenant) => handleEdit(record)
      },
      {
        key: 'allocate-tenant-user',
        label: t('common.page.button.allocate') + t('entity.user._self'),
        shape: 'plain',
        icon: RiUserSettingsLine,
        permission: 'identity:tenant:update',
        onClick: (record: Tenant) => handleAssignTenantUsers(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'identity:tenant:delete',
        onClick: (record: Tenant) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTenantId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTenantField = (record: any, field: string): any => record?.[field]

/**
 * 列表展示：可访问该租户的用户数量
 * @param record 租户行
 * @returns {string} 展示文案
 */
function formatTenantUserCount(record: Tenant): string {
  const id = getTenantId(record)
  const count = tenantUserCountMap.value[id]
  if (count === undefined) return '—'
  return String(count)
}

/**
 * 按当前页租户编码加载 RBAC 用户关联数量
 * @param rows 当前页租户列表
 */
async function loadTenantUserCounts(rows: Tenant[]) {
  if (!rows.length) {
    tenantUserCountMap.value = {}
    return
  }
  const results = await Promise.all(
    rows.map(async (row) => {
      const id = getTenantId(row)
      const code = (row.tenantCode ?? '').trim()
      if (!code) return { id, count: 0 }
      try {
        const list = await getTenantUserIds(code)
        return { id, count: Array.isArray(list) ? list.length : 0 }
      } catch {
        return { id, count: 0 }
      }
    })
  )
  const map: Record<string, number> = {}
  for (const { id, count } of results) {
    map[id] = count
  }
  tenantUserCountMap.value = map
}

/** 打开分配租户用户弹窗 */
function handleAssignTenantUsers(record: Tenant) {
  currentAssignTenant.value = record
  assignTenantUsersVisible.value = true
}

/** 分配租户用户成功后刷新用户数 */
async function handleAssignTenantUsersSuccess() {
  await loadTenantUserCounts(dataSource.value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Tenant[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Tenant, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getTenantId(selecmedRow.value) === getTenantId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: Tenant[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Tenant) => ({
  onClick: () => {
    const key = getTenantId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getTenantId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getTenantList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
    void loadTenantUserCounts(dataSource.value)
  } catch (error: any) {
    logger.error('[Tenant] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  tenantName: '',
  subscriptionStartTimeStart: '',
  subscriptionStartTimeEnd: '',
  subscriptionEndTimeStart: '',
  subscriptionEndTimeEnd: '',
  contactName: '',
  contactPhone: '',
  contactEmail: '',
  isBuiltIn: undefined as number | undefined,
  tenantStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.tenant._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Tenant) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.tenant._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.tenant._self') }))
  }
}
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateTenant(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.tenant._self') }))
    } else {
      await createTenant(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.tenant._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportTenant(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.tenant._self') }))
  } catch (error: any) {
    logger.error('[Tenant] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.tenant._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Tenant) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.tenant._self'), name: t('common.tip.this.target', { target: t('entity.tenant._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTenantById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.tenant._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.tenant._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.tenant._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTenantBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.tenant._self') }))
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  tenantName: '',
  subscriptionStartTimeStart: '',
  subscriptionStartTimeEnd: '',
  subscriptionEndTimeStart: '',
  subscriptionEndTimeEnd: '',
  contactName: '',
  contactPhone: '',
  contactEmail: '',
  isBuiltIn: undefined as number | undefined,
  tenantStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
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

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
