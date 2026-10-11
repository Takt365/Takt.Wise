<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/tracking-log/components -->
<!-- 文件名称：TrackingLogForm.vue -->
<!-- 功能描述：前端交互日志实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="tracking-log-form-tabs"
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
                :label="t('entity.trackinglog.userid')"
                name="userId"
              >
                <a-input
                  v-model:value="formState.userId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.userid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.username')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.username') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.eventtrackingtype')"
                name="eventTrackingType"
              >
                <a-input
                  v-model:value="formState.eventTrackingType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.eventtrackingtype') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.eventtrackingcategory')"
                name="eventTrackingCategory"
              >
                <a-input
                  v-model:value="formState.eventTrackingCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.eventtrackingcategory') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.eventtime')"
                name="eventTime"
              >
                <a-date-picker
                  v-model:value="formState.eventTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trackinglog.eventtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.durationms')"
                name="durationMs"
              >
                <a-input-number
                  v-model:value="formState.durationMs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.durationms') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.performancestartms')"
                name="performanceStartMs"
              >
                <a-input-number
                  v-model:value="formState.performanceStartMs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.performancestartms') })"
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
                :label="t('entity.trackinglog.entryname')"
                name="entryName"
              >
                <a-input
                  v-model:value="formState.entryName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.entryname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.trackinglevel')"
                name="trackingLevel"
              >
                <a-input-number
                  v-model:value="formState.trackingLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.trackinglevel') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.routepath')"
                name="routePath"
              >
                <a-input
                  v-model:value="formState.routePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.routepath') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.pageurl')"
                name="pageUrl"
              >
                <a-input
                  v-model:value="formState.pageUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.pageurl') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.containertype')"
                name="containerType"
              >
                <a-input
                  v-model:value="formState.containerType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containertype') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.containername')"
                name="containerName"
              >
                <a-input
                  v-model:value="formState.containerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containername') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.containersrc')"
                name="containerSrc"
              >
                <a-input
                  v-model:value="formState.containerSrc"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containersrc') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.containerid')"
                name="containerId"
              >
                <a-input
                  v-model:value="formState.containerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containerid') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.attributionjson')"
                name="attributionJson"
              >
                <a-input
                  v-model:value="formState.attributionJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.attributionjson') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trackinglog.useragent')"
                name="userAgent"
              >
                <a-input
                  v-model:value="formState.userAgent"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.useragent') })"
                  show-count
                  :maxlength="500"
                  allow-clear
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
                :label="t('entity.trackinglog.clientip')"
                name="clientIp"
              >
                <a-input
                  v-model:value="formState.clientIp"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.clientip') })"
                  show-count
                  :maxlength="50"
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
 * 前端交互日志实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/tracking-log/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TrackingLogCreate } from '@/types/statistics/logging/trackingLog.types'
import { RiQuestionLine } from '@remixicon/vue'
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
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","userId","userName","eventTrackingType","eventTrackingCategory","eventTime","durationMs","performanceStartMs","entryName","trackingLevel","routePath","pageUrl","containerType","containerName","containerSrc","containerId","attributionJson","userAgent","clientIp","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TrackingLogCreate & { trackingLogId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 trackingLogId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.trackingLogId) {
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.trackingLogId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  userName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.username') }),
      trigger: 'blur'
    }
  ],
  userId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.userid') }),
      trigger: 'blur'
    }
  ],
  eventTrackingType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.eventtrackingtype') }),
      trigger: 'blur'
    }
  ],
  eventTrackingCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.eventtrackingcategory') }),
      trigger: 'blur'
    }
  ],
  eventTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trackinglog.eventtime') }),
      trigger: 'change'
    }
  ],
  durationMs: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.trackinglog.durationms') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.trackinglog.durationms') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  performanceStartMs: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.trackinglog.performancestartms') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.trackinglog.performancestartms') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  entryName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.entryname') }),
      trigger: 'blur'
    }
  ],
  trackingLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.trackinglog.trackinglevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.trackinglog.trackinglevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  routePath: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.routepath') }),
      trigger: 'blur'
    }
  ],
  pageUrl: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.pageurl') }),
      trigger: 'blur'
    }
  ],
  containerType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containertype') }),
      trigger: 'blur'
    }
  ],
  containerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containername') }),
      trigger: 'blur'
    }
  ],
  containerSrc: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containersrc') }),
      trigger: 'blur'
    }
  ],
  containerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containerid') }),
      trigger: 'blur'
    }
  ],
  attributionJson: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.attributionjson') }),
      trigger: 'blur'
    }
  ],
  userAgent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.useragent') }),
      trigger: 'blur'
    }
  ],
  clientIp: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trackinglog.clientip') }),
      trigger: 'blur'
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
  if ('durationMs' in payload) {
    const rawdurationMs = payload.durationMs
    payload.durationMs = typeof rawdurationMs === 'number' ? rawdurationMs : Number(rawdurationMs)
  }
  if ('performanceStartMs' in payload) {
    const rawperformanceStartMs = payload.performanceStartMs
    payload.performanceStartMs = typeof rawperformanceStartMs === 'number' ? rawperformanceStartMs : Number(rawperformanceStartMs)
  }
  if ('trackingLevel' in payload) {
    const rawtrackingLevel = payload.trackingLevel
    payload.trackingLevel = typeof rawtrackingLevel === 'number' ? rawtrackingLevel : Number(rawtrackingLevel)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.trackingLogId)

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
