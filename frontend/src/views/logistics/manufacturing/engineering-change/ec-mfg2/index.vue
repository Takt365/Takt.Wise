<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-mfg2 -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变制造二课：左栏执行主表，右栏设变明细（采购类型非 F） -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    update-permission="logistics:manufacturing:engineering:change:mfg2:update"
    export-permission="logistics:manufacturing:engineering:change:mfg2:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.pcba"
    id-field="ecMfg2Id"
    dept-slug="ecmfg2"
    :exec-code="TaktEcExecCodes.Mfg2"
    :get-master-list="getEcMfg2List"
    :get-master-by-id="getEcMfg2ById"
    :update-master="updateEcMfg2"
    :update-discontinued-status="handleUpdateDiscontinuedStatus"
    :export-master="exportEcMfg2"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变制造二课列表页（左右主子表：主=执行，从=明细）
 */
import { getEcMfg2List, getEcMfg2ById, updateEcMfg2, exportEcMfg2, updateEcMfg2DiscontinuedStatus } from '@/api/logistics/manufacturing/engineering-change/ecMfg2.api'
import { TaktEcExecCodes } from '@/constants/logistics/ecExecCodes.constants'
import EcDeptExecLrPage from '../components/EcDeptExecLrPage.vue'
import EcDeptViewForm from './components/EcDeptViewForm.vue'

/**
 * 更新停产状态
 * @param id 执行行主键
 * @param discontinuedStatus 根物料停产状态
 */
function handleUpdateDiscontinuedStatus(id: string, discontinuedStatus: string) {
  return updateEcMfg2DiscontinuedStatus({
    ecMfg2Id: id,
    discontinuedStatus,
  })
}
</script>
