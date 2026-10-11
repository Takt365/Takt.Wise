<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-koubai -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变采购部门：左栏执行主表，右栏设变明细 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    update-permission="logistics:manufacturing:engineering:change:koubai:update"
    export-permission="logistics:manufacturing:engineering:change:koubai:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.koubai"
    id-field="ecKoubaiId"
    dept-slug="eckoubai"
    :exec-code="TaktEcExecCodes.Mp"
    :get-master-list="getEcKoubaiList"
    :get-master-by-id="getEcKoubaiById"
    :update-master="updateEcKoubai"
    :update-discontinued-status="handleUpdateDiscontinuedStatus"
    :export-master="exportEcKoubai"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变采购部门列表页（左右主子表：主=执行，从=明细）
 */
import { getEcKoubaiList, getEcKoubaiById, updateEcKoubai, exportEcKoubai, updateEcKoubaiDiscontinuedStatus } from '@/api/logistics/manufacturing/engineering-change/ecKoubai.api'
import { TaktEcExecCodes } from '@/constants/logistics/ecExecCodes.constants'
import EcDeptExecLrPage from '../components/EcDeptExecLrPage.vue'
import EcDeptViewForm from './components/EcDeptViewForm.vue'

/**
 * 更新停产状态
 * @param id 执行行主键
 * @param discontinuedStatus 根物料停产状态
 */
function handleUpdateDiscontinuedStatus(id: string, discontinuedStatus: string) {
  return updateEcKoubaiDiscontinuedStatus({
    ecKoubaiId: id,
    discontinuedStatus,
  })
}
</script>
