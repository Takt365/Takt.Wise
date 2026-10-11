<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/identity/role/components -->
<!-- 文件名称：AssignRoleCompanies.vue -->
<!-- 功能描述：分配角色可访问公司弹窗；Transfer + getCompanyOptions / getRoleCompanyIds / assignRoleCompanies。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.company._self') })"
    :width="'33.333vw'"
    :confirm-loading="loading"
    :centered="true"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-form
      :label-col="{ span: 4 }"
      :wrapper-col="{ span: 20 }"
      layout="horizontal"
    >
      <a-form-item :label="t('entity.role._self')">
        <a-input
          :value="roleInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.company._self')"
            :label-col="{ span: 24 }"
            :wrapper-col="{ span: 24 }"
          >
            <TaktTransfer
              v-model:target-keys="targetKeys"
              :data-source="dataSource"
              :titles="[t('common.tip.transfer.unassigned'), t('common.tip.transfer.assigned')]"
              :loading="optionsLoading"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配角色可访问公司弹窗：公司 Transfer，提交 assignRoleCompanies（companyCode 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getCompanyOptions } from '@/api/accounting/financial/company.api'
import { getRoleCompanyIds, assignRoleCompanies } from '@/api/identity/rbac.api'
import type { Role } from '@/types/identity/role.types'
import type { RoleCompany } from '@/types/identity/roleCompany.types'
import type { TaktSelectOption } from '@/types/common.types'

/**
 * 从异常对象提取可展示消息
 * @param error 捕获的异常
 * @returns {string | undefined} 错误文案
 */
function getErrorMessage(error: unknown): string | undefined {
  if (error instanceof Error) return error.message
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const msg = (error as { message?: unknown }).message
    return typeof msg === 'string' ? msg : undefined
  }
  return undefined
}

/** 组件入参 */
interface Props {
  /** 是否显示对话框 */
  open?: boolean
  /** 目标角色 */
  role?: Role | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  role: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignRoleCompanies')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 companyCode */
const targetKeys = ref<string[]>([])
/** 全量公司选项 */
const allOptions = ref<TaktSelectOption[]>([])
/** 角色 id */
const roleId = ref('')
/** 角色只读展示 */
const roleInfo = ref('')

/** Transfer 数据源 */
const dataSource = computed(() =>
  allOptions.value.map((item) => ({
    key: String(item.dictValue),
    title: item.dictLabel ?? '',
    description: String(item.dictValue ?? ''),
  }))
)

watch(() => props.open, (val) => {
  visible.value = val
  if (val && props.role) {
    loadRoleCompanies()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载公司选项与角色已绑 companyCode
 * @returns {Promise<void>}
 */
async function loadRoleCompanies() {
  const role = props.role
  if (!role?.roleId) return
  try {
    loading.value = true
    optionsLoading.value = true
    roleId.value = String(role.roleId)
    roleInfo.value = `${role.roleName ?? ''}（${role.roleCode ?? ''}）`
    const [allCompanies, roleCompanies] = await Promise.all([
      getCompanyOptions(),
      getRoleCompanyIds(roleId.value)
    ])
    allOptions.value = allCompanies
    targetKeys.value = roleCompanies
      .map((row: RoleCompany) => String(row.companyCode || ''))
      .filter((code: string) => code)
  } catch (error: unknown) {
    logger.error('[AssignRoleCompanies] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.role._self') + t('entity.company._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignRoleCompanies
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!roleId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.role._self') }))
    return
  }
  try {
    loading.value = true
    await assignRoleCompanies(roleId.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.company._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignRoleCompanies] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.company._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  roleId.value = ''
  targetKeys.value = []
  allOptions.value = []
  roleInfo.value = ''
}
</script>
