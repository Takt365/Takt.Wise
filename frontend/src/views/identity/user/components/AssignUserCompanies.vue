<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/identity/user/components -->
<!-- 文件名称：AssignUserCompanies.vue -->
<!-- 功能描述：分配用户可访问公司弹窗；Transfer + getCompanyOptions / getUserCompanyIds / assignUserCompanies。 -->
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
      <a-form-item :label="t('entity.user._self')">
        <a-input
          :value="userInfo"
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
 * 分配用户可访问公司弹窗：公司 Transfer，提交 assignUserCompanies（companyCode 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getCompanyOptions } from '@/api/accounting/financial/company.api'
import { getUserCompanyIds, assignUserCompanies } from '@/api/identity/rbac.api'
import type { User } from '@/types/identity/user.types'
import type { UserCompany } from '@/types/identity/userCompany.types'
import type { TaktSelectOption } from '@/types/common.types'

/** 分配弹窗用户记录（含展示别名） */
type UserAssignRecord = User & { userName?: string; nickName?: string }

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
  /** 目标用户 */
  user?: UserAssignRecord | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  user: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignUserCompanies')

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
/** 用户只读展示 */
const userInfo = ref('')

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
  if (val && props.user) {
    loadUserCompanies()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载公司选项与用户已绑 companyCode
 * @returns {Promise<void>}
 */
async function loadUserCompanies() {
  if (!props.user?.userId) return
  try {
    loading.value = true
    optionsLoading.value = true
    const u = props.user
    userInfo.value = `${u.userName || u.userName || ''}（${u.nickName || u.nickName || ''}）`
    const [allCompanies, userCompanies] = await Promise.all([
      getCompanyOptions(),
      getUserCompanyIds(String(u.userId))
    ])
    allOptions.value = allCompanies
    targetKeys.value = userCompanies
      .map((row: UserCompany) => String(row.companyCode || ''))
      .filter((code: string) => code)
  } catch (error: unknown) {
    logger.error('[AssignUserCompanies] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.user._self') + t('entity.company._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignUserCompanies
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!props.user?.userId) {
    message.error(t('common.validation.not.found', { field: t('entity.user._self') }))
    return
  }
  try {
    loading.value = true
    await assignUserCompanies(String(props.user.userId), targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.company._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignUserCompanies] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.company._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  targetKeys.value = []
  allOptions.value = []
  userInfo.value = ''
}
</script>
