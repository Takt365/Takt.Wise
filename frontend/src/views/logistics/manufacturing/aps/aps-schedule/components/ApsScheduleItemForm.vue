<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/components -->
<!-- 文件名称：ApsScheduleItemForm.vue -->
<!-- 功能描述：APS排程主表子表 apsScheduleItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form aps-schedule-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="aps-schedule-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.apsorderid')"
                name="apsOrderId"
              >
                <a-input
                  v-model:value="formState.apsOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.apsorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.apsoperationid')"
                name="apsOperationId"
              >
                <a-input
                  v-model:value="formState.apsOperationId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.apsoperationid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.routingitemid')"
                name="routingItemId"
              >
                <a-input
                  v-model:value="formState.routingItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.routingitemid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.workordercode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.workordercode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.apsScheduleItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.productcode')"
                name="productCode"
              >
                <a-input
                  v-model:value="formState.productCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.productcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.apsScheduleItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.productname')"
                name="productName"
              >
                <a-input
                  v-model:value="formState.productName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.productname') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsscheduleitem.workcentercode')"
                name="workCenterCode"
              >
                <a-input
                  v-model:value="formState.workCenterCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.workcentercode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.apsScheduleItemId"
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
 * APS排程主表子表 apsScheduleItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/aps/aps-schedule/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ApsScheduleItemCreate } from '@/types/logistics/manufacturing/aps/apsScheduleItem.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["apsOrderId","apsOperationId","routingItemId","lineNumber","workOrderCode","productCode","productName","workCenterCode"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsScheduleItemCreate & { apsScheduleItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 apsScheduleItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.apsScheduleItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.workordercode') }),
      trigger: 'blur'
    }
  ],
  productCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.productcode') }),
      trigger: 'blur'
    }
  ],
  productName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.productname') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 apsScheduleId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.apsScheduleId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
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
