<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output/components -->
<!-- 文件名称：AssyOutputForm.vue -->
<!-- 功能描述：组立日报维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assy-output-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="assy-output-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="assy-output-form-tabs"
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
                :label="pi.label('teamCode')"
                name="teamCode"
              >
                <TaktSelect
                  v-model:value="formState.teamCode"
                  api-url="TaktProductionTeams/options"
                  :api-params="assyTeamOptionsParams"
                  :placeholder="pi.ph('teamCode')"
                  :disabled="!!formData?.assyOutputId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('directLabor')"
                name="directLabor"
              >
                <a-input-number
                  v-model:value="formState.directLabor"
                  :placeholder="pi.ph('directLabor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('indirectLabor')"
                name="indirectLabor"
              >
                <a-input-number
                  v-model:value="formState.indirectLabor"
                  :placeholder="pi.ph('indirectLabor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_manufacturing_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdCapacity')"
                name="stdCapacity"
              >
                <a-input-number
                  v-model:value="formState.stdCapacity"
                  :placeholder="pi.ph('stdCapacity')"
                  style="width: 100%"
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
                  :disabled="!!formData?.assyOutputId"
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdMinutes')"
                name="stdMinutes"
              >
                <a-input-number
                  v-model:value="formState.stdMinutes"
                  :placeholder="pi.ph('stdMinutes')"
                  style="width: 100%"
                  disabled
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
            <a-col :span="12">
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
            <a-col :span="12">
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
      class="assy-output-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="assyOutputDetailTableRef"
      v-model="childAssyOutputDetailRows"
      :columns="assyOutputDetailFormColumns"
      :title="assyOutputDetailPi.self()"
      :add-button-entity="assyOutputDetailPi.self()"
      id-field="assyOutputDetailId"
      :default-row="createDefaultAssyOutputDetailRow"
      :show-add="false"
      :show-delete="false"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-downtimeReason="{ record }">
        <TaktSelect
          :model-value="getDetailDictMultiSelectModelValue(record, 'downtimeReason')"
          mode="multiple"
          :dict-type="ASSY_DETAIL_DOWNTIME_REASON_DICT"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyOutputDetailPi.ph('downtimeReason')"
          :disabled="loading"
          allow-clear
          @update:model-value="(v) => onDetailDictMultiSelectChange(record, 'downtimeReason', v)"
        />
      </template>
      <template #cell-unachievedReason="{ record }">
        <TaktSelect
          :model-value="getDetailDictMultiSelectModelValue(record, 'unachievedReason')"
          mode="multiple"
          :dict-type="ASSY_DETAIL_UNACHIEVED_REASON_DICT"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyOutputDetailPi.ph('unachievedReason')"
          :disabled="loading"
          allow-clear
          @update:model-value="(v) => onDetailDictMultiSelectChange(record, 'unachievedReason', v)"
        />
      </template>
    </TaktEditableTable>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 组立日报维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/assy-output/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssyOutputI18n } from '../composables/useAssyOutputI18n'
import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
} from '@/utils/tableScroll'

/** 实体字段 i18n */
const pi = useAssyOutputI18n()

import type { AssyOutputCreate } from '@/types/logistics/manufacturing/output/assyOutput.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import {
  applyProductionOrderFormFillToMaster,
  fetchProductionOrderFormFill,
} from '../../composables/useProductionOrderFormFill'
import { resolveDefaultOutputProdDateYmd } from '../../composables/taktOutputProdDateEditLock'
import {
  buildAssyProductionTeamOptionsParams,
} from '../../composables/productionTeamCategory'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 解析当前用户/公司默认区域文化（与 /me 字段对齐）
 * @returns {string} CultureCode
 */
function resolveScopeCultureCode(): string {
  const info = userStore.userInfo
  return String(
    info?.companyDefaultCulture
      ?? info?.defaultCulture
      ?? info?.cultureCode
      ?? '',
  ).trim()
}

/**
 * 解析当前公司关联工厂（选项 ExtValue → /me.relatedPlant）
 * @returns {string} PlantCode
 */
function resolveScopePlantCode(): string {
  return String(
    tenantStore.currentCompanyRelatedPlant
      || userStore.userInfo?.relatedPlant
      || '',
  ).trim()
}

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
    target.cultureCode = resolveScopeCultureCode()
  }
  if (force || !target.plantCode) {
    target.plantCode = resolveScopePlantCode()
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","prodCategory","prodDate","teamCode","directLabor","indirectLabor","shiftNo","stdCapacity","prodOrderCode","modelCode","materialCode","batchCode","prodOrderQty","serialCode","stdMinutes","prodOrderType","extField","remark"]

import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useAssyOutputDetailI18n } from '../composables/useAssyOutputDetailI18n'
import { useAssyOutputDetailFormColumns } from '../composables/useAssyOutputDetailFormColumns'
import {
  ASSY_OUTPUT_DEFAULT_TIME_PERIODS,
  buildDefaultAssyOutputDetailRows,
} from '../composables/assyOutputDefaultTimePeriods'
import { calculateAssyStdCapacity } from '@/utils/taktProductionStat'
import {
  ASSY_DETAIL_DOWNTIME_REASON_DICT,
  ASSY_DETAIL_UNACHIEVED_REASON_DICT,
} from '../composables/assyOutputDetailDictMulti'
import { useAssyOutputDetailEditableDict } from '../composables/useAssyOutputDetailEditableDict'
import {
  calculateAssyOutputDetailDerived,
  resolvePersonnelOperationRatePercent,
  type AssyOutputMasterCalcSnapshot,
} from '../composables/useAssyOutputDerivedCalc'
import { applyAssyCleaningPeriodDefaults } from '@/utils/taktProductionStat'

const assyOutputDetailPi = useAssyOutputDetailI18n()
/** 内嵌子表列（含只读生产时段） */
const assyOutputDetailFormColumns = useAssyOutputDetailFormColumns()
const {
  getDetailDictMultiSelectModelValue,
  applyDetailDictMultiChange,
  normalizeAssyOutputDetailRowForSubmit,
} = useAssyOutputDetailEditableDict()

/** 人员标准生产稼动率（%），供明细标准产能实时计算 */
const operationRatePercent = ref(0)
/** 防止派生字段回写触发深度 watch 死循环 */
let isRefreshingDetailDerived = false

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

/**
 * 子表多选字典变更写回
 * @param record 行
 * @param field 字段
 * @param values Select 值
 */
function onDetailDictMultiSelectChange(
  record: Record<string, unknown>,
  field: 'downtimeReason' | 'unachievedReason',
  values: string | number | readonly (string | number)[] | null | undefined,
) {
  applyDetailDictMultiChange(record, field, values)
}

const childAssyOutputDetailRows = ref<Record<string, unknown>[]>([])
/** 子表可编辑表格实例 */
const assyOutputDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
  forEachRow: (callback: (row: Record<string, unknown>, index: number) => void) => void
  syncModelValue: () => void
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
function isPersistedAssyOutputDetailRow(row: Record<string, unknown>): boolean {
  const id = row.assyOutputDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextAssyOutputDetailLineNumber(): number {
  const rows = assyOutputDetailTableRef.value?.getRows?.() ?? childAssyOutputDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/**
 * 主表派生计算快照
 * @returns 主表快照
 */
function getMasterCalcSnapshot(): AssyOutputMasterCalcSnapshot {
  return {
    directLabor: Number(formState.directLabor) || 0,
    indirectLabor: Number(formState.indirectLabor) || 0,
    stdCapacity: Number(formState.stdCapacity) || 0,
    stdMinutes: Number(formState.stdMinutes) || 0,
    operationRatePercent: operationRatePercent.value,
  }
}

/**
 * 将派生字段写回单行明细（清洁时段停线随直接人数；标准产能默认快照主表；有报工时按公式重算）
 * @param row 子表行（须为表格 innerRows，才能刷新只读列）
 */
function applyDerivedToDetailRow(row: Record<string, unknown>) {
  const master = getMasterCalcSnapshot()
  applyAssyCleaningPeriodDefaults(row, master.directLabor)
  const derived = calculateAssyOutputDetailDerived(master, {
    prodActualQty: Number(row.prodActualQty) || 0,
    downtimeMinutes: Number(row.downtimeMinutes) || 0,
    confirmMinutes: Number(row.confirmMinutes) || 0,
    mixedProd: Number(row.mixedProd) || 0,
  })
  row.stdCapacity = derived.stdCapacity
  row.inputMinutes = derived.inputMinutes
  row.actualMinutes = derived.actualMinutes
  row.indirectMinutes = derived.indirectMinutes
  row.achievementRate = derived.achievementRate
  if (String(formState.prodOrderCode ?? '').trim()) {
    row.prodOrderCode = String(formState.prodOrderCode).trim()
  }
}

/**
 * 刷新已有子表行的派生字段（只改 child 数组引用，不走 syncModelValue，避免冲空）
 */
function refreshAllDetailDerivedFields() {
  if (isRefreshingDetailDerived) {
    return
  }
  if (childAssyOutputDetailRows.value.length === 0) {
    return
  }
  isRefreshingDetailDerived = true
  try {
    childAssyOutputDetailRows.value = childAssyOutputDetailRows.value.map((row) => {
      const next = { ...row }
      applyDerivedToDetailRow(next)
      return next
    })
  } finally {
    isRefreshingDetailDerived = false
  }
}

/**
 * 按工厂+生产日期刷新稼动率，并重算子表派生字段
 * @returns 异步任务
 */
async function refreshOperationRateAndDetailStdCapacity() {
  const plantCode = String(formState.plantCode ?? '').trim()
  const prodDate = String(formState.prodDate ?? '').trim().slice(0, 10)
  if (!plantCode || !prodDate) {
    operationRatePercent.value = 0
    refreshAllDetailDerivedFields()
    return
  }
  operationRatePercent.value = await resolvePersonnelOperationRatePercent(plantCode, prodDate)
  refreshAllDetailDerivedFields()
}

/**
 * 按直接人数 × 标准工时 × 稼动率重算主表小时标准产能
 * @description 仅在算出 >0 时写入；绝不在此处清空（清空只走工单回填 / 用户清工单）
 */
function recomputeMasterStdCapacity() {
  if (props.formData?.assyOutputId) {
    return
  }
  const labor = Number(formState.directLabor)
  const minutes = Number(formState.stdMinutes)
  if (!(labor > 0) || !(minutes > 0)) {
    return
  }
  const next = calculateAssyStdCapacity(labor, minutes, operationRatePercent.value)
  if (next > 0) {
    formState.stdCapacity = next
  }
}

/**
 * 当前子表是否已是完整的 13 条固定时段
 * @param rows 子表行
 * @returns 是否齐套
 */
function hasFullDefaultAssyOutputDetailPeriods(rows: readonly Record<string, unknown>[]): boolean {
  if (rows.length !== ASSY_OUTPUT_DEFAULT_TIME_PERIODS.length) {
    return false
  }
  return ASSY_OUTPUT_DEFAULT_TIME_PERIODS.every(
    (period, index) => String(rows[index]?.timePeriod ?? '').trim() === period,
  )
}

/**
 * 按主表标准产能同步子表（唯一规则）
 * - stdCapacity ≤ 0 / 空 → 子表清空
 * - stdCapacity > 0 → 无 13 时段则新增；已有则更新派生字段
 */
function syncAssyDetailsByStdCapacity() {
  if (props.formData?.assyOutputId) {
    return
  }
  const capacity = Number(formState.stdCapacity)
  if (!Number.isFinite(capacity) || capacity <= 0) {
    if (childAssyOutputDetailRows.value.length > 0) {
      childAssyOutputDetailRows.value = []
    }
    return
  }
  const code = String(formState.prodOrderCode ?? '').trim()
  if (!hasFullDefaultAssyOutputDetailPeriods(childAssyOutputDetailRows.value)) {
    const rows = buildDefaultAssyOutputDetailRows(code)
    for (const row of rows) {
      applyDerivedToDetailRow(row)
    }
    childAssyOutputDetailRows.value = rows
    return
  }
  childAssyOutputDetailRows.value = childAssyOutputDetailRows.value.map((row) => {
    const next = { ...row }
    if (code) {
      next.prodOrderCode = code
    }
    applyDerivedToDetailRow(next)
    return next
  })
}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<AssyOutputCreate & { assyOutputId?: string }> | null | undefined) {
  const rows_assyOutputDetail = ((val as any)?.assyOutputDetails ?? []) as Record<string, unknown>[]
  childAssyOutputDetailRows.value = rows_assyOutputDetail
  void nextTick(() => {
    void refreshOperationRateAndDetailStdCapacity()
  })
}

function createDefaultAssyOutputDetailRow(): Record<string, unknown> {
  const row: Record<string, unknown> = {
    lineNumber: allocateNextAssyOutputDetailLineNumber(),
    timePeriod: '',
    prodActualQty: 0,
    downtimeMinutes: 0,
    downtimeReason: '',
    downtimeDescription: '',
    unachievedReason: '',
    unachievedDescription: '',
    confirmMinutes: 0,
    mixedProd: 0,
    isObsolete: 0,
  }
  applyDerivedToDetailRow(row)
  return row
}

/**
 * 清理子表提交行中的前端临时字段（避免 ModelState 绑定失败）
 * @param row 规范化后的明细行
 * @returns 可提交的明细行
 */
function sanitizeAssyOutputDetailSubmitRow(row: Record<string, unknown>): Record<string, unknown> {
  const cleaned = { ...row }
  const dropKeys = [
    'key',
    '_rowKey',
    'rowKey',
    'assyOutputName',
    'assyOutputDetailId',
    'id',
    'createdAt',
    'updatedAt',
    'createdBy',
    'updatedBy',
    'isDeleted',
  ]
  for (const key of dropKeys) {
    delete cleaned[key]
  }
  return cleaned
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  refreshAllDetailDerivedFields()
  applyScopeDefaults(formState as Record<string, unknown>, true)
  const masterId = String(props.formData?.assyOutputId ?? '').trim()
  const isUpdate = Boolean(masterId)
  const prodOrderCode = String(formState.prodOrderCode ?? '').trim()
  const scopeTenant = String(tenantStore.tenantCode ?? '').trim()
  const scopeCompany = String(tenantStore.companyCode ?? '').trim()
  const scopeCulture = String(formState.cultureCode ?? '').trim() || resolveScopeCultureCode()
  const scopePlant = String(formState.plantCode ?? '').trim() || resolveScopePlantCode()
  return {
    ...formState,
    tenantCode: scopeTenant,
    companyCode: scopeCompany,
    cultureCode: scopeCulture,
    plantCode: scopePlant,
    assyOutputDetails: (assyOutputDetailTableRef.value?.getRows?.() ?? childAssyOutputDetailRows.value).map((row) => {
      const normalized = sanitizeAssyOutputDetailSubmitRow({
        ...normalizeAssyOutputDetailRowForSubmit(row),
        tenantCode: scopeTenant,
        companyCode: scopeCompany,
        cultureCode: scopeCulture,
        plantCode: scopePlant,
        // 新增态必须传数字 0；空字符串会导致 long 绑定 ModelState 400（非 TaktApiResult）
        assyOutputId: isUpdate ? masterId : 0,
        prodOrderCode: String(row.prodOrderCode ?? '').trim() || prodOrderCode,
        lineNumber: Number(row.lineNumber) || 0,
        timePeriod: String(row.timePeriod ?? '').trim(),
      })
      if (isUpdate && isPersistedAssyOutputDetailRow(row)) {
        normalized.assyOutputDetailId = row.assyOutputDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyOutputCreate & { assyOutputId?: string }> | null
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

/** 班组下拉：按工厂 + 组立分类 */
const assyTeamOptionsParams = computed(() =>
  buildAssyProductionTeamOptionsParams(
    String(formState.plantCode ?? '').trim() || resolveScopePlantCode(),
  ),
)

/** 工单下拉：按工厂过滤 */
const prodOrderOptionsParams = computed(() => {
  const plant = String(formState.plantCode ?? '').trim() || resolveScopePlantCode()
  return plant ? { plantCode: plant } : {}
})
/** 班次字典值：白班（logistics_manufacturing_shift_category） */
const ASSY_OUTPUT_DEFAULT_SHIFT_DAY = 4

/**
 * 新增默认生产日期（昨天；不可选则回退今天）
 * @returns 日期文本
 */
function getDefaultAssyOutputProdDateYmd(): string {
  return resolveDefaultOutputProdDateYmd()
}

/** 表单字段默认值（新增 / resetFields） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  prodCategory: 'FPP',
  shiftNo: ASSY_OUTPUT_DEFAULT_SHIFT_DAY,
}

/** 写入表单默认值（仅补空：新增 / resetFields；编辑已有值不覆盖） */
function applyFormDefaults(target: Record<string, unknown>) {
  for (const [key, value] of Object.entries(FORM_FIELD_DEFAULTS)) {
    const current = target[key]
    if (current === undefined || current === null || current === '') {
      target[key] = value
    }
  }
  if (!target.prodDate) {
    target.prodDate = getDefaultAssyOutputProdDateYmd()
  }
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

/** 编辑态灌入 formData；新增态恢复默认值，子表待标准产能 > 0 后再生成 13 时段 */
watch(
  () => props.formData,
  (val) => {
    if (val?.assyOutputId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).assyOutputDetails
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
      syncAssyDetailsByStdCapacity()
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
  () =>
    [
      tenantStore.tenantCode,
      tenantStore.companyCode,
      tenantStore.currentCompanyRelatedPlant,
      userStore.userInfo?.companyDefaultCulture,
      userStore.userInfo?.defaultCulture,
      userStore.userInfo?.relatedPlant,
    ] as const,
  () => {
    if (!props.formData?.assyOutputId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 选工单 / 改生产日期或直接人数时回填主表只读字段与标准工时（仅新增态） */
let assyProdOrderFillSeq = 0
watch(
  () => [formState.prodOrderCode, formState.prodDate, formState.directLabor] as const,
  async ([prodOrderCode, prodDate, directLabor]) => {
    if (props.formData?.assyOutputId) {
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
      formState.stdMinutes = undefined
      formState.stdCapacity = undefined
      operationRatePercent.value = 0
      syncAssyDetailsByStdCapacity()
      return
    }
    const seq = ++assyProdOrderFillSeq
    const fill = await fetchProductionOrderFormFill(code, {
      plantCode: String(formState.plantCode ?? '').trim() || undefined,
      prodDate: String(prodDate ?? '').trim().slice(0, 10) || undefined,
      directLabor:
        directLabor === undefined || directLabor === null || directLabor === ''
          ? undefined
          : Number(directLabor),
      includeDefaultDetails: false,
    })
    if (seq !== assyProdOrderFillSeq) {
      return
    }
    if (!fill) {
      return
    }
    applyProductionOrderFormFillToMaster(formState, fill, true)
    operationRatePercent.value = Number(fill.operationRatePercent) || 0
    recomputeMasterStdCapacity()
    syncAssyDetailsByStdCapacity()
  }
)

/** 主表标准产能：≤0 清空；>0 新增或更新 13 时段 */
watch(
  () => Number(formState.stdCapacity) || 0,
  () => {
    syncAssyDetailsByStdCapacity()
  },
)

/** 主表人数/工时变化：能算则写产能；再按产能同步子表 */
watch(
  () => [formState.stdMinutes, formState.directLabor, formState.indirectLabor] as const,
  () => {
    if (props.formData?.assyOutputId) {
      refreshAllDetailDerivedFields()
      return
    }
    recomputeMasterStdCapacity()
    syncAssyDetailsByStdCapacity()
  },
)

/** 子表产量/报工/停线变化时，实时重算派生字段 */
watch(
  () =>
    childAssyOutputDetailRows.value
      .map((row) => `${row.prodActualQty ?? ''}|${row.confirmMinutes ?? ''}|${row.downtimeMinutes ?? ''}`)
      .join(';'),
  () => {
    refreshAllDetailDerivedFields()
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  cultureCode: [
    {
      required: true,
      message: pi.ph('cultureCode'),
      trigger: 'change'
    }
  ],
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
  teamCode: [
    {
      required: true,
      message: pi.ph('teamCode'),
      trigger: 'change'
    }
  ],
  directLabor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('directLabor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('directLabor'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectLabor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('indirectLabor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('indirectLabor'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
      trigger: 'change'
    }
  ],
  modelCode: [
    {
      required: true,
      message: pi.ph('modelCode'),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await assyOutputDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('directLabor' in payload) {
    const rawdirectLabor = payload.directLabor
    if (rawdirectLabor === undefined || rawdirectLabor === null || rawdirectLabor === '') {
      delete payload.directLabor
    } else {
      const numdirectLabor = typeof rawdirectLabor === 'number' ? rawdirectLabor : Number(rawdirectLabor)
      if (Number.isFinite(numdirectLabor)) payload.directLabor = numdirectLabor
      else delete payload.directLabor
    }
  }
  if ('indirectLabor' in payload) {
    const rawindirectLabor = payload.indirectLabor
    if (rawindirectLabor === undefined || rawindirectLabor === null || rawindirectLabor === '') {
      delete payload.indirectLabor
    } else {
      const numindirectLabor = typeof rawindirectLabor === 'number' ? rawindirectLabor : Number(rawindirectLabor)
      if (Number.isFinite(numindirectLabor)) payload.indirectLabor = numindirectLabor
      else delete payload.indirectLabor
    }
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    if (rawshiftNo === undefined || rawshiftNo === null || rawshiftNo === '') {
      delete payload.shiftNo
    } else {
      const numshiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
      if (Number.isFinite(numshiftNo)) payload.shiftNo = numshiftNo
      else delete payload.shiftNo
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.tenantCode = String(tenantStore.tenantCode ?? '').trim()
  payload.companyCode = String(tenantStore.companyCode ?? '').trim()
  if (!payload.plantCode) {
    payload.plantCode = resolveScopePlantCode()
  }
  if (!payload.cultureCode) {
    payload.cultureCode = resolveScopeCultureCode()
  }
  if (props.formData?.assyOutputId) {
    payload.assyOutputId = props.formData.assyOutputId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.assyOutputId)
  if (props.formData?.assyOutputId) {
    syncChildRowsFromFormData(props.formData)
  } else {
    recomputeMasterStdCapacity()
    syncAssyDetailsByStdCapacity()
  }
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.assy-output-form__master {
  display: flex;
  flex-direction: column;
}

.assy-output-form__master :deep(.assy-output-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.assy-output-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.assy-output-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.assy-output-form__master :deep(.ant-tabs-content),
.assy-output-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>
