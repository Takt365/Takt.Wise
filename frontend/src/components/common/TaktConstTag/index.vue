<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktConstTag -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：内置常量标签（TaktConstants 展示文案，非字典） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-tag v-bind="$attrs">
    <slot>{{ displayLabel }}</slot>
  </a-tag>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { formatTaktConstLabel, type TaktConstCategory } from '@/constants/takt.constants'

interface Props {
  /** 内置常量类别 */
  category: TaktConstCategory
  /** 落库常量值 */
  value?: string | null
}

const props = defineProps<Props>()

/** 展示文案 */
const displayLabel = computed(() => formatTaktConstLabel(props.category, props.value))
</script>
