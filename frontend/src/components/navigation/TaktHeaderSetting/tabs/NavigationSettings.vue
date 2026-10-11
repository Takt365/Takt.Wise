<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderSetting/tabs
文件名称:NavigationSettings.vue
功能描述:导航偏好（手风琴/菜单风格/面包屑）；@change 取新值直写 Pinia

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div class="navigation-settings">
    <a-space
      direction="vertical"
      size="large"
      style="width: 100%"
    >
      <a-form-item :label="$t('components.navigation.page.systemsetting.menuaccordion')">
        <a-switch
          :checked="setting.menuAccordion"
          @change="onMenuAccordion"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.menustyle')">
        <a-radio-group
          :value="setting.menuStyle"
          @change="onMenuStyleChange"
        >
          <a-radio-button value="plain">
            {{ $t('components.navigation.page.systemsetting.plain') }}
          </a-radio-button>
          <a-radio-button value="rounded">
            {{ $t('components.navigation.page.systemsetting.rounded') }}
          </a-radio-button>
        </a-radio-group>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.showbreadcrumb')">
        <a-switch
          :checked="setting.showBreadcrumb"
          @change="onShowBreadcrumb"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.showbreadcrumbicon')">
        <a-switch
          :checked="setting.breadcrumbIcon"
          @change="onBreadcrumbIcon"
        />
      </a-form-item>
    </a-space>
  </div>
</template>

<script setup lang="ts">
import type { RadioChangeEvent } from 'ant-design-vue/es/radio'
import type { AppSetting, MenuStyle } from '@/types/setting.types'
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
 * 菜单手风琴（Switch change 首参即为新值）
 * @param checked 是否开启
 */
function onMenuAccordion(checked: boolean | string | number): void {
  commit({ menuAccordion: !!checked })
}

/**
 * 菜单风格
 * @param e Radio change 事件
 */
function onMenuStyleChange(e: RadioChangeEvent): void {
  const raw = e?.target?.value
  const menuStyle: MenuStyle = raw === 'rounded' ? 'rounded' : 'plain'
  commit({ menuStyle })
}

/**
 * 显示面包屑
 * @param checked 是否显示
 */
function onShowBreadcrumb(checked: boolean | string | number): void {
  commit({ showBreadcrumb: !!checked })
}

/**
 * 面包屑显示图标
 * @param checked 是否显示图标
 */
function onBreadcrumbIcon(checked: boolean | string | number): void {
  commit({ breadcrumbIcon: !!checked })
}
</script>
