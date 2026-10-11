<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/todo/components -->
<!-- 文件名称：FlowAddSignForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：待办加签表单组件，选择加签人、加签类型（顺序/会签/或签）及是否回退到当前节点（由父级 TaktModal 包裹） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="flow-todo-modal-form"
    layout="vertical"
    :model="form"
    :rules="formRules"
  >
    <a-form-item
      :label="t('workflow.todo.page.add.sign.approvers.label')"
      name="approverIds"
      required
    >
      <a-select
        v-model:value="form.approverIds"
        mode="multiple"
        :placeholder="t('workflow.todo.page.add.sign.approvers.placeholder')"
        show-search
        :filter-option="filterUserOption"
        :options="userOptions"
        :field-names="{ label: 'dictLabel', value: 'dictValue' }"
        allow-clear
        style="width: 100%"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.flowaddsign.signtype')"
      name="approveType"
    >
      <TaktSelect
        v-model="form.approveType"
        dict-type="sys_flow_add_sign_type"
        style="width: 100%"
        :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowaddsign.signtype') })"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.flowaddsign.reason')"
      name="reason"
    >
      <a-textarea
        v-model:value="form.reason"
        :rows="2"
      />
    </a-form-item>
    <a-form-item name="returnToSignNode">
      <a-checkbox v-model:checked="form.returnToSignNode">
        {{ t('entity.flowaddsign.returntosignnode') }}
      </a-checkbox>
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 加签表单：选择加签人、加签类型（顺序/会签/或签）、原因、是否回退到当前节点；对外暴露 validate。
 */
import { useI18n } from 'vue-i18n'
import type { TaktSelectOption } from '@/types/common.types'

const { t } = useI18n()

/** 加签表单数据：加签人 ID 列表、类型、原因、是否回退 */
export interface AddSignFormModel {
  approverIds: string[]
  approveType: string
  reason: string
  returnToSignNode: boolean
}

/** 父组件传入的表单绑定与用户下拉选项 */
interface Props {
  form: AddSignFormModel
  userOptions: TaktSelectOption[]
}

const props = defineProps<Props>()
const form = props.form
const formRef = ref()

const formRules = computed(() => ({
  approverIds: [
    { required: true, type: 'array' as const, min: 1, message: t('workflow.todo.page.add.sign.approvers.placeholder') }
  ],
  approveType: [],
  reason: [],
  returnToSignNode: []
}))

/** 加签人下拉的 filter-option：按 dictLabel 包含输入（不区分大小写） */
function filterUserOption(input: string, option: unknown) {
  const o = option as TaktSelectOption
  const label = (o?.dictLabel ?? '').toString().toLowerCase()
  return label.includes((input || '').toLowerCase())
}

/** 校验加签表单，通过返回 true（供父组件提交前调用） */
async function validate(): Promise<boolean> {
  try {
    await formRef.value?.validate()
    return true
  } catch {
    return false
  }
}

defineExpose({
  validate
})
</script>

<style scoped lang="css">
.flow-todo-modal-form {
  width: 100%;
  max-width: 100%;
  box-sizing: border-box;
}
</style>
