<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-resignation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getEmployeeId"
      :master-row-selection="rowSelection"
      master-id-column-key="employeeId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="human:resource:personnel:employee:resignation:create"
      update-permission="human:resource:personnel:employee:resignation:update"
      delete-permission="human:resource:personnel:employee:resignation:delete"
      import-permission="human:resource:personnel:employee:resignation:import"
      export-permission="human:resource:personnel:employee:resignation:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
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
      </template>
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'gender'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'gender')"
            dict-type="sys_user_gender"
          />
        </template>
        <template v-else-if="column.key === 'nativePlace'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'nativePlace')"
            dict-type="humanresource_personnel_native_place_code"
          />
        </template>
        <template v-else-if="column.key === 'ethnicity'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'ethnicity')"
            dict-type="humanresource_personnel_ethnic_code"
          />
        </template>
        <template v-else-if="column.key === 'politicalAffiliation'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'politicalAffiliation')"
            dict-type="humanresource_personnel_political_affiliation"
          />
        </template>
        <template v-else-if="column.key === 'maritalStatus'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'maritalStatus')"
            dict-type="humanresource_personnel_marital_status"
          />
        </template>
        <template v-else-if="column.key === 'employeeStatus'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'employeeStatus')"
            dict-type="humanresource_personnel_employee_status"
          />
        </template>
        <template v-else-if="column.key === 'isBuiltIn'">
          <TaktDictTag
            :value="getEmployeeDictValue(record, 'isBuiltIn')"
            dict-type="sys_yes_no"
          />
        </template>
      </template>
      <template #detail>
        <EmployeeResignationPanel
          ref="employeeResignationPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

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
      <EmployeeForm
        :key="formData?.employeeId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-human-resource-personnel-employee-resignation'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeCode')">
      <a-form-item :label="pi.queryLabel('employeeCode')">
        <a-input
          v-model:value="advancedQueryForm.employeeCode"
          :placeholder="pi.queryPh('employeeCode', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="pi.queryLabel('employeeName')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="pi.queryPh('employeeName', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('gender')">
      <a-form-item :label="pi.queryLabel('gender')">
        <TaktSelect
          v-model:value="advancedQueryForm.gender"
          dict-type="sys_user_gender"
          :placeholder="pi.queryPh('gender', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('birthDateStart')">
      <a-form-item :label="pi.queryLabel('birthDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.birthDateStart"
          :placeholder="pi.queryPh('birthDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('birthDateEnd')">
      <a-form-item :label="pi.queryLabel('birthDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.birthDateEnd"
          :placeholder="pi.queryPh('birthDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idCardCode')">
      <a-form-item :label="pi.queryLabel('idCardCode')">
        <a-input
          v-model:value="advancedQueryForm.idCardCode"
          :placeholder="pi.queryPh('idCardCode', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mobile')">
      <a-form-item :label="pi.queryLabel('mobile')">
        <a-input
          v-model:value="advancedQueryForm.mobile"
          :placeholder="pi.queryPh('mobile', 'required')"
          show-count
          :maxlength="11"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('email')">
      <a-form-item :label="pi.queryLabel('email')">
        <a-input
          v-model:value="advancedQueryForm.email"
          :placeholder="pi.queryPh('email', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nativePlace')">
      <a-form-item :label="pi.queryLabel('nativePlace')">
        <TaktSelect
          v-model:value="advancedQueryForm.nativePlace"
          dict-type="humanresource_personnel_native_place_code"
          :placeholder="pi.queryPh('nativePlace', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ethnicity')">
      <a-form-item :label="pi.queryLabel('ethnicity')">
        <TaktSelect
          v-model:value="advancedQueryForm.ethnicity"
          dict-type="humanresource_personnel_ethnic_code"
          :placeholder="pi.queryPh('ethnicity', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('politicalAffiliation')">
      <a-form-item :label="pi.queryLabel('politicalAffiliation')">
        <TaktSelect
          v-model:value="advancedQueryForm.politicalAffiliation"
          dict-type="humanresource_personnel_political_affiliation"
          :placeholder="pi.queryPh('politicalAffiliation', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maritalStatus')">
      <a-form-item :label="pi.queryLabel('maritalStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.maritalStatus"
          dict-type="humanresource_personnel_marital_status"
          :placeholder="pi.queryPh('maritalStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeStatus')">
      <a-form-item :label="pi.queryLabel('employeeStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.employeeStatus"
          dict-type="humanresource_personnel_employee_status"
          :placeholder="pi.queryPh('employeeStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="pi.queryLabel('isBuiltIn')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isBuiltIn', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('avatar')">
      <a-form-item :label="pi.queryLabel('avatar')">
        <a-input
          v-model:value="advancedQueryForm.avatar"
          :placeholder="pi.queryPh('avatar', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="EMPLOYEE_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'employeeId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 员工实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee-resignation
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import EmployeeForm from './components/EmployeeForm.vue'
import EmployeeResignationPanel from './components/EmployeeResignationPanel.vue'
import { provideEmployeeMasterContext, type EmployeeRowRecord } from './composables/useEmployeeMasterContext'
import { getEmployeeList, getEmployeeById, createEmployee, updateEmployee, deleteEmployeeById, deleteEmployeeBatch, getEmployeeTemplate, importEmployee, exportEmployee, updateEmployeeStatus } from '@/api/human-resource/personnel/employee.api'
import type { Employee, EmployeeQuery } from '@/types/human-resource/personnel/employee.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useEmployeeI18n,
  EMPLOYEE_LIST_FIELDS,
  EMPLOYEE_QUERY_STRING_FIELDS,
  EMPLOYEE_QUERY_FIELDS,
  EMPLOYEE_SELF_I18N_KEY,
} from './composables/useEmployeeI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useEmployeeI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployee')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Employee[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EmployeeRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<EmployeeRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Employee> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of EMPLOYEE_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.gender !== undefined && form.gender !== null) {
    return true
  }
  if (form.ethnicity !== undefined && form.ethnicity !== null) {
    return true
  }
  if (form.politicalAffiliation !== undefined && form.politicalAffiliation !== null) {
    return true
  }
  if (form.maritalStatus !== undefined && form.maritalStatus !== null) {
    return true
  }
  if (form.employeeStatus !== undefined && form.employeeStatus !== null) {
    return true
  }
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(EMPLOYEE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof EMPLOYEE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    gender: undefined as number | undefined,
    ethnicity: undefined as number | undefined,
    politicalAffiliation: undefined as number | undefined,
    maritalStatus: undefined as number | undefined,
    employeeStatus: undefined as number | undefined,
    isBuiltIn: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  EMPLOYEE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'employeeId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideEmployeeMasterContext()
const employeeResignationPanelRef = ref<InstanceType<typeof EmployeeResignationPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EmployeeQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EmployeeQuery>): EmployeeQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EmployeeQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EmployeeQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of EMPLOYEE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.gender !== undefined && form.gender !== null) {
    query.gender = form.gender
  }
  if (form.ethnicity !== undefined && form.ethnicity !== null) {
    query.ethnicity = form.ethnicity
  }
  if (form.politicalAffiliation !== undefined && form.politicalAffiliation !== null) {
    query.politicalAffiliation = form.politicalAffiliation
  }
  if (form.maritalStatus !== undefined && form.maritalStatus !== null) {
    query.maritalStatus = form.maritalStatus
  }
  if (form.employeeStatus !== undefined && form.employeeStatus !== null) {
    query.employeeStatus = form.employeeStatus
  }
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    query.isBuiltIn = form.isBuiltIn
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: EmployeeRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getEmployeeId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as EmployeeRowRecord
  const key = getEmployeeId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadEmployeeDetail(record: EmployeeRowRecord): Promise<Employee | null> {
  const id = getEmployeeId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getEmployeeById(id)
    const index = dataSource.value.findIndex((row) => getEmployeeId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Employee
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeId') ?? ''
  },
  {
    title: pi.label('employeeCode'),
    dataIndex: 'employeeCode',
    key: 'employeeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeCode') ?? ''
  },
  {
    title: pi.label('employeeName'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeName') ?? ''
  },
  {
    title: pi.label('gender'),
    dataIndex: 'gender',
    key: 'gender',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('birthDate'),
    dataIndex: 'birthDate',
    key: 'birthDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'birthDate') ?? ''
  },
  {
    title: pi.label('idCardCode'),
    dataIndex: 'idCardCode',
    key: 'idCardCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'idCardCode') ?? ''
  },
  {
    title: pi.label('mobile'),
    dataIndex: 'mobile',
    key: 'mobile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'mobile') ?? ''
  },
  {
    title: pi.label('email'),
    dataIndex: 'email',
    key: 'email',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'email') ?? ''
  },
  {
    title: pi.label('nativePlace'),
    dataIndex: 'nativePlace',
    key: 'nativePlace',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('ethnicity'),
    dataIndex: 'ethnicity',
    key: 'ethnicity',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('politicalAffiliation'),
    dataIndex: 'politicalAffiliation',
    key: 'politicalAffiliation',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('maritalStatus'),
    dataIndex: 'maritalStatus',
    key: 'maritalStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('employeeStatus'),
    dataIndex: 'employeeStatus',
    key: 'employeeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isBuiltIn'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('avatar'),
    dataIndex: 'avatar',
    key: 'avatar',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'avatar') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:personnel:employee:resignation:update',
        onClick: (record: EmployeeRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:personnel:employee:resignation:delete',
        onClick: (record: EmployeeRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEmployeeId = (record: EmployeeRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEmployeeField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getEmployeeDictValue = (
  record: EmployeeRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EmployeeRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: EmployeeRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getEmployeeId(selectedRow.value) === getEmployeeId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getEmployeeList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Employee] 加载数据失败', { error })
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
  cultureCode: '',
  plantCode: '',
  employeeCode: '',
  employeeName: '',
  gender: undefined as number | undefined,
  birthDateStart: '',
  birthDateEnd: '',
  idCardCode: '',
  mobile: '',
  email: '',
  nativePlace: '',
  ethnicity: undefined as number | undefined,
  politicalAffiliation: undefined as number | undefined,
  maritalStatus: undefined as number | undefined,
  employeeStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  avatar: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: EmployeeRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadEmployeeDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
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
      await updateEmployee(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createEmployee(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  employeeResignationPanelRef.value?.reload?.()
    }
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEmployeeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importEmployee(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    employeeResignationPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportEmployee(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[Employee] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EmployeeRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
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
  cultureCode: '',
  plantCode: '',
  employeeCode: '',
  employeeName: '',
  gender: undefined as number | undefined,
  birthDateStart: '',
  birthDateEnd: '',
  idCardCode: '',
  mobile: '',
  email: '',
  nativePlace: '',
  ethnicity: undefined as number | undefined,
  politicalAffiliation: undefined as number | undefined,
  maritalStatus: undefined as number | undefined,
  employeeStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  avatar: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
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
</script>
