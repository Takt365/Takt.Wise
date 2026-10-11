<template>
  <div class="layout-settings">
    <a-space
      direction="vertical"
      size="large"
      style="width: 100%"
    >
      <a-form-item :label="$t('components.navigation.page.systemsetting.layoutmode')">
        <a-radio-group
          v-model:value="setting.layout"
          @change="handleChange"
        >
          <a-radio-button value="side">
            {{ $t('components.navigation.page.systemsetting.sidenav') }}
          </a-radio-button>
          <a-radio-button value="top">
            {{ $t('components.navigation.page.systemsetting.topnav') }}
          </a-radio-button>
          <a-radio-button value="mix">
            {{ $t('components.navigation.page.systemsetting.mixnav') }}
          </a-radio-button>
          <a-radio-button value="content">
            {{ $t('components.navigation.page.systemsetting.contentnav') }}
          </a-radio-button>
        </a-radio-group>
      </a-form-item>

      <a-form-item>
        <template #label>
          <span>{{ $t('components.navigation.page.systemsetting.titlelabel') }}</span>
          <a-tooltip
            :title="$t('components.navigation.page.systemsetting.titlehint')"
            placement="top"
          >
            <span class="label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
          </a-tooltip>
        </template>
        <a-input
          v-model:value="setting.logoText"
          placeholder="TaktWise (TDF) "
          @input="handleChange"
          @blur="handleChange"
        />
      </a-form-item>

      <a-form-item>
        <template #label>
          <span>{{ $t('components.navigation.page.systemsetting.collapsetitle') }}</span>
          <a-tooltip
            :title="$t('components.navigation.page.systemsetting.collapsetitlehint')"
            placement="top"
          >
            <span class="label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
          </a-tooltip>
        </template>
        <a-input
          v-model:value="setting.logoCollapsedText"
          placeholder="T"
          @input="handleChange"
          @blur="handleChange"
        />
      </a-form-item>

      <a-form-item>
        <template #label>
          <span>{{ $t('components.navigation.page.systemsetting.showlogo') }}</span>
          <a-tooltip
            :title="$t('components.navigation.page.systemsetting.showlogohint')"
            placement="top"
          >
            <span class="label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
          </a-tooltip>
        </template>
        <a-switch
          v-model:checked="setting.showLogo"
          @change="handleChange"
        />
      </a-form-item>

      <a-form-item v-if="setting.showLogo">
        <template #label>
          <span>{{ $t('components.navigation.page.systemsetting.logopath') }}</span>
          <a-tooltip
            :title="$t('components.navigation.page.systemsetting.logopathhint')"
            placement="top"
          >
            <span class="label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
          </a-tooltip>
        </template>
        <a-input
          v-model:value="logoPath"
          placeholder="assets/images/takt.svg"
          addon-before="@/"
          allow-clear
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.fixedheader')">
        <a-switch
          v-model:checked="setting.fixedHeader"
          @change="handleChange"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.fixedsidebar')">
        <a-switch
          v-model:checked="setting.fixedSider"
          @change="handleChange"
        />
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.sidebarwidth')">
        <a-slider
          v-model:value="setting.siderWidth"
          :min="160"
          :max="300"
          :step="10"
          @change="handleChange"
        />
        <span style="margin-left: 8px">{{ setting.siderWidth }}px</span>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.sidebarcollapsedwidth')">
        <a-slider
          v-model:value="setting.siderCollapsedWidth"
          :min="48"
          :max="80"
          :step="8"
          @change="handleChange"
        />
        <span style="margin-left: 8px">{{ setting.siderCollapsedWidth }}px</span>
      </a-form-item>

      <a-form-item :label="$t('components.navigation.page.systemsetting.contentwidth')">
        <a-radio-group
          v-model:value="setting.contentWidth"
          @change="handleChange"
        >
          <a-radio-button value="fluid">
            {{ $t('components.navigation.page.systemsetting.fluid') }}
          </a-radio-button>
          <a-radio-button value="fixed">
            {{ $t('components.navigation.page.systemsetting.fixed') }}
          </a-radio-button>
        </a-radio-group>
      </a-form-item>
    </a-space>
  </div>
</template>

<script setup lang="ts">
import { RiQuestionLine } from '@remixicon/vue'
import type { AppSetting } from '@/stores/common/useSettingStore'

const setting = inject<AppSetting>('setting')!

const emit = defineEmits<{
  'change': []
}>()

const logoPath = computed({
  get: () => {
    const path = setting.logo || ''
    if (path.startsWith('@/')) return path.substring(2)
    if (path.startsWith('/src/')) return path.substring(5)
    return path
  },
  set: (value: string) => {
    const trimmedValue = value?.trim() || ''
    if (!trimmedValue) {
      setting.logo = ''
    } else {
      setting.logo = (trimmedValue.startsWith('@/') || trimmedValue.startsWith('/')) ? trimmedValue : `@/${trimmedValue}`
    }
    handleChange()
  }
})

const handleChange = () => {
  emit('change')
}
</script>

<style scoped>
.label-hint-icon {
  margin-left: 4px;
  display: inline-flex;
  align-items: center;
  color: var(--ant-color-text-tertiary);
  cursor: help;
  vertical-align: middle;
}
</style>
