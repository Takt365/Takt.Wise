<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-address/components -->
<!-- 文件名称：EmployeeAddressForm.vue -->
<!-- 功能描述：员工实体子表 employeeAddress 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form employee-address-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="employee-address-form-tabs"
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
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
                :label="pi.label('addressType')"
                name="addressType"
              >
                <TaktSelect
                  v-model:value="formState.addressType"
                  dict-type="humanresource_personnel_employee_address_type"
                  :placeholder="pi.ph('addressType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('country')"
                name="country"
              >
                <TaktSelect
                  v-model:value="formState.country"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('country')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('province')"
                name="province"
              >
                <TaktSelect
                  v-model:value="formState.province"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 2 }"
                  :placeholder="pi.ph('province')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('city')"
                name="city"
              >
                <TaktSelect
                  v-model:value="formState.city"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 3 }"
                  :placeholder="pi.ph('city')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('district')"
                name="district"
              >
                <TaktSelect
                  v-model:value="formState.district"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 4 }"
                  :placeholder="pi.ph('district')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('address1')"
                name="address1"
              >
                <a-textarea
                  v-model:value="formState.address1"
                  :placeholder="pi.ph('address1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('address2')"
                name="address2"
              >
                <a-textarea
                  v-model:value="formState.address2"
                  :placeholder="pi.ph('address2')"
                  :rows="2"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 员工实体子表 employeeAddress 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/human-resource/personnel/employee-address/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useEmployeeAddressI18n } from '../composables/useEmployeeAddressI18n'

/** 实体字段 i18n */
const pi = useEmployeeAddressI18n()

import type { EmployeeAddressCreate } from '@/types/human-resource/personnel/employeeAddress.types'
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
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","addressType","country","province","city","district","address1","address2"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EmployeeAddressCreate & { employeeAddressId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterRow: null,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  country: "CN"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 employeeAddressId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.employeeAddressId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    if (!props.formData?.employeeAddressId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  addressType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('addressType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('addressType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  country: [
    {
      required: true,
      message: pi.ph('country'),
      trigger: 'change'
    }
  ],
  province: [
    {
      required: true,
      message: pi.ph('province'),
      trigger: 'change'
    }
  ],
  city: [
    {
      required: true,
      message: pi.ph('city'),
      trigger: 'change'
    }
  ],
  district: [
    {
      required: true,
      message: pi.ph('district'),
      trigger: 'change'
    }
  ],
  address1: [
    {
      required: true,
      message: pi.ph('address1'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 employeeId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('addressType' in payload) {
    const rawaddressType = payload.addressType
    if (rawaddressType === undefined || rawaddressType === null || rawaddressType === '') {
      delete payload.addressType
    } else {
      const numaddressType = typeof rawaddressType === 'number' ? rawaddressType : Number(rawaddressType)
      if (Number.isFinite(numaddressType)) payload.addressType = numaddressType
      else delete payload.addressType
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.employeeAddressId) {
    payload.employeeAddressId = props.formData.employeeAddressId
  }
  payload.employeeId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.employeeCode ?? masterRow.EmployeeCode
    const masterName = masterRow.employeeName ?? masterRow.EmployeeName
    if (masterCode != null && masterCode !== '' && !payload.employeeCode) {
      payload.employeeCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.employeeName) {
      payload.employeeName = masterName
    }
    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.employeeAddressId)
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
