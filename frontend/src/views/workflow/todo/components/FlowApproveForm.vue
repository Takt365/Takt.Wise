<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/todo/components -->
<!-- 文件名称：FlowApproveForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：待办审批表单组件，审批意见、通过/驳回、驳回节点（由父级 TaktModal 包裹） -->
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
      :label="t('entity.flowtask.comment')"
      name="comment"
    >
      <a-textarea
        v-model:value="form.comment"
        :rows="3"
        :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.flowtask.comment') })"
      />
    </a-form-item>
    <a-form-item
      :label="t('workflow.todo.page.approve.result')"
      name="approved"
    >
      <a-radio-group v-model:value="form.approved">
        <a-radio :value="true">
          {{ t('common.page.button.pass') }}
        </a-radio>
        <a-radio :value="false">
          {{ t('common.page.button.reject') }}
        </a-radio>
      </a-radio-group>
    </a-form-item>
    <a-form-item
      v-if="!form.approved"
      :label="t('workflow.todo.page.node.reject.step.label')"
      name="nodeRejectStep"
    >
      <a-input
        v-model:value="form.nodeRejectStep"
        :placeholder="t('workflow.todo.page.node.reject.step.placeholder')"
        allow-clear
      />
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 待办审批表单：审批意见、通过/驳回、驳回节点；对外暴露 validate()。
 */
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

/** 审批表单数据：意见、是否通过、驳回时目标节点（可选） */
export interface ApproveFormModel {
  comment: string
  approved: boolean
  nodeRejectStep?: string
}

/** 父组件传入的 form 绑定 */
interface Props {
  form: ApproveFormModel
}

const props = defineProps<Props>()
const form = props.form
const formRef = ref()

const formRules = computed(() => ({
  comment: [],
  approved: [],
  nodeRejectStep: []
}))

/** 校验审批表单，通过返回 true，否则 false（供父组件提交前调用） */
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
