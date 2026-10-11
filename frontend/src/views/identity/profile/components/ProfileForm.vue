<!-- ========================================
项目名称:TaktWise
命名空间:@/views/identity/profile/components
文件名称:ProfileForm.vue
功能描述:个人中心可编辑资料；仅昵称→用户表，手机/邮箱→员工表；姓名与性别只读不在此表单
版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="formRules"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="24">
        <a-form-item
          :label="t('entity.user.nickname')"
          name="nickName"
        >
          <a-input
            v-model:value="formState.nickName"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.user.nickname') })"
            :disabled="loading"
            :maxlength="USER_NICK_NAME_MAX_LENGTH"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="hasEmployee"
        :span="24"
      >
        <a-form-item
          :label="t('entity.employee.mobile')"
          name="mobile"
        >
          <a-input
            v-model:value="formState.mobile"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.mobile') })"
            :disabled="loading"
            :maxlength="11"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="hasEmployee"
        :span="24"
      >
        <a-form-item
          :label="t('entity.employee.email')"
          name="email"
        >
          <a-input
            v-model:value="formState.email"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.email') })"
            :disabled="loading"
            :maxlength="100"
            allow-clear
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 个人资料表单：仅可改昵称（TaktUser）与手机/邮箱（TaktEmployee）
 */
import { useI18n } from 'vue-i18n'
import type { FormInstance, Rule } from 'ant-design-vue/es/form'
import type { UpdateMyProfile } from '@/types/identity/user.types'
import type { UserInfoResponse } from '@/types/identity/login.types'
import {
  isValidUserNickName,
  USER_NICK_NAME_MIN_LENGTH,
  USER_NICK_NAME_MAX_LENGTH,
} from '@/utils/regex'

const props = withDefaults(
  defineProps<{
    /** 当前 /me 资料；灌入表单 */
    formData?: UserInfoResponse | null
    /** 提交中禁用控件 */
    loading?: boolean
  }>(),
  {
    formData: null,
    loading: false,
  },
)

const { t } = useI18n()

/** 表单实例 */
const formRef = ref<FormInstance>()

/** 是否已绑定员工（决定是否维护手机/邮箱） */
const hasEmployee = computed(() => {
  const id = props.formData?.employeeId
  if (id == null || id === '') {
    return false
  }
  const n = Number(id)
  return Number.isFinite(n) && n > 0
})

/** 表单状态 */
const formState = reactive({
  nickName: '',
  mobile: '',
  email: '',
})

/** 校验规则 */
const formRules = computed<Record<string, Rule[]>>(() => {
  const rules: Record<string, Rule[]> = {
    nickName: [
      {
        required: true,
        whitespace: true,
        message: t('common.validation.required', { field: t('entity.user.nickname') }),
        trigger: 'blur',
      },
      {
        validator: async (_rule, value) => {
          if (!isValidUserNickName(String(value ?? ''))) {
            return Promise.reject(
              t('common.validation.length.between', {
                field: t('entity.user.nickname'),
                min: USER_NICK_NAME_MIN_LENGTH,
                max: USER_NICK_NAME_MAX_LENGTH,
              }),
            )
          }
          return Promise.resolve()
        },
        trigger: 'blur',
      },
    ],
  }
  if (hasEmployee.value) {
    rules.mobile = [
      {
        required: true,
        whitespace: true,
        message: t('common.validation.required', { field: t('entity.employee.mobile') }),
        trigger: 'blur',
      },
      {
        max: 11,
        message: t('common.validation.length.between', {
          field: t('entity.employee.mobile'),
          min: 1,
          max: 11,
        }),
        trigger: 'blur',
      },
    ]
    rules.email = [
      {
        required: true,
        whitespace: true,
        message: t('common.validation.required', { field: t('entity.employee.email') }),
        trigger: 'blur',
      },
      {
        type: 'email',
        message: t('common.validation.format.invalid', {
          field: t('entity.employee.email'),
        }),
        trigger: 'blur',
      },
      {
        max: 100,
        message: t('common.validation.length.between', {
          field: t('entity.employee.email'),
          min: 1,
          max: 100,
        }),
        trigger: 'blur',
      },
    ]
  }
  return rules
})

/**
 * 从 formData 灌入表单
 */
function applyFormData(): void {
  const data = props.formData
  formState.nickName = data?.nickName?.trim() || ''
  formState.mobile = data?.mobile?.trim() || ''
  formState.email = data?.email?.trim() || ''
}

watch(
  () => props.formData,
  () => {
    applyFormData()
  },
  { immediate: true, deep: true },
)

/**
 * 校验表单
 * @returns {Promise<void>}
 */
async function validate(): Promise<void> {
  await formRef.value?.validate()
}

/**
 * 取提交 DTO（仅昵称 / 手机 / 邮箱）
 * @returns {UpdateMyProfile}
 */
function getValues(): UpdateMyProfile {
  const dto: UpdateMyProfile = {
    nickName: formState.nickName.trim(),
  }
  if (hasEmployee.value) {
    dto.mobile = formState.mobile.trim()
    dto.email = formState.email.trim()
  }
  return dto
}

/**
 * 重置为当前 formData
 */
function resetFields(): void {
  applyFormData()
  formRef.value?.clearValidate()
}

defineExpose({
  validate,
  getValues,
  resetFields,
})
</script>
