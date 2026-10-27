<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/dedicated-material -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：专用物料（工厂、期间、机种可选；物料只出现在一个机种） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <MaterialUsagePage
    kind="dedicated"
    locale-prefix="logistics.manufacturing.bom.dedicated-material.page"
    export-permission="logistics:manufacturing:bom:dedicated:material:export"
  />
</template>

<script setup lang="ts">
/**
 * 专用物料入口
 */
import MaterialUsagePage from '../material-usage/components/MaterialUsagePage.vue'
</script>
