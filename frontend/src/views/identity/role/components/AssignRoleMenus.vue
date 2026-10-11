<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/identity/role/components -->
<!-- 文件名称：AssignRoleMenus.vue -->
<!-- 功能描述：分配角色菜单。a-transfer + 双侧 a-tree（官方 children 槽）；左懒加载；右按 ParentId 构树。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.rolemenu._self') })"
    :width="'50vw'"
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
      <a-form-item :label="t('entity.role._self')">
        <a-input
          :value="roleInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.menu._self')"
            :label-col="{ span: 24 }"
            :wrapper-col="{ span: 24 }"
          >
            <TaktTransfer
              v-model:target-keys="selectedMenuIds"
              :data-source="transferDataSource"
              :titles="transferTitles"
              :loading="menuOptionsLoading"
              :disabled="menuOptionsLoading || !roleId"
              hide-select-all
              :show-search="false"
            >
              <template #children="{ direction, selectedKeys, onItemSelect }">
                <a-tree
                  v-if="direction === 'left'"
                  block-node
                  checkable
                  check-strictly
                  :checked-keys="asCheckedKeys(selectedKeys)"
                  :tree-data="leftTreeData"
                  :load-data="handleLeftLoadData"
                  :field-names="{ title: 'title', key: 'key', children: 'children' }"
                  :height="360"
                  @check="(c: unknown) => syncTreeCheck(c, leftTreeData, onItemSelect)"
                >
                  <template #title="node">
                    <span
                      :class="{ 'opacity-50': node.disableCheckbox }"
                      @click.stop="onNodeClick(node, selectedKeys, onItemSelect)"
                      @dblclick.stop="onNodeDblClick(node, 'left')"
                    >{{ node.title }}</span>
                  </template>
                </a-tree>
                <a-tree
                  v-else
                  block-node
                  checkable
                  check-strictly
                  :checked-keys="asCheckedKeys(selectedKeys)"
                  :tree-data="rightTreeData"
                  :field-names="{ title: 'title', key: 'key', children: 'children' }"
                  :height="360"
                  @check="(c: unknown) => syncTreeCheck(c, rightTreeData, onItemSelect)"
                >
                  <template #title="node">
                    <span
                      :class="{ 'opacity-50': node.disableCheckbox }"
                      @click.stop="onNodeClick(node, selectedKeys, onItemSelect)"
                      @dblclick.stop="onNodeDblClick(node, 'right')"
                    >{{ node.title }}</span>
                  </template>
                </a-tree>
              </template>
            </TaktTransfer>
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配角色菜单（Ant Design Vue Transfer + Tree）
 *
 * - dataSource：未分配项 disabled=false（否则 > 无法穿梭）；key 必须齐全
 * - 左：getMenuTree 懒加载；已在 targetKeys 的节点禁勾选
 * - 右：nodeIndex 按 ParentId 构树；同级 SortOrder 排序（与左 getMenuTree 一致）；缺父 getMenuById 补齐
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { TransferProps } from 'ant-design-vue'
import { getRoleMenuIds, assignRoleMenus } from '@/api/identity/rbac.api'
import { getMenuById, getMenuTree } from '@/api/identity/menu.api'
import type { Role } from '@/types/identity/role.types'
import type { MenuTree } from '@/types/identity/menu.types'
import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  type TaktLazyTreeNode,
} from '@/composables/useLazyTree'
import {
  taktAssignTransferHandleItemClick,
  taktAssignTransferShuttleByDblClick,
  type TaktAssignTransferDirection,
} from '@/components/business/TaktTransfer/interaction'

/** 按钮 */
const MENU_TYPE_BUTTON = 2

/** 树节点 */
interface MenuTreeNode extends TaktLazyTreeNode {
  menuType: number
  disableCheckbox?: boolean
}

/** 菜单索引 */
interface MenuNodeInfo {
  id: string
  parentId: string
  title: string
  menuType: number
  /** 与 getMenuTree OrderBy SortOrder 一致 */
  sortOrder: number
}

interface Props {
  /** 是否显示 */
  open?: boolean
  /** 目标角色 */
  role?: Role | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  role: null,
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignRoleMenus')

/** 弹窗 */
const visible = ref(false)
/** 提交中 */
const loading = ref(false)
/** 加载中 */
const menuOptionsLoading = ref(false)
/** 角色 Id */
const roleId = ref('')
/** 角色文案 */
const roleInfo = ref('')
/** Transfer targetKeys = 已分配 */
const selectedMenuIds = ref<string[]>([])
/** id → 父子/标题（构右树 + dataSource） */
const nodeIndex = ref<Map<string, MenuNodeInfo>>(new Map())
/** 左树 */
const leftTreeData = ref<MenuTreeNode[]>([])
/** 右树 */
const rightTreeData = ref<MenuTreeNode[]>([])
/** 打开加载中，忽略 watch */
const syncing = ref(false)

/** 已分配 Set */
const assignedSet = computed(() => new Set(selectedMenuIds.value))

/** 列标题 */
const transferTitles = computed((): [string, string] => [
  t('common.tip.transfer.unassigned'),
  `${t('common.tip.transfer.assigned')} (${selectedMenuIds.value.length})`,
])

/**
 * Transfer dataSource
 * 关键：未分配项不得 disabled，否则操作按钮无法穿梭到右侧
 */
const transferDataSource = computed((): TransferProps['dataSource'] => {
  const items: NonNullable<TransferProps['dataSource']> = []
  const seen = new Set<string>()
  const push = (key: string, title: string) => {
    if (!key || seen.has(key)) return
    seen.add(key)
    items.push({
      key,
      title: title || key,
      // 仅已在右侧的项在左侧不可再选；穿梭本身靠 targetKeys，此处勿禁未分配
      disabled: false,
    })
  }
  for (const [id, info] of nodeIndex.value) {
    push(id, info.title)
  }
  const walk = (list: MenuTreeNode[]) => {
    for (const n of list) {
      push(String(n.key), String(n.title || ''))
      if (n.children?.length) walk(n.children as MenuTreeNode[])
    }
  }
  walk(leftTreeData.value)
  walk(rightTreeData.value)
  for (const id of selectedMenuIds.value) {
    push(id, nodeIndex.value.get(id)?.title || id)
  }
  return items
})

/**
 * 解包树
 * @param res 响应
 * @returns {MenuTree[]}
 */
function unwrapMenuTree(res: unknown): MenuTree[] {
  if (Array.isArray(res)) return res as MenuTree[]
  const anyRes = res as { data?: MenuTree[]; Data?: MenuTree[] }
  return anyRes?.data ?? anyRes?.Data ?? []
}

/**
 * 异常文案
 * @param error 异常
 * @returns {string | undefined}
 */
function getErrorMessage(error: unknown): string | undefined {
  if (error instanceof Error) return error.message
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const msg = (error as { message?: unknown }).message
    return typeof msg === 'string' ? msg : undefined
  }
  return undefined
}

/**
 * 写入索引（ParentId + SortOrder 为左右树统一排序键）
 * @param id Id
 * @param parentId 父
 * @param title 标题
 * @param menuType 类型
 * @param sortOrder 同级排序
 */
function putNode(
  id: string,
  parentId: string,
  title: string,
  menuType: number,
  sortOrder = 0,
): void {
  if (!id) return
  const prev = nodeIndex.value.get(id)
  const name = (title || '').trim()
  const nextSort =
    sortOrder > 0 ? sortOrder : (prev?.sortOrder && prev.sortOrder > 0 ? prev.sortOrder : sortOrder)
  nodeIndex.value.set(id, {
    id,
    parentId: (parentId || prev?.parentId || '0') || '0',
    title: name && name !== id ? name : (prev?.title && prev.title !== id ? prev.title : name || id),
    menuType: Number.isFinite(menuType) ? menuType : (prev?.menuType ?? 0),
    sortOrder: nextSort,
  })
}

/**
 * 从树行写入
 * @param row 行
 */
function putFromTreeRow(row: MenuTree): void {
  putNode(
    String(row.menuId ?? ''),
    String(row.parentId ?? '0') || '0',
    String(row.menuName || row.menuCode || ''),
    Number(row.menuType ?? 0),
    Number(row.sortOrder ?? 0),
  )
}

/**
 * 同级排序：ParentId 分组后按 SortOrder（与 getMenuTree 一致）
 * @param a 节点
 * @param b 节点
 * @returns {number}
 */
function compareByParentIdAndSortOrder(a: MenuTreeNode, b: MenuTreeNode): number {
  const map = nodeIndex.value
  const ia = map.get(String(a.key))
  const ib = map.get(String(b.key))
  const pa = ia?.parentId || '0'
  const pb = ib?.parentId || '0'
  if (pa !== pb) return pa.localeCompare(pb)
  const sa = ia?.sortOrder ?? 0
  const sb = ib?.sortOrder ?? 0
  if (sa !== sb) return sa - sb
  return String(a.key).localeCompare(String(b.key))
}

/**
 * 补齐缺失父级（构树用，含 SortOrder）
 * @returns {Promise<void>}
 */
async function ensureMissingParents(): Promise<void> {
  for (;;) {
    const missing: string[] = []
    for (const info of nodeIndex.value.values()) {
      if (info.parentId && info.parentId !== '0' && !nodeIndex.value.has(info.parentId)) {
        missing.push(info.parentId)
      }
    }
    if (!missing.length) break
    const unique = [...new Set(missing)]
    const settled = await Promise.allSettled(unique.map((id) => getMenuById(id)))
    let added = 0
    settled.forEach((result, i) => {
      if (result.status !== 'fulfilled' || !result.value) return
      const m = result.value
      const id = String(m.menuId ?? unique[i] ?? '')
      if (!id) return
      putNode(
        id,
        String(m.parentId ?? '0') || '0',
        String(m.menuName || m.menuCode || ''),
        Number(m.menuType ?? 0),
        Number(m.sortOrder ?? 0),
      )
      added += 1
    })
    if (!added) break
  }
}

/**
 * 右树：已分配 ∪ 祖先，按 ParentId 挂载，同级 SortOrder 排序
 * @returns {MenuTreeNode[]}
 */
function buildRightTree(): MenuTreeNode[] {
  const map = nodeIndex.value
  const assigned = assignedSet.value
  const visible = new Set<string>()
  for (const id of assigned) {
    visible.add(id)
    let cur = map.get(id)?.parentId || '0'
    const guard = new Set<string>()
    while (cur && cur !== '0' && !guard.has(cur)) {
      guard.add(cur)
      visible.add(cur)
      cur = map.get(cur)?.parentId || '0'
    }
  }

  const nodes = new Map<string, MenuTreeNode>()
  for (const id of visible) {
    const info = map.get(id)
    if (!info) continue
    nodes.set(id, {
      key: id,
      title: info.title,
      menuType: info.menuType,
      disableCheckbox: !assigned.has(id),
      isLeaf: true,
      children: undefined,
    })
  }

  const roots: MenuTreeNode[] = []
  for (const node of nodes.values()) {
    const id = String(node.key)
    const parentId = map.get(id)?.parentId || '0'
    if (parentId !== '0' && nodes.has(parentId)) {
      const parent = nodes.get(parentId)!
      if (!parent.children) parent.children = []
      ;(parent.children as MenuTreeNode[]).push(node)
      parent.isLeaf = false
    } else {
      roots.push(node)
    }
  }
  for (const node of nodes.values()) {
    if (node.children?.length) {
      ;(node.children as MenuTreeNode[]).sort(compareByParentIdAndSortOrder)
    }
  }
  roots.sort(compareByParentIdAndSortOrder)
  return roots
}

/**
 * 刷新右树
 */
function refreshRightTree(): void {
  rightTreeData.value = buildRightTree()
}

/**
 * 左树一层
 * @param rows API 行
 * @returns {MenuTreeNode[]}
 */
function mapLeftRows(rows: MenuTree[]): MenuTreeNode[] {
  const assigned = assignedSet.value
  const kept: MenuTree[] = []
  for (const row of rows) {
    const id = String(row.menuId ?? '')
    if (!id) continue
    putFromTreeRow(row)
    // 已分配按钮不在左侧重复出现；目录/页面保留以便展开
    if (Number(row.menuType ?? 0) === MENU_TYPE_BUTTON && assigned.has(id)) continue
    kept.push(row)
  }
  // 同层：ParentId 已相同，按 SortOrder（与后端 getMenuTree 一致）
  kept.sort((a, b) => {
    const sa = Number(a.sortOrder ?? 0)
    const sb = Number(b.sortOrder ?? 0)
    if (sa !== sb) return sa - sb
    return String(a.menuId ?? '').localeCompare(String(b.menuId ?? ''))
  })
  return (mapLazyTreeNodes(kept, {
    getKey: (row) => String(row.menuId ?? ''),
    getTitle: (row) => String(row.menuName || row.menuCode || row.menuId || ''),
    isLeaf: (row) => Number(row.menuType ?? 0) === MENU_TYPE_BUTTON,
  }) as MenuTreeNode[]).map((n) => {
    const id = String(n.key)
    n.menuType = nodeIndex.value.get(id)?.menuType ?? 0
    n.disableCheckbox = assigned.has(id)
    return n
  })
}

/**
 * 刷新左树禁勾选
 * @param list 节点
 */
function applyLeftDisable(list: MenuTreeNode[]): void {
  const assigned = assignedSet.value
  for (const n of list) {
    n.disableCheckbox = assigned.has(String(n.key))
    if (n.children?.length) applyLeftDisable(n.children as MenuTreeNode[])
  }
}

/**
 * 勾选 keys
 * @param selectedKeys Transfer 勾选
 */
function asCheckedKeys(selectedKeys: readonly (string | number)[]) {
  return {
    checked: selectedKeys.map((k) => String(k)),
    halfChecked: [] as string[],
  }
}

/**
 * 树勾选 → onItemSelect（驱动 Transfer 中间按钮）
 * @param checked 结果
 * @param tree 树
 * @param onItemSelect 回调
 */
function syncTreeCheck(
  checked: unknown,
  tree: MenuTreeNode[],
  onItemSelect: (itemKey: string, selected: boolean) => void,
): void {
  const raw = Array.isArray(checked)
    ? checked
    : ((checked as { checked?: Array<string | number> })?.checked ?? [])
  const next = new Set(raw.map((k) => String(k)))
  const walk = (list: MenuTreeNode[]) => {
    for (const n of list) {
      if (!n.disableCheckbox) onItemSelect(String(n.key), next.has(String(n.key)))
      if (n.children?.length) walk(n.children as MenuTreeNode[])
    }
  }
  walk(tree)
}

/**
 * 单击
 * @param node 节点
 * @param selectedKeys 勾选
 * @param onItemSelect 回调
 */
function onNodeClick(
  node: MenuTreeNode,
  selectedKeys: readonly (string | number)[],
  onItemSelect: (itemKey: string, selected: boolean) => void,
): void {
  if (node.disableCheckbox) return
  taktAssignTransferHandleItemClick(String(node.key), selectedKeys, onItemSelect)
}

/**
 * 双击穿梭
 * @param node 节点
 * @param direction 方向
 */
function onNodeDblClick(node: MenuTreeNode, direction: TaktAssignTransferDirection): void {
  if (node.disableCheckbox) return
  const key = String(node.key)
  taktAssignTransferShuttleByDblClick(
    key,
    direction,
    () => selectedMenuIds.value,
    (keys) => {
      selectedMenuIds.value = keys
    },
  )
}

/**
 * 左懒加载
 * @param treeNode 父
 */
async function handleLeftLoadData(treeNode: { key: string | number }): Promise<void> {
  const parentKey = String(treeNode.key)
  const rows = unwrapMenuTree(await getMenuTree(parentKey, true))
  leftTreeData.value = mergeLoadedChildren(
    leftTreeData.value,
    parentKey,
    mapLeftRows(rows),
  ) as MenuTreeNode[]
}

/**
 * 左根
 * @returns {Promise<void>}
 */
async function loadLeftRoots(): Promise<void> {
  leftTreeData.value = mapLeftRows(unwrapMenuTree(await getMenuTree('0', true)))
}

/**
 * targetKeys 变化后重建右树、刷新左禁勾选
 * @returns {Promise<void>}
 */
async function syncAfterSelectionChange(): Promise<void> {
  await ensureMissingParents()
  refreshRightTree()
  applyLeftDisable(leftTreeData.value)
}

watch(
  () => props.open,
  (val) => {
    visible.value = val
    if (val && props.role) void openDialog()
  },
)

watch(visible, (val) => {
  emit('update:open', val)
})

watch(
  selectedMenuIds,
  () => {
    if (syncing.value || !visible.value) return
    void syncAfterSelectionChange()
  },
  { deep: true },
)

/**
 * 打开
 */
async function openDialog() {
  const role = props.role
  if (!role?.roleId) {
    message.error(t('common.validation.not.found', { field: `${t('entity.role._self')} ID` }))
    return
  }
  try {
    syncing.value = true
    menuOptionsLoading.value = true
    roleId.value = String(role.roleId)
    roleInfo.value = `${role.roleName ?? ''}（${role.roleCode ?? ''}）`
    nodeIndex.value = new Map()
    selectedMenuIds.value = []
    leftTreeData.value = []
    rightTreeData.value = []

    const roleMenus = await getRoleMenuIds(roleId.value)
    const ids: string[] = []
    for (const row of roleMenus) {
      const id = String(row.menuId || '')
      if (!id) continue
      putNode(
        id,
        String(row.parentId ?? '0') || '0',
        String(row.menuName || ''),
        Number(row.menuType ?? 0),
        Number(row.sortOrder ?? 0),
      )
      ids.push(id)
    }
    selectedMenuIds.value = ids
    await ensureMissingParents()
    refreshRightTree()
    await loadLeftRoots()
  } catch (error: unknown) {
    logger.error('[AssignRoleMenus] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.rolemenu._self') }))
  } finally {
    menuOptionsLoading.value = false
    syncing.value = false
  }
}

/**
 * 提交
 */
async function handleSubmit() {
  if (!roleId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.role._self') }))
    return
  }
  try {
    loading.value = true
    await assignRoleMenus(roleId.value, selectedMenuIds.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.rolemenu._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignRoleMenus] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.rolemenu._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭 */
function handleCancel() {
  visible.value = false
  roleId.value = ''
  selectedMenuIds.value = []
  nodeIndex.value = new Map()
  leftTreeData.value = []
  rightTreeData.value = []
  roleInfo.value = ''
}
</script>
