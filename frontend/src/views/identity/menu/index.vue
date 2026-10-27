<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/identity/menu -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：菜单管理页面，包含菜单列表、查询、新增、编辑、删除、导入、导出等 -->
<!-- ======================================== -->

<template>
  <div class="identity-menu">
    <!-- 第一行：左 1/4 树查询栏 | 右 3/4 表查询栏 -->
    <div class="menu-query-row">
      <TaktTreeLeftQueryBar
        v-model="treeQueryKeyword"
        @search="handleTreeQuerySearch"
      />
      <TaktTreeRightQueryBar
        v-model="queryKeyword"
        :placeholder="t('common.page.form.placeholder.search', { keyword: t('entity.menu.name') + t('common.tip.or') + t('entity.menu.code') })"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
    </div>

    <!-- 第二行：左 1/4 树工具栏 | 右 3/4 表工具栏 -->
    <div class="menu-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadData"
      />
      <TaktTreeRightToolsBar
        create-permission="identity:menu:create"
        update-permission="identity:menu:update"
        delete-permission="identity:menu:delete"
        import-permission="identity:menu:import"
        export-permission="identity:menu:export"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-import="true"
        :show-export="true"
        :show-advanced-query="true"
        :show-column-setting="true"
        :show-fullscreen="true"
        :show-refresh="true"
        :show-expand="true"
        :update-disabled="!selecmedRow"
        :delete-disabled="!selecmedRow && selecmedRows.length === 0"
        :create-loading="loading"
        :update-loading="loading"
        :delete-loading="loading"
        :refresh-loading="loading"
        :expanded="tableExpanded"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
        @import="handleImport"
        @export="handleExport"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 第三行：左 1/4 树 | 右 3/4 树表 -->
    <div class="menu-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selecmed-keys="selecmedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="true"
        :accordion="false"
        :expand-action="false"
        :load-data="handleLeftTreeLoadData"
        @tree-select="handleTreeSelect"
        @tree-drop="handleMenuTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="tenant"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'menuId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableFilteredTree"
        v-model:expanded-row-keys="tableExpandedRowKeys"
        :load-children="handleRightTreeLoadChildren"
        :loading="listLoading"
        :row-key="getMenuId"
        :stripe="true"
        :row-selection="rowSelection"
        :virtual="true"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'menuName'">
            {{ getMenuField(record, 'menuName') }}
          </template>
          <template v-else-if="column.key === 'icon'">
            <span
              class="inline-flex items-center gap-1"
              :title="String(getMenuField(record, 'icon') ?? '')"
            >
              <TaktRemixIcon
                :name="String(getMenuField(record, 'icon') ?? '')"
                :size="18"
                :show-placeholder="!!getMenuField(record, 'icon')"
              />
            </span>
          </template>
          <template v-else-if="column.key === 'menuType'">
            <TaktDictTag
              :value="getMenuField(record, 'menuType')"
              dict-type="sys_menu_type"
            />
          </template>
          <template v-else-if="column.key === 'menuStatus'">
            <a-switch
              :checked="getMenuField(record, 'menuStatus') === 1"
              :disabled="getMenuField(record, 'isBuiltIn') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':menuStatus'))"
              @change="(checked: unknown) => handleMenuStatusSwitch(record, Boolean(checked) ? 1 : 0)"
            />
          </template>
          <template v-else-if="column.key === 'isVisible'">
            <a-switch
              :checked="getMenuField(record, 'isVisible') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':isVisible'))"
              @change="(checked: unknown) => handleMenuSwitch(record, 'isVisible', Boolean(checked) ? 1 : 0)"
            />
          </template>
          <template v-else-if="column.key === 'isCached'">
            <a-switch
              :checked="getMenuField(record, 'isCached') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':isCached'))"
              @change="(checked: unknown) => handleMenuSwitch(record, 'isCached', Boolean(checked) ? 1 : 0)"
            />
          </template>
          <template v-else-if="column.key === 'isExternal'">
            <a-switch
              :checked="getMenuField(record, 'isExternal') === 1"
              :checked-children="t('common.page.button.enable')"
              :un-checked-children="t('common.page.button.disable')"
              :loading="Boolean(switchLoadingMap.get(getMenuId(record) + ':isExternal'))"
              @change="(checked: unknown) => handleMenuSwitch(record, 'isExternal', Boolean(checked) ? 1 : 0)"
            />
          </template>
        </template>
      </TaktTreeRightTable>
    </div>

    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MenuForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.menu.name')">
        <a-input v-model:value="advancedQueryForm.menuName" />
      </a-form-item>
      <a-form-item :label="t('entity.menu.code')">
        <a-input v-model:value="advancedQueryForm.menuCode" />
      </a-form-item>
      <a-form-item :label="t('entity.menu.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.menuType"
          dict-type="sys_menu_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.type') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.menu.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.menuStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.status') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.menu._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.menu._self"
        file-type="xlsx"
        :sheet-name="menuExcelNames.sheet"
        :template-file-name="menuExcelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>

    <AssignMenuRoles
      v-model:open="assignMenuRolesVisible"
      :menu="currentAssignMenu"
      @success="handleAssignSuccess"
    />

    <TaktColumnDrawer
      entity-scope="tenant"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'menuId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import {
  filterTaktTreeTableNodes,
  collectTaktTreeTableExpandableKeys,
  expandTaktLazyTreeFully,
  runWithTaktTreeLoadConcurrency,
  taktTreeExpandedKeysEqual,
  taktTreeTableNodeKey,
  type TaktTreeTableNode,
} from '@/utils/taktTreeTable'
import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  taktIsLeafFlag,
  type TaktLazyTreeNode,
} from '@/composables/useLazyTree'
import MenuForm from './components/MenuForm.vue'
import AssignMenuRoles from './components/AssignMenuRoles.vue'
import { getMenuTree, getMenuById, createMenu, updateMenu, updateMenuStatus, deleteMenuById, getMenuTemplate, importMenu, exportMenu } from '@/api/identity/menu.api'
import type { Menu, MenuQuery, MenuTree } from '@/types/identity/menu.types'
import type { TreeDropPayload } from '@/components/business/TaktTreeLeftTable/index.vue'
import { taktExcelEntityNames } from '@/utils/naming'
import { RiEditLine, RiDeleteBinLine, RiUserSettingsLine } from '@remixicon/vue'
import { TaktMenuType } from '@/utils/common'

const { t } = useI18n()
const menuExcelNames = taktExcelEntityNames('TaktMenu')

const queryKeyword = ref('')
const treeQueryKeyword = ref('')
/** 左侧树结构展开状态：true=全部展开，false=全部折叠（仅左侧工具栏控制），默认收缩 */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表（受控传给 TaktTreeTable） */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧树表工具栏「全部展开/收缩」 */
const tableExpanded = ref(false)
/** 右侧 a-table 树表当前展开行 key（与 row-key / menuId 一致） */
const tableExpandedRowKeys = ref<(string | number)[]>([])
/** 左侧树 loading */
const loading = ref(false)
/** 右侧树 loading */
const listLoading = ref(false)
/** 左侧 a-tree 数据（懒加载仅已展开路径） */
const entityTreeData = ref<TaktLazyTreeNode[]>([])
/** 右侧树表数据源（带 children / _hasChildren，组件内拍平 virtual） */
const tableTreeData = ref<Record<string, unknown>[]>([])
const selecmedTreeKeys = ref<(string | number)[]>([])
const selecmedRow = ref<Menu | null>(null)
const selecmedRows = ref<Menu[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Menu>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
const advancedQueryVisible = ref(false)
/** 高级查询（字段与 MenuQuery 一致） */
type MenuAdvancedQueryForm = Pick<MenuQuery, 'menuName' | 'menuCode' | 'menuType' | 'menuStatus'>
const emptyMenuAdvancedQueryForm = (): MenuAdvancedQueryForm => ({ menuName: '', menuCode: '' })
const advancedQueryForm = ref<MenuAdvancedQueryForm>(emptyMenuAdvancedQueryForm())
const importVisible = ref(false)
const assignMenuRolesVisible = ref(false)
const currentAssignMenu = ref<Menu | null>(null)
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
/** 开关列加载状态：key 为 `${menuId}:${field}` */
const switchLoadingMap = ref(new Map<string, boolean>())

/** 菜单类型：0=目录，1=菜单，2=按钮；左侧导航树排除类型为 2 的节点 */
function isMenuTypeButton(n: { menuType?: unknown; type?: unknown; MenuType?: unknown }): boolean {
  const typeVal = n?.menuType ?? n?.type ?? n?.MenuType
  return typeVal === TaktMenuType.Button || Number(typeVal) === TaktMenuType.Button
}

/** 左侧导航树：过滤按钮类型 */
function filterNavMenuRows(rows: MenuTree[]): MenuTree[] {
  return (rows ?? []).filter((n) => !isMenuTypeButton(n))
}

const getMenuId = (record: { menuId?: string; id?: string } | null | undefined): string =>
  record?.menuId != null ? String(record.menuId) : (record?.id != null ? String(record.id) : '')

/** 从树 API 响应中取出一层节点 */
function unwrapMenuTree(res: unknown): MenuTree[] {
  const resAny = res as { data?: MenuTree[]; Data?: MenuTree[] }
  if (Array.isArray(res)) return res as MenuTree[]
  return resAny?.data ?? resAny?.Data ?? []
}

/**
 * 将树 API 一层 DTO 映射为左侧懒加载节点（排除按钮类型）
 * @param rows 一层子节点
 */
function mapMenuLazyNodes(rows: MenuTree[]): TaktLazyTreeNode[] {
  return mapLazyTreeNodes(filterNavMenuRows(rows), {
    getKey: (n) => String(n.menuId ?? ''),
    getTitle: (n) => String(n.menuName || n.menuCode || n.menuId || ''),
    isLeaf: (n) => taktIsLeafFlag(n.isLeaf),
  })
}

/**
 * 将一层 DTO 映射为右侧树表节点（未加载子级用 _hasChildren 显示展开箭头）
 * @param rows 一层子节点
 */
function mapMenuRightTreeNodes(rows: MenuTree[]): Record<string, unknown>[] {
  return (rows ?? []).map((row) => {
    const rec = row as Record<string, unknown>
    const id = String(rec.menuId ?? '')
    const rawChildren = Array.isArray(rec.children) ? rec.children as MenuTree[] : []
    const children = rawChildren.length > 0 ? mapMenuRightTreeNodes(rawChildren) : undefined
    return {
      ...rec,
      key: id,
      children,
      _hasChildren: (children != null && children.length > 0) || !taktIsLeafFlag(rec.isLeaf),
    }
  })
}

/**
 * 按关键字过滤左侧树：仅过滤已加载节点
 * @param nodes 树节点
 * @param keyword 关键字
 */
function filterTreeByKeyword(nodes: TaktLazyTreeNode[], keyword: string): TaktLazyTreeNode[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  function filter(list: TaktLazyTreeNode[]): TaktLazyTreeNode[] {
    if (!list?.length) return []
    return list
      .map((node) => {
        const title = String(node.title ?? '').toLowerCase()
        const matched = title.includes(k)
        const filteredChildren = node.children?.length ? filter(node.children as TaktLazyTreeNode[]) : undefined
        const hasMatchInChildren = filteredChildren != null && filteredChildren.length > 0
        if (matched || hasMatchInChildren) {
          if (filteredChildren != null && filteredChildren.length > 0) {
            return { ...node, children: filteredChildren }
          }
          const { children: _omitChildren, ...rest } = node
          return rest as TaktLazyTreeNode
        }
        return null
      })
      .filter(Boolean) as TaktLazyTreeNode[]
  }
  return filter(nodes)
}

/** 左侧树绑定数据（按 treeQueryKeyword 客户端过滤已加载节点） */
const filteredTreeData = computed(() =>
  filterTreeByKeyword(entityTreeData.value, treeQueryKeyword.value)
)

/**
 * 收集左侧可展开 key（含尚未拉子的非叶子，供工具栏一次展开逐层 loadData）
 * @param nodes 树节点
 */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  return collectTaktTreeTableExpandableKeys(
    nodes,
    (node) => taktTreeTableNodeKey(node, 'menuId'),
    { includeUnloaded: true },
  )
}

/** 展开态下把当前树中全部可展开节点写入 expandedKeys */
function applyLeftTreeExpandKeys() {
  const next = collectTreeExpandableKeys(filteredTreeData.value)
  if (taktTreeExpandedKeysEqual(treeExpandedKeys.value, next)) return
  treeExpandedKeys.value = next
}

/**
 * 当前左侧选中节点 Id；未选中返回 null（右表必须为空）
 * @returns {string | null} 选中节点 Id
 */
function getSelecmedTreeNodeId(): string | null {
  const keys = selecmedTreeKeys.value
  if (keys.length > 0 && keys[keys.length - 1] != null) {
    return String(keys[keys.length - 1])
  }
  return null
}

/**
 * 加载右侧树：未选中则空；选中则该节点 + 直接子级一层（更深靠展开懒加载）
 */
async function loadRightTree() {
  const selecmedId = getSelecmedTreeNodeId()
  if (!selecmedId) {
    tableTreeData.value = []
    tableExpandedRowKeys.value = []
    tableExpanded.value = false
    return
  }
  listLoading.value = true
  try {
    const detail = await getMenuById(selecmedId) as Record<string, unknown>
    const childRows = unwrapMenuTree(await getMenuTree(selecmedId, true))
    const mappedChildren = mapMenuRightTreeNodes(childRows)
    const children = mappedChildren.length > 0 ? mappedChildren : undefined
    const root: Record<string, unknown> = {
      ...detail,
      key: selecmedId,
      children,
      _hasChildren: (children?.length ?? 0) > 0 || !taktIsLeafFlag(detail.isLeaf),
    }
    tableTreeData.value = [root]
    rightExpandEpoch += 1
    if (tableExpanded.value) {
      tableExpanded.value = false
      await nextTick()
    }
    tableExpandedRowKeys.value =
      (children?.length ?? 0) > 0 || root._hasChildren === true ? [selecmedId] : []
  } catch (error: unknown) {
    logger.error('[Menu] 加载右侧树失败', undefined, error)
    message.error((error as { message?: string })?.message || t('common.feedback.load.data.failed'))
    tableTreeData.value = []
  } finally {
    listLoading.value = false
  }
}

/**
 * 按父级 Id 拉取并合并右侧一层子节点
 * @param parentId 父节点 Id
 */
async function loadRightChildrenByParentId(parentId: string) {
  if (!parentId) return
  const trees = unwrapMenuTree(await getMenuTree(parentId, true))
  const children = mapMenuRightTreeNodes(trees)
  tableTreeData.value = mergeLoadedChildren(
    tableTreeData.value as TaktLazyTreeNode[],
    parentId,
    children as TaktLazyTreeNode[],
    { keyField: 'menuId' },
  )
}

/**
 * 右侧树展开：再拉一层子节点（懒加载，并发受限）
 * @param record 当前行
 */
async function handleRightTreeLoadChildren(record: Record<string, unknown>) {
  const id = getMenuId(record)
  if (!id) return
  await runWithTaktTreeLoadConcurrency(async () => {
    await loadRightChildrenByParentId(id)
  })
}

/** 加载左侧树根节点（仅 GET tree?parentId=0，一层） */
async function reloadLeftTreeRoots() {
  const trees = unwrapMenuTree(await getMenuTree('0', true))
  entityTreeData.value = mapMenuLazyNodes(trees)
  treeExpandedKeys.value = []
  treeExpanded.value = false
}

/**
 * 重新加载指定父节点下已展开的子节点（CRUD 后局部刷新）
 * @param parentKey 父节点 key
 */
async function reloadLeftTreeChildren(parentKey: string) {
  if (!parentKey || parentKey === '0') {
    await reloadLeftTreeRoots()
    return
  }
  const trees = unwrapMenuTree(await getMenuTree(parentKey, true))
  const children = mapMenuLazyNodes(trees)
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    parentKey,
    children,
  ) as TaktLazyTreeNode[]
}

/**
 * 按父级 Id 拉取并合并左侧一层子节点
 * @param parentId 父节点 Id
 * @returns {Promise<TaktLazyTreeNode[]>} 子节点
 */
async function loadLeftChildrenByParentId(parentId: string): Promise<TaktLazyTreeNode[]> {
  if (!parentId) return []
  const trees = unwrapMenuTree(await getMenuTree(parentId, true))
  const children = mapMenuLazyNodes(trees)
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    parentId,
    children,
  ) as TaktLazyTreeNode[]
  return children
}

/**
 * 左侧树懒加载子节点（Ant Design loadData；并发受限）
 * @param treeNode Ant Design Tree 节点
 */
async function handleLeftTreeLoadData(treeNode: Record<string, unknown>) {
  const dataRef = (treeNode.dataRef ?? treeNode) as Record<string, unknown>
  const key = dataRef.key ?? treeNode.key
  if (key == null) return
  if (Array.isArray(dataRef.children) && dataRef.children.length > 0) return
  await runWithTaktTreeLoadConcurrency(async () => {
    dataRef.children = await loadLeftChildrenByParentId(String(key))
  })
}

/** 右侧展示树：未选中为空；选中后为该节点+直接子级 */
const tableDisplayTree = computed(() => {
  if (!getSelecmedTreeNodeId()) return []
  return tableTreeData.value
})

const getMenuField = (record: any, field: string): any => record?.[field]

/** 右侧查询条件过滤（仅影响表格展示） */
function matchesMenuRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    const code = String(record.menuCode ?? '').toLowerCase()
    const name = String(record.menuName ?? '').toLowerCase()
    if (!code.includes(k) && !name.includes(k)) return false
  }
  const adv = advancedQueryForm.value
  if (adv.menuName && !String(record.menuName ?? '').includes(adv.menuName)) return false
  if (adv.menuCode && !String(record.menuCode ?? '').includes(adv.menuCode)) return false
  if (adv.menuType !== undefined && record.menuType !== adv.menuType) return false
  if (adv.menuStatus !== undefined && record.menuStatus !== adv.menuStatus) return false
  return true
}

/** 右侧过滤后的树（保留 children，供 a-table 展开/收缩） */
const tableFilteredTree = computed(() =>
  filterTaktTreeTableNodes(tableDisplayTree.value, matchesMenuRightQuery)
)

/** 同步右侧树表 expandable keys（展开态；含未加载非叶子） */
function applyMenuTableExpandState() {
  if (!tableExpanded.value) {
    tableExpandedRowKeys.value = []
    return
  }
  const next = collectTaktTreeTableExpandableKeys(
    tableFilteredTree.value,
    (node) => taktTreeTableNodeKey(node, 'menuId'),
    { includeUnloaded: true },
  )
  if (!taktTreeExpandedKeysEqual(tableExpandedRowKeys.value, next)) {
    tableExpandedRowKeys.value = next
  }
}

/** 右侧工具栏展开任务世代 */
let rightExpandEpoch = 0

/** 右侧一次全部展开：按层拉齐当前右表子树 */
async function expandRightTreeFully() {
  const epoch = (rightExpandEpoch += 1)
  await expandTaktLazyTreeFully({
    getNodes: () => tableFilteredTree.value as TaktTreeTableNode[],
    getKey: (node) => taktTreeTableNodeKey(node, 'menuId'),
    setExpandedKeys: (keys) => {
      if (epoch !== rightExpandEpoch) return
      if (!taktTreeExpandedKeysEqual(tableExpandedRowKeys.value, keys)) {
        tableExpandedRowKeys.value = keys
      }
    },
    loadChildren: async (parentId) => {
      await loadRightChildrenByParentId(parentId)
    },
    isActive: () => tableExpanded.value && epoch === rightExpandEpoch,
  })
}

watch(tableExpanded, async (expanded) => {
  if (!expanded) {
    rightExpandEpoch += 1
    tableExpandedRowKeys.value = []
    return
  }
  await expandRightTreeFully()
})

watch(tableFilteredTree, () => {
  if (tableExpanded.value) {
    applyMenuTableExpandState()
  }
})

/** 左侧工具栏展开任务世代 */
let leftExpandEpoch = 0

watch(treeExpanded, async (expanded) => {
  if (!expanded) {
    leftExpandEpoch += 1
    treeExpandedKeys.value = []
    return
  }
  const epoch = (leftExpandEpoch += 1)
  await nextTick()
  if (epoch !== leftExpandEpoch || !treeExpanded.value) return
  await expandTaktLazyTreeFully({
    getNodes: () => filteredTreeData.value as TaktTreeTableNode[],
    getKey: (node) => taktTreeTableNodeKey(node, 'menuId'),
    setExpandedKeys: (keys) => {
      if (epoch !== leftExpandEpoch) return
      if (!taktTreeExpandedKeysEqual(treeExpandedKeys.value, keys)) {
        treeExpandedKeys.value = keys
      }
    },
    loadChildren: async (parentId) => {
      await loadLeftChildrenByParentId(parentId)
    },
    isActive: () => treeExpanded.value && epoch === leftExpandEpoch,
  })
})

watch(filteredTreeData, () => {
  if (treeExpanded.value) {
    applyLeftTreeExpandKeys()
  }
})

/** 左侧树选中：右表展示该节点+直接子级；取消选中则右表清空 */
const handleTreeSelect = (selecmedKeys: (string | number)[]) => {
  selecmedTreeKeys.value = selecmedKeys
  void loadRightTree()
}

/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0',
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.menuId ?? '')
    if (k === keyStr) {
      return { parentId: parentKey, sortOrder: i }
    }
    const children = (node?.children as Array<Record<string, unknown>> | undefined) ?? []
    if (children.length) {
      const found = findParentAndOrderNum(children, targetKey, k)
      if (found) return found
    }
  }
  return null
}

const handleMenuTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  try {
    loading.value = true
    entityTreeData.value = newTreeData as TaktLazyTreeNode[]
    const full = await getMenuById(String(dragKey))
    await updateMenu(String(dragKey), {
      ...full,
      menuId: String(full.menuId ?? dragKey),
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    })
    message.success(t('identity.menu.page.msg.orderupdated'))
    await loadData()
  } catch (error: any) {
    message.error(error?.message ?? t('common.feedback.failed'))
    await reloadLeftTreeRoots().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

function setSwitchLoading(menuId: string, field: string, value: boolean) {
  const key = `${menuId}:${field}`
  const next = new Map(switchLoadingMap.value)
  if (value) next.set(key, true)
  else next.delete(key)
  switchLoadingMap.value = next
}

/** 可见/缓存/外联 开关变更：调用 update 部分更新后刷新列表 */
async function handleMenuSwitch(record: Menu, field: 'isVisible' | 'isCached' | 'isExternal', value: number) {
  const id = getMenuId(record)
  if (!id) return
  setSwitchLoading(id, field, true)
  try {
    const full = await getMenuById(id)
    await updateMenu(id, {
      ...full,
      menuId: id,
      [field]: value,
    })
    message.success(t('common.feedback.updated'))
    await refreshAfterMutation()
  } catch (error: any) {
    message.error(error?.message ?? t('common.feedback.failed'))
  } finally {
    setSwitchLoading(id, field, false)
  }
}

/** 状态开关变更：调用 updateStatus 后刷新列表 */
async function handleMenuStatusSwitch(record: any, menuStatus: number) {
  const id = getMenuId(record)
  if (!id) return
  setSwitchLoading(id, 'menuStatus', true)
  try {
    await updateMenuStatus({ menuId: id, menuStatus })
    message.success(t('common.feedback.updated'))
    await refreshAfterMutation()
  } catch (error: any) {
    message.error(error?.message ?? t('common.feedback.failed'))
  } finally {
    setSwitchLoading(id, 'menuStatus', false)
  }
}

/** 左侧树关键字搜索（客户端过滤已加载节点） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    applyLeftTreeExpandKeys()
  }
}

/** 加载左树根；默认不选中、右表为空 */
async function loadData() {
  loading.value = true
  try {
    selecmedTreeKeys.value = []
    tableTreeData.value = []
    tableExpandedRowKeys.value = []
    tableExpanded.value = false
    await reloadLeftTreeRoots()
  } catch (error: unknown) {
    logger.error('[Menu] 加载数据失败', undefined, error)
    message.error((error as { message?: string })?.message || t('common.feedback.load.data.failed'))
    entityTreeData.value = []
    tableTreeData.value = []
  } finally {
    loading.value = false
  }
}

/** CRUD / 状态变更后：刷新左树当前层 + 按选中重载右表 */
async function refreshAfterMutation() {
  loading.value = true
  try {
    const selecmedId = getSelecmedTreeNodeId()
    await reloadLeftTreeChildren(selecmedId ?? '0')
    await loadRightTree()
  } catch (error: unknown) {
    logger.error('[Menu] 刷新失败', undefined, error)
    message.error((error as { message?: string })?.message || t('common.feedback.load.data.failed'))
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadData()
})

useTableRefresh(loadData)

// 列顺序与 Menu 类型字段一致
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'menuId',
    key: 'menuId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left'
  },
  {
    title: t('entity.menu.code'),
    dataIndex: 'menuCode',
    key: 'menuCode',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.name'),
    dataIndex: 'menuName',
    key: 'menuName',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.l10nkey'),
    dataIndex: 'i18nKey',
    key: 'i18nKey',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.icon'),
    dataIndex: 'icon',
    key: 'icon',
    width: 90,
    ellipsis: true
  },
  {
    title: t('entity.menu.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 90,
    ellipsis: true
  },
  {
    title: t('entity.menu.level'),
    dataIndex: 'level',
    key: 'level',
    width: 70
  },
  {
    title: t('entity.menu.path'),
    dataIndex: 'menuPath',
    key: 'menuPath',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.isleaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 90
  },
  {
    title: t('entity.menu.type'),
    dataIndex: 'menuType',
    key: 'menuType',
    width: 80
  },
  {
    title: t('entity.menu.permission'),
    dataIndex: 'permission',
    key: 'permission',
    width: 140,
    ellipsis: true
  },
  {
    title: t('entity.menu.routepath'),
    dataIndex: 'routePath',
    key: 'routePath',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.component'),
    dataIndex: 'componentPath',
    key: 'componentPath',
    width: 160,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 80
  },
  {
    title: t('entity.menu.isexternal'),
    dataIndex: 'isExternal',
    key: 'isExternal',
    width: 80
  },
  {
    title: t('entity.menu.linkurl'),
    dataIndex: 'externalUrl',
    key: 'externalUrl',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.menu.iscached'),
    dataIndex: 'isCached',
    key: 'isCached',
    width: 70
  },
  {
    title: t('entity.menu.isvisible'),
    dataIndex: 'isVisible',
    key: 'isVisible',
    width: 70
  },
  {
    title: t('entity.menu.status'),
    dataIndex: 'menuStatus',
    key: 'menuStatus',
    width: 80
  },
  {
    title: t('entity.menu.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  CreateActionColumn<Menu>({
    actions: [
      { key: 'update', label: t('common.page.button.edit'), shape: 'plain', icon: RiEditLine, permission: 'identity:menu:update', onClick: (record: Menu) => handleEdit(record) },
      { key: 'allocate-role-menu', label: t('common.page.button.allocate') + t('entity.role._self'), shape: 'plain', icon: RiUserSettingsLine, permission: 'identity:menu:update', onClick: (record: Menu) => handleAssignMenuRoles(record) },
      { key: 'delete', label: t('common.page.button.delete'), shape: 'plain', icon: RiDeleteBinLine, permission: 'identity:menu:delete', onClick: (record: Menu) => handleDeleteOne(record) }
    ]
  })
])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Menu[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Menu, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && getMenuId(selecmedRow.value) === getMenuId(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: Menu[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 右侧查询（客户端过滤已加载树节点） */
const handleSearch = () => {}

/** 右侧重置（客户端过滤，不重建树） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = emptyMenuAdvancedQueryForm()
}

const handleTableChange = (_pagination: any, _filters: any, sorter: any) => {
  if (sorter?.order) logger.debug('[Menu] 排序', { field: sorter.field, order: sorter.order })
}

const handleResizeColumn = (w: number, col: any) => {
  const column = columns.value.find((c: any) => {
    const colKey = col.key || col.dataIndex || col.title
    const cKey = c.key || c.dataIndex || c.title
    return colKey && cKey && String(colKey) === String(cKey)
  })
  if (column) (column as any).width = w
}

const handleCreate = () => {
  formTitle.value = t('common.page.button.create') + t('entity.menu._self')
  formData.value = {}
  formVisible.value = true
}

const handleEdit = (record: Menu) => {
  formTitle.value = t('common.page.button.edit') + t('entity.menu._self')
  formData.value = { ...record }
  formVisible.value = true
}

const handleAssignMenuRoles = (record: Menu) => {
  currentAssignMenu.value = record
  assignMenuRolesVisible.value = true
}

const handleAssignSuccess = () => {
  void refreshAfterMutation()
}

const handleUpdate = () => {
  if (selecmedRow.value) handleEdit(selecmedRow.value)
  else message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.menu._self') }))
}

const handleDeleteOne = (record: Menu) => {
  const name = getMenuField(record, 'menuName') || getMenuId(record)
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.menu._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteMenuById(getMenuId(record))
        message.success(t('common.feedback.deleted'))
        await refreshAfterMutation()
      } catch (error: any) {
        message.error(error?.message || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleDelete = () => {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.menu._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { count: selecmedRows.value.length, entity: t('entity.menu._self') }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await Promise.all(selecmedRows.value.map(record => deleteMenuById(getMenuId(record))))
        message.success(t('common.feedback.deleted'))
        selecmedRows.value = []
        selecmedRowKeys.value = []
        selecmedRow.value = null
        await refreshAfterMutation()
      } catch (error: any) {
        message.error(error?.message || t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleFormSubmit = async () => {
  try {
    if (!formRef.value) return
    await formRef.value.validate()
    const formValues = formRef.value.getValues()
    formLoading.value = true
    if (formData.value?.menuId) {
      await updateMenu(formData.value.menuId, { ...formValues, menuId: formData.value.menuId })
      message.success(t('common.feedback.updated'))
    } else {
      await createMenu(formValues)
      message.success(t('common.feedback.created'))
    }
    formRef.value?.resetFields()
    formData.value = {}
    formVisible.value = false
    await refreshAfterMutation()
  } catch (error: any) {
    if (error?.errorFields) return
    message.error(error?.message || t('common.feedback.failed'))
  } finally {
    formLoading.value = false
  }
}

const handleFormCancel = () => {
  formVisible.value = false
  formData.value = {}
  formRef.value?.resetFields()
}

const handleImport = () => { importVisible.value = true }
const handleDownloadTemplate = async (sheetName?: string, fileName?: string) => {
  return await getMenuTemplate(sheetName, fileName)
}
const handleImportFile = async (file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> => {
  return await importMenu(file, sheetName)
}
const handleImportSuccess = (result: { success: number; fail: number; errors: string[] }) => {
  void refreshAfterMutation()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}
const handleImportCancel = () => { importVisible.value = false }

const handleExport = async () => {
  try {
    loading.value = true
    const queryParams: any = {}
    if (queryKeyword.value) queryParams.KeyWords = queryKeyword.value
    if (advancedQueryForm.value.menuName) queryParams.MenuName = advancedQueryForm.value.menuName
    if (advancedQueryForm.value.menuCode) queryParams.MenuCode = advancedQueryForm.value.menuCode
    if (advancedQueryForm.value.menuType !== undefined) queryParams.MenuType = advancedQueryForm.value.menuType
    if (advancedQueryForm.value.menuStatus !== undefined) queryParams.MenuStatus = advancedQueryForm.value.menuStatus
    const blob = await exportMenu(queryParams, menuExcelNames.sheet, menuExcelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `${menuExcelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success'))
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.export.failed'))
  } finally {
    loading.value = false
  }
}

const handleAdvancedQuery = () => { advancedQueryVisible.value = true }
const handleAdvancedQuerySubmit = () => {
  advancedQueryVisible.value = false
}
const handleAdvancedQueryReset = () => {
  advancedQueryForm.value = emptyMenuAdvancedQueryForm()
}

const handleColumnSetting = () => { columnSettingVisible.value = true }
const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map(k => String(k))
}
const handleColumnSettingReset = () => { visibleColumnKeys.value = [] }

const handleRefresh = () => { void loadData() }
</script>

<style scoped lang="css">
/* 边距由子组件（takt-tree-left-* / takt-tree-right-*）统一设置，本视图不重复设置 */
.identity-menu {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
  overflow: hidden;
}

.menu-query-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  flex-wrap: nowrap;
  min-width: 0;
  flex-shrink: 0;
}

.menu-toolbar-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  flex-wrap: nowrap;
  min-width: 0;
  flex-shrink: 0;
}

.menu-tree-table-wrap {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: row;
  min-width: 0;
  overflow: hidden;
  align-items: stretch;
}

</style>
