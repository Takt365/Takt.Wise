<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/code/generator/components -->
<!-- 文件名称：ImportTable.vue -->
<!-- 功能描述：代码生成「从数据库导入表」弹窗内嵌表单；选择租户业务库（tenantCode）与物理表后提交，由父页 index.vue 调用 importTableFromDatabase；defineExpose 提供 reset。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    :label-col="{ span: 6 }"
    :wrapper-col="{ span: 16 }"
  >
    <!-- 数据源：租户业务库 -->
    <a-form-item :label="t('entity.gentable.datasource')">
      <a-select
        v-model:value="tenantCode"
        :placeholder="t('common.page.form.placeholder.select', { field: t('entity.gentable.datasource') })"
        allow-clear
        style="width: 100%"
        @change="handleConfigChange"
      >
        <a-select-option
          v-for="c in databaseInfoList"
          :key="c.tenantCode"
          :value="c.tenantCode"
        >
          {{ c.displayName }} ({{ c.tenantCode }})
        </a-select-option>
      </a-select>
    </a-form-item>
    <!-- 物理表 -->
    <a-form-item :label="t('code.generator.page.importtable.datatable')">
      <a-select
        v-model:value="tableName"
        :placeholder="t('common.page.form.placeholder.selectfirst', { field: t('entity.gentable.datasource') })"
        :disabled="!tenantCode"
        :loading="databaseTablesLoading"
        allow-clear
        style="width: 100%"
      >
        <a-select-option
          v-for="tbl in databaseTables"
          :key="tbl.tableName"
          :value="tbl.tableName"
        >
          {{ tbl.tableName }} {{ tbl.tableComment ? `- ${tbl.tableComment}` : '' }}
        </a-select-option>
      </a-select>
    </a-form-item>
    <!-- 提交 -->
    <a-form-item :wrapper-col="{ offset: 6, span: 16 }">
      <a-button
        type="primary"
        :loading="importLoading"
        :disabled="!tenantCode || !tableName"
        @click="handleSubmit"
      >
        {{ t('common.page.button.import') }}
      </a-button>
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 从数据库导入表（有表导入）子表单。
 * 父组件传入 databaseInfoList / databaseTables；切换租户时 emit config-change 由父级拉表列表；提交 emit submit(tenantCode, tableName)。
 */
import { useI18n } from 'vue-i18n'
import type { DatabaseInfo, DatabaseTableInfo } from '@/types/code/database/databaseInfo.types'

/** i18n 翻译函数 */
const { t } = useI18n()

/** 组件入参（由 index.vue 传入） */
const props = withDefaults(
  defineProps<{
    /** 弹窗是否打开（打开时 reset 表单） */
    open?: boolean
    /** 可 introspect 的租户业务库列表（TaktDatabaseInfos） */
    databaseInfoList: DatabaseInfo[]
    /** 当前租户库下可选物理表（父级按 tenantCode 加载） */
    databaseTables: DatabaseTableInfo[]
    /** 表列表加载中 */
    databaseTablesLoading?: boolean
    /** 导入请求进行中 */
    importLoading?: boolean
  }>(),
  { open: false, databaseTablesLoading: false, importLoading: false }
)

/** 向父组件抛出的事件 */
const emit = defineEmits<{
  /** 租户编码变更，父级据此请求 getDatabaseTableInfoList */
  'config-change': [tenantCode: string]
  /** 确认导入：租户编码 + 表名 */
  'submit': [payload: { tenantCode: string; tableName: string }]
}>()

/** 选中的租户编码（3 位） */
const tenantCode = ref<string | undefined>()
/** 选中的物理表名 */
const tableName = ref<string | undefined>()

/** 弹窗打开时重置表单 */
watch(
  () => props.open,
  (isOpen) => {
    if (isOpen) reset()
  }
)

/**
 * 切换数据源：清空表名并通知父级加载表列表
 */
function handleConfigChange() {
  tableName.value = undefined
  if (tenantCode.value) emit('config-change', tenantCode.value)
}

/**
 * 提交导入：校验 tenantCode、tableName 后向父组件抛出 submit
 */
function handleSubmit() {
  if (!tenantCode.value || !tableName.value) return
  emit('submit', { tenantCode: tenantCode.value, tableName: tableName.value })
}

/**
 * 重置表单（弹窗打开或父组件通过 ref 调用）
 */
function reset() {
  tenantCode.value = undefined
  tableName.value = undefined
}

/** 暴露给父组件：reset */
defineExpose({ reset })
</script>
