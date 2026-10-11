<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/generator/gen-table/components -->
<!-- 文件名称：GenTableForm.vue -->
<!-- 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form gen-table-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="gen-table-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dataSource')"
                name="dataSource"
              >
                <a-input
                  v-model:value="formState.dataSource"
                  :placeholder="pi.ph('dataSource')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tableName')"
                name="tableName"
              >
                <a-input
                  v-model:value="formState.tableName"
                  :placeholder="pi.ph('tableName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tableComment')"
                name="tableComment"
              >
                <a-input
                  v-model:value="formState.tableComment"
                  :placeholder="pi.ph('tableComment')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subTableName')"
                name="subTableName"
              >
                <a-input
                  v-model:value="formState.subTableName"
                  :placeholder="pi.ph('subTableName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subTableFkName')"
                name="subTableFkName"
              >
                <a-input
                  v-model:value="formState.subTableFkName"
                  :placeholder="pi.ph('subTableFkName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('treeCode')"
                name="treeCode"
              >
                <a-input
                  v-model:value="formState.treeCode"
                  :placeholder="pi.ph('treeCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.genTableId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('treeParentCode')"
                name="treeParentCode"
              >
                <a-input
                  v-model:value="formState.treeParentCode"
                  :placeholder="pi.ph('treeParentCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.genTableId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('treeName')"
                name="treeName"
              >
                <a-input
                  v-model:value="formState.treeName"
                  :placeholder="pi.ph('treeName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inDatabase')"
                name="inDatabase"
              >
                <TaktSelect
                  v-model:value="formState.inDatabase"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('inDatabase')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genTemplateCategory')"
                name="genTemplateCategory"
              >
                <TaktSelect
                  v-model:value="formState.genTemplateCategory"
                  dict-type="code_generator_template_type"
                  :placeholder="pi.ph('genTemplateCategory')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genModuleName')"
                name="genModuleName"
              >
                <a-input
                  v-model:value="formState.genModuleName"
                  :placeholder="pi.ph('genModuleName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genBusinessName')"
                name="genBusinessName"
              >
                <a-input
                  v-model:value="formState.genBusinessName"
                  :placeholder="pi.ph('genBusinessName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genFunctionName')"
                name="genFunctionName"
              >
                <a-input
                  v-model:value="formState.genFunctionName"
                  :placeholder="pi.ph('genFunctionName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('permsPrefix')"
                name="permsPrefix"
              >
                <a-input
                  v-model:value="formState.permsPrefix"
                  :placeholder="pi.ph('permsPrefix')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('menuButtonGroup')"
                name="menuButtonGroup"
              >
                <TaktSelect
                  v-model:value="formState.menuButtonGroup"
                  dict-type="code_generator_button_category"
                  :placeholder="pi.ph('menuButtonGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('namePrefix')"
                name="namePrefix"
              >
                <a-input
                  v-model:value="formState.namePrefix"
                  :placeholder="pi.ph('namePrefix')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('entityNamespace')"
                name="entityNamespace"
              >
                <a-input
                  v-model:value="formState.entityNamespace"
                  :placeholder="pi.ph('entityNamespace')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('entityClassName')"
                name="entityClassName"
              >
                <a-input
                  v-model:value="formState.entityClassName"
                  :placeholder="pi.ph('entityClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dtoNamespace')"
                name="dtoNamespace"
              >
                <a-input
                  v-model:value="formState.dtoNamespace"
                  :placeholder="pi.ph('dtoNamespace')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dtoClassName')"
                name="dtoClassName"
              >
                <a-input
                  v-model:value="formState.dtoClassName"
                  :placeholder="pi.ph('dtoClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceNamespace')"
                name="serviceNamespace"
              >
                <a-input
                  v-model:value="formState.serviceNamespace"
                  :placeholder="pi.ph('serviceNamespace')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('iServiceClassName')"
                name="iServiceClassName"
              >
                <a-input
                  v-model:value="formState.iServiceClassName"
                  :placeholder="pi.ph('iServiceClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceClassName')"
                name="serviceClassName"
              >
                <a-input
                  v-model:value="formState.serviceClassName"
                  :placeholder="pi.ph('serviceClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('controllerNamespace')"
                name="controllerNamespace"
              >
                <a-input
                  v-model:value="formState.controllerNamespace"
                  :placeholder="pi.ph('controllerNamespace')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('controllerClassName')"
                name="controllerClassName"
              >
                <a-input
                  v-model:value="formState.controllerClassName"
                  :placeholder="pi.ph('controllerClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isRepository')"
                name="isRepository"
              >
                <TaktSelect
                  v-model:value="formState.isRepository"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isRepository')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repositoryInterfaceNamespace')"
                name="repositoryInterfaceNamespace"
              >
                <a-input
                  v-model:value="formState.repositoryInterfaceNamespace"
                  :placeholder="pi.ph('repositoryInterfaceNamespace')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('iRepositoryClassName')"
                name="iRepositoryClassName"
              >
                <a-input
                  v-model:value="formState.iRepositoryClassName"
                  :placeholder="pi.ph('iRepositoryClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repositoryNamespace')"
                name="repositoryNamespace"
              >
                <a-input
                  v-model:value="formState.repositoryNamespace"
                  :placeholder="pi.ph('repositoryNamespace')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repositoryClassName')"
                name="repositoryClassName"
              >
                <a-input
                  v-model:value="formState.repositoryClassName"
                  :placeholder="pi.ph('repositoryClassName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genFunction')"
                name="genFunction"
              >
                <TaktSelect
                  v-model:value="formState.genFunction"
                  dict-type="code_generator_function"
                  :placeholder="pi.ph('genFunction')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genMethod')"
                name="genMethod"
              >
                <TaktSelect
                  v-model:value="formState.genMethod"
                  dict-type="code_generator_method"
                  :placeholder="pi.ph('genMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genPath')"
                name="genPath"
              >
                <TaktSelect
                  v-model:value="formState.genPath"
                  dict-type="code_generator_path_type"
                  :placeholder="pi.ph('genPath')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isGenMenu')"
                name="isGenMenu"
              >
                <TaktSelect
                  v-model:value="formState.isGenMenu"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isGenMenu')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parentMenuId')"
                name="parentMenuId"
              >
                <TaktSelect
                  v-model:value="formState.parentMenuId"
                  api-url="TaktMenus/tree-options"
                  :placeholder="pi.ph('parentMenuId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isGenTranslation')"
                name="isGenTranslation"
              >
                <TaktSelect
                  v-model:value="formState.isGenTranslation"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isGenTranslation')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sortField')"
                name="sortField"
              >
                <a-input
                  v-model:value="formState.sortField"
                  :placeholder="pi.ph('sortField')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sortType')"
                name="sortType"
              >
                <TaktSelect
                  v-model:value="formState.sortType"
                  dict-type="sys_sort_type"
                  :placeholder="pi.ph('sortType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('frontUi')"
                name="frontUi"
              >
                <TaktSelect
                  v-model:value="formState.frontUi"
                  dict-type="code_generator_frontend_ui_type"
                  :placeholder="pi.ph('frontUi')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('frontFormLayout')"
                name="frontFormLayout"
              >
                <TaktSelect
                  v-model:value="formState.frontFormLayout"
                  dict-type="code_generator_frontend_form_layout"
                  :placeholder="pi.ph('frontFormLayout')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('frontBtnStyle')"
                name="frontBtnStyle"
              >
                <TaktSelect
                  v-model:value="formState.frontBtnStyle"
                  dict-type="code_generator_button_style"
                  :placeholder="pi.ph('frontBtnStyle')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isGenCode')"
                name="isGenCode"
              >
                <TaktSelect
                  v-model:value="formState.isGenCode"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isGenCode')"
                  :disabled="!!formData?.genTableId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('genCodeCount')"
                name="genCodeCount"
              >
                <a-input-number
                  v-model:value="formState.genCodeCount"
                  :placeholder="pi.ph('genCodeCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isUseTabs')"
                name="isUseTabs"
              >
                <TaktSelect
                  v-model:value="formState.isUseTabs"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isUseTabs')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tabsFieldCount')"
                name="tabsFieldCount"
              >
                <a-input-number
                  v-model:value="formState.tabsFieldCount"
                  :placeholder="pi.ph('tabsFieldCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('genAuthor')"
                name="genAuthor"
              >
                <a-input
                  v-model:value="formState.genAuthor"
                  :placeholder="pi.ph('genAuthor')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('otherGenOptions')"
                name="otherGenOptions"
              >
                <a-input
                  v-model:value="formState.otherGenOptions"
                  :placeholder="pi.ph('otherGenOptions')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/6)'"
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
    <!-- 下：子表 columns -->
    <TaktEditableTable
      ref="genTableColumnTableRef"
      v-model="childGenTableColumnRows"
      :columns="genTableColumnFormColumns"
      :title="genTableColumnPi.self()"
      :add-button-entity="genTableColumnPi.self()"
      id-field="genTableColumnId"
      :default-row="createDefaultGenTableColumnRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-databaseDataType="{ record }">
        <TaktSelect
          v-model:value="record.databaseDataType"
          dict-type="sys_db_data_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('databaseDataType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-csharpDataType="{ record }">
        <TaktSelect
          v-model:value="record.csharpDataType"
          dict-type="code_generator_csharp_data_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('csharpDataType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isPk="{ record }">
        <TaktSelect
          v-model:value="record.isPk"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isPk')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isIncrement="{ record }">
        <TaktSelect
          v-model:value="record.isIncrement"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isIncrement')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isRequired="{ record }">
        <TaktSelect
          v-model:value="record.isRequired"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isRequired')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isCreate="{ record }">
        <TaktSelect
          v-model:value="record.isCreate"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isCreate')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isUpdate="{ record }">
        <TaktSelect
          v-model:value="record.isUpdate"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isUpdate')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isUnique="{ record }">
        <TaktSelect
          v-model:value="record.isUnique"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isUnique')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isList="{ record }">
        <TaktSelect
          v-model:value="record.isList"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isList')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isExport="{ record }">
        <TaktSelect
          v-model:value="record.isExport"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isExport')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isSort="{ record }">
        <TaktSelect
          v-model:value="record.isSort"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isSort')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isQuery="{ record }">
        <TaktSelect
          v-model:value="record.isQuery"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('isQuery')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-queryType="{ record }">
        <TaktSelect
          v-model:value="record.queryType"
          dict-type="code_generator_query_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('queryType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-htmlType="{ record }">
        <TaktSelect
          v-model:value="record.htmlType"
          dict-type="code_generator_display_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.ph('htmlType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-dictType="{ record }">
        <TaktSelect
          v-model:value="record.dictType"
          api-url="TaktDictTypes/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="genTableColumnPi.queryPh('dictType', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/code/generator/gen-table/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useGenTableI18n } from '../composables/useGenTableI18n'

/** 实体字段 i18n */
const pi = useGenTableI18n()

import type { GenTableCreate } from '@/types/code/generator/genTable.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：租户级实体仅注入 tenantCode，表单只读
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","dataSource","tableName","tableComment","subTableName","subTableFkName","treeCode","treeParentCode","treeName","inDatabase","genTemplateCategory","genModuleName","genBusinessName","genFunctionName","permsPrefix","menuButtonGroup","namePrefix","entityNamespace","entityClassName","dtoNamespace","dtoClassName","serviceNamespace","iServiceClassName","serviceClassName","controllerNamespace","controllerClassName","isRepository","repositoryInterfaceNamespace","iRepositoryClassName","repositoryNamespace","repositoryClassName","genFunction","genMethod","genPath","isGenMenu","parentMenuId","isGenTranslation","sortField","sortType","frontUi","frontFormLayout","frontBtnStyle","isGenCode","genCodeCount","isUseTabs","tabsFieldCount","genAuthor","otherGenOptions","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveMaxLineNumber, resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useGenTableColumnI18n } from '../composables/useGenTableColumnI18n'

const genTableColumnPi = useGenTableColumnI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childGenTableColumnRows = ref<Record<string, unknown>[]>([])
/** 后端已占用子表最大行号（与 maxGenTableColumnLineNumber 对齐） */
const maxGenTableColumnLineNumber = ref(0)
const genTableColumnTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedGenTableColumnRow(row: Record<string, unknown>): boolean {
  const id = row.genTableColumnId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 同步已占用最大行号 */
function syncMaxGenTableColumnLineNumber(rows?: readonly Record<string, unknown>[]) {
  const sourceRows = rows ?? (genTableColumnTableRef.value?.getRows?.() ?? childGenTableColumnRows.value)
  const rowMax = resolveMaxLineNumber(sourceRows.map((row) => Number(row.lineNumber) || 0))
  maxGenTableColumnLineNumber.value = Math.max(maxGenTableColumnLineNumber.value, rowMax)
}

/** 分配下一可用子表行号 */
function allocateNextGenTableColumnLineNumber(): number {
  const rows = genTableColumnTableRef.value?.getRows?.() ?? childGenTableColumnRows.value
  const next = resolveNextDetailLineNumber(maxGenTableColumnLineNumber.value, rows)
  maxGenTableColumnLineNumber.value = next
  return next
}

/** 子表 genTableColumn 可编辑列 */
const genTableColumnFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: genTableColumnPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'databaseColumnName',
    title: genTableColumnPi.label('databaseColumnName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'columnComment',
    title: genTableColumnPi.label('columnComment'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: genTableColumnPi.ph('columnComment'),
  },
  {
    key: 'databaseDataType',
    title: genTableColumnPi.label('databaseDataType'),
    width: 140,
  },
  {
    key: 'csharpDataType',
    title: genTableColumnPi.label('csharpDataType'),
    width: 140,
  },
  {
    key: 'csharpColumnName',
    title: genTableColumnPi.label('csharpColumnName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'length',
    title: genTableColumnPi.label('length'),
    width: 140,
  },
  {
    key: 'decimalDigits',
    title: genTableColumnPi.label('decimalDigits'),
    width: 140,
  },
  {
    key: 'isPk',
    title: genTableColumnPi.label('isPk'),
    width: 140,
  },
  {
    key: 'isIncrement',
    title: genTableColumnPi.label('isIncrement'),
    width: 140,
  },
  {
    key: 'isRequired',
    title: genTableColumnPi.label('isRequired'),
    width: 140,
  },
  {
    key: 'isCreate',
    title: genTableColumnPi.label('isCreate'),
    width: 140,
  },
  {
    key: 'isUpdate',
    title: genTableColumnPi.label('isUpdate'),
    width: 140,
  },
  {
    key: 'isUnique',
    title: genTableColumnPi.label('isUnique'),
    width: 140,
  },
  {
    key: 'isList',
    title: genTableColumnPi.label('isList'),
    width: 140,
  },
  {
    key: 'isExport',
    title: genTableColumnPi.label('isExport'),
    width: 140,
  },
  {
    key: 'isSort',
    title: genTableColumnPi.label('isSort'),
    width: 140,
  },
  {
    key: 'isQuery',
    title: genTableColumnPi.label('isQuery'),
    width: 140,
  },
  {
    key: 'queryType',
    title: genTableColumnPi.label('queryType'),
    width: 140,
  },
  {
    key: 'htmlType',
    title: genTableColumnPi.label('htmlType'),
    width: 140,
  },
  {
    key: 'dictType',
    title: genTableColumnPi.label('dictType'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<GenTableCreate & { genTableId?: string }> | null | undefined) {
  const rows_genTableColumn = ((val as any)?.columns ?? []) as Record<string, unknown>[]
  maxGenTableColumnLineNumber.value = Number((val as { maxGenTableColumnLineNumber?: number })?.maxGenTableColumnLineNumber) || 0
  childGenTableColumnRows.value = rows_genTableColumn
  syncMaxGenTableColumnLineNumber(rows_genTableColumn)
}

function createDefaultGenTableColumnRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextGenTableColumnLineNumber(),
    databaseColumnName: '',
    columnComment: '',
    databaseDataType: '',
    csharpDataType: '',
    csharpColumnName: '',
    length: 0,
    decimalDigits: 0,
    isPk: 0,
    isIncrement: 0,
    isRequired: 0,
    isCreate: 0,
    isUpdate: 0,
    isUnique: 0,
    isList: 0,
    isExport: 0,
    isSort: 0,
    isQuery: 0,
    queryType: '',
    htmlType: '',
    dictType: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.genTableId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    columns: genTableColumnTableRef.value?.getRows?.() ?? childGenTableColumnRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        genTableId: masterId,
      }
      if (isUpdate && isPersistedGenTableColumnRow(row)) {
        normalized.genTableColumnId = row.genTableColumnId
      } else {
        delete normalized.genTableColumnId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<GenTableCreate & { genTableId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 genTableId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.genTableId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).columns
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
  },
  { immediate: true }
)

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => tenantStore.tenantCode,
  () => {
    if (!props.formData?.genTableId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  dataSource: [
    {
      required: true,
      message: pi.ph('dataSource'),
      trigger: 'blur'
    }
  ],
  tableName: [
    {
      required: true,
      message: pi.ph('tableName'),
      trigger: 'blur'
    }
  ],
  inDatabase: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inDatabase'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inDatabase'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  genTemplateCategory: [
    {
      required: true,
      message: pi.ph('genTemplateCategory'),
      trigger: 'change'
    }
  ],
  genBusinessName: [
    {
      required: true,
      message: pi.ph('genBusinessName'),
      trigger: 'blur'
    }
  ],
  permsPrefix: [
    {
      required: true,
      message: pi.ph('permsPrefix'),
      trigger: 'blur'
    }
  ],
  entityClassName: [
    {
      required: true,
      message: pi.ph('entityClassName'),
      trigger: 'blur'
    }
  ],
  isRepository: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isRepository'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isRepository'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  genMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('genMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('genMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  genPath: [
    {
      required: true,
      message: pi.ph('genPath'),
      trigger: 'change'
    }
  ],
  isGenMenu: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isGenMenu'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isGenMenu'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  parentMenuId: [
    {
      required: true,
      message: pi.ph('parentMenuId'),
      trigger: 'change'
    }
  ],
  isGenTranslation: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isGenTranslation'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isGenTranslation'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sortField: [
    {
      required: true,
      message: pi.ph('sortField'),
      trigger: 'blur'
    }
  ],
  sortType: [
    {
      required: true,
      message: pi.ph('sortType'),
      trigger: 'change'
    }
  ],
  frontUi: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('frontUi'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('frontUi'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  frontFormLayout: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('frontFormLayout'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('frontFormLayout'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  frontBtnStyle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('frontBtnStyle'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('frontBtnStyle'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isGenCode: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isGenCode'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isGenCode'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  genCodeCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('genCodeCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('genCodeCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isUseTabs: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isUseTabs'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isUseTabs'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  tabsFieldCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('tabsFieldCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('tabsFieldCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  genAuthor: [
    {
      required: true,
      message: pi.ph('genAuthor'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await genTableColumnTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('inDatabase' in payload) {
    const rawinDatabase = payload.inDatabase
    if (rawinDatabase === undefined || rawinDatabase === null || rawinDatabase === '') {
      delete payload.inDatabase
    } else {
      const numinDatabase = typeof rawinDatabase === 'number' ? rawinDatabase : Number(rawinDatabase)
      if (Number.isFinite(numinDatabase)) payload.inDatabase = numinDatabase
      else delete payload.inDatabase
    }
  }
  if ('isRepository' in payload) {
    const rawisRepository = payload.isRepository
    if (rawisRepository === undefined || rawisRepository === null || rawisRepository === '') {
      delete payload.isRepository
    } else {
      const numisRepository = typeof rawisRepository === 'number' ? rawisRepository : Number(rawisRepository)
      if (Number.isFinite(numisRepository)) payload.isRepository = numisRepository
      else delete payload.isRepository
    }
  }
  if ('genMethod' in payload) {
    const rawgenMethod = payload.genMethod
    if (rawgenMethod === undefined || rawgenMethod === null || rawgenMethod === '') {
      delete payload.genMethod
    } else {
      const numgenMethod = typeof rawgenMethod === 'number' ? rawgenMethod : Number(rawgenMethod)
      if (Number.isFinite(numgenMethod)) payload.genMethod = numgenMethod
      else delete payload.genMethod
    }
  }
  if ('isGenMenu' in payload) {
    const rawisGenMenu = payload.isGenMenu
    if (rawisGenMenu === undefined || rawisGenMenu === null || rawisGenMenu === '') {
      delete payload.isGenMenu
    } else {
      const numisGenMenu = typeof rawisGenMenu === 'number' ? rawisGenMenu : Number(rawisGenMenu)
      if (Number.isFinite(numisGenMenu)) payload.isGenMenu = numisGenMenu
      else delete payload.isGenMenu
    }
  }
  if ('isGenTranslation' in payload) {
    const rawisGenTranslation = payload.isGenTranslation
    if (rawisGenTranslation === undefined || rawisGenTranslation === null || rawisGenTranslation === '') {
      delete payload.isGenTranslation
    } else {
      const numisGenTranslation = typeof rawisGenTranslation === 'number' ? rawisGenTranslation : Number(rawisGenTranslation)
      if (Number.isFinite(numisGenTranslation)) payload.isGenTranslation = numisGenTranslation
      else delete payload.isGenTranslation
    }
  }
  if ('frontUi' in payload) {
    const rawfrontUi = payload.frontUi
    if (rawfrontUi === undefined || rawfrontUi === null || rawfrontUi === '') {
      delete payload.frontUi
    } else {
      const numfrontUi = typeof rawfrontUi === 'number' ? rawfrontUi : Number(rawfrontUi)
      if (Number.isFinite(numfrontUi)) payload.frontUi = numfrontUi
      else delete payload.frontUi
    }
  }
  if ('frontFormLayout' in payload) {
    const rawfrontFormLayout = payload.frontFormLayout
    if (rawfrontFormLayout === undefined || rawfrontFormLayout === null || rawfrontFormLayout === '') {
      delete payload.frontFormLayout
    } else {
      const numfrontFormLayout = typeof rawfrontFormLayout === 'number' ? rawfrontFormLayout : Number(rawfrontFormLayout)
      if (Number.isFinite(numfrontFormLayout)) payload.frontFormLayout = numfrontFormLayout
      else delete payload.frontFormLayout
    }
  }
  if ('frontBtnStyle' in payload) {
    const rawfrontBtnStyle = payload.frontBtnStyle
    if (rawfrontBtnStyle === undefined || rawfrontBtnStyle === null || rawfrontBtnStyle === '') {
      delete payload.frontBtnStyle
    } else {
      const numfrontBtnStyle = typeof rawfrontBtnStyle === 'number' ? rawfrontBtnStyle : Number(rawfrontBtnStyle)
      if (Number.isFinite(numfrontBtnStyle)) payload.frontBtnStyle = numfrontBtnStyle
      else delete payload.frontBtnStyle
    }
  }
  if ('isGenCode' in payload) {
    const rawisGenCode = payload.isGenCode
    if (rawisGenCode === undefined || rawisGenCode === null || rawisGenCode === '') {
      delete payload.isGenCode
    } else {
      const numisGenCode = typeof rawisGenCode === 'number' ? rawisGenCode : Number(rawisGenCode)
      if (Number.isFinite(numisGenCode)) payload.isGenCode = numisGenCode
      else delete payload.isGenCode
    }
  }
  if ('genCodeCount' in payload) {
    const rawgenCodeCount = payload.genCodeCount
    if (rawgenCodeCount === undefined || rawgenCodeCount === null || rawgenCodeCount === '') {
      delete payload.genCodeCount
    } else {
      const numgenCodeCount = typeof rawgenCodeCount === 'number' ? rawgenCodeCount : Number(rawgenCodeCount)
      if (Number.isFinite(numgenCodeCount)) payload.genCodeCount = numgenCodeCount
      else delete payload.genCodeCount
    }
  }
  if ('isUseTabs' in payload) {
    const rawisUseTabs = payload.isUseTabs
    if (rawisUseTabs === undefined || rawisUseTabs === null || rawisUseTabs === '') {
      delete payload.isUseTabs
    } else {
      const numisUseTabs = typeof rawisUseTabs === 'number' ? rawisUseTabs : Number(rawisUseTabs)
      if (Number.isFinite(numisUseTabs)) payload.isUseTabs = numisUseTabs
      else delete payload.isUseTabs
    }
  }
  if ('tabsFieldCount' in payload) {
    const rawtabsFieldCount = payload.tabsFieldCount
    if (rawtabsFieldCount === undefined || rawtabsFieldCount === null || rawtabsFieldCount === '') {
      delete payload.tabsFieldCount
    } else {
      const numtabsFieldCount = typeof rawtabsFieldCount === 'number' ? rawtabsFieldCount : Number(rawtabsFieldCount)
      if (Number.isFinite(numtabsFieldCount)) payload.tabsFieldCount = numtabsFieldCount
      else delete payload.tabsFieldCount
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.genTableId) {
    payload.genTableId = props.formData.genTableId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.genTableId)
  childGenTableColumnRows.value = []
  genTableColumnTableRef.value?.resetRows?.()
  maxGenTableColumnLineNumber.value = 0
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
