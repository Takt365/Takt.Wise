<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/code/generator/components -->
<!-- 文件名称：GenForm.vue -->
<!-- 功能描述：代码生成表配置大表单（表配置多 Tab + 字段配置可拖拽表格）；由 generator/index.vue 弹窗引用；defineExpose：validate、getValues、reset；切换 tenantCode 时 emit config-change。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="gen-form-root">
    <a-tabs v-model:active-key="activeTab">
      <!-- 表配置：拆成多 Tab，每块一行一列 -->
      <a-tab-pane
        key="table"
        :tab="tf('tab.table')"
        force-render
      >
        <a-form
          ref="formRef"
          :model="formState"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 18 }"
          layout="horizontal"
          label-align="right"
        >
          <a-tabs
            v-model:active-key="tableSubTab"
            type="card"
            size="small"
          >
            <a-tab-pane
              key="basic"
              :tab="tf('tab.basic')"
              force-render
            >
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.datasource')"
                    name="tenantCode"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-select
                      :value="parseSelectToOptionalString(formState.tenantCode)"
                      :placeholder="t('common.page.form.placeholder.select', { field: t('entity.gentable.datasource') })"
                      allow-clear
                      style="width: 100%"
                      :options="databaseConfigOptions"
                      :disabled="isEditMode"
                      @update:value="
                        (v: unknown) => {
                          let id: string | undefined
                          if (v == null) id = undefined
                          else if (typeof v === 'string' || typeof v === 'number') id = String(v)
                          else if (typeof v === 'object' && 'value' in (v as object)) {
                            const x = (v as { value: unknown }).value
                            id = x == null ? undefined : String(x)
                          }
                          formState.tenantCode = id
                          handleConfigChange(id)
                        }
                      "
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.tablename')"
                    name="tableName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="tableNameRules"
                  >
                    <a-input
                      v-if="!isEditMode"
                      :value="formState.tableName ?? ''"
                      :placeholder="tf('placeholder.tablenamenew')"
                      :disabled="!formState.tenantCode"
                      allow-clear
                      @update:value="(v: string) => { formState.tableName = v === '' ? undefined : v }"
                    />
                    <a-select
                      v-else
                      :value="formState.tableName ?? undefined"
                      :placeholder="tf('placeholder.tablenameedit')"
                      disabled
                      style="width: 100%"
                      :options="databaseTableOptions"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.tablecomment')"
                    name="tableComment"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('tablecomment')"
                  >
                    <a-input
                      :value="formState.tableComment ?? ''"
                      :placeholder="gentableInputPh('tablecomment')"
                      allow-clear
                      @update:value="(v: string) => { formState.tableComment = v === '' ? undefined : v }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.gentemplatecategory')"
                    name="genTemplateCategory"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('gentemplatecategory', 'select')"
                  >
                    <TaktSelect
                      :model-value="formState.genTemplateCategory ?? ''"
                      dict-type="code_generator_template_type"
                      :placeholder="gentableSelectPh('gentemplatecategory')"
                      allow-clear
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.genTemplateCategory = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.indatabase')"
                    name="inDatabase"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <TaktSelect
                      :model-value="formState.inDatabase ?? ''"
                      dict-type="sys_yes_no"
                      :placeholder="tf('placeholder.indatabase')"
                      style="width: 100%"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- 主子表：仅当 genTemplateCategory === 'sub' 时显示 -->
              <a-row
                v-if="formState.genTemplateCategory === 'sub'"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.subtablename')"
                    name="subTableName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="subTableNameRules"
                  >
                    <a-select
                      :value="formState.subTableName ?? undefined"
                      :placeholder="gentableSelectPh('subtablename')"
                      allow-clear
                      style="width: 100%"
                      :options="subTableNameOptions"
                      @update:value="(v: unknown) => { formState.subTableName = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row
                v-if="formState.genTemplateCategory === 'sub'"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.subtablefkname')"
                    name="subTableFkName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="subTableFkNameRules"
                  >
                    <a-select
                      :value="formState.subTableFkName ?? undefined"
                      :placeholder="gentableSelectPh('subtablefkname')"
                      allow-clear
                      style="width: 100%"
                      :options="columnSelectOptions"
                      @update:value="(v: unknown) => { formState.subTableFkName = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- 树表：仅当 genTemplateCategory === 'tree' 时显示 -->
              <a-row
                v-if="formState.genTemplateCategory === 'tree'"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.treecode')"
                    name="treeCode"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="treeCodeRules"
                  >
                    <a-select
                      :value="formState.treeCode ?? undefined"
                      :placeholder="gentableSelectPh('treecode')"
                      allow-clear
                      style="width: 100%"
                      :options="columnSelectOptions"
                      @update:value="(v: unknown) => { formState.treeCode = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row
                v-if="formState.genTemplateCategory === 'tree'"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.treeparentcode')"
                    name="treeParentCode"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="treeParentCodeRules"
                  >
                    <a-select
                      :value="formState.treeParentCode ?? undefined"
                      :placeholder="gentableSelectPh('treeparentcode')"
                      allow-clear
                      style="width: 100%"
                      :options="columnSelectOptions"
                      @update:value="(v: unknown) => { formState.treeParentCode = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row
                v-if="formState.genTemplateCategory === 'tree'"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.treename')"
                    name="treeName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="treeNameRules"
                  >
                    <a-select
                      :value="formState.treeName ?? undefined"
                      :placeholder="gentableSelectPh('treename')"
                      allow-clear
                      style="width: 100%"
                      :options="columnSelectOptions"
                      @update:value="(v: unknown) => { formState.treeName = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane
              key="business"
              :tab="tf('tab.business')"
              force-render
            >
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.nameprefix')"
                    name="namePrefix"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="namePrefixPascalRules"
                  >
                    <a-input
                      :value="formState.namePrefix ?? ''"
                      :placeholder="tf('placeholder.nameprefix')"
                      allow-clear
                      @update:value="(v: string) => { formState.namePrefix = v === '' ? undefined : v }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.permsprefix')"
                    name="permsPrefix"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('permsprefix')"
                  >
                    <a-input
                      :value="formState.permsPrefix ?? ''"
                      :placeholder="tf('placeholder.permsprefix')"
                      allow-clear
                      @update:value="(v: string) => { formState.permsPrefix = v === '' ? undefined : v }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.menubuttongroup')"
                    name="menuButtonGroup"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('menubuttongroup', 'select')"
                  >
                    <a-form-item-rest>
                      <div>
                        <a-checkbox
                          v-model:checked="menuButtonGroupCheckAll"
                          :indeterminate="menuButtonGroupIndeterminate"
                          @change="onMenuButtonGroupCheckAllChange"
                        >
                          {{ t('common.page.button.checkall') }}
                        </a-checkbox>
                      </div>
                      <a-divider style="margin: 8px 0" />
                    </a-form-item-rest>
                    <a-checkbox-group
                      v-model:value="menuButtonGroupSelect"
                      :options="menuButtonGroupOptions"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.genmodulename')"
                    name="genModuleName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('genmodulename', 'select')"
                  >
                    <TaktTreeSelect
                      :value="formState.genModuleName || undefined"
                      :tree-data="moduleOptionsTree"
                      :placeholder="tf('placeholder.genmodulename')"
                      allow-clear
                      style="width: 100%"
                      :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                      :loading="moduleOptionsLoading"
                      @update:value="(v: unknown) => { formState.genModuleName = parseTreeSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.genbusinessname')"
                    name="genBusinessName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('genbusinessname')"
                  >
                    <a-input
                      :value="formState.genBusinessName ?? ''"
                      :placeholder="
                        formState.inDatabase === 1
                          ? tf('placeholder.genbusinessnamefromtable')
                          : tf('placeholder.genbusinessnamemanual')
                      "
                      :disabled="formState.inDatabase === 1"
                      allow-clear
                      @update:value="(v: string) => { formState.genBusinessName = v === '' ? undefined : v }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.genfunctionname')"
                    name="genFunctionName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-input
                      :value="formState.genFunctionName ?? ''"
                      :placeholder="tf('placeholder.genfunctionname')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane
              key="entity"
              :tab="tf('tab.entitydto')"
              force-render
            >
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.entitynamespace')"
                    name="entityNamespace"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('entitynamespace')"
                  >
                    <a-input
                      :value="formState.entityNamespace ?? ''"
                      :placeholder="tf('placeholder.autofrommodule')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.entityclassname')"
                    name="entityClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('entityclassname')"
                  >
                    <a-input
                      :value="formState.entityClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.dtoclassname')"
                    name="dtoClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('dtoclassname')"
                  >
                    <a-input
                      :value="formState.dtoClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.dtonamespace')"
                    name="dtoNamespace"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-input
                      :value="formState.dtoNamespace ?? ''"
                      :placeholder="tf('placeholder.autofrommodule')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane
              key="service"
              :tab="tf('tab.service')"
              force-render
            >
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.servicenamespace')"
                    name="serviceNamespace"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-input
                      :value="formState.serviceNamespace ?? ''"
                      :placeholder="tf('placeholder.autofrommodule')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.iserviceclassname')"
                    name="iServiceClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('iserviceclassname')"
                  >
                    <a-input
                      :value="formState.iServiceClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.serviceclassname')"
                    name="serviceClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('serviceclassname')"
                  >
                    <a-input
                      :value="formState.serviceClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.controllernamespace')"
                    name="controllerNamespace"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('controllernamespace')"
                  >
                    <a-input
                      :value="formState.controllerNamespace ?? ''"
                      :placeholder="tf('placeholder.autofrommodule')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.controllerclassname')"
                    name="controllerClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-input
                      :value="formState.controllerClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.isrepository')"
                    name="isRepository"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('isrepository', 'select')"
                  >
                    <a-radio-group
                      :value="formState.isRepository ?? undefined"
                      :options="sysYesNoOptions"
                      @update:value="(v: unknown) => { formState.isRepository = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- 仓储相关字段：仅当「是否生成仓储」为「是」(1) 时显示，与「否」(0) 相斥 -->
              <a-row
                v-if="formState.isRepository === 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.repositoryinterfacenamespace')"
                    name="repositoryInterfaceNamespace"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="repositoryInterfaceNamespaceRules"
                  >
                    <a-input
                      :value="formState.repositoryInterfaceNamespace ?? ''"
                      :placeholder="tf('placeholder.autofrommodule')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row
                v-if="formState.isRepository === 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.irepositoryclassname')"
                    name="iRepositoryClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="iRepositoryClassNameRules"
                  >
                    <a-input
                      :value="formState.iRepositoryClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row
                v-if="formState.isRepository === 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.repositorynamespace')"
                    name="repositoryNamespace"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="repositoryNamespaceRules"
                  >
                    <a-input
                      :value="formState.repositoryNamespace ?? ''"
                      :placeholder="tf('placeholder.autofrommodule')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row
                v-if="formState.isRepository === 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.repositoryclassname')"
                    name="repositoryClassName"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="repositoryClassNameRules"
                  >
                    <a-input
                      :value="formState.repositoryClassName ?? ''"
                      :placeholder="tf('placeholder.autofrombusiness')"
                      disabled
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane
              key="generate"
              :tab="tf('tab.generate')"
              force-render
            >
              <a-row :gutter="24">
                <a-col :span="24">
                  <!-- 生成功能：仅收集 a-checkbox-group；全选与分隔线放入 a-form-item-rest 避免 Form.Item 收集多个控件 -->
                  <a-form-item
                    :label="t('entity.gentable.genfunction')"
                    name="genFunction"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-form-item-rest>
                      <div>
                        <a-checkbox
                          v-model:checked="genFunctionCheckAll"
                          :indeterminate="genFunctionIndeterminate"
                          @change="onGenFunctionCheckAllChange"
                        >
                          {{ t('common.page.button.checkall') }}
                        </a-checkbox>
                      </div>
                      <a-divider style="margin: 8px 0" />
                    </a-form-item-rest>
                    <a-checkbox-group
                      v-model:value="genFunctionSelect"
                      :options="filteredGenFunctionOptions"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.genmethod')"
                    name="genMethod"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('genmethod', 'select')"
                  >
                    <TaktSelect
                      :model-value="formState.genMethod ?? ''"
                      dict-type="code_generator_method"
                      :placeholder="tf('placeholder.genmethod')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.genMethod = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- 生成路径：仅当「生成方式」为「自定义路径」(1) 时显示；zip(0)、当前项目(2) 不显示 -->
              <a-row
                v-if="formState.genMethod === 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.genpath')"
                    name="genPath"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="genPathRules"
                  >
                    <TaktSelect
                      :model-value="formState.genPath ?? ''"
                      dict-type="code_generator_path_type"
                      :placeholder="tf('placeholder.genpath')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.genPath = v === '' || v == null ? undefined : String(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- 当前项目路径：仅当「生成方式」为「当前项目」(2) 时显示，自动从后端获取 -->
              <a-row
                v-if="formState.genMethod === 2"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="tf('label.currentprojectpath')"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-input
                      :value="currentProjectPathDisplay"
                      readonly
                      :placeholder="tf('placeholder.currentprojectidle')"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.isgenmenu')"
                    name="isGenMenu"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <TaktSelect
                      :model-value="formState.isGenMenu ?? ''"
                      dict-type="sys_yes_no"
                      :placeholder="t('common.page.form.placeholder.selectonly')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.isGenMenu = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- 上级菜单：仅当「是否生成菜单」为「是」(1) 时显示，与「否」相斥 -->
              <a-row
                v-if="formState.isGenMenu == 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.parentmenuid')"
                    name="parentMenuId"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="parentMenuIdRules"
                  >
                    <TaktTreeSelect
                      :value="formState.parentMenuId || undefined"
                      api-url="TaktMenus/tree-options"
                      :lazy="true"
                      :placeholder="gentableSelectPh('parentmenuid')"
                      allow-clear
                      style="width: 100%"
                      :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                      @update:value="(v: unknown) => { formState.parentMenuId = parseTreeSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.isgentranslation')"
                    name="isGenTranslation"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('isgentranslation', 'select')"
                  >
                    <TaktSelect
                      :model-value="formState.isGenTranslation ?? ''"
                      dict-type="sys_yes_no"
                      :placeholder="t('common.page.form.placeholder.selectonly')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.isGenTranslation = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.sortfield')"
                    name="sortField"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('sortfield', 'select')"
                  >
                    <a-select
                      :value="formState.sortField ?? undefined"
                      :placeholder="gentableSelectPh('sortfield')"
                      allow-clear
                      style="width: 100%"
                      :options="columnSelectOptions"
                      @update:value="(v: unknown) => { formState.sortField = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.sorttype')"
                    name="sortType"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('sorttype', 'select')"
                  >
                    <TaktSelect
                      :model-value="formState.sortType ?? ''"
                      dict-type="sys_sort_type"
                      :placeholder="gentableSelectPh('sorttype')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.sortType = parseSelectToOptionalString(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
            <a-tab-pane
              key="front"
              :tab="tf('tab.front')"
              force-render
            >
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.frontui')"
                    name="frontUi"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <TaktSelect
                      :model-value="formState.frontUi ?? ''"
                      dict-type="code_generator_frontend_ui_type"
                      :placeholder="tf('placeholder.frontui')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.frontUi = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.frontformlayout')"
                    name="frontFormLayout"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('frontformlayout', 'select')"
                  >
                    <TaktSelect
                      :model-value="formState.frontFormLayout ?? ''"
                      dict-type="code_generator_frontend_form_layout"
                      :placeholder="tf('placeholder.frontformlayout')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.frontFormLayout = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.frontbtnstyle')"
                    name="frontBtnStyle"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <TaktSelect
                      :model-value="formState.frontBtnStyle ?? ''"
                      dict-type="code_generator_button_style"
                      :placeholder="tf('placeholder.frontbtnstyle')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.frontBtnStyle = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.isusetabs')"
                    name="isUseTabs"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('isusetabs', 'select')"
                  >
                    <TaktSelect
                      :model-value="formState.isUseTabs ?? ''"
                      dict-type="sys_yes_no"
                      :placeholder="t('common.page.form.placeholder.selectonly')"
                      style="width: 100%"
                      @update:model-value="(v: unknown) => { formState.isUseTabs = parseSelectToOptionalNumber(v) }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <!-- Tabs 字段数：仅当「是否使用 Tabs」为「是」(0) 时显示，与「否」相斥 -->
              <a-row
                v-if="formState.isUseTabs == 1"
                :gutter="24"
              >
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.tabsfieldcount')"
                    name="tabsFieldCount"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="tabsFieldCountRules"
                  >
                    <a-input-number
                      :value="formState.tabsFieldCount ?? 0"
                      @update:value="(v: string | number | null) => { formState.tabsFieldCount = parseSelectToOptionalNumber(v) ?? 0 }"
                      :min="1"
                      style="width: 100%"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.genauthor')"
                    name="genAuthor"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                    :rules="rq('genauthor')"
                  >
                    <a-input
                      :value="formState.genAuthor ?? ''"
                      disabled
                      :placeholder="tf('placeholder.genauthor')"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row :gutter="24">
                <a-col :span="24">
                  <a-form-item
                    :label="t('entity.gentable.othergenoptions')"
                    name="otherGenOptions"
                    :label-col="{ span: 3 }"
                    :wrapper-col="{ span: 0 }"
                  >
                    <a-textarea
                      :value="formState.otherGenOptions ?? ''"
                      :placeholder="t('common.page.form.placeholder.optional', { field: gentableLabel('othergenoptions') })"
                      :rows="4"
                      allow-clear
                      @update:value="(v: string) => { formState.otherGenOptions = v === '' ? undefined : v }"
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-tab-pane>
          </a-tabs>
        </a-form>
      </a-tab-pane>

      <!-- 字段配置：表格行内编辑，横向滚动仅在此表格容器内 -->
      <a-tab-pane
        key="column"
        :tab="tf('tab.column')"
        force-render
      >
        <TaktToolsBar
          :show-create="false"
          :show-update="false"
          :show-delete="false"
          :show-import="false"
          :show-export="false"
          :show-advanced-query="false"
          :show-refresh="false"
          :show-transpose="false"
          :show-expand="false"
          :show-create-row="false"
          :show-delete-row="false"
          :show-column-setting="true"
          :show-fullscreen="true"
          @column-setting="handleColumnTableColumnSetting"
          @fullscreen="handleColumnTableFullscreen"
        >
          <template #left>
            <a-space>
              <a-button
                type="primary"
                class="takt-button-create-row"
                @click="addColumnRow"
              >
                <template #icon>
                  <RiInsertRowBottom class="takt-remix-icon" />
                </template>
                {{ t('common.page.button.createrow') }}
              </a-button>
              <a-button
                class="takt-button-delete-row"
                :disabled="selecmedColumnRowKeys.length === 0"
                @click="handleDeleteSelecmedColumnRows"
              >
                <template #icon>
                  <RiDeleteRow class="takt-remix-icon" />
                </template>
                {{ t('common.page.button.deleterow') }}
              </a-button>
              <a-button
                class="takt-button-reset"
                :loading="columnLoading"
                @click="handleResetAllColumnRows"
              >
                <template #icon>
                  <RiRefreshLine class="takt-remix-icon" />
                </template>
                {{ resetAllButtonLabel }}
              </a-button>
            </a-space>
          </template>
        </TaktToolsBar>
        <div
          class="column-table-wrap column-table-wrap--fixed-y"
          :style="columnTableBodyStyle"
        >
          <a-table
            table-layout="fixed"
            :columns="columnTableDisplayColumns"
            :data-source="columnList"
            :loading="columnLoading"
            :row-key="getColumnRowKey"
            :row-selection="columnRowSelection"
            :custom-row="(r: GenTableColumnRow, i?: number) => columnTableCustomRow(r, i ?? 0)"
            :pagination="false"
            :scroll="columnTableScroll"
            :virtual="columnTableVirtual"
            size="small"
            bordered
            @resize-column="handleColumnTableResizeColumn"
          >
            <template #bodyCell="{ column, record }">
              <!-- 拖拽把手：仅此格可拖拽，整行可放置 -->
              <template v-if="column.key === 'dragSort'">
                <span
                  class="column-drag-handle"
                  draggable="true"
                  @dragstart="(e: DragEvent) => onColumnDragStart(e, record as GenTableColumnRow)"
                  @dragover="onColumnDragOver"
                >
                  <HolderOutlined />
                </span>
              </template>
              <!-- 列名：行内输入 -->
              <template v-else-if="column.key === 'databaseColumnName'">
                <a-input
                  :value="record.databaseColumnName ?? ''"
                  allow-clear
                  class="column-cell-input"
                  @update:value="(v: string) => { record.databaseColumnName = v === '' ? undefined : v }"
                />
              </template>
              <!-- 描述：行内输入 -->
              <template v-else-if="column.key === 'columnComment'">
                <a-input
                  :value="record.columnComment ?? ''"
                  allow-clear
                  class="column-cell-input"
                  @update:value="(v: string) => { record.columnComment = v === '' ? undefined : v }"
                />
              </template>
              <!-- DB类型：字典 sys_db_data_type，选中后级联 C#类型 -->
              <template v-else-if="column.key === 'databaseDataType'">
                <TaktSelect
                  :model-value="record.databaseDataType ?? ''"
                  dict-type="sys_db_data_type"
                  :placeholder="gentableColumnSelectPh('databasedatatype')"
                  allow-clear
                  class="column-cell-select"
                  style="width: 100%"
                  @update:model-value="(v: unknown) => { record.databaseDataType = parseSelectToOptionalString(v) }"
                  @change="(v: string | number | (string | number)[] | undefined) => onColumnDbTypeChange(record as GenTableColumnRow, v != null ? String(v) : '')"
                />
              </template>
              <!-- C#类型：与 DB类型级联，仅显示当前 DB类型对应的 C#类型选项；切换时按类型清空长度/精度 -->
              <template v-else-if="column.key === 'csharpDataType'">
                <a-select
                  :value="record.csharpDataType ?? undefined"
                  :options="getCsharpTypeOptionsForRow(record.databaseDataType)"
                  :placeholder="gentableColumnSelectPh('csharpdatatype')"
                  allow-clear
                  class="column-cell-select"
                  style="width: 100%"
                  @update:value="(v: unknown) => { record.csharpDataType = parseSelectToOptionalString(v) }"
                  @change="(value) => onColumnCsharpTypeChange(record as GenTableColumnRow, value != null ? String(value) : '')"
                />
              </template>
              <!-- C#列名：行内输入 -->
              <template v-else-if="column.key === 'csharpColumnName'">
                <a-input
                  :value="record.csharpColumnName ?? ''"
                  allow-clear
                  class="column-cell-input"
                  @update:value="(v: string) => { record.csharpColumnName = v === '' ? undefined : v }"
                />
              </template>
              <!-- 长度：仅 string/decimal 类型显示（字符串长度或 decimal 整数位数） -->
              <template v-else-if="column.key === 'length'">
                <a-input-number
                  v-if="needLengthForCsharpType(record.csharpDataType)"
                  :value="record.length ?? undefined"
                  :min="0"
                  class="column-cell-input"
                  style="width: 100%"
                  @update:value="(v: unknown) => { record.length = parseSelectToOptionalNumber(v) }"
                />
                <span
                  v-else
                  class="column-cell-muted"
                >—</span>
              </template>
              <!-- 精度：仅 decimal 类型显示（小数位数） -->
              <template v-else-if="column.key === 'decimalDigits'">
                <a-input-number
                  v-if="needDecimalDigitsForCsharpType(record.csharpDataType)"
                  :value="record.decimalDigits ?? undefined"
                  :min="0"
                  class="column-cell-input"
                  style="width: 100%"
                  @update:value="(v: unknown) => { record.decimalDigits = parseSelectToOptionalNumber(v) }"
                />
                <span
                  v-else
                  class="column-cell-muted"
                >—</span>
              </template>
              <!-- 主键/自增/必填/查询/新增/更新/查重/列表/导出/排序：行内开关（后端约定 1=是、0=否）；是否查询为否时清空查询方式 -->
              <template v-else-if="column.key === 'isPk' || column.key === 'isIncrement' || column.key === 'isRequired' || column.key === 'isQuery' || column.key === 'isCreate' || column.key === 'isUpdate' || column.key === 'isUnique' || column.key === 'isList' || column.key === 'isExport' || column.key === 'isSort'">
                <a-switch
                  :checked="record[String(column.key)] === 1"
                  :checked-children="t('common.status.yes')"
                  :un-checked-children="t('common.status.no')"
                  @change="(checked: unknown) => {
                    const key = String(column.key)
                    const isOn = checked === true || checked === 1 || checked === '1'
                    record[key] = isOn ? 1 : 0
                    if (key === 'isQuery') onColumnIsQueryChange(record as GenTableColumnRow, isOn)
                  }"
                />
              </template>
              <!-- 查询方式：仅当「是否查询」为是时显示，字典 code_generator_query_type -->
              <template v-else-if="column.key === 'queryType'">
                <TaktSelect
                  v-if="record.isQuery === 1"
                  :model-value="record.queryType ?? undefined"
                  dict-type="code_generator_query_type"
                  :placeholder="gentableColumnSelectPh('querytype')"
                  allow-clear
                  class="column-cell-select"
                  style="width: 100%"
                  @update:model-value="(v: unknown) => { record.queryType = parseSelectToOptionalString(v) }"
                />
                <span
                  v-else
                  class="column-cell-muted"
                >—</span>
              </template>
              <!-- 显示类型：字典 code_generator_display_type（下拉框/复选框/单选框时需配合字典列绑定选项） -->
              <template v-else-if="column.key === 'htmlType'">
                <TaktSelect
                  :model-value="record.htmlType ?? undefined"
                  dict-type="code_generator_display_type"
                  :placeholder="gentableColumnSelectPh('htmltype')"
                  allow-clear
                  class="column-cell-select"
                  style="width: 100%"
                  @update:model-value="(v: unknown) => { record.htmlType = parseSelectToOptionalString(v) }"
                  @change="(v: string | number | (string | number)[] | undefined) => onColumnHtmlTypeChange(record as GenTableColumnRow, v)"
                />
              </template>
              <!-- 字典：仅当显示类型为下拉框/复选框/单选框时显示，用于绑定字典类型选项 -->
              <template v-else-if="column.key === 'dictType'">
                <TaktSelect
                  v-if="needDictTypeForHtmlType(record.htmlType)"
                  :model-value="record.dictType ?? undefined"
                  :options="dictTypeOptions"
                  :field-names="{ label: 'dictLabel', value: 'extLabel' }"
                  :placeholder="gentableColumnSelectPh('dicttype')"
                  allow-clear
                  class="column-cell-select"
                  style="width: 100%"
                  @update:model-value="(v: unknown) => { record.dictType = parseSelectToOptionalString(v) }"
                />
                <span
                  v-else
                  class="column-cell-muted"
                >—</span>
              </template>
              <!-- 排序：从 1 开始，支持拖拽调整顺序 -->
              <template v-else-if="column.key === 'orderNum'">
                <a-input-number
                  :value="record.orderNum ?? undefined"
                  :min="1"
                  class="column-cell-input"
                  style="width: 100%"
                  @update:value="(v: unknown) => { record.orderNum = parseSelectToOptionalNumber(v) }"
                />
              </template>
              <!-- 操作：删除行 -->
              <template v-else-if="column.key === 'action'">
                <a-button
                  type="link"
                  danger
                  @click="removeColumnRow(record as GenTableColumnRow)"
                >
                  {{ t('common.page.button.delete') }}
                </a-button>
              </template>
            </template>
            <template #emptyText>
              <a-empty
                v-if="!formData?.genTableId"
                :description="tf('column.emptysavefirst')"
              />
              <a-empty
                v-else
                :description="tf('column.emptynodata')"
              />
            </template>
          </a-table>
        </div>
      </a-tab-pane>
    </a-tabs>

    <!-- 字段配置表列设置（代码生成特例：仅业务列，不混入实体审计字段） -->
    <a-drawer
      v-model:open="columnTableColumnSettingVisible"
      :title="t('common.page.button.columnsetting')"
      placement="right"
      :width="400"
      class="gen-column-table-setting-drawer"
    >
      <template #extra>
        <a-button
          size="small"
          @click="handleColumnTableColumnSettingReset"
        >
          {{ t('common.page.button.reset') }}
        </a-button>
      </template>
      <a-checkbox-group
        v-model:value="columnTableVisibleKeys"
        class="gen-column-setting-group"
      >
        <div
          v-for="col in columnTableSettingOptions"
          :key="String(col.key)"
          class="gen-column-setting-item"
        >
          <a-checkbox :value="String(col.key)">
            {{ col.title }}
          </a-checkbox>
        </div>
      </a-checkbox-group>
    </a-drawer>
  </div>
</template>

<script setup lang="ts">
/**
 * 代码生成表配置表单：维护 GenTable 主表字段与 GenTableColumn 行列表。
 * 新建可从库选表（tenantCode + tableName）；编辑按 genTableId 拉列；提交由父级 createGenTable / updateGenTable。
 */
import { useI18n } from 'vue-i18n'
import { Modal } from 'ant-design-vue'
import type { FormInstance } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { RiDeleteRow, RiInsertRowBottom, RiRefreshLine } from '@remixicon/vue'
import type { GenTable } from '@/types/code/generator/genTable.types'
import type { GenTableColumn } from '@/types/code/generator/genTableColumn.types'
import { getGenTableColumnList } from '@/api/code/generator/genTableColumn.api'
import type { DatabaseInfo, DatabaseTableInfo } from '@/types/code/database/databaseInfo.types'
import { getDictTypeOptions } from '@/api/foundation/dictType.api'
import type {
  TaktDictSelectFieldNames,
  TaktDictSelectOption,
  TaktSelectOption,
  TaktTreeSelectOption,
} from '@/types/common.types'
import { getMenuTree } from '@/api/identity/menu.api'
import type { MenuTree } from '@/types/identity/menu.types'
import { taktIsLeafFlag } from '@/composables/useLazyTree'
import { HolderOutlined } from '@ant-design/icons-vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import {
  resolveTableScrollConfig,
  TAKT_TABLE_SCROLL_Y_MIN,
  resolveTableViewportHeight,
} from '@/utils/tableScroll'
import {
  generateLineNumberSequence,
  generateNextLineNumber,
  resolveMaxLineNumber,
} from '@/utils/taktSequence'

/** 表单可选字符串字段 */
type OptionalText = string | undefined

/** 表配置表单状态（与 GenTable DTO 对齐，含 UI 辅助字段 tenantCode） */
interface GenFormState {
  genTableId: string | undefined
  tenantCode: string | undefined
  dataSource: string | undefined
  tableName: string | undefined
  tableComment: string | undefined
  subTableName: string | undefined
  subTableFkName: string | undefined
  treeCode: string | undefined
  treeParentCode: string | undefined
  treeName: string | undefined
  inDatabase: number | undefined
  genTemplateCategory: string | undefined
  namePrefix: string | undefined
  entityNamespace: string | undefined
  entityClassName: string | undefined
  dtoNamespace: string | undefined
  dtoClassName: string | undefined
  serviceNamespace: string | undefined
  iServiceClassName: string | undefined
  serviceClassName: string | undefined
  controllerNamespace: string | undefined
  controllerClassName: string | undefined
  repositoryInterfaceNamespace: string | undefined
  iRepositoryClassName: string | undefined
  repositoryNamespace: string | undefined
  repositoryClassName: string | undefined
  genModuleName: string | undefined
  genBusinessName: string | undefined
  genFunctionName: string | undefined
  genFunction: string | undefined
  genMethod: number | undefined
  isRepository: number | undefined
  genPath: string | undefined
  parentMenuId: string | undefined
  isGenMenu: number | undefined
  isGenTranslation: number | undefined
  sortType: string | undefined
  sortField: string | undefined
  permsPrefix: string | undefined
  menuButtonGroup: string | undefined
  frontUi: number | undefined
  frontFormLayout: number | undefined
  frontBtnStyle: number | undefined
  isUseTabs: number | undefined
  tabsFieldCount: number | undefined
  genAuthor: string | undefined
  otherGenOptions: string | undefined
  columns: GenTableColumnRow[] | undefined
}

/** 字段配置表格行（columnId/orderNum 为 UI 别名，提交时映射后端 genTableColumnId/lineNumber） */
interface GenTableColumnRow extends Record<string, unknown> {
  columnId: number | string | undefined
  genTableId?: number | string | undefined
  tableId?: number | string | undefined
  databaseColumnName: OptionalText
  columnComment: OptionalText
  databaseDataType: OptionalText
  csharpDataType: OptionalText
  csharpColumnName: OptionalText
  length: number | undefined
  decimalDigits: number | undefined
  isPk: number | undefined
  isIncrement: number | undefined
  isRequired: number | undefined
  isCreate: number | undefined
  isUpdate: number | undefined
  isUnique: number | undefined
  isList: number | undefined
  isExport: number | undefined
  isSort: number | undefined
  isQuery: number | undefined
  queryType: OptionalText
  htmlType: OptionalText
  dictType: OptionalText
  orderNum: number | undefined
}

/** 组件入参 */
const props = withDefaults(
  defineProps<{
    /** 编辑时传入表配置；null 为新增 */
    formData?: Partial<GenTable> | null
    /** 租户业务库下拉（TaktDatabaseInfos） */
    databaseInfoList?: DatabaseInfo[]
    /** 当前租户下可选物理表 */
    databaseTables?: DatabaseTableInfo[]
    /** 物理表列表加载中 */
    databaseTablesLoading?: boolean
  }>(),
  { formData: null, databaseInfoList: () => [], databaseTables: () => [], databaseTablesLoading: false }
)

/** 向父组件抛出的事件 */
const emit = defineEmits<{
  /** 数据源 tenantCode 变更，父级加载 databaseTables */
  (e: 'config-change', tenantCode: string): void
}>()

/** i18n 翻译函数 */
const { t } = useI18n()

/** 静态 i18n 前缀 code.generator.page.form */
const FORM = 'code.generator.page.form'

/**
 * entity.gentable 字段标签（与 TaktGenTableI18nSeedData 键一致）
 * @param entityField 末段键名（小写）
 * @returns {string} 翻译结果
 */
function gentableLabel(entityField: string) {
  return t(`entity.gentable.${entityField}`)
}

/**
 * 下拉/树选占位（common.page.form.placeholder.select + entity.gentable.*）
 * @param entityField 末段键名（小写）
 * @returns {string} 占位文案
 */
function gentableSelectPh(entityField: string) {
  return t('common.page.form.placeholder.select', { field: gentableLabel(entityField) })
}

/**
 * 文本输入占位（common.page.form.placeholder.input + entity.gentable.*）
 * @param entityField 末段键名（小写）
 * @returns {string} 占位文案
 */
function gentableInputPh(entityField: string) {
  return t('common.page.form.placeholder.input', { field: gentableLabel(entityField) })
}

/**
 * entity.gentablecolumn 字段标签（TaktGenTableColumnI18nSeedData，末段与 C# 属性名小写一致）
 * @param entityField 末段键名（如 csharpdatatype、csharpcolumnname）
 * @returns {string} 列标题/标签文案
 */
function gentableColumnLabel(entityField: string) {
  return t(`entity.gentablecolumn.${entityField}`)
}

/**
 * entity.gentablecolumn 字段下拉占位（TaktGenTableColumnI18nSeedData + common.page.form.placeholder.select）
 * @param entityField 末段键名（小写，与种子一致）
 * @returns {string} 占位文案
 */
function gentableColumnSelectPh(entityField: string) {
  return t('common.page.form.placeholder.select', {
    field: gentableColumnLabel(entityField),
  })
}

/**
 * 必填校验规则（标签 entity.gentable.*，提示 common.page.form.placeholder.*）
 * @param entityField entity.gentable 末段键名（小写，与种子一致）
 * @param kind select 下拉/树选；input 文本输入
 * @returns Ant Design Form 规则项
 */
function rq(entityField: string, kind: 'select' | 'input' = 'input') {
  const messageKey =
    kind === 'select' ? 'common.page.form.placeholder.select' : 'common.page.form.placeholder.required'
  return [{ required: true, message: t(messageKey, { field: gentableLabel(entityField) }) }]
}

/**
 * 表单字段文案
 * @param suffix form 下的键后缀
 * @returns {string} 翻译结果
 */
function tf(suffix: string) {
  return t(`${FORM}.${suffix}`)
}

/** 「重置所有」：common.page.button.reset + common.page.button.all */
const resetAllButtonLabel = computed(
  () => `${t('common.page.button.reset')}${t('common.page.button.all')}`,
)

/** 顶层 Tab：table | column */
const activeTab = ref('table')
/** 表配置子 Tab：basic | gen | ... */
const tableSubTab = ref('basic')
/** Ant Design 表单实例 */
const formRef = ref<FormInstance>()
/** 主表表单状态 */
const formState = ref<GenFormState>(defaultFormState())
/** 字段配置行列表 */
const columnList = ref<GenTableColumnRow[]>([])
/** 字段列表初始快照（重置所有时恢复） */
const columnListBaseline = ref<GenTableColumnRow[]>([])
/** 字段配置表选中行 key（columnId 字符串） */
const selecmedColumnRowKeys = ref<string[]>([])

/**
 * 深拷贝字段行列表
 * @param rows 源列表
 * @returns {GenTableColumnRow[]} 拷贝结果
 */
function cloneColumnRows(rows: GenTableColumnRow[]): GenTableColumnRow[] {
  return JSON.parse(JSON.stringify(rows)) as GenTableColumnRow[]
}

/**
 * 同步字段列表初始快照
 * @param rows 当前列表
 */
function syncColumnListBaseline(rows: GenTableColumnRow[]): void {
  columnListBaseline.value = cloneColumnRows(normalizeColumnOrderNum(rows))
}

/**
 * 字段配置表 rowKey
 * @param record 字段行
 * @returns {string} columnId 字符串
 */
function getColumnRowKey(record: GenTableColumnRow): string {
  return String(record.columnId ?? '')
}

/** 字段配置表行选择 */
const columnRowSelection = computed(() => ({
  selecmedRowKeys: selecmedColumnRowKeys.value,
  onChange: (keys: (string | number)[]) => {
    selecmedColumnRowKeys.value = keys.map((k) => String(k))
  },
}))
/** 本地新增列临时 id 序号（负整数） */
let clientTempColumnSeq = 0
/** 字段列表加载中 */
const columnLoading = ref(false)
/** 字典数据 Pinia（code_generator_function、sys_yes_no 等） */
const dictDataStore = useDictDataStore()
/** 当前用户 Pinia（默认 genAuthor） */
const userStore = useUserStore()
/** 字典下拉 label/value 字段映射 */
const DICT_SELECT_FIELD_NAMES: TaktDictSelectFieldNames = {
  labelField: 'dictLabel',
  valueField: 'dictValue',
}
/**
 * 从字典缓存取 a-select 选项（label/value）
 * @param dictTypeCode 字典类型编码
 * @returns {TaktDictSelectOption[]} 下拉选项
 */
function getDictSelectOptions(dictTypeCode: string): TaktDictSelectOption[] {
  return dictDataStore.getDictOptionsForSelect(dictTypeCode, DICT_SELECT_FIELD_NAMES)
}
/** 字典类型选项（来自 /api/TaktDictTypes/options，供字段配置「字典」列选择要绑定的字典类型编码） */
const dictTypeOptions = ref<TaktSelectOption[]>([])
/** 模块名称选项（来自菜单树 Path 映射），用于模块名选择 */
const moduleOptionsTree = ref<TaktTreeSelectOption[]>([])
/** 模块名称树加载中 */
const moduleOptionsLoading = ref(false)

/**
 * 读取主表 id（优先 genTableId，兼容历史 id）
 * @param row 表配置或表单状态
 * @returns {string | undefined} 主表 id 字符串
 */
function readGenTablePrimaryId(row: Partial<GenTable> | GenFormState): string | undefined {
  const genId = 'genTableId' in row ? row.genTableId : undefined
  if (genId != null && String(genId).trim() !== '') return String(genId)
  const legacy = 'id' in row ? (row as { id?: unknown }).id : undefined
  if (typeof legacy === 'string' || typeof legacy === 'number') return String(legacy)
  return undefined
}

/**
 * Ant Design 选择/输入值 → string | undefined（不接受 null）
 * @param v 控件原始值
 * @returns {string | undefined} 去空后的字符串
 */
function parseSelectToOptionalString(v: unknown): string | undefined {
  if (v == null) return undefined
  if (typeof v === 'string' || typeof v === 'number') return String(v)
  if (typeof v === 'object' && 'value' in v) {
    return parseSelectToOptionalString((v as { value: unknown }).value)
  }
  return undefined
}

/**
 * Ant Design 数字控件值 → number | undefined
 * @param v 控件原始值
 * @returns {number | undefined} 有效数字或 undefined
 */
function parseSelectToOptionalNumber(v: unknown): number | undefined {
  if (v == null || v === '') return undefined
  if (typeof v === 'number') return Number.isFinite(v) ? v : undefined
  if (typeof v === 'string') {
    const n = Number(v)
    return Number.isFinite(n) ? n : undefined
  }
  if (typeof v === 'object' && 'value' in v) {
    return parseSelectToOptionalNumber((v as { value: unknown }).value)
  }
  return undefined
}

/**
 * TaktTreeSelect 单选值 → string | undefined
 * @param v 树选择原始值
 * @returns {string | undefined} 模块路径等字符串字段
 */
function parseTreeSelectToOptionalString(v: unknown): string | undefined {
  if (v == null) return undefined
  if (Array.isArray(v)) {
    const first = v[0]
    return first == null ? undefined : String(first)
  }
  if (typeof v === 'object' && 'value' in v) {
    return parseTreeSelectToOptionalString((v as { value: unknown }).value)
  }
  if (typeof v === 'string' || typeof v === 'number') return String(v)
  return undefined
}

/** 菜单权限组：选项与选中值仅用于 formState.menuButtonGroup，与生成功能同为字典多选 */
const menuButtonGroupOptions = computed(() => getDictSelectOptions('code_generator_button_category'))
/** 生成功能：选项与选中值仅用于 formState.genFunction */
const genFunctionOptions = computed(() => getDictSelectOptions('code_generator_function'))

/** 列中是否含 Status 字段（控制生成功能 Status 是否可选） */
const hasStatusColumn = computed(() => {
  return columnList.value.some(col => {
    const name = (col.csharpColumnName ?? col.databaseColumnName ?? '').toLowerCase()
    return name === 'status'
  })
})

/** 列中是否含 OrderNum/Sort 字段 */
const hasSortColumn = computed(() => {
  return columnList.value.some(col => {
    const name = (col.csharpColumnName ?? col.databaseColumnName ?? '').toLowerCase()
    return name === 'ordernum' || name === 'sort'
  })
})

/** 生成功能多选选项（无 Status/Sort 列时禁用对应项） */
const filteredGenFunctionOptions = computed(() => {
  const opts = genFunctionOptions.value
  return opts.map((opt: TaktDictSelectOption) => {
    const value = String(opt.value)
    // 如果没有 Status 字段，禁用 Status 功能
    if (value === 'Status' && !hasStatusColumn.value) {
      return { ...opt, disabled: true }
    }
    // 如果没有 Sort 字段，禁用 Sort 功能
    if (value === 'Sort' && !hasSortColumn.value) {
      return { ...opt, disabled: true }
    }
    return opt
  })
})

/** 是否（sys_yes_no）下拉，value 为 number */
const sysYesNoOptions = computed(() =>
  getDictSelectOptions('sys_yes_no').map((opt: TaktDictSelectOption) => ({
    label: opt.label,
    value: Number(opt.value),
  }))
)

/** GenMethod=2 时展示说明文案（路径由生成/预览时后端解析） */
const currentProjectPathDisplay = computed(() =>
  formState.value.genMethod === 2 ? tf('placeholder.currentprojectidle') : (formState.value.genPath || ''),
)

/** 是否编辑模式（有 genTableId） */
const isEditMode = computed(() => !!formState.value.genTableId)

/** 数据源下拉选项 */
const databaseConfigOptions = computed(() =>
  (props.databaseInfoList ?? []).map(c => ({ value: c.tenantCode, label: `${c.displayName} (${c.tenantCode})` }))
)
/** 同库其他表（主子表选父表） */
const databaseTableOptions = computed(() =>
  (props.databaseTables ?? []).map(t => ({
    value: t.tableName,
    label: t.tableComment ? `${t.tableName} - ${t.tableComment}` : t.tableName
  }))
)

/** 当前表列名下拉（排序/树/外键） */
const columnSelectOptions = computed(() =>
  columnList.value.map(col => {
    const name = col.databaseColumnName ?? col.csharpColumnName ?? ''
    const label = col.columnComment ? `${name} - ${col.columnComment}` : name
    return { value: name, label }
  })
)

/** 关联父表选项：同数据源下的表列表，排除当前表（仅主子表时用） */
const subTableNameOptions = computed(() => {
  const current = formState.value.tableName
  return (props.databaseTables ?? [])
    .filter(t => t.tableName !== current)
    .map(t => ({
      value: t.tableName,
      label: t.tableComment ? `${t.tableName} - ${t.tableComment}` : t.tableName
    }))
})

/** 菜单权限组多选：仅与 formState.menuButtonGroup 双向同步，选中值为字典 code_generator_button_category 的 value */
const menuButtonGroupSelect = computed({
  get() {
    const s = formState.value.menuButtonGroup
    if (!s || typeof s !== 'string') return []
    return s.split(/[,，\s]+/).map((x: string) => x.trim()).filter(Boolean)
  },
  set(v: (string | number)[] | undefined) {
    const arr = Array.isArray(v) ? v.map(x => String(x)).filter(Boolean) : []
    formState.value.menuButtonGroup = arr.length ? arr.join(',') : undefined
  }
})

/** 生成功能多选：仅与 formState.genFunction 双向同步，供「生成功能」a-checkbox-group 使用，选中值为字典 code_generator_function 的 value */
const genFunctionSelect = computed({
  get() {
    const s = formState.value.genFunction
    if (!s || typeof s !== 'string') return []
    return s.split(/[,，\s]+/).map((x: string) => x.trim()).filter(Boolean)
  },
  set(v: (string | number)[] | undefined) {
    const arr = Array.isArray(v) ? v.map(x => String(x)).filter(Boolean) : []
    formState.value.genFunction = arr.length ? arr.join(',') : undefined
  }
})

/** 菜单权限组全选：全选勾选态与半选态；默认全选 */
/** 菜单权限组全选勾选 */
const menuButtonGroupCheckAll = ref(true)
/** 菜单权限组半选态 */
const menuButtonGroupIndeterminate = ref(false)
/**
 * 菜单权限组全选变更
 * @param e 勾选事件
 */
function onMenuButtonGroupCheckAllChange(e: { target: { checked: boolean } }) {
  const checked = e.target.checked
  const opts = menuButtonGroupOptions.value
  formState.value.menuButtonGroup = checked && opts.length ? opts.map((o: TaktDictSelectOption) => String(o.value)).join(',') : undefined
  menuButtonGroupIndeterminate.value = false
  menuButtonGroupCheckAll.value = checked
}

/** 同步菜单权限组全选/半选态 */
watch(
  [() => formState.value.menuButtonGroup, menuButtonGroupOptions],
  () => {
    const list = (formState.value.menuButtonGroup ?? '')
      ? (formState.value.menuButtonGroup as string).split(/[,，\s]+/).map((x: string) => x.trim()).filter(Boolean)
      : []
    const total = menuButtonGroupOptions.value.length
    menuButtonGroupIndeterminate.value = list.length > 0 && list.length < total
    menuButtonGroupCheckAll.value = total > 0 && list.length === total
  },
  { immediate: true }
)

/** 生成功能：全选勾选态与半选态（与官方 a-checkbox-group 示例一致，选中值 = options 的 value 数组）；默认全选 */
/** 生成功能全选勾选 */
const genFunctionCheckAll = ref(true)
/** 生成功能半选态 */
const genFunctionIndeterminate = ref(false)
/**
 * 生成功能全选变更
 * @param e 勾选事件
 */
function onGenFunctionCheckAllChange(e: { target: { checked: boolean } }) {
  const checked = e.target.checked
  const opts = genFunctionOptions.value
  formState.value.genFunction = checked && opts.length ? opts.map((o: TaktDictSelectOption) => String(o.value)).join(',') : undefined
  genFunctionIndeterminate.value = false
  genFunctionCheckAll.value = checked
}

/** 同步生成功能全选/半选态 */
watch(
  [() => formState.value.genFunction, genFunctionOptions],
  () => {
    const list = (formState.value.genFunction ?? '')
      ? (formState.value.genFunction as string).split(/[,，\s]+/).map((x: string) => x.trim()).filter(Boolean)
      : []
    const total = genFunctionOptions.value.length
    genFunctionIndeterminate.value = list.length > 0 && list.length < total
    genFunctionCheckAll.value = total > 0 && list.length === total
  },
  { immediate: true }
)

/**
 * 挂载：加载字典、模块树、上级菜单树；新增态默认全选生成功能与按钮组
 */
onMounted(async () => {
  recalcColumnTableScrollY()
  window.addEventListener('resize', recalcColumnTableScrollY)
  document.addEventListener('fullscreenchange', recalcColumnTableScrollY)
  await dictDataStore.loadAllDictDataAsync()
  if (!formState.value.genTableId) {
    const genOpts = getDictSelectOptions('code_generator_function')
    const btnOpts = getDictSelectOptions('code_generator_button_category')
    if (genOpts.length > 0) formState.value.genFunction = genOpts.map((o: TaktDictSelectOption) => String(o.value)).join(',')
    if (btnOpts.length > 0) formState.value.menuButtonGroup = btnOpts.map((o: TaktDictSelectOption) => String(o.value)).join(',')
  }
  try {
    dictTypeOptions.value = await getDictTypeOptions()
  } catch {
    dictTypeOptions.value = []
  }
  try {
    moduleOptionsLoading.value = true
    const raw = await loadMenuTreeDeep('0', true)
    moduleOptionsTree.value = mapMenuTreeToTreeSelectOption(Array.isArray(raw) ? raw : [])
  } catch {
    moduleOptionsTree.value = []
  } finally {
    moduleOptionsLoading.value = false
  }
})

/**
 * 模块名下拉：按菜单 Path 映射；仍逐层拉取（DictValue≠Id，无法直接接 tree-options 懒加载）
 * @param parentId 父级 Id
 * @param includeDisabled 是否含禁用
 * @param depth 当前深度
 * @returns {Promise<MenuTree[]>} 嵌套树
 */
async function loadMenuTreeDeep(
  parentId: string,
  includeDisabled: boolean,
  depth = 0,
): Promise<MenuTree[]> {
  if (depth >= 10) return []
  const rows = await getMenuTree(parentId, includeDisabled)
  const list = Array.isArray(rows) ? rows : []
  await Promise.all(
    list.map(async (row) => {
      if (taktIsLeafFlag(row.isLeaf)) {
        row.children = []
        return
      }
      row.children = await loadMenuTreeDeep(String(row.menuId ?? row.id ?? ''), includeDisabled, depth + 1)
    }),
  )
  return list
}

/**
 * 将菜单 Path 还原为模块名称（帕斯卡加点）
 * @param path 路由 path，如 /accounting/controlling
 * @returns {string} 模块名，如 Accounting.Controlling
 */
function pathToModuleName(path: string | undefined): string {
  if (path == null || String(path).trim() === '') return ''
  const segments = String(path)
    .replace(/^\/+|\/+$/g, '')
    .split('/')
    .filter(Boolean)
  return segments
    .map(s => (s.length > 0 ? s.charAt(0).toUpperCase() + s.slice(1).toLowerCase() : s))
    .join('.')
}

/**
 * 将 menuCode 转为模块名（帕斯卡加点）
 * @param menuCode 菜单编码，如 ACCOUNTING_FINANCIAL
 * @returns {string} 模块名，如 Accounting.Financial
 */
function menuCodeToModuleName(menuCode: string | undefined): string {
  if (menuCode == null || String(menuCode).trim() === '') return ''
  const segments = String(menuCode).trim().split(/[._-]+/).filter(Boolean)
  return segments
    .map(s => (s.length > 0 ? s.charAt(0).toUpperCase() + s.slice(1).toLowerCase() : s))
    .join('.')
}

/**
 * 收窄为可赋给表单 string 的安全字符串
 * @param v 任意值
 * @returns {string} trim 后的字符串，无法转换时 ''
 */
function asTrimmedString(v: unknown): string {
  if (v == null) return ''
  if (typeof v === 'string') return v.trim()
  if (typeof v === 'number' || typeof v === 'boolean') return String(v)
  return ''
}

/**
 * 非空字符串参数（空串视为 undefined）
 * @param v 任意值
 * @returns {string | undefined} 有效字符串或 undefined
 */
function optionalStringParam(v: unknown): string | undefined {
  const s = asTrimmedString(v)
  return s === '' ? undefined : s
}

/**
 * 当前登录用户展示名（用于默认 genAuthor）
 * @returns {string | undefined} 用户名
 */
function readCurrentUserDisplayName(): string | undefined {
  const name = asTrimmedString(userStore.userName)
  return name || undefined
}

/** 菜单树 API 扩展字段（与 MenuTree 代码生成类型补全） */
type MenuTreeWithSelectExtras = {
  menuId?: string
  menuName?: string
  menuCode?: string
  path?: string
  routePath?: string
  orderNum?: number
  sortOrder?: number
  dictValue?: string | number
  dictLabel?: string | number
  children?: MenuTree[]
}

/**
 * 菜单树 → TaktTreeSelectOption（dictValue 为推导的模块名）
 * @param trees 菜单树节点
 * @returns {TaktTreeSelectOption[]} 树形下拉选项
 */
function mapMenuTreeToTreeSelectOption(trees: MenuTree[]): TaktTreeSelectOption[] {
  return trees.map((node): TaktTreeSelectOption => {
    const n = node as MenuTreeWithSelectExtras
    const path = n.path ?? n.routePath ?? undefined
    const menuCode = n.menuCode ?? undefined
    const dictVal = n.dictValue
    const idFallback = n.menuId != null ? String(n.menuId) : ''
    const moduleName =
      pathToModuleName(optionalStringParam(path)) ||
      menuCodeToModuleName(optionalStringParam(menuCode)) ||
      (dictVal != null && dictVal !== '' ? String(dictVal) : '') ||
      idFallback
    return {
      dictLabel: n.menuName ?? (n.dictLabel != null ? String(n.dictLabel) : ''),
      dictValue: moduleName,
      sortOrder: n.orderNum ?? n.sortOrder ?? 0,
      ...(n.children?.length
        ? { children: mapMenuTreeToTreeSelectOption(n.children as MenuTree[]) }
        : {})
    }
  })
}

/**
 * 切换租户数据源
 * @param value 下拉选中值
 */
function handleConfigChange(value: unknown) {
  const code = value != null ? String(value) : undefined
  if (code) {
    const info = props.databaseInfoList?.find(c => c.tenantCode === code)
    if (info) formState.value.dataSource = `${info.displayName}:${info.tenantCode}`
    emit('config-change', code)
  } else {
    formState.value.dataSource = undefined
  }
  formState.value.tableName = undefined
  formState.value.tableComment = undefined
}

/** DB类型 -> C#类型 级联映射（与后端 MapDbTypeToCsharp 一致） */
const DB_TYPE_TO_CSHARP: Record<string, string> = {
  bigint: 'long',
  bit: 'bool',
  datetime: 'DateTime',
  decimal: 'decimal',
  int: 'int',
  ntext: 'string',
  nvarchar: 'string',
  text: 'string',
  uniqueidentifier: 'Guid',
  varchar: 'string'
}

/** 全部 C#类型选项（来自字典 code_generator_csharp_data_type） */
const columnCsharpTypeOptions = computed(() =>
  getDictSelectOptions('code_generator_csharp_data_type').map((o: TaktDictSelectOption) => ({ label: o.label, value: o.value }))
)

/**
 * 按 DB 类型过滤 C# 类型下拉项
 * @param dbType 数据库类型
 * @returns {{ label: string; value: string | number }[]} 选项列表
 */
function getCsharpTypeOptionsForRow(dbType: string | undefined) {
  const all = columnCsharpTypeOptions.value
  if (!dbType || !DB_TYPE_TO_CSHARP[dbType]) return all
  const mapped = DB_TYPE_TO_CSHARP[dbType]
  const single = all.filter((o: { label: string; value: string | number }) => String(o.value) === mapped)
  return single.length ? single : all
}

/** string 默认长度 */
const DEFAULT_LENGTH_STRING = 64
/** decimal 默认整数位 */
const DEFAULT_LENGTH_DECIMAL = 18
/** decimal 默认小数位 */
const DEFAULT_DECIMAL_DIGITS = 2

/**
 * 是否需要填写「长度」
 * @param csharpType C# 类型
 * @returns {boolean} 是否显示长度输入
 */
function needLengthForCsharpType(csharpType: string | number | undefined): boolean {
  if (csharpType == null) return false
  const t = String(csharpType).trim()
  return t === 'string' || t === 'decimal'
}

/**
 * 是否需要填写「精度」（小数位）
 * @param csharpType C# 类型
 * @returns {boolean} 是否显示精度输入
 */
function needDecimalDigitsForCsharpType(csharpType: string | number | undefined): boolean {
  if (csharpType == null) return false
  return String(csharpType).trim() === 'decimal'
}

/**
 * 按 C# 类型设置长度/精度默认值
 * @param record 字段行
 * @param csharpType C# 类型
 */
function applyLengthDecimalDefaults(record: GenTableColumnRow, csharpType: string) {
  const t = csharpType.trim()
  if (t === 'string') {
    record.length = DEFAULT_LENGTH_STRING
    record.decimalDigits = undefined
  } else if (t === 'decimal') {
    record.length = DEFAULT_LENGTH_DECIMAL
    record.decimalDigits = DEFAULT_DECIMAL_DIGITS
  } else {
    record.length = undefined
    record.decimalDigits = undefined
  }
}

/**
 * 字段行 C# 类型变更
 * @param record 字段行
 * @param csharpType 新 C# 类型
 */
function onColumnCsharpTypeChange(record: GenTableColumnRow, csharpType: string) {
  if (!record || typeof record !== 'object') return
  const r = record
  applyLengthDecimalDefaults(r, csharpType)
}

/**
 * 字段行 DB 类型变更（级联 C# 类型）
 * @param record 字段行
 * @param dbType 新数据库类型
 */
function onColumnDbTypeChange(record: GenTableColumnRow, dbType: string) {
  const mapped = dbType ? DB_TYPE_TO_CSHARP[dbType] : undefined
  if (mapped === undefined || !record || typeof record !== 'object') return
  const r = record
  r.csharpDataType = mapped
  applyLengthDecimalDefaults(r, mapped)
}

/** 需要绑定 dictType 的 htmlType 列表 */
const HTML_TYPES_NEED_DICT = ['select', 'checkbox', 'radio']

/**
 * 显示类型是否需要字典
 * @param htmlType 显示类型
 * @returns {boolean} 是否需要 dictType
 */
function needDictTypeForHtmlType(htmlType: string | number | undefined): boolean {
  if (htmlType == null) return false
  return HTML_TYPES_NEED_DICT.includes(String(htmlType))
}

/**
 * 字段行显示类型变更
 * @param record 字段行
 * @param value 新 htmlType
 */
function onColumnHtmlTypeChange(record: GenTableColumnRow, value: string | number | (string | number)[] | undefined) {
  const v = Array.isArray(value) ? value[0] : value
  if (!record || typeof record !== 'object') return
  if (!needDictTypeForHtmlType(v)) record.dictType = undefined
}

/**
 * 是否查询开关变更：关时清空 queryType；开时按 C# 类型给默认 like/eq
 * @param record 字段行
 * @param isOn 是否启用查询
 */
function onColumnIsQueryChange(record: GenTableColumnRow, isOn?: boolean) {
  if (!record || typeof record !== 'object') return
  const enabled = isOn ?? record.isQuery === 1
  if (!enabled) {
    record.queryType = undefined
    return
  }
  if (!record.queryType?.trim()) {
    record.queryType = isStringLikeCsharpType(record.csharpDataType) ? 'like' : 'eq'
  }
}

/** C# 类型是否默认 like 查询 */
function isStringLikeCsharpType(csharpDataType: string | undefined): boolean {
  const t = csharpDataType?.trim().toLowerCase()
  return !t || t === 'string' || t === 'guid'
}

/** 数据库列名下划线命名正则 */
const SNAKE_CASE_REGEX = /^[a-z][a-z0-9]*(_[a-z0-9]+)*$/

/**
 * 是否为 snake_case
 * @param s 待校验字符串
 * @returns {boolean} 是否合法
 */
function isSnakeCase(s: string | undefined): boolean {
  if (s == null || String(s).trim() === '') return true
  return SNAKE_CASE_REGEX.test(String(s).trim())
}

/** C# 列名 PascalCase 正则 */
const PASCAL_CASE_REGEX = /^[A-Z][a-zA-Z0-9]*$/

/**
 * 是否为 PascalCase
 * @param s 待校验字符串
 * @returns {boolean} 是否合法
 */
function isPascalCase(s: string | undefined): boolean {
  if (s == null || String(s).trim() === '') return true
  return PASCAL_CASE_REGEX.test(String(s).trim())
}

/** 数据表名：必填 + 小写下划线格式（xxxx_xxxx_xxx） */
const tableNameRules = computed(() => [
  ...rq('tablename', 'input'),
  {
    validator: (_rule: unknown, v: string) =>
      !v || isSnakeCase(v)
        ? Promise.resolve()
        : Promise.reject(new Error(t(`${FORM}.validation.tablenameformat`)))
  }
])

/** 命名空间前缀校验：必填 + 帕斯卡命名 */
const namePrefixPascalRules = computed(() => [
  ...rq('nameprefix', 'input'),
  {
    validator: (_rule: unknown, v: string) =>
      !v || isPascalCase(v)
        ? Promise.resolve()
        : Promise.reject(new Error(t(`${FORM}.validation.nameprefixpascal`)))
  }
])

/** 根据 genTemplateCategory 决定：主子表时 subTableName、subTableFkName 必填 */
/** 主子表：父表名必填规则 */
const subTableNameRules = computed(() =>
  formState.value.genTemplateCategory === 'sub' ? rq('subtablename', 'select') : []
)
/** 主子表：外键列必填规则 */
const subTableFkNameRules = computed(() =>
  formState.value.genTemplateCategory === 'sub' ? rq('subtablefkname', 'select') : []
)
/** 根据 genTemplateCategory 决定：树表时 treeCode、treeName、treeParentCode 必填 */
/** 树表：树编码列必填 */
const treeCodeRules = computed(() =>
  formState.value.genTemplateCategory === 'tree' ? rq('treecode', 'select') : []
)
/** 树表：树名称列必填 */
const treeNameRules = computed(() =>
  formState.value.genTemplateCategory === 'tree' ? rq('treename', 'select') : []
)
/** 树表：树父编码列必填 */
const treeParentCodeRules = computed(() =>
  formState.value.genTemplateCategory === 'tree' ? rq('treeparentcode', 'select') : []
)
/** 生成方式：选中「自定义路径」(1) 时生成路径必填，zip(0)、当前项目(2) 时可空 */
const genPathRules = computed(() =>
  Number(formState.value.genMethod) === 1 ? rq('genpath', 'select') : []
)
/** 是否生成菜单：选「是」(1) 时上级菜单必填，选「否」(0) 时可空 */
const parentMenuIdRules = computed(() =>
  Number(formState.value.isGenMenu) === 1 ? rq('parentmenuid', 'select') : []
)
/** 是否生成仓储：选「是」(1) 时仓储相关字段必填 */
const repositoryInterfaceNamespaceRules = computed(() =>
  Number(formState.value.isRepository) === 1 ? rq('repositoryinterfacenamespace', 'input') : []
)
/** 仓储：IRepository 类名必填 */
const iRepositoryClassNameRules = computed(() =>
  Number(formState.value.isRepository) === 1 ? rq('irepositoryclassname', 'input') : []
)
/** 仓储：实现类命名空间必填 */
const repositoryNamespaceRules = computed(() =>
  Number(formState.value.isRepository) === 1 ? rq('repositorynamespace', 'input') : []
)
/** 仓储：Repository 类名必填 */
const repositoryClassNameRules = computed(() =>
  Number(formState.value.isRepository) === 1 ? rq('repositoryclassname', 'input') : []
)
/** 是否使用 Tabs：选「是」(1) 时 Tabs 字段数必填 */
const tabsFieldCountRules = computed(() =>
  Number(formState.value.isUseTabs) === 1 ? rq('tabsfieldcount', 'input') : []
)

/** 新增空白字段行 */
function addColumnRow() {
  const tidRaw = formState.value.genTableId
  const tableIdNum =
    tidRaw != null && String(tidRaw).trim() !== '' && !Number.isNaN(Number(tidRaw)) ? Number(tidRaw) : undefined
  const nextNum = columnList.value.length + 1
  const defaultDbName = `column_${nextNum}`
  const defaultCsharpName = `Column${nextNum}`
  clientTempColumnSeq += 1
  const newRow: GenTableColumnRow = {
    columnId: -clientTempColumnSeq,
    genTableId: tidRaw != null ? String(tidRaw) : undefined,
    tableId: tableIdNum,
    databaseColumnName: defaultDbName,
    columnComment: defaultCsharpName,
    databaseDataType: 'nvarchar',
    csharpDataType: 'string',
    csharpColumnName: defaultCsharpName,
    length: DEFAULT_LENGTH_STRING,
    decimalDigits: undefined,
    isPk: 0,
    isIncrement: 0,
    isRequired: 1,
    isCreate: 1,
    isUpdate: 1,
    isUnique: 0,
    isList: 1,
    isExport: 1,
    isSort: 0,
    isQuery: 0,
    queryType: undefined,
    htmlType: 'input',
    dictType: undefined,
    orderNum: generateNextLineNumber(
      resolveMaxLineNumber(columnList.value.map((row) => Number(row.orderNum ?? 0))),
    )
  }
  columnList.value = [...columnList.value, newRow]
}

/**
 * 删除字段行
 * @param record 表格行
 */
function removeColumnRow(record: GenTableColumnRow) {
  const id = (record).columnId
  if (id == null) return
  const idStr = String(id)
  const nextList = columnList.value.filter(r => r.columnId !== id)
  columnList.value = normalizeColumnOrderNum(nextList)
  selecmedColumnRowKeys.value = selecmedColumnRowKeys.value.filter((k) => k !== idStr)
}

/**
 * 删除选中的字段行
 */
function handleDeleteSelecmedColumnRows(): void {
  if (selecmedColumnRowKeys.value.length === 0) return
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      count: selecmedColumnRowKeys.value.length,
      entity: t('entity.gentablecolumn._self'),
    }),
    okText: t('common.page.button.confirm'),
    cancelText: t('common.page.button.cancel'),
    onOk: () => {
      const keySet = new Set(selecmedColumnRowKeys.value)
      const nextList = columnList.value.filter((r) => !keySet.has(getColumnRowKey(r)))
      columnList.value = normalizeColumnOrderNum(nextList)
      selecmedColumnRowKeys.value = []
    },
  })
}

/**
 * 重置所有字段行（已保存表从服务端重载；未保存表恢复打开/导入时的快照）
 */
function handleResetAllColumnRows(): void {
  Modal.confirm({
    title: t('common.tip.confirm.title', { action: resetAllButtonLabel.value }),
    content: t('common.tip.confirm.question', { action: resetAllButtonLabel.value }),
    okText: t('common.page.button.confirm'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const tableId = formState.value.genTableId
      selecmedColumnRowKeys.value = []
      if (tableId != null && String(tableId).trim() !== '') {
        await loadColumns(String(tableId))
        return
      }
      columnList.value = cloneColumnRows(columnListBaseline.value)
    },
  })
}

/**
 * 按当前 orderNum 排序后重排行号为 10、20、30…（步长 10，与后端 TaktSequenceDefaults 一致）
 * @param list 字段行列表
 * @returns {GenTableColumnRow[]} 排序后的新数组
 */
function normalizeColumnOrderNum(list: GenTableColumnRow[]) {
  const sorted = [...list].sort((a, b) => Number(a.orderNum ?? 0) - Number(b.orderNum ?? 0))
  const lineNumbers = generateLineNumberSequence(sorted.length, 0)
  sorted.forEach((row, i) => {
    row.orderNum = lineNumbers[i]
  })
  return sorted
}

/** 拖拽中的行索引 */
const columnDragRowIndex = ref<number | null>(null)

/**
 * 字段行在 columnList 中的索引
 * @param record 表格行
 * @returns {number} 索引，未找到为 -1
 */
function getColumnRowIndex(record: GenTableColumnRow): number {
  return columnList.value.findIndex(r => r.columnId === record.columnId)
}

/**
 * 开始拖拽字段行
 * @param e 拖拽事件
 * @param record 被拖行
 */
function onColumnDragStart(e: DragEvent, record: GenTableColumnRow) {
  const index = getColumnRowIndex(record)
  if (index < 0) return
  columnDragRowIndex.value = index
  e.dataTransfer!.effectAllowed = 'move'
  e.dataTransfer!.setData('text/plain', String(index))
  const tr = (e.target as HTMLElement).closest('tr')
  if (tr) e.dataTransfer!.setDragImage(tr, 0, 0)
}

/**
 * 拖拽经过行（允许 drop）
 * @param e 拖拽事件
 */
function onColumnDragOver(e: DragEvent) {
  e.preventDefault()
  e.dataTransfer!.dropEffect = 'move'
}

/**
 * 放置拖拽行并重排 orderNum
 * @param e 放置事件
 * @param dropRecord 目标行
 */
function onColumnDrop(e: DragEvent, dropRecord: GenTableColumnRow) {
  e.preventDefault()
  const dragIndex = columnDragRowIndex.value
  columnDragRowIndex.value = null
  if (dragIndex == null) return
  const dropIndex = getColumnRowIndex(dropRecord)
  if (dragIndex === dropIndex) return
  const list = [...columnList.value]
  const [removed] = list.splice(dragIndex, 1)
  if (removed == null) return
  list.splice(dropIndex, 0, removed)
  columnList.value = normalizeColumnOrderNum(list)
}

/**
 * 字段表格行属性（拖拽样式与 drop 处理）
 * @param record 行数据
 * @param index 行索引
 * @returns 行 customRow 配置
 */
function columnTableCustomRow(record: GenTableColumnRow, index: number) {
  return {
    class: columnDragRowIndex.value === index ? 'column-row-dragging' : '',
    onDragover: onColumnDragOver,
    onDrop: (e: DragEvent) => onColumnDrop(e, record)
  }
}

/** 弹窗内字段配置表纵向预留（75vh 弹窗壳 + Tab/工具栏/底栏，与 generator/index TaktModal body-style 对齐） */
const GEN_FORM_COLUMN_TABLE_MODAL_CHROME_PX = 220
/** 全屏模式下字段配置表上方预留（Tab + 工具栏 + 内边距） */
const GEN_FORM_COLUMN_TABLE_FULLSCREEN_CHROME_PX = 140

/**
 * 计算弹窗内字段配置表 scroll.y（固定布局高度，与数据行数无关）
 * @param viewportHeight 视口高度
 * @returns scroll.y 像素值
 */
function computeGenFormColumnTableScrollYPx(viewportHeight?: number): number {
  const vh = resolveTableViewportHeight(viewportHeight)
  if (typeof document !== 'undefined' && document.fullscreenElement) {
    return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor(vh - GEN_FORM_COLUMN_TABLE_FULLSCREEN_CHROME_PX))
  }
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor(vh * 0.75 - GEN_FORM_COLUMN_TABLE_MODAL_CHROME_PX))
}

/** 字段配置表 scroll.y（resize / 全屏时重算） */
const columnTableScrollYPx = ref(computeGenFormColumnTableScrollYPx())

/** 重算字段配置表 scroll.y */
function recalcColumnTableScrollY(): void {
  columnTableScrollYPx.value = computeGenFormColumnTableScrollYPx(
    typeof window !== 'undefined' ? window.innerHeight : undefined,
  )
}

/** 字段配置表列默认宽度（可拖拽调节） */
const COLUMN_TABLE_DEFAULT_WIDTHS: Record<string, number> = {
  dragSort: 36,
  orderNum: 72,
  databaseColumnName: 130,
  columnComment: 100,
  databaseDataType: 88,
  csharpDataType: 88,
  csharpColumnName: 110,
  length: 64,
  decimalDigits: 64,
  isPk: 64,
  isIncrement: 64,
  isRequired: 64,
  isQuery: 64,
  isCreate: 64,
  isUpdate: 64,
  isUnique: 64,
  isList: 64,
  isExport: 64,
  isSort: 64,
  queryType: 88,
  htmlType: 88,
  dictType: 95,
  action: 72,
}

/** 字段配置表列宽（拖拽后持久于当前表单会话） */
const columnTableWidths = ref<Record<string, number>>({ ...COLUMN_TABLE_DEFAULT_WIDTHS })

/** 字段配置表可勾选业务列 key（不含拖拽列与操作列；顺序与表格一致） */
const COLUMN_TABLE_BUSINESS_KEYS = [
  'orderNum',
  'databaseColumnName',
  'columnComment',
  'databaseDataType',
  'csharpDataType',
  'csharpColumnName',
  'length',
  'decimalDigits',
  'isPk',
  'isIncrement',
  'isRequired',
  'isQuery',
  'isCreate',
  'isUpdate',
  'isUnique',
  'isList',
  'isExport',
  'isSort',
  'queryType',
  'htmlType',
  'dictType'] as const

/**
 * 字段配置表默认可见业务列 key
 * @returns 业务列 key 列表
 */
function resolveColumnTableDefaultVisibleKeys(): string[] {
  return [...COLUMN_TABLE_BUSINESS_KEYS]
}

/** 字段配置表列设置抽屉显隐 */
const columnTableColumnSettingVisible = ref(false)
/** 字段配置表可见列 key（默认展示全部业务列） */
const columnTableVisibleKeys = ref<string[]>(resolveColumnTableDefaultVisibleKeys())

/**
 * 打开字段配置表列设置抽屉
 */
function handleColumnTableColumnSetting(): void {
  columnTableColumnSettingVisible.value = true
}

/** 字段配置表列设置恢复默认 */
function handleColumnTableColumnSettingReset(): void {
  columnTableVisibleKeys.value = resolveColumnTableDefaultVisibleKeys()
}

/**
 * 字段配置表列宽拖拽
 * @param w 新宽度
 * @param col 列定义
 */
function handleColumnTableResizeColumn(w: number, col: TableColumnsType[number]): void {
  const key = String(col.key ?? '')
  if (!key) return
  columnTableWidths.value = { ...columnTableWidths.value, [key]: w }
  ;(col as { width?: number }).width = w
}

/** 全屏切换后重算表体高度 */
function handleColumnTableFullscreen(): void {
  recalcColumnTableScrollY()
}

onBeforeUnmount(() => {
  window.removeEventListener('resize', recalcColumnTableScrollY)
  document.removeEventListener('fullscreenchange', recalcColumnTableScrollY)
})

/** 字段配置表格列定义（排序号紧随拖拽列；列宽可拖拽调节） */
const columnTableColumns = computed<TableColumnsType>(() => {
  const withPresentation = (col: TableColumnsType[number]): TableColumnsType[number] => {
    const key = String(col.key ?? '')
    const width = columnTableWidths.value[key] ?? (typeof col.width === 'number' ? col.width : undefined)
    return { ...col, width, resizable: true }
  }
  return [
    withPresentation({ title: t(`${FORM}.column.dragsort`), key: 'dragSort', width: 36, align: 'center', class: 'column-drag-cell' }),
    withPresentation({ title: gentableColumnLabel('linenumber'), dataIndex: 'orderNum', key: 'orderNum', width: 72 }),
    withPresentation({
      title: gentableColumnLabel('databasecolumnname'),
      dataIndex: 'databaseColumnName',
      key: 'databaseColumnName',
      width: 130,
      ellipsis: true,
    }),
    withPresentation({ title: gentableColumnLabel('columncomment'), dataIndex: 'columnComment', key: 'columnComment', width: 100 }),
    withPresentation({ title: gentableColumnLabel('databasedatatype'), dataIndex: 'databaseDataType', key: 'databaseDataType', width: 88 }),
    withPresentation({ title: gentableColumnLabel('csharpdatatype'), dataIndex: 'csharpDataType', key: 'csharpDataType', width: 88 }),
    withPresentation({ title: gentableColumnLabel('csharpcolumnname'), dataIndex: 'csharpColumnName', key: 'csharpColumnName', width: 110 }),
    withPresentation({ title: gentableColumnLabel('length'), dataIndex: 'length', key: 'length', width: 64 }),
    withPresentation({ title: gentableColumnLabel('decimaldigits'), dataIndex: 'decimalDigits', key: 'decimalDigits', width: 64 }),
    withPresentation({ title: gentableColumnLabel('ispk'), dataIndex: 'isPk', key: 'isPk', width: 64 }),
    withPresentation({ title: gentableColumnLabel('isincrement'), dataIndex: 'isIncrement', key: 'isIncrement', width: 64 }),
    withPresentation({ title: gentableColumnLabel('isrequired'), dataIndex: 'isRequired', key: 'isRequired', width: 64 }),
    withPresentation({ title: gentableColumnLabel('isquery'), dataIndex: 'isQuery', key: 'isQuery', width: 64 }),
    withPresentation({ title: gentableColumnLabel('iscreate'), dataIndex: 'isCreate', key: 'isCreate', width: 64 }),
    withPresentation({ title: gentableColumnLabel('isupdate'), dataIndex: 'isUpdate', key: 'isUpdate', width: 64 }),
    withPresentation({ title: gentableColumnLabel('isunique'), dataIndex: 'isUnique', key: 'isUnique', width: 64 }),
    withPresentation({ title: gentableColumnLabel('islist'), dataIndex: 'isList', key: 'isList', width: 64 }),
    withPresentation({ title: gentableColumnLabel('isexport'), dataIndex: 'isExport', key: 'isExport', width: 64 }),
    withPresentation({ title: gentableColumnLabel('issort'), dataIndex: 'isSort', key: 'isSort', width: 64 }),
    withPresentation({ title: gentableColumnLabel('querytype'), dataIndex: 'queryType', key: 'queryType', width: 88 }),
    withPresentation({ title: gentableColumnLabel('htmltype'), dataIndex: 'htmlType', key: 'htmlType', width: 88 }),
    withPresentation({ title: gentableColumnLabel('dicttype'), dataIndex: 'dictType', key: 'dictType', width: 95 }),
    withPresentation({ title: t('common.action.operation'), key: 'action', width: 72, fixed: 'right' })]
})

/** 按列设置过滤后的字段配置表展示列（拖拽列与操作列始终保留） */
const columnTableDisplayColumns = computed<TableColumnsType>(() => {
  const cols = columnTableColumns.value
  const keys = columnTableVisibleKeys.value
  if (!keys.length) return cols
  const keySet = new Set(keys.map((k) => String(k)))
  return cols.filter((c) => {
    const k = String(c.key ?? '')
    return k === 'dragSort' || k === 'action' || keySet.has(k)
  })
})

/** 列设置抽屉选项（仅业务列，不含 dragSort / action） */
const columnTableSettingOptions = computed(() =>
  columnTableColumns.value.filter((col) => {
    const key = String(col.key ?? '')
    return key && key !== 'dragSort' && key !== 'action'
  }),
)

/** 字段配置表 scroll（横向为列宽总和；纵向固定，参照 takt-single-table） */
const columnTableScroll = computed(() =>
  resolveTableScrollConfig({
    columns: columnTableDisplayColumns.value,
    enableVerticalScroll: true,
    verticalScrollHeight: columnTableScrollYPx.value,
  }),
)

/** 字段配置表固定表体高度 CSS 变量（与 scroll.y 一致） */
const columnTableBodyStyle = computed(() => ({
  '--takt-table-scroll-y': `${columnTableScrollYPx.value}px`,
}))

/** 字段行数较多时启用虚拟滚动（07-overflow-vue） */
const columnTableVirtual = computed(() => columnList.value.length > 100)

/**
 * 表单初始状态（新增默认值）
 * @returns {GenFormState} 默认状态
 */
function defaultFormState(): GenFormState {
  return {
    genTableId: undefined,
    tenantCode: undefined,
    dataSource: undefined,
    tableName: undefined,
    tableComment: undefined,
    subTableName: undefined,
    subTableFkName: undefined,
    treeCode: undefined,
    treeParentCode: undefined,
    treeName: undefined,
    inDatabase: 1,
    genTemplateCategory: 'crud',
    namePrefix: 'Takt',
    entityNamespace: 'Takt.Domain.Entities',
    entityClassName: undefined,
    dtoNamespace: 'Takt.Application.Dtos',
    dtoClassName: undefined,
    serviceNamespace: 'Takt.Application.Services',
    iServiceClassName: undefined,
    serviceClassName: undefined,
    controllerNamespace: 'Takt.WebApi.Controllers',
    controllerClassName: undefined,
    repositoryInterfaceNamespace: 'Takt.Domain.Repositories',
    iRepositoryClassName: undefined,
    repositoryNamespace: 'Takt.Infrastructure.Repositories',
    repositoryClassName: undefined,
    genModuleName: undefined,
    genBusinessName: undefined,
    genFunctionName: undefined,
    genFunction: 'Query,Create,Update,Delete,Status,Sort,Template,Import,Export',
    genMethod: 1,
    isRepository: 0,
    genPath: '/',
    parentMenuId: undefined,
    isGenMenu: 1,
    isGenTranslation: 1,
    sortType: 'asc',
    sortField: undefined,
    permsPrefix: undefined,
    menuButtonGroup: undefined,
    frontUi: 2,
    frontFormLayout: 24,
    frontBtnStyle: 1,
    isUseTabs: 1,
    tabsFieldCount: 10,
    genAuthor: undefined,
    otherGenOptions: undefined,
    columns: undefined
  }
}

/**
 * 将后端列 DTO 映射为表单行（columnId/orderNum 为 UI 字段别名）
 * @param col 后端列配置
 * @returns {GenTableColumnRow} 表单行
 */
function mapGenTableColumnToRow(col: GenTableColumn): GenTableColumnRow {
  return {
    columnId: col.genTableColumnId,
    genTableId: col.genTableId,
    tableId: col.genTableId,
    databaseColumnName: col.databaseColumnName,
    columnComment: col.columnComment,
    databaseDataType: col.databaseDataType,
    csharpDataType: col.csharpDataType,
    csharpColumnName: col.csharpColumnName,
    length: col.length,
    decimalDigits: col.decimalDigits,
    isPk: col.isPk,
    isIncrement: col.isIncrement,
    isRequired: col.isRequired,
    isCreate: col.isCreate,
    isUpdate: col.isUpdate,
    isUnique: col.isUnique,
    isList: col.isList,
    isExport: col.isExport,
    isSort: col.isSort,
    isQuery: col.isQuery,
    queryType: col.queryType,
    htmlType: col.htmlType,
    dictType: col.dictType,
    orderNum: col.lineNumber,
  }
}

/**
 * 按表 id 分页拉取字段配置
 * @param genTableId 代码生成表 id
 * @returns {Promise<void>}
 */
async function loadColumns(genTableId: string) {
  columnLoading.value = true
  try {
    const result = await getGenTableColumnList({
      genTableId,
      pageIndex: 1,
      pageSize: 500,
    })
    const rows = (result?.data ?? []).map(mapGenTableColumnToRow)
    columnList.value = normalizeColumnOrderNum(rows)
    syncColumnListBaseline(columnList.value)
    if (!formState.value.sortField?.trim() && columnList.value.length > 0) {
      formState.value.sortField = resolveDefaultSortField(columnList.value)
    }
  } catch {
    columnList.value = []
    columnListBaseline.value = []
  } finally {
    columnLoading.value = false
  }
}

/** genTemplateCategory 与 sub/tree 字段相斥：切换模板时清空另一类字段 */
watch(
  () => formState.value.genTemplateCategory,
  (next, prev) => {
    if (prev === undefined) return
    if (next !== 'sub') {
      formState.value.subTableName = undefined
      formState.value.subTableFkName = undefined
    }
    if (next !== 'tree') {
      formState.value.treeCode = undefined
      formState.value.treeParentCode = undefined
      formState.value.treeName = undefined
    }
  }
)

/** isRepository 与仓储相关字段相斥：选「否」(0) 时清空仓储命名空间/类名 */
watch(
  () => formState.value.isRepository,
  (next) => {
    if (next === 0) {
      formState.value.repositoryInterfaceNamespace = undefined
      formState.value.iRepositoryClassName = undefined
      formState.value.repositoryNamespace = undefined
      formState.value.repositoryClassName = undefined
    }
  }
)

/** isGenMenu 与上级菜单相斥：选「否」(0) 时清空上级菜单 */
watch(
  () => formState.value.isGenMenu,
  (next) => {
    if (next === 0) formState.value.parentMenuId = undefined
  }
)

/**
 * 模块名称 → 命名空间后缀（帕斯卡，多段用点连接）
 * @param val 模块名输入
 * @returns {string} 后缀段
 */
function toNamespaceSuffix(val: string | null | undefined): string {
  if (val == null || String(val).trim() === '') return ''
  const raw = String(val).trim()
  const parts = raw.split(/[.\s_-]+/).filter(Boolean)
  return parts
    .map(p => (p.length > 0 ? p.charAt(0).toUpperCase() + p.slice(1).toLowerCase() : p))
    .join('.')
}

/**
 * 表名 → 业务名（PascalCase，去掉首段 takt）
 * @param tableName 物理表名
 * @returns {string} 业务名
 */
function tableNameToBusinessName(tableName: string | undefined): string {
  if (tableName == null || String(tableName).trim() === '') return ''
  let parts = String(tableName).trim().split('_').filter(Boolean)
  if (parts.length > 1 && parts[0]?.toLowerCase() === 'takt') parts = parts.slice(1)
  return parts.map(p => (p.length > 0 ? p.charAt(0).toUpperCase() + p.slice(1).toLowerCase() : p)).join('')
}

/**
 * 模块名 → 权限码段（小写冒号连接）
 * @param genModuleName 模块名
 * @returns {string} 权限段
 */
function moduleNameToPermsSegment(genModuleName: string | null | undefined): string {
  if (genModuleName == null || String(genModuleName).trim() === '') return ''
  return String(genModuleName)
    .trim()
    .split('.')
    .flatMap(seg =>
      seg
        .trim()
        .split('_')
        .map(s => s.trim().toLowerCase())
        .filter(s => s.length > 0)
    )
    .join(':')
}

/**
 * 业务名 → 权限码段
 * @param genBusinessName 业务名 PascalCase
 * @returns {string} 权限段
 */
function businessNameToPermsSegment(genBusinessName: string | null | undefined): string {
  if (genBusinessName == null || String(genBusinessName).trim() === '') return ''
  const s = String(genBusinessName).trim()
  if (s.length === 0) return ''
  const parts: string[] = []
  let current = ''
  for (let i = 0; i < s.length; i++) {
    const c = s.charAt(i)
    if (c >= 'A' && c <= 'Z' && current.length > 0) {
      parts.push(current.toLowerCase())
      current = c
    } else {
      current += c
    }
  }
  if (current.length > 0) parts.push(current.toLowerCase())
  return parts.join(':')
}

/**
 * 根据模块名与业务名写入 permsPrefix
 */
function applyPermsPrefix() {
  const modulePart = moduleNameToPermsSegment(formState.value.genModuleName)
  const businessPart = businessNameToPermsSegment(formState.value.genBusinessName)
  const parts = [modulePart, businessPart].filter(Boolean)
  formState.value.permsPrefix = parts.length > 0 ? parts.join(':') : undefined
}

/** DDD 命名空间中间段映射 */
const DDD_NAMESPACE_SEGMENTS: Record<string, string> = {
  entityNamespace: 'Domain.Entities',
  dtoNamespace: 'Application.Dtos',
  serviceNamespace: 'Application.Services',
  controllerNamespace: 'WebApi.Controllers',
  repositoryNamespace: 'Infrastructure.Repositories',
  repositoryInterfaceNamespace: 'Domain.Repositories'
}

/**
 * 按命名前缀 + DDD 段 + 模块后缀更新各层命名空间
 */
function applyNamespacesFromPrefixAndModule() {
  const prefix = (formState.value.namePrefix ?? '').trim() || 'Takt'
  const raw = (formState.value.genModuleName ?? '').trim()
  const moduleSuffix = /^\d+$/.test(raw) ? '' : toNamespaceSuffix(formState.value.genModuleName)
  const suffixPart = moduleSuffix ? `.${moduleSuffix}` : ''

  formState.value.entityNamespace = `${prefix}.${DDD_NAMESPACE_SEGMENTS.entityNamespace}${suffixPart}`
  formState.value.dtoNamespace = `${prefix}.${DDD_NAMESPACE_SEGMENTS.dtoNamespace}${suffixPart}`
  formState.value.serviceNamespace = `${prefix}.${DDD_NAMESPACE_SEGMENTS.serviceNamespace}${suffixPart}`
  formState.value.controllerNamespace = `${prefix}.${DDD_NAMESPACE_SEGMENTS.controllerNamespace}${suffixPart}`
  formState.value.repositoryNamespace = `${prefix}.${DDD_NAMESPACE_SEGMENTS.repositoryNamespace}${suffixPart}`
  formState.value.repositoryInterfaceNamespace = `${prefix}.${DDD_NAMESPACE_SEGMENTS.repositoryInterfaceNamespace}${suffixPart}`
}

/** 模块名称（后缀）变更时，重新驱动命名空间与权限前缀 */
watch(
  () => formState.value.genModuleName,
  () => {
    applyNamespacesFromPrefixAndModule()
    applyPermsPrefix()
  }
)

/** 命名前缀变更时，按 前缀+DDD类型+后缀 重新驱动命名空间 */
watch(
  () => formState.value.namePrefix,
  () => applyNamespacesFromPrefixAndModule()
)

/** 数据表名变更时，仅「新建」时根据表名生成业务名（编辑时保留后端返回的 genBusinessName，避免被覆盖）；并刷新权限前缀 */
watch(
  () => formState.value.tableName,
  (tableName) => {
    const tableId = readGenTablePrimaryId(formState.value)
    if (!tableId) {
      formState.value.genBusinessName = tableName ? tableNameToBusinessName(tableName) : undefined
    }
    applyPermsPrefix()
  }
)

/** 业务名称变更时，同步更新所有类名与权限前缀 */
watch(
  () => formState.value.genBusinessName,
  (businessName) => {
    const base = businessName != null ? String(businessName).trim() : ''
    if (base !== '') {
      const entityName = `Takt${base}`
      formState.value.entityClassName = entityName
      formState.value.dtoClassName = `${entityName}Dto`
      formState.value.iServiceClassName = `I${entityName}Service`
      formState.value.serviceClassName = `${entityName}Service`
      formState.value.controllerClassName = `${entityName}Controller`
      formState.value.iRepositoryClassName = `I${entityName}Repository`
      formState.value.repositoryClassName = `${entityName}Repository`
    }
    applyPermsPrefix()
  }
)

/** 生成方式与生成路径：zip(0) 占位 "/"，自定义路径(1) 默认 solution */
watch(
  () => formState.value.genMethod,
  (next) => {
    if (next === 0) formState.value.genPath = '/'
    else if (next === 1 && (!formState.value.genPath?.trim() || formState.value.genPath === '/')) {
      formState.value.genPath = 'solution'
    }
  }
)

/** 是否使用 Tabs 与 Tabs 字段数相斥：选「否」(1) 时清空 Tabs 字段数 */
watch(
  () => formState.value.isUseTabs,
  (next) => {
    if (next === 0) formState.value.tabsFieldCount = undefined
  }
)

/** 功能名由表描述驱动，与表描述一致（只读） */
watch(
  () => formState.value.tableComment,
  (tableComment) => {
    formState.value.genFunctionName = tableComment != null ? String(tableComment).trim() || undefined : undefined
  }
)

/** 同步父级 formData：编辑回填 / 新增重置 / 拉取列 */
watch(
  () => props.formData,
  (val) => {
    if (val) {
      const tableId = readGenTablePrimaryId(val)
      formState.value = { ...defaultFormState(), ...(val as Record<string, unknown>) } as GenFormState
      if (tableId) formState.value.genTableId = String(tableId)
      /** 展开赋值时若显式带上 tabsFieldCount: undefined 会盖掉默认值；与 defaultFormState（10）及「使用 Tabs」展示逻辑一致 */
      if (Number(formState.value.isUseTabs) === 1 && formState.value.tabsFieldCount == null) {
        formState.value.tabsFieldCount = 10
      }
      formState.value.genFunctionName = formState.value.tableComment != null ? String(formState.value.tableComment).trim() || undefined : undefined
      if (!formState.value.genAuthor) {
        formState.value.genAuthor = readCurrentUserDisplayName()
      }
      if (Number(formState.value.genMethod) === 0 && !formState.value.genPath?.trim()) {
        formState.value.genPath = '/'
      }
      if (!formState.value.sortType?.trim()) {
        formState.value.sortType = 'asc'
      }
      applyNamespacesFromPrefixAndModule()
      applyPermsPrefix()
      const infos = props.databaseInfoList ?? []
      const valTenantCode = parseSelectToOptionalString((val as Partial<GenTable> & { tenantCode?: string }).tenantCode)
      if (valTenantCode) {
        emit('config-change', valTenantCode)
      } else if (val.dataSource && infos.length > 0) {
        const parts = (val.dataSource as string).split(':')
        const tenantFromSource = parts.length > 1 ? parts[parts.length - 1] : ''
        const matched = infos.find(
          c => c.tenantCode === tenantFromSource || val.dataSource === `${c.displayName}:${c.tenantCode}`
        )
        if (matched) {
          formState.value.tenantCode = matched.tenantCode
          emit('config-change', matched.tenantCode)
        }
      }

      if (val.columns != null && Array.isArray(val.columns) && val.columns.length > 0) {
        const mapped = (val.columns as GenTableColumn[]).map(mapGenTableColumnToRow)
        columnList.value = normalizeColumnOrderNum(
          tableId
            ? mapped
            : mapped.map(row => ({
              ...row,
              columnId: undefined,
              genTableId: undefined,
              tableId: undefined,
            }))
        )
        syncColumnListBaseline(columnList.value)
      } else if (tableId) {
        loadColumns(String(tableId))
      } else {
        columnList.value = []
        columnListBaseline.value = []
      }
    } else {
      formState.value = defaultFormState()
      formState.value.genPath = '/'
      formState.value.genAuthor = readCurrentUserDisplayName()
      const genOpts = getDictSelectOptions('code_generator_function')
      const btnOpts = getDictSelectOptions('code_generator_button_category')
      if (genOpts.length > 0) formState.value.genFunction = genOpts.map((o: TaktDictSelectOption) => String(o.value)).join(',')
      if (btnOpts.length > 0) formState.value.menuButtonGroup = btnOpts.map((o: TaktDictSelectOption) => String(o.value)).join(',')
      genFunctionCheckAll.value = true
      genFunctionIndeterminate.value = false
      menuButtonGroupCheckAll.value = true
      menuButtonGroupIndeterminate.value = false
      columnList.value = []
      columnListBaseline.value = []
      selecmedColumnRowKeys.value = []
    }
  },
  { immediate: true }
)

/**
 * 校验字段列表列名 snake_case、C# 名 PascalCase
 * @throws {Error} 命名不合法时抛出带行号的错误
 */
function validateColumnListNaming(): void {
  const list = columnList.value
  for (let i = 0; i < list.length; i++) {
    const row = list[i]
    if (!row) continue
    const rowNum = i + 1
    const colName = row.databaseColumnName
    const csharpName = row.csharpColumnName
    if (colName != null && String(colName).trim() !== '' && !isSnakeCase(String(colName))) {
      throw new Error(t(`${FORM}.validation.columnsnake`, { row: rowNum, value: colName }))
    }
    if (csharpName != null && String(csharpName).trim() !== '' && !isPascalCase(String(csharpName))) {
      throw new Error(t(`${FORM}.validation.columnpascal`, { row: rowNum, value: csharpName }))
    }
  }
}

/**
 * 校验主表表单与字段命名规则
 * @returns {Promise<void>}
 */
async function doValidate() {
  await formRef.value?.validate()
  validateColumnListNaming()
}

/**
 * 解析默认排序字段（主键列 → 可排序列 → 首列 → id）
 * @param rows 字段配置行
 * @returns {string} 数据库列名 snake_case
 */
function resolveDefaultSortField(rows: readonly GenTableColumnRow[]): string {
  const pk = rows.find((r) => Number(r.isPk) === 1)
  const pkName = pk?.databaseColumnName?.trim()
  if (pkName) return pkName
  const sortCol = rows.find((r) => Number(r.isSort) === 1)
  const sortName = sortCol?.databaseColumnName?.trim()
  if (sortName) return sortName
  const first = rows.find((r) => r.databaseColumnName?.trim())
  return first?.databaseColumnName?.trim() || 'id'
}

/**
 * 将字段配置行映射为 API 列 DTO（columnId/orderNum 为 UI 别名）
 * @param col 表单行
 * @param isTableUpdate 是否更新主表
 * @param tableId 主表 id
 * @returns {Record<string, unknown>} 提交列 DTO
 */
function mapColumnRowForSubmit(
  col: GenTableColumnRow,
  isTableUpdate: boolean,
  tableId?: string,
  tenantCode?: string,
): Record<string, unknown> {
  const rawColId = col.columnId
  const id = rawColId != null ? String(rawColId) : ''
  const n = Number(id)
  const hasPersistedColumnId = id !== '' && id !== '0' && Number.isFinite(n) && n > 0
  const genTableColumnId = isTableUpdate ? (hasPersistedColumnId ? id : '0') : '0'
  const lineNumber = col.orderNum ?? (col as { lineNumber?: number }).lineNumber ?? 0
  const genTableId = tableId ?? (col.genTableId != null ? String(col.genTableId) : '0')
  return {
    tenantCode: col.tenantCode != null ? String(col.tenantCode) : tenantCode,
    genTableId,
    genTableColumnId,
    lineNumber,
    databaseColumnName: col.databaseColumnName?.trim() ?? '',
    columnComment: col.columnComment ?? '',
    databaseDataType: col.databaseDataType?.trim() ?? '',
    csharpDataType: col.csharpDataType?.trim() ?? '',
    csharpColumnName: col.csharpColumnName?.trim() ?? '',
    length: col.length ?? 0,
    decimalDigits: col.decimalDigits ?? 0,
    isPk: col.isPk ?? 0,
    isIncrement: col.isIncrement ?? 0,
    isRequired: col.isRequired ?? 0,
    isCreate: col.isCreate ?? 0,
    isUpdate: col.isUpdate ?? 0,
    isUnique: col.isUnique ?? 0,
    isList: col.isList ?? 0,
    isExport: col.isExport ?? 0,
    isSort: col.isSort ?? 0,
    isQuery: col.isQuery ?? 0,
    queryType: col.isQuery === 1
      ? (col.queryType?.trim() || (isStringLikeCsharpType(col.csharpDataType) ? 'like' : 'eq'))
      : '',
    htmlType: col.htmlType?.trim() || 'input',
    dictType: col.dictType ?? '',
    extField: col.extField,
    remark: col.remark,
  }
}

/**
 * 汇总提交数据（含 columns 与 genTableColumnId/lineNumber 映射）
 * @returns {GenFormState} 可提交 DTO
 */
function getValues(): GenFormState {
  const rows = normalizeColumnOrderNum(columnList.value)
  const isTableUpdate = !!formState.value.genTableId
  const tableId = isTableUpdate ? String(formState.value.genTableId) : undefined
  const tenantCode = formState.value.tenantCode?.trim() || undefined
  const columns = rows.map((col) => mapColumnRowForSubmit(col, isTableUpdate, tableId, tenantCode)) as GenTableColumnRow[]
  const raw = {
    ...formState.value,
    columns,
    genTableId: isTableUpdate ? tableId : undefined,
  } as GenFormState
  const gm = Number(raw.genMethod ?? 0)
  raw.genMethod = Number.isFinite(gm) ? gm : 0
  /** zip 模式须传 genPath，避免 body 缺字段导致后端 GenPath Required 校验 400 */
  if (raw.genMethod === 0) {
    raw.genPath = raw.genPath?.trim() || '/'
  } else {
    raw.genPath = raw.genPath?.trim() ?? ''
  }
  if (!raw.sortField?.trim()) {
    raw.sortField = resolveDefaultSortField(rows)
  }
  if (!raw.sortType?.trim()) {
    raw.sortType = 'asc'
  }
  if (!raw.genAuthor?.trim()) {
    raw.genAuthor = readCurrentUserDisplayName()
  }
  if (!raw.tenantCode?.trim()) {
    raw.tenantCode = tenantCode
  }
  /** 上级菜单 id 按字符串保留，避免前端 Number 强转导致 int64 精度丢失；无上级菜单时传 "0" */
  if (raw.parentMenuId != null && String(raw.parentMenuId).trim() !== '') {
    raw.parentMenuId = String(raw.parentMenuId)
  } else {
    raw.parentMenuId = '0'
  }
  return raw
}

/**
 * 重置表单与字段列表（取消/提交后由父组件调用）
 */
function reset() {
  formState.value = defaultFormState()
  formState.value.genPath = '/'
  formState.value.genAuthor = readCurrentUserDisplayName()
  const genOpts = getDictSelectOptions('code_generator_function')
  const btnOpts = getDictSelectOptions('code_generator_button_category')
  if (genOpts.length > 0) formState.value.genFunction = genOpts.map((o: TaktDictSelectOption) => String(o.value)).join(',')
  if (btnOpts.length > 0) formState.value.menuButtonGroup = btnOpts.map((o: TaktDictSelectOption) => String(o.value)).join(',')
  columnList.value = []
  columnListBaseline.value = []
  selecmedColumnRowKeys.value = []
  columnTableVisibleKeys.value = resolveColumnTableDefaultVisibleKeys()
  columnTableWidths.value = { ...COLUMN_TABLE_DEFAULT_WIDTHS }
  activeTab.value = 'table'
  tableSubTab.value = 'basic'
  columnDragRowIndex.value = null
  genFunctionCheckAll.value = true
  genFunctionIndeterminate.value = false
  menuButtonGroupCheckAll.value = true
  menuButtonGroupIndeterminate.value = false
  formRef.value?.clearValidate()
}

/** 父组件调用：validate / getValues / reset */
defineExpose({ validate: doValidate, getValues, reset })
</script>

<style scoped lang="css">
.gen-form-root {
  overflow-x: hidden;
}
:deep(.ant-tabs-card .ant-tabs-content) {
  padding-top: 12px;
}
/* 字段配置表格：scroll.y 固定高度 + 横向滚动在 a-table 内（对齐 takt-single-table） */
.column-table-wrap {
  width: 100%;
  min-width: 0;
  overflow: hidden;
}
.column-table-wrap :deep(.ant-table-wrapper) {
  width: 100%;
  min-width: 0;
}
.column-table-wrap :deep(.ant-table-container) {
  min-width: 0;
}
/* 表格 size=small 控制行密度；行内控件仍为 middle，通过收紧单元格 padding 避免撑高行 */
.column-table-wrap :deep(.ant-table-small .ant-table-thead > tr > th),
.column-table-wrap :deep(.ant-table-small .ant-table-tbody > tr > td) {
  padding-block: 4px;
  vertical-align: middle;
}
.column-table-wrap :deep(.column-cell-input .ant-input-affix-wrapper),
.column-table-wrap :deep(.column-cell-input.ant-input-number),
.column-table-wrap :deep(.column-cell-select .ant-select-selector) {
  margin-block: 0;
}
.column-table-wrap--fixed-y :deep(.ant-table-body) {
  min-height: var(--takt-table-scroll-y);
  max-height: var(--takt-table-scroll-y);
  overflow-y: auto !important;
}
.column-table-wrap--fixed-y :deep(.ant-table-placeholder) {
  min-height: calc(var(--takt-table-scroll-y) - 8px);
}
.column-cell-input,
.column-cell-select {
  min-width: 0;
}
.column-cell-muted {
  color: rgba(0, 0, 0, 0.25);
}
/* 拖拽把手列 */
.column-drag-cell {
  padding: 4px 8px !important;
}
.column-drag-handle {
  cursor: move;
  color: rgba(0, 0, 0, 0.35);
  display: inline-flex;
  padding: 2px;
}
.column-row-dragging {
  opacity: 0.6;
  background: var(--ant-color-primary-bg, #e6f4ff);
}
.gen-column-setting-group {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.gen-column-setting-item {
  padding: 8px 0;
  border-bottom: 1px solid var(--ant-color-border-secondary);
}
.gen-column-setting-item:last-child {
  border-bottom: none;
}
</style>
