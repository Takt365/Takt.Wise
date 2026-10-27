<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderFont
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:字体大小切换组件,用于调整系统字体大小

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-dropdown>
    <a-button
      type="text"
      class="takt-header-font"
    >
      <template #icon>
        <RiFontSize class="takt-remix-icon" />
      </template>
    </a-button>
    <template #overlay>
      <a-menu @click="handleSizeChange">
        <a-menu-item 
          v-for="size in fontSizeOptions" 
          :key="size" 
          :class="{ 'selecmed': fontSize === size }"
        >
          {{ size }}px
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import { validateFontSize, defaultSetting } from '@/stores/common/useSettingStore'
import { RiFontSize } from '@remixicon/vue'

interface Props {
  modelValue?: number
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: defaultSetting.fontSize
})

const emit = defineEmits<{
  'update:modelValue': [value: number]
  'change': [value: number]
}>()

const fontSize = ref<number>(validateFontSize(props.modelValue ?? 15))

// 字体大小选项：15-22
const fontSizeOptions = computed(() => {
  const options: number[] = []
  for (let i = 15; i <= 22; i++) {
    options.push(i)
  }
  return options
})

const handleSizeChange = (info: MenuInfo) => {
  const newSize = validateFontSize(parseInt(String(info.key), 10))
  fontSize.value = newSize
  emit('update:modelValue', newSize)
  emit('change', newSize)
  
  // 应用字体大小到根元素
  const root = document.documentElement
  root.style.fontSize = `${newSize}px`
}

watch(() => props.modelValue, (val) => {
  if (val !== undefined) {
    const newSize = validateFontSize(val)
    fontSize.value = newSize
    // 应用字体大小到根元素
    const root = document.documentElement
    root.style.fontSize = `${newSize}px`
  }
}, { immediate: true })
</script>

<style scoped>

</style>
