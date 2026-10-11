<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/general-material/components -->
<!-- 文件名称：GeneralMaterialForm.vue -->
<!-- 功能描述：Takt全局物料实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="general-material-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.generalMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('completeMaintenanceStatus')"
                name="completeMaintenanceStatus"
              >
                <a-input
                  v-model:value="formState.completeMaintenanceStatus"
                  :placeholder="pi.ph('completeMaintenanceStatus')"
                  show-count
                  :maxlength="15"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maintenanceStatus')"
                name="maintenanceStatus"
              >
                <a-input
                  v-model:value="formState.maintenanceStatus"
                  :placeholder="pi.ph('maintenanceStatus')"
                  show-count
                  :maxlength="15"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientDeletionFlag')"
                name="clientDeletionFlag"
              >
                <TaktSelect
                  v-model:value="formState.clientDeletionFlag"
                  dict-type="logistics_client_deletion_flag"
                  :placeholder="pi.ph('clientDeletionFlag')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialType')"
                name="materialType"
              >
                <TaktSelect
                  v-model:value="formState.materialType"
                  dict-type="logistics_materials_material_type"
                  :placeholder="pi.ph('materialType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('industrySector')"
                name="industrySector"
              >
                <TaktSelect
                  v-model:value="formState.industrySector"
                  dict-type="logistics_materials_industry_sector"
                  :placeholder="pi.ph('industrySector')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialGroup')"
                name="materialGroup"
              >
                <TaktSelect
                  v-model:value="formState.materialGroup"
                  api-url="TaktMaterialGroups/options"
                  :placeholder="pi.ph('materialGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('oldMaterialNumber')"
                name="oldMaterialNumber"
              >
                <a-input
                  v-model:value="formState.oldMaterialNumber"
                  :placeholder="pi.ph('oldMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <TaktSelect
                  v-model:value="formState.baseUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('baseUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderUnit')"
                name="orderUnit"
              >
                <TaktSelect
                  v-model:value="formState.orderUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('orderUnit')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentNumber')"
                name="documentNumber"
              >
                <a-input
                  v-model:value="formState.documentNumber"
                  :placeholder="pi.ph('documentNumber')"
                  show-count
                  :maxlength="22"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentType')"
                name="documentType"
              >
                <TaktSelect
                  v-model:value="formState.documentType"
                  dict-type="logistics_document_type"
                  :placeholder="pi.ph('documentType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentVersion')"
                name="documentVersion"
              >
                <a-input
                  v-model:value="formState.documentVersion"
                  :placeholder="pi.ph('documentVersion')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentPageFormat')"
                name="documentPageFormat"
              >
                <TaktSelect
                  v-model:value="formState.documentPageFormat"
                  dict-type="logistics_document_page_format"
                  :placeholder="pi.ph('documentPageFormat')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentChangeNumber')"
                name="documentChangeNumber"
              >
                <a-input
                  v-model:value="formState.documentChangeNumber"
                  :placeholder="pi.ph('documentChangeNumber')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentPageNumber')"
                name="documentPageNumber"
              >
                <a-input
                  v-model:value="formState.documentPageNumber"
                  :placeholder="pi.ph('documentPageNumber')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentSheetCount')"
                name="documentSheetCount"
              >
                <a-input
                  v-model:value="formState.documentSheetCount"
                  :placeholder="pi.ph('documentSheetCount')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionInspectionMemo')"
                name="productionInspectionMemo"
              >
                <a-input
                  v-model:value="formState.productionInspectionMemo"
                  :placeholder="pi.ph('productionInspectionMemo')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionMemoPageFormat')"
                name="productionMemoPageFormat"
              >
                <TaktSelect
                  v-model:value="formState.productionMemoPageFormat"
                  dict-type="logistics_production_memo_page_format"
                  :placeholder="pi.ph('productionMemoPageFormat')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sizeDimensions')"
                name="sizeDimensions"
              >
                <a-input
                  v-model:value="formState.sizeDimensions"
                  :placeholder="pi.ph('sizeDimensions')"
                  show-count
                  :maxlength="32"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('basicMaterial')"
                name="basicMaterial"
              >
                <a-input
                  v-model:value="formState.basicMaterial"
                  :placeholder="pi.ph('basicMaterial')"
                  show-count
                  :maxlength="48"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('industryStandardDescription')"
                name="industryStandardDescription"
              >
                <a-textarea
                  v-model:value="formState.industryStandardDescription"
                  :placeholder="pi.ph('industryStandardDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('laboratoryDesignOffice')"
                name="laboratoryDesignOffice"
              >
                <TaktSelect
                  v-model:value="formState.laboratoryDesignOffice"
                  dict-type="logistics_laboratory_design_office"
                  :placeholder="pi.ph('laboratoryDesignOffice')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchasingValueKey')"
                name="purchasingValueKey"
              >
                <TaktSelect
                  v-model:value="formState.purchasingValueKey"
                  dict-type="logistics_purchasing_value_key"
                  :placeholder="pi.ph('purchasingValueKey')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossWeight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="pi.ph('grossWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netWeight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="pi.ph('netWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weightUnit')"
                name="weightUnit"
              >
                <TaktSelect
                  v-model:value="formState.weightUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('weightUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volume')"
                name="volume"
              >
                <a-input-number
                  v-model:value="formState.volume"
                  :placeholder="pi.ph('volume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volumeUnit')"
                name="volumeUnit"
              >
                <TaktSelect
                  v-model:value="formState.volumeUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('volumeUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('containerRequirements')"
                name="containerRequirements"
              >
                <TaktSelect
                  v-model:value="formState.containerRequirements"
                  dict-type="logistics_container_requirements"
                  :placeholder="pi.ph('containerRequirements')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('storageConditions')"
                name="storageConditions"
              >
                <TaktSelect
                  v-model:value="formState.storageConditions"
                  dict-type="logistics_storage_conditions"
                  :placeholder="pi.ph('storageConditions')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('temperatureConditions')"
                name="temperatureConditions"
              >
                <TaktSelect
                  v-model:value="formState.temperatureConditions"
                  dict-type="logistics_temperature_conditions"
                  :placeholder="pi.ph('temperatureConditions')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lowLevelCode')"
                name="lowLevelCode"
              >
                <a-input
                  v-model:value="formState.lowLevelCode"
                  :placeholder="pi.ph('lowLevelCode')"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.generalMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('transportationGroup')"
                name="transportationGroup"
              >
                <TaktSelect
                  v-model:value="formState.transportationGroup"
                  dict-type="logistics_transportation_group"
                  :placeholder="pi.ph('transportationGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hazardousMaterialNumber')"
                name="hazardousMaterialNumber"
              >
                <a-input
                  v-model:value="formState.hazardousMaterialNumber"
                  :placeholder="pi.ph('hazardousMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('division')"
                name="division"
              >
                <TaktSelect
                  v-model:value="formState.division"
                  dict-type="logistics_product_group"
                  :placeholder="pi.ph('division')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('competitor')"
                name="competitor"
              >
                <a-input
                  v-model:value="formState.competitor"
                  :placeholder="pi.ph('competitor')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('europeanArticleNumberObsolete')"
                name="europeanArticleNumberObsolete"
              >
                <a-input
                  v-model:value="formState.europeanArticleNumberObsolete"
                  :placeholder="pi.ph('europeanArticleNumberObsolete')"
                  show-count
                  :maxlength="13"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grGiSlipQuantity')"
                name="grGiSlipQuantity"
              >
                <a-input-number
                  v-model:value="formState.grGiSlipQuantity"
                  :placeholder="pi.ph('grGiSlipQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('procurementRule')"
                name="procurementRule"
              >
                <TaktSelect
                  v-model:value="formState.procurementRule"
                  dict-type="logistics_procurement_rule"
                  :placeholder="pi.ph('procurementRule')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceOfSupply')"
                name="sourceOfSupply"
              >
                <TaktSelect
                  v-model:value="formState.sourceOfSupply"
                  dict-type="logistics_source_of_supply_type"
                  :placeholder="pi.ph('sourceOfSupply')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonCategory')"
                name="seasonCategory"
              >
                <TaktSelect
                  v-model:value="formState.seasonCategory"
                  dict-type="logistics_season_category"
                  :placeholder="pi.ph('seasonCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('labelType')"
                name="labelType"
              >
                <TaktSelect
                  v-model:value="formState.labelType"
                  dict-type="logistics_label_type"
                  :placeholder="pi.ph('labelType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('labelForm')"
                name="labelForm"
              >
                <TaktSelect
                  v-model:value="formState.labelForm"
                  dict-type="logistics_label_form"
                  :placeholder="pi.ph('labelForm')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deactivatedField')"
                name="deactivatedField"
              >
                <a-input
                  v-model:value="formState.deactivatedField"
                  :placeholder="pi.ph('deactivatedField')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('internationalArticleNumber')"
                name="internationalArticleNumber"
              >
                <a-input
                  v-model:value="formState.internationalArticleNumber"
                  :placeholder="pi.ph('internationalArticleNumber')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('eanCategory')"
                name="eanCategory"
              >
                <TaktSelect
                  v-model:value="formState.eanCategory"
                  dict-type="logistics_ean_category"
                  :placeholder="pi.ph('eanCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('length')"
                name="length"
              >
                <a-input-number
                  v-model:value="formState.length"
                  :placeholder="pi.ph('length')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('width')"
                name="width"
              >
                <a-input-number
                  v-model:value="formState.width"
                  :placeholder="pi.ph('width')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('height')"
                name="height"
              >
                <a-input-number
                  v-model:value="formState.height"
                  :placeholder="pi.ph('height')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dimensionUnit')"
                name="dimensionUnit"
              >
                <TaktSelect
                  v-model:value="formState.dimensionUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('dimensionUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productHierarchy')"
                name="productHierarchy"
              >
                <TaktSelect
                  v-model:value="formState.productHierarchy"
                  dict-type="logistics_product_hierarchy"
                  :placeholder="pi.ph('productHierarchy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stockTransferNetChangeCosting')"
                name="stockTransferNetChangeCosting"
              >
                <a-input
                  v-model:value="formState.stockTransferNetChangeCosting"
                  :placeholder="pi.ph('stockTransferNetChangeCosting')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cadIndicator')"
                name="cadIndicator"
              >
                <a-input
                  v-model:value="formState.cadIndicator"
                  :placeholder="pi.ph('cadIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('qmInProcurement')"
                name="qmInProcurement"
              >
                <a-input
                  v-model:value="formState.qmInProcurement"
                  :placeholder="pi.ph('qmInProcurement')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingWeight')"
                name="allowedPackagingWeight"
              >
                <a-input-number
                  v-model:value="formState.allowedPackagingWeight"
                  :placeholder="pi.ph('allowedPackagingWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingWeightUnit')"
                name="allowedPackagingWeightUnit"
              >
                <TaktSelect
                  v-model:value="formState.allowedPackagingWeightUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('allowedPackagingWeightUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingVolume')"
                name="allowedPackagingVolume"
              >
                <a-input-number
                  v-model:value="formState.allowedPackagingVolume"
                  :placeholder="pi.ph('allowedPackagingVolume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allowedPackagingVolumeUnit')"
                name="allowedPackagingVolumeUnit"
              >
                <TaktSelect
                  v-model:value="formState.allowedPackagingVolumeUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('allowedPackagingVolumeUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('excessWeightTolerance')"
                name="excessWeightTolerance"
              >
                <a-input-number
                  v-model:value="formState.excessWeightTolerance"
                  :placeholder="pi.ph('excessWeightTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-6"
        :tab="t('common.page.form.tabs.basicinfo') + ' (7/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('excessVolumeTolerance')"
                name="excessVolumeTolerance"
              >
                <a-input-number
                  v-model:value="formState.excessVolumeTolerance"
                  :placeholder="pi.ph('excessVolumeTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variablePurchaseOrderUnit')"
                name="variablePurchaseOrderUnit"
              >
                <a-input
                  v-model:value="formState.variablePurchaseOrderUnit"
                  :placeholder="pi.ph('variablePurchaseOrderUnit')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revisionLevelAssigned')"
                name="revisionLevelAssigned"
              >
                <a-input
                  v-model:value="formState.revisionLevelAssigned"
                  :placeholder="pi.ph('revisionLevelAssigned')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('configurableMaterial')"
                name="configurableMaterial"
              >
                <a-input
                  v-model:value="formState.configurableMaterial"
                  :placeholder="pi.ph('configurableMaterial')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchManagementRequired')"
                name="batchManagementRequired"
              >
                <TaktSelect
                  v-model:value="formState.batchManagementRequired"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('batchManagementRequired')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('packagingMaterialType')"
                name="packagingMaterialType"
              >
                <TaktSelect
                  v-model:value="formState.packagingMaterialType"
                  dict-type="logistics_materials_material_type"
                  :placeholder="pi.ph('packagingMaterialType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumLevelByVolume')"
                name="maximumLevelByVolume"
              >
                <a-input-number
                  v-model:value="formState.maximumLevelByVolume"
                  :placeholder="pi.ph('maximumLevelByVolume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stackingFactor')"
                name="stackingFactor"
              >
                <a-input-number
                  v-model:value="formState.stackingFactor"
                  :placeholder="pi.ph('stackingFactor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('packagingMaterialGroup')"
                name="packagingMaterialGroup"
              >
                <TaktSelect
                  v-model:value="formState.packagingMaterialGroup"
                  dict-type="logistics_packaging_material_group"
                  :placeholder="pi.ph('packagingMaterialGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('authorizationGroup')"
                name="authorizationGroup"
              >
                <TaktSelect
                  v-model:value="formState.authorizationGroup"
                  dict-type="logistics_authorization_group"
                  :placeholder="pi.ph('authorizationGroup')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-7"
        :tab="t('common.page.form.tabs.basicinfo') + ' (8/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('validFromDate')"
                name="validFromDate"
              >
                <a-date-picker
                  v-model:value="formState.validFromDate"
                  :placeholder="pi.ph('validFromDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('validToDate')"
                name="validToDate"
              >
                <a-date-picker
                  v-model:value="formState.validToDate"
                  :placeholder="pi.ph('validToDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonYear')"
                name="seasonYear"
              >
                <TaktSelect
                  v-model:value="formState.seasonYear"
                  dict-type="logistics_season_year"
                  :placeholder="pi.ph('seasonYear')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceBandCategory')"
                name="priceBandCategory"
              >
                <TaktSelect
                  v-model:value="formState.priceBandCategory"
                  dict-type="logistics_price_band_category"
                  :placeholder="pi.ph('priceBandCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('emptiesBillOfMaterial')"
                name="emptiesBillOfMaterial"
              >
                <a-input
                  v-model:value="formState.emptiesBillOfMaterial"
                  :placeholder="pi.ph('emptiesBillOfMaterial')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('externalMaterialGroup')"
                name="externalMaterialGroup"
              >
                <TaktSelect
                  v-model:value="formState.externalMaterialGroup"
                  dict-type="logistics_external_material_group"
                  :placeholder="pi.ph('externalMaterialGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossPlantConfigurableMaterial')"
                name="crossPlantConfigurableMaterial"
              >
                <a-input
                  v-model:value="formState.crossPlantConfigurableMaterial"
                  :placeholder="pi.ph('crossPlantConfigurableMaterial')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCategory')"
                name="materialCategory"
              >
                <TaktSelect
                  v-model:value="formState.materialCategory"
                  dict-type="logistics_material_category"
                  :placeholder="pi.ph('materialCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('coProductIndicator')"
                name="coProductIndicator"
              >
                <a-input
                  v-model:value="formState.coProductIndicator"
                  :placeholder="pi.ph('coProductIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('followUpMaterialIndicator')"
                name="followUpMaterialIndicator"
              >
                <a-input
                  v-model:value="formState.followUpMaterialIndicator"
                  :placeholder="pi.ph('followUpMaterialIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-8"
        :tab="t('common.page.form.tabs.basicinfo') + ' (9/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingReferenceMaterial')"
                name="pricingReferenceMaterial"
              >
                <a-input
                  v-model:value="formState.pricingReferenceMaterial"
                  :placeholder="pi.ph('pricingReferenceMaterial')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossPlantMaterialStatus')"
                name="crossPlantMaterialStatus"
              >
                <TaktSelect
                  v-model:value="formState.crossPlantMaterialStatus"
                  dict-type="logistics_cross_plant_material_status"
                  :placeholder="pi.ph('crossPlantMaterialStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossDistributionChainStatus')"
                name="crossDistributionChainStatus"
              >
                <TaktSelect
                  v-model:value="formState.crossDistributionChainStatus"
                  dict-type="logistics_cross_distribution_chain_status"
                  :placeholder="pi.ph('crossDistributionChainStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossPlantStatusValidFrom')"
                name="crossPlantStatusValidFrom"
              >
                <a-date-picker
                  v-model:value="formState.crossPlantStatusValidFrom"
                  :placeholder="pi.ph('crossPlantStatusValidFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('crossDistributionStatusValidFrom')"
                name="crossDistributionStatusValidFrom"
              >
                <a-date-picker
                  v-model:value="formState.crossDistributionStatusValidFrom"
                  :placeholder="pi.ph('crossDistributionStatusValidFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxClassification')"
                name="taxClassification"
              >
                <TaktSelect
                  v-model:value="formState.taxClassification"
                  dict-type="logistics_material_tax_classification"
                  :placeholder="pi.ph('taxClassification')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catalogProfile')"
                name="catalogProfile"
              >
                <TaktSelect
                  v-model:value="formState.catalogProfile"
                  dict-type="logistics_catalog_profile"
                  :placeholder="pi.ph('catalogProfile')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('minimumRemainingShelfLife')"
                name="minimumRemainingShelfLife"
              >
                <a-input-number
                  v-model:value="formState.minimumRemainingShelfLife"
                  :placeholder="pi.ph('minimumRemainingShelfLife')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalShelfLife')"
                name="totalShelfLife"
              >
                <a-input-number
                  v-model:value="formState.totalShelfLife"
                  :placeholder="pi.ph('totalShelfLife')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('storagePercentage')"
                name="storagePercentage"
              >
                <a-input-number
                  v-model:value="formState.storagePercentage"
                  :placeholder="pi.ph('storagePercentage')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-9"
        :tab="t('common.page.form.tabs.basicinfo') + ' (10/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contentUnit')"
                name="contentUnit"
              >
                <TaktSelect
                  v-model:value="formState.contentUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('contentUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('netContents')"
                name="netContents"
              >
                <a-textarea
                  v-model:value="formState.netContents"
                  :placeholder="pi.ph('netContents')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('comparisonPriceUnit')"
                name="comparisonPriceUnit"
              >
                <a-input-number
                  v-model:value="formState.comparisonPriceUnit"
                  :placeholder="pi.ph('comparisonPriceUnit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('labelingMaterialGrouping')"
                name="labelingMaterialGrouping"
              >
                <TaktSelect
                  v-model:value="formState.labelingMaterialGrouping"
                  dict-type="logistics_labeling_material_grouping"
                  :placeholder="pi.ph('labelingMaterialGrouping')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('grossContents')"
                name="grossContents"
              >
                <a-textarea
                  v-model:value="formState.grossContents"
                  :placeholder="pi.ph('grossContents')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quantityConversionMethod')"
                name="quantityConversionMethod"
              >
                <TaktSelect
                  v-model:value="formState.quantityConversionMethod"
                  dict-type="logistics_quantity_conversion_method"
                  :placeholder="pi.ph('quantityConversionMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('internalObjectNumber')"
                name="internalObjectNumber"
              >
                <a-input
                  v-model:value="formState.internalObjectNumber"
                  :placeholder="pi.ph('internalObjectNumber')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('environmentallyRelevant')"
                name="environmentallyRelevant"
              >
                <a-input
                  v-model:value="formState.environmentallyRelevant"
                  :placeholder="pi.ph('environmentallyRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productAllocationProcedure')"
                name="productAllocationProcedure"
              >
                <TaktSelect
                  v-model:value="formState.productAllocationProcedure"
                  dict-type="logistics_product_allocation_procedure"
                  :placeholder="pi.ph('productAllocationProcedure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variantPricingProfile')"
                name="variantPricingProfile"
              >
                <TaktSelect
                  v-model:value="formState.variantPricingProfile"
                  dict-type="logistics_variant_pricing_profile"
                  :placeholder="pi.ph('variantPricingProfile')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-10"
        :tab="t('common.page.form.tabs.basicinfo') + ' (11/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountInKind')"
                name="discountInKind"
              >
                <a-input
                  v-model:value="formState.discountInKind"
                  :placeholder="pi.ph('discountInKind')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerPartNumber')"
                name="manufacturerPartNumber"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerPartNumber"
                  api-url="TaktManufacturerMaterials/options"
                  :placeholder="pi.ph('manufacturerPartNumber')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerNumber')"
                name="manufacturerNumber"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerNumber"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('manufacturerNumber')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inventoryManagedMaterialNumber')"
                name="inventoryManagedMaterialNumber"
              >
                <a-input
                  v-model:value="formState.inventoryManagedMaterialNumber"
                  :placeholder="pi.ph('inventoryManagedMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerPartProfile')"
                name="manufacturerPartProfile"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerPartProfile"
                  dict-type="logistics_manufacturer_part_profile"
                  :placeholder="pi.ph('manufacturerPartProfile')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitsOfMeasureUsage')"
                name="unitsOfMeasureUsage"
              >
                <TaktSelect
                  v-model:value="formState.unitsOfMeasureUsage"
                  dict-type="logistics_units_of_measure_usage"
                  :placeholder="pi.ph('unitsOfMeasureUsage')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonRollout')"
                name="seasonRollout"
              >
                <TaktSelect
                  v-model:value="formState.seasonRollout"
                  dict-type="logistics_season_rollout"
                  :placeholder="pi.ph('seasonRollout')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dangerousGoodsProfile')"
                name="dangerousGoodsProfile"
              >
                <TaktSelect
                  v-model:value="formState.dangerousGoodsProfile"
                  dict-type="logistics_dangerous_goods_profile"
                  :placeholder="pi.ph('dangerousGoodsProfile')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('highlyViscous')"
                name="highlyViscous"
              >
                <a-input
                  v-model:value="formState.highlyViscous"
                  :placeholder="pi.ph('highlyViscous')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inBulkLiquid')"
                name="inBulkLiquid"
              >
                <a-input
                  v-model:value="formState.inBulkLiquid"
                  :placeholder="pi.ph('inBulkLiquid')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-11"
        :tab="t('common.page.form.tabs.basicinfo') + ' (12/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNumberExplicitness')"
                name="serialNumberExplicitness"
              >
                <TaktSelect
                  v-model:value="formState.serialNumberExplicitness"
                  dict-type="logistics_serial_number_explicitness"
                  :placeholder="pi.ph('serialNumberExplicitness')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closedPackaging')"
                name="closedPackaging"
              >
                <a-input
                  v-model:value="formState.closedPackaging"
                  :placeholder="pi.ph('closedPackaging')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('approvedBatchRecordRequired')"
                name="approvedBatchRecordRequired"
              >
                <a-input
                  v-model:value="formState.approvedBatchRecordRequired"
                  :placeholder="pi.ph('approvedBatchRecordRequired')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('effectivityParameterOverride')"
                name="effectivityParameterOverride"
              >
                <a-input
                  v-model:value="formState.effectivityParameterOverride"
                  :placeholder="pi.ph('effectivityParameterOverride')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCompletionLevel')"
                name="materialCompletionLevel"
              >
                <TaktSelect
                  v-model:value="formState.materialCompletionLevel"
                  dict-type="logistics_material_completion_level"
                  :placeholder="pi.ph('materialCompletionLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shelfLifePeriodIndicator')"
                name="shelfLifePeriodIndicator"
              >
                <TaktSelect
                  v-model:value="formState.shelfLifePeriodIndicator"
                  dict-type="logistics_shelf_life_period_indicator"
                  :placeholder="pi.ph('shelfLifePeriodIndicator')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shelfLifeRoundingRule')"
                name="shelfLifeRoundingRule"
              >
                <TaktSelect
                  v-model:value="formState.shelfLifeRoundingRule"
                  dict-type="logistics_shelf_life_rounding_rule"
                  :placeholder="pi.ph('shelfLifeRoundingRule')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productCompositionOnPackaging')"
                name="productCompositionOnPackaging"
              >
                <a-input
                  v-model:value="formState.productCompositionOnPackaging"
                  :placeholder="pi.ph('productCompositionOnPackaging')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('generalItemCategoryGroup')"
                name="generalItemCategoryGroup"
              >
                <TaktSelect
                  v-model:value="formState.generalItemCategoryGroup"
                  dict-type="logistics_general_item_category_group"
                  :placeholder="pi.ph('generalItemCategoryGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('logisticalVariants')"
                name="logisticalVariants"
              >
                <a-input
                  v-model:value="formState.logisticalVariants"
                  :placeholder="pi.ph('logisticalVariants')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-12"
        :tab="t('common.page.form.tabs.basicinfo') + ' (13/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialLocked')"
                name="materialLocked"
              >
                <a-input
                  v-model:value="formState.materialLocked"
                  :placeholder="pi.ph('materialLocked')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('configurationManagementRelevant')"
                name="configurationManagementRelevant"
              >
                <a-input
                  v-model:value="formState.configurationManagementRelevant"
                  :placeholder="pi.ph('configurationManagementRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assortmentListType')"
                name="assortmentListType"
              >
                <a-input
                  v-model:value="formState.assortmentListType"
                  :placeholder="pi.ph('assortmentListType')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expirationDateType')"
                name="expirationDateType"
              >
                <a-date-picker
                  v-model:value="formState.expirationDateType"
                  :placeholder="pi.ph('expirationDateType')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('gtinVariant')"
                name="gtinVariant"
              >
                <a-input
                  v-model:value="formState.gtinVariant"
                  :placeholder="pi.ph('gtinVariant')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('genericMaterialNumber')"
                name="genericMaterialNumber"
              >
                <a-input
                  v-model:value="formState.genericMaterialNumber"
                  :placeholder="pi.ph('genericMaterialNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('samePackingReferenceMaterial')"
                name="samePackingReferenceMaterial"
              >
                <a-input
                  v-model:value="formState.samePackingReferenceMaterial"
                  :placeholder="pi.ph('samePackingReferenceMaterial')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('globalDataSyncRelevant')"
                name="globalDataSyncRelevant"
              >
                <a-input
                  v-model:value="formState.globalDataSyncRelevant"
                  :placeholder="pi.ph('globalDataSyncRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('acceptanceAtOrigin')"
                name="acceptanceAtOrigin"
              >
                <a-input
                  v-model:value="formState.acceptanceAtOrigin"
                  :placeholder="pi.ph('acceptanceAtOrigin')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('standardHuType')"
                name="standardHuType"
              >
                <TaktSelect
                  v-model:value="formState.standardHuType"
                  dict-type="logistics_standard_hu_type"
                  :placeholder="pi.ph('standardHuType')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-13"
        :tab="t('common.page.form.tabs.basicinfo') + ' (14/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pilferable')"
                name="pilferable"
              >
                <a-input
                  v-model:value="formState.pilferable"
                  :placeholder="pi.ph('pilferable')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseStorageCondition')"
                name="warehouseStorageCondition"
              >
                <TaktSelect
                  v-model:value="formState.warehouseStorageCondition"
                  dict-type="logistics_warehouse_storage_condition"
                  :placeholder="pi.ph('warehouseStorageCondition')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseMaterialGroup')"
                name="warehouseMaterialGroup"
              >
                <TaktSelect
                  v-model:value="formState.warehouseMaterialGroup"
                  dict-type="logistics_warehouse_material_group"
                  :placeholder="pi.ph('warehouseMaterialGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handlingIndicator')"
                name="handlingIndicator"
              >
                <TaktSelect
                  v-model:value="formState.handlingIndicator"
                  dict-type="logistics_handling_indicator"
                  :placeholder="pi.ph('handlingIndicator')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hazardousSubstancesRelevant')"
                name="hazardousSubstancesRelevant"
              >
                <a-input
                  v-model:value="formState.hazardousSubstancesRelevant"
                  :placeholder="pi.ph('hazardousSubstancesRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handlingUnitType')"
                name="handlingUnitType"
              >
                <TaktSelect
                  v-model:value="formState.handlingUnitType"
                  dict-type="logistics_handling_unit_type"
                  :placeholder="pi.ph('handlingUnitType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variableTareWeight')"
                name="variableTareWeight"
              >
                <a-input
                  v-model:value="formState.variableTareWeight"
                  :placeholder="pi.ph('variableTareWeight')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumAllowedCapacity')"
                name="maximumAllowedCapacity"
              >
                <a-input-number
                  v-model:value="formState.maximumAllowedCapacity"
                  :placeholder="pi.ph('maximumAllowedCapacity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('overcapacityTolerance')"
                name="overcapacityTolerance"
              >
                <a-input-number
                  v-model:value="formState.overcapacityTolerance"
                  :placeholder="pi.ph('overcapacityTolerance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingLength')"
                name="maximumPackingLength"
              >
                <a-input-number
                  v-model:value="formState.maximumPackingLength"
                  :placeholder="pi.ph('maximumPackingLength')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-14"
        :tab="t('common.page.form.tabs.basicinfo') + ' (15/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingWidth')"
                name="maximumPackingWidth"
              >
                <a-input-number
                  v-model:value="formState.maximumPackingWidth"
                  :placeholder="pi.ph('maximumPackingWidth')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingHeight')"
                name="maximumPackingHeight"
              >
                <a-input-number
                  v-model:value="formState.maximumPackingHeight"
                  :placeholder="pi.ph('maximumPackingHeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maximumPackingDimensionUnit')"
                name="maximumPackingDimensionUnit"
              >
                <TaktSelect
                  v-model:value="formState.maximumPackingDimensionUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('maximumPackingDimensionUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countryOfOrigin')"
                name="countryOfOrigin"
              >
                <TaktSelect
                  v-model:value="formState.countryOfOrigin"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('countryOfOrigin')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialFreightGroup')"
                name="materialFreightGroup"
              >
                <TaktSelect
                  v-model:value="formState.materialFreightGroup"
                  dict-type="logistics_material_freight_group"
                  :placeholder="pi.ph('materialFreightGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quarantinePeriod')"
                name="quarantinePeriod"
              >
                <a-input-number
                  v-model:value="formState.quarantinePeriod"
                  :placeholder="pi.ph('quarantinePeriod')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quarantinePeriodUnit')"
                name="quarantinePeriodUnit"
              >
                <TaktSelect
                  v-model:value="formState.quarantinePeriodUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('quarantinePeriodUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('qualityInspectionGroup')"
                name="qualityInspectionGroup"
              >
                <TaktSelect
                  v-model:value="formState.qualityInspectionGroup"
                  dict-type="logistics_quality_inspection_group"
                  :placeholder="pi.ph('qualityInspectionGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNumberProfile')"
                name="serialNumberProfile"
              >
                <TaktSelect
                  v-model:value="formState.serialNumberProfile"
                  dict-type="logistics_serial_number_profile"
                  :placeholder="pi.ph('serialNumberProfile')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('formName')"
                name="formName"
              >
                <TaktSelect
                  v-model:value="formState.formName"
                  dict-type="logistics_form_name"
                  :placeholder="pi.ph('formName')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-15"
        :tab="t('common.page.form.tabs.basicinfo') + ' (16/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('logisticsUnitOfMeasure')"
                name="logisticsUnitOfMeasure"
              >
                <TaktSelect
                  v-model:value="formState.logisticsUnitOfMeasure"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('logisticsUnitOfMeasure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catchWeightMaterial')"
                name="catchWeightMaterial"
              >
                <a-input
                  v-model:value="formState.catchWeightMaterial"
                  :placeholder="pi.ph('catchWeightMaterial')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catchWeightProfile')"
                name="catchWeightProfile"
              >
                <TaktSelect
                  v-model:value="formState.catchWeightProfile"
                  dict-type="logistics_catch_weight_profile"
                  :placeholder="pi.ph('catchWeightProfile')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('catchWeightToleranceGroup')"
                name="catchWeightToleranceGroup"
              >
                <TaktSelect
                  v-model:value="formState.catchWeightToleranceGroup"
                  dict-type="logistics_catch_weight_tolerance_group"
                  :placeholder="pi.ph('catchWeightToleranceGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('adjustmentProfile')"
                name="adjustmentProfile"
              >
                <TaktSelect
                  v-model:value="formState.adjustmentProfile"
                  dict-type="logistics_adjustment_profile"
                  :placeholder="pi.ph('adjustmentProfile')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('intellectualPropertyId')"
                name="intellectualPropertyId"
              >
                <a-input
                  v-model:value="formState.intellectualPropertyId"
                  :placeholder="pi.ph('intellectualPropertyId')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('variantPriceAllowed')"
                name="variantPriceAllowed"
              >
                <a-input
                  v-model:value="formState.variantPriceAllowed"
                  :placeholder="pi.ph('variantPriceAllowed')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('medium')"
                name="medium"
              >
                <TaktSelect
                  v-model:value="formState.medium"
                  dict-type="logistics_medium"
                  :placeholder="pi.ph('medium')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('physicalCommodity')"
                name="physicalCommodity"
              >
                <TaktSelect
                  v-model:value="formState.physicalCommodity"
                  dict-type="logistics_physical_commodity"
                  :placeholder="pi.ph('physicalCommodity')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('animalOrigin')"
                name="animalOrigin"
              >
                <a-input
                  v-model:value="formState.animalOrigin"
                  :placeholder="pi.ph('animalOrigin')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-16"
        :tab="t('common.page.form.tabs.basicinfo') + ' (17/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('textileCompositionFunction')"
                name="textileCompositionFunction"
              >
                <a-input
                  v-model:value="formState.textileCompositionFunction"
                  :placeholder="pi.ph('textileCompositionFunction')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationStructure')"
                name="segmentationStructure"
              >
                <TaktSelect
                  v-model:value="formState.segmentationStructure"
                  dict-type="logistics_segmentation_structure"
                  :placeholder="pi.ph('segmentationStructure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationStrategy')"
                name="segmentationStrategy"
              >
                <TaktSelect
                  v-model:value="formState.segmentationStrategy"
                  dict-type="logistics_segmentation_strategy"
                  :placeholder="pi.ph('segmentationStrategy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationStatus')"
                name="segmentationStatus"
              >
                <TaktSelect
                  v-model:value="formState.segmentationStatus"
                  dict-type="logistics_segmentation_status"
                  :placeholder="pi.ph('segmentationStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationScope')"
                name="segmentationScope"
              >
                <TaktSelect
                  v-model:value="formState.segmentationScope"
                  dict-type="logistics_segmentation_scope"
                  :placeholder="pi.ph('segmentationScope')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('segmentationRelevant')"
                name="segmentationRelevant"
              >
                <a-input
                  v-model:value="formState.segmentationRelevant"
                  :placeholder="pi.ph('segmentationRelevant')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('anpCode')"
                name="anpCode"
              >
                <TaktSelect
                  v-model:value="formState.anpCode"
                  dict-type="logistics_anp_code"
                  :placeholder="pi.ph('anpCode')"
                  :disabled="!!formData?.generalMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionAttribute1')"
                name="fashionAttribute1"
              >
                <TaktSelect
                  v-model:value="formState.fashionAttribute1"
                  dict-type="logistics_fashion_attribute"
                  :placeholder="pi.ph('fashionAttribute1')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionAttribute2')"
                name="fashionAttribute2"
              >
                <TaktSelect
                  v-model:value="formState.fashionAttribute2"
                  dict-type="logistics_fashion_attribute"
                  :placeholder="pi.ph('fashionAttribute2')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fashionAttribute3')"
                name="fashionAttribute3"
              >
                <TaktSelect
                  v-model:value="formState.fashionAttribute3"
                  dict-type="logistics_fashion_attribute"
                  :placeholder="pi.ph('fashionAttribute3')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-17"
        :tab="t('common.page.form.tabs.basicinfo') + ' (18/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonUsageIndicator')"
                name="seasonUsageIndicator"
              >
                <TaktSelect
                  v-model:value="formState.seasonUsageIndicator"
                  dict-type="logistics_season_usage_indicator"
                  :placeholder="pi.ph('seasonUsageIndicator')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonActiveInInventory')"
                name="seasonActiveInInventory"
              >
                <a-input
                  v-model:value="formState.seasonActiveInInventory"
                  :placeholder="pi.ph('seasonActiveInInventory')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('characteristicConversionId')"
                name="characteristicConversionId"
              >
                <a-input
                  v-model:value="formState.characteristicConversionId"
                  :placeholder="pi.ph('characteristicConversionId')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('packagingCode')"
                name="packagingCode"
              >
                <TaktSelect
                  v-model:value="formState.packagingCode"
                  dict-type="logistics_packaging_code"
                  :placeholder="pi.ph('packagingCode')"
                  :disabled="!!formData?.generalMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dangerousGoodsPackagingStatus')"
                name="dangerousGoodsPackagingStatus"
              >
                <TaktSelect
                  v-model:value="formState.dangerousGoodsPackagingStatus"
                  dict-type="logistics_dangerous_goods_packaging_status"
                  :placeholder="pi.ph('dangerousGoodsPackagingStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialConditionManagement')"
                name="materialConditionManagement"
              >
                <a-input
                  v-model:value="formState.materialConditionManagement"
                  :placeholder="pi.ph('materialConditionManagement')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('returnCode')"
                name="returnCode"
              >
                <TaktSelect
                  v-model:value="formState.returnCode"
                  dict-type="logistics_return_code"
                  :placeholder="pi.ph('returnCode')"
                  :disabled="!!formData?.generalMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('returnToLogisticsLevel')"
                name="returnToLogisticsLevel"
              >
                <TaktSelect
                  v-model:value="formState.returnToLogisticsLevel"
                  dict-type="logistics_return_to_logistics_level"
                  :placeholder="pi.ph('returnToLogisticsLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('natoItemIdentificationNumber')"
                name="natoItemIdentificationNumber"
              >
                <a-input
                  v-model:value="formState.natoItemIdentificationNumber"
                  :placeholder="pi.ph('natoItemIdentificationNumber')"
                  show-count
                  :maxlength="9"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fffClass')"
                name="fffClass"
              >
                <TaktSelect
                  v-model:value="formState.fffClass"
                  dict-type="logistics_fff_class"
                  :placeholder="pi.ph('fffClass')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-18"
        :tab="t('common.page.form.tabs.basicinfo') + ' (19/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supersessionChainNumber')"
                name="supersessionChainNumber"
              >
                <a-input
                  v-model:value="formState.supersessionChainNumber"
                  :placeholder="pi.ph('supersessionChainNumber')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('seasonalProcurementCreationStatus')"
                name="seasonalProcurementCreationStatus"
              >
                <TaktSelect
                  v-model:value="formState.seasonalProcurementCreationStatus"
                  dict-type="logistics_seasonal_procurement_creation_status"
                  :placeholder="pi.ph('seasonalProcurementCreationStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('colorCharacteristicInternalNumber')"
                name="colorCharacteristicInternalNumber"
              >
                <a-input
                  v-model:value="formState.colorCharacteristicInternalNumber"
                  :placeholder="pi.ph('colorCharacteristicInternalNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mainSizeCharacteristicInternalNumber')"
                name="mainSizeCharacteristicInternalNumber"
              >
                <a-input
                  v-model:value="formState.mainSizeCharacteristicInternalNumber"
                  :placeholder="pi.ph('mainSizeCharacteristicInternalNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('secondSizeCharacteristicInternalNumber')"
                name="secondSizeCharacteristicInternalNumber"
              >
                <a-input
                  v-model:value="formState.secondSizeCharacteristicInternalNumber"
                  :placeholder="pi.ph('secondSizeCharacteristicInternalNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('color')"
                name="color"
              >
                <TaktSelect
                  v-model:value="formState.color"
                  dict-type="logistics_color"
                  :placeholder="pi.ph('color')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mainSize')"
                name="mainSize"
              >
                <TaktSelect
                  v-model:value="formState.mainSize"
                  dict-type="logistics_main_size"
                  :placeholder="pi.ph('mainSize')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('secondSize')"
                name="secondSize"
              >
                <TaktSelect
                  v-model:value="formState.secondSize"
                  dict-type="logistics_second_size"
                  :placeholder="pi.ph('secondSize')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationCharacteristicValue')"
                name="evaluationCharacteristicValue"
              >
                <TaktSelect
                  v-model:value="formState.evaluationCharacteristicValue"
                  dict-type="logistics_evaluation_characteristic_value"
                  :placeholder="pi.ph('evaluationCharacteristicValue')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('careCode')"
                name="careCode"
              >
                <TaktSelect
                  v-model:value="formState.careCode"
                  dict-type="logistics_care_code"
                  :placeholder="pi.ph('careCode')"
                  :disabled="!!formData?.generalMaterialId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-19"
        :tab="t('common.page.form.tabs.basicinfo') + ' (20/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('brandId')"
                name="brandId"
              >
                <TaktSelect
                  v-model:value="formState.brandId"
                  dict-type="logistics_brand_id"
                  :placeholder="pi.ph('brandId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode1')"
                name="fiberCode1"
              >
                <TaktSelect
                  v-model:value="formState.fiberCode1"
                  dict-type="logistics_fiber_code"
                  :placeholder="pi.ph('fiberCode1')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart1')"
                name="fiberPart1"
              >
                <a-input
                  v-model:value="formState.fiberPart1"
                  :placeholder="pi.ph('fiberPart1')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode2')"
                name="fiberCode2"
              >
                <TaktSelect
                  v-model:value="formState.fiberCode2"
                  dict-type="logistics_fiber_code"
                  :placeholder="pi.ph('fiberCode2')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart2')"
                name="fiberPart2"
              >
                <a-input
                  v-model:value="formState.fiberPart2"
                  :placeholder="pi.ph('fiberPart2')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode3')"
                name="fiberCode3"
              >
                <TaktSelect
                  v-model:value="formState.fiberCode3"
                  dict-type="logistics_fiber_code"
                  :placeholder="pi.ph('fiberCode3')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart3')"
                name="fiberPart3"
              >
                <a-input
                  v-model:value="formState.fiberPart3"
                  :placeholder="pi.ph('fiberPart3')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode4')"
                name="fiberCode4"
              >
                <TaktSelect
                  v-model:value="formState.fiberCode4"
                  dict-type="logistics_fiber_code"
                  :placeholder="pi.ph('fiberCode4')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberPart4')"
                name="fiberPart4"
              >
                <a-input
                  v-model:value="formState.fiberPart4"
                  :placeholder="pi.ph('fiberPart4')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiberCode5')"
                name="fiberCode5"
              >
                <TaktSelect
                  v-model:value="formState.fiberCode5"
                  dict-type="logistics_fiber_code"
                  :placeholder="pi.ph('fiberCode5')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-20"
        :tab="t('common.page.form.tabs.basicinfo') + ' (21/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('fiberPart5')"
                name="fiberPart5"
              >
                <a-input
                  v-model:value="formState.fiberPart5"
                  :placeholder="pi.ph('fiberPart5')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('fashionGrade')"
                name="fashionGrade"
              >
                <TaktSelect
                  v-model:value="formState.fashionGrade"
                  dict-type="logistics_fashion_grade"
                  :placeholder="pi.ph('fashionGrade')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-21"
        :tab="t('common.page.form.tabs.basicinfo') + ' (22/22)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt全局物料实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/general-material/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useGeneralMaterialI18n } from '../composables/useGeneralMaterialI18n'

/** 实体字段 i18n */
const pi = useGeneralMaterialI18n()
import type { GeneralMaterialCreate } from '@/types/logistics/materials/generalMaterial.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：租户级实体仅注入 tenantCode，表单只读
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<GeneralMaterialCreate & { generalMaterialId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  materialType: "ROH",
  packagingMaterialType: "ROH",
  countryOfOrigin: "CN"
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 generalMaterialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.generalMaterialId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => tenantStore.tenantCode,
  () => {
    if (!props.formData?.generalMaterialId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'blur'
    }
  ],
  materialType: [
    {
      required: true,
      message: pi.ph('materialType'),
      trigger: 'change'
    }
  ],
  industrySector: [
    {
      required: true,
      message: pi.ph('industrySector'),
      trigger: 'change'
    }
  ],
  materialGroup: [
    {
      required: true,
      message: pi.ph('materialGroup'),
      trigger: 'change'
    }
  ],
  baseUnit: [
    {
      required: true,
      message: pi.ph('baseUnit'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    payload.grossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    payload.netWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
  }
  if ('volume' in payload) {
    const rawvolume = payload.volume
    payload.volume = typeof rawvolume === 'number' ? rawvolume : Number(rawvolume)
  }
  if ('grGiSlipQuantity' in payload) {
    const rawgrGiSlipQuantity = payload.grGiSlipQuantity
    payload.grGiSlipQuantity = typeof rawgrGiSlipQuantity === 'number' ? rawgrGiSlipQuantity : Number(rawgrGiSlipQuantity)
  }
  if ('length' in payload) {
    const rawlength = payload.length
    payload.length = typeof rawlength === 'number' ? rawlength : Number(rawlength)
  }
  if ('width' in payload) {
    const rawwidth = payload.width
    payload.width = typeof rawwidth === 'number' ? rawwidth : Number(rawwidth)
  }
  if ('height' in payload) {
    const rawheight = payload.height
    payload.height = typeof rawheight === 'number' ? rawheight : Number(rawheight)
  }
  if ('allowedPackagingWeight' in payload) {
    const rawallowedPackagingWeight = payload.allowedPackagingWeight
    payload.allowedPackagingWeight = typeof rawallowedPackagingWeight === 'number' ? rawallowedPackagingWeight : Number(rawallowedPackagingWeight)
  }
  if ('allowedPackagingVolume' in payload) {
    const rawallowedPackagingVolume = payload.allowedPackagingVolume
    payload.allowedPackagingVolume = typeof rawallowedPackagingVolume === 'number' ? rawallowedPackagingVolume : Number(rawallowedPackagingVolume)
  }
  if ('excessWeightTolerance' in payload) {
    const rawexcessWeightTolerance = payload.excessWeightTolerance
    payload.excessWeightTolerance = typeof rawexcessWeightTolerance === 'number' ? rawexcessWeightTolerance : Number(rawexcessWeightTolerance)
  }
  if ('excessVolumeTolerance' in payload) {
    const rawexcessVolumeTolerance = payload.excessVolumeTolerance
    payload.excessVolumeTolerance = typeof rawexcessVolumeTolerance === 'number' ? rawexcessVolumeTolerance : Number(rawexcessVolumeTolerance)
  }
  if ('maximumLevelByVolume' in payload) {
    const rawmaximumLevelByVolume = payload.maximumLevelByVolume
    payload.maximumLevelByVolume = typeof rawmaximumLevelByVolume === 'number' ? rawmaximumLevelByVolume : Number(rawmaximumLevelByVolume)
  }
  if ('stackingFactor' in payload) {
    const rawstackingFactor = payload.stackingFactor
    payload.stackingFactor = typeof rawstackingFactor === 'number' ? rawstackingFactor : Number(rawstackingFactor)
  }
  if ('minimumRemainingShelfLife' in payload) {
    const rawminimumRemainingShelfLife = payload.minimumRemainingShelfLife
    payload.minimumRemainingShelfLife = typeof rawminimumRemainingShelfLife === 'number' ? rawminimumRemainingShelfLife : Number(rawminimumRemainingShelfLife)
  }
  if ('totalShelfLife' in payload) {
    const rawtotalShelfLife = payload.totalShelfLife
    payload.totalShelfLife = typeof rawtotalShelfLife === 'number' ? rawtotalShelfLife : Number(rawtotalShelfLife)
  }
  if ('storagePercentage' in payload) {
    const rawstoragePercentage = payload.storagePercentage
    payload.storagePercentage = typeof rawstoragePercentage === 'number' ? rawstoragePercentage : Number(rawstoragePercentage)
  }
  if ('netContents' in payload) {
    const rawnetContents = payload.netContents
    payload.netContents = typeof rawnetContents === 'number' ? rawnetContents : Number(rawnetContents)
  }
  if ('comparisonPriceUnit' in payload) {
    const rawcomparisonPriceUnit = payload.comparisonPriceUnit
    payload.comparisonPriceUnit = typeof rawcomparisonPriceUnit === 'number' ? rawcomparisonPriceUnit : Number(rawcomparisonPriceUnit)
  }
  if ('grossContents' in payload) {
    const rawgrossContents = payload.grossContents
    payload.grossContents = typeof rawgrossContents === 'number' ? rawgrossContents : Number(rawgrossContents)
  }
  if ('maximumAllowedCapacity' in payload) {
    const rawmaximumAllowedCapacity = payload.maximumAllowedCapacity
    payload.maximumAllowedCapacity = typeof rawmaximumAllowedCapacity === 'number' ? rawmaximumAllowedCapacity : Number(rawmaximumAllowedCapacity)
  }
  if ('overcapacityTolerance' in payload) {
    const rawovercapacityTolerance = payload.overcapacityTolerance
    payload.overcapacityTolerance = typeof rawovercapacityTolerance === 'number' ? rawovercapacityTolerance : Number(rawovercapacityTolerance)
  }
  if ('maximumPackingLength' in payload) {
    const rawmaximumPackingLength = payload.maximumPackingLength
    payload.maximumPackingLength = typeof rawmaximumPackingLength === 'number' ? rawmaximumPackingLength : Number(rawmaximumPackingLength)
  }
  if ('maximumPackingWidth' in payload) {
    const rawmaximumPackingWidth = payload.maximumPackingWidth
    payload.maximumPackingWidth = typeof rawmaximumPackingWidth === 'number' ? rawmaximumPackingWidth : Number(rawmaximumPackingWidth)
  }
  if ('maximumPackingHeight' in payload) {
    const rawmaximumPackingHeight = payload.maximumPackingHeight
    payload.maximumPackingHeight = typeof rawmaximumPackingHeight === 'number' ? rawmaximumPackingHeight : Number(rawmaximumPackingHeight)
  }
  if ('quarantinePeriod' in payload) {
    const rawquarantinePeriod = payload.quarantinePeriod
    payload.quarantinePeriod = typeof rawquarantinePeriod === 'number' ? rawquarantinePeriod : Number(rawquarantinePeriod)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.generalMaterialId)

  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
