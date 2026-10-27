<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/user/components -->
<!-- 文件名称：UserForm.vue -->
<!-- 功能描述：用户维护弹窗内嵌表单。由 user/index.vue 引用；defineExpose 提供 validate、getValues、resetFields、setServerValidationErrors。员工/用户信息/权限多标签；TaktSelect 字典 sys_user_type、sys_normal_disable；视图模型见 `@/types/identity/userFormView.types`（勿写入自动生成的 user.d.ts）；新增密码由父组件映射为 UserCreate.passwordHash。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <!-- 根：a-form 包裹 a-tabs；formRef 供父级 validate、resetFields -->
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs v-model:active-key="activeTab">
      <!-- 标签1：员工信息（编辑=已绑定员工只读+描述；新增=下拉选员工，可跳转人事员工页建档，变更时带出默认 userName） -->
      <a-tab-pane
        key="employee"
        :tab="t('identity.user.page.tabs.employee.info')"
        force-render
      >
        <div :class="formContentClass">
          <!-- 编辑态 -->
          <template v-if="formData?.userId">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item
                  :label="t('entity.user.employeeid')"
                  name="employeeId"
                >
                  <TaktSelect
                    v-model:value="formState.employeeId"
                    :options="employeeOptions"
                    disabled
                  />
                </a-form-item>
              </a-col>
            </a-row>
            <a-descriptions
              v-if="boundEmployeeOption"
              bordered
              size="middle"
              :column="1"
            >
              <a-descriptions-item :label="t('entity.employee.code')">
                {{ displayStr(boundEmployeeOption.extLabel) }}
              </a-descriptions-item>
              <a-descriptions-item :label="t('entity.employee.name')">
                {{ displayStr(boundEmployeeOption.dictLabel) }}
              </a-descriptions-item>
            </a-descriptions>
            <a-alert
              v-else
              type="warning"
              show-icon
              :message="t('identity.user.page.tip.employee.option.missing')"
            />
          </template>
          <!-- 新增态 -->
          <template v-else>
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item
                  :label="t('entity.user.employeeid')"
                  name="employeeId"
                >
                  <TaktSelect
                    v-model:value="formState.employeeId"
                    :options="employeeOptions"
                    :placeholder="t('common.page.form.placeholder.select', { field: t('entity.user.employeeid') })"
                    show-search
                    :filter-option="filterOption"
                    @change="onEmployeeChange"
                  />
                </a-form-item>
              </a-col>
            </a-row>
            <a-alert
              type="info"
              show-icon
              :message="t('identity.user.page.tip.employee.snapshot.hint')"
              style="margin-bottom: 16px"
            />
            <a-space style="margin-bottom: 16px">
              <span>{{ t('identity.user.page.tip.employee.link.create.new.hint') }}</span>
              <a-button
                type="link"
                @click="goToEmployeeCreate"
              >
                {{ t('common.page.button.create') }}{{ t('entity.employee._self') }}
              </a-button>
            </a-space>
            <a-descriptions
              v-if="selecmedEmployeeOption"
              bordered
              size="middle"
              :column="1"
            >
              <a-descriptions-item :label="t('entity.employee.code')">
                {{ displayStr(selecmedEmployeeOption.extLabel) }}
              </a-descriptions-item>
              <a-descriptions-item :label="t('entity.employee.name')">
                {{ displayStr(selecmedEmployeeOption.dictLabel) }}
              </a-descriptions-item>
            </a-descriptions>
            <a-alert
              v-else-if="formState.employeeId"
              type="warning"
              show-icon
              :message="t('identity.user.page.tip.employee.option.missing')"
            />
          </template>
        </div>
      </a-tab-pane>

      <!-- 标签2：用户信息（账号、类型/状态、备注；新增时含密码） -->
      <a-tab-pane
        key="user"
        :tab="t('identity.user.page.tabs.user.info')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.user.name')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.user.name') })"
                  :disabled="!!formData?.userId"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.user.nickname')"
                name="nickName"
              >
                <a-input
                  v-model:value="formState.nickName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.user.nickname') })"
                  show-count
                  :maxlength="USER_NICK_NAME_MAX_LENGTH"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.user.type')"
                name="userType"
              >
                <TaktSelect
                  v-model:value="formState.userType"
                  dict-type="sys_user_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.user.type') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.user.status')"
                name="userStatus"
              >
                <TaktSelect
                  v-model:value="formState.userStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.user.status') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <!-- 仅新增展示密码；编辑不展示，提交时由父级不传 passwordHash -->
          <a-row
            v-if="!formData?.userId"
            :gutter="24"
          >
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.button.password')"
                name="password"
              >
                <a-input-password
                  v-model:value="formState.password"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.button.password') })"
                  show-count
                  :maxlength="20"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="4"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>

      <!-- 标签3：权限分配（角色；绑定 permissionState，随 getValues 返回） -->
      <a-tab-pane
        key="permission"
        :tab="t('common.tip.tab.target.allocation', { target: t('identity.user.page.tabs.permission') })"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item :label="t('entity.role._self')">
                <TaktSelect
                  v-model:value="permissionState.roleIds"
                  :options="roleOptions"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.role._self') })"
                  multiple
                  :max-tag-count="'responsive'"
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
 * 用户维护表单脚本：与 `user/index.vue` 中表单 ref 配合；
 * 父组件通过 `validate`、`getValues`、`resetFields`、`setServerValidationErrors` 完成弹窗提交流程。
 */
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import type { Rule } from 'ant-design-vue/es/form'
import { message } from 'ant-design-vue'
import type {
  UserFormDataInput,
  UserFormModel,
  UserFormPermissionModel,
  UserFormValues
} from '@/types/identity/userFormView.types'
import type { TaktSelectOption } from '@/types/common.types'
import { getRoleOptions } from '@/api/identity/role.api'
import { getEmployeeOptions } from '@/api/human-resource/personnel/employee.api'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import {
  isValidUserName,
  isValidUserNickName,
  isValidPassword,
  USER_NICK_NAME_MIN_LENGTH,
  USER_NICK_NAME_MAX_LENGTH,
} from '@/utils/regex'
const { t } = useI18n()
const router = useRouter()
const userStore = useUserStore()

/** 用户名长度下限（与 `RegexPatterns.userName` 一致） */
const UserName_MIN_LENGTH = 4
/** 用户名长度上限（与 `RegexPatterns.userName` 一致） */
const UserName_MAX_LENGTH = 20

/** 用户表单日志器 */
const userFormLogger = createLogger('identity.user.form')

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 当前激活 Tab：employee | user | permission */
const activeTab = ref('employee')
/** 表单内容区纵向布局类名（与弹窗内其他表单一致） */
const formContentClass = 'takt-form-content-rows-10'

/**
 * 组件入参：由列表页传入当前编辑用户或空对象（新增）。
 */
interface Props {
  /** 当前用户数据（编辑时含 userId）；API 为 `userName`/`nickName`，列表可带 `userName`/`nickName` 别名 */
  formData?: UserFormDataInput
  /** 提交中状态，由父级传入 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

/** Ant Design Vue 表单实例，用于 validate、resetFields、setFields */
const formRef = ref()

/** 与 `UserFormModel` 一致的初始值 */
function createEmptyUserFormModel(): UserFormModel {
  return {
    employeeId: '',
    userName: '',
    nickName: '',
    userType: 0,
    password: '',
    userStatus: 1,
    remark: ''
  }
}

/** a-form 绑定模型（UserFormModel） */
const formState = reactive<UserFormModel>(createEmptyUserFormModel())

/** 与 `UserFormPermissionModel` 一致的初始值 */
function createEmptyUserFormPermission(): UserFormPermissionModel {
  return {
    roleIds: []
  }
}

/** 权限标签页模型（UserFormPermissionModel），由 getValues 与 formState 合并为 UserFormValues */
const permissionState = reactive<UserFormPermissionModel>(createEmptyUserFormPermission())

/** 角色 / 员工 下拉选项 */
const roleOptions = ref<TaktSelectOption[]>([])
/** 员工下拉（新增与编辑拉取策略由 loadBusinessOptions 区分） */
const employeeOptions = ref<TaktSelectOption[]>([])

/** 当前登录用户是否为管理员类型（userType 1 或 2） */
const isAdmin = computed(() => {
  const ut = userStore.userType
  return ut === 1 || ut === 2
})

/** 根据 formState.employeeId 在员工选项中解析当前项 */
const selecmedEmployeeOption = computed(() => {
  const id = formState.employeeId
  if (id === undefined || id === null || String(id).trim() === '') return undefined
  return employeeOptions.value.find((o) => String(o.dictValue) === String(id))
})

/** 编辑态下与已绑定员工对应的选项（依赖 selecmedEmployeeOption） */
const boundEmployeeOption = computed(() => {
  if (!props.formData?.userId) return undefined
  return selecmedEmployeeOption.value
})

/** descriptions 展示用，空值显示为 `-` */
const displayStr = (v: unknown) => {
  if (v === undefined || v === null || String(v).trim() === '') return '-'
  return String(v)
}

/** 跳转人事员工建档页 */
const goToEmployeeCreate = () => {
  router.push('/human-resource/personnel/employee')
}

/** TaktSelect 搜索：按 label / extLabel 模糊匹配 */
const filterOption = (input: string, option: any) => {
  const label = (option?.label ?? option?.dictLabel ?? '') as string
  const ext = (option?.extLabel ?? '') as string
  return (label + ' ' + ext).toLowerCase().includes((input || '').toLowerCase())
}

/** 员工变更：新增时按选项带出默认 userName */
const onEmployeeChange = (value: string | number | (string | number)[] | undefined) => {
  const v = value == null ? '' : Array.isArray(value) ? value[0] : value
  const opt = employeeOptions.value.find((o) => String(o.dictValue) === String(v))
  if (opt && !props.formData?.userId) {
    formState.userName = (opt.extLabel ?? opt.dictLabel ?? '')
  }
}

/** 并行加载角色、员工选项 */
const loadBusinessOptions = async () => {
  try {
    const [roles, employees] = await Promise.all([
      getRoleOptions(),
      getEmployeeOptions()
    ])
    roleOptions.value = roles
    employeeOptions.value = employees
  } catch (error: any) {
    userFormLogger.error(
      '加载业务选项数据失败',
      {
        action: 'loadBusinessOptions',
        message: error?.message || t('common.feedback.load.options.failed')
      },
      error
    )
    const errorMessage = error?.response?.data?.message || error?.message || t('common.feedback.load.options.failed')
    message.error({ content: errorMessage, duration: 5 })
  }
}

/** 挂载时预加载字典并拉取角色、员工选项 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  loadBusinessOptions()
})

/** 字典整型字段校验器 */
function dictIntRule(fieldLabelKey: string): Rule {
  return {
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldLabelKey) }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldLabelKey) }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/** 校验规则：`name` 与 `a-form-item` 的 `name` 一致 */
const rules = computed<Record<string, Rule[]>>(() => {
  /** 对应 name=employeeId：新增必填；编辑已绑定则跳过 */
  const employeeIdRules: Rule[] = [
    {
      validator: (_rule: any, value: string) => {
        if (props.formData?.userId) return Promise.resolve()
        if (!value) {
          return Promise.reject(t('common.validation.required', { field: t('entity.user.employeeid') }))
        }
        return Promise.resolve()
      },
      trigger: 'change'
    }
  ]

  /** 对应 name=userName：新增必填 + 用户名格式；编辑禁用输入故跳过 */
  const UserNameRules: Rule[] = [
    {
      validator: (_rule: any, value: string) => {
        if (props.formData?.userId) return Promise.resolve()
        if (!value) {
          return Promise.reject(t('common.page.form.placeholder.required', { field: t('entity.user.name') }))
        }
        if (!isValidUserName(value)) {
          return Promise.reject(t('common.validation.length.between', {
            field: t('entity.user.name'),
            min: UserName_MIN_LENGTH,
            max: UserName_MAX_LENGTH
          }))
        }
        return Promise.resolve()
      },
      trigger: 'blur'
    }
  ]

  return {
    employeeId: employeeIdRules,
    userName: UserNameRules,
    nickName: [
      {
        validator: (_rule: any, value: string) => {
          if (!value || !String(value).trim()) return Promise.resolve()
          if (!isValidUserNickName(String(value))) {
            return Promise.reject(t('common.validation.length.between', {
              field: t('entity.user.nickname'),
              min: USER_NICK_NAME_MIN_LENGTH,
              max: USER_NICK_NAME_MAX_LENGTH
            }))
          }
          return Promise.resolve()
        },
        trigger: 'blur'
      }
    ],
    // password：仅新增必填 + 密码策略；编辑不展示该项
    password: [
      {
        required: !props.formData?.userId,
        message: t('common.page.form.placeholder.required', { field: t('common.page.button.password') }),
        trigger: 'blur'
      },
      {
        validator: (_rule: any, value: string) => {
          if (!value) return Promise.resolve()
          if (!isValidPassword(value)) {
            return Promise.reject(t('common.validation.too.weak', { field: t('common.page.button.password') }))
          }
          return Promise.resolve()
        },
        trigger: 'blur'
      }
    ],
    // userType：字典 sys_user_type，必选
    userType: [dictIntRule('entity.user.type')],
    // userStatus：字典 sys_normal_disable，必选
    userStatus: [dictIntRule('entity.user.status')],
  }
})

/** 监听 formData：编辑回填表单与权限 ID；新增重置为空；immediate + deep 与父级异步赋行对齐 */
watch(
  () => props.formData,
  (newData) => {
    if (newData?.userId) {
      Object.assign(formState, {
        employeeId: newData.employeeId != null ? String(newData.employeeId) : '',
        userName: newData.userName ?? newData.userName ?? '',
        nickName: newData.nickName ?? '',
        userType: newData.userType !== undefined && newData.userType !== null ? newData.userType : 0,
        password: '',
        userStatus: newData.userStatus !== undefined && newData.userStatus !== null ? newData.userStatus : 1,
        remark: newData.remark ?? ''
      })
      permissionState.roleIds = newData.roleIds?.map((id: string | number) => String(id)) ?? []
    } else {
      Object.assign(formState, createEmptyUserFormModel())
      Object.assign(permissionState, createEmptyUserFormPermission())
    }
  },
  { immediate: true, deep: true }
)

/** 预留：可结合当前用户类型与编辑态做权限类 UI 控制（当前仅占位） */
watch(
  () => [userStore.userType, props.formData?.userId] as const,
  () => {
    if (props.formData?.userId || !isAdmin.value) return
  }
)

/** 执行 a-form 校验；父组件在提交前 await 本方法 */
const validate = async () => {
  await formRef.value?.validate()
}

/** 合并基础字段与权限 ID，供父级组装 UserCreate / 更新 DTO */
const getValues = (): UserFormValues => {
  return {
    employeeId: formState.employeeId ? String(formState.employeeId) : '',
    userName: formState.userName || '',
    nickName: formState.nickName?.trim() ?? '',
    userType: formState.userType ?? 0,
    password: formState.password || '',
    userStatus: formState.userStatus ?? 1,
    remark: formState.remark || '',
    roleIds: permissionState.roleIds ?? []
  }
}

/** 重置 Ant 校验与本地 model，Tab 回到员工 */
const resetFields = () => {
  formRef.value?.resetFields()
  Object.assign(formState, createEmptyUserFormModel())
  Object.assign(permissionState, createEmptyUserFormPermission())
  activeTab.value = 'employee'
}

/** 将后端校验错误写入对应表单项 */
const setServerValidationErrors = (errors: Array<{ field?: string; message?: string }>) => {
  if (!Array.isArray(errors) || errors.length === 0) return
  const fields = errors
    .filter((e) => e?.field && e?.message)
    .map((e) => ({ name: e.field as string, errors: [e.message as string] }))
  if (fields.length > 0) {
    formRef.value?.setFields(fields)
  }
}

/** 暴露给 `user/index.vue` 弹窗 ref：`validate`、`getValues`、`resetFields`、`setServerValidationErrors` */
defineExpose({
  validate,
  getValues,
  resetFields,
  setServerValidationErrors
})
</script>

