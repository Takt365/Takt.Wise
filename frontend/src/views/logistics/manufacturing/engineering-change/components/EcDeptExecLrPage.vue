<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：EcDeptExecLrPage.vue -->
<!-- 功能描述：执行部门左右主子表壳：左栏设变主表 TaktEcGijutsu，右栏页签为本部门执行表与设变明细 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <TaktMasterDetailTableLr
      ref="lrRef"
      v-model:master-current="gijutsuPage"
      v-model:master-page-size="gijutsuPageSize"
      v-model:selected-master-key="selectedGijutsuKey"
      class="min-h-0 flex-1"
      :master-columns="gijutsuColumns"
      :master-data-source="gijutsuRows"
      :master-loading="gijutsuLoading"
      :master-row-key="getGijutsuId"
      master-id-column-key="ecGijutsuId"
      :master-visible-column-keys="gijutsuVisibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="gijutsuTotal"
      master-entity-scope="company"
      @master-pagination-change="loadGijutsu"
      @master-select="handleGijutsuSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="gijutsuKeyword"
          :placeholder="t('common.page.form.placeholder.search')"
          :loading="gijutsuLoading"
          @search="handleGijutsuSearch"
          @reset="handleGijutsuReset"
        />
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
          :refresh-loading="gijutsuLoading"
          @column-setting="gijutsuColumnSettingVisible = true"
          @refresh="loadGijutsu"
        />
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'changeStatus'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_status" :value="record.changeStatus" />
        </template>
        <template v-else-if="column.key === 'ecScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_scope_category" :value="record.ecScope" />
        </template>
        <template v-else-if="column.key === 'ecDeptScope'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_dept_scope_category" :value="record.ecDeptScope" />
        </template>
        <template v-else-if="column.key === 'ecStatus'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_gijutsu_status" :value="record.ecStatus" />
        </template>
      </template>
      <template #detail>
        <a-tabs
          ref="tabsRef"
          v-model:active-key="rightTab"
          class="flex h-full min-h-0 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-tabpane]:h-full"
        >
          <a-tab-pane key="exec" :tab="t(menuI18nKey)" class="h-full min-h-0" :force-render="true">
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
                    <template v-else-if="column.key === 'ecScope'">
                      <TaktDictTag dict-type="logistics_manufacturing_ec_scope_category" :value="record.ecScope" />
                    </template>
                    <template v-else-if="column.key === 'ecDeptScope'">
                      <TaktDictTag dict-type="logistics_manufacturing_ec_dept_scope_category" :value="record.ecDeptScope" />
                    </template>
                    <template v-else-if="isYesNoField(String(column.key))">
                      <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
                    </template>
                  </template>
                </TaktSingleTable>
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="detail" :tab="t(detailSelfKey)" class="h-full min-h-0" :force-render="true">
            <EcDeptExecDetailPanel
              ref="detailPanelRef"
              class="h-full min-h-0"
              :ec-gijutsu-id="selectedGijutsuKey"
              :scroll-y="tabTableScrollCap"
            />
          </a-tab-pane>
        </a-tabs>
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
      v-model:open="gijutsuColumnSettingVisible"
      :columns="gijutsuColumns"
      :checked-keys="gijutsuVisibleColumnKeys"
      id-column-key="ecGijutsuId"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleGijutsuColumnKeysChange"
      @reset="handleGijutsuColumnReset"
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
 * 执行部门左右主子表页面壳（左栏设变主表，右栏执行表与明细页签）
 */
import type { Component } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import { message, Modal } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiEditLine, RiPlayCircleLine, RiStopCircleLine } from '@remixicon/vue'
import { getEcGijutsuList } from '@/api/logistics/manufacturing/engineering-change/ecGijutsu.api'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { TAKT_TABLE_INNER_TABS_CHROME_PX, measureConstrainedTableScrollY } from '@/utils/tableScroll'
import type { TaktPagedResult } from '@/types/common.types'
import type { EcGijutsu } from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types'
import type { TaktEcExecCode } from '@/constants/logistics/ecExecCodes.constants'
import { getEcDeptExecLineFields } from '@/constants/logistics/ecDeptExecLineFields.constants'
import {
  EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS,
  TaktEcScope,
} from '@/constants/logistics/ecScope.constants'
import { useEcExecSignalRGroup } from '@/composables/useEcDeptSignalrGroup'
import { taktShowEcRootMaterialDescription } from '@/constants/logistics/ecScope.constants'
import { useEcDeptViewI18n } from '../composables/useEcDeptViewI18n'
import { ECGIJUTSU_LIST_FIELDS, useEcGijutsuI18n } from '../ec-gijutsu/composables/useEcGijutsuI18n'
import { ECDETAIL_SELF_I18N_KEY } from '../ec-gijutsu/composables/useEcDetailI18n'
import type { EcDeptExecMasterRow } from '../composables/useEcDeptExecMasterContext'
import EcDeptExecDetailPanel from './EcDeptExecDetailPanel.vue'

/** 主表用字典 sys_yes_no 渲染的字段 */
const YES_NO_FIELDS = new Set(['isImplemented', 'isSopUpdated', 'ecNewRequiresInspection'])

/** 左侧设变主表默认可见列：工厂、设变号、发行日期、标题、担当 */
const GIJUTSU_DEFAULT_VISIBLE_COLUMN_KEYS = [
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
  /** 执行行主键字段（同明细上视图外键，如 ecBukanId） */
  idField: string
  /** 部门实体 slug（eckoubai / echinkan 等） */
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
const gijutsuI18n = useEcGijutsuI18n()
const formModalWidthPx = useTaktContentModalWidth()
/** 明细页签标题键 */
const detailSelfKey = ECDETAIL_SELF_I18N_KEY
/** 右栏页签 */
const rightTab = ref('exec')

/** 设变主表关键词 */
const gijutsuKeyword = ref('')
/** 设变主表 loading */
const gijutsuLoading = ref(false)
/** 设变主表行 */
const gijutsuRows = ref<EcGijutsu[]>([])
/** 设变主表页码 */
const gijutsuPage = ref(getTaktDefaultPageIndex())
/** 设变主表页大小 */
const gijutsuPageSize = ref(getTaktDefaultPageSize())
/** 设变主表总数 */
const gijutsuTotal = ref(0)
/** 选中设变主表主键 */
const selectedGijutsuKey = ref('')
/** 选中设变单号 */
const selectedEcCode = ref('')
/** 设变主表列设置 */
const gijutsuColumnSettingVisible = ref(false)
/** 设变主表可见列 */
const gijutsuVisibleColumnKeys = ref<string[]>([...GIJUTSU_DEFAULT_VISIBLE_COLUMN_KEYS])

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
const selectedRows = ref<EcDeptExecMasterRow[]>([])
/** 选中 keys */
const selectedRowKeys = ref<(string | number)[]>([])
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
/** 执行表外壳（按剩余高度留出横向滚动条） */
const execTableWrapRef = ref<HTMLElement | null>(null)
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
  if (field === 'ecRootMaterialCodeGroup') {
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
  const scope = Number(record.ecScope ?? 0)
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
  const scope = Number(record.ecScope ?? 0)
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
        if (selectedRowKeys.value.length === 1 && String(selectedRowKeys.value[0]) === id) {
          const refreshed = dataSource.value.find((row) => getMasterId(row) === id)
          if (refreshed) {
            selectedRows.value = [refreshed]
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

/** 左侧设变主表列（TaktEcGijutsu） */
const gijutsuColumns = computed<TableColumnsType>(() => [
  {
    title: gijutsuI18n.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 100,
    ellipsis: true,
  },
  ...ECGIJUTSU_LIST_FIELDS.map((field) => ({
    title: gijutsuI18n.label(field),
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
      || dataSource.value.some((row) => taktShowEcRootMaterialDescription(row.ecScope, row.ecDeptScope)))
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
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDeptExecMasterRow[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
  },
}))

/** 更新按钮禁用：须单选且当前行允许更新（以 dataSource 最新停产状态为准） */
const updateDisabled = computed(() => {
  if (selectedRowKeys.value.length !== 1) {
    return true
  }
  const key = String(selectedRowKeys.value[0] ?? '')
  const row =
    dataSource.value.find((r) => getMasterId(r) === key) ?? selectedRows.value[0]
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
    selectedRowKeys.value = []
    selectedRows.value = []
    return
  }
  const key = getMasterId(record)
  selectedRowKeys.value = key ? [key] : []
  selectedRows.value = key ? [record] : []
}

/**
 * 设变主表主键
 * @param record 设变主表行
 * @returns {string} 主键
 */
function getGijutsuId(record: EcGijutsu | Record<string, unknown>) {
  const row = record as EcGijutsu & { id?: string }
  return String(row.ecGijutsuId || row.id || '')
}

/**
 * 选中左侧设变后加载本部门执行行
 * @param record 设变主表行
 */
function handleGijutsuSelect(record: Record<string, unknown>) {
  const row = record as EcGijutsu
  selectedGijutsuKey.value = getGijutsuId(row)
  selectedEcCode.value = String(row.ecCode ?? '').trim()
  currentPage.value = getTaktDefaultPageIndex()
  selectedRowKeys.value = []
  selectedRows.value = []
  void loadData()
  nextTick(() => detailPanelRef.value?.reload?.())
}

/**
 * 加载左侧设变主表
 */
async function loadGijutsu() {
  gijutsuLoading.value = true
  try {
    const res = await getEcGijutsuList({
      pageIndex: gijutsuPage.value,
      pageSize: gijutsuPageSize.value,
      keyWords: gijutsuKeyword.value.trim() || undefined,
      execDeptCode: props.execCode,
    })
    gijutsuRows.value = res.data ?? []
    gijutsuTotal.value = res.total ?? 0
    const found = gijutsuRows.value.find((row) => getGijutsuId(row) === selectedGijutsuKey.value)
    if (!found) {
      selectedGijutsuKey.value = ''
      selectedEcCode.value = ''
      dataSource.value = []
      total.value = 0
    } else {
      selectedEcCode.value = String(found.ecCode ?? '').trim()
    }
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载设变主表失败', { error })
    message.error(t('common.feedback.load.data.failed'))
    gijutsuRows.value = []
    gijutsuTotal.value = 0
  } finally {
    gijutsuLoading.value = false
  }
}

/** 搜索设变主表 */
function handleGijutsuSearch() {
  gijutsuPage.value = getTaktDefaultPageIndex()
  void loadGijutsu()
}

/** 重置设变主表查询 */
function handleGijutsuReset() {
  gijutsuKeyword.value = ''
  gijutsuPage.value = getTaktDefaultPageIndex()
  void loadGijutsu()
}

/**
 * 设变主表列显隐
 * @param keys 可见列
 */
function handleGijutsuColumnKeysChange(keys: string[]) {
  gijutsuVisibleColumnKeys.value = keys
}

/** 设变主表列重置 */
function handleGijutsuColumnReset() {
  gijutsuVisibleColumnKeys.value = [...GIJUTSU_DEFAULT_VISIBLE_COLUMN_KEYS]
}

/** 加载本部门执行表 */
async function loadData() {
  if (!selectedEcCode.value) {
    dataSource.value = []
    total.value = 0
    return
  }
  loading.value = true
  try {
    const res = await props.getMasterList({
      ...(props.extraQuery ?? {}),
      ecCode: selectedEcCode.value,
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
    })
    dataSource.value = (res.data ?? []) as EcDeptExecMasterRow[]
    total.value = res.total ?? 0
    const currentKey = String(selectedRowKeys.value[0] ?? '')
    if (currentKey) {
      const found = dataSource.value.find((row) => getMasterId(row) === currentKey)
      syncMasterSelection(found ?? null)
      // 停产/在产切换后须用新行刷新选中，否则工具栏更新显隐仍按旧 discontinuedStatus
      if (found) {
        selectedRowKeys.value = [currentKey]
        selectedRows.value = [found]
      } else {
        selectedRowKeys.value = []
        selectedRows.value = []
      }
    }
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载主表失败', { error })
    message.error(t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
    syncMasterSelection(null)
    selectedRowKeys.value = []
    selectedRows.value = []
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
  selectedRowKeys.value = [key]
  selectedRows.value = [record]
  syncMasterSelection(record)
  loading.value = true
  try {
    const detail = await props.getMasterById(key)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    loading.value = false
  }
}

/** 工具栏编辑 */
function handleUpdate() {
  if (selectedRowKeys.value.length !== 1) {
    return
  }
  const key = String(selectedRowKeys.value[0] ?? '')
  const row =
    dataSource.value.find((r) => getMasterId(r) === key) ?? selectedRows.value[0]
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
      ecCode: selectedEcCode.value || undefined,
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

useTableRefresh(loadGijutsu)
useEcExecSignalRGroup(props.execCode)

watch(rightTab, () => {
  nextTick(() => syncTabTableScroll())
})

/** 左右布局高度监听 */
let layoutResizeObserver: ResizeObserver | null = null

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
  const fitted = measureConstrainedTableScrollY(execTableWrapRef.value, { reservePagination: true })
  if (fitted > 0) {
    execFittedScrollY.value = fitted
  }
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  await loadGijutsu()
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
    if (execTableWrapRef.value) {
      layoutResizeObserver.observe(execTableWrapRef.value)
    }
  })
})

onBeforeUnmount(() => {
  layoutResizeObserver?.disconnect()
  layoutResizeObserver = null
})
</script>
