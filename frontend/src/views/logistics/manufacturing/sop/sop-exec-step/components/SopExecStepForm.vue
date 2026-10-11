<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-exec-step/components -->
<!-- 文件名称：SopExecStepForm.vue -->
<!-- 功能描述：SOP 工位执行追溯实体子表 sopExecStep 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form sop-exec-step-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="sop-exec-step-form-tabs"
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
                :label="t('entity.sopexecstep.execid')"
                name="execId"
              >
                <a-input
                  v-model:value="formState.execId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.execid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.stepid')"
                name="stepId"
              >
                <a-input
                  v-model:value="formState.stepId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.stepid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.stepno')"
                name="stepNo"
              >
                <a-input-number
                  v-model:value="formState.stepNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.stepno') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.startedat')"
                name="startedAt"
              >
                <a-input
                  v-model:value="formState.startedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.startedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.endedat')"
                name="endedAt"
              >
                <a-input
                  v-model:value="formState.endedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.endedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.stepresult')"
                name="stepResult"
              >
                <TaktSelect
                  v-model:value="formState.stepResult"
                  dict-type="logistics_manufacturing_sop_check_result"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexecstep.stepresult') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.confirmedby')"
                name="confirmedBy"
              >
                <a-input
                  v-model:value="formState.confirmedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.confirmedby') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexecstep.confirmedat')"
                name="confirmedAt"
              >
                <a-input
                  v-model:value="formState.confirmedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.confirmedat') })"
                  show-count
                  :maxlength="20"
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
 * SOP 工位执行追溯实体子表 sopExecStep 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/sop/sop-exec-step/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SopExecStepCreate } from '@/types/logistics/manufacturing/sop/sopExecStep.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["execId","stepId","stepNo","startedAt","endedAt","stepResult","confirmedBy","confirmedAt"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SopExecStepCreate & { sopExecStepId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  stepResult: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 sopExecStepId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sopExecStepId) {
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
  execId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.execid') }),
      trigger: 'blur'
    }
  ],
  stepId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.stepid') }),
      trigger: 'blur'
    }
  ],
  stepNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexecstep.stepno') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexecstep.stepno') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startedAt: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.startedat') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 sopExecId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('stepNo' in payload) {
    const rawstepNo = payload.stepNo
    payload.stepNo = typeof rawstepNo === 'number' ? rawstepNo : Number(rawstepNo)
  }
  if ('stepResult' in payload) {
    const rawstepResult = payload.stepResult
    payload.stepResult = typeof rawstepResult === 'number' ? rawstepResult : Number(rawstepResult)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.sopExecId = props.masterId
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
