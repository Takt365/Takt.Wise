<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：EcDeptExecLrPage.vue -->
<!-- 功能描述：执行部门左右主子表壳：左栏部门执行主表，右栏设变明细（视图主从） -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getMasterId"
      :master-row-selection="rowSelection"
      :master-id-column-key="idField"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
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
          :show-update="true"
          :show-delete="false"
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
      </template>
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
        <template v-else-if="isYesNoField(String(column.key))">
          <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
        </template>
      </template>
      <template #detail>
        <EcDeptExecDetailPanel
          ref="detailPanelRef"
          class="h-full min-h-0 flex-1"
          :id-field="idField"
          :get-master-by-id="getMasterById"
        />
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
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="idField"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 执行部门左右主子表页面壳（左栏部门执行主表，右栏设变明细）
 */
import type { Component } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import { message, Modal } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiEditLine, RiPlayCircleLine, RiStopCircleLine } from '@remixicon/vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import type { TaktPagedResult } from '@/types/common.types'
import type { TaktEcExecCode } from '@/constants/logistics/ecExecCodes.constants'
import { getEcDeptExecLineFields } from '@/constants/logistics/ecDeptExecLineFields.constants'
import {
  EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS,
  TaktEcScope,
} from '@/constants/logistics/ecScope.constants'
import { useEcExecSignalRGroup } from '@/composables/useEcDeptSignalrGroup'
import { useEcDeptViewI18n } from '../composables/useEcDeptViewI18n'
import {
  provideEcDeptExecMasterContext,
  type EcDeptExecMasterRow,
} from '../composables/useEcDeptExecMasterContext'
import EcDeptExecDetailPanel from './EcDeptExecDetailPanel.vue'

/** 主表用字典 sys_yes_no 渲染的字段 */
const YES_NO_FIELDS = new Set(['isImplemented', 'isSopUpdated', 'ecNewRequiresInspection'])

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
const { selectedMasterRow } = provideEcDeptExecMasterContext()
const formModalWidthPx = useTaktContentModalWidth()

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
/** 主表选中 key */
const selectedMasterKey = ref('')
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

/** 主表列（部门执行实体字段 + 停产/在产操作；数据列一律 ellipsis，禁止长文本撑高行） */
const columns = computed<TableColumnsType>(() => {
  const fieldCols = getEcDeptExecLineFields(props.deptSlug).map((field) => ({
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
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMasterId(record) : ''
}

/**
 * 主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as EcDeptExecMasterRow
  const key = getMasterId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  syncMasterSelection(row)
}

/** 主表分页 */
function handleMasterPaginationChange() {
  loadData()
}

/** 加载主表 */
async function loadData() {
  loading.value = true
  try {
    const res = await props.getMasterList({
      ...(props.extraQuery ?? {}),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
    })
    dataSource.value = (res.data ?? []) as EcDeptExecMasterRow[]
    total.value = res.total ?? 0
    const currentKey = selectedMasterKey.value || String(selectedRowKeys.value[0] ?? '')
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

/** 表格变化 */
function handleTableChange() {}

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

useTableRefresh(loadData)
useEcExecSignalRGroup(props.execCode)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})
</script>
