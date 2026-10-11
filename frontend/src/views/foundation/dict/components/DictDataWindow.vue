<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/routine/dict/components -->
<!-- 文件名称：DictDataWindow.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：字典数据子表窗口组件，包含完整的查询、新增、编辑、删除、导出等功能 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="localVisible"
    :title="windowTitle"
    :width="1600"
    :footer="null"
    :centered="true"
    @update:open="handleVisibleChange"
  >
    <div
      v-if="dictType"
      class="dict-data-window"
    >
      <div style="margin-bottom: 16px; font-weight: bold; color: #1890ff">
        {{
          t('foundation.dict.page.data.window.dicttype.line', {
            name: dictType.dictTypeName,
            code: dictType.dictTypeCode
          })
        }}
      </div>

      <!-- 查询栏 -->
      <TaktQueryBar
        v-model="queryKeyword"
        :placeholder="t('common.page.form.placeholder.search', { keyword: t('entity.dictdata.dictlabel') + t('common.tip.or') + t('entity.dictdata.dictvalue') })"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />

      <!-- 工具栏 -->
      <TaktToolsBar
        create-permission="foundation:dict:create"
        update-permission="foundation:dict:update"
        delete-permission="foundation:dict:delete"
        export-permission="foundation:dict:export"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-export="true"
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
        :export-loading="loading"
        :refresh-loading="loading"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
        @export="handleExport"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
      />

      <!-- 表格 -->
      <TaktSingleTable
        entity-scope="tenant-culture"
        :columns="displayColumns"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="(record: any) => record.dictDataId || ''"
        :row-selection="rowSelection"
        :pagination="false"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 - 支持行内编辑 -->
        <template #bodyCell="{ column, record }">
          <!-- 字典类型编码 - 只读 -->
          <template v-if="column.key === 'dictTypeCode'">
            <span>{{ record.dictTypeCode }}</span>
          </template>
          <!-- 字典标签 - 可编辑 -->
          <template v-else-if="column.key === 'dictLabel'">
            <a-input
              v-if="editingKey === `${record.dictDataId}-dictLabel`"
              v-model:value="editingRecord.dictLabel"
              :maxlength="40"
              size="small"
              @blur="handleSaveCell(record, 'dictLabel')"
              @press-enter="handleSaveCell(record, 'dictLabel')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'dictLabel')"
            >
              {{ record.dictLabel || '-' }}
            </span>
          </template>
          <!-- 字典国际化键 - 可编辑 -->
          <template v-else-if="column.key === 'i18nKey'">
            <a-input
              v-if="editingKey === `${record.dictDataId}-i18nKey`"
              v-model:value="editingRecord.i18nKey"
              :maxlength="140"
              size="small"
              @blur="handleSaveCell(record, 'i18nKey')"
              @press-enter="handleSaveCell(record, 'i18nKey')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'i18nKey')"
            >
              {{ record.i18nKey || '-' }}
            </span>
          </template>
          <!-- 字典值 - 可编辑 -->
          <template v-else-if="column.key === 'dictValue'">
            <a-input
              v-if="editingKey === `${record.dictDataId}-dictValue`"
              v-model:value="editingRecord.dictValue"
              :maxlength="40"
              size="small"
              @blur="handleSaveCell(record, 'dictValue')"
              @press-enter="handleSaveCell(record, 'dictValue')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'dictValue')"
            >
              {{ record.dictValue || '-' }}
            </span>
          </template>
          <!-- CSS类名 - 可编辑 -->
          <template v-else-if="column.key === 'cssClass'">
            <a-input-number
              v-if="editingKey === `${record.dictDataId}-cssClass`"
              v-model:value="editingRecord.cssClass"
              :min="0"
              size="small"
              style="width: 100%"
              @blur="handleSaveCell(record, 'cssClass')"
              @press-enter="handleSaveCell(record, 'cssClass')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'cssClass')"
            >
              {{ record.cssClass ?? 0 }}
            </span>
          </template>
          <!-- 列表类名 - 可编辑 -->
          <template v-else-if="column.key === 'listClass'">
            <a-input-number
              v-if="editingKey === `${record.dictDataId}-listClass`"
              v-model:value="editingRecord.listClass"
              :min="0"
              size="small"
              style="width: 100%"
              @blur="handleSaveCell(record, 'listClass')"
              @press-enter="handleSaveCell(record, 'listClass')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'listClass')"
            >
              {{ record.listClass ?? 0 }}
            </span>
          </template>
          <!-- 扩展标签 - 可编辑 -->
          <template v-else-if="column.key === 'extLabel'">
            <a-input
              v-if="editingKey === `${record.dictDataId}-extLabel`"
              v-model:value="editingRecord.extLabel"
              :maxlength="140"
              size="small"
              @blur="handleSaveCell(record, 'extLabel')"
              @press-enter="handleSaveCell(record, 'extLabel')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'extLabel')"
            >
              {{ record.extLabel || '-' }}
            </span>
          </template>
          <!-- 扩展值 - 可编辑 -->
          <template v-else-if="column.key === 'extValue'">
            <a-input
              v-if="editingKey === `${record.dictDataId}-extValue`"
              v-model:value="editingRecord.extValue"
              :maxlength="140"
              size="small"
              @blur="handleSaveCell(record, 'extValue')"
              @press-enter="handleSaveCell(record, 'extValue')"
              @keydown.esc="handleCancelEdit"
            />
            <span
              v-else
              style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
              @click="handleStartEdit(record, 'extValue')"
            >
              {{ record.extValue || '-' }}
            </span>
          </template>
        </template>
      </TaktSingleTable>

      <!-- 分页组件 -->
      <TaktPagination
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        @change="handlePaginationChange"
        @show-size-change="handlePaginationSizeChange"
      />

      <!-- 新增/编辑对话框 -->
      <TaktModal
        v-model:open="formVisible"
        :title="formTitle"
        :width="800"
        :confirm-loading="formLoading"
        @ok="handleFormSubmit"
        @cancel="handleFormCancel"
      >
        <DictDataForm
          ref="formRef"
          :form-data="formData"
          :dict-type-code="dictType?.dictTypeCode || ''"
          :dict-type-id="dictType?.dictTypeId || ''"
          :loading="formLoading"
        />
      </TaktModal>

      <!-- 高级查询抽屉 -->
      <TaktQueryDrawer
        v-model:open="advancedQueryVisible"
        :form-model="advancedQueryForm"
        @submit="handleAdvancedQuerySubmit"
        @reset="handleAdvancedQueryReset"
      >
        <a-form-item :label="t('entity.dictdata.culturecode')">
          <TaktSelect v-model:value="advancedQueryForm.cultureCode" dict-type="sys_culture_code" allow-clear />
        </a-form-item>
        <a-form-item :label="t('entity.dictdata.dictlabel')">
          <a-input v-model:value="advancedQueryForm.dictLabel" />
        </a-form-item>
        <a-form-item :label="t('entity.dictdata.dictvalue')">
          <a-input v-model:value="advancedQueryForm.dictValue" />
        </a-form-item>
      </TaktQueryDrawer>

      <!-- 列设置抽屉 -->
      <TaktColumnDrawer
        v-model:open="columnDrawerVisible"
        :columns="mergedColumns"
        :checked-keys="visibleColumnKeys"
        :id-column-key="'dictDataId'"
        :action-column-key="'action'"
        entity-scope="tenant-culture"
        @update:checked-keys="handleColumnKeysChange"
        @reset="handleColumnSettingReset"
      />
    </div>
  </TaktModal>
</template>

<script setup lang="ts">
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { mergeDefaultColumns } from '@/utils/tableColumns'
import DictDataForm from './DictDataForm.vue'
import * as dictDataApi from '@/api/foundation/dictData.api'
import type { DictType } from '@/types/foundation/dictType.types'
import type { DictData, DictDataQuery, DictDataCreate, DictDataUpdate } from '@/types/foundation/dictData.types'
import { CreateActionColumn } from '@/components/business/TaktActionColumn'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'

const { t } = useI18n()

/** 高级查询抽屉：`DictDataQuery` 中 `dictLabel`/`dictValue` 为可选时推断为 `string | undefined`，与 `a-input` 在 exactOptionalPropertyTypes 下不兼容；表单内固定为 `string`（空串表示未填）。 */
type DictDataAdvancedQueryFormState = Omit<DictDataQuery, 'dictLabel' | 'dictValue' | 'cultureCode'> & {
  dictLabel: string
  dictValue: string
  cultureCode: string
}

/** 行内可编辑列（与模板 bodyCell 分支一致） */
type DictDataEditableField =
  | 'dictLabel'
  | 'i18nKey'
  | 'dictValue'
  | 'cssClass'
  | 'listClass'
  | 'extLabel'
  | 'extValue'

/** 行内编辑缓冲区：绑定 a-input / a-input-number 须为必填类型，不能用 `Partial<DictData>`（可选字段为 `T | undefined`） */
type DictDataInlineEditState = {
  dictLabel: string
  i18nKey: string
  dictValue: string
  cssClass: number
  listClass: number
  extLabel: string
  extValue: string
}

function dictDataToInlineEditState(r: DictData): DictDataInlineEditState {
  return {
    dictLabel: r.dictLabel ?? '',
    i18nKey: r.i18nKey ?? '',
    dictValue: r.dictValue ?? '',
    cssClass: r.cssClass ?? 0,
    listClass: r.listClass ?? 0,
    extLabel: r.extLabel ?? '',
    extValue: r.extValue ?? ''
  }
}

function emptyInlineEditState(): DictDataInlineEditState {
  return {
    dictLabel: '',
    i18nKey: '',
    dictValue: '',
    cssClass: 0,
    listClass: 0,
    extLabel: '',
    extValue: ''
  }
}

// ========================================
// Props & Emits
// ========================================

interface Props {
  visible?: boolean
  dictType?: DictType | null
}

const props = withDefaults(defineProps<Props>(), {
  visible: false,
  dictType: null
})

const emit = defineEmits<{
  'update:visible': [value: boolean]
}>()

// ========================================
// 数据定义
// ========================================

const localVisible = ref(props.visible)
const queryKeyword = ref('')
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const dataSource = ref<DictData[]>([])
const loading = ref(false)

// 行选择
const selectedRowKeys = ref<(string | number)[]>([])
const selectedRows = ref<DictData[]>([])
const selectedRow = ref<DictData | null>(null)

// 表单
const formVisible = ref(false)
const formTitle = ref('')
const formLoading = ref(false)
const formData = ref<DictData | null>(null)
const formRef = ref<InstanceType<typeof DictDataForm> | null>(null)

// 高级查询
const advancedQueryVisible = ref(false)
const advancedQueryForm = reactive<DictDataAdvancedQueryFormState>({
  pageIndex: 1,
  pageSize: 20,
  keyWords: '',
  dictTypeId: '',
  dictTypeCode: '',
  cultureCode: '',
  dictLabel: '',
  dictValue: ''
})

// 列设置
const visibleColumnKeys = ref<string[]>([])
const columnDrawerVisible = ref(false)

// 行内编辑
const editingKey = ref<string>('')
const editingRecord = ref<DictDataInlineEditState>(emptyInlineEditState())
const originalRecord = ref<Partial<DictData>>({})

// 窗口标题（组合文案走 routine 静态补全，后端未提供时可显示）
const windowTitle = computed(() => {
  if (!props.dictType) {
    return t('entity.dictdata._self')
  }
  return t('foundation.dict.page.data.window.title', {
    name: props.dictType.dictTypeName ?? '',
    code: props.dictType.dictTypeCode ?? ''
  })
})

// ========================================
// 列定义
// ========================================

// 字典数据子表列定义（与 DictData 接口字段顺序一致）
const columns = computed<TableColumnsType<DictData>>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'dictDataId',
    key: 'dictDataId',
    width: 120,
    fixed: 'left'
  },
  {
    title: t('entity.dictdata.dicttypeid'),
    dataIndex: 'dictTypeId',
    key: 'dictTypeId',
    width: 120
  },
  {
    title: t('entity.dictdata.dicttypecode'),
    dataIndex: 'dictTypeCode',
    key: 'dictTypeCode',
    width: 150
  },
  {
    title: t('entity.dictdata.culturecode'),
    dataIndex: 'cultureCode',
    key: 'cultureCode',
    width: 120
  },
  {
    title: t('entity.dictdata.dictlabel'),
    dataIndex: 'dictLabel',
    key: 'dictLabel',
    width: 150
  },
  {
    title: t('entity.dictdata.i18nkey'),
    dataIndex: 'i18nKey',
    key: 'i18nKey',
    width: 200,
    ellipsis: true
  },
  {
    title: t('entity.dictdata.dictvalue'),
    dataIndex: 'dictValue',
    key: 'dictValue',
    width: 150
  },
  {
    title: t('entity.dictdata.cssclass'),
    dataIndex: 'cssClass',
    key: 'cssClass',
    width: 100
  },
  {
    title: t('entity.dictdata.listclass'),
    dataIndex: 'listClass',
    key: 'listClass',
    width: 100
  },
  {
    title: t('entity.dictdata.extlabel'),
    dataIndex: 'extLabel',
    key: 'extLabel',
    width: 150,
    ellipsis: true
  },
  {
    title: t('entity.dictdata.extvalue'),
    dataIndex: 'extValue',
    key: 'extValue',
    width: 150,
    ellipsis: true
  },
  CreateActionColumn<DictData>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:dict:update',
        onClick: (record: DictData) => handleEditOne(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:dict:delete',
        onClick: (record: DictData) => handleDeleteOne(record)
      }
    ]
  })
])

// 合并列配置（包含审计字段）
const mergedColumns = computed((): any => {
  return mergeDefaultColumns(columns.value as any, t, true, 'tenant-culture')
})

// 根据可见列过滤显示的列
const displayColumns = computed((): any => {
  const keys = visibleColumnKeys.value || []
  const merged: any = mergedColumns.value || []
  
  if (keys.length === 0) {
    const cols: any = columns.value
    return cols
  }
  
  const getColumnKey = (col: any): string => {
    const key = col.key || col.dataIndex || col.title
    return key ? String(key) : ''
  }
  
  const keysSet = new Set(keys.map(k => String(k)))
  
  return merged.filter((col: any) => {
    const colKey = getColumnKey(col)
    return colKey && keysSet.has(colKey)
  })
})

// 行选择配置
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: DictData[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: DictData, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value?.dictDataId === record?.dictDataId) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: DictData[]) => {
    if (selected) {
      selectedRow.value =
        selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    } else {
      selectedRow.value = null
    }
  }
}))

// ========================================
// 方法定义
// ========================================

// 监听 visible 变化
watch(
  () => props.visible,
  (newVal) => {
    localVisible.value = newVal
    if (newVal && props.dictType) {
      // 窗口打开时初始化
      initWindow()
    }
  },
  { immediate: true }
)

// 处理 visible 变化
const handleVisibleChange = (value: boolean) => {
  localVisible.value = value
  emit('update:visible', value)
}

// 初始化窗口
const initWindow = () => {
  if (!props.dictType?.dictTypeId) return
  
  // 重置查询条件
  queryKeyword.value = ''
  currentPage.value = 1
  pageSize.value = 20
  Object.assign(advancedQueryForm, {
    pageIndex: 1,
    pageSize: 20,
    keyWords: '',
    dictTypeId: props.dictType.dictTypeId,
    dictTypeCode: props.dictType.dictTypeCode,
    dictLabel: '',
    dictValue: '',
    cultureCode: ''
  })
  
  // 重置选择状态
  selectedRowKeys.value = []
  selectedRows.value = []
  selectedRow.value = null
  
  // 加载数据
  loadData()
}

// 加载数据
const loadData = async () => {
  if (!props.dictType?.dictTypeId) return
  
  try {
    loading.value = true
    const query: DictDataQuery = {
      ...advancedQueryForm,
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value ?? '',
      dictTypeId: props.dictType.dictTypeId
    }
    
    const result = await dictDataApi.getDictDataList(query)
    dataSource.value = result.data || []
    total.value = result.total || 0
  } catch (error) {
    logger.error('[DictData] 加载数据失败', { action: 'loadData' }, error)
    message.error(t('common.feedback.load.failed', { target: t('entity.dictdata._self') }))
  } finally {
    loading.value = false
  }
}

// 搜索
const handleSearch = () => {
  currentPage.value = 1
  loadData()
}

// 重置
const handleReset = () => {
  queryKeyword.value = ''
  currentPage.value = 1
  Object.assign(advancedQueryForm, {
    keyWords: '',
    dictLabel: '',
    dictValue: '',
    cultureCode: ''
  })
  loadData()
}

// 新增
const handleCreate = () => {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.dictdata._self') })
  formData.value = null
  formVisible.value = true
}

// 编辑
const handleUpdate = () => {
  if (!selectedRow.value) {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.dictdata._self') })
    )
    return
  }
  
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.dictdata._self') })
  formData.value = { ...selectedRow.value }
  formVisible.value = true
}

// 编辑单条记录（操作列使用）
const handleEditOne = (record: DictData) => {
  selectedRow.value = record
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.dictdata._self') })
  formData.value = { ...record }
  formVisible.value = true
}

// 删除
const handleDelete = async () => {
  if (selectedRows.value.length === 0) {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.dictdata._self') })
    )
    return
  }
  
  const ids = selectedRows.value.map(row => row.dictDataId).filter(Boolean)
  if (ids.length === 0) {
    message.warning(t('common.validation.invalid', { field: t('common.page.entity.id') }))
    return
  }
  
  try {
    loading.value = true
    if (ids.length === 1) {
      const onlyId = ids[0]
      if (!onlyId) return
      await dictDataApi.deleteDictDataById(onlyId)
    } else {
      // 批量删除
      for (const id of ids) {
        await dictDataApi.deleteDictDataById(id)
      }
    }
    message.success(t('common.feedback.deleted'))
    await loadData()
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
  } catch (error) {
    logger.error('[DictData] 删除失败', { action: 'deleteBatch' }, error)
    message.error(t('common.feedback.delete.failed'))
  } finally {
    loading.value = false
  }
}

// 删除单条记录（操作列使用）
const handleDeleteOne = async (record: DictData) => {
  if (!record.dictDataId) {
    message.warning(t('common.validation.invalid', { field: t('common.page.entity.id') }))
    return
  }
  
  try {
    loading.value = true
    await dictDataApi.deleteDictDataById(record.dictDataId)
    message.success(t('common.feedback.deleted'))
    await loadData()
    if (selectedRow.value?.dictDataId === record.dictDataId) {
      selectedRow.value = null
    }
    selectedRowKeys.value = selectedRowKeys.value.filter(k => k !== record.dictDataId)
    selectedRows.value = selectedRows.value.filter(r => r.dictDataId !== record.dictDataId)
  } catch (error) {
    logger.error('[DictData] 删除失败', { action: 'deleteOne' }, error)
    message.error(t('common.feedback.delete.failed'))
  } finally {
    loading.value = false
  }
}

// 导出
const handleExport = async () => {
  if (!props.dictType?.dictTypeId) return
  
  try {
    loading.value = true
    const query: DictDataQuery = {
      ...advancedQueryForm,
      pageIndex: 1,
      pageSize: 10000,
      keyWords: queryKeyword.value ?? '',
      dictTypeId: props.dictType.dictTypeId
    }
    
    const blob = await dictDataApi.exportDictData(
      query,
      undefined,
      t('entity.dictdata._self')
    )
    const ts = new Date()
    const padNum = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `${t('entity.dictdata._self')}_${ts.getFullYear()}${padNum(ts.getMonth() + 1)}${padNum(ts.getDate())}${padNum(ts.getHours())}${padNum(ts.getMinutes())}${padNum(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    message.success(t('common.feedback.export.success'))
  } catch (error) {
    logger.error('[DictData] 导出失败', { action: 'export' }, error)
    message.error(t('common.feedback.export.failed'))
  } finally {
    loading.value = false
  }
}

// 高级查询
const handleAdvancedQuery = () => {
  advancedQueryVisible.value = true
}

const handleAdvancedQuerySubmit = () => {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

const handleAdvancedQueryReset = () => {
  Object.assign(advancedQueryForm, {
    keyWords: '',
    dictLabel: '',
    dictValue: '',
    cultureCode: ''
  })
}

// 列设置
const handleColumnSetting = () => {
  columnDrawerVisible.value = true
}

const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map(k => String(k))
}

const handleColumnSettingReset = () => {
  visibleColumnKeys.value = []
}

// 刷新
const handleRefresh = () => {
  loadData()
}

// 表单提交
const handleFormSubmit = async () => {
  if (!formRef.value) return
  
  try {
    await formRef.value.validate()
    formLoading.value = true
    
    const formDataValue = formRef.value.getFormData()
    if ('dictDataId' in formDataValue && formDataValue.dictDataId) {
      // 更新
      await dictDataApi.updateDictData(formDataValue.dictDataId, formDataValue as DictDataUpdate)
      message.success(t('common.feedback.updated'))
    } else {
      // 新增
      await dictDataApi.createDictData(formDataValue as DictDataCreate)
      message.success(t('common.feedback.created'))
    }
    
    formVisible.value = false
    await loadData()
  } catch (error: any) {
    if (error?.errorFields) {
      message.warning(t('common.feedback.failed'))
    } else {
      logger.error('[DictData] 保存失败', { action: 'saveForm' }, error)
      message.error(t('common.feedback.failed'))
    }
  } finally {
    formLoading.value = false
  }
}

// 表单取消
const handleFormCancel = () => {
  formVisible.value = false
  formData.value = null
}

// 表格变化
const handleTableChange = () => {
  // 处理表格变化
}

// 列调整
const handleResizeColumn = () => {
  // 处理列调整
}

// 分页变化
const handlePaginationChange = (page: number) => {
  currentPage.value = page
  loadData()
}

const handlePaginationSizeChange = (current: number, size: number) => {
  currentPage.value = current
  pageSize.value = size
  loadData()
}

// ========================================
// 行内编辑方法
// ========================================

// 开始编辑单元格
const handleStartEdit = (record: DictData, field: DictDataEditableField) => {
  const key = `${record.dictDataId}-${field}`
  editingKey.value = key
  editingRecord.value = dictDataToInlineEditState(record)
  originalRecord.value = {
    [field]: record[field]
  }
}

// 保存单元格
const handleSaveCell = async (record: DictData, field: DictDataEditableField) => {
  const key = `${record.dictDataId}-${field}`
  if (editingKey.value !== key) return
  
  const newValue = editingRecord.value[field]
  const oldValue = originalRecord.value[field]
  
  // 如果值没有变化，直接取消编辑
  if (newValue === oldValue) {
    handleCancelEdit()
    return
  }
  
  // 验证必填字段
  if ((field === 'dictLabel' || field === 'dictValue') && !newValue) {
    message.warning(
      field === 'dictLabel'
        ? t('common.validation.required', { field: t('entity.dictdata.dictlabel') })
        : t('common.validation.required', { field: t('entity.dictdata.dictvalue') })
    )
    handleCancelEdit()
    return
  }
  
  // 查找记录索引
  const index = dataSource.value.findIndex(item => item.dictDataId === record.dictDataId)
  if (index === -1) {
    handleCancelEdit()
    return
  }
  
  try {
    loading.value = true
    
    const rowAt = dataSource.value[index]
    if (!rowAt) {
      handleCancelEdit()
      return
    }

    // 更新本地数据
    dataSource.value[index] = {
      ...rowAt,
      [field]: newValue
    } as DictData

    // 调用API保存（可选字段用空串→undefined；exactOptionalPropertyTypes 下不对可选键赋显式 undefined）
    const strOrUndef = (v: string) => (v === '' ? undefined : v)
    const updateData = {
      dictDataId: record.dictDataId,
      dictTypeId: record.dictTypeId,
      dictTypeCode: record.dictTypeCode,
      dictLabel: field === 'dictLabel' ? (newValue as string) : record.dictLabel,
      i18nKey: field === 'i18nKey' ? (newValue as string) : record.i18nKey,
      isDefault: record.isDefault ?? 0,
      dictValue: field === 'dictValue' ? (newValue as string) : record.dictValue,
      cssClass: field === 'cssClass' ? (newValue as number) : record.cssClass,
      listClass: field === 'listClass' ? (newValue as number) : record.listClass,
      extLabel: field === 'extLabel' ? strOrUndef(newValue as string) : record.extLabel,
      extValue: field === 'extValue' ? strOrUndef(newValue as string) : record.extValue,
      sortOrder: record.sortOrder
    } as DictDataUpdate
    
    await dictDataApi.updateDictData(record.dictDataId, updateData)
    message.success(t('common.feedback.saved'))
    editingKey.value = ''
    editingRecord.value = emptyInlineEditState()
    originalRecord.value = {}
  } catch (error) {
    logger.error('[DictData] 保存失败', { action: 'saveCell' }, error)
    message.error(t('common.feedback.failed'))
    // 恢复原值
    const rollbackRow = dataSource.value[index]
    if (rollbackRow) {
      dataSource.value[index] = {
        ...rollbackRow,
        [field]: oldValue
      } as DictData
    }
    handleCancelEdit()
  } finally {
    loading.value = false
  }
}

// 取消编辑
const handleCancelEdit = () => {
  editingKey.value = ''
  editingRecord.value = emptyInlineEditState()
  originalRecord.value = {}
}
</script>

<style scoped lang="css">
.dict-data-window {
  padding: 0;
}
</style>
