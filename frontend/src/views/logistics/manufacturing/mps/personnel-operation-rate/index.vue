<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/personnel-operation-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：人员稼动率实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:mps:personnel:operation:rate:create"
      update-permission="logistics:manufacturing:mps:personnel:operation:rate:update"
      delete-permission="logistics:manufacturing:mps:personnel:operation:rate:delete"
      import-permission="logistics:manufacturing:mps:personnel:operation:rate:import"
      export-permission="logistics:manufacturing:mps:personnel:operation:rate:export"
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
      :id-column-key="'personnelOperationRateId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPersonnelOperationRateId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'shiftNo'">
          <TaktDictTag
            :value="getPersonnelOperationRateDictValue(record, 'shiftNo')"
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
      <PersonnelOperationRateForm
        :key="formData?.personnelOperationRateId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-mps-personnel-operation-rate'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
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
      <div v-show="isFieldVisible('TeamCodeName')">
      <a-form-item :label="pi.queryLabel('TeamCodeName')">
        <a-input
          v-model:value="advancedQueryForm.TeamCodeName"
          :placeholder="pi.queryPh('TeamCodeName', 'required')"
          show-count
          :maxlength="100"
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
      <div v-show="isFieldVisible('plannedDirectPersonnelCount')">
      <a-form-item :label="pi.queryLabel('plannedDirectPersonnelCount')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDirectPersonnelCount"
          :placeholder="pi.queryPh('plannedDirectPersonnelCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualDirectPersonnelCount')">
      <a-form-item :label="pi.queryLabel('actualDirectPersonnelCount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualDirectPersonnelCount"
          :placeholder="pi.queryPh('actualDirectPersonnelCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedIndirectPersonnelCount')">
      <a-form-item :label="pi.queryLabel('plannedIndirectPersonnelCount')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedIndirectPersonnelCount"
          :placeholder="pi.queryPh('plannedIndirectPersonnelCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualIndirectPersonnelCount')">
      <a-form-item :label="pi.queryLabel('actualIndirectPersonnelCount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualIndirectPersonnelCount"
          :placeholder="pi.queryPh('actualIndirectPersonnelCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedWorkTime')">
      <a-form-item :label="pi.queryLabel('plannedWorkTime')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedWorkTime"
          :placeholder="pi.queryPh('plannedWorkTime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualWorkTime')">
      <a-form-item :label="pi.queryLabel('actualWorkTime')">
        <a-input-number
          v-model:value="advancedQueryForm.actualWorkTime"
          :placeholder="pi.queryPh('actualWorkTime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakTime')">
      <a-form-item :label="pi.queryLabel('breakTime')">
        <a-input-number
          v-model:value="advancedQueryForm.breakTime"
          :placeholder="pi.queryPh('breakTime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleTime')">
      <a-form-item :label="pi.queryLabel('idleTime')">
        <a-input-number
          v-model:value="advancedQueryForm.idleTime"
          :placeholder="pi.queryPh('idleTime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('personnelOperationRate')">
      <a-form-item :label="pi.queryLabel('personnelOperationRate')">
        <a-input-number
          v-model:value="advancedQueryForm.personnelOperationRate"
          :placeholder="pi.queryPh('personnelOperationRate', 'required')"
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
      <div v-show="isFieldVisible('workEfficiency')">
      <a-form-item :label="pi.queryLabel('workEfficiency')">
        <a-input-number
          v-model:value="advancedQueryForm.workEfficiency"
          :placeholder="pi.queryPh('workEfficiency', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleReasonType')">
      <a-form-item :label="pi.queryLabel('idleReasonType')">
        <a-input-number
          v-model:value="advancedQueryForm.idleReasonType"
          :placeholder="pi.queryPh('idleReasonType', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleReason')">
      <a-form-item :label="pi.queryLabel('idleReason')">
        <a-input
          v-model:value="advancedQueryForm.idleReason"
          :placeholder="pi.queryPh('idleReason', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeHours')">
      <a-form-item :label="pi.queryLabel('overtimeHours')">
        <a-input-number
          v-model:value="advancedQueryForm.overtimeHours"
          :placeholder="pi.queryPh('overtimeHours', 'required')"
          style="width: 100%"
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
      <div v-show="isFieldVisible('supervisor')">
      <a-form-item :label="pi.queryLabel('supervisor')">
        <TaktSelect
          v-model:value="advancedQueryForm.supervisor"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('supervisor', 'select')"
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
        :entity-i18n-key="PERSONNELOPERATIONRATE_SELF_I18N_KEY"
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
      :id-column-key="'personnelOperationRateId'"
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
 * 人员稼动率实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/personnel-operation-rate
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import PersonnelOperationRateForm from './components/PersonnelOperationRateForm.vue'
import { getPersonnelOperationRateList, getPersonnelOperationRateById, createPersonnelOperationRate, updatePersonnelOperationRate, deletePersonnelOperationRateById, deletePersonnelOperationRateBatch, getPersonnelOperationRateTemplate, importPersonnelOperationRate, exportPersonnelOperationRate, updatePersonnelOperationRateStatus } from '@/api/logistics/manufacturing/mps/personnelOperationRate.api'
import type { PersonnelOperationRate, PersonnelOperationRateQuery } from '@/types/logistics/manufacturing/mps/personnelOperationRate.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  usePersonnelOperationRateI18n,
  PERSONNELOPERATIONRATE_LIST_FIELDS,
  PERSONNELOPERATIONRATE_QUERY_STRING_FIELDS,
  PERSONNELOPERATIONRATE_QUERY_FIELDS,
  PERSONNELOPERATIONRATE_SELF_I18N_KEY,
} from './composables/usePersonnelOperationRateI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = usePersonnelOperationRateI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type PersonnelOperationRateRowRecord = PersonnelOperationRate | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPersonnelOperationRate')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PersonnelOperationRate[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<PersonnelOperationRateRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<PersonnelOperationRateRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PersonnelOperationRate> | null>(null)
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
  const form = Object.fromEntries(PERSONNELOPERATIONRATE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PERSONNELOPERATIONRATE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    timeCategory: undefined as number | undefined,
    weekNumber: undefined as number | undefined,
    monthNumber: undefined as number | undefined,
    shiftNo: undefined as number | undefined,
    plannedDirectPersonnelCount: undefined as number | undefined,
    actualDirectPersonnelCount: undefined as number | undefined,
    plannedIndirectPersonnelCount: undefined as number | undefined,
    actualIndirectPersonnelCount: undefined as number | undefined,
    plannedWorkTime: undefined as number | undefined,
    actualWorkTime: undefined as number | undefined,
    breakTime: undefined as number | undefined,
    idleTime: undefined as number | undefined,
    personnelOperationRate: undefined as number | undefined,
    plannedOutput: undefined as number | undefined,
    actualOutput: undefined as number | undefined,
    qualifiedQuantity: undefined as number | undefined,
    defectiveQuantity: undefined as number | undefined,
    yieldRate: undefined as number | undefined,
    workEfficiency: undefined as number | undefined,
    idleReasonType: undefined as number | undefined,
    overtimeHours: undefined as number | undefined,
    rateStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  PERSONNELOPERATIONRATE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'personnelOperationRateId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PersonnelOperationRateQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PersonnelOperationRateQuery>): PersonnelOperationRateQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PersonnelOperationRateQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PersonnelOperationRateQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PERSONNELOPERATIONRATE_QUERY_STRING_FIELDS) {
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
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  if (form.plannedDirectPersonnelCount !== undefined && form.plannedDirectPersonnelCount !== null) {
    query.plannedDirectPersonnelCount = form.plannedDirectPersonnelCount
  }
  if (form.actualDirectPersonnelCount !== undefined && form.actualDirectPersonnelCount !== null) {
    query.actualDirectPersonnelCount = form.actualDirectPersonnelCount
  }
  if (form.plannedIndirectPersonnelCount !== undefined && form.plannedIndirectPersonnelCount !== null) {
    query.plannedIndirectPersonnelCount = form.plannedIndirectPersonnelCount
  }
  if (form.actualIndirectPersonnelCount !== undefined && form.actualIndirectPersonnelCount !== null) {
    query.actualIndirectPersonnelCount = form.actualIndirectPersonnelCount
  }
  if (form.plannedWorkTime !== undefined && form.plannedWorkTime !== null) {
    query.plannedWorkTime = form.plannedWorkTime
  }
  if (form.actualWorkTime !== undefined && form.actualWorkTime !== null) {
    query.actualWorkTime = form.actualWorkTime
  }
  if (form.breakTime !== undefined && form.breakTime !== null) {
    query.breakTime = form.breakTime
  }
  if (form.idleTime !== undefined && form.idleTime !== null) {
    query.idleTime = form.idleTime
  }
  if (form.personnelOperationRate !== undefined && form.personnelOperationRate !== null) {
    query.personnelOperationRate = form.personnelOperationRate
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
  if (form.workEfficiency !== undefined && form.workEfficiency !== null) {
    query.workEfficiency = form.workEfficiency
  }
  if (form.idleReasonType !== undefined && form.idleReasonType !== null) {
    query.idleReasonType = form.idleReasonType
  }
  if (form.overtimeHours !== undefined && form.overtimeHours !== null) {
    query.overtimeHours = form.overtimeHours
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
function buildPersonnelOperationRateListColumn(
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
  buildPersonnelOperationRateListColumn('personnelOperationRateId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...PERSONNELOPERATIONRATE_LIST_FIELDS.map((key) => buildPersonnelOperationRateListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:mps:personnel:operation:rate:update',
        onClick: (record: PersonnelOperationRateRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:mps:personnel:operation:rate:delete',
        onClick: (record: PersonnelOperationRateRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPersonnelOperationRateId = (record: PersonnelOperationRateRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getPersonnelOperationRateDictValue = (
  record: PersonnelOperationRateRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PersonnelOperationRateRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PersonnelOperationRateRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getPersonnelOperationRateId(selecmedRow.value) === getPersonnelOperationRateId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: PersonnelOperationRateRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PersonnelOperationRateRowRecord) => ({
  onClick: () => {
    const key = getPersonnelOperationRateId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getPersonnelOperationRateId(item)))
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
    const res = await getPersonnelOperationRateList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PersonnelOperationRate] 加载数据失败', { error })
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
async function handleEdit(record: PersonnelOperationRateRowRecord) {
  const id = getPersonnelOperationRateId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getPersonnelOperationRateById(id)
    formData.value = detail ?? ({ ...record } as Partial<PersonnelOperationRate>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
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
      await updatePersonnelOperationRate(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createPersonnelOperationRate(payload as any)
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
  const res = await getPersonnelOperationRateTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importPersonnelOperationRate(file, sheetName)
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
    const exportMeta = await exportPersonnelOperationRate(
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
    logger.error('[PersonnelOperationRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PersonnelOperationRateRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePersonnelOperationRateById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePersonnelOperationRateBatch(ids)
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
