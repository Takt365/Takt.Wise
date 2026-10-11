<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/identity/tenant/components -->
<!-- 文件名称：AssignTenantUsers.vue -->
<!-- 功能描述：分配租户用户；未分配 options 上限+搜索；已分配并入 dataSource；两侧互斥。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.user._self') })"
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
      <a-form-item :label="t('entity.tenant._self')">
        <a-input
          :value="tenantInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.user._self')"
            :label-col="{ span: 24 }"
            :wrapper-col="{ span: 24 }"
          >
            <TaktTransfer
              v-model:target-keys="targetKeys"
              :data-source="dataSource"
              :titles="[
                t('common.tip.transfer.unassigned'),
                `${t('common.tip.transfer.assigned')} (${targetKeys.length})`,
              ]"
              :loading="optionsLoading"
              hide-select-all
              @search="handleTransferSearch"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配租户用户：未分配走 getUserOptions(keyword) 上限；已分配来自 getTenantUserIds，不出现在未分配侧。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { TransferProps } from 'ant-design-vue'
import { getUserOptions } from '@/api/identity/user.api'
import { getTenantUserIds, assignTenantUsers } from '@/api/identity/rbac.api'
import type { Tenant } from '@/types/identity/tenant.types'
import type { UserTenant } from '@/types/identity/userTenant.types'
import type { TaktSelectOption } from '@/types/common.types'
import type { TaktAssignTransferDirection } from '@/utils/taktTransfer'

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
  /** 目标租户 */
  tenant?: Tenant | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  tenant: null,
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignTenantUsers')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 userId */
const targetKeys = ref<string[]>([])
/** 未分配候选（含搜索结果；不含已分配） */
const unassignedOptions = ref<TaktSelectOption[]>([])
/** 已分配标题 */
const assignedLabelMap = ref<Map<string, string>>(new Map())
/** 租户编码 */
const tenantCode = ref('')
/** 租户只读展示 */
const tenantInfo = ref('')
/** 左侧搜索关键字 */
const leftKeyword = ref('')

/**
 * Transfer 数据源：未分配候选 ∪ 已分配（右侧展示）；a-transfer 按 targetKeys 拆左右，已分配不会出现在未分配
 */
const dataSource = computed((): TransferProps['dataSource'] => {
  const map = new Map<string, { key: string; title: string; description: string }>()
  for (const item of unassignedOptions.value) {
    const key = String(item.dictValue)
    if (targetKeys.value.includes(key)) continue
    map.set(key, {
      key,
      title: item.dictLabel ?? '',
      description: key,
    })
  }
  for (const id of targetKeys.value) {
    map.set(id, {
      key: id,
      title: assignedLabelMap.value.get(id) || id,
      description: id,
    })
  }
  return [...map.values()]
})

watch(() => props.open, (val) => {
  visible.value = val
  if (val && props.tenant) {
    void loadTenantUsers()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

watch(targetKeys, (ids) => {
  for (const id of ids) {
    if (assignedLabelMap.value.has(id)) continue
    const opt = unassignedOptions.value.find((o) => String(o.dictValue) === id)
    if (opt?.dictLabel) assignedLabelMap.value.set(id, opt.dictLabel)
  }
})

/**
 * 加载未分配候选（上限由后端 options 控制）
 * @param keyword 关键字
 * @returns {Promise<void>}
 */
async function loadUnassignedOptions(keyword?: string): Promise<void> {
  const options = await getUserOptions(undefined, keyword?.trim() || undefined)
  const assigned = new Set(targetKeys.value)
  unassignedOptions.value = options.filter((o) => !assigned.has(String(o.dictValue)))
}

/**
 * Transfer 搜索：左侧走服务端 keyword；右侧本地 filter 由 a-transfer 处理
 * @param direction 列方向
 * @param value 关键字
 */
async function handleTransferSearch(direction: TaktAssignTransferDirection, value: string): Promise<void> {
  if (direction !== 'left') return
  leftKeyword.value = value
  try {
    optionsLoading.value = true
    await loadUnassignedOptions(value)
  } catch (error: unknown) {
    logger.error('[AssignTenantUsers] 搜索用户失败', undefined, error)
  } finally {
    optionsLoading.value = false
  }
}

/**
 * 加载用户选项与租户已绑 userId
 * @returns {Promise<void>}
 */
async function loadTenantUsers() {
  const tenant = props.tenant
  const code = tenant?.tenantCode?.trim()
  if (!code) return
  try {
    loading.value = true
    optionsLoading.value = true
    tenantCode.value = code
    tenantInfo.value = `${tenant?.tenantName ?? ''}（${code}）`
    leftKeyword.value = ''
    const userTenants = await getTenantUserIds(code)
    const labelMap = new Map<string, string>()
    const ids: string[] = []
    for (const row of userTenants as UserTenant[]) {
      const id = String(row.userId || '')
      if (!id) continue
      ids.push(id)
      if (row.userName) labelMap.set(id, row.userName)
    }
    assignedLabelMap.value = labelMap
    targetKeys.value = ids
    await loadUnassignedOptions()
  } catch (error: unknown) {
    logger.error('[AssignTenantUsers] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.tenant._self') + t('entity.user._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignTenantUsers
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!tenantCode.value) {
    message.error(t('common.validation.not.found', { field: t('entity.tenant._self') }))
    return
  }
  try {
    loading.value = true
    await assignTenantUsers(tenantCode.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.user._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignTenantUsers] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.user._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  tenantCode.value = ''
  targetKeys.value = []
  unassignedOptions.value = []
  assignedLabelMap.value = new Map()
  leftKeyword.value = ''
  tenantInfo.value = ''
}
</script>
