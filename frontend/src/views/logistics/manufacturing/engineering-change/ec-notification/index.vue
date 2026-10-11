<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-notification -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：工程变更通知单管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:engineering:change:gijutsu:create"
      update-permission="logistics:manufacturing:engineering:change:gijutsu:update"
      delete-permission="logistics:manufacturing:engineering:change:gijutsu:delete"
      import-permission="logistics:manufacturing:engineering:change:gijutsu:import"
      export-permission="logistics:manufacturing:engineering:change:gijutsu:export"
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

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="approval"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecNotificationId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcNotificationId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

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
      <EcNotificationForm
        :key="formData?.ecNotificationId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-ec-notification'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="pi.queryPh('plantCode', 'optional')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationCode')">
      <a-form-item :label="pi.queryLabel('ecNotificationCode')">
        <a-input
          v-model:value="advancedQueryForm.ecNotificationCode"
          :placeholder="pi.queryPh('ecNotificationCode', 'required')"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecGijutsuId')">
      <a-form-item :label="pi.queryLabel('ecGijutsuId')">
        <a-input
          v-model:value="advancedQueryForm.ecGijutsuId"
          :placeholder="pi.queryPh('ecGijutsuId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecCode')">
      <a-form-item :label="pi.queryLabel('ecCode')">
        <a-input
          v-model:value="advancedQueryForm.ecCode"
          :placeholder="pi.queryPh('ecCode', 'required')"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecTitle')">
      <a-form-item :label="pi.queryLabel('ecTitle')">
        <a-input
          v-model:value="advancedQueryForm.ecTitle"
          :placeholder="pi.queryPh('ecTitle', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationDateStart')">
      <a-form-item :label="pi.queryLabel('ecNotificationDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecNotificationDateStart"
          :placeholder="pi.queryPh('ecNotificationDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationDateEnd')">
      <a-form-item :label="pi.queryLabel('ecNotificationDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecNotificationDateEnd"
          :placeholder="pi.queryPh('ecNotificationDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationDeptCodes')">
      <a-form-item :label="pi.queryLabel('ecNotificationDeptCodes')">
        <a-input
          v-model:value="advancedQueryForm.ecNotificationDeptCodes"
          :placeholder="pi.queryPh('ecNotificationDeptCodes', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationDeptNames')">
      <a-form-item :label="pi.queryLabel('ecNotificationDeptNames')">
        <a-input
          v-model:value="advancedQueryForm.ecNotificationDeptNames"
          :placeholder="pi.queryPh('ecNotificationDeptNames', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationNotifierGroupCode')">
      <a-form-item :label="pi.queryLabel('ecNotificationNotifierGroupCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecNotificationNotifierGroupCode"
          api-url="TaktEcGroups/options"
          :placeholder="pi.queryPh('ecNotificationNotifierGroupCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationNotifierGroupName')">
      <a-form-item :label="pi.queryLabel('ecNotificationNotifierGroupName')">
        <a-input
          v-model:value="advancedQueryForm.ecNotificationNotifierGroupName"
          :placeholder="pi.queryPh('ecNotificationNotifierGroupName', 'optional')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationMethod')">
      <a-form-item :label="pi.queryLabel('ecNotificationMethod')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNotificationMethod"
          :placeholder="pi.queryPh('ecNotificationMethod', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNotificationStatus')">
      <a-form-item :label="pi.queryLabel('ecNotificationStatus')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNotificationStatus"
          :placeholder="pi.queryPh('ecNotificationStatus', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="pi.queryLabel('approvalStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="pi.queryPh('approvalStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="pi.queryLabel('initiatorId')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="pi.queryPh('initiatorId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="pi.queryLabel('initiatedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="pi.queryPh('initiatedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="pi.queryLabel('initiatedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="pi.queryPh('initiatedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="pi.queryLabel('approvedBy')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="pi.queryPh('approvedBy', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="pi.queryLabel('approvedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="pi.queryPh('approvedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="pi.queryLabel('approvedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="pi.queryPh('approvedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="pi.queryLabel('flowInstanceId')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="pi.queryPh('flowInstanceId', 'required')"
          show-count
          :maxlength="20"
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
        :entity-i18n-key="ECNOTIFICATION_SELF_I18N_KEY"
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
      :id-column-key="'ecNotificationId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 工程变更通知单管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-notification
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import EcNotificationForm from './components/EcNotificationForm.vue'
import { getEcNotificationList, getEcNotificationById, createEcNotification, updateEcNotification, deleteEcNotificationById, deleteEcNotificationBatch, getEcNotificationTemplate, importEcNotification, exportEcNotification, updateEcNotificationStatus } from '@/api/logistics/manufacturing/engineering-change/ecNotification.api'
import type { EcNotification, EcNotificationQuery } from '@/types/logistics/manufacturing/engineering-change/ecNotification.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import {
  useEcNotificationI18n,
  ECNOTIFICATION_QUERY_FIELDS,
  ECNOTIFICATION_SELF_I18N_KEY,
} from './composables/useEcNotificationI18n'

/** 实体字段 i18n（视图只传小驼峰字段名） */
const pi = useEcNotificationI18n()
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcNotification')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EcNotification[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EcNotification | null>(null)
/** 表格多选行 */
const selectedRows = ref<EcNotification[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EcNotification> | null>(null)
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
  plantCode: '',
  ecNotificationCode: '',
  ecGijutsuId: '',
  ecCode: '',
  ecTitle: '',
  ecNotificationDateStart: '',
  ecNotificationDateEnd: '',
  ecNotificationDeptCodes: '',
  ecNotificationDeptNames: '',
  ecNotificationNotifierGroupCode: '',
  ecNotificationNotifierGroupName: '',
  ecNotificationMethod: undefined as number | undefined,
  ecNotificationStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ECNOTIFICATION_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'ecNotificationId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcNotificationQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcNotificationQuery>): EcNotificationQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcNotificationQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcNotificationQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('ecNotificationCode', form.ecNotificationCode)
  assignTrimmed('ecGijutsuId', form.ecGijutsuId)
  assignTrimmed('ecCode', form.ecCode)
  assignTrimmed('ecTitle', form.ecTitle)
  assignTrimmed('ecNotificationDateStart', form.ecNotificationDateStart)
  assignTrimmed('ecNotificationDateEnd', form.ecNotificationDateEnd)
  assignTrimmed('ecNotificationDeptCodes', form.ecNotificationDeptCodes)
  assignTrimmed('ecNotificationDeptNames', form.ecNotificationDeptNames)
  assignTrimmed('ecNotificationNotifierGroupCode', form.ecNotificationNotifierGroupCode)
  assignTrimmed('ecNotificationNotifierGroupName', form.ecNotificationNotifierGroupName)
  if (form.ecNotificationMethod !== undefined && form.ecNotificationMethod !== null) {
    query.ecNotificationMethod = form.ecNotificationMethod
  }
  if (form.ecNotificationStatus !== undefined && form.ecNotificationStatus !== null) {
    query.ecNotificationStatus = form.ecNotificationStatus
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecNotificationId',
    key: 'ecNotificationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationId') ?? ''
  },
  {
    title: pi.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'plantCode') ?? ''
  },
  {
    title: pi.label('ecNotificationCode'),
    dataIndex: 'ecNotificationCode',
    key: 'ecNotificationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationCode') ?? ''
  },
  {
    title: pi.label('ecGijutsuId'),
    dataIndex: 'ecGijutsuId',
    key: 'ecGijutsuId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecGijutsuId') ?? ''
  },
  {
    title: pi.label('ecCode'),
    dataIndex: 'ecCode',
    key: 'ecCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecCode') ?? ''
  },
  {
    title: pi.label('ecTitle'),
    dataIndex: 'ecTitle',
    key: 'ecTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecTitle') ?? ''
  },
  {
    title: pi.label('ecNotificationDate'),
    dataIndex: 'ecNotificationDate',
    key: 'ecNotificationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationDate') ?? ''
  },
  {
    title: pi.label('ecNotificationDeptCodes'),
    dataIndex: 'ecNotificationDeptCodes',
    key: 'ecNotificationDeptCodes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationDeptCodes') ?? ''
  },
  {
    title: pi.label('ecNotificationDeptNames'),
    dataIndex: 'ecNotificationDeptNames',
    key: 'ecNotificationDeptNames',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationDeptNames') ?? ''
  },
  {
    title: pi.label('ecNotificationNotifierGroupCode'),
    dataIndex: 'ecNotificationNotifierGroupCode',
    key: 'ecNotificationNotifierGroupCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationNotifierGroupCode') ?? ''
  },
  {
    title: pi.label('ecNotificationNotifierGroupName'),
    dataIndex: 'ecNotificationNotifierGroupName',
    key: 'ecNotificationNotifierGroupName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationNotifierGroupName') ?? ''
  },
  {
    title: pi.label('ecNotificationMethod'),
    dataIndex: 'ecNotificationMethod',
    key: 'ecNotificationMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationMethod') ?? ''
  },
  {
    title: pi.label('ecNotificationStatus'),
    dataIndex: 'ecNotificationStatus',
    key: 'ecNotificationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNotificationField(record, 'ecNotificationStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:update',
        onClick: (record: EcNotification) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:delete',
        onClick: (record: EcNotification) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEcNotificationId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEcNotificationField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcNotification[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcNotification, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getEcNotificationId(selectedRow.value) === getEcNotificationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcNotification[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EcNotification) => ({
  onClick: () => {
    const key = getEcNotificationId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEcNotificationId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getEcNotificationList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EcNotification] 加载数据失败', { error })
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
  plantCode: '',
  ecNotificationCode: '',
  ecGijutsuId: '',
  ecCode: '',
  ecTitle: '',
  ecNotificationDateStart: '',
  ecNotificationDateEnd: '',
  ecNotificationDeptCodes: '',
  ecNotificationDeptNames: '',
  ecNotificationNotifierGroupCode: '',
  ecNotificationNotifierGroupName: '',
  ecNotificationMethod: undefined as number | undefined,
  ecNotificationStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
/** 打开编辑弹窗 */
function handleEdit(record: EcNotification) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
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
      await updateEcNotification(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createEcNotification(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getEcNotificationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importEcNotification(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
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
    const exportMeta = await exportEcNotification(
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
    logger.error('[EcNotification] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EcNotification) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcNotificationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
      await deleteEcNotificationBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
  plantCode: '',
  ecNotificationCode: '',
  ecGijutsuId: '',
  ecCode: '',
  ecTitle: '',
  ecNotificationDateStart: '',
  ecNotificationDateEnd: '',
  ecNotificationDeptCodes: '',
  ecNotificationDeptNames: '',
  ecNotificationNotifierGroupCode: '',
  ecNotificationNotifierGroupName: '',
  ecNotificationMethod: undefined as number | undefined,
  ecNotificationStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
