<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/generator/gen-table -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selecmed-master-key="selecmedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getGenTableId"
      :master-row-selection="rowSelection"
      master-id-column-key="genTableId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="tenant"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="code:generator:gen:table:create"
      update-permission="code:generator:gen:table:update"
      delete-permission="code:generator:gen:table:delete"
      import-permission="code:generator:gen:table:import"
      export-permission="code:generator:gen:table:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
        />
      </template>
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'inDatabase'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'inDatabase')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'genTemplateCategory'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'genTemplateCategory')"
            dict-type="code_generator_template_type"
          />
        </template>
        <template v-else-if="column.key === 'menuButtonGroup'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'menuButtonGroup')"
            dict-type="code_generator_button_category"
          />
        </template>
        <template v-else-if="column.key === 'isRepository'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'isRepository')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'genFunction'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'genFunction')"
            dict-type="code_generator_function"
          />
        </template>
        <template v-else-if="column.key === 'genMethod'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'genMethod')"
            dict-type="code_generator_method"
          />
        </template>
        <template v-else-if="column.key === 'genPath'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'genPath')"
            dict-type="code_generator_path_type"
          />
        </template>
        <template v-else-if="column.key === 'isGenMenu'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'isGenMenu')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'isGenTranslation'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'isGenTranslation')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'sortType'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'sortType')"
            dict-type="sys_sort_type"
          />
        </template>
        <template v-else-if="column.key === 'frontUi'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'frontUi')"
            dict-type="code_generator_frontend_ui_type"
          />
        </template>
        <template v-else-if="column.key === 'frontFormLayout'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'frontFormLayout')"
            dict-type="code_generator_frontend_form_layout"
          />
        </template>
        <template v-else-if="column.key === 'frontBtnStyle'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'frontBtnStyle')"
            dict-type="code_generator_button_style"
          />
        </template>
        <template v-else-if="column.key === 'isGenCode'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'isGenCode')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'isUseTabs'">
          <TaktDictTag
            :value="getGenTableDictValue(record, 'isUseTabs')"
            dict-type="sys_yes_no"
          />
        </template>
      </template>
      <template #detail>
        <GenTableColumnPanel
          ref="genTableColumnPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

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
      <GenTableForm
        :key="formData?.genTableId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-code-generator-gen-table'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('dataSource')">
      <a-form-item :label="pi.queryLabel('dataSource')">
        <a-input
          v-model:value="advancedQueryForm.dataSource"
          :placeholder="pi.queryPh('dataSource', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tableName')">
      <a-form-item :label="pi.queryLabel('tableName')">
        <a-input
          v-model:value="advancedQueryForm.tableName"
          :placeholder="pi.queryPh('tableName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tableComment')">
      <a-form-item :label="pi.queryLabel('tableComment')">
        <a-input
          v-model:value="advancedQueryForm.tableComment"
          :placeholder="pi.queryPh('tableComment', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subTableName')">
      <a-form-item :label="pi.queryLabel('subTableName')">
        <a-input
          v-model:value="advancedQueryForm.subTableName"
          :placeholder="pi.queryPh('subTableName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subTableFkName')">
      <a-form-item :label="pi.queryLabel('subTableFkName')">
        <a-input
          v-model:value="advancedQueryForm.subTableFkName"
          :placeholder="pi.queryPh('subTableFkName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('treeCode')">
      <a-form-item :label="pi.queryLabel('treeCode')">
        <a-input
          v-model:value="advancedQueryForm.treeCode"
          :placeholder="pi.queryPh('treeCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('treeParentCode')">
      <a-form-item :label="pi.queryLabel('treeParentCode')">
        <a-input
          v-model:value="advancedQueryForm.treeParentCode"
          :placeholder="pi.queryPh('treeParentCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('treeName')">
      <a-form-item :label="pi.queryLabel('treeName')">
        <a-input
          v-model:value="advancedQueryForm.treeName"
          :placeholder="pi.queryPh('treeName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inDatabase')">
      <a-form-item :label="pi.queryLabel('inDatabase')">
        <TaktSelect
          v-model:value="advancedQueryForm.inDatabase"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('inDatabase', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genTemplateCategory')">
      <a-form-item :label="pi.queryLabel('genTemplateCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.genTemplateCategory"
          dict-type="code_generator_template_type"
          :placeholder="pi.queryPh('genTemplateCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genModuleName')">
      <a-form-item :label="pi.queryLabel('genModuleName')">
        <a-input
          v-model:value="advancedQueryForm.genModuleName"
          :placeholder="pi.queryPh('genModuleName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genBusinessName')">
      <a-form-item :label="pi.queryLabel('genBusinessName')">
        <a-input
          v-model:value="advancedQueryForm.genBusinessName"
          :placeholder="pi.queryPh('genBusinessName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genFunctionName')">
      <a-form-item :label="pi.queryLabel('genFunctionName')">
        <a-input
          v-model:value="advancedQueryForm.genFunctionName"
          :placeholder="pi.queryPh('genFunctionName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('permsPrefix')">
      <a-form-item :label="pi.queryLabel('permsPrefix')">
        <a-input
          v-model:value="advancedQueryForm.permsPrefix"
          :placeholder="pi.queryPh('permsPrefix', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('menuButtonGroup')">
      <a-form-item :label="pi.queryLabel('menuButtonGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.menuButtonGroup"
          dict-type="code_generator_button_category"
          :placeholder="pi.queryPh('menuButtonGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('namePrefix')">
      <a-form-item :label="pi.queryLabel('namePrefix')">
        <a-input
          v-model:value="advancedQueryForm.namePrefix"
          :placeholder="pi.queryPh('namePrefix', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('entityNamespace')">
      <a-form-item :label="pi.queryLabel('entityNamespace')">
        <a-input
          v-model:value="advancedQueryForm.entityNamespace"
          :placeholder="pi.queryPh('entityNamespace', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('entityClassName')">
      <a-form-item :label="pi.queryLabel('entityClassName')">
        <a-input
          v-model:value="advancedQueryForm.entityClassName"
          :placeholder="pi.queryPh('entityClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dtoNamespace')">
      <a-form-item :label="pi.queryLabel('dtoNamespace')">
        <a-input
          v-model:value="advancedQueryForm.dtoNamespace"
          :placeholder="pi.queryPh('dtoNamespace', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dtoClassName')">
      <a-form-item :label="pi.queryLabel('dtoClassName')">
        <a-input
          v-model:value="advancedQueryForm.dtoClassName"
          :placeholder="pi.queryPh('dtoClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceNamespace')">
      <a-form-item :label="pi.queryLabel('serviceNamespace')">
        <a-input
          v-model:value="advancedQueryForm.serviceNamespace"
          :placeholder="pi.queryPh('serviceNamespace', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('iServiceClassName')">
      <a-form-item :label="pi.queryLabel('iServiceClassName')">
        <a-input
          v-model:value="advancedQueryForm.iServiceClassName"
          :placeholder="pi.queryPh('iServiceClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceClassName')">
      <a-form-item :label="pi.queryLabel('serviceClassName')">
        <a-input
          v-model:value="advancedQueryForm.serviceClassName"
          :placeholder="pi.queryPh('serviceClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('controllerNamespace')">
      <a-form-item :label="pi.queryLabel('controllerNamespace')">
        <a-input
          v-model:value="advancedQueryForm.controllerNamespace"
          :placeholder="pi.queryPh('controllerNamespace', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('controllerClassName')">
      <a-form-item :label="pi.queryLabel('controllerClassName')">
        <a-input
          v-model:value="advancedQueryForm.controllerClassName"
          :placeholder="pi.queryPh('controllerClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isRepository')">
      <a-form-item :label="pi.queryLabel('isRepository')">
        <TaktSelect
          v-model:value="advancedQueryForm.isRepository"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isRepository', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('repositoryInterfaceNamespace')">
      <a-form-item :label="pi.queryLabel('repositoryInterfaceNamespace')">
        <a-input
          v-model:value="advancedQueryForm.repositoryInterfaceNamespace"
          :placeholder="pi.queryPh('repositoryInterfaceNamespace', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('iRepositoryClassName')">
      <a-form-item :label="pi.queryLabel('iRepositoryClassName')">
        <a-input
          v-model:value="advancedQueryForm.iRepositoryClassName"
          :placeholder="pi.queryPh('iRepositoryClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('repositoryNamespace')">
      <a-form-item :label="pi.queryLabel('repositoryNamespace')">
        <a-input
          v-model:value="advancedQueryForm.repositoryNamespace"
          :placeholder="pi.queryPh('repositoryNamespace', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('repositoryClassName')">
      <a-form-item :label="pi.queryLabel('repositoryClassName')">
        <a-input
          v-model:value="advancedQueryForm.repositoryClassName"
          :placeholder="pi.queryPh('repositoryClassName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genFunction')">
      <a-form-item :label="pi.queryLabel('genFunction')">
        <TaktSelect
          v-model:value="advancedQueryForm.genFunction"
          dict-type="code_generator_function"
          :placeholder="pi.queryPh('genFunction', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genMethod')">
      <a-form-item :label="pi.queryLabel('genMethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.genMethod"
          dict-type="code_generator_method"
          :placeholder="pi.queryPh('genMethod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genPath')">
      <a-form-item :label="pi.queryLabel('genPath')">
        <TaktSelect
          v-model:value="advancedQueryForm.genPath"
          dict-type="code_generator_path_type"
          :placeholder="pi.queryPh('genPath', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isGenMenu')">
      <a-form-item :label="pi.queryLabel('isGenMenu')">
        <TaktSelect
          v-model:value="advancedQueryForm.isGenMenu"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isGenMenu', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMenuId')">
      <a-form-item :label="pi.queryLabel('parentMenuId')">
        <TaktSelect
          v-model:value="advancedQueryForm.parentMenuId"
          api-url="TaktMenus/tree-options"
          :placeholder="pi.queryPh('parentMenuId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isGenTranslation')">
      <a-form-item :label="pi.queryLabel('isGenTranslation')">
        <TaktSelect
          v-model:value="advancedQueryForm.isGenTranslation"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isGenTranslation', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortField')">
      <a-form-item :label="pi.queryLabel('sortField')">
        <a-input
          v-model:value="advancedQueryForm.sortField"
          :placeholder="pi.queryPh('sortField', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortType')">
      <a-form-item :label="pi.queryLabel('sortType')">
        <TaktSelect
          v-model:value="advancedQueryForm.sortType"
          dict-type="sys_sort_type"
          :placeholder="pi.queryPh('sortType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('frontUi')">
      <a-form-item :label="pi.queryLabel('frontUi')">
        <TaktSelect
          v-model:value="advancedQueryForm.frontUi"
          dict-type="code_generator_frontend_ui_type"
          :placeholder="pi.queryPh('frontUi', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('frontFormLayout')">
      <a-form-item :label="pi.queryLabel('frontFormLayout')">
        <TaktSelect
          v-model:value="advancedQueryForm.frontFormLayout"
          dict-type="code_generator_frontend_form_layout"
          :placeholder="pi.queryPh('frontFormLayout', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('frontBtnStyle')">
      <a-form-item :label="pi.queryLabel('frontBtnStyle')">
        <TaktSelect
          v-model:value="advancedQueryForm.frontBtnStyle"
          dict-type="code_generator_button_style"
          :placeholder="pi.queryPh('frontBtnStyle', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isGenCode')">
      <a-form-item :label="pi.queryLabel('isGenCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.isGenCode"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isGenCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genCodeCount')">
      <a-form-item :label="pi.queryLabel('genCodeCount')">
        <a-input-number
          v-model:value="advancedQueryForm.genCodeCount"
          :placeholder="pi.queryPh('genCodeCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isUseTabs')">
      <a-form-item :label="pi.queryLabel('isUseTabs')">
        <TaktSelect
          v-model:value="advancedQueryForm.isUseTabs"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isUseTabs', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tabsFieldCount')">
      <a-form-item :label="pi.queryLabel('tabsFieldCount')">
        <a-input-number
          v-model:value="advancedQueryForm.tabsFieldCount"
          :placeholder="pi.queryPh('tabsFieldCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genAuthor')">
      <a-form-item :label="pi.queryLabel('genAuthor')">
        <a-input
          v-model:value="advancedQueryForm.genAuthor"
          :placeholder="pi.queryPh('genAuthor', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherGenOptions')">
      <a-form-item :label="pi.queryLabel('otherGenOptions')">
        <a-input
          v-model:value="advancedQueryForm.otherGenOptions"
          :placeholder="pi.queryPh('otherGenOptions', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="GENTABLE_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'genTableId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/code/generator/gen-table
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import GenTableForm from './components/GenTableForm.vue'
import GenTableColumnPanel from './components/GenTableColumnPanel.vue'
import { provideGenTableMasterContext, type GenTableRowRecord } from './composables/useGenTableMasterContext'
import { getGenTableList, getGenTableById, createGenTable, updateGenTable, deleteGenTableById, deleteGenTableBatch, getGenTableTemplate, importGenTable, exportGenTable } from '@/api/code/generator/genTable.api'
import type { GenTable, GenTableQuery } from '@/types/code/generator/genTable.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useGenTableI18n,
  GENTABLE_LIST_FIELDS,
  GENTABLE_QUERY_STRING_FIELDS,
  GENTABLE_QUERY_FIELDS,
  GENTABLE_SELF_I18N_KEY,
} from './composables/useGenTableI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useGenTableI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktGenTable')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<GenTable[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<GenTableRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<GenTableRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<GenTable> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of GENTABLE_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.inDatabase !== undefined && form.inDatabase !== null) {
    return true
  }
  if (form.isRepository !== undefined && form.isRepository !== null) {
    return true
  }
  if (form.genMethod !== undefined && form.genMethod !== null) {
    return true
  }
  if (form.isGenMenu !== undefined && form.isGenMenu !== null) {
    return true
  }
  if (form.isGenTranslation !== undefined && form.isGenTranslation !== null) {
    return true
  }
  if (form.frontUi !== undefined && form.frontUi !== null) {
    return true
  }
  if (form.frontFormLayout !== undefined && form.frontFormLayout !== null) {
    return true
  }
  if (form.frontBtnStyle !== undefined && form.frontBtnStyle !== null) {
    return true
  }
  if (form.isGenCode !== undefined && form.isGenCode !== null) {
    return true
  }
  if (form.genCodeCount !== undefined && form.genCodeCount !== null) {
    return true
  }
  if (form.isUseTabs !== undefined && form.isUseTabs !== null) {
    return true
  }
  if (form.tabsFieldCount !== undefined && form.tabsFieldCount !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(GENTABLE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof GENTABLE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    inDatabase: undefined as number | undefined,
    isRepository: undefined as number | undefined,
    genMethod: undefined as number | undefined,
    isGenMenu: undefined as number | undefined,
    isGenTranslation: undefined as number | undefined,
    frontUi: undefined as number | undefined,
    frontFormLayout: undefined as number | undefined,
    frontBtnStyle: undefined as number | undefined,
    isGenCode: undefined as number | undefined,
    genCodeCount: undefined as number | undefined,
    isUseTabs: undefined as number | undefined,
    tabsFieldCount: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  GENTABLE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'genTableId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = provideGenTableMasterContext()
const genTableColumnPanelRef = ref<InstanceType<typeof GenTableColumnPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {GenTableQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<GenTableQuery>): GenTableQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: GenTableQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof GenTableQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of GENTABLE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.inDatabase !== undefined && form.inDatabase !== null) {
    query.inDatabase = form.inDatabase
  }
  if (form.isRepository !== undefined && form.isRepository !== null) {
    query.isRepository = form.isRepository
  }
  if (form.genMethod !== undefined && form.genMethod !== null) {
    query.genMethod = form.genMethod
  }
  if (form.isGenMenu !== undefined && form.isGenMenu !== null) {
    query.isGenMenu = form.isGenMenu
  }
  if (form.isGenTranslation !== undefined && form.isGenTranslation !== null) {
    query.isGenTranslation = form.isGenTranslation
  }
  if (form.frontUi !== undefined && form.frontUi !== null) {
    query.frontUi = form.frontUi
  }
  if (form.frontFormLayout !== undefined && form.frontFormLayout !== null) {
    query.frontFormLayout = form.frontFormLayout
  }
  if (form.frontBtnStyle !== undefined && form.frontBtnStyle !== null) {
    query.frontBtnStyle = form.frontBtnStyle
  }
  if (form.isGenCode !== undefined && form.isGenCode !== null) {
    query.isGenCode = form.isGenCode
  }
  if (form.genCodeCount !== undefined && form.genCodeCount !== null) {
    query.genCodeCount = form.genCodeCount
  }
  if (form.isUseTabs !== undefined && form.isUseTabs !== null) {
    query.isUseTabs = form.isUseTabs
  }
  if (form.tabsFieldCount !== undefined && form.tabsFieldCount !== null) {
    query.tabsFieldCount = form.tabsFieldCount
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 主表行点击选中 key（左右主子表高亮） */
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: GenTableRowRecord | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? getGenTableId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as GenTableRowRecord
  const key = getGenTableId(row)
  selecmedRowKeys.value = [key]
  selecmedRows.value = [row]
  selecmedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadGenTableDetail(record: GenTableRowRecord): Promise<GenTable | null> {
  const id = getGenTableId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getGenTableById(id)
    const index = dataSource.value.findIndex((row) => getGenTableId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as GenTable
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'genTableId',
    key: 'genTableId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'genTableId') ?? ''
  },
  {
    title: pi.label('dataSource'),
    dataIndex: 'dataSource',
    key: 'dataSource',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'dataSource') ?? ''
  },
  {
    title: pi.label('tableName'),
    dataIndex: 'tableName',
    key: 'tableName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'tableName') ?? ''
  },
  {
    title: pi.label('tableComment'),
    dataIndex: 'tableComment',
    key: 'tableComment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'tableComment') ?? ''
  },
  {
    title: pi.label('subTableName'),
    dataIndex: 'subTableName',
    key: 'subTableName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'subTableName') ?? ''
  },
  {
    title: pi.label('subTableFkName'),
    dataIndex: 'subTableFkName',
    key: 'subTableFkName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'subTableFkName') ?? ''
  },
  {
    title: pi.label('treeCode'),
    dataIndex: 'treeCode',
    key: 'treeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'treeCode') ?? ''
  },
  {
    title: pi.label('treeParentCode'),
    dataIndex: 'treeParentCode',
    key: 'treeParentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'treeParentCode') ?? ''
  },
  {
    title: pi.label('treeName'),
    dataIndex: 'treeName',
    key: 'treeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'treeName') ?? ''
  },
  {
    title: pi.label('inDatabase'),
    dataIndex: 'inDatabase',
    key: 'inDatabase',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('genTemplateCategory'),
    dataIndex: 'genTemplateCategory',
    key: 'genTemplateCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('genModuleName'),
    dataIndex: 'genModuleName',
    key: 'genModuleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'genModuleName') ?? ''
  },
  {
    title: pi.label('genBusinessName'),
    dataIndex: 'genBusinessName',
    key: 'genBusinessName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'genBusinessName') ?? ''
  },
  {
    title: pi.label('genFunctionName'),
    dataIndex: 'genFunctionName',
    key: 'genFunctionName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'genFunctionName') ?? ''
  },
  {
    title: pi.label('permsPrefix'),
    dataIndex: 'permsPrefix',
    key: 'permsPrefix',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'permsPrefix') ?? ''
  },
  {
    title: pi.label('menuButtonGroup'),
    dataIndex: 'menuButtonGroup',
    key: 'menuButtonGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('namePrefix'),
    dataIndex: 'namePrefix',
    key: 'namePrefix',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'namePrefix') ?? ''
  },
  {
    title: pi.label('entityNamespace'),
    dataIndex: 'entityNamespace',
    key: 'entityNamespace',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'entityNamespace') ?? ''
  },
  {
    title: pi.label('entityClassName'),
    dataIndex: 'entityClassName',
    key: 'entityClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'entityClassName') ?? ''
  },
  {
    title: pi.label('dtoNamespace'),
    dataIndex: 'dtoNamespace',
    key: 'dtoNamespace',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'dtoNamespace') ?? ''
  },
  {
    title: pi.label('dtoClassName'),
    dataIndex: 'dtoClassName',
    key: 'dtoClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'dtoClassName') ?? ''
  },
  {
    title: pi.label('serviceNamespace'),
    dataIndex: 'serviceNamespace',
    key: 'serviceNamespace',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'serviceNamespace') ?? ''
  },
  {
    title: pi.label('iServiceClassName'),
    dataIndex: 'iServiceClassName',
    key: 'iServiceClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'iServiceClassName') ?? ''
  },
  {
    title: pi.label('serviceClassName'),
    dataIndex: 'serviceClassName',
    key: 'serviceClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'serviceClassName') ?? ''
  },
  {
    title: pi.label('controllerNamespace'),
    dataIndex: 'controllerNamespace',
    key: 'controllerNamespace',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'controllerNamespace') ?? ''
  },
  {
    title: pi.label('controllerClassName'),
    dataIndex: 'controllerClassName',
    key: 'controllerClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'controllerClassName') ?? ''
  },
  {
    title: pi.label('isRepository'),
    dataIndex: 'isRepository',
    key: 'isRepository',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('repositoryInterfaceNamespace'),
    dataIndex: 'repositoryInterfaceNamespace',
    key: 'repositoryInterfaceNamespace',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'repositoryInterfaceNamespace') ?? ''
  },
  {
    title: pi.label('iRepositoryClassName'),
    dataIndex: 'iRepositoryClassName',
    key: 'iRepositoryClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'iRepositoryClassName') ?? ''
  },
  {
    title: pi.label('repositoryNamespace'),
    dataIndex: 'repositoryNamespace',
    key: 'repositoryNamespace',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'repositoryNamespace') ?? ''
  },
  {
    title: pi.label('repositoryClassName'),
    dataIndex: 'repositoryClassName',
    key: 'repositoryClassName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'repositoryClassName') ?? ''
  },
  {
    title: pi.label('genFunction'),
    dataIndex: 'genFunction',
    key: 'genFunction',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('genMethod'),
    dataIndex: 'genMethod',
    key: 'genMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('genPath'),
    dataIndex: 'genPath',
    key: 'genPath',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isGenMenu'),
    dataIndex: 'isGenMenu',
    key: 'isGenMenu',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('parentMenuId'),
    dataIndex: 'parentMenuId',
    key: 'parentMenuId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'parentMenuId') ?? ''
  },
  {
    title: pi.label('parentMenuName'),
    dataIndex: 'parentMenuName',
    key: 'parentMenuName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'parentMenuName') ?? ''
  },
  {
    title: pi.label('isGenTranslation'),
    dataIndex: 'isGenTranslation',
    key: 'isGenTranslation',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('sortField'),
    dataIndex: 'sortField',
    key: 'sortField',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'sortField') ?? ''
  },
  {
    title: pi.label('sortType'),
    dataIndex: 'sortType',
    key: 'sortType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('frontUi'),
    dataIndex: 'frontUi',
    key: 'frontUi',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('frontFormLayout'),
    dataIndex: 'frontFormLayout',
    key: 'frontFormLayout',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('frontBtnStyle'),
    dataIndex: 'frontBtnStyle',
    key: 'frontBtnStyle',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isGenCode'),
    dataIndex: 'isGenCode',
    key: 'isGenCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('genCodeCount'),
    dataIndex: 'genCodeCount',
    key: 'genCodeCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'genCodeCount') ?? ''
  },
  {
    title: pi.label('isUseTabs'),
    dataIndex: 'isUseTabs',
    key: 'isUseTabs',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('tabsFieldCount'),
    dataIndex: 'tabsFieldCount',
    key: 'tabsFieldCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'tabsFieldCount') ?? ''
  },
  {
    title: pi.label('genAuthor'),
    dataIndex: 'genAuthor',
    key: 'genAuthor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'genAuthor') ?? ''
  },
  {
    title: pi.label('otherGenOptions'),
    dataIndex: 'otherGenOptions',
    key: 'otherGenOptions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getGenTableField(record, 'otherGenOptions') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'code:generator:gen:table:update',
        onClick: (record: GenTableRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'code:generator:gen:table:delete',
        onClick: (record: GenTableRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getGenTableId = (record: GenTableRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getGenTableField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getGenTableDictValue = (
  record: GenTableRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: GenTableRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: GenTableRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
      syncMasterSelection(record)
    } else if (selecmedRow.value && getGenTableId(selecmedRow.value) === getGenTableId(record)) {
      selecmedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: GenTableRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    syncMasterSelection(selecmedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getGenTableList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[GenTable] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
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

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  dataSource: '',
  tableName: '',
  tableComment: '',
  subTableName: '',
  subTableFkName: '',
  treeCode: '',
  treeParentCode: '',
  treeName: '',
  inDatabase: undefined as number | undefined,
  genTemplateCategory: '',
  genModuleName: '',
  genBusinessName: '',
  genFunctionName: '',
  permsPrefix: '',
  menuButtonGroup: '',
  namePrefix: '',
  entityNamespace: '',
  entityClassName: '',
  dtoNamespace: '',
  dtoClassName: '',
  serviceNamespace: '',
  iServiceClassName: '',
  serviceClassName: '',
  controllerNamespace: '',
  controllerClassName: '',
  isRepository: undefined as number | undefined,
  repositoryInterfaceNamespace: '',
  iRepositoryClassName: '',
  repositoryNamespace: '',
  repositoryClassName: '',
  genFunction: '',
  genMethod: undefined as number | undefined,
  genPath: '',
  isGenMenu: undefined as number | undefined,
  parentMenuId: '',
  isGenTranslation: undefined as number | undefined,
  sortField: '',
  sortType: '',
  frontUi: undefined as number | undefined,
  frontFormLayout: undefined as number | undefined,
  frontBtnStyle: undefined as number | undefined,
  isGenCode: undefined as number | undefined,
  genCodeCount: undefined as number | undefined,
  isUseTabs: undefined as number | undefined,
  tabsFieldCount: undefined as number | undefined,
  genAuthor: '',
  otherGenOptions: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: GenTableRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadGenTableDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateGenTable(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createGenTable(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selecmedMasterKey.value) {
  genTableColumnPanelRef.value?.reload?.()
    }
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getGenTableTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importGenTable(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selecmedMasterKey.value) {
    genTableColumnPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportGenTable(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[GenTable] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: GenTableRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteGenTableById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteGenTableBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  dataSource: '',
  tableName: '',
  tableComment: '',
  subTableName: '',
  subTableFkName: '',
  treeCode: '',
  treeParentCode: '',
  treeName: '',
  inDatabase: undefined as number | undefined,
  genTemplateCategory: '',
  genModuleName: '',
  genBusinessName: '',
  genFunctionName: '',
  permsPrefix: '',
  menuButtonGroup: '',
  namePrefix: '',
  entityNamespace: '',
  entityClassName: '',
  dtoNamespace: '',
  dtoClassName: '',
  serviceNamespace: '',
  iServiceClassName: '',
  serviceClassName: '',
  controllerNamespace: '',
  controllerClassName: '',
  isRepository: undefined as number | undefined,
  repositoryInterfaceNamespace: '',
  iRepositoryClassName: '',
  repositoryNamespace: '',
  repositoryClassName: '',
  genFunction: '',
  genMethod: undefined as number | undefined,
  genPath: '',
  isGenMenu: undefined as number | undefined,
  parentMenuId: '',
  isGenTranslation: undefined as number | undefined,
  sortField: '',
  sortType: '',
  frontUi: undefined as number | undefined,
  frontFormLayout: undefined as number | undefined,
  frontBtnStyle: undefined as number | undefined,
  isGenCode: undefined as number | undefined,
  genCodeCount: undefined as number | undefined,
  isUseTabs: undefined as number | undefined,
  tabsFieldCount: undefined as number | undefined,
  genAuthor: '',
  otherGenOptions: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
</script>
