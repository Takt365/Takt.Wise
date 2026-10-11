<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seizouikka -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变制一部门：左栏执行主表，右栏设变明细 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <EcDeptExecLrPage
    update-permission="logistics:manufacturing:engineering:change:seizouikka:update"
    export-permission="logistics:manufacturing:engineering:change:seizouikka:export"
    menu-i18n-key="menu.logistics.manufacturing.engineering.change.seizouikka"
    id-field="ecSeizouikkaId"
    dept-slug="ecseizouikka"
    :exec-code="TaktEcExecCodes.Assy"
    :get-master-list="getEcSeizouikkaList"
    :get-master-by-id="getEcSeizouikkaById"
    :update-master="updateEcSeizouikka"
    :update-discontinued-status="handleUpdateDiscontinuedStatus"
    :export-master="exportEcSeizouikka"
    :form-component="EcDeptViewForm"
  />
</template>

<script setup lang="ts">
/**
 * 设变制一部门列表页（左右主子表：主=执行，从=明细）
 */
import { getEcSeizouikkaList, getEcSeizouikkaById, updateEcSeizouikka, exportEcSeizouikka, updateEcSeizouikkaDiscontinuedStatus } from '@/api/logistics/manufacturing/engineering-change/ecSeizouikka.api'
import { TaktEcExecCodes } from '@/constants/logistics/ecExecCodes.constants'
import EcDeptExecLrPage from '../components/EcDeptExecLrPage.vue'
import EcDeptViewForm from './components/EcDeptViewForm.vue'

/**
 * 更新停产状态
 * @param id 执行行主键
 * @param discontinuedStatus 根物料停产状态
 */
function handleUpdateDiscontinuedStatus(id: string, discontinuedStatus: string) {
  return updateEcSeizouikkaDiscontinuedStatus({
    ecSeizouikkaId: id,
    discontinuedStatus,
  })
}
</script>
