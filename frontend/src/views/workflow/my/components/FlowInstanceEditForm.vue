<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/my/components -->
<!-- 文件名称：FlowInstanceEditForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：我的流程中流程实例编辑表单，可修改流程标题与表单数据（运行中且发起人，由父级 TaktModal 包裹） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    layout="vertical"
    :model="form"
    :rules="formRules"
  >
    <a-form-item
      :label="t('entity.flowinstance.processtitle')"
      name="processTitle"
    >
      <a-input
        v-model:value="form.processTitle"
        :placeholder="t('common.page.form.placeholder.required', { field: t('entity.flowinstance.processtitle') })"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.flowinstance.frmdata')"
      name="frmData"
    >
      <a-textarea
        v-model:value="form.frmData"
        :rows="6"
        :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.flowinstance.frmdata') })"
      />
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 我的流程·实例编辑表单：流程标题、表单数据（JSON 文本）；对外暴露 validate、getFormData。
 */
import { useI18n } from 'vue-i18n'

/** 实例编辑表单数据：仅可改标题与 frmData */
export interface FlowInstanceEditFormModel {
  processTitle: string
  frmData: string
}

/** 父组件传入的表单绑定 */
interface Props {
  form: FlowInstanceEditFormModel
}

const props = defineProps<Props>()
const form = props.form
const formRef = ref()
const { t } = useI18n()

const formRules = computed(() => ({
  processTitle: [],
  frmData: []
}))

/** 校验编辑表单，通过返回 true（供父组件提交前调用） */
async function validate(): Promise<boolean> {
  try {
    await formRef.value?.validate()
    return true
  } catch {
    return false
  }
}

defineExpose({ validate })
</script>
