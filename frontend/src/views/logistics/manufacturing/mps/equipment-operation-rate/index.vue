<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/equipment-operation-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：机器稼动率实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:mps:equipment:operation:rate:create"
      update-permission="logistics:manufacturing:mps:equipment:operation:rate:update"
      delete-permission="logistics:manufacturing:mps:equipment:operation:rate:delete"
      import-permission="logistics:manufacturing:mps:equipment:operation:rate:import"
      export-permission="logistics:manufacturing:mps:equipment:operation:rate:export"
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
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'equipmentOperationRateId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEquipmentOperationRateId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'equipmentType'">
          <TaktDictTag
            :value="getEquipmentOperationRateDictValue(record, 'equipmentType')"
            dict-type="logistics_maintenance_equipment_type"
          />
        </template>
        <template v-else-if="column.key === 'shiftNo'">
          <TaktDictTag
            :value="getEquipmentOperationRateDictValue(record, 'shiftNo')"
            dict-type="logistics_manufacturing_shift_category"
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
      <EquipmentOperationRateForm
        :key="formData?.equipmentOperationRateId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-mps-equipment-operation-rate'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
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
      <div v-show="isFieldVisible('timeCategory')">
      <a-form-item :label="pi.queryLabel('timeCategory')">
        <a-input-number
          v-model:value="advancedQueryForm.timeCategory"
          :placeholder="pi.queryPh('timeCategory', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="pi.queryLabel('startDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="pi.queryPh('startDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="pi.queryLabel('startDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="pi.queryPh('startDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateStart')">
      <a-form-item :label="pi.queryLabel('endDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateStart"
          :placeholder="pi.queryPh('endDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateEnd')">
      <a-form-item :label="pi.queryLabel('endDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateEnd"
          :placeholder="pi.queryPh('endDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weekNumber')">
      <a-form-item :label="pi.queryLabel('weekNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.weekNumber"
          :placeholder="pi.queryPh('weekNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('monthNumber')">
      <a-form-item :label="pi.queryLabel('monthNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.monthNumber"
          :placeholder="pi.queryPh('monthNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipCode')">
      <a-form-item :label="pi.queryLabel('EquipCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.EquipCode"
          api-url="TaktProductionEquipments/options"
          :placeholder="pi.queryPh('EquipCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="pi.queryLabel('equipmentName')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="pi.queryPh('equipmentName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentType')">
      <a-form-item :label="pi.queryLabel('equipmentType')">
        <TaktSelect
          v-model:value="advancedQueryForm.equipmentType"
          dict-type="logistics_maintenance_equipment_type"
          :placeholder="pi.queryPh('equipmentType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('TeamCode')">
      <a-form-item :label="pi.queryLabel('TeamCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.TeamCode"
          api-url="TaktProductionTeams/options"
          :placeholder="pi.queryPh('TeamCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="pi.queryLabel('shiftNo')">
        <TaktSelect
          v-model:value="advancedQueryForm.shiftNo"
          dict-type="logistics_manufacturing_shift_category"
          :placeholder="pi.queryPh('shiftNo', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedRuntime')">
      <a-form-item :label="pi.queryLabel('plannedRuntime')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedRuntime"
          :placeholder="pi.queryPh('plannedRuntime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualRuntime')">
      <a-form-item :label="pi.queryLabel('actualRuntime')">
        <a-input-number
          v-model:value="advancedQueryForm.actualRuntime"
          :placeholder="pi.queryPh('actualRuntime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtime')">
      <a-form-item :label="pi.queryLabel('downtime')">
        <a-input-number
          v-model:value="advancedQueryForm.downtime"
          :placeholder="pi.queryPh('downtime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentOperationRate')">
      <a-form-item :label="pi.queryLabel('equipmentOperationRate')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentOperationRate"
          :placeholder="pi.queryPh('equipmentOperationRate', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedOutput')">
      <a-form-item :label="pi.queryLabel('plannedOutput')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedOutput"
          :placeholder="pi.queryPh('plannedOutput', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualOutput')">
      <a-form-item :label="pi.queryLabel('actualOutput')">
        <a-input-number
          v-model:value="advancedQueryForm.actualOutput"
          :placeholder="pi.queryPh('actualOutput', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="pi.queryLabel('qualifiedQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="pi.queryPh('qualifiedQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectiveQuantity')">
      <a-form-item :label="pi.queryLabel('defectiveQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectiveQuantity"
          :placeholder="pi.queryPh('defectiveQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('yieldRate')">
      <a-form-item :label="pi.queryLabel('yieldRate')">
        <a-input-number
          v-model:value="advancedQueryForm.yieldRate"
          :placeholder="pi.queryPh('yieldRate', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReasonType')">
      <a-form-item :label="pi.queryLabel('downtimeReasonType')">
        <a-input-number
          v-model:value="advancedQueryForm.downtimeReasonType"
          :placeholder="pi.queryPh('downtimeReasonType', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReason')">
      <a-form-item :label="pi.queryLabel('downtimeReason')">
        <a-input
          v-model:value="advancedQueryForm.downtimeReason"
          :placeholder="pi.queryPh('downtimeReason', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentOperator')">
      <a-form-item :label="pi.queryLabel('equipmentOperator')">
        <TaktSelect
          v-model:value="advancedQueryForm.equipmentOperator"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('equipmentOperator', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentMaintainer')">
      <a-form-item :label="pi.queryLabel('equipmentMaintainer')">
        <TaktSelect
          v-model:value="advancedQueryForm.equipmentMaintainer"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('equipmentMaintainer', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teamLeader')">
      <a-form-item :label="pi.queryLabel('teamLeader')">
        <TaktSelect
          v-model:value="advancedQueryForm.teamLeader"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('teamLeader', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rateStatus')">
      <a-form-item :label="pi.queryLabel('rateStatus')">
        <a-input-number
          v-model:value="advancedQueryForm.rateStatus"
          :placeholder="pi.queryPh('rateStatus', 'required')"
          style="width: 100%"
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
        :entity-i18n-key="EQUIPMENTOPERATIONRATE_SELF_I18N_KEY"
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
      :id-column-key="'equipmentOperationRateId'"
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
 * 机器稼动率实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/equipment-operation-rate
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import EquipmentOperationRateForm from './components/EquipmentOperationRateForm.vue'
import { getEquipmentOperationRateList, getEquipmentOperationRateById, createEquipmentOperationRate, updateEquipmentOperationRate, deleteEquipmentOperationRateById, deleteEquipmentOperationRateBatch, getEquipmentOperationRateTemplate, importEquipmentOperationRate, exportEquipmentOperationRate, updateEquipmentOperationRateStatus } from '@/api/logistics/manufacturing/mps/equipmentOperationRate.api'
import type { EquipmentOperationRate, EquipmentOperationRateQuery } from '@/types/logistics/manufacturing/mps/equipmentOperationRate.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useEquipmentOperationRateI18n,
  EQUIPMENTOPERATIONRATE_LIST_FIELDS,
  EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS,
  EQUIPMENTOPERATIONRATE_QUERY_FIELDS,
  EQUIPMENTOPERATIONRATE_SELF_I18N_KEY,
} from './composables/useEquipmentOperationRateI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useEquipmentOperationRateI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type EquipmentOperationRateRowRecord = EquipmentOperationRate | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEquipmentOperationRate')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EquipmentOperationRate[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EquipmentOperationRateRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<EquipmentOperationRateRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EquipmentOperationRate> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    timeCategory: undefined as number | undefined,
    weekNumber: undefined as number | undefined,
    monthNumber: undefined as number | undefined,
    equipmentType: undefined as number | undefined,
    shiftNo: undefined as number | undefined,
    plannedRuntime: undefined as number | undefined,
    actualRuntime: undefined as number | undefined,
    downtime: undefined as number | undefined,
    equipmentOperationRate: undefined as number | undefined,
    plannedOutput: undefined as number | undefined,
    actualOutput: undefined as number | undefined,
    qualifiedQuantity: undefined as number | undefined,
    defectiveQuantity: undefined as number | undefined,
    yieldRate: undefined as number | undefined,
    downtimeReasonType: undefined as number | undefined,
    rateStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  EQUIPMENTOPERATIONRATE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'equipmentOperationRateId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EquipmentOperationRateQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EquipmentOperationRateQuery>): EquipmentOperationRateQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EquipmentOperationRateQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EquipmentOperationRateQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.timeCategory !== undefined && form.timeCategory !== null) {
    query.timeCategory = form.timeCategory
  }
  if (form.weekNumber !== undefined && form.weekNumber !== null) {
    query.weekNumber = form.weekNumber
  }
  if (form.monthNumber !== undefined && form.monthNumber !== null) {
    query.monthNumber = form.monthNumber
  }
  if (form.equipmentType !== undefined && form.equipmentType !== null) {
    query.equipmentType = form.equipmentType
  }
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  if (form.plannedRuntime !== undefined && form.plannedRuntime !== null) {
    query.plannedRuntime = form.plannedRuntime
  }
  if (form.actualRuntime !== undefined && form.actualRuntime !== null) {
    query.actualRuntime = form.actualRuntime
  }
  if (form.downtime !== undefined && form.downtime !== null) {
    query.downtime = form.downtime
  }
  if (form.equipmentOperationRate !== undefined && form.equipmentOperationRate !== null) {
    query.equipmentOperationRate = form.equipmentOperationRate
  }
  if (form.plannedOutput !== undefined && form.plannedOutput !== null) {
    query.plannedOutput = form.plannedOutput
  }
  if (form.actualOutput !== undefined && form.actualOutput !== null) {
    query.actualOutput = form.actualOutput
  }
  if (form.qualifiedQuantity !== undefined && form.qualifiedQuantity !== null) {
    query.qualifiedQuantity = form.qualifiedQuantity
  }
  if (form.defectiveQuantity !== undefined && form.defectiveQuantity !== null) {
    query.defectiveQuantity = form.defectiveQuantity
  }
  if (form.yieldRate !== undefined && form.yieldRate !== null) {
    query.yieldRate = form.yieldRate
  }
  if (form.downtimeReasonType !== undefined && form.downtimeReasonType !== null) {
    query.downtimeReasonType = form.downtimeReasonType
  }
  if (form.rateStatus !== undefined && form.rateStatus !== null) {
    query.rateStatus = form.rateStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function buildEquipmentOperationRateListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  buildEquipmentOperationRateListColumn('equipmentOperationRateId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...EQUIPMENTOPERATIONRATE_LIST_FIELDS.map((key) => buildEquipmentOperationRateListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:mps:equipment:operation:rate:update',
        onClick: (record: EquipmentOperationRateRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:mps:equipment:operation:rate:delete',
        onClick: (record: EquipmentOperationRateRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEquipmentOperationRateId = (record: EquipmentOperationRateRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getEquipmentOperationRateDictValue = (
  record: EquipmentOperationRateRowRecord,
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
  onChange: (keys: (string | number)[], rows: EquipmentOperationRateRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EquipmentOperationRateRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getEquipmentOperationRateId(selectedRow.value) === getEquipmentOperationRateId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EquipmentOperationRateRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EquipmentOperationRateRowRecord) => ({
  onClick: () => {
    const key = getEquipmentOperationRateId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEquipmentOperationRateId(item)))
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
    const res = await getEquipmentOperationRateList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EquipmentOperationRate] 加载数据失败', { error })
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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
/** 打开编辑弹窗（拉取详情，避免列表列裁剪字段） */
async function handleEdit(record: EquipmentOperationRateRowRecord) {
  const id = getEquipmentOperationRateId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getEquipmentOperationRateById(id)
    formData.value = detail ?? ({ ...record } as Partial<EquipmentOperationRate>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
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
      await updateEquipmentOperationRate(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createEquipmentOperationRate(payload as any)
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
  const res = await getEquipmentOperationRateTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importEquipmentOperationRate(file, sheetName)
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
    const exportMeta = await exportEquipmentOperationRate(
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
    logger.error('[EquipmentOperationRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EquipmentOperationRateRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEquipmentOperationRateById((record as any)[entityIdName])
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
      await deleteEquipmentOperationRateBatch(ids)
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
