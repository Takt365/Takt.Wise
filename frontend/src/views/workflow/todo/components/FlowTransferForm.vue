<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/todo/components -->
<!-- 文件名称：FlowTransferForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：待办转办表单组件，选择转办对象与转办意见（由父级 TaktModal 包裹） -->
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
      :label="t('workflow.todo.page.transfer.to.user.label')"
      name="toUserId"
      required
    >
      <a-select
        v-model:value="form.toUserId"
        :placeholder="t('workflow.todo.page.transfer.to.user.placeholder')"
        show-search
        :filter-option="filterUserOption"
        :options="userOptions"
        :field-names="{ label: 'dictLabel', value: 'dictValue' }"
        allow-clear
        style="width: 100%"
        @change="onUserChange"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.flowtask.comment')"
      name="comment"
    >
      <a-textarea
        v-model:value="commentModel"
        :rows="2"
      />
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 转办表单：选择转办对象、转办意见；对外暴露 validate。
 */
import { useI18n } from 'vue-i18n'
import type { SelectValue } from 'ant-design-vue/es/select'
import type { TaktSelectOption } from '@/types/common.types'

const { t } = useI18n()

/** 转办表单数据：转办目标用户、意见 */
export interface TransferFormModel {
  toUserId?: string
  toUserName: string
  comment?: string
}

/** 父组件传入的表单绑定与用户下拉选项 */
interface Props {
  form: TransferFormModel
  userOptions: TaktSelectOption[]
}

const props = defineProps<Props>()
const form = props.form
const formRef = ref()
const commentModel = computed<string>({
  get: () => form.comment ?? '',
  set: (value) => { form.comment = value }
})

const formRules = computed(() => ({
  toUserId: [
    { required: true, message: t('workflow.todo.page.transfer.to.user.placeholder') }
  ],
  comment: []
}))

/** 转办用户下拉的 filter-option：按 dictLabel 包含输入（不区分大小写） */
function filterUserOption(input: string, option: unknown) {
  const o = option as TaktSelectOption
  const label = (o?.dictLabel ?? '').toString().toLowerCase()
  return label.includes((input || '').toLowerCase())
}

/** 转办用户选择变化时同步 toUserName */
function onUserChange(value: SelectValue) {
  const selecmed = value == null || Array.isArray(value) || typeof value === 'object'
    ? ''
    : String(value)
  const opt = props.userOptions.find(o => String(o.dictValue) === selecmed)
  form.toUserName = opt?.dictLabel ?? ''
}

/** 校验转办表单，通过返回 true（供父组件提交前调用） */
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
