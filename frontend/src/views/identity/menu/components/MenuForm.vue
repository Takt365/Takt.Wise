<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/menu/components -->
<!-- 文件名称：MenuForm.vue -->
<!-- 功能描述：菜单维护弹窗内嵌表单（树表 ParentId）。Tab：基本信息 / 路由与权限 / 显示与外链；一行一列；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs v-model:active-key="activeTab">
      <!-- 基本信息 -->
      <a-tab-pane
        key="basic"
        :tab="t('identity.menu.page.tabs.basic')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.name')"
                name="menuName"
              >
                <a-input
                  v-model:value="formState.menuName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.name') })"
                  show-count
                  :maxlength="50"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.code')"
                name="menuCode"
              >
                <a-input
                  v-model:value="formState.menuCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.code') })"
                  show-count
                  :maxlength="200"
                  :disabled="!!formData?.menuId"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.parentid')"
                name="parentId"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="TaktMenus/tree-options"
                  :lazy="true"
                  :placeholder="t('identity.menu.page.placeholder.parentmenuhint')"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.type')"
                name="menuType"
              >
                <TaktSelect
                  v-model:value="formState.menuType"
                  dict-type="sys_menu_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.type') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.status')"
                name="menuStatus"
              >
                <TaktSelect
                  v-model:value="formState.menuStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.status') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.icon')"
                name="icon"
              >
                <TaktIconPicker
                  v-model="formState.icon"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.icon') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.l10nkey')"
                name="i18nKey"
              >
                <a-input
                  v-model:value="formState.i18nKey"
                  :placeholder="t('identity.menu.page.placeholder.l10nhint')"
                  show-count
                  :maxlength="140"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.description') })"
                  :rows="2"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>

      <!-- 路由与权限 -->
      <a-tab-pane
        key="route"
        :tab="t('identity.menu.page.tabs.route')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.path')"
                name="menuPath"
              >
                <a-input
                  v-model:value="formState.menuPath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.path') })"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.routepath')"
                name="routePath"
              >
                <a-input
                  v-model:value="formState.routePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.routepath') })"
                  show-count
                  :maxlength="200"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.component')"
                name="componentPath"
              >
                <a-input
                  v-model:value="formState.componentPath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.component') })"
                  show-count
                  :maxlength="200"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.permission')"
                name="permission"
              >
                <a-input
                  v-model:value="formState.permission"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.permission') })"
                  show-count
                  :maxlength="100"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>

      <!-- 显示与外链 -->
      <a-tab-pane
        key="display"
        :tab="t('identity.menu.page.tabs.display')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.isvisible')"
                name="isVisible"
              >
                <TaktSelect
                  v-model:value="formState.isVisible"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.isvisible') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.iscached')"
                name="isCached"
              >
                <TaktSelect
                  v-model:value="formState.isCached"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.iscached') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.isexternal')"
                name="isExternal"
              >
                <TaktSelect
                  v-model:value="formState.isExternal"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.isexternal') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.linkurl')"
                name="externalUrl"
              >
                <a-input
                  v-model:value="formState.externalUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.linkurl') })"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { Menu, MenuCreate } from '@/types/identity/menu.types'

const { t } = useI18n()

interface Props {
  /** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
  formData?: Partial<Menu>
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

/** 表单实例 */
const formRef = ref()
/** 当前 Tab */
const activeTab = ref('basic')
/** 表单内容区高度类（单 Tab 字段数未达 30） */
const formContentClass = computed(() => 'takt-form-content-rows-5')

/** 表单状态（字段名与 Menu / MenuCreate 一致；parentId 兼容树选择组件 string | number） */
interface FormState extends Omit<MenuCreate, 'parentId' | 'remark'> {
  parentId: string | number
}

/**
 * 创建空表单状态
 * @returns {FormState} 默认值
 */
function createEmptyFormState(): FormState {
  return {
    menuName: '',
    menuCode: '',
    i18nKey: '',
    parentId: '0',
    menuPath: '',
    routePath: '',
    componentPath: '',
    icon: '',
    menuType: 0,
    menuStatus: 1,
    permission: '',
    isVisible: 1,
    isCached: 1,
    isExternal: 0,
    externalUrl: '',
    isBuiltIn: 0,
    description: ''
  }
}

/** 表单字段状态 */
const formState = reactive<FormState>(createEmptyFormState())

/** 校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  menuName: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.menu.name') }), trigger: 'blur' }],
  menuCode: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.menu.code') }), trigger: 'blur' }],
  menuType: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.menu.type') }), trigger: 'change' }]
}))

watch(() => props.formData, (newData) => {
  if (newData && Object.keys(newData).length > 0) {
    Object.assign(formState, {
      menuName: newData.menuName ?? '',
      menuCode: newData.menuCode ?? '',
      i18nKey: newData.i18nKey ?? '',
      parentId: newData.parentId ?? '0',
      menuPath: newData.menuPath ?? '',
      routePath: newData.routePath ?? '',
      componentPath: newData.componentPath ?? '',
      icon: newData.icon ?? '',
      menuType: newData.menuType ?? 0,
      menuStatus: newData.menuStatus ?? 1,
      permission: newData.permission ?? '',
      isVisible: newData.isVisible ?? 1,
      isCached: newData.isCached ?? 1,
      isExternal: newData.isExternal ?? 0,
      externalUrl: newData.externalUrl ?? '',
      isBuiltIn: newData.isBuiltIn ?? 0,
      description: newData.description ?? ''
    })
  } else {
    Object.assign(formState, createEmptyFormState())
  }
  activeTab.value = 'basic'
}, { immediate: true, deep: true })

/**
 * 校验表单（跨 Tab，依赖 force-render）
 * @returns {Promise<void>}
 */
const validate = async () => {
  await formRef.value?.validate()
}

/**
 * 获取提交载荷
 * @returns {MenuCreate} 创建/更新 DTO
 */
const getValues = (): MenuCreate => {
  const p = formState.parentId
  const parentId = p === '' || p === undefined || p === null ? '0' : String(p)
  return {
    menuName: formState.menuName,
    menuCode: formState.menuCode,
    i18nKey: formState.i18nKey,
    parentId,
    menuPath: formState.menuPath,
    menuType: formState.menuType,
    permission: formState.permission,
    routePath: formState.routePath,
    componentPath: formState.componentPath,
    icon: formState.icon,
    isExternal: formState.isExternal,
    externalUrl: formState.externalUrl,
    isCached: formState.isCached,
    isVisible: formState.isVisible,
    menuStatus: formState.menuStatus,
    isBuiltIn: formState.isBuiltIn,
    description: formState.description
  }
}

/**
 * 重置表单
 * @returns {void}
 */
const resetFields = () => {
  formRef.value?.resetFields()
  Object.assign(formState, createEmptyFormState())
  activeTab.value = 'basic'
}

defineExpose({ validate, getValues, resetFields })
</script>
