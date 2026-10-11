<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/components/business/TaktActionColumn -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：操作列样式；布局与下拉菜单壳在此，按钮配色见 button-base.css。逻辑见 index.ts。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-action-column-style-host" aria-hidden="true" />
</template>

<script setup lang="ts">
/** 操作列样式注入占位；逻辑见 index.ts。 */
</script>

<style lang="css">
.takt-action-column {
  padding-left: 4px;
  padding-right: 4px;
  overflow: visible !important;
}

.takt-action-column .ant-space {
  flex-wrap: nowrap;
  gap: 4px;
}

.takt-action-column .ant-dropdown,
.takt-action-column .takt-action-column-dropdown {
  z-index: 1060 !important;
}

.takt-action-column .ant-btn {
  position: relative;
  z-index: 1;
  width: 24px;
  min-width: 24px;
  height: 24px;
  padding: 0;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.takt-action-column .ant-btn .anticon,
.takt-action-column .ant-btn > span.anticon,
.takt-action-column .ant-btn i[class^='ri-'],
.takt-action-column .ant-btn i[class*=' ri-'],
.takt-action-column .ant-btn svg {
  font-size: 16px !important;
  width: 16px !important;
  height: 16px !important;
  line-height: 1;
}

.takt-action-column .takt-button-more {
  position: relative;
  z-index: 2;
}

.takt-action-column-dropdown {
  z-index: 1060 !important;
}

.takt-action-column-dropdown-menu.ant-menu,
.takt-action-column-dropdown .ant-menu,
.takt-action-column-dropdown .ant-dropdown-menu,
.ant-dropdown.takt-action-column-dropdown .ant-dropdown-menu,
body .takt-action-column-dropdown .ant-menu {
  background-color: var(--ant-color-bg-container) !important;
  box-shadow: var(--ant-box-shadow-secondary) !important;
  border: 1px solid var(--ant-color-border-secondary) !important;
  border-radius: var(--ant-border-radius-lg) !important;
  padding: 4px !important;
}

.takt-action-column-dropdown-menu [class*='takt-button-'].ant-menu-item,
.takt-action-column-dropdown [class*='takt-button-'].ant-menu-item,
.takt-action-column-dropdown-menu [class*='takt-button-'].ant-dropdown-menu-item,
.takt-action-column-dropdown [class*='takt-button-'].ant-dropdown-menu-item {
  background-color: transparent !important;
  background: transparent !important;
}

.takt-action-column-dropdown .takt-action-column-menu-label,
.takt-action-column-dropdown-menu .takt-action-column-menu-label {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  line-height: 1.5715;
  white-space: nowrap;
}

.takt-action-column-dropdown .takt-action-column-menu-icon,
.takt-action-column-dropdown .takt-action-column-menu-icon svg,
.takt-action-column-dropdown-menu .takt-action-column-menu-icon,
.takt-action-column-dropdown-menu .takt-action-column-menu-icon svg {
  flex-shrink: 0;
  font-size: 16px !important;
  width: 16px !important;
  height: 16px !important;
  line-height: 1;
}

.takt-action-column-dropdown-menu .ant-menu-item .ant-menu-title-content,
.takt-action-column-dropdown .ant-menu-item .ant-menu-title-content {
  display: inline-flex;
  align-items: center;
  line-height: 1.5715;
}

.takt-action-column-dropdown-menu .ant-menu-item,
.takt-action-column-dropdown .ant-menu-item,
.takt-action-column-dropdown .ant-dropdown-menu-item {
  background-color: transparent !important;
  background: transparent !important;
  box-shadow: none !important;
  border: none !important;
  margin: 0 !important;
  padding: 4px 8px !important;
  border-radius: 4px;
  transition: background-color 0.2s, color 0.2s;
}

.takt-action-column-dropdown-menu .ant-menu-item::before,
.takt-action-column-dropdown-menu .ant-menu-item::after,
.takt-action-column-dropdown .ant-menu-item::before,
.takt-action-column-dropdown .ant-menu-item::after,
.takt-action-column-dropdown .ant-dropdown-menu-item::before,
.takt-action-column-dropdown .ant-dropdown-menu-item::after {
  display: none !important;
  background: transparent !important;
  content: none !important;
}

.takt-action-column-dropdown-menu .ant-menu-item:not([class*='takt-button-']),
.takt-action-column-dropdown-menu .ant-menu-item:not([class*='takt-button-']):hover,
.takt-action-column-dropdown .ant-menu-item:not([class*='takt-button-']),
.takt-action-column-dropdown .ant-menu-item:not([class*='takt-button-']):hover {
  background-color: transparent !important;
  background: transparent !important;
  box-shadow: none !important;
  border: none !important;
}

.takt-action-column-dropdown-menu [class*='takt-button-'].ant-menu-item,
.takt-action-column-dropdown-menu [class*='takt-button-'].ant-menu-item:focus,
.takt-action-column-dropdown-menu [class*='takt-button-'].ant-menu-item:active,
.takt-action-column-dropdown [class*='takt-button-'].ant-menu-item,
.takt-action-column-dropdown [class*='takt-button-'].ant-menu-item:focus,
.takt-action-column-dropdown [class*='takt-button-'].ant-menu-item:active {
  background-color: transparent !important;
  background: transparent !important;
  box-shadow: none !important;
  border: none !important;
}

.takt-action-column-dropdown-menu .ant-menu-item .ant-menu-title-content,
.takt-action-column-dropdown .ant-menu-item .ant-menu-title-content {
  background-color: transparent !important;
  background: transparent !important;
}

.takt-action-column-dropdown-menu .ant-menu-item-divider,
.takt-action-column-dropdown .ant-menu-item-divider {
  display: none !important;
}
</style>
