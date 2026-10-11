<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/organization/dept/components -->
<!-- 文件名称：DeptForm.vue -->
<!-- 功能描述：部门实体 代表组织架构中的部门树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="dept-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('parentId')"
                name="parentId"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="TaktDepts/tree-options"
                  :lazy="true"
                  :placeholder="pi.ph('parentId')"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
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
                :label="pi.label('deptCode')"
                name="deptCode"
              >
                <a-input
                  v-model:value="formState.deptCode"
                  :placeholder="pi.ph('deptCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptShortName')"
                name="deptShortName"
              >
                <a-input
                  v-model:value="formState.deptShortName"
                  :placeholder="pi.ph('deptShortName')"
                  show-count
                  :maxlength="6"
                  allow-clear
                  @update:value="syncDeptIsoFromShortName"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptName1')"
                name="deptName1"
              >
                <a-input
                  v-model:value="formState.deptName1"
                  :placeholder="pi.ph('deptName1')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptName2')"
                name="deptName2"
              >
                <a-input
                  v-model:value="formState.deptName2"
                  :placeholder="pi.ph('deptName2')"
                  show-count
                  :maxlength="70"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isoCode')"
                name="isoCode"
              >
                <a-input
                  v-model:value="formState.isoCode"
                  :placeholder="pi.ph('isoCode')"
                  show-count
                  :maxlength="6"
                  allow-clear
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costCenterCode')"
                name="costCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.costCenterCode"
                  api-url="TaktCostCenters/tree-options"
                  :placeholder="pi.ph('costCenterCode')"
                  :disabled="!!formData?.deptId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costCategory')"
                name="costCategory"
              >
                <TaktSelect
                  v-model:value="formState.costCategory"
                  dict-type="humanresource_organization_dept_cost_category"
                  :placeholder="pi.ph('costCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('headUserId')"
                name="headUserId"
              >
                <TaktSelect
                  v-model:value="formState.headUserId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('headUserId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('headUserName')"
                name="headUserName"
              >
                <a-input
                  v-model:value="formState.headUserName"
                  :placeholder="pi.ph('headUserName')"
                  show-count
                  :maxlength="40"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('phone')"
                name="phone"
              >
                <a-input
                  v-model:value="formState.phone"
                  :placeholder="pi.ph('phone')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('email')"
                name="email"
              >
                <a-input
                  v-model:value="formState.email"
                  :placeholder="pi.ph('email')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('location')"
                name="location"
              >
                <a-input
                  v-model:value="formState.location"
                  :placeholder="pi.ph('location')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isBuiltIn')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isBuiltIn')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deptDescription')"
                name="deptDescription"
              >
                <a-textarea
                  v-model:value="formState.deptDescription"
                  :placeholder="pi.ph('deptDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deptStatus')"
                name="deptStatus"
              >
                <TaktSelect
                  v-model:value="formState.deptStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('deptStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 部门实体 代表组织架构中的部门维护表单 · 由 generate-vue-tree-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/organization/dept/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useDeptI18n } from '../composables/useDeptI18n'

/** 实体字段 i18n */
const pi = useDeptI18n()

import type { DeptCreate } from '@/types/human-resource/organization/dept.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","deptCode","deptShortName","deptName1","deptName2","isoCode","costCenterCode","costCategory","headUserId","headUserName","phone","email","location","isBuiltIn","deptDescription","deptStatus","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DeptCreate & { deptId?: string }> | null
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
const formState = reactive<Record<string, any>>({ parentId: '0' })
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  isBuiltIn: 0,
  deptStatus: 1
}

/** 部门简称变更时同步 ISO 编码（最多 6 位，与后端一致） */
function syncDeptIsoFromShortName(value: string) {
  const next = (value ?? '').trim().slice(0, 6)
  formState.deptShortName = next
  formState.isoCode = next
}
/** 树表 parentId：空值归一为根节点 0（string，与后端 ParentId=0 一致） */
function normalizeTreeParentId(target: Record<string, unknown>) {
  const raw = target.parentId
  target.parentId = raw === '' || raw === undefined || raw === null ? '0' : String(raw)
}
/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 deptId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.deptId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      normalizeTreeParentId(formState)
      syncDeptIsoFromShortName(String(formState.deptShortName ?? ''))
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
    if (!props.formData?.deptId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  parentId: [
    {
      required: true,
      message: pi.ph('parentId'),
      trigger: 'change'
    }
  ],
  deptCode: [
    {
      required: true,
      message: pi.ph('deptCode'),
      trigger: 'blur'
    }
  ],
  deptShortName: [
    {
      required: true,
      message: pi.ph('deptShortName'),
      trigger: 'blur'
    }
  ],
  deptName1: [
    {
      required: true,
      message: pi.ph('deptName1'),
      trigger: 'blur'
    }
  ],
  deptName2: [
    {
      required: true,
      message: pi.ph('deptName2'),
      trigger: 'blur'
    }
  ],
  isoCode: [
    {
      required: true,
      message: pi.ph('isoCode'),
      trigger: 'blur'
    }
  ],
  costCenterCode: [
    {
      required: true,
      message: pi.ph('costCenterCode'),
      trigger: 'change'
    }
  ],
  costCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('costCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('costCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  headUserId: [
    {
      required: true,
      message: pi.ph('headUserId'),
      trigger: 'change'
    }
  ],
  phone: [
    {
      required: true,
      message: pi.ph('phone'),
      trigger: 'blur'
    }
  ],
  email: [
    {
      required: true,
      message: pi.ph('email'),
      trigger: 'blur'
    }
  ],
  location: [
    {
      required: true,
      message: pi.ph('location'),
      trigger: 'blur'
    }
  ],
  isBuiltIn: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isBuiltIn'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isBuiltIn'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  deptDescription: [
    {
      required: true,
      message: pi.ph('deptDescription'),
      trigger: 'blur'
    }
  ],
  deptStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('deptStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('deptStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('costCategory' in payload) {
    const rawcostCategory = payload.costCategory
    if (rawcostCategory === undefined || rawcostCategory === null || rawcostCategory === '') {
      delete payload.costCategory
    } else {
      const numcostCategory = typeof rawcostCategory === 'number' ? rawcostCategory : Number(rawcostCategory)
      if (Number.isFinite(numcostCategory)) payload.costCategory = numcostCategory
      else delete payload.costCategory
    }
  }
  if ('isBuiltIn' in payload) {
    const rawisBuiltIn = payload.isBuiltIn
    if (rawisBuiltIn === undefined || rawisBuiltIn === null || rawisBuiltIn === '') {
      delete payload.isBuiltIn
    } else {
      const numisBuiltIn = typeof rawisBuiltIn === 'number' ? rawisBuiltIn : Number(rawisBuiltIn)
      if (Number.isFinite(numisBuiltIn)) payload.isBuiltIn = numisBuiltIn
      else delete payload.isBuiltIn
    }
  }
  if ('deptStatus' in payload) {
    const rawdeptStatus = payload.deptStatus
    if (rawdeptStatus === undefined || rawdeptStatus === null || rawdeptStatus === '') {
      delete payload.deptStatus
    } else {
      const numdeptStatus = typeof rawdeptStatus === 'number' ? rawdeptStatus : Number(rawdeptStatus)
      if (Number.isFinite(numdeptStatus)) payload.deptStatus = numdeptStatus
      else delete payload.deptStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.deptId) {
    payload.deptId = props.formData.deptId
  }
  return payload
}

/** 重置表单与子表行 */
/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.deptId)

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
