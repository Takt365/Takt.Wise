<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-iqc -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变受检部门：左栏执行主表，右栏设变明细 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    update-permission="logistics:manufacturing:engineering:change:iqc:update"
    export-permission="logistics:manufacturing:engineering:change:iqc:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.iqc"
    id-field="ecIqcId"
    dept-slug="eciqc"
    :exec-code="TaktEcExecCodes.Iqc"
    :get-master-list="getEcIqcList"
    :get-master-by-id="getEcIqcById"
    :update-master="updateEcIqc"
    :update-discontinued-status="handleUpdateDiscontinuedStatus"
    :export-master="exportEcIqc"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变受检部门列表页（左右主子表：主=执行，从=明细）
 */
import { getEcIqcList, getEcIqcById, updateEcIqc, exportEcIqc, updateEcIqcDiscontinuedStatus } from '@/api/logistics/manufacturing/engineering-change/ecIqc.api'
import { TaktEcExecCodes } from '@/constants/logistics/ecExecCodes.constants'
import EcDeptExecLrPage from '../components/EcDeptExecLrPage.vue'
import EcDeptViewForm from './components/EcDeptViewForm.vue'

/**
 * 更新停产状态
 * @param id 执行行主键
 * @param discontinuedStatus 根物料停产状态
 */
function handleUpdateDiscontinuedStatus(id: string, discontinuedStatus: string) {
  return updateEcIqcDiscontinuedStatus({
    ecIqcId: id,
    discontinuedStatus,
  })
}
</script>
