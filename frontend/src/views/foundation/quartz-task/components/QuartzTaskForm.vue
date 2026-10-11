<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task/components -->
<!-- 文件名称：QuartzTaskForm.vue -->
<!-- 功能描述：Quartz 定时任务实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quartz-task-form-tabs"
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
                :label="pi.label('taskCode')"
                name="taskCode"
              >
                <a-input
                  v-model:value="formState.taskCode"
                  :placeholder="pi.ph('taskCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.quartzTaskId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taskName')"
                name="taskName"
              >
                <a-input
                  v-model:value="formState.taskName"
                  :placeholder="pi.ph('taskName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('jobName')"
                name="jobName"
              >
                <a-input
                  v-model:value="formState.jobName"
                  :placeholder="pi.ph('jobName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('jobGroup')"
                name="jobGroup"
              >
                <TaktSelect
                  v-model:value="formState.jobGroup"
                  dict-type="sys_quartz_job_group"
                  :placeholder="pi.ph('jobGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taskType')"
                name="taskType"
              >
                <TaktSelect
                  v-model:value="formState.taskType"
                  dict-type="sys_quartz_task_type"
                  :placeholder="pi.ph('taskType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assemblyName')"
                name="assemblyName"
              >
                <a-input
                  v-model:value="formState.assemblyName"
                  :placeholder="pi.ph('assemblyName')"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('className')"
                name="className"
              >
                <a-input
                  v-model:value="formState.className"
                  :placeholder="pi.ph('className')"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('apiUrl')"
                name="apiUrl"
              >
                <a-input
                  v-model:value="formState.apiUrl"
                  :placeholder="pi.ph('apiUrl')"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestMethod')"
                name="requestMethod"
              >
                <a-input
                  v-model:value="formState.requestMethod"
                  :placeholder="pi.ph('requestMethod')"
                  show-count
                  :maxlength="10"
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
                :label="pi.label('sqlScript')"
                name="sqlScript"
              >
                <a-input
                  v-model:value="formState.sqlScript"
                  :placeholder="pi.ph('sqlScript')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('triggerType')"
                name="triggerType"
              >
                <TaktSelect
                  v-model:value="formState.triggerType"
                  dict-type="sys_quartz_trigger_type"
                  :placeholder="pi.ph('triggerType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cronExpression')"
                name="cronExpression"
              >
                <a-input
                  v-model:value="formState.cronExpression"
                  :placeholder="pi.ph('cronExpression')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('intervalSeconds')"
                name="intervalSeconds"
              >
                <a-input-number
                  v-model:value="formState.intervalSeconds"
                  :placeholder="pi.ph('intervalSeconds')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('executeParams')"
                name="executeParams"
              >
                <a-input
                  v-model:value="formState.executeParams"
                  :placeholder="pi.ph('executeParams')"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('concurrent')"
                name="concurrent"
              >
                <TaktSelect
                  v-model:value="formState.concurrent"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('concurrent')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('misfirePolicy')"
                name="misfirePolicy"
              >
                <TaktSelect
                  v-model:value="formState.misfirePolicy"
                  dict-type="sys_quartz_misfire_policy"
                  :placeholder="pi.ph('misfirePolicy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('firstRunAt')"
                name="firstRunAt"
              >
                <a-date-picker
                  v-model:value="formState.firstRunAt"
                  :placeholder="pi.ph('firstRunAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('executeCount')"
                name="executeCount"
              >
                <a-input-number
                  v-model:value="formState.executeCount"
                  :placeholder="pi.ph('executeCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lastRunAt')"
                name="lastRunAt"
              >
                <a-date-picker
                  v-model:value="formState.lastRunAt"
                  :placeholder="pi.ph('lastRunAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('nextRunAt')"
                name="nextRunAt"
              >
                <a-date-picker
                  v-model:value="formState.nextRunAt"
                  :placeholder="pi.ph('nextRunAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taskDescription')"
                name="taskDescription"
              >
                <a-textarea
                  v-model:value="formState.taskDescription"
                  :placeholder="pi.ph('taskDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taskStatus')"
                name="taskStatus"
              >
                <TaktSelect
                  v-model:value="formState.taskStatus"
                  dict-type="sys_quartz_task_status"
                  :placeholder="pi.ph('taskStatus')"
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
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="pi.ph('cultureCode')"
                  show-count
                  :maxlength="20"
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
 * Quartz 定时任务实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/quartz-task/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useQuartzTaskI18n } from '../composables/useQuartzTaskI18n'

/** 实体字段 i18n */
const pi = useQuartzTaskI18n()
import type { QuartzTaskCreate } from '@/types/foundation/quartzTask.types'
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
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QuartzTaskCreate & { quartzTaskId?: string }> | null
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
  jobGroup: "DEFAULT",
  taskType: "ASSEMBLY",
  triggerType: 1,
  misfirePolicy: 0,
  taskStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 quartzTaskId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.quartzTaskId) {
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
    if (!props.formData?.quartzTaskId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  taskCode: [
    {
      required: true,
      message: pi.ph('taskCode'),
      trigger: 'blur'
    }
  ],
  taskName: [
    {
      required: true,
      message: pi.ph('taskName'),
      trigger: 'blur'
    }
  ],
  jobName: [
    {
      required: true,
      message: pi.ph('jobName'),
      trigger: 'blur'
    }
  ],
  jobGroup: [
    {
      required: true,
      message: pi.ph('jobGroup'),
      trigger: 'change'
    }
  ],
  taskType: [
    {
      required: true,
      message: pi.ph('taskType'),
      trigger: 'change'
    }
  ],
  assemblyName: [
    {
      required: true,
      message: pi.ph('assemblyName'),
      trigger: 'blur'
    }
  ],
  className: [
    {
      required: true,
      message: pi.ph('className'),
      trigger: 'blur'
    }
  ],
  triggerType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('triggerType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('triggerType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  cronExpression: [
    {
      required: true,
      message: pi.ph('cronExpression'),
      trigger: 'blur'
    }
  ],
  intervalSeconds: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('intervalSeconds'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('intervalSeconds'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  concurrent: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('concurrent'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('concurrent'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  misfirePolicy: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('misfirePolicy'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('misfirePolicy'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  executeCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('executeCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('executeCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taskStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taskStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taskStatus'))
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
  if ('triggerType' in payload) {
    const rawtriggerType = payload.triggerType
    payload.triggerType = typeof rawtriggerType === 'number' ? rawtriggerType : Number(rawtriggerType)
  }
  if ('intervalSeconds' in payload) {
    const rawintervalSeconds = payload.intervalSeconds
    payload.intervalSeconds = typeof rawintervalSeconds === 'number' ? rawintervalSeconds : Number(rawintervalSeconds)
  }
  if ('concurrent' in payload) {
    const rawconcurrent = payload.concurrent
    payload.concurrent = typeof rawconcurrent === 'number' ? rawconcurrent : Number(rawconcurrent)
  }
  if ('misfirePolicy' in payload) {
    const rawmisfirePolicy = payload.misfirePolicy
    payload.misfirePolicy = typeof rawmisfirePolicy === 'number' ? rawmisfirePolicy : Number(rawmisfirePolicy)
  }
  if ('executeCount' in payload) {
    const rawexecuteCount = payload.executeCount
    payload.executeCount = typeof rawexecuteCount === 'number' ? rawexecuteCount : Number(rawexecuteCount)
  }
  if ('taskStatus' in payload) {
    const rawtaskStatus = payload.taskStatus
    payload.taskStatus = typeof rawtaskStatus === 'number' ? rawtaskStatus : Number(rawtaskStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.quartzTaskId)

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
