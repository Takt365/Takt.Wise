<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/human-resource/organization/dept/components -->
<!-- 文件名称：AssignDeptEmployees.vue -->
<!-- 功能描述：分配部门员工弹窗；Transfer + getEmployeeOptions / getDeptEmployeeIds / assignDeptEmployees。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.employee._self') })"
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
      <a-form-item :label="t('entity.dept._self')">
        <a-input
          :value="deptInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.employee._self')"
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
 * 分配部门员工弹窗：员工 Transfer，提交 assignDeptEmployees（employeeId 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getEmployeeOptions } from '@/api/human-resource/personnel/employee.api'
import { getDeptEmployeeIds, assignDeptEmployees } from '@/api/identity/rbac.api'
import type { Dept } from '@/types/human-resource/organization/dept.types'
import type { EmployeeDept } from '@/types/human-resource/organization/employeeDept.types'
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
  /** 目标部门 */
  dept?: Dept | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  dept: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignDeptEmployees')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 employeeId */
const targetKeys = ref<string[]>([])
/** 全量员工选项 */
const allOptions = ref<TaktSelectOption[]>([])
/** 部门 id */
const deptId = ref('')
/** 部门只读展示 */
const deptInfo = ref('')

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
  if (val && props.dept) {
    loadDeptEmployees()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载员工选项与部门已绑 employeeId
 * @returns {Promise<void>}
 */
async function loadDeptEmployees() {
  const dept = props.dept
  if (!dept?.deptId) return
  try {
    loading.value = true
    optionsLoading.value = true
    deptId.value = String(dept.deptId)
    deptInfo.value = `${dept.deptName1 ?? ''}${dept.deptCode ? `（${dept.deptCode}）` : ''}`
    const [allEmployees, employeeDepts] = await Promise.all([
      getEmployeeOptions(),
      getDeptEmployeeIds(deptId.value)
    ])
    allOptions.value = allEmployees
    targetKeys.value = employeeDepts
      .map((row: EmployeeDept) => String(row.employeeId || ''))
      .filter((id: string) => id)
  } catch (error: unknown) {
    logger.error('[AssignDeptEmployees] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.dept._self') + t('entity.employee._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignDeptEmployees
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!deptId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.dept._self') }))
    return
  }
  try {
    loading.value = true
    await assignDeptEmployees(deptId.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.employee._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignDeptEmployees] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.employee._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  deptId.value = ''
  targetKeys.value = []
  allOptions.value = []
  deptInfo.value = ''
}
</script>
