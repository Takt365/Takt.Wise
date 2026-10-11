<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/identity/menu/components -->
<!-- 文件名称：AssignMenuRoles.vue -->
<!-- 功能描述：分配菜单角色弹窗；Transfer + getRoleOptions / getMenuRoleIds / assignMenuRoles。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.role._self') })"
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
      <a-form-item :label="t('entity.menu._self')">
        <a-input
          :value="menuInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.role._self')"
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
 * 分配菜单角色弹窗：角色 Transfer，提交 assignMenuRoles（roleId 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getRoleOptions } from '@/api/identity/role.api'
import { getMenuRoleIds, assignMenuRoles } from '@/api/identity/rbac.api'
import type { Menu } from '@/types/identity/menu.types'
import type { RoleMenu } from '@/types/identity/roleMenu.types'
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
  /** 目标菜单 */
  menu?: Menu | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  menu: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignMenuRoles')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 roleId */
const targetKeys = ref<string[]>([])
/** 全量角色选项 */
const allOptions = ref<TaktSelectOption[]>([])
/** 菜单 id */
const menuId = ref('')
/** 菜单只读展示 */
const menuInfo = ref('')

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
  if (val && props.menu) {
    loadMenuRoles()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载角色选项与菜单已绑 roleId
 * @returns {Promise<void>}
 */
async function loadMenuRoles() {
  const menu = props.menu
  if (!menu?.menuId) return
  try {
    loading.value = true
    optionsLoading.value = true
    menuId.value = String(menu.menuId)
    menuInfo.value = `${menu.menuName ?? ''}${menu.menuCode ? `（${menu.menuCode}）` : ''}`
    const [allRoles, roleMenus] = await Promise.all([
      getRoleOptions(),
      getMenuRoleIds(menuId.value)
    ])
    allOptions.value = allRoles
    targetKeys.value = roleMenus
      .map((row: RoleMenu) => String(row.roleId || ''))
      .filter((id: string) => id)
  } catch (error: unknown) {
    logger.error('[AssignMenuRoles] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.menu._self') + t('entity.role._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignMenuRoles
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!menuId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.menu._self') }))
    return
  }
  try {
    loading.value = true
    await assignMenuRoles(menuId.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.role._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignMenuRoles] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.role._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  menuId.value = ''
  targetKeys.value = []
  allOptions.value = []
  menuInfo.value = ''
}
</script>
