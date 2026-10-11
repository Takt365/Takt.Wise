<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：EcForm.vue -->
<!-- 功能描述：设变维护弹窗内嵌表单；主表仅 ecLeader/ecContent/ecStatus/remark 可编辑；实施范围单独 Tab：生管选实施范围并联动部门范围，制二/制一/品管跟随，采购/受检/部管/SMT/制技固定按设变；来源导入时 ecEntryDate 只读且固定当天，设变状态默认发行；明细 Tab 客户端分页，表高按弹出窗体 × 5/4 且不超过 Tab 剩余区（仅表格滚动、弹出窗体无滚动条）；附件 Tab 工具栏增删改（来源导入无预置行，须手工维护） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-form flex h-full min-h-0 min-w-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-form-tabs flex-1 min-h-0 min-w-0"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="gi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('tenantCode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('companyCode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('cultureCode') })"
                  show-count
                  :maxlength="5"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('plantCode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('ecCode')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('ecCode') })"
                  show-count
                  :maxlength="10"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('ecIssueDate')"
                name="ecIssueDate"
              >
                <a-date-picker
                  v-model:value="formState.ecIssueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: gi.label('ecIssueDate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('changeStatus')"
                name="changeStatus"
              >
                <TaktSelect
                  v-model:value="formState.changeStatus"
                  dict-type="logistics_manufacturing_ec_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: gi.label('changeStatus') })"
                  allow-clear
                  class="w-full"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('ecTitle')"
                name="ecTitle"
              >
                <a-input
                  v-model:value="formState.ecTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('ecTitle') })"
                  show-count
                  :maxlength="500"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('ecLeader')"
                name="ecLeader"
              >
                <TaktSelect
                  v-model:value="formState.ecLeader"
                  api-url="TaktEcGroups/options"
                  :api-params="{ ecGroupCategory: 'D0710' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: gi.label('ecLeader') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="gi.label('ecEntryDate')"
                name="ecEntryDate"
              >
                <a-date-picker
                  v-model:value="formState.ecEntryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: gi.label('ecEntryDate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  :disabled="loading || sourceImportMode"
                  :allow-clear="!sourceImportMode"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="gi.label('ecContent')"
                name="ecContent"
              >
                <a-textarea
                  v-model:value="formState.ecContent"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('ecContent') })"
                  :rows="8"
                  allow-clear
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="gi.label('ecLossAmount')"
                name="ecLossAmount"
              >
                <a-input-number
                  v-model:value="formState.ecLossAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: gi.label('ecLossAmount') })"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="gi.label('ecStatus')"
                name="ecStatus"
              >
                <TaktSelect
                  v-model:value="formState.ecStatus"
                  dict-type="logistics_manufacturing_ec_gijutsu_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: gi.label('ecStatus') })"
                  allow-clear
                  class="w-full"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="gi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: gi.label('remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-scope"
        :tab="t('logistics.manufacturing.engineering-change.ec-gijutsu.page.tabs.scope')"
        force-render
      >
        <div :class="formContentClass">
          <a-descriptions
            bordered
            :column="1"
            size="middle"
            class="w-full max-w-xl"
            :label-style="{ width: '160px' }"
          >
            <a-descriptions-item :label="gi.label('ecScope')">
              <a-form-item
                name="ecScope"
                no-style
              >
                <a-select
                  v-model:value="formState.ecScope"
                  :options="ecScopeSelectOptions"
                  variant="borderless"
                  class="w-full"
                  :disabled="loading"
                  @change="handleEcScopeChange"
                />
              </a-form-item>
            </a-descriptions-item>
            <a-descriptions-item :label="t('logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.master')">
              <a-form-item
                name="ecDeptScope"
                no-style
              >
                <a-select
                  v-model:value="formState.ecDeptScope"
                  :options="ecDeptScopeSelectOptions"
                  variant="borderless"
                  class="w-full"
                  :disabled="loading"
                />
              </a-form-item>
            </a-descriptions-item>
            <a-descriptions-item
              v-for="dept in deptScopeRows"
              :key="dept.code"
              :label="t(dept.labelKey)"
            >
              {{ dept.text }}
            </a-descriptions-item>
          </a-descriptions>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="pi.self()"
        force-render
      >
        <div
          ref="ecDetailTableHostEl"
          class="ec-form-sub-table-wrap ec-form-detail-table-wrap min-h-0 min-w-0 flex-1 overflow-hidden"
        >
          <TaktSingleTable
            class="h-full min-h-0"
            entity-scope="company"
            :columns="ecDetailTableColumns"
            :visible-column-keys="ecDetailVisibleColumnKeys"
            :data-source="paginatedEcDetailRows"
            :loading="loading"
            :stripe="true"
            :row-key="getEcDetailRowKey"
            :show-row-selection="false"
            :include-audit-fields="false"
            scroll-layout="editable"
            :scroll="ecDetailTableScroll"
            table-mode="single"
            :show-pagination="true"
            v-model:current="ecDetailCurrentPage"
            v-model:page-size="ecDetailPageSize"
            :total="ecDetailTotal"
          />
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="ai.self()"
        force-render
      >
        <TaktToolsBar
          create-permission="logistics:manufacturing:engineering:change:gijutsu:create"
          update-permission="logistics:manufacturing:engineering:change:gijutsu:update"
          delete-permission="logistics:manufacturing:engineering:change:gijutsu:delete"
          :show-create="true"
          :show-update="true"
          :show-delete="true"
          :show-import="false"
          :show-export="false"
          :show-expand="false"
          :show-refresh="false"
          :show-advanced-query="false"
          :show-column-setting="false"
          :show-fullscreen="false"
          :create-disabled="loading"
          :update-disabled="loading || attachmentUpdateDisabled"
          :delete-disabled="loading || attachmentDeleteDisabled"
          :create-loading="loading"
          :update-loading="loading"
          :delete-loading="loading"
          @create="handleAttachmentCreate"
          @update="handleAttachmentUpdate"
          @delete="handleAttachmentDelete"
        />
        <div class="ec-form-sub-table-wrap min-h-0 min-w-0 flex-1 overflow-hidden">
          <TaktSingleTable
            class="h-full min-h-0"
            entity-scope="company"
            :columns="ecAttachmentTableColumns"
            :data-source="childEcAttachmentRows"
            :loading="loading"
            :stripe="true"
            :row-key="getEcAttachmentTableRowKey"
            :row-selection="attachmentRowSelection"
            :custom-row="onAttachmentClickRow"
            :show-row-selection="true"
            :include-audit-fields="false"
            scroll-layout="editable"
            table-mode="single"
            :show-pagination="false"
          />
        </div>
        <TaktModal
          v-model:open="attachmentFormVisible"
          :title="attachmentFormTitle"
          :width="ecGijutsuModalWidthPx"
          :confirm-loading="attachmentFormLoading"
          @ok="handleAttachmentFormSubmit"
          @cancel="handleAttachmentFormCancel"
        >
          <EcAttachmentForm
            :key="String(attachmentFormData?.ecAttachmentId ?? attachmentFormData?.__rowKey ?? 'create')"
            ref="attachmentFormRef"
            :form-data="attachmentFormData"
            :master-id="masterEcIdForAttachment"
            :current-ec-code="currentMasterEcCode"
            :current-plant-code="String(formState.plantCode ?? '')"
            :current-culture-code="String(formState.cultureCode ?? '')"
            :sibling-doc-codes="attachmentSiblingDocCodes"
            :sibling-file-names="attachmentSiblingFileNames"
            :loading="attachmentFormLoading || loading"
          />
        </TaktModal>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { useI18n } from 'vue-i18n'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcGijutsuFormData } from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import EcAttachmentForm from './EcAttachmentForm.vue'
import TaktDictTag from '@/components/common/TaktDictTag/index.vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import {
  computeFormHostRatioScrollYPx,
  TAKT_TABLE_SCROLL_Y_MIN,
} from '@/utils/tableScroll'
import {
  ECDETAIL_FORM_SUBTABLE_VISIBLE_COLUMN_KEYS,
  buildEcDetailTableColumns,
  useEcDetailI18n,
} from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n'
import {
  buildEcAttachmentFileName,
  getEcAttachmentDocCodeHintKey,
  isValidEcAttachmentDocCode,
} from '@/utils/taktEcAttachmentDocCode'
import { useEcAttachmentPreview } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcAttachmentPreview'
import { useEcGijutsuI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcGijutsuI18n'
import { useEcAttachmentI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcAttachmentI18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import {
  EC_SCOPE_FORM_DEPTS,
  taktEcDeptDedupDisplay,
  taktEcDeptScopeChoices,
  taktNormalizeEcDeptScope,
} from '@/constants/logistics/ecScope.constants'
import { translateLocaleMessage } from '@/utils/taktI18nMessage'

/** i18n 翻译函数 */
const { t } = useI18n()
const gi = useEcGijutsuI18n()
const ai = useEcAttachmentI18n()
const pi = useEcDetailI18n()
/** 附件弹窗宽度：与来源设变导入弹窗相同，（视口 − 左侧菜单）× 80% */
const ecGijutsuModalWidthPx = useTaktContentModalWidth()
const {
  canPreviewAttachment,
  hasPreviewableAccessUrl,
  handleAttachmentDocCodeClick,
} = useEcAttachmentPreview()
/** 附件 DocCode 文案前缀 */
const ATTACHMENT_DOC_CODE_I18N = 'logistics.manufacturing.engineering-change.ec-gijutsu.page.attachment.docCode'

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 区域文化 / 工厂（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (formFields.includes('plantCode') && (force || !target.plantCode)) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","ecNo","ecIssueDate","changeStatus","ecTitle","ecContent","ecLeader","ecLossAmount","ecScope","ecDeptScope","ecEntryDate","ecStatus","remark"]

const childEcDetailRows = ref<Record<string, unknown>[]>([])
/** 明细子表当前页 */
const ecDetailCurrentPage = ref(getTaktDefaultPageIndex())
/** 明细子表每页条数 */
const ecDetailPageSize = ref(getTaktDefaultPageSize())
/** 明细子表总行数 */
const ecDetailTotal = computed(() => childEcDetailRows.value.length)
/** 明细子表当前页数据（内存分页） */
const paginatedEcDetailRows = computed(() => {
  const rows = childEcDetailRows.value
  if (rows.length === 0) {
    return []
  }
  const size = Math.max(1, ecDetailPageSize.value)
  const start = (ecDetailCurrentPage.value - 1) * size
  return rows.slice(start, start + size)
})

/** 明细表宿主（定位弹出窗体并实测 Tab 剩余高度） */
const ecDetailTableHostEl = ref<HTMLElement | null>(null)
/** 弹出窗体 ResizeObserver */
let ecDetailFormHostResizeObserver: ResizeObserver | null = null

/**
 * 明细表 scroll.y：目标为弹出窗体高度 × 5/4，且不得超过 Tab 剩余高度
 * （超出则弹出窗体出现第二套滚动条；❌ 禁止用浏览器视口）
 */
function computeEcDetailTableScrollYPx(): number {
  const host = ecDetailTableHostEl.value
  const ratioY = computeFormHostRatioScrollYPx(host, 5, 4)
  if (host == null || host.clientHeight <= 0) {
    return ratioY
  }
  const tableBody = host.querySelector('.takt-single-table__body') as HTMLElement | null
  if (tableBody == null || tableBody.clientHeight <= 0) {
    return ratioY
  }
  const fittedY = measureMasterDetailLrTableScrollY(host, { reserveSummaryRow: false })
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.min(ratioY, fittedY))
}

/** 明细表纵向滚动高度（px） */
const ecDetailTableScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)

/** 明细表 scroll 配置 */
const ecDetailTableScroll = computed(() => ({ y: ecDetailTableScrollYPx.value }))

/** 按弹出窗体与 Tab 剩余区重算明细表高度 */
function recalcEcDetailTableScrollY(): void {
  ecDetailTableScrollYPx.value = computeEcDetailTableScrollYPx()
}

/** 绑定弹出窗体 ResizeObserver（全屏/拖拽改高时重算） */
function bindEcDetailFormHostResizeObserver(): void {
  ecDetailFormHostResizeObserver?.disconnect()
  ecDetailFormHostResizeObserver = null
  const host = ecDetailTableHostEl.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const modalTarget =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
  ecDetailFormHostResizeObserver = new ResizeObserver(() => {
    recalcEcDetailTableScrollY()
  })
  if (modalTarget != null) {
    ecDetailFormHostResizeObserver.observe(modalTarget)
  }
  ecDetailFormHostResizeObserver.observe(host)
}

onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  void nextTick(() => {
    recalcEcDetailTableScrollY()
    bindEcDetailFormHostResizeObserver()
  })
})

/** 切到明细 Tab 时再测一次（弹窗动画/全屏切换后） */
watch(activeTab, (key) => {
  if (key === 'tab-2') {
    void nextTick(() => {
      recalcEcDetailTableScrollY()
      bindEcDetailFormHostResizeObserver()
    })
  }
})

onBeforeUnmount(() => {
  ecDetailFormHostResizeObserver?.disconnect()
  ecDetailFormHostResizeObserver = null
})

const childEcAttachmentRows = ref<Record<string, unknown>[]>([])
/** 附件子表选中行 */
const attachmentSelectedRowKeys = ref<(string | number)[]>([])
const attachmentSelectedRows = ref<Record<string, unknown>[]>([])
const attachmentSelectedRow = ref<Record<string, unknown> | null>(null)
/** 附件子弹窗 */
const attachmentFormVisible = ref(false)
const attachmentFormTitle = ref('')
const attachmentFormData = ref<Record<string, unknown>>({})
const attachmentFormLoading = ref(false)
const attachmentFormRef = ref()

/**
 * 当前编辑行以外的附件文件编码（供弹窗查重）
 */
const attachmentSiblingDocCodes = computed(() => {
  const editing = attachmentFormData.value
  return childEcAttachmentRows.value
    .filter((row, index) => !isSameAttachmentRow(row, editing, index))
    .map((row) => String(row.docCode ?? row.docNo ?? '').trim())
    .filter(Boolean)
})

/**
 * 当前编辑行以外的附件文件名称（供弹窗查重）
 */
const attachmentSiblingFileNames = computed(() => {
  const editing = attachmentFormData.value
  return childEcAttachmentRows.value
    .filter((row, index) => !isSameAttachmentRow(row, editing, index))
    .map((row) => String(row.fileName ?? '').trim())
    .filter(Boolean)
})

/** 主表当前设变号码（供附件 EC 类型自动赋文件编码） */
const currentMasterEcCode = computed(() =>
  String(formState.ecCode ?? formState.ecNo ?? '').trim(),
)

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: EcGijutsuFormData | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 来源设变导入草稿模式：明细只读，附件须上传后保存 */
  sourceImportMode?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  sourceImportMode: false,
})

/** 主表 Id（附件 form 外键；未保存时为空） */
const masterEcIdForAttachment = computed(() => props.formData?.ecGijutsuId ?? '')
const attachmentUpdateDisabled = computed(() => attachmentSelectedRows.value.length !== 1)
const attachmentDeleteDisabled = computed(() => attachmentSelectedRows.value.length === 0)

/** 明细子表列（与 TaktEcDetail / 来源设变导入草稿对齐） */
const ecDetailTableColumns = computed<TableColumnsType>(() =>
  buildEcDetailTableColumns((field) => pi.columnLabel(field)),
)

/** 内嵌明细 Tab 可见列（显式全列，绕过 TaktSingleTable 默认 8 列截断） */
const ecDetailVisibleColumnKeys = computed(() => [...ECDETAIL_FORM_SUBTABLE_VISIBLE_COLUMN_KEYS])

/** 附件子表列（只读展示 + 操作列） */
const ecAttachmentTableColumns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: gi.label('plantCode'),
      dataIndex: 'plantCode',
      key: 'plantCode',
      width: 90,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'plantCode'),
    },
    {
      title: ai.label('lineNumber'),
      dataIndex: 'lineNumber',
      key: 'lineNumber',
      width: 90,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'lineNumber'),
    },
    {
      title: ai.label('attachmentType'),
      dataIndex: 'attachmentType',
      key: 'attachmentType',
      width: 120,
      ellipsis: true,
      customRender: ({ record }) => h(TaktDictTag, {
        dictType: 'logistics_manufacturing_ec_attachment_type',
        value: String((record as Record<string, unknown>).attachmentType ?? ''),
      }),
    },
    {
      title: ai.label('docCode'),
      dataIndex: 'docCode',
      key: 'docCode',
      width: 140,
      ellipsis: true,
      customRender: ({ record }) => renderAttachmentDocCodeCell(record as Record<string, unknown>),
    },
    {
      title: ai.label('fileName'),
      dataIndex: 'fileName',
      key: 'fileName',
      width: 140,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'fileName'),
    },
    {
      title: ai.label('accessUrl'),
      dataIndex: 'accessUrl',
      key: 'accessUrl',
      width: 200,
      ellipsis: true,
      customRender: ({ record }) => formatSubTableCell(record as Record<string, unknown>, 'accessUrl'),
    },
  ]
  const actions = [
    {
      key: 'update',
      label: t('common.page.button.edit'),
      shape: 'plain' as const,
      icon: RiEditLine,
      permission: 'logistics:manufacturing:engineering:change:gijutsu:update',
      onClick: (record: Record<string, unknown>) => void handleAttachmentEdit(record),
    },
    {
      key: 'delete',
      label: t('common.page.button.delete'),
      shape: 'plain' as const,
      icon: RiDeleteBinLine,
      permission: 'logistics:manufacturing:engineering:change:gijutsu:delete',
      onClick: (record: Record<string, unknown>) => void handleAttachmentDeleteOne(record),
    },
  ]
  cols.push(CreateActionColumn({ actions }))
  return cols
})

/**
 * 明细行 row-key
 * @param record 明细行
 * @param index 行索引
 * @returns 行 key
 */
function getEcDetailRowKey(record: Record<string, unknown>, index?: number): string {
  return String(record.ecDetailId ?? record.__rowKey ?? `detail-row-${index ?? 0}`)
}

/**
 * 附件行 row-key（TaktSingleTable）
 * @param record 附件行
 * @param index 行索引
 * @returns 行 key
 */
function getEcAttachmentTableRowKey(record: Record<string, unknown>, index?: number): string {
  return getAttachmentRowKey(record, index ?? 0)
}

/**
 * 子表单元格展示文本
 * @param record 行数据
 * @param key 字段名
 * @returns 展示文本
 */
function formatSubTableCell(record: Record<string, unknown>, key: string): string {
  const value = record[key]
  if (value === undefined || value === null || value === '') {
    return '-'
  }
  return String(value)
}

/**
 * 附件子表文件编码：有访问地址且具备 preview 权限时渲染为超链接（鉴权 preview API，不跳转 /uploads 页面）
 * @param record 附件行
 */
function renderAttachmentDocCodeCell(record: Record<string, unknown>): ReturnType<typeof h> | string {
  const code = String(record.docCode ?? record.docNo ?? '').trim()
  const canOpen =
    canPreviewAttachment.value &&
    hasPreviewableAccessUrl(record.accessUrl) &&
    !!code
  if (canOpen) {
    return h(
      'a',
      {
        href: '#',
        class: 'text-primary hover:underline',
        title: t('common.page.button.preview'),
        onClick: (e: MouseEvent) => {
          handleAttachmentDocCodeClick(record, e)
        },
      },
      code,
    )
  }
  return code || '-'
}

/** 规范化附件行（补全 __rowKey） */
function normalizeAttachmentRows(rows: Record<string, unknown>[]): Record<string, unknown>[] {
  const plantCode = String(formState.plantCode ?? '').trim()
  const cultureCode = String(formState.cultureCode ?? '').trim()
  return rows.map((row, index) => {
    const docCode = String(row.docCode ?? row.docNo ?? '').trim()
    return {
      ...row,
      tenantCode: row.tenantCode ?? tenantStore.tenantCode,
      companyCode: row.companyCode ?? tenantStore.companyCode,
      cultureCode: row.cultureCode ?? cultureCode,
      plantCode: row.plantCode ?? plantCode,
      ecCode: row.ecCode ?? row.ecNo ?? '',
      docCode,
      fileName: buildEcAttachmentFileName(docCode, String(row.fileName ?? ''), String(row.accessUrl ?? '')) || row.fileName,
      __rowKey: row.__rowKey ?? row.ecAttachmentId ?? `row-${index}`,
    }
  })
}

/** 重置明细子表分页至第一页 */
function resetEcDetailPagination(): void {
  ecDetailCurrentPage.value = getTaktDefaultPageIndex()
}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: EcGijutsuFormData | null | undefined) {
  const plantCode = String(val?.plantCode ?? formState.plantCode ?? '')
  const ecCode = String(val?.ecCode ?? val?.ecNo ?? formState.ecCode ?? formState.ecNo ?? '')
  childEcDetailRows.value = (((val as any)?.ecDetails ?? []) as Record<string, unknown>[]).map((row, index) => ({
    ...row,
    plantCode: row.plantCode ?? plantCode,
    ecCode: row.ecCode ?? row.ecNo ?? ecCode,
    __rowKey: row.__rowKey ?? row.ecDetailId ?? `detail-row-${index}`,
  }))
  resetEcDetailPagination()
  childEcAttachmentRows.value = normalizeAttachmentRows(((val as any)?.attachments ?? []) as Record<string, unknown>[])
  clearAttachmentSelection()
}

/**
 * 附件行唯一键
 * @param record 附件行
 * @param index 行索引
 * @returns 行 key
 */
function getAttachmentRowKey(record: Record<string, unknown>, index: number): string {
  return String(record.__rowKey ?? record.ecAttachmentId ?? `row-${index}`)
}

/** 清空附件子表选中态 */
function clearAttachmentSelection() {
  attachmentSelectedRowKeys.value = []
  attachmentSelectedRows.value = []
  attachmentSelectedRow.value = null
}

/**
 * 是否同一附件行
 * @param row 列表行
 * @param target 目标行
 * @param index 列表索引
 */
function isSameAttachmentRow(
  row: Record<string, unknown>,
  target: Record<string, unknown>,
  index: number,
): boolean {
  const rowKey = getAttachmentRowKey(row, index)
  const targetKey = getAttachmentRowKey(target, 0)
  if (rowKey === targetKey) {
    return true
  }
  const rowId = String(row.ecAttachmentId ?? '')
  const targetId = String(target.ecAttachmentId ?? '')
  return rowId.length > 0 && rowId === targetId
}

const attachmentRowSelection = computed(() => ({
  selectedRowKeys: attachmentSelectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Record<string, unknown>[]) => {
    attachmentSelectedRowKeys.value = keys
    attachmentSelectedRows.value = rows
    attachmentSelectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Record<string, unknown>, selected: boolean) => {
    if (selected) {
      attachmentSelectedRow.value = record
    } else if (
      attachmentSelectedRow.value != null
      && isSameAttachmentRow(record, attachmentSelectedRow.value, 0)
    ) {
      attachmentSelectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, rows: Record<string, unknown>[]) => {
    attachmentSelectedRow.value = selected && rows.length === 1 ? (rows[0] ?? null) : null
  },
}))

/**
 * 附件行点击选中
 * @param record 行数据
 */
function onAttachmentClickRow(record: Record<string, unknown>) {
  const index = childEcAttachmentRows.value.findIndex((row, i) => isSameAttachmentRow(row, record, i))
  const key = getAttachmentRowKey(record, index >= 0 ? index : 0)
  return {
    onClick: () => {
      attachmentSelectedRowKeys.value = [key]
      attachmentSelectedRows.value = [record]
      attachmentSelectedRow.value = record
    },
    class: attachmentSelectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/** 打开新增附件弹窗 */
function handleAttachmentCreate() {
  attachmentFormTitle.value = t('common.dialog.title.create', { entity: ai.self() })
  const ecCode = String(formState.ecCode ?? formState.ecNo ?? '').trim()
  attachmentFormData.value = {
    tenantCode: String(formState.tenantCode ?? tenantStore.tenantCode ?? ''),
    companyCode: String(formState.companyCode ?? tenantStore.companyCode ?? ''),
    cultureCode: String(formState.cultureCode ?? userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''),
    plantCode: String(formState.plantCode ?? ''),
    ecCode,
    attachmentType: 'EC',
    docCode: ecCode,
  }
  attachmentFormVisible.value = true
}

/** 工具栏编辑：打开当前单选附件行 */
function handleAttachmentUpdate() {
  if (attachmentSelectedRow.value) {
    handleAttachmentEdit(attachmentSelectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: ai.self(),
    }))
  }
}

/**
 * 打开编辑附件弹窗
 * @param record 附件行
 */
function handleAttachmentEdit(record: Record<string, unknown>) {
  attachmentFormTitle.value = t('common.dialog.title.edit', { entity: ai.self() })
  attachmentFormData.value = { ...record }
  attachmentFormVisible.value = true
}

/** 提交附件子弹窗（内存增改，随主表一并保存） */
async function handleAttachmentFormSubmit() {
  const refInst = attachmentFormRef.value
  if (!refInst?.validate) {
    return
  }
  try {
    await refInst.validate()
  } catch {
    return
  }
  attachmentFormLoading.value = true
  try {
    const values = refInst.getValues?.() ?? {}
    const editing = attachmentFormData.value
    const editingId = editing?.ecAttachmentId
    const editingClientKey = editing?.__rowKey
    const isEdit = !!(editingId || editingClientKey)
    if (isEdit) {
      childEcAttachmentRows.value = childEcAttachmentRows.value.map((row, index) => {
        if (isSameAttachmentRow(row, editing, index)) {
          return {
            ...row,
            ...values,
            ecAttachmentId: row.ecAttachmentId,
            __rowKey: row.__rowKey ?? editingClientKey,
          }
        }
        return row
      })
    } else {
      const rows = childEcAttachmentRows.value
      const maxLine = rows.reduce((max, row) => {
        const line = Number(row.lineNumber)
        return Number.isFinite(line) ? Math.max(max, line) : max
      }, 0)
      const lineNumber = values.lineNumber ?? (maxLine > 0 ? maxLine + 10 : 10)
      childEcAttachmentRows.value = [
        ...rows,
        {
          __rowKey: `client-${crypto.randomUUID()}`,
          ...values,
          lineNumber,
          ecCode: String(formState.ecCode ?? formState.ecNo ?? ''),
        },
      ]
    }
    attachmentFormVisible.value = false
    attachmentFormData.value = {}
    nextTick(() => attachmentFormRef.value?.resetFields?.())
    clearAttachmentSelection()
  } finally {
    attachmentFormLoading.value = false
  }
}

/** 关闭附件子弹窗 */
function handleAttachmentFormCancel() {
  attachmentFormVisible.value = false
  attachmentFormData.value = {}
  nextTick(() => attachmentFormRef.value?.resetFields?.())
}

/**
 * 从内存列表移除附件行
 * @param records 待删行
 */
function removeAttachmentRows(records: Record<string, unknown>[]) {
  if (records.length === 0) {
    return
  }
  childEcAttachmentRows.value = childEcAttachmentRows.value.filter(
    (row, index) => !records.some((target) => isSameAttachmentRow(row, target, index)),
  )
  clearAttachmentSelection()
}

/** 工具栏批删附件行 */
function handleAttachmentDelete() {
  if (attachmentSelectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: ai.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: ai.self(),
      count: attachmentSelectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => {
      removeAttachmentRows([...attachmentSelectedRows.value])
      message.success(t('common.feedback.deleted', { target: ai.self() }))
    },
  })
}

/**
 * 行内删除单条附件
 * @param record 附件行
 */
function handleAttachmentDeleteOne(record: Record<string, unknown>) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: ai.self(),
      name: t('common.tip.this.target', { target: ai.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => {
      removeAttachmentRows([record])
      message.success(t('common.feedback.deleted', { target: ai.self() }))
    },
  })
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = String(props.formData?.ecGijutsuId ?? '').trim()
  const ecIdForChild = masterId || '0'
  const masterEcCode = String(formState.ecCode ?? formState.ecNo ?? '').trim()
  const masterCulture = String(formState.cultureCode ?? userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '').trim()
  const masterPlant = String(formState.plantCode ?? '').trim()
  const payload: Record<string, unknown> = {
    ...formState,
    ecCode: masterEcCode,
    cultureCode: masterCulture,
    plantCode: masterPlant,
    ecDetails: childEcDetailRows.value.map((rest) => {
      const { __rowKey, ecNo, docNo, ...row } = rest
      return {
        ...row,
        tenantCode: row.tenantCode ?? tenantStore.tenantCode,
        companyCode: row.companyCode ?? tenantStore.companyCode,
        cultureCode: row.cultureCode ?? masterCulture,
        plantCode: row.plantCode ?? masterPlant,
        ecCode: row.ecCode ?? ecNo ?? masterEcCode,
        ecGijutsuId: ecIdForChild,
      }
    }),
    attachments: childEcAttachmentRows.value.map((rest) => {
      const { __rowKey, ecNo, docNo, ...row } = rest
      const docCode = String(row.docCode ?? docNo ?? '').trim()
      return {
        ...row,
        tenantCode: row.tenantCode ?? tenantStore.tenantCode,
        companyCode: row.companyCode ?? tenantStore.companyCode,
        cultureCode: row.cultureCode ?? masterCulture,
        plantCode: row.plantCode ?? masterPlant,
        ecGijutsuId: ecIdForChild,
        ecCode: row.ecCode ?? ecNo ?? masterEcCode,
        docCode,
        fileName: buildEcAttachmentFileName(docCode, String(row.fileName ?? ''), String(row.accessUrl ?? '')) || row.fileName,
      }
    }),
  }
  if (props.sourceImportMode) {
    payload.ecEntryDate = formatLocalTodayYmd()
  }
  // 大明细：不回传 EcDetails，仅带 SourceEcId，由服务端从来源表物化
  const detailsDeferred = Boolean(formState.detailsDeferred ?? props.formData?.detailsDeferred)
  const sourceEcId = String(formState.sourceEcId ?? props.formData?.sourceEcId ?? '').trim()
  if (detailsDeferred && sourceEcId) {
    payload.sourceEcId = sourceEcId
    payload.detailsDeferred = true
    payload.deferredDetailCount = Number(formState.deferredDetailCount ?? props.formData?.deferredDetailCount ?? 0)
    payload.ecDetails = []
  }
  return payload
}

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/**
 * 本地当天日期（YYYY-MM-DD）；来源导入录入日期固定为此值
 * @returns 当天日期字符串
 */
function formatLocalTodayYmd(): string {
  const now = new Date()
  const y = String(now.getFullYear())
  const m = String(now.getMonth() + 1).padStart(2, '0')
  const d = String(now.getDate()).padStart(2, '0')
  return `${y}-${m}-${d}`
}

/**
 * 来源导入时将录入日期锁定为当天
 * @param target 表单数据
 */
function applySourceImportEntryDate(target: Record<string, unknown>) {
  if (props.sourceImportMode) {
    target.ecEntryDate = formatLocalTodayYmd()
  }
}

/** 来源导入设变状态默认：发行（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成） */
const SOURCE_IMPORT_EC_STATUS_ISSUED = 1

/** 实施范围默认：全仕向（字典 logistics_manufacturing_ec_scope_category；1=全仕向） */
const DEFAULT_EC_SCOPE_ALL_DESTINATION = 1

/**
 * 来源导入时设变状态默认选中发行（0 / 空视为未选）
 * @param target 表单数据
 */
function applySourceImportEcStatus(target: Record<string, unknown>) {
  if (!props.sourceImportMode) {
    return
  }
  const raw = target.ecStatus
  const num = typeof raw === 'number' ? raw : Number(raw)
  if (num !== 1 && num !== 2 && num !== 3) {
    target.ecStatus = SOURCE_IMPORT_EC_STATUS_ISSUED
  }
}

/** 字典下拉 */
const dictDataStore = useDictDataStore()

/**
 * 字典项转为下拉（数值 value + 翻译后的标签）
 * @param dictType 字典类型
 * @param includeValues 仅保留这些值；空则全部
 * @returns {{ label: string; value: number }[]} 选项
 */
function scopeDictOptions(dictType: string, includeValues?: readonly number[]) {
  return dictDataStore.getDictOptionsForSelect(dictType, {
    valueField: 'dictValue',
    labelField: 'dictLabel',
  }).flatMap((option) => {
    const num = Number(option.value)
    if (!Number.isFinite(num)) {
      return []
    }
    if (includeValues?.length && !includeValues.some((item) => item === num)) {
      return []
    }
    const i18nKey = typeof option.i18nKey === 'string' ? option.i18nKey.trim() : ''
    const translated = i18nKey ? translateLocaleMessage(i18nKey) : ''
    const label = translated && translated !== i18nKey ? translated : String(option.label ?? option.dictLabel ?? num)
    return [{ label, value: num }]
  })
}

/** 实施范围下拉 */
const ecScopeSelectOptions = computed(() => scopeDictOptions('logistics_manufacturing_ec_scope_category'))

/** 部门范围可选项（全仕向三项；部管/内部/技术仅按设变） */
const ecDeptScopeIncludeValues = computed(() => taktEcDeptScopeChoices(Number(formState.ecScope)).allowed)

/** 部门范围下拉 */
const ecDeptScopeSelectOptions = computed(() => scopeDictOptions(
  'logistics_manufacturing_ec_dept_scope_category',
  ecDeptScopeIncludeValues.value,
))

/**
 * 实施范围对照表。制二/制一/品管随部门范围显示同一字典文案
 */
const deptScopeRows = computed(() => {
  const ecScope = Number(formState.ecScope)
  const ecDeptScope = Number(formState.ecDeptScope)
  const autoFill = t('logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.autoFill')
  return EC_SCOPE_FORM_DEPTS.map((dept) => {
    const display = taktEcDeptDedupDisplay(dept.code, ecScope, ecDeptScope)
    const text = display.mode === 'dict'
      ? (scopeDictOptions('logistics_manufacturing_ec_dept_scope_category').find((item) => item.value === display.value)?.label ?? '')
      : autoFill
    return { ...dept, text }
  })
})

/**
 * 切换实施范围时，部门实施范围改选该范围的默认项
 * @param value 实施范围
 */
function handleEcScopeChange(value: unknown) {
  const scope = typeof value === 'number' ? value : Number(value)
  formState.ecDeptScope = taktEcDeptScopeChoices(scope).defaultValue
}

/**
 * 实施范围未选时默认全仕向；部门实施范围不在允许集合内时改为该范围的默认项
 * @param target 表单数据
 */
function applyDefaultEcScope(target: Record<string, unknown>) {
  const raw = target.ecScope
  const num = typeof raw === 'number' ? raw : Number(raw)
  const scope = num === 1 || num === 2 || num === 3 || num === 4 ? num : DEFAULT_EC_SCOPE_ALL_DESTINATION
  target.ecScope = scope
  target.ecDeptScope = taktNormalizeEcDeptScope(scope, target.ecDeptScope)
}

/** 表单字段默认值（来源导入时录入日期固定当天，设变状态默认发行，实施范围默认全仕向） */
function applyFormDefaults(target: Record<string, unknown>) {
  applySourceImportEntryDate(target)
  applySourceImportEcStatus(target)
  applyDefaultEcScope(target)
}

/** 将 API ecCode 映射到表单 ecNo（历史表单字段名） */
function normalizeMasterFormFields(target: Record<string, unknown>): void {
  const ecCode = target.ecCode
  if (typeof ecCode === 'string' && ecCode.trim() && !target.ecNo) {
    target.ecNo = ecCode
  }
  if (!target.ecCode && target.ecNo) {
    target.ecCode = target.ecNo
  }
  if (target.ExtField != null && target.extField == null) {
    target.extField = target.ExtField
    delete target.ExtField
  }
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecGijutsuId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecGijutsuId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as any).ecDetails
      delete (next as any).attachments
      normalizeMasterFormFields(next)
      applyScopeDefaults(next)
      applyDefaultEcScope(next)
      Object.assign(formState, next)
      syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        delete (next as any).ecDetails
        delete (next as any).attachments
        normalizeMasterFormFields(next)
        Object.assign(formState, next)
        syncChildRowsFromFormData(val)
      } else {
        childEcDetailRows.value = []
        resetEcDetailPagination()
        childEcAttachmentRows.value = []
        clearAttachmentSelection()
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 明细行数变化时校正当前页（避免删行后停留在空页） */
watch(
  () => childEcDetailRows.value.length,
  (total) => {
    if (total === 0) {
      resetEcDetailPagination()
      return
    }
    const size = Math.max(1, ecDetailPageSize.value)
    const maxPage = Math.max(1, Math.ceil(total / size))
    if (ecDetailCurrentPage.value > maxPage) {
      ecDetailCurrentPage.value = maxPage
    }
  },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, userStore.userInfo?.cultureCode] as const,
  () => {
    const isCreate = !props.formData?.ecGijutsuId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: gi.label('plantCode') }),
      trigger: 'blur'
    }
  ],
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: gi.label('ecCode') }),
      trigger: 'blur'
    }
  ],
  ecIssueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: gi.label('ecIssueDate') }),
      trigger: 'change'
    }
  ],
  changeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('changeStatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('changeStatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: gi.label('ecTitle') }),
      trigger: 'blur'
    }
  ],
  ecContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: gi.label('ecContent') }),
      trigger: 'blur'
    }
  ],
  ecLeader: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: gi.label('ecLeader') }),
      trigger: 'change'
    }
  ],
  ecLossAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('ecLossAmount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('ecLossAmount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecScope: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('ecScope') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('ecScope') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecEntryDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: gi.label('ecEntryDate') }),
      trigger: 'change'
    }
  ],
  ecStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('ecStatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: gi.label('ecStatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  const attachmentRows = childEcAttachmentRows.value
  if (props.sourceImportMode && attachmentRows.length === 0) {
    activeTab.value = 'tab-3'
    const msg = t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.attachmentRequired')
    message.warning(msg)
    throw new Error(msg)
  }
  for (let i = 0; i < attachmentRows.length; i += 1) {
    const row = attachmentRows[i]
    const url = String(row.accessUrl ?? '').trim()
    const fileName = String(row.fileName ?? '').trim()
    const attachmentType = String(row.attachmentType ?? '').trim()
    const docCode = String(row.docCode ?? row.docNo ?? '').trim()
    const ecCode = String(formState.ecCode ?? formState.ecNo ?? row.ecCode ?? row.ecNo ?? '').trim()
    if (!props.sourceImportMode && (!attachmentType || !docCode)) {
      activeTab.value = 'tab-3'
      const msg = t('common.page.form.placeholder.required', { field: ai.self() })
      message.warning(msg)
      throw new Error(msg)
    }
    if (attachmentType && docCode && !isValidEcAttachmentDocCode(attachmentType, docCode, ecCode)) {
      activeTab.value = 'tab-3'
      const hintKey = getEcAttachmentDocCodeHintKey(attachmentType)
      const msg = t(`${ATTACHMENT_DOC_CODE_I18N}.formatInvalid`, {
        hint: t(`${ATTACHMENT_DOC_CODE_I18N}.hint.${hintKey}`),
      })
      message.warning(msg)
      throw new Error(msg)
    }
    if (!url || url === '-' || !fileName) {
      activeTab.value = 'tab-3'
      const msg = t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.attachmentUploadRequired', { row: i + 1 })
      message.warning(msg)
      throw new Error(msg)
    }
  }
  const seenDocCodes = new Set<string>()
  for (let i = 0; i < attachmentRows.length; i += 1) {
    const docCode = String(attachmentRows[i].docCode ?? attachmentRows[i].docNo ?? '').trim()
    if (!docCode) {
      continue
    }
    if (seenDocCodes.has(docCode)) {
      activeTab.value = 'tab-3'
      const msg = t(`${ATTACHMENT_DOC_CODE_I18N}.duplicate`, { code: docCode })
      message.warning(msg)
      throw new Error(msg)
    }
    seenDocCodes.add(docCode)
  }
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('changeStatus' in payload) {
    const rawchangeStatus = payload.changeStatus
    payload.changeStatus = typeof rawchangeStatus === 'number' ? rawchangeStatus : Number(rawchangeStatus)
  }
  if ('ecLossAmount' in payload) {
    const rawecLossAmount = payload.ecLossAmount
    payload.ecLossAmount = typeof rawecLossAmount === 'number' ? rawecLossAmount : Number(rawecLossAmount)
  }
  if ('ecStatus' in payload) {
    const rawecStatus = payload.ecStatus
    payload.ecStatus = typeof rawecStatus === 'number' ? rawecStatus : Number(rawecStatus)
  }
  if ('ecScope' in payload) {
    const rawEcScope = payload.ecScope
    const parsed = typeof rawEcScope === 'number' ? rawEcScope : Number(rawEcScope)
    payload.ecScope = Number.isFinite(parsed) ? parsed : 0
  }
  if ('ecDeptScope' in payload) {
    const rawEcDeptScope = payload.ecDeptScope
    const parsedDept = typeof rawEcDeptScope === 'number' ? rawEcDeptScope : Number(rawEcDeptScope)
    payload.ecDeptScope = Number.isFinite(parsedDept) ? parsedDept : 0
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if ('ecNo' in payload && payload.ecCode) delete payload.ecNo
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecGijutsuId)
  childEcDetailRows.value = []
  resetEcDetailPagination()
  childEcAttachmentRows.value = []
  clearAttachmentSelection()
  attachmentFormVisible.value = false
  attachmentFormData.value = {}
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
.ec-form-tabs.ant-tabs {
  display: flex;
  flex-direction: column;
  height: 100%;
  min-height: 0;
  min-width: 0;
}

.ec-form-tabs :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.ec-form-tabs :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
}

.ec-form-tabs :deep(.ant-tabs-content),
.ec-form-tabs :deep(.ant-tabs-tabpane) {
  height: 100%;
  min-height: 0;
  min-width: 0;
}

.ec-form-tabs :deep(.ant-tabs-tabpane) {
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.ec-form-tabs :deep(.takt-form-content-rows-10),
.ec-form-tabs :deep(.takt-form-content-rows-5) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.ec-form-tabs :deep(.ant-tabs-tabpane-active) {
  height: 100%;
}

.ec-form-detail-table-wrap {
  min-height: 0;
  min-width: 0;
  height: 100%;
  overflow: hidden;
}
</style>
