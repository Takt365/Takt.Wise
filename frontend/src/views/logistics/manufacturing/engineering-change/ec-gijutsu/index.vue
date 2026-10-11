<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getEcId"
      :master-row-selection="rowSelection"
      master-id-column-key="ecGijutsuId"
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
          source-permission="logistics:manufacturing:engineering:change:gijutsu:create"
          update-permission="logistics:manufacturing:engineering:change:gijutsu:update"
          export-permission="logistics:manufacturing:engineering:change:gijutsu:export"
          :show-create="false"
          :show-source="true"
          :show-update="true"
          :show-delete="false"
          :show-import="false"
          :show-export="true"
          :show-expand="false"
          :show-advanced-query="true"
          :show-column-setting="true"
          :show-fullscreen="true"
          :show-refresh="true"
          :update-disabled="updateDisabled"
          :update-loading="loading"
          :refresh-loading="loading"
          @source="handleOpenSourceEcInput"
          @update="handleUpdate"
          @export="handleExport"
          @advanced-query="handleAdvancedQuery"
          @column-setting="handleColumnSetting"
          @refresh="handleRefresh"
        />
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'changeStatus'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_status" :value="getEcField(record, 'changeStatus')" />
        </template>
        <template v-else-if="column.key === 'ecScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_scope_category" :value="getEcField(record, 'ecScope')" />
        </template>
        <template v-else-if="column.key === 'ecDeptScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_dept_scope_category" :value="getEcField(record, 'ecDeptScope')" />
        </template>
        <template v-else-if="column.key === 'ecStatus'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_gijutsu_status" :value="getEcField(record, 'ecStatus')" />
        </template>
      </template>
      <template #detail>
        <EcSubPanels
          ref="ecSubPanelsRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="ecGijutsuModalWidthPx"
      wrap-class-name="takt-form-modal-resizable ec-gijutsu-modal-no-body-scroll"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <EcForm
        :key="formData?.ecGijutsuId ?? (sourceImportMode ? 'source-import' : 'create')"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
        :source-import-mode="sourceImportMode"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-ec-gijutsu'"
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
      <div v-show="isFieldVisible('ecCode')">
      <a-form-item :label="pi.queryLabel('ecCode')">
        <a-input
          v-model:value="advancedQueryForm.ecCode"
          :placeholder="pi.queryPh('ecCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecIssueDateStart')">
      <a-form-item :label="pi.queryLabel('ecIssueDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecIssueDateStart"
          :placeholder="pi.queryPh('ecIssueDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecIssueDateEnd')">
      <a-form-item :label="pi.queryLabel('ecIssueDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecIssueDateEnd"
          :placeholder="pi.queryPh('ecIssueDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeStatus')">
      <a-form-item :label="pi.queryLabel('changeStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.changeStatus"
          dict-type="logistics_manufacturing_ec_status"
          :placeholder="pi.queryPh('changeStatus', 'select')"
          allow-clear
          class="w-full"
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
      <div v-show="isFieldVisible('ecContent')">
      <a-form-item :label="pi.queryLabel('ecContent')">
        <a-input
          v-model:value="advancedQueryForm.ecContent"
          :placeholder="pi.queryPh('ecContent', 'optional')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecLeader')">
      <a-form-item :label="pi.queryLabel('ecLeader')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecLeader"
          api-url="TaktEcGroups/options"
          :api-params="{ ecGroupCategory: 'D0710' }"
          :placeholder="pi.queryPh('ecLeader', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecLossAmount')">
      <a-form-item :label="pi.queryLabel('ecLossAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.ecLossAmount"
          :placeholder="pi.queryPh('ecLossAmount', 'select')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecScope')">
      <a-form-item :label="pi.queryLabel('ecScope')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecScope"
          dict-type="logistics_manufacturing_ec_scope_category"
          :placeholder="pi.queryPh('ecScope', 'select')"
          allow-clear
          class="w-full"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecDeptScope')">
      <a-form-item :label="pi.queryLabel('ecDeptScope')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecDeptScope"
          dict-type="logistics_manufacturing_ec_dept_scope_category"
          :placeholder="pi.queryPh('ecDeptScope', 'select')"
          allow-clear
          class="w-full"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateStart')">
      <a-form-item :label="pi.queryLabel('ecEntryDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateStart"
          :placeholder="pi.queryPh('ecEntryDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateEnd')">
      <a-form-item :label="pi.queryLabel('ecEntryDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateEnd"
          :placeholder="pi.queryPh('ecEntryDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecStatus')">
      <a-form-item :label="pi.queryLabel('ecStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecStatus"
          dict-type="logistics_manufacturing_ec_gijutsu_status"
          :placeholder="pi.queryPh('ecStatus', 'select')"
          allow-clear
          class="w-full"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 来源设变录入 -->
    <TaktModal
      v-model:open="sourceEcInputVisible"
      :title="t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.title')"
      :width="ecGijutsuModalWidthPx"
      wrap-class-name="takt-form-modal-resizable ec-gijutsu-modal-no-body-scroll"
      :hide-footer="true"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleSourceEcInputCancel"
    >
      <SourceEcInput
        v-if="sourceEcInputVisible"
        ref="sourceEcInputRef"
        @draft-ready="handleSourceEcDraftReady"
      />
    </TaktModal>
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
        :entity-i18n-key="ECGIJUTSU_SELF_I18N_KEY"
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
      :id-column-key="'ecGijutsuId'"
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
 * 设变管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import EcForm from './components/EcForm.vue'
import EcSubPanels from './components/EcSubPanels.vue'
import SourceEcInput from './components/SourceEcInput.vue'
import { provideEcMasterContext } from './composables/useEcMasterContext'
import {
  useEcGijutsuI18n,
  ECGIJUTSU_QUERY_STRING_FIELDS,
  ECGIJUTSU_QUERY_FIELDS,
  ECGIJUTSU_SELF_I18N_KEY,
} from './composables/useEcGijutsuI18n'
import { getEcGijutsuList, getEcGijutsuById, createEcGijutsu, updateEcGijutsu, getEcGijutsuTemplate, importEcGijutsu, exportEcGijutsu, updateEcGijutsuStatus } from '@/api/logistics/manufacturing/engineering-change/ecGijutsu.api'
import { getEcGroupOptions } from '@/api/logistics/manufacturing/engineering-change/ecGroup.api'
import type { EcGijutsu, EcGijutsuFormData, EcGijutsuQuery } from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import {
  EC_GIJUTSU_TABLE_NAME,
  useEcGijutsuPersistSignalR,
} from '@/composables/useEcGijutsuPersistSignalr'
import { RiEditLine, RiQuestionLine } from '@remixicon/vue'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useEcGijutsuI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcGijutsu')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EcGijutsu[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EcGijutsu | null>(null)
/** 表格多选行 */
const selectedRows = ref<EcGijutsu[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<EcGijutsuFormData | null>(null)
/** 来源设变导入草稿模式（明细只读，附件上传后 create 落库） */
const sourceImportMode = ref(false)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 来源设变导入 / 来源设变录入弹窗宽度：（视口 − 左侧菜单）× 80% */
const ecGijutsuModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单（字段与 TaktEcGijutsuQueryDto 对齐）
 * @returns 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(ECGIJUTSU_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ECGIJUTSU_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    changeStatus: undefined as number | undefined,
    ecLossAmount: undefined as number | undefined,
    ecScope: undefined as number | undefined,
    ecDeptScope: undefined as number | undefined,
    ecStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ECGIJUTSU_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 来源设变录入弹窗是否打开 */
const sourceEcInputVisible = ref(false)
/** 来源设变录入组件 ref */
const sourceEcInputRef = ref()
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'ecGijutsuId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideEcMasterContext()
const ecSubPanelsRef = ref<InstanceType<typeof EcSubPanels> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcGijutsuQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcGijutsuQuery>): EcGijutsuQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcGijutsuQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcGijutsuQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of ECGIJUTSU_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.changeStatus !== undefined && form.changeStatus !== null) {
    query.changeStatus = form.changeStatus
  }
  if (form.ecLossAmount !== undefined && form.ecLossAmount !== null) {
    query.ecLossAmount = form.ecLossAmount
  }
  if (form.ecScope !== undefined && form.ecScope !== null) {
    query.ecScope = form.ecScope
  }
  if (form.ecDeptScope !== undefined && form.ecDeptScope !== null) {
    query.ecDeptScope = form.ecDeptScope
  }
  if (form.ecStatus !== undefined && form.ecStatus !== null) {
    query.ecStatus = form.ecStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void loadEcGroupLabelMap()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: EcGijutsu | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getEcId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as EcGijutsu
  const key = getEcId(row)
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
async function loadEcDetail(record: EcGijutsu): Promise<EcGijutsu | null> {
  const id = getEcId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getEcGijutsuById(id)
    const index = dataSource.value.findIndex((row) => getEcId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as EcGijutsu
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化；dataIndex 与 TaktEcGijutsu / TaktEcGijutsuDto 对齐） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecGijutsuId',
    key: 'ecGijutsuId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecGijutsuId') ?? ''
  },
  {
    title: pi.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'plantCode') ?? ''
  },
  {
    title: pi.label('ecCode'),
    dataIndex: 'ecCode',
    key: 'ecCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    sorter: (a: EcGijutsu, b: EcGijutsu) =>
      String(getEcField(a, 'ecCode') ?? '').localeCompare(String(getEcField(b, 'ecCode') ?? '')),
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecCode') ?? ''
  },
  {
    title: pi.label('ecIssueDate'),
    dataIndex: 'ecIssueDate',
    key: 'ecIssueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    sorter: (a: EcGijutsu, b: EcGijutsu) =>
      new Date(String(getEcField(a, 'ecIssueDate') ?? 0)).getTime()
      - new Date(String(getEcField(b, 'ecIssueDate') ?? 0)).getTime(),
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecIssueDate') ?? ''
  },
  {
    title: pi.label('changeStatus'),
    dataIndex: 'changeStatus',
    key: 'changeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('ecTitle'),
    dataIndex: 'ecTitle',
    key: 'ecTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecTitle') ?? ''
  },
  {
    title: pi.label('ecContent'),
    dataIndex: 'ecContent',
    key: 'ecContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecContent') ?? ''
  },
  {
    title: pi.label('ecLeader'),
    dataIndex: 'ecLeader',
    key: 'ecLeader',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => formatEcLeader(record)
  },
  {
    title: pi.label('ecLossAmount'),
    dataIndex: 'ecLossAmount',
    key: 'ecLossAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecLossAmount') ?? ''
  },
  {
    title: pi.label('ecScope'),
    dataIndex: 'ecScope',
    key: 'ecScope',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('ecDeptScope'),
    dataIndex: 'ecDeptScope',
    key: 'ecDeptScope',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('ecEntryDate'),
    dataIndex: 'ecEntryDate',
    key: 'ecEntryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecEntryDate') ?? ''
  },
  {
    title: pi.label('ecStatus'),
    dataIndex: 'ecStatus',
    key: 'ecStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    width: 80,
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:update',
        onClick: (record: EcGijutsu) => handleEdit(record)
      },
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEcId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEcField = (record: any, field: string): any => record?.[field]

/** 设变组编码 → 名称（EcLeader 列表展示） */
const ecGroupLabelByCode = ref<Record<string, string>>({})

/** 预加载设变组选项，供设变担当列编码转名称 */
async function loadEcGroupLabelMap() {
  try {
    const list = await getEcGroupOptions()
    const map: Record<string, string> = {}
    for (const item of list ?? []) {
      if (item.dictValue != null && String(item.dictValue) !== '') {
        map[String(item.dictValue)] = String(item.dictLabel ?? item.dictValue)
      }
    }
    ecGroupLabelByCode.value = map
  } catch {
    ecGroupLabelByCode.value = {}
  }
}

/**
 * 格式化设变担当列：设变组编码显示设变组名称，无映射时回退编码
 * @param record 行数据
 * @returns 展示文案
 */
function formatEcLeader(record: any): string {
  const raw = getEcField(record, 'ecLeader')
  if (raw == null || raw === '') return ''
  const key = String(raw)
  return ecGroupLabelByCode.value[key] ?? key
}


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcGijutsu[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: EcGijutsu, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getEcId(selectedRow.value) === getEcId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcGijutsu[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getEcGijutsuList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EcGijutsu] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData, EC_GIJUTSU_TABLE_NAME)

/** 后台保存完成后刷新子面板（列表由 table:refresh 处理） */
useEcGijutsuPersistSignalR(() => {
  if (selectedMasterKey.value) {
    ecSubPanelsRef.value?.reload?.()
  }
})

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  sourceImportMode.value = false
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主表 + 附件/通知；明细由右侧面板分页查询） */
async function handleEdit(record: EcGijutsu) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  sourceImportMode.value = false
  formLoading.value = true
  try {
    const detail = await loadEcDetail(record)
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
/** 提交新增/编辑表单（后台落库；立即关窗，完成后 SignalR 刷新） */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch (err: unknown) {
    const msg = err instanceof Error ? err.message : undefined
    if (msg) {
      message.warning(msg)
    }
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    const submitted = id
      ? await updateEcGijutsu(id, payload as any)
      : await createEcGijutsu(payload as any)
    message.info(
      t('logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.submitted', {
        ecCode: submitted?.ecCode || (payload as any)?.ecCode || '',
        action: t(
          id
            ? 'logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.actionUpdate'
            : 'logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.actionCreate',
        ),
        detailCount:
          submitted?.detailCount
          ?? (payload as any)?.deferredDetailCount
          ?? (payload as any)?.ecDetails?.length
          ?? 0,
      }),
    )
    formVisible.value = false
    formData.value = null
    sourceImportMode.value = false
    nextTick(() => formRef.value?.resetFields())
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  sourceImportMode.value = false
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 打开来源设变录入弹窗 */
function handleOpenSourceEcInput() {
  sourceEcInputVisible.value = true
}

/** 关闭来源设变录入弹窗 */
function handleSourceEcInputCancel() {
  sourceEcInputVisible.value = false
  nextTick(() => sourceEcInputRef.value?.resetFields?.())
}

/**
 * 来源设变草稿就绪：关闭来源弹窗，打开 ec-form 补全后保存
 * @param draft 创建草稿 DTO
 */
function handleSourceEcDraftReady(draft: EcGijutsuFormData) {
  sourceEcInputVisible.value = false
  nextTick(() => sourceEcInputRef.value?.resetFields?.())
  sourceImportMode.value = true
  formTitle.value = t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.formTitle')
  formData.value = { ...draft }
  formVisible.value = true
}

/** 打开导入对话框 */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEcGijutsuTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcGijutsu(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportEcGijutsu(
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
    logger.error('[EcGijutsu] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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

<style>
/* 设变维护 / 来源设变录入弹窗：body 不出现滚动条，仅内嵌表格滚动 */
.ec-gijutsu-modal-no-body-scroll.takt-modal.takt-modal-viewport-size .ant-modal-body,
.ec-gijutsu-modal-no-body-scroll.takt-modal.takt-modal-is-fs .ant-modal-body {
  display: flex;
  flex-direction: column;
  min-width: 0;
  overflow: hidden !important;
}

.ec-gijutsu-modal-no-body-scroll.takt-modal.takt-modal-viewport-size .ant-modal-body > *,
.ec-gijutsu-modal-no-body-scroll.takt-modal.takt-modal-is-fs .ant-modal-body > * {
  flex: 1 1 auto;
  min-height: 0;
  min-width: 0;
  height: 100%;
}
</style>
