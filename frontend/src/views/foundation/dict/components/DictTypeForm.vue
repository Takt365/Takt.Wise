<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/routine/dict/type/components -->
<!-- 文件名称：DictTypeForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：字典类型表单组件，包含主表和子表（字典数据） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="dict-type-form">
    <a-tabs v-model:active-key="activeTab">
      <!-- 主表：字典类型信息 -->
      <a-tab-pane
        key="main"
        :tab="t('common.page.form.tabs.basicinfo')"
      >
        <a-form
          ref="mainFormRef"
          class="takt-generated-form"
          :model="mainFormState"
          :rules="mainFormRules"
          layout="horizontal"
          label-align="right"
        >
          <a-form-item
            :label="t('entity.dicttype.code')"
            name="dictTypeCode"
          >
            <a-input
              v-model:value="mainFormState.dictTypeCode"
              :maxlength="140"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dicttype.code') })"
              :disabled="!!props.formData?.dictTypeId"
            />
          </a-form-item>

          <a-form-item
            :label="t('entity.dicttype.name')"
            name="dictTypeName"
          >
            <a-input
              v-model:value="mainFormState.dictTypeName"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dicttype.name') })"
            />
          </a-form-item>

          <a-form-item
            :label="t('entity.dicttype.datasource')"
            name="dataSource"
          >
            <TaktSelect
              v-model:value="mainFormState.dataSource"
              dict-type="sys_data_source"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dicttype.datasource') })"
              allow-clear
            />
          </a-form-item>

          <a-form-item
            v-if="mainFormState.dataSource === 1"
            :label="t('entity.dicttype.dictscript')"
            name="dictScript"
          >
            <a-textarea
              v-model:value="mainFormState.dictScript"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dicttype.dictscript') })"
              :rows="4"
            />
          </a-form-item>

          <a-form-item
            :label="t('entity.dicttype.isbuiltin')"
            name="isBuiltIn"
          >
            <TaktSelect
              v-model:value="mainFormState.isBuiltIn"
              dict-type="sys_yes_no"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dicttype.isbuiltin') })"
              allow-clear
            />
          </a-form-item>

          <a-form-item
            :label="t('entity.dicttype.dictstatus')"
            name="dictStatus"
          >
            <TaktSelect
              v-model:value="mainFormState.dictStatus"
              dict-type="sys_normal_disable"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dicttype.dictstatus') })"
              allow-clear
            />
          </a-form-item>

          <a-form-item
            :label="t('common.page.entity.remark')"
            name="remark"
          >
            <a-textarea
              v-model:value="mainFormState.remark"
              :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
              :rows="3"
            />
          </a-form-item>
        </a-form>
      </a-tab-pane>

      <!-- 子表：字典数据列表 -->
      <a-tab-pane
        key="data"
        :tab="t('entity.dicttype.dictdatalist')"
      >
        <div class="dict-data-toolbar">
          <a-button
            type="primary"
            @click="handleAddDictData"
          >
            <template #icon>
              <PlusOutlined />
            </template>
            {{ t('common.page.button.createrow') }}
          </a-button>
        </div>

        <a-table
          :columns="dictDataColumns"
          :data-source="dictDataList"
          :pagination="false"
          row-key="dictDataId"
          size="small"
        >
          <template #bodyCell="{ column, record, index }">
            <!-- 字典类型ID - 只读 -->
            <template v-if="column.key === 'dictTypeId'">
              <span style="color: #999">{{ record.dictTypeId || '-' }}</span>
            </template>
            <!-- 字典类型编码 - 只读 -->
            <template v-else-if="column.key === 'dictTypeCode'">
              <span>{{ record.dictTypeCode }}</span>
            </template>
            <!-- 字典标签 - 可编辑 -->
            <template v-else-if="column.key === 'dictLabel'">
              <a-input
                v-if="editingKey === `${record.dictDataId || index}-dictLabel`"
                v-model:value="editingRecord.dictLabel"
                :maxlength="40"
                size="small"
                @blur="handleSaveCell(record, index, 'dictLabel')"
                @press-enter="handleSaveCell(record, index, 'dictLabel')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'dictLabel')"
              >
                {{ record.dictLabel || '-' }}
              </span>
            </template>
            <!-- 字典国际化键 - 可编辑 -->
            <template v-else-if="column.key === 'i18nKey'">
              <a-input
                v-if="editingKey === `${record.dictDataId || index}-i18nKey`"
                v-model:value="editingRecord.i18nKey"
                :maxlength="140"
                size="small"
                @blur="handleSaveCell(record, index, 'i18nKey')"
                @press-enter="handleSaveCell(record, index, 'i18nKey')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'i18nKey')"
              >
                {{ record.i18nKey || '-' }}
              </span>
            </template>
            <!-- 字典值 - 可编辑 -->
            <template v-else-if="column.key === 'dictValue'">
              <a-input
                v-if="editingKey === `${record.dictDataId || index}-dictValue`"
                v-model:value="editingRecord.dictValue"
                :maxlength="40"
                size="small"
                @blur="handleSaveCell(record, index, 'dictValue')"
                @press-enter="handleSaveCell(record, index, 'dictValue')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'dictValue')"
              >
                {{ record.dictValue || '-' }}
              </span>
            </template>
            <!-- CSS类名 - 可编辑 -->
            <template v-else-if="column.key === 'cssClass'">
              <a-input-number
                v-if="editingKey === `${record.dictDataId || index}-cssClass`"
                v-model:value="editingRecord.cssClass"
                :min="0"
                size="small"
                style="width: 100%"
                @blur="handleSaveCell(record, index, 'cssClass')"
                @press-enter="handleSaveCell(record, index, 'cssClass')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'cssClass')"
              >
                {{ record.cssClass ?? 0 }}
              </span>
            </template>
            <!-- 列表类名 - 可编辑 -->
            <template v-else-if="column.key === 'listClass'">
              <a-input-number
                v-if="editingKey === `${record.dictDataId || index}-listClass`"
                v-model:value="editingRecord.listClass"
                :min="0"
                size="small"
                style="width: 100%"
                @blur="handleSaveCell(record, index, 'listClass')"
                @press-enter="handleSaveCell(record, index, 'listClass')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'listClass')"
              >
                {{ record.listClass ?? 0 }}
              </span>
            </template>
            <!-- 扩展标签 - 可编辑 -->
            <template v-else-if="column.key === 'extLabel'">
              <a-input
                v-if="editingKey === `${record.dictDataId || index}-extLabel`"
                v-model:value="editingRecord.extLabel"
                :maxlength="140"
                size="small"
                @blur="handleSaveCell(record, index, 'extLabel')"
                @press-enter="handleSaveCell(record, index, 'extLabel')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'extLabel')"
              >
                {{ record.extLabel || '-' }}
              </span>
            </template>
            <!-- 扩展值 - 可编辑 -->
            <template v-else-if="column.key === 'extValue'">
              <a-input
                v-if="editingKey === `${record.dictDataId || index}-extValue`"
                v-model:value="editingRecord.extValue"
                :maxlength="140"
                size="small"
                @blur="handleSaveCell(record, index, 'extValue')"
                @press-enter="handleSaveCell(record, index, 'extValue')"
                @keydown.esc="handleCancelEdit"
              />
              <span
                v-else
                style="cursor: pointer; padding: 4px 8px; display: inline-block; min-height: 24px; width: 100%"
                @click="handleStartEdit(record, index, 'extValue')"
              >
                {{ record.extValue || '-' }}
              </span>
            </template>
            <!-- 操作列 -->
            <template v-else-if="column.key === 'action'">
              <a-button
                type="link"
                danger
                size="small"
                @click="handleRemoveDictData(index)"
              >
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { PlusOutlined } from '@ant-design/icons-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { TableColumnsType } from 'ant-design-vue'
import type { DictType, DictTypeCreate, DictTypeUpdate } from '@/types/foundation/dictType.types'
import type { DictData } from '@/types/foundation/dictData.types'

const { t } = useI18n()

/** 主表表单状态：`DictTypeCreate` 中 `dictScript`/`remark` 为可选时推断为 `string | undefined`，与 `a-textarea` 在 exactOptionalPropertyTypes 下要求的 `value: string | number` 不兼容，故在此收窄为必填 `string`（空串表示无内容）。 */
type DictTypeMainFormState = Omit<DictTypeCreate, 'dictScript' | 'remark'> & {
  dictScript: string
  remark: string
  dictTypeId?: string
}

/** 子表行内可编辑列（与模板 bodyCell 分支一致） */
type DictDataEditableField =
  | 'dictLabel'
  | 'i18nKey'
  | 'dictValue'
  | 'cssClass'
  | 'listClass'
  | 'extLabel'
  | 'extValue'

/** 子表行内编辑缓冲区：禁止 `Partial<DictData>`，否则 `v-model` 与 exactOptionalPropertyTypes 不兼容 */
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

function emptyDictDataInlineEditState(): DictDataInlineEditState {
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
  formData?: DictType | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false
})

// ========================================
// 数据定义
// ========================================

const activeTab = ref('main')
const mainFormRef = ref()
const dictDataList = ref<DictData[]>([])

// 行内编辑状态
const editingKey = ref<string>('')
const editingRecord = ref<DictDataInlineEditState>(emptyDictDataInlineEditState())
const editingIndex = ref<number>(-1)

const mainFormState = reactive<DictTypeMainFormState>({
  dictTypeCode: '',
  dictTypeName: '',
  dataSource: 0,
  dictScript: '',
  isBuiltIn: 1,
  dictStatus: 1,
  remark: ''
})

const mainFormRules = computed<Record<string, Rule[]>>(() => ({
  dictTypeCode: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dicttype.code') }), trigger: 'blur' }
  ],
  dictTypeName: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dicttype.name') }), trigger: 'blur' }
  ],
  dataSource: [
    { required: true, message: t('common.page.form.placeholder.select', { field: t('entity.dicttype.datasource') }), trigger: 'change' }
  ],
  dictScript: [
    {
      validator: (_rule, value) => {
        if (mainFormState.dataSource === 1 && !value) {
          return Promise.reject(t('common.page.form.placeholder.required', { field: t('entity.dicttype.dictscript') }))
        }
        return Promise.resolve()
      },
      trigger: 'blur'
    }
  ]
}))

// 字典数据子表列定义（与 DictData 接口字段顺序一致；列标题与后端 entity.dictdata 种子对齐）
const dictDataColumns = computed<TableColumnsType>(() => [
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
    title: t('common.action.operation'),
    key: 'action',
    width: 80,
    fixed: 'right'
  }
])

// ========================================
// 方法定义
// ========================================

// 监听 formData 变化
watch(
  () => props.formData,
  (newData) => {
    if (newData) {
      // 编辑模式：填充主表数据（按 DictType 接口字段顺序）
      Object.assign(mainFormState, {
        dictTypeId: newData.dictTypeId,
        dictTypeCode: newData.dictTypeCode || '',
        dictTypeName: newData.dictTypeName || '',
        dataSource: newData.dataSource ?? 0,
        dictScript: newData.dictScript || '',
        isBuiltIn: newData.isBuiltIn ?? 1,
        dictStatus: newData.dictStatus ?? 1,
        remark: newData.remark || ''
      })

      // 填充子表数据
      dictDataList.value = ((newData.dictDataList ?? []) as DictData[]).map(item => ({ ...item }))
    } else {
      // 新增模式：重置表单（按 DictType 接口字段顺序）
      Object.assign(mainFormState, {
        dictTypeId: undefined,
        dictTypeCode: '',
        dictTypeName: '',
        dataSource: 0,
        dictScript: '',
        isBuiltIn: 1,
        dictStatus: 1,
        remark: ''
      })
      dictDataList.value = []
    }
  },
  { immediate: true, deep: true }
)

// 新增字典数据（按 DictData 接口字段顺序）
const handleAddDictData = () => {
  dictDataList.value.push({
    dictDataId: `temp_${Date.now()}_${Math.random()}`,
    dictTypeId: '',
    dictTypeCode: mainFormState.dictTypeCode || '',
    cultureCode: 'mul',
    tenantCode: '',
    dictLabel: '',
    i18nKey: '',
    dictValue: '',
    cssClass: 0,
    listClass: 0,
    isDefault: 0,
    extLabel: '',
    extValue: ''
  } as DictData)
}

// 删除字典数据
const handleRemoveDictData = (index: number) => {
  dictDataList.value.splice(index, 1)
}

// 表单验证
const validate = async () => {
  await mainFormRef.value?.validate()
  
  // 验证子表数据
  for (let i = 0; i < dictDataList.value.length; i++) {
    const item = dictDataList.value[i]
    if (!item) continue
    if (!item.dictLabel || !item.dictValue) {
      throw new Error(
        t('common.data.invalid.row', {
          row: i + 1,
          reason: t('common.validation.required', { field: t('entity.dictdata.dictlabel') + t('common.tip.or') + t('entity.dictdata.dictvalue') })
        })
      )
    }
  }
}

// 获取表单数据（按 DictType 和 DictData 接口字段顺序）
const getFormData = (): DictTypeCreate | DictTypeUpdate => {
  const baseData: any = {
    dictTypeCode: mainFormState.dictTypeCode,
    dictTypeName: mainFormState.dictTypeName,
    dataSource: mainFormState.dataSource,
    dictScript: mainFormState.dictScript || undefined,
    isBuiltIn: mainFormState.isBuiltIn,
    dictStatus: mainFormState.dictStatus,
    remark: mainFormState.remark || undefined,
    dictDataList: dictDataList.value
      .filter(item => item.dictLabel && item.dictValue) // 过滤空数据
      .map(item => ({
        dictTypeId: item.dictTypeId || '',
        dictTypeCode: mainFormState.dictTypeCode,
        dictLabel: item.dictLabel,
        i18nKey: item.i18nKey || '',
        dictValue: item.dictValue,
        cssClass: item.cssClass ?? 0,
        listClass: item.listClass ?? 0,
        isDefault: item.isDefault ?? 0,
        extLabel: item.extLabel || undefined,
        extValue: item.extValue || undefined,
        remark: item.remark || undefined
      }))
  }
  
  if (mainFormState.dictTypeId) {
    baseData.dictTypeId = mainFormState.dictTypeId
  }
  
  return baseData
}

// ========================================
// 行内编辑方法
// ========================================

// 开始编辑单元格（表格 bodyCell 的 record 推断为 Record，此处收窄为 DictData）
const handleStartEdit = (record: Record<string, unknown>, index: number, field: DictDataEditableField) => {
  const row = record as unknown as DictData
  const key = `${row.dictDataId || index}-${field}`
  editingKey.value = key
  editingIndex.value = index
  editingRecord.value = dictDataToInlineEditState(row)
}

// 保存单元格（表单中的子表，直接更新本地数据，不需要调用API）
const handleSaveCell = (record: Record<string, unknown>, index: number, field: DictDataEditableField) => {
  const row = record as unknown as DictData
  const key = `${row.dictDataId || index}-${field}`
  if (editingKey.value !== key) return

  const newValue = editingRecord.value[field]

  // 验证必填字段
  if ((field === 'dictLabel' || field === 'dictValue') && !newValue) {
    handleCancelEdit()
    return
  }

  // 更新本地数据
  if (index >= 0 && index < dictDataList.value.length) {
    const cur = dictDataList.value[index]
    if (!cur) return
    dictDataList.value[index] = {
      ...cur,
      [field]: newValue
    } as DictData
  }

  // 清除编辑状态
  handleCancelEdit()
}

// 取消编辑
const handleCancelEdit = () => {
  editingKey.value = ''
  editingRecord.value = emptyDictDataInlineEditState()
  editingIndex.value = -1
}

// ========================================
// 暴露方法
// ========================================

defineExpose({
  validate,
  getFormData
})
</script>

<style scoped lang="css">
.dict-type-form {
  .dict-data-toolbar {
    margin-bottom: 16px;
  }
}
</style>
