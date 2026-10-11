<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktColumnDrawer -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-21 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 列设置抽屉组件，封装 a-drawer，统一设置中文按钮和布局，包含完整的列设置功能 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-drawer
    :open="props.open"
    v-bind="drawerProps"
    :title="title ?? t('common.page.button.columnsetting')"
    :placement="placement"
    :width="width"
    class="takt-column-drawer"
    @close="handleClose"
  >
    <template #extra>
      <a-button
        size="small"
        @click="handleReset"
      >
        {{ resetText ?? t('common.page.button.reset') }}
      </a-button>
    </template>
    
    <div class="column-setting-content">
      <a-checkbox-group
        v-model:value="selectedColumnKeys"
        class="column-checkbox-group"
      >
        <div
          v-for="column in validColumns"
          :key="getColumnKey(column)"
          class="column-item"
        >
          <a-checkbox
            :value="getColumnKey(column)"
            :disabled="isFixedColumn(column)"
          >
            {{ getColumnTitle(column) }}
          </a-checkbox>
          <a-tag
            v-if="isFixedColumn(column)"
            size="small"
            color="blue"
          >
            {{ t('components.business.page.columndrawer.fixed') }}
          </a-tag>
        </div>
      </a-checkbox-group>
    </div>
  </a-drawer>
</template>

<script setup lang="ts">
import type { TableColumnsType } from 'ant-design-vue'
import {
  mergeDefaultColumns,
  normalizeUserTableColumns,
  readColumnSettingLabel,
  resolveDefaultVisibleColumnKeys,
  type TaktEntityScope,
  type TaktTableLayoutMode,
} from '@/utils/tableColumns'
import { useI18n } from 'vue-i18n'
import { createLogger } from '@/utils/logger'

const columnDrawerLogger = createLogger('takt-column-drawer')

const { t } = useI18n()

/** 与 Table `columns` 单项类型一致（title 可为 ColumnTitle，不可收窄为 string） */
type ColumnItem = TableColumnsType[number]

/** 列设置里仅展示 string/number 标题；VNode/函数标题须由 taktColumnSettingLabel 提供文案 */
function columnTitlePrimitive(column: ColumnItem): string | number | undefined {
  const v = column.title
  if (v == null) return undefined
  if (typeof v === 'string' || typeof v === 'number') return v
  return undefined
}

/** ColumnGroupType 无 dataIndex；DataIndex 可为路径数组，统一成 string | number 供 key 使用 */
function columnDataIndex(column: ColumnItem): string | number | undefined {
  if (!('dataIndex' in column)) return undefined
  const di = column.dataIndex
  if (di == null) return undefined
  if (typeof di === 'string' || typeof di === 'number') return di
  const parts = [...di]
  return parts.length ? parts.join('.') : undefined
}

interface Props {
  /** 是否显示抽屉 */
  open?: boolean
  /** 抽屉标题，默认为"列设置" */
  title?: string
  /** 抽屉位置，默认为"right" */
  placement?: 'top' | 'right' | 'bottom' | 'left'
  /** 抽屉宽度，默认为 400 */
  width?: string | number
  /** 重置按钮文本，默认为"重置" */
  resetText?: string
  /** 列配置 */
  columns: TableColumnsType
  /** 已选中的列键 */
  checkedKeys?: string[]
  /** ID列的键（默认 'id'） */
  idColumnKey?: string | number
  /** 操作列的键（默认 'action'） */
  actionColumnKey?: string | number
  /** 实体基类作用域：tenant | company | approval，与 common.d.ts 三个 EntityBase 字段一一对应，驱动 mergeDefaultColumns */
  entityScope: TaktEntityScope
  /** 单表 8 个业务列 / 左树右表 4 个业务列（默认 single） */
  tableMode?: TaktTableLayoutMode
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  placement: 'right',
  width: 400,
  checkedKeys: (): string[] => [],
  idColumnKey: 'id',
  actionColumnKey: 'action',
  tableMode: 'single',
})

const emit = defineEmits<{
  'update:open': [open: boolean]
  'update:checkedKeys': [keys: string[]]
  'reset': []
  'close': []
}>()

/** 页面业务列（解包 Ref/ComputedRef，避免 merge 时只剩审计字段） */
const userColumns = computed(() => normalizeUserTableColumns(props.columns))

const mergedColumns = computed(() => {
  const columns = userColumns.value
  if (columns.length === 0) {
    if (import.meta.env.DEV) {
      columnDrawerLogger.warn('业务 columns 为空，列设置仅显示实体基座字段', { action: 'mergedColumns' })
    }
    return mergeDefaultColumns([], t, true, props.entityScope)
  }
  if (import.meta.env.DEV) {
    columnDrawerLogger.debug('mergedColumns 计算', {
      action: 'mergedColumns',
      columnsLength: columns.length,
      columnsKeys: columns.slice(0, 5).map((col) => col.key ?? columnDataIndex(col) ?? columnTitlePrimitive(col)),
    })
  }
  const merged = mergeDefaultColumns(columns, t, true, props.entityScope)
  
  if (import.meta.env.DEV) {
    columnDrawerLogger.debug('mergeDefaultColumns 结果', {
      action: 'mergeDefaultColumns',
      mergedLength: merged.length,
      mergedKeys: merged.slice(0, 10).map((col) => col.key ?? columnDataIndex(col) ?? columnTitlePrimitive(col)),
    })
  }
  
  return merged
})

const attrs = useAttrs()

// 计算 drawer 的所有属性，排除已定义的 props
const drawerProps = computed(() => {
  return attrs
})

// 确保操作列始终被包含，并在最后
const ensureFixedColumns = (keys: string[]): string[] => {
  const actionKey = String(props.actionColumnKey)
  // 移除操作列（如果存在）
  const keysWithoutAction = keys.filter(k => k !== actionKey)
  // 将操作列添加到最后
  return [...keysWithoutAction, actionKey]
}

// 获取列键（统一转换为字符串）
const getColumnKey = (column: ColumnItem): string => {
  const key = column.key ?? columnDataIndex(column) ?? columnTitlePrimitive(column)
  if (key == null || key === '') {
    return ''
  }
  return String(key)
}

// 获取列标题（列设置抽屉用；函数/VNode 表头依赖 taktColumnSettingLabel）
const getColumnTitle = (column: ColumnItem): string => {
  const settingLabel = readColumnSettingLabel(column)
  if (settingLabel) {
    return settingLabel
  }
  const fromTitle = columnTitlePrimitive(column)
  if (fromTitle != null && fromTitle !== '') return String(fromTitle)
  return String(column.key ?? columnDataIndex(column) ?? '')
}

// 判断是否为固定列（只有操作列是固定的）
const isFixedColumn = (column: ColumnItem): boolean => {
  const key = getColumnKey(column)
  const actionKey = String(props.actionColumnKey)
  return key === actionKey
}

// 获取有效的列（有 key、dataIndex 或 title 的列）
// 使用合并后的列配置（包含审计字段）
const validColumns = computed(() => {
  return mergedColumns.value.filter((col) => {
    const key = col.key ?? columnDataIndex(col) ?? columnTitlePrimitive(col)
    return key != null && key !== ''
  })
})

/** 默认可见列：ID + 按 columns 顺序的前 N 个业务字段 + 操作列（不从 merge 审计列猜测） */
const getDefaultCheckedKeys = (): string[] => {
  return resolveDefaultVisibleColumnKeys(userColumns.value, {
    idColumnKey: props.idColumnKey,
    actionColumnKey: props.actionColumnKey,
    tableMode: props.tableMode,
    entityScope: props.entityScope,
  })
}

// 内部选中状态，用于跟踪用户的选择
const internalCheckedKeys = ref<string[]>([])

// 初始化内部状态
const initCheckedKeys = () => {
  if (props.checkedKeys && props.checkedKeys.length > 0) {
    // 如果父组件提供了 checkedKeys，使用它
    internalCheckedKeys.value = ensureFixedColumns([...props.checkedKeys]).map(k => String(k))
  } else {
    // 否则使用默认值
    internalCheckedKeys.value = getDefaultCheckedKeys()
    // 立即通知父组件默认值
    emit('update:checkedKeys', internalCheckedKeys.value)
  }
}

// 初始化
initCheckedKeys()

// 监听 props.checkedKeys 的变化（从外部同步，比如重置操作）
watch(
  () => props.checkedKeys,
  (newKeys) => {
    if (newKeys && newKeys.length > 0) {
      const propsKeys = ensureFixedColumns([...newKeys]).map(k => String(k))
      // 只有当值真正改变时才更新，避免循环更新
      const currentSorted = [...internalCheckedKeys.value].sort()
      const newSorted = [...propsKeys].sort()
      if (JSON.stringify(currentSorted) !== JSON.stringify(newSorted)) {
        internalCheckedKeys.value = propsKeys
      }
    } else if (newKeys && newKeys.length === 0) {
      // 如果外部清空，重置为默认值
      const defaultKeys = getDefaultCheckedKeys()
      internalCheckedKeys.value = defaultKeys
      emit('update:checkedKeys', defaultKeys)
    }
  },
  { deep: true }
)

// 组件挂载后，确保父组件有初始值
onMounted(() => {
  // 如果父组件没有提供 checkedKeys，确保发送默认值
  if (!props.checkedKeys || props.checkedKeys.length === 0) {
    const defaultKeys = getDefaultCheckedKeys()
    if (internalCheckedKeys.value.length === 0) {
      internalCheckedKeys.value = defaultKeys
    }
    emit('update:checkedKeys', internalCheckedKeys.value)
  }
})

// 使用 computed 来绑定到 checkbox-group
// 当用户勾选/取消勾选时，会触发 setter，更新内部状态并通知父组件
const selectedColumnKeys = computed({
  get: () => {
    return internalCheckedKeys.value
  },
  set: (val: string[]) => {
    // 确保固定列（操作列）始终被包含
    const finalKeys = ensureFixedColumns(val.map(k => String(k)))
    // 更新内部状态
    internalCheckedKeys.value = finalKeys
    // 立即通知父组件选中状态变化
    emit('update:checkedKeys', finalKeys)
  }
})

// 处理重置
const handleReset = () => {
  // 重置为默认可见列（ID + 前 N 个业务字段 + 操作列）
  selectedColumnKeys.value = getDefaultCheckedKeys()
  emit('reset')
}

// 处理关闭
const handleClose = () => {
  emit('close')
  emit('update:open', false)
}

// 暴露合并后的列配置和根据选中 keys 过滤的列配置，供外部使用
defineExpose({
  // 合并后的完整列配置（包含审计字段）
  mergedColumns,
  // 根据选中 keys 过滤的列配置
  displayColumns: computed(() => {
    const keys = selectedColumnKeys.value || []
    const merged = mergedColumns.value || []
    return merged.filter(col => {
      const key = getColumnKey(col)
      return key && keys.includes(key)
    })
  })
})
</script>

<style scoped>
.takt-column-drawer {
  :deep(.ant-drawer-body) {
    padding: 16px;
  }
}

.column-setting-content {
  .column-checkbox-group {
    width: 100%;
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  .column-item {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 8px 0;
    border-bottom: 1px solid var(--ant-color-border-secondary);

    &:last-child {
      border-bottom: none;
    }

    :deep(.ant-checkbox-wrapper) {
      flex: 1;
    }
  }
}
</style>
