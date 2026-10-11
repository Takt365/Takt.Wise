<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/notification/components -->
<!-- 文件名称：NotificationForm.vue -->
<!-- 功能描述：维护通知单实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="notification-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
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
                :label="t('entity.maintenancenotification.notificationcode')"
                name="notificationCode"
              >
                <a-input
                  v-model:value="formState.notificationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceNotificationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.equipmentid')"
                name="equipmentId"
              >
                <a-input
                  v-model:value="formState.equipmentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.equipmentid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.EquipCode')"
                name="EquipCode"
              >
                <a-input
                  v-model:value="formState.EquipCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.EquipCode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceNotificationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.equipmentname')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.equipmentname') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.maintenancecategory')"
                name="maintenanceCategory"
              >
                <TaktSelect
                  v-model:value="formState.maintenanceCategory"
                  dict-type="logistics_maintenance_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.maintenancecategory') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.priority') })"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.notificationstatus')"
                name="notificationStatus"
              >
                <a-input-number
                  v-model:value="formState.notificationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenancenotification.faultdescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenancenotification.faultdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.discoveredat')"
                name="discoveredAt"
              >
                <a-date-picker
                  v-model:value="formState.discoveredAt"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.discoveredat') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.breakdownstarttime')"
                name="breakdownStartTime"
              >
                <a-date-picker
                  v-model:value="formState.breakdownStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.breakdownendtime')"
                name="breakdownEndTime"
              >
                <a-date-picker
                  v-model:value="formState.breakdownEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownendtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.reportedby')"
                name="reportedBy"
              >
                <a-input
                  v-model:value="formState.reportedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.reportedby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.costcenterid')"
                name="costCenterId"
              >
                <a-input
                  v-model:value="formState.costCenterId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.costcenterid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.costcentercode')"
                name="costCenterCode"
              >
                <a-input
                  v-model:value="formState.costCenterCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.costcentercode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.maintenanceNotificationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.maintenanceworkorderid')"
                name="maintenanceWorkOrderId"
              >
                <a-input
                  v-model:value="formState.maintenanceWorkOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.maintenanceworkorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenancenotification.maintenanceworkordercode')"
                name="maintenanceWorkOrderCode"
              >
                <a-input
                  v-model:value="formState.maintenanceWorkOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.maintenanceworkordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceNotificationId"
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
                :label="t('entity.maintenancenotification.notificationimages')"
                name="notificationImages"
              >
                <a-input
                  v-model:value="formState.notificationImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationimages') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 维护通知单实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/notification/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaintenanceNotificationCreate } from '@/types/logistics/maintenance/notification.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","notificationCode","equipmentId","EquipCode","equipmentName","maintenanceCategory","priority","notificationStatus","faultDescription","discoveredAt","breakdownStartTime","breakdownEndTime","reportedBy","costCenterId","costCenterCode","maintenanceWorkOrderId","maintenanceWorkOrderCode","notificationImages","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaintenanceNotificationCreate & { maintenanceNotificationId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 maintenanceNotificationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.maintenanceNotificationId) {
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
    const isCreate = !props.formData?.maintenanceNotificationId
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
  notificationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationcode') }),
      trigger: 'blur'
    }
  ],
  equipmentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.equipmentid') }),
      trigger: 'blur'
    }
  ],
  EquipCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.EquipCode') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.equipmentname') }),
      trigger: 'blur'
    }
  ],
  maintenanceCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.maintenancecategory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.maintenancecategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.priority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.priority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  notificationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.notificationstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.notificationstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  faultDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.faultdescription') }),
      trigger: 'blur'
    }
  ],
  discoveredAt: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.discoveredat') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('maintenanceCategory' in payload) {
    const rawmaintenanceCategory = payload.maintenanceCategory
    payload.maintenanceCategory = typeof rawmaintenanceCategory === 'number' ? rawmaintenanceCategory : Number(rawmaintenanceCategory)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('notificationStatus' in payload) {
    const rawnotificationStatus = payload.notificationStatus
    payload.notificationStatus = typeof rawnotificationStatus === 'number' ? rawnotificationStatus : Number(rawnotificationStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.maintenanceNotificationId)

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
