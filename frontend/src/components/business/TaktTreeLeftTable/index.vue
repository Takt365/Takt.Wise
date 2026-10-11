<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktTreeLeftTable
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:左树区域；外框与右表同高撑满父级；空数据居中暂无数据；点击节点仅选中；三角手风琴展开（不绑 a-tree accordion，以便工具栏完整展开）

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->

<template>
  <div
    class="takt-tree-left-table"
    :style="leftStyle"
  >
    <div
      ref="viewportRef"
      class="takt-tree-left-table__viewport"
      :class="{
        'takt-tree-left-table__viewport--virtual': effectiveVirtual,
      }"
    >
      <a-tree
        v-if="!showEmpty"
        :expanded-keys="expandedKeys"
        v-model:selected-keys="selectedKeys"
        class="takt-tree-left-table__tree"
        :class="{ 'draggable-tree': draggable }"
        :tree-data="rawTreeData"
        :field-names="fieldNames"
        :block-node="blockNode"
        :show-line="showLine"
        :selectable="selectable"
        :draggable="draggable"
        :expand-action="expandAction"
        :accordion="false"
        :virtual="effectiveVirtual"
        :item-height="treeItemHeightPx"
        :load-data="loadData"
        v-bind="effectiveVirtual ? { height: treeScrollYPx } : {}"
        @expand="handleExpand"
        @select="handleSelect"
        @dragenter="onDragEnter"
        @drop="onDrop"
      >
        <template
          v-if="$slots.title"
          #title="{ title, key, dataRef }"
        >
          <slot
            :key="key"
            name="title"
            :title="title"
            :data-ref="dataRef"
          />
        </template>
      </a-tree>
      <div
        v-else-if="showEmpty"
        class="takt-tree-left-table__empty"
      >
        <a-empty :description="t('common.status.empty')" />
      </div>
      <div
        v-if="loading"
        class="takt-tree-left-table__loading"
      >
        <a-spin :spinning="true" />
      </div>
    </div>
    <div
      v-if="showFooterRemark"
      class="takt-tree-left-table__footer-remark shrink-0 px-1 pt-2 text-sm leading-relaxed text-text-secondary"
    >
      <slot name="footerRemark">
        {{ footerRemark }}
      </slot>
    </div>
  </div>
</template>

<script setup lang="ts">
import {
  countTreeNodesForVirtualScroll,
  resolveVerticalScrollY,
  shouldUseTableVirtualScroll,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TREE_LEFT_VIRTUAL_HEIGHT_FALLBACK,
  type TaktTableScrollLayout,
} from '@/utils/tableScroll'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { useTaktFillHeightScrollY } from '@/composables/useTaktFillHeightScrollY'
import { useI18n } from 'vue-i18n'

type TreeNode = Record<string, unknown> & {
  key: string | number
  children?: TreeNode[]
  isLeaf?: boolean
}
type TreeSelectEvent = Record<string, unknown>

export interface TreeFieldNames {
  title?: string
  key?: string
  children?: string
}

interface Props {
  /** 树数据 */
  treeData?: TreeNode[]
  /** 树字段映射 */
  treeFieldNames?: TreeFieldNames
  /** 当前展开的节点 key 列表（v-model:expanded-keys） */
  expandedKeys?: (string | number)[]
  /** 当前选中的节点 key 列表（v-model:selected-keys） */
  selectedKeys?: (string | number)[]
  /** 左侧宽度比例（相对内容视口），如 0.2 表示内容视口的 1/5，即 20% */
  treeWidthRatio?: number
  /** 加载状态 */
  loading?: boolean
  /** 是否节点占满一行 */
  blockNode?: boolean
  /** 是否显示连接线 */
  showLine?: boolean
  /** 是否可选 */
  selectable?: boolean
  /** 是否开启虚拟滚动(由页面控制,大数据展开时建议开启) */
  virtual?: boolean
  /**
   * 滚动配置（与右表同一语义；a-tree 无 scroll API）
   * y：纵向视口高度(px)；未传则由 scrollLayout 计算
   */
  scroll?: { y?: number | string }
  /** 布局场景（默认 treeLeft，与右表 treeRight 同一套 chrome 预留） */
  scrollLayout?: TaktTableScrollLayout
  /** 虚拟滚动列表高度(px)；等价于 scroll.y 覆盖（兼容旧用法） */
  height?: number
  /** 虚拟滚动单项高度(px)，默认 28 与 a-tree 行高对齐，避免展开时逐行测量 */
  itemHeight?: number
  /** 是否开启拖拽排序/变更父节点（由页面控制，与 virtual 独立） */
  draggable?: boolean
  /** 点击节点标题时展开/收起子级（false 则仅点击三角，标题用于选中右表） */
  expandAction?: 'click' | 'doubleclick' | false
  /** 手风琴：同级仅允许展开一个节点（仅自定义 handleExpand，不使用 a-tree accordion） */
  accordion?: boolean
  /** 表尾备注说明（树区域下方） */
  footerRemark?: string
  /**
   * 懒加载子节点（Ant Design Tree loadData）。
   * 传入后树进入懒模式：非叶子须 isLeaf=false 且 children 为 undefined。
   */
  loadData?: (treeNode: Record<string, unknown>) => Promise<void>
}

const props = withDefaults(defineProps<Props>(), {
  treeData: () => [],
  treeFieldNames: () => ({ title: 'title', key: 'key', children: 'children' }),
  expandedKeys: () => [],
  selectedKeys: () => [],
  treeWidthRatio: 0.2,
  loading: false,
  blockNode: true,
  showLine: false,
  selectable: true,
  virtual: true,
  scroll: undefined,
  scrollLayout: 'treeLeft',
  draggable: false,
  expandAction: false,
  accordion: true,
  itemHeight: 28,
  footerRemark: '',
  loadData: undefined,
})

const slots = useSlots()
const { t } = useI18n()

/** 树视口 DOM（撑满父级后实测高度） */
const viewportRef = ref<HTMLElement | null>(null)

/** 无数据且非加载中：显示与右表一致的空状态 */
const showEmpty = computed(
  () => !props.loading && !(props.treeData?.length),
)

/** 是否展示表尾备注 */
const showFooterRemark = computed(
  () => !!props.footerRemark?.trim() || !!slots.footerRemark,
)

/**
 * 把 scroll.y 转成 a-tree.height 所需像素
 * @param {number | string} y 视口高度
 * @returns {number} 像素
 */
function toTreeScrollYPx(y: number | string): number {
  if (typeof y === 'number' && Number.isFinite(y) && y > 0) {
    return Math.floor(y)
  }
  const parsed = Number.parseInt(String(y), 10)
  if (Number.isFinite(parsed) && parsed > 0) {
    return parsed
  }
  return TAKT_TREE_LEFT_VIRTUAL_HEIGHT_FALLBACK
}

/** 窗口回退 scroll.y（容器尚未布局时） */
const viewportScrollYPx = useTaktTableViewportScrollY(computed(() => props.scrollLayout ?? 'treeLeft'))

/** 填满父级后的实测高度（与右表外框同高） */
const fillHeightScrollYPx = useTaktFillHeightScrollY(viewportRef, {
  fallbackPx: viewportScrollYPx,
  recalcToken: computed(() => [props.loading, props.treeData?.length ?? 0, showFooterRemark.value]),
})

/** 生效 scroll.y：scroll.y > height > 容器实测 */
const resolvedScrollY = computed(() =>
  resolveVerticalScrollY(
    props.scroll?.y ?? (props.height != null && props.height > 0 ? props.height : undefined),
    fillHeightScrollYPx.value,
  ),
)

/** 传给 a-tree.height 的像素高度 */
const treeScrollYPx = computed(() =>
  Math.max(TAKT_TABLE_SCROLL_Y_MIN, toTreeScrollYPx(resolvedScrollY.value)),
)

export interface TreeDropPayload {
  newTreeData: TreeNode[]
  dragKey: string | number
  dropKey: string | number
  dropToGap: boolean
  dropPosition: number
  dragNode: TreeNode
  dropNode: TreeNode
}

const emit = defineEmits<{
  'update:expandedKeys': [keys: (string | number)[]]
  'update:selectedKeys': [keys: (string | number)[]]
  'tree-select': [selectedKeys: (string | number)[], e: TreeSelectEvent]
  'tree-drop': [payload: TreeDropPayload]
}>()

const expandedKeys = computed(() => props.expandedKeys ?? [])

const selectedKeys = computed({
  get: () => props.selectedKeys ?? [],
  set: (val) => emit('update:selectedKeys', val)
})

const fieldNames = computed(() => ({
  title: props.treeFieldNames?.title ?? 'title',
  key: props.treeFieldNames?.key ?? 'key',
  children: props.treeFieldNames?.children ?? 'children'
}))

/**
 * 交给 a-tree 的非代理树：页面 ref 深代理会导致展开时 flatten 全树走 Proxy，大数据必卡
 */
const rawTreeData = computed(() => {
  const data = props.treeData
  if (!data?.length) return []
  return toRaw(data)
})

/** 虚拟列表行高（px） */
const treeItemHeightPx = computed(() => {
  const h = props.itemHeight
  if (typeof h === 'number' && Number.isFinite(h) && h > 0) return Math.floor(h)
  return 28
})

/** 树节点总数（达阈值即停计，仅用于是否强制 virtual；不截断树数据） */
const treeNodeCount = computed(() =>
  countTreeNodesForVirtualScroll(
    props.treeData as Record<string, unknown>[] | undefined,
    fieldNames.value.children || 'children',
  ),
)

/** 是否启用虚拟滚动：显式 true，或节点数超过 5000 */
const effectiveVirtual = computed(() =>
  shouldUseTableVirtualScroll(treeNodeCount.value, props.virtual),
)

/** 左侧宽度：内容视口的 1/5（treeWidthRatio 0.2 = 20%） */
const leftStyle = computed(() => {
  const ratio = (props.treeWidthRatio ?? 0.2) * 100
  return {
    flex: `0 0 ${ratio}%`,
    width: `${ratio}%`,
    maxWidth: `${ratio}%`,
    height: '100%',
    minHeight: 0,
    alignSelf: 'stretch',
  }
})

const handleSelect = (keys: (string | number)[], e: TreeSelectEvent) => {
  emit('tree-select', keys, e)
}

/**
 * 查找与目标节点同级的全部 key（含自身；根节点同级为整层根）
 * @param nodes 树数据
 * @param targetKey 目标节点 key
 * @returns {(string | number)[]} 同级 key；未找到则为空数组
 */
function findSiblingKeys(nodes: TreeNode[], targetKey: string | number): (string | number)[] {
  const keyF = fieldNames.value.key
  const chF = fieldNames.value.children
  const keyStr = String(targetKey)
  const readKey = (node: TreeNode): string | number | undefined => {
    const raw = node[keyF]
    if (raw == null || String(raw) === '') return undefined
    return raw as string | number
  }
  const layerKeys = (layer: TreeNode[]): (string | number)[] =>
    layer.map(readKey).filter((k): k is string | number => k != null)
  if (nodes.some((n) => String(readKey(n) ?? '') === keyStr)) {
    return layerKeys(nodes)
  }
  const stack = [...nodes]
  while (stack.length > 0) {
    const node = stack.pop()
    if (!node) continue
    const children = (node[chF] as TreeNode[] | undefined) ?? []
    if (children.some((c) => String(readKey(c) ?? '') === keyStr)) {
      return layerKeys(children)
    }
    for (let i = 0; i < children.length; i += 1) {
      const child = children[i]
      if (child) stack.push(child)
    }
  }
  return []
}

/**
 * 展开/收起：手风琴模式下展开某一节点时收起其同级已展开项。
 * 非手风琴：展开时与当前 expandedKeys 合并（a-tree 批量展开会多次 @expand 且 keys 不完整，否则工具栏「全部展开」会被盖成只剩一项）。
 * @param keys a-tree 给出的展开 key
 * @param info 展开节点信息
 */
function handleExpand(
  keys: (string | number)[],
  info: { expanded?: boolean; node?: Record<string, unknown> },
): void {
  const node = info?.node ?? {}
  const dataRef = (node.dataRef ?? node.data) as Record<string, unknown> | undefined
  const keyF = fieldNames.value.key
  const nodeKey = ([node.eventKey, node.key, dataRef?.[keyF], dataRef?.key] as unknown[])
    .find((k) => k != null && String(k) !== '') as string | number | undefined

  if (props.accordion && info?.expanded === true && nodeKey != null && String(nodeKey) !== '') {
    const siblings = findSiblingKeys(rawTreeData.value ?? [], nodeKey)
    if (siblings.length === 0) {
      emit('update:expandedKeys', keys)
      return
    }
    const siblingSet = new Set(siblings.map((k) => String(k)))
    const next = keys.filter((k) => String(k) === String(nodeKey) || !siblingSet.has(String(k)))
    emit('update:expandedKeys', next)
    return
  }

  if (!props.accordion && info?.expanded === true) {
    const merged = new Set((props.expandedKeys ?? []).map((k) => String(k)))
    for (const k of keys) {
      if (k != null && String(k) !== '') merged.add(String(k))
    }
    if (nodeKey != null && String(nodeKey) !== '') merged.add(String(nodeKey))
    emit('update:expandedKeys', Array.from(merged))
    return
  }

  emit('update:expandedKeys', keys)
}

/** 深拷贝树节点（保留 key/title/children 等字段） */
function deepCloneTree(arr: TreeNode[]): TreeNode[] {
  if (!arr?.length) return []
  return arr.map(item => {
    const next = { ...item } as TreeNode
    const ch = fieldNames.value.children
    const children = next[ch] as TreeNode[] | undefined
    if (children?.length) {
      next[ch] = deepCloneTree(children)
    }
    return next
  })
}

/** 在树中查找节点并执行 callback,用于删除或插入;找到并执行后返回 true */
function loop(
  data: TreeNode[],
  key: string | number,
  callback: (item: TreeNode, index: number, arr: TreeNode[]) => void
): boolean {
  const keyF = fieldNames.value.key
  const chF = fieldNames.value.children
  for (let i = 0; i < data.length; i++) {
    const item = data[i]
    if (!item) continue
    
    const itemKey = item[keyF]
    if (itemKey !== undefined && String(itemKey) === String(key)) {
      callback(item, i, data)
      return true
    }
    const children = item[chF] as TreeNode[] | undefined
    if (children?.length && loop(children, key, callback)) {
      return true
    }
  }
  return false
}

function onDragEnter(info: { expandedKeys: (string | number)[] }) {
  if (props.draggable && info?.expandedKeys?.length) {
    emit('update:expandedKeys', info.expandedKeys)
  }
}

function onDrop(info: {
  node: { key: string | number; pos?: string; children?: TreeNode[]; expanded?: boolean }
  dragNode: { key: string | number }
  dropPosition: number
  dropToGap?: boolean
}) {
  if (!props.draggable) return
  const dropKey = info.node.key
  const dragKey = info.dragNode.key
  const posStr = info.node.pos ?? ''
  const dropPos = posStr.split('-')
  const dropPosition = info.dropPosition - Number(dropPos[dropPos.length - 1] ?? 0)
  const chF = fieldNames.value.children

  const data = deepCloneTree(props.treeData ?? [])
  let dragObj: TreeNode | null = null

  loop(data, dragKey, (item: TreeNode, index: number, arr: TreeNode[]) => {
    arr.splice(index, 1)
    dragObj = item
  })

  if (dragObj == null) return

  if (!info.dropToGap) {
    loop(data, dropKey, (item: TreeNode) => {
      const children = (item[chF] as TreeNode[] | undefined) ?? []
      item[chF] = [dragObj, ...children]
    })
  } else if (
    (info.node.children ?? []).length > 0 &&
    info.node.expanded &&
    dropPosition === 1
  ) {
    loop(data, dropKey, (item: TreeNode) => {
      const children = (item[chF] as TreeNode[] | undefined) ?? []
      item[chF] = [dragObj, ...children]
    })
  } else {
    let ar: TreeNode[] = []
    let i = 0
    loop(data, dropKey, (_item: TreeNode, index: number, arr: TreeNode[]) => {
      ar = arr
      i = index
    })
    if (dropPosition === -1) {
      ar.splice(i, 0, dragObj)
    } else {
      ar.splice(i + 1, 0, dragObj)
    }
  }

  emit('tree-drop', {
    newTreeData: data,
    dragKey,
    dropKey,
    dropToGap: info.dropToGap ?? false,
    dropPosition: info.dropPosition,
    dragNode: info.dragNode,
    dropNode: info.node
  })
}
</script>

<style scoped>
/* 左树外框与右表同高撑满 wrap；内部滚动由 a-tree.height / overflow 承担 */
.takt-tree-left-table {
  min-width: 160px;
  min-height: 0;
  height: 100%;
  align-self: stretch;
  margin: 0;
  overflow: hidden;
  padding: 0;
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
}

.takt-tree-left-table__viewport {
  position: relative;
  flex: 1;
  min-height: 0;
  min-width: 0;
  height: 100%;
  overflow: hidden;
}

.takt-tree-left-table__viewport:not(.takt-tree-left-table__viewport--virtual) {
  overflow-x: hidden;
  overflow-y: auto;
  scrollbar-gutter: stable;
}

.takt-tree-left-table__viewport--virtual {
  overflow: hidden;
}

.takt-tree-left-table__loading {
  position: absolute;
  inset: 0;
  z-index: 20;
  display: flex;
  align-items: center;
  justify-content: center;
  background: color-mix(in srgb, var(--ant-color-bg-container) 65%, transparent);
}

.takt-tree-left-table__empty {
  position: absolute;
  inset: 0;
  z-index: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 16px;
  box-sizing: border-box;
}

.takt-tree-left-table__tree {
  min-height: 0;
}
</style>
