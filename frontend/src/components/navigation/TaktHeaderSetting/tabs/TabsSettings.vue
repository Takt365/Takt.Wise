<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderSetting/tabs
文件名称:TabsSettings.vue
功能描述:标签页偏好（显示/风格/持久化/上限/多页签）；直接写 Pinia，避免 inject 副本与 @change 时序导致不生效

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div class="tabs-settings">
    <a-space
      direction="vertical"
      size="large"
      style="width: 100%"
    >
      <a-form-item :label="$t('components.navigation.page.systemsetting.showtabs')">
        <a-switch
          :checked="setting.showTabs"
          @update:checked="onShowTabs"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.tabstyle')">
        <a-radio-group
          :value="setting.tabStyle"
          @update:value="onTabStyle"
        >
          <a-radio-button value="card">
            {{ $t('components.navigation.page.systemsetting.card') }}
          </a-radio-button>
          <a-radio-button value="google">
            {{ $t('components.navigation.page.systemsetting.google') }}
          </a-radio-button>
        </a-radio-group>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.persisttabs')">
        <a-switch
          :checked="setting.persistTabs"
          @update:checked="onPersistTabs"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.maxtabs')">
        <a-input-number
          :value="setting.maxTabs"
          :min="5"
          :max="50"
          @update:value="onMaxTabs"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.enablemultitab')">
        <a-switch
          :checked="setting.multiTab"
          @update:checked="onMultiTab"
        />
      </a-form-item>
    </a-space>
  </div>
</template>

<script setup lang="ts">
import type { TabStyle } from '@/types/setting.types'
import type { AppSetting } from '@/stores/common/useSettingStore'
import { useSettingStore } from '@/stores/common/useSettingStore'
import { applySettings, notifySettingsChanged } from '@/utils/applySettings'

const settingStore = useSettingStore()
const { setting } = storeToRefs(settingStore)
/** 抽屉快照（若存在则同步，防止其它 tab 提交覆盖） */
const draftSetting = inject<AppSetting | null>('setting', null)

/**
 * 写入偏好并应用到布局 / DOM
 * @param partial 局部字段
 */
function commit(partial: Partial<AppSetting>): void {
  settingStore.patchSetting(partial)
  if (draftSetting) {
    Object.assign(draftSetting, partial)
  }
  applySettings()
  notifySettingsChanged()
}

/**
 * 显示标签栏
 * @param checked 是否显示
 */
function onShowTabs(checked: boolean): void {
  commit({ showTabs: !!checked })
}

/**
 * 标签风格
 * @param value card | google
 */
function onTabStyle(value: TabStyle | string): void {
  const tabStyle: TabStyle = value === 'card' ? 'card' : 'google'
  commit({ tabStyle })
}

/**
 * 持久化标签
 * @param checked 是否持久化
 */
function onPersistTabs(checked: boolean): void {
  commit({ persistTabs: !!checked })
}

/**
 * 最大标签数
 * @param value 数量
 */
function onMaxTabs(value: number | string | null): void {
  const n = typeof value === 'number' ? value : Number(value)
  if (!Number.isFinite(n)) {
    return
  }
  commit({ maxTabs: Math.min(50, Math.max(5, Math.trunc(n))) })
}

/**
 * 启用多页签（关闭后导航仅保留首页+当前页）
 * @param checked 是否多页签
 */
function onMultiTab(checked: boolean): void {
  commit({ multiTab: !!checked })
}
</script>
