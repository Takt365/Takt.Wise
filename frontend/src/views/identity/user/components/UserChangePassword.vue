<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/user/components -->
<!-- 文件名称：UserChangePassword.vue -->
<!-- 功能描述：修改密码弹窗内嵌表单。由 user/index.vue 引用；defineExpose 提供 validate、getValues、resetFields；表单模型为 ChangePassword；旧/新/确认密码校验含 isValidPassword；loading 结束时 watch 重置表单。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <!-- 旧密码、新密码、确认密码；用户名为只读展示 -->
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    :label-col="{ span: 6 }"
    :wrapper-col="{ span: 18 }"
    layout="horizontal"
  >
    <a-form-item :label="t('entity.user.name')">
      <a-input
        :value="userName"
        disabled
      />
    </a-form-item>
    <a-form-item
      :label="t('identity.user.page.password.old.label')"
      name="oldPassword"
    >
      <a-input-password
        v-model:value="formState.oldPassword"
        :placeholder="t('common.page.form.placeholder.required', { field: t('identity.user.page.password.old.label') })"
        :disabled="loading"
        show-count
        :maxlength="20"
      />
    </a-form-item>
    <a-form-item
      :label="t('identity.user.page.password.new.label')"
      name="newPassword"
    >
      <a-input-password
        v-model:value="formState.newPassword"
        :placeholder="t('common.page.form.placeholder.required', { field: t('identity.user.page.password.new.label') })"
        :disabled="loading"
        show-count
        :maxlength="20"
      />
    </a-form-item>
    <a-form-item
      :label="t('identity.user.page.password.confirm.label')"
      name="confirmPassword"
    >
      <a-input-password
        v-model:value="formState.confirmPassword"
        :placeholder="t('common.page.form.placeholder.requiredagain', { field: t('identity.user.page.password.new.label') })"
        :disabled="loading"
        show-count
        :maxlength="20"
      />
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 修改密码表单脚本：与 `user/index.vue` 弹窗 ref 配合；
 * 父组件通过 `validate`、`getValues`、`resetFields` 完成提交流程。
 */
import { useI18n } from 'vue-i18n'
import type { FormInstance } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { ChangePassword } from '@/types/identity/user.types'
import { isValidPassword } from '@/utils/regex'

/**
 * 组件入参：嵌入用户管理页「修改密码」弹窗。
 */
interface Props {
  /** 当前用户登录名（只读展示） */
  userName: string
  /** 提交请求进行中时禁用输入 */
  loading?: boolean
}

const props = defineProps<Props>()

const emit = defineEmits<{
  submit: [values: ChangePassword]
}>()

const { t } = useI18n()

/** Ant Design Vue 表单实例 */
const formRef = ref<FormInstance>()

function createEmptyChangePasswordForm(): ChangePassword {
  return {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  }
}

/** a-form 绑定模型 */
const formState = reactive<ChangePassword>(createEmptyChangePasswordForm())

/** 确认密码：必填且须与 newPassword 一致 */
const validateConfirmPassword = (_rule: Rule, value: string) => {
  if (!value) {
    return Promise.reject(t('common.page.form.placeholder.requiredagain', { field: t('identity.user.page.password.new.label') }))
  }
  if (value !== formState.newPassword) {
    return Promise.reject(t('identity.user.page.password.confirm.validation.mismatch'))
  }
  return Promise.resolve()
}

/** 校验规则，name 与 a-form-item 一致 */
const rules: Record<string, Rule[]> = {
  // oldPassword：必填
  oldPassword: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('identity.user.page.password.old.label') }), trigger: 'blur' }
  ],
  // newPassword：必填 + isValidPassword
  newPassword: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('identity.user.page.password.new.label') }), trigger: 'blur' },
    {
      validator: (_rule: Rule, value: string) => {
        if (!value) {
          return Promise.resolve()
        }
        if (!isValidPassword(value)) {
          return Promise.reject(t('identity.user.page.password.new.validation.format'))
        }
        return Promise.resolve()
      },
      trigger: 'blur'
    }
  ],
  // confirmPassword：必填 + 与 newPassword 一致
  confirmPassword: [
    { required: true, message: t('common.page.form.placeholder.requiredagain', { field: t('identity.user.page.password.new.label') }), trigger: 'blur' },
    { validator: validateConfirmPassword, trigger: 'blur' }
  ]
}

/** 执行 a-form 校验；父组件提交前 await */
const validate = async (): Promise<void> => {
  if (!formRef.value) {
    return Promise.reject(new Error(t('validation.formRefNotFound', { target: t('identity.user.page.fields.formref.label') })))
  }
  await formRef.value.validate()
}

/** 返回后端 ChangePassword DTO */
const getValues = (): ChangePassword => {
  return {
    oldPassword: formState.oldPassword,
    newPassword: formState.newPassword,
    confirmPassword: formState.confirmPassword
  }
}

/** 清空校验与本地字段 */
const resetFields = () => {
  formRef.value?.resetFields()
  Object.assign(formState, createEmptyChangePasswordForm())
}

/** 监听 loading：由 true→false 时视为一次提交结束，重置表单避免残留 */
watch(
  () => props.loading,
  (newVal, oldVal) => {
    if (oldVal && !newVal) {
      resetFields()
    }
  }
)

/** 暴露给 `user/index.vue` 弹窗 ref：`validate`、`getValues`、`resetFields` */
defineExpose({
  validate,
  getValues,
  resetFields
})
</script>

<style scoped lang="css">
/* 本组件无局部样式；占位与项目其他表单子组件一致 */
</style>
