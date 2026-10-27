<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-plant -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt工厂物料实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:materials:material:plant:create"
      update-permission="logistics:materials:material:plant:update"
      delete-permission="logistics:materials:material:plant:delete"
      import-permission="logistics:materials:material:plant:import"
      export-permission="logistics:materials:material:plant:export"
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
      :id-column-key="'materialPlantId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getMaterialPlantId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'materialStatus'">
          <a-switch
            :checked="getMaterialPlantDictValue(record, 'materialStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleMaterialStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'industrySector'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'industrySector')"
            dict-type="logistics_materials_industry_sector"
          />
        </template>
        <template v-else-if="column.key === 'materialType'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'materialType')"
            dict-type="logistics_materials_material_type"
          />
        </template>
        <template v-else-if="column.key === 'baseUnit'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'baseUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'purchaseType'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'purchaseType')"
            dict-type="logistics_procurement_type"
          />
        </template>
        <template v-else-if="column.key === 'specialProcurement'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'specialProcurement')"
            dict-type="logistics_procurement_special_procurement_type"
          />
        </template>
        <template v-else-if="column.key === 'isBulk'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'isBulk')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'currencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'priceControl'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'priceControl')"
            dict-type="logistics_materials_price_control"
          />
        </template>
        <template v-else-if="column.key === 'priceUnit'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'priceUnit')"
            dict-type="logistics_materials_price_unit_param"
          />
        </template>
        <template v-else-if="column.key === 'valuation'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'valuation')"
            dict-type="logistics_materials_valuation_class"
          />
        </template>
        <template v-else-if="column.key === 'requiresInspection'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'requiresInspection')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'isBatch'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'isBatch')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'discontinuedStatus'">
          <TaktDictTag
            :value="getMaterialPlantDictValue(record, 'discontinuedStatus')"
            dict-type="logistics_materials_material_discontinued_status"
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
      <MaterialPlantForm
        :key="formData?.materialPlantId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-materials-material-plant'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCode"
          api-url="TaktGeneralMaterials/options"
          :placeholder="pi.queryPh('materialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="pi.queryLabel('materialDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="pi.queryPh('materialDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="pi.queryLabel('materialSpecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="pi.queryPh('materialSpecification', 'required')"
          show-count
          :maxlength="70"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="pi.queryLabel('industrySector')">
        <TaktSelect
          v-model:value="advancedQueryForm.industrySector"
          dict-type="logistics_materials_industry_sector"
          :placeholder="pi.queryPh('industrySector', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialHierarchy')">
      <a-form-item :label="pi.queryLabel('materialHierarchy')">
        <a-input
          v-model:value="advancedQueryForm.materialHierarchy"
          :placeholder="pi.queryPh('materialHierarchy', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialGroup')">
      <a-form-item :label="pi.queryLabel('materialGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialGroup"
          api-url="TaktMaterialGroups/options"
          :placeholder="pi.queryPh('materialGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="pi.queryLabel('materialType')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialType"
          dict-type="logistics_materials_material_type"
          :placeholder="pi.queryPh('materialType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="pi.queryLabel('baseUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.baseUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('baseUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseGroup')">
      <a-form-item :label="pi.queryLabel('purchaseGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseGroup"
          api-url="TaktPurchaseGroups/options"
          :placeholder="pi.queryPh('purchaseGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseType')">
      <a-form-item :label="pi.queryLabel('purchaseType')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseType"
          dict-type="logistics_procurement_type"
          :placeholder="pi.queryPh('purchaseType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('specialProcurement')">
      <a-form-item :label="pi.queryLabel('specialProcurement')">
        <TaktSelect
          v-model:value="advancedQueryForm.specialProcurement"
          dict-type="logistics_procurement_special_procurement_type"
          :placeholder="pi.queryPh('specialProcurement', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBulk')">
      <a-form-item :label="pi.queryLabel('isBulk')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBulk"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isBulk', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minOrderQuantity')">
      <a-form-item :label="pi.queryLabel('minOrderQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.minOrderQuantity"
          :placeholder="pi.queryPh('minOrderQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('roundingValue')">
      <a-form-item :label="pi.queryLabel('roundingValue')">
        <a-input-number
          v-model:value="advancedQueryForm.roundingValue"
          :placeholder="pi.queryPh('roundingValue', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDeliveryTimeDays')">
      <a-form-item :label="pi.queryLabel('plannedDeliveryTimeDays')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDeliveryTimeDays"
          :placeholder="pi.queryPh('plannedDeliveryTimeDays', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inHouseProductionDays')">
      <a-form-item :label="pi.queryLabel('inHouseProductionDays')">
        <a-input-number
          v-model:value="advancedQueryForm.inHouseProductionDays"
          :placeholder="pi.queryPh('inHouseProductionDays', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturer')">
      <a-form-item :label="pi.queryLabel('manufacturer')">
        <TaktSelect
          v-model:value="advancedQueryForm.manufacturer"
          api-url="TaktSuppliers/options"
          :placeholder="pi.queryPh('manufacturer', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerMaterialCode')">
      <a-form-item :label="pi.queryLabel('manufacturerMaterialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.manufacturerMaterialCode"
          api-url="TaktManufacturerMaterials/options"
          :placeholder="pi.queryPh('manufacturerMaterialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceControl')">
      <a-form-item :label="pi.queryLabel('priceControl')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceControl"
          dict-type="logistics_materials_price_control"
          :placeholder="pi.queryPh('priceControl', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceUnit')">
      <a-form-item :label="pi.queryLabel('priceUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceUnit"
          dict-type="logistics_materials_price_unit_param"
          :placeholder="pi.queryPh('priceUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valuation')">
      <a-form-item :label="pi.queryLabel('valuation')">
        <TaktSelect
          v-model:value="advancedQueryForm.valuation"
          dict-type="logistics_materials_valuation_class"
          :placeholder="pi.queryPh('valuation', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movingPrice')">
      <a-form-item :label="pi.queryLabel('movingPrice')">
        <a-input-number
          v-model:value="advancedQueryForm.movingPrice"
          :placeholder="pi.queryPh('movingPrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('differenceCode')">
      <a-form-item :label="pi.queryLabel('differenceCode')">
        <a-input
          v-model:value="advancedQueryForm.differenceCode"
          :placeholder="pi.queryPh('differenceCode', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenter')">
      <a-form-item :label="pi.queryLabel('profitCenter')">
        <TaktSelect
          v-model:value="advancedQueryForm.profitCenter"
          api-url="TaktProfitCenters/options"
          :placeholder="pi.queryPh('profitCenter', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentStock')">
      <a-form-item :label="pi.queryLabel('currentStock')">
        <a-input-number
          v-model:value="advancedQueryForm.currentStock"
          :placeholder="pi.queryPh('currentStock', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLocation')">
      <a-form-item :label="pi.queryLabel('productionLocation')">
        <TaktSelect
          v-model:value="advancedQueryForm.productionLocation"
          api-url="TaktWarehouses/options"
          :placeholder="pi.queryPh('productionLocation', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasingLocation')">
      <a-form-item :label="pi.queryLabel('purchasingLocation')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchasingLocation"
          api-url="TaktWarehouses/options"
          :placeholder="pi.queryPh('purchasingLocation', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storageLocation')">
      <a-form-item :label="pi.queryLabel('storageLocation')">
        <TaktSelect
          v-model:value="advancedQueryForm.storageLocation"
          api-url="TaktStorageLocations/options"
          :placeholder="pi.queryPh('storageLocation', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiresInspection')">
      <a-form-item :label="pi.queryLabel('requiresInspection')">
        <TaktSelect
          v-model:value="advancedQueryForm.requiresInspection"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('requiresInspection', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBatch')">
      <a-form-item :label="pi.queryLabel('isBatch')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBatch"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isBatch', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discontinuedStatus')">
      <a-form-item :label="pi.queryLabel('discontinuedStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.discontinuedStatus"
          dict-type="logistics_materials_material_discontinued_status"
          :placeholder="pi.queryPh('discontinuedStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialStatus')">
      <a-form-item :label="pi.queryLabel('materialStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('materialStatus', 'select')"
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
        :entity-i18n-key="MATERIALPLANT_SELF_I18N_KEY"
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
      :id-column-key="'materialPlantId'"
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
 * Takt工厂物料实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-plant
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import MaterialPlantForm from './components/MaterialPlantForm.vue'
import { getMaterialPlantList, getMaterialPlantById, createMaterialPlant, updateMaterialPlant, deleteMaterialPlantById, deleteMaterialPlantBatch, getMaterialPlantTemplate, importMaterialPlant, exportMaterialPlant, updateMaterialPlantStatus } from '@/api/logistics/materials/materialPlant.api'
import type { MaterialPlant, MaterialPlantQuery } from '@/types/logistics/materials/materialPlant.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useMaterialPlantI18n,
  MATERIALPLANT_LIST_FIELDS,
  MATERIALPLANT_QUERY_STRING_FIELDS,
  MATERIALPLANT_QUERY_FIELDS,
  MATERIALPLANT_SELF_I18N_KEY,
} from './composables/useMaterialPlantI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMaterialPlantI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type MaterialPlantRowRecord = MaterialPlant | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialPlant')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MaterialPlant[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<MaterialPlantRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<MaterialPlantRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MaterialPlant> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 当前自然月起止（与后端 GetCurrentMonthRangeBounds 对齐）
 * @returns {{ start: string, end: string }}
 */
function getCurrentMonthQueryBounds() {
  const now = new Date()
  const y = now.getFullYear()
  const m = now.getMonth()
  const pad = (n: number) => String(n).padStart(2, '0')
  const lastDay = new Date(y, m + 1, 0).getDate()
  const ym = `${y}-${pad(m + 1)}`
  return {
    start: `${ym}-01 00:00:00`,
    end: `${ym}-${pad(lastDay)} 23:59:59`,
  }
}

/**
 * 是否存在除默认当前月日期外的查询条件（有参则不强制当月）
 * @param form 高级查询表单
 * @param kw 关键字
 * @returns {boolean}
 */
function hasListQueryFiltersBesidesDefaultScope(
  form: Record<string, unknown>,
  kw: string,
): boolean {
  if ((kw ?? '').trim().length > 0) {
    return true
  }
  for (const key of MATERIALPLANT_QUERY_STRING_FIELDS) {
    if (key === 'createdAtStart' || key === 'createdAtEnd') {
      continue
    }
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.specialProcurement !== undefined && form.specialProcurement !== null) {
    return true
  }
  if (form.isBulk !== undefined && form.isBulk !== null) {
    return true
  }
  if (form.minOrderQuantity !== undefined && form.minOrderQuantity !== null) {
    return true
  }
  if (form.roundingValue !== undefined && form.roundingValue !== null) {
    return true
  }
  if (form.plannedDeliveryTimeDays !== undefined && form.plannedDeliveryTimeDays !== null) {
    return true
  }
  if (form.inHouseProductionDays !== undefined && form.inHouseProductionDays !== null) {
    return true
  }
  if (form.priceUnit !== undefined && form.priceUnit !== null) {
    return true
  }
  if (form.movingPrice !== undefined && form.movingPrice !== null) {
    return true
  }
  if (form.currentStock !== undefined && form.currentStock !== null) {
    return true
  }
  if (form.requiresInspection !== undefined && form.requiresInspection !== null) {
    return true
  }
  if (form.isBatch !== undefined && form.isBatch !== null) {
    return true
  }
  if (form.materialStatus !== undefined && form.materialStatus !== null) {
    return true
  }
  return false
}
/**
 * 创建空的高级查询表单（无参默认当前月或当前期间）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(MATERIALPLANT_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof MATERIALPLANT_QUERY_STRING_FIELDS)[number],
    string
  >
  const month = getCurrentMonthQueryBounds()
  return {
    ...form,
    specialProcurement: undefined as number | undefined,
    isBulk: undefined as number | undefined,
    minOrderQuantity: undefined as number | undefined,
    roundingValue: undefined as number | undefined,
    plannedDeliveryTimeDays: undefined as number | undefined,
    inHouseProductionDays: undefined as number | undefined,
    priceUnit: undefined as number | undefined,
    movingPrice: undefined as number | undefined,
    currentStock: undefined as number | undefined,
    requiresInspection: undefined as number | undefined,
    isBatch: undefined as number | undefined,
    materialStatus: undefined as number | undefined,    createdAtStart: month.start,
    createdAtEnd: month.end,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  MATERIALPLANT_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'materialPlantId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参默认当前月或当前期间）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialPlantQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialPlantQuery>): MaterialPlantQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialPlantQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialPlantQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MATERIALPLANT_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.specialProcurement !== undefined && form.specialProcurement !== null) {
    query.specialProcurement = form.specialProcurement
  }
  if (form.isBulk !== undefined && form.isBulk !== null) {
    query.isBulk = form.isBulk
  }
  if (form.minOrderQuantity !== undefined && form.minOrderQuantity !== null) {
    query.minOrderQuantity = form.minOrderQuantity
  }
  if (form.roundingValue !== undefined && form.roundingValue !== null) {
    query.roundingValue = form.roundingValue
  }
  if (form.plannedDeliveryTimeDays !== undefined && form.plannedDeliveryTimeDays !== null) {
    query.plannedDeliveryTimeDays = form.plannedDeliveryTimeDays
  }
  if (form.inHouseProductionDays !== undefined && form.inHouseProductionDays !== null) {
    query.inHouseProductionDays = form.inHouseProductionDays
  }
  if (form.priceUnit !== undefined && form.priceUnit !== null) {
    query.priceUnit = form.priceUnit
  }
  if (form.movingPrice !== undefined && form.movingPrice !== null) {
    query.movingPrice = form.movingPrice
  }
  if (form.currentStock !== undefined && form.currentStock !== null) {
    query.currentStock = form.currentStock
  }
  if (form.requiresInspection !== undefined && form.requiresInspection !== null) {
    query.requiresInspection = form.requiresInspection
  }
  if (form.isBatch !== undefined && form.isBatch !== null) {
    query.isBatch = form.isBatch
  }
  if (form.materialStatus !== undefined && form.materialStatus !== null) {
    query.materialStatus = form.materialStatus
  }
  // 无参默认当前月；有其它条件且未填日期 → 不限制月份
  if (!hasListQueryFiltersBesidesDefaultScope(form, kw)) {
    const startVal = String(form.createdAtStart ?? '').trim()
    const endVal = String(form.createdAtEnd ?? '').trim()
    if (!startVal && !endVal) {
      const month = getCurrentMonthQueryBounds()
      query.createdAtStart = month.start as never
      query.createdAtEnd = month.end as never
    }
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
function buildMaterialPlantListColumn(
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
  buildMaterialPlantListColumn('materialPlantId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...MATERIALPLANT_LIST_FIELDS.map((key) => buildMaterialPlantListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:material:plant:update',
        onClick: (record: MaterialPlantRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:plant:delete',
        onClick: (record: MaterialPlantRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaterialPlantId = (record: MaterialPlantRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialPlantDictValue = (
  record: MaterialPlantRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toMaterialPlantNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialPlantRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaterialPlantRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getMaterialPlantId(selecmedRow.value) === getMaterialPlantId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: MaterialPlantRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: MaterialPlantRowRecord) => ({
  onClick: () => {
    const key = getMaterialPlantId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getMaterialPlantId(item)))
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
    const res = await getMaterialPlantList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MaterialPlant] 加载数据失败', { error })
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
async function handleEdit(record: MaterialPlantRowRecord) {
  const id = getMaterialPlantId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getMaterialPlantById(id)
    formData.value = detail ?? ({ ...record } as Partial<MaterialPlant>)
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
      await updateMaterialPlant(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMaterialPlant(payload as any)
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
  const res = await getMaterialPlantTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMaterialPlant(file, sheetName)
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
    const exportMeta = await exportMaterialPlant(
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
    logger.error('[MaterialPlant] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaterialPlantRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialPlantById((record as any)[entityIdName])
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
      await deleteMaterialPlantBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleMaterialStatusChange(record: MaterialPlantRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toMaterialPlantNumber(getMaterialPlantDictValue(record, 'materialStatus'))
  const id = getMaterialPlantId(record)
  const row = dataSource.value.find((item) => getMaterialPlantId(item) === id)
  if (row) {
    row.materialStatus = newVal
  }
  try {
    await updateMaterialPlantStatus({ materialPlantId: id, materialStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.materialStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
