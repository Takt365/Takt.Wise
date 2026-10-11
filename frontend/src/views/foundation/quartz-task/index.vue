<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Quartz 定时任务实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="foundation:quartz:task:create"
      update-permission="foundation:quartz:task:update"
      delete-permission="foundation:quartz:task:delete"
      import-permission="foundation:quartz:task:import"
      export-permission="foundation:quartz:task:export"
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
      :id-column-key="'quartzTaskId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getQuartzTaskId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'jobGroup'">
          <TaktDictTag
            :value="getQuartzTaskDictValue(record, 'jobGroup')"
            dict-type="sys_quartz_job_group"
          />
        </template>
        <template v-else-if="column.key === 'taskType'">
          <TaktDictTag
            :value="getQuartzTaskDictValue(record, 'taskType')"
            dict-type="sys_quartz_task_type"
          />
        </template>
        <template v-else-if="column.key === 'triggerType'">
          <TaktDictTag
            :value="getQuartzTaskDictValue(record, 'triggerType')"
            dict-type="sys_quartz_trigger_type"
          />
        </template>
        <template v-else-if="column.key === 'concurrent'">
          <TaktDictTag
            :value="getQuartzTaskDictValue(record, 'concurrent')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'misfirePolicy'">
          <TaktDictTag
            :value="getQuartzTaskDictValue(record, 'misfirePolicy')"
            dict-type="sys_quartz_misfire_policy"
          />
        </template>
        <template v-else-if="column.key === 'taskStatus'">
          <TaktDictTag
            :value="getQuartzTaskDictValue(record, 'taskStatus')"
            dict-type="sys_quartz_task_status"
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
      <QuartzTaskForm
        :key="formData?.quartzTaskId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <!-- 立即执行：核算月份 + 目标库（BOM / PCB SECT） -->
    <TaktModal
      v-model:open="executeMonthVisible"
      :title="t('foundation.quartz-task.page.executeMonth.modalTitle')"
      :use-viewport-size="false"
      width="25vw"
      :confirm-loading="executeSubmitting"
      @ok="handleExecuteMonthSubmit"
      @cancel="handleExecuteMonthCancel"
    >
      <a-form class="takt-generated-form" layout="horizontal" label-align="right">
        <a-form-item :label="t('foundation.quartz-task.page.executeDb.targetDatabase')" required>
          <a-select
            v-model:value="executeTargetDatabase"
            :options="databaseSelectOptions"
            :loading="databaseInfoLoading"
            allow-clear
            show-search
            option-filter-prop="label"
            :placeholder="t('foundation.quartz-task.page.executeDb.targetDatabase')"
          />
        </a-form-item>
        <a-form-item :label="t('foundation.quartz-task.page.executeMonth.costingMonth')" required>
          <a-date-picker
            v-model:value="executeCostingMonth"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="w-full"
            :placeholder="t('foundation.quartz-task.page.executeMonth.costingMonthPlaceholder')"
          />
        </a-form-item>
        <a-typography-text type="secondary">
          {{ t('foundation.quartz-task.page.executeMonth.hint') }}
        </a-typography-text>
      </a-form>
    </TaktModal>

    <!-- 立即执行：同步库选择 -->
    <TaktModal
      v-model:open="executeDbVisible"
      :title="t('foundation.quartz-task.page.executeDb.modalTitle')"
      :use-viewport-size="false"
      width="25vw"
      :confirm-loading="executeSubmitting"
      @ok="handleExecuteDbSubmit"
      @cancel="handleExecuteDbCancel"
    >
      <a-form class="takt-generated-form" layout="horizontal" label-align="right">
        <a-form-item
          v-if="executeDbNeedsSource"
          :label="t('foundation.quartz-task.page.executeDb.sourceDatabase')"
          required
        >
          <a-select
            v-model:value="executeSourceDatabase"
            :options="databaseSelectOptions"
            :loading="databaseInfoLoading"
            allow-clear
            show-search
            option-filter-prop="label"
            :placeholder="t('foundation.quartz-task.page.executeDb.sourceDatabase')"
          />
        </a-form-item>
        <a-form-item :label="t('foundation.quartz-task.page.executeDb.targetDatabase')" required>
          <a-select
            v-model:value="executeTargetDatabase"
            :options="databaseSelectOptions"
            :loading="databaseInfoLoading"
            allow-clear
            show-search
            option-filter-prop="label"
            :placeholder="t('foundation.quartz-task.page.executeDb.targetDatabase')"
          />
        </a-form-item>
        <a-typography-text type="secondary">
          {{
            executeDbNeedsSource
              ? t('foundation.quartz-task.page.executeDb.hintSourceTarget')
              : t('foundation.quartz-task.page.executeDb.hintTargetOnly')
          }}
        </a-typography-text>
      </a-form>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-foundation-quartz-task'"
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
      <div v-show="isFieldVisible('taskCode')">
      <a-form-item :label="pi.queryLabel('taskCode')">
        <a-input
          v-model:value="advancedQueryForm.taskCode"
          :placeholder="pi.queryPh('taskCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskName')">
      <a-form-item :label="pi.queryLabel('taskName')">
        <a-input
          v-model:value="advancedQueryForm.taskName"
          :placeholder="pi.queryPh('taskName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobName')">
      <a-form-item :label="pi.queryLabel('jobName')">
        <a-input
          v-model:value="advancedQueryForm.jobName"
          :placeholder="pi.queryPh('jobName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGroup')">
      <a-form-item :label="pi.queryLabel('jobGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.jobGroup"
          dict-type="sys_quartz_job_group"
          :placeholder="pi.queryPh('jobGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskType')">
      <a-form-item :label="pi.queryLabel('taskType')">
        <TaktSelect
          v-model:value="advancedQueryForm.taskType"
          dict-type="sys_quartz_task_type"
          :placeholder="pi.queryPh('taskType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assemblyName')">
      <a-form-item :label="pi.queryLabel('assemblyName')">
        <a-input
          v-model:value="advancedQueryForm.assemblyName"
          :placeholder="pi.queryPh('assemblyName', 'required')"
          show-count
          :maxlength="255"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('className')">
      <a-form-item :label="pi.queryLabel('className')">
        <a-input
          v-model:value="advancedQueryForm.className"
          :placeholder="pi.queryPh('className', 'required')"
          show-count
          :maxlength="255"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('apiUrl')">
      <a-form-item :label="pi.queryLabel('apiUrl')">
        <a-input
          v-model:value="advancedQueryForm.apiUrl"
          :placeholder="pi.queryPh('apiUrl', 'required')"
          show-count
          :maxlength="255"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestMethod')">
      <a-form-item :label="pi.queryLabel('requestMethod')">
        <a-input
          v-model:value="advancedQueryForm.requestMethod"
          :placeholder="pi.queryPh('requestMethod', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sqlScript')">
      <a-form-item :label="pi.queryLabel('sqlScript')">
        <a-input
          v-model:value="advancedQueryForm.sqlScript"
          :placeholder="pi.queryPh('sqlScript', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('triggerType')">
      <a-form-item :label="pi.queryLabel('triggerType')">
        <TaktSelect
          v-model:value="advancedQueryForm.triggerType"
          dict-type="sys_quartz_trigger_type"
          :placeholder="pi.queryPh('triggerType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cronExpression')">
      <a-form-item :label="pi.queryLabel('cronExpression')">
        <a-input
          v-model:value="advancedQueryForm.cronExpression"
          :placeholder="pi.queryPh('cronExpression', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('intervalSeconds')">
      <a-form-item :label="pi.queryLabel('intervalSeconds')">
        <a-input-number
          v-model:value="advancedQueryForm.intervalSeconds"
          :placeholder="pi.queryPh('intervalSeconds', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeParams')">
      <a-form-item :label="pi.queryLabel('executeParams')">
        <a-input
          v-model:value="advancedQueryForm.executeParams"
          :placeholder="pi.queryPh('executeParams', 'required')"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('concurrent')">
      <a-form-item :label="pi.queryLabel('concurrent')">
        <TaktSelect
          v-model:value="advancedQueryForm.concurrent"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('concurrent', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('misfirePolicy')">
      <a-form-item :label="pi.queryLabel('misfirePolicy')">
        <TaktSelect
          v-model:value="advancedQueryForm.misfirePolicy"
          dict-type="sys_quartz_misfire_policy"
          :placeholder="pi.queryPh('misfirePolicy', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstRunAtStart')">
      <a-form-item :label="pi.queryLabel('firstRunAtStart')">
        <a-input
          v-model:value="advancedQueryForm.firstRunAtStart"
          :placeholder="pi.queryPh('firstRunAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstRunAtEnd')">
      <a-form-item :label="pi.queryLabel('firstRunAtEnd')">
        <a-input
          v-model:value="advancedQueryForm.firstRunAtEnd"
          :placeholder="pi.queryPh('firstRunAtEnd', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeCount')">
      <a-form-item :label="pi.queryLabel('executeCount')">
        <a-input-number
          v-model:value="advancedQueryForm.executeCount"
          :placeholder="pi.queryPh('executeCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastRunAtStart')">
      <a-form-item :label="pi.queryLabel('lastRunAtStart')">
        <a-input
          v-model:value="advancedQueryForm.lastRunAtStart"
          :placeholder="pi.queryPh('lastRunAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastRunAtEnd')">
      <a-form-item :label="pi.queryLabel('lastRunAtEnd')">
        <a-input
          v-model:value="advancedQueryForm.lastRunAtEnd"
          :placeholder="pi.queryPh('lastRunAtEnd', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextRunAtStart')">
      <a-form-item :label="pi.queryLabel('nextRunAtStart')">
        <a-input
          v-model:value="advancedQueryForm.nextRunAtStart"
          :placeholder="pi.queryPh('nextRunAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextRunAtEnd')">
      <a-form-item :label="pi.queryLabel('nextRunAtEnd')">
        <a-input
          v-model:value="advancedQueryForm.nextRunAtEnd"
          :placeholder="pi.queryPh('nextRunAtEnd', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskDescription')">
      <a-form-item :label="pi.queryLabel('taskDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.taskDescription"
          :placeholder="pi.queryPh('taskDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskStatus')">
      <a-form-item :label="pi.queryLabel('taskStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.taskStatus"
          dict-type="sys_quartz_task_status"
          :placeholder="pi.queryPh('taskStatus', 'select')"
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
        :entity-i18n-key="QUARTZTASK_SELF_I18N_KEY"
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
      :id-column-key="'quartzTaskId'"
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
 * Quartz 定时任务实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/quartz-task
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import QuartzTaskForm from './components/QuartzTaskForm.vue'
import {
  getQuartzTaskList,
  getQuartzTaskById,
  createQuartzTask,
  updateQuartzTask,
  deleteQuartzTaskById,
  deleteQuartzTaskBatch,
  getQuartzTaskTemplate,
  importQuartzTask,
  exportQuartzTask,
  updateQuartzTaskStatus,
  executeQuartzTaskNow,
} from '@/api/foundation/quartzTask.api'
import type { QuartzTask, QuartzTaskQuery } from '@/types/foundation/quartzTask.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { useQuartzSignalRRefresh } from '@/composables/useQuartzSignalrRefresh'
import { useDatabaseInfoCatalog } from '@/composables/useDatabaseInfoCatalog'
import {
  DEFAULT_STAGING_SOURCE_DATABASE,
  buildBomDbAndMonthExecuteParams,
  buildSyncExecuteParams,
  needsBomDbAndMonthPicker,
  needsSyncSourceTargetPicker,
  needsSyncTargetOnlyFromTask,
  parseSyncExecuteDatabaseParams,
} from './utils/quartzSyncDbFields'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine, RiPlayLine, RiPauseLine, RiFlashlightLine } from '@remixicon/vue'

import {
  useQuartzTaskI18n,
  QUARTZTASK_LIST_FIELDS,
  QUARTZTASK_QUERY_STRING_FIELDS,
  QUARTZTASK_QUERY_FIELDS,
  QUARTZTASK_SELF_I18N_KEY,
} from './composables/useQuartzTaskI18n'

/** 任务状态：正常（字典 sys_quartz_task_status） */
const QUARTZ_TASK_STATUS_NORMAL = 0
/** 任务状态：暂停（字典 sys_quartz_task_status） */
const QUARTZ_TASK_STATUS_PAUSED = 1

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useQuartzTaskI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type QuartzTaskRowRecord = QuartzTask | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQuartzTask')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QuartzTask[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<QuartzTaskRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<QuartzTaskRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<QuartzTask> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 操作列「立即执行」loading 行 Id */
const executeLoadingId = ref('')
/** 操作列「启动/暂停」loading 行 Id */
const statusLoadingId = ref('')
/** 立即执行提交中（弹窗确认） */
const executeSubmitting = ref(false)
/** 当前待执行任务 */
const executeTargetTask = ref<QuartzTask | null>(null)
/** 核算月份弹窗 */
const executeMonthVisible = ref(false)
/** 核算月份 YYYY-MM */
const executeCostingMonth = ref<string | undefined>(undefined)
/** 同步库弹窗 */
const executeDbVisible = ref(false)
/** 是否需选源库 */
const executeDbNeedsSource = ref(false)
/** 源库 displayName */
const executeSourceDatabase = ref<string | undefined>(undefined)
/** 目标库 displayName */
const executeTargetDatabase = ref<string | undefined>(undefined)

const {
  databaseInfoList,
  databaseInfoLoading,
  loadDatabaseInfoList,
} = useDatabaseInfoCatalog()

/** 同步库下拉选项（value=displayName） */
const databaseSelectOptions = computed(() =>
  (databaseInfoList.value ?? []).map((item) => ({
    value: item.displayName,
    label: `${item.displayName} (${item.tenantCode})`,
  })),
)

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
  for (const key of QUARTZTASK_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.triggerType !== undefined && form.triggerType !== null) {
    return true
  }
  if (form.intervalSeconds !== undefined && form.intervalSeconds !== null) {
    return true
  }
  if (form.concurrent !== undefined && form.concurrent !== null) {
    return true
  }
  if (form.misfirePolicy !== undefined && form.misfirePolicy !== null) {
    return true
  }
  if (form.executeCount !== undefined && form.executeCount !== null) {
    return true
  }
  if (form.taskStatus !== undefined && form.taskStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(QUARTZTASK_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof QUARTZTASK_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    triggerType: undefined as number | undefined,
    intervalSeconds: undefined as number | undefined,
    concurrent: undefined as number | undefined,
    misfirePolicy: undefined as number | undefined,
    executeCount: undefined as number | undefined,
    taskStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  QUARTZTASK_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'quartzTaskId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {QuartzTaskQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QuartzTaskQuery>): QuartzTaskQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QuartzTaskQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QuartzTaskQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of QUARTZTASK_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.triggerType !== undefined && form.triggerType !== null) {
    query.triggerType = form.triggerType
  }
  if (form.intervalSeconds !== undefined && form.intervalSeconds !== null) {
    query.intervalSeconds = form.intervalSeconds
  }
  if (form.concurrent !== undefined && form.concurrent !== null) {
    query.concurrent = form.concurrent
  }
  if (form.misfirePolicy !== undefined && form.misfirePolicy !== null) {
    query.misfirePolicy = form.misfirePolicy
  }
  if (form.executeCount !== undefined && form.executeCount !== null) {
    query.executeCount = form.executeCount
  }
  if (form.taskStatus !== undefined && form.taskStatus !== null) {
    query.taskStatus = form.taskStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
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
function buildQuartzTaskListColumn(
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
  buildQuartzTaskListColumn('quartzTaskId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...QUARTZTASK_LIST_FIELDS.map((key) => buildQuartzTaskListColumn(key, pi.label(key))),
  CreateActionColumn({
    width: 168,
    actions: [
      {
        key: 'execute',
        label: t('common.page.button.execute'),
        shape: 'plain',
        icon: RiFlashlightLine,
        buttonClass: 'takt-button-run',
        permission: 'foundation:quartz:task:execute',
        loadingFn: (record: QuartzTaskRowRecord) =>
          executeLoadingId.value === getQuartzTaskId(record),
        onClick: (record: QuartzTaskRowRecord) => handleExecuteNow(record),
      },
      {
        key: 'start',
        label: t('common.page.button.start'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'foundation:quartz:task:start',
        visible: (record: QuartzTaskRowRecord) =>
          Number((record as QuartzTask).taskStatus) === QUARTZ_TASK_STATUS_PAUSED,
        loadingFn: (record: QuartzTaskRowRecord) =>
          statusLoadingId.value === getQuartzTaskId(record),
        onClick: (record: QuartzTaskRowRecord) =>
          handleUpdateTaskStatus(record, QUARTZ_TASK_STATUS_NORMAL),
      },
      {
        key: 'pause',
        label: t('common.page.button.pause'),
        shape: 'plain',
        icon: RiPauseLine,
        permission: 'foundation:quartz:task:pause',
        visible: (record: QuartzTaskRowRecord) =>
          Number((record as QuartzTask).taskStatus) !== QUARTZ_TASK_STATUS_PAUSED,
        loadingFn: (record: QuartzTaskRowRecord) =>
          statusLoadingId.value === getQuartzTaskId(record),
        onClick: (record: QuartzTaskRowRecord) =>
          handleUpdateTaskStatus(record, QUARTZ_TASK_STATUS_PAUSED),
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:quartz:task:update',
        onClick: (record: QuartzTaskRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:quartz:task:delete',
        onClick: (record: QuartzTaskRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getQuartzTaskId = (record: QuartzTaskRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getQuartzTaskDictValue = (
  record: QuartzTaskRowRecord,
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
  onChange: (keys: (string | number)[], rows: QuartzTaskRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QuartzTaskRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getQuartzTaskId(selectedRow.value) === getQuartzTaskId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QuartzTaskRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: QuartzTaskRowRecord) => ({
  onClick: () => {
    const key = getQuartzTaskId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQuartzTaskId(item)))
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
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getQuartzTaskList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QuartzTask] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** Quartz SignalR：定义变更 / 执行完成刷新列表 */
useQuartzSignalRRefresh(loadData)

/**
 * 是否为 BOM / PCB SECT / BC 采购价回填任务（立即执行须选目标库 + 核算月份）
 * @param record 任务行
 */
function needsBomCostingMonthPicker(record: QuartzTask): boolean {
  return needsBomDbAndMonthPicker(record)
}

/**
 * 解析行任务实体
 * @param record 行数据
 */
function asQuartzTask(record: QuartzTaskRowRecord): QuartzTask {
  return record as QuartzTask
}

/**
 * 提交立即执行
 * @param id 任务 Id
 * @param executeParams 可选覆盖参数
 */
async function submitExecuteNow(id: string, executeParams?: string): Promise<void> {
  executeLoadingId.value = id
  executeSubmitting.value = true
  try {
    await executeQuartzTaskNow(id, executeParams)
    message.success(t('foundation.quartz-task.page.executeSubmitted'))
    await loadData()
  } catch (error: unknown) {
    const errMsg = error instanceof Error ? error.message : String(error ?? '')
    message.error(errMsg || t('common.feedback.failed'))
  } finally {
    executeLoadingId.value = ''
    executeSubmitting.value = false
  }
}

/**
 * 操作列：立即执行（按任务类型弹窗补参数）
 * @param record 行数据
 */
async function handleExecuteNow(record: QuartzTaskRowRecord): Promise<void> {
  const task = asQuartzTask(record)
  const id = getQuartzTaskId(record)
  if (!id) {
    return
  }
  executeTargetTask.value = task
  if (needsBomCostingMonthPicker(task)) {
    const now = new Date()
    const month = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`
    executeCostingMonth.value = month
    const parsed = parseSyncExecuteDatabaseParams(task.executeParams)
    try {
      await loadDatabaseInfoList()
    } catch {
      // catalog 失败时仍打开弹窗，由用户重试
    }
    executeTargetDatabase.value = parsed.targetDatabase
    executeMonthVisible.value = true
    return
  }
  if (needsSyncTargetOnlyFromTask(task) || needsSyncSourceTargetPicker(task.sqlScript)) {
    executeDbNeedsSource.value =
      !needsSyncTargetOnlyFromTask(task) && needsSyncSourceTargetPicker(task.sqlScript)
    const parsed = parseSyncExecuteDatabaseParams(task.executeParams)
    try {
      await loadDatabaseInfoList()
    } catch {
      // catalog 失败时仍打开弹窗，由用户重试
    }
    executeSourceDatabase.value =
      parsed.sourceDatabase ||
      (executeDbNeedsSource.value ? DEFAULT_STAGING_SOURCE_DATABASE : undefined)
    executeTargetDatabase.value = parsed.targetDatabase
    executeDbVisible.value = true
    return
  }
  Modal.confirm({
    title: t('common.page.button.execute'),
    content: task.taskCode || task.taskName || pi.self(),
    okText: t('common.page.button.execute'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => submitExecuteNow(id),
  })
}

/**
 * 核算月规范为 yyyy-MM（月份选择器可能给出 yyyy-MM-dd / Dayjs）
 * @param value 弹窗选中值
 * @returns {string} yyyy-MM；无法识别则空串
 */
function resolveCostingPeriodMonth(value: unknown): string {
  if (typeof value === 'string') {
    const s = value.trim()
    const matched = s.match(/^(\d{4}-\d{2})/)
    return matched?.[1] ?? ''
  }
  if (
    value
    && typeof value === 'object'
    && 'format' in value
    && typeof (value as { format: (f: string) => string }).format === 'function'
  ) {
    return (value as { format: (f: string) => string }).format('YYYY-MM')
  }
  return ''
}

/** 核算月份 + 目标库弹窗确认 */
async function handleExecuteMonthSubmit(): Promise<void> {
  const id = executeTargetTask.value ? getQuartzTaskId(executeTargetTask.value) : ''
  const month = resolveCostingPeriodMonth(executeCostingMonth.value)
  const targetDatabase = executeTargetDatabase.value?.trim()
  if (!id) {
    return
  }
  if (!targetDatabase) {
    message.warning(t('foundation.quartz-task.page.executeDb.targetRequired'))
    return
  }
  if (!month) {
    message.warning(t('foundation.quartz-task.page.executeMonth.costingMonthRequired'))
    return
  }
  executeMonthVisible.value = false
  await submitExecuteNow(
    id,
    buildBomDbAndMonthExecuteParams({ targetDatabase, costingPeriod: month }),
  )
  executeTargetTask.value = null
}

/** 关闭核算月份弹窗 */
function handleExecuteMonthCancel(): void {
  executeMonthVisible.value = false
  executeTargetTask.value = null
}

/** 同步库弹窗确认 */
async function handleExecuteDbSubmit(): Promise<void> {
  const id = executeTargetTask.value ? getQuartzTaskId(executeTargetTask.value) : ''
  if (!id) {
    return
  }
  if (executeDbNeedsSource.value && !executeSourceDatabase.value?.trim()) {
    message.warning(t('foundation.quartz-task.page.executeDb.sourceRequired'))
    return
  }
  if (!executeTargetDatabase.value?.trim()) {
    message.warning(t('foundation.quartz-task.page.executeDb.targetRequired'))
    return
  }
  const params = buildSyncExecuteParams({
    sourceDatabase: executeDbNeedsSource.value ? executeSourceDatabase.value : undefined,
    targetDatabase: executeTargetDatabase.value,
  })
  executeDbVisible.value = false
  await submitExecuteNow(id, params)
  executeTargetTask.value = null
}

/** 关闭同步库弹窗 */
function handleExecuteDbCancel(): void {
  executeDbVisible.value = false
  executeTargetTask.value = null
}

/**
 * 操作列：启动 / 暂停（更新 TaskStatus 并挂接调度器）
 * @param record 行数据
 * @param taskStatus 目标状态
 */
async function handleUpdateTaskStatus(
  record: QuartzTaskRowRecord,
  taskStatus: number,
): Promise<void> {
  const id = getQuartzTaskId(record)
  if (!id) {
    return
  }
  statusLoadingId.value = id
  try {
    await updateQuartzTaskStatus({ quartzTaskId: id, taskStatus })
    message.success(
      taskStatus === QUARTZ_TASK_STATUS_PAUSED
        ? t('common.feedback.updated', { target: t('common.page.button.pause') })
        : t('common.feedback.updated', { target: t('common.page.button.start') }),
    )
    await loadData()
  } catch (error: unknown) {
    const errMsg = error instanceof Error ? error.message : String(error ?? '')
    message.error(errMsg || t('common.feedback.failed'))
  } finally {
    statusLoadingId.value = ''
  }
}

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
async function handleEdit(record: QuartzTaskRowRecord) {
  const id = getQuartzTaskId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getQuartzTaskById(id)
    formData.value = detail ?? ({ ...record } as Partial<QuartzTask>)
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
      await updateQuartzTask(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createQuartzTask(payload as any)
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
  const res = await getQuartzTaskTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importQuartzTask(file, sheetName)
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
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportQuartzTask(
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
    logger.error('[QuartzTask] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QuartzTaskRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQuartzTaskById((record as any)[entityIdName])
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
      await deleteQuartzTaskBatch(ids)
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
