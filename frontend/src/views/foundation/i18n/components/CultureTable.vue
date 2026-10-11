<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/foundation/i18n/components -->
<!-- 文件名称：CultureTable.vue -->
<!-- 创建时间：2026-06-16 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：区域文化列表弹窗（查询、高级查询、列设置、全屏、分页、CRUD；表单 culture-form） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="localVisible"
    :title="t('foundation.i18n.page.culture.window.title')"
    :footer="null"
    :use-viewport-size="true"
  >
    <div class="culture-table flex min-h-0 flex-col">
      <TaktQueryBar
        v-model="queryKeyword"
        :placeholder="t('common.page.form.placeholder.search', { keyword: [t('entity.culture.nativename'), t('entity.culture.code')].join(t('common.tip.or')) })"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
      <TaktToolsBar
        create-permission="foundation:i18n:create"
        update-permission="foundation:i18n:update"
        delete-permission="foundation:i18n:delete"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-advanced-query="true"
        :show-column-setting="true"
        :show-fullscreen="true"
        :show-refresh="true"
        :create-disabled="false"
        :update-disabled="!selectedRow"
        :delete-disabled="selectedRows.length === 0"
        :create-loading="loading"
        :update-loading="loading"
        :delete-loading="loading"
        :refresh-loading="loading"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @fullscreen="handleFullscreen"
        @refresh="handleRefresh"
      />
      <div class="culture-table-list-wrap min-h-0 flex-1">
        <TaktSingleTable
          entity-scope="tenant-core"
          table-mode="single"
          :scroll="tableScroll"
          :columns="columns"
          :visible-column-keys="visibleColumnKeys"
          :id-column-key="'cultureId'"
          :data-source="dataSource"
          :loading="loading"
          :stripe="true"
          :row-key="getCultureRowKey"
          :row-selection="rowSelection"
          :pagination="false"
        >
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'isDefault'">
              <TaktDictTag
                :value="record.isDefault"
                dict-type="sys_yes_no"
              />
            </template>
          </template>
        </TaktSingleTable>
      </div>
      <TaktPagination
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        @change="handlePaginationChange"
        @show-size-change="handlePaginationSizeChange"
      />
    </div>
  </TaktModal>
  <!-- 高级查询 -->
  <TaktQueryDrawer
    v-model:open="advancedQueryVisible"
    v-model:visible-field-keys="visibleQueryFieldKeys"
    :fields="queryFieldsMeta"
    :storage-key="'takt-query-fields-foundation-culture'"
    :form-model="advancedQueryForm"
    @submit="handleAdvancedQuerySubmit"
    @reset="handleAdvancedQueryReset"
  >
    <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
        <a-form-item :label="t('entity.culture.code')">
          <a-input
            v-model:value="advancedQueryForm.cultureCode"
            :placeholder="t('common.page.form.placeholder.input', { field: t('entity.culture.code') })"
            allow-clear
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('nativeName')">
        <a-form-item :label="t('entity.culture.nativename')">
          <a-input
            v-model:value="advancedQueryForm.nativeName"
            :placeholder="t('common.page.form.placeholder.input', { field: t('entity.culture.nativename') })"
            allow-clear
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('icon')">
        <a-form-item :label="t('entity.culture.icon')">
          <a-input
            v-model:value="advancedQueryForm.icon"
            :placeholder="t('common.page.form.placeholder.input', { field: t('entity.culture.icon') })"
            allow-clear
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
        <a-form-item :label="t('entity.culture.sortorder')">
          <a-input-number
            v-model:value="advancedQueryForm.sortOrder"
            :min="0"
            :placeholder="t('common.page.form.placeholder.input', { field: t('entity.culture.sortorder') })"
            style="width: 100%"
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('isDefault')">
        <a-form-item :label="t('entity.culture.isdefault')">
          <TaktSelect
            v-model:value="advancedQueryForm.isDefault"
            dict-type="sys_yes_no"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.culture.isdefault') })"
            allow-clear
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
        <a-form-item :label="t('common.page.entity.createdatstart')">
          <a-date-picker
            v-model:value="advancedQueryForm.createdAtStart"
            :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
            value-format="YYYY-MM-DD HH:mm:ss"
            show-time
            style="width: 100%"
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
        <a-form-item :label="t('common.page.entity.createdatend')">
          <a-date-picker
            v-model:value="advancedQueryForm.createdAtEnd"
            :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
            value-format="YYYY-MM-DD HH:mm:ss"
            show-time
            style="width: 100%"
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
        <a-form-item :label="t('common.page.entity.extfield')">
          <a-textarea
            v-model:value="advancedQueryForm.extField"
            :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="2"
            allow-clear
          />
        </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
        <a-form-item :label="t('common.page.entity.remark')">
          <a-textarea
            v-model:value="advancedQueryForm.remark"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="2"
            allow-clear
          />
        </a-form-item>
      </div>
    </template>
  </TaktQueryDrawer>
  <!-- 列设置 -->
  <TaktColumnDrawer
    v-model:open="columnSettingVisible"
    entity-scope="tenant-core"
    table-mode="single"
    :columns="columns"
    :checked-keys="visibleColumnKeys"
    :id-column-key="'cultureId'"
    :action-column-key="'action'"
    @update:checked-keys="handleColumnKeysChange"
    @reset="handleColumnSettingReset"
  />
  <TaktModal
    v-model:open="formVisible"
    :title="formTitle"
    :width="640"
    :confirm-loading="formLoading"
    :use-viewport-size="false"
    @ok="handleFormSubmit"
    @cancel="handleFormCancel"
  >
    <CultureForm
      ref="formRef"
      :form-data="formData"
      :loading="formLoading"
    />
  </TaktModal>
</template>

<script setup lang="ts">
import { message, Modal } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { TableColumnsType } from 'ant-design-vue'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import CultureForm from './CultureForm.vue'
import {
  getCultureList,
  getCultureById,
  createCulture,
  updateCulture,
  deleteCultureById,
  deleteCultureBatch
} from '@/api/foundation/culture.api'
import type { Culture, CultureQuery, CultureCreate, CultureUpdate } from '@/types/foundation/culture.types'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

interface Props {
  /** 弹窗是否打开 */
  open?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  open: false
})

const emit = defineEmits<{
  'update:open': [value: boolean]
}>()

const { t } = useI18n()

/** 弹窗 open 双向绑定 */
const localVisible = computed({
  get: () => props.open,
  set: (value: boolean) => emit('update:open', value)
})

/** 列表 loading */
const loading = ref(false)
/** 查询关键字 */
const queryKeyword = ref('')
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总条数 */
const total = ref(0)
/** 列表数据 */
const dataSource = ref<Culture[]>([])
/** 选中行 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 选中行 */
const selectedRows = ref<Culture[]>([])
/** 单选行 */
const selectedRow = ref<Culture | null>(null)
/** 表单弹窗 */
const formVisible = ref(false)
/** 表单标题 */
const formTitle = ref(t('common.dialog.title.create', { entity: t('entity.culture._self') }))
/** 表单提交 loading */
const formLoading = ref(false)
/** 表单数据 */
const formData = ref<Culture | null>(null)
/** 表单 ref */
const formRef = ref<InstanceType<typeof CultureForm> | null>(null)
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false)
/** 高级查询表单 */
const advancedQueryForm = ref({
  cultureCode: '',
  nativeName: '',
  icon: '',
  sortOrder: undefined as number | undefined,
  isDefault: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: ''
})
/** 高级查询可见字段 */
const visibleQueryFieldKeys = ref<string[]>([])
/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'cultureCode', label: t('entity.culture.code') },
  { key: 'nativeName', label: t('entity.culture.nativename') },
  { key: 'icon', label: t('entity.culture.icon') },
  { key: 'sortOrder', label: t('entity.culture.sortorder') },
  { key: 'isDefault', label: t('entity.culture.isdefault') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }
])
/** 列设置抽屉 */
const columnSettingVisible = ref(false)
/** 可见列 keys */
const visibleColumnKeys = ref<string[]>([])
/** 是否浏览器全屏 */
const isFullscreen = ref(false)

/** 表格纵向滚动（弹窗视口 / 浏览器全屏自适应） */
const tableScroll = computed(() => ({
  y: isFullscreen.value ? 'calc(100vh - 220px)' : 'calc(85vh - 280px)'
}))

/**
 * 行主键
 * @param record 表格行
 * @returns {string} cultureId
 */
const getCultureRowKey = (record: unknown): string => {
  if (record == null || typeof record !== 'object') return ''
  const id = (record as Record<string, unknown>)['cultureId']
  return id != null && String(id) !== '' ? String(id) : ''
}

/**
 * 空高级查询表单
 * @returns {typeof advancedQueryForm.value} 初始表单
 */
function createEmptyAdvancedQueryForm() {
  return {
    cultureCode: '',
    nativeName: '',
    icon: '',
    sortOrder: undefined as number | undefined,
    isDefault: undefined as number | undefined,
    createdAtStart: '',
    createdAtEnd: '',
    extField: '',
    remark: ''
  }
}

/**
 * 构建分页查询参数
 * @returns {CultureQuery} 查询 DTO
 */
function buildCultureQuery(): CultureQuery {
  const form = advancedQueryForm.value
  const query: CultureQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CultureQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('cultureCode', form.cultureCode)
  assignTrimmed('nativeName', form.nativeName)
  assignTrimmed('icon', form.icon)
  if (form.sortOrder !== undefined && form.sortOrder !== null) {
    query.sortOrder = form.sortOrder
  }
  if (form.isDefault !== undefined && form.isDefault !== null) {
    query.isDefault = form.isDefault
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}

/** 表格列 */
const columns = computed<TableColumnsType<Culture>>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'cultureId',
    key: 'cultureId',
    width: 120,
    fixed: 'left'
  },
  {
    title: t('entity.culture.code'),
    dataIndex: 'cultureCode',
    key: 'cultureCode',
    width: 120
  },
  {
    title: t('entity.culture.nativename'),
    dataIndex: 'nativeName',
    key: 'nativeName',
    width: 150
  },
  {
    title: t('entity.culture.icon'),
    dataIndex: 'icon',
    key: 'icon',
    width: 160,
    ellipsis: true
  },
  {
    title: t('entity.culture.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 90
  },
  {
    title: t('entity.culture.isdefault'),
    dataIndex: 'isDefault',
    key: 'isDefault',
    width: 100
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 160,
    ellipsis: true
  },
  {
    title: t('common.page.entity.createdat'),
    dataIndex: 'createdAt',
    key: 'createdAt',
    width: 170,
    ellipsis: true
  },
  CreateActionColumn<Culture>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:i18n:update',
        onClick: (record: Culture) => handleEditOne(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:i18n:delete',
        onClick: (record: Culture) => handleDeleteOne(record)
      }
    ]
  })
])

/** 行选择 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Culture[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  }
}))

/**
 * 加载区域文化列表
 */
async function loadData() {
  try {
    loading.value = true
    const result = await getCultureList(buildCultureQuery())
    dataSource.value = result.data ?? []
    total.value = result.total ?? 0
  } catch (error) {
    logger.error('[Culture] 加载列表失败', undefined, error)
    message.error(t('common.feedback.load.failed', { target: t('entity.culture._self') }))
  } finally {
    loading.value = false
  }
}

/**
 * 弹窗打开时自动加载；关闭时重置查询与选中态（父级 v-model:open 变 true 不会触发 update:open）
 */
watch(
  () => props.open,
  (visible) => {
    if (visible) {
      queryKeyword.value = ''
      advancedQueryForm.value = createEmptyAdvancedQueryForm()
      currentPage.value = getTaktDefaultPageIndex()
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      void loadData()
    } else {
      if (document.fullscreenElement) {
        void document.exitFullscreen().catch(() => {})
      }
      isFullscreen.value = false
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      queryKeyword.value = ''
      advancedQueryForm.value = createEmptyAdvancedQueryForm()
    }
  }
)

const handleSearch = () => {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

const handleAdvancedQuery = () => {
  advancedQueryVisible.value = true
}

const handleAdvancedQuerySubmit = () => {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

const handleAdvancedQueryReset = () => {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

const handleColumnSetting = () => {
  columnSettingVisible.value = true
}

const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map((k) => String(k))
}

const handleColumnSettingReset = () => {
  visibleColumnKeys.value = []
}

/**
 * 浏览器全屏切换回调
 * @param value 是否全屏
 */
const handleFullscreen = (value: boolean) => {
  isFullscreen.value = value
}

const handleRefresh = () => {
  loadData()
}

const handlePaginationChange = (page: number, size: number) => {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

const handlePaginationSizeChange = (_current: number, size: number) => {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}

const handleCreate = () => {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.culture._self') })
  formData.value = null
  formVisible.value = true
}

const handleUpdate = async () => {
  if (!selectedRow.value?.cultureId) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.culture._self') }))
    return
  }
  await openEditForm(selectedRow.value.cultureId)
}

/**
 * 打开编辑表单（拉取详情含子表翻译）
 * @param cultureId 区域文化主键
 */
async function openEditForm(cultureId: string) {
  try {
    loading.value = true
    const detail = await getCultureById(cultureId)
    formTitle.value = t('common.dialog.title.edit', { entity: t('entity.culture._self') })
    formData.value = detail
    formVisible.value = true
  } catch (error) {
    logger.error('[Culture] 加载详情失败', undefined, error)
    message.error(t('common.feedback.load.failed', { target: t('entity.culture._self') }))
  } finally {
    loading.value = false
  }
}

const handleEditOne = async (record: Culture) => {
  if (!record.cultureId) {
    message.warning(t('common.validation.invalid', { field: t('common.page.entity.id') }))
    return
  }
  selectedRow.value = record
  await openEditForm(record.cultureId)
}

const handleDelete = () => {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.culture._self') }))
    return
  }
  const ids = selectedRows.value.map((row) => row.cultureId).filter(Boolean) as string[]
  if (ids.length === 0) {
    message.warning(t('common.validation.invalid', { field: t('common.page.entity.id') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.culture._self'),
      count: ids.length
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        if (ids.length === 1) {
          await deleteCultureById(ids[0]!)
        } else {
          await deleteCultureBatch(ids)
        }
        message.success(t('common.feedback.deleted'))
        await loadData()
        selectedRowKeys.value = []
        selectedRows.value = []
        selectedRow.value = null
      } catch (error) {
        logger.error('[Culture] 删除失败', undefined, error)
        message.error(t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleDeleteOne = (record: Culture) => {
  if (!record.cultureId) {
    message.warning(t('common.validation.invalid', { field: t('common.page.entity.id') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.culture._self'),
      count: 1
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteCultureById(record.cultureId)
        message.success(t('common.feedback.deleted'))
        await loadData()
        if (selectedRow.value?.cultureId === record.cultureId) {
          selectedRow.value = null
        }
        selectedRowKeys.value = selectedRowKeys.value.filter((k) => k !== record.cultureId)
        selectedRows.value = selectedRows.value.filter((r) => r.cultureId !== record.cultureId)
      } catch (error) {
        logger.error('[Culture] 删除失败', undefined, error)
        message.error(t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleFormSubmit = async () => {
  if (!formRef.value) return
  try {
    await formRef.value.validate()
    formLoading.value = true
    const payload = formRef.value.getFormData()
    if ('cultureId' in payload && payload.cultureId) {
      await updateCulture(payload.cultureId, payload as CultureUpdate)
      message.success(t('common.feedback.updated'))
    } else {
      await createCulture(payload as CultureCreate)
      message.success(t('common.feedback.created'))
    }
    formVisible.value = false
    formData.value = null
    await loadData()
  } catch (error: unknown) {
    const err = error as { errorFields?: unknown }
    if (err?.errorFields) {
      message.warning(t('common.feedback.failed'))
      return
    }
    logger.error('[Culture] 保存失败', undefined, error)
    message.error(t('common.feedback.failed'))
  } finally {
    formLoading.value = false
  }
}

const handleFormCancel = () => {
  formVisible.value = false
  formData.value = null
}
</script>

<style scoped lang="css">
.culture-table {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}
.culture-table-list-wrap {
  flex: 1;
  min-height: 0;
  min-width: 0;
}
</style>
