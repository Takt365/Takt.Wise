<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/business/TaktFolderExplorer -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：通用资源管理器式目录浏览器（工具栏历史+地址栏+左树+右表+状态栏）；数据由父级注入 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="takt-folder-explorer flex flex-col border border-border rounded overflow-hidden bg-container"
    :style="{ height: `${panelHeight}px` }"
  >
    <!-- 工具栏：后退 / 前进 / 上级 / 刷新 + 地址栏 -->
    <div class="flex items-center gap-0.5 px-1.5 py-1 border-b border-border bg-page shrink-0">
      <a-tooltip :title="t('components.business.page.folderexplorer.back')">
        <a-button type="text" size="small" :disabled="!canBack" @click="goBack">
          <template #icon><RiArrowLeftLine class="takt-remix-icon" /></template>
        </a-button>
      </a-tooltip>
      <a-tooltip :title="t('components.business.page.folderexplorer.forward')">
        <a-button type="text" size="small" :disabled="!canForward" @click="goForward">
          <template #icon><RiArrowRightLine class="takt-remix-icon" /></template>
        </a-button>
      </a-tooltip>
      <a-tooltip :title="t('components.business.page.folderexplorer.up')">
        <a-button type="text" size="small" :disabled="!canGoUp" @click="emit('up')">
          <template #icon><RiArrowUpLine class="takt-remix-icon" /></template>
        </a-button>
      </a-tooltip>
      <a-tooltip :title="t('common.page.button.refresh')">
        <a-button type="text" size="small" :loading="loading" @click="emit('refresh')">
          <template #icon><RiRefreshLine class="takt-remix-icon" /></template>
        </a-button>
      </a-tooltip>
      <a-tooltip v-if="allowCreate" :title="t('components.business.page.folderexplorer.newfolder')">
        <a-button type="text" size="small" :disabled="!canCreate" @click="openCreateModal">
          <template #icon><RiFolderAddLine class="takt-remix-icon" /></template>
        </a-button>
      </a-tooltip>

      <div
        class="flex-1 min-w-0 ml-1 flex items-center gap-1 rounded border border-border bg-container px-1.5 h-8"
        @dblclick="startAddressEdit"
      >
        <button
          v-if="mode === 'local'"
          type="button"
          class="inline-flex items-center shrink-0 text-text-secondary hover:text-primary cursor-pointer bg-transparent border-0 p-0"
          :title="t('components.business.page.folderexplorer.computer')"
          @click.stop="handleRootClick"
        >
          <RiComputerLine class="takt-remix-icon" />
        </button>
        <template v-if="!addressEditing">
          <div class="flex flex-1 min-w-0 items-center gap-0.5 overflow-x-auto">
            <template v-for="(c, idx) in crumbs" :key="`${c.path}-${idx}`">
              <span v-if="idx > 0" class="text-text-secondary shrink-0 select-none">›</span>
              <button
                type="button"
                class="shrink-0 max-w-[160px] truncate px-1 rounded hover:bg-primary/10 text-sm text-text cursor-pointer bg-transparent border-0"
                @click.stop="handleCrumbClick(c)"
              >
                {{ c.label }}
              </button>
            </template>
            <span
              v-if="!crumbs.length"
              class="text-sm text-text-secondary truncate px-1"
            >{{ resolvedEmptyHint }}</span>
          </div>
          <button
            type="button"
            class="shrink-0 text-text-secondary hover:text-primary cursor-pointer bg-transparent border-0 p-0"
            :title="t('components.business.page.folderexplorer.editaddress')"
            @click.stop="startAddressEdit"
          >
            <RiEditLine class="takt-remix-icon" />
          </button>
        </template>
        <a-input
          v-else
          ref="addressInputRef"
          v-model:value="addressValue"
          size="small"
          class="!border-0 !shadow-none flex-1"
          :placeholder="addressPlaceholder || t('components.business.page.folderexplorer.addressplaceholder')"
          @press-enter="commitAddressEdit"
          @blur="commitAddressEdit"
          @keydown.esc.prevent="cancelAddressEdit"
        />
      </div>
    </div>

    <!-- 主体：左树 + 右表 -->
    <div class="flex flex-1 min-h-0">
      <div
        v-if="showNavTree"
        class="w-[220px] shrink-0 border-r border-border overflow-auto bg-page py-1"
      >
        <a-directory-tree
          v-if="treeData.length"
          block-node
          :tree-data="treeData"
          :expanded-keys="expandedKeys"
          :selecmed-keys="selecmedKeys"
          :field-names="{ title: 'title', key: 'key', children: 'children' }"
          @expand="onTreeExpand"
          @select="onTreeSelect"
        >
          <template #icon="{ dataRef }">
            <RiComputerLine
              v-if="isFolderExplorerLocalRoot(String(dataRef?.key ?? ''))"
              class="takt-remix-icon"
            />
            <RiHardDrive2Line
              v-else-if="isDriveRootKey(String(dataRef?.key ?? ''))"
              class="takt-remix-icon"
            />
            <RiFolderLine v-else class="takt-remix-icon" />
          </template>
        </a-directory-tree>
        <div v-else class="px-3 py-2 text-xs text-text-secondary">
          {{ resolvedEmptyHint }}
        </div>
      </div>

      <div class="flex-1 min-w-0 overflow-hidden">
        <a-spin :spinning="loading" class="h-full">
          <a-empty
            v-if="!loading && items.length === 0"
            class="mt-10"
            :description="emptyListHint"
          />
          <a-table
            v-else-if="items.length"
            size="small"
            :pagination="false"
            :show-header="true"
            :row-key="(r: TaktFolderExplorerItem) => r.fullPath"
            :data-source="items"
            :columns="columns"
            :custom-row="customRow"
            :row-class-name="rowClassName"
            :scroll="{ y: tableScrollY }"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'name'">
                <div class="flex items-center gap-2 min-w-0">
                  <RiHardDrive2Line
                    v-if="isDriveRootKey(rowFullPath(record))"
                    class="takt-remix-icon shrink-0 text-primary"
                  />
                  <RiFolderLine
                    v-else-if="record.isDirectory !== false"
                    class="takt-remix-icon shrink-0 text-primary"
                  />
                  <RiFileLine v-else class="takt-remix-icon shrink-0 text-text-secondary" />
                  <span class="truncate">{{ record.name }}</span>
                </div>
              </template>
              <template v-else-if="column.key === 'modified'">
                <span class="text-text-secondary">{{ formatModified(record.modifiedTime) }}</span>
              </template>
              <template v-else-if="column.key === 'type'">
                <span class="text-text-secondary">{{ formatItemType(record) }}</span>
              </template>
            </template>
          </a-table>
        </a-spin>
      </div>
    </div>

    <!-- 状态栏 -->
    <div class="flex items-center justify-between gap-2 px-2 py-1 border-t border-border text-xs text-text-secondary shrink-0 bg-page">
      <span class="shrink-0">
        {{ t('components.business.page.folderexplorer.itemcount', { count: items.length }) }}
      </span>
      <span class="truncate text-right" :title="statusPath">{{ statusPath }}</span>
    </div>

    <TaktModal
      v-model:open="createModalOpen"
      :title="t('components.business.page.folderexplorer.newfolder')"
      :use-viewport-size="false"
      width="400px"
      @ok="confirmCreateFolder"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('components.business.page.folderexplorer.newfoldername')" required>
          <a-input
            v-model:value="createFolderName"
            :placeholder="t('components.business.page.folderexplorer.newfolderplaceholder')"
            allow-clear
            @press-enter="confirmCreateFolder"
          />
        </a-form-item>
      </a-form>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 通用资源管理器式目录浏览器：左导航树 + 工具栏历史 + 地址栏面包屑 + 多列列表。
 * 浏览数据由父组件加载后传入；本组件维护导航历史与左树缓存。
 */
import { computed, nextTick, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import {
  RiArrowLeftLine,
  RiArrowRightLine,
  RiArrowUpLine,
  RiComputerLine,
  RiEditLine,
  RiFileLine,
  RiFolderAddLine,
  RiFolderLine,
  RiHardDrive2Line,
  RiRefreshLine,
} from '@remixicon/vue'
import type {
  TaktFolderExplorerCrumb,
  TaktFolderExplorerItem,
  TaktFolderExplorerPathMode,
  TaktFolderExplorerTreeNode,
} from '@/types/components/folderExplorer.types'
import {
  buildFolderExplorerCrumbs,
  isFolderExplorerLocalRoot,
  readFolderExplorerItemFullPath,
  TAKT_FOLDER_EXPLORER_LOCAL_ROOT,
} from '@/utils/taktFolderExplorerPath'

/** i18n */
const { t } = useI18n()

const props = withDefaults(defineProps<{
  /** 加载中 */
  loading?: boolean
  /** 当前路径 */
  currentPath?: string
  /** 上级路径；null 表示不可再上 */
  parentPath?: string | null
  /** 列表项 */
  items?: TaktFolderExplorerItem[]
  /** 当前选中路径 */
  selecmedPath?: string
  /** 路径模式 */
  mode?: TaktFolderExplorerPathMode
  /** 地址栏占位 */
  addressPlaceholder?: string
  /** 无路径提示 */
  emptyHint?: string
  /** 是否显示左导航树 */
  showNavTree?: boolean
  /** 面板总高度 */
  panelHeight?: number
  /** 表格可视高度（滚动区） */
  tableScrollY?: number
  /** 是否显示新建文件夹 */
  allowCreate?: boolean
  /** 当前是否可新建 */
  canCreate?: boolean
}>(), {
  mode: 'local',
  items: () => [],
  showNavTree: true,
  allowCreate: false,
  canCreate: false,
  panelHeight: 460,
  tableScrollY: 320,
})

const emit = defineEmits<{
  /** 导航到路径（面包屑/树/历史/地址栏） */
  navigate: [path: string]
  /** 进入目录（双击） */
  enter: [path: string]
  /** 上级 */
  up: []
  /** 刷新 */
  refresh: []
  /** 单击选中 */
  select: [path: string]
  /** 地址栏跳转 */
  go: [path: string]
  /** 新建文件夹（名称） */
  'create-folder': [name: string]
}>()

/** 地址栏编辑态 */
const addressEditing = ref(false)
/** 地址栏文本 */
const addressValue = ref('')
/** 地址输入框 */
const addressInputRef = ref<{ focus?: () => void } | null>(null)
/** 左树数据 */
const treeData = ref<TaktFolderExplorerTreeNode[]>([])
/** 展开键 */
const expandedKeys = ref<string[]>([])
/** 选中键 */
const selecmedKeys = ref<string[]>([])
/** 历史栈 */
const historyStack = ref<string[]>([])
/** 历史指针 */
const historyIndex = ref(-1)
/** 历史回退/前进时抑制 push */
const suppressHistoryPush = ref(false)
/** 新建文件夹弹窗 */
const createModalOpen = ref(false)
/** 新建文件夹名 */
const createFolderName = ref('')

const items = computed(() => props.items ?? [])

const canGoUp = computed(() => props.parentPath !== null && props.parentPath !== undefined)

const canBack = computed(() => historyIndex.value > 0)

const canForward = computed(() =>
  historyIndex.value >= 0 && historyIndex.value < historyStack.value.length - 1,
)

const resolvedEmptyHint = computed(
  () => props.emptyHint || t('components.business.page.folderexplorer.roothint'),
)

const emptyListHint = computed(() => {
  if (props.mode === 'local' && isFolderExplorerLocalRoot(props.currentPath)) {
    return t('components.business.page.folderexplorer.nodrives')
  }
  if (!props.currentPath && props.mode !== 'local') {
    return props.emptyHint || t('components.business.page.folderexplorer.notconnected')
  }
  return t('components.business.page.folderexplorer.emptyfolder')
})

const crumbs = computed(() =>
  buildFolderExplorerCrumbs(
    props.currentPath,
    props.mode || 'local',
    t('components.business.page.folderexplorer.computer'),
  ),
)

const statusPath = computed(() => {
  const selecmed = String(props.selecmedPath || '').trim()
  if (selecmed && !isFolderExplorerLocalRoot(selecmed)) {
    return selecmed
  }
  const current = String(props.currentPath || '').trim()
  if (current && !isFolderExplorerLocalRoot(current)) {
    return current
  }
  return props.mode === 'local'
    ? t('components.business.page.folderexplorer.computer')
    : resolvedEmptyHint.value
})

const columns = computed(() => [
  {
    title: t('components.business.page.folderexplorer.namecolumn'),
    key: 'name',
    dataIndex: 'name',
    ellipsis: true,
  },
  {
    title: t('components.business.page.folderexplorer.modifiedcolumn'),
    key: 'modified',
    width: 160,
  },
  {
    title: t('components.business.page.folderexplorer.typecolumn'),
    key: 'type',
    width: 100,
  },
])

watch(
  () => props.currentPath,
  (v) => {
    if (!addressEditing.value) {
      addressValue.value = isFolderExplorerLocalRoot(v) ? '' : String(v || '')
    }
    pushHistoryIfNeeded(toHistoryKey(v))
    syncTreeWithBrowse()
  },
  { immediate: true },
)

watch(
  () => props.items,
  () => {
    syncTreeWithBrowse()
  },
  { deep: true },
)

watch(
  () => props.selecmedPath,
  (v) => {
    const key = toHistoryKey(v || props.currentPath)
    if (key) {
      selecmedKeys.value = [key]
    }
  },
)

/**
 * @param path 原始路径
 * @returns 历史/树用键
 */
function toHistoryKey(path: string | null | undefined): string {
  if (props.mode === 'local' && isFolderExplorerLocalRoot(path)) {
    return TAKT_FOLDER_EXPLORER_LOCAL_ROOT
  }
  if (props.mode === 'ftp' && (!path || path === '')) {
    return '/'
  }
  return String(path ?? '').trim()
}

/**
 * @param key 历史键
 */
function pushHistoryIfNeeded(key: string) {
  if (!key) {
    return
  }
  if (suppressHistoryPush.value) {
    suppressHistoryPush.value = false
    return
  }
  if (historyStack.value[historyIndex.value] === key) {
    return
  }
  if (historyIndex.value < historyStack.value.length - 1) {
    historyStack.value = historyStack.value.slice(0, historyIndex.value + 1)
  }
  historyStack.value.push(key)
  historyIndex.value = historyStack.value.length - 1
}

function goBack() {
  if (!canBack.value) {
    return
  }
  suppressHistoryPush.value = true
  historyIndex.value -= 1
  emitNavigate(historyStack.value[historyIndex.value])
}

function goForward() {
  if (!canForward.value) {
    return
  }
  suppressHistoryPush.value = true
  historyIndex.value += 1
  emitNavigate(historyStack.value[historyIndex.value])
}

/**
 * @param path 目标
 */
function emitNavigate(path: string) {
  emit('navigate', path)
  emit('go', path)
}

function handleRootClick() {
  addressEditing.value = false
  addressValue.value = ''
  emitNavigate(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
}

/**
 * @param crumb 节点
 */
function handleCrumbClick(crumb: TaktFolderExplorerCrumb) {
  const path = String(crumb?.path ?? '')
  addressEditing.value = false
  addressValue.value = isFolderExplorerLocalRoot(path) ? '' : path
  emitNavigate(path)
}

async function startAddressEdit() {
  addressEditing.value = true
  addressValue.value = isFolderExplorerLocalRoot(props.currentPath)
    ? ''
    : String(props.currentPath || '')
  await nextTick()
  addressInputRef.value?.focus?.()
}

function cancelAddressEdit() {
  addressEditing.value = false
  addressValue.value = isFolderExplorerLocalRoot(props.currentPath)
    ? ''
    : String(props.currentPath || '')
}

function commitAddressEdit() {
  if (!addressEditing.value) {
    return
  }
  addressEditing.value = false
  const raw = addressValue.value?.trim() || ''
  if (props.mode === 'local' && isFolderExplorerLocalRoot(raw)) {
    emitNavigate(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
    return
  }
  if (!raw && props.mode === 'local') {
    emitNavigate(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
    return
  }
  if (!raw) {
    return
  }
  emitNavigate(raw)
}

/**
 * @param path 路径
 * @returns 是否盘符根（C: / C:\ / C:/）
 */
function isDriveRootKey(path: string): boolean {
  return /^[A-Za-z]:[\\/]?$/.test(String(path || '').trim())
}

function openCreateModal() {
  createFolderName.value = ''
  createModalOpen.value = true
}

function confirmCreateFolder() {
  const name = createFolderName.value.trim().replace(/[\\/]/g, '')
  if (!name) {
    return
  }
  createModalOpen.value = false
  emit('create-folder', name)
}

/**
 * @param record 行
 * @returns 完整路径
 */
function rowFullPath(record: TaktFolderExplorerItem) {
  return readFolderExplorerItemFullPath(record as TaktFolderExplorerItem & { FullPath?: string })
}

/**
 * @param record 行
 */
function customRow(record: TaktFolderExplorerItem) {
  return {
    onClick: () => {
      const full = rowFullPath(record)
      addressValue.value = full
      emit('select', full)
    },
    onDblclick: () => {
      const full = rowFullPath(record)
      addressValue.value = full
      if (record.isDirectory === false) {
        emit('select', full)
        return
      }
      emit('enter', full)
    },
  }
}

/**
 * @param record 行
 * @returns class
 */
function rowClassName(record: TaktFolderExplorerItem) {
  return props.selecmedPath === rowFullPath(record)
    ? 'bg-primary/10 cursor-pointer'
    : 'cursor-pointer'
}

/**
 * @param value ISO 或空
 * @returns 显示
 */
function formatModified(value: string | null | undefined) {
  if (!value) {
    return '—'
  }
  const d = new Date(value)
  if (Number.isNaN(d.getTime())) {
    return '—'
  }
  const pad = (n: number) => String(n).padStart(2, '0')
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())} ${pad(d.getHours())}:${pad(d.getMinutes())}`
}

/**
 * @param record 行
 * @returns 类型文案
 */
function formatItemType(record: TaktFolderExplorerItem) {
  if (isDriveRootKey(rowFullPath(record))) {
    return t('components.business.page.folderexplorer.drivetype')
  }
  if (record.isDirectory === false) {
    return t('components.business.page.folderexplorer.filetype')
  }
  return t('components.business.page.folderexplorer.foldertype')
}

/**
 * 将当前浏览结果合并进左树（强制替换树引用以触发 DirectoryTree 刷新）
 */
function syncTreeWithBrowse() {
  if (!props.showNavTree) {
    return
  }
  ensureTreeRoot()
  const parentKey = toHistoryKey(props.currentPath)
  ensurePathNodes(parentKey)
  const snapshot = cloneTreeNodes(treeData.value)
  const node = findTreeNode(snapshot, parentKey)
  if (!node) {
    treeData.value = snapshot
    return
  }
  const prevChildren = node.children || []
  node.children = items.value
    .filter((it) => it.isDirectory !== false)
    .map((it) => {
      const key = normalizeTreeKey(it.fullPath)
      const prev = prevChildren.find((c) => normalizeTreeKey(c.key) === key)
      return {
        key: it.fullPath,
        title: it.name,
        isLeaf: false,
        children: prev?.children,
      }
    })
  treeData.value = snapshot
  expandAncestors(parentKey)
  selecmedKeys.value = [parentKey]
}

/**
 * @param nodes 树
 * @returns 深拷贝
 */
function cloneTreeNodes(nodes: TaktFolderExplorerTreeNode[]): TaktFolderExplorerTreeNode[] {
  return nodes.map((n) => ({
    key: n.key,
    title: n.title,
    isLeaf: n.isLeaf,
    children: n.children ? cloneTreeNodes(n.children) : undefined,
  }))
}

/**
 * @param key 路径键
 * @returns 规范化比较键
 */
function normalizeTreeKey(key: string): string {
  return String(key || '').replace(/[/\\]+$/, '').toLowerCase()
}

function ensureTreeRoot() {
  if (treeData.value.length) {
    return
  }
  if (props.mode === 'local') {
    treeData.value = [{
      key: TAKT_FOLDER_EXPLORER_LOCAL_ROOT,
      title: t('components.business.page.folderexplorer.computer'),
      children: [],
    }]
    expandedKeys.value = [TAKT_FOLDER_EXPLORER_LOCAL_ROOT]
    return
  }
  if (props.mode === 'ftp') {
    treeData.value = [{
      key: '/',
      title: '/',
      children: [],
    }]
    expandedKeys.value = ['/']
    return
  }
  const path = String(props.currentPath || '').trim()
  if (path) {
    const rootLabel = crumbs.value[0]?.label || path
    const rootKey = crumbs.value[0]?.path || path
    treeData.value = [{
      key: rootKey,
      title: rootLabel,
      children: [],
    }]
    expandedKeys.value = [rootKey]
  }
}

/**
 * @param targetKey 确保从根到目标的路径节点存在
 */
function ensurePathNodes(targetKey: string) {
  if (!targetKey || isFolderExplorerLocalRoot(targetKey)) {
    return
  }
  if (props.mode === 'local') {
    ensureTreeRoot()
    const chain = buildFolderExplorerCrumbs(
      targetKey,
      'local',
      t('components.business.page.folderexplorer.computer'),
    )
    for (let i = 0; i < chain.length; i += 1) {
      const cur = chain[i]
      if (isFolderExplorerLocalRoot(cur.path)) {
        continue
      }
      const parent = i === 0 || isFolderExplorerLocalRoot(chain[i - 1].path)
        ? TAKT_FOLDER_EXPLORER_LOCAL_ROOT
        : chain[i - 1].path
      const parentNode = findTreeNode(treeData.value, parent)
      if (!parentNode) {
        continue
      }
      if (!parentNode.children) {
        parentNode.children = []
      }
      if (!parentNode.children.some((c) => c.key === cur.path)) {
        parentNode.children.push({
          key: cur.path,
          title: cur.label,
          isLeaf: false,
          children: [],
        })
      }
    }
    return
  }
  if (props.mode === 'ftp' || props.mode === 'unc') {
    ensureTreeRoot()
    const chain = buildFolderExplorerCrumbs(targetKey, props.mode)
    for (let i = 1; i < chain.length; i += 1) {
      const parent = chain[i - 1].path
      const cur = chain[i]
      const parentNode = findTreeNode(treeData.value, parent)
      if (!parentNode) {
        continue
      }
      if (!parentNode.children) {
        parentNode.children = []
      }
      if (!parentNode.children.some((c) => c.key === cur.path)) {
        parentNode.children.push({
          key: cur.path,
          title: cur.label,
          isLeaf: false,
          children: [],
        })
      }
    }
  }
}

/**
 * @param key 展开祖先
 */
function expandAncestors(key: string) {
  const keys = new Set(expandedKeys.value)
  if (props.mode === 'local') {
    keys.add(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
    const chain = buildFolderExplorerCrumbs(
      isFolderExplorerLocalRoot(key) ? '' : key,
      'local',
      t('components.business.page.folderexplorer.computer'),
    )
    for (const c of chain) {
      keys.add(c.path)
    }
  } else {
    keys.add(key)
    const chain = buildFolderExplorerCrumbs(key, props.mode || 'custom')
    for (const c of chain) {
      keys.add(c.path)
    }
  }
  expandedKeys.value = [...keys]
}

/**
 * @param nodes 树
 * @param key 键
 * @returns 节点
 */
function findTreeNode(
  nodes: TaktFolderExplorerTreeNode[],
  key: string,
): TaktFolderExplorerTreeNode | null {
  for (const n of nodes) {
    if (n.key === key) {
      return n
    }
    if (n.children?.length) {
      const found = findTreeNode(n.children, key)
      if (found) {
        return found
      }
    }
  }
  return null
}

/**
 * @param keys 展开键
 */
function onTreeExpand(keys: (string | number)[]) {
  expandedKeys.value = keys.map(String)
  const last = expandedKeys.value[expandedKeys.value.length - 1]
  if (last && last !== toHistoryKey(props.currentPath)) {
    const node = findTreeNode(treeData.value, last)
    if (node && (!node.children || node.children.length === 0)) {
      emitNavigate(last)
    }
  }
}

/**
 * @param keys 选中键
 */
function onTreeSelect(keys: (string | number)[]) {
  const key = String(keys[0] ?? '')
  if (!key) {
    return
  }
  selecmedKeys.value = [key]
  emitNavigate(key)
}
</script>
