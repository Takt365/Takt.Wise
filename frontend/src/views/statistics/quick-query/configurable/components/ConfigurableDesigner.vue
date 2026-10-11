<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/quick-query/configurable/components -->
<!-- 文件名称：ConfigurableDesigner.vue -->
<!-- 功能描述：定制报表全栈设计器（主表+六子表字段、分步向导、validate/getValues） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
    class="configurable-designer-form takt-generated-form"
  >
    <div class="flex flex-col gap-4">
    <a-steps
      :current="currentStepIndex"
      size="small"
      class="mb-2"
      @change="handleStepChange"
    >
      <a-step
        v-for="step in wizardSteps"
        :key="step.id"
        :title="step.title"
      />
    </a-steps>
    <!-- 步骤：基本信息（单表模式含选表；表连接模式仅选类型） -->
    <section v-show="currentStepId === 'basic'" class="rounded border border-border bg-container">
      <div class="p-4">
        <a-row :gutter="24">
          <a-col :span="12">
            <a-form-item :label="t('common.page.entity.tenantcode')" name="tenantCode">
              <a-input v-model:value="formState.tenantCode" size="small" disabled />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('common.page.entity.companycode')" name="companyCode">
              <a-input v-model:value="formState.companyCode" size="small" disabled />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('common.page.entity.plantcode')" name="plantCode">
              <a-input v-model:value="formState.plantCode" size="small" disabled />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('common.page.entity.culturecode')" name="cultureCode">
              <a-input v-model:value="formState.cultureCode" size="small" disabled />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item
              v-if="!formData?.configurableId"
              :label="t('common.page.form.numberingRule')"
              name="numberingRuleCode"
            >
              <TaktSelect
                v-model:value="formState.numberingRuleCode"
                api-url="TaktNumberings/options"
                :api-params="{ documentType: '定制报表' }"
                :placeholder="t('common.page.form.placeholder.selectonly')"
                size="small"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.code')" name="configurableCode">
              <a-input
                v-model:value="formState.configurableCode"
                size="small"
                :placeholder="t('common.page.form.numberingCodePreview')"
                :disabled="!!formData?.configurableId"
                readonly
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.name')" name="configurableName">
              <a-input v-model:value="formState.configurableName" size="small" allow-clear />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.domain')" name="configurableDomain">
              <a-select
                v-model:value="formState.configurableDomain"
                :options="moduleOptions"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.domain') })"
                size="small"
                allow-clear
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.subcategory')" name="configurableSubCategory">
              <a-select
                v-model:value="formState.configurableSubCategory"
                :options="subCategoryOptions"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.subcategory') })"
                :disabled="!formState.configurableDomain || subCategoryOptions.length === 0"
                size="small"
                allow-clear
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.distinctrows')" name="distinctRows">
              <TaktSelect
                v-model:value="formState.distinctRows"
                dict-type="sys_yes_no"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.distinctrows') })"
                size="small"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.maxexportrows')" name="maxExportRows">
              <a-input-number v-model:value="formState.maxExportRows" :min="1" :max="50000" size="small" class="w-full" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.maxqueryrows')" name="maxQueryRows">
              <a-input-number v-model:value="formState.maxQueryRows" :min="1" :max="50000" size="small" class="w-full" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.ispublic')" name="isPublic">
              <TaktSelect
                v-model:value="formState.isPublic"
                dict-type="sys_public_type"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.ispublic') })"
                size="small"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurable.status')" name="configurableStatus">
              <TaktSelect
                v-model:value="formState.configurableStatus"
                dict-type="sys_normal_disable"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.status') })"
                size="small"
              />
            </a-form-item>
          </a-col>
          <a-col :span="24">
            <a-form-item :label="t('common.page.entity.remark')" name="remark">
              <a-textarea v-model:value="formState.remark" :rows="2" size="small" />
            </a-form-item>
          </a-col>
        </a-row>
        <div class="mt-4 border-t border-border pt-4">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item :label="t('statistics.quickquery.configurable.page.designer.section.datasource')">
                <div class="flex flex-col gap-1">
                  <a-select
                    v-model:value="sourceType"
                    size="small"
                    class="w-48"
                    :disabled="sourceTypeLocked"
                    :options="sourceTypeOptions"
                    @change="handleSourceTypeChange"
                  />
                  <p v-if="sourceTypeLocked" class="text-xs text-text-secondary">
                    {{ t('statistics.quickquery.configurable.page.designer.sourcetype.locked') }}
                  </p>
                </div>
              </a-form-item>
            </a-col>
          </a-row>
          <!-- 单表：基本信息步选表；表连接：仅选类型，下一步进入表连接设计 -->
          <template v-if="isSingleTableMode">
            <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item :label="t('statistics.quickquery.configurable.page.field.tenant')">
                  <a-select
                    v-model:value="primaryCatalogTenant"
                    :loading="databaseInfoLoading"
                    :disabled="sourceTypeLocked"
                    show-search
                    option-filter-prop="label"
                    size="small"
                    class="w-full"
                    @change="handlePrimaryTenantChange"
                  >
                    <a-select-option
                      v-for="item in databaseInfoList"
                      :key="item.tenantCode"
                      :value="item.tenantCode"
                      :label="`${item.displayName} (${item.tenantCode})`"
                    >
                      {{ item.displayName }} ({{ item.tenantCode }})
                    </a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col :span="12">
                <a-form-item :label="t('statistics.quickquery.configurable.page.field.database')">
                  <span class="text-sm text-text">{{ resolveDatabaseDisplayName(primaryCatalogTenant) }}</span>
                </a-form-item>
              </a-col>
            </a-row>

            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('statistics.quickquery.configurable.page.designer.tableview')">
                  <a-select
                    v-model:value="primaryTableName"
                    :loading="isTablesLoading(primaryCatalogTenant)"
                    :disabled="sourceTypeLocked"
                    show-search
                    option-filter-prop="label"
                    allow-clear
                    size="small"
                    class="w-full"
                    :placeholder="t('common.page.form.placeholder.select', { field: t('statistics.quickquery.configurable.page.designer.tableview') })"
                    @change="handlePrimaryTableChange"
                  >
                    <a-select-option
                      v-for="tbl in primaryTableOptions"
                      :key="tbl.tableName"
                      :value="tbl.tableName"
                      :label="tbl.tableName"
                    >
                      {{ tbl.tableName }}{{ tbl.tableComment ? ` - ${tbl.tableComment}` : '' }}
                    </a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
            </a-row>
          </template>
        </div>
      </div>
    </section>
    <!-- 步骤：表连接设计（仅表连接模式） -->
    <section v-show="currentStepId === 'join'" class="rounded border border-border bg-container">
      <div class="p-4">
        <a-row :gutter="24">
          <a-col :span="12">
            <a-form-item :label="t('statistics.quickquery.configurable.page.field.tenant')">
              <a-select
                v-model:value="primaryCatalogTenant"
                :loading="databaseInfoLoading"
                show-search
                option-filter-prop="label"
                size="small"
                class="w-full"
                @change="handlePrimaryTenantChange"
              >
                <a-select-option
                  v-for="item in databaseInfoList"
                  :key="item.tenantCode"
                  :value="item.tenantCode"
                  :label="`${item.displayName} (${item.tenantCode})`"
                >
                  {{ item.displayName }} ({{ item.tenantCode }})
                </a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('statistics.quickquery.configurable.page.field.database')">
              <span class="text-sm text-text">{{ resolveDatabaseDisplayName(primaryCatalogTenant) }}</span>
            </a-form-item>
          </a-col>
        </a-row>
        <div class="mt-2 text-sm font-medium text-text">
          {{ t('statistics.quickquery.configurable.page.designer.section.tabledata') }}
        </div>
        <a-row :gutter="24">
          <a-col :span="12">
            <a-form-item :label="t('statistics.quickquery.configurable.page.designer.join.primarytable')">
              <div class="flex flex-wrap items-center gap-2">
                <a-select
                  v-model:value="primaryTableName"
                  :loading="isTablesLoading(primaryCatalogTenant)"
                  show-search
                  option-filter-prop="label"
                  allow-clear
                  size="small"
                  class="min-w-0 flex-1"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('statistics.quickquery.configurable.page.designer.tableview') })"
                  @change="handlePrimaryTableChange"
                >
                  <a-select-option
                    v-for="tbl in primaryTableOptions"
                    :key="tbl.tableName"
                    :value="tbl.tableName"
                    :label="tbl.tableName"
                  >
                    {{ tbl.tableName }}{{ tbl.tableComment ? ` - ${tbl.tableComment}` : '' }}
                  </a-select-option>
                </a-select>
                <span class="shrink-0 text-sm text-text-secondary">{{ t('entity.configurablesource.sourcealias') }}</span>
                <a-input v-model:value="primaryAlias" size="small" class="w-16" allow-clear />
              </div>
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('statistics.quickquery.configurable.page.designer.join.jointable')">
              <div class="flex flex-wrap items-center gap-2">
                <a-select
                  v-model:value="joinTableName"
                  :loading="isTablesLoading(primaryCatalogTenant)"
                  show-search
                  option-filter-prop="label"
                  allow-clear
                  size="small"
                  class="min-w-0 flex-1"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('statistics.quickquery.configurable.page.designer.join.jointable') })"
                  @change="handleJoinTableChange"
                >
                  <a-select-option
                    v-for="tbl in primaryTableOptions"
                    :key="tbl.tableName"
                    :value="tbl.tableName"
                    :label="tbl.tableName"
                  >
                    {{ tbl.tableName }}{{ tbl.tableComment ? ` - ${tbl.tableComment}` : '' }}
                  </a-select-option>
                </a-select>
                <span class="shrink-0 text-sm text-text-secondary">{{ t('entity.configurablesource.sourcealias') }}</span>
                <a-input v-model:value="joinTableAlias" size="small" class="w-16" allow-clear />
              </div>
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item :label="t('entity.configurablejoin.jointype')">
              <a-select
                v-model:value="joinType"
                size="small"
                class="w-full"
                :options="joinTypeOptions"
              />
            </a-form-item>
          </a-col>
          <a-col :span="24">
            <a-form-item :label="t('statistics.quickquery.configurable.page.designer.join.condition')">
              <div class="flex flex-wrap items-center gap-2">
                <a-select
                  v-model:value="joinLeftColumn"
                  :loading="primaryColumnsLoading"
                  show-search
                  option-filter-prop="label"
                  allow-clear
                  size="small"
                  class="min-w-[160px] flex-1"
                  :placeholder="primaryAlias + '.column'"
                >
                  <a-select-option
                    v-for="col in primaryColumnOptions"
                    :key="col.databaseColumnName"
                    :value="col.databaseColumnName"
                    :label="col.databaseColumnName"
                  >
                    {{ primaryAlias }}.{{ col.databaseColumnName }}
                  </a-select-option>
                </a-select>
                <span class="shrink-0 text-sm text-text-secondary">=</span>
                <a-select
                  v-model:value="joinRightColumn"
                  :loading="joinColumnsLoading"
                  show-search
                  option-filter-prop="label"
                  allow-clear
                  size="small"
                  class="min-w-[160px] flex-1"
                  :placeholder="joinTableAlias + '.column'"
                >
                  <a-select-option
                    v-for="col in joinColumnOptions"
                    :key="col.databaseColumnName"
                    :value="col.databaseColumnName"
                    :label="col.databaseColumnName"
                  >
                    {{ joinTableAlias }}.{{ col.databaseColumnName }}
                  </a-select-option>
                </a-select>
              </div>
            </a-form-item>
          </a-col>
        </a-row>
      </div>
    </section>
    <!-- 步骤：数据列清单（单表 / 表连接共用） -->
    <section v-show="currentStepId === 'fields'" class="rounded border border-border bg-container">
      <div class="p-2">
        <a-spin :spinning="fieldTreeLoading">
          <a-empty
            v-if="!fieldTreeLoading && !hasFieldTreeSources"
            :description="t('statistics.quickquery.configurable.page.designer.fieldtree.empty')"
          />
          <a-table
            v-else
            :columns="fieldTreeColumns"
            :data-source="fieldTreeData"
            :pagination="false"
            :scroll="{ y: 420, x: 'max-content' }"
            :default-expand-all-rows="true"
            :row-key="(row: SqviFieldTreeNode) => row.key"
            :custom-row="fieldTreeCustomRow"
            size="small"
            bordered
            class="designer-field-tree-table"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'output'">
                <span v-if="record.nodeType !== 'column'" class="block text-center tabular-nums">
                  {{ countFieldTreeOutput(record as SqviFieldTreeNode) }}
                </span>
                <a-checkbox
                  v-else
                  :checked="isFieldOutputChecked(record.sourceAlias!, record.columnName!)"
                  @change="(e: { target: { checked: boolean } }) => toggleFieldOutput(record as SqviFieldTreeNode, e.target.checked)"
                />
              </template>
              <template v-else-if="column.key === 'selection'">
                <span v-if="record.nodeType !== 'column'" class="block text-center tabular-nums">
                  {{ countFieldTreeSelection(record as SqviFieldTreeNode) }}
                </span>
                <a-checkbox
                  v-else
                  :checked="isFieldSelectionChecked(record.sourceAlias!, record.columnName!)"
                  @change="(e: { target: { checked: boolean } }) => toggleFieldSelection(record as SqviFieldTreeNode, e.target.checked)"
                />
              </template>
              <template v-else-if="column.key === 'technical'">
                <span
                  v-if="record.nodeType === 'column'"
                  class="font-mono text-xs text-text-secondary"
                >
                  {{ record.technicalName }}
                </span>
                <span
                  v-else-if="record.nodeType === 'source'"
                  class="font-mono text-xs text-text-secondary"
                >
                  {{ record.tableName }}
                </span>
              </template>
            </template>
          </a-table>
        </a-spin>
      </div>
    </section>
    <!-- 步骤 4：高级 -->
    <section v-show="currentStepId === 'advanced'" class="rounded border border-border bg-container p-4">
      <a-collapse>
        <a-collapse-panel
          v-if="isJoinMode"
          key="joins"
          :header="t('entity.configurablejoin._self')"
        >
          <p class="mb-2 text-xs text-text-secondary">
            {{ t('statistics.quickquery.configurable.page.designer.join.summaryreadonly') }}
          </p>
          <a-table
            :columns="joinReadonlyColumns"
            :data-source="joinRows"
            :pagination="false"
            size="small"
            bordered
            :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'joinType'">
                {{ resolveJoinTypeLabel(Number(record.joinType ?? 1)) }}
              </template>
              <template v-else-if="column.key === 'leftSourceAlias'">
                {{ record.leftSourceAlias }}
              </template>
              <template v-else-if="column.key === 'leftColumnName'">
                {{ record.leftColumnName }}
              </template>
              <template v-else-if="column.key === 'rightSourceAlias'">
                {{ record.rightSourceAlias }}
              </template>
              <template v-else-if="column.key === 'rightColumnName'">
                {{ record.rightColumnName }}
              </template>
            </template>
          </a-table>
        </a-collapse-panel>
        <a-collapse-panel key="groupby" :header="t('entity.configurablegroupby._self')">
          <div class="mb-2">
            <a-button type="primary" size="small" @click="addGroupByRow">
              {{ t('common.page.button.create') }}
            </a-button>
          </div>
          <a-table
            :columns="groupByColumnsDisplay"
            :data-source="groupByRows"
            :pagination="false"
            size="small"
            bordered
            :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          >
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'sourceAlias'">
                <a-select
                  v-model:value="record.sourceAlias"
                  size="small"
                  class="w-full"
                  :options="configuredSourceAliasOptions"
                  @change="handleGroupByAliasChange(record)"
                />
              </template>
              <template v-else-if="column.key === 'columnName'">
                <a-select
                  v-model:value="record.columnName"
                  :loading="isGroupByColumnsLoading(record)"
                  show-search
                  option-filter-prop="label"
                  allow-clear
                  size="small"
                  class="w-full"
                >
                  <a-select-option
                    v-for="col in resolveColumnOptionsForRecord(record)"
                    :key="col.databaseColumnName"
                    :value="col.databaseColumnName"
                    :label="col.databaseColumnName"
                  >
                    {{ col.databaseColumnName }}
                  </a-select-option>
                </a-select>
              </template>
              <template v-else-if="column.key === '__action'">
                <a-button type="link" danger size="small" @click="removeGroupByRow(index)">
                  {{ t('common.page.button.delete') }}
                </a-button>
              </template>
            </template>
          </a-table>
        </a-collapse-panel>
        <a-collapse-panel key="orderby" :header="t('entity.configurableorderby._self')">
          <div class="mb-2">
            <a-button type="primary" size="small" @click="addOrderByRow">
              {{ t('common.page.button.create') }}
            </a-button>
          </div>
          <a-table
            :columns="orderByColumnsDisplay"
            :data-source="orderByRows"
            :pagination="false"
            size="small"
            bordered
            :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          >
            <template #bodyCell="{ column, record, index }">
              <template v-if="column.key === 'sourceAlias'">
                <a-select
                  v-model:value="record.sourceAlias"
                  size="small"
                  class="w-full"
                  :options="configuredSourceAliasOptions"
                  @change="handleOrderByAliasChange(record)"
                />
              </template>
              <template v-else-if="column.key === 'columnName'">
                <a-select
                  v-model:value="record.columnName"
                  :loading="isOrderByColumnsLoading(record)"
                  show-search
                  option-filter-prop="label"
                  allow-clear
                  size="small"
                  class="w-full"
                >
                  <a-select-option
                    v-for="col in resolveColumnOptionsForRecord(record)"
                    :key="col.databaseColumnName"
                    :value="col.databaseColumnName"
                    :label="col.databaseColumnName"
                  >
                    {{ col.databaseColumnName }}
                  </a-select-option>
                </a-select>
              </template>
              <template v-else-if="column.key === 'sortDirection'">
                <TaktSelect
                  v-model:value="record.sortDirection"
                  dict-type="sys_sort_type"
                  size="small"
                  class="w-full"
                />
              </template>
              <template v-else-if="column.key === '__action'">
                <a-button type="link" danger size="small" @click="removeOrderByRow(index)">
                  {{ t('common.page.button.delete') }}
                </a-button>
              </template>
            </template>
          </a-table>
        </a-collapse-panel>
      </a-collapse>
    </section>
  <!-- 步骤导航 -->
    <div class="flex justify-between border-t border-border pt-4">
      <a-button
        size="small"
        :disabled="currentStepIndex === 0"
        @click="goPrevStep"
      >
        {{ t('statistics.quickquery.configurable.page.designer.prevstep') }}
      </a-button>
      <div class="flex gap-2">
        <a-button
          v-if="currentStepIndex < maxWizardStepIndex"
          type="primary"
          size="small"
          class="takt-button-query"
          @click="goNextStep"
        >
          <template v-if="currentStepId === 'join'">
            <RiCheckLine class="takt-remix-icon mr-1" />
            {{ t('statistics.quickquery.configurable.page.designer.confirm') }}
          </template>
          <template v-else>
            {{ t('statistics.quickquery.configurable.page.designer.nextstep') }}
          </template>
        </a-button>
      </div>
    </div>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 定制报表全栈设计器：主表 + 六子表、分步向导；defineExpose validate/getValues/resetFields
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SelectValue } from 'ant-design-vue/es/select'
import { RiCheckLine } from '@remixicon/vue'
import TaktSelect from '@/components/business/TaktSelect/index.vue'
import type { ConfigurableCreate } from '@/types/statistics/quick-query/configurable.types'
import type { DatabaseTableColumnInfo, DatabaseTableInfo } from '@/types/code/database/databaseInfo.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useConfigurableSchemaCatalog } from '../composables/useConfigurableSchemaCatalog'
import { useConfigurableCategory } from '../composables/useConfigurableCategory'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'

/** 字段树节点类型 */
type SqviFieldTreeNodeType = 'root' | 'source' | 'column'

/** 步骤 3 字段树节点 */
interface SqviFieldTreeNode {
  key: string
  nodeType: SqviFieldTreeNodeType
  title: string
  technicalName?: string
  sourceAlias?: string
  columnName?: string
  tableName?: string
  children?: SqviFieldTreeNode[]
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ConfigurableCreate & { configurableId?: string }> | null
  /** 父级提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 */
const formRef = ref()
/** 主表表单模型（TaktConfigurable） */
const formState = reactive<Record<string, any>>({})
/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.configurableId)
useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'configurableCode',
})
/** 定制报表业务域 / 子分类（菜单联动） */
const { moduleOptions, subCategoryOptions } = useConfigurableCategory(
  toRef(formState, 'configurableDomain'),
  toRef(formState, 'configurableSubCategory')
)
/** 数据源子表行（TaktConfigurableSource） */
const sourceRows = ref<Record<string, unknown>[]>([])
/** 关联子表行（TaktConfigurableJoin） */
const joinRows = ref<Record<string, unknown>[]>([])
/** 输出字段子表行（TaktConfigurableField） */
const fieldRows = ref<Record<string, unknown>[]>([])
/** 筛选条件子表行（TaktConfigurableSelection） */
const selectionRows = ref<Record<string, unknown>[]>([])

/** 分组子表行（TaktConfigurableGroupBy） */
const groupByRows = ref<Record<string, unknown>[]>([])
/** 排序子表行（TaktConfigurableOrderBy） */
const orderByRows = ref<Record<string, unknown>[]>([])

/** 向导步骤标识 */
type SqviWizardStepId = 'basic' | 'join' | 'fields' | 'advanced'

/** 向导步骤项 */
interface SqviWizardStepItem {
  id: SqviWizardStepId
  title: string
}

/** i18n */
const { t } = useI18n()
/** 租户上下文 */
const tenantStore = useTenantStore()
/** 用户上下文 */
const userStore = useUserStore()
/** Schema 目录 */
const {
  databaseInfoList,
  databaseInfoLoading,
  tablesByTenant,
  loadDatabaseInfoList,
  loadTablesForTenant,
  resolveDatabaseDisplayName,
  isTablesLoading,
  loadColumnsForTable,
  getCachedColumns,
  isColumnsLoading,
} = useConfigurableSchemaCatalog()

/** 当前步骤索引（与 a-steps :current 对齐） */
const currentStepIndex = ref(0)
/** 主表租户编码（目录） */
const primaryCatalogTenant = ref(tenantStore.tenantCode)
/** 主表物理表名 */
const primaryTableName = ref('')
/** 单表模式内部数据源别名（SQL 编译用，不向用户展示） */
const SINGLE_TABLE_INTERNAL_ALIAS = 'A'

/**
 * 单表模式内部数据源别名
 * @returns 固定别名
 */
function resolveSingleTableInternalAlias(): string {
  return SINGLE_TABLE_INTERNAL_ALIAS
}

/**
 * 解析当前模式下的主表数据源别名（单表=内部固定值；连接=用户输入）
 * @returns 主表别名
 */
function resolvePrimarySourceAlias(): string {
  if (isSingleTableMode.value) {
    return resolveSingleTableInternalAlias()
  }
  return primaryAlias.value?.trim() || 'A'
}

/** 主表数据源别名（表连接模式用户可编辑；单表仅内部使用） */
const primaryAlias = ref('A')
/** 数据源类型：table=单表，join=表连接 */
const sourceType = ref('table')
/** 离开基本信息步后锁定数据源类型（禁止改选单表/表连接） */
const sourceTypeLocked = ref(false)
/** 连接表物理表名 */
const joinTableName = ref('')
/** 连接表别名（默认 B） */
const joinTableAlias = ref('B')
/** 关联类型（1=内连接，2=左，3=右，4=全） */
const joinType = ref(1)
/** 关联左列（主表） */
const joinLeftColumn = ref('')
/** 关联右列（连接表） */
const joinRightColumn = ref('')
/** 字段树列加载中 */
const fieldTreeLoading = ref(false)

/** CreateDto 主表字段名 */
const formFields = [
  'tenantCode', 'companyCode', 'cultureCode', 'configurableCode', 'configurableName',
  'configurableDomain', 'configurableSubCategory', 'distinctRows',   'maxExportRows', 'maxQueryRows', 'isPublic', 'configurableStatus', 'remark']

/**
 * 注入租户/公司/默认语言
 * @param target 目标对象
 * @param force 是否强制覆盖
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
  target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
}

/**
 * 新增态主表数值默认值（对齐实体 DefaultValue）
 * @param target 目标对象
 */
function applyCreateFieldDefaults(target: Record<string, unknown>) {
  if (target.configurableDomain == null || target.configurableDomain === '') {
    target.configurableDomain = 9
  }
  if (!String(target.numberingRuleCode ?? '').trim()) {
    target.numberingRuleCode = 'ST-RPT'
  }
  if (target.distinctRows == null || target.distinctRows === '') {
    target.distinctRows = 1
  }
  if (target.maxExportRows == null || target.maxExportRows === '') {
    target.maxExportRows = 500
  }
  if (target.maxQueryRows == null || target.maxQueryRows === '') {
    target.maxQueryRows = 500
  }
  if (target.isPublic == null || target.isPublic === '') {
    target.isPublic = 0
  }
  if (target.configurableStatus == null || target.configurableStatus === '') {
    target.configurableStatus = 1
  }
}

/** 编辑态从 formData 同步子表行 */
function syncChildRowsFromFormData(
  val: Partial<ConfigurableCreate & { configurableId?: string }> | null | undefined
) {
  sourceRows.value = ((val as any)?.sources ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __catalogTenantCode: item.__catalogTenantCode ?? tenantStore.tenantCode,
    __rowKey: item.configurableSourceId ?? `new-${index}`,
  }))
  joinRows.value = ((val as any)?.joins ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableJoinId ?? `new-${index}`,
  }))
  fieldRows.value = ((val as any)?.fields ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableFieldId ?? `new-${index}`,
  }))
  selectionRows.value = ((val as any)?.selections ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableSelectionId ?? `new-${index}`,
  }))
  groupByRows.value = ((val as any)?.groupBys ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableGroupById ?? `new-${index}`,
  }))
  orderByRows.value = ((val as any)?.orderBys ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.configurableOrderById ?? `new-${index}`,
  }))
  if (joinRows.value.length > 0 || sourceRows.value.length > 1) {
    sourceType.value = 'join'
  } else if (sourceRows.value.length === 1) {
    sourceType.value = 'table'
    if (!isPrimarySourceRow(sourceRows.value[0])) {
      sourceRows.value[0].isPrimary = 1
    }
  }
  if ((val as any)?.configurableId && (sourceRows.value.length > 0 || joinRows.value.length > 0)) {
    sourceTypeLocked.value = true
  }
  if (sourceType.value === 'table') {
    normalizeRowsForSingleTable()
  }
}

/** 组装 Create/Update 载荷（主表 + 六子表） */
function buildSubmitPayload() {
  if (isSingleTableMode.value) {
    normalizeRowsForSingleTable()
    pruneSourcesForSingleTable()
  }
  normalizeFieldRelatedRowOrder()
  const payload: Record<string, unknown> = {
    ...formState,
    sources: sourceRows.value.map(({ __rowKey, __catalogTenantCode, ...rest }) => rest),
    joins: isSingleTableMode.value
      ? []
      : joinRows.value.map(({ __rowKey, ...rest }) => rest),
    fields: fieldRows.value.map(({ __rowKey, ...rest }, index) => ({
      ...rest,
      sortOrder: index,
    })),
    selections: selectionRows.value.map(({ __rowKey, ...rest }, index) => ({
      ...rest,
      sortOrder: index + 1,
      filterOperator: Number(rest.filterOperator) || 7,
    })),
    groupBys: groupByRows.value.map(({ __rowKey, ...rest }) => rest),
    orderBys: orderByRows.value.map(({ __rowKey, ...rest }) => rest),
  }
  delete payload.ExtField
  delete payload.plantCode
  if (!props.formData?.configurableId) {
    delete payload.sortOrder
  } else if (payload.numberingRuleCode !== undefined) {
    delete payload.numberingRuleCode
  }
  if (payload.numberingRuleCode !== undefined && !String(payload.numberingRuleCode ?? '').trim()) {
    delete payload.numberingRuleCode
  }
  return payload
}

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  configurableCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  configurableName: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.configurable.name') }),
    trigger: 'blur',
  }],
  configurableDomain: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.configurable.domain') }),
    trigger: 'change',
  }],
  distinctRows: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.configurable.distinctrows') }),
    trigger: 'change',
  }],
  maxExportRows: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.configurable.maxexportrows') }),
    trigger: 'change',
  }],
  maxQueryRows: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.configurable.maxqueryrows') }),
    trigger: 'change',
  }],
  isPublic: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.configurable.ispublic') }),
    trigger: 'change',
  }],
  configurableStatus: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.configurable.status') }),
    trigger: 'change',
  }],
}))

/** 校验表单 */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 API 入参 */
function getValues(): Record<string, unknown> {
  if (isSingleTableMode.value) {
    normalizeRowsForSingleTable()
    pruneSourcesForSingleTable()
  }
  return buildSubmitPayload()
}

/** 重置表单与子表 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  sourceRows.value = []
  joinRows.value = []
  fieldRows.value = []
  selectionRows.value = []
  groupByRows.value = []
  orderByRows.value = []
  resetWizardPickerState()
  lastHydratedFormDataRef = undefined
}

/**
 * 重置向导选表/连接选择器（新建或关闭弹窗）
 */
function resetWizardPickerState(): void {
  sourceType.value = 'table'
  sourceTypeLocked.value = false
  ensurePrimaryCatalogTenantFromContext()
  primaryTableName.value = ''
  primaryAlias.value = 'A'
  clearJoinPickerState()
  currentStepIndex.value = 0
}

/** 是否为表连接模式 */
const isJoinMode = computed(() => sourceType.value === 'join')
/** 是否为单表模式 */
const isSingleTableMode = computed(() => sourceType.value === 'table')

/** 单表：基本信息 → 数据列清单 → 高级；表连接：基本信息 → 表连接设计 → 数据列清单 → 高级 */
const wizardSteps = computed((): SqviWizardStepItem[] => {
  if (isJoinMode.value) {
    return [
      { id: 'basic', title: t('statistics.quickquery.configurable.page.designer.steps.basicinfo') },
      { id: 'join', title: t('statistics.quickquery.configurable.page.designer.steps.joindesign') },
      { id: 'fields', title: t('statistics.quickquery.configurable.page.designer.steps.datalist') },
      { id: 'advanced', title: t('statistics.quickquery.configurable.page.designer.steps.advanced') }]
  }
  return [
    { id: 'basic', title: t('statistics.quickquery.configurable.page.designer.steps.basicinfo') },
    { id: 'fields', title: t('statistics.quickquery.configurable.page.designer.steps.datalist') },
    { id: 'advanced', title: t('statistics.quickquery.configurable.page.designer.steps.advanced') }]
})

/** 当前步骤标识 */
const currentStepId = computed((): SqviWizardStepId =>
  wizardSteps.value[currentStepIndex.value]?.id ?? 'basic'
)

/** 向导最大步骤索引 */
const maxWizardStepIndex = computed(() => Math.max(0, wizardSteps.value.length - 1))

/** 数据源类型选项 */
const sourceTypeOptions = computed(() => [
  {
    value: 'table',
    label: t('statistics.quickquery.configurable.page.designer.sourcetype.table'),
  },
  {
    value: 'join',
    label: t('statistics.quickquery.configurable.page.designer.sourcetype.join'),
  }])

/** 比较符下拉 */
const primaryTableOptions = computed(() => tablesByTenant.value[primaryCatalogTenant.value] ?? [])
/** 主表列是否加载中 */
const primaryColumnsLoading = computed(() =>
  isColumnsLoading(primaryCatalogTenant.value, primaryTableName.value)
)
/** 主表列选项 */
const primaryColumnOptions = computed(() =>
  getCachedColumns(primaryCatalogTenant.value, primaryTableName.value)
)
/** 连接表列是否加载中 */
const joinColumnsLoading = computed(() =>
  isColumnsLoading(primaryCatalogTenant.value, joinTableName.value)
)
/** 连接表列选项 */
const joinColumnOptions = computed(() =>
  getCachedColumns(primaryCatalogTenant.value, joinTableName.value)
)

/** 关联类型选项（与 TaktStatQueryBuilder JoinType 一致） */
const joinTypeOptions = computed(() => [
  { value: 1, label: t('statistics.quickquery.configurable.page.designer.jointype.inner') },
  { value: 2, label: t('statistics.quickquery.configurable.page.designer.jointype.left') },
  { value: 3, label: t('statistics.quickquery.configurable.page.designer.jointype.right') },
  { value: 4, label: t('statistics.quickquery.configurable.page.designer.jointype.full') }])

/** 步骤 3 字段树表列（快速查询：字段清单(描述) / 清单字段 / 选择字段 / 字段名称） */
const fieldTreeColumns = computed(() => [
  {
    title: t('statistics.quickquery.configurable.page.designer.fieldtree.datafield'),
    dataIndex: 'title',
    key: 'title',
    width: 320,
    ellipsis: true,
  },
  {
    title: t('statistics.quickquery.configurable.page.designer.fieldtree.outputlist'),
    key: 'output',
    width: 96,
    align: 'center' as const,
  },
  {
    title: t('statistics.quickquery.configurable.page.designer.fieldtree.selectionfield'),
    key: 'selection',
    width: 96,
    align: 'center' as const,
  },
  {
    title: t('statistics.quickquery.configurable.page.designer.fieldtree.fieldname'),
    key: 'technical',
    width: 220,
  }])

/** 字段树是否有可用数据源（含选表器兜底） */
const hasFieldTreeSources = computed(() => resolveFieldTreeSourceRows().length > 0)

/**
 * 构建字段树表节点（表 → 列）
 * @param source 数据源行
 * @param tables 租户物理表元数据
 * @returns 表节点或 null
 */
function buildFieldTreeSourceNode(
  source: Record<string, unknown>,
  tables: Record<string, DatabaseTableInfo[]>
): SqviFieldTreeNode | null {
  const tableName = String(source.tableName ?? '').trim()
  const tenant = String(source.__catalogTenantCode ?? primaryCatalogTenant.value).trim()
  const alias = isSingleTableMode.value
    ? (String(source.sourceAlias ?? '').trim() || resolveSingleTableInternalAlias())
    : String(source.sourceAlias ?? '').trim()
  if (!tableName || (!isSingleTableMode.value && !alias)) {
    return null
  }
  const tableMeta = (tables[tenant] ?? []).find((item) => item.tableName === tableName)
  const tableLabel = tableMeta?.tableComment?.trim() || tableName
  const columns = getCachedColumns(tenant, tableName)
  return {
    key: `source-${alias}`,
    nodeType: 'source',
    title: tableLabel,
    sourceAlias: alias,
    tableName,
    children: columns.map((col) => ({
      key: `col-${alias}-${col.databaseColumnName}`,
      nodeType: 'column' as const,
      title: col.columnComment?.trim() || col.databaseColumnName,
      sourceAlias: alias,
      columnName: col.databaseColumnName,
      tableName,
      technicalName: formatFieldTreeTechnicalName(alias, tableName, col.databaseColumnName),
    })),
  }
}

/** 字段树数据（单表：表→列；表连接：根→多表→列） */
const fieldTreeData = computed((): SqviFieldTreeNode[] => {
  const effectiveSources = resolveFieldTreeSourceRows()
  if (effectiveSources.length === 0) {
    return []
  }
  const tables = tablesByTenant.value
  const sourceNodes = effectiveSources
    .map((source) => buildFieldTreeSourceNode(source, tables))
    .filter((node): node is SqviFieldTreeNode => node != null)
  if (isSingleTableMode.value && sourceNodes.length === 1) {
    return sourceNodes
  }
  return [
    {
      key: 'root',
      nodeType: 'root',
      title: t('statistics.quickquery.configurable.page.designer.fieldtree.root'),
      children: sourceNodes,
    }]
})

/** 表连接只读预览列（高级步骤） */
const joinReadonlyColumns = computed(() => [
  { title: t('entity.configurablejoin.jointype'), key: 'joinType', width: 100 },
  { title: t('entity.configurablejoin.leftsourcealias'), key: 'leftSourceAlias', width: 100 },
  { title: t('entity.configurablejoin.leftcolumnname'), key: 'leftColumnName', width: 120 },
  { title: t('entity.configurablejoin.rightsourcealias'), key: 'rightSourceAlias', width: 100 },
  { title: t('entity.configurablejoin.rightcolumnname'), key: 'rightColumnName', width: 120 }])

/** 已配置数据源别名选项（表连接筛选用） */
const configuredSourceAliasOptions = computed(() =>
  sourceRows.value
    .map((row) => {
      const alias = String(row.sourceAlias ?? '').trim()
      if (!alias) {
        return null
      }
      return {
        value: alias,
        label: `${alias} (${String(row.tableName ?? '')})`,
        tableName: String(row.tableName ?? ''),
        catalogTenant: String(row.__catalogTenantCode ?? primaryCatalogTenant.value),
      }
    })
    .filter((item): item is NonNullable<typeof item> => item != null)
)

/** 分组表列（单表不展示数据源别名） */
const groupByColumnsDisplay = computed(() => {
  const columns: Array<{ title: string; key: string; width: number }> = []
  if (isJoinMode.value) {
    columns.push({
      title: t('entity.configurablegroupby.sourcealias'),
      key: 'sourceAlias',
      width: 120,
    })
  }
  columns.push(
    { title: t('entity.configurablegroupby.columnname'), key: 'columnName', width: 160 },
    { title: t('common.action.operation'), key: '__action', width: 72 }
  )
  return columns
})

/** 排序表列（单表不展示数据源别名） */
const orderByColumnsDisplay = computed(() => {
  const columns: Array<{ title: string; key: string; width: number }> = []
  if (isJoinMode.value) {
    columns.push({
      title: t('entity.configurableorderby.sourcealias'),
      key: 'sourceAlias',
      width: 120,
    })
  }
  columns.push(
    { title: t('entity.configurableorderby.columnname'), key: 'columnName', width: 160 },
    { title: t('entity.configurableorderby.sortdirection'), key: 'sortDirection', width: 100 },
    { title: t('common.action.operation'), key: '__action', width: 72 }
  )
  return columns
})

/**
 * 离开基本信息步后锁定数据源类型
 */
function lockSourceTypeAfterBasic(): void {
  sourceTypeLocked.value = true
}

/**
 * 步骤条点击切换
 * @param step 目标步骤索引
 */
async function handleStepChange(step: number): Promise<void> {
  if (step < 0 || step > maxWizardStepIndex.value) {
    return
  }
  if (step > currentStepIndex.value) {
    for (let index = currentStepIndex.value; index < step; index += 1) {
      const stepId = wizardSteps.value[index]?.id
      if (!stepId || !validateStepBeforeLeave(stepId)) {
        return
      }
      if (stepId === 'basic') {
        lockSourceTypeAfterBasic()
      }
    }
  }
  const targetId = wizardSteps.value[step]?.id
  if (targetId === 'fields') {
    await ensureFieldTreeReady()
  }
  currentStepIndex.value = step
}

/**
 * 上一步
 */
function goPrevStep(): void {
  if (currentStepIndex.value > 0) {
    currentStepIndex.value -= 1
  }
}

/**
 * 下一步
 */
async function goNextStep(): Promise<void> {
  if (!validateStepBeforeLeave(currentStepId.value)) {
    return
  }
  if (currentStepId.value === 'basic') {
    lockSourceTypeAfterBasic()
  }
  if (currentStepIndex.value < maxWizardStepIndex.value) {
    const nextIndex = currentStepIndex.value + 1
    const nextId = wizardSteps.value[nextIndex]?.id
    if (nextId === 'fields') {
      await ensureFieldTreeReady()
    }
    currentStepIndex.value = nextIndex
  }
}

/**
 * 离开当前步骤前校验
 * @param stepId 当前步骤标识
 * @returns 是否允许离开
 */
function validateStepBeforeLeave(stepId: SqviWizardStepId): boolean {
  if (stepId === 'basic') {
    if (!isEditMode.value && !String(formState.numberingRuleCode ?? '').trim()) {
      message.warning(t('common.page.form.numberingRuleRequired'))
      return false
    }
    const configurableCode = String(formState.configurableCode ?? '').trim()
    const configurableName = String(formState.configurableName ?? '').trim()
    if (!configurableCode) {
      message.warning(t('common.page.form.numberingCodePreview'))
      return false
    }
    if (!configurableName) {
      message.warning(
        t('common.page.form.placeholder.required', { field: t('entity.configurable.name') })
      )
      return false
    }
    if (isSingleTableMode.value) {
      ensurePrimaryCatalogTenantFromContext()
      const tenantCode = resolvePrimaryCatalogTenant()
      if (!tenantCode) {
        message.warning(
          t('common.page.form.placeholder.select', {
            field: t('statistics.quickquery.configurable.page.field.tenant'),
          })
        )
        return false
      }
      primaryCatalogTenant.value = tenantCode
      if (!primaryTableName.value?.trim()) {
        message.warning(
          t('common.page.form.placeholder.select', {
            field: t('statistics.quickquery.configurable.page.designer.tableview'),
          })
        )
        return false
      }
      if (!writePrimarySourceRow()) {
        message.warning(t('statistics.quickquery.configurable.page.designer.fieldtree.sourcenotready'))
        return false
      }
    }
    return true
  }
  if (stepId === 'join') {
    if (!primaryTableName.value?.trim()) {
      message.warning(
        t('common.page.form.placeholder.select', {
          field: t('statistics.quickquery.configurable.page.designer.tableview'),
        })
      )
      return false
    }
    if (!joinTableName.value?.trim()) {
      message.warning(
        t('common.page.form.placeholder.select', {
          field: t('statistics.quickquery.configurable.page.designer.join.jointable'),
        })
      )
      return false
    }
    return applyTableJoin()
  }
  if (stepId === 'fields') {
    const hasVisible = fieldRows.value.some((row) => Number(row.isVisible) === 1)
    if (!hasVisible) {
      message.warning(t('statistics.quickquery.configurable.page.designer.novisiblefield'))
      return false
    }
    return true
  }
  if (stepId === 'advanced' && isJoinMode.value && joinRows.value.length === 0) {
    message.warning(t('statistics.quickquery.configurable.page.designer.join.conditionrequired'))
    return false
  }
  return true
}

/**
 * 作用域默认值
 * @returns 租户/公司/语言
 */
function scopeDefaults() {
  return {
    tenantCode: tenantStore.tenantCode,
    companyCode: tenantStore.companyCode,
    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
  }
}

/**
 * 从已有数据源行同步主表选择器
 */
function syncPrimaryPickerFromSources(): void {
  const primary = resolvePrimarySourceRow() ?? sourceRows.value[0]
  if (!primary) {
    return
  }
  primaryCatalogTenant.value = String(
    primary.__catalogTenantCode ?? tenantStore.tenantCode ?? ''
  )
  primaryTableName.value = String(primary.tableName ?? '')
  if (isJoinMode.value) {
    primaryAlias.value = String(primary.sourceAlias ?? 'A')
  }
  syncJoinPickerFromRows()
}

/**
 * 从关联子表与次要数据源同步表连接选择器（不改写用户已选的数据源类型）
 */
function syncJoinPickerFromRows(): void {
  const secondary = sourceRows.value.find((row) => Number(row.isPrimary) === 0)
  if (secondary) {
    joinTableName.value = String(secondary.tableName ?? '')
    joinTableAlias.value = String(secondary.sourceAlias ?? 'B')
  }
  const join = joinRows.value[0]
  if (join) {
    joinType.value = Number(join.joinType ?? 1)
    joinLeftColumn.value = String(join.leftColumnName ?? '')
    joinRightColumn.value = String(join.rightColumnName ?? '')
    if (!secondary) {
      joinTableAlias.value = String(join.rightSourceAlias ?? 'B')
    }
  }
}

/**
 * 清空表连接选择器状态
 */
function clearJoinPickerState(): void {
  joinTableName.value = ''
  joinTableAlias.value = 'B'
  joinType.value = 1
  joinLeftColumn.value = ''
  joinRightColumn.value = ''
}

/**
 * 获取当前有效数据源别名集合
 * @returns 别名集合
 */
function getValidSourceAliases(): Set<string> {
  return new Set(
    sourceRows.value.map((row) => String(row.sourceAlias ?? '').trim()).filter(Boolean)
  )
}

/**
 * 移除引用无效别名的子表行
 * @param validAliases 有效别名
 */
function pruneChildRowsByValidAliases(validAliases: Set<string>): void {
  const fallback = isSingleTableMode.value
    ? resolveSingleTableInternalAlias()
    : (primaryAlias.value?.trim() || 'A')
  const aliases = validAliases.size > 0 ? validAliases : new Set([fallback])
  fieldRows.value = fieldRows.value.filter((row) =>
    aliases.has(String(row.sourceAlias ?? '').trim())
  )
  selectionRows.value = selectionRows.value.filter((row) =>
    aliases.has(String(row.sourceAlias ?? '').trim())
  )
  groupByRows.value = groupByRows.value.filter((row) =>
    aliases.has(String(row.sourceAlias ?? '').trim())
  )
  orderByRows.value = orderByRows.value.filter((row) =>
    aliases.has(String(row.sourceAlias ?? '').trim())
  )
}

/**
 * 单表模式：统一子表行主表别名
 */
function normalizeRowsForSingleTable(): void {
  const alias = resolveSingleTableInternalAlias()
  const primary = resolvePrimarySourceRow()
  if (primary) {
    primary.isPrimary = 1
    primary.sourceAlias = alias
  }
  for (const field of fieldRows.value) {
    field.sourceAlias = alias
  }
  for (const selection of selectionRows.value) {
    selection.sourceAlias = alias
  }
  for (const groupBy of groupByRows.value) {
    groupBy.sourceAlias = alias
  }
  for (const orderBy of orderByRows.value) {
    orderBy.sourceAlias = alias
  }
}

/**
 * 切换为单表模式并清理连接相关配置
 */
function switchToSingleTableMode(): void {
  pruneSourcesForSingleTable()
  clearJoinPickerState()
  normalizeRowsForSingleTable()
  pruneChildRowsByValidAliases(getValidSourceAliases())
}

/**
 * 数据源类型切换
 * @param value 选中值（table | join）
 */
function handleSourceTypeChange(value: SelectValue): void {
  if (sourceTypeLocked.value || typeof value !== 'string') {
    return
  }
  if (value === 'table') {
    switchToSingleTableMode()
    message.info(t('statistics.quickquery.configurable.page.designer.sourcetype.clearedjoin'))
  } else {
    joinTableAlias.value = joinTableAlias.value?.trim() || 'B'
  }
}

/**
 * 解析关联类型展示文案
 * @param joinTypeValue 关联类型值
 * @returns 文案
 */
function resolveJoinTypeLabel(joinTypeValue: number): string {
  return joinTypeOptions.value.find((item) => item.value === joinTypeValue)?.label
    ?? String(joinTypeValue)
}

/**
 * 字段树技术名（单表=列名；连接=别名-列名）
 * @param sourceAlias 数据源别名
 * @param tableName 物理表名
 * @param columnName 列名
 * @returns 技术名
 */
function formatFieldTreeTechnicalName(
  sourceAlias: string,
  tableName: string,
  columnName: string
): string {
  const col = columnName.trim()
  if (!col) {
    return ''
  }
  if (isSingleTableMode.value) {
    return col
  }
  const alias = sourceAlias.trim() || tableName.trim()
  return alias ? `${alias}-${col}` : col
}

/**
 * 解析主表目录租户（选择器 → 当前租户 → 库列表首项）
 * @returns 租户编码
 */
function resolvePrimaryCatalogTenant(): string {
  const fromPicker = primaryCatalogTenant.value?.trim()
  if (fromPicker) {
    return fromPicker
  }
  const fromStore = tenantStore.tenantCode?.trim()
  if (fromStore) {
    return fromStore
  }
  return databaseInfoList.value[0]?.tenantCode?.trim() ?? ''
}

/**
 * 将目录租户写入选择器（仅在为空时）
 */
function ensurePrimaryCatalogTenantFromContext(): void {
  const resolved = resolvePrimaryCatalogTenant()
  if (resolved) {
    primaryCatalogTenant.value = resolved
  }
}

/**
 * 是否主表数据源行
 * @param row 数据源行
 * @returns 是否主表
 */
function isPrimarySourceRow(row: Record<string, unknown>): boolean {
  return Number(row.isPrimary) === 1
}

/**
 * 解析单表模式主表数据源行（兼容 isPrimary 缺失或仅一行）
 * @returns 主表行或 undefined
 */
function resolvePrimarySourceRow(): Record<string, unknown> | undefined {
  const explicit = sourceRows.value.find((row) => isPrimarySourceRow(row))
  if (explicit) {
    return explicit
  }
  const tableName = primaryTableName.value?.trim()
  if (tableName) {
    const matched = sourceRows.value.find(
      (row) => String(row.tableName ?? '').trim() === tableName
    )
    if (matched) {
      return matched
    }
  }
  if (sourceRows.value.length === 1) {
    return sourceRows.value[0]
  }
  return undefined
}

/**
 * 解析字段树数据源行（优先已应用 sourceRows，否则按选表器兜底）
 * @returns 数据源行列表
 */
function resolveFieldTreeSourceRows(): Array<Record<string, unknown>> {
  ensurePrimaryCatalogTenantFromContext()
  if (sourceRows.value.length > 0) {
    return sourceRows.value
      .map((row, index) => ({ row, index }))
      .sort((a, b) => {
        const primaryA = Number(a.row.isPrimary) === 1 ? 0 : 1
        const primaryB = Number(b.row.isPrimary) === 1 ? 0 : 1
        if (primaryA !== primaryB) {
          return primaryA - primaryB
        }
        return a.index - b.index
      })
      .map(({ row }) => row)
  }
  const tenantCode = resolvePrimaryCatalogTenant()
  const tableName = primaryTableName.value?.trim()
  const alias = resolveSingleTableInternalAlias()
  if (isSingleTableMode.value && tenantCode && tableName) {
    return [
      {
        __catalogTenantCode: tenantCode,
        sourceAlias: alias,
        tableName,
        isPrimary: 1,
      }]
  }
  if (isJoinMode.value && tenantCode && tableName && joinTableName.value?.trim()) {
    const joinAlias = joinTableAlias.value?.trim() || 'B'
    return [
      {
        __catalogTenantCode: tenantCode,
        sourceAlias: alias,
        tableName,
        isPrimary: 1,
      },
      {
        __catalogTenantCode: tenantCode,
        sourceAlias: joinAlias,
        tableName: joinTableName.value.trim(),
        isPrimary: 0,
      }]
  }
  return []
}

/**
 * 进入字段清单步前：写入数据源并加载物理列
 */
async function ensureFieldTreeReady(): Promise<void> {
  ensurePrimaryCatalogTenantFromContext()
  if (isSingleTableMode.value && primaryTableName.value?.trim()) {
    writePrimarySourceRow()
  } else if (
    isJoinMode.value
    && sourceRows.value.length === 0
    && primaryTableName.value?.trim()
    && joinTableName.value?.trim()
    && joinLeftColumn.value?.trim()
    && joinRightColumn.value?.trim()
  ) {
    applyTableJoin()
  }
  await ensureFieldTreeColumnsLoaded()
}

/**
 * 加载字段树所需的全部物理表列
 */
async function ensureFieldTreeColumnsLoaded(): Promise<void> {
  const effectiveSources = resolveFieldTreeSourceRows()
  if (effectiveSources.length === 0) {
    return
  }
  fieldTreeLoading.value = true
  try {
    await Promise.all(
      effectiveSources.map(async (source) => {
        const tenant = String(source.__catalogTenantCode ?? primaryCatalogTenant.value).trim()
        const tableName = String(source.tableName ?? '').trim()
        if (!tenant || !tableName) {
          return
        }
        await loadColumnsForTable(tenant, tableName)
      })
    )
    normalizeFieldRelatedRowOrder()
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    fieldTreeLoading.value = false
  }
}

/**
 * 收集节点下全部列节点
 * @param node 树节点
 * @returns 列节点列表
 */
function collectFieldTreeColumnNodes(node: SqviFieldTreeNode): SqviFieldTreeNode[] {
  if (node.nodeType === 'column') {
    return [node]
  }
  const children = node.children ?? []
  return children.flatMap((child) => collectFieldTreeColumnNodes(child))
}

/**
 * 统计节点下已选输出字段数
 * @param node 树节点
 * @returns 数量
 */
function countFieldTreeOutput(node: SqviFieldTreeNode): number {
  return collectFieldTreeColumnNodes(node).filter((item) =>
    isFieldOutputChecked(item.sourceAlias!, item.columnName!)
  ).length
}

/**
 * 统计节点下已选筛选字段数
 * @param node 树节点
 * @returns 数量
 */
function countFieldTreeSelection(node: SqviFieldTreeNode): number {
  return collectFieldTreeColumnNodes(node).filter((item) =>
    isFieldSelectionChecked(item.sourceAlias!, item.columnName!)
  ).length
}

/**
 * 是否已勾选为报表输出列
 * @param sourceAlias 数据源别名
 * @param columnName 列名
 * @returns 是否勾选
 */
function isFieldOutputChecked(sourceAlias: string, columnName: string): boolean {
  if (isSingleTableMode.value) {
    return fieldRows.value.some((row) =>
      String(row.columnName ?? '').trim() === columnName.trim()
      && Number(row.isVisible) === 1
    )
  }
  return fieldRows.value.some((row) =>
    String(row.sourceAlias ?? '').trim() === sourceAlias.trim()
    && String(row.columnName ?? '').trim() === columnName.trim()
    && Number(row.isVisible) === 1
  )
}

/**
 * 是否已勾选为查询条件
 * @param sourceAlias 数据源别名
 * @param columnName 列名
 * @returns 是否勾选
 */
function isFieldSelectionChecked(sourceAlias: string, columnName: string): boolean {
  if (isSingleTableMode.value) {
    return selectionRows.value.some((row) =>
      String(row.columnName ?? '').trim() === columnName.trim()
    )
  }
  return selectionRows.value.some((row) =>
    String(row.sourceAlias ?? '').trim() === sourceAlias.trim()
    && String(row.columnName ?? '').trim() === columnName.trim()
  )
}

/**
 * 解析列在实体属性清单中的序号（与 schema API 返回顺序一致）
 * @param tenantCode 租户编码
 * @param tableName 物理表名
 * @param columnName 列名
 * @returns 序号；未找到时返回极大值
 */
function resolveEntityColumnIndex(tenantCode: string, tableName: string, columnName: string): number {
  const columns = getCachedColumns(tenantCode, tableName)
  const index = columns.findIndex(
    (col) => col.databaseColumnName.toLowerCase() === columnName.toLowerCase()
  )
  return index >= 0 ? index : Number.MAX_SAFE_INTEGER
}

/**
 * 解析数据源行在字段树中的顺序（主表优先，其余按 sourceRows 顺序）
 * @param row 子表行
 * @returns 排序权重
 */
function resolveSourceRowOrder(row: Record<string, unknown>): number {
  if (isSingleTableMode.value) {
    return 0
  }
  const alias = String(row.sourceAlias ?? '').trim()
  const sources = resolveFieldTreeSourceRows()
  const index = sources.findIndex((source) => String(source.sourceAlias ?? '').trim() === alias)
  return index >= 0 ? index : Number.MAX_SAFE_INTEGER
}

/**
 * 解析子表行所属物理表（用于列序对照）
 * @param row 子表行
 * @returns 租户与表名；无法解析时 null
 */
function resolveRowTableMeta(row: Record<string, unknown>): { tenantCode: string; tableName: string } | null {
  if (isSingleTableMode.value) {
    const primary = resolvePrimarySourceRow() ?? sourceRows.value[0]
    if (!primary) {
      return null
    }
    const tenantCode = String(primary.__catalogTenantCode ?? primaryCatalogTenant.value).trim()
    const tableName = String(primary.tableName ?? '').trim()
    if (!tenantCode || !tableName) {
      return null
    }
    return { tenantCode, tableName }
  }
  const alias = String(row.sourceAlias ?? '').trim()
  const source = sourceRows.value.find((item) => String(item.sourceAlias ?? '').trim() === alias)
  if (!source) {
    return null
  }
  const tenantCode = String(source.__catalogTenantCode ?? primaryCatalogTenant.value).trim()
  const tableName = String(source.tableName ?? '').trim()
  if (!tenantCode || !tableName) {
    return null
  }
  return { tenantCode, tableName }
}

/**
 * 按数据源与实体列声明顺序排序行
 * @param rows 待排序行
 * @returns 新数组（不修改原引用顺序外的对象）
 */
function sortRowsByEntityColumnOrder<T extends Record<string, unknown>>(rows: readonly T[]): T[] {
  return [...rows].sort((a, b) => {
    const sourceOrderA = resolveSourceRowOrder(a)
    const sourceOrderB = resolveSourceRowOrder(b)
    if (sourceOrderA !== sourceOrderB) {
      return sourceOrderA - sourceOrderB
    }
    const tableA = resolveRowTableMeta(a)
    const tableB = resolveRowTableMeta(b)
    const columnA = String(a.columnName ?? '').trim()
    const columnB = String(b.columnName ?? '').trim()
    if (!tableA || !tableB) {
      return columnA.localeCompare(columnB)
    }
    const ordA = resolveEntityColumnIndex(tableA.tenantCode, tableA.tableName, columnA)
    const ordB = resolveEntityColumnIndex(tableB.tenantCode, tableB.tableName, columnB)
    if (ordA !== ordB) {
      return ordA - ordB
    }
    return columnA.localeCompare(columnB)
  })
}

/** 将输出列/选择屏幕列按实体列序重排并刷新 sortOrder */
function normalizeFieldRelatedRowOrder(): void {
  fieldRows.value = sortRowsByEntityColumnOrder(fieldRows.value)
  fieldRows.value.forEach((row, index) => {
    row.sortOrder = index
  })
  selectionRows.value = sortRowsByEntityColumnOrder(selectionRows.value)
  selectionRows.value.forEach((row, index) => {
    row.sortOrder = index + 1
  })
}

/**
 * 解析列元数据
 * @param sourceAlias 数据源别名
 * @param columnName 列名
 * @returns 列信息
 */
function resolveColumnMeta(sourceAlias: string, columnName: string): DatabaseTableColumnInfo | undefined {
  const source = isSingleTableMode.value
    ? (sourceRows.value.find((row) => Number(row.isPrimary) === 1) ?? sourceRows.value[0])
    : sourceRows.value.find(
      (row) => String(row.sourceAlias ?? '').trim() === sourceAlias.trim()
    )
  if (!source) {
    return undefined
  }
  const tenant = String(source.__catalogTenantCode ?? primaryCatalogTenant.value).trim()
  const tableName = String(source.tableName ?? '').trim()
  return getCachedColumns(tenant, tableName).find((col) => col.databaseColumnName === columnName)
}

/**
 * 切换报表输出列勾选
 * @param node 列节点
 * @param checked 是否勾选
 */
function toggleFieldOutput(node: SqviFieldTreeNode, checked: boolean): void {
  const alias = isSingleTableMode.value
    ? resolveSingleTableInternalAlias()
    : String(node.sourceAlias ?? '').trim()
  const column = String(node.columnName ?? '').trim()
  if (!column || (!isSingleTableMode.value && !alias)) {
    return
  }
  const existingIndex = fieldRows.value.findIndex((row) => {
    if (isSingleTableMode.value) {
      return String(row.columnName ?? '').trim() === column
    }
    return String(row.sourceAlias ?? '').trim() === alias
      && String(row.columnName ?? '').trim() === column
  })
  if (checked) {
    const meta = resolveColumnMeta(alias, column)
    if (existingIndex >= 0) {
      fieldRows.value[existingIndex].isVisible = 1
      if (!String(fieldRows.value[existingIndex].displayName ?? '').trim()) {
        fieldRows.value[existingIndex].displayName = meta?.columnComment?.trim() || column
      }
      return
    }
    fieldRows.value.push({
      __rowKey: `new-${Date.now()}`,
      ...scopeDefaults(),
      sourceAlias: alias,
      columnName: column,
      displayName: meta?.columnComment?.trim() || column,
      outputAlias: '',
      aggregateFunc: 0,
      isVisible: 1,
      sortOrder: fieldRows.value.length,
      ExtField: '',
    })
    normalizeFieldRelatedRowOrder()
    return
  }
  if (existingIndex >= 0) {
    fieldRows.value.splice(existingIndex, 1)
  }
  normalizeFieldRelatedRowOrder()
}

/**
 * 切换查询条件勾选
 * @param node 列节点
 * @param checked 是否勾选
 */
function toggleFieldSelection(node: SqviFieldTreeNode, checked: boolean): void {
  const alias = isSingleTableMode.value
    ? resolveSingleTableInternalAlias()
    : String(node.sourceAlias ?? '').trim()
  const column = String(node.columnName ?? '').trim()
  if (!column || (!isSingleTableMode.value && !alias)) {
    return
  }
  const existingIndex = selectionRows.value.findIndex((row) => {
    if (isSingleTableMode.value) {
      return String(row.columnName ?? '').trim() === column
    }
    return String(row.sourceAlias ?? '').trim() === alias
      && String(row.columnName ?? '').trim() === column
  })
  if (checked) {
    if (existingIndex >= 0) {
      return
    }
    const meta = resolveColumnMeta(alias, column)
    selectionRows.value.push({
      __rowKey: `new-${Date.now()}`,
      ...scopeDefaults(),
      sourceAlias: alias,
      columnName: column,
      displayName: meta?.columnComment?.trim() || column,
      filterOperator: 7,
      defaultValue: '',
      defaultValueTo: '',
      isRequired: 0,
      sortOrder: selectionRows.value.length + 1,
    })
    normalizeFieldRelatedRowOrder()
    return
  }
  if (existingIndex >= 0) {
    selectionRows.value.splice(existingIndex, 1)
  }
  normalizeFieldRelatedRowOrder()
}

/**
 * 字段树行样式（已选行高亮）
 * @param record 行数据
 * @returns 行属性
 */
function fieldTreeCustomRow(record: SqviFieldTreeNode) {
  if (record.nodeType !== 'column') {
    return { class: 'designer-field-tree-summary-row' }
  }
  const active = isFieldOutputChecked(record.sourceAlias!, record.columnName!)
    || isFieldSelectionChecked(record.sourceAlias!, record.columnName!)
  return { class: active ? 'designer-field-tree-active-row' : '' }
}

/**
 * 按行解析可选列（单表=主表列；连接=所选别名对应表列）
 * @param record 子表行
 * @returns 列选项
 */
function resolveColumnOptionsForRecord(record: Record<string, unknown>) {
  if (isSingleTableMode.value) {
    return primaryColumnOptions.value
  }
  const alias = String(record.sourceAlias ?? primaryAlias.value ?? 'A').trim()
  return getColumnOptionsForAlias(alias)
}

/**
 * 按数据源别名获取缓存列
 * @param alias 数据源别名
 * @returns 列选项
 */
function getColumnOptionsForAlias(alias: string) {
  const matched = configuredSourceAliasOptions.value.find((item) => item.value === alias)
  if (!matched) {
    return primaryColumnOptions.value
  }
  return getCachedColumns(matched.catalogTenant, matched.tableName)
}

/**
 * 选择屏幕列加载态
 * @param record 行
 * @returns 是否 loading
 */
function isSelectionColumnsLoading(record: Record<string, unknown>): boolean {
  if (isSingleTableMode.value) {
    return primaryColumnsLoading.value
  }
  const alias = String(record.sourceAlias ?? primaryAlias.value ?? 'A').trim()
  const matched = configuredSourceAliasOptions.value.find((item) => item.value === alias)
  if (!matched) {
    return false
  }
  return isColumnsLoading(matched.catalogTenant, matched.tableName)
}

/**
 * 分组列加载态
 * @param record 行
 * @returns 是否 loading
 */
function isGroupByColumnsLoading(record: Record<string, unknown>): boolean {
  return isSelectionColumnsLoading(record)
}

/**
 * 排序列加载态
 * @param record 行
 * @returns 是否 loading
 */
function isOrderByColumnsLoading(record: Record<string, unknown>): boolean {
  return isSelectionColumnsLoading(record)
}

/**
 * 单表模式：仅保留主表数据源并清空关联
 */
function pruneSourcesForSingleTable(): void {
  const primaryRow = resolvePrimarySourceRow()
  if (primaryRow) {
    primaryRow.isPrimary = 1
    if (!String(primaryRow.__catalogTenantCode ?? '').trim()) {
      primaryRow.__catalogTenantCode = resolvePrimaryCatalogTenant()
    }
    sourceRows.value = [primaryRow]
  } else {
    sourceRows.value = []
  }
  joinRows.value.splice(0, joinRows.value.length)
}

/**
 * 将当前选表写入数据源子表（单表/连接主表共用）
 * @returns 是否成功写入至少一行数据源
 */
function writePrimarySourceRow(): boolean {
  ensurePrimaryCatalogTenantFromContext()
  const tenantCode = resolvePrimaryCatalogTenant()
  const tableName = primaryTableName.value?.trim()
  const alias = resolvePrimarySourceAlias()
  if (!tenantCode || !tableName) {
    return false
  }
  primaryCatalogTenant.value = tenantCode
  const scope = scopeDefaults()
  let row = resolvePrimarySourceRow()
  if (!row) {
    row = {
      __rowKey: `new-${Date.now()}`,
      __catalogTenantCode: tenantCode,
      ...scope,
      sourceAlias: alias,
      tableName,
      isPrimary: 1,
      sortOrder: 0,
      ExtField: '',
      remark: '',
    }
    sourceRows.value.push(row)
  } else {
    row.__catalogTenantCode = tenantCode
    row.sourceAlias = alias
    row.tableName = tableName
    row.isPrimary = 1
    Object.assign(row, scope)
  }
  for (const other of sourceRows.value) {
    if (other !== row && isPrimarySourceRow(other)) {
      other.isPrimary = 0
    }
  }
  for (const field of fieldRows.value) {
    if (!field.sourceAlias || field.sourceAlias === alias) {
      field.sourceAlias = alias
    }
  }
  for (const sel of selectionRows.value) {
    if (!sel.sourceAlias || sel.sourceAlias === alias) {
      sel.sourceAlias = alias
    }
  }
  if (isSingleTableMode.value) {
    pruneSourcesForSingleTable()
  }
  void loadPrimaryColumns()
  return sourceRows.value.some(
    (item) => isPrimarySourceRow(item) && String(item.tableName ?? '').trim() === tableName
  )
}

/**
 * 应用主表到数据源子表
 */
function applyPrimarySource(): void {
  writePrimarySourceRow()
}

/**
 * 加载主表列
 */
async function loadPrimaryColumns(): Promise<void> {
  const tenantCode = primaryCatalogTenant.value?.trim()
  const tableName = primaryTableName.value?.trim()
  if (!tenantCode || !tableName) {
    return
  }
  try {
    await loadColumnsForTable(tenantCode, tableName)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/**
 * 主表变更
 */
function handlePrimaryTableChange(): void {
  ensurePrimaryCatalogTenantFromContext()
  if (primaryTableName.value?.trim()) {
    writePrimarySourceRow()
  }
}

/**
 * 加载连接表列
 */
async function loadJoinColumns(): Promise<void> {
  const tenantCode = primaryCatalogTenant.value?.trim()
  const tableName = joinTableName.value?.trim()
  if (!tenantCode || !tableName) {
    return
  }
  try {
    await loadColumnsForTable(tenantCode, tableName)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/**
 * 连接表变更
 */
function handleJoinTableChange(): void {
  joinLeftColumn.value = ''
  joinRightColumn.value = ''
  loadJoinColumns()
}

/**
 * 应用表连接（主表 + 连接表 + JOIN 条件写入子表）
 * @returns 是否成功
 */
function applyTableJoin(): boolean {
  const tenantCode = primaryCatalogTenant.value?.trim()
  const leftTable = primaryTableName.value?.trim()
  const rightTable = joinTableName.value?.trim()
  const leftAlias = primaryAlias.value?.trim() || 'A'
  const rightAlias = joinTableAlias.value?.trim() || 'B'
  const leftCol = joinLeftColumn.value?.trim()
  const rightCol = joinRightColumn.value?.trim()
  if (!tenantCode || !leftTable || !rightTable) {
    message.warning(t('statistics.quickquery.configurable.page.designer.join.incomplete'))
    return false
  }
  if (!leftCol || !rightCol) {
    message.warning(t('statistics.quickquery.configurable.page.designer.join.conditionrequired'))
    return false
  }
  if (leftTable === rightTable && leftAlias === rightAlias) {
    message.warning(t('statistics.quickquery.configurable.page.designer.join.samealias'))
    return false
  }
  applyPrimarySource()
  const scope = scopeDefaults()
  let rightSource = sourceRows.value.find(
    (row) => String(row.sourceAlias ?? '').trim() === rightAlias
  )
  if (!rightSource) {
    rightSource = {
      __rowKey: `new-${Date.now()}-join`,
      __catalogTenantCode: tenantCode,
      ...scope,
      sourceAlias: rightAlias,
      tableName: rightTable,
      isPrimary: 0,
      sortOrder: 1,
      ExtField: '',
      remark: '',
    }
    sourceRows.value.push(rightSource)
  } else {
    rightSource.__catalogTenantCode = tenantCode
    rightSource.sourceAlias = rightAlias
    rightSource.tableName = rightTable
    rightSource.isPrimary = 0
    Object.assign(rightSource, scope)
  }
  const keepAliases = new Set([leftAlias, rightAlias])
  for (let index = sourceRows.value.length - 1; index >= 0; index -= 1) {
    const alias = String(sourceRows.value[index].sourceAlias ?? '').trim()
    if (!keepAliases.has(alias)) {
      sourceRows.value.splice(index, 1)
    }
  }
  let joinRow = joinRows.value[0] as Record<string, unknown> | undefined
  if (!joinRow) {
    joinRow = {
      __rowKey: `new-${Date.now()}-join-row`,
      ...scope,
      joinType: joinType.value,
      leftSourceAlias: leftAlias,
      leftColumnName: leftCol,
      rightSourceAlias: rightAlias,
      rightColumnName: rightCol,
      sortOrder: 0,
      ExtField: '',
      remark: '',
    }
    joinRows.value.push(joinRow)
  } else {
    joinRow.joinType = joinType.value
    joinRow.leftSourceAlias = leftAlias
    joinRow.leftColumnName = leftCol
    joinRow.rightSourceAlias = rightAlias
    joinRow.rightColumnName = rightCol
    Object.assign(joinRow, scope)
  }
  if (joinRows.value.length > 1) {
    joinRows.value.splice(1)
  }
  return true
}

/**
 * 主表租户变更
 */
async function handlePrimaryTenantChange(): Promise<void> {
  primaryTableName.value = ''
  joinTableName.value = ''
  joinLeftColumn.value = ''
  joinRightColumn.value = ''
  const code = primaryCatalogTenant.value
  if (!code) {
    return
  }
  try {
    await loadTablesForTenant(code)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/**
 * 分组数据源别名变更
 * @param record 行
 */
function handleGroupByAliasChange(record: Record<string, unknown>): void {
  record.columnName = ''
}

/**
 * 排序数据源别名变更
 * @param record 行
 */
function handleOrderByAliasChange(record: Record<string, unknown>): void {
  record.columnName = ''
}

/**
 * 新增分组行
 */
function addGroupByRow(): void {
  const alias = isJoinMode.value
    ? (configuredSourceAliasOptions.value[0]?.value ?? primaryAlias.value?.trim() ?? 'A')
    : resolveSingleTableInternalAlias()
  groupByRows.value.push({
    __rowKey: `new-${Date.now()}`,
    ...scopeDefaults(),
    sourceAlias: alias,
    columnName: '',
    sortOrder: groupByRows.value.length,
    ExtField: '',
    remark: '',
  })
}

/**
 * 删除分组行
 * @param index 行索引
 */
function removeGroupByRow(index: number): void {
  groupByRows.value.splice(index, 1)
}

/**
 * 新增排序行
 */
function addOrderByRow(): void {
  const alias = isJoinMode.value
    ? (configuredSourceAliasOptions.value[0]?.value ?? primaryAlias.value?.trim() ?? 'A')
    : resolveSingleTableInternalAlias()
  orderByRows.value.push({
    __rowKey: `new-${Date.now()}`,
    ...scopeDefaults(),
    sourceAlias: alias,
    columnName: '',
    sortDirection: 'ASC',
    sortOrder: orderByRows.value.length,
    ExtField: '',
    remark: '',
  })
}

/**
 * 删除排序行
 * @param index 行索引
 */
function removeOrderByRow(index: number): void {
  orderByRows.value.splice(index, 1)
}

onMounted(async () => {
  try {
    await loadDatabaseInfoList()
    ensurePrimaryCatalogTenantFromContext()
    const tenantCode = resolvePrimaryCatalogTenant()
    if (tenantCode) {
      await loadTablesForTenant(tenantCode)
    }
    syncPrimaryPickerFromSources()
    if (primaryTableName.value) {
      await loadPrimaryColumns()
    }
    if (joinTableName.value) {
      await loadJoinColumns()
    }
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
})

/** 上次灌入的 formData 引用（避免 deep watch 清空向导中的 sourceRows） */
let lastHydratedFormDataRef: unknown

/**
 * 从父级 formData 灌入主表与子表行
 * @param val 父级传入 DTO
 */
function hydrateFromFormData(
  val: Partial<ConfigurableCreate & { configurableId?: string }> | null | undefined
): void {
  const isEdit = Boolean((val as any)?.configurableId)
  const next = val ? { ...val } : {}
  Object.keys(formState).forEach((k) => delete formState[k])
  delete (next as any).sources
  delete (next as any).joins
  delete (next as any).fields
  delete (next as any).selections
  delete (next as any).groupBys
  delete (next as any).orderBys
  applyScopeDefaults(next)
  applyCreateFieldDefaults(next)
  Object.assign(formState, next)
  if (!isEdit) {
    resetWizardPickerState()
  }
  syncChildRowsFromFormData(val)
  if (isEdit) {
    syncPrimaryPickerFromSources()
    sourceTypeLocked.value = true
  }
  currentStepIndex.value = 0
}

watch(
  () => props.formData,
  (val) => {
    if (val === lastHydratedFormDataRef) {
      return
    }
    const isCreateSession = !(val as any)?.configurableId
    if (
      isCreateSession
      && lastHydratedFormDataRef != null
      && currentStepIndex.value > 0
      && sourceRows.value.length > 0
    ) {
      lastHydratedFormDataRef = val
      return
    }
    lastHydratedFormDataRef = val
    hydrateFromFormData(val)
  },
  { immediate: true }
)

watch(currentStepId, async (stepId) => {
  if (stepId === 'fields') {
    await ensureFieldTreeReady()
  }
})

watch(wizardSteps, (steps) => {
  const max = Math.max(0, steps.length - 1)
  if (currentStepIndex.value > max) {
    currentStepIndex.value = max
  }
})

watch(
  () => [
    tenantStore.tenantCode,
    tenantStore.companyCode,
    tenantStore.currentCompanyRelatedPlant,
    userStore.userInfo?.companyDefaultCulture,
  ] as const,
  () => {
    applyScopeDefaults(formState, !props.formData?.configurableId)
    if (!props.formData?.configurableId) {
      ensurePrimaryCatalogTenantFromContext()
    }
  }
)

watch(
  () => databaseInfoList.value.length,
  () => {
    if (!props.formData?.configurableId) {
      ensurePrimaryCatalogTenantFromContext()
    }
  }
)

watch(
  () => sourceRows.value.length,
  () => syncPrimaryPickerFromSources()
)

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
.configurable-designer-form {
  min-height: 50vh;
}

.configurable-designer-form :deep(.ant-form-item) {
  margin-bottom: 12px;
}

.configurable-designer-form :deep(.designer-field-tree-active-row > td) {
  background-color: color-mix(in srgb, var(--takt-cn-feicui, #7cb342) 16%, transparent);
}

.configurable-designer-form :deep(.designer-field-tree-summary-row > td) {
  font-weight: 500;
}
</style>
