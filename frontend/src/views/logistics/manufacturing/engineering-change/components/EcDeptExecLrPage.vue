<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：EcDeptExecLrPage.vue -->
<!-- 功能描述：执行部门左右主子表壳：左栏设变主表，右栏为本部门未实施、已实施与设变明细三个页签 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-list-layout p-4 flex flex-col min-h-0 h-full">
    <TaktMasterDetailTableLr
      ref="lrRef"
      v-model:master-current="tePage"
      v-model:master-page-size="tePageSize"
      v-model:selecmed-master-key="selecmedTeKey"
      class="min-h-0 flex-1"
      :master-columns="teColumns"
      :master-data-source="teRows"
      :master-loading="teLoading"
      :master-row-key="getTeId"
      master-id-column-key="ecTeId"
      :master-visible-column-keys="teVisibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="teTotal"
      master-entity-scope="company"
      @master-pagination-change="loadTe"
      @master-select="handleTeSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="teKeyword"
          :placeholder="t('common.page.form.placeholder.search')"
          :loading="teLoading"
          @search="handleTeSearch"
          @reset="handleTeReset"
        >
          <template #leading>
            <span class="whitespace-nowrap text-sm text-text-secondary">{{ t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange') }}</span>
            <a-range-picker
              v-model:value="teEntryPeriod"
              picker="month"
              format="YYYY-MM"
              value-format="YYYY-MM"
              class="w-64 min-w-56"
              :placeholder="[
                t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
                t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
              ]"
              :disabled-date="entryPeriodLimit.disabledDate"
              @calendar-change="entryPeriodLimit.onCalendarChange"
              @open-change="entryPeriodLimit.onOpenChange"
            />
          </template>
        </TaktQueryBar>
        <TaktToolsBar
          :show-create="false"
          :show-create-row="false"
          :show-update="false"
          :show-delete="false"
          :show-delete-row="false"
          :show-import="false"
          :show-export="false"
          :show-expand="false"
          :show-advanced-query="false"
          :show-column-setting="true"
          :show-fullscreen="true"
          :show-refresh="true"
          :refresh-loading="teLoading"
          @column-setting="teColumnSettingVisible = true"
          @refresh="loadTe"
        />
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'changeStatus'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_status" :value="record.changeStatus" />
        </template>
        <template v-else-if="column.key === 'ecImplScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_impl_scope_category" :value="record.ecImplScope" />
        </template>
        <template v-else-if="column.key === 'ecControlScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_control_scope_category" :value="record.ecControlScope" />
        </template>
        <template v-else-if="column.key === 'ecStatus'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_te_status" :value="record.ecStatus" />
        </template>
        <template v-else-if="column.key === 'ecLeader'">
          <TaktDictTag
            v-if="record.ecLeader"
            :value="record.ecLeader"
            :option="ecGroupOption(record.ecLeader)"
          />
        </template>
        <template v-else-if="isCalendarDateColumn(column.key)">
          {{ formatCalendarDate(record[String(column.key)]) }}
        </template>
      </template>
      <template #detail>
        <TaktPaneTabs
          ref="tabsRef"
          v-model:active-key="rightTab"
          class="flex h-full min-h-0 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-tabpane]:h-full"
          :items="rightTabItems"
          destroy-inactive-tab-pane
        >
          <template
            v-for="pane in execPanes"
            :key="pane.key"
            #[pane.key]
          >
            <div class="flex h-full min-h-0 flex-col overflow-hidden">
              <TaktQueryBar
                v-model="queryKeyword"
                :placeholder="t('common.page.form.placeholder.search')"
                :loading="loading"
                @search="handleSearch"
                @reset="handleReset"
              />
              <TaktToolsBar
                :update-permission="updatePermission"
                :export-permission="exportPermission"
                :show-create="false"
                :show-create-row="false"
                :show-update="true"
                :show-delete="false"
                :show-delete-row="false"
                :show-import="false"
                :show-export="true"
                :show-expand="false"
                :show-advanced-query="false"
                :show-column-setting="true"
                :show-fullscreen="true"
                :show-refresh="true"
                :update-disabled="updateDisabled"
                :update-loading="loading"
                :export-loading="loading"
                :refresh-loading="loading"
                @update="handleUpdate"
                @export="handleExport"
                @column-setting="columnSettingVisible = true"
                @refresh="handleRefresh"
              />
              <div ref="execTableWrapRef" class="min-h-0 flex-1 overflow-hidden">
                <TaktSingleTable
                  class="h-full min-h-0"
                  entity-scope="company"
                  :columns="columns"
                  :visible-column-keys="visibleColumnKeys"
                  :id-column-key="idField"
                  table-mode="masterDetailDetail"
                  scroll-layout="masterDetailLr"
                  :scroll="{ y: tabTableScrollY }"
                  :data-source="dataSource"
                  :loading="loading"
                  :stripe="true"
                  :virtual="true"
                  :row-key="getMasterId"
                  :row-selection="rowSelection"
                  :show-pagination="true"
                  v-model:current="currentPage"
                  v-model:page-size="pageSize"
                  :total="total"
                  @pagination-change="loadData"
                  @resize-column="handleResizeColumn"
                >
                  <template #bodyCell="{ column, record }">
                    <template v-if="column.key === 'discontinuedStatus'">
                      <TaktDictTag dict-type="logistics_materials_material_discontinued_status" :value="record.discontinuedStatus" />
                    </template>
                    <template v-else-if="column.key === 'ecNewPurchaseType'">
                      <TaktDictTag dict-type="logistics_procurement_type" :value="record.ecNewPurchaseType" />
                    </template>
                    <template v-else-if="column.key === 'ecOldPartDisposition'">
                      <TaktDictTag dict-type="logistics_manufacturing_ec_old_part_disposition" :value="record.ecOldPartDisposition" />
                    </template>
                    <template v-else-if="column.key === 'ecImplScope'">
                      <TaktDictTag dict-type="logistics_manufacturing_ec_impl_scope_category" :value="record.ecImplScope" />
                    </template>
                    <template v-else-if="column.key === 'ecControlScope'">
                      <TaktDictTag dict-type="logistics_manufacturing_ec_control_scope_category" :value="record.ecControlScope" />
                    </template>
                    <template v-else-if="column.key === 'techLeader' || column.key === 'sopLeader'">
                      <TaktDictTag
                        v-if="record[String(column.key)]"
                        :value="record[String(column.key)]"
                        :option="ecGroupOption(record[String(column.key)])"
                      />
                    </template>
                    <template v-else-if="isYesNoField(String(column.key))">
                      <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
                    </template>
                    <template v-else-if="isCalendarDateColumn(column.key)">
                      {{ formatCalendarDate(record[String(column.key)]) }}
                    </template>
                  </template>
                </TaktSingleTable>
              </div>
            </div>
          </template>
          <template #detail>
            <EcDeptExecDetailPanel
              ref="detailPanelRef"
              class="h-full min-h-0"
              :ec-te-id="selecmedTeKey"
              :scroll-y="tabTableScrollCap"
            />
          </template>
        </TaktPaneTabs>
      </template>
    </TaktMasterDetailTableLr>
    <TaktModal
      v-model:open="formVisible"
      :title="t('common.dialog.title.edit', { entity: t(menuI18nKey) })"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
    >
      <component
        :is="formComponent"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="teColumnSettingVisible"
      :columns="teColumns"
      :checked-keys="teVisibleColumnKeys"
      id-column-key="ecTeId"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleTeColumnKeysChange"
      @reset="handleTeColumnReset"
    />
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="idField"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 执行部门左右主子表页面壳（左栏设变主表，右栏未实施、已实施与设变明细）
 */
import '../ec-list-layout.css'
import type { Component } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import { message, Modal } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiEditLine, RiPlayCircleLine, RiStopCircleLine } from '@remixicon/vue'
import type { TaktPaneTabItem } from '@/components/business/TaktPaneTabs/index.vue'
import { getEcTeList } from '@/api/logistics/manufacturing/engineering-change/ecTe.api'
import { getEcGroupOptions } from '@/api/logistics/manufacturing/engineering-change/ecGroup.api'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { formatCalendarDate } from '@/utils/datetime'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { TAKT_TABLE_INNER_TABS_CHROME_PX, measureConstrainedTableScrollY } from '@/utils/tableScroll'
import type { TaktPagedResult, TaktSelectOption } from '@/types/common.types'
import type { EcTe } from '@/types/logistics/manufacturing/engineering-change/ecTe.types'
import type { TaktEcExecCode } from '@/constants/logistics/ecExecCodes.constants'
import { getEcDeptExecLineFields } from '@/constants/logistics/ecDeptExecLineFields.constants'
import {
  EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS,
  TaktEcScope,
} from '@/constants/logistics/ecScope.constants'
import { useEcExecSignalRGroup } from '@/composables/useEcDeptSignalrGroup'
import { taktShowEcRootMaterialDescription } from '@/constants/logistics/ecScope.constants'
import { useEcDeptViewI18n } from '../composables/useEcDeptViewI18n'
import { ECENG_LIST_FIELDS, useEcTeI18n } from '../ec-te/composables/useEcTeI18n'
import { ECDETAIL_SELF_I18N_KEY } from '../ec-te/composables/useEcDetailI18n'
import type { EcDeptExecMasterRow } from '../composables/useEcDeptExecMasterContext'
import EcDeptExecDetailPanel from './EcDeptExecDetailPanel.vue'

/** 主表用字典 sys_yes_no 渲染的字段 */
const YES_NO_FIELDS = new Set(['isImplemented', 'isSopUpdated', 'ecNewRequiresInspection'])
/** 设变组编码 → 选项（设变担当 / 制技担当） */
const ecGroupOptionByCode = ref<Record<string, TaktSelectOption>>({})

/**
 * 担当编码对应的设变组选项
 * @param value 设变组编码
 * @returns 选项；无映射时 undefined，标签回退编码
 */
function ecGroupOption(value: unknown): TaktSelectOption | undefined {
  if (value == null || value === '') {
    return undefined
  }
  return ecGroupOptionByCode.value[String(value)]
}

/** 预加载设变组选项，供担当列编码转名称 */
async function loadEcGroupLabelMap(): Promise<void> {
  try {
    const list = await getEcGroupOptions()
    const map: Record<string, TaktSelectOption> = {}
    for (const item of list ?? []) {
      if (item.dictValue != null && String(item.dictValue) !== '') {
        map[String(item.dictValue)] = item
      }
    }
    ecGroupOptionByCode.value = map
  } catch {
    ecGroupOptionByCode.value = {}
  }
}

/**
 * 列是否为日历日（字段名以 Date 结尾）
 * @param key 列 key
 * @returns {boolean} 是否按 yyyy-MM-dd 展示
 */
function isCalendarDateColumn(key: unknown): boolean {
  return String(key ?? '').endsWith('Date')
}

/**
 * 把行上的日期字段收成 yyyy-MM-dd，供表格与日期控件使用
 * @param row 列表或详情行
 * @returns {T} 日期已裁剪的新对象
 */
function applyCalendarDates<T extends Record<string, unknown>>(row: T): T {
  const next = { ...row }
  for (const key of Object.keys(next)) {
    if (!key.endsWith('Date')) {
      continue
    }
    const formatted = formatCalendarDate(next[key] as string | null | undefined)
    ;(next as Record<string, unknown>)[key] = formatted || undefined
  }
  return next
}

/** 左侧设变主表默认可见列：工厂、设变号、发行日期、标题、担当 */
const ENG_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'plantCode',
  'ecCode',
  'ecIssueDate',
  'ecTitle',
  'ecLeader',
] as const

/** 计划物料（在产） */
const PLANNED_MATERIAL_STATUS = 'Z0'
/** 停产按钮默认写入（生产结束） */
const EOL_MATERIAL_STATUS = 'ZQ'

const props = defineProps<{
  /** 更新权限（与控制器 [TaktPermission] 一致，末段 update） */
  updatePermission: string
  /** 导出权限（与控制器 [TaktPermission] 一致，末段 export） */
  exportPermission: string
  /** 菜单 i18n 键 */
  menuI18nKey: string
  /** 执行行主键字段（同明细上视图外键，如 ecMcId） */
  idField: string
  /** 部门实体 slug（ecpur / ecqc 等） */
  deptSlug: string
  /** SignalR 部门编码 */
  execCode: TaktEcExecCode
  /** 左栏部门执行主表列表（已含 VisibleExec 去重） */
  getMasterList: (query: any) => Promise<TaktPagedResult<any>>
  /** 按主键取执行行（含 EcDetails，供右栏） */
  getMasterById: (id: string) => Promise<any>
  /** 更新执行行 */
  updateMaster: (id: string, dto: any) => Promise<any>
  /**
   * 更新停产状态（Z0=在产 / ZQ=停产），并触发后端自动填充/清除
   * @param id 执行行主键
   * @param discontinuedStatus 根物料停产状态
   */
  updateDiscontinuedStatus: (id: string, discontinuedStatus: string) => Promise<any>
  /** 导出执行行 */
  exportMaster: (query?: any) => Promise<Blob>
  /** 编辑表单组件 */
  formComponent: Component
  /** 附加查询参数 */
  extraQuery?: Record<string, unknown>
}>()

const { t } = useI18n()
const pi = useEcDeptViewI18n(props.deptSlug)
const teI18n = useEcTeI18n()
const formModalWidthPx = useTaktContentModalWidth()
/** 未实施 */
const EXEC_NOT_IMPLEMENTED = 0
/** 已实施 */
const EXEC_IMPLEMENTED = 1
/** 未实施页签 */
const EXEC_TAB_PENDING = 'pending'
/** 已实施页签 */
const EXEC_TAB_DONE = 'done'
/** 页签文案键前缀 */
const DEPT_EXEC_TAB_I18N = 'logistics.manufacturing.engineering-change.dept-exec.page.tab'
/** 明细页签标题键 */
const detailSelfKey = ECDETAIL_SELF_I18N_KEY
/** 右栏页签 */
const rightTab = ref(EXEC_TAB_PENDING)
/** 当前页签的 0/1；制技对应是否更新，其余部门对应是否实施。停留在明细页签时保持上一次 */
const execImplemented = ref(EXEC_NOT_IMPLEMENTED)
/** 制造技术课按是否更新拆分 */
const usesSopUpdated = computed(() => props.deptSlug === 'ecme')
/** 未实施 / 已实施，或制技的未更新 / 已更新 */
const execPanes = computed(() => {
  const pendingLabel = usesSopUpdated.value ? 'notUpdated' : 'notImplemented'
  const doneLabel = usesSopUpdated.value ? 'updated' : 'implemented'
  return [
    {
      key: EXEC_TAB_PENDING,
      tab: `${t(props.menuI18nKey)}-${t(`${DEPT_EXEC_TAB_I18N}.${pendingLabel}`)}`,
    },
    {
      key: EXEC_TAB_DONE,
      tab: `${t(props.menuI18nKey)}-${t(`${DEPT_EXEC_TAB_I18N}.${doneLabel}`)}`,
    },
  ]
})
/** 右栏页签（执行态 + 明细；可按 show/permission 删减） */
const rightTabItems = computed<TaktPaneTabItem[]>(() => [
  ...execPanes.value.map((pane) => ({
    key: pane.key,
    label: pane.tab,
    paneClass: 'h-full min-h-0',
  })),
  {
    key: 'detail',
    label: t(detailSelfKey),
    paneClass: 'h-full min-h-0',
  },
])

/** 设变主表录入期间，截止本月 */
const teEntryPeriod = ref<[string, string] | null>(null)
/** 录入期间可选到本月，最多 12 个月 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })
/** 设变主表关键词 */
const teKeyword = ref('')
/** 设变主表 loading */
const teLoading = ref(false)
/** 设变主表行 */
const teRows = ref<EcTe[]>([])
/** 设变主表页码 */
const tePage = ref(getTaktDefaultPageIndex())
/** 设变主表页大小 */
const tePageSize = ref(getTaktDefaultPageSize())
/** 设变主表总数 */
const teTotal = ref(0)
/** 选中设变主表主键 */
const selecmedTeKey = ref('')
/** 选中设变单号 */
const selecmedEcCode = ref('')
/** 设变主表列设置 */
const teColumnSettingVisible = ref(false)
/** 设变主表可见列 */
const teVisibleColumnKeys = ref<string[]>([...ENG_DEFAULT_VISIBLE_COLUMN_KEYS])

/** 查询关键词 */
const queryKeyword = ref('')
/** 主表 loading */
const loading = ref(false)
/** 主表数据 */
const dataSource = ref<EcDeptExecMasterRow[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 选中行 */
const selecmedRows = ref<EcDeptExecMasterRow[]>([])
/** 选中 keys */
const selecmedRowKeys = ref<(string | number)[]>([])
/** 页签组件（测量页签栏高度） */
const tabsRef = ref<{ $el?: HTMLElement } | null>(null)
/** 左右布局根（读取左侧基础表 scroll.y） */
const lrRef = ref<{ $el?: HTMLElement } | null>(null)
/** 视口公式下的左右表 scroll.y（左侧尚未测到时的回退） */
const viewportBaseScrollY = useTaktTableViewportScrollY('masterDetailLr')
/** 左侧基础表实测 scroll.y */
const measuredBaseScrollY = ref(0)
/** 页签栏高度（未测到时用内嵌页签预留） */
const tabNavHeightPx = ref(TAKT_TABLE_INNER_TABS_CHROME_PX)
/** 执行表外壳（按剩余高度留出横向滚动条；页签内 ref 可能是节点或节点列表） */
const execTableWrapRef = ref<HTMLElement | HTMLElement[] | null>(null)
/** 执行表按外壳实测的 scroll.y */
const execFittedScrollY = ref(0)
/**
 * 页签内表格高度上限：左侧基础表减去页签栏
 */
const tabTableScrollCap = computed(() => {
  const base = measuredBaseScrollY.value > 0 ? measuredBaseScrollY.value : viewportBaseScrollY.value
  return Math.max(1, base - tabNavHeightPx.value)
})
/**
 * 执行表 scroll.y：不超过基础表减页签，且必须落在外壳内，横向滚动条才不会被裁掉
 */
const tabTableScrollY = computed(() => {
  const cap = tabTableScrollCap.value
  if (execFittedScrollY.value > 0) {
    return Math.min(cap, execFittedScrollY.value)
  }
  return cap
})
/** 列设置 */
const columnSettingVisible = ref(false)
/** 表单可见 */
const formVisible = ref(false)
/** 表单 loading */
const formLoading = ref(false)
/** 编辑数据 */
const formData = ref<EcDeptExecMasterRow | null>(null)
/** 表单 ref */
const formRef = ref<{ validate: () => Promise<void>; getValues: () => Record<string, unknown> } | null>(null)
/** 右栏明细面板 */
const detailPanelRef = ref<{ reload?: () => void } | null>(null)

/**
 * 是否用 sys_yes_no 字典展示
 * @param field 列 key
 * @returns {boolean} 是否是/否字段
 */
function isYesNoField(field: string): boolean {
  return YES_NO_FIELDS.has(field)
}

/**
 * 主表列宽
 * @param field DTO camelCase
 * @returns {number} 列宽
 */
function masterColumnWidth(field: string): number {
  if (field === props.idField || (field.endsWith('Id') && field !== 'ecDetailId')) {
    return 170
  }
  if (field === 'execContent' || field === 'supplier') {
    return 180
  }
  if (field === 'ecRootMaterialDescription' || field === 'ecParentMaterialDescription' || field === 'deptName') {
    return 160
  }
  if (YES_NO_FIELDS.has(field) || field === 'productionTeam' || field === 'lineNumber' || field === 'deptCode') {
    return 100
  }
  if (field === 'ecRootMaterialCodeGroup' || field === 'ecModelCodeGroup') {
    return 220
  }
  if (field.endsWith('Date') || field.endsWith('Code') || field.endsWith('Batch') || field === 'ecRootMaterialCode') {
    return 140
  }
  return 120
}

/**
 * 是否视为停产（各部门执行表 DiscontinuedStatus：非空且非 Z0）
 * @param record 主表行
 * @returns {boolean} 是否停产
 */
function isEolRow(record: EcDeptExecMasterRow): boolean {
  const status = String(record.discontinuedStatus ?? '').trim().toUpperCase()
  return !!status && status !== PLANNED_MATERIAL_STATUS
}

/**
 * 是否计划物料 Z0（可显示修改）：仅看执行表 DiscontinuedStatus；空视为 Z0
 * @param record 主表行
 * @returns {boolean} 是否 Z0 / 可填报状态
 */
function isZ0Row(record: EcDeptExecMasterRow): boolean {
  const status = String(record.discontinuedStatus ?? '').trim().toUpperCase()
  return !status || status === PLANNED_MATERIAL_STATUS
}

/**
 * 是否允许更新填报（按实施范围 + Z0）
 * - 技术/内部：各课均不可改
 * - 部管：仅采购/生管/受检/部管/SMT 且 Z0 可改；其余课不可改
 * - 按成品/按机种：各课 Z0 可改
 * @param record 主表行
 * @returns {boolean} 是否可更新
 */
function canUpdateRow(record: EcDeptExecMasterRow): boolean {
  const scope = Number(record.ecImplScope ?? 0)
  if (scope === TaktEcScope.Internal || scope === TaktEcScope.Technical) {
    return false
  }
  if (scope === TaktEcScope.MaterialControl) {
    if (!EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS.has(props.deptSlug)) {
      return false
    }
    return isZ0Row(record)
  }
  return isZ0Row(record)
}

/**
 * 是否显示停产/在产按钮
 * - 技术/内部：完全不需要
 * - 部管：仅采购/生管/受检/部管/SMT；其余课完全不需要
 * - 按成品/按机种：需要
 * @param record 主表行
 * @returns {boolean} 是否显示停产/在产
 */
function canShowDiscontinuedActions(record: EcDeptExecMasterRow): boolean {
  const scope = Number(record.ecImplScope ?? 0)
  if (scope === TaktEcScope.Internal || scope === TaktEcScope.Technical) {
    return false
  }
  if (scope === TaktEcScope.MaterialControl) {
    return EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS.has(props.deptSlug)
  }
  return true
}

/**
 * 更新停产状态并刷新
 * @param record 主表行
 * @param discontinuedStatus 目标状态
 * @param confirmKey 确认文案键
 */
function confirmDiscontinuedStatus(
  record: EcDeptExecMasterRow,
  discontinuedStatus: string,
  confirmKey: string,
) {
  const id = getMasterId(record)
  if (!id) {
    return
  }
  Modal.confirm({
    title: t(confirmKey),
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      loading.value = true
      try {
        await props.updateDiscontinuedStatus(id, discontinuedStatus)
        message.success(t('common.feedback.updated', { target: t(props.menuI18nKey) }))
        // 本地同步执行表 DiscontinuedStatus，驱动修改/停产/在产按钮显隐
        dataSource.value = dataSource.value.map((row) => {
          if (getMasterId(row) !== id) {
            return row
          }
          return { ...row, discontinuedStatus }
        })
        if (selecmedRowKeys.value.length === 1 && String(selecmedRowKeys.value[0]) === id) {
          const refreshed = dataSource.value.find((row) => getMasterId(row) === id)
          if (refreshed) {
            selecmedRows.value = [refreshed]
            syncMasterSelection(refreshed)
          }
        }
        await loadData()
        detailPanelRef.value?.reload?.()
      } finally {
        loading.value = false
      }
    },
  })
}

/** 左侧设变主表列（TaktEcTe） */
const teColumns = computed<TableColumnsType>(() => [
  {
    title: teI18n.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 100,
    ellipsis: true,
  },
  ...ECENG_LIST_FIELDS.map((field) => ({
    title: teI18n.label(field),
    dataIndex: field,
    key: field,
    width: field === 'ecContent' || field === 'ecTitle' ? 180 : 120,
    ellipsis: true,
  })),
])

/** 主表列（部门执行实体字段 + 停产/在产操作；数据列一律 ellipsis，禁止长文本撑高行） */
const columns = computed<TableColumnsType>(() => {
  const fieldCols = getEcDeptExecLineFields(props.deptSlug)
    .filter((field) => field !== 'ecRootMaterialDescription'
      || dataSource.value.some((row) => taktShowEcRootMaterialDescription(row.ecImplScope, row.ecControlScope)))
    .map((field) => ({
    title: pi.label(field),
    dataIndex: field,
    key: field,
    width: masterColumnWidth(field),
    ellipsis: true,
  }))
  fieldCols.push(
    CreateActionColumn<EcDeptExecMasterRow>({
      width: 120,
      actions: [
        {
          key: 'update',
          label: t('common.page.button.edit'),
          shape: 'plain',
          icon: RiEditLine,
          buttonClass: 'takt-button-update',
          permission: props.updatePermission,
          visible: (record) => canUpdateRow(record),
          onClick: (record) => {
            void openEditForm(record)
          },
        },
        {
          key: 'discontinue',
          label: t('common.page.button.discontinue'),
          shape: 'plain',
          icon: RiStopCircleLine,
          buttonClass: 'takt-button-discontinue',
          permission: props.updatePermission,
          visible: (record) => canShowDiscontinuedActions(record) && !isEolRow(record),
          onClick: (record) =>
            confirmDiscontinuedStatus(
              record,
              EOL_MATERIAL_STATUS,
              'common.page.button.discontinue',
            ),
        },
        {
          key: 'inproduction',
          label: t('common.page.button.inproduction'),
          shape: 'plain',
          icon: RiPlayCircleLine,
          buttonClass: 'takt-button-inproduction',
          permission: props.updatePermission,
          visible: (record) => canShowDiscontinuedActions(record) && isEolRow(record),
          onClick: (record) =>
            confirmDiscontinuedStatus(
              record,
              PLANNED_MATERIAL_STATUS,
              'common.page.button.inproduction',
            ),
        },
      ],
    }),
  )
  return fieldCols
})
/** 可见列 */
const visibleColumnKeys = ref<string[]>([])

/** 行选择 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDeptExecMasterRow[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
  },
}))

/** 更新按钮禁用：须单选且当前行允许更新（以 dataSource 最新停产状态为准） */
const updateDisabled = computed(() => {
  if (selecmedRowKeys.value.length !== 1) {
    return true
  }
  const key = String(selecmedRowKeys.value[0] ?? '')
  const row =
    dataSource.value.find((r) => getMasterId(r) === key) ?? selecmedRows.value[0]
  return !row || !canUpdateRow(row)
})

/**
 * 主表主键
 * @param record 主表行
 * @returns {string} 主键
 */
function getMasterId(record: EcDeptExecMasterRow | Record<string, unknown>) {
  return String(record[props.idField] ?? '')
}

/**
 * 同步主表选中到右栏
 * @param record 主表行
 */
function syncMasterSelection(record: EcDeptExecMasterRow | null) {
  if (!record) {
    selecmedRowKeys.value = []
    selecmedRows.value = []
    return
  }
  const key = getMasterId(record)
  selecmedRowKeys.value = key ? [key] : []
  selecmedRows.value = key ? [record] : []
}

/**
 * 设变主表主键
 * @param record 设变主表行
 * @returns {string} 主键
 */
function getTeId(record: EcTe | Record<string, unknown>) {
  const row = record as EcTe & { id?: string }
  return String(row.ecTeId || row.id || '')
}

/**
 * 选中左侧设变后加载本部门执行行
 * @param record 设变主表行
 */
function handleTeSelect(record: Record<string, unknown>) {
  const row = record as EcTe
  selecmedTeKey.value = getTeId(row)
  selecmedEcCode.value = String(row.ecCode ?? '').trim()
  currentPage.value = getTaktDefaultPageIndex()
  selecmedRowKeys.value = []
  selecmedRows.value = []
  void loadData()
  nextTick(() => detailPanelRef.value?.reload?.())
}

/**
 * 加载左侧设变主表
 */
async function loadTe() {
  teLoading.value = true
  try {
    const entryDates = entryPeriodToQuery(teEntryPeriod.value)
    const res = await getEcTeList({
      pageIndex: tePage.value,
      pageSize: tePageSize.value,
      ecEntryDateStart: entryDates.start,
      ecEntryDateEnd: entryDates.end,
      keyWords: teKeyword.value.trim() || undefined,
      execDeptCode: props.execCode,
    })
    teRows.value = (res.data ?? []).map((row) => applyCalendarDates(row as Record<string, unknown>)) as EcTe[]
    teTotal.value = res.total ?? 0
    const found = teRows.value.find((row) => getTeId(row) === selecmedTeKey.value)
    if (!found) {
      selecmedTeKey.value = ''
      selecmedEcCode.value = ''
      dataSource.value = []
      total.value = 0
    } else {
      selecmedEcCode.value = String(found.ecCode ?? '').trim()
    }
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载设变主表失败', { error })
    message.error(t('common.feedback.load.data.failed'))
    teRows.value = []
    teTotal.value = 0
  } finally {
    teLoading.value = false
  }
}

/** 搜索设变主表 */
function handleTeSearch() {
  tePage.value = getTaktDefaultPageIndex()
  void loadTe()
}

/**
 * 录入期间转成技术主表的日期起止。结束日取该月最后一天
 * @param range 年月区间
 * @returns 录入日期起止
 */
function entryPeriodToQuery(range: [string, string] | null): { start?: string; end?: string } {
  if (!range?.[0] || !range?.[1]) {
    return {}
  }
  const parts = range[1].split('-').map(Number)
  const year = parts[0]
  const month = parts[1]
  if (!year || !month) {
    return { start: `${range[0]}-01` }
  }
  const lastDay = new Date(year, month, 0).getDate()
  return {
    start: `${range[0]}-01`,
    end: `${range[1]}-${String(lastDay).padStart(2, '0')}`,
  }
}

/** 左栏默认：录入期间截止本月共 3 个月 */
function applyTeQueryDefaults(): void {
  teEntryPeriod.value = buildDefaultThroughCurrentPeriodRange(3)
}

/** 重置设变主表查询（关键字由 TaktQueryBar 清空） */
async function handleTeReset() {
  tePage.value = getTaktDefaultPageIndex()
  await applyTeQueryDefaults()
  void loadTe()
}

/**
 * 设变主表列显隐
 * @param keys 可见列
 */
function handleTeColumnKeysChange(keys: string[]) {
  teVisibleColumnKeys.value = keys
}

/** 设变主表列重置 */
function handleTeColumnReset() {
  teVisibleColumnKeys.value = [...ENG_DEFAULT_VISIBLE_COLUMN_KEYS]
}

/**
 * 当前页签的过滤条件。制技用 isSopUpdated（0=未更新，1=已更新），其余部门用 isImplemented。
 * @returns {Record<string, number>} 查询字段
 */
function implementationQuery(): Record<string, number> {
  if (props.deptSlug === 'ecme') {
    return { isSopUpdated: execImplemented.value }
  }
  return { isImplemented: execImplemented.value }
}

/** 加载本部门执行表 */
async function loadData() {
  if (!selecmedEcCode.value) {
    dataSource.value = []
    total.value = 0
    return
  }
  loading.value = true
  try {
    const res = await props.getMasterList({
      ...(props.extraQuery ?? {}),
      ecCode: selecmedEcCode.value,
      ...implementationQuery(),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
    })
    dataSource.value = (res.data ?? []).map((row) => applyCalendarDates(row as Record<string, unknown>)) as EcDeptExecMasterRow[]
    total.value = res.total ?? 0
    const currentKey = String(selecmedRowKeys.value[0] ?? '')
    if (currentKey) {
      const found = dataSource.value.find((row) => getMasterId(row) === currentKey)
      syncMasterSelection(found ?? null)
      // 停产/在产切换后须用新行刷新选中，否则工具栏更新显隐仍按旧 discontinuedStatus
      if (found) {
        selecmedRowKeys.value = [currentKey]
        selecmedRows.value = [found]
      } else {
        selecmedRowKeys.value = []
        selecmedRows.value = []
      }
    }
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载主表失败', { error })
    message.error(t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
    syncMasterSelection(null)
    selecmedRowKeys.value = []
    selecmedRows.value = []
  } finally {
    loading.value = false
  }
}

/** 搜索 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 刷新 */
function handleRefresh() {
  loadData()
}

/** 列宽 */
function handleResizeColumn() {}

/**
 * 列显隐
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/**
 * 执行部门主表默认可见列：主键 ID、行号默认隐藏（列设置未勾选）；仅本页特例
 * @returns {string[]} 默认可见列 key（含 action）
 */
function defaultVisibleMasterColumnKeys(): string[] {
  const hidden = new Set([props.idField, 'lineNumber'])
  return [
    ...columns.value
      .map((c) => String(c.key))
      .filter((k) => k !== 'action' && !hidden.has(k)),
    'action',
  ]
}

/** 列重置（恢复默认：不含主键/行号） */
function handleColumnSettingReset() {
  visibleColumnKeys.value = defaultVisibleMasterColumnKeys()
}

/**
 * 打开编辑：按主键 getById 拉全量（含 DeptName / 执行主键），与技术课编辑一致
 * @param record 主表行
 */
async function openEditForm(record: EcDeptExecMasterRow) {
  const key = getMasterId(record)
  if (!key) {
    message.error(t('common.feedback.failed'))
    return
  }
  selecmedRowKeys.value = [key]
  selecmedRows.value = [record]
  syncMasterSelection(record)
  loading.value = true
  try {
    const detail = await props.getMasterById(key)
    const source = (detail ?? record) as Record<string, unknown>
    formData.value = applyCalendarDates({ ...source }) as EcDeptExecMasterRow
    formVisible.value = true
  } finally {
    loading.value = false
  }
}

/** 工具栏编辑 */
function handleUpdate() {
  if (selecmedRowKeys.value.length !== 1) {
    return
  }
  const key = String(selecmedRowKeys.value[0] ?? '')
  const row =
    dataSource.value.find((r) => getMasterId(r) === key) ?? selecmedRows.value[0]
  if (!row || !canUpdateRow(row)) {
    return
  }
  void openEditForm(row)
}

/** 提交 */
async function handleFormSubmit() {
  if (!formRef.value || !formData.value) {
    return
  }
  await formRef.value.validate()
  const dto = formRef.value.getValues()
  formLoading.value = true
  try {
    await props.updateMaster(getMasterId(formData.value), dto)
    message.success(t('common.feedback.updated', { target: t(props.menuI18nKey) }))
    formVisible.value = false
    await loadData()
    detailPanelRef.value?.reload?.()
  } finally {
    formLoading.value = false
  }
}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true
    const blob = await props.exportMaster({
      ...(props.extraQuery ?? {}),
      ecCode: selecmedEcCode.value || undefined,
      ...implementationQuery(),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
    })
    const url = window.URL.createObjectURL(blob as Blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `${t(props.menuI18nKey)}.xlsx`
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
  } finally {
    loading.value = false
  }
}

watch(
  () => getEcDeptExecLineFields(props.deptSlug).join(','),
  () => {
    visibleColumnKeys.value = defaultVisibleMasterColumnKeys()
  },
  { immediate: true },
)

useTableRefresh(loadTe)
useEcExecSignalRGroup(props.execCode)

watch(rightTab, (tab) => {
  if (tab === EXEC_TAB_PENDING || tab === EXEC_TAB_DONE) {
    const nextImplemented = tab === EXEC_TAB_DONE ? EXEC_IMPLEMENTED : EXEC_NOT_IMPLEMENTED
    if (execImplemented.value !== nextImplemented) {
      execImplemented.value = nextImplemented
      currentPage.value = getTaktDefaultPageIndex()
      selecmedRowKeys.value = []
      selecmedRows.value = []
      void loadData()
    }
  }
  nextTick(() => {
    syncTabTableScroll()
    const wrap = resolveExecTableWrap()
    if (wrap && layoutResizeObserver) {
      layoutResizeObserver.observe(wrap)
    }
  })
})

/** 左右布局高度监听 */
let layoutResizeObserver: ResizeObserver | null = null

/**
 * 当前执行表外壳。页签内的 ref 可能收成数组。
 * @returns {HTMLElement | null} 外壳节点
 */
function resolveExecTableWrap(): HTMLElement | null {
  const raw = execTableWrapRef.value
  if (raw instanceof HTMLElement) {
    return raw
  }
  if (Array.isArray(raw)) {
    return raw.find((node) => node instanceof HTMLElement) ?? null
  }
  return null
}

/**
 * 读取左侧基础表高度、页签栏和执行表外壳，页签内表格既不超过基础表减页签，也不把横向滚动条裁出外壳
 */
function syncTabTableScroll(): void {
  const tabRoot = tabsRef.value?.$el
  const nav = tabRoot?.querySelector('.ant-tabs-nav')
  if (nav instanceof HTMLElement && nav.offsetHeight > 0) {
    tabNavHeightPx.value = nav.offsetHeight
  }
  const lrRoot = lrRef.value?.$el
  if (lrRoot) {
    const raw = getComputedStyle(lrRoot).getPropertyValue('--takt-master-detail-lr-scroll-y')
    const measured = Number.parseFloat(raw)
    if (Number.isFinite(measured) && measured > 0) {
      measuredBaseScrollY.value = measured
    }
  }
  const fitted = measureConstrainedTableScrollY(resolveExecTableWrap(), { reservePagination: true })
  if (fitted > 0) {
    execFittedScrollY.value = fitted
  }
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  await loadEcGroupLabelMap()
  await applyTeQueryDefaults()
  await loadTe()
  nextTick(() => {
    syncTabTableScroll()
    if (typeof ResizeObserver === 'undefined') {
      return
    }
    layoutResizeObserver = new ResizeObserver(() => syncTabTableScroll())
    const lrRoot = lrRef.value?.$el
    if (lrRoot) {
      layoutResizeObserver.observe(lrRoot)
    }
    const wrap = resolveExecTableWrap()
    if (wrap) {
      layoutResizeObserver.observe(wrap)
    }
  })
})

onBeforeUnmount(() => {
  layoutResizeObserver?.disconnect()
  layoutResizeObserver = null
})
</script>
