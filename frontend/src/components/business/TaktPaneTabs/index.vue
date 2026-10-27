<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/business/TaktPaneTabs -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：页面内容页签（≠ navigation/TaktTabs 路由多标签）；items 可按 show/permission 删减 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-tabs
    v-model:activeKey="activeKeyProxy"
    class="takt-pane-tabs"
    :class="[
      rootClass,
      { 'takt-pane-tabs--hide-content': hideContent },
    ]"
    :type="type"
    :size="size"
    :tab-position="tabPosition"
    :centered="centered"
    :animated="animated"
    :destroy-inactive-tab-pane="destroyInactiveTabPane"
    :hide-add="hideAdd"
    @change="onChange"
    @tab-click="onTabClick"
    @edit="onEdit"
  >
    <template v-if="useItemsMode">
      <a-tab-pane
        v-for="item in visibleItems"
        :key="item.key"
        :tab="resolveLabel(item)"
        :disabled="item.disabled"
        :closable="item.closable"
        :force-render="item.forceRender"
        :class="item.paneClass"
      >
        <slot
          :name="item.slot ?? item.key"
          :item="item"
        />
      </a-tab-pane>
    </template>
    <slot v-else />
    <template
      v-if="$slots.leftExtra"
      #leftExtra
    >
      <slot name="leftExtra" />
    </template>
    <template
      v-if="$slots.rightExtra"
      #rightExtra
    >
      <slot name="rightExtra" />
    </template>
    <template
      v-if="$slots.addIcon"
      #addIcon
    >
      <slot name="addIcon" />
    </template>
  </a-tabs>
</template>

<script setup lang="ts">
/**
 * TaktPaneTabs：页面内容页签；items.show / permission 控制可见；hideContent 仅作导航条
 */
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import { hasAnyPermission } from '@/utils/permission'

/** 页面内容页签项（可删减） */
export interface TaktPaneTabItem {
  /** 页签键（同时默认具名插槽名） */
  key: string
  /** 页签标题；亦可仅用 labelKey + i18n 由调用方算好字符串 */
  label: string
  /** 是否显示；默认 true；false 则不渲染，便于按场景删减 */
  show?: boolean
  /** 权限码或数组；无权限时隐藏（与 v-permission 同源） */
  permission?: string | string[]
  /** 禁用 */
  disabled?: boolean
  /** 可关闭（editable-card） */
  closable?: boolean
  /** 强制渲染内容 */
  forceRender?: boolean
  /** 覆盖默认具名插槽名（默认用 key） */
  slot?: string
  /** 页签面板 class */
  paneClass?: string
}

const props = withDefaults(
  defineProps<{
    /** 当前页签（v-model:active-key） */
    activeKey?: string
    /**
     * 页签定义；传入后按 show/permission 过滤渲染。
     * 不传则透传默认插槽内的 a-tab-pane（兼容表单多段 Tabs）
     */
    items?: TaktPaneTabItem[]
    /** 仅导航：隐藏内容区（页签切换驱动外侧内容） */
    hideContent?: boolean
    /** a-tabs type */
    type?: 'line' | 'card' | 'editable-card'
    /** 尺寸 */
    size?: 'large' | 'middle' | 'small'
    /** 位置 */
    tabPosition?: 'top' | 'right' | 'bottom' | 'left'
    /** 居中 */
    centered?: boolean
    /** 动画 */
    animated?: boolean | { inkBar?: boolean; tabPane?: boolean }
    /** 销毁非激活面板 */
    destroyInactiveTabPane?: boolean
    /** 隐藏新增按钮 */
    hideAdd?: boolean
    /** 根 class */
    rootClass?: string
  }>(),
  {
    activeKey: undefined,
    items: undefined,
    hideContent: false,
    type: 'line',
    size: 'middle',
    tabPosition: 'top',
    centered: false,
    animated: true,
    destroyInactiveTabPane: false,
    hideAdd: true,
    rootClass: undefined,
  },
)

const emit = defineEmits<{
  'update:activeKey': [key: string]
  change: [key: string]
  tabClick: [key: string | number, event: Event]
  edit: [targetKey: string | MouseEvent | KeyboardEvent, action: 'add' | 'remove']
}>()

const permissionStore = usePermissionStore()

/** 是否 items 驱动 */
const useItemsMode = computed(() => Array.isArray(props.items))

/**
 * 可见页签（show !== false 且权限通过）
 */
const visibleItems = computed(() => {
  const list = props.items ?? []
  return list.filter((item) => {
    if (item.show === false) {
      return false
    }
    if (item.permission == null || item.permission === '' || (Array.isArray(item.permission) && item.permission.length === 0)) {
      return true
    }
    return hasAnyPermission(permissionStore.permissions, item.permission)
  })
})

/** 双向绑定 activeKey */
const activeKeyProxy = computed({
  get: () => props.activeKey,
  set: (key: string) => {
    emit('update:activeKey', key)
  },
})

/**
 * 标题
 * @param item 页签项
 */
function resolveLabel(item: TaktPaneTabItem): string {
  return item.label
}

/**
 * 切换
 * @param key 页签键
 */
function onChange(key: string | number) {
  const next = String(key)
  emit('update:activeKey', next)
  emit('change', next)
}

/**
 * 点击页签
 * @param key 键
 * @param event 事件
 */
function onTabClick(key: string | number, event: Event) {
  emit('tabClick', key, event)
}

/**
 * 增删（editable-card）
 * @param targetKey 目标
 * @param action 动作
 */
function onEdit(targetKey: string | MouseEvent | KeyboardEvent, action: 'add' | 'remove') {
  emit('edit', targetKey, action)
}

/**
 * 当前键被删减后落到第一个可见页签
 */
watch(
  visibleItems,
  (items) => {
    if (!useItemsMode.value || items.length === 0) {
      return
    }
    const current = props.activeKey
    if (current && items.some((item) => item.key === current)) {
      return
    }
    const first = items[0]?.key
    if (first) {
      emit('update:activeKey', first)
      emit('change', first)
    }
  },
  { immediate: true },
)

defineExpose({
  visibleItems,
})
</script>

<style scoped>
.takt-pane-tabs--hide-content :deep(.ant-tabs-content-holder) {
  display: none;
}
</style>
