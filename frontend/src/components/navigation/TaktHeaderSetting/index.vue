<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderSetting
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:系统设置抽屉组件,提供布局、主题、导航等设置项;引用键 components.navigation.page.systemsetting.* 与 common.page.button.*

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-button
    type="text"
    class="takt-header-setting"
    :title="$t('components.navigation.page.systemsetting.title')"
    @click="handleClick"
  >
    <template #icon>
      <RiTShirtLine class="takt-remix-icon" />
    </template>
  </a-button>
  <a-drawer
    v-model:open="visible"
    :title="$t('components.navigation.page.systemsetting.systemtitle')"
    placement="right"
    :width="600"
    class="takt-header-setting-drawer"
  >
    <template #extra>
      <div class="drawer-extra-actions">
        <a-space>
          <a-button
            class="takt-button takt-button-copy"
            @click="handleCopy"
          >
            <template #icon>
              <RiFileCopyLine class="takt-remix-icon" />
            </template>
            {{ $t('common.page.button.copy') }} {{ $t('common.page.button.preferences') }} {{ $t('common.page.button.config') }}
          </a-button>
          <a-button
            class="takt-button takt-button-reset"
            @click="handleReset"
          >
            <template #icon>
              <RiRefreshLine class="takt-remix-icon" />
            </template>
            {{ $t('common.page.button.reset') }}
          </a-button>
        </a-space>
      </div>
    </template>
    <div class="takt-header-setting-drawer__body">
      <a-tabs v-model:active-key="activeTab">
        <a-tab-pane
          key="layout"
          :tab="$t('components.navigation.page.systemsetting.layout')"
        >
          <LayoutSettings @change="handleSettingChange" />
        </a-tab-pane>
        <a-tab-pane
          key="theme"
          :tab="$t('components.navigation.page.systemsetting.theme')"
        >
          <ThemeSettings @change="handleSettingChange" />
        </a-tab-pane>
        <a-tab-pane
          key="navigation"
          :tab="$t('components.navigation.page.systemsetting.navigation')"
        >
          <NavigationSettings />
        </a-tab-pane>
        <a-tab-pane
          key="tabs"
          :tab="$t('components.navigation.page.systemsetting.tabs')"
        >
          <TabsSettings />
        </a-tab-pane>
        <a-tab-pane
          key="other"
          :tab="$t('components.navigation.page.systemsetting.other')"
        >
          <OtherSettings />
        </a-tab-pane>
      </a-tabs>
    </div>
  </a-drawer>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { RiTShirtLine, RiFileCopyLine, RiRefreshLine } from '@remixicon/vue'
import { getSetting, defaultSetting, useSettingStore, type AppSetting } from '@/stores/common/useSettingStore'
import { applySettings, notifySettingsChanged } from '@/utils/applySettings'
import { createLogger } from '@/utils/logger'

const headerSettingLogger = createLogger('takt-header-setting')

const { t } = useI18n()

interface Props {
  /** 组件类型（用于兼容性，实际不使用） */
  type?: 'icon' | 'button' | 'dropdown'
}

// 定义 props 以接受 type 属性，避免 Vue 警告
defineProps<Props>()

const visible = ref(false)
const activeTab = ref('layout')
const settingStore = useSettingStore()

/** 抽屉内编辑用快照（布局/主题等 tab 仍用 inject）；标签页 tab 直接写 Pinia */
const currentSetting = reactive<AppSetting>({ ...getSetting() })

provide('setting', currentSetting)

/**
 * 打开抽屉时从 Pinia / 存储同步快照
 */
const handleClick = () => {
  visible.value = true
  Object.assign(currentSetting, settingStore.setting)
}

/**
 * 将抽屉快照提交到 Pinia（须 nextTick，保证 v-model 已写入）
 * 标签页字段以 Store 为准，避免 TabsSettings 直写后被快照旧值覆盖
 */
const handleSettingChange = async () => {
  await nextTick()
  const latest = settingStore.setting
  Object.assign(currentSetting, {
    showTabs: latest.showTabs,
    tabStyle: latest.tabStyle,
    persistTabs: latest.persistTabs,
    maxTabs: latest.maxTabs,
    multiTab: latest.multiTab,
    menuAccordion: latest.menuAccordion,
    menuStyle: latest.menuStyle,
    showBreadcrumb: latest.showBreadcrumb,
    breadcrumbIcon: latest.breadcrumbIcon,
    borderRadius: latest.borderRadius,
    showFooter: latest.showFooter,
    copyright: latest.copyright,
    watermark: latest.watermark,
    watermarkContent: latest.watermarkContent,
    demo: latest.demo,
    showForgotPassword: latest.showForgotPassword,
    showRegister: latest.showRegister,
  })
  const raw = toRaw(currentSetting)
  settingStore.setSetting({
    ...raw,
    themeColor: { ...raw.themeColor },
  })
  Object.assign(currentSetting, settingStore.setting)
  applySettings()
  notifySettingsChanged()
}

/**
 * 复制偏好设置到剪贴板
 */
const handleCopy = async () => {
  try {
    Object.assign(currentSetting, settingStore.setting)
    const settingJson = JSON.stringify(toRaw(currentSetting), null, 2)
    await navigator.clipboard.writeText(settingJson)
    message.success(t('common.feedback.copied.clipboard'))
  } catch (error) {
    headerSettingLogger.error('复制偏好设置失败', { action: 'copy' }, error)
    message.error(t('common.feedback.copy.manual'))
  }
}

/**
 * 重置设置为默认值
 */
const handleReset = () => {
  Object.assign(currentSetting, defaultSetting)
  settingStore.resetSetting()
  applySettings()
  notifySettingsChanged()
  message.success(t('common.feedback.action.success', { action: t('common.page.button.reset') }))
}
</script>

<style scoped>
.takt-header-setting-drawer {
  :deep(.ant-drawer-content-wrapper) {
    right: 16px;
  }
}

.drawer-extra-actions {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
}
</style>

<!-- 非 scoped：Drawer 可能渲染到 body，内容区内边距与表单项样式统一在此设置，避免与各 tab 的 padding 冲突 -->
<style>
/* 本抽屉的 body 不保留组件库默认 padding，仅由下方 __body 提供 4px 8px，避免叠加 */
.ant-drawer-body:has(.takt-header-setting-drawer__body) {
  padding: 0;
}

.takt-header-setting-drawer__body {
  padding: 4px 8px; /* 上下 4px，左右 8px */
}

/* 各 tab 内 a-space 表单项间隔统一为 16px */
.takt-header-setting-drawer__body .ant-space-vertical {
  row-gap: 16px !important;
}
.takt-header-setting-drawer__body .ant-space-vertical .ant-space-item {
  margin-bottom: 0 !important;
}

.takt-header-setting-drawer__body .ant-form-item {
  width: 100%;
  margin-bottom: 0;

  .ant-form-item-row {
    display: flex;
    flex-wrap: nowrap;
    width: 100%;
    align-items: flex-start;
    gap: 8px;
  }

  .ant-form-item-label {
    flex: 0 0 30%;
    max-width: 30%;
    min-width: 0;
    text-align: left;
    padding-right: 0;

    > label {
      justify-content: flex-start;
    }
  }

  .ant-form-item-control {
    flex: 1 1 70%;
    min-width: 0;
    display: flex;
    justify-content: flex-start;

    .ant-form-item-control-input {
      flex: 1;
      min-width: 0;
      max-width: 100%;
    }

    .ant-form-item-control-input-content {
      display: flex;
      flex-wrap: wrap;
      justify-content: flex-start;
      align-items: center;
      width: 100%;
      min-width: 0;
    }

    /* switch、radio-group、input-number 控件区右对齐 */
    .ant-form-item-control-input-content:has(.ant-switch),
    .ant-form-item-control-input-content:has(.ant-radio-group),
    .ant-form-item-control-input-content:has(.ant-input-number) {
      justify-content: flex-end;
    }

    /* slider 占满控件区可用宽度，轨道正常显示 */
    .ant-form-item-control-input-content .ant-slider {
      flex: 1;
      min-width: 100px;
      max-width: 100%;
      margin-inline: 0 8px;
    }
    .ant-form-item-control-input-content .ant-slider .ant-slider-rail {
      width: 100%;
    }
  }
}
</style>
