<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/serial/outbound/components -->
<!-- 文件名称：OutboundForm.vue -->
<!-- 功能描述：序列号出库主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form outbound-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="outbound-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('outboundCode')"
                name="outboundCode"
              >
                <a-input
                  v-model:value="formState.outboundCode"
                  :placeholder="pi.ph('outboundCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shippingInvoiceCode')"
                name="shippingInvoiceCode"
              >
                <a-input
                  v-model:value="formState.shippingInvoiceCode"
                  :placeholder="pi.ph('shippingInvoiceCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('outboundDate')"
                name="outboundDate"
              >
                <a-date-picker
                  v-model:value="formState.outboundDate"
                  :placeholder="pi.ph('outboundDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('destination')"
                name="destination"
              >
                <TaktSelect
                  v-model:value="formState.destination"
                  api-url="TaktModelDestinations/options"
                  :placeholder="pi.ph('destination')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('destinationPort')"
                name="destinationPort"
              >
                <TaktSelect
                  v-model:value="formState.destinationPort"
                  dict-type="logistics_serial_destination_port_code"
                  :placeholder="pi.ph('destinationPort')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('outboundType')"
                name="outboundType"
              >
                <TaktSelect
                  v-model:value="formState.outboundType"
                  dict-type="logistics_materials_outbound_type"
                  :placeholder="pi.ph('outboundType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseCode')"
                name="warehouseCode"
              >
                <TaktSelect
                  v-model:value="formState.warehouseCode"
                  api-url="TaktWarehouses/options"
                  :placeholder="pi.ph('warehouseCode')"
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('locationCode')"
                name="locationCode"
              >
                <TaktSelect
                  v-model:value="formState.locationCode"
                  api-url="TaktStorageLocations/options"
                  :placeholder="pi.ph('locationCode')"
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalQuantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="pi.ph('totalQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="serialOutboundItemTableRef"
      v-model="childSerialOutboundItemRows"
      :columns="serialOutboundItemFormColumns"
      :title="serialOutboundItemPi.self()"
      :add-button-entity="serialOutboundItemPi.self()"
      id-field="serialOutboundItemId"
      :default-row="createDefaultSerialOutboundItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-referenceInboundId="{ record }">
        <TaktSelect
          v-model:value="record.referenceInboundId"
          api-url="TaktSerialInbounds/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="serialOutboundItemPi.queryPh('referenceInboundId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-referenceInboundCode="{ record }">
        <TaktSelect
          v-model:value="record.referenceInboundCode"
          api-url="TaktSerialInbounds/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="serialOutboundItemPi.queryPh('referenceInboundCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="serialOutboundItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 序列号出库主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/serial/outbound/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSerialOutboundI18n } from '../composables/useOutboundI18n'

/** 实体字段 i18n */
const pi = useSerialOutboundI18n()

import type { SerialOutboundCreate } from '@/types/logistics/serial/outbound.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","outboundCode","shippingInvoiceCode","outboundDate","destination","destinationPort","outboundType","warehouseCode","locationCode","totalQuantity","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useSerialOutboundItemI18n } from '../composables/useOutboundItemI18n'

const serialOutboundItemPi = useSerialOutboundItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSerialOutboundItemRows = ref<Record<string, unknown>[]>([])
const serialOutboundItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedSerialOutboundItemRow(row: Record<string, unknown>): boolean {
  const id = row.serialOutboundItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextSerialOutboundItemLineNumber(): number {
  const rows = serialOutboundItemTableRef.value?.getRows?.() ?? childSerialOutboundItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 serialOutboundItem 可编辑列 */
const serialOutboundItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: serialOutboundItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'outboundSerialCode',
    title: serialOutboundItemPi.label('outboundSerialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'referenceInboundId',
    title: serialOutboundItemPi.label('referenceInboundId'),
    width: 140,
  },
  {
    key: 'referenceInboundCode',
    title: serialOutboundItemPi.label('referenceInboundCode'),
    width: 140,
  },
  {
    key: 'referenceInboundLineNumber',
    title: serialOutboundItemPi.label('referenceInboundLineNumber'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: serialOutboundItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SerialOutboundCreate & { serialOutboundId?: string }> | null | undefined) {
  const rows_serialOutboundItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSerialOutboundItemRows.value = rows_serialOutboundItem
}

function createDefaultSerialOutboundItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextSerialOutboundItemLineNumber(),
    outboundSerialCode: '',
    referenceInboundId: '',
    referenceInboundCode: '',
    referenceInboundLineNumber: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.serialOutboundId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: serialOutboundItemTableRef.value?.getRows?.() ?? childSerialOutboundItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        outboundId: masterId,
      }
      if (isUpdate && isPersistedSerialOutboundItemRow(row)) {
        normalized.serialOutboundItemId = row.serialOutboundItemId
      } else {
        delete normalized.serialOutboundItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SerialOutboundCreate & { serialOutboundId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  destinationPort: "ACE_AIR",
  outboundType: 5
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 serialOutboundId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.serialOutboundId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.serialOutboundId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  outboundCode: [
    {
      required: true,
      message: pi.ph('outboundCode'),
      trigger: 'blur'
    }
  ],
  shippingInvoiceCode: [
    {
      required: true,
      message: pi.ph('shippingInvoiceCode'),
      trigger: 'blur'
    }
  ],
  outboundDate: [
    {
      required: true,
      message: pi.ph('outboundDate'),
      trigger: 'change'
    }
  ],
  destination: [
    {
      required: true,
      message: pi.ph('destination'),
      trigger: 'change'
    }
  ],
  destinationPort: [
    {
      required: true,
      message: pi.ph('destinationPort'),
      trigger: 'change'
    }
  ],
  outboundType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('outboundType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('outboundType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  warehouseCode: [
    {
      required: true,
      message: pi.ph('warehouseCode'),
      trigger: 'change'
    }
  ],
  locationCode: [
    {
      required: true,
      message: pi.ph('locationCode'),
      trigger: 'change'
    }
  ],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await serialOutboundItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('outboundType' in payload) {
    const rawoutboundType = payload.outboundType
    if (rawoutboundType === undefined || rawoutboundType === null || rawoutboundType === '') {
      delete payload.outboundType
    } else {
      const numoutboundType = typeof rawoutboundType === 'number' ? rawoutboundType : Number(rawoutboundType)
      if (Number.isFinite(numoutboundType)) payload.outboundType = numoutboundType
      else delete payload.outboundType
    }
  }
  if ('totalQuantity' in payload) {
    const rawtotalQuantity = payload.totalQuantity
    if (rawtotalQuantity === undefined || rawtotalQuantity === null || rawtotalQuantity === '') {
      delete payload.totalQuantity
    } else {
      const numtotalQuantity = typeof rawtotalQuantity === 'number' ? rawtotalQuantity : Number(rawtotalQuantity)
      if (Number.isFinite(numtotalQuantity)) payload.totalQuantity = numtotalQuantity
      else delete payload.totalQuantity
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.serialOutboundId) {
    payload.serialOutboundId = props.formData.serialOutboundId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.serialOutboundId)
  childSerialOutboundItemRows.value = []
  serialOutboundItemTableRef.value?.resetRows?.()
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
