<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktTreeSelect -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 树形下拉选择框组件，支持 API 动态加载、树形数据、业务选项等 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="takt-tree-select-wrapper"
    :class="treeSelectRootClass"
  >
    <a-tree-select
      ref="treeSelectRef"
      class="takt-tree-select"
      :value="internalValue"
      :tree-data="treeData"
      :tree-expanded-keys="expandedKeys"
      :loading="loading"
      :placeholder="placeholder ?? t('common.page.form.placeholder.selectonly')"
      :allow-clear="allowClear"
      :disabled="disabled"
      :multiple="multiple"
      :tree-checkable="effectiveTreeCheckable"
      :tree-check-strictly="treeCheckStrictly"
      :size="size"
      :show-search="showSearch"
      :tree-node-filter-prop="treeNodeFilterProp"
      :field-names="treeFieldNames"
      :virtual="shouldUseVirtual"
      :list-height="listHeight"
      :load-data="apiLazyLoadData"
      popup-class-name="takt-tree-select-dropdown"
      v-bind="{
        ...filteredAttrs,
        ...(props.multiple ? { maxTagCount: effectiveMaxTagCount } : {})
      }"
      @update:value="handleUpdateValue"
      @change="handleChange"
      @search="handleSearch"
      @tree-expand="handleTreeExpand"
      @dropdown-visible-change="handleDropdownVisibleChange"
    >
      <template
        v-if="$slots.default"
        #default
      >
        <slot />
      </template>
    </a-tree-select>
  </div>
</template>

<script setup lang="ts">
import { render } from 'vue';
import { useI18n } from 'vue-i18n'
import { Button } from 'ant-design-vue'
import type { TaktTreeSelectOption } from '@/types/common.types'
import request from '@/api/request'
import { createLogger } from '@/utils/logger'
import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  taktIsLeafFlag,
} from '@/composables/useLazyTree'
const treeSelectLogger = createLogger('takt-tree-select')

/** API 树节点告警上限（08-overflow；懒加载按已加载节点计） */
const MAX_TREE_NODES = 5000

const { t } = useI18n()
type TreeValue = string | number
type TreeValueArray = TreeValue[]
type TreeModelValue = TreeValue | TreeValueArray | undefined
/** 业务/API 树节点（与后端 TaktTreeSelectOption 一致） */
type TreeNodeLike = TaktTreeSelectOption
/** 经 fieldNames 映射后传给 a-tree-select 的节点 */
type AntTreeSelectNode = Record<string, unknown>
type TreeSelectOptionLike = Record<string, unknown>
type TreeSelectRefLike = {
  $el?: Element
  $?: { exposed?: { setValue?: (value: TreeValueArray) => void } }
  $emit?: (event: string, ...args: unknown[]) => void
  emit?: (event: string, ...args: unknown[]) => void
}

// 获取 attrs，排除冲突的事件处理器和已定义的 props
const attrs = useAttrs()
const excludedProps = new Set([
  'onUpdate:value', 'onUpdateValue', 
  'allowClear', 'allow-clear',
  'disabled', 'multiple', 'placeholder', 'size',
  'showSearch', 'treeCheckable', 'treeCheckStrictly',
  'treeData', 'value', 'modelValue'
])
const filteredAttrs = computed(() => {
  const result: Record<string, unknown> = {}
  for (const key in attrs) {
    if (!key.startsWith('onUpdate') && !excludedProps.has(key)) {
      result[key] = (attrs as Record<string, unknown>)[key]
    }
  }
  return result
})

interface Props {
  /** 绑定值(支持 v-model 和 v-model:value) */
  modelValue?: TreeValue | TreeValueArray | undefined
  /** 绑定值(v-model:value 的别名) */
  value?: TreeValue | TreeValueArray | undefined
  /** API 端点(可选,如果提供了 treeData 或 dictType 则不需要) */
  apiUrl?: string | undefined
  /** 树形选项数据(可选,如果提供了则直接使用,不再通过 apiUrl 或 dictType 加载) */
  treeData?: TreeNodeLike[] | undefined
  /** 占位符 */
  placeholder?: string | undefined
  /** 是否显示清除按钮 */
  allowClear?: boolean
  /** 是否禁用 */
  disabled?: boolean
  /** 是否多选 */
  multiple?: boolean
  /** 是否显示选中框(多选模式下默认 true) */
  treeCheckable?: boolean | undefined
  /** 父子节点是否关联选择(false 为关联,true 为不关联) */
  treeCheckStrictly?: boolean
  /** 是否显示全选、反选按钮(多选模式下) */
  showCheckAll?: boolean
  /** 是否显示展开、收缩按钮 */
  showExpand?: boolean
  /** 尺寸 */
  size?: 'small' | 'middle' | 'large'
  /** 是否支持搜索 */
  showSearch?: boolean
  /** 树节点过滤属性名(用于搜索) */
  treeNodeFilterProp?: string
  /** 多选标签上限；未传默认 responsive（按宽度溢出 +N） */
  maxTagCount?: number | 'responsive' | undefined
  /** 是否开启虚拟滚动(大数据量时建议开启,可提升渲染性能)。如果不指定,当节点数量超过 100 个时会自动开启 */
  virtual?: boolean
  /** 虚拟滚动时列表高度(单位:px),默认 256px */
  listHeight?: number
  /** 字段映射配置(用于自定义 label、value 和 children 字段名) */
  fieldNames?: {
    label?: string
    value?: string
    children?: string
  }
  /**
   * 懒加载模式（apiUrl 按 parentId 分层拉取）。
   * true=强制懒加载；false=一次拉全树；undefined=根据首包是否含 isLeaf 自动判断
   */
  lazy?: boolean | undefined
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: undefined,
  value: undefined,
  apiUrl: undefined,
  treeData: undefined,
  placeholder: undefined,
  allowClear: true,
  disabled: false,
  multiple: false,
  treeCheckable: undefined, // 默认根据 multiple 自动决定
  treeCheckStrictly: false,
  showCheckAll: true, // 多选模式下默认显示全选、反选按钮
  showExpand: true, // 默认显示展开、收缩按钮
  size: 'middle',
  showSearch: true,
  treeNodeFilterProp: 'title',
  maxTagCount: undefined,
  virtual: false,
  listHeight: 256,
  fieldNames: () => ({
    label: 'title',
    value: 'value',
    children: 'children'
  }),
  lazy: undefined,
})

const emit = defineEmits<{
  'update:modelValue': [value: TreeModelValue]
  'update:value': [value: TreeModelValue]
  'change': [value: TreeModelValue, option: TreeSelectOptionLike | null]
  'search': [value: string]
}>()

const loading = ref(false)
const rawData = ref<TreeNodeLike[]>([])
const expandedKeys = ref<(string | number)[]>([])
const treeSelectRef = ref<TreeSelectRefLike | null>(null) // 预留供外部访问 TreeSelect 组件实例
const dropdownVisible = ref(false)
const actionButtonsRef = ref<HTMLElement | null>(null)
/** 运行时是否启用 api 懒加载（由 prop.lazy 或首包 isLeaf 推断） */
const lazyApiEnabled = ref(false)

// 内部值，优先使用 value（v-model:value），否则使用 modelValue（v-model）
const internalValue = computed(() => props.value ?? props.modelValue)

// 标记 treeSelectRef 已被使用（通过 defineExpose）
void treeSelectRef.value

// 计算 treeCheckable（多选模式下默认启用选中框）
const effectiveTreeCheckable = computed(() => {
  if (props.treeCheckable !== undefined) {
    return props.treeCheckable
  }
  // 多选模式下默认启用选中框
  return props.multiple
})

// 计算树形数据的总节点数（用于决定是否开启虚拟滚动）
const totalNodeCount = computed(() => {
  function countNodes(nodes: TreeNodeLike[]): number {
    let count = 0
    for (const node of nodes) {
      count++
      if (node.children?.length) {
        count += countNodes(node.children)
      }
    }
    return count
  }
  const dataToCount = props.treeData?.length ? props.treeData : rawData.value
  return countNodes(dataToCount)
})

// 根据数据量自动决定是否开启虚拟滚动（超过 100 个节点自动开启）
const shouldUseVirtual = computed(() => {
  return props.virtual === true || totalNodeCount.value > 100
})

/** 多选根 class（宽=基准×2，见 select-base.css） */
const treeSelectRootClass = computed(() => {
  const multiple = !!props.multiple
  const cls = attrs.class
  const text = Array.isArray(cls) ? cls.map(String).join(' ') : String(cls ?? '')
  const fullWidth = /\bw-full\b/.test(text) || /\bmin-w-full\b/.test(text)
  return [
    multiple ? 'takt-tree-select--multiple' : null,
    multiple && !fullWidth ? 'takt-tree-select--multiple-auto-width' : null,
  ]
})

// 多选：未指定时默认 responsive
const effectiveMaxTagCount = computed<number | 'responsive'>(() => {
  return props.maxTagCount ?? 'responsive'
})

// TreeSelect 组件需要的字段名映射
const treeFieldNames = computed(() => ({
  label: props.fieldNames?.label ?? 'title',
  value: props.fieldNames?.value ?? 'value',
  children: props.fieldNames?.children ?? 'children'
}))

/**
 * 推断期望的值类型（根据 modelValue 的类型）
 * @param modelValue 绑定值
 * @returns 期望的值类型：'number' | 'string'
 */
function inferValueType(modelValue: TreeModelValue): 'number' | 'string' {
  if (modelValue === undefined || modelValue === null) {
    return 'string'
  }
  
  if (Array.isArray(modelValue)) {
    if (modelValue.length === 0) {
      return 'string'
    }
    return typeof modelValue[0] === 'number' ? 'number' : 'string'
  }
  
  return typeof modelValue === 'number' ? 'number' : 'string'
}

/**
 * 转换值类型（根据期望的类型转换字典数据的值）
 * @param value 原始值（可能来自 dictValue、extLabel 或 extValue）
 * @param expectedType 期望的类型：'number' | 'string'
 * @param context 上下文信息（用于错误信息）
 * @returns 转换后的值
 */
function convertValueType(value: string | number, expectedType: 'number' | 'string', context: string): string | number {
  if (typeof value === expectedType) {
    return value
  }
  
  if (expectedType === 'number' && typeof value === 'string') {
    if (value === '') {
      treeSelectLogger.warn('树节点值为空字符串，无法转换为 number，返回 0', {
        action: 'convertValueType',
        context,
        value,
      })
      return 0
    }
    
    const numValue = Number(value)
    if (isNaN(numValue)) {
      treeSelectLogger.error('树节点值无法转换为 number', {
        action: 'convertValueType',
        context,
        value,
        expectedType,
      })
      throw new Error(`树形数据值类型转换失败：${context} 的值 "${value}" 无法转换为 number 类型`)
    }
    
    return numValue
  }
  
  if (expectedType === 'string' && typeof value === 'number') {
    return String(value)
  }
  
  return value
}

/**
 * 树节点展示文案：有 i18nKey 则走动态翻译（部门名为 org.dept.{编码}）
 * @param node 树选项
 * @returns {string} 文案
 */
function resolveTreeOptionLabel(node: { i18nKey?: string; dictLabel?: string; title?: string }): string {
  const key = node.i18nKey?.trim()
  if (key) {
    return t(key)
  }
  return String(node.dictLabel ?? node.title ?? '')
}

// 将后端树形数据转换为 TreeSelect 组件需要的格式
function convertToTreeData(tree: TreeNodeLike[]): AntTreeSelectNode[] {
  const { label: labelField, value: valueField, children: childrenField } = treeFieldNames.value
  const expectedValueType = props.apiUrl ? 'string' as const : inferValueType(props.modelValue)

  function convertNode(node: TreeNodeLike): AntTreeSelectNode {
    const label = resolveTreeOptionLabel(node)
    let value = (node.dictValue ?? (node as { value?: string | number }).value ?? '') as string | number
    // apiUrl：后端树选项主键已为 string，与 TaktSelect 一致走 string 转换
    value = props.apiUrl
      ? convertValueType(value, 'string', `树节点 "${label}"`)
      : convertValueType(value, expectedValueType, `树节点 "${label}"`)

    const leaf = taktIsLeafFlag(node.isLeaf)
    const result: AntTreeSelectNode = {
      ...node,
      [labelField]: label,
      [valueField]: value,
    }
    if (lazyApiEnabled.value || props.lazy === true) {
      result.isLeaf = leaf
    }

    if (node.children?.length) {
      result[childrenField] = node.children.map(convertNode)
    } else if (!leaf && (lazyApiEnabled.value || props.lazy === true) && props.apiUrl) {
      // 懒加载非叶子：不设 children，由 loadData 拉取
      delete result[childrenField]
    }

    return result
  }

  return tree.map(convertNode)
}

/**
 * 将 API 一层选项规范为带 isLeaf 的原始节点（children 不设）
 * @param rows API 返回
 */
function normalizeApiLazyOptions(rows: TaktTreeSelectOption[]): TreeNodeLike[] {
  return mapLazyTreeNodes(rows, {
    getKey: (r) => String(r.dictValue ?? ''),
    getTitle: (r) => resolveTreeOptionLabel(r),
    isLeaf: (r) => taktIsLeafFlag(r.isLeaf),
  }).map((n) => {
    const { key: _k, title: _t, children: _c, ...rest } = n
    return {
      ...rest,
      dictValue: n.key,
      dictLabel: n.title,
      isLeaf: n.isLeaf,
    } as TreeNodeLike
  })
}

// 计算最终的树形数据（优先使用 props.treeData，否则使用从 API 加载的数据）
const treeData = computed(() => {
  if (props.treeData && props.treeData.length > 0) {
    return convertToTreeData(props.treeData)
  }
  return convertToTreeData(rawData.value)
})

/**
 * apiUrl 懒加载：展开时按 parentId 拉取一层子节点（无 apiUrl 或外部 treeData / 非懒模式时不绑定）
 * @param treeNode Ant TreeSelect 节点
 */
async function onApiLazyLoadData(treeNode: Record<string, unknown>) {
  if (!props.apiUrl || props.treeData !== undefined || !lazyApiEnabled.value) return
  const valueField = treeFieldNames.value.value
  const dataRef = (treeNode.dataRef ?? treeNode) as Record<string, unknown>
  // ExtValue=实体 Id（如 valueBy=name 时 DictValue=名称）；API parentId 须用 Id
  const apiParentId = dataRef.extValue ?? dataRef[valueField] ?? dataRef.value ?? dataRef.key
  const mergeKey = dataRef[valueField] ?? dataRef.value ?? dataRef.key
  if (apiParentId == null || apiParentId === '' || mergeKey == null || mergeKey === '') return
  const existing = dataRef[treeFieldNames.value.children] as unknown[] | undefined
  if (existing?.length) return
  try {
    const data = await request<TaktTreeSelectOption[]>({
      url: props.apiUrl,
      method: 'get',
      params: { parentId: String(apiParentId) },
    })
    const children = normalizeApiLazyOptions(Array.isArray(data) ? data : [])
    rawData.value = mergeLoadedChildren(
      rawData.value as unknown as Record<string, unknown>[],
      String(mergeKey),
      children as unknown as Record<string, unknown>[],
      { keyField: 'dictValue', childrenField: 'children' },
    ) as unknown as TreeNodeLike[]
  } catch (error) {
    treeSelectLogger.error('懒加载子节点失败', { action: 'apiLazyLoadData', parentId: apiParentId }, error)
  }
}

/** 仅懒模式向 a-tree-select 传入 loadData */
const apiLazyLoadData = computed(() =>
  props.apiUrl && props.treeData === undefined && lazyApiEnabled.value ? onApiLazyLoadData : undefined,
)

/**
 * 根据 prop / 首包推断是否启用懒加载
 * @param rows API 首包
 */
function resolveLazyApiEnabled(rows: TaktTreeSelectOption[]) {
  // 强制：仅显式 lazy=false 才关懒加载；默认/true/自动一律懒加载
  if (props.lazy === false) return false
  if (props.lazy === true) return true
  return true
}

// 加载根层数据（有 treeData 时直接用；懒/自动模式带 parentId=0；lazy=false 不传参以兼容旧全量接口）
const loadData = async () => {
  if (props.treeData !== undefined) {
    return
  }
  if (!props.apiUrl) {
    treeSelectLogger.warn('apiUrl 和 treeData 都未提供，无法加载数据', { action: 'loadData' })
    return
  }

  try {
    loading.value = true
    const data = await request<TaktTreeSelectOption[]>({
      url: props.apiUrl,
      method: 'get',
      ...(props.lazy === false ? {} : { params: { parentId: '0' } }),
    })
    const rows = Array.isArray(data) ? data : []
    lazyApiEnabled.value = resolveLazyApiEnabled(rows)
    rawData.value = lazyApiEnabled.value ? normalizeApiLazyOptions(rows) : rows
    if (totalNodeCount.value > MAX_TREE_NODES) {
      treeSelectLogger.warn('树节点数超过上限', {
        action: 'loadData',
        apiUrl: props.apiUrl,
        count: totalNodeCount.value,
        max: MAX_TREE_NODES,
      })
    }
  } catch (error) {
    treeSelectLogger.error('加载树形数据失败', { action: 'loadData', apiUrl: props.apiUrl }, error)
    rawData.value = []
  } finally {
    loading.value = false
  }
}

// 统一更新值的辅助函数（同时触发 update:value 与 update:modelValue，保证 v-model:value 与 v-model 都能收到选中值）
const updateValue = (value: TreeModelValue) => {
  emit('update:value', value)
  emit('update:modelValue', value)
}

// 处理 update:value 事件（v-model:value 的双向绑定）
const handleUpdateValue = updateValue

// 处理值变化
const handleChange = (value: TreeModelValue, labelList: unknown[], _extra: unknown) => {
  updateValue(value)
  const label = Array.isArray(labelList) && labelList.length > 0 ? String(labelList[0] ?? '').trim() : ''
  emit('change', value, label ? { dictLabel: label, label } : null)
}

// 处理搜索
const handleSearch = (value: string) => {
  emit('search', value)
}

// 处理树节点展开/收缩
const handleTreeExpand = (keys: (string | number)[]) => {
  expandedKeys.value = keys
}

// 处理下拉框显示/隐藏
const handleDropdownVisibleChange = (open: boolean) => {
  dropdownVisible.value = open
  if (open) {
    // 下拉框打开时，将按钮插入到下拉框内部
    // 使用多次 nextTick 和 setTimeout 确保下拉框完全渲染
    nextTick(() => {
      setTimeout(() => {
        insertActionButtons()
      }, 50)
    })
  } else {
    // 下拉框关闭时，移除按钮
    removeActionButtons()
  }
}

// 查找下拉框容器的辅助函数
const findDropdownElement = (): Element | null => {
  // 方式1: 通过自定义类名查找
  let element = document.querySelector('.takt-tree-select-dropdown')
  if (element) return element
  
  // 方式2: 通过 TreeSelect 组件的 ref 查找
  if (treeSelectRef.value) {
    try {
      const selectElement = treeSelectRef.value?.$el || treeSelectRef.value?.$el?.parentElement
      if (selectElement) {
        element = selectElement.closest('.ant-select-dropdown')
        if (element) return element
      }
    } catch {
      // 忽略错误
    }
  }
  
  // 方式3: 查找所有下拉框，选择最后一个（最新打开的）
  const allDropdowns = document.querySelectorAll('.ant-select-dropdown')
  return allDropdowns.length > 0 ? allDropdowns[allDropdowns.length - 1] as Element : null
}

// 创建按钮的辅助函数
const createButton = (text: string, onClick: () => void): HTMLElement => {
  const container = document.createElement('div')
  const buttonVNode = h(Button, {
    type: 'link',
    size: 'small',
    onClick: (e: Event) => {
      e.preventDefault()
      e.stopPropagation()
      onClick()
    }
  }, { default: () => text })
  render(buttonVNode, container)
  return container.firstElementChild as HTMLElement || container
}

// 插入操作按钮到下拉框内部
const insertActionButtons = () => {
  if (!((props.showExpand || (props.multiple && props.showCheckAll)) && treeData.value?.length)) {
    return
  }
  
  const dropdownElement = findDropdownElement()
  if (!dropdownElement) {
    treeSelectLogger.warn('无法找到下拉框容器，尝试延迟重试', { action: 'insertActionButtons' })
    setTimeout(insertActionButtons, 100)
    return
  }
  
  // 检查是否已经插入过按钮（避免重复插入）
  if (dropdownElement.querySelector('.takt-tree-select-dropdown-actions')) {
    return
  }
  
  // 创建按钮容器
  const actionsContainer = document.createElement('div')
  actionsContainer.className = 'takt-tree-select-dropdown-actions'
  
  // 创建按钮
  if (props.showExpand) {
    actionsContainer.appendChild(createButton(t('common.page.button.expand'), handleExpandAll))
    actionsContainer.appendChild(createButton(t('common.page.button.collapse'), handleCollapseAll))
  }
  
  if (props.multiple && props.showCheckAll) {
    actionsContainer.appendChild(createButton(t('common.page.button.checkall'), handleCheckAll))
    actionsContainer.appendChild(
      createButton(`${t('common.page.button.cancel')}${t('common.page.button.all')}`, handleUncheckAll),
    )
  }
  
  // 查找下拉框内容区域或树容器
  const contentElement = dropdownElement.querySelector('.ant-select-dropdown-content') || dropdownElement
  const treeContainer = contentElement.querySelector('.ant-select-tree, .ant-tree, .rc-tree-select-tree, .ant-select-tree-list')
  
  const insertTarget = treeContainer?.parentNode || contentElement || dropdownElement
  const insertBefore = treeContainer || insertTarget.firstChild
  insertTarget.insertBefore(actionsContainer, insertBefore)
  
  actionButtonsRef.value = actionsContainer
}

// 移除操作按钮
const removeActionButtons = () => {
  if (actionButtonsRef.value?.parentNode) {
    actionButtonsRef.value.parentNode.removeChild(actionButtonsRef.value)
    actionButtonsRef.value = null
  }
  
  // 清理所有可能残留的按钮容器
  document.querySelectorAll('.takt-tree-select-dropdown-actions').forEach(el => {
    el.parentNode?.removeChild(el)
  })
}

/**
 * 收集树中所有节点的 key（用于全部展开）
 */
function collectAllKeys(nodes: AntTreeSelectNode[], valueField: string = 'value'): (string | number)[] {
  const keys: (string | number)[] = []
  
  function traverse(nodes: AntTreeSelectNode[]) {
    for (const node of nodes) {
      const key = node[valueField] as string | number | undefined
      if (key != null) {
        keys.push(key)
      }
      const children = node[treeFieldNames.value.children] as AntTreeSelectNode[] | undefined
      if (children?.length) {
        traverse(children)
      }
    }
  }
  
  traverse(nodes)
  return keys
}

/**
 * 全部展开
 */
const handleExpandAll = () => {
  if (!treeData.value?.length) return
  expandedKeys.value = collectAllKeys(treeData.value, treeFieldNames.value.value)
}

/**
 * 全部收缩
 */
const handleCollapseAll = () => {
  expandedKeys.value = []
}

/**
 * 收集树中所有节点的值（用于全选）
 */
function collectAllValues(nodes: AntTreeSelectNode[], valueField: string = 'value'): (string | number)[] {
  const values: (string | number)[] = []
  const childrenField = treeFieldNames.value.children
  
  function traverse(nodes: AntTreeSelectNode[]) {
    for (const node of nodes) {
      // 跳过禁用的节点
      if (node.disabled === true || node.checkable === false) {
        const children = node[childrenField] as AntTreeSelectNode[] | undefined
        if (children?.length) {
          traverse(children)
        }
        continue
      }
      
      const value = node[valueField] as string | number | undefined
      if (value != null) {
        values.push(value)
      }
      
      const children = node[childrenField] as AntTreeSelectNode[] | undefined
      if (children?.length) {
        traverse(children)
      }
    }
  }
  
  traverse(nodes)
  return values
}

// 尝试通过 ref 直接更新 TreeSelect 的值（可选，主要依赖 emit）
const tryUpdateTreeSelectValue = (values: (string | number)[]) => {
  nextTick(() => {
    if (!treeSelectRef.value) return
    
    try {
      const instance = treeSelectRef.value
      if (instance.$?.exposed?.setValue) {
        instance.$.exposed.setValue(values)
      } else if (instance.$emit) {
        instance.$emit('change', values, null)
      } else if (instance.emit) {
        instance.emit('change', values, null)
      }
    } catch {
      treeSelectLogger.debug('无法直接操作 TreeSelect，依赖 emit 更新', { action: 'tryUpdateTreeSelectValue' })
    }
  })
}

/**
 * 全选
 */
const handleCheckAll = () => {
  if (!treeData.value?.length) {
    treeSelectLogger.warn('树数据为空，无法全选', { action: 'checkAll' })
    return
  }
  if (totalNodeCount.value > MAX_TREE_NODES) {
    treeSelectLogger.warn('树节点过多，禁止全选', {
      action: 'checkAll',
      count: totalNodeCount.value,
      max: MAX_TREE_NODES,
    })
    return
  }
  
  const allValues = collectAllValues(treeData.value, treeFieldNames.value.value)
  treeSelectLogger.debug('全选', { action: 'checkAll', count: allValues.length, allValues })
  
  updateValue(allValues)
  emit('change', allValues, null)
  tryUpdateTreeSelectValue(allValues)
}

/**
 * 反选（取消所有选中）
 */
const handleUncheckAll = () => {
  treeSelectLogger.debug('反选，清空所有选中值', { action: 'uncheckAll' })
  const emptyValues: (string | number)[] = []
  
  updateValue(emptyValues)
  emit('change', emptyValues, null)
  tryUpdateTreeSelectValue(emptyValues)
}

// 监听 API URL 和 treeData 变化
watch(() => [props.apiUrl, props.treeData], () => {
  if (!props.treeData?.length && props.apiUrl) {
    loadData()
  }
})

// 监听下拉框显示状态和树数据变化，重新插入按钮
watch([() => dropdownVisible.value, () => treeData.value], () => {
  if (dropdownVisible.value && treeData.value?.length) {
    nextTick(() => {
      setTimeout(() => {
        removeActionButtons()
        insertActionButtons()
      }, 50)
    })
  }
})

onMounted(() => {
  loadData()
})

// 组件卸载时清理按钮
onBeforeUnmount(() => {
  removeActionButtons()
})

// 暴露方法供外部调用
defineExpose({
  checkAll: handleCheckAll,
  uncheckAll: handleUncheckAll,
  expandAll: handleExpandAll,
  collapseAll: handleCollapseAll,
  // 暴露 TreeSelect 组件实例
  treeSelect: treeSelectRef
})
</script>

<style scoped>
.takt-tree-select-wrapper {
  position: relative;
}
</style>

<style>
.takt-tree-select-dropdown .takt-tree-select-dropdown-actions,
.ant-select-dropdown .takt-tree-select-dropdown-actions {
  height: 24px;
  padding: 0 12px;
  border-bottom: 1px solid var(--ant-color-border-secondary, #f0f0f0);
  display: flex;
  align-items: center;
  gap: 8px;
  justify-content: flex-end;
  flex-shrink: 0;
  z-index: 10;
  position: relative;
}

.takt-tree-select-dropdown .ant-select-dropdown-content,
.ant-select-dropdown .ant-select-dropdown-content {
  display: flex;
  flex-direction: column;
}

.takt-tree-select-dropdown .ant-select-tree-list,
.takt-tree-select-dropdown .ant-select-tree,
.takt-tree-select-dropdown .ant-tree,
.takt-tree-select-dropdown .rc-tree-select-tree,
.ant-select-dropdown .ant-select-tree-list,
.ant-select-dropdown .ant-select-tree,
.ant-select-dropdown .ant-tree,
.ant-select-dropdown .rc-tree-select-tree {
  flex: 1;
  overflow-y: auto;
}

.takt-tree-select-dropdown .rc-virtual-list,
.ant-select-dropdown .rc-virtual-list {
  flex: 1;
}
</style>
