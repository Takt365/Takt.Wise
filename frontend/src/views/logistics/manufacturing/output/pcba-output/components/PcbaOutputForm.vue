<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：PcbaOutputForm.vue -->
<!-- 功能描述：PCBA日报维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-output-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="pcba-output-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-output-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodCategory')"
                name="prodCategory"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_manufacturing_prod_category"
                  :placeholder="pi.ph('prodCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodDate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="pi.ph('prodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderType')"
                name="prodOrderType"
              >
                <a-input
                  v-model:value="formState.prodOrderType"
                  :placeholder="pi.ph('prodOrderType')"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.prodOrderCode"
                  api-url="TaktProductionOrders/options"
                  :api-params="prodOrderOptionsParams"
                  :placeholder="pi.ph('prodOrderCode')"
                  :disabled="!!formData?.pcbaOutputId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelCode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="pi.ph('modelCode')"
                  show-count
                  :maxlength="40"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchCode')"
                name="batchCode"
              >
                <a-input
                  v-model:value="formState.batchCode"
                  :placeholder="pi.ph('batchCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderQty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="pi.ph('prodOrderQty')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('serialCode')"
                name="serialCode"
              >
                <a-input
                  v-model:value="formState.serialCode"
                  :placeholder="pi.ph('serialCode')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    </div>
    <!-- 下：子表（弹窗视口约 1/2；表体内横+纵滚动） -->
    <div
      ref="detailHostRef"
      class="pcba-output-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="pcbaOutputDetailTableRef"
      v-model="childPcbaOutputDetailRows"
      :columns="pcbaOutputDetailFormColumns"
      :title="pcbaOutputDetailPi.self()"
      :add-button-entity="pcbaOutputDetailPi.self()"
      id-field="pcbaOutputDetailId"
      :default-row="createDefaultPcbaOutputDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-teamCode="{ record }">
        <TaktSelect
          v-model:value="record.teamCode"
          api-url="TaktProductionTeams/options"
          :api-params="pcbaTeamOptionsParams"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.queryPh('teamCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-prodEquipCode="{ record }">
        <TaktSelect
          v-model:value="record.prodEquipCode"
          api-url="TaktProductionEquipments/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.queryPh('prodEquipCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-shiftNo="{ record }">
        <TaktSelect
          v-model:value="record.shiftNo"
          dict-type="logistics_manufacturing_shift_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('shiftNo')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-pcbBoardType="{ record }">
        <TaktSelect
          v-model:value="record.pcbBoardType"
          :dict-type="PCBA_DETAIL_PCB_BOARD_TYPE_DICT"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('pcbBoardType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-panelSide="{ record }">
        <TaktSelect
          v-model:value="record.panelSide"
          dict-type="logistics_manufacturing_pcba_side_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('panelSide')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-completedStatus="{ record }">
        <TaktSelect
          v-model:value="record.completedStatus"
          dict-type="logistics_manufacturing_pcba_completed_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('completedStatus')"
          disabled
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaOutputDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA日报实体 达成率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaOutputI18n } from '../composables/usePcbaOutputI18n'
import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
} from '@/utils/tableScroll'

/** 实体字段 i18n */
const pi = usePcbaOutputI18n()

import type { PcbaOutputCreate } from '@/types/logistics/manufacturing/output/pcbaOutput.types'
import {
  buildPcbaProductionTeamOptionsParams,
} from '../../composables/productionTeamCategory'
import { PCBA_DETAIL_PCB_BOARD_TYPE_DICT } from '../composables/pcbaOutputDetailDictFormat'
import { usePcbaOutputDetailDictFormat } from '../composables/usePcbaOutputDetailDictFormat'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import {
  applyProductionOrderFormFillToMaster,
  fetchProductionOrderFormFill,
  mapFormFillDefaultDetailsToPcbaRows,
} from '../../composables/useProductionOrderFormFill'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","prodCategory","prodDate","prodOrderType","prodOrderCode","modelCode","materialCode","batchCode","prodOrderQty","serialCode","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { usePcbaOutputDetailI18n } from '../composables/usePcbaOutputDetailI18n'

const pcbaOutputDetailPi = usePcbaOutputDetailI18n()
/** 子表字典：PCB板别等 Label/Value 转换 */
const { hydrateDetailDictFields, formatDetailDictFieldsForSubmit } = usePcbaOutputDetailDictFormat()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPcbaOutputDetailRows = ref<Record<string, unknown>[]>([])
const pcbaOutputDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表半区宿主（弹窗视口约 1/2） */
const detailHostRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（半区内扣除标题/表头/汇总） */
const detailScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailHostResizeObserver: ResizeObserver | null = null

/**
 * 按子表半区实测 scroll.y
 */
function recalcDetailScrollYPx(): void {
  const host = detailHostRef.value
  if (host == null || host.clientHeight <= 0) {
    return
  }
  const tableRoot = host.querySelector('.takt-editable-table') as HTMLElement | null
  const toolbar = tableRoot?.querySelector(':scope > .mb-2') as HTMLElement | null
  const toolbarH = toolbar?.offsetHeight ?? 0
  const sectionPad = 12
  const next = Math.floor(
    host.clientHeight
      - toolbarH
      - sectionPad
      - TAKT_TABLE_HEADER_FALLBACK_PX
      - TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
  )
  detailScrollYPx.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, next)
}

/** 监听弹窗/半区尺寸变化 */
function bindDetailHostResizeObserver(): void {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  const host = detailHostRef.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const target =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
    ?? host
  detailHostResizeObserver = new ResizeObserver(() => {
    recalcDetailScrollYPx()
  })
  detailHostResizeObserver.observe(target)
  detailHostResizeObserver.observe(host)
}

/** 是否已持久化的子表行 */
function isPersistedPcbaOutputDetailRow(row: Record<string, unknown>): boolean {
  const id = row.pcbaOutputDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPcbaOutputDetailLineNumber(): number {
  const rows = pcbaOutputDetailTableRef.value?.getRows?.() ?? childPcbaOutputDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表数值列合计：仅当日完成数、不良台数、报工工时 */
const PCBA_DETAIL_FORM_SUMMARY_SUM_KEYS = new Set([
  'dailyCompletedQty',
  'defectCount',
  'confirmMinutes',
])

/**
 * 构建带合计的数值列
 * @param key 字段名
 * @param width 列宽
 * @returns 列配置
 */
function buildPcbaDetailNumberColumn(key: string, width = 140): TaktEditableTableColumn {
  const column: TaktEditableTableColumn = {
    key,
    title: pcbaOutputDetailPi.label(key as 'dailyCompletedQty'),
    editor: 'inputNumber',
    width,
  }
  if (PCBA_DETAIL_FORM_SUMMARY_SUM_KEYS.has(key)) {
    column.summary = 'sum'
  }
  return column
}

/** 子表 pcbaOutputDetail 可编辑列（与 TaktPcbaOutputDetail 实体业务字段一一对应） */
const pcbaOutputDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'prodOrderCode',
    title: pcbaOutputDetailPi.label('prodOrderCode'),
    editor: 'readonly',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: pcbaOutputDetailPi.label('lineNumber'),
    editor: 'readonly',
    width: 100,
  },
  {
    key: 'timePeriod',
    title: pcbaOutputDetailPi.label('timePeriod'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'teamCode',
    title: pcbaOutputDetailPi.label('teamCode'),
    width: 140,
  },
  {
    key: 'prodEquipCode',
    title: pcbaOutputDetailPi.label('prodEquipCode'),
    width: 140,
  },
  buildPcbaDetailNumberColumn('directLabor'),
  buildPcbaDetailNumberColumn('indirectLabor'),
  {
    key: 'shiftNo',
    title: pcbaOutputDetailPi.label('shiftNo'),
    width: 120,
  },
  {
    key: 'stdMinutes',
    title: pcbaOutputDetailPi.label('stdMinutes'),
    editor: 'readonly',
    width: 120,
  },
  {
    key: 'stdLaborCapacity',
    title: pcbaOutputDetailPi.label('stdLaborCapacity'),
    editor: 'readonly',
    width: 140,
  },
  buildPcbaDetailNumberColumn('stdShorts'),
  {
    key: 'stdEquipmentCapacity',
    title: pcbaOutputDetailPi.label('stdEquipmentCapacity'),
    editor: 'readonly',
    width: 140,
  },
  {
    key: 'pcbBoardType',
    title: pcbaOutputDetailPi.label('pcbBoardType'),
    width: 160,
  },
  {
    key: 'panelSide',
    title: pcbaOutputDetailPi.label('panelSide'),
    width: 120,
  },
  buildPcbaDetailNumberColumn('batchQty'),
  buildPcbaDetailNumberColumn('dailyCompletedQty'),
  {
    key: 'totalCompletedQty',
    title: pcbaOutputDetailPi.label('totalCompletedQty'),
    editor: 'readonly',
    width: 120,
  },
  {
    key: 'completedStatus',
    title: pcbaOutputDetailPi.label('completedStatus'),
    width: 120,
  },
  {
    key: 'serialCode',
    title: pcbaOutputDetailPi.label('serialCode'),
    editor: 'input',
    width: 140,
  },
  buildPcbaDetailNumberColumn('defectCount'),
  buildPcbaDetailNumberColumn('downtimeMinutes'),
  {
    key: 'downtimeReason',
    title: pcbaOutputDetailPi.label('downtimeReason'),
    editor: 'input',
    width: 140,
    allowClear: true,
    placeholder: pcbaOutputDetailPi.ph('downtimeReason'),
  },
  {
    key: 'downtimeDescription',
    title: pcbaOutputDetailPi.label('downtimeDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: pcbaOutputDetailPi.ph('downtimeDescription'),
    width: 180,
  },
  {
    key: 'inputMinutes',
    title: pcbaOutputDetailPi.label('inputMinutes'),
    editor: 'readonly',
    width: 120,
  },
  {
    key: 'actualMinutes',
    title: pcbaOutputDetailPi.label('actualMinutes'),
    editor: 'readonly',
    width: 120,
  },
  buildPcbaDetailNumberColumn('repairMinutes'),
  buildPcbaDetailNumberColumn('switchCount'),
  buildPcbaDetailNumberColumn('switchTime'),
  buildPcbaDetailNumberColumn('stopTime'),
  buildPcbaDetailNumberColumn('totalMinutes'),
  {
    key: 'unachievedReason',
    title: pcbaOutputDetailPi.label('unachievedReason'),
    editor: 'input',
    width: 140,
    allowClear: true,
    placeholder: pcbaOutputDetailPi.ph('unachievedReason'),
  },
  {
    key: 'unachievedDescription',
    title: pcbaOutputDetailPi.label('unachievedDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: pcbaOutputDetailPi.ph('unachievedDescription'),
    width: 180,
  },
  buildPcbaDetailNumberColumn('confirmMinutes'),
  buildPcbaDetailNumberColumn('mixedProd'),
  {
    key: 'achievementRate',
    title: pcbaOutputDetailPi.label('achievementRate'),
    editor: 'readonly',
    width: 120,
  },
  {
    key: 'isObsolete',
    title: pcbaOutputDetailPi.label('isObsolete'),
    width: 120,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null | undefined) {
  const rows_pcbaOutputDetail = ((val as any)?.pcbaOutputDetails ?? []) as Record<string, unknown>[]
  childPcbaOutputDetailRows.value = rows_pcbaOutputDetail.map((row) => {
    const next = { ...row }
    hydrateDetailDictFields(next)
    return next
  })
}

/** 新建空行默认值（字段与实体对齐） */
function createDefaultPcbaOutputDetailRow(): Record<string, unknown> {
  return {
    prodOrderCode: String(formState.prodOrderCode ?? '').trim(),
    lineNumber: allocateNextPcbaOutputDetailLineNumber(),
    timePeriod: '',
    teamCode: '',
    prodEquipCode: '',
    directLabor: 0,
    indirectLabor: 0,
    shiftNo: 1,
    stdMinutes: 0,
    stdLaborCapacity: 0,
    stdShorts: 0,
    stdEquipmentCapacity: 0,
    pcbBoardType: '',
    panelSide: '',
    batchQty: 0,
    dailyCompletedQty: 0,
    totalCompletedQty: 0,
    completedStatus: 0,
    serialCode: '',
    defectCount: 0,
    downtimeMinutes: 0,
    downtimeReason: '',
    downtimeDescription: '',
    inputMinutes: 0,
    actualMinutes: 0,
    repairMinutes: 0,
    switchCount: 0,
    switchTime: 0,
    stopTime: 0,
    totalMinutes: 0,
    unachievedReason: '',
    unachievedDescription: '',
    confirmMinutes: 0,
    mixedProd: 0,
    achievementRate: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaOutputId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    pcbaOutputDetails: (pcbaOutputDetailTableRef.value?.getRows?.() ?? childPcbaOutputDetailRows.value).map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        pcbaOutputId: masterId,
        prodOrderCode: String(row.prodOrderCode ?? '').trim() || String(formState.prodOrderCode ?? '').trim(),
      }
      formatDetailDictFieldsForSubmit(normalized)
      if (isUpdate && isPersistedPcbaOutputDetailRow(row)) {
        normalized.pcbaOutputDetailId = row.pcbaOutputDetailId
      } else {
        delete normalized.pcbaOutputDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 班组下拉：按工厂 + PCBA 分类 */
const pcbaTeamOptionsParams = computed(() =>
  buildPcbaProductionTeamOptionsParams(
    String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || '',
  ),
)

/** 工单下拉：按工厂过滤 */
const prodOrderOptionsParams = computed(() => {
  const plant =
    String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || ''
  return plant ? { plantCode: plant } : {}
})

/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  prodCategory: "FPP"
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载：预热字典 + 半区高度测量 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  void nextTick(() => {
    recalcDetailScrollYPx()
    bindDetailHostResizeObserver()
  })
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', recalcDetailScrollYPx)
  }
})

onBeforeUnmount(() => {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', recalcDetailScrollYPx)
  }
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaOutputId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaOutputId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaOutputDetails
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
    void nextTick(() => {
      recalcDetailScrollYPx()
      bindDetailHostResizeObserver()
    })
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.pcbaOutputId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 选工单 / 改生产日期时回填主表只读字段，并按标准工序生成子表预览行（仅新增态） */
let pcbaProdOrderFillSeq = 0
watch(
  () => [formState.prodOrderCode, formState.prodDate] as const,
  async ([prodOrderCode, prodDate]) => {
    if (props.formData?.pcbaOutputId) {
      return
    }
    const code = String(prodOrderCode ?? '').trim()
    if (!code) {
      formState.prodOrderType = ''
      formState.modelCode = ''
      formState.materialCode = ''
      formState.batchCode = ''
      formState.prodOrderQty = undefined
      formState.serialCode = ''
      childPcbaOutputDetailRows.value = []
      return
    }
    const seq = ++pcbaProdOrderFillSeq
    const fill = await fetchProductionOrderFormFill(code, {
      plantCode: String(formState.plantCode ?? '').trim() || undefined,
      prodDate: String(prodDate ?? '').trim().slice(0, 10) || undefined,
      includeDefaultDetails: true,
    })
    if (seq !== pcbaProdOrderFillSeq) {
      return
    }
    if (!fill) {
      return
    }
    applyProductionOrderFormFillToMaster(formState, fill, false)
    childPcbaOutputDetailRows.value = mapFormFillDefaultDetailsToPcbaRows(
      fill.defaultDetails,
      code,
    ).map((row) => {
      const next = { ...row }
      hydrateDetailDictFields(next)
      return next
    })
    void nextTick(() => {
      recalcDetailScrollYPx()
      bindDetailHostResizeObserver()
    })
  }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  prodCategory: [
    {
      required: true,
      message: pi.ph('prodCategory'),
      trigger: 'change'
    }
  ],
  prodDate: [
    {
      required: true,
      message: pi.ph('prodDate'),
      trigger: 'change'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await pcbaOutputDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.pcbaOutputId) {
    payload.pcbaOutputId = props.formData.pcbaOutputId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaOutputId)
  childPcbaOutputDetailRows.value = []
  pcbaOutputDetailTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.pcba-output-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

.pcba-output-form__master :deep(.pcba-output-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.pcba-output-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.pcba-output-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.pcba-output-form__master :deep(.ant-tabs-content),
.pcba-output-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
