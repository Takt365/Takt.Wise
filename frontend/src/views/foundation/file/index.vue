<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/file -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：文件实体管理页面，含查询、上传、删除、导出与只读详情，不支持导入与编辑 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-file">
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
      create-permission="foundation:file:create"
      delete-permission="foundation:file:delete"
      export-permission="foundation:file:export"
      :show-create="true"
      :show-update="false"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @delete="handleDelete"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <div class="foundation-file-table-wrap">
      <TaktSingleTable
        :scroll="tableScroll"
        :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'fileId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getFileId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'storageType'">
          <TaktDictTag
            :value="getFileField(record, 'storageType')"
            dict-type="sys_storage_type"
          />
        </template>
        <template v-else-if="column.key === 'fileHash'">
          <a-typography-link
            v-if="canDownloadFile(record)"
            :disabled="downloadingFileId === getFileId(record)"
            @click.stop="handleDownloadFile(record)"
          >
            {{ getFileDisplayName(record) }}
          </a-typography-link>
          <span v-else>{{ getFileDisplayName(record) || '-' }}</span>
        </template>
        <template v-else-if="column.key === 'fileStatus'">
          <a-switch
            :checked="getFileField(record, 'fileStatus') === 1"
            :disabled="getFileField(record, 'fileStatus') === TAKT_FILE_STATUS_LOCKED"
            :checked-children="t('common.page.button.enable')"
            :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleFileStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'isPublic'">
          <a-switch
            :checked="getFileField(record, 'isPublic') === 0"
            :checked-children="t('dict.sys.public.type.0')"
            :un-checked-children="t('dict.sys.public.type.1')"
            @change="(checked: unknown) => handleFilePublicChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'fileTags'">
          <TaktTagColor :value="getFileField(record, 'fileTags')" />
        </template>
      </template>

    </TaktSingleTable>
    </div>

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增上传对话框 -->
    <TaktModal
      v-model:open="uploadVisible"
      :title="t('common.dialog.title.create', { entity: t('entity.file._self') })"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="uploadLoading"
      @ok="handleUploadSubmit"
      @cancel="handleUploadCancel"
    >
      <FileFormUpload ref="uploadFormRef" :loading="uploadLoading" />
    </TaktModal>
    <!-- 详情对话框 -->
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('entity.file._self') })"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <FileDetail :detail="detailData" @downloaded="handleDetailDownloaded" />
      </a-spin>
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-foundation-file'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('fileCode')">
      <a-form-item :label="t('entity.file.code')">
        <a-input
          v-model:value="advancedQueryForm.fileCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="t('entity.file.name')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileOriginalName')">
      <a-form-item :label="t('entity.file.originalname')">
        <a-input
          v-model:value="advancedQueryForm.fileOriginalName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.originalname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('filePath')">
      <a-form-item :label="t('entity.file.path')">
        <a-input
          v-model:value="advancedQueryForm.filePath"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.path') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileSize')">
      <a-form-item :label="t('entity.file.size')">
        <a-input
          v-model:value="advancedQueryForm.fileSize"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.size') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileType')">
      <a-form-item :label="t('entity.file.type')">
        <a-input
          v-model:value="advancedQueryForm.fileType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileExtension')">
      <a-form-item :label="t('entity.file.extension')">
        <a-input
          v-model:value="advancedQueryForm.fileExtension"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.extension') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileHash')">
      <a-form-item :label="t('entity.file.hash')">
        <a-input
          v-model:value="advancedQueryForm.fileHash"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.hash') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileCategory')">
      <a-form-item :label="t('entity.file.category')">
        <a-input-number
          v-model:value="advancedQueryForm.fileCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.category') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storageType')">
      <a-form-item :label="t('entity.file.storagetype')">
        <TaktSelect
          v-model:value="advancedQueryForm.storageType"
          dict-type="sys_storage_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.storagetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storageConfig')">
      <a-form-item :label="t('entity.file.storageconfig')">
        <a-input
          v-model:value="advancedQueryForm.storageConfig"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.storageconfig') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accessUrl')">
      <a-form-item :label="t('entity.file.accessurl')">
        <a-input
          v-model:value="advancedQueryForm.accessUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.accessurl') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downloadCount')">
      <a-form-item :label="t('entity.file.downloadcount')">
        <a-input-number
          v-model:value="advancedQueryForm.downloadCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.downloadcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastDownloadTimeStart')">
      <a-form-item :label="t('entity.file.lastdownloadtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastDownloadTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.lastdownloadtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastDownloadTimeEnd')">
      <a-form-item :label="t('entity.file.lastdownloadtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastDownloadTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.lastdownloadtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileStatus')">
      <a-form-item :label="t('entity.file.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.fileStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPublic')">
      <a-form-item :label="t('entity.file.ispublic')">
        <TaktSelect
          v-model:value="advancedQueryForm.isPublic"
          dict-type="sys_public_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.file.ispublic') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileDescription')">
      <a-form-item :label="t('entity.file.description')">
        <a-textarea
          v-model:value="advancedQueryForm.fileDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.file.description') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileTags')">
      <a-form-item :label="t('entity.file.tags')">
        <a-input
          v-model:value="advancedQueryForm.fileTags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.tags') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ipAddress')">
      <a-form-item :label="t('entity.file.ipaddress')">
        <a-textarea
          v-model:value="advancedQueryForm.ipAddress"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.file.ipaddress') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('location')">
      <a-form-item :label="t('entity.file.location')">
        <a-input
          v-model:value="advancedQueryForm.location"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.file.location') })"
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
      :id-column-key="'fileId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
/**
 * 文件实体 公司级实体：文件元数据按租户+公司隔离管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/file
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import FileFormUpload from './components/FileFormUpload.vue'
import FileDetail from './components/FileDetail.vue'
import { getFileList, getFileById, deleteFileById, deleteFileBatch, exportFile, downloadFileById, updateFileStatus, updateFilePublic } from '@/api/foundation/file.api'
import type { File, FileQuery } from '@/types/foundation/file.types'
import { isFileStatusEnabled, TAKT_FILE_STATUS_LOCKED } from '@/utils/taktFileStatus'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEyeLine, RiDeleteBinLine } from '@remixicon/vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 文件下载权限（与 Foundation 按钮 download 种子、TaktFiles/{id}/download 一致） */
const permissionStore = usePermissionStore()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktFile')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.file._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<File[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 表格 scroll.y（服务端分页固定视口高度） */
const tableScroll = { y: 'calc(100vh - 300px)' } as const
/** 工具栏单选时当前行 */
const selectedRow = ref<File | null>(null)
/** 表格多选行 */
const selectedRows = ref<File[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])
/** 正在下载的文件 ID（行内链接 loading） */
const downloadingFileId = ref('')

/** 新增上传弹窗是否打开 */
const uploadVisible = ref(false)
/** 上传提交 loading */
const uploadLoading = ref(false)
/** 上传表单 ref（validate / resetFields / uploadFiles） */
const uploadFormRef = ref<InstanceType<typeof FileFormUpload> | null>(null)
/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情展示数据 */
const detailData = ref<Partial<File> | null>(null)
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  fileCode: '',
  fileName: '',
  fileOriginalName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  fileHash: '',
  fileCategory: undefined as number | undefined,
  storageType: undefined as number | undefined,
  storageConfig: '',
  accessUrl: '',
  downloadCount: undefined as number | undefined,
  lastDownloadTimeStart: '',
  lastDownloadTimeEnd: '',
  fileStatus: undefined as number | undefined,
  isPublic: undefined as number | undefined,
  fileDescription: '',
  fileTags: '',
  ipAddress: '',
  location: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'fileCode', label: t('entity.file.code') },
  { key: 'fileName', label: t('entity.file.name') },
  { key: 'fileOriginalName', label: t('entity.file.originalname') },
  { key: 'filePath', label: t('entity.file.path') },
  { key: 'fileSize', label: t('entity.file.size') },
  { key: 'fileType', label: t('entity.file.type') },
  { key: 'fileExtension', label: t('entity.file.extension') },
  { key: 'fileHash', label: t('entity.file.hash') },
  { key: 'fileCategory', label: t('entity.file.category') },
  { key: 'storageType', label: t('entity.file.storagetype') },
  { key: 'storageConfig', label: t('entity.file.storageconfig') },
  { key: 'accessUrl', label: t('entity.file.accessurl') },
  { key: 'downloadCount', label: t('entity.file.downloadcount') },
  { key: 'lastDownloadTimeStart', label: t('entity.file.lastdownloadtimestart') },
  { key: 'lastDownloadTimeEnd', label: t('entity.file.lastdownloadtimeend') },
  { key: 'fileStatus', label: t('entity.file.status') },
  { key: 'isPublic', label: t('entity.file.ispublic') },
  { key: 'fileDescription', label: t('entity.file.description') },
  { key: 'fileTags', label: t('entity.file.tags') },
  { key: 'ipAddress', label: t('entity.file.ipaddress') },
  { key: 'location', label: t('entity.file.location') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'fileId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 组装列表/导出查询参数（空字符串与未填数值不下发，避免后端模型绑定 400） */
function buildListQuery(overrides?: Partial<FileQuery>): FileQuery {
  const form = advancedQueryForm.value
  const query: FileQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof FileQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('fileCode', form.fileCode)
  assignTrimmed('fileName', form.fileName)
  assignTrimmed('fileOriginalName', form.fileOriginalName)
  assignTrimmed('filePath', form.filePath)
  assignTrimmed('fileSize', form.fileSize)
  assignTrimmed('fileType', form.fileType)
  assignTrimmed('fileExtension', form.fileExtension)
  assignTrimmed('fileHash', form.fileHash)
  assignTrimmed('storageConfig', form.storageConfig)
  assignTrimmed('accessUrl', form.accessUrl)
  assignTrimmed('fileDescription', form.fileDescription)
  assignTrimmed('fileTags', form.fileTags)
  assignTrimmed('ipAddress', form.ipAddress)
  assignTrimmed('location', form.location)
  assignTrimmed('lastDownloadTimeStart', form.lastDownloadTimeStart)
  assignTrimmed('lastDownloadTimeEnd', form.lastDownloadTimeEnd)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
  assignTrimmed('remark', form.remark)
  if (form.fileCategory !== undefined && form.fileCategory !== null) {
    query.fileCategory = form.fileCategory
  }
  if (form.storageType !== undefined && form.storageType !== null) {
    query.storageType = form.storageType
  }
  if (form.downloadCount !== undefined && form.downloadCount !== null) {
    query.downloadCount = form.downloadCount
  }
  if (form.fileStatus !== undefined && form.fileStatus !== null) {
    query.fileStatus = form.fileStatus
  }
  if (form.isPublic !== undefined && form.isPublic !== null) {
    query.isPublic = form.isPublic
  }
  return query
}

/** 页面挂载后加载分页列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'fileId',
    key: 'fileId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileId') ?? ''
  },
  {
    title: t('entity.file.code'),
    dataIndex: 'fileCode',
    key: 'fileCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileCode') ?? ''
  },
  {
    title: t('entity.file.hash'),
    dataIndex: 'fileHash',
    key: 'fileHash',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileHash') ?? ''
  },
  {
    title: t('entity.file.storagetype'),
    dataIndex: 'storageType',
    key: 'storageType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.file.status'),
    dataIndex: 'fileStatus',
    key: 'fileStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.file.ispublic'),
    dataIndex: 'isPublic',
    key: 'isPublic',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.file.tags'),
    dataIndex: 'fileTags',
    key: 'fileTags',
    width: 160,
    resizable: true,
    ellipsis: false,
  },
  {
    title: t('entity.file.downloadcount'),
    dataIndex: 'downloadCount',
    key: 'downloadCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'downloadCount') ?? ''
  },
  {
    title: t('entity.file.lastdownloadtime'),
    dataIndex: 'lastDownloadTime',
    key: 'lastDownloadTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'lastDownloadTime') ?? ''
  },
  {
    title: t('entity.file.name'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 160,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.file.originalname'),
    dataIndex: 'fileOriginalName',
    key: 'fileOriginalName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileOriginalName') ?? ''
  },
  {
    title: t('entity.file.path'),
    dataIndex: 'filePath',
    key: 'filePath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'filePath') ?? ''
  },
  {
    title: t('entity.file.size'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileSize') ?? ''
  },
  {
    title: t('entity.file.type'),
    dataIndex: 'fileType',
    key: 'fileType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileType') ?? ''
  },
  {
    title: t('entity.file.extension'),
    dataIndex: 'fileExtension',
    key: 'fileExtension',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileExtension') ?? ''
  },

  {
    title: t('entity.file.category'),
    dataIndex: 'fileCategory',
    key: 'fileCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileCategory') ?? ''
  },

  {
    title: t('entity.file.storageconfig'),
    dataIndex: 'storageConfig',
    key: 'storageConfig',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'storageConfig') ?? ''
  },
  {
    title: t('entity.file.accessurl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'accessUrl') ?? ''
  },

  {
    title: t('entity.file.description'),
    dataIndex: 'fileDescription',
    key: 'fileDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'fileDescription') ?? ''
  },

  {
    title: t('entity.file.ipaddress'),
    dataIndex: 'ipAddress',
    key: 'ipAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'ipAddress') ?? ''
  },
  {
    title: t('entity.file.location'),
    dataIndex: 'location',
    key: 'location',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFileField(record, 'location') ?? ''
  },
  CreateActionColumn({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'foundation:file:query',
        onClick: (record: File) => handleShowDetail(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:file:delete',
        onClick: (record: File) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getFileId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getFileField = (record: any, field: string): any => record?.[field]

/**
 * 表格展示用文件名（优先原始名）
 * @param record 行数据
 * @returns {string} 展示名
 */
function getFileDisplayName(record: File): string {
  const original = getFileField(record, 'fileHashOriginalName')
  const stored = getFileField(record, 'fileName')
  const name = (original ?? stored ?? '').toString().trim()
  return name
}

/**
 * 是否允许下载（启用态且有主键）
 * @param record 行数据
 * @returns 可下载返回 true
 */
function canDownloadFile(record: File): boolean {
  const id = getFileId(record)
  if (!id) {
    return false
  }
  if (!permissionStore.hasPermission('foundation:file:download')) {
    return false
  }
  return isFileStatusEnabled(getFileField(record, 'fileStatus'))
}

/**
 * 点击文件名称下载
 * @param record 当前行
 */
async function handleDownloadFile(record: File) {
  const id = getFileId(record)
  if (!id || !canDownloadFile(record)) {
    return
  }
  const fallbackName = getFileDisplayName(record) || 'download'
  downloadingFileId.value = id
  try {
    const blob = await downloadFileById(id)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fallbackName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.success'))
    await loadData()
  } catch (error: unknown) {
    const err = error as { message?: string }
    logger.error('[File] 下载失败', { fileId: id, error })
    message.error(err?.message || t('common.feedback.failed'))
  } finally {
    downloadingFileId.value = ''
  }
}

/**
 * 表格行内切换文件状态（sys_normal_disable：1=启用，0=禁用；2=锁定不可切换）
 * @param record 当前行
 * @param checked 开关是否选中（启用）
 */
async function handleFileStatusChange(record: File, checked: boolean) {
  const id = getFileId(record)
  if (!id || getFileField(record, 'fileStatus') === TAKT_FILE_STATUS_LOCKED) {
    return
  }
  const newStatus = checked ? 1 : 0
  const oldStatus = getFileField(record, 'fileStatus')
  ;(record as File).fileStatus = newStatus
  try {
    await updateFileStatus({ fileId: id, fileStatus: newStatus })
    message.success(t('common.feedback.updated'))
  } catch (error: unknown) {
    ;(record as File).fileStatus = oldStatus
    const err = error as { message?: string }
    logger.error('[File] 状态更新失败', { fileId: id, error })
    message.error(err?.message || t('common.feedback.failed'))
  }
}

/**
 * 表格行内切换公开（sys_public_type：0=公开，1=私有）
 * @param record 当前行
 * @param checked 开关是否选中（公开）
 */
async function handleFilePublicChange(record: File, checked: boolean) {
  const id = getFileId(record)
  if (!id) {
    return
  }
  const newIsPublic = checked ? 0 : 1
  const oldIsPublic = getFileField(record, 'isPublic')
  ;(record as File).isPublic = newIsPublic
  try {
    await updateFilePublic({ fileId: id, isPublic: newIsPublic })
    message.success(t('common.feedback.updated'))
  } catch (error: unknown) {
    ;(record as File).isPublic = oldIsPublic
    const err = error as { message?: string }
    logger.error('[File] 公开范围更新失败', { fileId: id, error })
    message.error(err?.message || t('common.feedback.failed'))
  }
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: File[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: File, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getFileId(selectedRow.value) === getFileId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: File[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: File) => ({
  onClick: () => {
    const key = getFileId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getFileId(item)))
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
    const res = await getFileList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[File] 加载数据失败', { error })
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
  fileCode: '',
  fileName: '',
  fileOriginalName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  fileHash: '',
  fileCategory: undefined as number | undefined,
  storageType: undefined as number | undefined,
  storageConfig: '',
  accessUrl: '',
  downloadCount: undefined as number | undefined,
  lastDownloadTimeStart: '',
  lastDownloadTimeEnd: '',
  fileStatus: undefined as number | undefined,
  isPublic: undefined as number | undefined,
  fileDescription: '',
  fileTags: '',
  ipAddress: '',
  location: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增上传弹窗 */
function handleCreate() {
  uploadVisible.value = true
  nextTick(() => {
    uploadFormRef.value?.resetFields()
  })
}
/** 打开详情弹窗 */
async function handleShowDetail(record: File) {
  const id = getFileId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getFileById(id)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    detailVisible.value = false
  } finally {
    detailLoading.value = false
  }
}
/** 提交新增上传 */
async function handleUploadSubmit() {
  const refInst = uploadFormRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  uploadLoading.value = true
  try {
    await refInst.uploadFiles()
    message.success(t('common.feedback.created', { target: t('entity.file._self') }))
    uploadVisible.value = false
    uploadFormRef.value?.resetFields()
    loadData()
  } finally {
    uploadLoading.value = false
  }
}

/** 关闭新增上传弹窗 */
function handleUploadCancel() {
  uploadVisible.value = false
  uploadFormRef.value?.resetFields()
}

/** 关闭详情弹窗 */
function handleDetailClose() {
  detailVisible.value = false
  detailData.value = null
}

/**
 * 详情内下载成功后刷新详情与列表下载次数
 * @param fileId 文件ID
 */
async function handleDetailDownloaded(fileId: string) {
  if (!fileId) {
    return
  }
  try {
    detailData.value = await getFileById(fileId)
  } catch {
    // 详情已本地更新下载次数；拉取失败不阻断
  }
  await loadData()
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportFile(
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
    message.success(t('common.feedback.export.success', { target: t('entity.file._self') }))
  } catch (error: any) {
    logger.error('[File] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.file._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: File) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.file._self'), name: t('common.tip.this.target', { target: t('entity.file._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteFileById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.file._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.file._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.file._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteFileBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.file._self') }))
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
  fileCode: '',
  fileName: '',
  fileOriginalName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  fileHash: '',
  fileCategory: undefined as number | undefined,
  storageType: undefined as number | undefined,
  storageConfig: '',
  accessUrl: '',
  downloadCount: undefined as number | undefined,
  lastDownloadTimeStart: '',
  lastDownloadTimeEnd: '',
  fileStatus: undefined as number | undefined,
  isPublic: undefined as number | undefined,
  fileDescription: '',
  fileTags: '',
  ipAddress: '',
  location: '',
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
/** 分页每页条数变更（重置到默认页码） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

<style scoped lang="css">
.foundation-file {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}
.foundation-file-table-wrap {
  flex: 1;
  min-height: 0;
}
</style>