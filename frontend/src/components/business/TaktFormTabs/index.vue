<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/business/TaktFormTabs -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：表单分段页签；可按 fieldNames/autoTabCount 自动分页（与 generate-vue-common FORM_TAB_* 对齐）；亦可透传 a-tab-pane -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-tabs
    v-model:activeKey="activeKeyProxy"
    class="takt-form-tabs"
    :class="rootClass"
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
        :force-render="item.forceRender !== false"
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
 * TaktFormTabs：表单内分段页签；字段数自动分页与流水线 generate-vue-common 一致
 */
import { useI18n } from 'vue-i18n'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import { hasAnyPermission } from '@/utils/permission'
import {
  TAKT_FORM_TAB_FIELDS_PER_TAB,
  taktBuildBasicInfoFormTabItems,
  taktPartitionFormFieldsForTabs,
} from '@/utils/taktFormTabs'

/** 表单页签项（可删减）；工具函数请从 @/utils/taktFormTabs 导入 */
interface TaktFormTabItem {
  /** 页签键（同时默认具名插槽名） */
  key: string
  /** 页签标题 */
  label: string
  /** 是否显示；默认 true */
  show?: boolean
  /** 权限码或数组；无权限时隐藏 */
  permission?: string | string[]
  /** 禁用 */
  disabled?: boolean
  /** 可关闭（editable-card） */
  closable?: boolean
  /**
   * 强制渲染；表单默认 true（未写则按 true），保证校验能扫到隐藏页签字段
   */
  forceRender?: boolean
  /** 覆盖默认具名插槽名 */
  slot?: string
  /** 页签面板 class */
  paneClass?: string
}

const props = withDefaults(
  defineProps<{
    /** 当前页签（v-model:active-key） */
    activeKey?: string
    /**
     * 页签定义；传入后按 show/permission 过滤。
     * 优先级高于 fieldNames / autoTabCount
     */
    items?: TaktFormTabItem[]
    /**
     * 表单字段名列表；按流水线规则自动拆 Tab 并生成 items（#tab-0… 插槽）
     * 与 scripts/gen/generate-vue-common.cjs partitionFormFieldsForTabs 一致
     */
    fieldNames?: string[]
    /**
     * 仅按数量生成 tab-0…（简单 ceil；无 leading/trailing）。
     * fieldNames / items 优先
     */
    autoTabCount?: number
    /**
     * 每 Tab 字段上限；默认 TAKT_FORM_TAB_FIELDS_PER_TAB（10）
     */
    fieldsPerTab?: number
    /** 基础信息文案；默认 t('common.page.form.tabs.basicinfo') */
    basicInfoLabel?: string
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
    /**
     * 销毁非激活面板；表单默认 false，避免未校验字段随面板销毁
     */
    destroyInactiveTabPane?: boolean
    /** 隐藏新增按钮 */
    hideAdd?: boolean
    /** 根 class（可叠 ec-form-tabs 等布局类） */
    rootClass?: string
  }>(),
  {
    activeKey: undefined,
    items: undefined,
    fieldNames: undefined,
    autoTabCount: undefined,
    fieldsPerTab: TAKT_FORM_TAB_FIELDS_PER_TAB,
    basicInfoLabel: undefined,
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

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 基础信息标签 */
const resolvedBasicInfoLabel = computed(
  () => props.basicInfoLabel ?? t('common.page.form.tabs.basicinfo'),
)

/**
 * 由 fieldNames / autoTabCount 推导的 items
 */
const autoItems = computed<TaktFormTabItem[]>(() => {
  const perTab = props.fieldsPerTab ?? TAKT_FORM_TAB_FIELDS_PER_TAB
  const label = resolvedBasicInfoLabel.value
  if (props.fieldNames?.length) {
    const fields = props.fieldNames.map((name) => ({ name }))
    const tabCount = taktPartitionFormFieldsForTabs(fields, perTab).length
    return taktBuildBasicInfoFormTabItems(Math.max(1, tabCount), label)
  }
  if (props.autoTabCount != null && props.autoTabCount > 0) {
    return taktBuildBasicInfoFormTabItems(props.autoTabCount, label)
  }
  return []
})

/**
 * 最终 items：显式 items 优先，否则自动分页
 */
const resolvedItems = computed<TaktFormTabItem[] | undefined>(() => {
  if (Array.isArray(props.items)) {
    return props.items
  }
  if (autoItems.value.length > 0) {
    return autoItems.value
  }
  return undefined
})

/** 是否 items 驱动（含自动分页） */
const useItemsMode = computed(() => Array.isArray(resolvedItems.value))

/**
 * 可见页签（show !== false 且权限通过）
 */
const visibleItems = computed(() => {
  const list = resolvedItems.value ?? []
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
function resolveLabel(item: TaktFormTabItem): string {
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
  fieldsPerTab: computed(() => props.fieldsPerTab ?? TAKT_FORM_TAB_FIELDS_PER_TAB),
})
</script>
