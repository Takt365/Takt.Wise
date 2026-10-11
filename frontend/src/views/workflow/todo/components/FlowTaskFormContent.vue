<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/todo/components -->
<!-- 文件名称：FlowTaskFormContent.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：待办审批时展示的表单内容（只读），流程标题 + 表单数据（由 formConfig 渲染或 frmData 原文） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flow-task-form-content">
    <template v-if="detail">
      <div
        v-if="detail.processTitle"
        class="flow-task-form-content__title"
      >
        <span class="flow-task-form-content__label">{{ t('entity.flowinstance.processtitle') }}</span>
        <span>{{ detail.processTitle }}</span>
      </div>
      <div class="flow-task-form-content__body">
        <div
          v-if="formConfigLoading"
          class="flow-task-form-content__loading"
        >
          <a-spin />
        </div>
        <template v-else-if="formConfigRule.length">
          <form-create
            :key="formCreateKey"
            v-model:api="formCreateApi"
            name="flowTaskFrmDataReadonly"
            :rule="formConfigRule"
            :option="formCreateOption"
          />
        </template>
        <div
          v-else-if="detail.frmData?.trim()"
          class="flow-task-form-content__raw"
        >
          <pre class="flow-task-form-content__pre">{{ detail.frmData }}</pre>
        </div>
        <div
          v-else
          class="flow-task-form-content__empty"
        >
          {{ t('workflow.my.page.start.flow.form.form.data.label') }}{{ t('workflow.instance.page.form.data.empty') }}
        </div>
      </div>
    </template>
    <div
      v-else
      class="flow-task-form-content__empty"
    >
      {{ t('common.status.empty') }}
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 待办任务表单内容（只读）：引擎详情含 processDefinitionId，经方案 CRUD 取 formConfig 渲染 frmData；否则展示 frmData 原文。
 */
import { useI18n } from 'vue-i18n'
import { getFlowFormById } from '@/api/workflow/flowForm.api'
import { getFlowSchemeById } from '@/api/workflow/flowScheme.api'
import { getEmployeeOptions } from '@/api/human-resource/personnel/employee.api'
import { FORM_CREATE_DEFAULT_OPTION } from '@/utils/constants/formCreate'
import type { FlowInstanceDetail } from '@/types/workflow/flowEngine.types'
import type { FlowForm } from '@/types/workflow/flowForm.types'

const { t } = useI18n()

/** form-create 规则类型 */
type FormConfigRule = Record<string, unknown>[]

/** 父组件传入的引擎实例详情（含 processDefinitionId、frmData 等） */
interface Props {
  detail: FlowInstanceDetail | null
}

const props = defineProps<Props>()
const formConfigRule = ref<FormConfigRule>([])
const formConfigLoading = ref(false)
const formCreateApi = ref<{ setValue: (data: Record<string, unknown>) => void } | null>(null)
const formCreateKey = ref(0)

const formCreateOption = computed(() => ({ ...FORM_CREATE_DEFAULT_OPTION, disabled: true }))

/** 为 field=employeeId / optionsSource=employee 的 select 注入员工下拉选项，便于 form-create 只读时显示姓名 */
async function enrichFormConfigWithEmployeeOptions(rule: FormConfigRule): Promise<FormConfigRule> {
  const copy = JSON.parse(JSON.stringify(rule)) as FormConfigRule
  const needFetch = copy.some((r) => {
    const field = (r as { field?: string }).field
    const src = (r as { props?: { optionsSource?: string } }).props?.optionsSource
    return field === 'employeeId' || src === 'employee'
  })
  if (!needFetch) return copy
  try {
    const list = await getEmployeeOptions()
    const opts = (list ?? []).map((o) => ({
      label: o.dictLabel ?? String(o.dictValue ?? ''),
      value: String(o.dictValue ?? '')
    }))
    for (const r of copy) {
      const field = (r as { field?: string }).field
      const src = (r as { props?: { optionsSource?: string } }).props?.optionsSource
      if (field === 'employeeId' || src === 'employee') {
        const row = r as { props?: Record<string, unknown> }
        row.props = { ...(row.props ?? {}), options: opts }
      }
    }
  } catch {
    // ignore
  }
  return copy
}

/** 根据引擎详情 processDefinitionId → 方案 formId 拉取表单配置 */
async function loadFormConfig() {
  const d = props.detail
  formConfigRule.value = []
  if (!d) return
  formConfigLoading.value = true
  try {
    let flowForm: FlowForm | null = null
    const schemeId = d.processDefinitionId?.trim()
    if (schemeId) {
      try {
        const scheme = await getFlowSchemeById(schemeId)
        if (scheme?.formId) {
          flowForm = await getFlowFormById(String(scheme.formId))
        }
      } catch {
        // ignore
      }
    }
    const configStr = flowForm?.formConfig?.trim()
    if (!configStr) return
    const parsed = JSON.parse(configStr) as FormConfigRule
    if (!Array.isArray(parsed) || !parsed.length) return
    formConfigRule.value = await enrichFormConfigWithEmployeeOptions(parsed)
    formCreateKey.value += 1
  } catch {
    formConfigRule.value = []
  } finally {
    formConfigLoading.value = false
  }
}

watch(
  () => props.detail,
  (d) => {
    if (d) loadFormConfig()
    else formConfigRule.value = []
  },
  { immediate: true }
)

watch(
  [formCreateApi, () => props.detail?.frmData],
  () => {
    const api = formCreateApi.value
    const d = props.detail
    if (api?.setValue && formConfigRule.value.length && d?.frmData?.trim()) {
      try {
        const data = JSON.parse(d.frmData) as Record<string, unknown>
        nextTick(() => api.setValue(data))
      } catch {
        // ignore
      }
    }
  },
  { immediate: true }
)
</script>

<style scoped lang="css">
.flow-task-form-content {
  width: 100%;
  max-width: 100%;
  box-sizing: border-box;
  &__label {
    color: var(--ant-color-text-secondary);
    margin-right: 4px;
  }
  &__title {
    margin-bottom: 12px;
    font-weight: 500;
  }
  &__body {
    background: var(--ant-color-fill-quaternary);
    border-radius: 6px;
    padding: 12px;
    min-height: 60px;
  }
  &__loading {
    display: flex;
    align-items: center;
    justify-content: center;
    min-height: 80px;
  }
  &__raw {
    font-size: 12px;
  }
  &__pre {
    margin: 0;
    white-space: pre-wrap;
    word-break: break-all;
    max-height: 320px;
    overflow: auto;
  }
  &__empty {
    color: var(--ant-color-text-tertiary);
    font-size: 12px;
  }

  :deep(form) {
    width: 100%;
    max-width: 100%;
  }
}
</style>
