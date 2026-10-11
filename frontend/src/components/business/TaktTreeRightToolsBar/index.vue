<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktTreeRightToolsBar
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:右树工具栏（树表布局 4/5 宽），复用 TaktToolsBar 与单表工具栏风格一致

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <TaktToolsBar
    class="takt-tree-right-tools-bar"
    v-bind="$attrs"
  >
    <template
      v-for="(_, slotName) in $slots"
      #[slotName]="slotData"
    >
      <slot
        :name="slotName"
        v-bind="slotData ?? {}"
      />
    </template>
  </TaktToolsBar>
</template>

<script setup lang="ts">
import TaktToolsBar from '@/components/business/TaktToolsBar/index.vue'

export type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'

defineOptions({ inheritAttrs: false })
</script>

<style scoped lang="css">
/* 树表右侧工具栏占内容区 4/5，样式与边距由内部 TaktToolsBar 统一 */
.takt-tree-right-tools-bar {
  flex: 0 0 80%;
  width: 80%;
  max-width: 80%;
  box-sizing: border-box;
}
</style>
