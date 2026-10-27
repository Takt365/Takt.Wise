<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/routine/dict/type -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：字典类型管理页面，包含字典类型列表、查询、新增、编辑、删除等功能（主子表） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-dict-type">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="
        t('common.page.form.placeholder.search', {
          keyword:
            t('entity.dicttype.code') +
            t('common.tip.or') +
            t('entity.dicttype.name')
        })
      "
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
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
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
    <div class="routine-dict-type-table-wrap">
      <TaktSingleTable
        :scroll="tableScroll"
        entity-scope="tenant-core"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'id'"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getDictTypeId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      :expanded-row-keys="expandedRowKeys"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
      @expand="handleExpand"
    >
      <!-- 自定义列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'dictTypeCode'">
          <a
            style="color: #1890ff; cursor: pointer"
            @click.stop="handleOpenDictDataWindow(record)"
          >
            {{ record.dictTypeCode }}
          </a>
        </template>
        <template v-else-if="column.key === 'dictStatus'">
          <a-switch
            :checked="record.dictStatus === 1"
            :checked-children="t('common.page.button.enable')"
            :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'dataSource'">
          <TaktDictTag
            :value="getDictTypeField(record, 'dataSource')"
            dict-type="sys_data_source"
          />
        </template>
        <template v-else-if="column.key === 'isBuiltIn'">
          <a-switch
            :checked="getDictTypeField(record, 'isBuiltIn') === 1"
            :checked-children="t('common.status.yes')"
            :un-checked-children="t('common.status.no')"
            @change="(checked: unknown) => handleDictTypeBuiltInChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div style="padding: 16px">
          <a-table
            v-if="hasExpandedDictDataList(record.dictTypeId)"
            :columns="dictDataColumns"
            :data-source="getExpandedDictDataList(record.dictTypeId)"
            :row-key="(r: DictData) => r.dictDataId || ''"
            :pagination="false"
            size="small"
            bordered
          />
          <a-empty v-else />
        </div>
      </template>
    </TaktSingleTable>
    </div>

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
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <DictTypeForm
        ref="formRef"
        :form-data="formData"
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
      <a-form-item :label="t('entity.dicttype.code')">
        <a-input v-model:value="advancedQueryForm.dictTypeCode" />
      </a-form-item>
      <a-form-item :label="t('entity.dicttype.name')">
        <a-input v-model:value="advancedQueryForm.dictTypeName" />
      </a-form-item>
      <a-form-item :label="t('entity.dicttype.dictstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.dictStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dicttype.dictstatus') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <!-- 审计字段统一在 TaktColumnDrawer 中处理 -->
    <TaktColumnDrawer
      entity-scope="tenant-core"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'id'"
      :action-column-key="'action'"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

    <!-- 字典数据子表窗口 -->
    <DictDataWindow
      v-model:visible="dictDataWindowVisible"
      :dict-type="currentDictType"
    />
  </div>
</template>

<script setup lang="ts">
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import type { FilterValue } from 'ant-design-vue/es/table/interface'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import DictTypeForm from './components/DictTypeForm.vue'
import DictDataWindow from './components/DictDataWindow.vue'
import * as dictTypeApi from '@/api/foundation/dictType.api'
import * as dictDataApi from '@/api/foundation/dictData.api'
import type { DictType, DictTypeQuery, DictTypeCreate, DictTypeUpdate } from '@/types/foundation/dictType.types'
import type { DictData } from '@/types/foundation/dictData.types'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import { useUserStore } from '@/stores/identity/useUserStore'
import { isLogoutInProgress } from '@/bootstrap/taktLogoutFlow'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'

const { t } = useI18n()

/** 与 `identity/user/index.vue` 的 `getUserField` 一致：供表格单元格（如 TaktDictTag）安全取行字段 */
const getDictTypeField = (record: unknown, field: string): any =>
  (record as Record<string, unknown> | null | undefined)?.[field]

const getDictTypeId = (record: unknown): string =>
  String(getDictTypeField(record, 'dictTypeId') ?? '')

/** 与 `TaktSingleTable` 的 `@change` 签名一致（见 `takt-single-table/index.vue`） */
type TaktTableChangeSorter = {
  field?: string | number | readonly (string | number)[]
  order?: string
}
type TaktTableChangeFilters = Record<string, FilterValue | null>
type TaktTableChangePagination = { current?: number; pageSize?: number; total?: number }

/** 与 `TaktSingleTable` 的 `@resize-column` 第二参数一致（`ResizableColumn`） */
type TaktResizeColumn = { width?: string | number } & Record<string, unknown>

// ========================================
// 数据定义
// ========================================

const loading = ref(false)
const queryKeyword = ref('')
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const dataSource = ref<DictType[]>([])

/** 表格 scroll.y（服务端分页固定视口高度） */
const tableScroll = { y: 'calc(100vh - 300px)' } as const

/**
 * 按字典类型 id 取展开行嵌套子表数据
 * @param dictTypeId 字典类型主键
 * @returns {DictData[]} 字典数据行
 */
function getExpandedDictDataList(dictTypeId: string | undefined): DictData[] {
  if (!dictTypeId) return []
  const row = dataSource.value.find((item: DictType) => item.dictTypeId === dictTypeId)
  return row?.dictDataList ?? []
}

/**
 * 展开行嵌套子表是否有数据
 * @param dictTypeId 字典类型主键
 * @returns {boolean} 是否有字典数据
 */
function hasExpandedDictDataList(dictTypeId: string | undefined): boolean {
  return getExpandedDictDataList(dictTypeId).length > 0
}

// 行选择
const selecmedRowKeys = ref<(string | number)[]>([])
const selecmedRows = ref<DictType[]>([])
const selecmedRow = ref<DictType | null>(null)

const updateDisabled = computed(() => selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

// 表单
const formVisible = ref(false)
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
const formTitle = ref('')
const formLoading = ref(false)
const formData = ref<DictType | null>(null)
const formRef = ref<InstanceType<typeof DictTypeForm> | null>(null)

// 高级查询（字段与列表查询一致；`loadData` / `handleExport` 合并为 `DictTypeQuery`）
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref<{
  dictTypeCode: string
  dictTypeName: string
  dictStatus?: number
}>({
  dictTypeCode: '',
  dictTypeName: ''
})

// 列设置
const visibleColumnKeys = ref<string[]>([])
const columnSettingVisible = ref(false)

// 展开行
const expandedRowKeys = ref<(string | number)[]>([])

// 字典数据子表窗口
const dictDataWindowVisible = ref(false)
const currentDictType = ref<DictType | null>(null)

// 字典数据子表列定义（用于展开行显示，与 DictData 接口字段顺序一致）
const dictDataColumns = computed<TableColumnsType<DictData>>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'dictDataId',
    key: 'dictDataId',
    width: 120
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
  {
    title: t('entity.dictdata.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 100
  }
])

// ========================================
// 列定义
// ========================================

const columns = computed<TableColumnsType<DictType>>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'dictTypeId',
    key: 'id',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: DictType }) =>
      String(getDictTypeField(record, 'dictTypeId') ?? '')
  },
  {
    title: t('entity.dicttype.code'),
    dataIndex: 'dictTypeCode',
    key: 'dictTypeCode',
    width: 150,
    fixed: 'left'
  },
  {
    title: t('entity.dicttype.name'),
    dataIndex: 'dictTypeName',
    key: 'dictTypeName',
    width: 200
  },
  {
    title: t('entity.dicttype.datasource'),
    dataIndex: 'dataSource',
    key: 'dataSource',
    width: 100
  },
  {
    title: t('entity.dicttype.dictscript'),
    dataIndex: 'dictScript',
    key: 'dictScript',
    width: 200,
    ellipsis: true
  },
  {
    title: t('entity.dicttype.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 100
  },
  {
    title: t('entity.dicttype.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 100
  },
  {
    title: t('entity.dicttype.dictstatus'),
    dataIndex: 'dictStatus',
    key: 'dictStatus',
    width: 100
  },
  CreateActionColumn<DictType>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:dict:update',
        onClick: (record: DictType) => handleEditOne(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:dict:delete',
        onClick: (record: DictType) => handleDeleteOne(record)
      }
    ]
  })
])

// ========================================
// 方法定义
// ========================================

// 加载数据
const loadData = async () => {
  if (!useUserStore().isLoggedIn || isLogoutInProgress()) {
    return
  }
  try {
    loading.value = true
    const query: DictTypeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value
    }
    if (queryKeyword.value) query.keyWords = queryKeyword.value
    const adv = advancedQueryForm.value
    if (adv.dictTypeCode) query.dictTypeCode = adv.dictTypeCode
    if (adv.dictTypeName) query.dictTypeName = adv.dictTypeName
    if (adv.dictStatus !== undefined && adv.dictStatus !== null) query.dictStatus = adv.dictStatus

    const result = await dictTypeApi.getDictTypeList(query)
    dataSource.value = result.data || []
    total.value = result.total || 0
    
    // 字典数据按需加载（点击展开时加载），不再一次性加载所有数据
  } catch (error) {
    if (isLogoutInProgress() || !useUserStore().isLoggedIn) {
      return
    }
    logger.error('[DictType] 加载数据失败', { action: 'loadData' }, error)
    message.error(t('common.feedback.load.failed', { target: t('entity.dicttype._self') }))
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

// 重置
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = { dictTypeCode: '', dictTypeName: '' }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

// 新增
const handleCreate = () => {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.dicttype._self') })
  formData.value = null
  formVisible.value = true
}

// 编辑
const handleUpdate = () => {
  if (!selecmedRow.value) {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.dicttype._self') })
    )
    return
  }
  
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.dicttype._self') })
  formData.value = { ...selecmedRow.value }
  formVisible.value = true
}

// 编辑单条记录（操作列使用）
const handleEditOne = (record: DictType) => {
  selecmedRow.value = record
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.dicttype._self') })
  formData.value = { ...record }
  formVisible.value = true
}

// 删除
const handleDelete = () => {
  if (selecmedRows.value.length === 0) {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.dicttype._self') })
    )
    return
  }
  
  const ids = selecmedRows.value.map((row: DictType) => row.dictTypeId).filter(Boolean)
  if (ids.length === 0) {
    message.warning(t('common.validation.required', { field: t('common.page.entity.id') }))
    return
  }

  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.dicttype._self'),
      count: selecmedRows.value.length
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        if (ids.length === 1) {
          await dictTypeApi.deleteDictTypeById(ids[0]!)
        } else {
          await dictTypeApi.deleteDictTypeBatch(ids as string[])
        }
        message.success(t('common.feedback.deleted'))
        await loadData()
        selecmedRowKeys.value = []
        selecmedRows.value = []
        selecmedRow.value = null
      } catch (error) {
        logger.error('[DictType] 删除失败', { action: 'deleteBatch' }, error)
        message.error(t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

// 删除单条记录（操作列使用）
const handleDeleteOne = (record: DictType) => {
  if (!record.dictTypeId) {
    message.warning(t('common.validation.required', { field: t('common.page.entity.id') }))
    return
  }

  const name = record.dictTypeCode || t('common.tip.this.target', { target: t('entity.dicttype._self') })
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.dicttype._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await dictTypeApi.deleteDictTypeById(record.dictTypeId)
        message.success(t('common.feedback.deleted'))
        await loadData()
        if (selecmedRow.value?.dictTypeId === record.dictTypeId) {
          selecmedRow.value = null
        }
        selecmedRowKeys.value = selecmedRowKeys.value.filter(k => k !== record.dictTypeId)
        selecmedRows.value = selecmedRows.value.filter((r: DictType) => r.dictTypeId !== record.dictTypeId)
      } catch (error) {
        logger.error('[DictType] 删除失败', { action: 'deleteOne' }, error)
        message.error(t('common.feedback.delete.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

// 导出
const handleExport = async () => {
  try {
    loading.value = true
    const query: DictTypeQuery = {
      pageIndex: 1,
      pageSize: 10000
    }
    if (queryKeyword.value) query.keyWords = queryKeyword.value
    const adv = advancedQueryForm.value
    if (adv.dictTypeCode) query.dictTypeCode = adv.dictTypeCode
    if (adv.dictTypeName) query.dictTypeName = adv.dictTypeName
    if (adv.dictStatus !== undefined && adv.dictStatus !== null) query.dictStatus = adv.dictStatus

    const blob = await dictTypeApi.exportDictType(query, undefined, t('entity.dicttype._self'))
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `${t('entity.dicttype._self')}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    message.success(t('common.feedback.export.success', { target: t('entity.dicttype._self') }))
  } catch (error) {
    logger.error('[DictType] 导出失败', { action: 'export' }, error)
    message.error(t('common.feedback.export.failed', { target: t('entity.dicttype._self') }))
  } finally {
    loading.value = false
  }
}

// 状态切换
const handleStatusChange = async (record: DictType, checked: boolean) => {
  try {
    await dictTypeApi.updateDictTypeStatus({
      dictTypeId: record.dictTypeId,
      dictStatus: checked ? 1 : 0
    })
    message.success(t('common.feedback.updated', { target: t('entity.dicttype._self') }))
    await loadData()
  } catch (error) {
    logger.error('[DictType] 状态更新失败', { action: 'updateStatus' }, error)
    message.error(t('common.feedback.failed'))
  }
}

/**
 * 表格行内切换内置（sys_yes_no：1=是，0=否）
 * @param record 当前行
 * @param checked 开关是否选中（内置）
 */
async function handleDictTypeBuiltInChange(record: DictType, checked: boolean) {
  const id = getDictTypeId(record)
  if (!id) {
    return
  }
  const newBuiltIn = checked ? 1 : 0
  const oldBuiltIn = getDictTypeField(record, 'isBuiltIn')
  const row = dataSource.value.find((item) => getDictTypeId(item) === id)
  if (row) {
    row.isBuiltIn = newBuiltIn
  }
  try {
    await dictTypeApi.updateDictTypeBuiltIn({ dictTypeId: id, isBuiltIn: newBuiltIn })
    message.success(t('common.feedback.updated', { target: t('entity.dicttype._self') }))
  } catch (error: unknown) {
    if (row) {
      row.isBuiltIn = oldBuiltIn as number
    }
    const err = error as { message?: string }
    logger.error('[DictType] 内置标识更新失败', { action: 'updateBuiltIn' }, error)
    message.error(err?.message || t('common.feedback.failed'))
  }
}

// 高级查询
const handleAdvancedQuery = () => {
  advancedQueryVisible.value = true
}

const handleAdvancedQuerySubmit = () => {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

const handleAdvancedQueryReset = () => {
  advancedQueryForm.value = { dictTypeCode: '', dictTypeName: '' }
}

// 列设置
const handleColumnSetting = () => {
  columnSettingVisible.value = true
}

// 列设置变化 - TaktColumnDrawer 传递选中的 keys，更新 visibleColumnKeys
const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map(k => String(k))
}

// 列设置重置：TaktColumnDrawer 会自动重置为默认值
const handleColumnSettingReset = () => {
  // TaktColumnDrawer 组件内部会自动处理重置逻辑
  // 这里只需要清空，让组件使用默认值
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
    
    const formData = formRef.value.getFormData()
    if ('dictTypeId' in formData && formData.dictTypeId) {
      // 更新
      await dictTypeApi.updateDictType(formData.dictTypeId, formData as DictTypeUpdate)
      message.success(t('common.feedback.updated', { target: t('entity.dicttype._self') }))
    } else {
      // 新增
      await dictTypeApi.createDictType(formData as DictTypeCreate)
      message.success(t('common.feedback.created', { target: t('entity.dicttype._self') }))
    }
    
    formVisible.value = false
    await loadData()
  } catch (error: any) {
    if (error?.errorFields) {
      message.warning(t('common.feedback.failed'))
    } else {
      logger.error('[DictType] 保存失败', { action: 'save' }, error)
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

// 表格变化（分页由 `TaktPagination` 处理；签名须与 `TaktSingleTable` 的 `change` 一致）
const handleTableChange = (
  _pagination: TaktTableChangePagination,
  _filters: TaktTableChangeFilters,
  sorter: TaktTableChangeSorter | TaktTableChangeSorter[]
) => {
  const one = Array.isArray(sorter) ? sorter[0] : sorter
  if (one?.order) logger.debug('[DictType] 排序', { field: one.field, order: one.order })
}

// 列宽拖拽（与 `TaktSingleTable` 的 `resize-column` 及 `identity/user` 页一致：`col`/`c` 用 `any` 避免列 `key: Key` 与窄对象在 EOPT 下不兼容）
const handleResizeColumn = (w: number, col: TaktResizeColumn) => {
  const colAny = col as Record<string, unknown>
  const colKey = colAny.key ?? colAny.dataIndex ?? colAny.title
  const column = columns.value.find((c: any) => {
    const cKey = c.key ?? c.dataIndex ?? c.title
    return colKey != null && cKey != null && String(colKey) === String(cKey)
  }) as { width?: number } | undefined
  if (column) column.width = w
}

// 行选择配置
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: DictType[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: DictType, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value?.dictTypeId === record?.dictTypeId) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: DictType[]) => {
    if (selecmed) {
      selecmedRow.value = selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    } else {
      selecmedRow.value = null
    }
  }
}))

// 行点击处理 - 切换展开状态（手风琴模式：只允许一个展开，保留行选择功能）
const onClickRow = (record: DictType) => {
  return {
    onClick: (event: MouseEvent) => {
      // 如果点击的是复选框或操作列，不处理展开
      const target = event.target as HTMLElement
      if (target.closest('.ant-checkbox-wrapper') || target.closest('.takt-action-column')) {
        return
      }
      
      const key = record.dictTypeId || ''
      // 手风琴模式：切换展开状态
      if (expandedRowKeys.value.includes(key)) {
        // 如果当前行已展开，则收起
        expandedRowKeys.value = []
      } else {
        // 如果当前行未展开，先关闭其他已展开的行，再展开当前行
        expandedRowKeys.value = []
        
        // 确保字典数据已加载，等待加载完成后再展开
        const item = dataSource.value.find((row: DictType) => row.dictTypeId === record.dictTypeId)
        if (item && (!item.dictDataList || item.dictDataList.length === 0)) {
          // 先加载数据，等待完成后再展开
          loadDictData(record).then(() => {
            expandedRowKeys.value = [key]
          })
        } else {
          expandedRowKeys.value = [key]
        }
      }
    }
  }
}

/**
 * 展开/收起处理（手风琴模式：只允许一个展开）
 * @param expanded 是否展开
 * @param record TaktSingleTable 行数据（TableRecord）
 */
const handleExpand = async (expanded: boolean, record: Record<string, unknown>) => {
  const row = record as unknown as DictType
  if (expanded && row.dictTypeId) {
    // 手风琴模式：先关闭其他已展开的行
    const currentKey = row.dictTypeId || ''
    if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== currentKey) {
      expandedRowKeys.value = []
    }

    // 检查 dataSource 中是否有数据，如果没有则加载
    const item = dataSource.value.find((r: DictType) => r.dictTypeId === row.dictTypeId)
    if (item && (!item.dictDataList || item.dictDataList.length === 0)) {
      await loadDictData(row)
    }

    // 设置当前行为唯一展开的行
    expandedRowKeys.value = [currentKey]
  } else {
    // 收起时清空
    expandedRowKeys.value = []
  }
}

// 加载字典数据 - 根据 dictTypeId 动态获取
const loadDictData = async (record: DictType) => {
  if (!record.dictTypeId) return
  
  try {
    // 使用 dictDataApi.getList 根据 dictTypeId 查询字典数据（dictTypeId 是唯一标识）
    const result = await dictDataApi.getDictDataList({
      pageIndex: 1,
      pageSize: 10000, // 获取所有数据
      dictTypeId: record.dictTypeId
    })
    
    if (result && result.data) {
      // 更新 dataSource 中对应的记录，确保响应式更新
      const index = dataSource.value.findIndex((row: DictType) => row.dictTypeId === record.dictTypeId)
      if (index !== -1) {
        const row = dataSource.value[index]
        // 仅替换 dictDataList；展开推断在 EOPT 下会变「部分字段可选」，与 DictType 必选冲突，故断言为 DictType
        dataSource.value[index] = { ...row, dictDataList: result.data } as DictType
      }
      return result.data
    }
    return []
  } catch (error) {
    logger.error('[DictType] 加载字典数据失败', { action: 'loadDictData' }, error)
    message.error(t('common.feedback.load.data.failed'))
    return []
  }
}

// 打开字典数据子表窗口
const handleOpenDictDataWindow = async (record: DictType) => {
  if (!record.dictTypeId) {
    message.warning(t('common.validation.required', { field: t('common.page.entity.id') }))
    return
  }
  
  currentDictType.value = record
  dictDataWindowVisible.value = true
}




// 分页变化
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

// ========================================
// 生命周期
// ========================================

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})
</script>

<style scoped lang="css">
.routine-dict-type {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}
.routine-dict-type-table-wrap {
  flex: 1;
  min-height: 0;
}
</style>
