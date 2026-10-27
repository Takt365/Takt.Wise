<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowLogicDesigner -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：基于 @logicflow/core 的流程逻辑设计组件，画布+属性面板，图元由 DndPanel 拖拽添加，与后端 processContent 兼容 -->
<!-- 依赖：@logicflow/core、@logicflow/extension -->
<!-- ======================================== -->

<template>
  <div
    ref="rootRef"
    class="takt-flow-logic-designer"
    :class="{ 'takt-flow-logic-designer--dark': isDark, 'takt-flow-logic-designer--fullscreen': isFullscreen }"
  >
    <a-tooltip :title="isFullscreen ? t('common.page.button.exitfullscreen') : t('common.page.button.fullscreen')">
      <a-button
        type="text"
        class="takt-flow-logic-designer__fullscreen-btn"
        @click="toggleFullscreen"
      >
        <template #icon>
          <RiFullscreenExitLine class="takt-remix-icon" v-if="isFullscreen" />
          <RiFullscreenLine class="takt-remix-icon" v-else />
        </template>
      </a-button>
    </a-tooltip>
    <!-- 画布（图元由 DndPanel 拖拽添加） -->
    <div class="takt-flow-logic-designer__canvas-wrap">
      <div
        ref="containerRef"
        class="takt-flow-logic-designer__canvas"
      />
    </div>

    <!-- 属性面板：右侧 -->
    <aside class="takt-flow-logic-designer__property-panel">
      <div class="takt-flow-logic-designer__property-title">
        {{ t('workflow.designer.page.propertypanel') }}
      </div>
      <div class="takt-flow-logic-designer__property-body">
        <template v-if="!selecmedNodeId && !selecmedEdgeId">
          <div class="takt-flow-logic-designer__property-empty">
            {{ t('workflow.designer.page.selectnodeoredge') }}
          </div>
        </template>
        <template v-else-if="selecmedNodeId && selecmedNodeData">
          <a-form
            layout="vertical"
            size="small"
            class="takt-flow-logic-designer__property-form"
          >
            <a-form-item :label="t('workflow.designer.page.propid')">
              <a-input
                :value="selecmedNodeData.id"
                disabled
              />
            </a-form-item>
            <a-form-item :label="t('workflow.designer.page.propname')">
              <a-input
                :value="selecmedNodeData.name ?? ''"
                :disabled="readonly"
                @update:value="(v: string) => applySelecmedNodeName(v)"
              />
            </a-form-item>
            <a-form-item :label="t('workflow.designer.page.proptype')">
              <a-select
                :value="selecmedNodeData.type"
                style="width: 100%"
                :disabled="readonly"
                @update:value="(v) => applySelecmedNodeType(v == null ? 'userTask' : String(v))"
              >
                <a-select-option value="start">
                  {{ t('workflow.designer.page.nodetypestart') }}
                </a-select-option>
                <a-select-option value="userTask">
                  {{ t('workflow.designer.page.nodetypeusertask') }}
                </a-select-option>
                <a-select-option value="copy">
                  {{ t('workflow.designer.page.nodetypecopy') }}
                </a-select-option>
                <a-select-option value="systemTask">
                  {{ t('workflow.designer.page.nodetypesystemtask') }}
                </a-select-option>
                <a-select-option value="gateway">
                  {{ t('workflow.designer.page.nodetypegateway') }}
                </a-select-option>
                <a-select-option value="countersign">
                  {{ t('workflow.designer.page.nodetypecountersign') }}
                </a-select-option>
                <a-select-option value="end">
                  {{ t('workflow.designer.page.nodetypeend') }}
                </a-select-option>
              </a-select>
            </a-form-item>
            <!-- 审批人设置（参照 AntFlow-Designer approverDrawer） -->
            <template v-if="selecmedNodeData.type === 'userTask'">
              <a-form-item :label="t('workflow.designer.page.propassigneetype')">
                <a-select
                  :value="selecmedNodeData.assigneeType"
                  style="width: 100%"
                  :disabled="readonly"
                  allow-clear
                  @update:value="(v) => applySelecmedNodeAssigneeType(v === undefined || v === null ? undefined : String(v))"
                >
                  <a-select-option value="starter">
                    {{ t('workflow.designer.page.assigneestarter') }}
                  </a-select-option>
                  <a-select-option value="assignee">
                    {{ t('workflow.designer.page.assigneeassignee') }}
                  </a-select-option>
                  <a-select-option value="selfSelect">
                    {{ t('workflow.designer.page.assigneeselfselect') }}
                  </a-select-option>
                  <a-select-option value="role">
                    {{ t('workflow.designer.page.assigneerole') }}
                  </a-select-option>
                  <a-select-option value="dept">
                    {{ t('workflow.designer.page.assigneedept') }}
                  </a-select-option>
                </a-select>
              </a-form-item>
              <a-form-item
                v-if="selecmedNodeData.assigneeType === 'role'"
                :label="t('workflow.designer.page.proproles')"
              >
                <a-select
                  :value="selecmedNodeData.roles"
                  mode="multiple"
                  style="width: 100%"
                  :placeholder="t('workflow.designer.page.placeholderselectroles')"
                  :options="roleOptions"
                  :field-names="{ label: 'label', value: 'value' }"
                  :disabled="readonly"
                  allow-clear
                  @update:value="(v) => applySelecmedNodeRoles(Array.isArray(v) ? v.map(String) : [])"
                />
              </a-form-item>
              <a-form-item
                v-if="selecmedNodeData.assigneeType === 'dept'"
                :label="t('workflow.designer.page.propdepartments')"
              >
                <a-tree-select
                  :value="selecmedNodeData.departments"
                  style="width: 100%"
                  :tree-data="deptTreeOptions as any"
                  tree-checkable
                  :placeholder="t('workflow.designer.page.placeholderselectdepts')"
                  :disabled="readonly"
                  allow-clear
                  :field-names="{ label: 'label', value: 'value' }"
                  @update:value="(v) => applySelecmedNodeDepartments(Array.isArray(v) ? v.map(String) : [])"
                />
              </a-form-item>
              <a-form-item
                v-if="selecmedNodeData.assigneeType === 'assignee'"
                :label="t('workflow.designer.page.propassignees')"
              >
                <a-select
                  :value="selecmedNodeData.assigneeUserIds"
                  mode="multiple"
                  style="width: 100%"
                  :placeholder="t('workflow.designer.page.placeholderselectusers')"
                  :options="userOptions"
                  :field-names="{ label: 'label', value: 'value' }"
                  :disabled="readonly"
                  allow-clear
                  show-search
                  :filter-option="filterOption"
                  @update:value="(v) => applySelecmedNodeAssigneeUserIds(Array.isArray(v) ? v.map(String) : [])"
                />
              </a-form-item>
            </template>
            <!-- 抄送人设置（参照 AntFlow-Designer copyerDrawer） -->
            <template v-if="selecmedNodeData.type === 'copy'">
              <a-form-item :label="t('workflow.designer.page.propcopyusers')">
                <a-select
                  :value="selecmedNodeData.copyUserIds"
                  mode="multiple"
                  style="width: 100%"
                  :placeholder="t('workflow.designer.page.placeholderselectcopyusers')"
                  :options="userOptions"
                  :field-names="{ label: 'label', value: 'value' }"
                  :disabled="readonly"
                  allow-clear
                  show-search
                  :filter-option="filterOption"
                  @update:value="(v) => applySelecmedNodeCopyUserIds(Array.isArray(v) ? v.map(String) : [])"
                />
              </a-form-item>
            </template>
            <a-form-item v-if="!readonly">
              <a-button
                type="primary"
                danger
                block
                @click="deleteSelecmedNode"
              >
                {{ t('workflow.designer.page.deletenode') }}
              </a-button>
            </a-form-item>
          </a-form>
        </template>
        <template v-else-if="selecmedEdgeId && selecmedEdgeData">
          <a-form
            layout="vertical"
            size="small"
            class="takt-flow-logic-designer__property-form"
          >
            <a-form-item :label="t('workflow.designer.page.propfrom')">
              <a-input
                :value="selecmedEdgeData.from"
                disabled
              />
            </a-form-item>
            <a-form-item :label="t('workflow.designer.page.propto')">
              <a-input
                :value="selecmedEdgeData.to"
                disabled
              />
            </a-form-item>
            <!-- 条件设置（参照 AntFlow-Designer conditionDrawer：优先级、条件描述、表达式） -->
            <a-form-item :label="t('workflow.designer.page.edgelabel')">
              <a-input
                :value="selecmedEdgeData.label ?? ''"
                :placeholder="t('workflow.designer.page.placeholderedgelabel')"
                :disabled="readonly"
                @update:value="(v: string) => applySelecmedEdgeLabel(v ?? '')"
              />
            </a-form-item>
            <a-form-item :label="t('workflow.designer.page.edgepriority')">
              <a-input-number
                v-if="selecmedEdgeData.priority != null"
                :value="selecmedEdgeData.priority"
                style="width: 100%"
                :min="1"
                :placeholder="t('workflow.designer.page.placeholderedgepriority')"
                :disabled="readonly"
                @update:value="(v) => applySelecmedEdgePriority(typeof v === 'number' ? v : undefined)"
              />
              <a-input-number
                v-else
                style="width: 100%"
                :min="1"
                :placeholder="t('workflow.designer.page.placeholderedgepriority')"
                :disabled="readonly"
                @update:value="(v) => applySelecmedEdgePriority(typeof v === 'number' ? v : undefined)"
              />
            </a-form-item>
            <a-form-item :label="t('workflow.designer.page.edgecondition')">
              <a-textarea
                :value="selecmedEdgeData.condition ?? ''"
                :placeholder="t('workflow.designer.page.placeholderedgecondition')"
                :rows="2"
                :disabled="readonly"
                @update:value="(v: string) => applySelecmedEdgeCondition(v ?? '')"
              />
            </a-form-item>
            <a-form-item v-if="!readonly">
              <a-button
                type="primary"
                danger
                block
                @click="deleteSelecmedEdge"
              >
                {{ t('workflow.designer.page.deleteedge') }}
              </a-button>
            </a-form-item>
          </a-form>
        </template>
      </div>
    </aside>
  </div>
</template>

<script setup lang="ts">
/**
 * 流程逻辑设计器：节点 id、name、type（start/userTask/end）、assigneeType；连线 from、to。
 * 与后端 TaktFlowRuntime/FlowProcessNode/FlowProcessEdge 一致。
 */
import LogicFlow from '@logicflow/core'
import { DndPanel, SelectionSelect } from '@logicflow/extension'
import { useI18n } from 'vue-i18n'
import { useThemeStore } from '@/stores/common/useThemeStore'
import { RiFullscreenLine, RiFullscreenExitLine } from '@remixicon/vue'
import { getRoleOptions } from '@/api/identity/role.api'
import { getDeptTreeOptions } from '@/api/human-resource/organization/dept.api'
import { getUserOptions } from '@/api/identity/user.api'
import '@logicflow/core/lib/style/index.css'
import shapesData from './shapesData'
/** 与后端 TaktFlowNode 对齐:审批人/抄送人配置 */
export interface FlowNode {
  id: string
  name?: string | undefined
  type?: string | undefined
  assigneeType?: string | undefined
  roles?: string[] | undefined
  departments?: string[] | undefined
  assigneeUserIds?: string[] | undefined
  copyUserIds?: string[] | undefined
  x?: number | undefined
  y?: number | undefined
}

/** 与后端 TaktFlowLine 对齐:连线条件(网关分支) */
export interface FlowEdge {
  from: string
  to: string
  condition?: string | undefined
  label?: string | undefined
  priority?: number | undefined
}

interface Props {
  modelValue?: string
  readonly?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: '',
  readonly: false
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const { t } = useI18n()
const themeStore = useThemeStore()
/** 主题：LogicFlow 2.2.x 内置 ThemeMode 为 default | retro | dark | colorful；圆角由 lfStyleConfig 覆盖，亮色用 default */
const isDark = computed(() => themeStore.resolvedTheme === 'dark')
const lfThemeMode = computed<LogicFlow.ThemeMode>(() => (isDark.value ? 'dark' : 'default'))

const rootRef = ref<HTMLDivElement | null>(null)
const containerRef = ref<HTMLDivElement | null>(null)
let lf: LogicFlow | null = null

const isFullscreen = ref(false)
function toggleFullscreen() {
  const el = rootRef.value
  if (!el) return
  if (!document.fullscreenElement) {
    el.requestFullscreen().then(() => { isFullscreen.value = true }).catch(() => {})
  } else {
    document.exitFullscreen().then(() => { isFullscreen.value = false }).catch(() => {})
  }
}
function onFullscreenChange() {
  isFullscreen.value = document.fullscreenElement === rootRef.value
}

const selecmedNodeId = ref<string | null>(null)
const selecmedEdgeId = ref<string | null>(null)

/** 当前图数据快照，用于属性面板与添加节点 */
const graphData = ref<{ nodes: FlowNode[]; edges: FlowEdge[] }>({ nodes: [], edges: [] })

/** 审批人/抄送人选项（参照 AntFlow-Designer 选择人员、角色、部门） */
const roleOptions = ref<{ label: string; value: string }[]>([])
interface DeptOption {
  label: string
  value: string
  children?: DeptOption[] | undefined
}
const deptTreeOptions = ref<DeptOption[]>([])
const userOptions = ref<{ label: string; value: string }[]>([])

function filterOption(input: string, option?: { label?: string }) {
  const label = option?.label ?? ''
  return label.toLowerCase().includes((input ?? '').toLowerCase())
}

const selecmedNodeData = computed(() => {
  if (!selecmedNodeId.value) return null
  return graphData.value.nodes.find((n) => n.id === selecmedNodeId.value) ?? null
})

const selecmedEdgeData = computed(() => {
  if (!selecmedEdgeId.value) return null
  return graphData.value.edges.find((x) => `${x.from}-${x.to}` === selecmedEdgeId.value) ?? null
})

/** 按数组下标的简单横向排布（无拓扑时兜底） */
function layoutPosition(index: number, total: number): { x: number; y: number } {
  if (total <= 1) return { x: 200, y: 150 }
  const width = 400
  const step = total > 1 ? width / (total - 1) : 0
  const x = 100 + index * step
  let y = 150
  if (index === 0) y = 80
  if (index === total - 1) y = 220
  return { x, y }
}

const LAYOUT_STEP_X = 280
const LAYOUT_STEP_Y = 120
const LAYOUT_BASE_X = 100
const LAYOUT_BASE_Y = 100
const ROW_PADDING = 50

export interface FlowLayoutResult {
  positionMap: Map<string, { x: number; y: number }>
  rowMap: Map<string, number>
}

/** 按流程拓扑（从开始节点 BFS 分层）计算无坐标节点的位置；maxRowWidth 时按行宽换行；并返回每节点行号供连线换行用 */
function flowLayout(
  data: { nodes: FlowNode[]; edges: FlowEdge[] },
  maxRowWidth?: number
): FlowLayoutResult {
  const positionMap = new Map<string, { x: number; y: number }>()
  const rowMap = new Map<string, number>()
  const nextMap = new Map<string, string[]>()
  for (const e of data.edges) {
    const list = nextMap.get(e.from) ?? []
    list.push(e.to)
    nextMap.set(e.from, list)
  }
  const startId = data.nodes.find((n) => n.type === 'start')?.id
  if (!startId) return { positionMap, rowMap }

  const levelList: string[][] = []
  const levelById = new Map<string, number>()
  const queue: { id: string; level: number }[] = [{ id: startId, level: 0 }]
  levelById.set(startId, 0)
  while (queue.length) {
    const { id, level } = queue.shift()!
    if (!levelList[level]) levelList[level] = []
    levelList[level].push(id)
    for (const to of nextMap.get(id) ?? []) {
      if (!levelById.has(to)) {
        levelById.set(to, level + 1)
        queue.push({ id: to, level: level + 1 })
      }
    }
  }

  const stepX = LAYOUT_STEP_X
  const stepY = LAYOUT_STEP_Y
  const baseX = LAYOUT_BASE_X
  const baseY = LAYOUT_BASE_Y
  const levelsPerRow =
    maxRowWidth != null && maxRowWidth > stepX ? Math.max(1, Math.floor(maxRowWidth / stepX)) : levelList.length

  const rowHeights: number[] = []
  for (let r = 0; r * levelsPerRow < levelList.length; r++) {
    let maxH = 0
    for (let c = 0; c < levelsPerRow && r * levelsPerRow + c < levelList.length; c++) {
      const ids = levelList[r * levelsPerRow + c]
      if (!ids) continue
      const h = ids.length <= 1 ? stepY : (ids.length - 1) * stepY
      maxH = Math.max(maxH, h)
    }
    rowHeights.push(Math.max(maxH, stepY))
  }
  for (let L = 0; L < levelList.length; L++) {
    const rowIndex = Math.floor(L / levelsPerRow)
    const colInRow = L % levelsPerRow
    const rowY = rowIndex === 0 ? baseY : baseY + rowHeights.slice(0, rowIndex).reduce((a, b) => a + b + ROW_PADDING, 0)
    const ids = levelList[L]
    if (!ids) continue
    const n = ids.length
    const totalH = (n - 1) * stepY
    ids.forEach((id, i) => {
      const y = rowY + i * stepY - totalH / 2
      positionMap.set(id, { x: baseX + colInRow * stepX, y })
      rowMap.set(id, rowIndex)
    })
  }
  const maxLevel = levelList.length
  const orphanRowY =
    rowHeights.length === 0
      ? baseY
      : baseY + rowHeights.slice(0, -1).reduce((a, b) => a + b + ROW_PADDING, 0) + (rowHeights[rowHeights.length - 1] ?? 0) / 2
  const orphanRow = rowHeights.length
  data.nodes.forEach((n) => {
    if (!levelById.has(n.id)) {
      positionMap.set(n.id, { x: baseX + (maxLevel % levelsPerRow) * stepX, y: orphanRowY })
      rowMap.set(n.id, orphanRow)
    }
  })
  return { positionMap, rowMap }
}

function normalizeNode(raw: Record<string, unknown>): FlowNode {
  const arr = (v: unknown) => (Array.isArray(v) ? v.map((x) => String(x)) : undefined)
  return {
    id: String(raw.id ?? raw.Id ?? ''),
    name: raw.name != null || raw.Name != null ? String(raw.name ?? raw.Name) : undefined,
    type: raw.type != null || raw.Type != null ? String(raw.type ?? raw.Type) : undefined,
    assigneeType: raw.assigneeType != null || raw.AssigneeType != null ? String(raw.assigneeType ?? raw.AssigneeType) : undefined,
    roles: arr(raw.roles ?? raw.Roles),
    departments: arr(raw.departments ?? raw.Departments),
    assigneeUserIds: arr(raw.assigneeUserIds ?? raw.AssigneeUserIds),
    copyUserIds: arr(raw.copyUserIds ?? raw.CopyUserIds),
    x: typeof raw.x === 'number' ? raw.x : typeof raw.X === 'number' ? raw.X : undefined,
    y: typeof raw.y === 'number' ? raw.y : typeof raw.Y === 'number' ? raw.Y : undefined
  }
}

function normalizeEdge(raw: Record<string, unknown>): FlowEdge {
  return {
    from: String(raw.from ?? raw.From ?? ''),
    to: String(raw.to ?? raw.To ?? ''),
    condition: raw.condition != null || raw.Condition != null ? String(raw.condition ?? raw.Condition) : undefined,
    label: raw.label != null || raw.Label != null ? String(raw.label ?? raw.Label) : undefined,
    priority: typeof raw.priority === 'number' ? raw.priority : typeof raw.Priority === 'number' ? raw.Priority : undefined
  }
}

function parseJson(json?: string | { nodes?: unknown[]; edges?: unknown[] }): { nodes: FlowNode[]; edges: FlowEdge[] } {
  if (json == null) return { nodes: [], edges: [] }
  if (typeof json === 'object') {
    const nodes = Array.isArray(json.nodes)
      ? json.nodes.map((n) => normalizeNode(typeof n === 'object' && n !== null ? (n as Record<string, unknown>) : {}))
      : []
    const edges = Array.isArray(json.edges)
      ? json.edges.map((e) => normalizeEdge(typeof e === 'object' && e !== null ? (e as Record<string, unknown>) : {}))
      : []
    return { nodes, edges }
  }
  const content = (json).trim()
  if (!content) return { nodes: [], edges: [] }
  try {
    const obj = JSON.parse(content) as { nodes?: unknown[]; edges?: unknown[] }
    const nodes = Array.isArray(obj.nodes)
      ? obj.nodes.map((n) => normalizeNode(typeof n === 'object' && n !== null ? (n as Record<string, unknown>) : {}))
      : []
    const edges = Array.isArray(obj.edges)
      ? obj.edges.map((e) => normalizeEdge(typeof e === 'object' && e !== null ? (e as Record<string, unknown>) : {}))
      : []
    return { nodes, edges }
  } catch {
    return { nodes: [], edges: [] }
  }
}

/** 节点类型与颜色一一对应（图标与画布一致），参照 AntFlow-Designer 区分审批/抄送 */
const FLOW_NODE_COLORS: Record<string, string> = {
  start: '#177cb0',
  userTask: '#21a675',
  systemTask: '#ffa631',
  gateway: '#801dae',
  countersign: '#ed5736',
  copy: '#3296fa',
  end: '#800020'
}

function getNodeStyle(flowType: string | undefined): { fill: string; stroke: string } | undefined {
  const fill = flowType ? FLOW_NODE_COLORS[flowType] : undefined
  if (!fill) return undefined
  return { fill, stroke: fill }
}

/** 生成与画布形状、颜色一致的 DndPanel 图标（SVG data URL，32x32） */
function flowIcon(flowType: string, shape: 'circle' | 'rect' | 'diamond' | 'polygon'): string {
  const fill = FLOW_NODE_COLORS[flowType] ?? '#999'
  let path: string
  if (shape === 'circle') path = '<circle cx="16" cy="16" r="14" fill="' + fill + '" stroke="' + fill + '"/>'
  else if (shape === 'rect') path = '<rect x="4" y="6" width="24" height="20" rx="3" fill="' + fill + '" stroke="' + fill + '"/>'
  else if (shape === 'diamond') path = '<path d="M16 4L28 16 16 28 4 16z" fill="' + fill + '" stroke="' + fill + '"/>'
  else path = '<polygon points="16,4 28,16 16,28 4,16" fill="' + fill + '" stroke="' + fill + '"/>'
  return 'data:image/svg+xml,' + encodeURIComponent('<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32">' + path + '</svg>')
}

/** 选区工具图标（与 flowIcon 同规格 32x32 SVG，虚线框表示框选） */
function selectionIcon(): string {
  const svg =
    '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32">' +
    '<rect x="4" y="6" width="24" height="20" rx="3" fill="none" stroke="#8c8c8c" stroke-width="1.5" stroke-dasharray="4 2"/>' +
    '</svg>'
  return 'data:image/svg+xml,' + encodeURIComponent(svg)
}

function getNodeShape(flowType: string | undefined): 'rect' | 'circle' | 'diamond' | 'polygon' {
  if (flowType === 'start' || flowType === 'end') return 'circle'
  if (flowType === 'gateway') return 'diamond'
  if (flowType === 'countersign') return 'polygon'
  if (flowType === 'copy') return 'rect'
  return 'rect'
}

/** 锚点索引：0=上 1=右 2=下 3=左（与 LogicFlow 默认锚点顺序一致） */
const ANCHOR_TOP = 0
const ANCHOR_RIGHT = 1
const ANCHOR_BOTTOM = 2
const ANCHOR_LEFT = 3

/** 根据源、目标相对位置选择左右或上下锚点，使连线与截图一致：水平用左右，换行/回环用上下 */
function getAnchorPair(
  fromPos: { x: number; y: number },
  toPos: { x: number; y: number },
  fromRow: number,
  toRow: number
): { sourceAnchor: number; targetAnchor: number } {
  const dy = toPos.y - fromPos.y
  const dx = toPos.x - fromPos.x
  const crossRow = fromRow >= 0 && toRow >= 0 && fromRow !== toRow
  if (crossRow || Math.abs(dy) > Math.abs(dx)) {
    if (dy > 0) return { sourceAnchor: ANCHOR_BOTTOM, targetAnchor: ANCHOR_TOP }
    if (dy < 0) return { sourceAnchor: ANCHOR_TOP, targetAnchor: ANCHOR_BOTTOM }
  }
  if (dx >= 0) return { sourceAnchor: ANCHOR_RIGHT, targetAnchor: ANCHOR_LEFT }
  return { sourceAnchor: ANCHOR_LEFT, targetAnchor: ANCHOR_RIGHT }
}

/** 跨行边的折线路径点：先垂直到两行中间，再水平到目标正上方/下，再垂直到目标，避免斜线穿行 */
function edgeWaypointsForWrap(
  fromPos: { x: number; y: number },
  toPos: { x: number; y: number },
  fromRow: number,
  toRow: number
): Array<{ x: number; y: number }> {
  if (fromRow === toRow) return []
  const midY = (fromPos.y + toPos.y) / 2
  return [{ x: fromPos.x, y: midY }, { x: toPos.x, y: midY }]
}

function ourToLfGraph(
  data: { nodes: FlowNode[]; edges: FlowEdge[] },
  maxRowWidth?: number
): LogicFlow.GraphConfigData {
  const { positionMap, rowMap } = flowLayout(data, maxRowWidth)
  const idToPos = new Map<string, { x: number; y: number }>()
  data.nodes.forEach((n) => {
    const pos =
      n.x != null && n.y != null ? { x: n.x, y: n.y } : positionMap.get(n.id)
    if (pos) idToPos.set(n.id, pos)
  })
  const nodes: LogicFlow.NodeConfig[] = data.nodes.map((n, i) => {
    const pos =
      n.x != null && n.y != null
        ? { x: n.x, y: n.y }
        : positionMap.get(n.id) ?? layoutPosition(i, data.nodes.length)
    const nodeType = getNodeShape(n.type)
    const style = getNodeStyle(n.type)
    const needRadius = nodeType === 'polygon' || nodeType === 'diamond'
    const nodeStyle = style ? (needRadius ? { ...style, radius: 5 } : style) : undefined
    const properties: Record<string, unknown> = {
      flowType: n.type,
      assigneeType: n.assigneeType,
      roles: n.roles,
      departments: n.departments,
      assigneeUserIds: n.assigneeUserIds,
      copyUserIds: n.copyUserIds,
      ...(nodeStyle && { style: nodeStyle })
    }
    return {
      id: n.id,
      type: nodeType,
      x: pos.x,
      y: pos.y,
      text: n.name || n.id,
      properties,
      ...(nodeStyle && { style: nodeStyle })
    }
  })
  const edges: LogicFlow.EdgeConfig[] = data.edges.map((e) => {
    const fromPos = idToPos.get(e.from)
    const toPos = idToPos.get(e.to)
    const fromRow = rowMap.get(e.from) ?? -1
    const toRow = rowMap.get(e.to) ?? -1
    const points =
      fromPos && toPos && fromRow >= 0 && toRow >= 0 && fromRow !== toRow
        ? edgeWaypointsForWrap(fromPos, toPos, fromRow, toRow)
        : undefined
    const anchors =
      fromPos && toPos ? getAnchorPair(fromPos, toPos, fromRow, toRow) : null
    const edge: LogicFlow.EdgeConfig & {
      points?: Array<{ x: number; y: number }>
      sourceAnchor?: number
      targetAnchor?: number
      sourceAnchorId?: string
      targetAnchorId?: string
      properties?: { condition?: string; label?: string; priority?: number }
    } = {
      sourceNodeId: e.from,
      targetNodeId: e.to,
      type: 'polyline',
      properties: {
        ...(e.condition != null && { condition: e.condition }),
        ...(e.label != null && { label: e.label }),
        ...(e.priority != null && { priority: e.priority })
      }
    }
    if (points?.length) edge.points = points
    if (anchors) {
      edge.sourceAnchor = anchors.sourceAnchor
      edge.targetAnchor = anchors.targetAnchor
      edge.sourceAnchorId = String(anchors.sourceAnchor)
      edge.targetAnchorId = String(anchors.targetAnchor)
    }
    return edge
  })
  return { nodes, edges }
}

function lfToOurGraph(data: LogicFlow.GraphData): { nodes: FlowNode[]; edges: FlowEdge[] } {
  const p = (n: LogicFlow.NodeConfig) => (n.properties || {}) as Record<string, unknown>
  const nodes: FlowNode[] = data.nodes.map((n) => ({
    id: n.id,
    name: typeof n.text === 'string' ? n.text : (n.text as { value?: string })?.value ?? n.id,
    type: (p(n).flowType as string) ?? 'userTask',
    assigneeType: p(n).assigneeType as string | undefined,
    roles: Array.isArray(p(n).roles) ? (p(n).roles as string[]) : undefined,
    departments: Array.isArray(p(n).departments) ? (p(n).departments as string[]) : undefined,
    assigneeUserIds: Array.isArray(p(n).assigneeUserIds) ? (p(n).assigneeUserIds as string[]) : undefined,
    copyUserIds: Array.isArray(p(n).copyUserIds) ? (p(n).copyUserIds as string[]) : undefined,
    x: n.x,
    y: n.y
  }))
  const ep = (e: LogicFlow.EdgeConfig) => ((e as { properties?: { condition?: string; label?: string; priority?: number } }).properties || {})
  const edges: FlowEdge[] = data.edges.map((e) => ({
    from: e.sourceNodeId,
    to: e.targetNodeId,
    condition: ep(e).condition,
    label: ep(e).label,
    priority: ep(e).priority
  }))
  return { nodes, edges }
}

function refreshGraphData() {
  if (!lf) return
  const raw = lf.getGraphRawData()
  graphData.value = lfToOurGraph(raw)
  syncDndPanelItems()
}

/** 根据画布是否已有开始/结束，禁用面板中对应项拖拽（画布只允许一个开始、一个结束） */
function syncDndPanelItems() {
  const dnd = lf?.extension?.dndPanel as { setPatternItems: (items: unknown[]) => void } | undefined
  if (!dnd?.setPatternItems) return
  const hasStart = graphData.value.nodes.some((n) => n.type === 'start')
  const hasEnd = graphData.value.nodes.some((n) => n.type === 'end')
  const ext = lf!.extension as unknown as { selectionSelect: { openSelectionSelect: () => void; closeSelectionSelect: () => void } }
  const items = [
    {
      text: '选区',
      label: '选区',
      icon: selectionIcon(),
      callback: () => {
        ext.selectionSelect.openSelectionSelect()
        lf!.once('selection:selecmed', () => ext.selectionSelect.closeSelectionSelect())
      }
    },
    { type: 'circle', text: '开始', label: '开始', icon: flowIcon('start', 'circle'), properties: { flowType: 'start' }, disabled: hasStart },
    { type: 'rect', text: '用户任务', label: '用户任务', icon: flowIcon('userTask', 'rect'), properties: { flowType: 'userTask' } },
    { type: 'rect', text: '抄送', label: '抄送', icon: flowIcon('copy', 'rect'), properties: { flowType: 'copy' } },
    { type: 'rect', text: '系统任务', label: '系统任务', icon: flowIcon('systemTask', 'rect'), properties: { flowType: 'systemTask' } },
    { type: 'diamond', text: '网关', label: '网关', icon: flowIcon('gateway', 'diamond'), properties: { flowType: 'gateway' } },
    { type: 'polygon', text: '会签', label: '会签', icon: flowIcon('countersign', 'polygon'), properties: { flowType: 'countersign' } },
    { type: 'circle', text: '结束', label: '结束', icon: flowIcon('end', 'circle'), properties: { flowType: 'end' }, disabled: hasEnd }
  ]
  dnd.setPatternItems(items)
}

function getDataJson(): string {
  if (!lf) return JSON.stringify({ nodes: [], edges: [] })
  const raw = lf.getGraphRawData()
  const our = lfToOurGraph(raw)
  return JSON.stringify(our)
}

function emitUpdate() {
  emit('update:modelValue', getDataJson())
  refreshGraphData()
}

/** 画布只允许一个开始、一个结束。加载时：只保留第一个开始、第一个结束，多余的同类型节点直接去掉 */
function normalizeSingleStartEnd(nodes: FlowNode[]): FlowNode[] {
  const firstStart = nodes.find((n) => n.type === 'start')
  const firstEnd = nodes.find((n) => n.type === 'end')
  const others = nodes.filter((n) => n.type !== 'start' && n.type !== 'end')
  return [...(firstStart ? [firstStart] : []), ...others, ...(firstEnd ? [firstEnd] : [])]
}

function applyModelValue(val?: string) {
  if (!lf) return
  const { nodes, edges } = parseJson(val)
  const canvasWidth = (containerRef.value?.clientWidth || 0) > 0 ? containerRef.value!.clientWidth : 800
  if (nodes.length === 0 && edges.length === 0) {
    lf.render(shapesData as LogicFlow.GraphConfigData)
    lf.translateCenter()
  } else {
    const normalized = normalizeSingleStartEnd(nodes)
    const nodeIds = new Set(normalized.map((n) => n.id))
    const validEdges = edges.filter((e) => nodeIds.has(e.from) && nodeIds.has(e.to))
    lf.render(ourToLfGraph({ nodes: normalized, edges: validEdges }, canvasWidth))
    nextTick(() => lf?.translateCenter())
  }
  refreshGraphData()
}

function applySelecmedNodeName(name: string) {
  const nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value ? { ...n, name: name || n.id } : n
  )
  const graph = ourToLfGraph({ nodes: nodes, edges: graphData.value.edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges: graphData.value.edges }
    emitUpdate()
  }
}

function applySelecmedNodeType(type: string) {
  let nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value
      ? { ...n, type: type as FlowNode['type'], assigneeType: type === 'userTask' ? n.assigneeType : undefined }
      : n
  )
  if (type === 'start' || type === 'end') {
    const keepId = selecmedNodeId.value
    nodes = nodes.filter((n) => n.type !== type || n.id === keepId)
  }
  const nodeIds = new Set(nodes.map((n) => n.id))
  const edges = graphData.value.edges.filter((e) => nodeIds.has(e.from) && nodeIds.has(e.to))
  const graph = ourToLfGraph({ nodes, edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges }
    emitUpdate()
  }
}

function applySelecmedNodeAssigneeType(assigneeType: string | undefined) {
  const nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value ? { ...n, assigneeType } : n
  )
  const graph = ourToLfGraph({ nodes, edges: graphData.value.edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges: graphData.value.edges }
    emitUpdate()
  }
}

function applySelecmedNodeRoles(roles: string[]) {
  const nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value ? { ...n, roles: roles?.length ? roles : undefined } : n
  )
  const graph = ourToLfGraph({ nodes, edges: graphData.value.edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges: graphData.value.edges }
    emitUpdate()
  }
}

function applySelecmedNodeDepartments(departments: string[]) {
  const nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value ? { ...n, departments: departments?.length ? departments : undefined } : n
  )
  const graph = ourToLfGraph({ nodes, edges: graphData.value.edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges: graphData.value.edges }
    emitUpdate()
  }
}

function applySelecmedNodeAssigneeUserIds(assigneeUserIds: string[]) {
  const nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value ? { ...n, assigneeUserIds: assigneeUserIds?.length ? assigneeUserIds : undefined } : n
  )
  const graph = ourToLfGraph({ nodes, edges: graphData.value.edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges: graphData.value.edges }
    emitUpdate()
  }
}

function applySelecmedNodeCopyUserIds(copyUserIds: string[]) {
  const nodes = graphData.value.nodes.map((n) =>
    n.id === selecmedNodeId.value ? { ...n, copyUserIds: copyUserIds?.length ? copyUserIds : undefined } : n
  )
  const graph = ourToLfGraph({ nodes, edges: graphData.value.edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes, edges: graphData.value.edges }
    emitUpdate()
  }
}

function applySelecmedEdgeCondition(condition: string) {
  const edges = graphData.value.edges.map((e) =>
    selecmedEdgeId.value === `${e.from}-${e.to}` ? { ...e, condition: condition || undefined } : e
  )
  const graph = ourToLfGraph({ nodes: graphData.value.nodes, edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes: graphData.value.nodes, edges }
    emitUpdate()
  }
}

function applySelecmedEdgeLabel(label: string) {
  const edges = graphData.value.edges.map((e) =>
    selecmedEdgeId.value === `${e.from}-${e.to}` ? { ...e, label: label || undefined } : e
  )
  const graph = ourToLfGraph({ nodes: graphData.value.nodes, edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes: graphData.value.nodes, edges }
    emitUpdate()
  }
}

function applySelecmedEdgePriority(priority: number | undefined) {
  const edges = graphData.value.edges.map((e) =>
    selecmedEdgeId.value === `${e.from}-${e.to}` ? { ...e, priority } : e
  )
  const graph = ourToLfGraph({ nodes: graphData.value.nodes, edges })
  if (lf) {
    lf.render(graph)
    graphData.value = { nodes: graphData.value.nodes, edges }
    emitUpdate()
  }
}

/** 删除当前选中的节点（会同时删除与之相连的边） */
function deleteSelecmedNode() {
  if (props.readonly || !lf || !selecmedNodeId.value) return
  lf.deleteNode(selecmedNodeId.value)
  selecmedNodeId.value = null
  selecmedEdgeId.value = null
  refreshGraphData()
  emitUpdate()
}

/** 删除当前选中的连线 */
function deleteSelecmedEdge() {
  if (props.readonly || !lf || !selecmedEdgeData.value) return
  const { from, to } = selecmedEdgeData.value
  lf.deleteEdgeByNodeId({ sourceNodeId: from, targetNodeId: to })
  selecmedEdgeId.value = null
  selecmedNodeId.value = null
  refreshGraphData()
  emitUpdate()
}

watch(
  () => props.modelValue,
  (val) => applyModelValue(val),
  { immediate: true }
)

/** 运行时切换主题：官方示例用 lf.setTheme({}, mode)，见 https://logicflow.cn/tutorial/basic/theme */
watch(lfThemeMode, (mode: LogicFlow.ThemeMode) => {
  const lfAny = lf as unknown as { setTheme?: (config: object, themeMode?: LogicFlow.ThemeMode) => void }
  if (lf && typeof lfAny.setTheme === 'function') {
    lfAny.setTheme({}, mode)
  }
})

onMounted(() => {
  initLogicFlow()
  document.addEventListener('fullscreenchange', onFullscreenChange)
  getRoleOptions().then((list) => {
    const opts = list || []
    roleOptions.value = opts.map((o: { dictLabel?: string; dictValue?: string | number; label?: string; value?: string | number }) => ({
      label: o.dictLabel ?? o.label ?? String(o.dictValue ?? o.value ?? ''),
      value: String(o.dictValue ?? o.value ?? '')
    }))
  }).catch(() => {})
  getDeptTreeOptions('0').then((list) => {
    const opts = list || []
    const map = (o: { dictLabel?: string; dictValue?: string | number; label?: string; value?: string | number; children?: unknown[] }): DeptOption => ({
      label: o.dictLabel ?? o.label ?? String(o.dictValue ?? o.value ?? ''),
      value: String(o.dictValue ?? o.value ?? ''),
      children: Array.isArray((o as { children?: unknown[] }).children) ? (o as { children: unknown[] }).children.map((c) => map(c as Parameters<typeof map>[0])) : undefined
    })
    deptTreeOptions.value = opts.map((o) => map(o as Parameters<typeof map>[0]))
  }).catch(() => {})
  getUserOptions().then((list) => {
    const opts = list || []
    userOptions.value = opts.map((o: { dictLabel?: string; dictValue?: string | number; label?: string; value?: string | number }) => ({
      label: o.dictLabel ?? o.label ?? String(o.dictValue ?? o.value ?? ''),
      value: String(o.dictValue ?? o.value ?? '')
    }))
  }).catch(() => {})
})

onBeforeUnmount(() => {
  document.removeEventListener('fullscreenchange', onFullscreenChange)
  controlFitCenterUnbind?.()
  if (lf) {
    lf.destroy()
    lf = null
  }
})

/** 与官方示例一致的全局样式：颜色 + 圆角（rect 用 rx/ry，polygon/diamond 用 radius）；流程节点在 ourToLfGraph 里用 getNodeStyle 覆盖 fill/stroke */
const lfStyleConfig: Partial<LogicFlow.Options> = {
  style: {
    rect: { rx: 5, ry: 5, strokeWidth: 2 },
    circle: { fill: '#f5f5f5', stroke: '#666' },
    ellipse: { fill: '#dae8fc', stroke: '#6c8ebf' },
    polygon: { fill: '#d5e8d4', stroke: '#82b366', radius: 5 },
    diamond: { fill: '#ffe6cc', stroke: '#d79b00', radius: 5 },
    text: { color: '#b85450', fontSize: 12 }
  }
}

function initLogicFlow() {
  const el = containerRef.value
  if (!el) return
  // themeMode 见官方主题教程 http://logicflow.cn/tutorial/basic/theme
  lf = new LogicFlow({
    container: el,
    plugins: [DndPanel, SelectionSelect],
    grid: true,
    keyboard: { enabled: true },
    history: true,
    edgeType: 'polyline',
    themeMode: lfThemeMode.value,
    ...lfStyleConfig,
    isSilentMode: props.readonly,
    stopScrollGraph: props.readonly,
    stopZoomGraph: props.readonly,
    stopMoveGraph: props.readonly,
    adapterIn(data: unknown) {
      const our = typeof data === 'string' ? parseJson(data) : (data as { nodes: FlowNode[]; edges: FlowEdge[] })
      return ourToLfGraph(our) as LogicFlow.GraphData
    },
    adapterOut(data: LogicFlow.GraphData) {
      return lfToOurGraph(data)
    }
  })
  lf.on('history:change', () => emitUpdate())
  lf.on('node:click', ({ data }: { data: { id: string } }) => {
    selecmedNodeId.value = data.id
    selecmedEdgeId.value = null
    refreshGraphData()
  })
  lf.on('edge:click', ({ data }: { data: { sourceNodeId: string; targetNodeId: string } }) => {
    selecmedEdgeId.value = `${data.sourceNodeId}-${data.targetNodeId}`
    selecmedNodeId.value = null
    refreshGraphData()
  })
  lf.on('blank:click', () => {
    selecmedNodeId.value = null
    selecmedEdgeId.value = null
  })
  lf.on('node:add', ({ data }: { data: { id: string; properties?: Record<string, unknown> } }) => {
    const flowType = data.properties?.flowType as string | undefined
    if (flowType === 'start' || flowType === 'end') {
      const raw = lf!.getGraphRawData()
      const alreadyHas = raw.nodes.some(
        (n) => (n.properties as { flowType?: string })?.flowType === flowType && n.id !== data.id
      )
      if (alreadyHas) {
        nextTick(() => {
          lf!.deleteNode(data.id)
          refreshGraphData()
          emitUpdate()
        })
      }
    }
  })
  // 包装 fitView（若有），使 control 中「适应」也居中画布
  const lfAny = lf as unknown as { fitView?: () => void }
  if (lf && typeof lfAny.fitView === 'function') {
    const rawFitView = lfAny.fitView.bind(lf)
    lfAny.fitView = () => {
      rawFitView()
      lf!.translateCenter()
    }
  }
  nextTick(() => {
    applyModelValue(props.modelValue)
    // 与官方示例一致：显式 setTheme 确保当前 mode 生效
    const lfAny = lf as unknown as { setTheme?: (config: object, mode: string) => void }
    if (typeof lfAny.setTheme === 'function') {
      lfAny.setTheme({}, lfThemeMode.value)
    }
    setTimeout(() => bindControlFitCenter(el), 200)
  })
}

/** 绑定 control 面板点击：点击「适应」按钮（通常为第 3 个 .lf-control-item）后延迟执行 translateCenter */
let controlFitCenterUnbind: (() => void) | null = null
function bindControlFitCenter(container: HTMLDivElement) {
  controlFitCenterUnbind?.()
  if (!lf) return
  const control = container.querySelector('.lf-control')
  if (!control) return
  const handler = (e: Event) => {
    const item = (e.target as Element).closest('.lf-control-item')
    if (!item) return
    const items = control.querySelectorAll('.lf-control-item')
    const index = Array.from(items).indexOf(item)
    const title = (item.getAttribute('title') ?? item.textContent ?? '').trim()
    const isFit =
      index === 2 ||
      /适应|fit|adapt/i.test(title)
    if (isFit) {
      setTimeout(() => lf?.translateCenter(), 80)
    }
  }
  control.addEventListener('click', handler)
  controlFitCenterUnbind = () => {
    control.removeEventListener('click', handler)
    controlFitCenterUnbind = null
  }
}

function syncToModel() {
  emitUpdate()
}

function setDataFromJson(json: string) {
  applyModelValue(json)
  emitUpdate()
}

function clear() {
  if (lf) {
    lf.render({ nodes: [], edges: [] })
    graphData.value = { nodes: [], edges: [] }
    selecmedNodeId.value = null
    selecmedEdgeId.value = null
    syncDndPanelItems()
    emitUpdate()
  }
}

defineExpose({
  getDataJson,
  syncToModel,
  setDataFromJson,
  clear
})
</script>

<style scoped>
/* 样式：主题以 LogicFlow 官方 themeMode + setTheme 为准（见 http://logicflow.cn/tutorial/basic/theme）；下面为容器与右侧属性面板兜底 */
.takt-flow-logic-designer {
  position: relative;
  display: flex;
  width: 100%;
  height: 100%;
  min-height: 320px;
  box-sizing: border-box;
  border: 1px solid var(--ant-color-border, #d9d9d9);
  border-radius: var(--ant-border-radius, 6px);
  overflow: hidden;
  background: var(--ant-color-bg-container, #fff);
}
.takt-flow-logic-designer--fullscreen {
  width: 100vw;
  height: 100vh;
  border-radius: 0;
}
.takt-flow-logic-designer__fullscreen-btn {
  position: absolute;
  top: 8px;
  right: 8px;
  z-index: 10;
  width: 32px;
  height: 32px;
  padding: 0;
  color: var(--ant-color-text-secondary);
  border-radius: var(--ant-border-radius, 6px);
}
.takt-flow-logic-designer__fullscreen-btn:hover {
  color: var(--ant-color-text);
  background: var(--ant-color-fill-quaternary);
}

/* 画布区域填满父级（父级在 scheme-form 中设为 80vh） */
.takt-flow-logic-designer__canvas-wrap {
  flex: 1;
  min-width: 0;
  min-height: 0;
}

.takt-flow-logic-designer__canvas {
  width: 100%;
  height: 100%;
}

.takt-flow-logic-designer__property-panel {
  width: 240px;
  flex-shrink: 0;
  border-left: 1px solid var(--ant-color-border, #d9d9d9);
  display: flex;
  flex-direction: column;
  overflow: hidden;
  background-color: var(--ant-color-bg-container, #fff);
}

.takt-flow-logic-designer__property-title {
  font-size: 12px;
  font-weight: 600;
  color: var(--ant-color-text-heading, rgba(0, 0, 0, 0.88));
  padding: 8px 12px;
  border-bottom: 1px solid var(--ant-color-border, #d9d9d9);
  background-color: var(--ant-color-bg-container, #fff);
}

.takt-flow-logic-designer__property-body {
  flex: 1;
  overflow-y: auto;
  padding: 12px;
  background-color: var(--ant-color-bg-container, #fff);
}

.takt-flow-logic-designer__property-empty {
  font-size: 12px;
  color: var(--ant-color-text-tertiary, rgba(0, 0, 0, 0.45));
  text-align: center;
  padding: 24px 8px;
}

.takt-flow-logic-designer__property-form {
  :deep(.ant-form-item) { margin-bottom: 12px; }
}

/* 暗色兜底：LogicFlow themeMode='dark' 会改画布；control/dnd 若未随 theme 变化则用此处 */
.takt-flow-logic-designer--dark .takt-flow-logic-designer__property-panel,
.takt-flow-logic-designer--dark .takt-flow-logic-designer__property-body,
.takt-flow-logic-designer--dark .takt-flow-logic-designer__property-title {
  background-color: var(--ant-color-bg-container, #1f1f1f) !important;
  border-color: var(--ant-color-border, rgba(255, 255, 255, 0.12));
}
.takt-flow-logic-designer--dark .takt-flow-logic-designer__property-title {
  color: var(--ant-color-text-heading, rgba(255, 255, 255, 0.85));
}
.takt-flow-logic-designer--dark .takt-flow-logic-designer__property-empty {
  color: var(--ant-color-text-tertiary, rgba(255, 255, 255, 0.45));
}
.takt-flow-logic-designer--dark :deep(.lf-control),
.takt-flow-logic-designer--dark :deep(.lf-dndpanel) {
  background-color: var(--ant-color-bg-container, #1f1f1f) !important;
  background: var(--ant-color-bg-container, #1f1f1f) !important;
  border-color: var(--ant-color-border, rgba(255, 255, 255, 0.12)) !important;
}
.takt-flow-logic-designer--dark :deep(.lf-control-item) {
  color: var(--ant-color-text, rgba(255, 255, 255, 0.85));
}
/* Control 图标是 <i> 的 background-image(PNG)，不是 SVG，用 filter 反转为浅色 */
.takt-flow-logic-designer--dark :deep(.lf-control-item i) {
  filter: invert(1) brightness(1.15);
}
.takt-flow-logic-designer--dark :deep(.lf-control-item:hover) {
  background: var(--ant-color-fill-quaternary, rgba(255, 255, 255, 0.08)) !important;
}
.takt-flow-logic-designer--dark :deep(.lf-dndpanel .lf-dnd-item),
.takt-flow-logic-designer--dark :deep(.lf-dndpanel .lf-dnd-item-label) {
  color: var(--ant-color-text, rgba(255, 255, 255, 0.85));
}
/* 仅给画布容器设背景色，不盖住 .lf-background 下的网格层 .lf-grid，否则网格全主题不可见 */
.takt-flow-logic-designer :deep(.lf-graph) {
  background-color: var(--ant-color-bg-container, #fff);
}
.takt-flow-logic-designer--dark :deep(.lf-graph) {
  background-color: var(--ant-color-bg-container, #1f1f1f) !important;
}
/* 暗色下网格：LogicFlow 默认网格色在暗底上不可见，强制 .lf-grid 内 SVG 为浅色 */
.takt-flow-logic-designer--dark :deep(.lf-grid path),
.takt-flow-logic-designer--dark :deep(.lf-grid circle) {
  stroke: rgba(255, 255, 255, 0.18) !important;
  fill: rgba(255, 255, 255, 0.18) !important;
}
</style>
