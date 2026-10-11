<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/history/components -->
<!-- 文件名称：EquipmentForm.vue -->
<!-- 功能描述：Takt工厂设备实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form equipment-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="equipment-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.code')"
                name="EquipCode"
              >
                <a-input
                  v-model:value="formState.EquipCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.equipmentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.name')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.name') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.type')"
                name="equipmentType"
              >
                <a-input-number
                  v-model:value="formState.equipmentType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.type') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.model')"
                name="equipmentModel"
              >
                <a-input
                  v-model:value="formState.equipmentModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.model') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.specification')"
                name="EquipSpecification"
              >
                <a-input
                  v-model:value="formState.EquipSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.specification') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.brand')"
                name="EquipBrand"
              >
                <a-input
                  v-model:value="formState.EquipBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.brand') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.manufacturer') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.dealerby')"
                name="dealerBy"
              >
                <a-input
                  v-model:value="formState.dealerBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.dealerby') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.serialnumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.serialnumber') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.workshopby')"
                name="workshopBy"
              >
                <a-input
                  v-model:value="formState.workshopBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workshopby') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.productionlineby')"
                name="productionLineBy"
              >
                <a-input
                  v-model:value="formState.productionLineBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.productionlineby') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.workstationby')"
                name="workstationBy"
              >
                <a-input
                  v-model:value="formState.workstationBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workstationby') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.deptby')"
                name="deptBy"
              >
                <a-input
                  v-model:value="formState.deptBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.deptby') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.location')"
                name="equipmentLocation"
              >
                <a-input
                  v-model:value="formState.equipmentLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.location') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.responsibleuserby')"
                name="responsibleUserBy"
              >
                <a-input
                  v-model:value="formState.responsibleUserBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.responsibleuserby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.operatorby')"
                name="operatorBy"
              >
                <a-input
                  v-model:value="formState.operatorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.operatorby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.purchasedate')"
                name="purchaseDate"
              >
                <a-date-picker
                  v-model:value="formState.purchaseDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.purchasedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.installationdate')"
                name="installationDate"
              >
                <a-date-picker
                  v-model:value="formState.installationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.installationdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.startdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantystartdate')"
                name="warrantyStartDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyStartDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystartdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantyenddate')"
                name="warrantyEndDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyEndDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantyenddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.originalvalue')"
                name="equipmentOriginalValue"
              >
                <a-input-number
                  v-model:value="formState.equipmentOriginalValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.originalvalue') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.technicalparameters')"
                name="technicalParameters"
              >
                <a-input
                  v-model:value="formState.technicalParameters"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.technicalparameters') })"
                  show-count
                  :maxlength="4000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.images')"
                name="equipmentImages"
              >
                <a-input
                  v-model:value="formState.equipmentImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.images') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.documents')"
                name="equipmentDocuments"
              >
                <a-input
                  v-model:value="formState.equipmentDocuments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.documents') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.iscritical')"
                name="isCritical"
              >
                <a-input-number
                  v-model:value="formState.isCritical"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.iscritical') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.equipment.warrantystatus')"
                name="warrantyStatus"
              >
                <a-input-number
                  v-model:value="formState.warrantyStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.warrantystatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.equipment.status')"
                name="equipmentStatus"
              >
                <TaktSelect
                  v-model:value="formState.equipmentStatus"
                  dict-type="sys_equipment_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.status') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
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
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 maintenanceHistories -->
    <TaktEditableTable
      ref="maintenanceHistoryTableRef"
      v-model="childMaintenanceHistoryRows"
      :columns="maintenanceHistoryFormColumns"
      :title="t('entity.maintenancehistory._self')"
      :add-button-entity="t('entity.maintenancehistory._self')"
      id-field="maintenanceHistoryId"
      :default-row="createDefaultMaintenanceHistoryRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt工厂设备实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/history/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EquipmentCreate } from '@/types/logistics/maintenance/equipment.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (formFields.includes('plantCode') && (force || !target.plantCode)) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
  if (formFields.includes('relatedPlant') && (force || !target.relatedPlant)) {
    target.relatedPlant = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","EquipCode","equipmentName","equipmentType","equipmentModel","EquipSpecification","EquipBrand","manufacturer","dealerBy","serialNumber","workshopBy","productionLineBy","workstationBy","deptBy","equipmentLocation","responsibleUserBy","operatorBy","purchaseDate","installationDate","startDate","warrantyStartDate","warrantyEndDate","equipmentOriginalValue","technicalParameters","equipmentImages","equipmentDocuments","isCritical","warrantyStatus","equipmentStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childMaintenanceHistoryRows = ref<Record<string, unknown>[]>([])
const maintenanceHistoryTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 maintenanceHistory 可编辑列 */
const maintenanceHistoryFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'maintenanceWorkOrderId',
    title: t('entity.maintenancehistory.maintenanceworkorderid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'workOrderCode',
    title: t('entity.maintenancehistory.workordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'maintenanceType',
    title: t('entity.maintenancehistory.maintenancetype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'maintenanceCategory',
    title: t('entity.maintenancehistory.maintenancecategory'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'maintenanceCompany',
    title: t('entity.maintenancehistory.maintenancecompany'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.maintenancehistory.maintenancecompany') }),
  },
  {
    key: 'maintenanceTechnician',
    title: t('entity.maintenancehistory.maintenancetechnician'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.maintenancehistory.maintenancetechnician') }),
  },
  {
    key: 'maintenanceDate',
    title: t('entity.maintenancehistory.maintenancedate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'maintenanceStartTime',
    title: t('entity.maintenancehistory.maintenancestarttime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<EquipmentCreate & { equipmentId?: string }> | null | undefined) {
  childMaintenanceHistoryRows.value = ((val as any)?.maintenanceHistories ?? []) as Record<string, unknown>[]
}

function createDefaultMaintenanceHistoryRow(): Record<string, unknown> {
  return {
    maintenanceWorkOrderId: '',
    workOrderCode: '',
    maintenanceType: 0,
    maintenanceCategory: 0,
    maintenanceCompany: '',
    maintenanceTechnician: '',
    maintenanceDate: '',
    maintenanceStartTime: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.equipmentId ?? ''
  return {
    ...formState,
    maintenanceHistories: maintenanceHistoryTableRef.value?.getRows?.() ?? childMaintenanceHistoryRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      equipmentId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EquipmentCreate & { equipmentId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 equipmentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.equipmentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).maintenanceHistories
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    const isCreate = !props.formData?.equipmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') }),
      trigger: 'blur'
    }
  ],
  EquipCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.code') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.name') }),
      trigger: 'blur'
    }
  ],
  equipmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.type') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.type') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  equipmentOriginalValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.originalvalue') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.originalvalue') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCritical: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.iscritical') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.iscritical') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  warrantyStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  equipmentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipment.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await maintenanceHistoryTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('equipmentType' in payload) {
    const rawequipmentType = payload.equipmentType
    payload.equipmentType = typeof rawequipmentType === 'number' ? rawequipmentType : Number(rawequipmentType)
  }
  if ('equipmentOriginalValue' in payload) {
    const rawequipmentOriginalValue = payload.equipmentOriginalValue
    payload.equipmentOriginalValue = typeof rawequipmentOriginalValue === 'number' ? rawequipmentOriginalValue : Number(rawequipmentOriginalValue)
  }
  if ('isCritical' in payload) {
    const rawisCritical = payload.isCritical
    payload.isCritical = typeof rawisCritical === 'number' ? rawisCritical : Number(rawisCritical)
  }
  if ('warrantyStatus' in payload) {
    const rawwarrantyStatus = payload.warrantyStatus
    payload.warrantyStatus = typeof rawwarrantyStatus === 'number' ? rawwarrantyStatus : Number(rawwarrantyStatus)
  }
  if ('equipmentStatus' in payload) {
    const rawequipmentStatus = payload.equipmentStatus
    payload.equipmentStatus = typeof rawequipmentStatus === 'number' ? rawequipmentStatus : Number(rawequipmentStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.equipmentId)
  childMaintenanceHistoryRows.value = []
  maintenanceHistoryTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
