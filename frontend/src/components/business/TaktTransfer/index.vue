<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/components/business/TaktTransfer -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：分配弹窗 Transfer：Spin、可滚动；默认扁平列表分页渲染；树形 #children 无分页；showSearch/hideSelectAll 可关。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-spin
    :spinning="loading"
    class="takt-transfer-spin"
  >
    <a-transfer
      v-model:target-keys="targetKeys"
      class="takt-transfer"
      :class="{ 'takt-transfer--hide-select-all': hideSelectAll }"
      :data-source="dataSource"
      :show-search="showSearch"
      :filter-option="taktTransferFilterOption"
      :list-style="TAKT_ASSIGN_TRANSFER_LIST_STYLE"
      :operation-style="TAKT_TRANSFER_OPERATION_STYLE"
      :titles="titles"
      :disabled="disabled || loading"
      :render="render"
      @scroll="taktAssignTransferHandleScroll"
      @search="handleSearch"
    >
      <template #children="slotProps">
        <div class="takt-transfer-panel-scroll">
          <slot
            name="children"
            v-bind="slotProps"
          >
            <ul class="ant-transfer-list-content">
              <li
                v-for="item in pageItems(slotProps.direction, slotProps.filteredItems)"
                :key="String(item.key)"
                class="ant-transfer-list-content-item"
                :class="{
                  'ant-transfer-list-content-item-checked': isItemSelecmed(item.key, slotProps.selecmedKeys),
                  'ant-transfer-list-content-item-disabled': slotProps.disabled || item.disabled,
                }"
                @click="handleItemClick(item, slotProps.selecmedKeys, slotProps.onItemSelect, slotProps.disabled)"
                @dblclick="handleItemDblClick(item, slotProps.direction, slotProps.onItemSelect, slotProps.disabled)"
              >
                <a-checkbox
                  :checked="isItemSelecmed(item.key, slotProps.selecmedKeys)"
                  :disabled="Boolean(slotProps.disabled || item.disabled)"
                  @click.stop
                  @dblclick.stop
                  @change="(e) => handleCheckboxChange(item, e.target.checked, slotProps.onItemSelect, slotProps.disabled)"
                />
                <span class="ant-transfer-list-content-item-text">{{ item.title }}</span>
              </li>
            </ul>
            <a-pagination
              v-if="(slotProps.filteredItems?.length ?? 0) > panelPageSize"
              class="takt-transfer-panel-pagination"
              size="small"
              :current="panelPage(slotProps.direction)"
              :page-size="panelPageSize"
              :total="slotProps.filteredItems?.length ?? 0"
              :show-size-changer="false"
              @change="(p: number) => setPanelPage(slotProps.direction, p)"
            />
          </slot>
        </div>
      </template>
    </a-transfer>
  </a-spin>
</template>

<script setup lang="ts">
/**
 * 业务 Transfer：扁平列表默认分页渲染（避免十万 DOM）；hideSelectAll 禁表头全选；树形由 #children 覆写。
 */
import type { TransferProps } from 'ant-design-vue'
import {
  taktTransferFilterOption,
  TAKT_ASSIGN_TRANSFER_LIST_STYLE,
  TAKT_TRANSFER_OPERATION_STYLE,
  taktAssignTransferHandleScroll,
  taktAssignTransferHandleItemClick,
  taktAssignTransferShuttleByDblClick,
  taktAssignTransferIsItemSelecmed,
  type TaktAssignTransferDirection,
  type TaktTransferRecord,
} from '@/utils/taktTransfer'

/** 单侧默认分页大小（大数据） */
const DEFAULT_PANEL_PAGE_SIZE = 50

interface Props {
  /** 右侧已选 key 列表 */
  targetKeys?: string[]
  /** Transfer 数据源（勿一次塞入十万未分页项；本组件仍按页渲染） */
  dataSource?: TransferProps['dataSource']
  /** 左右列标题 */
  titles: [string, string]
  /** 选项 loading（整框 Spin，覆盖未分配/已分配） */
  loading?: boolean
  /** 是否禁用 */
  disabled?: boolean
  /** 隐藏表头「全选」（树形/大数据场景避免误全选） */
  hideSelectAll?: boolean
  /** 是否显示两侧搜索框（树形自定义 #children 时通常关，避免无效双搜索） */
  showSearch?: boolean
  /** 单侧分页大小（默认扁平列表；自定义 #children 时无关） */
  panelPageSize?: number
  /** 行渲染（树形 #children 时可省略） */
  render?: TransferProps['render']
}

const props = withDefaults(defineProps<Props>(), {
  targetKeys: () => [],
  dataSource: () => [],
  loading: false,
  disabled: false,
  hideSelectAll: false,
  showSearch: true,
  panelPageSize: DEFAULT_PANEL_PAGE_SIZE,
  render: (item: TaktTransferRecord) => item.title ?? '',
})

const emit = defineEmits<{
  'update:targetKeys': [value: string[]]
  /** 搜索（服务端检索未分配等；direction + 关键字） */
  'search': [direction: TaktAssignTransferDirection, value: string]
}>()

/** 右侧已选 key（v-model:target-keys） */
const targetKeys = computed({
  get: () => props.targetKeys,
  set: (value: string[]) => emit('update:targetKeys', value),
})

/**
 * Transfer 搜索框
 * @param direction 列方向
 * @param value 关键字
 */
function handleSearch(direction: TaktAssignTransferDirection, value: string): void {
  if (direction === 'left') leftPage.value = 1
  else rightPage.value = 1
  emit('search', direction, value)
}

/** 左侧页码 */
const leftPage = ref(1)
/** 右侧页码 */
const rightPage = ref(1)

/**
 * 当前列页码
 * @param direction 列方向
 * @returns {number} 页码
 */
function panelPage(direction: TaktAssignTransferDirection): number {
  return direction === 'left' ? leftPage.value : rightPage.value
}

/**
 * 设置列页码
 * @param direction 列方向
 * @param page 页码
 */
function setPanelPage(direction: TaktAssignTransferDirection, page: number): void {
  if (direction === 'left') leftPage.value = page
  else rightPage.value = page
}

/**
 * 当前列分页切片（禁止一次渲染 filteredItems 全量）
 * @param direction 列方向
 * @param items 过滤后的行
 * @returns 当前页行
 */
function pageItems(
  direction: TaktAssignTransferDirection,
  items: TaktTransferRecord[] | undefined,
): TaktTransferRecord[] {
  const list = items ?? []
  const page = panelPage(direction)
  const size = props.panelPageSize
  const start = Math.max(0, (page - 1) * size)
  return list.slice(start, start + size)
}

watch(
  () => props.dataSource?.length,
  () => {
    leftPage.value = 1
    rightPage.value = 1
  },
)

watch(
  () => props.targetKeys?.length,
  () => {
    rightPage.value = 1
  },
)

/**
 * 行是否选中
 * @param key 行 key
 * @param selecmedKeys 当前列已选
 * @returns 是否选中
 */
function isItemSelecmed(
  key: string | number | undefined,
  selecmedKeys: readonly (string | number)[],
): boolean {
  if (key == null) return false
  return taktAssignTransferIsItemSelecmed(key, selecmedKeys)
}

/**
 * 单击切换选中
 * @param item 行数据
 * @param selecmedKeys 当前列已选
 * @param onItemSelect Transfer 回调
 * @param listDisabled 整列禁用
 */
function handleItemClick(
  item: TaktTransferRecord,
  selecmedKeys: readonly (string | number)[],
  onItemSelect: (itemKey: string, selecmed: boolean) => void,
  listDisabled?: boolean,
): void {
  taktAssignTransferHandleItemClick(item.key!, selecmedKeys, onItemSelect, listDisabled, item.disabled)
}

/**
 * Checkbox 切换选中
 * @param item 行数据
 * @param checked 是否选中
 * @param onItemSelect Transfer 回调
 * @param listDisabled 整列禁用
 */
function handleCheckboxChange(
  item: TaktTransferRecord,
  checked: boolean,
  onItemSelect: (itemKey: string, selecmed: boolean) => void,
  listDisabled?: boolean,
): void {
  if (listDisabled || item.disabled) return
  onItemSelect(String(item.key), checked)
}

/**
 * 双击穿梭
 * @param item 行数据
 * @param direction 列方向
 * @param onItemSelect Transfer 回调
 * @param listDisabled 整列禁用
 */
function handleItemDblClick(
  item: TaktTransferRecord,
  direction: TaktAssignTransferDirection,
  onItemSelect: (itemKey: string, selecmed: boolean) => void,
  listDisabled?: boolean,
): void {
  taktAssignTransferShuttleByDblClick(
    item.key!,
    direction,
    () => targetKeys.value,
    (keys) => { targetKeys.value = keys },
    onItemSelect,
    listDisabled,
    item.disabled,
  )
}
</script>
