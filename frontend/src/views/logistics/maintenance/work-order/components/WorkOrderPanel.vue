<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：WorkOrderPanel.vue -->
<!-- 功能描述：Takt工厂设备实体主表实体右侧明细 maintenanceWorkOrder 独立 CRUD（按主表选中 equipmentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="work-order-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.maintenanceworkorder._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:maintenance:equipment:create"
      update-permission="logistics:maintenance:equipment:update"
      delete-permission="logistics:maintenance:equipment:delete"
      import-permission="logistics:maintenance:equipment:import"
      export-permission="logistics:maintenance:equipment:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div class="work-order-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMaintenanceWorkOrderId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="maintenanceWorkOrderId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MaintenanceWorkOrderForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterEquipmentId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-maintenance-work-order-work-order"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('workOrderCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.workordercode')">
        <a-input
          v-model:value="advancedQueryForm.workOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceNotificationId')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancenotificationid')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceNotificationId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancenotificationid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('notificationCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.notificationcode')">
        <a-input
          v-model:value="advancedQueryForm.notificationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.notificationcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.EquipCode')">
        <a-input
          v-model:value="advancedQueryForm.EquipCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.EquipCode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="t('entity.maintenanceworkorder.equipmentname')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.equipmentname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCategory')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceCategory"
          dict-type="logistics_maintenance_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancecategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceType')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancetype')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceType"
          dict-type="logistics_maintenance_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.maintenancetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderStatus')">
      <a-form-item :label="t('entity.maintenanceworkorder.workorderstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.workOrderStatus"
          dict-type="sys_ticket_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.workorderstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.maintenanceworkorder.priority')">
        <a-input-number
          v-model:value="advancedQueryForm.priority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.priority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenter')">
      <a-form-item :label="t('entity.maintenanceworkorder.workcenter')">
        <a-input
          v-model:value="advancedQueryForm.workCenter"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.workcenter') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedTechnician')">
      <a-form-item :label="t('entity.maintenanceworkorder.assignedtechnician')">
        <a-input
          v-model:value="advancedQueryForm.assignedTechnician"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.assignedtechnician') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCompany')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecompany')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceCompany"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancecompany') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.plannedendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.plannedendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.actualendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.actualendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="t('entity.maintenanceworkorder.faultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenanceworkorder.faultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceContent')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecontent')">
        <a-textarea
          v-model:value="advancedQueryForm.maintenanceContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenanceworkorder.maintenancecontent') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('solution')">
      <a-form-item :label="t('entity.maintenanceworkorder.solution')">
        <a-input
          v-model:value="advancedQueryForm.solution"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.solution') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterId')">
      <a-form-item :label="t('entity.maintenanceworkorder.costcenterid')">
        <a-input
          v-model:value="advancedQueryForm.costCenterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costcenterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.costcentercode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costcentercode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementId')">
      <a-form-item :label="t('entity.maintenanceworkorder.costelementid')">
        <a-input
          v-model:value="advancedQueryForm.costElementId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costelementid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementCode')">
      <a-form-item :label="t('entity.maintenanceworkorder.costelementcode')">
        <a-input
          v-model:value="advancedQueryForm.costElementCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.costelementcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalMaterialCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totalmaterialcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalMaterialCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalmaterialcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalLaborCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totallaborcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalLaborCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totallaborcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalOtherCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totalothercost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalOtherCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalothercost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalCost')">
      <a-form-item :label="t('entity.maintenanceworkorder.totalcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.totalcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementStatus')">
      <a-form-item :label="t('entity.maintenanceworkorder.settlementstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.settlementStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.settlementstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.settlementtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.settlementTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('settlementTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.settlementtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.settlementTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.settlementtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.completedatstart')">
        <a-input
          v-model:value="advancedQueryForm.completedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.completedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.completedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.completedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.completedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedBy')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedby')">
        <a-input
          v-model:value="advancedQueryForm.acceptedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedatstart')">
        <a-input
          v-model:value="advancedQueryForm.acceptedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.acceptedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.acceptedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceResult')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenanceresult')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenanceresult') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.nextmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.nextmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.nextmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.nextmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCycleDays')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancecycledays')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCycleDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancecycledays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceImages')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenanceimages')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenanceimages') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDocuments')">
      <a-form-item :label="t('entity.maintenanceworkorder.maintenancedocuments')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceDocuments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.maintenancedocuments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedSummary')">
      <a-form-item :label="t('entity.maintenanceworkorder.acceptedsummary')">
        <a-input
          v-model:value="advancedQueryForm.acceptedSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.acceptedsummary') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isHistoryArchived')">
      <a-form-item :label="t('entity.maintenanceworkorder.ishistoryarchived')">
        <TaktSelect
          v-model:value="advancedQueryForm.isHistoryArchived"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.ishistoryarchived') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.maintenanceworkorder.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.maintenanceworkorder.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkorder.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.maintenanceworkorder.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkorder.flowinstanceid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ t('common.page.entity.extfield') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.maintenanceworkorder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.maintenanceworkorder._self"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="maintenanceWorkOrderId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt工厂设备实体子表 maintenanceWorkOrder 右栏面板
 * @module views/logistics/maintenance/work-order/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import MaintenanceWorkOrderForm from './WorkOrderForm.vue'
import { useEquipmentMasterContext } from '../composables/useEquipmentMasterContext'
import {
  getMaintenanceWorkOrderList,
  getMaintenanceWorkOrderById,
  createMaintenanceWorkOrder,
  updateMaintenanceWorkOrder,
  deleteMaintenanceWorkOrderById,
  deleteMaintenanceWorkOrderBatch,
  getMaintenanceWorkOrderTemplate,
  importMaintenanceWorkOrder,
  exportMaintenanceWorkOrder,
} from '@/api/logistics/maintenance/workOrder.api'
import type { MaintenanceWorkOrder, MaintenanceWorkOrderQuery } from '@/types/logistics/maintenance/workOrder.types'

const { t } = useI18n()
const { selecmedMasterRow } = useEquipmentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceWorkOrder')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenanceworkorder._self') }),
)

const loading = ref(false)
const dataSource = ref<MaintenanceWorkOrder[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<MaintenanceWorkOrder | null>(null)
const selecmedRows = ref<MaintenanceWorkOrder[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaintenanceWorkOrder>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  workOrderCode: '',
  maintenanceNotificationId: '',
  notificationCode: '',
  EquipCode: '',
  equipmentName: '',
  maintenanceCategory: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  workOrderStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  workCenter: '',
  assignedTechnician: '',
  maintenanceCompany: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  faultDescription: '',
  maintenanceContent: '',
  solution: '',
  costCenterId: '',
  costCenterCode: '',
  costElementId: '',
  costElementCode: '',
  totalMaterialCost: undefined as number | undefined,
  totalLaborCost: undefined as number | undefined,
  totalOtherCost: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  settlementStatus: undefined as number | undefined,
  settlementTimeStart: '',
  settlementTimeEnd: '',
  completedAtStart: '',
  completedAtEnd: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  maintenanceResult: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceImages: '',
  maintenanceDocuments: '',
  acceptedSummary: '',
  isHistoryArchived: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'workOrderCode', label: t('entity.maintenanceworkorder.workordercode') },
  { key: 'maintenanceNotificationId', label: t('entity.maintenanceworkorder.maintenancenotificationid') },
  { key: 'notificationCode', label: t('entity.maintenanceworkorder.notificationcode') },
  { key: 'EquipCode', label: t('entity.maintenanceworkorder.EquipCode') },
  { key: 'equipmentName', label: t('entity.maintenanceworkorder.equipmentname') },
  { key: 'maintenanceCategory', label: t('entity.maintenanceworkorder.maintenancecategory') },
  { key: 'maintenanceType', label: t('entity.maintenanceworkorder.maintenancetype') },
  { key: 'workOrderStatus', label: t('entity.maintenanceworkorder.workorderstatus') },
  { key: 'priority', label: t('entity.maintenanceworkorder.priority') },
  { key: 'workCenter', label: t('entity.maintenanceworkorder.workcenter') },
  { key: 'assignedTechnician', label: t('entity.maintenanceworkorder.assignedtechnician') },
  { key: 'maintenanceCompany', label: t('entity.maintenanceworkorder.maintenancecompany') },
  { key: 'plannedStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedstarttime')) },
  { key: 'plannedStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedstarttime')) },
  { key: 'plannedEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedendtime')) },
  { key: 'plannedEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.plannedendtime')) },
  { key: 'actualStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualstarttime')) },
  { key: 'actualStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualstarttime')) },
  { key: 'actualEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualendtime')) },
  { key: 'actualEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.actualendtime')) },
  { key: 'faultDescription', label: t('entity.maintenanceworkorder.faultdescription') },
  { key: 'maintenanceContent', label: t('entity.maintenanceworkorder.maintenancecontent') },
  { key: 'solution', label: t('entity.maintenanceworkorder.solution') },
  { key: 'costCenterId', label: t('entity.maintenanceworkorder.costcenterid') },
  { key: 'costCenterCode', label: t('entity.maintenanceworkorder.costcentercode') },
  { key: 'costElementId', label: t('entity.maintenanceworkorder.costelementid') },
  { key: 'costElementCode', label: t('entity.maintenanceworkorder.costelementcode') },
  { key: 'totalMaterialCost', label: t('entity.maintenanceworkorder.totalmaterialcost') },
  { key: 'totalLaborCost', label: t('entity.maintenanceworkorder.totallaborcost') },
  { key: 'totalOtherCost', label: t('entity.maintenanceworkorder.totalothercost') },
  { key: 'totalCost', label: t('entity.maintenanceworkorder.totalcost') },
  { key: 'settlementStatus', label: t('entity.maintenanceworkorder.settlementstatus') },
  { key: 'settlementTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.settlementtime')) },
  { key: 'settlementTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.settlementtime')) },
  { key: 'completedAtStart', label: t('entity.maintenanceworkorder.completedatstart') },
  { key: 'completedAtEnd', label: t('entity.maintenanceworkorder.completedatend') },
  { key: 'acceptedBy', label: t('entity.maintenanceworkorder.acceptedby') },
  { key: 'acceptedAtStart', label: t('entity.maintenanceworkorder.acceptedatstart') },
  { key: 'acceptedAtEnd', label: t('entity.maintenanceworkorder.acceptedatend') },
  { key: 'maintenanceResult', label: t('entity.maintenanceworkorder.maintenanceresult') },
  { key: 'nextMaintenanceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.nextmaintenancedate')) },
  { key: 'nextMaintenanceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkorder.nextmaintenancedate')) },
  { key: 'maintenanceCycleDays', label: t('entity.maintenanceworkorder.maintenancecycledays') },
  { key: 'maintenanceImages', label: t('entity.maintenanceworkorder.maintenanceimages') },
  { key: 'maintenanceDocuments', label: t('entity.maintenanceworkorder.maintenancedocuments') },
  { key: 'acceptedSummary', label: t('entity.maintenanceworkorder.acceptedsummary') },
  { key: 'isHistoryArchived', label: t('entity.maintenanceworkorder.ishistoryarchived') },
  { key: 'approvalStatus', label: t('entity.maintenanceworkorder.approvalstatus') },
  { key: 'initiatorId', label: t('entity.maintenanceworkorder.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.maintenanceworkorder.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.maintenanceworkorder.initiatedatend') },
  { key: 'approvedBy', label: t('entity.maintenanceworkorder.approvedby') },
  { key: 'approvedAtStart', label: t('entity.maintenanceworkorder.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.maintenanceworkorder.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.maintenanceworkorder.flowinstanceid') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  plantCode: '',
  workOrderCode: '',
  maintenanceNotificationId: '',
  notificationCode: '',
  EquipCode: '',
  equipmentName: '',
  maintenanceCategory: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  workOrderStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  workCenter: '',
  assignedTechnician: '',
  maintenanceCompany: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  faultDescription: '',
  maintenanceContent: '',
  solution: '',
  costCenterId: '',
  costCenterCode: '',
  costElementId: '',
  costElementCode: '',
  totalMaterialCost: undefined as number | undefined,
  totalLaborCost: undefined as number | undefined,
  totalOtherCost: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  settlementStatus: undefined as number | undefined,
  settlementTimeStart: '',
  settlementTimeEnd: '',
  completedAtStart: '',
  completedAtEnd: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  maintenanceResult: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceImages: '',
  maintenanceDocuments: '',
  acceptedSummary: '',
  isHistoryArchived: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'maintenanceWorkOrderId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.equipmentId)
const masterEquipmentId = computed(() => selecmedMasterRow.value?.equipmentId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getMaintenanceWorkOrderId(record: MaintenanceWorkOrder | Record<string, unknown>): string {
  return String((record as MaintenanceWorkOrder)?.[entityIdName] ?? '')
}

function getMaintenanceWorkOrderField(record: MaintenanceWorkOrder | Record<string, unknown>, field: string): unknown {
  return (record as MaintenanceWorkOrder)?.[field as keyof MaintenanceWorkOrder]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceWorkOrderId',
    key: 'maintenanceWorkOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'maintenanceWorkOrderId') ?? ''),
  },
  {
    title: t('common.page.entity.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'plantCode') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'workOrderCode') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.maintenancenotificationid'),
    dataIndex: 'maintenanceNotificationId',
    key: 'maintenanceNotificationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'maintenanceNotificationId') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.notificationcode'),
    dataIndex: 'notificationCode',
    key: 'notificationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'notificationCode') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.EquipCode'),
    dataIndex: 'EquipCode',
    key: 'EquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'EquipCode') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'equipmentName') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.maintenancecategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'maintenanceCategory') ?? ''),
  },
  {
    title: t('entity.maintenanceworkorder.maintenancetype'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrder }) =>
      String(getMaintenanceWorkOrderField(record, 'maintenanceType') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:equipment:update',
        onClick: (record: MaintenanceWorkOrder) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:equipment:delete',
        onClick: (record: MaintenanceWorkOrder) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceWorkOrder[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaintenanceWorkOrder, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getMaintenanceWorkOrderId(selecmedRow.value) === getMaintenanceWorkOrderId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: MaintenanceWorkOrder[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaintenanceWorkOrder) {
  const key = getMaintenanceWorkOrderId(record)
  return {
    onClick: () => {
      selecmedRowKeys.value = [key]
      selecmedRows.value = [record]
      selecmedRow.value = record
    },
    class: selecmedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaintenanceWorkOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaintenanceWorkOrderQuery>): MaintenanceWorkOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaintenanceWorkOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    equipmentId: masterEquipmentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaintenanceWorkOrderQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('workOrderCode', form.workOrderCode)
  assignTrimmed('maintenanceNotificationId', form.maintenanceNotificationId)
  assignTrimmed('notificationCode', form.notificationCode)
  assignTrimmed('EquipCode', form.EquipCode)
  assignTrimmed('equipmentName', form.equipmentName)
  if (form.maintenanceCategory !== undefined && form.maintenanceCategory !== null) {
    query.maintenanceCategory = form.maintenanceCategory
  }
  if (form.maintenanceType !== undefined && form.maintenanceType !== null) {
    query.maintenanceType = form.maintenanceType
  }
  if (form.workOrderStatus !== undefined && form.workOrderStatus !== null) {
    query.workOrderStatus = form.workOrderStatus
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
  assignTrimmed('workCenter', form.workCenter)
  assignTrimmed('assignedTechnician', form.assignedTechnician)
  assignTrimmed('maintenanceCompany', form.maintenanceCompany)
  assignTrimmed('plannedStartTimeStart', form.plannedStartTimeStart)
  assignTrimmed('plannedStartTimeEnd', form.plannedStartTimeEnd)
  assignTrimmed('plannedEndTimeStart', form.plannedEndTimeStart)
  assignTrimmed('plannedEndTimeEnd', form.plannedEndTimeEnd)
  assignTrimmed('actualStartTimeStart', form.actualStartTimeStart)
  assignTrimmed('actualStartTimeEnd', form.actualStartTimeEnd)
  assignTrimmed('actualEndTimeStart', form.actualEndTimeStart)
  assignTrimmed('actualEndTimeEnd', form.actualEndTimeEnd)
  assignTrimmed('faultDescription', form.faultDescription)
  assignTrimmed('maintenanceContent', form.maintenanceContent)
  assignTrimmed('solution', form.solution)
  assignTrimmed('costCenterId', form.costCenterId)
  assignTrimmed('costCenterCode', form.costCenterCode)
  assignTrimmed('costElementId', form.costElementId)
  assignTrimmed('costElementCode', form.costElementCode)
  if (form.totalMaterialCost !== undefined && form.totalMaterialCost !== null) {
    query.totalMaterialCost = form.totalMaterialCost
  }
  if (form.totalLaborCost !== undefined && form.totalLaborCost !== null) {
    query.totalLaborCost = form.totalLaborCost
  }
  if (form.totalOtherCost !== undefined && form.totalOtherCost !== null) {
    query.totalOtherCost = form.totalOtherCost
  }
  if (form.totalCost !== undefined && form.totalCost !== null) {
    query.totalCost = form.totalCost
  }
  if (form.settlementStatus !== undefined && form.settlementStatus !== null) {
    query.settlementStatus = form.settlementStatus
  }
  assignTrimmed('settlementTimeStart', form.settlementTimeStart)
  assignTrimmed('settlementTimeEnd', form.settlementTimeEnd)
  assignTrimmed('completedAtStart', form.completedAtStart)
  assignTrimmed('completedAtEnd', form.completedAtEnd)
  assignTrimmed('acceptedBy', form.acceptedBy)
  assignTrimmed('acceptedAtStart', form.acceptedAtStart)
  assignTrimmed('acceptedAtEnd', form.acceptedAtEnd)
  if (form.maintenanceResult !== undefined && form.maintenanceResult !== null) {
    query.maintenanceResult = form.maintenanceResult
  }
  assignTrimmed('nextMaintenanceDateStart', form.nextMaintenanceDateStart)
  assignTrimmed('nextMaintenanceDateEnd', form.nextMaintenanceDateEnd)
  if (form.maintenanceCycleDays !== undefined && form.maintenanceCycleDays !== null) {
    query.maintenanceCycleDays = form.maintenanceCycleDays
  }
  assignTrimmed('maintenanceImages', form.maintenanceImages)
  assignTrimmed('maintenanceDocuments', form.maintenanceDocuments)
  assignTrimmed('acceptedSummary', form.acceptedSummary)
  if (form.isHistoryArchived !== undefined && form.isHistoryArchived !== null) {
    query.isHistoryArchived = form.isHistoryArchived
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getMaintenanceWorkOrderList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterEquipmentId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenanceworkorder._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaintenanceWorkOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenanceworkorder._self') })
  formLoading.value = true
  try {
    const detail = await getMaintenanceWorkOrderById(getMaintenanceWorkOrderId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.maintenanceworkorder._self'),
    }))
  }
}

async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.maintenanceWorkOrderId
    if (id) {
      await updateMaintenanceWorkOrder(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.maintenanceworkorder._self') }))
    } else {
      await createMaintenanceWorkOrder(payload)
      message.success(t('common.feedback.created', { target: t('entity.maintenanceworkorder._self') }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: MaintenanceWorkOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.maintenanceworkorder._self'),
      name: t('common.tip.this.target', { target: t('entity.maintenanceworkorder._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceWorkOrderById(getMaintenanceWorkOrderId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.maintenanceworkorder._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.maintenanceworkorder._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.maintenanceworkorder._self'),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getMaintenanceWorkOrderId(r)).filter(Boolean)
      await deleteMaintenanceWorkOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenanceworkorder._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMaintenanceWorkOrderTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceWorkOrder(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportMaintenanceWorkOrder(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.maintenanceworkorder._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.maintenanceworkorder._self') }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
