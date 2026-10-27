<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：MaterialDocumentItemPanel.vue -->
<!-- 功能描述：Takt物料凭证主表实体主表实体右侧明细 materialDocumentItem 独立 CRUD（按主表选中 materialDocumentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-document-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:materials:material:document:create"
      update-permission="logistics:materials:material:document:update"
      delete-permission="logistics:materials:material:document:delete"
      import-permission="logistics:materials:material:document:import"
      export-permission="logistics:materials:material:document:export"
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
    <div
      ref="detailTableWrapRef"
      class="material-document-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getMaterialDocumentItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="materialDocumentItemId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MaterialDocumentItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMaterialDocumentId"
        :master-row="selecmedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-materials-material-document-material-document-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDocumentCode')">
      <a-form-item :label="pi.queryLabel('materialDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.materialDocumentCode"
          :placeholder="pi.queryPh('materialDocumentCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineId')">
      <a-form-item :label="pi.queryLabel('lineId')">
        <a-input
          v-model:value="advancedQueryForm.lineId"
          :placeholder="pi.queryPh('lineId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentLineId')">
      <a-form-item :label="pi.queryLabel('parentLineId')">
        <a-input
          v-model:value="advancedQueryForm.parentLineId"
          :placeholder="pi.queryPh('parentLineId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineDepth')">
      <a-form-item :label="pi.queryLabel('lineDepth')">
        <a-input
          v-model:value="advancedQueryForm.lineDepth"
          :placeholder="pi.queryPh('lineDepth', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movementType')">
      <a-form-item :label="pi.queryLabel('movementType')">
        <TaktSelect
          v-model:value="advancedQueryForm.movementType"
          dict-type="logistics_materials_movement_type"
          :placeholder="pi.queryPh('movementType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('autoCreatedFlag')">
      <a-form-item :label="pi.queryLabel('autoCreatedFlag')">
        <a-input
          v-model:value="advancedQueryForm.autoCreatedFlag"
          :placeholder="pi.queryPh('autoCreatedFlag', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCode"
          api-url="TaktMaterialPlants/options"
          :placeholder="pi.queryPh('materialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseCode')">
      <a-form-item :label="pi.queryLabel('warehouseCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.warehouseCode"
          api-url="TaktWarehouses/options"
          :placeholder="pi.queryPh('warehouseCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchCode')">
      <a-form-item :label="pi.queryLabel('batchCode')">
        <a-input
          v-model:value="advancedQueryForm.batchCode"
          :placeholder="pi.queryPh('batchCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stockType')">
      <a-form-item :label="pi.queryLabel('stockType')">
        <TaktSelect
          v-model:value="advancedQueryForm.stockType"
          dict-type="logistics_stock_type"
          :placeholder="pi.queryPh('stockType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('restrictedStockFlag')">
      <a-form-item :label="pi.queryLabel('restrictedStockFlag')">
        <a-input
          v-model:value="advancedQueryForm.restrictedStockFlag"
          :placeholder="pi.queryPh('restrictedStockFlag', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('specialStock')">
      <a-form-item :label="pi.queryLabel('specialStock')">
        <TaktSelect
          v-model:value="advancedQueryForm.specialStock"
          dict-type="logistics_materials_special_stock_type"
          :placeholder="pi.queryPh('specialStock', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="pi.queryLabel('supplierCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.supplierCode"
          api-url="TaktSuppliers/options"
          :placeholder="pi.queryPh('supplierCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="pi.queryLabel('customerCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerCode"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('debitCreditIndicator')">
      <a-form-item :label="pi.queryLabel('debitCreditIndicator')">
        <a-input
          v-model:value="advancedQueryForm.debitCreditIndicator"
          :placeholder="pi.queryPh('debitCreditIndicator', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('localCurrencyAmount')">
      <a-form-item :label="pi.queryLabel('localCurrencyAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.localCurrencyAmount"
          :placeholder="pi.queryPh('localCurrencyAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('alternativeAmount')">
      <a-form-item :label="pi.queryLabel('alternativeAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.alternativeAmount"
          :placeholder="pi.queryPh('alternativeAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quantity')">
      <a-form-item :label="pi.queryLabel('quantity')">
        <a-input-number
          v-model:value="advancedQueryForm.quantity"
          :placeholder="pi.queryPh('quantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="pi.queryLabel('baseUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.baseUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('baseUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('entryQuantity')">
      <a-form-item :label="pi.queryLabel('entryQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.entryQuantity"
          :placeholder="pi.queryPh('entryQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('entryUnit')">
      <a-form-item :label="pi.queryLabel('entryUnit')">
        <a-input
          v-model:value="advancedQueryForm.entryUnit"
          :placeholder="pi.queryPh('entryUnit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('poPriceQuantity')">
      <a-form-item :label="pi.queryLabel('poPriceQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.poPriceQuantity"
          :placeholder="pi.queryPh('poPriceQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('poPriceUnit')">
      <a-form-item :label="pi.queryLabel('poPriceUnit')">
        <a-input
          v-model:value="advancedQueryForm.poPriceUnit"
          :placeholder="pi.queryPh('poPriceUnit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="pi.queryLabel('purchaseOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="pi.queryPh('purchaseOrderCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderItem')">
      <a-form-item :label="pi.queryLabel('purchaseOrderItem')">
        <a-input-number
          v-model:value="advancedQueryForm.purchaseOrderItem"
          :placeholder="pi.queryPh('purchaseOrderItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentYear')">
      <a-form-item :label="pi.queryLabel('referenceDocumentYear')">
        <a-input
          v-model:value="advancedQueryForm.referenceDocumentYear"
          :placeholder="pi.queryPh('referenceDocumentYear', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentCode')">
      <a-form-item :label="pi.queryLabel('referenceDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.referenceDocumentCode"
          :placeholder="pi.queryPh('referenceDocumentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentItem')">
      <a-form-item :label="pi.queryLabel('referenceDocumentItem')">
        <a-input-number
          v-model:value="advancedQueryForm.referenceDocumentItem"
          :placeholder="pi.queryPh('referenceDocumentItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originalMaterialDocumentYear')">
      <a-form-item :label="pi.queryLabel('originalMaterialDocumentYear')">
        <a-input
          v-model:value="advancedQueryForm.originalMaterialDocumentYear"
          :placeholder="pi.queryPh('originalMaterialDocumentYear', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originalMaterialDocumentCode')">
      <a-form-item :label="pi.queryLabel('originalMaterialDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.originalMaterialDocumentCode"
          :placeholder="pi.queryPh('originalMaterialDocumentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originalLineNumber')">
      <a-form-item :label="pi.queryLabel('originalLineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.originalLineNumber"
          :placeholder="pi.queryPh('originalLineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryCompletedFlag')">
      <a-form-item :label="pi.queryLabel('deliveryCompletedFlag')">
        <a-input
          v-model:value="advancedQueryForm.deliveryCompletedFlag"
          :placeholder="pi.queryPh('deliveryCompletedFlag', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemText')">
      <a-form-item :label="pi.queryLabel('itemText')">
        <a-input
          v-model:value="advancedQueryForm.itemText"
          :placeholder="pi.queryPh('itemText', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentCode')">
      <a-form-item :label="pi.queryLabel('equipmentCode')">
        <a-input
          v-model:value="advancedQueryForm.equipmentCode"
          :placeholder="pi.queryPh('equipmentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('goodsRecipient')">
      <a-form-item :label="pi.queryLabel('goodsRecipient')">
        <a-input
          v-model:value="advancedQueryForm.goodsRecipient"
          :placeholder="pi.queryPh('goodsRecipient', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unloadingPoint')">
      <a-form-item :label="pi.queryLabel('unloadingPoint')">
        <a-input
          v-model:value="advancedQueryForm.unloadingPoint"
          :placeholder="pi.queryPh('unloadingPoint', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessAreaCode')">
      <a-form-item :label="pi.queryLabel('businessAreaCode')">
        <a-input
          v-model:value="advancedQueryForm.businessAreaCode"
          :placeholder="pi.queryPh('businessAreaCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('controllingAreaCode')">
      <a-form-item :label="pi.queryLabel('controllingAreaCode')">
        <a-input
          v-model:value="advancedQueryForm.controllingAreaCode"
          :placeholder="pi.queryPh('controllingAreaCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tradingPartnerBusinessArea')">
      <a-form-item :label="pi.queryLabel('tradingPartnerBusinessArea')">
        <a-input
          v-model:value="advancedQueryForm.tradingPartnerBusinessArea"
          :placeholder="pi.queryPh('tradingPartnerBusinessArea', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionOrderCode')">
      <a-form-item :label="pi.queryLabel('productionOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.productionOrderCode"
          :placeholder="pi.queryPh('productionOrderCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetCode')">
      <a-form-item :label="pi.queryLabel('assetCode')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="pi.queryPh('assetCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetSubCode')">
      <a-form-item :label="pi.queryLabel('assetSubCode')">
        <a-input
          v-model:value="advancedQueryForm.assetSubCode"
          :placeholder="pi.queryPh('assetSubCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiscalYear')">
      <a-form-item :label="pi.queryLabel('fiscalYear')">
        <a-input
          v-model:value="advancedQueryForm.fiscalYear"
          :placeholder="pi.queryPh('fiscalYear', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postToPreviousPeriodFlag')">
      <a-form-item :label="pi.queryLabel('postToPreviousPeriodFlag')">
        <a-input
          v-model:value="advancedQueryForm.postToPreviousPeriodFlag"
          :placeholder="pi.queryPh('postToPreviousPeriodFlag', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postToPreviousYearFlag')">
      <a-form-item :label="pi.queryLabel('postToPreviousYearFlag')">
        <a-input
          v-model:value="advancedQueryForm.postToPreviousYearFlag"
          :placeholder="pi.queryPh('postToPreviousYearFlag', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountingDocumentCode')">
      <a-form-item :label="pi.queryLabel('accountingDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.accountingDocumentCode"
          :placeholder="pi.queryPh('accountingDocumentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountingDocumentItem')">
      <a-form-item :label="pi.queryLabel('accountingDocumentItem')">
        <a-input-number
          v-model:value="advancedQueryForm.accountingDocumentItem"
          :placeholder="pi.queryPh('accountingDocumentItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revaluationDocumentCode')">
      <a-form-item :label="pi.queryLabel('revaluationDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.revaluationDocumentCode"
          :placeholder="pi.queryPh('revaluationDocumentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revaluationDocumentItem')">
      <a-form-item :label="pi.queryLabel('revaluationDocumentItem')">
        <a-input
          v-model:value="advancedQueryForm.revaluationDocumentItem"
          :placeholder="pi.queryPh('revaluationDocumentItem', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reservationCode')">
      <a-form-item :label="pi.queryLabel('reservationCode')">
        <a-input
          v-model:value="advancedQueryForm.reservationCode"
          :placeholder="pi.queryPh('reservationCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reservationItem')">
      <a-form-item :label="pi.queryLabel('reservationItem')">
        <a-input-number
          v-model:value="advancedQueryForm.reservationItem"
          :placeholder="pi.queryPh('reservationItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('finalIssueFlag')">
      <a-form-item :label="pi.queryLabel('finalIssueFlag')">
        <a-input
          v-model:value="advancedQueryForm.finalIssueFlag"
          :placeholder="pi.queryPh('finalIssueFlag', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reservationQuantity')">
      <a-form-item :label="pi.queryLabel('reservationQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.reservationQuantity"
          :placeholder="pi.queryPh('reservationQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivingMaterialCode')">
      <a-form-item :label="pi.queryLabel('receivingMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.receivingMaterialCode"
          :placeholder="pi.queryPh('receivingMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivingPlantCode')">
      <a-form-item :label="pi.queryLabel('receivingPlantCode')">
        <a-input
          v-model:value="advancedQueryForm.receivingPlantCode"
          :placeholder="pi.queryPh('receivingPlantCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivingWarehouseCode')">
      <a-form-item :label="pi.queryLabel('receivingWarehouseCode')">
        <a-input
          v-model:value="advancedQueryForm.receivingWarehouseCode"
          :placeholder="pi.queryPh('receivingWarehouseCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenterCode')">
      <a-form-item :label="pi.queryLabel('profitCenterCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.profitCenterCode"
          api-url="TaktProfitCenters/options"
          :placeholder="pi.queryPh('profitCenterCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valuatedStockQuantity')">
      <a-form-item :label="pi.queryLabel('valuatedStockQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.valuatedStockQuantity"
          :placeholder="pi.queryPh('valuatedStockQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalValuatedStockValue')">
      <a-form-item :label="pi.queryLabel('totalValuatedStockValue')">
        <a-input-number
          v-model:value="advancedQueryForm.totalValuatedStockValue"
          :placeholder="pi.queryPh('totalValuatedStockValue', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceControl')">
      <a-form-item :label="pi.queryLabel('priceControl')">
        <a-input
          v-model:value="advancedQueryForm.priceControl"
          :placeholder="pi.queryPh('priceControl', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartMaterialCode')">
      <a-form-item :label="pi.queryLabel('manufacturerPartMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPartMaterialCode"
          :placeholder="pi.queryPh('manufacturerPartMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mkpfReferenceCode')">
      <a-form-item :label="pi.queryLabel('mkpfReferenceCode')">
        <a-input
          v-model:value="advancedQueryForm.mkpfReferenceCode"
          :placeholder="pi.queryPh('mkpfReferenceCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('imDeliveryCode')">
      <a-form-item :label="pi.queryLabel('imDeliveryCode')">
        <a-input
          v-model:value="advancedQueryForm.imDeliveryCode"
          :placeholder="pi.queryPh('imDeliveryCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('imDeliveryItem')">
      <a-form-item :label="pi.queryLabel('imDeliveryItem')">
        <a-input-number
          v-model:value="advancedQueryForm.imDeliveryItem"
          :placeholder="pi.queryPh('imDeliveryItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postedBy')">
      <a-form-item :label="pi.queryLabel('postedBy')">
        <a-input
          v-model:value="advancedQueryForm.postedBy"
          :placeholder="pi.queryPh('postedBy', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isObsolete', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="MATERIALDOCUMENTITEM_SELF_I18N_KEY"
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
      id-column-key="materialDocumentItemId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt物料凭证主表实体子表 materialDocumentItem 右栏面板
 * @module views/logistics/materials/material-document/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import MaterialDocumentItemForm from './MaterialDocumentItemForm.vue'
import { useMaterialDocumentMasterContext } from '../composables/useMaterialDocumentMasterContext'
import {
  getMaterialDocumentItemList,
  getMaterialDocumentItemById,
  createMaterialDocumentItem,
  updateMaterialDocumentItem,
  deleteMaterialDocumentItemById,
  deleteMaterialDocumentItemBatch,
  getMaterialDocumentItemTemplate,
  importMaterialDocumentItem,
  exportMaterialDocumentItem,
} from '@/api/logistics/materials/materialDocumentItem.api'
import type { MaterialDocumentItem, MaterialDocumentItemQuery } from '@/types/logistics/materials/materialDocumentItem.types'

import {
  useMaterialDocumentItemI18n,
  MATERIALDOCUMENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS,
  MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS,
  MATERIALDOCUMENTITEM_QUERY_FIELDS,
  MATERIALDOCUMENTITEM_SELF_I18N_KEY,
} from '../composables/useMaterialDocumentItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMaterialDocumentItemI18n()

const { t } = useI18n()
const { selecmedMasterRow } = useMaterialDocumentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialDocumentItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)

/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}
const dataSource = ref<MaterialDocumentItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<MaterialDocumentItem | null>(null)
const selecmedRows = ref<MaterialDocumentItem[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaterialDocumentItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.localCurrencyAmount !== undefined && form.localCurrencyAmount !== null) {
    return true
  }
  if (form.alternativeAmount !== undefined && form.alternativeAmount !== null) {
    return true
  }
  if (form.quantity !== undefined && form.quantity !== null) {
    return true
  }
  if (form.entryQuantity !== undefined && form.entryQuantity !== null) {
    return true
  }
  if (form.poPriceQuantity !== undefined && form.poPriceQuantity !== null) {
    return true
  }
  if (form.purchaseOrderItem !== undefined && form.purchaseOrderItem !== null) {
    return true
  }
  if (form.referenceDocumentItem !== undefined && form.referenceDocumentItem !== null) {
    return true
  }
  if (form.originalLineNumber !== undefined && form.originalLineNumber !== null) {
    return true
  }
  if (form.accountingDocumentItem !== undefined && form.accountingDocumentItem !== null) {
    return true
  }
  if (form.reservationItem !== undefined && form.reservationItem !== null) {
    return true
  }
  if (form.reservationQuantity !== undefined && form.reservationQuantity !== null) {
    return true
  }
  if (form.valuatedStockQuantity !== undefined && form.valuatedStockQuantity !== null) {
    return true
  }
  if (form.totalValuatedStockValue !== undefined && form.totalValuatedStockValue !== null) {
    return true
  }
  if (form.imDeliveryItem !== undefined && form.imDeliveryItem !== null) {
    return true
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    localCurrencyAmount: undefined as number | undefined,
    alternativeAmount: undefined as number | undefined,
    quantity: undefined as number | undefined,
    entryQuantity: undefined as number | undefined,
    poPriceQuantity: undefined as number | undefined,
    purchaseOrderItem: undefined as number | undefined,
    referenceDocumentItem: undefined as number | undefined,
    originalLineNumber: undefined as number | undefined,
    accountingDocumentItem: undefined as number | undefined,
    reservationItem: undefined as number | undefined,
    reservationQuantity: undefined as number | undefined,
    valuatedStockQuantity: undefined as number | undefined,
    totalValuatedStockValue: undefined as number | undefined,
    imDeliveryItem: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  MATERIALDOCUMENTITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...MATERIALDOCUMENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...MATERIALDOCUMENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'materialDocumentItemId'
const masterMaterialDocumentId = computed((): string => {
  const id = (selecmedMasterRow.value as Record<string, unknown> | null)?.['materialDocumentId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterMaterialDocumentId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getMaterialDocumentItemId(record: MaterialDocumentItem | Record<string, unknown>): string {
  return String((record as MaterialDocumentItem)?.[entityIdName] ?? '')
}

function getMaterialDocumentItemField(record: MaterialDocumentItem | Record<string, unknown>, field: string): unknown {
  return (record as MaterialDocumentItem)?.[field as keyof MaterialDocumentItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'materialDocumentItemId',
    key: 'materialDocumentItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialDocumentItemId') ?? ''),
  },
  {
    title: pi.label('materialDocumentId'),
    dataIndex: 'materialDocumentId',
    key: 'materialDocumentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialDocumentId') ?? ''),
  },
  {
    title: pi.label('materialDocumentCode'),
    dataIndex: 'materialDocumentCode',
    key: 'materialDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialDocumentCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('lineId'),
    dataIndex: 'lineId',
    key: 'lineId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'lineId') ?? ''),
  },
  {
    title: pi.label('lineName'),
    dataIndex: 'lineName',
    key: 'lineName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'lineName') ?? ''),
  },
  {
    title: pi.label('parentLineId'),
    dataIndex: 'parentLineId',
    key: 'parentLineId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'parentLineId') ?? ''),
  },
  {
    title: pi.label('lineDepth'),
    dataIndex: 'lineDepth',
    key: 'lineDepth',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'lineDepth') ?? ''),
  },
  {
    title: pi.label('movementType'),
    dataIndex: 'movementType',
    key: 'movementType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'movementType') ?? ''),
  },
  {
    title: pi.label('autoCreatedFlag'),
    dataIndex: 'autoCreatedFlag',
    key: 'autoCreatedFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'autoCreatedFlag') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('warehouseCode'),
    dataIndex: 'warehouseCode',
    key: 'warehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'warehouseCode') ?? ''),
  },
  {
    title: pi.label('batchCode'),
    dataIndex: 'batchCode',
    key: 'batchCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'batchCode') ?? ''),
  },
  {
    title: pi.label('stockType'),
    dataIndex: 'stockType',
    key: 'stockType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'stockType') ?? ''),
  },
  {
    title: pi.label('restrictedStockFlag'),
    dataIndex: 'restrictedStockFlag',
    key: 'restrictedStockFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'restrictedStockFlag') ?? ''),
  },
  {
    title: pi.label('specialStock'),
    dataIndex: 'specialStock',
    key: 'specialStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'specialStock') ?? ''),
  },
  {
    title: pi.label('supplierCode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'supplierCode') ?? ''),
  },
  {
    title: pi.label('customerCode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'customerCode') ?? ''),
  },
  {
    title: pi.label('debitCreditIndicator'),
    dataIndex: 'debitCreditIndicator',
    key: 'debitCreditIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'debitCreditIndicator') ?? ''),
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'currencyCode') ?? ''),
  },
  {
    title: pi.label('localCurrencyAmount'),
    dataIndex: 'localCurrencyAmount',
    key: 'localCurrencyAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'localCurrencyAmount') ?? ''),
  },
  {
    title: pi.label('alternativeAmount'),
    dataIndex: 'alternativeAmount',
    key: 'alternativeAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'alternativeAmount') ?? ''),
  },
  {
    title: pi.label('quantity'),
    dataIndex: 'quantity',
    key: 'quantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'quantity') ?? ''),
  },
  {
    title: pi.label('baseUnit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'baseUnit') ?? ''),
  },
  {
    title: pi.label('entryQuantity'),
    dataIndex: 'entryQuantity',
    key: 'entryQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'entryQuantity') ?? ''),
  },
  {
    title: pi.label('entryUnit'),
    dataIndex: 'entryUnit',
    key: 'entryUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'entryUnit') ?? ''),
  },
  {
    title: pi.label('poPriceQuantity'),
    dataIndex: 'poPriceQuantity',
    key: 'poPriceQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'poPriceQuantity') ?? ''),
  },
  {
    title: pi.label('poPriceUnit'),
    dataIndex: 'poPriceUnit',
    key: 'poPriceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'poPriceUnit') ?? ''),
  },
  {
    title: pi.label('purchaseOrderCode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'purchaseOrderCode') ?? ''),
  },
  {
    title: pi.label('purchaseOrderItem'),
    dataIndex: 'purchaseOrderItem',
    key: 'purchaseOrderItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'purchaseOrderItem') ?? ''),
  },
  {
    title: pi.label('referenceDocumentYear'),
    dataIndex: 'referenceDocumentYear',
    key: 'referenceDocumentYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'referenceDocumentYear') ?? ''),
  },
  {
    title: pi.label('referenceDocumentCode'),
    dataIndex: 'referenceDocumentCode',
    key: 'referenceDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'referenceDocumentCode') ?? ''),
  },
  {
    title: pi.label('referenceDocumentItem'),
    dataIndex: 'referenceDocumentItem',
    key: 'referenceDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'referenceDocumentItem') ?? ''),
  },
  {
    title: pi.label('originalMaterialDocumentYear'),
    dataIndex: 'originalMaterialDocumentYear',
    key: 'originalMaterialDocumentYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'originalMaterialDocumentYear') ?? ''),
  },
  {
    title: pi.label('originalMaterialDocumentCode'),
    dataIndex: 'originalMaterialDocumentCode',
    key: 'originalMaterialDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'originalMaterialDocumentCode') ?? ''),
  },
  {
    title: pi.label('originalLineNumber'),
    dataIndex: 'originalLineNumber',
    key: 'originalLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'originalLineNumber') ?? ''),
  },
  {
    title: pi.label('deliveryCompletedFlag'),
    dataIndex: 'deliveryCompletedFlag',
    key: 'deliveryCompletedFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'deliveryCompletedFlag') ?? ''),
  },
  {
    title: pi.label('itemText'),
    dataIndex: 'itemText',
    key: 'itemText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'itemText') ?? ''),
  },
  {
    title: pi.label('equipmentCode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'equipmentCode') ?? ''),
  },
  {
    title: pi.label('goodsRecipient'),
    dataIndex: 'goodsRecipient',
    key: 'goodsRecipient',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'goodsRecipient') ?? ''),
  },
  {
    title: pi.label('unloadingPoint'),
    dataIndex: 'unloadingPoint',
    key: 'unloadingPoint',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'unloadingPoint') ?? ''),
  },
  {
    title: pi.label('businessAreaCode'),
    dataIndex: 'businessAreaCode',
    key: 'businessAreaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'businessAreaCode') ?? ''),
  },
  {
    title: pi.label('controllingAreaCode'),
    dataIndex: 'controllingAreaCode',
    key: 'controllingAreaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'controllingAreaCode') ?? ''),
  },
  {
    title: pi.label('tradingPartnerBusinessArea'),
    dataIndex: 'tradingPartnerBusinessArea',
    key: 'tradingPartnerBusinessArea',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'tradingPartnerBusinessArea') ?? ''),
  },
  {
    title: pi.label('productionOrderCode'),
    dataIndex: 'productionOrderCode',
    key: 'productionOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'productionOrderCode') ?? ''),
  },
  {
    title: pi.label('assetCode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'assetCode') ?? ''),
  },
  {
    title: pi.label('assetSubCode'),
    dataIndex: 'assetSubCode',
    key: 'assetSubCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'assetSubCode') ?? ''),
  },
  {
    title: pi.label('fiscalYear'),
    dataIndex: 'fiscalYear',
    key: 'fiscalYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'fiscalYear') ?? ''),
  },
  {
    title: pi.label('postToPreviousPeriodFlag'),
    dataIndex: 'postToPreviousPeriodFlag',
    key: 'postToPreviousPeriodFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postToPreviousPeriodFlag') ?? ''),
  },
  {
    title: pi.label('postToPreviousYearFlag'),
    dataIndex: 'postToPreviousYearFlag',
    key: 'postToPreviousYearFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postToPreviousYearFlag') ?? ''),
  },
  {
    title: pi.label('accountingDocumentCode'),
    dataIndex: 'accountingDocumentCode',
    key: 'accountingDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'accountingDocumentCode') ?? ''),
  },
  {
    title: pi.label('accountingDocumentItem'),
    dataIndex: 'accountingDocumentItem',
    key: 'accountingDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'accountingDocumentItem') ?? ''),
  },
  {
    title: pi.label('revaluationDocumentCode'),
    dataIndex: 'revaluationDocumentCode',
    key: 'revaluationDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'revaluationDocumentCode') ?? ''),
  },
  {
    title: pi.label('revaluationDocumentItem'),
    dataIndex: 'revaluationDocumentItem',
    key: 'revaluationDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'revaluationDocumentItem') ?? ''),
  },
  {
    title: pi.label('reservationCode'),
    dataIndex: 'reservationCode',
    key: 'reservationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'reservationCode') ?? ''),
  },
  {
    title: pi.label('reservationItem'),
    dataIndex: 'reservationItem',
    key: 'reservationItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'reservationItem') ?? ''),
  },
  {
    title: pi.label('finalIssueFlag'),
    dataIndex: 'finalIssueFlag',
    key: 'finalIssueFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'finalIssueFlag') ?? ''),
  },
  {
    title: pi.label('reservationQuantity'),
    dataIndex: 'reservationQuantity',
    key: 'reservationQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'reservationQuantity') ?? ''),
  },
  {
    title: pi.label('receivingMaterialCode'),
    dataIndex: 'receivingMaterialCode',
    key: 'receivingMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'receivingMaterialCode') ?? ''),
  },
  {
    title: pi.label('receivingPlantCode'),
    dataIndex: 'receivingPlantCode',
    key: 'receivingPlantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'receivingPlantCode') ?? ''),
  },
  {
    title: pi.label('receivingWarehouseCode'),
    dataIndex: 'receivingWarehouseCode',
    key: 'receivingWarehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'receivingWarehouseCode') ?? ''),
  },
  {
    title: pi.label('profitCenterCode'),
    dataIndex: 'profitCenterCode',
    key: 'profitCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'profitCenterCode') ?? ''),
  },
  {
    title: pi.label('valuatedStockQuantity'),
    dataIndex: 'valuatedStockQuantity',
    key: 'valuatedStockQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'valuatedStockQuantity') ?? ''),
  },
  {
    title: pi.label('totalValuatedStockValue'),
    dataIndex: 'totalValuatedStockValue',
    key: 'totalValuatedStockValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'totalValuatedStockValue') ?? ''),
  },
  {
    title: pi.label('priceControl'),
    dataIndex: 'priceControl',
    key: 'priceControl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'priceControl') ?? ''),
  },
  {
    title: pi.label('manufacturerPartMaterialCode'),
    dataIndex: 'manufacturerPartMaterialCode',
    key: 'manufacturerPartMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'manufacturerPartMaterialCode') ?? ''),
  },
  {
    title: pi.label('mkpfReferenceCode'),
    dataIndex: 'mkpfReferenceCode',
    key: 'mkpfReferenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'mkpfReferenceCode') ?? ''),
  },
  {
    title: pi.label('imDeliveryCode'),
    dataIndex: 'imDeliveryCode',
    key: 'imDeliveryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'imDeliveryCode') ?? ''),
  },
  {
    title: pi.label('imDeliveryItem'),
    dataIndex: 'imDeliveryItem',
    key: 'imDeliveryItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'imDeliveryItem') ?? ''),
  },
  {
    title: pi.label('postedBy'),
    dataIndex: 'postedBy',
    key: 'postedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postedBy') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:material:document:update',
        onClick: (record: MaterialDocumentItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:document:delete',
        onClick: (record: MaterialDocumentItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'materialDocumentItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getMaterialDocumentItemField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialDocumentItem[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaterialDocumentItem, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getMaterialDocumentItemId(selecmedRow.value) === getMaterialDocumentItemId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: MaterialDocumentItem[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaterialDocumentItem) {
  const key = getMaterialDocumentItemId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialDocumentItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialDocumentItemQuery>): MaterialDocumentItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialDocumentItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    materialDocumentId: masterMaterialDocumentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialDocumentItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.localCurrencyAmount !== undefined && form.localCurrencyAmount !== null) {
    query.localCurrencyAmount = form.localCurrencyAmount
  }
  if (form.alternativeAmount !== undefined && form.alternativeAmount !== null) {
    query.alternativeAmount = form.alternativeAmount
  }
  if (form.quantity !== undefined && form.quantity !== null) {
    query.quantity = form.quantity
  }
  if (form.entryQuantity !== undefined && form.entryQuantity !== null) {
    query.entryQuantity = form.entryQuantity
  }
  if (form.poPriceQuantity !== undefined && form.poPriceQuantity !== null) {
    query.poPriceQuantity = form.poPriceQuantity
  }
  if (form.purchaseOrderItem !== undefined && form.purchaseOrderItem !== null) {
    query.purchaseOrderItem = form.purchaseOrderItem
  }
  if (form.referenceDocumentItem !== undefined && form.referenceDocumentItem !== null) {
    query.referenceDocumentItem = form.referenceDocumentItem
  }
  if (form.originalLineNumber !== undefined && form.originalLineNumber !== null) {
    query.originalLineNumber = form.originalLineNumber
  }
  if (form.accountingDocumentItem !== undefined && form.accountingDocumentItem !== null) {
    query.accountingDocumentItem = form.accountingDocumentItem
  }
  if (form.reservationItem !== undefined && form.reservationItem !== null) {
    query.reservationItem = form.reservationItem
  }
  if (form.reservationQuantity !== undefined && form.reservationQuantity !== null) {
    query.reservationQuantity = form.reservationQuantity
  }
  if (form.valuatedStockQuantity !== undefined && form.valuatedStockQuantity !== null) {
    query.valuatedStockQuantity = form.valuatedStockQuantity
  }
  if (form.totalValuatedStockValue !== undefined && form.totalValuatedStockValue !== null) {
    query.totalValuatedStockValue = form.totalValuatedStockValue
  }
  if (form.imDeliveryItem !== undefined && form.imDeliveryItem !== null) {
    query.imDeliveryItem = form.imDeliveryItem
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
  }
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
    const res = await getMaterialDocumentItemList(buildListQuery())
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
watch(masterMaterialDocumentId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selecmed) => {
  if (selecmed) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaterialDocumentItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getMaterialDocumentItemById(getMaterialDocumentItemId(record))
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
      entity: pi.self(),
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
    const id = formData.value?.materialDocumentItemId
    if (id) {
      await updateMaterialDocumentItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMaterialDocumentItem(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
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

async function handleDeleteOne(record: MaterialDocumentItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialDocumentItemById(getMaterialDocumentItemId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getMaterialDocumentItemId(r)).filter(Boolean)
      await deleteMaterialDocumentItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMaterialDocumentItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMaterialDocumentItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
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
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportMaterialDocumentItem(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
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
