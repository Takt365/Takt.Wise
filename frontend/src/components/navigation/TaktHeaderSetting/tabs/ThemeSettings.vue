<template>
  <div class="theme-settings">
    <a-space
      direction="vertical"
      class="theme-settings-space"
      style="width: 100%"
    >
      <a-form-item :label="$t('components.navigation.page.systemsetting.thememode')">
        <a-radio-group
          v-model:value="setting.theme"
          @change="handleAppearanceChange"
        >
          <a-radio-button value="light">
            {{ $t('common.page.theme.light') }}
          </a-radio-button>
          <a-radio-button value="dark">
            {{ $t('common.page.theme.dark') }}
          </a-radio-button>
        </a-radio-group>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.themecolor')">
        <div class="theme-color-picker">
          <a-tooltip
            v-for="(color, key) in themeColorMap"
            :key="key"
            :title="$t(`common.page.color.${themeColorI18nKeyMap[key as Exclude<ThemeColor, 'custom'>]}`)"
            placement="top"
          >
            <div
              class="color-item"
              :class="{ active: setting.themeColor.type === key }"
              :style="{ backgroundColor: color }"
              @click="handleColorSelect(key as ThemeColor)"
            >
              <RiCheckLine
                v-if="setting.themeColor.type === key"
                class="color-item-check takt-remix-icon"
              />
            </div>
          </a-tooltip>
          <a-tooltip
            :title="$t('common.page.color.custom')"
            placement="top"
          >
            <div
              class="color-item custom"
              :class="{ active: setting.themeColor.type === 'custom' }"
              @click="handleColorSelect('custom')"
            >
              <RiCheckLine
                v-if="setting.themeColor.type === 'custom'"
                class="color-item-check custom-check takt-remix-icon"
              />
            </div>
          </a-tooltip>
        </div>
        <div
          v-if="setting.themeColor.type === 'custom'"
          style="margin-top: 8px; display: flex; align-items: center; gap: 8px;"
        >
          <input
            type="color"
            :value="customColorValue"
            style="width: 40px; height: 32px; border: 1px solid #d9d9d9; border-radius: 4px; cursor: pointer;"
            @input="handleCustomColorPickerInput"
          >
          <a-input
            v-model:value="customColorValue"
            placeholder="#1890ff"
            style="flex: 1"
          />
        </div>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.borderradius')">
        <a-radio-group
          :value="setting.borderRadius"
          @update:value="onBorderRadius"
        >
          <a-radio-button :value="0">
            0
          </a-radio-button>
          <a-radio-button :value="5">
            5
          </a-radio-button>
          <a-radio-button :value="10">
            10
          </a-radio-button>
          <a-radio-button :value="15">
            15
          </a-radio-button>
          <a-radio-button :value="20">
            20
          </a-radio-button>
        </a-radio-group>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.fontsize')">
        <a-slider
          v-model:value="setting.fontSize"
          :min="15"
          :max="22"
          :step="1"
          @change="handleFontSizeChange"
        />
        <span style="margin-left: 8px">{{ setting.fontSize }}px</span>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.colorweak')">
        <a-switch
          v-model:checked="setting.colorWeak"
          @change="handleChange"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.graymode')">
        <a-switch
          v-model:checked="setting.grayscale"
          @change="handleChange"
        />
      </a-form-item>
    </a-space>
  </div>
</template>

<script setup lang="ts">
import { RiCheckLine } from '@remixicon/vue'
import type { AppSetting, ThemeColor } from '@/stores/common/useSettingStore'
import { themeColorMap, themeColorI18nKeyMap, validateFontSize, useSettingStore } from '@/stores/common/useSettingStore'
import { applySettings, notifySettingsChanged } from '@/utils/applySettings'
import { TAKT_THEME_STORAGE_KEY } from '@/utils/common'

const settingStore = useSettingStore()
const draftSetting = inject<AppSetting>('setting')!
/** 主题色/明暗仍用抽屉快照；圆角等走 Store 以便 ConfigProvider 立即响应 */
const setting = draftSetting

const emit = defineEmits<{
  'change': []
}>()

const handleChange = () => {
  emit('change')
}

/** 允许的圆角档位 */
const BORDER_RADIUS_OPTIONS = [0, 5, 10, 15, 20] as const

/**
 * 写入圆角并同步 Store / ConfigProvider token
 * @param value 圆角 px
 */
function onBorderRadius(value: number | string): void {
  const n = typeof value === 'number' ? value : Number(value)
  const borderRadius = (BORDER_RADIUS_OPTIONS as readonly number[]).includes(n)
    ? n
    : 5
  setting.borderRadius = borderRadius
  settingStore.patchSetting({ borderRadius })
  applySettings()
  notifySettingsChanged()
  emit('change')
}

/** 用户改明暗/主色时标记外观锁定，并通知父级保存 */
const handleAppearanceChange = () => {
  setting.appearanceUserOverride = true
  if (setting.theme === 'light' || setting.theme === 'dark') {
    localStorage.setItem(TAKT_THEME_STORAGE_KEY, setting.theme)
  }
  emit('change')
}

const customColorValue = computed({
  get: () => setting.themeColor.customColor || '#1890ff',
  set: (v) => {
    const val = (v && String(v).trim()) || '#1890ff'
    setting.themeColor.customColor = val
    handleAppearanceChange()
  }
})

const handleColorSelect = (color: ThemeColor) => {
  setting.themeColor.type = color
  if (color !== 'custom') {
    setting.themeColor.customColor = undefined
  } else {
    setting.themeColor.customColor = customColorValue.value
  }
  handleAppearanceChange()
}

const handleCustomColorPickerInput = (e: Event) => {
  const target = e.target as HTMLInputElement
  customColorValue.value = target.value
}

const handleFontSizeChange = () => {
  setting.fontSize = validateFontSize(setting.fontSize)
  handleChange()
}
</script>

<style scoped>
.theme-settings-space :deep(.ant-space-item) {
  margin-bottom: 4px;
}
.theme-settings-space :deep(.ant-space-item:last-child) {
  margin-bottom: 0;
}

.setting-label-tip {
  margin-left: 4px;
  color: var(--ant-color-text-tertiary);
  cursor: help;
  font-size: 14px;
}

.theme-color-picker {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;

  :deep(.ant-tooltip-trigger) {
    display: block;
  }

  .color-item {
    width: 32px;
    height: 32px;
    border-radius: 4px;
    cursor: pointer;
    border: 2px solid transparent;
    transition: all 0.2s;
    display: flex;
    align-items: center;
    justify-content: center;

    .color-item-check {
      color: white;
    }

    &.active {
      border-color: transparent;
      box-shadow: none;
    }

    &.custom {
      background: #f0f0f0;
      border: 1px dashed #d9d9d9;

      span {
        font-size: 12px;
        color: #666;
      }

      .color-item-check.custom-check {
        color: var(--ant-color-primary);
      }

      &.active {
        border-color: #d9d9d9;
        box-shadow: none;
      }
    }
  }
}
</style>
