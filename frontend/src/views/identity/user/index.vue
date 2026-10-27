<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/user -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：用户管理列表页；查询、CRUD、导入导出、分配角色/部门/岗位/租户、改密/重置/解锁 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="
        t('common.page.form.placeholder.required', {
          field: [t('entity.user.name'), t('entity.user.nickname')].join('、')
        }) + t('common.page.button.query')
      "
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="identity:user:create"
      update-permission="identity:user:update"
      delete-permission="identity:user:delete"
      import-permission="identity:user:import"
      export-permission="identity:user:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="tenant"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="userRowKey"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 自定义列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'userStatus'">
          <a-switch
            :checked="record.userStatus === 1"
            :disabled="isAdminUser(toUserRecord(record))"
            :checked-children="t('common.page.button.enable')"
            :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleUserStatusChange(toUserRecord(record), Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'userType'">
          <TaktDictTag
            :value="record.userType"
            dict-type="sys_user_type"
          />
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

    <!-- 新增/编辑对话框：宽度 =（视口 − 左侧菜单）× 80%，可拖拽调整宽高 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <UserForm
        :key="formData?.userId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <!-- 修改密码对话框 -->
    <TaktModal
      v-model:open="changePasswordVisible"
      :title="t('common.dialog.title.changepwd')"
      :width="'33.333vw'"
      :confirm-loading="changePasswordLoading"
      @ok="handleChangePasswordSubmit"
      @cancel="handleChangePasswordCancel"
    >
      <UserChangePassword
        v-if="currentChangePasswordUser"
        ref="changePasswordFormRef"
        :user-name="getUserName(currentChangePasswordUser) || ''"
        :loading="changePasswordLoading"
      />
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.user.name')">
        <a-input v-model:value="advancedQueryForm.userName" />
      </a-form-item>
      <a-form-item :label="t('entity.user.nickname')">
        <a-input v-model:value="advancedQueryForm.nickName" />
      </a-form-item>
      <a-form-item :label="t('entity.user.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.userStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.user.status') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.user._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.user._self"
        file-type="xlsx"
        :sheet-name="userExcelNames.sheet"
        :template-file-name="userExcelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>

    <!-- 分配角色 -->
    <AssignUserRoles
      v-model:open="assignUserRoleVisible"
      :user="currentAssignUser"
      @success="handleAssignSuccess"
    />

    <!-- 分配可访问租户 -->
    <AssignUserTenants
      v-model:open="assignUserTenantVisible"
      :user="currentAssignUser"
      @success="handleAssignSuccess"
    />

    <!-- 分配可访问公司 -->
    <AssignUserCompanies
      v-model:open="assignUserCompanyVisible"
      :user="currentAssignUser"
      @success="handleAssignSuccess"
    />

    <TaktColumnDrawer
      entity-scope="tenant"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'id'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 用户列表页脚本模块。
 * - API：@/api/identity/user.api；表单子组件为 ./components/UserForm.vue。
 * - 列表字段与后端 User / UserQuery 对齐（userName、nickName 等）。
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import UserForm from './components/UserForm.vue'
import AssignUserRoles from './components/AssignUserRoles.vue'
import AssignUserTenants from './components/AssignUserTenants.vue'
import AssignUserCompanies from './components/AssignUserCompanies.vue'
import UserChangePassword from './components/UserChangePassword.vue'
import {
  getUserList,
  createUser,
  updateUser,
  deleteUserById,
  deleteUserBatch,
  updateUserStatus,
  unlock,
  exportUserData,
  getUserTemplate,
  importUserData,
  resetPassword,
  changePassword
} from '@/api/identity/user.api'
import type { TaktBinaryDownload } from '@/types/common.types'
import type {
  CreateUser,
  UpdateUser,
  User,
  UserQuery
} from '@/types/identity/user.types'
import type { UserFormValues } from '@/types/identity/userFormView.types'
import { resolveRequestLocale } from '@/stores/foundation/useLocaleStore'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktExcelEntityNames } from '@/utils/naming'
import {
  RiEditLine,
  RiDeleteBinLine,
  RiUserSettingsLine,
  RiBuilding2Line,
  RiGlobalLine,
  RiLockUnlockLine,
  RiLockPasswordLine,
  RiRestartLine
} from '@remixicon/vue'

const { t } = useI18n()

/** 用户管理页日志器 */
const userLogger = createLogger('identity.user')

/** 分配/改密等子组件仍使用 userName 展示，由列表行补齐别名 */
type UserAssignRecord = User & { userName?: string; nickName?: string }

// 导入/导出 Excel 名称（与服务端实体名一致）
const userExcelNames = taktExcelEntityNames('TaktUser')

// 顶栏查询关键字
const queryKeyword = ref('')
// 表格加载中
const loading = ref(false)
// 表格数据
const dataSource = ref<User[]>([])
// 分页：当前页、每页条数、总条数
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
// 行选择：单选行、多选行、勾选 key
const selecmedRow = ref<User | null>(null)
const selecmedRows = ref<User[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
// 新增/编辑表单弹窗
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<UserAssignRecord>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
// 修改密码弹窗
const changePasswordFormRef = ref()
const changePasswordVisible = ref(false)
const changePasswordLoading = ref(false)
const currentChangePasswordUser = ref<User | null>(null)
// 高级查询抽屉（字段与 UserQuery 一致）
const advancedQueryVisible = ref(false)
type UserAdvancedQueryForm = { userName: string; nickName: string; userStatus?: number }
const emptyUserAdvancedQueryForm = (): UserAdvancedQueryForm => ({ userName: '', nickName: '' })
const advancedQueryForm = ref<UserAdvancedQueryForm>(emptyUserAdvancedQueryForm())
// 导入弹窗
const importVisible = ref(false)
// RBAC 分配弹窗（用户-角色/租户/公司）
const assignUserRoleVisible = ref(false)
const assignUserTenantVisible = ref(false)
const assignUserCompanyVisible = ref(false)
const currentAssignUser = ref<UserAssignRecord | null>(null)
// 列设置抽屉
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

/**
 * 构建列表查询参数
 * @param overrides 覆盖分页等字段
 * @returns {UserQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<UserQuery>): UserQuery {
  const query: UserQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  if (advancedQueryForm.value.userName) {
    query.userName = advancedQueryForm.value.userName
  }
  if (advancedQueryForm.value.nickName) {
    query.nickName = advancedQueryForm.value.nickName
  }
  if (advancedQueryForm.value.userStatus !== undefined) {
    query.userStatus = advancedQueryForm.value.userStatus
  }
  return query
}

/** 页面挂载：加载分页配置后拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

// 表格列配置（computed 以便列标题与操作列 label 随 locale 更新）
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'userId',
    key: 'id',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => {
      return getUserField(record, 'userId') || ''
    }
  },
  {
    title: t('entity.user.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.user.name'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    sorter: (a: User, b: User) => getUserName(a).localeCompare(getUserName(b))
  },
  {
    title: t('entity.user.nickname'),
    dataIndex: 'nickName',
    key: 'nickName',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.user.type'),
    dataIndex: 'userType',
    key: 'userType',
    width: 100
  },
  {
    title: t('entity.user.status'),
    dataIndex: 'userStatus',
    key: 'userStatus',
    width: 100
  },
  CreateActionColumn<User>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'identity:user:update',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleEdit(record)
      },
      {
        key: 'changepwd',
        label: t('common.page.button.changepwd'),
        shape: 'plain',
        icon: RiLockPasswordLine,
        permission: 'identity:user:changepwd',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleUpdatePassword(record)
      },
      {
        key: 'resetpwd',
        label: t('common.page.button.reset') + ' ' + t('common.page.button.password'),
        shape: 'plain',
        icon: RiRestartLine,
        permission: 'identity:user:resetpwd',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleResetPassword(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'identity:user:delete',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleDeleteOne(record)
      },
      {
        key: 'allocate-user-role',
        label: t('common.page.button.allocate') + t('entity.role._self'),
        shape: 'plain',
        icon: RiUserSettingsLine,
        permission: 'identity:user:allocate',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleAssignUserRole(record)
      },
      {
        key: 'allocate-user-tenant',
        label: t('common.page.button.allocate') + t('entity.tenant._self'),
        shape: 'plain',
        icon: RiGlobalLine,
        permission: 'identity:user:update',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleAssignUserTenant(record)
      },
      {
        key: 'allocate-user-company',
        label: t('common.page.button.allocate') + t('entity.company._self'),
        shape: 'plain',
        icon: RiBuilding2Line,
        permission: 'identity:user:update',
        visible: (record) => !isAdminUser(record),
        onClick: (record) => handleAssignUserCompany(record)
      },
      {
        key: 'unlock',
        label: t('common.page.button.unlock'),
        shape: 'plain',
        icon: RiLockUnlockLine,
        permission: 'identity:user:unlock',
        visible: (record) => {
          if (isAdminUser(record)) return false
          return record.userStatus === 2
        },
        onClick: (record) => handleUnlock(record)
      }
    ]
  })
])

// 辅助函数：获取用户ID
const getUserId = (user: User | null | undefined): string => user?.userId ?? ''

/** 与 TaktSingleTable 行记录类型一致（dataSource 实际为 User[]） */
type UserTableRow = Record<string, unknown>

/**
 * 将表格行记录收窄为 User
 * @param record 表格行
 */
const asUserRow = (record: UserTableRow): User => record as unknown as User

/** 表格 row-key */
const userRowKey = (record: UserTableRow) => getUserId(asUserRow(record))

/** 读取用户名（API 字段 userName） */
const getUserName = (user: User | null | undefined): string => user?.userName ?? ''

/** 读取昵称（API 字段 nickName） */
const getNickName = (user: User | null | undefined): string => user?.nickName ?? ''

// 辅助函数：获取字段值
const getUserField = (user: User | null | undefined, field: keyof User): unknown =>
  user?.[field]

/**
 * 映射为表单/分配子组件可识别的记录（补齐 userName、nickName 别名）
 * @param record 列表行
 */
const toUserAssignRecord = (record: User): UserAssignRecord => ({
  ...record,
  userName: getUserName(record),
  nickName: getNickName(record)
})

// 辅助函数：判断是否为受保护的管理员用户（admin、guest）
const isAdminUser = (user: User | null): boolean => {
  if (!user) return false
  const lowerUserName = getUserName(user).toLowerCase()
  return lowerUserName === 'admin' || lowerUserName === 'guest'
}

/**
 * bodyCell 插槽行转为 User（TaktSingleTable 透传 record 为 Record）
 * @param record 表格行
 * @returns {User} 用户行
 */
function toUserRecord(record: Record<string, unknown>): User {
  return record as unknown as User
}

// 更新/删除按钮禁用状态（表格无选中时自动灰色；基于 selecmedRows 保证与表格勾选一致）
// 更新：仅当恰好选中 1 行且非管理员时可点
// 删除：仅当至少选中 1 行且选中里无管理员时可点
const updateDisabled = computed(
  () =>
    selecmedRows.value.length !== 1 ||
    (selecmedRows.value.length === 1 && isAdminUser(selecmedRows.value[0] ?? null))
)
const deleteDisabled = computed(
  () => selecmedRows.value.length === 0 || selecmedRows.value.some((u) => isAdminUser(u))
)

// 行选择配置（官方标准方式）
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: User[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: User, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getUserId(selecmedRow.value) === getUserId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: User[]) => {
    if (selecmed) {
      selecmedRow.value = selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    } else {
      selecmedRow.value = null
    }
  }
}))

// 行点击处理（点击行选中/取消选中 - 官方标准方式）
const onClickRow = (record: UserTableRow) => {
  const user = asUserRow(record)
  return {
    onClick: () => {
      // 根据业务需求判断是否可选
      // if (user.disabled) return;

      const key = getUserId(user)
      const index = selecmedRowKeys.value.indexOf(key)
      
      if (index > -1) {
        // 已选中，则取消
        selecmedRowKeys.value.splice(index, 1)
      } else {
        // 未选中，则添加
        selecmedRowKeys.value.push(key)
      }
      
      // 注意：此处需手动同步 selecmedRows
      selecmedRows.value = dataSource.value.filter(item => 
        selecmedRowKeys.value.includes(getUserId(item))
      )
      
      // 更新 selecmedRow
      selecmedRow.value =
        selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
      
      // 触发 rowSelection.onChange 以同步 checkbox 状态
      if (rowSelection.value.onChange) {
        rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
      }
    }
  }
}

// 加载数据
const loadData = async () => {
  try {
    loading.value = true
    const response = await getUserList(buildListQuery())
    const responseAny = response as { Data?: User[]; Total?: number }
    const items = response?.data ?? responseAny?.Data ?? []
    const totalCount = response?.total ?? responseAny?.Total ?? 0

    dataSource.value = items
    total.value = totalCount
  } catch (error: any) {
    userLogger.error('加载数据失败', { action: 'loadData' }, error)
    message.error(error.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

// 查询
const handleSearch = () => {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/**
 * 表格行内切换用户状态（sys_normal_disable：1=启用，0=禁用）
 * @param record 当前行
 * @param checked 开关是否选中（启用）
 */
const handleUserStatusChange = async (record: User, checked: boolean) => {
  const id = getUserId(record)
  if (!id || isAdminUser(record)) return
  const newStatus = checked ? 1 : 0
  const oldStatus = record.userStatus
  record.userStatus = newStatus
  try {
    await updateUserStatus(id, { userId: id, userStatus: newStatus })
    message.success(t('common.feedback.updated'))
  } catch (error: unknown) {
    record.userStatus = oldStatus
    const err = error as { message?: string }
    userLogger.error('状态更新失败', { action: 'updateUserStatus', userId: id }, error)
    message.error(err?.message || t('common.feedback.failed'))
  }
}

// 重置
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = emptyUserAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

// 表格变化（仅处理排序，分页由 TaktPagination 处理）
const handleTableChange = (_pagination: any, _filters: any, sorter: any) => {
  if (sorter && sorter.order) {
    userLogger.debug('表格排序', { action: 'sort', field: sorter.field, order: sorter.order })
  }
}

// 分页变化
const handlePaginationChange = (page: number, size: number) => {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
const handlePaginationSizeChange = (_current: number, size: number) => {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}

// 列宽调整处理
const handleResizeColumn = (w: number, col: any) => {
  // 更新对应列的宽度
  const column = columns.value.find((c: any) => {
    const colKey = col.key || col.dataIndex || col.title
    const cKey = c.key || c.dataIndex || c.title
    return colKey && cKey && String(colKey) === String(cKey)
  })
  if (column) {
    column.width = w
  }
}

// 新增
const handleCreate = () => {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.user._self') })
  formData.value = {}
  formVisible.value = true
}

// 编辑
const handleEdit = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.update') }))
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.user._self') })
  formData.value = toUserAssignRecord(record)
  formVisible.value = true
}

// 更新（工具栏按钮）
const handleUpdate = () => {
  if (selecmedRow.value) {
    if (isAdminUser(selecmedRow.value)) {
      message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.update') }))
      return
    }
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.user._self') }))
  }
}

// 删除单个
const handleDeleteOne = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.delete') }))
    return
  }
  const userName = getUserName(record) || t('common.tip.this.target', { target: t('entity.user._self') })
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.user._self'), name: userName }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteUserById(getUserId(record))
        message.success(t('common.feedback.deleted'))
        loadData()
      } catch (error: any) {
        message.error(error.message || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

// 删除（工具栏按钮）
const handleDelete = () => {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.user._self') }))
    return
  }

  // 检查是否包含admin用户
  const adminUsers = selecmedRows.value.filter(u => isAdminUser(u))
  if (adminUsers.length > 0) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.delete') }))
    return
  }

  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.user._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteUserBatch(selecmedRows.value.map(user => getUserId(user)))
        message.success(t('common.feedback.deleted'))
        selecmedRows.value = []
        selecmedRowKeys.value = []
        selecmedRow.value = null
        loadData()
      } catch (error: any) {
        message.error(error.message || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 打开分配用户角色弹窗 */
const handleAssignUserRole = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.allocate') + t('entity.role._self') }))
    return
  }
  currentAssignUser.value = toUserAssignRecord(record)
  assignUserRoleVisible.value = true
}

/** 打开分配用户可访问租户弹窗 */
const handleAssignUserTenant = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.allocate') + t('entity.tenant._self') }))
    return
  }
  currentAssignUser.value = toUserAssignRecord(record)
  assignUserTenantVisible.value = true
}

/** 打开分配用户可访问公司弹窗 */
const handleAssignUserCompany = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.allocate') + t('entity.company._self') }))
    return
  }
  currentAssignUser.value = toUserAssignRecord(record)
  assignUserCompanyVisible.value = true
}

// 分配部门
// 更新密码
const handleUpdatePassword = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.changepwd') }))
    return
  }
  currentChangePasswordUser.value = record
  changePasswordVisible.value = true
}

// 修改密码提交
const handleChangePasswordSubmit = async () => {
  try {
    if (!changePasswordFormRef.value) {
      return
    }
    
    await changePasswordFormRef.value.validate()
    const formValues = changePasswordFormRef.value.getValues()
    
    if (!currentChangePasswordUser.value) {
      message.error(t('common.validation.not.found', { field: t('entity.user._self') }))
      return
    }
    
    changePasswordLoading.value = true
    
    await changePassword(formValues)
    message.success(t('common.feedback.action.success', { action: t('common.page.button.changepwd') }))
    changePasswordVisible.value = false
    currentChangePasswordUser.value = null
    changePasswordFormRef.value?.resetFields()
  } catch (error: any) {
    if (error.errorFields) {
      // 表单验证错误
      return
    }
    message.error(error.message || t('common.feedback.action.failed', { action: t('common.page.button.changepwd') }))
  } finally {
    changePasswordLoading.value = false
  }
}

// 修改密码取消
const handleChangePasswordCancel = () => {
  changePasswordVisible.value = false
  currentChangePasswordUser.value = null
  changePasswordFormRef.value?.resetFields()
}

// 重置密码
const handleResetPassword = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.reset') + ' ' + t('common.page.button.password') }))
    return
  }
  const userName = getUserName(record) || t('common.tip.this.target', { target: t('entity.user._self') })
  Modal.confirm({
    title: t('common.page.button.reset') + ' ' + t('common.page.button.password'),
    content: t('common.tip.confirm.entity.action', { action: t('common.page.button.resetpwd'), entity: t('entity.user._self'), name: userName }),
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        // newPassword：契约必填；后端 ResetPasswordAsync 忽略该值，仅用配置 DefaultPassword
        await resetPassword({
          userId: getUserId(record),
          newPassword: ''
        })
        message.success(t('common.feedback.action.success', { action: t('common.page.button.reset') + ' ' + t('common.page.button.password') }))
        loadData()
      } catch (error: any) {
        message.error(error.message || t('common.feedback.action.failed', { action: t('common.page.button.reset') + ' ' + t('common.page.button.password') }))
      } finally {
        loading.value = false
      }
    }
  })
}

// 解除锁定
const handleUnlock = (record: User) => {
  if (isAdminUser(record)) {
    message.warning(t('common.tip.subject.cannot.action', { subject: t('common.tip.super.user'), action: t('common.page.button.unlock') }))
    return
  }
  const userName = getUserName(record) || t('common.tip.this.target', { target: t('entity.user._self') })
  Modal.confirm({
    title: t('common.tip.confirm.title', { action: t('common.page.button.unlock') }),
    content: t('common.tip.confirm.entity.action', { action: t('common.page.button.unlock'), entity: t('entity.user._self'), name: userName }),
    okText: t('common.page.button.unlock'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await unlock({ userId: getUserId(record) })
        message.success(t('common.feedback.action.success', { action: t('common.page.button.unlock') }))
        loadData()
      } catch (error: any) {
        message.error(error.message || t('common.feedback.action.failed', { action: t('common.page.button.unlock') }))
      } finally {
        loading.value = false
      }
    }
  })
}

// 分配成功回调
const handleAssignSuccess = () => {
  // 刷新数据列表
  loadData()
}

// 导入
const handleImport = () => {
  importVisible.value = true
}

// 下载导入模板（优先服务端 Content-Disposition，与导出一致）
const handleDownloadTemplate = async (sheetName?: string, fileName?: string) => {
  return await getUserTemplate(sheetName, fileName)
}

// 导入文件
const handleImportFile = async (file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> => {
  return await importUserData(file, sheetName)
}

// 导入成功回调
const handleImportSuccess = (result: { success: number; fail: number; errors: string[] }) => {
  userLogger.info('导入完成', { action: 'import', ...result })
  // 刷新数据列表
  loadData()
  // 如果全部成功，可以关闭导入对话框
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

// 取消导入
const handleImportCancel = () => {
  importVisible.value = false
}

// 导出
const handleExport = async () => {
  try {
    loading.value = true
    
    // 构建查询参数（使用当前查询条件）
    const queryParams: Partial<UserQuery> = {}

    if (queryKeyword.value) {
      queryParams.keyWords = queryKeyword.value
    }
    if (advancedQueryForm.value.userName) {
      queryParams.userName = advancedQueryForm.value.userName
    }
    if (advancedQueryForm.value.nickName) {
      queryParams.nickName = advancedQueryForm.value.nickName
    }
    if (advancedQueryForm.value.userStatus !== undefined) {
      queryParams.userStatus = advancedQueryForm.value.userStatus
    }

    const exportMeta: TaktBinaryDownload = await exportUserData(
      queryParams as Parameters<typeof exportUserData>[0],
      userExcelNames.sheet,
      userExcelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${userExcelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: exportMeta.contentDisposition ?? null,
      contentType: exportMeta.contentType ?? null,
      fallbackBase
    })

    const url = window.URL.createObjectURL(exportMeta.blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    
    // 延迟清理 URL，确保下载完成
    setTimeout(() => {
      window.URL.revokeObjectURL(url)
    }, 100)
    
    message.success(t('common.feedback.export.success'))
  } catch (error: any) {
    userLogger.error('导出失败', { action: 'export' }, error)
    message.error(error.message || t('common.feedback.export.failed'))
  } finally {
    loading.value = false
  }
}

// 高级查询
const handleAdvancedQuery = () => {
  advancedQueryVisible.value = true
}

// 高级查询提交
const handleAdvancedQuerySubmit = (_values?: Record<string, any>) => {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
  advancedQueryVisible.value = false
}

// 高级查询重置
const handleAdvancedQueryReset = () => {
  advancedQueryForm.value = emptyUserAdvancedQueryForm()
}

// 列设置
const handleColumnSetting = () => {
  columnSettingVisible.value = true
}

// 列设置变化 - TaktColumnDrawer 传递选中的 keys，更新 visibleColumnKeys
const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map(k => String(k))
}

// 列设置重置：TaktColumnDrawer 会自动重置为默认的9个列（ID + 7个字段 + 操作列）
const handleColumnSettingReset = () => {
  // TaktColumnDrawer 组件内部会自动处理重置逻辑
  // 这里只需要清空，让组件使用默认值
  visibleColumnKeys.value = []
}

// 刷新
const handleRefresh = () => {
  loadData()
}

// 表单提交
const handleFormSubmit = async () => {
  try {
    if (!formRef.value) {
      return
    }

    await formRef.value.validate()
    const formValues = formRef.value.getValues()

    formLoading.value = true

    if (formData.value.userId) {
      const currentUser = dataSource.value.find((u) => getUserId(u) === formData.value.userId)
      if (currentUser && isAdminUser(currentUser)) {
        message.warning(
          t('common.tip.user.cannot.action', {
            name: getUserName(currentUser) || 'admin',
            action: t('common.page.button.update')
          })
        )
        formLoading.value = false
        return
      }
      const fv = formValues as UserFormValues
      const updateData: UpdateUser = {
        userId: formData.value.userId,
        employeeId: fv.employeeId ? String(fv.employeeId) : '',
        userName: fv.userName ?? '',
        nickName: fv.nickName?.trim() ?? '',
        userType: fv.userType ?? 0,
        userStatus: fv.userStatus ?? 1,
        cultureCode: currentUser?.cultureCode?.trim() || resolveRequestLocale(),
        remark: fv.remark ?? '',
        passwordHash: '',
        roleIds: fv.roleIds?.map((id) => String(id)) ?? []
      }
      userLogger.debug('更新用户', { action: 'updateUser', userId: updateData.userId })
      await updateUser(formData.value.userId, updateData)
      message.success(t('common.feedback.updated'))
    } else {
      const fv = formValues as UserFormValues
      const createData: CreateUser = {
        employeeId: String(fv.employeeId || ''),
        userName: fv.userName ?? '',
        nickName: fv.nickName?.trim() ?? '',
        userType: fv.userType ?? 0,
        passwordHash: fv.password ?? '',
        userStatus: fv.userStatus ?? 1,
        cultureCode: resolveRequestLocale(),
        remark: fv.remark ?? '',
        roleIds: fv.roleIds?.map((id) => String(id)) ?? []
      }
      await createUser(createData)
      message.success(t('common.feedback.created'))
    }

    // 重置表单
    formRef.value?.resetFields()
    formData.value = {}
    
    formVisible.value = false
    loadData()
  } catch (error: any) {
    if (error.errorFields) {
      // 表单验证错误
      return
    }
    if (Array.isArray(error?.validationErrors) && error.validationErrors.length > 0) {
      formRef.value?.setServerValidationErrors?.(error.validationErrors)
      return
    }
    message.error(error.message || t('common.feedback.failed'))
  } finally {
    formLoading.value = false
  }
}

// 表单取消
const handleFormCancel = () => {
  formVisible.value = false
  formData.value = {}
  if (formRef.value) {
    formRef.value.resetFields()
  }
}

</script>
