<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderSetting/tabs
文件名称:OtherSettings.vue
功能描述:其它偏好（页脚/版权/水印/Demo只读/登录入口）；直接写 Pinia
版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div class="other-settings">
    <a-space
      direction="vertical"
      size="large"
      style="width: 100%"
    >
      <a-form-item :label="$t('components.navigation.page.systemsetting.showfooter')">
        <a-switch
          :checked="setting.showFooter"
          @update:checked="onShowFooter"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.copyright')">
        <a-input
          :value="setting.copyright"
          :placeholder="$t('common.page.form.placeholder.copyright')"
          @update:value="onCopyright"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.demoswitch')">
        <a-switch
          :checked="setting.demo"
          @update:checked="onDemo"
        />
        <div class="setting-hint">
          {{ $t('components.navigation.page.systemsetting.demohint') }}
        </div>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.watermark')">
        <a-switch
          :checked="setting.watermark"
          :disabled="setting.demo"
          @update:checked="onWatermark"
        />
      </a-form-item>

      <a-form-item
        v-if="setting.watermark"
        :label="$t('components.navigation.page.systemsetting.watermarkcontent')"
      >
        <a-input
          :value="setting.watermarkContent"
          :placeholder="$t('common.page.form.placeholder.watermark')"
          @update:value="onWatermarkContent"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.showforgotpassword')">
        <a-switch
          :checked="setting.showForgotPassword"
          @update:checked="onShowForgotPassword"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.showregister')">
        <a-switch
          :checked="setting.showRegister"
          @update:checked="onShowRegister"
        />
      </a-form-item>
    </a-space>
  </div>
</template>

<script setup lang="ts">
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
 * 显示页脚
 * @param checked 是否显示
 */
function onShowFooter(checked: boolean): void {
  commit({ showFooter: !!checked })
}

/**
 * 版权文案
 * @param value 文案
 */
function onCopyright(value: string): void {
  commit({ copyright: value ?? '' })
}

/**
 * 启用水印（Demo 开启时不可关闭）
 * @param checked 是否启用
 */
function onWatermark(checked: boolean): void {
  if (setting.value.demo && !checked) {
    return
  }
  commit({ watermark: !!checked })
}

/**
 * 水印内容
 * @param value 文案
 */
function onWatermarkContent(value: string): void {
  commit({ watermarkContent: value ?? '' })
}

/**
 * Demo 只读（开/关均与水印同步；配置落库后全站只读由拦截器生效）
 * @param checked 是否 Demo
 */
function onDemo(checked: boolean): void {
  const demo = !!checked
  commit({ demo, watermark: demo })
}

/**
 * 登录页显示忘记密码
 * @param checked 是否显示
 */
function onShowForgotPassword(checked: boolean): void {
  commit({ showForgotPassword: !!checked })
}

/**
 * 登录页显示注册
 * @param checked 是否显示
 */
function onShowRegister(checked: boolean): void {
  commit({ showRegister: !!checked })
}
</script>

<style scoped>
.setting-hint {
  margin-top: 4px;
  color: var(--ant-color-text-tertiary);
  font-size: 12px;
  line-height: 1.5;
}
</style>
