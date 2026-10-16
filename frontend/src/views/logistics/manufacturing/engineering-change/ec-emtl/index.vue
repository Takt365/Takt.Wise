<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-emtl -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变 E-MTL：左栏执行主表，右栏设变明细（F+C003） -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    update-permission="logistics:manufacturing:engineering:change:smt:update"
    export-permission="logistics:manufacturing:engineering:change:smt:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.emtl"
    id-field="ecEmtlId"
    dept-slug="ecemtl"
    :exec-code="TaktEcExecCodes.Smt"
    :get-master-list="getEcEmtlList"
    :get-master-by-id="getEcEmtlById"
    :update-master="updateEcEmtl"
    :update-discontinued-status="handleUpdateDiscontinuedStatus"
    :export-master="exportEcEmtl"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变 E-MTL 列表页（左右主子表：主=执行，从=明细）
 */
import { getEcEmtlList, getEcEmtlById, updateEcEmtl, exportEcEmtl, updateEcEmtlDiscontinuedStatus } from '@/api/logistics/manufacturing/engineering-change/ecEmtl.api'
import { TaktEcExecCodes } from '@/constants/logistics/ecExecCodes.constants'
import EcDeptExecLrPage from '../components/EcDeptExecLrPage.vue'
import EcDeptViewForm from './components/EcDeptViewForm.vue'

/**
 * 更新停产状态
 * @param id 执行行主键
 * @param discontinuedStatus 根物料停产状态
 */
function handleUpdateDiscontinuedStatus(id: string, discontinuedStatus: string) {
  return updateEcEmtlDiscontinuedStatus({
    ecEmtlId: id,
    discontinuedStatus,
  })
}
</script>
